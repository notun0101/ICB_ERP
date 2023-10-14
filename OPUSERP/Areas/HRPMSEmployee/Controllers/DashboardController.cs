using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.HRPMSEmployee.Models.Dashboard;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Services.Dashboard.interfaces;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
	[Authorize]
	[Area("HRPMSEmployee")]
	public class DashboardController : Controller
	{
		private readonly IHrmDashboardService hrmDashboardService;
		private readonly IPersonalInfoService personalInfoService;
		private readonly IDesignationDepartmentService designationDepartmentService;
		private readonly IEmployeePunchCardInfoService employeePunchCardInfoService;

		public DashboardController(IHrmDashboardService hrmDashboardService, IEmployeePunchCardInfoService employeePunchCardInfoService, IPersonalInfoService personalInfoService, IDesignationDepartmentService designationDepartmentService)
		{
			this.hrmDashboardService = hrmDashboardService;
			this.personalInfoService = personalInfoService;
			this.designationDepartmentService = designationDepartmentService;
			this.employeePunchCardInfoService = employeePunchCardInfoService;
		}

		public async Task<IActionResult> Index()
		{
			MainDashboardViewModel model = new MainDashboardViewModel
			{
				activeEmployeeCountViewModel = await hrmDashboardService.GetActiveEmployeeCountViewModel()
			};
			return View(model);
		}






		public async Task<IActionResult> DashboardValue()
		{
			MainDashboardViewModel model = new MainDashboardViewModel
			{
				activeEmployeeCountViewModel = await hrmDashboardService.GetActiveEmployeeCountViewModel()
			};
			return Json(model);
		}

		public async Task<IActionResult> ActiveEmployee()
		{
			var data = await personalInfoService.GetAllActiveDraftEmployeeInfo();
			return Json(data);
		}
        public async Task<IActionResult> TodayLeaveAllEmployee()
		{
			var data = await personalInfoService.GetTodayLeaveEmployeeInfo();
			return Json(data);
		}




        public async Task<IActionResult> PresentEmployee()
		{
			var data = await personalInfoService.GetAllPresentEmployeeInfo();
			return Json(data);
		}
         public async Task<IActionResult> AbsentEmployee()
		{
			var data = await personalInfoService.GetAllAbsentEmployeeInfo();
			return Json(data);
		}




		public async Task<IActionResult> HRDashboard()
		{
			try
			{
				await employeePunchCardInfoService.ProcessAttendance(DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("dd/MM/yyyy"));
			}
			catch (Exception)
			{

			}
			var activeEmp = await personalInfoService.GetAllActiveDraftEmployeeInfo();
			var totalPresentToday = await personalInfoService.GetTotalPresentToday();
			var totalLeaveToday = await personalInfoService.GetTotalLeaveToday();
			var todayLeavePersons = await personalInfoService.GetTodayLeavePersons();
			var todayAttendPersons = await personalInfoService.GetAttendsToday();
			var todayAbsentPersons = await personalInfoService.GetAbsentsToday();

            var allBranches = await personalInfoService.GetAllBranchesWithEmployeesSp();
            var allDesignations = await personalInfoService.GetAllDesignationWithEmployeesSp();
            var allDepartments = await personalInfoService.GetAllDeptWithEmployeesSp();
            var allHrDivisions = await personalInfoService.GetAllDivisionWithEmployeesSp();
            var allZones = await personalInfoService.GetAllZoneWithEmployeesSp();
            var allOffices = await personalInfoService.GetAllOfficeWithEmployeesSp();
            var allHrUnits = await personalInfoService.GetAllUnitWithEmployeesSp();

            var data = allBranches.Where(x => x.branchId != 205);
			var datadeg = allDesignations;
			var dataDep = allDepartments;
			var dataDiv = allHrDivisions;
			var dataZone = allZones;
			var dataOffice = allOffices;
			var dataUnit = allHrUnits;
			
			var empSum = await personalInfoService.GetTotalBranchEmpCount();
			var empSumDeg = await personalInfoService.GetTotalDegEmpCount();
			var empSumDep = await personalInfoService.GetTotalDepEmpCount();
			var empSumDiv = await personalInfoService.GetTotalDivEmpCount();
			var empSumZone = await personalInfoService.GetTotalZoneEmpCount();
			var empSumOffice = await personalInfoService.GetTotalOfficeEmpCount();
			var empSumSubsidery = await personalInfoService.GetTotalSubsideryEmpCount();
			var empSumHrUnit = await personalInfoService.GetTotalHrUnitEmpCount();
			var branch = await personalInfoService.GetBranch();
			var designation = await designationDepartmentService.GetDesignations();
			var departments = await designationDepartmentService.GetDepartment();
			var branchEmployee = await personalInfoService.GetTotalBranchEmployee();
			var DesignationEmployee = await personalInfoService.GetTotalDegEmp();
			var DepertmentEmployee = await personalInfoService.GetTotalDepEmp();
			//foreach (var item in allBranches)
			//{
			//	data.Add(new BranchWithEmployeeCount
			//	{
			//		branchId = item.Id,
			//		branchName = item.branchName,
			//		empCount = await personalInfoService.GetEmpCountByBranchId(item.Id)
			//	});
			//}
			//foreach (var item1 in allDesignations)
			//{
			//	datadeg.Add(new DesignationWithEmployeeCount
			//	{
			//		designationId = item1.Id,
			//		designationName = item1.designationName,
			//		empCount = await personalInfoService.GetEmpCountByDegId(item1.Id)
			//	});
			//}
			//foreach (var item2 in allDepartments.Where(x => x.isDelete != 1))
			//{
			//	dataDep.Add(new DepartmentWithEmployeeCount
			//	{
			//		departmentId = item2.Id,
			//		departmentName = item2.deptName,
			//		empCount = await personalInfoService.GetEmpCountByDepId(item2.Id)
			//	});
			//}
			//foreach (var item3 in allHrDivisions)
			//{
			//	dataDiv.Add(new DivisionWithEmployeeCount
			//	{
			//		divisionId = item3.Id,
			//		divisionName = item3.divName,
			//		empCount = await personalInfoService.GetEmpCountByDivId(item3.Id)
			//	});
			//}
			//foreach (var item4 in allZones)
			//{
			//	dataZone.Add(new ZonesWithEmployeeCount
			//	{
			//		zoneId = item4.Id,
			//		zoneName = item4.branchUnitName,
			//		empCount = await personalInfoService.GetEmpCountByZoneId(item4.Id)
			//	});
			//}
			//foreach (var item5 in allOffices)
			//{
			//	dataOffice.Add(new OfficeWithEmployeeCount
			//	{
			//		officeId = item5.Id,
			//		officeName = item5.branchUnitName,
			//		empCount = await personalInfoService.GetEmpCountByOfficeId(item5.Id)
			//	});
			//}
			//foreach (var item6 in allHrUnits)
			//{
			//	dataUnit.Add(new HrUnitWithEmployeeCount
			//	{
			//		hrUnitId = item6.Id,
			//		hrUnitName = item6.unitName,
			//		empCount = await personalInfoService.GetEmpCountByHrUnitId(item6.Id)
			//	});
			//}





			var model = new HrDashBoardViewModel
			{
				activeEmployee = activeEmp.Count(),
				todayPresent = totalPresentToday,
				todayAbsent = activeEmp.Count() - totalPresentToday - totalLeaveToday,
				leaveToday = totalLeaveToday,
				totalMalePercent = activeEmp.Where(x => x.gender == "Male").Count(),
				totalFemalePercent = activeEmp.Where(x => x.gender == "Female").Count(),
				todayLeave = todayLeavePersons,
				totalBranches = allBranches.Count(),
				totalBranchEmployees = empSum,
				branchEmps = data,
				totalDegs = allDesignations.Count(),
				totalDep = allDepartments.Count(),
				totalDiv = allHrDivisions.Count(),
				totalZone = allZones.Count(),
				totalOffice = allOffices.Where(x => x.officeId != 17 && x.officeId != 10 && x.officeId != 11 && x.officeId != 12).Count(),
				totalSubsidery = allOffices.Where(x => x.officeId == 17 || x.officeId == 10 || x.officeId == 11 || x.officeId == 12).Count(),
				totalHrUnit = allHrUnits.Count(),
				totalDegEmployees = empSumDeg,
				totalDepEmployees = empSumDep,
				totalDivEmployees = empSumDiv,
				totalZoneEmployees = empSumZone,
				totalOfficeEmployees = empSumOffice,
                totalSubsideryEmployees = empSumSubsidery,
				totalHrUnitEmployees = empSumHrUnit,
				degEmps = datadeg,
				depEmps = dataDep,
				divEmps = dataDiv,
				zoneEmps = dataZone,
				officeEmps = dataOffice,
				hrUnitEmps = dataUnit,
				presentEmployees = todayAttendPersons,
				absentEmployees = todayAbsentPersons,
				hrBranches = branch,
				designations = designation,
				departments = departments,
				branchEmployee = branchEmployee,
				DesignationEmployee = DesignationEmployee,
				DepertmentEmployee = DepertmentEmployee
            };
			return View(model);
		}

        public async Task<IActionResult> AttendanceDataForChart() {
            var attendanceDataForCharts = await personalInfoService.GetAttendanceChart();
            var data = new AttendanceDataViewModel
            {
                workDays = attendanceDataForCharts.Select(x => x.workD),
                presents = attendanceDataForCharts.Select(x => x.Present),
                absents = attendanceDataForCharts.Select(x => x.Absent)
            };
            return Json(data);
        }

		public async Task<IActionResult> GetJoiningInfoByYear()
		{
			var model = new List<JoningInfoViewModel>();
			var data = await personalInfoService.GetAllActiveDraftEmployeeInfo();
			model.Add(new JoningInfoViewModel
			{
				yearRange = "1965-1974",
				empCount = data.Where(x => Convert.ToDateTime(x.joiningDateGovtService).Year >= 1965 && Convert.ToDateTime(x.joiningDateGovtService).Year <= 1974).Count()
			});
			model.Add(new JoningInfoViewModel
			{
				yearRange = "1975-1984",
				empCount = data.Where(x => Convert.ToDateTime(x.joiningDateGovtService).Year >= 1975 && Convert.ToDateTime(x.joiningDateGovtService).Year <= 1984).Count()
			});
			model.Add(new JoningInfoViewModel
			{
				yearRange = "1985-1994",
				empCount = data.Where(x => Convert.ToDateTime(x.joiningDateGovtService).Year >= 1985 && Convert.ToDateTime(x.joiningDateGovtService).Year <= 1994).Count()
			});
			model.Add(new JoningInfoViewModel
			{
				yearRange = "1995-2004",
				empCount = data.Where(x => Convert.ToDateTime(x.joiningDateGovtService).Year >= 1995 && Convert.ToDateTime(x.joiningDateGovtService).Year <= 2004).Count()
			});
			model.Add(new JoningInfoViewModel
			{
				yearRange = "2005-2014",
				empCount = data.Where(x => Convert.ToDateTime(x.joiningDateGovtService).Year >= 2005 && Convert.ToDateTime(x.joiningDateGovtService).Year <= 2014).Count()
			});
			model.Add(new JoningInfoViewModel
			{
				yearRange = "2015-2024",
				empCount = data.Where(x => Convert.ToDateTime(x.joiningDateGovtService).Year >= 2015 && Convert.ToDateTime(x.joiningDateGovtService).Year <= 2024).Count()
			});
			return Json(model);
		}

		public async Task<IActionResult> GetEmployeesByTypeId(string type, int id)
		{
			if (type == "branch")
			{
				var data = await personalInfoService.GetEmployeeInfoByHrBranchId(id);
				return Json(data);
			}
			else if (type == "Designation")
			{
				var data = await personalInfoService.GetEmployeeInfoByDesignationId(id);
				return Json(data);
			}
			else if (type == "Department")
			{
				var data = await personalInfoService.GetEmployeeInfoByDepartmentId(id);
				return Json(data);
			}
			else if (type == "Zone")
			{
				var data = await personalInfoService.GetEmployeeInfoByZoneId(id);
				return Json(data);
			}
			else if (type == "Office")
			{
				var data = await personalInfoService.GetEmployeeInfoByOfficeId(id);
				return Json(data);
			}
			else if (type == "HrDivision")
			{
				var data = await personalInfoService.GetEmployeeInfoByHrDivisionId(id);
				return Json(data);
			}
			else if (type == "HrUnit")
			{
				var data = await personalInfoService.GetEmployeeInfoByHrUnitId(id);
				return Json(data);
			}
			else
			{
				return Json("nodata");
			}
		}

	}
}