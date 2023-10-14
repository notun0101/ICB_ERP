using Microsoft.EntityFrameworkCore;
using OPUSERP.Areas.HRPMSAttendence.Models;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Attendance;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Leave;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Data.Entity;
using OPUSERP.Data.Entity.MasterData;
using OPUSERP.Areas.HRPMSAttendence;

namespace OPUSERP.HRPMS.Services.Attendance
{
	public class EmployeePunchCardInfoService : IEmployeePunchCardInfoService
	{
		private readonly ERPDbContext _context;

		public EmployeePunchCardInfoService(ERPDbContext context)
		{
			_context = context;
		}

		public async Task<bool> SaveEmployeePunchCardInfo(EmployeePunchCardInfo employeePunchCardInfo)
		{
			if (employeePunchCardInfo.Id != 0)
				_context.employeePunchCardInfos.Update(employeePunchCardInfo);
			else
				_context.employeePunchCardInfos.Add(employeePunchCardInfo);
			//_context.employeePunchCardInfos.Add(employeePunchCardInfo);
			return 1 == await _context.SaveChangesAsync();
		}
        public async Task<string> GetPlaceNameById(string type, int id)
        {
            var placeNameBn = "";
            if (type == "branch")
            {
                placeNameBn = await _context.hrBranches.Where(x => x.Id == id).Select(x => x.branchNameBn).FirstOrDefaultAsync();
            }
            else if (type == "department")
            {
                placeNameBn = await _context.departments.Where(x => x.Id == id).Select(x => x.deptNameBn).FirstOrDefaultAsync();
            }
            else if (type == "division")
            {
                placeNameBn = await _context.hrDivisions.Where(x => x.Id == id).Select(x => x.divNameBn).FirstOrDefaultAsync();
            }
            else if (type == "office")
            {
                placeNameBn = await _context.FunctionInfos.Where(x => x.Id == id).Select(x => x.branchUnitNameBN).FirstOrDefaultAsync();
            }
            else if (type == "location")
            {
                placeNameBn = await _context.Locations.Where(x => x.Id == id).Select(x => x.branchUnitNameBN).FirstOrDefaultAsync();
            }
            else if (type == "unit")
            {
                placeNameBn = await _context.hrUnits.Where(x => x.Id == id).Select(x => x.unitNameBn).FirstOrDefaultAsync();
            }
            else
            {
                placeNameBn = "";
            }
            return placeNameBn;
        }

