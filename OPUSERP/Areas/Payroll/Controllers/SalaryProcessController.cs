using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OPUSERP.Areas.Payroll.Models;
using OPUSERP.Data.Entity;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.Payroll.Data.Entity.Salary;
using OPUSERP.Payroll.Services.PF.Interfaces;
using OPUSERP.Payroll.Services.Salary.Interfaces;
using OPUSERP.ProvidentFund.Data.Entity;
using OPUSERP.Utility.Models;

namespace OPUSERP.Areas.Payroll.Controllers
{
    [Area("HR")]
    [Authorize]
    public class SalaryProcessController : Controller
    {
        private readonly ISalaryService salaryService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly IPFService pFService;
        private readonly IERPCompanyService eRPCompanyService;
		private readonly UserManager<ApplicationUser> _userManager;
		public SalaryProcessController(ISalaryService salaryService, IPersonalInfoService personalInfoService, IPFService pFService, UserManager<ApplicationUser> _userManager, IERPCompanyService eRPCompanyService)
        {
            this.salaryService = salaryService;
            this.eRPCompanyService = eRPCompanyService;
			this._userManager = _userManager;
            this.personalInfoService = personalInfoService;
            this.pFService = pFService;
		}

        #region Salary Process
		public async Task<IActionResult> CheckLoans()
		{
			var data = new SalaryProcessViewModel
			{
				checkLoans = await salaryService.CheckLoans()
			};
			return Json(data);
		}

        public async Task<IActionResult> ResolveAllLoans()
        {
            var data = new SalaryProcessViewModel
            {
                resolveAllVms = await salaryService.ResolveStructureWithLoan()
            };
            return Json(data);
        }

        //Asad added on 08.06.2023
        public async Task<IActionResult> SetAllToSalaryStructure(int salaryPeriodId)
        {
            var data = new SalaryProcessViewModel
            {
                resolveAllVms = await salaryService.SetAllToSalaryStructure(salaryPeriodId)
            };
            return Json(data);
        }

        public async Task<IActionResult> ProcessPartialHouseRent(int periodId)
        {
            var data = await salaryService.ProcessPartialHouseRent(periodId);
            return Json(data);
        }


        public async Task<ActionResult> Index()
        {
            var companies = await eRPCompanyService.GetAllCompany();
            ViewBag.Company = companies.FirstOrDefault().companyName;

			await salaryService.UpdateSalaryStructureExpire();

            SalaryProcessViewModel model = new SalaryProcessViewModel
            {
                salaryPeriods = await salaryService.GetAllSalaryPeriod(),
                hrBranches = await salaryService.GetAllHrBranchs(),
                locations = await salaryService.GetAllZones()
            };
            return View(model);
        }
        
  
        public async Task<ActionResult> PeriodList(int id)
        {
            var companies = await eRPCompanyService.GetAllCompany();
            ViewBag.Company = companies.FirstOrDefault().companyName;

            SalaryProcessViewModel model = new SalaryProcessViewModel
            {
                salaryPeriods = await salaryService.GetSalaryPeriodByName(id)
            };
            return Json(model);
        }


		public async Task<IActionResult> HrBranchForSalaryProcess(int periodId)
		{
			var data = await salaryService.GetHrBranchForSalaryProcess(periodId);
			return Json(data);
		}
		public async Task<IActionResult> HrZonesForSalaryProcess(int periodId)
		{
			var data = await salaryService.GetHrZonesForSalaryProcess(periodId);
			return Json(data);
		}

