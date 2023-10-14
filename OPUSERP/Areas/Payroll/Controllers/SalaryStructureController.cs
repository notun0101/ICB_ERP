using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.Payroll.Models;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.Payroll.Data.Entity;
using OPUSERP.Payroll.Data.Entity.Salary;
using OPUSERP.Payroll.Services.PF.Interfaces;
using OPUSERP.Payroll.Services.Salary.Interfaces;
using OPUSERP.ProvidentFund.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Payroll.Controllers
{
	[Area("Payroll")]
	[Authorize]
	public class SalaryStructureController : Controller
	{
		private readonly ISalaryService salaryService;
		private readonly IPFService pFService;
		private readonly IPhotographService photographService;
		private readonly IPersonalInfoService personalInfoService;
		private readonly IEmployeeProjectActivityService employeeProjectActivityService;
		public SalaryStructureController(ISalaryService salaryService, IPhotographService photographService, IPFService pFService, IPersonalInfoService personalInfoService, IEmployeeProjectActivityService employeeProjectActivityService)
		{
			this.salaryService = salaryService;
			this.photographService = photographService;
			this.personalInfoService = personalInfoService;
			this.employeeProjectActivityService = employeeProjectActivityService;
			this.pFService = pFService;
		}

		#region Salary Structure

		public async Task<ActionResult> Index(int? employeeInfoId)
		{
			var pfOwn = new EmployeesSalaryStructure();
			if (employeeInfoId != null)
			{
				pfOwn = await salaryService.GetSalaryStructureByEmpAndHeadId(Convert.ToInt32(employeeInfoId), 149);

			}
			EmployeesSalaryStructureViewModel model = new EmployeesSalaryStructureViewModel
			{
				employeesSalaryStructuresList = await salaryService.GetEmployeesSalaryStructureByEmpId(Convert.ToInt32(employeeInfoId)),
				salaryGradesList = await salaryService.GetAllSalaryGrade(),
				salaryPeriod = await salaryService.GetSalaryPeriodmax(),
				pfType = pfOwn.pfType,
				pfPercentage = pfOwn.pfPercentage
			};
			if (model.employeesSalaryStructure == null) model.employeesSalaryStructure = new EmployeesSalaryStructure();
			ViewBag.employeeInfoId = employeeInfoId;
			return View(model);
		}
		public async Task<ActionResult> SalStructure(int? employeeInfoId)
		{
			EmployeesSalaryStructureViewModel model = new EmployeesSalaryStructureViewModel
			{
				employeesSalaryStructuresList = await salaryService.GetEmployeesSalaryStructureByEmpId(Convert.ToInt32(employeeInfoId)),
				salaryGradesList = await salaryService.GetAllSalaryGrade(),
				salaryPeriod = await salaryService.GetSalaryPeriodmax()


			};
			if (model.employeesSalaryStructure == null) model.employeesSalaryStructure = new EmployeesSalaryStructure();
			ViewBag.employeeInfoId = employeeInfoId;
			return View(model);
		}
		

		public async Task<IActionResult> CalculateHouseRent(int empId, decimal basic)
		{
			var data = await salaryService.GetBasicCalculation(empId, basic);
			return Json(data);
		}
		public async Task<IActionResult> CalculatePFSubscription(int empId, decimal basic, string pfType, int? pfPercentage)
		{
			var data = await salaryService.GetPFSubsByEmpId(empId, basic, pfType, pfPercentage);
			return Json(data);
		}

		public async Task<JsonResult> GetHouseRentAmount(int employeeId)
        {
			var data = await salaryService.GetSalaryStructureByEmpAndHeadId(employeeId, 134);
            if (data != null)
            {
				return Json(data.amount);
            }
            else
            {
				return Json(0);
            }
		}
		public async Task<IActionResult> GetPfTypeByEmpId(int empId)
		{
			var data = await salaryService.GetSalaryStructureByEmpAndHeadId(empId, 149);
			if (data == null)
			{
				var emp = await personalInfoService.GetEmployeeInfoById(empId);
				if (emp.joiningDateGovtService != null)
				{
					if (Convert.ToDateTime(emp.joiningDateGovtService).Date >= new DateTime(2011, 1, 1).Date)
					{
						data = new EmployeesSalaryStructure
						{
							pfType = "CPF",
							pfPercentage = 10
						};
					}
					else
					{
						data = new EmployeesSalaryStructure
						{
							pfType = "GPF",
							pfPercentage = null
						};
					}
				}
				else
				{
					data = new EmployeesSalaryStructure
					{
						pfType = null,
						pfPercentage = null
					};
				}
			}
			return Json(data);
		}

        public async Task<IActionResult> PartialSalary()
        {
            var data = new PartialSalaryViewModel
            {
                salaryPeriods = await salaryService.GetAllSalaryPeriod(),
                distinctPartialSalaryLogs = await salaryService.GetAllPartialSalaryLog()
            };
            return View(data);
        }
        public async Task<IActionResult> PRLEmployeeList()
        {
            var data = new PartialSalaryViewModel
            {
                //salaryPeriods = await salaryService.GetAllSalaryPeriod(),
                pRLEmployeeViewModels = await salaryService.GetAllPrlEmployees()
            };
            return View(data);
        }

        [HttpPost]
		public async Task<IActionResult> ProcessPartialSalary(PartialSalaryViewModel model)
		{
			await salaryService.ProcessPartialSalary(Convert.ToInt32(model.salaryPeriod1), Convert.ToInt32(model.type1), 0);
			var employees = await salaryService.GetAllEmployeesByType(Convert.ToInt32(model.salaryPeriod1), Convert.ToInt32(model.type1));
			foreach (var item in employees)
			{
				await salaryService.ProcessIndividualEmployeeSalaryForLongReport(Convert.ToInt32(model.salaryPeriod1), Convert.ToInt32(item.employeeInfoId));
			}
			return RedirectToAction(nameof(PartialSalary));
		}

		[HttpPost]
		public async Task<IActionResult> SavePartialSalary(PartialSalaryViewModel model)
		{
			var totalDays = (Convert.ToDateTime(model.toDate) - Convert.ToDateTime(model.fromDate).Date).Days;
			await salaryService.SavePartialSalary(Convert.ToInt32(model.type), Convert.ToInt32(model.employeeID), Convert.ToInt32(model.salaryPeriodId), model.fromDate.ToString(), model.toDate.ToString(), totalDays + 1);

			return RedirectToAction(nameof(PartialSalary));
		}

		public async Task<IActionResult> DeletePartialSalary(int empId, int periodId)
		{
			var data = await salaryService.DeletePartialSalary(empId, periodId);
			return Json(true);
		}

		public async Task<IActionResult> PartialSalaryList()
		{
			var data = new PartialSalaryViewModel
			{
				distinctPartialSalaryLogs = await salaryService.GetAllPartialSalaryLog()
			};
			return View(data);
		}

		public async Task<IActionResult> CarMaintenance()
		{
			var model = new CarMaintenanceViewModel
			{
				salaryPeriods = await salaryService.GetAllSalaryPeriod(),
				carMaintenances = await salaryService.GetAllCarMaintenance()
			};
			return View(model);
		}
		public async Task<IActionResult> DeleteCarMaintenance(int id)
		{
			var data = await salaryService.DeleteCarMaintenance(id);
			if (data == 1)
			{
				return Json(true);
			}
			else
			{
				return Json(false);
			}
		}

		public async Task<IActionResult> GetEmployeeInfoById(int id)
		{
			var data = await salaryService.GetEmployeeInfoById(id);
			return Json(data.FirstOrDefault());
		}

		[HttpPost]
		public async Task<IActionResult> SaveCarMaintenance(CarMaintenanceViewModel model)
		{
			var data = new CarMaintenance
			{
				Id = Convert.ToInt32(model.Id),
				employeeInfoId = model.employeeInfoId,
				salaryPeriodId = model.salaryPeriodId,
				effectiveDate = model.effectiveDate,
				amount = model.amount
			};
			await salaryService.SaveCarMaintenance(data);

			return RedirectToAction(nameof(CarMaintenance));
		}

		public async Task<IActionResult> MobileAllowance()
		{
			var model = new MobileAllowanceViewModel
			{
				salaryPeriods = await salaryService.GetAllSalaryPeriod(),
				mobileAllowances = await salaryService.GetAllMobileAllowanceStructure()
			};
			return View(model);
		}
		public async Task<IActionResult> DeleteMobileAllowance(int id)
		{
			var data = await salaryService.DeleteMobileAllowanceStructure(id);
			if (data == 1)
			{
				return Json(true);
			}
			else
			{
				return Json(false);
			}
		}
		[HttpPost]
		public async Task<IActionResult> SaveMobileAllowance(MobileAllowanceViewModel model)
		{
			var data = new MobileAllowanceStructure
			{
				Id = Convert.ToInt32(model.Id),
				employeeInfoId = model.employeeInfoId,
				salaryPeriodId = model.salaryPeriodId,
				effectiveDate = model.effectiveDate,
				amount = model.amount
			};
			await salaryService.SaveMobileAllowanceStructure(data);

			return RedirectToAction(nameof(MobileAllowance));
		}

		public async Task<IActionResult> GetLoansByEmpCodeAndType(string code, string type)
		{
			var model = new EmployeesSalaryStructureViewModel
			{
				staffLoanHBA = await salaryService.GetLoansByEmpCodeAndType(code, type)
			};
			return Json(model);
		}

		public async Task<ActionResult> PayrollIndexList(int? employeeInfoId)
		{
			ViewBag.employeeInfoId = employeeInfoId;
			var empInSalaryStruc = await salaryService.GetIndividualEmployeeFromSalaryStructure();

			var empWithSalary = new List<EmpWithSalaryStructure>();

			foreach (var item in empInSalaryStruc)
			{
				empWithSalary.Add(new EmpWithSalaryStructure
				{
					employeeInfo = item,
					employeesSalaryStructures = await salaryService.GetSalaryStructureByEmp(item.Id)
				});
			}
			EmployeesSalaryStructureViewModel model = new EmployeesSalaryStructureViewModel
			{
				//employeesSalaryStructuresList = await salaryService.GetAllEmployeesSalaryStructure(),
				empWithSalaryStructures = empWithSalary,
				salaryHeads = await salaryService.GetAllSalaryHead(),
				salaryPeriods = await salaryService.GetAllSalaryPeriod()
			};
			return View(model);
		}
		public async Task<ActionResult> PayrollIndexListApi(int? branchId, int? departmentId, int? designationId)
		{
			if (branchId == null)
			{
				branchId = 0;
			};
			if (departmentId == null)
			{
				departmentId = 0;
			};
			if (designationId == null)
			{
				designationId = 0;
			};

			var empInSalaryStruc = await salaryService.GetIndividualEmployeeFromSalaryStructure(Convert.ToInt32(branchId), Convert.ToInt32(departmentId), Convert.ToInt32(designationId));

			var empWithSalary = new List<EmpWithSalaryStructure>();

			foreach (var item in empInSalaryStruc)
			{
				empWithSalary.Add(new EmpWithSalaryStructure
				{
					employeeInfo = item,
					employeesSalaryStructures = await salaryService.GetSalaryStructureByEmp(item.Id),
					department = await personalInfoService.GetAllDepartmentById((int)item.departmentId)
				});
			}
			EmployeesSalaryStructureViewModel model = new EmployeesSalaryStructureViewModel
			{
				empWithSalaryStructures = empWithSalary,
				salaryHeads = await salaryService.GetAllSalaryHead(),
				salaryPeriods = await salaryService.GetAllSalaryPeriod()
			};
			return Json(model);
		}
		public async Task<ActionResult> EmployeeSalaryStructureList(int? employeeInfoId)
		{
			EmployeesSalaryStructureViewModel model = new EmployeesSalaryStructureViewModel
			{
				employeesSalaryStructuresList = await salaryService.GetEmployeesSalaryStructureByEmpId(Convert.ToInt32(employeeInfoId)),
				salaryGradesList = await salaryService.GetAllSalaryGrade(),
				salaryPeriod = await salaryService.GetSalaryPeriodmax()
			};
			if (model.employeesSalaryStructure == null) model.employeesSalaryStructure = new EmployeesSalaryStructure();
			ViewBag.employeeInfoId = employeeInfoId;
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Index([FromForm] EmployeesSalaryStructureViewModel model)
		{
			if (!ModelState.IsValid)
			{
				var errors = ModelState.Select(x => x.Value.Errors)
						   .Where(y => y.Count > 0)
						   .ToList();

				model.employeesSalaryStructuresList = await salaryService.GetEmployeesSalaryStructureByEmpId(model.employeeInfoId);
				model.salaryGradesList = await salaryService.GetAllSalaryGrade();
				return View(model);
			}

			IEnumerable<SalaryHead> lstSalaryHead = await salaryService.GetAllSalaryHead();

			var head = await salaryService.GetEmployeesSalaryStructureByEmpId(model.employeeInfoId);
			if (head.Count() > 0)
			{
				await salaryService.SaveStructureHistory(model.employeeInfoId, HttpContext.User.Identity.Name);
				await salaryService.DeleteEmployeesSalaryStructureByempId(Convert.ToInt32(model.employeeInfoId));
			}

			foreach (var item in model.salaryStrustureList)
			{
                try
                {
                    var isSalaryHead = lstSalaryHead.Where(x => x.Id == item.headId).FirstOrDefault();
                    if (isSalaryHead != null)
                    {
						//var expiryDate = item.ex != null ? Convert.ToDateTime(item.ex) : null;
						DateTime? expire = null;
						if (item.ex != null)
						{
							expire = Convert.ToDateTime(item.ex);
						}
						EmployeesSalaryStructure data = new EmployeesSalaryStructure
                        {
                            Id = 0,
                            employeeInfoId = model.employeeInfoId,
                            salaryHeadId = isSalaryHead.Id,
                            amount = (decimal)item.amount,
                            isActive = item.status,
                            effectiveDate = model.effectiveDate,
							expiryDate = expire
						};
                        if (model.salarySlab == null)
                        {
                            data.salarySlabId = null;
                        }
                        else
                        {
                            data.salarySlabId = Convert.ToInt32(model.salarySlab);
                        }

                        if (isSalaryHead.Id == 149)
                        {
                            data.pfType = model.pfType;
                            data.pfPercentage = model.pfPercentage;
                        }
                        if (isSalaryHead.Id == 132)
                        {
                            data.pfType = model.deductionType;
                            data.pfPercentage = Convert.ToInt32(model.amntPerLakh);
                        }

						if (isSalaryHead.Id == 167)
						{
							data.pfType = model.deductionType;
							data.pfPercentage = Convert.ToInt32(model.amntPerLakh);
						}

						await salaryService.SaveEmployeesSalaryStructure(data);
                    }

                    if (item.headId == 119)
                    {
						personalInfoService.UpdateEmployeeBasic(model.employeeInfoId, item.amount);
					}
                }
                catch (Exception ex)
                {

                    throw;
                }
			}

			return Json("ok");

		}

		[HttpPost]
		public async Task<JsonResult> Editsalarystructure([FromForm] EditsalarystructureViewModel model)
		{
			try
			{
				await salaryService.UpdateStructureHistory(model.editsalarystructureId, HttpContext.User.Identity.Name);

				var masterId = await salaryService.EditEmployeesSalaryStructure(model.editsalarystructureId, model.headamount, model.salarystructureStatus);

				return Json(masterId);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public async Task<ActionResult> Edit(int? employeeInfoId)
		{
			EmployeesSalaryStructureViewModel model = new EmployeesSalaryStructureViewModel
			{
				employeesSalaryStructuresList = await salaryService.GetEmployeesSalaryStructureByEmpId(Convert.ToInt32(employeeInfoId)),
				salaryGradesList = await salaryService.GetAllSalaryGrade(),
				salaryPeriod = await salaryService.GetSalaryPeriodmax()


			};
			if (model.employeesSalaryStructure == null) model.employeesSalaryStructure = new EmployeesSalaryStructure();
			ViewBag.employeeInfoId = employeeInfoId;
			return View(model);
		}
		[HttpPost]
		public async Task<IActionResult> Edit([FromForm] EmployeesSalaryStructureViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.employeesSalaryStructuresList = await salaryService.GetEmployeesSalaryStructureByEmpId(model.employeeInfoId);
				model.salaryGradesList = await salaryService.GetAllSalaryGrade();
				return View(model);
			}

			IEnumerable<SalaryHead> lstSalaryHead = await salaryService.GetAllSalaryHead();

			var head = await salaryService.GetEmployeesSalaryStructureByEmpId(model.employeeInfoId);
			if (head.Count() > 0)
			{
				await salaryService.SaveStructureHistory(model.employeeInfoId, HttpContext.User.Identity.Name);
				await salaryService.DeleteEmployeesSalaryStructureByempId(Convert.ToInt32(model.employeeInfoId));
			}
			//foreach (SalaryHead Headdata in lstSalaryHead)
			//{
			//    decimal Amount = 0;
			//    var headinfo = await salaryService.GetSalaryGradePercentBysalaryHeadId(Convert.ToInt32(model.salaryGradeId), Headdata.Id);
			//    if (headinfo != null)
			//    {
			//        if (headinfo.salaryCalulationTypeId == 2)
			//        {
			//            Amount = model.amount * (Convert.ToDecimal(headinfo.percentAmount) / 100);
			//        }
			//        else if (headinfo.salaryCalulationTypeId == 1 || headinfo.salaryCalulationTypeId == 3)
			//        {
			//            Amount = Convert.ToDecimal(headinfo.percentAmount);
			//        }
			//    }

			//    EmployeesSalaryStructure data = new EmployeesSalaryStructure
			//    {
			//        Id = 0,
			//        employeeInfoId = model.employeeInfoId,
			//        salarySlabId = Convert.ToInt32(model.salarySlab),
			//        salaryHeadId = Headdata.Id,
			//        amount = Math.Ceiling(Amount),
			//        isActive = "Active",
			//        effectiveDate = model.effectiveDate
			//    };
			//    await salaryService.SaveEmployeesSalaryStructure(data);
			//}


			foreach (var item in model.salaryStrustureList)
			{
				var isSalaryHead = lstSalaryHead.Where(x => x.Id == item.headId).FirstOrDefault();
				if (isSalaryHead != null)
				{

					EmployeesSalaryStructure data = new EmployeesSalaryStructure
					{
						Id = 0,
						employeeInfoId = model.employeeInfoId,
						salarySlabId = Convert.ToInt32(model.salarySlab),
						salaryHeadId = isSalaryHead.Id,
						amount = Math.Ceiling((decimal)item.amount),
						isActive = item.status,
						effectiveDate = model.effectiveDate
					};
					await salaryService.SaveEmployeesSalaryStructure(data);
				}
			}

			return RedirectToAction("Index", "SalaryStructure", new { employeeInfoId = model.employeeInfoId, Area = "Payroll" });

		}
		[HttpGet]
		public async Task<IActionResult> GetSalarySlabBysalaryGradeId(int salaryGradeId)
		{
			return Json(await salaryService.GetSalarySlabBysalaryGradeId(salaryGradeId));
		}

		[HttpGet]
		public async Task<IActionResult> GetsalaryGrade()
		{
			return Json(await salaryService.GetAllSalaryGrade());
		}
		[HttpGet]
		public async Task<IActionResult> GetSalarySlabById(int Id)
		{
			return Json(await salaryService.GetSalarySlabById(Id));
		}
		[HttpGet]
		public async Task<IActionResult> GetSalarySlabByAmount(decimal amount)
		{
			return Json(await salaryService.GetSalarySlabByAmount(amount));
		}
		[HttpGet]
		public async Task<IActionResult> GetEmployeesSalaryStructureAdditionByEmpId(int employeeInfoId)
		{
			var data = await salaryService.GetEmployeesSalaryStructureByEmpId(employeeInfoId);
			return Json(data.Where(x => x.salaryHead.salaryHeadType == "Addition"));
		}

		[HttpGet]
		public async Task<IActionResult> GetSalaryslabByEmpId(int employeeInfoId)
		{
			var data = await salaryService.GetSalaryStructureByEmp(employeeInfoId);
			return Json(data);
		}


		[HttpGet]
		public async Task<IActionResult> GetEmployeesSalaryStructureDeductionByEmpId(int employeeInfoId)
		{
			var data = await salaryService.GetEmployeesSalaryStructureByEmpId(employeeInfoId);
			return Json(data.Where(x => x.salaryHead.salaryHeadType == "Deduction"));
		}
		[HttpGet]
		public async Task<JsonResult> GetEmployeesGrossAmountByEmpId(int employeeInfoId)
		{
			decimal GrossAmount = 0;
			var data = await salaryService.GetEmployeesSalaryStructureByEmpId(employeeInfoId);
			decimal additions = data.Where(x => x.salaryHead.salaryHeadType == "Addition" && x.isActive == "Active").Sum(x => x.amount);
			decimal deductions = data.Where(x => x.salaryHead.salaryHeadType == "Deduction" && x.isActive == "Active").Sum(x => x.amount);
			GrossAmount = additions - deductions;
			decimal summation = data.Sum(x => x.amount);

			var res = new
			{
				GrossAmount,
				summation
			};
			return Json(res);
		}

		[HttpGet]
		public async Task<IActionResult> GetEmpStructureByEmpId(int employeeInfoId)
		{
			var data = await salaryService.GetEmpStructureByEmpId(employeeInfoId);
			return Json(data);
		}

		


		[HttpGet]
		public async Task<IActionResult> GetEmployeesSalaryStructureBeforeProces(EmployeesSalaryStructureViewModel model)
		{

			IEnumerable<SalaryHead> lstSalaryHead = await salaryService.GetAllSalaryHeadForStruct();

			var head = await salaryService.GetEmployeesSalaryStructureByEmpId(model.employeeInfoId);
			if (head.Count() > 0)
			{
				//await salaryService.SaveStructureHistory(model.employeeInfoId, HttpContext.User.Identity.Name);
				// await salaryService.DeleteEmployeesSalaryStructureByempId(Convert.ToInt32(model.employeeInfoId));
			}
			List<EmployeesSalaryStructureModel> AdditonList = new List<EmployeesSalaryStructureModel>();
			List<EmployeesSalaryStructureModel> DeductionList = new List<EmployeesSalaryStructureModel>();

			foreach (SalaryHead Headdata in lstSalaryHead)
			{
				decimal Amount = 0;
				var headinfo = await salaryService.GetSalaryGradePercentBysalaryHeadId(Convert.ToInt32(model.salaryGradeId), Headdata.Id);
				if (headinfo != null)
				{
					if (headinfo.salaryCalulationTypeId == 2)
					{
						Amount = Convert.ToDecimal(model.amount) * (Convert.ToDecimal(headinfo.percentAmount) / 100);
					}
					else if (headinfo.salaryCalulationTypeId == 1 || headinfo.salaryCalulationTypeId == 3)
					{
						Amount = Convert.ToDecimal(headinfo.percentAmount);
					}
				}


				EmployeesSalaryStructureModel data = new EmployeesSalaryStructureModel
				{
					employeeInfoId = model.employeeInfoId,
					salarySlabId = Convert.ToInt32(model.salarySlab),
					effectiveDate = model.effectiveDate,
					salaryHeadId = Headdata.Id,
					salaryHead = Headdata,
					amount = Math.Ceiling(Amount),
					isActive = "Active",
					expiryDate = null
				};

				if (Headdata.salaryHeadType == "Addition")
				{
					AdditonList.Add(data);
				}
				else if (Headdata.salaryHeadType == "Deduction")
				{
					DeductionList.Add(data);
				}

				//result.Add(data);

			}
			decimal GrossAmount = 0;
			decimal additionAmount = AdditonList.Sum(x => x.amount);
			decimal deductionAmount = DeductionList.Sum(x => x.amount);
			GrossAmount = additionAmount - deductionAmount;

			var res = new
			{
				AdditonList,
				DeductionList,
				additionAmount,
				deductionAmount,
				GrossAmount
			};

			return Json(res);
		}
		#endregion

		#region Structure History
		public IActionResult StructureHistory()
		{
			EmployeesSalaryStructureViewModel model = new EmployeesSalaryStructureViewModel
			{

			};
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> GetStructureHistoryByEmpId(int empId)
		{

			return Json(await salaryService.GetStructureHistoryByEmpId(empId));
		}

		#endregion


		#region Cash Setup
		public async Task<ActionResult> CashSetup()
		{
			EmployeesCashSetupViewModel model = new EmployeesCashSetupViewModel
			{
				employeesCashSetups = await salaryService.GetAllEmployeesCashSetup()
			};
			return View(model);
		}

		[HttpPost]
		public async Task<JsonResult> CashSetup([FromForm] EmployeesCashSetupViewModel model)
		{
			int cashId = 0;

			var cashCheck = await salaryService.GetEmployeesCashSetupByEmployeeId(model.employeeInfoId);
			if (cashCheck.Count() > 0 && model.cashSetupId == 0)
			{
				cashId = 0;
			}
			else
			{
				EmployeesCashSetup data = new EmployeesCashSetup
				{
					Id = model.cashSetupId,
					employeeInfoId = model.employeeInfoId,
					bankAmount = model.bankAmount,
					cashAmount = model.cashAmount,
					walletAmount = model.walletAmount
				};

				cashId = await salaryService.SaveEmployeesCashSetup(data);
			}

			return Json(cashId);
		}

		[HttpPost]
		public async Task<JsonResult> DeleteEmployeesCashSetupById(int Id)
		{
			await salaryService.DeleteEmployeesCashSetupById(Id);
			return Json(true);
		}

		#endregion

		#region SalaryActivityProject
		public async Task<ActionResult> SalaryActivityPercent()
		{
			EmployeesSalaryActivityViewModel model = new EmployeesSalaryActivityViewModel
			{
				// employeeProjectActivities = await employeeProjectActivityService.GetEmployeeProjectActivity(),
				GetSalaryActivityPercents = await salaryService.GetsalaryActivityPercent()

			};

			return View(model);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SalaryActivityPercent([FromForm] EmployeesSalaryActivityViewModel model)
		{
			//if (!ModelState.IsValid)
			//{
			//    model.slabIncomeTaxes = await salaryService.GetEmployeesSalaryStructureByEmpId(model.taxYearId);
			//    model.salaryGradesList = await salaryService.GetAllSalaryGrade();               
			//    return View(model);
			//}

			SalaryActivityPercent data = new SalaryActivityPercent
			{
				Id = model.salaryActivityPercentId,

				employeeInfoId = model.employeeInfoId,
				employeeProjectActivityId = model.employeeProjectActivityId,
				Amount = model.Amount


			};
			await salaryService.SavesalaryActivityPercent(data);


			return RedirectToAction("SalaryActivityPercent", "SalaryStructure", new { Area = "Payroll" });


		}

		[HttpPost]
		public async Task<IActionResult> SaveSalaryStructure(EmployeesSalaryStructureViewModel model)
		{
			var data = new EmployeesSalaryStructure
			{
				Id = model.editsalarystructureDedId,
				employeeInfoId = model.employeeInfoId,
				salaryHeadId = model.salaryHeadId,
				salarySlabId = Convert.ToInt32(model.salarySlab),
				amount = Convert.ToDecimal(model.amount),
				isActive = model.isActive
			};
			var d = await salaryService.SaveEmployeesSalaryStructure(data);
			if (d == true)
			{
				return Json("Saved");
			}
			else
			{
				return Json("Failed");
			}
		}
		#endregion

		#region API Section

		[Route("global/api/GetAllEmployess")]
		[HttpGet]
		public async Task<IActionResult> GetAllEmployess()
		{
			return Json(await personalInfoService.GetEmployeeInfo());
		}
		[Route("global/api/GetAllEmployessLoan")]
		[HttpGet]
		public async Task<IActionResult> GetAllEmployessLoan()
		{
			return Json(await personalInfoService.GetEmployeeInfoLoan());
		}
		[Route("global/api/GetAllEmployessForLoan")]
		[HttpGet]
		public async Task<IActionResult> GetAllEmployessForLoan()
		{
			return Json(await personalInfoService.GetAllEmployessForLoan());
		}

		[Route("global/api/GetDesignationsByLoanPolicyId/{id}")]
		[HttpGet]
		public async Task<IActionResult> GetDesignationsByLoanPolicyId(int? id)
		{
			return Json(await personalInfoService.GetDesignationsByLoanPolicyId(id));
		}

		[Route("global/api/GetAllDesignations")]
		[HttpGet]
		public async Task<IActionResult> GetAllDesignations()
		{
			return Json(await personalInfoService.GetAllDesignation());
		}


		[Route("global/api/GetEmployessPhotoByIdStatus/{id}")]
		[HttpGet]
		public async Task<IActionResult> GetEmployessPhotoByIdStatus(int id)
		{
			return Json(await photographService.GetPhotographByEmpIdAndType(id, "profile"));
		}

		[Route("global/api/GetActiveAllEmployeeInfo")]
		[HttpGet]
		public async Task<IActionResult> GetActiveAllEmployeeInfo()
		{
			var data = await personalInfoService.GetActiveAllEmployeeInfo1();

            return Json(data);
		}

        [Route("global/api/GetEmployeeInfoByDesignation")]
        [HttpGet]
        public async Task<IActionResult> GetEmployeeInfoByDesignation()
        {
            return Json(await personalInfoService.GetEmployeeInfoByDesignation());
        }





        [Route("global/api/GetActiveEmployeeInfo")]
		[HttpGet]
		public async Task<IActionResult> GetActiveEmployeeInfo()
		{
            var username = HttpContext.User.Identity.Name;
            return Json(await personalInfoService.GetActiveEmployeeInfo(username));
		}



		[Route("global/api/GetActiveAllEmployeeInfoForFixation")]
		[HttpGet]
		public async Task<IActionResult> GetActiveAllEmployeeInfoForFixation()
		{
			return Json(await personalInfoService.GetActiveAllEmployeeInfoForFixation());
		}
		[Route("global/api/GetActiveEmployeeInfoWithPosting")]
		[HttpGet]
		public async Task<IActionResult> GetActiveEmployeeInfoWithPosting()
		{
			return Json(await personalInfoService.GetActiveEmployeeInfoWithPosting());
		}
		[Route("global/api/GetActiveAllEmployeeInfo1")]
		[HttpGet]
		public async Task<IActionResult> GetActiveAllEmployeeInfo1()
		{
			return Json(await personalInfoService.GetActiveAllEmployeeInfo1());
		}
		[Route("global/api/GetAllActiveEmployee")]
		[HttpGet]
		public async Task<IActionResult> GetAllActiveEmployee()
		{
			return Json(await personalInfoService.GetAllActiveEmployees());
		}
		[Route("global/api/GetAllOffices")]
		[HttpGet]
		public async Task<IActionResult> GetAllOffices()
		{
			return Json(await personalInfoService.GetAllOffices());
		}
		[Route("global/api/GetAllDivisions")]
		[HttpGet]
		public async Task<IActionResult> GetAllDivisions()
		{
			return Json(await personalInfoService.GetAllDivisions());
		}
		[Route("global/api/GetAllDepartments")]
		[HttpGet]
		public async Task<IActionResult> GetAllDepartments()
		{
			return Json(await personalInfoService.GetAllDepartments1());
		}
		[Route("global/api/GetActiveEmployeeAndPfMemberInfo")]
		[HttpGet]
		public async Task<IActionResult> GetActiveEmployeeAndPfMemberInfo()
		{
			return Json(await personalInfoService.GetActiveEmployeeAndPfMemberInfo());
		}

		[Route("global/api/GetAllBranch")]
		[HttpGet]
		public async Task<IActionResult> GetAllBranch()
		{
			return Json(await personalInfoService.GetAllBranch());
		}

		[Route("global/api/GetAllDesignation")]
		[HttpGet]
		public async Task<IActionResult> GetAllDesignation()
		{
			return Json(await personalInfoService.GetAllDesignation());
		}
		[Route("global/api/GetAllDepartment")]
		[HttpGet]
		public async Task<IActionResult> GetAllDepartment()
		{
			return Json(await personalInfoService.GetAllDepartment());
		}

		[Route("global/api/GetAllEmployessbyId/{Id}")]
		[HttpGet]
		public async Task<IActionResult> GetAllEmployessbyId(int Id)
		{
			return Json(await personalInfoService.GetEmployeeInfoById(Id));
		}

		[Route("global/api/GetAcitivyProject/{Id}")]
		[HttpGet]
		public async Task<IActionResult> GetAcitivyProject(int Id)
		{
			return Json(await employeeProjectActivityService.GetEmployeeProjectActivityByEmpId(Id));
		}
		#endregion

	}
}