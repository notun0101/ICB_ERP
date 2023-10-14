using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OPUSERP.Areas.Payroll.Models;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.Models;
using OPUSERP.Payroll.Data.Entity;
using OPUSERP.Payroll.Data.Entity.Loan;
using OPUSERP.Payroll.Services.PF.Interfaces;
using OPUSERP.Payroll.Services.Salary.Interfaces;
using OPUSERP.Utility.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Payroll.Controllers
{
    [Area("Payroll")]
    [Authorize]
    public class StaffLoanController : Controller
    {
        private IHostingEnvironment Environment;
        private IConfiguration Configuration;
        private readonly ISalaryService salaryService;
        private readonly IPFService pFService;
        private readonly IPhotographService photographService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly IEmployeeProjectActivityService employeeProjectActivityService;
        public StaffLoanController(IHostingEnvironment _environment, IConfiguration _configuration, ISalaryService salaryService, IPhotographService photographService, IPFService pFService, IPersonalInfoService personalInfoService, IEmployeeProjectActivityService employeeProjectActivityService)
        {
            Environment = _environment;
            Configuration = _configuration;
            this.salaryService = salaryService;
            this.photographService = photographService;
            this.personalInfoService = personalInfoService;
            this.employeeProjectActivityService = employeeProjectActivityService;
            this.pFService = pFService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new StaffLoanVm
            {
                staffLoans = await salaryService.GetAllStaffLoans()
            };
            return View(model);
        }

        public async Task<IActionResult> GetExpiryDateByLoanType(string loanType, DateTime? disburseDate, int employeeInfoId)
        {
            var data = DateTime.Now;

            if (loanType == "MCA")
            {
                data = DateTime.Now.AddYears(10);
            }
            else if (loanType == "CL")
            {
                data = DateTime.Now.AddYears(5);
            }
            else if (loanType == "HBA-B13" || loanType == "HBA-A13")
            {
                var prl = await salaryService.GetPRLDateByEmpId(employeeInfoId);
                if (prl != null)
                {
                    data = Convert.ToDateTime(prl);
                }
            }
            return Json(data);
        }


        public async Task<IActionResult> StaffLoanRecovery()
        {

            //Asad Added On 07.08.2023
            var model = new StaffLoanVm
            {
                staffLoanLogs = await salaryService.GetStaffLoanLog()
            };

			//Asad Commented On 07.08.2023
			//         var model = new StaffLoanVm
			//         {
			//             staffLoans = await salaryService.GetAllStaffLoans(),
			//             staffLoanLogs = await salaryService.GetAllStaffLoansLog()
			//};


			return View(model);
        }
		//Asad Added On 07.08.2023
		public async Task<IActionResult> GetStaffLoanLogByTrxNo(string trxNo)
		{

			var staffLoanLogs = await salaryService.GetStaffLoanLogByTrxNo(trxNo);
			return Json(staffLoanLogs);
		}
		
		public async Task<IActionResult> GetStaffLoanLogById(int id)
		{
			var data = await salaryService.GetStaffLoanLogById(id);
			return Json(data);
		}
		public async Task<IActionResult> GetAllStaffLoansByDate(string fromDate, string toDate)
        {
            //var data = await salaryService.GetAllStaffLoansByDate(Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate));
            var data = await salaryService.GetStaffLoansLogAllByDate(Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate));
            return Json(data);
        }

        
		[HttpPost]
		public async Task<IActionResult> UpdateLoanRecovery(StaffLoanVm model)
		{

			var recoverFrom = await salaryService.RecoverFromPrincipalOrInterest(Convert.ToInt32(model.staffLoanId));
			var lparticular = String.Empty;
			var loan = await salaryService.GetStaffLoanInfoById(Convert.ToInt32(model.staffLoanId));

			// New Code Added on 23.06.2023
			//lparticular = GetParticular(model.trxType);
		
			var data = new StaffLoanLog
			{
				Id = Convert.ToInt32(model.staffLoanLogId),
				staffLoanId = model.staffLoanId,
				effectiveDate = Convert.ToDateTime(model.loanDate).Date + Convert.ToDateTime(DateTime.Now).TimeOfDay,
				particular = lparticular,
				isActive = 1,
				status = 1,
				remarks = model.remarks,
				trxNo = model.trxNo,
				type = model.trxType,
			};

			//New Code Added on 23.06.2023
			data = SetDebitCredit(model, data, recoverFrom);
            		
			
			return RedirectToAction("StaffLoanRecovery");

		}


		private StaffLoanVm SetParticular(StaffLoanVm model, StaffLoanInfoViewModel loan)
        {                                    

			if (model.trxType == "Recovery")
			{
				model.Particular = "Loan Recovery";
			}
			if (model.trxType == "RecoveryBoth")
			{
				model.Particular = "Loan Recovery";
			}
			else if (model.trxType == "Disbursement")
			{
				model.Particular = "Loan Disbursement";
				model.staffLoanId = loan.loanId;
			}
			else if (model.trxType == "LegalADoc")
			{
				model.Particular = "Legal And Documentation";
				model.staffLoanId = loan.loanId;
			}
			else if (model.trxType == "Waiver")
			{
				model.Particular = "Loan Waiver";
				model.staffLoanId = loan.loanId;
			}
			else if (model.trxType == "IntReverse")
			{
				model.Particular = "Interest Reverse";
			}
			else if (model.trxType == "IntReverseCr")
			{
				model.Particular = "Interest Reverse (Cr)";
			}
			else if (model.trxType == "PRefund")
			{
				model.Particular = "Principal Refund";
			}
			else if (model.trxType == "IRefund")
			{
				model.Particular = "Interest Refund";
			}
			else if (model.trxType == "PriorerAdj")
			{
				model.Particular = "Priorer Adjustment";
			}
			else if (model.trxType == "Interest")
			{
				model.Particular = "Interest Charge Transfer";
			}
			return model;
        }

        private StaffLoanLog SetDebitCredit(StaffLoanVm model, StaffLoanLog data, CheckLoanDedVm recoverFrom)
        {
			if (model.trxType == "Disbursement")
			{
				data.debit = model.amount;
				data.credit = null;
				data.principal = model.amount;
			}
			else if (model.trxType == "LegalADoc")
			{
				data.debit = model.amount;
				data.credit = null;
				data.charge = model.amount;
			}
			else if (model.trxType == "IntReverse")
			{
				data.debit = null;
				data.credit = model.amount;
				data.interest = model.amount;
				data.principal = null;
			}
			else if (model.trxType == "IntReverseCr")
			{
				data.debit = null;
				data.credit = model.amount;
				data.interest = model.amount;
				data.principal = null;
			}
			else if (model.trxType == "Waiver")
			{
				data.debit = null;
				data.credit = model.amount;
				data.interest = model.amount;
				data.principal = null;
			}
			else if (model.trxType == "PRefund")
			{
				data.debit = model.amount;
				data.credit = null;
				data.interest = null;
				data.principal = model.amount;
			}
			else if (model.trxType == "IRefund")
			{
				data.debit = model.amount;
				data.credit = null;
				data.interest = model.amount;
				data.principal = null;
			}
			else if (model.trxType == "PriorerAdj")
			{
				data.debit = null;
				data.credit = model.amount;
				data.interest = model.amount;
				data.principal = null;
			}
			else if (model.trxType == "Interest") //Asad added on 15.06.2023
			{
				data.debit = model.amount;
				data.credit = null;
				data.interest = model.amount;
				data.principal = null;
			}

			//Added by Asad on 06.07.2023
			else if (model.trxType == "RecoveryBoth")
			{
				if (recoverFrom.Principal > 0 && recoverFrom.Principal >= model.amount)
				{
					data.debit = null;
					data.credit = model.amount;
					data.interest = null;
					data.principal = model.amount;
					data.remarks = "Principal";
				}

				if (recoverFrom.Principal > 0 && recoverFrom.Principal < model.amount)
				{
					data.debit = null;
					data.credit = recoverFrom.Principal;
					data.interest = null;
					data.principal = recoverFrom.Principal;
					data.remarks = "Principal";
				}
			}

			else
			{
				data.credit = model.amount;
				data.debit = null;
			}

			if (recoverFrom.Principal >= model.amount && model.trxType == "Recovery")
			{
				data.principal = model.amount;
			}
			else if (recoverFrom.Interest >= model.amount && model.trxType == "Recovery")
			{
				data.interest = model.amount;
			}
			else if (recoverFrom.Interest >= model.amount && model.trxType == "Waiver")
			{
				data.interest = model.amount;
			}
			else if (recoverFrom.Charge >= model.amount && model.trxType == "Recovery")
			{
				data.charge = model.amount;
			}
			else
			{

			}

			return data;
        }

        //Old: Commented on 24.06.2023 and Uncommented on 03.08.2023
        [HttpPost]
        public async Task<IActionResult> LoanRecovery(StaffLoanVm model)
        {
			//GetStaffLoanLogByTransactionNo

			CheckLoanDedVm recoverFrom = new CheckLoanDedVm();
			StaffLoanInfoViewModel loan = new StaffLoanInfoViewModel();

			var logs = new List<StaffLoanLog>();

			DateTime effectiveDate = DateTime.Now;
			bool isUpdateOnSameDate = false;

            if (model.staffLoanLogId != null && model.staffLoanLogId > 0)
			{
				logs = await salaryService.GetStaffLoanLogByTransactionNo(model.trxNo);

                effectiveDate = logs.Min(x => Convert.ToDateTime(x.effectiveDate));
				var date = effectiveDate.Date;

				if(model.loanDate == date) 
				{
                    isUpdateOnSameDate = true;
                }
				
                recoverFrom = await salaryService.RecoverFromPrincipalOrInterestExpSelf(model.trxNo, Convert.ToInt32(model.staffLoanId));
				loan = await salaryService.GetStaffLoanInfoExpSelfById(model.trxNo, Convert.ToInt32(model.staffLoanId));
			}
			else
			{
				recoverFrom = await salaryService.RecoverFromPrincipalOrInterest(Convert.ToInt32(model.staffLoanId));
				loan = await salaryService.GetStaffLoanInfoById(Convert.ToInt32(model.staffLoanId));
			}

			if (recoverFrom != null)
			{
				var principal = recoverFrom.Principal == null ? 0 : recoverFrom.Principal;
				var interest = recoverFrom.Interest == null ? 0 : recoverFrom.Interest;
				var charge = recoverFrom.Charge == null ? 0 : recoverFrom.Charge;

				if (principal + interest + charge < model.amount)
				{
					return new OkObjectResult(new ResponseObject { Status = "NOK", Message = "You are trying to recover more amount than needed." });
				}
			}

			//Today: 11.08.2013
			//var logs = new List<StaffLoanLog>();
			//if (model.staffLoanLogId != null && model.staffLoanLogId > 0)
			//{
			//	logs = await salaryService.GetStaffLoanLogByTransactionNo(model.trxNo);				
			//}
			//foreach (var item in logs)
			//{
			//	await salaryService.DeleteStaffLoanLogById(item.Id);
			//}


			#region Old To New: Asad Commented On 17.08.2023
			//Today: 11.08.2013
			//var recoverFrom = await salaryService.RecoverFromPrincipalOrInterest(Convert.ToInt32(model.staffLoanId));
			//var loan = await salaryService.GetStaffLoanInfoById(Convert.ToInt32(model.staffLoanId));
			var lparticular = String.Empty;


			if (model.trxType == "Recovery")
			{
				lparticular = "Loan Recovery";
			}
			if (model.trxType == "RecoveryBoth")
			{
				lparticular = "Loan Recovery";
			}
			else if (model.trxType == "Disbursement")
			{
				lparticular = "Loan Disbursement";
				model.staffLoanId = loan.loanId;
			}
			else if (model.trxType == "LegalADoc")
			{
				lparticular = "Legal And Documentation";
				model.staffLoanId = loan.loanId;
			}
			else if (model.trxType == "Waiver")
			{
				lparticular = "Loan Waiver";
				model.staffLoanId = loan.loanId;
			}
			else if (model.trxType == "IntReverse")
			{
				lparticular = "Interest Reverse";
			}
			else if (model.trxType == "IntReverseCr")
			{
				lparticular = "Interest Reverse (Cr)";
			}
			else if (model.trxType == "PRefund")
			{
				lparticular = "Principal Refund";
			}
			else if (model.trxType == "IRefund")
			{
				lparticular = "Interest Refund";
			}
			else if (model.trxType == "PriorerAdj")
			{
				lparticular = "Priorer Adjustment";
			}
			else if (model.trxType == "Interest")
			{
				lparticular = "Interest Charge Transfer";
			}
						
			var data = new StaffLoanLog
			{
				//New
				Id = 0,
				//Old
				//Id = Convert.ToInt32(model.staffLoanLogId),
				staffLoanId = model.staffLoanId,
                effectiveDate = isUpdateOnSameDate ? effectiveDate.AddMilliseconds(1) : Convert.ToDateTime(model.loanDate).Date + Convert.ToDateTime(DateTime.Now).TimeOfDay,
                //effectiveDate = Convert.ToDateTime(model.loanDate).Date + Convert.ToDateTime(DateTime.Now).TimeOfDay,
                particular = lparticular,
				isActive = 1,
				status = 1,
				remarks = model.remarks,
				trxNo = model.trxNo,
				type = model.trxType,
			};

			if (model.trxType == "Disbursement")
			{
				data.debit = model.amount;
				data.credit = null;
				data.principal = model.amount;
			}
			else if (model.trxType == "LegalADoc")
			{
				data.debit = model.amount;
				data.credit = null;
				data.charge = model.amount;
			}
			else if (model.trxType == "IntReverse")
			{
				data.debit = null;
				data.credit = model.amount;
				data.interest = model.amount;
				data.principal = null;
			}
			else if (model.trxType == "IntReverseCr")
			{
				data.debit = null;
				data.credit = model.amount;
				data.interest = model.amount;
				data.principal = null;
			}
			else if (model.trxType == "Waiver")
			{
				data.debit = null;
				data.credit = model.amount;
				data.interest = model.amount;
				data.principal = null;
			}
			else if (model.trxType == "PRefund")
			{
				data.debit = model.amount;
				data.credit = null;
				data.interest = null;
				data.principal = model.amount;
			}
			else if (model.trxType == "IRefund")
			{
				data.debit = model.amount;
				data.credit = null;
				data.interest = model.amount;
				data.principal = null;
			}
			else if (model.trxType == "PriorerAdj")
			{
				data.debit = null;
				data.credit = model.amount;
				data.interest = model.amount;
				data.principal = null;
			}
			else if (model.trxType == "Interest") //Asad added on 15.06.2023
			{
				data.debit = model.amount;
				data.credit = null;
				data.interest = model.amount;
				data.principal = null;
			}

			//Added by Asad on 06.07.2023
			else if (model.trxType == "RecoveryBoth")
			{
				if (recoverFrom.Principal > 0 && recoverFrom.Principal >= model.amount)
				{
					data.debit = null;
					data.credit = model.amount;
					data.interest = null;
					data.principal = model.amount;
					//data.remarks = model.remarks; //"Principal";
				}

				if (recoverFrom.Principal > 0 && recoverFrom.Principal < model.amount)
				{
					data.debit = null;
					data.credit = recoverFrom.Principal;
					data.interest = null;
					data.principal = recoverFrom.Principal;
					//data.remarks = model.remarks; // "Principal";
				}
			}

			else
			{
				data.credit = model.amount;
				data.debit = null;
			}

			if (recoverFrom.Principal >= model.amount && model.trxType == "Recovery")
			{
				data.principal = model.amount;
			}
			else if (recoverFrom.Interest >= model.amount && model.trxType == "Recovery")
			{
				data.interest = model.amount;
			}
			else if (recoverFrom.Interest >= model.amount && model.trxType == "Waiver")
			{
				data.interest = model.amount;
			}
			else if (recoverFrom.Charge >= model.amount && model.trxType == "Recovery")
			{
				data.charge = model.amount;
			}
			else
			{

			}

			if (model.trxType == "RecoveryBoth")
			{
				if (recoverFrom.Principal > 0)
				{
					await salaryService.SaveStaffLoanLog(data);
				}
			}
			else
			{
				await salaryService.SaveStaffLoanLog(data);
			}


			//Added by Asad on 06.07.2023
			if (model.trxType == "RecoveryBoth" && recoverFrom.Principal < model.amount)
			{

				decimal? remainingAmount = model.amount - recoverFrom.Principal;
				decimal? interest = 0;
				decimal? charge = 0;

				if (remainingAmount > recoverFrom.Interest)
				{
					interest = recoverFrom.Interest;
				}
				else
				{
					interest = remainingAmount;
				}

				if (remainingAmount >= recoverFrom.Interest + recoverFrom.Charge)
				{
					charge = recoverFrom.Charge;
				}
				if (remainingAmount > recoverFrom.Interest && remainingAmount < recoverFrom.Interest + recoverFrom.Charge)
				{
					charge = remainingAmount - recoverFrom.Interest;
				}

				if (interest > 0)
				{
					var data1 = new StaffLoanLog
					{
						//New
						Id = 0,
						//Old
						//Id = Convert.ToInt32(model.staffLoanLogId),
						staffLoanId = model.staffLoanId,
                        effectiveDate = isUpdateOnSameDate ? effectiveDate.AddMilliseconds(2) : Convert.ToDateTime(model.loanDate).Date + Convert.ToDateTime(DateTime.Now).TimeOfDay,

                        //effectiveDate = Convert.ToDateTime(model.loanDate).Date + Convert.ToDateTime(DateTime.Now).TimeOfDay,
                        particular = lparticular,
						isActive = 1,
						status = 1,
						//remarks = "Interest",
						trxNo = model.trxNo,
						type = model.trxType,
					};
					data1.debit = null;
					data1.credit = interest;
					data1.interest = interest;					
					data1.principal = null;

					await salaryService.SaveStaffLoanLog(data1);
				}
				if (charge > 0)
				{
					var data1 = new StaffLoanLog
					{
						//New
						Id = 0,
						//Old
						//Id = Convert.ToInt32(model.staffLoanLogId),
						staffLoanId = model.staffLoanId,
                        effectiveDate = isUpdateOnSameDate ? effectiveDate.AddMilliseconds(3) : Convert.ToDateTime(model.loanDate).Date + Convert.ToDateTime(DateTime.Now).TimeOfDay,

                        //effectiveDate = Convert.ToDateTime(model.loanDate).Date + Convert.ToDateTime(DateTime.Now).TimeOfDay,
                        particular = lparticular,
						isActive = 1,
						status = 1,
						//remarks = "Interest Charged",
						trxNo = model.trxNo,
						type = model.trxType,
					};
					data1.debit = null;
					data1.credit = charge;
					data1.charge = charge;
					data1.principal = null;

					await salaryService.SaveStaffLoanLog(data1);
				}

				
			}
			#endregion

			foreach (var item in logs)
			{
				await salaryService.DeleteStaffLoanLogById(item.Id);
			}


			#region Asad Added for Edit
			/*
			model = SetParticular(model, loan);
            var data = new StaffLoanLog();
			data = new StaffLoanLog
			{
				//Id = Convert.ToInt32(model.staffLoanLogId),
				staffLoanId = model.staffLoanId,
				effectiveDate = Convert.ToDateTime(model.loanDate).Date + Convert.ToDateTime(DateTime.Now).TimeOfDay,
				particular = model.Particular,
				isActive = 1,
				status = 1,
				remarks = model.remarks,
				trxNo = model.trxNo,
				type = model.trxType,
			};
			data = SetDebitCredit(model, data, recoverFrom);	
			if (model.trxType != "RecoveryBoth")
			{
				if (logs.Count() > 0)
				{
					data.Id = Convert.ToInt32(model.staffLoanLogId);
				}
				else
				{
					data.Id = 0;
				}
				await salaryService.SaveStaffLoanLog(data);
			}
			await SaveRecoveryTransaction(model, data, recoverFrom, logs);
			*/
			#endregion

			#region Old: Asad Commented On 12.08.2023

			//var recoverFrom = await salaryService.RecoverFromPrincipalOrInterest(Convert.ToInt32(model.staffLoanId));
			//var lparticular = String.Empty;
			//var loan = await salaryService.GetStaffLoanInfoById(Convert.ToInt32(model.staffLoanId));

			//if (model.trxType == "Recovery")
			//{
			//    lparticular = "Loan Recovery";
			//}
			//if (model.trxType == "RecoveryBoth")
			//{
			//    lparticular = "Loan Recovery";
			//}
			//else if (model.trxType == "Disbursement")
			//{
			//    lparticular = "Loan Disbursement";
			//    model.staffLoanId = loan.loanId;
			//}
			//else if (model.trxType == "LegalADoc")
			//{
			//    lparticular = "Legal And Documentation";
			//    model.staffLoanId = loan.loanId;
			//}
			//else if (model.trxType == "Waiver")
			//{
			//    lparticular = "Loan Waiver";
			//    model.staffLoanId = loan.loanId;
			//}
			//else if (model.trxType == "IntReverse")
			//{
			//    lparticular = "Interest Reverse";
			//}
			//else if (model.trxType == "IntReverseCr")
			//{
			//    lparticular = "Interest Reverse (Cr)";
			//}
			//else if (model.trxType == "PRefund")
			//{
			//    lparticular = "Principal Refund";
			//}
			//else if (model.trxType == "IRefund")
			//{
			//    lparticular = "Interest Refund";
			//}
			//else if (model.trxType == "PriorerAdj")
			//{
			//    lparticular = "Priorer Adjustment";
			//}
			//else if (model.trxType == "Interest")
			//{
			//    lparticular = "Interest Charge Transfer";
			//}

			//var data = new StaffLoanLog
			//{
			//    Id = Convert.ToInt32(model.staffLoanLogId),
			//    staffLoanId = model.staffLoanId,
			//    effectiveDate = Convert.ToDateTime(model.loanDate).Date + Convert.ToDateTime(DateTime.Now).TimeOfDay,
			//    particular = lparticular,
			//    isActive = 1,
			//    status = 1,
			//    remarks = model.remarks,
			//    trxNo = model.trxNo,
			//    type = model.trxType,
			//};

			//if (model.trxType == "Disbursement")
			//{
			//    data.debit = model.amount;
			//    data.credit = null;
			//    data.principal = model.amount;
			//}
			//else if (model.trxType == "LegalADoc")
			//{
			//    data.debit = model.amount;
			//    data.credit = null;
			//    //data.principal = model.amount;
			//    data.charge = model.amount;
			//}
			//else if (model.trxType == "IntReverse")
			//{
			//    data.debit = null;
			//    data.credit = model.amount;
			//    data.interest = model.amount;
			//    data.principal = null;
			//}
			//else if (model.trxType == "IntReverseCr")
			//{
			//    data.debit = null;
			//    data.credit = model.amount;
			//    data.interest = model.amount;
			//    data.principal = null;
			//}
			//else if (model.trxType == "Waiver")
			//{
			//    data.debit = null;
			//    data.credit = model.amount;
			//    data.interest = model.amount;
			//    data.principal = null;
			//}
			//else if (model.trxType == "PRefund")
			//{
			//    data.debit = model.amount;
			//    data.credit = null;
			//    data.interest = null;
			//    data.principal = model.amount;
			//}
			//else if (model.trxType == "IRefund")
			//{
			//    data.debit = model.amount;
			//    data.credit = null;
			//    data.interest = model.amount;
			//    data.principal = null;
			//}
			//else if (model.trxType == "PriorerAdj")
			//{
			//    data.debit = null;
			//    data.credit = model.amount;
			//    data.interest = model.amount;
			//    data.principal = null;
			//}
			//else if (model.trxType == "Interest") //Asad added on 15.06.2023
			//{
			//    data.debit = model.amount;
			//    data.credit = null;
			//    data.interest = model.amount;
			//    data.principal = null;
			//}

			////Added by Asad on 06.07.2023
			//else if (model.trxType == "RecoveryBoth")
			//{
			//    if (recoverFrom.Principal > 0 && recoverFrom.Principal >= model.amount)
			//    {
			//        data.debit = null;
			//        data.credit = model.amount;
			//        data.interest = null;
			//        data.principal = model.amount;
			//        data.remarks = "Principal";
			//    }

			//    if (recoverFrom.Principal > 0 && recoverFrom.Principal < model.amount)
			//    {
			//        data.debit = null;
			//        data.credit = recoverFrom.Principal;
			//        data.interest = null;
			//        data.principal = recoverFrom.Principal;
			//        data.remarks = "Principal";
			//    }
			//}

			//else
			//{
			//    data.credit = model.amount;
			//    data.debit = null;
			//}

			//if (recoverFrom.Principal >= model.amount && model.trxType == "Recovery")
			//{
			//    data.principal = model.amount;
			//}
			//else if (recoverFrom.Interest >= model.amount && model.trxType == "Recovery")
			//{
			//    data.interest = model.amount;
			//}
			//else if (recoverFrom.Interest >= model.amount && model.trxType == "Waiver")
			//{
			//    data.interest = model.amount;
			//}
			//else if (recoverFrom.Charge >= model.amount && model.trxType == "Recovery")
			//{
			//    data.charge = model.amount;
			//}
			//else
			//{

			//}

			//if (model.trxType == "RecoveryBoth")
			//{
			//    if (recoverFrom.Principal > 0)
			//    {
			//        await salaryService.SaveStaffLoanLog(data);
			//    }
			//}
			//else
			//{
			//    await salaryService.SaveStaffLoanLog(data);
			//}


			////Added by Asad on 06.07.2023
			//if (model.trxType == "RecoveryBoth" && recoverFrom.Principal < model.amount)
			//{

			//    decimal? remainingAmount = model.amount - recoverFrom.Principal;
			//    decimal? interest = 0;
			//    decimal? charge = 0;

			//    if (remainingAmount > recoverFrom.Interest)
			//    {
			//        interest = recoverFrom.Interest;
			//    }
			//    else
			//    {
			//        interest = remainingAmount;
			//    }

			//    if (remainingAmount >= recoverFrom.Interest + recoverFrom.Charge)
			//    {
			//        charge = recoverFrom.Charge;
			//    }
			//    if (remainingAmount > recoverFrom.Interest && remainingAmount < recoverFrom.Interest + recoverFrom.Charge)
			//    {
			//        charge = remainingAmount - recoverFrom.Interest;
			//    }

			//    if (interest > 0)
			//    {
			//        var data1 = new StaffLoanLog
			//        {
			//            Id = Convert.ToInt32(model.staffLoanLogId),
			//            staffLoanId = model.staffLoanId,
			//            effectiveDate = Convert.ToDateTime(model.loanDate).Date + Convert.ToDateTime(DateTime.Now).TimeOfDay,
			//            particular = lparticular,
			//            isActive = 1,
			//            status = 1,
			//            remarks = "Interest",
			//            trxNo = model.trxNo,
			//            type = model.trxType,
			//        };
			//        data1.debit = null;
			//        data1.credit = interest;
			//        data1.interest = interest;
			//        //data1.credit = model.amount - recoverFrom.Principal;
			//        //data1.interest = model.amount - recoverFrom.Principal;
			//        data1.principal = null;

			//        await salaryService.SaveStaffLoanLog(data1);
			//    }
			//    if (charge > 0)
			//    {
			//        var data1 = new StaffLoanLog
			//        {
			//            Id = Convert.ToInt32(model.staffLoanLogId),
			//            staffLoanId = model.staffLoanId,
			//            effectiveDate = Convert.ToDateTime(model.loanDate).Date + Convert.ToDateTime(DateTime.Now).TimeOfDay,
			//            particular = lparticular,
			//            isActive = 1,
			//            status = 1,
			//            remarks = "Interest Charged",
			//            trxNo = model.trxNo,
			//            type = model.trxType,
			//        };
			//        data1.debit = null;
			//        data1.credit = charge;
			//        data1.charge = charge;
			//        data1.principal = null;

			//        await salaryService.SaveStaffLoanLog(data1);
			//    }
			//}
			#endregion


			return RedirectToAction("StaffLoanRecovery");

        }

		private async Task<int> SaveRecoveryTransaction(StaffLoanVm model, StaffLoanLog data, CheckLoanDedVm recoverFrom, List<StaffLoanLog> logs)
		{
			int principalAddCount = 0;
			int interestAddCount = 0;
			int chargeAddCount = 0;

			if (model.trxType == "RecoveryBoth")
			{
				if (recoverFrom.Principal > 0)
				{
					//principalAddCount = principalAddCount + 1;

					//if (logs.Where(x => x.principal > 0).Count()>0)
     //               {
					//	data.Id = (int)logs.Where(x => x.principal > 0).SingleOrDefault()?.Id;
					//}
     //               else
     //               {
     //                   data.Id = 0;
					//}
					await salaryService.SaveStaffLoanLog(data);
				}
				//else //If Previous Record Exist
				//{
				//	if (logs.Where(x => x.principal > 0).Count() > 0)
				//	{                        
				//		var id = (int)logs.Where(x => x.principal > 0).SingleOrDefault()?.Id;
				//		await salaryService.DeleteStaffLoanLogsByLoanId(id);
				//	}
				//}
			}

			if (model.trxType == "RecoveryBoth" && recoverFrom.Principal < model.amount)
			{
				decimal? remainingAmount = model.amount - recoverFrom.Principal;
				decimal? interest = 0;
				decimal? charge = 0;

				interestAddCount = interestAddCount + 1;

				if (remainingAmount > recoverFrom.Interest)
				{
					interest = recoverFrom.Interest;
				}
				else
				{
					interest = remainingAmount;
				}

				if (remainingAmount >= recoverFrom.Interest + recoverFrom.Charge)
				{
					charge = recoverFrom.Charge;
				}
				if (remainingAmount > recoverFrom.Interest && remainingAmount < recoverFrom.Interest + recoverFrom.Charge)
				{
					charge = remainingAmount - recoverFrom.Interest;
				}

				if (interest > 0)
				{
                    var data1 = new StaffLoanLog();

					data1 = new StaffLoanLog
					{
						staffLoanId = model.staffLoanId,
						effectiveDate = Convert.ToDateTime(model.loanDate).Date + Convert.ToDateTime(DateTime.Now).TimeOfDay,
						particular = model.Particular,
						isActive = 1,
						status = 1,
						remarks = "Interest",
						trxNo = model.trxNo,
						type = model.trxType,
					};

					//if (model.staffLoanLogId != null && model.staffLoanLogId > 0)
					//               {
					//                   data1 = new StaffLoanLog
					//                   {

					//	    //Id = Convert.ToInt32(model.staffLoanLogId),
					//	    staffLoanId = model.staffLoanId,
					//                       effectiveDate = Convert.ToDateTime(model.loanDate).Date + Convert.ToDateTime(DateTime.Now).TimeOfDay,
					//                       particular = model.Particular,
					//                       isActive = 1,
					//                       status = 1,
					//                       remarks = "Interest",
					//                       trxNo = model.trxNo,
					//                       type = model.trxType,
					//                   };
					//               }
					//               else
					//               {
					//	data1 = new StaffLoanLog
					//	{
					//		Id = 0, //Convert.ToInt32(model.staffLoanLogId),
					//		staffLoanId = model.staffLoanId,
					//		effectiveDate = Convert.ToDateTime(model.loanDate).Date + Convert.ToDateTime(DateTime.Now).TimeOfDay,
					//		particular = model.Particular,
					//		isActive = 1,
					//		status = 1,
					//		remarks = "Interest",
					//		trxNo = model.trxNo,
					//		type = model.trxType,
					//	};
					//}

					data1.debit = null;
					data1.credit = interest;
					data1.interest = interest;
					data1.principal = null;

					//if (logs.Where(x => x.interest > 0).Count() > 0)
					//{
					//	data1.Id = (int)logs.Where(x => x.interest > 0).SingleOrDefault()?.Id;
					//}
					//else
					//{
					//	data1.Id = 0;
					//}

					await salaryService.SaveStaffLoanLog(data1);
				}
				//else //If Previous Record Exist
				//{
				//	if (logs.Where(x => x.interest > 0).Count() > 0)
				//	{
				//		var id = (int)logs.Where(x => x.interest > 0).SingleOrDefault()?.Id;
				//		await salaryService.DeleteStaffLoanLogsByLoanId(id);
				//	}
				//}


				if (charge > 0)
				{
					chargeAddCount = chargeAddCount + 1;

					var data2 = new StaffLoanLog();

					data2 = new StaffLoanLog
					{
						//Id = Convert.ToInt32(model.staffLoanLogId),
						staffLoanId = model.staffLoanId,
						effectiveDate = Convert.ToDateTime(model.loanDate).Date + Convert.ToDateTime(DateTime.Now).TimeOfDay,
						particular = model.Particular,
						isActive = 1,
						status = 1,
						remarks = "Interest Charged",
						trxNo = model.trxNo,
						type = model.trxType,
					};

					//if (model.staffLoanLogId != null && model.staffLoanLogId > 0)
					//               {
					//                   data1 = new StaffLoanLog
					//                   {
					//                       //Id = Convert.ToInt32(model.staffLoanLogId),
					//                       staffLoanId = model.staffLoanId,
					//                       effectiveDate = Convert.ToDateTime(model.loanDate).Date + Convert.ToDateTime(DateTime.Now).TimeOfDay,
					//                       particular = model.Particular,
					//                       isActive = 1,
					//                       status = 1,
					//                       remarks = "Interest Charged",
					//                       trxNo = model.trxNo,
					//                       type = model.trxType,
					//                   };
					//               }
					//               else
					//               {
					//	data1 = new StaffLoanLog
					//	{
					//		Id = 0, //Convert.ToInt32(model.staffLoanLogId),
					//		staffLoanId = model.staffLoanId,
					//		effectiveDate = Convert.ToDateTime(model.loanDate).Date + Convert.ToDateTime(DateTime.Now).TimeOfDay,
					//		particular = model.Particular,
					//		isActive = 1,
					//		status = 1,
					//		remarks = "Interest Charged",
					//		trxNo = model.trxNo,
					//		type = model.trxType,
					//	};
					//}


					data2.debit = null;
					data2.credit = charge;
					data2.charge = charge;
					data2.principal = null;

					//if (logs.Where(x => x.charge > 0).Count() > 0)
					//{
					//	data2.Id = (int)logs.Where(x => x.charge > 0).SingleOrDefault()?.Id;
					//}
					//else
					//{
					//	data2.Id = 0;
					//}

					await salaryService.SaveStaffLoanLog(data2);
				}
				//            else //If Previous Recoed Exist
				//            {
				//	if (logs.Where(x => x.charge > 0).Count() > 0)
				//	{
				//		var id = (int)logs.Where(x => x.charge > 0).SingleOrDefault()?.Id;
				//		await salaryService.DeleteStaffLoanLogsByLoanId(id);
				//	}
				//}
			}

			foreach (var item in logs)
			{
				await salaryService.DeleteStaffLoanLogById(item.Id);
			}


			return 1;
		}

		public async Task<IActionResult> CheckRecovaryAmount(int staffLoanId)
        {
            var recoverFrom = await salaryService.RecoverFromPrincipalOrInterest(Convert.ToInt32(staffLoanId));
            return Json(recoverFrom);
        }
        public async Task<IActionResult> GetAvailableLoans(string empcode)
        {
            var data = await salaryService.GetAvailableLoans(empcode);
            return Json(data);
        }
        public async Task<IActionResult> GenerateLoanTransactionNo(string type, int loanId)
        {
            var loanName = await salaryService.GetLoanNameById(loanId);

            var data = await salaryService.GenerateLoanTransactionNo(type, loanName);
            return Json(data);
        }


        public async Task<IActionResult> ProcessLoan()
        {
            var model = new StaffLoanVm
            {
                salaryPeriods = await salaryService.GetAllSalaryPeriod()
            };
            return View(model);
        }

        public async Task<IActionResult> GetLoanNoByLoanId(int loanId)
        {
            var loan = await salaryService.GetStaffLoanInfoById(loanId);
            return Json(loan);
        }

        public async Task<IActionResult> ProcessLoanApi(string loanType, int salaryPeriodId, string empCode)
        {
            int result = 0;
            if (loanType == "MCA" || loanType == "MCAR" || loanType == "CL" || loanType == "HBA" || loanType == "HBA-B13" || loanType == "HBA-A13" || loanType == "BCA")
            {
                var data = new CBSProcessSp();

                if (loanType.Contains("HBA"))
                {
                    data = await salaryService.ProcessLoanByType("HBA-B13", salaryPeriodId, empCode);
                    data = await salaryService.ProcessLoanByType("HBA-A13", salaryPeriodId, empCode);
                }
                else
                {
                    data = await salaryService.ProcessLoanByType(loanType, salaryPeriodId, empCode);

                }
                result = Convert.ToInt32(data.status);
            }
            return Json(result);
        }

        public async Task<IActionResult> GetStaffLoanNo(string empCode, string loanType)
        {
            var loanDate = await salaryService.GetStaffLoanNo(empCode, loanType);
            return Json(loanDate.loanNo);
        }

        [HttpPost]
        public async Task<IActionResult> Index(StaffLoanVm model)
        {
            var employee = await personalInfoService.GetEmployeeInfoByEmpId(Convert.ToInt32(model.employeeInfoId));
            var loan = await salaryService.GetStaffLoanByEmpIdAndType(Convert.ToInt32(model.employeeInfoId), "HBA-A13");
            var loanId = 0;

            if (model.loanType == "HBA-A13" && loan != null && model.staffLoanId == 0)
            {
                var data = new StaffLoanLog
                {
                    Id = 0,
                    staffLoanId = loan.Id,
                    effectiveDate = model.loanDate,
                    particular = "Loan Disbursement",
                    principal = model.principal,
                    interest = model.interest,
                    charge = model.charge,
                    total = model.totalAmount,
                    isActive = 1,
                    status = 1,
                    debit = model.principal
                };
                await salaryService.SaveStaffLoanLog(data);
            }
            else
            {
                var staffLoan = new StaffLoan
                {
                    Id = Convert.ToInt32(model.staffLoanId),
                    employeeInfoId = model.employeeInfoId,
                    empCode = employee.employeeCode,
                    empName = employee.nameEnglish,
                    designationId = employee.lastDesignationId,
                    LoanNo = model.loanNo,
                    NewLoanNo = model.loanAc,
                    principalAmount = model.principal,
                    interestRate = 100,
                    interestAmount = model.interest,
                    chargeAmount = model.charge,
                    totalAmount = model.totalAmount,
                    totalDisbursement = model.totalDisbursement,
                    loanDate = model.loanDate,
                    name = employee.nameEnglish,
                    designationEn = employee.lastDesignation?.designationName,
                    loanType = model.loanType,
                    loanNewType = model.loanNewType,
                    sanctionAmount = model.sanctionAmount,
                    sanctionDate = model.sanctionDate,
                    firstInstallmentDate = model.firstInstallmentDate,
                    loanTenure = model.loanTenure,
                    expiryDate = model.expiryDate,
                    DisburseDate = model.DisburseDate
                };

                loanId = await salaryService.SaveStaffLoan(staffLoan);
            }


            var staffLoanLogId = 0;
            if (model.staffLoanId > 0)
            {
                var staffLoanLog = await salaryService.GetStaffLoanLogByLoanId(loanId);
                staffLoanLogId = staffLoanLog.Id;
            }

            if (model.staffLoanId == 0)
            {
                var data = new StaffLoanLog
                {
                    Id = staffLoanLogId,
                    staffLoanId = loanId,
                    effectiveDate = model.loanDate,
                    particular = "Opening Balance",
                    principal = model.principal,
                    interest = model.interest,
                    charge = model.charge,
                    total = model.totalAmount,
                    isActive = 1,
                    status = 1
                };
                await salaryService.SaveStaffLoanLog(data);
            }
            else
            {
                var data = await salaryService.GetStaffLoanLogByLoanId(Convert.ToInt32(model.staffLoanId));
                data.principal = model.principal;
                data.interest = model.interest;
                data.charge = model.charge;
                data.total = model.totalAmount;

                await salaryService.SaveStaffLoanLog(data);
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> SaveStaffLoan(StaffLoanVm model)
        {
            var employee = await personalInfoService.GetEmployeeInfoByEmpId(Convert.ToInt32(model.employeeInfoId));
            

            
            var loan = await salaryService.GetStaffLoanByEmpIdAndType(Convert.ToInt32(model.employeeInfoId), model.loanType);
            
            if(loan != null && (loan.loanType == "MCA" || loan.loanType == "CL"))
            {
               var LoanPaid = await salaryService.IsFullPaid(loan.Id);
               
                if(LoanPaid != null && LoanPaid.IsPaid == 1)
               {
                   await salaryService.CloseFullPaidStaffLoan(loan.Id);
                   loan = null;
               }
            }
            

            var loanId = 0;

            int? salaryHeadId = null;

            if (model.loanType == "CL")
            {
                salaryHeadId = 123; //Computer Loan
            }
            if (model.loanType == "MCA")
            {
                salaryHeadId = 139; //Motor Cycle Loan
            }

            if (loan == null)
            {
                var staffLoan = new StaffLoan
                {
                    Id = Convert.ToInt32(model.staffLoanId),
                    employeeInfoId = model.employeeInfoId,
                    empCode = employee.employeeCode,
                    empName = employee.nameEnglish,
                    designationId = employee.lastDesignationId,
                    LoanNo = model.loanNo,
                    NewLoanNo = model.loanAc,
                    principalAmount = model.principal,
                    interestAmount = model.interest,
                    chargeAmount = model.charge,
                    totalAmount = model.totalAmount,
                    totalDisbursement = model.totalDisbursement,
                    loanDate = Convert.ToDateTime(model.loanDate).Date + Convert.ToDateTime(DateTime.Now).TimeOfDay,
                    name = employee.nameEnglish,
                    designationEn = employee.lastDesignation?.designationName,
                    loanType = model.loanType,
                    interestRate = Convert.ToDecimal(0.04),
                    salaryHeadId = salaryHeadId,
                    firstInstallmentDate = model.firstInstallmentDate
                };
                if (staffLoan.loanType == "HBA-A13" || staffLoan.loanType == "HBA-B13")
                {
                    staffLoan.InstallmentType = model.InstallmentType > 0 ? model.InstallmentType : null;
                    staffLoan.DeductionAmount = model.DeductionAmount > 0 ? model.DeductionAmount : null;

                    loanId = await salaryService.SaveStaffLoan(staffLoan);
                    await salaryService.UpdateHouseLoanDeductAmountBasedOnType(staffLoan);
                }
                else
                {
                    try
                    {
                        loanId = await salaryService.SaveStaffLoan(staffLoan);
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                    }
                }
            }
            else
            {                
                model.staffLoanId = loan.Id;

                var data = new StaffLoanLog
                {
                    Id = 0,
                    staffLoanId = loan.Id,
                    effectiveDate = Convert.ToDateTime(model.loanDate).Date + Convert.ToDateTime(DateTime.Now).TimeOfDay,
                    particular = "Loan Disbursement",
                    principal = model.principal,
                    interest = model.interest,
                    charge = model.charge,
                    total = model.totalAmount,
                    isActive = 1,
                    status = 1,
                    debit = model.principal
                };
                await salaryService.SaveStaffLoanLog(data);
            }


            var staffLoanLogId = 0;
            if (model.staffLoanId > 0 && loan == null)
            {
                var staffLoanLog = await salaryService.GetStaffLoanLogByLoanId(loanId);
                staffLoanLogId = staffLoanLog.Id;
            }

            if (model.staffLoanId == 0)
            {
                var data = new StaffLoanLog
                {
                    Id = staffLoanLogId,
                    staffLoanId = loanId,
                    effectiveDate = Convert.ToDateTime(model.loanDate).Date + Convert.ToDateTime(DateTime.Now).TimeOfDay,
                    particular = "Loan Disbursement",
                    principal = model.principal,
                    interest = model.interest,
                    charge = model.charge,
                    total = model.totalAmount,
                    isActive = 1,
                    interestProcessDate = Convert.ToDateTime(model.loanDate).AddDays(-1),
                    status = 1
                };
                await salaryService.SaveStaffLoanLog(data);
            }
            else
            {
                if (loan == null)
                {
                    var data = await salaryService.GetStaffLoanLogByLoanId(Convert.ToInt32(model.staffLoanId));
                    data.principal = model.principal;
                    data.interest = model.interest;
                    data.charge = model.charge;
                    data.total = model.totalAmount;
                    data.interestProcessDate = Convert.ToDateTime(model.loanDate).AddDays(-1);
                    await salaryService.SaveStaffLoanLog(data);
                }
            }
            return Json("ok");
        }

        [HttpPost]
        public async Task<IActionResult> SaveLoanDeductionType(StaffLoanVm model)
        {
            var loan = await salaryService.GetStaffLoanByEmpIdAndType(Convert.ToInt32(model.employeeInfoId), model.loanType);


            if (loan != null)
            {

                if (loan.loanType == "HBA-A13" || loan.loanType == "HBA-B13")
                {
                    loan.InstallmentType = model.InstallmentType > 0 ? model.InstallmentType : null;
                    loan.DeductionAmount = model.DeductionAmount > 0 ? model.DeductionAmount : null;
                    await salaryService.SaveStaffLoan(loan);
                    await salaryService.UpdateHouseLoanDeductAmountBasedOnType(loan);
                }
                return Json("ok");

            }
            return Json("Failed");
        }
        public async Task<IActionResult> DeleteStaffLoan(int id)
        {
            //await salaryService.DeleteStaffLoanLogsByLoanId(id);
            //var data = await salaryService.DeleteStaffLoanByLoanId(id);

            var data = await salaryService.DeleteStaffLoanMasterDetail(id);
            return Json(data.loanNo);
        }

        public async Task<IActionResult> FilterStaffLoan()
        {
            var model = new StaffLoanFilterVm
            {
                hrBranches = await salaryService.GetAllHrBranchs(),
                zones = await personalInfoService.GetAllZones()
            };
            return View(model);
        }

        public async Task<IActionResult> GetStaffLoansApi(int zoneId, int branchId, int empId, string loanType)
        {
            var data = await salaryService.FilterStaffLoan(branchId, zoneId, empId, loanType);
            return Json(data);
        }

        public async Task<IActionResult> UploadLoanData()
        {
            var model = new UploadLoanViewModel
            {
                uploadExcelLogs = await salaryService.GetAllExcelUploadLogsByType("StaffLoan")
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadLoanData(IFormFile postedFile)
        {
            var loans = new List<UploadLoanDataExcelFormat>();
            if (postedFile != null)
            {
                string fileName = $"{Environment.WebRootPath}\\UploadLoanExcel\\{postedFile.FileName}";
                using (FileStream fileStream = System.IO.File.Create(fileName))
                {
                    postedFile.CopyTo(fileStream);
                    fileStream.Flush();
                }
                loans = this.GetAllDataFromExcel(fileName);

            }

            foreach (var item in loans)
            {
                var emp = await personalInfoService.GetEmployeeInfoByCodeSp(item.EmpId);
                if (emp != null)
                {
                    if (item.EmpId != "EmpId" && emp.Id > 0 && (item.LoanType != "MCA" || item.LoanType != "MCAR" || item.LoanType != "HBA-B13" || item.LoanType != "HBA-A13" || item.LoanType != "CL"))
                    {
                        await salaryService.DeleteExistingStaffLoan(Convert.ToInt32(emp.Id), item.LoanType, item.CBSAc, item.LoanAc);

                        var staffLoan = new StaffLoan
                        {
                            Id = 0,
                            employeeInfoId = emp.Id,
                            empCode = emp.employeeCode,
                            empName = emp.nameEnglish,
                            postingPlace = emp.placeOfPosting,
                            designationId = emp.lastDesignationId,
                            salaryPeriodId = null,
                            LoanNo = item.CBSAc,
                            NewLoanNo = item.LoanAc,
                            principalAmount = Convert.ToDecimal(item.Principal),
                            interestAmount = Convert.ToDecimal(item.Interest),
                            chargeAmount = Convert.ToDecimal(item.Charge),
                            totalAmount = Convert.ToDecimal(item.Total),
                            totalDisbursement = Convert.ToDecimal(item.TotalDisbursement),
                            loanDate = Convert.ToDateTime(item.loanDate),
                            name = emp.nameEnglish,
                            designationEn = emp.designationName,
                            loanType = item.LoanType
                        };

                        var loanId = await salaryService.SaveStaffLoan(staffLoan);

                        var staffLoanLogId = 0;

                        var data = new StaffLoanLog
                        {
                            Id = staffLoanLogId,
                            staffLoanId = loanId,
                            createdAt = DateTime.Now,
                            effectiveDate = staffLoan.loanDate,
                            interestProcessDate = staffLoan.loanDate,
                            particular = "Opening Balance",
                            principal = staffLoan.principalAmount,
                            interest = staffLoan.interestAmount,
                            charge = staffLoan.chargeAmount,
                            total = staffLoan.totalAmount,
                            isActive = 1,
                            status = 1
                        };
                        await salaryService.SaveStaffLoanLog(data);
                    }
                    else
                    {
                        var err = new StaffLoanUploadFailed
                        {
                            Id = 0,
                            empCode = item.EmpId,
                            empName = item.Name,
                            LoanNo = item.CBSAc,
                            NewLoanNo = item.LoanAc,
                            principalAmount = Convert.ToDecimal(item.Principal),
                            interestAmount = Convert.ToDecimal(item.Interest),
                            chargeAmount = Convert.ToDecimal(item.Charge),
                            totalAmount = Convert.ToDecimal(item.Total),
                            totalDisbursement = Convert.ToDecimal(item.TotalDisbursement),
                            loanDate = Convert.ToDateTime(item.loanDate),
                            loanType = item.LoanType
                        };

                        await salaryService.SaveStaffLoanErrors(err);
                    }
                }

            }

            var uploadLog = new UploadExcelLog
            {
                Id = 0,
                fileName = postedFile.FileName,
                totalData = loans.Count(),
                filePath = $"{Environment.WebRootPath}\\UploadLoanExcel\\{postedFile.FileName}",
                uploadDate = DateTime.Now,
                type = "StaffLoan"
            };
            await salaryService.saveUploadLoanExcelLog(uploadLog);

            return RedirectToAction(nameof(UploadLoanData));
        }

        private List<UploadLoanDataExcelFormat> GetAllDataFromExcel(string fileName)
        {
            var loans = new List<UploadLoanDataExcelFormat>();
            //var fName = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\UploadLoanExcel\"}" + "\\" + fileName;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            try
            {
                using (var stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        while (reader.Read())
                        {
                            //var EmpId = reader.GetValue(0)?.ToString();
                            //var Name = reader.GetValue(1)?.ToString();
                            //var Designation = reader.GetValue(2)?.ToString();
                            //var LoanType = reader.GetValue(3)?.ToString();
                            //var TotalDisbursement = reader?.GetValue(4).ToString();
                            //var CBSAc = reader.GetValue(5)?.ToString();
                            //var LoanAc = reader.GetValue(6)?.ToString();
                            //var Principal = reader.GetValue(7)?.ToString();
                            //var Interest = reader.GetValue(8)?.ToString();
                            //var Charge = reader.GetValue(9)?.ToString();
                            //var Total = reader.GetValue(10)?.ToString();
                            //var loanDate = reader.GetValue(11)?.ToString();

                            if (reader.GetValue(0).ToString() != "EmpId")
                            {
                                loans.Add(new UploadLoanDataExcelFormat
                                {
                                    EmpId = reader.GetValue(0).ToString(),
                                    Name = reader.GetValue(1).ToString(),
                                    Designation = reader.GetValue(2).ToString(),
                                    LoanType = reader.GetValue(3).ToString(),
                                    TotalDisbursement = reader.GetValue(4).ToString(),
                                    CBSAc = reader.GetValue(5)?.ToString(),
                                    LoanAc = reader.GetValue(6)?.ToString(),
                                    Principal = reader.GetValue(7).ToString(),
                                    Interest = reader.GetValue(8).ToString(),
                                    Charge = reader.GetValue(9).ToString(),
                                    Total = reader.GetValue(10).ToString(),
                                    loanDate = reader.GetValue(11).ToString()
                                });
                            }
                        }
                    }
                }
                return loans;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Asad added on 08.06.2023
        public async Task<IActionResult> GetNewLoans(int salaryPeriodId)
        {
            var data = new StaffLoanInfoVm
            {
               staffLoanInfo = await salaryService.GetNewLoans(salaryPeriodId)
            };
            return Json(data);
        }
        public async Task<IActionResult> GetNumberOfNewLoans(int salaryPeriodId)
        {
            var data = new SalaryProcessViewModel
            {
                resolveAllVms = await salaryService.GetNumberOfNewLoans(salaryPeriodId)
            };
            return Json(data);
        }

    }
}