		[HttpPost]
        public async Task<JsonResult> EMPSalaryProcess([FromForm] SalaryProcessViewModel model)
        {
            try
            {
				string userName = HttpContext.User.Identity.Name;
				var user = await _userManager.FindByNameAsync(userName);

				var count = 1;
                if (model.employeeInfoId != null && model.hrBranchId == 0)
                {
                     await salaryService.EmpSalaryProcess(Convert.ToInt32(model.salaryPeriodId), Convert.ToInt32(model.employeeInfoId));
                }
                else if (model.hrBranchId != 0 && model.employeeInfoId == null)
                {
                    await salaryService.BranchWiseEmpSalaryProcess(Convert.ToInt32(model.salaryPeriodId), Convert.ToInt32(model.hrBranchId));
                }
				else if (model.locationId != 0)
				{
					await salaryService.ZoneWiseEmpSalaryProcess(Convert.ToInt32(model.salaryPeriodId), Convert.ToInt32(model.locationId));
				}
				else
                {
                    var masterId = await salaryService.EmpSalaryProcess(Convert.ToInt32(model.salaryPeriodId), 0);
                    count = masterId.Count();
                }

                var remote = HttpContext.Connection.RemoteIpAddress;
                var local = HttpContext.Connection.LocalIpAddress;
                string userip = remote.ToString();
                string useripLocal = local.ToString();

                SalaryProcessLog data = new SalaryProcessLog
                {
                    Id = 0,
                    salaryPeriodId = model.salaryPeriodId,
                    processType = "Salary",
                    ipAddress = userip
                };
                await salaryService.SaveSalaryProcessLog(data);

                SalaryStatusLog logdata = new SalaryStatusLog
                {
                    Id = 0,
                    salaryPeriodId = model.salaryPeriodId,
                    statusType = "Salary Process",
                    ipAddress = userip
                };
                await salaryService.SaveSalaryStatusLog(logdata);

				//new lines
				var salaryPeriod = await salaryService.GetSalaryPeriodById1(Convert.ToInt32(model.salaryPeriodId));
				salaryPeriod.lockLabel = model.lockLabel;
                await salaryService.SaveSalaryPeriod(salaryPeriod);

                var employeeIds = new List<int>();
                if (model.hrBranchId != 0)
                {
                    employeeIds = await salaryService.GetBranchEmployeesWithSalaryStructure(Convert.ToInt32(model.hrBranchId));
                }
                else
                {
                    employeeIds = await salaryService.GetEmployeesWithSalaryStructure();
                }
                

                
              
           

                

                return Json(count);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult ProcessBackgroundTask(int periodId, int empId)
        {
            var data = salaryService.ProcessSalaryBackgroundTasks(periodId, empId);
            return RedirectToAction("Index");
        }

		public async Task<IActionResult> UpdateLockStatusBranchWise(int lockId)
		{
			var data = await salaryService.UpdateLockStatusBranchWise(lockId);
			return Json(data);
		}

		public async Task<IActionResult> BadgePeriodLock() {
			var data = new SalaryProcessViewModel
			{
				//hrBranches = await salaryService.GetAllHrBranchs(),
				//locations = await salaryService.GetAllZones(),
				salaryPeriods = await salaryService.GetAllSalaryPeriod(),
				//allLockSalary = await salaryService.GetAllSalaryLocksWithStatus()
			};
			return View(data);
		}

        public async Task<IActionResult> GetPartialHouseRent(int periodId)
        {
            var data = await salaryService.GetPartialHouseRent(periodId);
            return Json(data);
        }


        [HttpPost]
        public async Task<JsonResult> EMPSalaryReProcess([FromForm] SalaryProcessViewModel model)
        {
            try
            {
				string userName = HttpContext.User.Identity.Name;
				var user = await _userManager.FindByNameAsync(userName);


				var count = 1;
                if (model.employeeInfoId != null)
                {
                    await salaryService.EmpSalaryProcess(Convert.ToInt32(model.salaryPeriodId), Convert.ToInt32(model.employeeInfoId));
                }
                else
                {
                    var masterId = await salaryService.EmpSalaryProcess(Convert.ToInt32(model.salaryPeriodId), 0);
                    count = masterId.FirstOrDefault().employeeInfoId;
                }

                var remote = HttpContext.Connection.RemoteIpAddress;
                var local = HttpContext.Connection.LocalIpAddress;
                string userip = remote.ToString();
                string useripLocal = local.ToString();

                SalaryProcessLog data = new SalaryProcessLog
                {
                    Id = 0,
                    salaryPeriodId = model.salaryPeriodId,
                    processType = "Salary",
                    ipAddress = userip
                };
                await salaryService.SaveSalaryProcessLog(data);

                SalaryStatusLog logdata = new SalaryStatusLog
                {
                    Id = 0,
                    salaryPeriodId = model.salaryPeriodId,
                    statusType = "Salary ReProcess",
                    ipAddress = userip
                };
                await salaryService.SaveSalaryStatusLog(logdata);

				var salaryPeriod = await salaryService.GetSalaryPeriodById1(Convert.ToInt32(model.salaryPeriodId));
				salaryPeriod.lockLabel = model.lockLabel;
				if (model.lockLabel == 2)
				{
					salaryPeriod.madeBy = user.userId;
					salaryPeriod.madeDate = DateTime.Now;
				}
				await salaryService.SaveSalaryPeriod(salaryPeriod);

                return Json(count);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<ActionResult> EditSalaryPeriodForlockLabel(int Id, int lockLabel)
        {
            await salaryService.EditSalaryPeriodForlockLabel(Id, lockLabel);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetMonthlySalaryReportByPeriodId(int salaryPeriodId)
        {
            var data = await salaryService.GetMonthlySalaryReportByPeriodId(salaryPeriodId, 0, 0);
            return Json(data);
        }


        //Asad Added on 16.09.2023
        public async Task<IActionResult> CollectPayrollContributionByPeriodId(int salaryPeriodId)
        {
            string employeeCode = HttpContext.User.Identity.Name;
            var result = await salaryService.CollectPayrollContributionByPeriodId(salaryPeriodId, employeeCode);

            string message = string.Empty;
            string status = string.Empty;

            if (result != null)
            {
                message = result.Message;
                status = result.Status;
            }
            else
            {
                message = "Unable Collect Contribution from Payroll.";
                status = "NOK";
            }

            return new OkObjectResult(new ResponseObject { Status = status, Message = message });
        }

        //
        public async Task<IActionResult> GetPayrollContributionByPeriodId(int salaryPeriodId)
        {
            var data = await salaryService.GetPayrollContributionByPeriodId(salaryPeriodId);
            return Json(data);
        }

        public async Task<ActionResult> PFEmiCollection()
        {
            SalaryProcessViewModel model = new SalaryProcessViewModel
            {
                salaryPeriods = await salaryService.GetAllSalaryPeriod()//,
                //ContributionViewModels = await salaryService.GetPayrollContributionByPeriodId
            };

            return View(model);
        }

        //public async Task<IActionResult> CollectPayrollLoanEmiByPeriodId(int salaryPeriodId)
        //{
        //    string employeeCode = HttpContext.User.Identity.Name;
        //    var result = await salaryService.CollectPayrollLoanEmiByPeriodId(salaryPeriodId, employeeCode);

        //    string message = string.Empty;
        //    string status = string.Empty;

        //    if (result != null)
        //    {
        //        message = result.Message;
        //        status = result.Status;
        //    }
        //    else
        //    {
        //        message = "Unable to Collect Loan EMI from Payroll.";
        //        status = "NOK";
        //    }

        //    return new OkObjectResult(new ResponseObject { Status = status, Message = message });
        //}
        //Asad Added On 30.09.2023
        //public async Task<IActionResult> GetPayrollLoanEmiByPeriodId(int salaryPeriodId)
        //{
        //    var data = await salaryService.GetPayrollLoanEmiByPeriodId(salaryPeriodId);
        //    return Json(data);
        //}


        //public async Task<IActionResult> GenerateVoucherOnPayrollLoanEmiByPeriodId(int salaryPeriodId)
        //{
        //    string employeeCode = HttpContext.User.Identity.Name;
        //    var result = await salaryService.GenerateVoucherOnPayrollLoanEmiByPeriodId(salaryPeriodId, employeeCode);

        //    string message = string.Empty;
        //    string status = string.Empty;

        //    if (result != null)
        //    {
        //        message = result.Message;
        //        status = result.Status;
        //    }
        //    else
        //    {
        //        message = "Unable to generate voucher.";
        //        status = "NOK";
        //    }

        //    return new OkObjectResult(new ResponseObject { Status = status, Message = message });
        //}

        public async Task<IActionResult> GetProcessedData(string year, string month)
		{
			var data = await salaryService.GetProcessedDataByYearAndMonth(year, month);
			return Json(data);
		}

        [HttpPost]
        public async Task<JsonResult> SaveSalaryComments([FromForm] ProcessSalaryRemarksViewModel model)
        { 
            try
            {
                await salaryService.DeleteProcessSalaryRemarks(model.empId, model.salperId);
                ProcessSalaryRemarks data = new ProcessSalaryRemarks
                {
                    Id = 0,
                    employeeInfoId = model.empId,
                    salaryPeriodId = model.salperId,
                    comments = model.comments,
                    proposedAmount=model.proposedAmount
                };
                int DocumentId = await salaryService.SaveProcessSalaryRemarks(data);
                return Json(DocumentId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteProcessSalaryRemarks(int? empId, int? salperId)
        {
            await salaryService.DeleteProcessSalaryRemarks(empId, salperId);
            return Json(true);
        }        

        [HttpPost]
        public async Task<JsonResult> SaveSalaryStatusLog([FromForm] SalaryStatusLogViewModel model)
        {
            try
            {
				string userName = HttpContext.User.Identity.Name;
				var user = await _userManager.FindByNameAsync(userName);

				var local = HttpContext.Connection.LocalIpAddress;                
                string useripLocal = local.ToString();

                SalaryStatusLog data = new SalaryStatusLog
                {
                    Id = 0,
                    salaryPeriodId = model.salaryPeriodLoadId,                   
                    statusType = model.statusType,
                    remarks=model.remarks,
                    ipAddress= useripLocal
                };
                int DocumentId = await salaryService.SaveSalaryStatusLog(data);
                //await salaryService.EditSalaryPeriodForlockLabel(Convert.ToInt32(model.salaryPeriodLoadId), Convert.ToInt32(model.draftFinalId));

				var salaryPeriod = await salaryService.GetSalaryPeriodById1(Convert.ToInt32(model.salaryPeriodLoadId));
				salaryPeriod.lockLabel = model.lockLabel;
				if (model.lockLabel == 2)
				{
					salaryPeriod.madeBy = user.userId;
					salaryPeriod.madeDate = DateTime.Now;
				}
				if (model.lockLabel == 3)
				{
					salaryPeriod.checkedBy = user.userId;
					salaryPeriod.checkedDate = DateTime.Now;
				}
				if (model.lockLabel == 4)
				{
					salaryPeriod.approvedBy = user.userId;
					salaryPeriod.approvedDate = DateTime.Now;
				}
				if (model.lockLabel == 5)
				{
					salaryPeriod.disbursedBy = user.userId;
					salaryPeriod.disbursedDate = DateTime.Now;
				}
				await salaryService.SaveSalaryPeriod(salaryPeriod);
                if (model.lockLabel == 5)
                {
                    salaryService.LoanDeductionByPeriodId((int)model.salaryPeriodLoadId);
                }
				
                return Json(DocumentId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<JsonResult> SaveSalaryPeriodLock([FromForm] SalaryStatusLogViewModel model)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                var user = await _userManager.FindByNameAsync(userName);

                var local = HttpContext.Connection.LocalIpAddress;
                string useripLocal = local.ToString();

                SalaryStatusLog data = new SalaryStatusLog
                {
                    Id = 0,
                    salaryPeriodId = model.periodId,
                    statusType = model.statusType,
                    remarks = model.remarks,
                    ipAddress = useripLocal
                };
                int DocumentId = await salaryService.SaveSalaryStatusLog(data);

                var salaryPeriod = await salaryService.GetSalaryPeriodById1(Convert.ToInt32(model.periodId));
                salaryPeriod.lockLabel = model.lockLabel;
                if (model.lockLabel == 6)
                {
                    salaryPeriod.lockBy = userName;
                    salaryPeriod.lockDate =  Convert.ToDateTime(model.PaymentDate); //DateTime.Now;
                    salaryPeriod.disbursedDate = Convert.ToDateTime(model.PaymentDate); //DateTime.Now;

                }

				await salaryService.SaveSalaryPeriod(salaryPeriod);//lockLabel == 6 Salary Period Lock
                return Json(DocumentId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public async Task<ActionResult> SalaryDisburse(int userId)
        //{
        //	string userName = HttpContext.User.Identity.Name;
        //	var user = await _userManager.FindByNameAsync(userName);

        //	var companies = await eRPCompanyService.GetAllCompany();
        //	ViewBag.Company = companies.FirstOrDefault().companyName;

        //	SalaryProcessViewModel model = new SalaryProcessViewModel
        //	{
        //		salaryPeriods = await salaryService.GetAllSalaryDisburse(user.userId)
        //	};
        //	return View(model);
        //}

        //public async Task<ActionResult> SalaryApprove(int userId, int lockLabel)
        //{
        //	string userName = HttpContext.User.Identity.Name;
        //	var user = await _userManager.FindByNameAsync(userName);

        //	var companies = await eRPCompanyService.GetAllCompany();
        //	ViewBag.Company = companies.FirstOrDefault().companyName;

        //	SalaryProcessViewModel model = new SalaryProcessViewModel
        //	{
        //		salaryPeriods = await salaryService.GetAllSalaryPeriodApproved(user.userId, 3)
        //	};
        //	return View(model);
        //}

        public async Task<ActionResult> SalaryDisbursePending()
		{
			var companies = await eRPCompanyService.GetAllCompany();
			ViewBag.Company = companies.FirstOrDefault().companyName;

			SalaryProcessViewModel model = new SalaryProcessViewModel
			{
				salaryPeriods = await salaryService.GetAllSalaryPeriod()
			};
			return View(model);
		}

		public async Task<ActionResult> SalaryOnLoad()
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);

			var companies = await eRPCompanyService.GetAllCompany();
			ViewBag.Company = companies.FirstOrDefault().companyName;

			SalaryProcessViewModel model = new SalaryProcessViewModel
			{
				salaryPeriods = await salaryService.GetAllSalaryPeriodByUserId(user.userId, "made"),

			};
			return View(model);
		}

		#endregion

		#region Process Log List

		public async Task<ActionResult> SalaryProcessLog()
        {            
            ProcessSalaryRemarksViewModel model = new ProcessSalaryRemarksViewModel
            {
                salaryProcessLogs = await salaryService.GetAllSalaryProcessLog()
            };
            return View(model);
        }

        #endregion

        #region Status Log List

        public async Task<ActionResult> SalaryStatusLog()
        {
            SalaryStatusLogViewModel model = new SalaryStatusLogViewModel
            {
                salaryPeriods = await salaryService.GetAllSalaryPeriod()
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetSalaryStatusLogByPeriodId(int salaryPeriodId)
        {
            var data = await salaryService.GetSalaryStatusLogByPeriodId(salaryPeriodId);
            return Json(data);
        }

        #endregion

        #region Salary Ongoing List

        public async Task<ActionResult> SalaryOngoing()
        {
            var companies = await eRPCompanyService.GetAllCompany();
            ViewBag.Company = companies.FirstOrDefault().companyName;

            SalaryProcessViewModel model = new SalaryProcessViewModel
            {
                salaryPeriods = await salaryService.GetAllSalaryPeriod()
            };
            return View(model);
        }
        public async Task<ActionResult> SalaryReturns()
        {
            var companies = await eRPCompanyService.GetAllCompany();
            ViewBag.Company = companies.FirstOrDefault().companyName;

            SalaryProcessViewModel model = new SalaryProcessViewModel
            {
                salaryPeriods = await salaryService.GetAllSalaryPeriod()
            };
            return View(model);
        }
         public async Task<ActionResult> SalaryCheckerReturns()
        {
            var companies = await eRPCompanyService.GetAllCompany();
            ViewBag.Company = companies.FirstOrDefault().companyName;

            SalaryProcessViewModel model = new SalaryProcessViewModel
            {
                salaryPeriods = await salaryService.GetAllSalaryPeriod()
            };
            return View(model);
        }

         public async Task<ActionResult> SalaryApproverReturns()
        {
            var companies = await eRPCompanyService.GetAllCompany();
            ViewBag.Company = companies.FirstOrDefault().companyName;

            SalaryProcessViewModel model = new SalaryProcessViewModel
            {
                salaryPeriods = await salaryService.GetAllSalaryPeriod()
            };
            return View(model);
        }



        public async Task<ActionResult> CheckerOngoing()
        {
            var companies = await eRPCompanyService.GetAllCompany();
            ViewBag.Company = companies.FirstOrDefault().companyName;

            SalaryProcessViewModel model = new SalaryProcessViewModel
            {
                salaryPeriods = await salaryService.GetAllSalaryPeriod()
            };
            return View(model);
        }


        public async Task<ActionResult> CheckerChecked()
        {
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);

			var companies = await eRPCompanyService.GetAllCompany();
			ViewBag.Company = companies.FirstOrDefault().companyName;

			SalaryProcessViewModel model = new SalaryProcessViewModel
			{
				salaryPeriods = await salaryService.GetAllSalaryPeriodByUserId(user.userId, "checked"),

			};
			return View(model);
		}


        public async Task<ActionResult> ApproverOngoing()
        {
            var companies = await eRPCompanyService.GetAllCompany();
            ViewBag.Company = companies.FirstOrDefault().companyName;

            SalaryProcessViewModel model = new SalaryProcessViewModel
            {
                salaryPeriods = await salaryService.GetAllSalaryPeriod()
            };
            return View(model);
        }

        #endregion

        #region Salary Approve List

        public async Task<ActionResult> SalaryApprove()
        {
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);

			var companies = await eRPCompanyService.GetAllCompany();
			ViewBag.Company = companies.FirstOrDefault().companyName;

			SalaryProcessViewModel model = new SalaryProcessViewModel
			{
				salaryPeriods = await salaryService.GetAllSalaryPeriodByUserId(user.userId, "approved"),

			};
			return View(model);
		}

        #endregion

        #region Salary Disburse List

        public async Task<ActionResult> SalaryDisburse()
        {
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);

			var companies = await eRPCompanyService.GetAllCompany();
			ViewBag.Company = companies.FirstOrDefault().companyName;

			SalaryProcessViewModel model = new SalaryProcessViewModel
			{
				salaryPeriods = await salaryService.GetAllSalaryPeriodByUserId(user.userId, "disbursed"),

			};
			return View(model);
		}

        #endregion

        #region Period Lock
        public async Task<ActionResult> SalarySheetLock()
        {
            SalaryProcessViewModel model = new SalaryProcessViewModel
            {
                salaryPeriods = await salaryService.GetAllSalaryPeriod()

            };

            return View(model);
        }
        [HttpGet]
        public async Task<ActionResult> EditSalaryPeriodForSalarySheetLock(int Id, int lockLabel)
        {
            await salaryService.SetSalaryPeriodLock(Id, lockLabel,HttpContext.User.Identity.Name);
            return RedirectToAction("SalarySheetLock");

        }
        public async Task<ActionResult> SalaryPeriodLock()
        {
            SalaryProcessViewModel model = new SalaryProcessViewModel
            {
                salaryPeriods = await salaryService.GetAllSalaryPeriod(),
                salaryStatusLogs = await salaryService.GetLockStatusLogs()
            };

            return View(model);
        }

		public async Task<IActionResult> BranchWiseLock() {
			var data = new SalaryProcessViewModel
			{
				hrBranches = await salaryService.GetAllHrBranchs(),
				locations = await salaryService.GetAllZones(),
				salaryPeriods = await salaryService.GetAllSalaryPeriod(),
				allLockSalary = await salaryService.GetAllSalaryLocksWithStatus()
			};
			return View(data);
		}

		public async Task<IActionResult> UpdateBadgeLock(int periodId, string type, int status)
		{
			var data = await salaryService.UpdateBadgeLock(periodId, type, status);
			return Json(data);
		}

		public async Task<IActionResult> BadgeLockType(int periodId, string type)
		{
			var data = await salaryService.GetAllBadgeLock(periodId, type);
			return Json(data);
		}

		#endregion

		#region Period UnLock
		public async Task<ActionResult> PeriodUnLock()
        {
            SalaryProcessViewModel model = new SalaryProcessViewModel
            {
                salaryPeriods = await salaryService.GetAllSalaryPeriod()
            };

            return View(model);
        }
        public async Task<ActionResult> SalarySheetUnLock()
        {
            SalaryProcessViewModel model = new SalaryProcessViewModel
            {
                salaryPeriods = await salaryService.GetAllSalaryPeriod()
            };

            return View(model);
        }
        [HttpGet]
        public async Task<ActionResult> SalarySheetUnLock(int Id, int lockLabel)
        {
            await salaryService.SetSalaryPeriodLock(Id, lockLabel, HttpContext.User.Identity.Name);
            return RedirectToAction("SalarySheetUnLock");

        }
        [HttpGet]
        public async Task<ActionResult> SalaryPeriodUnLock(int Id, int lockLabel)
        {
            await salaryService.SetSalaryPeriodLock(Id, lockLabel, HttpContext.User.Identity.Name);
            return RedirectToAction("PeriodUnLock");

        }

        #endregion

        #region Wages Process

        public async Task<ActionResult> WagesIndex()
        {
            SalaryProcessViewModel model = new SalaryProcessViewModel
            {
                salaryPeriods = await salaryService.GetAllSalaryPeriod()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> WagesSalaryProcess([FromForm] SalaryProcessViewModel model)
        {
            try
            {
                var count = 1;
                if (model.employeeInfoId != null)
                {
                    await salaryService.WagesSalaryProcess(Convert.ToInt32(model.salaryPeriodId), Convert.ToInt32(model.employeeInfoId));
                }
                else
                {
                    var masterId = await salaryService.WagesSalaryProcess(Convert.ToInt32(model.salaryPeriodId), 0);
                    count = masterId.Count();
                }

                return Json(count);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region
        public async Task<ActionResult> pfContribution()
        {
            SalaryProcessViewModel model = new SalaryProcessViewModel
            {
                salaryPeriods = await salaryService.GetAllSalaryPeriod()//,
                //ContributionViewModels = await salaryService.GetPayrollContributionByPeriodId
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> pfContribution([FromForm] SalaryProcessViewModel model)
        {
            try
            {
                if (model.SalaryContributionID != null)
                {
                    for (int i = 0; i < model.SalaryContributionID.Length; i++)
                    {
                        var SalaryContribution = new OPUSERP.Payroll.Data.Entity.PF.PFSalaryContribution
                        {
                            Id = (int)model.SalaryContributionID[i],
                            employeeId = model.employeeId[i],
                            employeeCode = model.employeeCode[i],
                            year = model.year[i],
                            month = model.month[i],
                            pfOwn = model.own[i],
                            pfCompany = model.company[i],
                           
                        };
                       
                        await salaryService.SaveSalaryContribution(SalaryContribution);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction(nameof(pfContribution));
        }

        public async Task<ActionResult> pfContributionPreview()
        {
           
            SalaryProcessViewModel model = new SalaryProcessViewModel
            {
                pFSalaryContributions = await salaryService.GetAllpfContribution()
            };
            return View(model);
        }
        #endregion

        #region Upload
        public async Task<ActionResult> UploadSalaryStructure()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpsalaryExcell([FromForm] IFormFile formFile, string processDate)
        {

            if (formFile == null || formFile.Length <= 0)
            {
                return Json("formfile is empty");
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return Json("Not Support file extension");
            }
            int rownumber = 0;
            try
            {

                using (var stream = new MemoryStream())
                {
                    await formFile.CopyToAsync(stream);

                    //using (var package = new ExcelPackage(stream))
                    //{
                    //    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    //    var rowCount = worksheet.Dimension.Rows;
                    //    string processNOo = await grayManualStockInService.GetProcessNo();
                    //    GrayManualStockInFileInfo grayManualStockInFileInfo = new GrayManualStockInFileInfo
                    //    {
                    //        processNo = processNOo,
                    //        processDate = Convert.ToDateTime(processDate),
                    //        fileName = formFile.FileName,
                    //        statusId = 1,
                    //    };
                    //    int fileId = await grayManualStockInService.SaveGrayManualStockInFileInfo(grayManualStockInFileInfo);
                    //    var rollList = new List<GrayManualStockIn>();
                    //    for (int row = 3; row <= rowCount; row++)
                    //    {
                    //        rownumber = row;
                    //        string stockInDate = worksheet.Cells[row, 1].Value == null ? "" : worksheet.Cells[row, 1].Value.ToString().Trim();
                    //        string poNo = worksheet.Cells[row, 2].Value == null ? "" : worksheet.Cells[row, 2].Value.ToString().Trim();
                    //        string knitCardNo = worksheet.Cells[row, 3].Value == null ? "" : worksheet.Cells[row, 3].Value.ToString().Trim();
                    //        string subConNo = worksheet.Cells[row, 4].Value == null ? "" : worksheet.Cells[row, 4].Value.ToString().Trim();
                    //        string materialNo = worksheet.Cells[row, 5].Value == null ? "" : worksheet.Cells[row, 5].Value.ToString().Trim();
                    //        string rollNo = worksheet.Cells[row, 6].Value == null ? "" : worksheet.Cells[row, 6].Value.ToString().Trim();
                    //        decimal? qtyKg = worksheet.Cells[row, 7].Value == null ? 0 : Convert.ToDecimal(worksheet.Cells[row, 7].Value);
                    //        decimal? qtyMtr = worksheet.Cells[row, 8].Value == null ? 0 : Convert.ToDecimal(worksheet.Cells[row, 8].Value);
                    //        string rackLocation = worksheet.Cells[row, 9].Value == null ? "" : worksheet.Cells[row, 9].Value.ToString().Trim();
                    //        rollList.Add(new GrayManualStockIn
                    //        {
                    //            fileInfoId = fileId,
                    //            stockInDate = stockInDate,
                    //            poNo = poNo.Trim(),
                    //            knitCardNo = knitCardNo.Trim(),
                    //            subConNo = subConNo.Trim(),
                    //            materialNo = materialNo.Trim(),
                    //            rollNo = rollNo.Trim(),
                    //            qtyKg = qtyKg,
                    //            qtyMtr = qtyMtr,
                    //            rackLocation = rackLocation.Trim(),

                    //            statusId = 1
                    //        });
                    //    }
                    //    await grayManualStockInService.SaveGrayManualStockIn(rollList);
                    //}
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(rownumber);
                throw ex;
            }

            // add list to db ..  
            // here just read and return  

            return RedirectToAction(nameof(UpsalaryExcell));
        }
        #endregion

        #region PF

        //Asad Added On 30.09.2023
        public async Task<IActionResult> CollectPayrollLoanEmiByPeriodId(int salaryPeriodId)
        {
            string employeeCode = HttpContext.User.Identity.Name;
            var result = await salaryService.CollectPayrollLoanEmiByPeriodId(salaryPeriodId, employeeCode);

            string message = string.Empty;
            string status = string.Empty;

            if (result != null)
            {
                message = result.Message;
                status = result.Status;
            }
            else
            {
                message = "Unable to Collect Loan EMI from Payroll.";
                status = "NOK";
            }

            return new OkObjectResult(new ResponseObject { Status = status, Message = message });
        }

        //Asad Added On 30.09.2023
        public async Task<IActionResult> GetPayrollLoanEmiByPeriodId(int salaryPeriodId)
        {
            var data = await salaryService.GetPayrollLoanEmiByPeriodId(salaryPeriodId);
            return Json(data);
        }
        public async Task<IActionResult> GenerateVoucherOnPayrollLoanEmiByPeriodId(int salaryPeriodId)
        {
            string employeeCode = HttpContext.User.Identity.Name;
            var result = await salaryService.GenerateVoucherOnPayrollLoanEmiByPeriodId(salaryPeriodId, employeeCode);

            string message = string.Empty;
            string status = string.Empty;

            if (result != null)
            {
                message = result.Message;
                status = result.Status;
            }
            else
            {
                message = "Unable to generate voucher.";
                status = "NOK";
            }

            return new OkObjectResult(new ResponseObject { Status = status, Message = message });
        }

        #endregion
    }
}