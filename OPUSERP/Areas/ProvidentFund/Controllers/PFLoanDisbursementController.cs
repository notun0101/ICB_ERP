using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.ProvidentFund.Models;
using OPUSERP.ProvidentFund.Data.Entity;
using OPUSERP.ProvidentFund.Service.Interface;
using System.Threading.Tasks;
using System;
using OPUSERP.ProvidentFund.Service;

using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Office2010.Excel;
using OPUSERP.HRPMS.Services.Employee.Interfaces;

namespace OPUSERP.Areas.ProvidentFund.Controllers
{
    [Area("ProvidentFund")]

    public class PFLoanDisbursementController : Controller
    {
        private readonly IPFLoanDisbursementService disbursementService;
        //private readonly IPFLoanManagementService pFLoanManagementService;
        private readonly IPFMemberInfoService pFMemberInfoService;
        private readonly IPersonalInfoService personalInfoService;

        public PFLoanDisbursementController(IPFLoanDisbursementService disbursementService, IPFMemberInfoService pFMemberInfoService, IPersonalInfoService personalInfoService)
        {
            this.disbursementService = disbursementService;
            this.pFMemberInfoService = pFMemberInfoService;
            this.personalInfoService = personalInfoService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LoanOverview()
        {
            //New
            PFLoanDisbursementViewModel model = new PFLoanDisbursementViewModel
            {
                PFLoanDisbursements = await disbursementService.GetAllLoanDisbursement()
            };

            //Old
            //LoanApplicationViewModel model = new LoanApplicationViewModel
            //{
            //    pFLoanManagements = await pFLoanManagementService.GetAllPFLoanManagement()
            //};
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AllApplicationList()
        {
            PFLoanDisbursementViewModel model = new PFLoanDisbursementViewModel
            {
                PFLoanDisbursements = await disbursementService.GetAllLoanDisbursement()
            };
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> EditAppliedLoan(int id)
        {
            var data = await disbursementService.GetLoanDisbursementId(id);

            PFLoanDisbursementViewModel model = new PFLoanDisbursementViewModel
            {


                loanId = data.Id,
                pfmemberId = data.pfmemberId,
                loanAmount = data.loanAmount,
                installmentAmount = data.installmentAmount,
                tenure = data.tenure,
                interestRate = data.interestRate,
                expectedSettlementDate = data.expectedSettlementDate,
                expectedDate = data.expectedDate,
                applicationDate = data.applicationDate,
                note = data.note,
                approveStatus = data.approveStatus,
                isActive = data.isActive,
            };

            return View(model);
        }
        [HttpPost]
        public async Task<JsonResult> UpdateLoanStatus(int id, int status)
        {
            try
            {
                await disbursementService.UpdateLoanDisbursementStatus(id, status, HttpContext.User.Identity.Name);
                return Json(true);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpGet]
        public IActionResult LoanDisbursement()
        //public async Task<IActionResult> LoanApplication()
        {
            PFLoanDisbursementViewModel model = new PFLoanDisbursementViewModel
            {

            };

            return View(model);

        }

        [HttpPost]

        public async Task<IActionResult> LoanDisbursement([FromForm] PFLoanDisbursementViewModel model)
        //public async Task<IActionResult> LoanApplication([FromForm] PFLoanDisbursementViewModel model)
        {

            var pfMember = await pFMemberInfoService.GetPFMemberInfoByEmployeeCode(model.employeeCode);
            if (pfMember == null)
            {
                //Return Message: Be PF Member First
                return RedirectToAction(nameof(LoanDisbursement));
            }
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(model.employeeCode);

            if (EmpInfo == null)
            {
                //Return Message: He is not a employee or Inactive
                return RedirectToAction(nameof(LoanDisbursement));
            }
            var disbursement = await disbursementService.GetLoanDisbursementByEmployeeId(EmpInfo.Id);

            if (disbursement != null && (disbursement.approveStatus == 0))
            {
                //Return Message: Waiting for approval
                return RedirectToAction(nameof(LoanDisbursement));
            }
            if (disbursement != null && (disbursement.approveStatus == 1 && disbursement.isActive == 1))
            {
                //Return Message: Loan is Running
                return RedirectToAction(nameof(LoanDisbursement));
            }


            int status = 0;
            if (model.approveStatus == null)
            {
                status = 0;
            }
            else
            {
                status = (int)model.approveStatus;
            }

            PFLoanDisbursement data = new PFLoanDisbursement
            {

                Id = model.loanId,

                EmployeeInfoId = pfMember.employeeInfoId,

                pfmemberId = pfMember.Id,
                loanAmount = model.loanAmount,
                installmentAmount = model.installmentAmount,
                tenure = model.tenure,
                interestRate = model.interestRate,
                expectedSettlementDate = model.expectedSettlementDate,
                expectedDate = model.expectedDate,
                applicationDate = model.applicationDate,
                note = model.note,
                approveStatus = status,
                isActive = 0  //Maintain 2 values for [yes-no] condition, 0 for inactive and 1 for active. Never use null in this case:   

            };
            await disbursementService.SaveLoanDisbursement(data);
            return RedirectToAction(nameof(LoanDisbursement));
        }

        [HttpGet]
        public async Task<IActionResult> EditLoanApplication(int id)
        {
            var data = await disbursementService.GetLoanDisbursementId(id);

            PFLoanDisbursementViewModel model = new PFLoanDisbursementViewModel
            {
                loanId = data.Id,
                pfmemberId = data.pfmemberId,
                loanAmount = data.loanAmount,
                installmentAmount = data.installmentAmount,
                tenure = data.tenure,
                interestRate = data.interestRate,
                expectedSettlementDate = data.expectedSettlementDate,
                expectedDate = data.expectedDate,
                applicationDate = data.applicationDate,
                note = data.note,
                approveStatus = data.approveStatus,
                isActive = data.isActive,
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditLoanApplication([FromForm] PFLoanDisbursementViewModel model)
        {
            PFLoanDisbursement data = new PFLoanDisbursement
            {

                Id = model.loanId,
                pfmemberId = model.pfmemberId,
                loanAmount = model.loanAmount,
                installmentAmount = model.installmentAmount,
                tenure = model.tenure,
                interestRate = model.interestRate,
                expectedSettlementDate = model.expectedSettlementDate,
                expectedDate = model.expectedDate,
                applicationDate = model.applicationDate,
                note = model.note,
                approveStatus = model.approveStatus,
                isActive = 1

            };
            await disbursementService.SaveLoanDisbursement(data);
            return RedirectToAction(nameof(LoanDisbursement));
        }

        public async Task<IActionResult> Delete(int id)
        {
            //await stockService.DeleteStockMasterById(id);
            if (id > 0)
            {

                try
                {
                    await disbursementService.DeleteLoanDisbursement(id);
                    return Json(new { Success = true, Message = "Deleted Successfully!" });
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Message = "Unable to delete! please try again.. ", exception = ex.Message });
                }
            }

            return Json("ok");
        }

        //New

        [HttpGet]
        public async Task<IActionResult> PendingLoanDisbursement()
        {
            PFLoanDisbursementViewModel model = new PFLoanDisbursementViewModel
            {
                PFLoanDisbursements = await disbursementService.GetAllPendingLoanDisbursement()
            };
            return View(model);
        }

        //Old
        //[HttpGet]
        //public async Task<IActionResult> LoanPending()
        //{
        //    PFLoanDisbursementViewModel model = new PFLoanDisbursementViewModel
        //    {
        //        PFLoanDisbursements = await disbursementService.GetAllPendingLoanDisbursement() 
        //    };
        //    return View(model);
        //}
        public async Task<IActionResult> LoanDisbursment()
        {
            PFLoanDisbursementViewModel model = new PFLoanDisbursementViewModel
            {
                PFLoanDisbursements = await disbursementService.GetAllLoanDisbursement()
            };
            return View(model);
        }
        public async Task<IActionResult> PendingLoanPay()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PendingLoanPay([FromForm] PendingLoanPayViewModel model)
        {
            return View();
        }
        public async Task<IActionResult> LoanRepayment()
        {
            PFLoanDisbursementViewModel model = new PFLoanDisbursementViewModel
            {
                PFLoanDisbursements = await disbursementService.GetAllLoanDisbursement()
            };
            return View(model);
        }
        public async Task<IActionResult> EditLoanRepayment()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EditLoanRepayment([FromForm] LoanRepaymentViewModel model)
        {
            return View();
        }

        public async Task<IActionResult> LoanReport()
        {
            PFLoanDisbursementViewModel model = new PFLoanDisbursementViewModel
            {
                PFLoanDisbursements = await disbursementService.GetAllLoanDisbursement()
            };
            return View(model);
        }

        public async Task<IActionResult> ViewLoanReport()
        {
            return View();
        }
        public async Task<IActionResult> ProcessDueLoan()
        {
            return View();
        }


    }
}