        public async Task<IEnumerable<EmployeePunchCardInfo>> GetAllEmployeePunchCardInfo()
		{
			return await _context.employeePunchCardInfos.Include(a => a.shiftGroupMaster).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<EmployeePunchCardInfoViewModel>> GetAllEmployeeWithPunchCardInfo()
		{
			var data = await (from e in _context.employeeInfos
							  join ep in _context.employeePunchCardInfos
							  on e.employeeCode equals ep.employeeCode
							  select new EmployeePunchCardInfoViewModel
							  {
								  employeeInfo = e,
								  employeePunchCard = ep
							  }).ToListAsync();
			return data;
		}

		public async Task<EmployeePunchCardInfo> GetEmployeePunchCardInfoById(int id)
		{
			return await _context.employeePunchCardInfos.FindAsync(id);
		}

		public async Task<bool> DeleteEmployeePunchCardInfoById(int id)
		{
			//_context.myquery.FromSql("");
			_context.employeePunchCardInfos.Remove(_context.employeePunchCardInfos.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<int> SaveEmployeeManualAttendence(EmpManualAttendance model)
		{
			try
			{
				if (model.Id != 0)
				{
					_context.empManualAttendances.Update(model);
				}
				else
				{
					_context.empManualAttendances.Add(model);
				}
				return await _context.SaveChangesAsync();
			}
			catch (System.Exception ex)
			{

				throw;
			}
		}
		public async Task<int> SaveEmpAttendence(EmpAttendance model)
		{
			try
			{
				var data = _context.empAttendances.Where(x => x.workDate == model.workDate).AsNoTracking().FirstOrDefault();
				if (data != null)
				{
					_context.empAttendances.Remove(data);
					await _context.SaveChangesAsync();
				}
				
				if (model.Id != 0)
				{
					_context.empAttendances.Update(model);
				}
				else
				{
					_context.empAttendances.Add(model);
				}
				return await _context.SaveChangesAsync();
			}
			catch (System.Exception ex)
			{

				throw;
			}
		}
		public async Task<int> ApproveManualAttendance(int Id ,string code)
		{
			_context.Database.ExecuteSqlCommand($"sp_ApproveManualAttendance {Id},{code}");
			return 1;
			//var att = _context.empManualAttendances.Find(Id);
			//att.summaryId = 9;
			//_context.empManualAttendances.Update(att);
			//return await _context.SaveChangesAsync();
		}
		public async Task<int> CheckAttendanceByDateAndPunchCard(string workDate, string cardNo)
		{
			var data = await _context.empManualAttendances.Where(x => Convert.ToDateTime(x.workDate).Date == Convert.ToDateTime(workDate).Date && x.punchCardNo == cardNo).AsNoTracking().FirstOrDefaultAsync();
			if (data != null)
			{
				return data.Id;
			}
			else
			{
				return 0;
			}
		}
		public async Task<EmpAttendance> GetAttendanceById(int id)
		{
			var data = await _context.empAttendances.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
			return data;
		}
		public async Task<EmpManualAttendance> GetManualAttendanceById(int id)
		{
			var data = await _context.empManualAttendances.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
			return data;
		}
		public async Task<IEnumerable<EmpAttendance>> GetAllManualAttendence()
		{
			var data = await _context.empAttendances.Where(x=>x.summaryId ==8).ToListAsync();
			return data;
		}

		public async Task<ShiftGroupViewModel> GetShiftByEmpCode(string code)
		{
			var data = await (from e in _context.employeeInfos
							  join sm in _context.shiftGroupMasters
							  on e.shiftGroupId equals sm.Id
							  join sd in _context.shiftGroupDetails
							  on sm.Id equals sd.shiftGroupMasterId
							  select new ShiftGroupViewModel
							  {
								  EmpId = e.Id,
								  EmpCode = e.employeeCode,
								  ShiftGroupId = sm.Id,
								  GroupName = sm.shiftName,
								  InTime = sd.startTime,
								  OutTime = sd.endTime
							  }).FirstOrDefaultAsync();
			return data;
		}

		public async Task<IEnumerable<EmpAttendance>> GetAllManualAttendenceByDateRange(DateTime? from, DateTime? to)
		{
			var data = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Date >= Convert.ToDateTime(from).Date && Convert.ToDateTime(x.workDate).Date <= Convert.ToDateTime(to).Date).ToListAsync();
			return data;
		}

		public async Task<IEnumerable<EmpAttendance>> GetAllManualAttendenceByDateFilter(string date)
		{
			var data = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Date == Convert.ToDateTime(date).Date).ToListAsync();
			return data;
		}

		public async Task<ApplicationUser> GetUserByUsername(string username)
		{
			return await _context.Users.Where(x => x.UserName == username).FirstOrDefaultAsync();
		}
		public async Task<EmployeeInfo> GetEmpInfoByPunchCard(string punchCardNo)
		{
			var empCode = _context.employeePunchCardInfos.Where(x => x.punchCardNo == punchCardNo).Select(x => x.employeeCode).FirstOrDefault();
			return await _context.employeeInfos.Include(x => x.department).Include(x=> x.lastDesignation).Where(x => x.employeeCode == empCode).FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<Department>> GetAllDepartment()
		{
			return await _context.departments.AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<HrBranch>> GetAllHrBranches()
		{
			return await _context.hrBranches.AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<HrBranch>> GetHrBranchesByZoneId(int? zoneId)
		{
            return await _context.hrBranches.Where(x=> x.locationId == zoneId).AsNoTracking().ToListAsync();
        }
        public async Task<string> GetBrancheNameById(int Id)
        {
            return await _context.hrBranches.Where(x => x.Id == Id).Select(x => x.branchName).FirstOrDefaultAsync();
        }


        public async Task<string> GetDivisionNameById(int Id)
        {
            return await _context.hrDivisions.Where(x => x.Id == Id).Select(x => x.divName).FirstOrDefaultAsync();
        }
        public async Task<string> GetLocatioNameById(int Id)
        {
            return await _context.Locations.Where(x => x.Id == Id).Select(x => x.branchUnitName).FirstOrDefaultAsync();
        }

        public async Task<string> GetOfficeNameById(int Id)
        {
            return await _context.FunctionInfos.Where(x => x.Id == Id).Select(x => x.branchUnitName).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<HrDivision>> GetAllHrDivisions()
		{
			var data = await _context.hrDivisions.AsNoTracking().ToListAsync();
			return data;
		}
		public async Task<IEnumerable<FunctionInfo>> GetFunctionInfo()
		{
			return await _context.FunctionInfos.AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<ShiftGroupMaster>> GetAllShift()
		{
			return await _context.shiftGroupMasters.AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<EmployeeInfo>> GetAllEmployee()
		{
			return await _context.employeeInfos.AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<EmployeeInfo>> GetAllFilteredEmployee(int department, string designation, int shift)
		{
			if (designation == null)
			{
				designation = "0";
			}
			var data = await _context.employeeInfos.Include(x => x.department).Include(x=> x.lastDesignation).Include(x => x.shiftGroup).AsNoTracking().ToListAsync();
			if (department != 0 && designation == "0" && shift == 0)
			{
				return data.Where(x => x.departmentId == department).ToList();
			}
			else if (department == 0 && designation != "0" && shift == 0)
			{
				return data.Where(x => x.lastDesignation?.designationName == designation).ToList();
			}
			//else if (department == 0 && designation == "0" && shift != 0)
			//{
			//	return data.Where(x => x.shiftGroupId == shift).ToList();
			//}
			else if (department != 0 && designation != "0" && shift == 0)
			{
				return data.Where(x => x.departmentId == department && x.lastDesignation?.designationName == designation).ToList();
			}
			//else if (department == 0 && designation != "0" && shift != 0)
			//{
			//	return data.Where(x => x.designation == designation && x.shiftGroupId == shift).ToList();
			//}
			//else if (department != 0 && designation == "0" && shift != 0)
			//{
			//	return data.Where(x => x.departmentId == department && x.shiftGroupId == shift).ToList();
			//}
			//else if (department != 0 && designation != "0" && shift != 0)
			//{
			//	return data.Where(x => x.departmentId == department && x.lastDesignation?.designationName == designation && x.shiftGroupId == shift).ToList();
			//}
			else
			{
				return data;
			}
		}
		public async Task<int> FilteredEmployeeCount(int department, string designation, int shift)
		{
			var data = await _context.employeeInfos.Include(x => x.department).Include(x => x.lastDesignation).Include(x => x.shiftGroup).AsNoTracking().ToListAsync();
			if (department != 0 && designation == "0" && shift == 0)
			{
				return data.Where(x => x.departmentId == department).ToList().Count;
			}
			else if (department == 0 && designation != "0" && shift == 0)
			{
				return data.Where(x => x.lastDesignation.designationName == designation).ToList().Count;
			}
			//else if (department == 0 && designation == "0" && shift != 0)
			//{
			//	return data.Where(x => x.shiftGroupId == shift).ToList().Count;
			//}
			else if (department != 0 && designation != "0" && shift == 0)
			{
				return data.Where(x => x.departmentId == department && x.lastDesignation?.designationName == designation).ToList().Count;
			}
			//         else if (department == 0 && designation != "0" && shift != 0)
			//{
			//	return data.Where(x => x.designation == designation && x.shiftGroupId == shift).ToList().Count;
			//}
			//else if (department != 0 && designation == "0" && shift != 0)
			//{
			//    return data.Where(x => x.departmentId == department && x.shiftGroupId == shift).ToList().Count;
			//}
   //         else if (department != 0 && designation != "0" && shift != 0)
			//{
			//	return data.Where(x => x.departmentId == department && x.designation == designation && x.shiftGroupId == shift).ToList().Count;
			//}
			else
			{
				return data.Count;
			}
		}
		public async Task<int> FilteredEmployeeLateCount(int year, int month, int department, string designation, int shift)
		{
			var data = await (from e in _context.employeeInfos
							  join p in _context.employeePunchCardInfos on e.employeeCode equals p.employeeCode
							  join a in _context.empAttendances on p.punchCardNo equals a.punchCardNo
							  where a.latetime != null && Convert.ToDateTime(a.workDate).Year == year && Convert.ToDateTime(a.workDate).Month == month
							  select e).ToListAsync();
			if (department != 0 && designation == "0" && shift == 0)
			{
				return data.Where(x => x.departmentId == department).ToList().Count;
			}
			else if (department == 0 && designation != "0" && shift == 0)
			{
				return data.Where(x => x.lastDesignation.designationName == designation).ToList().Count;
			}
			else if (department == 0 && designation == "0" && shift != 0)
			{
				return data.Where(x => x.shiftGroupId == shift).ToList().Count;
			}
			else if (department != 0 && designation != "0" && shift == 0)
			{
				return data.Where(x => x.departmentId == department && x.lastDesignation.designationName == designation).ToList().Count;
			}
			else if (department == 0 && designation != "0" && shift != 0)
			{
				return data.Where(x => x.lastDesignation.designationName == designation && x.shiftGroupId == shift).ToList().Count;
			}
			else if (department != 0 && designation == "0" && shift != 0)
			{
				return data.Where(x => x.departmentId == department && x.shiftGroupId == shift).ToList().Count;
			}
			else if (department != 0 && designation != "0" && shift != 0)
			{
				return data.Where(x => x.departmentId == department && x.lastDesignation.designationName == designation && x.shiftGroupId == shift).ToList().Count;
			}
			else
			{
				return data.Count;
			}
		}
		public async Task<IEnumerable<EmployeeInfo>> GetAllEmployeeDept()
		{
			return await _context.employeeInfos.Include(x => x.lastDesignation).GroupBy(x => x.lastDesignationId).Select(x => x.FirstOrDefault()).AsNoTracking().ToListAsync();
		}
		public async Task<EmployeeInfo> GetEmployeename()
		{
			return await _context.employeeInfos.FirstOrDefaultAsync();
		}
		public async Task<IEnumerable<Designation>> GetAllDesignation()
		{
			return await _context.designations.OrderBy(x => Convert.ToInt32(x.designationCode)).AsNoTracking().ToListAsync();
		}
		 public async Task<IEnumerable<Location>> GetAllZone()
		{
			return await _context.Locations.OrderBy(x => x.branchUnitName).AsNoTracking().ToListAsync();
		}
		 public async Task<IEnumerable<FunctionInfo>> GetAllOffice()
		{
			return await _context.FunctionInfos.OrderBy(x => x.branchUnitName).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<HrUnit>> GetAllUnits()
		{
			return await _context.hrUnits.OrderBy(x => x.unitName).AsNoTracking().ToListAsync();
		}

		public async Task<EmployeeInfo> GetEmployeeByEmpCode(string empCode)
		{
			return await _context.employeeInfos.Where(x => x.employeeCode == empCode).Include(x=>x.lastDesignation).FirstOrDefaultAsync();
		}
        public async Task<IEnumerable<EmpAbsentDates>> GetAllAbsentByMonthAndYearAndEmpCode(int month, int year, string empCode)
        {
            var employee = await _context.employeeInfos.Where(x => x.employeeCode == empCode).FirstOrDefaultAsync();

            var holidays = await _context.holidays.Where(x => Convert.ToDateTime(x.weeklyHoliday).Month == month && Convert.ToDateTime(x.weeklyHoliday).Year == year).Select(x => x.weeklyHoliday).ToListAsync();
            //var isleave = await _context.leaveRegisters.Where(x => x.employeeId == employee.Id && (Convert.ToDateTime(x.leaveFrom) >= Convert.ToDateTime(new DateTime(year, month, 1)) &&  Convert.ToDateTime(new DateTime(year, month, DateTime.DaysInMonth(year, month)))<= Convert.ToDateTime(x.leaveTo))).CountAsync(); 
            var isleave = await _context.leaveRegisters.Where(x => x.employeeId == employee.Id && x.leaveStatus==3 && (Convert.ToDateTime(x.leaveFrom) >= Convert.ToDateTime(new DateTime(year, month, 1)) && Convert.ToDateTime(x.leaveTo) <= Convert.ToDateTime(new DateTime(year, month, DateTime.DaysInMonth(year, month))))).CountAsync(); 
            var punchcard = await _context.employeePunchCardInfos.Where(x => x.employeeCode == empCode).Select(x => x.punchCardNo).AsNoTracking().FirstOrDefaultAsync();
            var daysinmonth = DateTime.DaysInMonth(year, month);
            var list = new List<EmpAbsentDates>();
            for (int i = 1; i <= daysinmonth; i++)
            {
                var value = await _context.empAttendances.Where(x => x.punchCardNo == punchcard && !holidays.Contains(Convert.ToDateTime(x.workDate)) && Convert.ToDateTime(x.workDate).Date == new DateTime(year, month, i).Date).CountAsync();
                var dd = new DateTime(year, month, i);
                if (!holidays.Contains(dd) && isleave == 0)
                {
                    list.Add(new EmpAbsentDates
                    {
                        date = dd,
                        isPresent = value > 0 ? 1 : 0
                    });
                }
			}
			return list;
			//var punchcard = await _context.employeePunchCardInfos.Where(x => x.employeeCode == empCode).Select(x => x.punchCardNo).AsNoTracking().FirstOrDefaultAsync();
			//var attendance = await _context.empAttendances.Where(x => x.punchCardNo == punchcard && Convert.ToDateTime(x.workDate).Month == month && Convert.ToDateTime(x.workDate).Year == year).AsNoTracking().ToListAsync();
			//return attendance;
		}
		public async Task<int> GetAllAbsentCountByMonthAndYearAndEmpCode(int month, int year, string empCode)
		{
            var data = await _context.holidays.Where(x => Convert.ToDateTime(x.weeklyHoliday).Month == month && Convert.ToDateTime(x.weeklyHoliday).Year == DateTime.Now.Year).Select(x => x.weeklyHoliday).ToListAsync();
            var punchcard = await _context.employeePunchCardInfos.Where(x => x.employeeCode == empCode).Select(x => x.punchCardNo).AsNoTracking().FirstOrDefaultAsync();
			var attendance = await _context.empAttendances.Where(x => x.punchCardNo == punchcard && !data.Contains(Convert.ToDateTime(x.workDate)) && Convert.ToDateTime(x.workDate).Month == month && Convert.ToDateTime(x.workDate).Year == year).AsNoTracking().CountAsync();
			return attendance;
		}
		public async Task<int> GetTotalAttendanceByEmpCodeMonthAndYear(string empCode, int month, int year)
		{
			var punchcard = await _context.employeePunchCardInfos.Where(x => x.employeeCode == empCode).Select(x => x.punchCardNo).AsNoTracking().FirstOrDefaultAsync();
			var attendance = await _context.empAttendances.Where(x => x.punchCardNo == punchcard && Convert.ToDateTime(x.workDate).Month == month && Convert.ToDateTime(x.workDate).Year == year).AsNoTracking().CountAsync();
			return attendance;
		}

        public async Task<IEnumerable<MonthlyAttendanceVm>> GetMonthlyAttendance(int year, int month, int deptId, int branchId, int zoneId)
        {
            var data = await _context.monthlyAttendanceVms.FromSql($"sp_GetMonthlyAttendanceSp {year}, {month}, {deptId}, {branchId}, {zoneId}").ToListAsync();
            return data;
        }

		public async Task<IEnumerable<MonthlyAttendanceVm>> GetMonthlyInTimeAttendance(int year, int month, int deptId, int branchId, int zoneId,int officeId, int hrUnitId, int hrDivitionId)
		{
			var data = await _context.monthlyAttendanceVms.FromSql($"sp_GetMonthlyInTimeAttendanceSp {year}, {month}, {deptId}, {branchId}, {zoneId},{officeId},{hrUnitId},{hrDivitionId}").ToListAsync();
			return data;
		}


		public int GetTotalSubsideryDaysByEmpCodeMonthAndYear(string empCode, int month, int year)
		{
			var totalsubsideryday = _context.subsidyCounts.FromSql($"sp_subsidarydayscount {year}, {month}, {empCode}");

			return totalsubsideryday.FirstOrDefault().attendDays;
		}
        public int GetTotalSubsideryDaysByEmpCodeBranchMonthAndYear(string empCode,int hrBranch, int month, int year)
        {
            var totalsubsideryday = _context.subsidyCounts.FromSql($"sp_subsidarydayscountbybranch {hrBranch},{year}, {month}, {empCode}");

            return totalsubsideryday.FirstOrDefault().attendDays;
        }
        public async Task<int> GetAllActiveCountByMonthAndYear(int month, int year)
		{
			var attendance = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Month == month && Convert.ToDateTime(x.workDate).Year == year).AsNoTracking().CountAsync();
			return attendance;
		}

		public async Task<EmployeePunchCardInfo> GetEmployeePunchCardInfoByByEmpCode(string empCode)
		{
			var punch = await _context.employeePunchCardInfos.Where(x => x.employeeCode == empCode).AsNoTracking().FirstOrDefaultAsync();
			return punch;
		}
		public async Task<EmployeeInfo> GetEmployeeInfoByByEmpCode(string empCode)
		{
			var punch = await _context.employeeInfos.Where(x => x.employeeCode == empCode).AsNoTracking().FirstOrDefaultAsync();
			return punch;
		}
		public async Task<EmpManualAttendance> GetEmpManualAttendanceById(int Id)
		{
			var punch = await _context.empManualAttendances.Where(x => x.Id == Id).AsNoTracking().FirstOrDefaultAsync();
			return punch;
		}
		

		public async Task<EmpAttendance> GetDailyAttendenceByEmpCode(string code)
		{
			var punchCard = _context.employeePunchCardInfos.Where(x => x.employeeCode == code).Select(x => x.punchCardNo).FirstOrDefault();

			return await _context.empAttendances.Where(x => x.punchCardNo == punchCard && Convert.ToDateTime(x.workDate).Date == DateTime.Now.Date).FirstOrDefaultAsync();
		}
		public async Task<EmpAttendance> GetDailyAttendenceByEmpCodeDate(string code, string date)
		{
			var punchCard = _context.employeePunchCardInfos.Where(x => x.employeeCode == code).Select(x => x.punchCardNo).FirstOrDefault();

			return await _context.empAttendances.Where(x => x.punchCardNo == punchCard && Convert.ToDateTime(x.workDate).Date == Convert.ToDateTime(date).Date).FirstOrDefaultAsync();
		}

		public async Task<EmpAttendance> GetDailyAbsentByEmpCodeDate(string code, string date)
		{
			var punchCard = _context.employeePunchCardInfos.Where(x => x.employeeCode == code).Select(x => x.punchCardNo).FirstOrDefault();

			return await _context.empAttendances.Where(x => x.punchCardNo == punchCard && Convert.ToDateTime(x.workDate).Date == Convert.ToDateTime(date).Date).FirstOrDefaultAsync();
		}




		public async Task<int> GetLeaveStatusByEmpCodeAndDate(string code, string date)
		{
			var leavestatus = 0;
			var allLeaves = await _context.leaveRegisters.Where(x => x.employee.employeeCode == code && x.leaveStatus == 3).ToListAsync();
			foreach (var item in allLeaves)
			{
				if (Convert.ToDateTime(item.leaveFrom).Date <= Convert.ToDateTime(date).Date && Convert.ToDateTime(item.leaveTo).Date >= Convert.ToDateTime(date).Date)
				{
					leavestatus = 1;
				}
			}
			return leavestatus;
		}
		public async Task<EmployeeInfo> GetEmployeeInfoByDeptDesigShift(int departmet, string dsig, int shift)
		{
			return await _context.employeeInfos.Include(x => x.department).Include(x => x.lastDesignation).Include(x => x.shiftGroup).Where(x => x.departmentId == departmet && x.designation == dsig && x.shiftGroupId == shift).FirstOrDefaultAsync();
		}

		public async Task<int> GetAllEmployeeAttendenceCountByDateRange1(DateTime? start, DateTime? end)
		{
			var data = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Date >= Convert.ToDateTime(start).Date && Convert.ToDateTime(x.workDate).Date <= Convert.ToDateTime(end).Date).ToListAsync();
			return data.Count;
		}
		public async Task<int> GetAllEmployeeAttendenceCountByDateRange(DateTime? date, int department, string designation, int shift)
		{
			var count = 0;
			if (date != null)
			{
				var data = await (from e in _context.employeeInfos
								  join p in _context.employeePunchCardInfos on e.employeeCode equals p.employeeCode
								  join a in _context.empAttendances on p.punchCardNo equals a.punchCardNo
								  where Convert.ToDateTime(a.workDate).Date == date
								  select e).ToListAsync();
				if (department != 0 && designation == "A" && shift == 0)
				{
					count = data.Where(x => x.departmentId == department).Count();
				}
				else if (department == 0 && designation != "A" && shift == 0)
				{
					count = data.Where(x => x.lastDesignation?.designationName == designation).Count();
				}
				else if (department == 0 && designation == "A" && shift != 0)
				{
					count = data.Count();
				}
				else if (department != 0 && designation != "A" && shift == 0)
				{
					count = data.Where(x => x.departmentId == department && x.lastDesignation?.designationName == designation).Count();
				}
				else if (department == 0 && designation != "A" && shift != 0)
				{
					count = data.Where(x => x.lastDesignation?.designationName == designation && x.shiftGroupId == shift).Count();
				}
				else if (department != 0 && designation == "A" && shift != 0)
				{
					count = data.Where(x => x.departmentId == department && x.shiftGroupId == shift).Count();
				}
				else if (department != 0 && designation != "A" && shift != 0)
				{
					count = data.Where(x => x.departmentId == department && x.lastDesignation?.designationName == designation && x.shiftGroupId == shift).Count();
				}
				else
				{
					count = data.Count();
				}
			}
			else
			{

			}
			//var data = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Date >= Convert.ToDateTime(start).Date && Convert.ToDateTime(x.workDate).Date <= Convert.ToDateTime(end).Date).ToListAsync();
			return count;
		}

		public async Task<EmployeePunchCardInfo> GetPunchCardByUserName(string username)
		{
			var empCode = _context.employeeInfos.Where(x => x.ApplicationUser.UserName == username).Select(x => x.employeeCode).AsNoTracking().FirstOrDefault();
			return await _context.employeePunchCardInfos.Where(x => x.employeeCode == empCode).AsNoTracking().FirstOrDefaultAsync();
		}
		public async Task<EmployeePunchCardInfo> GetPunchCardByCode(string code)
		{
			return await _context.employeePunchCardInfos.Where(x => x.employeeCode == code).FirstOrDefaultAsync();
		}

		public async Task<string> GetUsernameByEMpCode(string empCode)
		{
			return await _context.Users.Where(x => x.EmpCode == empCode).Select(x => x.UserName).FirstOrDefaultAsync();
		}

		public async Task<EmpAttendance> GetAttendenceByDateAndPunchCard(DateTime? date, string punchCardNo)
		{
			return await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Date == Convert.ToDateTime(date).Date && x.punchCardNo == punchCardNo).OrderBy(x => x.workDate).AsNoTracking().FirstOrDefaultAsync();
		}
		public async Task<int> GetHolidaysByMonth(int month)
		{
			
            var data=await _context.holidays.Where(x=> Convert.ToDateTime(x.weeklyHoliday) >= Convert.ToDateTime(new DateTime(DateTime.Now.Year, month, 1)) && Convert.ToDateTime(x.weeklyHoliday) <= Convert.ToDateTime(new DateTime(DateTime.Now.Year, month, DateTime.DaysInMonth(DateTime.Now.Year, month)))).GroupBy(x=>x.weeklyHoliday).Distinct().AsNoTracking().CountAsync();
            return data;
		}

        public async Task<int> GetAllHolidaysByMonthYearNew(int month, int year)
        {
            var data = await _context.holidays.Where(x => Convert.ToDateTime(x.weeklyHoliday).Date >= new DateTime(year, month, 1).Date && Convert.ToDateTime(x.weeklyHoliday).Date <= new DateTime(year, month, DateTime.DaysInMonth(year, month)).Date).GroupBy(x => x.weeklyHoliday).Distinct().AsNoTracking().CountAsync();
            return data;
        }


        public async Task<int> GetHolidaysByDateRange(DateTime fromDate, DateTime toDate)
		{
			return await _context.holidays.Where(x=> (Convert.ToDateTime(x.holiday).Date >= Convert.ToDateTime(fromDate).Date && Convert.ToDateTime(x.holiday).Date <= Convert.ToDateTime(toDate).Date)).AsNoTracking().CountAsync();
		}
		public async Task<EmpAttendance> GetLateAttendenceByDateAndPunchCard(DateTime? date, string punchCardNo)
		{
			return await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Date == Convert.ToDateTime(date).Date && x.latetime == null && x.punchCardNo == punchCardNo).OrderBy(x => x.workDate).AsNoTracking().FirstOrDefaultAsync();
		}
		public async Task<EmployeeInfo> GetEmployeeByUsername(string username)
		{
			return await _context.employeeInfos.Where(x => x.ApplicationUser.UserName == username).AsNoTracking().FirstOrDefaultAsync();
		}
		public async Task<int> CheckLeaveByDate(DateTime? mydateh, string empCode)
		{
            var employee = await _context.employeeInfos.Where(x => x.employeeCode == empCode).FirstOrDefaultAsync();
            return await _context.leaveRegisters.Where(x => x.leaveStatus == 3 && (Convert.ToDateTime(x.leaveFrom).Date <= Convert.ToDateTime(mydateh).Date && Convert.ToDateTime(x.leaveTo).Date >= Convert.ToDateTime(mydateh).Date) && x.employeeId == employee.Id).AsNoTracking().CountAsync();
		}
		public async Task<int> MonthlyAttendenceCountByUserAndPunchCard(string username, int month)
		{
			var empCode = _context.employeeInfos.Where(x => x.ApplicationUser.UserName == username).Select(x => x.employeeCode).AsNoTracking().FirstOrDefault();
			var punchCard = _context.employeePunchCardInfos.Where(x => x.employeeCode == empCode).Select(x => x.punchCardNo).FirstOrDefault();
			var present1 = await _context.empAttendances.Where(x => x.punchCardNo == punchCard && Convert.ToDateTime(x.workDate).Month == month && Convert.ToDateTime(x.workDate).Year == DateTime.Now.Year).AsNoTracking().ToListAsync();

            var data = await _context.holidays.Where(x => Convert.ToDateTime(x.weeklyHoliday).Month == month && Convert.ToDateTime(x.weeklyHoliday).Year == DateTime.Now.Year).Select(x => x.weeklyHoliday).ToListAsync();
            var present = await _context.empAttendances.Where(x => x.punchCardNo == punchCard && !data.Contains(Convert.ToDateTime(x.workDate)) && Convert.ToDateTime(x.workDate).Month == month && Convert.ToDateTime(x.workDate).Year == DateTime.Now.Year).GroupBy(x => x.workDate).Select(x => x.First()).AsNoTracking().CountAsync();
			return present;
		}
		public async Task<int> MonthlyPresentAttendanceByPunchCard(string punch, int month)
		{
            var data = await _context.holidays.Where(x => Convert.ToDateTime(x.weeklyHoliday).Month == month && Convert.ToDateTime(x.weeklyHoliday).Year == DateTime.Now.Year).Select(x => x.weeklyHoliday).ToListAsync();
			return await _context.empAttendances.Where(x => x.punchCardNo == punch && !data.Contains(Convert.ToDateTime(x.workDate)) && Convert.ToDateTime(x.workDate).Month == month && Convert.ToDateTime(x.workDate).Year == DateTime.Now.Year).GroupBy(x => x.workDate).Select(x => x.First()).CountAsync();
		}
        public async Task<int> MonthlyPresentAttendanceByPunchCard(string punch, int month,int year)
        {
            var data = await _context.holidays.Where(x => Convert.ToDateTime(x.weeklyHoliday).Month == month && Convert.ToDateTime(x.weeklyHoliday).Year == year).Select(x => x.weeklyHoliday).ToListAsync();
            return await _context.empAttendances.Where(x => x.punchCardNo == punch && !data.Contains(Convert.ToDateTime(x.workDate)) && Convert.ToDateTime(x.workDate).Month == month && Convert.ToDateTime(x.workDate).Year == year).GroupBy(x => x.workDate).Select(x => x.First()).CountAsync();
        }
        public async Task<int> PresentAttendanceByPunchCardAndDateRange(string punch, DateTime fromDate, DateTime toDate)
        {
            return await _context.empAttendances.Where(x => x.punchCardNo == punch && (Convert.ToDateTime(x.workDate).Date >= Convert.ToDateTime(fromDate).Date && Convert.ToDateTime(x.workDate).Date <= Convert.ToDateTime(fromDate).Date)).GroupBy(x => x.workDate).Select(x => x.First()).CountAsync();
        }

        public async Task<IEnumerable<EmployeeInfo>> GetAllEmployeeWithPunchCard()
		{
			var data = await (from e in _context.employeeInfos
							  join p in _context.employeePunchCardInfos
							  on e.employeeCode equals p.employeeCode
							  join d in _context.designations
							  on e.lastDesignationId equals d.Id
							  select e).ToListAsync();
			return data;
		}
		public async Task<int> GetTotalWorkingDaysByEmpId(int id, int year, int month)
		{
			var data = await (from e in _context.employeeInfos
							  join p in _context.employeePunchCardInfos on e.employeeCode equals p.employeeCode
							  join a in _context.empAttendances on p.punchCardNo equals a.punchCardNo
							  where e.Id == id && Convert.ToDateTime(a.workDate).Year == year && Convert.ToDateTime(a.workDate).Month == month
							  select a).CountAsync();
			return data;
		}
		public async Task<int> GetTotalLateByEmpId(int id, int year, int month)
		{
			var data = await (from e in _context.employeeInfos
							  join p in _context.employeePunchCardInfos on e.employeeCode equals p.employeeCode
							  join a in _context.empAttendances on p.punchCardNo equals a.punchCardNo
							  where e.Id == id && a.latetime != null && Convert.ToDateTime(a.workDate).Year == year && Convert.ToDateTime(a.workDate).Month == month
							  select a).CountAsync();
			return data;
		}
		public async Task<IEnumerable<EmployeeInfo>> GetAllEmployeeWithPunchCardNo(int department, string designation, int shift)
		{
			var data = new List<EmployeeInfo>();

			if (department != 0 && designation == "A" && shift == 0)
			{
				data = await _context.employeeInfos.Include(x => x.department).Include(x => x.lastDesignation).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.departmentId == department && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (department == 0 && designation != "A" && shift == 0)
			{
				data = await _context.employeeInfos.Include(x => x.department).Include(x => x.lastDesignation).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignation.designationName == designation && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (department == 0 && designation == "A" && shift != 0)
			{
				data = await _context.employeeInfos.Include(x => x.department).Include(x => x.lastDesignation).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (department != 0 && designation != "A" && shift == 0)
			{
				data = await _context.employeeInfos.Include(x => x.department).Include(x => x.lastDesignation).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignation.designationName == designation && x.departmentId == department && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (department == 0 && designation != "A" && shift != 0)
			{
				data = await _context.employeeInfos.Include(x => x.department).Include(x => x.lastDesignation).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignation.designationName == designation && x.shiftGroupId == shift && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (department != 0 && designation == "A" && shift != 0)
			{
				data = await _context.employeeInfos.Include(x => x.department).Include(x => x.lastDesignation).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.departmentId == department && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (department != 0 && designation != "A" && shift != 0)
			{
				data = await _context.employeeInfos.Include(x => x.department).Include(x => x.lastDesignation).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignation.designationName == designation && x.departmentId == department && x.shiftGroupId== shift && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else
			{
				data = await _context.employeeInfos.Include(x => x.department).Include(x => x.lastDesignation).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			return data;
		}
		public async Task<IEnumerable<EmployeeInfo>> GetAllEmployeeWithPunchCardNoForDDSZO(int department, string designation, int shift, int zone, int office)
		{
			var data = new List<EmployeeInfo>();

			if (department != 0 && designation == "A" && shift == 0 && zone==0 && office==0)
			{
				data = await _context.employeeInfos.Include(x => x.department).Include(x => x.lastDesignation).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.departmentId == department && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (department == 0 && designation != "A" && shift == 0 && zone == 0 && office == 0)
			{
				data = await _context.employeeInfos.Include(x => x.department).Include(x => x.lastDesignation).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignation.designationName == designation && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (department == 0 && designation == "A" && shift != 0 && zone == 0 && office == 0)
			{
				data = await _context.employeeInfos.Include(x => x.department).Include(x => x.lastDesignation).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}

			else if (department == 0 && designation == "A" && shift == 0 && zone != 0 && office == 0)
			{
				data = await _context.employeeInfos.Include(x => x.department).Include(x => x.lastDesignation).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x =>x.locationId==zone   && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}

			else if (department == 0 && designation == "A" && shift == 0 && zone == 0 && office != 0)
			{
				data = await _context.employeeInfos.Include(x => x.department).Include(x => x.lastDesignation).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x =>x.functionInfoId==office && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}


			else if (department != 0 && designation != "A" && shift == 0 && zone == 0 && office == 0)
			{
				data = await _context.employeeInfos.Include(x => x.department).Include(x => x.lastDesignation).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignation.designationName == designation && x.departmentId == department && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (department == 0 && designation != "A" && shift == 0 && zone != 0 && office == 0)
			{
				data = await _context.employeeInfos.Include(x => x.department).Include(x => x.lastDesignation).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignation.designationName == designation && x.locationId == zone && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			 else if (department == 0 && designation != "A" && shift == 0 && zone == 0 && office != 0)
			{
				data = await _context.employeeInfos.Include(x => x.department).Include(x => x.lastDesignation).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignation.designationName == designation && x.functionInfoId == office && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}


			else if (department == 0 && designation != "A" && shift != 0 && zone == 0 && office == 0)
			{
				data = await _context.employeeInfos.Include(x => x.department).Include(x => x.lastDesignation).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignation.designationName == designation && x.shiftGroupId == shift && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (department != 0 && designation == "A" && shift != 0 && zone == 0 && office == 0)
			{
				data = await _context.employeeInfos.Include(x => x.department).Include(x => x.lastDesignation).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.departmentId == department && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (department != 0 && designation != "A" && shift != 0 && zone!=0 && office !=0)
			{
				data = await _context.employeeInfos.Include(x => x.department).Include(x => x.lastDesignation).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignation.designationName == designation && x.departmentId == department && x.functionInfoId==office && x.locationId==zone && x.shiftGroupId== shift && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else
			{
				data = await _context.employeeInfos.Include(x => x.department).Include(x => x.lastDesignation).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			return data;
		}

		public async Task<IEnumerable<EmployeeInfo>> GetAllEmployeeWithPunchCardNoBranch(int branch, string designation, int shift)
		{
			var employees = new List<EmployeeInfo>();

			if (branch != 0)
			{
				employees = await _context.employeePostings.Include(x => x.employee).Include(x => x.employee.lastDesignation).OrderBy(x => Convert.ToInt32(x.employee.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.employee.seniorityLevel)).Where(x => x.hrBranchId == branch && x.employee.activityStatus != 0 && x.employee.lastDesignation != null && x.status == 1).Select(x => x.employee).ToListAsync();
			}
			else
			{
				employees = await _context.employeePostings.Include(x => x.employee).Include(x => x.employee.lastDesignation).OrderBy(x => Convert.ToInt32(x.employee.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.employee.seniorityLevel)).Where(x => x.hrBranchId != null && x.employee.lastDesignation != null && x.employee.activityStatus != 0 && x.status == 1).Select(x => x.employee).ToListAsync();
			}

			var data = new List<EmployeeInfo>();
			if (branch != 0 && designation == "A" && shift == 0)
			{
				data = await _context.employeeInfos.Include(x => x.hrBranch).Include(x => x.lastDesignation).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.hrBranchId == branch && x.activityStatus != 0 && x.lastDesignation != null).ToListAsync();
			}
			else if (branch == 0 && designation != "A" && shift == 0)
			{
				data = await _context.employeeInfos.Include(x => x.hrBranch).Include(x => x.lastDesignation).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignation.designationName == designation && x.activityStatus != 0 && x.lastDesignation != null).ToListAsync();
			}
			else if (branch == 0 && designation == "A" && shift != 0)
			{
				data = await _context.employeeInfos.Include(x => x.hrBranch).Include(x => x.lastDesignation).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.activityStatus != 0 && x.lastDesignation != null).ToListAsync();
			}
			else if (branch != 0 && designation != "A" && shift == 0)
			{
				data = await _context.employeeInfos.Include(x => x.hrBranch).Include(x => x.lastDesignation).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignation.designationName == designation && x.hrBranchId == branch && x.activityStatus != 0 && x.lastDesignation != null).ToListAsync();
			}
			else if (branch == 0 && designation != "A" && shift != 0) //ok
			{
				data = employees.Where(x => x.lastDesignation.designationName == designation).ToList();
				//data = await _context.employeeInfos.Include(x => x.hrBranch).Include(x => x.lastDesignation).Where(x => x.lastDesignation.designationName == designation && x.shiftGroupId == shift && x.activityStatus != 0).ToListAsync();
			}
			else if (branch != 0 && designation == "A" && shift != 0) //ok
			{
				data = employees.ToList();
				//data = await _context.employeeInfos.Include(x => x.hrBranch).Include(x => x.lastDesignation).Where(x => x.hrBranchId == branch && x.activityStatus != 0).ToListAsync();
			}
			else if (branch != 0 && designation != "A" && shift != 0) //ok
			{
				data = employees.Where(x => x.lastDesignation.designationName == designation && x.activityStatus != 0).ToList();
				//data = await _context.employeeInfos.Include(x => x.hrBranch).Include(x => x.lastDesignation).Where(x => x.lastDesignation.designationName == designation && x.hrBranchId == branch && x.shiftGroupId == shift && x.activityStatus != 0).ToListAsync();
			}
			else
			{
				data = employees;
			}
			return data;
		}

	




		public async Task<IEnumerable<EmployeeInfo>> GetAllEmployeeWithPunchCardNoDivision(int division, string designation, int shift)
		{
			var data = new List<EmployeeInfo>();
			if (division != 0 && designation == "A" && shift == 0)
			{
				data = await _context.employeeInfos.Include(x => x.hrDivision).Include(x => x.lastDesignation).Include(x => x.location).Include(x => x.functionInfo).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.hrBranchId != null && x.activityStatus != 0).Where(x => x.hrDivisionId == division && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (division == 0 && designation != "A" && shift == 0)
			{
				data = await _context.employeeInfos.Include(x => x.hrDivision).Include(x => x.lastDesignation).Include(x => x.location).Include(x => x.functionInfo).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignation.designationName == designation && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (division == 0 && designation == "A" && shift != 0)
			{
				data = await _context.employeeInfos.Include(x => x.hrDivision).Include(x => x.lastDesignation).Include(x => x.location).Include(x => x.functionInfo).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (division != 0 && designation != "A" && shift == 0)
			{
				data = await _context.employeeInfos.Include(x => x.hrDivision).Include(x => x.lastDesignation).Include(x => x.location).Include(x => x.functionInfo).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignation.designationName == designation && x.hrDivisionId == division && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (division == 0 && designation != "A" && shift != 0)
			{
				data = await _context.employeeInfos.Include(x => x.hrDivision).Include(x => x.lastDesignation).Include(x => x.location).Include(x => x.functionInfo).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignation.designationName == designation && x.shiftGroupId == shift && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (division != 0 && designation == "A" && shift != 0)
			{
				data = await _context.employeeInfos.Include(x => x.hrDivision).Include(x => x.lastDesignation).Include(x => x.location).Include(x => x.functionInfo).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.hrDivisionId == division && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (division != 0 && designation != "A" && shift != 0)
			{
				data = await _context.employeeInfos.Include(x => x.hrDivision).Include(x => x.lastDesignation).Include(x => x.location).Include(x => x.functionInfo).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignation.designationName == designation && x.hrDivisionId == division && x.shiftGroupId == shift && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else
			{
				data = await _context.employeeInfos.Include(x => x.hrDivision).Include(x => x.lastDesignation).Include(x => x.location).Include(x => x.functionInfo).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			return data;
		}
		

		  public async Task<IEnumerable<EmployeeInfo>> GetAllEmployeeWithPunchCardNoZoneNew(int zone, string designation, int shift)
		{
			var data = new List<EmployeeInfo>();
			if (zone != 0 && designation == "A" && shift == 0)
			{
				data = await _context.employeeInfos.Include(x => x.hrDivision).Include(x => x.lastDesignation).Include(x => x.location).Include(x => x.functionInfo).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.locationId == zone && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (zone == 0 && designation != "A" && shift == 0)
			{
				data = await _context.employeeInfos.Include(x => x.hrDivision).Include(x => x.lastDesignation).Include(x => x.location).Include(x => x.functionInfo).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignation.designationName == designation && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (zone == 0 && designation == "A" && shift != 0)
			{
				data = await _context.employeeInfos.Include(x => x.hrDivision).Include(x => x.lastDesignation).Include(x => x.location).Include(x => x.functionInfo).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (zone != 0 && designation != "A" && shift == 0)
			{
				data = await _context.employeeInfos.Include(x => x.hrDivision).Include(x => x.lastDesignation).Include(x => x.location).Include(x => x.functionInfo).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignation.designationName == designation && x.locationId == zone && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (zone == 0 && designation != "A" && shift != 0)
			{
				data = await _context.employeeInfos.Include(x => x.hrDivision).Include(x => x.lastDesignation).Include(x => x.location).Include(x => x.functionInfo).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignation.designationName == designation && x.shiftGroupId == shift && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (zone != 0 && designation == "A" && shift != 0)
			{
				data = await _context.employeeInfos.Include(x => x.hrDivision).Include(x => x.lastDesignation).Include(x => x.location).Include(x => x.functionInfo).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.locationId == zone && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (zone != 0 && designation != "A" && shift != 0)
			{
				data = await _context.employeeInfos.Include(x => x.hrDivision).Include(x => x.lastDesignation).Include(x => x.location).Include(x => x.functionInfo).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignation.designationName == designation && x.locationId == zone && x.shiftGroupId == shift && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else
			{
				data = await _context.employeeInfos.Include(x => x.hrDivision).Include(x => x.lastDesignation).Include(x => x.location).Include(x => x.functionInfo).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			return data;
		}

		public async Task<IEnumerable<EmployeeInfo>> GetAllEmployeeWithPunchCardNoOffice(int office, string designation, int shift)
		{
			var data = new List<EmployeeInfo>();
			if (office != 0 && designation == "A" && shift == 0)
			{
				data = await _context.employeeInfos.Include(x => x.hrDivision).Include(x => x.lastDesignation).Include(x => x.location).Include(x => x.functionInfo).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.functionInfoId == office && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (office == 0 && designation != "A" && shift == 0)
			{
				data = await _context.employeeInfos.Include(x => x.hrDivision).Include(x => x.lastDesignation).Include(x => x.location).Include(x => x.functionInfo).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignation.designationName == designation && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (office == 0 && designation == "A" && shift != 0)
			{
				data = await _context.employeeInfos.Include(x => x.hrDivision).Include(x => x.lastDesignation).Include(x => x.location).Include(x => x.functionInfo).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (office != 0 && designation != "A" && shift == 0)
			{
				data = await _context.employeeInfos.Include(x => x.hrDivision).Include(x => x.lastDesignation).Include(x => x.location).Include(x => x.functionInfo).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignation.designationName == designation && x.functionInfoId == office && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (office == 0 && designation != "A" && shift != 0)
			{
				data = await _context.employeeInfos.Include(x => x.hrDivision).Include(x => x.lastDesignation).Include(x => x.location).Include(x => x.functionInfo).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignation.designationName == designation && x.shiftGroupId == shift && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (office != 0 && designation == "A" && shift != 0)
			{
				data = await _context.employeeInfos.Include(x => x.hrDivision).Include(x => x.lastDesignation).Include(x => x.location).Include(x => x.functionInfo).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.functionInfoId == office && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else if (office != 0 && designation != "A" && shift != 0)
			{
				data = await _context.employeeInfos.Include(x => x.hrDivision).Include(x => x.lastDesignation).Include(x => x.location).Include(x => x.functionInfo).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignation.designationName == designation && x.functionInfoId == office && x.shiftGroupId == shift && x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			else
			{
				data = await _context.employeeInfos.Include(x => x.hrDivision).Include(x => x.lastDesignation).Include(x => x.location).Include(x => x.functionInfo).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.activityStatus != 0 && x.lastDesignationId != null).ToListAsync();
			}
			return data;
		}


		public async Task<int> GetTotalPresentByMonthandYear(int month, int year)
		{
			var data = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Month == month && Convert.ToDateTime(x.workDate).Year == year).ToListAsync();
			return data.Count;
		}
		public async Task<int> GetTotalPresentByMonthandYear(int month, int year, int department, string designation, int shift)
		{
			var count = 0;
			if (year != -1 && month != -1)
			{
				var data = await (from e in _context.employeeInfos
								  join p in _context.employeePunchCardInfos on e.employeeCode equals p.employeeCode
								  join a in _context.empAttendances on p.punchCardNo equals a.punchCardNo
								  where Convert.ToDateTime(a.workDate).Year == year && Convert.ToDateTime(a.workDate).Month == month
								  select e).ToListAsync();
				if (department != 0 && designation == "0" && shift == 0)
				{
					count = data.Where(x => x.departmentId == department).Count();

				}
				else if (department == 0 && designation != "0" && shift == 0)
				{
					count = data.Where(x => x.lastDesignation.designationName == designation).Count();

				}
				else if (department == 0 && designation == "0" && shift != 0)
				{
					count = data.Where(x => x.shiftGroupId == shift).Count();

				}
				else if (department != 0 && designation != "0" && shift == 0)
				{
					count = data.Where(x => x.departmentId == department && x.lastDesignation.designationName == designation).Count();

				}
				else if (department == 0 && designation != "0" && shift != 0)
				{
					count = data.Where(x => x.lastDesignation.designationName == designation && x.shiftGroupId == shift).Count();

				}
				else if (department != 0 && designation == "0" && shift != 0)
				{
					count = data.Where(x => x.departmentId == department && x.shiftGroupId == shift).Count();

				}
				else if (department != 0 && designation != "0" && shift != 0)
				{
					count = data.Where(x => x.departmentId == department && x.lastDesignation.designationName == designation && x.shiftGroupId == shift).Count();
				}
				else
				{
					count = data.Count();
				}
			}
			else
			{

			}
			

			//var data = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Month == month && Convert.ToDateTime(x.workDate).Year == year).ToListAsync();
			return count;
		}
		//public async Task<IEnumerable<EmpAttendanceWithDates>> GetTotalPresentByMonthandYearDepartDesigShift(int month, int year, string code, string department, string designation, string shift)
		//{
		//    if (department != null && designation == "0" && shift == null)
		//    {
		//        var punchCard = _context.employeePunchCardInfos.Where(x => x.employeeCode == code).FirstOrDefault();

		//        var dates = new List<DateTime>();

		//        for (var date = new DateTime(DateTime.Now.Year, month, 1); date.Month == month; date = date.AddDays(1))
		//        {
		//            dates.Add(date);
		//        }

		//        var data1 = new List<EmpAttendanceWithDates>();

		//        foreach (var item in dates)
		//        {
		//            data1.Add(new EmpAttendanceWithDates
		//            {
		//                date = item,
		//                empAttendance = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Date == Convert.ToDateTime(item).Date && x.punchCardNo == punchCard.punchCardNo).AsNoTracking().FirstOrDefaultAsync()
		//            });
		//        }
		//        return data1;
		//    }
		//}

		public async Task<int> GetTotalLateByMonth(int month, int year)
		{
			var data = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Month == month && Convert.ToDateTime(x.workDate).Year == year && x.latetime != null).ToListAsync();
			return data.Count;
		}
		public async Task<int> GetTotalLateByMonth(int month, string cardNo, int year)
		{
			var data = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Month == month && Convert.ToDateTime(x.workDate).Year == year && x.punchCardNo == cardNo && x.latetime != null).ToListAsync();
			return data.Count;
		}
		public async Task<IEnumerable<EmployeeInfo>> GetTotalLeaveByMonth(int month, int year)
		{
			var firstDate = new DateTime(year, month, 1);
			var lastDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

			var lst = new List<EmployeeInfo>();

			var leaveList = await _context.leaveRegisters.ToListAsync();
			var empList = await (from e in _context.employeeInfos
								 join p in _context.employeePunchCardInfos
								 on e.employeeCode equals p.employeeCode
								 select e).ToListAsync();

			foreach (var item in empList)
			{
				var count = 0;
				foreach (var leave in leaveList.Where(x => x.employeeId == item.Id))
				{
					if (Convert.ToDateTime(leave.leaveFrom).Date >= firstDate.Date && Convert.ToDateTime(leave.leaveTo).Date <= lastDate.Date)
					{
						//in this month
						DateTime[] dates = { Convert.ToDateTime(leave.leaveFrom), Convert.ToDateTime(leave.leaveTo) };
						var fridays = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						var saturdays = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						var totaldays = fridays + (saturdays / 2);
						var diff = Convert.ToDateTime(leave.leaveTo).Day - Convert.ToDateTime(leave.leaveFrom).Day + 1;
						count += diff - totaldays;
					}
					else if (Convert.ToDateTime(leave.leaveFrom).Date >= firstDate.Date && Convert.ToDateTime(leave.leaveTo).Date > lastDate.Date)
					{
						//only first date in month
						DateTime[] dates = { Convert.ToDateTime(leave.leaveFrom), lastDate };
						var fridays = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						var saturdays = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						var totaldays = fridays + (saturdays / 2);
						var diff = lastDate.Day - Convert.ToDateTime(leave.leaveFrom).Day + 1;
						count += diff - totaldays;

						//DateTime[] dates1 = { Convert.ToDateTime(leave.leaveTo), new DateTime(year, month + 1, DateTime.DaysInMonth(year, month + 1)) };
						//var friday = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						//var saturday = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						//var totalday = fridays + (saturdays / 2);
						//var dif = Convert.ToDateTime(leave.leaveTo).Day;
						//count += dif - totalday;
					}
					else if (Convert.ToDateTime(leave.leaveFrom).Date < firstDate.Date && Convert.ToDateTime(leave.leaveTo).Date > firstDate.Date)
					{
						//only first date in month
						DateTime[] dates = { Convert.ToDateTime(firstDate), Convert.ToDateTime(leave.leaveTo).Date };
						var fridays = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						var saturdays = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						var totaldays = fridays + (saturdays / 2);
						var diff = Convert.ToDateTime(leave.leaveTo).Day;
						count += diff - totaldays;
					}
					else
					{
						count += 0;
					}
				}

				lst.Add(new EmployeeInfo
				{
					Id = item.Id,
					nameEnglish = item.nameEnglish,
					isDelete = count
				});
			}
			return lst;

		}

		public async Task<IEnumerable<EmployeeInfo>> GetIndivisualTotalLeaveByMonth(int month)
		{
			var year = DateTime.Now.Year;
			var firstDate = new DateTime(year, month, 1);
			var lastDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

			var lst = new List<EmployeeInfo>();

			var leaveList = await _context.leaveRegisters.ToListAsync();
			var empList = await (from e in _context.employeeInfos
								 join p in _context.employeePunchCardInfos
								 on e.employeeCode equals p.employeeCode
								 select e).ToListAsync();

			foreach (var item in empList)
			{
				var count = 0;
				foreach (var leave in leaveList.Where(x => x.employeeId == item.Id))
				{
					if (Convert.ToDateTime(leave.leaveFrom).Date >= firstDate.Date && Convert.ToDateTime(leave.leaveTo).Date <= lastDate.Date)
					{
						//in this month
						DateTime[] dates = { Convert.ToDateTime(leave.leaveFrom), Convert.ToDateTime(leave.leaveTo) };
						var fridays = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						var saturdays = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						var totaldays = fridays + (saturdays / 2);
						var diff = Convert.ToDateTime(leave.leaveTo).Day - Convert.ToDateTime(leave.leaveFrom).Day + 1;
						count += diff - totaldays;
					}
					else if (Convert.ToDateTime(leave.leaveFrom).Date >= firstDate.Date && Convert.ToDateTime(leave.leaveTo).Date > lastDate.Date)
					{
						//only first date in month
						DateTime[] dates = { Convert.ToDateTime(leave.leaveFrom), lastDate };
						var fridays = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						var saturdays = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						var totaldays = fridays + (saturdays / 2);
						var diff = lastDate.Day - Convert.ToDateTime(leave.leaveFrom).Day + 1;
						count += diff - totaldays;

						//DateTime[] dates1 = { Convert.ToDateTime(leave.leaveTo), new DateTime(year, month + 1, DateTime.DaysInMonth(year, month + 1)) };
						//var friday = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						//var saturday = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						//var totalday = fridays + (saturdays / 2);
						//var dif = Convert.ToDateTime(leave.leaveTo).Day;
						//count += dif - totalday;
					}
					else if (Convert.ToDateTime(leave.leaveFrom).Date < firstDate.Date && Convert.ToDateTime(leave.leaveTo).Date > firstDate.Date)
					{
						//only first date in month
						DateTime[] dates = { Convert.ToDateTime(firstDate), Convert.ToDateTime(leave.leaveTo).Date };
						var fridays = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						var saturdays = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						var totaldays = fridays + (saturdays / 2);
						var diff = Convert.ToDateTime(leave.leaveTo).Day;
						count += diff - totaldays;
					}
					else
					{
						count += 0;
					}
				}

				lst.Add(new EmployeeInfo
				{
					Id = item.Id,
					nameEnglish = item.nameEnglish,
					isDelete = count
				});
			}
			return lst;

		}

        public async Task<IEnumerable<EmployeeInfo>> GetIndivisualTotalLeaveByDateRange(DateTime fromDate, DateTime toDate)
		{

			var lst = new List<EmployeeInfo>();

			var leaveList = await _context.leaveRegisters.ToListAsync();
			var empList = await (from e in _context.employeeInfos
								 join p in _context.employeePunchCardInfos
								 on e.employeeCode equals p.employeeCode
								 select e).ToListAsync();

			foreach (var item in empList)
			{
				var count = 0;
				foreach (var leave in leaveList.Where(x => x.employeeId == item.Id))
				{
					if (Convert.ToDateTime(leave.leaveFrom).Date >= fromDate.Date && Convert.ToDateTime(leave.leaveTo).Date <= toDate)
					{
						//in this month
						DateTime[] dates = { Convert.ToDateTime(leave.leaveFrom), Convert.ToDateTime(leave.leaveTo) };
						var fridays = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						var saturdays = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						var totaldays = fridays + (saturdays / 2);
						var diff = Convert.ToDateTime(leave.leaveTo).Day - Convert.ToDateTime(leave.leaveFrom).Day + 1;
						count += diff - totaldays;
					}
					else if (Convert.ToDateTime(leave.leaveFrom).Date >= fromDate.Date && Convert.ToDateTime(leave.leaveTo).Date > toDate)
					{
						//only first date in month
						DateTime[] dates = { Convert.ToDateTime(leave.leaveFrom), toDate };
						var fridays = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						var saturdays = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						var totaldays = fridays + (saturdays / 2);
						var diff = toDate.Day - Convert.ToDateTime(leave.leaveFrom).Day + 1;
						count += diff - totaldays;

						//DateTime[] dates1 = { Convert.ToDateTime(leave.leaveTo), new DateTime(year, month + 1, DateTime.DaysInMonth(year, month + 1)) };
						//var friday = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						//var saturday = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						//var totalday = fridays + (saturdays / 2);
						//var dif = Convert.ToDateTime(leave.leaveTo).Day;
						//count += dif - totalday;
					}
					else if (Convert.ToDateTime(leave.leaveFrom).Date < fromDate && Convert.ToDateTime(leave.leaveTo).Date > fromDate)
					{
						//only first date in month
						DateTime[] dates = { Convert.ToDateTime(fromDate), Convert.ToDateTime(leave.leaveTo).Date };
						var fridays = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						var saturdays = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						var totaldays = fridays + (saturdays / 2);
						var diff = Convert.ToDateTime(leave.leaveTo).Day;
						count += diff - totaldays;
					}
					else
					{
						count += 0;
					}
				}

				lst.Add(new EmployeeInfo
				{
					Id = item.Id,
					nameEnglish = item.nameEnglish,
					isDelete = count
				});
			}
			return lst;

		}

		public async Task<IEnumerable<EmployeeInfo>> GetDailyTotalLeaveByMonth()
		{
			var year = DateTime.Now.Year;
			var month = DateTime.Now.Month;
			var day = DateTime.Now.Day;
			var firstDate = new DateTime(year, month, day);
			var lastDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

			var lst = new List<EmployeeInfo>();

			var leaveList = await _context.leaveRegisters.ToListAsync();
			var empList = await (from e in _context.employeeInfos
								 join p in _context.employeePunchCardInfos
								 on e.employeeCode equals p.employeeCode
								 select e).ToListAsync();

			foreach (var item in empList)
			{
				var count = 0;
				foreach (var leave in leaveList.Where(x => x.employeeId == item.Id))
				{
					if (Convert.ToDateTime(leave.leaveFrom).Date >= firstDate.Date && Convert.ToDateTime(leave.leaveTo).Date <= lastDate.Date)
					{
						//in this month
						DateTime[] dates = { Convert.ToDateTime(leave.leaveFrom), Convert.ToDateTime(leave.leaveTo) };
						var fridays = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						var saturdays = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						var totaldays = fridays + (saturdays / 2);
						var diff = Convert.ToDateTime(leave.leaveTo).Day - Convert.ToDateTime(leave.leaveFrom).Day + 1;
						count += diff - totaldays;
					}
					else if (Convert.ToDateTime(leave.leaveFrom).Date >= firstDate.Date && Convert.ToDateTime(leave.leaveTo).Date > lastDate.Date)
					{
						//only first date in month
						DateTime[] dates = { Convert.ToDateTime(leave.leaveFrom), lastDate };
						var fridays = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						var saturdays = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						var totaldays = fridays + (saturdays / 2);
						var diff = lastDate.Day - Convert.ToDateTime(leave.leaveFrom).Day + 1;
						count += diff - totaldays;

						//DateTime[] dates1 = { Convert.ToDateTime(leave.leaveTo), new DateTime(year, month + 1, DateTime.DaysInMonth(year, month + 1)) };
						//var friday = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						//var saturday = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						//var totalday = fridays + (saturdays / 2);
						//var dif = Convert.ToDateTime(leave.leaveTo).Day;
						//count += dif - totalday;
					}
					else if (Convert.ToDateTime(leave.leaveFrom).Date < firstDate.Date && Convert.ToDateTime(leave.leaveTo).Date > firstDate.Date)
					{
						//only first date in month
						DateTime[] dates = { Convert.ToDateTime(firstDate), Convert.ToDateTime(leave.leaveTo).Date };
						var fridays = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						var saturdays = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						var totaldays = fridays + (saturdays / 2);
						var diff = Convert.ToDateTime(leave.leaveTo).Day;
						count += diff - totaldays;
					}
					else
					{
						count += 0;
					}
				}

				lst.Add(new EmployeeInfo
				{
					Id = item.Id,
					nameEnglish = item.nameEnglish,
					isDelete = count
				});
			}
			return lst;

		}
		public async Task<IEnumerable<EmployeeInfo>> GetTotalLeaveByMonthWP(int month, int year)
		{
			var firstDate = new DateTime(year, month, 1);
			var lastDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

			var lst = new List<EmployeeInfo>();

			var leaveList = await _context.leaveRegisters.Where(x => x.leaveTypeId == 10).ToListAsync();
			var empList = await (from e in _context.employeeInfos
								 join p in _context.employeePunchCardInfos
								 on e.employeeCode equals p.employeeCode
								 select e).ToListAsync();

			foreach (var item in empList)
			{
				var count = 0;
				foreach (var leave in leaveList.Where(x => x.employeeId == item.Id))
				{
					if (Convert.ToDateTime(leave.leaveFrom).Date >= firstDate.Date && Convert.ToDateTime(leave.leaveTo).Date <= lastDate.Date)
					{
						//in this month
						DateTime[] dates = { Convert.ToDateTime(leave.leaveFrom), Convert.ToDateTime(leave.leaveTo) };
						var fridays = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						var saturdays = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						var totaldays = fridays + (saturdays / 2);
						var diff = Convert.ToDateTime(leave.leaveTo).Day - Convert.ToDateTime(leave.leaveFrom).Day + 1;
						count += diff - totaldays;
					}
					else if (Convert.ToDateTime(leave.leaveFrom).Date >= firstDate.Date && Convert.ToDateTime(leave.leaveTo).Date > lastDate.Date)
					{
						//only first date in month
						DateTime[] dates = { Convert.ToDateTime(leave.leaveFrom), lastDate };
						var fridays = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						var saturdays = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						var totaldays = fridays + (saturdays / 2);
						var diff = lastDate.Day - Convert.ToDateTime(leave.leaveFrom).Day + 1;
						count += diff - totaldays;

						//DateTime[] dates1 = { Convert.ToDateTime(leave.leaveTo), new DateTime(year, month + 1, DateTime.DaysInMonth(year, month + 1)) };
						//var friday = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						//var saturday = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						//var totalday = fridays + (saturdays / 2);
						//var dif = Convert.ToDateTime(leave.leaveTo).Day;
						//count += dif - totalday;
					}
					else if (Convert.ToDateTime(leave.leaveFrom).Date < firstDate.Date && Convert.ToDateTime(leave.leaveTo).Date > firstDate.Date)
					{
						//only first date in month
						DateTime[] dates = { Convert.ToDateTime(firstDate), Convert.ToDateTime(leave.leaveTo).Date };
						var fridays = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						var saturdays = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						var totaldays = fridays + (saturdays / 2);
						var diff = Convert.ToDateTime(leave.leaveTo).Day;
						count += diff - totaldays;
					}
					else
					{
						count += 0;
					}
				}

				lst.Add(new EmployeeInfo
				{
					Id = item.Id,
					nameEnglish = item.nameEnglish,
					isDelete = count
				});
			}
			return lst;
		}

		public async Task<IEnumerable<EmployeeInfo>> GetIndivisualTotalLeaveByMonthWP(int month)
		{
			var year = DateTime.Now.Year;
			var firstDate = new DateTime(year, month, 1);
			var lastDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

			var lst = new List<EmployeeInfo>();

			var leaveList = await _context.leaveRegisters.Where(x => x.leaveTypeId == 10).ToListAsync();
			var empList = await (from e in _context.employeeInfos
								 join p in _context.employeePunchCardInfos
								 on e.employeeCode equals p.employeeCode
								 select e).ToListAsync();

			foreach (var item in empList)
			{
				var count = 0;
				foreach (var leave in leaveList.Where(x => x.employeeId == item.Id))
				{
					if (Convert.ToDateTime(leave.leaveFrom).Date >= firstDate.Date && Convert.ToDateTime(leave.leaveTo).Date <= lastDate.Date)
					{
						//in this month
						DateTime[] dates = { Convert.ToDateTime(leave.leaveFrom), Convert.ToDateTime(leave.leaveTo) };
						var fridays = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						var saturdays = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						var totaldays = fridays + (saturdays / 2);
						var diff = Convert.ToDateTime(leave.leaveTo).Day - Convert.ToDateTime(leave.leaveFrom).Day + 1;
						count += diff - totaldays;
					}
					else if (Convert.ToDateTime(leave.leaveFrom).Date >= firstDate.Date && Convert.ToDateTime(leave.leaveTo).Date > lastDate.Date)
					{
						//only first date in month
						DateTime[] dates = { Convert.ToDateTime(leave.leaveFrom), lastDate };
						var fridays = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						var saturdays = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						var totaldays = fridays + (saturdays / 2);
						var diff = lastDate.Day - Convert.ToDateTime(leave.leaveFrom).Day + 1;
						count += diff - totaldays;

						//DateTime[] dates1 = { Convert.ToDateTime(leave.leaveTo), new DateTime(year, month + 1, DateTime.DaysInMonth(year, month + 1)) };
						//var friday = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						//var saturday = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						//var totalday = fridays + (saturdays / 2);
						//var dif = Convert.ToDateTime(leave.leaveTo).Day;
						//count += dif - totalday;
					}
					else if (Convert.ToDateTime(leave.leaveFrom).Date < firstDate.Date && Convert.ToDateTime(leave.leaveTo).Date > firstDate.Date)
					{
						//only first date in month
						DateTime[] dates = { Convert.ToDateTime(firstDate), Convert.ToDateTime(leave.leaveTo).Date };
						var fridays = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						var saturdays = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						var totaldays = fridays + (saturdays / 2);
						var diff = Convert.ToDateTime(leave.leaveTo).Day;
						count += diff - totaldays;
					}
					else
					{
						count += 0;
					}
				}

				lst.Add(new EmployeeInfo
				{
					Id = item.Id,
					nameEnglish = item.nameEnglish,
					isDelete = count
				});
			}
			return lst;
		}

        public async Task<IEnumerable<EmployeeInfo>> GetIndivisualTotalLeaveByDateRangeWP(DateTime fromDate, DateTime toDate)
		{

			var lst = new List<EmployeeInfo>();

			var leaveList = await _context.leaveRegisters.Where(x => x.leaveTypeId == 10).ToListAsync();
			var empList = await (from e in _context.employeeInfos
								 join p in _context.employeePunchCardInfos
								 on e.employeeCode equals p.employeeCode
								 select e).ToListAsync();

			foreach (var item in empList)
			{
				var count = 0;
				foreach (var leave in leaveList.Where(x => x.employeeId == item.Id))
				{
					if (Convert.ToDateTime(leave.leaveFrom).Date >= fromDate && Convert.ToDateTime(leave.leaveTo).Date <= toDate)
					{
						//in this month
						DateTime[] dates = { Convert.ToDateTime(leave.leaveFrom), Convert.ToDateTime(leave.leaveTo) };
						var fridays = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						var saturdays = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						var totaldays = fridays + (saturdays / 2);
						var diff = Convert.ToDateTime(leave.leaveTo).Day - Convert.ToDateTime(leave.leaveFrom).Day + 1;
						count += diff - totaldays;
					}
					else if (Convert.ToDateTime(leave.leaveFrom).Date >= fromDate && Convert.ToDateTime(leave.leaveTo).Date > toDate)
					{
						//only first date in month
						DateTime[] dates = { Convert.ToDateTime(leave.leaveFrom), toDate };
						var fridays = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						var saturdays = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						var totaldays = fridays + (saturdays / 2);
						var diff = toDate.Day - Convert.ToDateTime(leave.leaveFrom).Day + 1;
						count += diff - totaldays;

						//DateTime[] dates1 = { Convert.ToDateTime(leave.leaveTo), new DateTime(year, month + 1, DateTime.DaysInMonth(year, month + 1)) };
						//var friday = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						//var saturday = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						//var totalday = fridays + (saturdays / 2);
						//var dif = Convert.ToDateTime(leave.leaveTo).Day;
						//count += dif - totalday;
					}
					else if (Convert.ToDateTime(leave.leaveFrom).Date < fromDate && Convert.ToDateTime(leave.leaveTo).Date > fromDate)
					{
						//only first date in month
						DateTime[] dates = { Convert.ToDateTime(fromDate), Convert.ToDateTime(leave.leaveTo).Date };
						var fridays = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						var saturdays = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						var totaldays = fridays + (saturdays / 2);
						var diff = Convert.ToDateTime(leave.leaveTo).Day;
						count += diff - totaldays;
					}
					else
					{
						count += 0;
					}
				}

				lst.Add(new EmployeeInfo
				{
					Id = item.Id,
					nameEnglish = item.nameEnglish,
					isDelete = count
				});
			}
			return lst;
		}

		public async Task<IEnumerable<EmployeeInfo>> GetTotalLeaveByDate(DateTime date)
		{
			var firstDate = Convert.ToDateTime(date).Date;
			var lastDate = Convert.ToDateTime(date).Date;

			var lst = new List<EmployeeInfo>();

			var leaveList = await _context.leaveRegisters.ToListAsync();
			var empList = await (from e in _context.employeeInfos
								 join p in _context.employeePunchCardInfos
								 on e.employeeCode equals p.employeeCode
								 select e).ToListAsync();

			foreach (var item in empList)
			{
				var count = 0;
				foreach (var leave in leaveList.Where(x => x.employeeId == item.Id))
				{
					if (Convert.ToDateTime(leave.leaveFrom).Date >= firstDate.Date && Convert.ToDateTime(leave.leaveTo).Date <= lastDate.Date)
					{
						//in this month
						DateTime[] dates = { Convert.ToDateTime(leave.leaveFrom), Convert.ToDateTime(leave.leaveTo) };
						var fridays = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						var saturdays = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						var totaldays = fridays + (saturdays / 2);
						var diff = Convert.ToDateTime(leave.leaveTo).Day - Convert.ToDateTime(leave.leaveFrom).Day + 1;
						count += diff - totaldays;
					}
					else if (Convert.ToDateTime(leave.leaveFrom).Date >= firstDate.Date && Convert.ToDateTime(leave.leaveTo).Date > lastDate.Date)
					{
						//only first date in month
						DateTime[] dates = { Convert.ToDateTime(leave.leaveFrom), lastDate };
						var fridays = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						var saturdays = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						var totaldays = fridays + (saturdays / 2);
						var diff = lastDate.Day - Convert.ToDateTime(leave.leaveFrom).Day + 1;
						count += diff - totaldays;

						//DateTime[] dates1 = { Convert.ToDateTime(leave.leaveTo), new DateTime(year, month + 1, DateTime.DaysInMonth(year, month + 1)) };
						//var friday = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						//var saturday = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						//var totalday = fridays + (saturdays / 2);
						//var dif = Convert.ToDateTime(leave.leaveTo).Day;
						//count += dif - totalday;
					}
					else if (Convert.ToDateTime(leave.leaveFrom).Date < firstDate.Date && Convert.ToDateTime(leave.leaveTo).Date > firstDate.Date)
					{
						//only first date in month
						DateTime[] dates = { Convert.ToDateTime(firstDate), Convert.ToDateTime(leave.leaveTo).Date };
						var fridays = dates.Where(x => x.DayOfWeek == DayOfWeek.Friday).Count();
						var saturdays = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday).Count();
						var totaldays = fridays + (saturdays / 2);
						var diff = Convert.ToDateTime(leave.leaveTo).Day;
						count += diff - totaldays;
					}
					else
					{
						count += 0;
					}
				}

				lst.Add(new EmployeeInfo
				{
					Id = item.Id,
					nameEnglish = item.nameEnglish,
					isDelete = count
				});
			}
			return lst;

		}
		public async Task<int> GetDailyTotalLateByMonth(DateTime? start)
		{
			var data = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Date >= Convert.ToDateTime(start)).ToListAsync();
			return data.Count;
		}
		public async Task<int> GetDailyTotalLateByMonthCard(DateTime? start,string cardNo)
		{
			var data = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Date >= Convert.ToDateTime(start) && x.punchCardNo == cardNo).ToListAsync();
			return data.Count;
		}

		public async Task<int> GetIndivisualTotalLateByMonth(int month)
		{
			var data = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Date.Month == month && Convert.ToDateTime(x.workDate).Year == DateTime.Now.Year).ToListAsync();
			return data.Count;
		}
		public async Task<int> GetIndivisualTotalLateByMonth(int month, string cardNo)
		{
			var data = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Date.Month == month && Convert.ToDateTime(x.workDate).Year == DateTime.Now.Year && x.punchCardNo == cardNo).ToListAsync();
			return data.Count;
		}
        public async Task<int> GetIndivisualTotalLateByDateRange(DateTime fromDate, DateTime toDate, string cardNo)
		{
			var data = await _context.empAttendances.Where(x => (Convert.ToDateTime(x.workDate).Date <= Convert.ToDateTime(fromDate).Date && Convert.ToDateTime(x.workDate).Date >= Convert.ToDateTime(fromDate).Date) && x.punchCardNo == cardNo).ToListAsync();
			return data.Count;
		}
  //      public async Task<int> GetIndivisualTotalLateByMonthEmp(int month,string cardNo)
		//{
		//	var data = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Date.Month == month && Convert.ToDateTime(x.workDate).Year == DateTime.Now.Year).ToListAsync();
		//	return data.Count;
		//}
		public async Task<int> GetIndivisualTotalLateByMonthAndCard(int month, string cardNo)
		{
			var data = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Date.Month == month && Convert.ToDateTime(x.workDate).Year == DateTime.Now.Year && x.punchCardNo == cardNo).ToListAsync();
			return data.Count;
		}
		//public async Task<int> GetTotalLeaveByMonth(int month, int year)
		//{
		//	var lst = new List<string>();
		//	var reg = await _context.leaveRegisters.ToListAsync();
		//	foreach (var item in reg)
		//	{
		public async Task<string> GetDepartmentNameById(int id)
		{
			return await _context.departments.Where(x => x.Id == id).Select(x => x.deptName).AsNoTracking().FirstOrDefaultAsync();
		}
		public async Task<string> GetShiftNameById(int id)
		{
			return await _context.shiftGroupMasters.Where(x => x.Id == id).Select(x => x.shiftName).AsNoTracking().FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<EmpAttendanceWithDates>> GetAttendenceByMonthAndEmpCode(int month, string code, int year)
		{
			var punchCard = _context.employeePunchCardInfos.Where(x => x.employeeCode == code).FirstOrDefault();

			var dates = new List<DateTime>();

			for (var date = new DateTime(DateTime.Now.Year, month, 1); date.Month == month; date = date.AddDays(1))
			{
				dates.Add(date);
			}

			var data = new List<EmpAttendanceWithDates>();

			foreach (var item in dates)
			{
				data.Add(new EmpAttendanceWithDates
				{
					date = item,
					empAttendance = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Date == Convert.ToDateTime(item).Date && Convert.ToDateTime(x.workDate).Month == Convert.ToDateTime(item).Month && Convert.ToDateTime(x.workDate).Year == Convert.ToDateTime(item).Year && x.punchCardNo == punchCard.punchCardNo).AsNoTracking().FirstOrDefaultAsync()
				});
			}
			return data;
		}
		public async Task<IEnumerable<EmpAttendanceWithDates>> GetAttendenceByMonthAndEmpCodeWithDepert1(int month, string code, int year, int department, string designation, int shift)
		{
			var punchCard = _context.employeePunchCardInfos.Where(x => x.employeeCode == code).FirstOrDefault();

			var dates = new List<DateTime>();

			for (var date = new DateTime(DateTime.Now.Year, month, 1); date.Month == month; date = date.AddDays(1))
			{
				dates.Add(date);
			}

			var data = new List<EmpAttendanceWithDates>();
			if (department != 0 && designation == "0" && shift == 0)
			{
				foreach (var item in dates)
				{
					data.Add(new EmpAttendanceWithDates
					{
						date = item,
						empAttendance = await (from a in _context.empAttendances
											   join p in _context.employeePunchCardInfos on a.punchCardNo equals p.punchCardNo
											   join e in _context.employeeInfos on p.employeeCode equals e.employeeCode
											   where Convert.ToDateTime(a.workDate).Date == Convert.ToDateTime(item).Date && e.employeeCode == code && e.departmentId == department
											   select a).AsNoTracking().FirstOrDefaultAsync()
						//empAttendance = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Date == Convert.ToDateTime(item).Date && x.punchCardNo == punchCard.punchCardNo).AsNoTracking().FirstOrDefaultAsync()
					});
				}
			}
			else if (department == 0 && designation != "0" && shift == 0)
			{
				foreach (var item in dates)
				{
					data.Add(new EmpAttendanceWithDates
					{
						date = item,
						empAttendance = await (from a in _context.empAttendances
											   join p in _context.employeePunchCardInfos on a.punchCardNo equals p.punchCardNo
											   join e in _context.employeeInfos on p.employeeCode equals e.employeeCode
											   where Convert.ToDateTime(a.workDate).Date == Convert.ToDateTime(item).Date && e.employeeCode == code && e.designation == designation
											   select a).AsNoTracking().FirstOrDefaultAsync()
					});
				}
			}
			else if (department == 0 && designation == "0" && shift != 0)
			{
				foreach (var item in dates)
				{
					data.Add(new EmpAttendanceWithDates
					{
						date = item,
						empAttendance = await (from a in _context.empAttendances
											   join p in _context.employeePunchCardInfos on a.punchCardNo equals p.punchCardNo
											   join e in _context.employeeInfos on p.employeeCode equals e.employeeCode
											   where Convert.ToDateTime(a.workDate).Date == Convert.ToDateTime(item).Date && e.employeeCode == code && e.shiftGroupId == shift
											   select a).AsNoTracking().FirstOrDefaultAsync()
					});
				}
			}
			else if (department != 0 && designation != "0" && shift == 0)
			{
				foreach (var item in dates)
				{
					data.Add(new EmpAttendanceWithDates
					{
						date = item,
						empAttendance = await (from a in _context.empAttendances
											   join p in _context.employeePunchCardInfos on a.punchCardNo equals p.punchCardNo
											   join e in _context.employeeInfos on p.employeeCode equals e.employeeCode
											   where Convert.ToDateTime(a.workDate).Date == Convert.ToDateTime(item).Date && e.employeeCode == code && e.departmentId == department && e.designation == designation
											   select a).AsNoTracking().FirstOrDefaultAsync()
					});
				}
			}
			else if (department == 0 && designation != "0" && shift != 0)
			{
				foreach (var item in dates)
				{
					data.Add(new EmpAttendanceWithDates
					{
						date = item,
						empAttendance = await (from a in _context.empAttendances
											   join p in _context.employeePunchCardInfos on a.punchCardNo equals p.punchCardNo
											   join e in _context.employeeInfos on p.employeeCode equals e.employeeCode
											   where Convert.ToDateTime(a.workDate).Date == Convert.ToDateTime(item).Date && e.employeeCode == code && e.designation == designation && e.shiftGroupId == shift
											   select a).AsNoTracking().FirstOrDefaultAsync()
					});
				}
			}
			else if (department != 0 && designation == "0" && shift != 0)
			{
				foreach (var item in dates)
				{
					data.Add(new EmpAttendanceWithDates
					{
						date = item,
						empAttendance = await (from a in _context.empAttendances
											   join p in _context.employeePunchCardInfos on a.punchCardNo equals p.punchCardNo
											   join e in _context.employeeInfos on p.employeeCode equals e.employeeCode
											   where Convert.ToDateTime(a.workDate).Date == Convert.ToDateTime(item).Date && e.employeeCode == code && e.departmentId == department && e.shiftGroupId == shift
											   select a).AsNoTracking().FirstOrDefaultAsync()
					});
				}
			}
			else if (department != 0 && designation != "0" && shift != 0)
			{
				foreach (var item in dates)
				{
					data.Add(new EmpAttendanceWithDates
					{
						date = item,
						empAttendance = await (from a in _context.empAttendances
											   join p in _context.employeePunchCardInfos on a.punchCardNo equals p.punchCardNo
											   join e in _context.employeeInfos on p.employeeCode equals e.employeeCode
											   where Convert.ToDateTime(a.workDate).Date == Convert.ToDateTime(item).Date && e.employeeCode == code && e.departmentId == department && e.designation == designation && e.shiftGroupId == shift
											   select a).AsNoTracking().FirstOrDefaultAsync()
					});
				}
			}
			else
			{
				foreach (var item in dates)
				{
					data.Add(new EmpAttendanceWithDates
					{
						date = item,
						empAttendance = await (from a in _context.empAttendances
											   join p in _context.employeePunchCardInfos on a.punchCardNo equals p.punchCardNo
											   join e in _context.employeeInfos on p.employeeCode equals e.employeeCode
											   where Convert.ToDateTime(a.workDate).Date == Convert.ToDateTime(item).Date && e.employeeCode == code
											   select a).AsNoTracking().FirstOrDefaultAsync()
						//empAttendance = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Date == Convert.ToDateTime(item).Date && x.punchCardNo == punchCard.punchCardNo).AsNoTracking().FirstOrDefaultAsync()
					});
				}
			}

			return data;
		}

		public async Task<IEnumerable<EmpAttendanceWithDates>> GetAttendenceByMonthAndEmpCodeWithDepert(int month, string code, int year, int department, string designation, int shift)
		{
			var punchCard = _context.employeePunchCardInfos.Where(x => x.employeeCode == code).FirstOrDefault();

			var dates = new List<DateTime>();

			for (var date = new DateTime(DateTime.Now.Year, month, 1); date.Month == month; date = date.AddDays(1))
			{
				dates.Add(date);
			}

			var data = new List<EmpAttendanceWithDates>();
			if (department != 0 && designation == "0" && shift == 0)
			{
				foreach (var item in dates)
				{
					data.Add(new EmpAttendanceWithDates
					{
						date = item,
						empAttendance = await (from a in _context.empAttendances
											   join p in _context.employeePunchCardInfos on a.punchCardNo equals p.punchCardNo
											   join e in _context.employeeInfos on p.employeeCode equals e.employeeCode
											   where Convert.ToDateTime(a.workDate).Month == month && Convert.ToDateTime(a.workDate).Year == year && e.employeeCode == code && e.departmentId == department
											   select a).AsNoTracking().FirstOrDefaultAsync()
						//empAttendance = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Date == Convert.ToDateTime(item).Date && x.punchCardNo == punchCard.punchCardNo).AsNoTracking().FirstOrDefaultAsync()
					});
				}
			}
			else if (department == 0 && designation != "0" && shift == 0)
			{
				foreach (var item in dates)
				{
					data.Add(new EmpAttendanceWithDates
					{
						date = item,
						empAttendance = await (from a in _context.empAttendances
											   join p in _context.employeePunchCardInfos on a.punchCardNo equals p.punchCardNo
											   join e in _context.employeeInfos on p.employeeCode equals e.employeeCode
											   where Convert.ToDateTime(a.workDate).Month == month && Convert.ToDateTime(a.workDate).Year == year && e.employeeCode == code && e.designation == designation
											   select a).AsNoTracking().FirstOrDefaultAsync()
					});
				}
			}
			else if (department == 0 && designation == "0" && shift != 0)
			{
				foreach (var item in dates)
				{
					data.Add(new EmpAttendanceWithDates
					{
						date = item,
						empAttendance = await (from a in _context.empAttendances
											   join p in _context.employeePunchCardInfos on a.punchCardNo equals p.punchCardNo
											   join e in _context.employeeInfos on p.employeeCode equals e.employeeCode
											   where Convert.ToDateTime(a.workDate).Month == month && Convert.ToDateTime(a.workDate).Year == year && e.employeeCode == code && e.shiftGroupId == shift
											   select a).AsNoTracking().FirstOrDefaultAsync()
					});
				}
			}
			else if (department != 0 && designation != "0" && shift == 0)
			{
				foreach (var item in dates)
				{
					data.Add(new EmpAttendanceWithDates
					{
						date = item,
						empAttendance = await (from a in _context.empAttendances
											   join p in _context.employeePunchCardInfos on a.punchCardNo equals p.punchCardNo
											   join e in _context.employeeInfos on p.employeeCode equals e.employeeCode
											   where Convert.ToDateTime(a.workDate).Month == month && Convert.ToDateTime(a.workDate).Year == year && e.employeeCode == code && e.departmentId == department && e.designation == designation
											   select a).AsNoTracking().FirstOrDefaultAsync()
					});
				}
			}
			else if (department == 0 && designation != "0" && shift != 0)
			{
				foreach (var item in dates)
				{
					data.Add(new EmpAttendanceWithDates
					{
						date = item,
						empAttendance = await (from a in _context.empAttendances
											   join p in _context.employeePunchCardInfos on a.punchCardNo equals p.punchCardNo
											   join e in _context.employeeInfos on p.employeeCode equals e.employeeCode
											   where Convert.ToDateTime(a.workDate).Month == month && Convert.ToDateTime(a.workDate).Year == year && e.employeeCode == code && e.designation == designation && e.shiftGroupId == shift
											   select a).AsNoTracking().FirstOrDefaultAsync()
					});
				}
			}
			else if (department != 0 && designation == "0" && shift != 0)
			{
				foreach (var item in dates)
				{
					data.Add(new EmpAttendanceWithDates
					{
						date = item,
						empAttendance = await (from a in _context.empAttendances
											   join p in _context.employeePunchCardInfos on a.punchCardNo equals p.punchCardNo
											   join e in _context.employeeInfos on p.employeeCode equals e.employeeCode
											   where Convert.ToDateTime(a.workDate).Month == month && Convert.ToDateTime(a.workDate).Year == year && e.employeeCode == code && e.departmentId == department && e.shiftGroupId == shift
											   select a).AsNoTracking().FirstOrDefaultAsync()
					});
				}
			}
			else if (department != 0 && designation != "0" && shift != 0)
			{
				foreach (var item in dates)
				{
					data.Add(new EmpAttendanceWithDates
					{
						date = item,
						empAttendance = await (from a in _context.empAttendances
											   join p in _context.employeePunchCardInfos on a.punchCardNo equals p.punchCardNo
											   join e in _context.employeeInfos on p.employeeCode equals e.employeeCode
											   where Convert.ToDateTime(a.workDate).Month == month && Convert.ToDateTime(a.workDate).Year == year && e.employeeCode == code && e.departmentId == department && e.designation == designation && e.shiftGroupId == shift
											   select a).AsNoTracking().FirstOrDefaultAsync()
					});
				}
			}
			else
			{
				foreach (var item in dates)
				{
					data.Add(new EmpAttendanceWithDates
					{
						date = item,
						empAttendance = await (from a in _context.empAttendances
											   join p in _context.employeePunchCardInfos on a.punchCardNo equals p.punchCardNo
											   join e in _context.employeeInfos on p.employeeCode equals e.employeeCode
											   where Convert.ToDateTime(a.workDate).Month == month && Convert.ToDateTime(a.workDate).Year == year && e.employeeCode == code
											   select a).AsNoTracking().FirstOrDefaultAsync()
						//empAttendance = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Date == Convert.ToDateTime(item).Date && x.punchCardNo == punchCard.punchCardNo).AsNoTracking().FirstOrDefaultAsync()
					});
				}
			}

			return data;
		}
		//public async Task<IEnumerable<DailyAttendenceViewModel>> GetAttendenceBetweenDates(DateTime? fromDate, DateTime? toDate)
		//{
		//	var data = await (from a in _context.empAttendances
		//						join p in _context.employeePunchCardInfos
		//						on a.punchCardNo equals p.punchCardNo
		//						join e in _context.employeeInfos
		//						on p.employeeCode equals e.employeeCode
		//						where Convert.ToDateTime(a.workDate).Date >= Convert.ToDateTime(fromDate).Date && Convert.ToDateTime(a.workDate).Date <= Convert.ToDateTime(toDate)
		//						select new DailyAttendenceViewModel
		//						{
		//							EmpId = e.Id,
		//							EmpCode = e.employeeCode,
		//							EmpName = e.nameEnglish,
		//							InTime = a.startTime,
		//							OutTime = a.endTime,
		//							Status = "P",
		//							Remarks = "Demo Remarks"
		//						}).ToListAsync();
		//	return data;
		//}
		//public async Task<IEnumerable<DailyAttendenceViewModel>> GetAttendenceBetweenDates(DateTime? fromDate, DateTime? toDate)
		//{
		//	var data = await (from a in _context.empAttendances
		//						join p in _context.employeePunchCardInfos
		//						on a.punchCardNo equals p.punchCardNo
		//						join e in _context.employeeInfos
		//						on p.employeeCode equals e.employeeCode
		//						where Convert.ToDateTime(a.workDate).Date >= Convert.ToDateTime(fromDate).Date && Convert.ToDateTime(a.workDate).Date <= Convert.ToDateTime(toDate)
		//						select new DailyAttendenceViewModel
		//						{
		//							EmpId = e.Id,
		//							EmpCode = e.employeeCode,
		//							EmpName = e.nameEnglish,
		//							InTime = a.startTime,
		//							OutTime = a.endTime,
		//							Status = "P",
		//							Remarks = "Demo Remarks"
		//						}).ToListAsync();
		//	return data;
		//}

		public async Task<bool> SaveUploadEmployeeAttendence(AttendenceApi att)
		{
			if (att.Id != 0)
				_context.AttendenceApi.Update(att);
			else
				_context.AttendenceApi.Add(att);
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<string> GetBranchNameBanglaByType(string type, int typeId)
		{
			var data = "";
			if (type == "branch")
			{
				data = await _context.hrBranches.Where(x => x.Id == typeId).Select(x => x.branchNameBn).FirstOrDefaultAsync();
			}
			else if (type == "department")
			{
				data = await _context.departments.Where(x => x.Id == typeId).Select(x => x.deptNameBn).FirstOrDefaultAsync();
			}
			else if (type == "zone")
			{
				data = await _context.Locations.Where(x => x.Id == typeId).Select(x => x.branchUnitNameBN).FirstOrDefaultAsync();
			}
			else if (type == "office")
			{
				data = await _context.FunctionInfos.Where(x => x.Id == typeId).Select(x => x.branchUnitNameBN).FirstOrDefaultAsync();
			}
			else
			{
				data = "";
			}
			return data;
		}

		public async Task<IEnumerable<EmpAttendance>> ProcessAttendance(string Fdate, string Tdate)
		{
			var fromD = Fdate.Split('/');
			var toD = Tdate.Split('/');
			var fromDate = new DateTime(Convert.ToInt32(fromD[2]), Convert.ToInt32(fromD[1]), Convert.ToInt32(fromD[0])).Date;
			var toDate = new DateTime(Convert.ToInt32(toD[2]), Convert.ToInt32(toD[1]), Convert.ToInt32(toD[0])).Date;
			_context.Database.ExecuteSqlCommand($"Dailyattendence_New {Fdate},{Tdate}, {2}");
			//_context.Database.ExecuteSqlCommand($"Dailyattendence_New @p0, @p1, @p2", parameters: new[] { Fdate, Tdate, 2 });
			//var data = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Date >= Convert.ToDateTime(Fdate).Date && Convert.ToDateTime(x.workDate).Date <= Convert.ToDateTime(Tdate).Date).AsNoTracking().ToListAsync();
			var data = await _context.empAttendances.Where(x =>Convert.ToDateTime(x.workDate).Date >= fromDate && Convert.ToDateTime(x.workDate).Date <= toDate && x.summaryId != 2).AsNoTracking().ToListAsync();

			return data;
		}

        //Asad added on 05.08.2023
        public async Task<IEnumerable<LunchSubsidyViewModel>> GetLunchSubsidy(string type, int typeId, int year, int month )
        {
           //var data = await _context.LunchSubsidyViewModel.Where(x => Convert.ToDateTime(x.workDate).Date >= fromDate && Convert.ToDateTime(x.workDate).Date <= toDate && x.summaryId != 2).AsNoTracking().ToListAsync();

            var Data = await _context.LunchSubsidyViewModel.FromSql($"sp_GetLunchSubsidary @p0,@p1,@p2,@p3", type, typeId, year, month).ToListAsync();
            return Data;

        }

        public async Task<int> GetTotalPresentByMonthYearAndEmpCode(int month, int year, string code)
		{
			var data = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Year == year && Convert.ToDateTime(x.workDate).Month == month && x.punchCardNo == code).Select(x => x.Id).CountAsync();
			return data;
		}

		public async Task<IEnumerable<EmployeeInfo>> GetEmployeesByType(string type, int typeId)
		{
			var data = new List<EmployeeInfo>();
			if (type == "department")
			{
				data = await _context.employeeInfos.Include(x => x.lastDesignation).AsNoTracking().OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.departmentId == typeId && x.lastDesignation != null && x.activityStatus==1).ToListAsync();
			}
			if (type == "branch")
			{
				data = await _context.employeeInfos.Include(x => x.lastDesignation).AsNoTracking().OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.hrBranchId == typeId && x.lastDesignation != null && x.activityStatus == 1).ToListAsync();
			}
			if (type == "zone")
			{
				data = await _context.employeeInfos.Include(x => x.lastDesignation).AsNoTracking().OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.locationId == typeId && x.lastDesignation != null && x.activityStatus == 1).ToListAsync();
			}
			if (type == "office")
			{
				data = await _context.employeeInfos.Include(x => x.lastDesignation).AsNoTracking().OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.functionInfoId == typeId && x.lastDesignation != null && x.activityStatus == 1).ToListAsync();
			}
			return data;
		}
        public async Task<IEnumerable<EmployeeInfo>> GetEmployeesByTypeNew(string type, int typeId,int month, int year)
        {
            var data = new List<EmployeeInfo>();
            if (type == "department")
            {
                data = await _context.employeeInfos.Include(x => x.lastDesignation).AsNoTracking().OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.departmentId == typeId && x.lastDesignation != null && x.activityStatus == 1).ToListAsync();
                var trEmp = await _context.employeePostings.Where(x => Convert.ToDateTime(x.endDate).Month == month && Convert.ToDateTime(x.endDate).Year == year && x.employee.activityStatus == 1 && x.departmentId == typeId).Include(x => x.employee).Include(x => x.employee.lastDesignation).ToListAsync();
                data.AddRange(trEmp.Select(x => x.employee));
            }
            if (type == "branch")
            {
                data = await _context.employeeInfos.Include(x => x.lastDesignation).AsNoTracking().OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.hrBranchId == typeId && x.lastDesignation != null && x.activityStatus == 1).ToListAsync();
				var trEmp = await _context.employeePostings.Where(x => Convert.ToDateTime(x.endDate).Month == month && Convert.ToDateTime(x.endDate).Year == year && x.employee.activityStatus==1 && x.hrBranchId== typeId).Include(x=>x.employee).Include(x => x.employee.lastDesignation).ToListAsync();
				//New
				data.AddRange(trEmp.Where(x=> x.hrBranchId == typeId).Select(x => x.employee));
				//Old
				//data.AddRange(trEmp.Select(x => x.employee));
			}
            if (type == "zone")
            {
                data = await _context.employeeInfos.Include(x => x.lastDesignation).AsNoTracking().OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.locationId == typeId && x.lastDesignation != null && x.activityStatus == 1).ToListAsync();
                var trEmp = await _context.employeePostings.Where(x => Convert.ToDateTime(x.endDate).Month == month && Convert.ToDateTime(x.endDate).Year == year && x.employee.activityStatus == 1 && x.locationId == typeId).Include(x => x.employee).Include(x => x.employee.lastDesignation).ToListAsync();
                data.AddRange(trEmp.Select(x => x.employee));
            }
            if (type == "office")
            {
                data = await _context.employeeInfos.Include(x => x.lastDesignation).AsNoTracking().OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.functionInfoId == typeId && x.lastDesignation != null && x.activityStatus == 1).ToListAsync();
                
            }
            return data;
        }

        public async Task<LunchSubsidyReportViewModel> GetEmployeesByTypeAndTypeId(string type, int typeID)
        {
            var Data = await _context.lunchSubsidyReportViewModels.FromSql($"sp_GetDeptHeadNameByEmpId @p0,@p1", type, typeID).FirstOrDefaultAsync();
            return Data;
        }

        #region monthlyDateWise
        public async Task<int> GetTotalPresentByDateAndEmpCode(DateTime? startDate, DateTime? endDate, string code)
		{
			var data = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Date >= Convert.ToDateTime(startDate).Date && Convert.ToDateTime(x.workDate).Date <= Convert.ToDateTime(endDate).Date && x.punchCardNo == code).Select(x => x.Id).CountAsync();
			return data;
		}
		public async Task<int> GetTotalAbsentByDateAndEmpCode(DateTime? startDate, DateTime? endDate, string code)
		{
			var empId = await _context.employeeInfos.Where(x => x.employeeCode == code).AsNoTracking().Select(x => x.Id).FirstOrDefaultAsync();
			var data = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Date >= Convert.ToDateTime(startDate).Date && Convert.ToDateTime(x.workDate).Date <= Convert.ToDateTime(endDate).Date && x.punchCardNo == code).Select(x => x.Id).CountAsync();
			var leave = await _context.leaveRegisters.Where(x => x.leaveStatus == 3 && Convert.ToDateTime(x.leaveFrom).Date >= Convert.ToDateTime(startDate).Date && Convert.ToDateTime(x.leaveTo).Date <= Convert.ToDateTime(endDate).Date && x.employeeId == empId).Select(x => x.Id).CountAsync();
			var holiday = await _context.holidays.Where(x => Convert.ToDateTime(x.weeklyHoliday).Date >= Convert.ToDateTime(startDate).Date && Convert.ToDateTime(x.weeklyHoliday).Date <= Convert.ToDateTime(endDate).Date).Select(x => x.Id).CountAsync();

			var absent = (Convert.ToDateTime(endDate) - Convert.ToDateTime(startDate)).Days + 1 - data - leave - holiday;
			return absent;
		}

		public async Task<int> GetTotalLeaveByDateAndEmpCode(DateTime? startDate, DateTime? endDate, string code)
		{
			var employee = await _context.employeeInfos.Where(x => x.employeeCode == code).FirstOrDefaultAsync();
			var data = await _context.leaveRegisters.Where(x => x.leaveStatus == 3 && Convert.ToDateTime(x.leaveFrom).Date >= Convert.ToDateTime(startDate).Date && Convert.ToDateTime(x.leaveTo).Date <= Convert.ToDateTime(endDate).Date && x.employeeId == employee.Id).Select(x => x.Id).CountAsync();
			return data;
		}

		public async Task<int> GetTotalHolidayByDateAndEmpCode(DateTime? startDate, DateTime? endDate)
		{
			var data = await _context.holidays.Where(x => Convert.ToDateTime(x.weeklyHoliday).Date >= Convert.ToDateTime(startDate).Date && Convert.ToDateTime(x.weeklyHoliday).Date <= Convert.ToDateTime(endDate).Date).Select(x => x.weeklyHoliday).Distinct().CountAsync();
			return data;
		}

		public async Task<int> GetTotalWorkByDateAndEmpCode(DateTime? startDate, DateTime? endDate, string code)
		{
			var holiday = await _context.holidays.Where(x => Convert.ToDateTime(x.weeklyHoliday).Date >= Convert.ToDateTime(startDate).Date && Convert.ToDateTime(x.weeklyHoliday).Date <= Convert.ToDateTime(endDate).Date).Select(x => x.Id).CountAsync();
			return (Convert.ToDateTime(endDate) - Convert.ToDateTime(startDate)).Days+1 - holiday;
		}

		public async Task<decimal> GetAverageByDateAndEmpCode(DateTime? startDate, DateTime? endDate, string code)
		{
			var holiday = await _context.holidays.Where(x => Convert.ToDateTime(x.weeklyHoliday).Date >= Convert.ToDateTime(startDate).Date && Convert.ToDateTime(x.weeklyHoliday).Date <= Convert.ToDateTime(endDate).Date).Select(x => x.Id).CountAsync();
			var present = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Date >= Convert.ToDateTime(startDate).Date && Convert.ToDateTime(x.workDate).Date <= Convert.ToDateTime(endDate).Date && x.punchCardNo == code).Select(x => x.Id).CountAsync();
			var average = (Convert.ToDecimal(present) / (((Convert.ToDateTime(endDate).Date - Convert.ToDateTime(startDate).Date).Days + 1) - holiday)) * 100;
			return Math.Round(average, 2);
		}

		#endregion



		#region MonthYearWise
		public async Task<int> GetTotalAbsentByMonthYearAndEmpCode(int month, int year, string code)
		{
			var empId = await _context.employeeInfos.Where(x => x.employeeCode == code).AsNoTracking().Select(x => x.Id).FirstOrDefaultAsync();
			var data = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Year == year && Convert.ToDateTime(x.workDate).Month == month && x.punchCardNo == code).Select(x => x.Id).CountAsync();
			var leave = await _context.leaveRegisters.Where(x => x.leaveStatus == 3 && Convert.ToDateTime(x.leaveFrom).Year == year && Convert.ToDateTime(x.leaveFrom).Month == month && Convert.ToDateTime(x.leaveTo).Year == year && Convert.ToDateTime(x.leaveTo).Month == month && x.employeeId == empId).Select(x => x.Id).CountAsync();
			var holiday = await _context.holidays.Where(x => x.year == year && Convert.ToDateTime(x.weeklyHoliday).Month == month).Select(x => x.Id).CountAsync();

			var absent = DateTime.DaysInMonth(year, month) - data - leave - holiday;
			return absent;
		}

		//public async Task<int> GetTotalAbsentByDateAndEmpCode(DateTime Fdate, DateTime Tdate, string code)
		//{
		//    var data = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Date >= Fdate && Convert.ToDateTime(x.workDate).Date <= Tdate && x.punchCardNo == code).Select(x => x.Id).CountAsync();
		//    var absent = DateTime.DaysInMonth(year, month) - data;
		//    return absent;
		//}
		public async Task<int> GetTotalLeaveByMonthYearAndEmpCode(int month, int year, string code)
		{
			var employee = await _context.employeeInfos.Where(x => x.employeeCode == code).FirstOrDefaultAsync();
			var data = await _context.leaveRegisters.Where(x => x.leaveStatus == 3 && Convert.ToDateTime(x.leaveFrom).Year == year && Convert.ToDateTime(x.leaveFrom).Month == month && Convert.ToDateTime(x.leaveTo).Year == year && Convert.ToDateTime(x.leaveTo).Month == month && x.employeeId == employee.Id).Select(x => x.Id).CountAsync();
			return data;
		}





		public async Task<int> GetTotalHolidayByMonthYearAndEmpCode(int month, int year)
		{
			var data = await _context.holidays.Where(x => x.year == year && Convert.ToDateTime(x.weeklyHoliday).Month == month).Select(x => x.Id).CountAsync();
			return data;
		}
		public async Task<int> GetTotalWorkByMonthYearAndEmpCode(int month, int year, string code)
		{
			//var employeeId = await _context.employeeInfos.Where(x => x.employeeCode == code).Select(x => x.Id).FirstOrDefaultAsync();
			//var leave = await _context.leaveRegisters.Where(x => x.leaveStatus == 3 && Convert.ToDateTime(x.leaveFrom).Year == year && Convert.ToDateTime(x.leaveFrom).Month == month && Convert.ToDateTime(x.leaveTo).Year == year && Convert.ToDateTime(x.leaveTo).Month == month && x.employeeId == employeeId).Select(x => x.Id).CountAsync();

			var holiday = await _context.holidays.Where(x => x.year == year && Convert.ToDateTime(x.weeklyHoliday).Month == month).Select(x => x.Id).CountAsync();

			//var data = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Year == year && Convert.ToDateTime(x.workDate).Month == month && x.punchCardNo == code).Select(x => x.Id).CountAsync();
			//var work = DateTime.DaysInMonth(year, month) - data - (DateTime.DaysInMonth(year, month) - data) - holiday - leave;
			return DateTime.DaysInMonth(year, month) - holiday;
		}

		public async Task<decimal> GetAverageByMonthYearAndEmpCode(int month, int year, string code)
		{
			var holiday = await _context.holidays.Where(x => x.year == year && Convert.ToDateTime(x.weeklyHoliday).Month == month).Select(x => x.Id).CountAsync();
			var present = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Year == year && Convert.ToDateTime(x.workDate).Month == month && x.punchCardNo == code).Select(x => x.Id).CountAsync();
			var average = (Convert.ToDecimal(present) / (DateTime.DaysInMonth(year, month) - holiday)) * 100;
			return Math.Round(average, 2);
		}

        //public Task<int> GetDailyTotalLateByMonth(DateTime? start)
        //{
        //	throw new NotImplementedException();
        //}

        public async Task<int> GetTotalLeaveByMonthAndEmpCode(string code, int month)
        {
            var employee = await _context.employeeInfos.Where(x => x.employeeCode == code).FirstOrDefaultAsync();
            var data = await _context.leaveRegisters.Where(x => x.leaveStatus == 3 && (Convert.ToDateTime(x.leaveFrom) >= Convert.ToDateTime(new DateTime(DateTime.Now.Year, month, 1)) && Convert.ToDateTime(x.leaveTo) <= Convert.ToDateTime(new DateTime(DateTime.Now.Year, month, DateTime.DaysInMonth(DateTime.Now.Year,month))) )&& x.employeeId == employee.Id).Select(x => x.Id).CountAsync();
            return data;
        }


        public async Task<int> GetTotalLeaveByMonthAndEmpCodeInd(string code, int month,int year)
        {
            var employee = await _context.employeeInfos.Where(x => x.employeeCode == code).FirstOrDefaultAsync();
            var data = await _context.leaveRegisters.Where(x => x.leaveStatus == 3 && (Convert.ToDateTime(x.leaveFrom) >= Convert.ToDateTime(new DateTime(year, month, 1)) && Convert.ToDateTime(x.leaveTo) <= Convert.ToDateTime(new DateTime(year, month, DateTime.DaysInMonth(year, month)))) && x.employeeId == employee.Id).Select(x => x.Id).CountAsync();
            return data;
        }
        #endregion



        public async Task<IEnumerable<AllEmployeeAttendanceViewModel>> GetEmployeeAttendanceBySp(string fromDate, string toDate)
        {
            var attendanceData= await _context.allEmployeeAttendanceViewModels.FromSql($"sp_GetMonthlyAttendance @p0,@p1", fromDate, toDate).ToListAsync();
            return attendanceData;


        }

		public async Task<IEnumerable<EmpManualAttBySpVM>> GetAllManualAttendanceBySp()
		{
			var data = await _context.empManualAttBySpVMs.FromSql($"GetManualAttendanceBySP").ToListAsync();
			return data;
		}
		public async Task<IEnumerable<ManualAttendanceApproval>> GetManualAttendanceForApproval(string user)
		{
			var data = await _context.manualAttendanceApprovals.FromSql($"sp_GetAttendanceForApproval {user}").ToListAsync();
			return data;
		}
		public async Task<IEnumerable<ManualAttendanceApproval>> GetApprovedAttendanceForApproval(string user)
		{
			var data = await _context.manualAttendanceApprovals.FromSql($"sp_GetApprovedAttedanceForApproval {user}").ToListAsync();
			return data;
		}
		public async Task<IEnumerable<ManualAttendanceApproval>> GetRejectedAttendanceForApproval(string user)
		{
			var data = await _context.manualAttendanceApprovals.FromSql($"sp_GetRejectedAttedanceForApproval {user}").ToListAsync();
			return data;
		}
		public async Task<IEnumerable<ManualAttendanceApproval>> GetMyPendingManualAttendance(string empCode)
        {
            var data = await (from a in _context.empManualAttendances
                              join e in _context.employeeInfos on a.punchCardNo equals e.employeeCode
                              join d in _context.designations on e.lastDesignationId equals d.Id
                              where a.approveStatus == 0 && a.punchCardNo == empCode
                              select new ManualAttendanceApproval
                              {
                                  Id = a.Id,
                                  workDate = a.workDate,
                                  employeeCode = e.employeeCode,
                                  designationName = d.designationName,
                                  nameEnglish = e.nameEnglish,
                                  startTime = a.startTime,
                                  endTime = a.endTime,
                                  summaryId = a.summaryId,                                
                              }).ToListAsync();

            return data;
        }
		public async Task<IEnumerable<ManualAttendanceViewModel>> GetManualAttendanceByAnyKey(string employeeCode, int? approverId, string fromDate, string toDate)
		{
			return await _context.ManualAttendanceViewModels.FromSql($"SP_GetAttendanceByAnyKey {employeeCode},{approverId}, {fromDate}, {toDate}").AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<ManualAttendanceApprovalApi>> GetMyPendingManualAttendanceAPI(string empCode)
		{
			var data = await (from a in _context.empManualAttendances
							  join e in _context.employeeInfos on a.punchCardNo equals e.employeeCode
							  join d in _context.designations on e.lastDesignationId equals d.Id
							  where a.approveStatus == 0 && a.punchCardNo == empCode
							  select new ManualAttendanceApprovalApi
							  {
								  Id = a.Id,
								  workDate = a.workDate,
								  employeeCode = e.employeeCode,
								  designationName = d.designationName,
								  nameEnglish = e.nameEnglish,
								  startTime = a.startTime,
								  endTime = a.endTime,
								  summaryId = a.summaryId,
								  location = a.location,
								  lat = a.lat,
								  lon = a.lon,
								  reason=a.notes,
								  Remarks = a.remarks,
								  AttendanceTypeId = a.AttendanceTypeId
							  }).ToListAsync();

			return data;
		}
		public async Task<IEnumerable<ManualAttendanceApproval>> GetMyApprovedManualAttendance(string empCode)
        {
            var data = await (from a in _context.empManualAttendances
                              join e in _context.employeeInfos on a.punchCardNo equals e.employeeCode
                              join d in _context.designations on e.lastDesignationId equals d.Id
                              where a.approveStatus == 1 && a.punchCardNo == empCode
                              select new ManualAttendanceApproval
                              {
                                  Id = a.Id,
                                  workDate = a.workDate,
                                  employeeCode = e.employeeCode,
                                  designationName = d.designationName,
                                  nameEnglish = e.nameEnglish,
                                  startTime = a.startTime,
                                  endTime = a.endTime,
                                  summaryId = a.summaryId,
                                  reason = a.notes,
                                
                              }).ToListAsync();

            return data;
        }
		public async Task<IEnumerable<ManualAttendanceApprovalApi>> GetMyApprovedManualAttendanceAPI(string empCode)
		{
			var data = await (from a in _context.empManualAttendances
							  join e in _context.employeeInfos on a.punchCardNo equals e.employeeCode
							  join d in _context.designations on e.lastDesignationId equals d.Id
							  where a.approveStatus == 1 && a.punchCardNo == empCode
							  select new ManualAttendanceApprovalApi
							  {
								  Id = a.Id,
								  workDate = a.workDate,
								  employeeCode = e.employeeCode,
								  designationName = d.designationName,
								  nameEnglish = e.nameEnglish,
								  startTime = a.startTime,
								  endTime = a.endTime,
								  summaryId = a.summaryId,
								  reason = a.notes,
								  location = a.location,
								  lat = a.lat,
								  lon = a.lon,
								  Remarks = a.remarks,
								  AttendanceTypeId = a.AttendanceTypeId


							  }).ToListAsync();

			return data;
		}

		public async Task<IEnumerable<ManualAttendanceApproval>> GetMyRejectedManualAttendance(string empCode)
        {
            var data = await (from a in _context.empManualAttendances
                              join e in _context.employeeInfos on a.punchCardNo equals e.employeeCode
                              join d in _context.designations on e.lastDesignationId equals d.Id
                              where a.approveStatus == 2 && a.punchCardNo == empCode
                              select new ManualAttendanceApproval
                              {
                                  Id = a.Id,
                                  workDate = a.workDate,
                                  employeeCode = e.employeeCode,
                                  designationName = d.designationName,
                                  nameEnglish = e.nameEnglish,
                                  startTime = a.startTime,
                                  endTime = a.endTime,
                                  summaryId = a.summaryId                                
                              }).ToListAsync();

            return data;
        }
		public async Task<IEnumerable<ManualAttendanceApprovalApi>> GetMyRejectedManualAttendanceAPI(string empCode)
        {
            var data = await (from a in _context.empManualAttendances
                              join e in _context.employeeInfos on a.punchCardNo equals e.employeeCode
                              join d in _context.designations on e.lastDesignationId equals d.Id
                              where a.approveStatus == 2 && a.punchCardNo == empCode
                              select new ManualAttendanceApprovalApi
							  {
                                  Id = a.Id,
                                  workDate = a.workDate,
                                  employeeCode = e.employeeCode,
                                  designationName = d.designationName,
                                  nameEnglish = e.nameEnglish,
                                  startTime = a.startTime,
                                  endTime = a.endTime,
                                  summaryId = a.summaryId ,
								  location = a.location,
								  lat = a.lat,
								  lon = a.lon,
								  reason = a.notes,
								  Remarks = a.remarks,
								  AttendanceTypeId = a.AttendanceTypeId
							  }).ToListAsync();

            return data;
        }

        public async Task<string> ApproveManualAttendance(string empCode, int id, string approverEmpCode)
        {
            var data = await _context.empManualAttendances.AsNoTracking().Where(x => x.punchCardNo == empCode && x.Id == id).FirstOrDefaultAsync();
            data.approveStatus = 1;
            data.summaryId = 9;
            data.attenUpdateAppBy = approverEmpCode;
            _context.empManualAttendances.Update(data);
            var result = await _context.SaveChangesAsync();
            if (result == 1)
            {
                return "Approved";
            }
            else
            {
                return "Failed";
            }
        }		
		public async Task<string> RejectManualAttendance(string empCode, int id, string rejectedByEmpCode, string reasonforreject)
        {
            var data = await _context.empManualAttendances.AsNoTracking().Where(x => x.punchCardNo == empCode && x.Id == id).FirstOrDefaultAsync();
            data.approveStatus = 2;
            data.summaryId = 10;
            data.attenUpdateBy = rejectedByEmpCode;
            data.reasonforrejecting = reasonforreject;
            _context.empManualAttendances.Update(data);
            var result = await _context.SaveChangesAsync();
            if (result == 1)
            {
                return "Rejected , Reason of rejection:  "+ reasonforreject;
            }
            else
            {
                return "Failed";
            }
        }

        public async Task<IEnumerable<OverTimeReportViewModel>> GetOverTimeReport(DateTime fdate, DateTime todate, int depId, int branchId, int divisionId, int zoneId, int officeId,int employeeInfoId)
        {
            var data = await _context.OverTimeReportViewModels.FromSql($"SP_OverTimeForStaffs @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7",fdate,todate,depId,branchId,divisionId,zoneId,officeId, employeeInfoId).ToListAsync();



            return data;
            
        }	
		

		public async Task<int> SaveSMSResponseLog(SMSResponseLog log)
		{
			if (log.Id != 0)
			{
				_context.SmsResponses.Update(log);
			}
			else
			{
				_context.SmsResponses.Add(log);
			}
			return await _context.SaveChangesAsync();
		}
	}
}