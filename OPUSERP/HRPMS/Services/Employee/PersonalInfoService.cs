using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OPUSERP.Areas.Auth.Models;
using OPUSERP.Areas.HRPMSACR.Models;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.Payroll.Models;
using OPUSERP.Data;
using OPUSERP.Data.Entity;
using OPUSERP.Data.Entity.Auth;
using OPUSERP.Data.Entity.Matrix;
using OPUSERP.HRPMS.Data.Entity.ACR;
using OPUSERP.HRPMS.Data.Entity.Attendance;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.IELTS;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.Payroll.Data.Entity.Fixation;
using OPUSERP.Payroll.Data.Entity.Salary;
using OPUSERP.ProvidentFund.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;


namespace OPUSERP.HRPMS.Services.Employee
{
	public class PersonalInfoService : IPersonalInfoService
	{
		private readonly ERPDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public PersonalInfoService(ERPDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}


		//EmployeeInfo

		public async Task<IEnumerable<PeerSearchViewModel>> GetEmployeeInfoBySearch(string searchKey)
		{
			return await _context.peerSearchViewModels.FromSql($"GetQuickSearchEmpInfo {searchKey}").AsNoTracking().ToListAsync();
		}
		public async Task<bool> DeleteEmployeeInfoById(int id)
		{
			_context.employeeInfos.Remove(_context.employeeInfos.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<int> DeleteEmployeeById(int id)
		{
			var emp = _context.employeeInfos.Find(id);
			emp.activityStatus = 10;
			_context.employeeInfos.Update(emp);
			return await _context.SaveChangesAsync();
		}
		public async Task<IEnumerable<EmployeeInfo>> GetEmployeeInfos()
		{
			_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
			return await _context.employeeInfos.Include(x => x.department).Include(x => x.lastDesignation).ToListAsync();
		}
		public async Task<IEnumerable<EmployeeInfo>> GetEmployeeInfo()
		{
			_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
			return await _context.employeeInfos.Where(x => x.salaryStatus == 1 && x.activityStatus == 1).Include(x => x.department).Include(x => x.lastDesignation).ToListAsync();
		}
		public async Task<IEnumerable<EmployeeInfo>> GetAllSalaryActiveEmployees()
		{
			var data = await _context.employeeInfos.Where(x => x.salaryStatus == 1).AsNoTracking().ToListAsync();
			return data;
		}
		public async Task<IEnumerable<FixationIntegration>> GetAllFixationEmployee(string type, int year)
		{
			var data = await _context.fixationIntegrations.Where(x => x.fixationType == type && Convert.ToDateTime(x.effectiveDate).Year == year).Include(x => x.employee).AsNoTracking().ToListAsync();
			return data;
		}

		//Asad Added on 08.08.2023
		public async Task<IEnumerable<EmployeeInfo>> GetSalaryPaidEmployeeByEmployeeId(int periodId, int? employeeId)
		{
			var data = await _context.ProcessEmpSalaryStructures.Where(x => x.salaryPeriodId == periodId && x.employeeInfoId == employeeId && !string.IsNullOrWhiteSpace(x.employeeInfo.emailAddress)).Select(x => x.employeeInfo).Distinct().ToListAsync();
			return data;
		}

		//Asad Added on 08.08.2023
		public async Task<IEnumerable<EmployeeInfo>> GetSalaryPaidEmployeeByDegignationId(int periodId, int? designationId)
		{
			//from s in studentList // outer sequence
			//join st in standardList //inner sequence 
			//on s.StandardID equals st.StandardID // key selector 
			//select new
			//{ // result selector 
			//	StudentName = s.StudentName,
			//	StandardName = st.StandardName
			//};

			//var data = from s in _context.ProcessEmpSalaryStructures
			//		   join e in _context.employeeInfos
			//		   on s.employeeInfoId equals e.Id
			//		   where
			//			  s.salaryPeriodId == periodId
			//		   && e.departmentId == designationId
			//		   && e.emailAddress != null
			//		   select e;


			var data = await _context.ProcessEmpSalaryStructures.Where(x => x.salaryPeriodId == periodId && x.designationId == designationId && !string.IsNullOrWhiteSpace(x.employeeInfo.emailAddress)).Select(x => x.employeeInfo).Distinct().ToListAsync();

			//var data = await _context.ProcessEmpSalaryStructures.Where(x => x.salaryPeriodId == periodId && x.designationId == designationId && !string.IsNullOrWhiteSpace(x.employeeInfo.emailAddress)).Select(x => x.employeeInfo).Distinct().ToListAsync();

			return data;
		}

		//Asad Added on 08.08.2023
		public async Task<IEnumerable<EmployeeInfo>> GetSalaryPaidEmployeeByBranchId(int periodId, int? branchId)
		{
			var data = await _context.ProcessEmpSalaryStructures.Where(x => x.salaryPeriodId == periodId && x.hrBranchId == branchId && !string.IsNullOrWhiteSpace(x.employeeInfo.emailAddress)).Select(x => x.employeeInfo).Distinct().ToListAsync();
			return data;
		}
				
		public async Task<IEnumerable<EmployeeInfo>> GetAllSalaryActiveEmployeesByPeriodId(int periodId)
		{
			var data = await _context.ProcessEmpSalaryStructures.Where(x => x.salaryPeriodId == periodId).Select(x => x.employeeInfo).Distinct().ToListAsync();
			return data;
		}
		public async Task<IEnumerable<EmployeeInfo>> GetAllSalaryActiveEmployeesByPeriodAndDesig(int periodId, int desig)
		{
			var sent = await _context.SendEmailLogStatus.Where(x => x.isDelete == periodId && x.type != "Pending").Select(x => x.employeeId).ToListAsync();
			var data = await _context.ProcessEmpSalaryStructures.Where(x => x.salaryPeriodId == periodId && x.designationId == desig && x.employeeInfo.emailAddress != null && !sent.Contains(x.employeeInfoId)).Select(x => x.employeeInfo).Distinct().ToListAsync();
			return data;
		}
		public async Task<IEnumerable<EmployeeInfo>> GetEmployeeInfoLoan()
		{
			_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
			return await _context.employeeInfos.Where(x => x.isDelete == 0).Include(x => x.department).Include(x => x.lastDesignation).Include(x => x.currentGrade).Include(x => x.joiningGrade).ToListAsync();
		}
		public async Task<IEnumerable<EmployeeInfo>> GetAllEmployessForLoan()
		{
            var loanpolicy = await _context.loanPolicyNews.Where(x => x.loanPolicyName == "MCAR").FirstOrDefaultAsync();
            var designations = await _context.loanPolicyDetails.Where(x => x.loanPolicyId == loanpolicy.Id).Select(x => x.designationId).ToListAsync();
			_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
			return await _context.employeeInfos.Where(x => x.isDelete == 0 && designations.Contains(x.lastDesignationId) && (DateTime.Now.Date - Convert.ToDateTime(x.joiningDateGovtService).Date).TotalDays > loanpolicy.jobDuration * 365).Include(x => x.department).Include(x => x.lastDesignation).Include(x => x.currentGrade).Include(x => x.joiningGrade).ToListAsync();
		}
        
		public async Task<IEnumerable<LoanPolicyDetail>> GetDesignationsByLoanPolicyId(int? id)
		{
            return await _context.loanPolicyDetails.Include(x => x.designation).Where(x => x.loanPolicyId == id).ToListAsync();
		}

		public async Task<IEnumerable<EmployeeInfo>> GetEmployeeInfoAI()
		{
			_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
			return await _context.employeeInfos.Include(x => x.department).ToListAsync();
		}
		public async Task<IEnumerable<StatusLog>> GetStatusLogsByReqMasterId(int masterid)
		{
			var data = await _context.StatusLogs.Include(x => x.requisition).Include(x => x.requisition.project).Include(x => x.cSMaster).Include(x => x.purchase).Include(x => x.matrixType).Include(x => x.iOUPaymentMaster).Where(x => x.requisitionId == masterid).ToListAsync();
			return data;
		}
		public async Task<IEnumerable<EmployeeInfo>> GetUserEmployeeInfo()
		{
			_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
			return await _context.employeeInfos.Include(x => x.department).Include(x => x.lastDesignation).Where(x => x.ApplicationUserId != null).ToListAsync();
		}

		public async Task<IEnumerable<EmployeeInfo>> GetActiveAllEmployeeInfo()
		{
			_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
			return await _context.employeeInfos.Include(x => x.department).Include(x => x.branch).Include(x => x.hrBranch).Include(x => x.lastDesignation).Include(x => x.currentGrade).Where(a => (a.activityStatus == 1 || a.activityStatus == 3) && a.emailAddress != null).ToListAsync();
		}
		public async Task<IEnumerable<EmployeeInfo>> GetActiveAllEmployeeInfoDepWise(int? depId ,int? branchId ,int? divId ,int? unitId ,int? officeId ,int? zoneId , int empId)
		{
			_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
			return await _context.employeeInfos.Include(x => x.department).Include(x => x.branch).Include(x => x.hrBranch).Include(x => x.lastDesignation).Include(x => x.currentGrade).Where(a => ( a.departmentId == depId && a.hrBranchId == branchId && a.hrDivisionId == divId && a.hrUnitId == divId && a.functionInfoId == officeId && a.locationId == zoneId) && a.Id != empId).ToListAsync();
		}

		public async Task<IEnumerable<EmployeeFixatonAuto>> GetActiveAllEmployeeInfoForFixation()
		{
			var data  = await _context.employeeFixatonAutos.FromSql($"sp_GetActiveAllEmployeeInfoForFixation").ToListAsync();
			return data;
		}


        public async Task<IEnumerable<SP_GetEmpOverTime>> GetEmployeeInfoByDesignation()
        {
          
            return await _context.sP_GetEmpOverTimes.FromSql($"SP_GetEmpOverTime").ToListAsync();
        }
        public async Task<EmployeeSignature> GetEmployeeInfoByDesignation(string designation, int branchId, int deprtId)
        {
			var data = await (from e in _context.employeeInfos
							  join ds in _context.designations on e.lastDesignationId equals ds.Id
							  join pp in _context.employeePostings on e.Id equals pp.employeeId
							  join s in _context.employeeSignatures on e.Id equals s.employeeId
							  where ds.shortName==designation && e.branchId==(branchId==0?e.branchId: branchId) && e.departmentId == (deprtId == 0 ? e.departmentId : deprtId)
                              orderby pp.startDate descending
							  select s).OrderByDescending(x=>x).FirstOrDefaultAsync();
			return data;
        }

        //new: 04.10.2023
        public async Task<IEnumerable<EmployeePostingPlace>> GetEmployeePostingPlace(int? isActive)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            var data = await (from e in _context.employeeInfos
                              join p in _context.employeePostings
                              on e.Id equals p.employeeId
                              where
                                 p.status == 1 && p.endDate == null && p.type == 1
							  && (e.activityStatus == isActive || isActive == null)
                              select p)
							 .Include(x => x.employee)
							 .Include(x => x.employee.department)
							 .Include(x => x.employee.branch)
							 .Include(x => x.employee.currentGrade)
							 .Include(x => x.employee.joiningGrade)
							 .Include(x => x.employee.hrBranch)
							 .Include(x => x.employee.lastDesignation)
							 .Include(x => x.employee.employeeStatus)
							 .Include(x => x.hrBranch)
							 .Include(x => x.hrDivision)
							 .Include(x => x.hrUnit)
							 .Include(x => x.location)

                        .ToListAsync();
            return data;

        }



        public async Task<IEnumerable<EmployeeInfo>> GetActiveAllEmployeeInfo1()
		{
			_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
			return await _context.employeeInfos.Include(x => x.department).Include(x => x.branch).Include(x => x.currentGrade).Include(x => x.joiningGrade).Include(x => x.hrBranch).Include(x => x.lastDesignation).Include(x => x.employeeStatus).ToListAsync();
		}
        public async Task<IEnumerable<EmployeeInfo>> GetActiveEmployeeInfo(string code)
		{
            var user = await _userManager.FindByNameAsync(code);
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("HRAdmin") || roles.Contains("admin") || roles.Contains("UAT Admin") || roles.Contains("PayrollAdmin"))
            {
                var data = await _context.employeeInfos.Include(x => x.department).Include(x => x.branch).Include(x => x.currentGrade).Include(x => x.joiningGrade).Include(x => x.hrBranch).Include(x => x.lastDesignation).Include(x => x.employeeStatus).ToListAsync();
                return data;
            }
            else
            {
                var data = await _context.employeeInfos.Where(x => x.employeeCode == code).Include(x => x.department).Include(x => x.branch).Include(x => x.currentGrade).Include(x => x.joiningGrade).Include(x => x.hrBranch).Include(x => x.lastDesignation).Include(x => x.employeeStatus).ToListAsync();
                return data;
            }
            
        }



		public async Task<IEnumerable<FunctionInfo>> GetAllOffices()
		{
			return await _context.FunctionInfos.AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<FunctionInfo>> GetAllOffices1()
		{
			return await _context.employeeInfos.Select(x => x.functionInfo).AsNoTracking().ToListAsync();
		}
		public async Task<PFMemberInfo> GetPfMemberByEmpId(int empId)
		{
			return await _context.pfMemberInfos.Where(x => x.employeeInfoId == empId).FirstOrDefaultAsync();
		}
		public async Task<IEnumerable<HrUnit>> GetAllHrUnits()
		{
			return await _context.hrUnits.AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<HrDivision>> GetAllDivisions()
		{
			return await _context.hrDivisions.AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<Department>> GetAllDepartments1()
		{
			return await _context.departments.AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<EmployeeInfo>> GetActiveEmployeeAndPfMemberInfo()
		{
			var data = await (from e in _context.employeeInfos
							  join p in _context.pfMemberInfos
							  on e.Id equals p.employeeInfoId
							  select e
							 ).ToListAsync();

			return data;
		}


		public async Task<IEnumerable<HrBranch>> GetAllBranch()
		{
			_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
			return await _context.hrBranches.ToListAsync();
		}

        public async Task<IEnumerable<HrBranch>> GetAllBranchWithoutHeadOffice()
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return await _context.hrBranches.Where(x => x.Id != 205).ToListAsync();
        }
        public async Task<IEnumerable<Location>> GetAllZone()
		{
			_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
			return await _context.Locations.ToListAsync();
		}

		public async Task<IEnumerable<Designation>> GetAllDesignation()
		{
			_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
			return await _context.designations.OrderBy(x => Convert.ToInt32(x.designationCode)).ToListAsync();
		}


		public async Task<IEnumerable<Department>> GetAllDepartment()
		{
			_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
			return await _context.departments.ToListAsync();
		}
		public async Task<IEnumerable<HrDivision>> GetAllHrDivision()
		{
			_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
			return await _context.hrDivisions.ToListAsync();
		}
		public async Task<IEnumerable<Location>> GetAllZones()
		{
			_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
			return await _context.Locations.ToListAsync();
		}
		//public async Task<string> GetAllDepartmentById(int id)
		//{
		//    return await _context.departments.Where(x => x.Id == id).FirstOrDefaultAsync();
		//}

		public async Task<Department> GetAllDepartmentById(int id)
		{
			return await _context.departments.FindAsync(id);
		}


		public async Task<IEnumerable<ApplicationRole>> GetRolesByUserId(string id)
		{
			var data = await (from ur in _context.UserRoles.Where(x => x.UserId == id)
							  join r in _context.Roles
							  on ur.RoleId equals r.Id
							  select r).ToListAsync();
			return data;
		}
		public async Task<IEnumerable<PageAssingInfo>> GetPagesByRoleNames(string[] roles)
		{
			var data = await (from p in _context.UserAccessPages
							  join n in _context.Navbars on p.navbarId equals n.Id
							  join r in _context.Roles on p.applicationRoleId equals r.Id
							  join B in _context.Navbars on n.parentID equals B.Id into bbk
							  from bk in bbk.DefaultIfEmpty()
							  join Pa in _context.Navbars on bk.parentID equals Pa.Id into rrk
							  from rk in rrk.DefaultIfEmpty()
							  where roles.Contains(r.Name)
							  select new PageAssingInfo
							  {
								  pagename = n.nameOption,
								  bandname = bk.nameOption,
								  parentname = rk.nameOption,
							  }).ToListAsync();
			return data;
		}

		public async Task<IEnumerable<EmployeeInfo>> GetEmployeeInfoCROAnalyst()
		{
			_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
			List<string> leader = _context.Teams.Where(x => x.moduleId == 13 && x.teamId != null).Select(x => x.aspnetuserId).ToList();
			return await _context.employeeInfos.Include(x => x.department).Where(x => leader.Contains(x.ApplicationUserId)).ToListAsync();
		}
		public async Task<IEnumerable<EmployeeInfo>> GetEmployeeInfoCRMAnalyst()
		{
			_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
			List<string> leader = _context.Teams.Where(x => x.moduleId == 2 && x.teamId != null).Select(x => x.aspnetuserId).ToList();
			return await _context.employeeInfos.Include(x => x.department).Where(x => leader.Contains(x.ApplicationUserId)).ToListAsync();
		}
		public async Task<IEnumerable<EmployeeInfo>> GetEmployeeInfoCROAnalystByTeamId(int teamId)
		{
			_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
			List<string> leader = _context.Teams.Where(x => x.teamId == teamId).Select(x => x.aspnetuserId).ToList();
			return await _context.employeeInfos.Include(x => x.department).Where(x => leader.Contains(x.ApplicationUserId)).ToListAsync();
		}
		public async Task<IEnumerable<EmployeeInfo>> GetEmployeeInfoCROTeamLeader()
		{
			_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
			List<string> leader = _context.Teams.Where(x => x.moduleId == 13 && x.teamId == null).Select(x => x.aspnetuserId).ToList();
			return await _context.employeeInfos.Include(x => x.department).Where(x => leader.Contains(x.ApplicationUserId)).ToListAsync();
		}
		public async Task<IEnumerable<EmployeeInfo>> GetEmployeeInfoCROReview()
		{
			_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
			List<string> leader = _context.Teams.Where(x => x.moduleId == 13).Select(x => x.aspnetuserId).ToList();
			return await _context.employeeInfos.Include(x => x.department).Where(x => leader.Contains(x.ApplicationUserId)).ToListAsync();
		}

		public async Task<EmployeeInfo> GetEmployeeInfoById(int id)
		{
			_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
			return await _context.employeeInfos.Include(x => x.department).Include(x => x.currentGrade).Include(x => x.religion).Include(x => x.employeeType).Include(x => x.disabilityType).Include(x => x.JoinDesignation).Include(x => x.employeeStatus).Include(x => x.hrProgram).Include(x => x.hrUnit).Include(x => x.branch).Include(x => x.branch.company).Include(x => x.lastDesignation).Include(x => x.hrBranch).Include(x => x.hrDivision).Include(x => x.functionInfo).Include(x => x.location).AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
			// return await _context.employeeInfos.Where(x=>x.Id == id).FirstAsync();
		}

		public async Task<int> DeleteEducationById(int id)
		{
			_context.educationalQualifications.Remove(_context.educationalQualifications.Find(id));
			return await _context.SaveChangesAsync();
		}

		public async Task<string> GetBranchNameById(int? id)
		{
			var data = await _context.hrBranches.AsNoTracking().Where(x => x.Id == id).Select(x => x.branchName).FirstOrDefaultAsync();
			return data;
		}

		public async Task<string> GetDepartmentNameById(int id)
		{
			var data = await _context.departments.AsNoTracking().Where(x => x.Id == id).Select(x => x.deptName).FirstOrDefaultAsync();
			return data;
		}
        
		public async Task<string> GetDesignationNameBNBydesigId(int desigid)
		{
			var data = await _context.designations.AsNoTracking().Where(x => x.Id == desigid).Select(x => x.designationNameBN).FirstOrDefaultAsync();
			return data;
		}


		public async Task<string> GetOfficeNameById(int id)
		{
			var data = await _context.FunctionInfos.AsNoTracking().Where(x => x.Id == id).Select(x => x.branchUnitName).FirstOrDefaultAsync();
			return data;
		}
		public async Task<string> GetHrUnitNameById(int id)
		{
			var data = await _context.hrUnits.AsNoTracking().Where(x => x.Id == id).Select(x => x.unitName).FirstOrDefaultAsync();
			return data;
		}
		
		public async Task<string> GetZoneNameById(int id)
		{
			var data = await _context.Locations.AsNoTracking().Where(x => x.Id == id).Select(x => x.branchUnitName).FirstOrDefaultAsync();
			return data;
		}
        public async Task<int> GetZoneIdByBranchId(int BranchId)
		{
			var data = await _context.hrBranches.AsNoTracking().Where(x => x.Id == BranchId).Select(x => x.locationId).FirstOrDefaultAsync();
			return (int)data;
		}

		public async Task<EmployeeInfo> GetEmployeeInfoByCodetranfer(string code)
		{
			_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
			
			return await _context.employeeInfos.Include(x => x.department).Include(x => x.religion).Include(x => x.employeeType).Include(x => x.employeeStatus).Include(x => x.hrProgram).Include(x => x.hrUnit).Include(x => x.branch).Include(x => x.branch.company).Include(x => x.lastDesignation).Include(x => x.department).Include(x => x.hrBranch).Include(x => x.hrDivision).Include(x => x.functionInfo).Include(x => x.location).Where(x => x.employeeCode == code).FirstOrDefaultAsync();
			// return await _context.employeeInfos.Where(x=>x.Id == id).FirstAsync();
		}



		public async Task<IEnumerable<EmployeeDeath>> GetDeathRecordByEmpId(int empId)
		{
			return await _context.employeeDeaths.Where(x => x.employeeInfoId == empId).Include(x => x.employeeInfo).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<EmployeeDeath>> GetDeathRecordOfEmployee()
		{
			return await _context.employeeDeaths.Include(x => x.employeeInfo).Include(x => x.employeeInfo.department).Include(x => x.employeeInfo.lastDesignation).Include(x => x.employeeInfo.branch).AsNoTracking().ToListAsync();
		}

		public async Task<EmployeeInfo> GetEmployeeInfoByEmpId(int id)
		{
			return await _context.employeeInfos.FindAsync(id);
		}
		public async Task<string> GetTransferAttachmentUrlById(int id)
		{
			return await _context.transferLogs.Where(x => x.Id == id).Select(x => x.url).FirstOrDefaultAsync();
		}
		public async Task<string> GetClearanceAttachmentUrlById(int id)
		{
			return await _context.transferLogs.Where(x => x.Id == id).Select(x => x.clearenceUrl).FirstOrDefaultAsync();
		}
		public async Task<IEnumerable<EmployeeHobby>> GetAllHobbiesByEmp(int empId)
		{
			return await _context.employeeHobbies.Where(x => x.employeeInfoId == empId).Include(x => x.employeeInfo).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<EmployeeInfo>> GetPRLEmployeeList()
		{
			return await _context.employeeInfos.Where(x => x.activityStatus == 7).Include(x => x.Division).Include(x => x.lastDesignation).Include(x => x.department).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<ExperianceLetter>> GetExprienceLetterEmployeeList()
		{
			return await _context.experianceLetters.Include(x => x.employee).Include(x => x.employee.department).Include(x => x.employee.lastDesignation).Include(x => x.employee.branch).AsNoTracking().ToListAsync();
		}
		public async Task<int> SaveExperienceLetter(ExperianceLetter model)
		{
			if (model.Id != 0)
			{
				_context.experianceLetters.Update(model);
			}
			else
			{
				_context.experianceLetters.Add(model);
			}
			await _context.SaveChangesAsync();
			return model.Id;
		}
		public async Task<int> UpdateEmployeeNew(EmployeeInfo model)
		{
			if (model.Id != 0)
			{
				var emp = _context.employeeInfos.Find(model.Id);
				emp.seniorityLevel = model.seniorityLevel;
				_context.employeeInfos.Update(emp);
				return await _context.SaveChangesAsync();
			}
			else
			{
				return 1;
			}
		}
		public async Task<IEnumerable<EmployeeInfo>> GetPensionEmployeeList()
		{
			return await _context.employeeInfos.Where(x => x.activityStatus == 7).Include(x => x.Division).Include(x => x.lastDesignation).Include(x => x.department).AsNoTracking().ToListAsync();
		}



		public async Task<FoodLiking> GetFoodLikingById(int id)
		{
			var data = await _context.foodLikings.Where(x => x.employeeId == id).FirstOrDefaultAsync();
			return data;
		}


		public async Task<int> SaveEmployeeInfo(EmployeeInfo employeeInfo)
		{
			try
			{
				if (employeeInfo.Id != 0)
				{
					//_context.Entry(employeeInfo).State = EntityState.Modified;
					_context.employeeInfos.Update(employeeInfo);
				}
                else
                {
                    _context.employeeInfos.Add(employeeInfo);
                }
				await _context.SaveChangesAsync();
				return employeeInfo.Id;
			}
			catch (Exception ex)
			{

				throw;
			}
		}

		public int UpdateEmployeeBasic(int empId, decimal? amount)
		{
			_context.Database.ExecuteSqlCommand($"sp_UpdateEmployeeBasic {empId}, {amount}");
			return 1;
		}

		public async Task<IEnumerable<PrevJobHistory>> GetAllPrevJobHistoryByEmpId(int empId)
		{
			var data = await _context.prevJobHistories.AsNoTracking().Where(x => x.employeeId == empId).ToListAsync();
			return data;
		}
		public async Task<IEnumerable<EmployeeAccounts>> GetAllEmployeeAccountsByEmpId(int empId)
		{
			var data = await _context.employeeAccounts.AsNoTracking().Where(x => x.employeeInfoId == empId).ToListAsync();
			return data;
		}
		public async Task<IEnumerable<JobResponsibility>> GetAllResponsibilities()
		{
			var data = await _context.jobResponsibilities.AsNoTracking().ToListAsync();
			return data;
		}
		public async Task<int> SaveAppreciationLetter(AppreciationLetter appreciationLetter)
		{
			try
			{
				if (appreciationLetter.Id != 0)
				{
					_context.appreciationLetters.Update(appreciationLetter);
				}

				else
				{
					_context.appreciationLetters.Add(appreciationLetter);
				}

				await _context.SaveChangesAsync();
				return appreciationLetter.Id;
			}
			catch (Exception)
			{

				throw;
			}
		}
		public async Task<string> GetAppreciationImgUrlById(int id)
		{
			return await _context.appreciationLetters.Where(x => x.Id == id).Select(x => x.fileUrl).FirstOrDefaultAsync();
		}

		public async Task<int> SaveFoodLiking(FoodLiking foodLiking)
		{
			try
			{
				if (foodLiking.Id != 0)
				{
					_context.foodLikings.Update(foodLiking);
				}
				else
				{
					_context.foodLikings.Add(foodLiking);
				}
				await _context.SaveChangesAsync();
				return foodLiking.Id;
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<int> SaveIeltsInfo(IeltsInfo ieltsInfo)
		{
			try
			{
				if (ieltsInfo.Id != 0)
					_context.ieltsInfos.Update(ieltsInfo);
				else
					_context.ieltsInfos.Add(ieltsInfo);
				await _context.SaveChangesAsync();
				return ieltsInfo.Id;

			}
			catch (Exception)
			{
				throw;

			}

		}
		public async Task<bool> DeleteIeltsById(int id)
		{
			_context.ieltsInfos.Remove(_context.ieltsInfos.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}


		public async Task<bool> UpdateEmployeeinfoById(int id)
		{
			EmployeeInfo employeeInfo = await _context.employeeInfos.FindAsync(id);
			if (employeeInfo != null)
			{

				//employeeInfo.isApproved = 0;
				//employeeInfo.activityStatus = 3;

				employeeInfo.isApproved = 1;
				//employeeInfo.activityStatus = 1;
				return 1 == await _context.SaveChangesAsync();
			}
			else
			{
				return 1 == 0;
			}
		}

		public async Task<int> UpdatePreviousPostingDate(int empId, int postingId)
		{
			var prevPost = await _context.employeePostings.AsNoTracking().Where(x => x.employeeId == empId && x.Id != postingId && x.endDate == null).ToListAsync();
			var post = await _context.employeePostings.AsNoTracking().Where(x => x.Id == postingId).FirstOrDefaultAsync();
			foreach (var item in prevPost)
			{
				item.endDate = post.startDate;
				_context.employeePostings.Update(item);
				await _context.SaveChangesAsync();
			}
			return 1;
		}
		public int UpdateEmployeePosting(int empId, int status, int? departmentId, int? locationId, int? functionInfo, int? hrDivisionId, int? hrUnitId, int? hrBranchId, int? postid)
		{
			_context.Database.ExecuteSqlCommand($"sp_updateEmployeePosting {empId}, {status}, {departmentId}, {locationId}, {functionInfo}, {hrDivisionId}, {hrUnitId}, {hrBranchId}, {postid}");
			return 1;
		}
		public int UpdateOnlineStatus(string username, int status)
		{
			_context.Database.ExecuteSqlCommand($"sp_UpdateLoginStatus {username}, {status}");
			return 1;
		}
		public int BackupDatabaseCopy(string dbName, string path)
		{
			_context.Database.ExecuteSqlCommand($"sp_BackupDatabaseCopy {dbName}, {path}");
			return 1;
		}
		public int PopulateAcrNotify(string username)
		{
			_context.Database.ExecuteSqlCommand($"sp_PopulateAcrNotification {username}");
			return 1;
		}

		public async Task<IEnumerable<ApplicationUser>> GetOnlineUsers()
		{
			var data = await _context.Users.Where(x => x.isOnline == 1).ToListAsync();
			return data;
		}



		public async Task<bool> UpdateEmployeeinfoById1(int id)
		{
			EmployeeInfo employeeInfo = await _context.employeeInfos.FindAsync(id);
			if (employeeInfo != null)
			{

				employeeInfo.isApproved = 1;
				employeeInfo.activityStatus = 1;
				return 1 == await _context.SaveChangesAsync();
			}
			else
			{
				return 1 == 0;
			}
		}

		//public async Task<bool> SaveEmpDeathRecord(EmployeeDeath employeeDeath)
		//{
		//    if (employeeDeath.Id != 0)
		//        _context.employeeDeaths.Update(employeeDeath);
		//    else
		//        _context.employeeDeaths.Add(employeeDeath);

		//    return 1 == await _context.SaveChangesAsync();
		//}
		public async Task<int> SaveEmpDeathRecord(EmployeeDeath model)
		{
			if (model.Id != 0)
			{
				_context.employeeDeaths.Update(model);
			}
			else
			{
				_context.employeeDeaths.Add(model);
			}
			await _context.SaveChangesAsync();
			return model.Id;
		}

		public async Task<bool> DeleteEmpDeathRecordById(int id)
		{
			_context.employeeDeaths.Remove(_context.employeeDeaths.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<bool> UpdateEmployeeinfoApproveStatusById(string username, int id)
		{
			string userName = username;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);

			EmployeeInfo employeeInfo = await _context.employeeInfos.FindAsync(id);
			if (employeeInfo != null)
			{
				if (roles.Contains("HRAdmin") || roles.Contains("admin"))
				{
					employeeInfo.isApproved = 0;
					employeeInfo.activityStatus = 1;
				}
				else
				{
					employeeInfo.isApproved = 1;
					employeeInfo.activityStatus = 3;
				}

				return 1 == await _context.SaveChangesAsync();
			}
			else
			{
				return 1 == 0;
			}
		}
		public async Task<bool> ApproveEmployeeinfoById(int id)
		{
			EmployeeInfo employeeInfo = await _context.employeeInfos.FindAsync(id);
			if (employeeInfo != null)
			{
				employeeInfo.isApproved = 1;
				return 1 == await _context.SaveChangesAsync();
			}
			else
			{
				return 1 == 0;
			}
		}
		public int CheckHolidayByDate(DateTime? dateh)
		{
			var data = _context.holidays.Where(x => Convert.ToDateTime(x.weeklyHoliday).Date == Convert.ToDateTime(dateh).Date).Count();
			return data;
		}
		public async Task<IEnumerable<DateTime>> GetAllHolidaysByMonthYear(int month, int year)
		{
			var data = await _context.holidays.Where(x => Convert.ToDateTime(x.weeklyHoliday).Date >= new DateTime(year, month, 1).Date && Convert.ToDateTime(x.weeklyHoliday).Date <= new DateTime(year, month, DateTime.DaysInMonth(year, month)).Date).Select(x => Convert.ToDateTime(x.weeklyHoliday)).ToListAsync();
			return data;
		}
		public async Task<IEnumerable<DateTime>> GetAllHolidaysByDateRange(DateTime? startDate, DateTime? endDate)
		{
			var data = await _context.holidays.Where(x => Convert.ToDateTime(x.weeklyHoliday).Date >= Convert.ToDateTime(startDate).Date && Convert.ToDateTime(x.weeklyHoliday).Date <= Convert.ToDateTime(endDate).Date).Select(x => Convert.ToDateTime(x.weeklyHoliday)).ToListAsync();
			return data;
		}

		public async Task<EmpManualAttendance> GetManualAttendanceByEmpCode(string empCode, DateTime? punchDate)
		{
			var data = await _context.empManualAttendances.Where(x => x.punchCardNo == empCode && Convert.ToDateTime(x.workDate).Date == Convert.ToDateTime(punchDate).Date).FirstOrDefaultAsync();
			return data;
		}
		//Asad Added
        public async Task<EmpManualAttendance> GetManualAttendanceByType(string empCode, DateTime? punchDate, int? attendanceTypeId)
        {
            var data = await _context.empManualAttendances.Where(x => x.punchCardNo == empCode &&   
				x.AttendanceTypeId == attendanceTypeId && Convert.ToDateTime(x.workDate).Date == Convert.ToDateTime(punchDate).Date).FirstOrDefaultAsync();
            return data;
        }

        public async Task<int> SaveManualAttendance(EmpManualAttendance model)
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
		public async Task<EmployeeInfo> GetEmployeeInfoByCode(string code)
		{
			
				var result = (from e in _context.employeeInfos.Include(x => x.ApplicationUser).Where(x => x.employeeCode == code).Include(x => x.hrBranch).Include(x => x.employeeType).Include(x => x.hrDivision).Include(x => x.department).Include(x => x.currentGrade).Include(x => x.lastDesignation).Include(x => x.branch).Include(x => x.shiftGroup)
							  join s in _context.employeePostings.Where(x => x.status == 1 && x.type == 1) on e.Id equals s.employeeId into ss
							  from sss in ss.DefaultIfEmpty()
							  where e.employeeCode == code
							  select new EmployeeInfo
							  {
								  Id = e.Id,
								  employeeCode = e.employeeCode,
								  nameEnglish = e.nameEnglish,
								  designation = e.lastDesignation.designationName,
								  nameBangla = e.nameBangla,
								  emailAddress = e.emailAddress,
								  mobileNumberOffice = e.mobileNumberOffice,
								  lastDesignation = e.lastDesignation,
								  shiftGroup = e.shiftGroup,
								  shiftGroupId = e.shiftGroupId,
								  departmentId = e.departmentId,
								  hrBranchId = e.hrBranchId,
								  locationId = e.locationId,
								  placeOfPosting = sss.hrBranchId == 205 ? "Head Office" : sss.placeName.ToString()
							  });
				return await result.FirstOrDefaultAsync();
			
		}

        public async Task<EmployeeUserViewModel> GetEmployeeUserInfoByCode(string code)
        {
            var result = (from e in _context.employeeInfos.Include(x => x.ApplicationUser).Include(x => x.hrBranch).Include(x => x.hrDivision).Include(x => x.department).Include(x => x.currentGrade).Include(x => x.lastDesignation).Include(x => x.branch)
                          join s in _context.spouses on e.Id equals s.employeeId into ss
                          from sss in ss.DefaultIfEmpty()
                          join c in _context.childrens on e.Id equals c.employeeId into cc
                          from ccc in cc.DefaultIfEmpty()
                          join sign in _context.employeeSignatures on e.Id equals sign.employeeId into ssign
                          from sssign in ssign.DefaultIfEmpty()
                          join photo in _context.photographs on e.Id equals photo.employeeId into p
                          from pp in p.DefaultIfEmpty()
                          join pre in _context.addresses.Include(a => a.district).Include(a => a.thana) on e.Id equals pre.employeeId into pa
                          from ppa in pa.DefaultIfEmpty()
                          join per in _context.addresses.Include(a => a.district).Include(a => a.thana) on e.Id equals per.employeeId into pea
                          from ppea in pea.DefaultIfEmpty()
                          join bd1 in _context.bankingDiplomas.Where(a => a.diplomaPart == "Part 1") on e.Id equals bd1.employeeId into bd11
                          from bd111 in bd11.DefaultIfEmpty()
                          join bd2 in _context.bankingDiplomas.Where(a => a.diplomaPart == "Part 2") on e.Id equals bd2.employeeId into bd22
                          from bd222 in bd22.DefaultIfEmpty()
                          join preEmp in _context.prevJobHistories on e.Id equals preEmp.employeeId into preEmpmt
                          from preEmpmnt in preEmpmt.DefaultIfEmpty()
                          join a in _context.AcrEmployeeInfos on e.employeeCode equals a.EmpCode into aa
                          from acr in aa.DefaultIfEmpty()
                          where e.employeeCode == code
                          select new EmployeeUserViewModel
                          {
                              EmpId = e.Id,
                              EmpCode = e.employeeCode,
                              EmpName = e.nameEnglish,
                              EmpNameBn = e.nameBangla,
                              FatherName = e.fatherNameEnglish,
                              MotherName = e.motherNameEnglish,
                              SpouseName = sss.spouseName,
                              BranchCode = e.hrBranch.branchCode,
                              BranchName = e.hrBranch.branchName,
                              //WorkStation = e.hrBranchId != null ? e.hrBranch.branchName : e.hrDivisionId != null ? e.hrDivision.divName : e.departmentId != null ? e.department.deptName : e.hrBranch.branchName,
                              WorkStation = e.presentPosting,
                              DesignationName = e.lastDesignation.designationName,
                              SignUrl = sssign.url,
                              ImageUrl = pp.url,
                              BirthDate = e.dateOfBirth.ToString(),
                              JoiningDate = e.joiningDateGovtService.ToString(),
                              PrlDate = e.prlDate.ToString(),
                              NID = e.nationalID,
                              JoiningDesignation = _context.designations.Where(s => s.Id == Convert.ToInt32(e.joiningDesignation)).Select(s => s.designationName).FirstOrDefault(),
                              MaritalStatus = e.maritalStatus,
                              BD1 = bd111.diplomaPart,
                              BD2 = bd222.diplomaPart,
                              PunishmentDate = acr.PunishmentDate,
                              PunishmentLetterNo = acr.PunishmentLetterNo,
                              PunishmentList = acr.PunishmentList,
                              RewardsList = acr.RewardsList,
                              PresentAddress = ppa.houseVillage + ',' + ppa.thana.thanaName + ',' + ppa.district.districtName,
                              PermanentAddress = ppea.houseVillage + ',' + ppa.thana.thanaName + ',' + ppa.district.districtName,
                              childrenCount = _context.childrens.Where(x => x.employeeId == e.Id).Select(x => x.Id).Count()
                              //PreviousEmployment = preEmpmnt.position + ',' + preEmpmnt.company
                          }).FirstOrDefaultAsync();
            return await result;
        }

		public async Task<EmployeeSignature> GetSignatureByEmpId(int id)
		{
			return await _context.employeeSignatures.AsNoTracking().Where(x => x.employeeId == id).LastOrDefaultAsync();
		}
        public async Task<EmployeeSignature> GetLeaveApproverSignatureByEmpId(int id)
        {
            var data = await (from lv in _context.leaveRegisters
							  join am in _context.approvalMasters on lv.employeeId equals am.employeeInfoId
                              join ad in _context.approvalDetails on am.Id equals ad.approvalMasterId
                              where lv.Id == id
                              select ad).FirstOrDefaultAsync();
			var sign = await _context.employeeSignatures.Where(x => x.employeeId == data.approverId).FirstOrDefaultAsync();
            return sign;
        }
        public async Task<bool> UpdateEmployeeinfoBystatus(int id, int employeeId)
		{
			EmployeeInfo employeeInfo = await _context.employeeInfos.FindAsync(id);
			if (employeeInfo != null)
			{

				employeeInfo.activityStatus = 1;
				return 1 == await _context.SaveChangesAsync();
			}
			else
			{
				return 1 == 0;
			}
		}

		public async Task<decimal> GetNewIntegrationAmountByEmpCode(string code)
		{
			var emp = await _context.employeeInfos.Include(x => x.currentGrade).Where(x => x.employeeCode == code).FirstOrDefaultAsync();
			var newSlabAmount = await _context.salarySlabs.Where(x => x.slabAmount > emp.currentBasic && x.salaryGradeId == emp.currentGradeId).OrderBy(x => x.slabAmount).Select(x => x.slabAmount).FirstOrDefaultAsync();
			return newSlabAmount;
		}

		public async Task<string> GetFixAmountByEmpCode(string code)
		{
			var emp = await _context.employeeInfos.Include(x => x.currentGrade).Where(x => x.employeeCode == code).FirstOrDefaultAsync();
			var newSlabAmount = await _context.salarySlabs.Where(x => x.slabAmount > emp.currentBasic && x.salaryGradeId == emp.currentGradeId).OrderBy(x => x.slabAmount).Select(x => x.slabAmount).FirstOrDefaultAsync();
			return Convert.ToString(newSlabAmount.ToString("N0"));
		}


		public async Task<decimal> GetNewPromotionAmountByEmpCode(string code)
		{
			var emp = await _context.employeeInfos.Include(x => x.currentGrade).Where(x => x.employeeCode == code).FirstOrDefaultAsync();
			var nextGrade = await _context.salaryGrades.Where(x => x.Id < emp.currentGradeId).LastOrDefaultAsync();
			var newSlabAmount = await _context.salarySlabs.Where(x => x.salaryGradeId == nextGrade.Id && x.slabAmount > Convert.ToDecimal(emp.currentBasic)).OrderBy(x => x.slabAmount).Select(x => x.slabAmount).FirstOrDefaultAsync();
			return newSlabAmount;
		}
		public async Task<string> GetNewPromotionGradeNameByEmpCode(string code)
		{
			var emp = await _context.employeeInfos.Include(x => x.currentGrade).Where(x => x.employeeCode == code).FirstOrDefaultAsync();
			var nextGrade = await _context.salaryGrades.Where(x => x.Id < emp.currentGradeId).LastOrDefaultAsync();
			var newGradename = await _context.salarySlabs.Where(x => x.salaryGradeId == nextGrade.Id).Select(x => x.salaryGrade.gradeName).FirstOrDefaultAsync();
			return newGradename;
		}


		public async Task<int> GetNewPromotionGradeIdByEmpCode(string code)
		{
			var emp = await _context.employeeInfos.Include(x => x.currentGrade).Where(x => x.employeeCode == code).FirstOrDefaultAsync();
			var nextGrade = await _context.salaryGrades.Where(x => x.Id < emp.currentGradeId).LastOrDefaultAsync();
			var newGradeId = await _context.salarySlabs.Where(x => x.salaryGradeId == nextGrade.Id).Select(x => x.salaryGrade.Id).FirstOrDefaultAsync();
			return newGradeId;
		}




		public async Task<EmployeeInfo> GetEmployeeInfoByCode1(string code)
		{
			return await _context.employeeInfos.Where(x => x.employeeCode == code).Include(x => x.department).Include(x => x.ApplicationUser).Include(x => x.lastDesignation).AsNoTracking().FirstOrDefaultAsync();
		}

        public async Task<EmployeeInfoSpLoanViewModel> GetEmployeeInfoByCodeSp(string empCode)
        {
            return await _context.employeeInfoSpLoanViewModels.FromSql($"sp_GetEmployeeByCodeLoan {empCode}").FirstOrDefaultAsync();
        }
		//Asad Added on 02.08.2023
		public async Task<DepartBranchZoneHeadViewModel> GetApproverByEmployeeId(int employeeId)
		{
			try
			{
				var data = await _context.departBranchZoneHeadVM.FromSql($"sp_GetHeadOfBankByEmpIdNew @p0", employeeId).FirstOrDefaultAsync();
				return data;
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}
		//Asad Added on 02.08.2023
		public async Task<DepartBranchZoneHeadViewModel> GetSecondLevelApproverByEmployeeId(int employeeId)
		{
			try
			{
				var data = await _context.departBranchZoneHeadVM.FromSql($"sp_GetHeadOfBankForHeadByEmpIdNew @p0", employeeId).FirstOrDefaultAsync();
				return data;
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}


		public async Task<DepartBranchZoneHeadViewModel> GetHeadByDepartBranchZoneId(int? depId, int? branchId, int? zoneId)
        {

            try
            {
                var data = await _context.departBranchZoneHeadVM.FromSql($"sp_GetHeadOfBankByEmpId @p0,@p1,@p2", depId, branchId, zoneId).FirstOrDefaultAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public async Task<BranchZoneDepHeadInfoViewModel> GetHeadInfoByEmpOrDepartBranchZoneId(int? empId,int? depId, int? branchId, int? zoneId)
        {

            try
            {
                var data = await _context.branchZoneDepHeadInfoVM.FromSql($"sp_CheckHeadOfDepBranchZoneByEmpId  @p0,@p1,@p2,@p3", empId,depId, branchId, zoneId).FirstOrDefaultAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

		 public async Task<DepartBranchZoneHeadViewModel> GetHeadForHeadByDepartBranchZoneId(int? depId, int? branchId, int? zoneId)
        {

            try
            {
                var data = await _context.departBranchZoneHeadVM.FromSql($"sp_GetHeadOfBankForHeadByEmpId @p0,@p1,@p2", depId, branchId, zoneId).FirstOrDefaultAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }




		public async Task<EmployeeInfoLoanVm> GetEmployeeInfoByCodeForLoan(string code)
		{
            return await _context.employeeInfos.Where(x => x.employeeCode == code).Include(x => x.department).Include(x => x.lastDesignation).AsNoTracking().Select(x => new EmployeeInfoLoanVm
            {
                nameEnglish = x.nameEnglish,
                designation = x.lastDesignation.designationName,
                employeeCode = x.employeeCode,
                mobilePersonal = x.mobileNumberPersonal,
            }).FirstOrDefaultAsync();
		}

		public async Task<EmployeeInfoLoanVmNew> GetEmployeeInfoByCodeForLoanNew(string code)
		{
            return await _context.employeeInfos
									.Where(x => x.employeeCode == code)
									.Include(x => x.department)
									.Include(x => x.lastDesignation)
									.Include(x => x.hrBranch)
									.Include(x => x.employeeStatus)
									.AsNoTracking().Select(x => new EmployeeInfoLoanVmNew
									{
										nameEnglish = x.nameEnglish,
										designation = x.lastDesignation.designationName,
										employeeCode = x.employeeCode,
										mobilePersonal = x.mobileNumberPersonal,
										postingPlace=x.hrBranch.branchName,
										status=x.employeeStatus.statusName
									}).FirstOrDefaultAsync();
		}


		public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
		{
			return await _context.Users.AsNoTracking().ToListAsync();
		}

		public async Task<ApplicationUser> GetUserInfoByUserName(string username)
		{
			var data = await _context.Users.Where(x => x.UserName == username).FirstOrDefaultAsync();
			return data;
		}
		public async Task<ApplicationUser> GetUserInfoByUserId(int userid)
		{
			var data = await _context.Users.Where(x => x.userId == userid).FirstOrDefaultAsync();
			return data;
		}
		public async Task<int> saveUserLogHistory(UserLogHistory model)
		{
			if (model.Id != 0)
			{
				_context.UserLogHistories.Update(model);
			}
			else
			{
				_context.UserLogHistories.Add(model);
			}
			return await _context.SaveChangesAsync();
		}
		public async Task<int> CheckUserLoginCount(string username)
		{
			//var data = await _context.UserLogHistories.Where(x => x.userId == username).Select(x => x.Id).ToListAsync();
			var data = await _context.Users.Where(x => x.UserName == username).Select(x => x.isPassChange).FirstOrDefaultAsync();
			return data;
		}

		public async Task<EmployeeInfo> GetEmployeeInfoByAspnetId(string Id)
		{
			return await _context.employeeInfos.Where(x => x.ApplicationUser.Id == Id).AsNoTracking().FirstOrDefaultAsync();
		}

		public async Task<AspNetUsersViewModel> GetUserInfoByEmpCode(string code)
		{
			var result = (from U in _context.Users
						  join E in _context.employeeInfos on U.Id equals E.ApplicationUserId
						  where E.employeeCode == code
						  select new AspNetUsersViewModel
						  {
							  aspnetId = U.Id,
							  UserName = U.UserName,
							  Email = U.Email,
							  EmpName = E.nameEnglish,
							  EmployeeId = E.Id,
							  UserId = U.userId
						  }).FirstOrDefaultAsync();
			return await result;
		}
        public async Task<IEnumerable<AspNetUsersViewModel>> GetUserAllInfoByEmpCode(string code)
        {
            var result = await (from U in _context.Users
                          join E in _context.employeeInfos.Include(x=>x.hrBranch) on U.Id equals E.ApplicationUserId into emp
						  from em in emp.DefaultIfEmpty()
						  join UR in _context.UserRoles on U.Id equals UR.UserId
						  join R in _context.Roles on UR.RoleId equals R.Id
                          where em.employeeCode == code
                          select new AspNetUsersViewModel
                          {
                              aspnetId = U.Id,
                              UserName = U.UserName,
                              Email = U.Email,
                              EmpName = em.nameEnglish,
                              EmployeeId = em.Id,
                              UserId = U.userId,
							  roleId=R.Name,
                              BranchUnit=em.hrBranch.branchName,
                              specialBranchUnitId =em.hrBranchId
                          }).ToListAsync();
            return  result;
        }
        public async Task<ApplicationUser> GetUserIdByEmpId(int Id)
		{
			var result = await (from U in _context.Users
								join E in _context.employeeInfos on U.Id equals E.ApplicationUserId
								where E.Id == Id
								select U).FirstOrDefaultAsync();
			return result;
		}



		public async Task<int> GetEmployeeIdByCode(string code)
		{
			EmployeeInfo data = await _context.employeeInfos.Where(x => x.employeeCode == code).FirstOrDefaultAsync();
			if (data != null)
			{
				return data.Id;
			}
			else
			{
				return 1;
			}
		}
		public async Task<int> GetEmployeeIdByUsername(string username)
		{

			EmployeeInfo data = await _context.employeeInfos.Where(x => x.ApplicationUser.UserName == username).FirstOrDefaultAsync();
			if (data != null)
			{
				return data.Id;
			}
			else
			{
				return 1;
			}
		}
		public async Task<EmployeeInfo> GetEmployeeIdByCode2(string code)
		{
			EmployeeInfo data = await _context.employeeInfos.Where(x => x.employeeCode == code).FirstOrDefaultAsync();
			return data;
		}
		public async Task<EmployeeInfo> GetEmployeeInfoByCode2(string code)
		{
			return await _context.employeeInfos.Where(x => x.employeeCode == code).AsNoTracking().FirstOrDefaultAsync();
		}
		public async Task<IEnumerable<EmployeeInfo>> GetEmployeeInfoList()
		{
			return await _context.employeeInfos.AsNoTracking().ToListAsync();
		}


		public async Task<EmployeeInfo> GetEmployeeInfoByApplicationId(string userId)
		{
			return await _context.employeeInfos.Where(x => x.ApplicationUserId == userId).AsNoTracking().FirstOrDefaultAsync();
		}

		public async Task<EmployeeInfo> GetEmployeeInfoByApplicationId1(int userId)
		{
			return await _context.employeeInfos.Where(x => x.ApplicationUser.userId == userId).AsNoTracking().FirstOrDefaultAsync();
		}
		public async Task<IEnumerable<EmployeeInfoWithPostingVm>> GetActiveEmployeeInfoWithPosting()
		{
			return await _context.employeeInfoWithPostings.FromSql($"sp_GetEmployeeInfoWithPosting").ToListAsync();
		}
		public async Task<EmployeeInformationByCodeVm> GetEmployeeInformationByCode(string code)
		{
			return await _context.employeeInformationByCodes.FromSql($"sp_GetEmployeeInformationByCode {code}").FirstOrDefaultAsync();
		}
		public async Task<IEnumerable<EmployeeInfoWithPostingVm>> GetAllEmployeeInfoWithPosting()
		{
			return await _context.employeeInfoWithPostings.FromSql($"sp_GetAllEmployeeInfoWithPosting").ToListAsync();
		}
		public async Task<IEnumerable<EmployeeInfoWithPostingVm>> GetAllEmployeeInfoWithPostingForVoucherEntry()
		{
			return await _context.employeeInfoWithPostings.FromSql($"sp_GetAllEmployeeInfoWithPostingForVoucherEntry").ToListAsync();
		}
		
		public async Task<IEnumerable<AllUsersOnlineStatusVm>> GetAllUsersOnlineStatus()
		{
			return await _context.allUsersOnlineStatusVms.FromSql($"sp_GetAllUsersOnlineStatus").ToListAsync();
		}

		public async Task<IEnumerable<EmployeeWithDesignationVM>> GetEmployeeInfoDetailsList(int empId)
		{
			return await _context.employeeWithDesignations.FromSql($"sp_GetEmployeeDetailsList @p0,@p1", empId, string.Empty).ToListAsync();
		}
		public async Task<IEnumerable<GetAllActiveEmployeesVm>> GetAllActiveEmployees()
		{
			return await _context.getAllActiveEmployees.FromSql($"sp_GetAllActiveEmployees").ToListAsync();
		}

		public async Task<FixationReportViewModel> GetFixationByFixId(int fixId)
		{
            return await _context.fixationReports.FromSql($"sp_GetFixationReport @p0", fixId).FirstOrDefaultAsync();
        }

        public async Task<FixationReportPreviewViewModel> GetFixationByEmpId(int empId,int fixationGrade)
		{
            try
            {
                var data = await _context.fixationReportPreviewViewModels.FromSql($"sp_GetFixationReportNew @p0,@p1", empId, fixationGrade).FirstOrDefaultAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }



		public async Task<IEnumerable<IeltsInfo>> GetIeltsInfoByEmpId(int empId)
		{
			return await _context.ieltsInfos.Where(x => x.employeeId == empId).Include(x => x.employee).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
		}

		public async Task<string> GetEmployeeNameCodeById(int Id)
		{
			EmployeeInfo data = await _context.employeeInfos.FindAsync(Id);
			if (data != null)
			{
				return data.nameEnglish + "-" + data.employeeCode;
			}
			else
			{
				return "";
			}
		}
		public async Task<string> GetIeltsImgUrlById(int id)
		{
			return await _context.ieltsInfos.Where(x => x.Id == id).Select(x => x.attachment).FirstOrDefaultAsync();
		}
		public async Task<string> GetPassportImgUrlById(int id)
		{
			return await _context.passportDetails.Where(x => x.Id == id).Select(x => x.attachmentUrl).FirstOrDefaultAsync();
		}
		public async Task<string> GetExperienceLetterById(int id)
		{
			return await _context.experianceLetters.Where(x => x.Id == id).Select(x => x.fileUrl).FirstOrDefaultAsync();
		}


		public async Task<string> GetNomineeImgUrlById(int id)
		{
			return await _context.nominees.Where(x => x.Id == id).Select(x => x.imageUrl).FirstOrDefaultAsync();
		}
		//public async Task<IEnumerable<Occupation>> GetOccupation()
		//{
		//    return await _context.occupations.AsNoTracking().OrderBy(x => x.occupationName).ToListAsync();
		//}

		public async Task<IEnumerable<AllEmployeeJson>> GetAllEmployeeInfo(int empStatus, string org)
		{
			IQueryable<EmployeeInfo> queryData = _context.employeeInfos.Where(x => x.orgType == org && empStatus == x.activityStatus);

			#region Result Process
			IEnumerable<EmployeeInfo> data = await queryData.ToListAsync();
			List<AllEmployeeJson> filteredData = new List<AllEmployeeJson>();

			foreach (EmployeeInfo employeeInfo in data)
			{
				filteredData.Add(new AllEmployeeJson
				{
					employeeCode = employeeInfo.employeeCode,
					nameEnglish = employeeInfo.nameEnglish,
					emailAddress = employeeInfo.emailAddress,
					mobileNumberOffice = employeeInfo.mobileNumberOffice,
					designation = employeeInfo.designation,
					lastDesignation = employeeInfo.lastDesignation.designationName,
					action = $"<a class='btn btn-success' data-toggle='tooltip' title='Edit' target='_blank' href='/HRPMSEmployee/Photograph/EditGrid/{employeeInfo.Id}'><i class='fa fa-edit'></i></a> <a class='btn btn-success' data-toggle='tooltip' title='Preview' target='_blank' href='/HRPMSEmployee/InfoView/Index/{employeeInfo.Id}'><i class='fas fa-eye'></i></a> <a class='btn btn-info' data-toggle='tooltip' title='Print' target='_blank' href='/HRPMSEmployee/InfoView/pdspdf/{employeeInfo.Id}'><i class='fa fa-print'></i></a>"
				});
			}
			#endregion

			return filteredData;
		}

		public async Task<IEnumerable<AllEmployeeJson>> GetAllEmployeeInfoJson()
		{
			IQueryable<EmployeeInfo> queryData = _context.employeeInfos.OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).OrderBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.activityStatus != 2 && x.activityStatus != 10 && x.lastDesignation != null).Include(x=>x.hrBranch); //Not Inactive/Delete

			#region Result Process
			IEnumerable<EmployeeInfo> data = await queryData.ToListAsync();
			List<AllEmployeeJson> filteredData = new List<AllEmployeeJson>();

			foreach (EmployeeInfo employeeInfo in data)
			{
				filteredData.Add(new AllEmployeeJson
				{
					employeeId = employeeInfo.Id,
					employeeCode = employeeInfo.employeeCode,
					branch=employeeInfo.hrBranch?.branchName,
					nameEnglish = employeeInfo.nameEnglish,
					emailAddress = employeeInfo.emailAddress,
					mobileNumberOffice = employeeInfo.mobileNumberOffice,
					designation = employeeInfo.designation,
					lastDesignation = await _context.designations.Where(x => x.Id == employeeInfo.lastDesignationId).Select(x => x.designationName).AsNoTracking().FirstOrDefaultAsync(),
					department = await _context.departments.Where(x => x.Id == employeeInfo.departmentId).Select(x => x.deptName).AsNoTracking().FirstOrDefaultAsync(),
					companies = _context.Companies.AsNoTracking().FirstOrDefault().companyName,
					imageUrl = await _context.photographs.Where(x => x.employeeId == employeeInfo.Id).Select(x => x.url).AsNoTracking().FirstOrDefaultAsync(),
					activityStatus = employeeInfo.activityStatus,
					action = $"<a class='btn btn-success' data-toggle='tooltip' title='Edit' target='_blank' href='/HRPMSEmployee/Photograph/EditGrid/{employeeInfo.Id}'><i class='fa fa-edit'></i></a> <a class='btn btn-success' data-toggle='tooltip' title='Preview' target='_blank' href='/HRPMSEmployee/InfoView/Index/{employeeInfo.Id}'><i class='fas fa-eye'></i></a> <a class='btn btn-info' data-toggle='tooltip' title='Print' target='_blank' href='/HRPMSEmployee/InfoView/pdspdf/{employeeInfo.Id}'><i class='fa fa-print'></i></a>"
				});
			}
			#endregion

			return filteredData;
		}

		public async Task<IEnumerable<AllEmployeeJson>> GetAllActiveEmployeeInfo()
		{
			IQueryable<EmployeeInfo> queryData = _context.employeeInfos.Where(x => x.activityStatus == 1 || x.activityStatus == 3); //Active+Draft

			#region Result Process
			IEnumerable<EmployeeInfo> data = await queryData.ToListAsync();
			List<AllEmployeeJson> filteredData = new List<AllEmployeeJson>();

			foreach (EmployeeInfo employeeInfo in data)
			{
				filteredData.Add(new AllEmployeeJson
				{
					employeeId = employeeInfo.Id,
					employeeCode = employeeInfo.employeeCode,
					nameEnglish = employeeInfo.nameEnglish,
					emailAddress = employeeInfo.emailAddress,
					mobileNumberOffice = employeeInfo.mobileNumberOffice,
					designation = employeeInfo.designation,
					lastDesignation = await _context.designations.Where(x => x.Id == employeeInfo.lastDesignationId).Select(x => x.designationName).AsNoTracking().FirstOrDefaultAsync(),
					department = await _context.departments.Where(x => x.Id == employeeInfo.departmentId).Select(x => x.deptName).AsNoTracking().FirstOrDefaultAsync(),
					companies = _context.Companies.AsNoTracking().FirstOrDefault().companyName,
					imageUrl = await _context.photographs.Where(x => x.employeeId == employeeInfo.Id).Select(x => x.url).AsNoTracking().FirstOrDefaultAsync(),
					activityStatus = employeeInfo.activityStatus,
					action = $"<a class='btn btn-success' data-toggle='tooltip' title='Edit' target='_blank' href='/HRPMSEmployee/Photograph/EditGrid/{employeeInfo.Id}'><i class='fa fa-edit'></i></a> <a class='btn btn-success' data-toggle='tooltip' title='Preview' target='_blank' href='/HRPMSEmployee/InfoView/Index/{employeeInfo.Id}'><i class='fas fa-eye'></i></a> <a class='btn btn-info' data-toggle='tooltip' title='Print' target='_blank' href='/HRPMSEmployee/InfoView/pdspdf/{employeeInfo.Id}'><i class='fa fa-print'></i></a>"
				});
			}
			#endregion

			return filteredData;
		}

        public async Task<IEnumerable<AllEmployeeJsonNew>> GetAllActiveDraftEmployeeInfo()
        {
            var data = _context.allEmployeeJsonNews.FromSql($"sp_GetAllActiveAndDraftEmployee");
            return await data.ToListAsync();
        }
		public async Task<IEnumerable<AllEmployeeJsonNew>> GetNotPfMemberEmployeeList()
        {
            var data = _context.allEmployeeJsonNews.FromSql($"sp_GetNotPfMemberEmployeeList");
            return await data.ToListAsync();
        }
        public async Task<IEnumerable<AttendanceDataForChart>> GetAttendanceChart()
        {
            var data = _context.attendanceDataForCharts.FromSql($"sp_GetAttendanceDataForChart");
            return await data.ToListAsync();
        }
        public async Task<IEnumerable<AllEmployeeJsonNew>> GetAllAbsentEmployeeInfo()
		{
			var data = _context.allEmployeeJsonNews.FromSql($"sp_GetAllAbsentEmployeeDashboard");
			return await data.ToListAsync();
		}



        public async Task<IEnumerable<AllEmployeeJsonNewForPresent>> GetAllPresentEmployeeInfo()
        {
            var data = _context.allEmployeeJsonNewForPresents.FromSql($"sp_GetAllPresentEmployeeDashboard");
            return await data.ToListAsync();
        }

        public async Task<EmployeeInfoUserDash> GetEmployeeInfoForUserDashboard(int empId)
        {
            var data = _context.employeeInfoUserDashes.FromSql($"sp_EmployeeInfoForUserDashboard {empId}");
            return await data.FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<GetAttendanceLogByEmpIdVm>> GetAttendanceByEmpId(int empId)
        {
            var data = _context.getAttendanceLogByEmpIdVms.FromSql($"sp_GetAttendanceLogByEmpId {empId}");
            return await data.ToListAsync();
        }
        public async Task<IEnumerable<AllEmployeeJsonForLeave>> GetTodayLeaveEmployeeInfo()
		{
			var data = _context.allEmployeeJsonForLeaves.FromSql($"sp_GetAllLeaveEmployeeDashboard");
			return await data.ToListAsync();
		}



		public async Task<int> GetTotalPresentToday()
		{
			var data = await _context.empAttendances.Where(x => Convert.ToDateTime(x.workDate).Date == DateTime.Now.Date).GroupBy(x => x.punchCardNo).Select(x => x.First()).CountAsync();
			return data;
		}
		public async Task<int> GetTotalLeaveToday()
		{
			var data = await (from e in _context.employeeInfos
							  join l in _context.leaveRegisters
							  on e.Id equals l.employeeId
							  where Convert.ToDateTime(l.leaveFrom).Date <= DateTime.Now.Date && Convert.ToDateTime(l.leaveTo).Date >= DateTime.Now.Date && l.leaveStatus == 3
							  select e.Id).CountAsync();
			return data;
		}
		public async Task<IEnumerable<OPUSERP.Areas.HRPMSEmployee.Models.EmployeeInfoViewModel>> GetTodayLeavePersons()
		{
			var data = await (from e in _context.employeeInfos
							  join l in _context.leaveRegisters
							  on e.Id equals l.employeeId
							  join p in _context.photographs
							  on e.Id equals p.employeeId into emp
							  from ph in emp.DefaultIfEmpty()
							  where Convert.ToDateTime(l.leaveFrom).Date <= DateTime.Now.Date && Convert.ToDateTime(l.leaveTo).Date >= DateTime.Now.Date && l.leaveStatus == 3
							  select new OPUSERP.Areas.HRPMSEmployee.Models.EmployeeInfoViewModel
							  {
								  employeeInfo = e,
								  photograph = ph
							  }).ToListAsync();
			return data;
		}
		public async Task<IEnumerable<OPUSERP.Areas.HRPMSEmployee.Models.EmployeeInfoViewModel>> GetAttendsToday()
		{
			var data = await (from e in _context.employeeInfos.Include(x => x.lastDesignation)
							  join d in _context.designations
							  on e.lastDesignationId equals d.Id
							  join a in _context.empAttendances
							  on e.employeeCode equals a.punchCardNo
							  join p in _context.photographs
							  on e.Id equals p.employeeId into emp
							  from ph in emp.DefaultIfEmpty()
							  where Convert.ToDateTime(DateTime.Now).Date == Convert.ToDateTime(a.workDate).Date
							  select new OPUSERP.Areas.HRPMSEmployee.Models.EmployeeInfoViewModel
							  {
								  employeeInfo = e,
								  photograph = ph
							  }).ToListAsync();
			return data;
		}



		public async Task<IEnumerable<OPUSERP.Areas.HRPMSEmployee.Models.EmployeeInfoViewModel>> GetAbsentsToday()
		{
			var empCodes = await _context.empAttendances.Where(x => Convert.ToDateTime(DateTime.Now).Date == Convert.ToDateTime(x.workDate).Date).Select(x => x.punchCardNo).Distinct().ToListAsync();

            //var employees = new List<EmployeeInfo>();

            var data = await (from e in _context.employeeInfos.Include(x => x.lastDesignation)
                              join d in _context.designations
                              on e.lastDesignationId equals d.Id
                              join p in _context.photographs
                              on e.Id equals p.employeeId into emp
                              from ph in emp.DefaultIfEmpty()
                              where !empCodes.Contains(e.employeeCode) && e.activityStatus != 0
                              select new OPUSERP.Areas.HRPMSEmployee.Models.EmployeeInfoViewModel
                              {
                                  employeeInfo = e,
                                  photograph = ph,
                                  employeePostingPlaceInfo = String.Join(',', _context.employeePostings.Where(x => x.type == 1 && x.status == 1 && x.employeeId == e.Id).Select(x => x.placeName).ToList())
                              }).ToListAsync();
			return data;
		}

        public string GetPostingByEmpId(int empId)
        {
            var data = String.Join(',', _context.employeePostings.Where(x => x.type == 1 && x.status == 1 && x.employeeId == empId).Select(x => x.placeName).ToList());
            return data;
        }

        public async Task<int> GetEmpCountByBranchId(int id)
        {
            return await _context.employeeInfos.AsNoTracking().Where(x => x.hrBranchId == id && x.activityStatus != 0).CountAsync();
        }
        public async Task<string> GetProfilePhotoByEmpId(int empId)
        {
            var data = await _context.photographs.Where(x => x.employeeId == empId && x.type == "profile").Select(x => x.url).LastOrDefaultAsync();
            return data;
        }
        public async Task<int> GetEmpCountByDegId(int id)
		{
			return await _context.employeeInfos.AsNoTracking().Where(x => x.lastDesignationId == id && x.activityStatus != 0).CountAsync();
		}
		public async Task<int> GetEmpCountByDepId(int id)
		{
			return await _context.employeeInfos.AsNoTracking().Where(x => x.departmentId == id && x.activityStatus != 0).CountAsync();
		}
		public async Task<int> GetEmpCountByDivId(int id)
		{
			return await _context.employeeInfos.AsNoTracking().Where(x => x.hrDivisionId == id && x.activityStatus != 0).CountAsync();
		}
		public async Task<int> GetEmpCountByZoneId(int id)
		{
			return await _context.employeeInfos.AsNoTracking().Where(x => x.locationId == id && x.activityStatus != 0).CountAsync();
		}
		public async Task<int> GetEmpCountByOfficeId(int id)
		{
			return await _context.employeeInfos.AsNoTracking().Where(x => x.functionInfoId == id && x.activityStatus != 0).CountAsync();
		}
		public async Task<int> GetEmpCountByHrUnitId(int id)
		{
			return await _context.employeeInfos.AsNoTracking().Where(x => x.hrUnitId == id && x.activityStatus != 0).CountAsync();
		}
		public async Task<int> GetTotalBranchEmpCount()
		{
			return await _context.employeeInfos.AsNoTracking().Where(x => x.hrBranchId != null && x.hrBranchId != 205 && x.activityStatus != 0).CountAsync();
		}
		public async Task<IEnumerable<EmployeeInfo>> GetTotalBranchEmployee()
		{
			return await _context.employeeInfos.Include(x => x.lastDesignation).Include(x => x.branch).Include(x => x.hrBranch).Where(x => x.hrBranchId != null && x.activityStatus != 0).AsNoTracking().ToListAsync();
		}
		public async Task<int> GetTotalDegEmpCount()
		{
			return await _context.employeeInfos.AsNoTracking().Where(x => x.lastDesignationId != null && x.activityStatus != 0).CountAsync();
		}
		public async Task<IEnumerable<EmployeeInfo>> GetTotalDegEmp()
		{
			return await _context.employeeInfos.Include(x => x.lastDesignation).Include(x => x.branch).Include(x => x.hrBranch).Where(x => x.lastDesignationId != null && x.activityStatus != 0).AsNoTracking().ToListAsync();
		}
		public async Task<int> GetTotalDepEmpCount()
		{
			return await _context.employeeInfos.AsNoTracking().Where(x => x.departmentId != null && x.activityStatus != 0 && x.employeeCode != "665544").CountAsync();
		}
		public async Task<int> GetTotalDivEmpCount()
		{
			return await _context.employeeInfos.AsNoTracking().Where(x => x.hrDivisionId != null && x.activityStatus != 0).CountAsync();
		}
		public async Task<int> GetTotalZoneEmpCount()
		{
			return await _context.employeeInfos.AsNoTracking().Where(x => x.locationId != null && x.activityStatus != 0).CountAsync();
		}
		public async Task<int> GetTotalOfficeEmpCount()
		{
			return await _context.employeeInfos.AsNoTracking().Where(x => x.functionInfo != null && x.activityStatus != 0 && (x.functionInfoId != 17 && x.functionInfoId != 10 && x.functionInfoId != 11 && x.functionInfoId != 12)).CountAsync();
		}
		public async Task<int> GetTotalSubsideryEmpCount()
		{
			return await _context.employeeInfos.AsNoTracking().Where(x => x.functionInfo != null && x.activityStatus != 0 && (x.functionInfoId == 17 || x.functionInfoId == 10 || x.functionInfoId == 11 || x.functionInfoId == 12)).CountAsync();
		}
		public async Task<int> GetTotalHrUnitEmpCount()
		{
			return await _context.employeeInfos.AsNoTracking().Where(x => x.hrUnitId != null && x.activityStatus != 0).CountAsync();
		}
		public async Task<IEnumerable<EmployeeInfo>> GetTotalDepEmp()
		{
			return await _context.employeeInfos.Include(x => x.department).Include(x => x.lastDesignation).Where(x => x.departmentId != null && x.activityStatus != 0).AsNoTracking().ToListAsync();
		}


		public async Task<AllEmployeeJson> GetEmployeeInfoJson(string username)
		{
			EmployeeInfo employeeInfo = await _context.employeeInfos.AsNoTracking().Where(x => x.ApplicationUser.UserName == username).FirstOrDefaultAsync();

			#region Result Process
			var data = new AllEmployeeJson();
			if (employeeInfo != null)
			{
				data = new AllEmployeeJson
				{
					employeeId = employeeInfo.Id,
					employeeCode = employeeInfo.employeeCode,
					nameEnglish = employeeInfo.nameEnglish,
					emailAddress = employeeInfo.emailAddress,
					mobileNumberOffice = employeeInfo.mobileNumberOffice,
					designation = employeeInfo.designation,
					lastDesignation = await _context.designations.Where(x => x.Id == employeeInfo.lastDesignationId).Select(x => x.designationName).AsNoTracking().FirstOrDefaultAsync(),
					department = await _context.departments.Where(x => x.Id == employeeInfo.departmentId).Select(x => x.deptName).FirstOrDefaultAsync(),
					branch = await _context.hrBranches.Where(x => x.Id == employeeInfo.hrBranchId).Select(x => x.branchName).FirstOrDefaultAsync(),
					division = await _context.hrDivisions.Where(x => x.Id == employeeInfo.hrDivisionId).Select(x => x.divName).FirstOrDefaultAsync(),
					companies = _context.Companies.FirstOrDefault().companyName,
					imageUrl = await _context.photographs.Where(x => x.employeeId == employeeInfo.Id).Select(x => x.url).FirstOrDefaultAsync(),
					activityStatus = employeeInfo.activityStatus,
					action = $"<a class='btn btn-success' data-toggle='tooltip' title='Edit' target='_blank' href='/HRPMSEmployee/Photograph/EditGrid/{employeeInfo.Id}'><i class='fa fa-edit'></i></a> <a class='btn btn-success' data-toggle='tooltip' title='Preview' target='_blank' href='/HRPMSEmployee/InfoView/Index/{employeeInfo.Id}'><i class='fas fa-eye'></i></a> <a class='btn btn-info' data-toggle='tooltip' title='Print' target='_blank' href='/HRPMSEmployee/InfoView/pdspdf/{employeeInfo.Id}'><i class='fa fa-print'></i></a>"
				};
			}
			#endregion

			return data;
		}
		public async Task<IEnumerable<HrBranch>> GetBranch()
		{
			return await _context.hrBranches.Include(x => x.office).Include(x => x.branchType).Include(x => x.location).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<AllEmployeeJson>> GetAllInactiveEmployeeInfoJson()
		{
			IQueryable<EmployeeInfo> queryData = _context.employeeInfos.Where(x => x.activityStatus == 0);

			#region Result Process
			IEnumerable<EmployeeInfo> data = await queryData.ToListAsync();
			List<AllEmployeeJson> filteredData = new List<AllEmployeeJson>();

			foreach (EmployeeInfo employeeInfo in data)
			{
				filteredData.Add(new AllEmployeeJson
				{
					employeeId = employeeInfo.Id,
					employeeCode = employeeInfo.employeeCode,
					nameEnglish = employeeInfo.nameEnglish,
					emailAddress = employeeInfo.emailAddress,
					mobileNumberOffice = employeeInfo.mobileNumberOffice,
					department = await _context.departments.Where(x => x.Id == employeeInfo.departmentId).Select(x => x.deptName).FirstOrDefaultAsync(),
					designation = employeeInfo.designation,
					companies = _context.Companies.FirstOrDefault().companyName,
					imageUrl = await _context.photographs.Where(x => x.employeeId == employeeInfo.Id).Select(x => x.url).FirstOrDefaultAsync(),
					action = $"<a class='btn btn-success' data-toggle='tooltip' title='Edit' target='_blank' href='/HRPMSEmployee/Photograph/EditGrid/{employeeInfo.Id}'><i class='fa fa-edit'></i></a> <a class='btn btn-success' data-toggle='tooltip' title='Preview' target='_blank' href='/HRPMSEmployee/InfoView/Index/{employeeInfo.Id}'><i class='fas fa-eye'></i></a> <a class='btn btn-info' data-toggle='tooltip' title='Print' target='_blank' href='/HRPMSEmployee/InfoView/pdspdf/{employeeInfo.Id}'><i class='fa fa-print'></i></a>"
				});
			}
			#endregion

			return filteredData;
		}

		public async Task<IEnumerable<AllEmployeeJson>> GetEmployeeInfoSingle(int empStatus, string org, string empode)
		{
			IQueryable<EmployeeInfo> queryData = _context.employeeInfos.Where(x => x.orgType == org && x.employeeCode == empode && empStatus == x.activityStatus);

			#region Result Process
			IEnumerable<EmployeeInfo> data = await queryData.ToListAsync();
			List<AllEmployeeJson> filteredData = new List<AllEmployeeJson>();

			foreach (EmployeeInfo employeeInfo in data)
			{
				filteredData.Add(new AllEmployeeJson
				{
					employeeCode = employeeInfo.employeeCode,
					nameEnglish = employeeInfo.nameEnglish,
					emailAddress = employeeInfo.emailAddress,
					mobileNumberOffice = employeeInfo.mobileNumberOffice,
					designation = employeeInfo.designation,
					imageUrl = await _context.photographs.Where(x => x.employeeId == employeeInfo.Id).Select(x => x.url).FirstOrDefaultAsync(),
					action = $"<a class='btn btn-success' data-toggle='tooltip' title='Edit' target='_blank' href='/HRPMSEmployee/Photograph/EditGrid/{employeeInfo.Id}'><i class='fa fa-edit'></i></a> <a class='btn btn-success' data-toggle='tooltip' title='Preview' target='_blank' href='/HRPMSEmployee/InfoView/Index/{employeeInfo.Id}'><i class='fas fa-eye'></i></a> <a class='btn btn-info' data-toggle='tooltip' title='Print' target='_blank' href='/HRPMSEmployee/InfoView/pdspdf/{employeeInfo.Id}'><i class='fa fa-print'></i></a>"
				});
			}
			#endregion

			return filteredData;
		}


		//Here We GetQuery Result
		public async Task<IEnumerable<AllEmployeeJson>> GetEmployeeInfoAsQueryAble(string queryString, string org)
		{
			IQueryable<EmployeeInfo> queryData = _context.employeeInfos.Where(x => x.orgType == org);

			#region Filtering...

			string[] Tokens = queryString.Split("&");
			foreach (string token in Tokens)
			{
				string[] SepToken = token.Split("=");
				if (SepToken.Length > 1)
				{
					if (SepToken[0] == "EmpType")
					{
						queryData = queryData.Where(x => x.employeeTypeId == Int32.Parse(SepToken[1]));
					}
					else if (SepToken[0] == "PRL")
					{
						DateTime nowAndEx = DateTime.Now.AddMonths(Int32.Parse(SepToken[1]));
						DateTime now = DateTime.Now;
						queryData = queryData.Where(x => (x.LPRDate <= nowAndEx && x.LPRDate >= now));
					}
				}
			}
			#endregion

			#region Result Process
			IEnumerable<EmployeeInfo> data = await queryData.ToListAsync();
			List<AllEmployeeJson> filteredData = new List<AllEmployeeJson>();

			foreach (EmployeeInfo employeeInfo in data)
			{
				filteredData.Add(new AllEmployeeJson
				{
					employeeCode = employeeInfo.employeeCode,
					nameEnglish = employeeInfo.nameEnglish,
					emailAddress = employeeInfo.emailAddress,
					mobileNumberOffice = employeeInfo.mobileNumberOffice,
					designation = employeeInfo.designation,
					imageUrl = await _context.photographs.Where(x => x.employeeId == employeeInfo.Id).Select(x => x.url).FirstOrDefaultAsync(),
					action = $"<a class='btn btn-success' data-toggle='tooltip' title='Edit' target='_blank' href='/HRPMSEmployee/Photograph/EditGrid/{employeeInfo.Id}'><i class='fa fa-edit'></i></a> <a class='btn btn-success' data-toggle='tooltip' title='Preview' target='_blank' href='/HRPMSEmployee/InfoView/Index/{employeeInfo.Id}'><i class='fas fa-eye'></i></a> <a class='btn btn-info' data-toggle='tooltip' title='Print' target='_blank' href='/HRPMSEmployee/InfoView/pdspdf/{employeeInfo.Id}'><i class='fa fa-print'></i></a>"
				});
			}
			#endregion

			return filteredData;
		}

		//Here We GetQuery Result
		public async Task<IEnumerable<AllEmployeeJson>> GetActiveEmployeeInfoAsQueryAble(string queryString, string org)
		{
			IQueryable<EmployeeInfo> queryData = _context.employeeInfos.Where(x => x.orgType == org && (x.activityStatus == 1 || x.activityStatus == 3));

			#region Filtering...

			string[] Tokens = queryString.Split("&");
			foreach (string token in Tokens)
			{
				string[] SepToken = token.Split("=");
				if (SepToken.Length > 1)
				{
					if (SepToken[0] == "EmpType")
					{
						queryData = queryData.Where(x => x.employeeTypeId == Int32.Parse(SepToken[1]));
					}
					else if (SepToken[0] == "PRL")
					{
						DateTime nowAndEx = DateTime.Now.AddMonths(Int32.Parse(SepToken[1]));
						DateTime now = DateTime.Now;
						queryData = queryData.Where(x => (x.LPRDate <= nowAndEx && x.LPRDate >= now));
					}
				}
			}
			#endregion

			#region Result Process
			IEnumerable<EmployeeInfo> data = await queryData.ToListAsync();
			List<AllEmployeeJson> filteredData = new List<AllEmployeeJson>();

			foreach (EmployeeInfo employeeInfo in data)
			{
				filteredData.Add(new AllEmployeeJson
				{
					employeeCode = employeeInfo.employeeCode,
					nameEnglish = employeeInfo.nameEnglish,
					emailAddress = employeeInfo.emailAddress,
					mobileNumberOffice = employeeInfo.mobileNumberOffice,
					designation = employeeInfo.designation,
					imageUrl = await _context.photographs.Where(x => x.employeeId == employeeInfo.Id).Select(x => x.url).FirstOrDefaultAsync(),
					action = $"<a class='btn btn-success' data-toggle='tooltip' title='Edit' target='_blank' href='/HRPMSEmployee/Photograph/EditGrid/{employeeInfo.Id}'><i class='fa fa-edit'></i></a> <a class='btn btn-success' data-toggle='tooltip' title='Preview' target='_blank' href='/HRPMSEmployee/InfoView/Index/{employeeInfo.Id}'><i class='fas fa-eye'></i></a> <a class='btn btn-info' data-toggle='tooltip' title='Print' target='_blank' href='/HRPMSEmployee/InfoView/pdspdf/{employeeInfo.Id}'><i class='fa fa-print'></i></a>"
				});
			}
			#endregion

			return filteredData;
		}

		//Here We GetQuery Result
		public async Task<IEnumerable<AllEmployeeJson>> GetInactiveEmployeeInfoAsQueryAble(string queryString, string org)
		{
			IQueryable<EmployeeInfo> queryData = _context.employeeInfos.Where(x => x.orgType == org && x.activityStatus == 0);

			#region Filtering...

			string[] Tokens = queryString.Split("&");
			foreach (string token in Tokens)
			{
				string[] SepToken = token.Split("=");
				if (SepToken.Length > 1)
				{
					if (SepToken[0] == "EmpType")
					{
						queryData = queryData.Where(x => x.employeeTypeId == Int32.Parse(SepToken[1]));
					}
					else if (SepToken[0] == "PRL")
					{
						DateTime nowAndEx = DateTime.Now.AddMonths(Int32.Parse(SepToken[1]));
						DateTime now = DateTime.Now;
						queryData = queryData.Where(x => (x.LPRDate <= nowAndEx && x.LPRDate >= now));
					}
				}
			}
			#endregion

			#region Result Process
			IEnumerable<EmployeeInfo> data = await queryData.ToListAsync();
			List<AllEmployeeJson> filteredData = new List<AllEmployeeJson>();

			foreach (EmployeeInfo employeeInfo in data)
			{
				filteredData.Add(new AllEmployeeJson
				{
					employeeCode = employeeInfo.employeeCode,
					nameEnglish = employeeInfo.nameEnglish,
					emailAddress = employeeInfo.emailAddress,
					mobileNumberOffice = employeeInfo.mobileNumberOffice,
					designation = employeeInfo.designation,
					imageUrl = await _context.photographs.Where(x => x.employeeId == employeeInfo.Id).Select(x => x.url).FirstOrDefaultAsync(),
					action = $"<a class='btn btn-success' data-toggle='tooltip' title='Edit' target='_blank' href='/HRPMSEmployee/Photograph/EditGrid/{employeeInfo.Id}'><i class='fa fa-edit'></i></a> <a class='btn btn-success' data-toggle='tooltip' title='Preview' target='_blank' href='/HRPMSEmployee/InfoView/Index/{employeeInfo.Id}'><i class='fas fa-eye'></i></a> <a class='btn btn-info' data-toggle='tooltip' title='Print' target='_blank' href='/HRPMSEmployee/InfoView/pdspdf/{employeeInfo.Id}'><i class='fa fa-print'></i></a>"
				});
			}
			#endregion

			return filteredData;
		}

		public async Task<IEnumerable<AllEmployeeJson>> GetEmployeeInfoAsQueryAbleSingle(string queryString, string org, string empode)
		{
			IQueryable<EmployeeInfo> queryData = _context.employeeInfos.Where(x => x.orgType == org && x.employeeCode == empode);

			#region Filtering...

			string[] Tokens = queryString.Split("&");
			foreach (string token in Tokens)
			{
				string[] SepToken = token.Split("=");
				if (SepToken.Length > 1)
				{
					if (SepToken[0] == "EmpType")
					{
						queryData = queryData.Where(x => x.employeeTypeId == Int32.Parse(SepToken[1]));
					}
					else if (SepToken[0] == "PRL")
					{
						DateTime nowAndEx = DateTime.Now.AddMonths(Int32.Parse(SepToken[1]));
						DateTime now = DateTime.Now;
						queryData = queryData.Where(x => (x.LPRDate <= nowAndEx && x.LPRDate >= now));
					}
				}
			}


			#endregion

			#region Result Process
			IEnumerable<EmployeeInfo> data = await queryData.ToListAsync();
			List<AllEmployeeJson> filteredData = new List<AllEmployeeJson>();

			foreach (EmployeeInfo employeeInfo in data)
			{
				filteredData.Add(new AllEmployeeJson
				{
					employeeCode = employeeInfo.employeeCode,
					nameEnglish = employeeInfo.nameEnglish,
					emailAddress = employeeInfo.emailAddress,
					mobileNumberOffice = employeeInfo.mobileNumberOffice,
					designation = employeeInfo.designation,
					imageUrl = await _context.photographs.Where(x => x.employeeId == employeeInfo.Id).Select(x => x.url).FirstOrDefaultAsync(),
					action = $"<a class='btn btn-success' data-toggle='tooltip' title='Edit' target='_blank' href='/HRPMSEmployee/Photograph/EditGrid/{employeeInfo.Id}'><i class='fa fa-edit'></i></a> <a class='btn btn-success' data-toggle='tooltip' title='Preview' target='_blank' href='/HRPMSEmployee/InfoView/Index/{employeeInfo.Id}'><i class='fas fa-eye'></i></a> <a class='btn btn-info' data-toggle='tooltip' title='Print' target='_blank' href='/HRPMSEmployee/InfoView/pdspdf/{employeeInfo.Id}'><i class='fa fa-print'></i></a>"
				});
			}
			#endregion

			return filteredData;
		}
		public async Task<IEnumerable<AllEmployeeJson>> GetEmployeeInfoAsQueryAbleApprove(string queryString, string org, string empode)
		{
			EmployeeInfo employee = await _context.employeeInfos.Where(x => x.employeeCode == empode).FirstOrDefaultAsync();
			List<int?> empId = await _context.approvalDetails.Where(x => x.approverId == employee.Id).Select(x => x.approvalMaster.employeeInfoId).ToListAsync();
			IQueryable<EmployeeInfo> queryData = _context.employeeInfos.Where(x => x.orgType == org && x.isApproved == 0 && empId.Contains(x.Id));

			#region Filtering...

			string[] Tokens = queryString.Split("&");
			foreach (string token in Tokens)
			{
				string[] SepToken = token.Split("=");
				if (SepToken.Length > 1)
				{
					if (SepToken[0] == "EmpType")
					{
						queryData = queryData.Where(x => x.employeeTypeId == Int32.Parse(SepToken[1]));
					}
					else if (SepToken[0] == "PRL")
					{
						DateTime nowAndEx = DateTime.Now.AddMonths(Int32.Parse(SepToken[1]));
						DateTime now = DateTime.Now;
						queryData = queryData.Where(x => (x.LPRDate <= nowAndEx && x.LPRDate >= now));
					}
				}
			}


			#endregion

			#region Result Process
			IEnumerable<EmployeeInfo> data = await queryData.ToListAsync();
			List<AllEmployeeJson> filteredData = new List<AllEmployeeJson>();

			foreach (EmployeeInfo employeeInfo in data)
			{
				filteredData.Add(new AllEmployeeJson
				{
					employeeCode = employeeInfo.employeeCode,
					nameEnglish = employeeInfo.nameEnglish,
					emailAddress = employeeInfo.emailAddress,
					mobileNumberOffice = employeeInfo.mobileNumberOffice,
					designation = employeeInfo.designation,
					action = $"<a class='btn btn-success' data-toggle='tooltip' title='Approve'  href='/HRPMSEmployee/Info/ApproveInfo/{employeeInfo.Id}'><i class='fa fa-check'></i></a> <a class='btn btn-success' data-toggle='tooltip' title='Preview' target='_blank' href='/HRPMSEmployee/InfoView/Index/{employeeInfo.Id}'><i class='fas fa-eye'></i></a> <a class='btn btn-info' data-toggle='tooltip' title='Print' target='_blank' href='/HRPMSEmployee/InfoView/pdspdf/{employeeInfo.Id}'><i class='fa fa-print'></i></a>"
				});
			}
			#endregion

			return filteredData;
		}

		public async Task<EmployeeInfo> GetFreeEmployeeByCode(string code)
		{
			return await _context.employeeInfos.Where(x => x.employeeCode == code && (x.ApplicationUserId == null || x.ApplicationUserId == "" || x.ApplicationUserId == "0")).FirstAsync();
		}

		public async Task<bool> UpdateEmployee(int Id, string authId, string org)
		{
			EmployeeInfo data = await _context.employeeInfos.FindAsync(Id);

			if (data == null) return false;
			data.ApplicationUserId = authId;
			data.orgType = org;

			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<EmployeeInfo>> GetEmployeeInfoByOrg(string org)
		{
			return await _context.employeeInfos.Include(x => x.lastDesignation).Where(x => x.orgType == org && x.isDelete == 0).AsNoTracking().ToListAsync();
		}

		public async Task<EmployeeInfo> GetEmployeeInfoByCodeAndOrg(string code, string orgType)
		{
			return await _context.employeeInfos.Where(x => x.employeeCode == code).Where(x => x.orgType == orgType).AsNoTracking().FirstOrDefaultAsync();
		}

		public async Task<string> GetAuthCodeByUserId(int empId)
		{
			return await _context.employeeInfos.Where(x => x.Id == empId).AsNoTracking().Select(x => x.ApplicationUserId).FirstOrDefaultAsync();
		}



		public async Task<string> GetCompanyIdByEmpId(int empId)
		{
			return await _context.employeeInfos.Where(x => x.Id == empId).Select(x => x.branch.company.companyName).AsNoTracking().FirstOrDefaultAsync();
		}
		public async Task<string> GetEmpBranchByempCode(string code)
		{
			return await _context.employeeInfos.Where(x => x.employeeCode == code).Select(x => x.hrBranch.branchName).AsNoTracking().FirstOrDefaultAsync();
		}


		public async Task<int> IsThisEmpIDPresent(string employeeId)
		{
			return await _context.employeeInfos.Where(x => x.employeeCode == employeeId).AsNoTracking().Select(x => x.Id).FirstOrDefaultAsync();
		}
		public async Task<int> IsThisApplicantIdPresent(int? ApplicantId)
		{
			return await _context.applicationForms.Where(x => x.Id == ApplicantId).AsNoTracking().Select(x => x.Id).FirstOrDefaultAsync();
		}


		public async Task<IEnumerable<EmployeeInfo>> GetDuplicateEmpCode(int empId, string empCode)
		{
			return await _context.employeeInfos.Where(x => x.Id != empId && x.employeeCode == empCode).AsNoTracking().ToListAsync();
		}

		//Dashboard 

		public async Task<IEnumerable<EmployeeInfo>> PRLInNextSixMonthByOrg(string org)
		{
			DateTime frm = DateTime.Now;
			DateTime to = frm.AddMonths(6);
			return await _context.employeeInfos.Where(x => x.orgType == org && (x.LPRDate <= to && x.LPRDate >= frm)).AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<EmployeeInfo>> LeaveInLastOneMonthByOrg(string org)
		{
			DateTime to = DateTime.Now;
			DateTime frm = to.AddMonths(-1);
			List<int> ids = await _context.leaveLogs.Where(x => x.leaveFrom >= frm && x.leaveFrom <= to).AsNoTracking().Select(x => (int)x.employeeId).ToListAsync();
			return await _context.employeeInfos.Where(x => x.orgType == org && ids.Contains(x.Id)).AsNoTracking().ToListAsync();
		}
		public async Task<string> GetEmployeeIDByAuthID(string empId)
		{
			return await _context.employeeInfos.Where(x => x.ApplicationUserId == empId).Select(x => x.employeeCode.ToString()).FirstOrDefaultAsync();
		}

		//Visual Data
		public async Task<string> GetEmpCodeNameVisualData()
		{
			return await _context.VisualDatas.AsNoTracking().Select(x => x.empCodeName).FirstOrDefaultAsync();
		}
		public async Task<IEnumerable<IeltsInfo>> GetIeltsInfos()
		{
			return await _context.ieltsInfos.Include(x => x.employee).AsNoTracking().ToListAsync();
		}
		public async Task<bool> SavePRL(EmployeeInfo employeeInfo)
		{
			if (employeeInfo.Id != 0)
				_context.employeeInfos.Update(employeeInfo);
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<IEnumerable<AppreciationLetter>> GetAppreciationLetters()
		{
			return await _context.appreciationLetters.Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.employee.department).AsNoTracking().ToListAsync();
		}
		public async Task<AppreciationLetter> GetAppreciationInformationById(int id)
		{
			return await _context.appreciationLetters.Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.employee.department).Where(x => x.Id == id).FirstOrDefaultAsync();
		}
		public async Task<bool> DeleteAppreciationLetterById(int id)
		{
			_context.appreciationLetters.Remove(_context.appreciationLetters.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<bool> DeleteExperienceLetterById(int id)
		{
			_context.experianceLetters.Remove(_context.experianceLetters.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<IEnumerable<BondLetter>> GetBondLetters()
		{
			return await _context.bondLetters.Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.employee.department).AsNoTracking().ToListAsync();
		}

		public async Task<string> GetBondFileUrlById(int id)
		{
			return await _context.bondLetters.Where(x => x.Id == id).Select(x => x.fileUrl).FirstOrDefaultAsync();
		}
		public async Task<int> SaveBondLetter(BondLetter bondLetter)
		{
			try
			{
				if (bondLetter.Id != 0)
					_context.bondLetters.Update(bondLetter);
				else
					_context.bondLetters.Add(bondLetter);
				await _context.SaveChangesAsync();
				return bondLetter.Id;
			}
			catch (Exception)
			{

				throw;
			}

		}
		public async Task<bool> DeleteBondLetterById(int id)
		{
			_context.bondLetters.Remove(_context.bondLetters.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<HrBranch> GetHrBranchById(int id)
		{
			return await _context.hrBranches.Where(x => x.Id == id).FirstOrDefaultAsync();
		}
		public async Task<Department> GetDepartmentById(int id)
		{
			return await _context.departments.Where(x => x.Id == id).FirstOrDefaultAsync();
		}
		public async Task<HrDivision> GetHrDivisionById(int id)
		{
			return await _context.hrDivisions.Where(x => x.Id == id).FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<BondLetter>> GetBondLetterEmployeeList()
		{
			return await _context.bondLetters.Include(x => x.employee).Include(x => x.employee.department).Include(x => x.employee.lastDesignation).Include(x => x.employee.branch).AsNoTracking().ToListAsync();
		}

		public async Task<int> saveUser(ApplicationUser user)
		{
			if (user.Id != null)
			{
				_context.Update(user);
			}
			else
			{
				_context.Add(user);
			}
			return await _context.SaveChangesAsync();
		}

		public async Task<List<string>> GetRoleNaturesByUserName(string username)
		{
			var data = await (from ur in _context.UserRoles
							  join u in _context.Users
							  on ur.UserId equals u.Id
							  join r in _context.Roles
							  on ur.RoleId equals r.Id
							  where u.UserName == username
							  select r.roleNature).ToListAsync();

			return data;
		}

		public async Task<EmployeeInfoByUsernameSp> EmployeeInfoByUsernameSp(string username)
		{
			return await _context.employeeInfoByUsernameSps.FromSql($"sp_GetEmployeeInfoByUsername {username}").FirstOrDefaultAsync();
		}
		public async Task<IEnumerable<AuditTables>> GetAuditTables()
		{
			var result = await _context.auditTables.FromSql($"sp_GetAuditTables").ToListAsync();
			return result;
		}

		public async Task<IEnumerable<AuditTrailViewModel>> GetAuditTrailsForLastSevenDays()
		{
            var tblName = "0";
            var colName = "0";
            var fromDate = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            var toDate = DateTime.Now.ToString("yyyy-MM-dd");
            var username = "0";
            var type = "0";
            var result = await _context.auditTrailViewModels.FromSql($"sp_GetAuditData {tblName}, {colName}, {fromDate}, {toDate}, {username}, {type}").ToListAsync();
            return result;
        }
        public async Task<IEnumerable<AuditTrailViewModel>> GetAuditTrailsByParams(string tableName, string operationType, DateTime? startDate, DateTime? endDate)
		{
            var colName = "0";
            var fromDate = startDate?.ToString("yyyy-MM-dd");
            var toDate = endDate?.ToString("yyyy-MM-dd");
            var username = "0";
            var result = await _context.auditTrailViewModels.FromSql($"sp_GetAuditData {tableName}, {colName}, {fromDate}, {toDate}, {username}, {operationType}").ToListAsync();
            return result;
        }

		public async Task<IEnumerable<EmployeeInfoSpViewModel>> GetEmployeeInfoByHrBranchId(int branchId)
		{
			//var data = await _context.employeeInfos.Include(x => x.lastDesignation).Include(x => x.hrBranch).Where(x => x.hrBranchId == branchId).AsNoTracking().ToListAsync();
			//return data;
			var data = _context.employeeInfoSpViewModels.FromSql($"sp_GetEmployeesByHrBranch {branchId}");
			return await data.ToListAsync();
		}
		public async Task<IEnumerable<EmployeeInfoSpViewModel>> GetEmployeeInfoByDesignationId(int designationId)
		{
			//var data = await _context.employeeInfos.Include(x => x.lastDesignation).Where(x => x.lastDesignation.Id == designationId).AsNoTracking().ToListAsync();
			var data = _context.employeeInfoSpViewModels.FromSql($"sp_GetEmployeesByDesignation {designationId}");
			return await data.ToListAsync();
		}
		public async Task<IEnumerable<EmployeeInfoSpViewModel>> GetEmployeeInfoByDepartmentId(int departmentId)
		{
			//var data = await _context.employeeInfos.Include(x => x.lastDesignation).Include(x => x.department).Where(x => x.department.Id == departmentId).AsNoTracking().ToListAsync();
			//return data;
			var data = _context.employeeInfoSpViewModels.FromSql($"sp_GetEmployeesByDepartment {departmentId}");
			return await data.ToListAsync();
		}

        public async Task<IEnumerable<BranchWithEmployeeCount>> GetAllBranchesWithEmployeesSp()
        {
            var data = await _context.branchWithEmployeeCounts.FromSql($"sp_branchWithEmployees").ToListAsync();
            return data;
        }
        public async Task<IEnumerable<DesignationWithEmployeeCount>> GetAllDesignationWithEmployeesSp()
        {
            var data = await _context.designationWithEmployeeCounts.FromSql($"sp_designationWithEmployees").ToListAsync();
            return data;
        }
        public async Task<IEnumerable<DepartmentWithEmployeeCount>> GetAllDeptWithEmployeesSp()
        {
            var data = await _context.departmentWithEmployeeCounts.FromSql($"sp_departmentWithEmployees").ToListAsync();
            return data;
        }
        public async Task<IEnumerable<DivisionWithEmployeeCount>> GetAllDivisionWithEmployeesSp()
        {
            var data = await _context.divisionWithEmployeeCounts.FromSql($"sp_divisionWithEmployees").ToListAsync();
            return data;
        }
        public async Task<IEnumerable<ZonesWithEmployeeCount>> GetAllZoneWithEmployeesSp()
        {
            var data = await _context.zonesWithsEmployeeCounts.FromSql($"sp_zoneWithEmployees").ToListAsync();
            return data;
        }
        public async Task<IEnumerable<OfficeWithEmployeeCount>> GetAllOfficeWithEmployeesSp()
        {
            var data = await _context.officeWithEmployeeCounts.FromSql($"sp_officeWithEmployees").ToListAsync();
            return data;
        }
        public async Task<IEnumerable<HrUnitWithEmployeeCount>> GetAllUnitWithEmployeesSp()
        {
            var data = await _context.hrUnitWithEmployeeCounts.FromSql($"sp_hrunitWithEmployees").ToListAsync();
            return data;
        }
        public async Task<IEnumerable<EmployeeInfoSpViewModel>> GetEmployeeInfoByZoneId(int locationId)
		{
			//var data = await _context.employeeInfos.Include(x => x.lastDesignation).Include(x => x.department).Where(x => x.location.Id == locationId).AsNoTracking().ToListAsync();
			//return data;
			var data = _context.employeeInfoSpViewModels.FromSql($"sp_GetEmployeesByZone {locationId}");
			return await data.ToListAsync();
		}
		public async Task<IEnumerable<EmployeeInfoSpViewModel>> GetEmployeeInfoByOfficeId(int officeId)
		{
			//var data = await _context.employeeInfos.Include(x => x.lastDesignation).Include(x => x.department).Where(x => x.functionInfo.Id == officeId).AsNoTracking().ToListAsync();
			//return data;
			var data = _context.employeeInfoSpViewModels.FromSql($"sp_GetEmployeesByOffice {officeId}");
			return await data.ToListAsync();
		}
		public async Task<IEnumerable<EmployeeInfoSpViewModel>> GetEmployeeInfoByHrDivisionId(int divisionId)
		{
			//var data = await _context.employeeInfos.Include(x => x.lastDesignation).Include(x => x.department).Where(x => x.functionInfo.Id == officeId).AsNoTracking().ToListAsync();
			//return data;
			var data = _context.employeeInfoSpViewModels.FromSql($"sp_GetEmployeesByHrDivision {divisionId}");
			return await data.ToListAsync();
		}
		public async Task<IEnumerable<EmployeeInfoSpViewModel>> GetEmployeeInfoByHrUnitId(int hrUnitId)
		{
			//var data = await _context.employeeInfos.Include(x => x.lastDesignation).Include(x => x.department).Where(x => x.hrUnit.Id == hrUnitId).AsNoTracking().ToListAsync();
			//return data;
			var data = _context.employeeInfoSpViewModels.FromSql($"sp_GetEmployeesByHrUnit {hrUnitId}");
			return await data.ToListAsync();
		}

		public async Task<EmployeeAcrViewModel> GetEmployeeUserInfoByCodeBySp(string empCode)
		{
			return await _context.employeeAcrViewModels.FromSql($"sp_employeeAcrViewModel {empCode}").FirstOrDefaultAsync();
		}

        public async Task<UserIsHRAdminVm> CheckUserIsHRAdmin(string username)
        {
			return await _context.userIsHRAdminVms.FromSql($"sp_CheckUserIsHRAdmin {username}").FirstOrDefaultAsync();
        }



        public async Task<SeniorityStatus> UpdateSeniorityNullByDesignation(int desigId)
		{
			return await _context.seniorityStatuses.FromSql($"sp_UpdateSeniorityNull {desigId}").FirstOrDefaultAsync();
		}
		public async Task<IEnumerable<SeniorityListVm>> GetSeniorityList(int desigId)
		{
			return await _context.seniorityListVms.FromSql($"sp_GetSeniorityList {desigId}").ToListAsync();
		}
		public async Task<MarkingInfoVm> GetMarkingInfosByEmpId(int employeeId)
		{
			return await _context.markingInfoVms.FromSql($"sp_GetMargingEmpInfo {employeeId}").FirstOrDefaultAsync();
		}
		
		public async Task<IEnumerable<MarkingEducation>> GetMarkingEducationByEmpId(int employeeId)
		{
			return await _context.markingEducations.FromSql($"sp_GetEducationalQualification {employeeId}").ToListAsync();
		}
		public async Task<IEnumerable<EmployeeAward>> GetAllAwardsByEmpId(int employeeId)
		{
			return await _context.employeeAwards.Include(x => x.award).Where(x => x.employeeId == employeeId).ToListAsync();
		}

		public async Task<int> CheckLeaveApprover(int approverId)
		{
			var data = await _context.approvalDetails.Where(x => x.approvalMaster.approvalTypeId == 1 && x.approverId == approverId && x.status == "Active").CountAsync();

			return data;
		}

    }
}
