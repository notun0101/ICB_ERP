//New
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Accounting.Data.Entity.AccountingSettings;
using OPUSERP.Accounting.Services.AccountingSettings;
using OPUSERP.Areas.ProvidentFund.Models;
using OPUSERP.HRPMS.Services.MasterData;
using OPUSERP.Payroll.Services.Salary;
using OPUSERP.ProvidentFund.Data.Entity;
using OPUSERP.ProvidentFund.Service.Interface;
using OPUSERP.Utility.Models;

namespace OPUSERP.Areas.ProvidentFund.Controllers
{
    [Area("ProvidentFund")]

    public class LoanManagementController : Controller
    {
        private readonly IPFLoanManagementService pFLoanManagementService;
        private readonly IPFMemberInfoService pFMemberInfoService;

        public LoanManagementController(IPFLoanManagementService pFLoanManagementService, IPFMemberInfoService pFMemberInfoService)
        {
            this.pFLoanManagementService = pFLoanManagementService;
            this.pFMemberInfoService = pFMemberInfoService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LoanOverview()
        {
            LoanApplicationViewModel model = new LoanApplicationViewModel
            {
                pFLoanManagements = await pFLoanManagementService.GetAllPFLoanManagement()
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AllApplicationList()
        {
            LoanApplicationViewModel model = new LoanApplicationViewModel
            {
                pFLoanManagements = await pFLoanManagementService.GetAllPFLoanManagement()
            };
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> EditAppliedLoan(int id)
        {
            var data = await pFLoanManagementService.GetPFLoanManagementId(id);

            LoanApplicationViewModel model = new LoanApplicationViewModel
            {
                loanId = data.Id,
                pfmemberId = data.pfmemberId,
                loanAmount = data.loanAmount,
                installmentAmount = data.installmentAmount,
                installmentPeriod = data.installmentPeriod,
                interestRate = data.interestRate,
                expectedSettlementDate = data.expectedSettlementDate,
                expectedDate = data.disbursementDate,
                applicationDate = data.applicationDate,
                note = data.note,
                approveStatus = data.approveStatus,
                isActive = data.isActive,
                memberName=data.pfmember.memberName,
                memberCode=data.pfmember.memberCode,
            };

            return View(model);
        }
        [HttpPost]
        public async Task<JsonResult> UpdateLoanStatus(int id, int status)
        {
            try
            {
                await pFLoanManagementService.UpdatePFLoanManagementStatus(id, status, HttpContext.User.Identity.Name);
                return Json(true);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> LoanApplication()
        {
            LoanApplicationViewModel model = new LoanApplicationViewModel
            {

            };

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> LoanApplication([FromForm] LoanApplicationViewModel model)
        {
            int status = 0;
            if (model.approveStatus == null)
            {
                status = 0;
            }
            else
            {
                status = (int)model.approveStatus;
            }

            PFLoanManagement data = new PFLoanManagement
            {

                Id = model.loanId,
                pfmemberId = model.pfmemberId,
                loanAmount = model.loanAmount,
                installmentAmount = model.installmentAmount,
                installmentPeriod = model.installmentPeriod,
                interestRate = model.interestRate,
                expectedSettlementDate = model.expectedSettlementDate,
                disbursementDate = model.expectedDate,
                applicationDate = model.applicationDate,
                note = model.note,
                approveStatus = status,
                isActive = model.isActive,
                EmployeeInfoId = model.employeeInfoId

            };
            await pFLoanManagementService.SavePFLoanManagement(data);



            return RedirectToAction(nameof(LoanApplication));
        }

        [HttpGet]
        public async Task<IActionResult> EditLoanApplication(int id)
        {
            var data = await pFLoanManagementService.GetPFLoanManagementId(id);

            LoanApplicationViewModel model = new LoanApplicationViewModel
            {
                loanId = data.Id,
                pfmemberId = data.pfmemberId,
                loanAmount = data.loanAmount,
                installmentAmount = data.installmentAmount,
                installmentPeriod = data.installmentPeriod,
                interestRate = data.interestRate,
                expectedSettlementDate = data.expectedSettlementDate,
                expectedDate = data.disbursementDate,
                applicationDate = data.applicationDate,
                note = data.note,
                approveStatus = data.approveStatus,
                isActive = data.isActive,
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditLoanApplication([FromForm] LoanApplicationViewModel model)
        {
            PFLoanManagement data = new PFLoanManagement
            {

                Id = model.loanId,
                pfmemberId = model.pfmemberId,
                loanAmount = model.loanAmount,
                installmentAmount = model.installmentAmount,
                installmentPeriod = model.installmentPeriod,
                interestRate = model.interestRate,
                expectedSettlementDate = model.expectedSettlementDate,
                disbursementDate = model.expectedDate,
                applicationDate = model.applicationDate,
                note = model.note,
                approveStatus = model.approveStatus,
                isActive = model.isActive

            };
            await pFLoanManagementService.SavePFLoanManagement(data);
            return RedirectToAction(nameof(LoanApplication));
        }

        public async Task<IActionResult> Delete(int id)
        {
            //await stockService.DeleteStockMasterById(id);
            if (id > 0)
            {

                try
                {
                    await pFLoanManagementService.DeletePFLoanManagement(id);
                    return Json(new { Success = true, Message = "Deleted Successfully!" });
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Message = "Unable to delete! please try again.. ", exception = ex.Message });
                }
            }

            return Json("ok");
        }
        [HttpGet]
        public async Task<IActionResult> LoanPending()
        {
            LoanApplicationViewModel model = new LoanApplicationViewModel
            {
                pFLoanManagements = await pFLoanManagementService.GetAllPendingPFLoanManagement()
            };
            return View(model);
        }
        public async Task<IActionResult> LoanDisbursmentPending()
        {
            LoanApplicationViewModel model = new LoanApplicationViewModel
            {
                pFLoanManagements = await pFLoanManagementService.GetAllApprovePFLoanManagement()
            };
            return View(model);
        }
        public async Task<IActionResult> DisbursedLoanList()
        {
            LoanApplicationViewModel model = new LoanApplicationViewModel
            {
                pFLoanManagements = await pFLoanManagementService.GetAllDisbursedLoanList()
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetMemberForLoanSettlement()
        {
            return Json(await pFLoanManagementService.GetAllDisbursedLoanList());

        }

        public async Task<IActionResult> PendingLoanPay(int id)
        {
            var data = await pFLoanManagementService.GetPFLoanManagementId(id);

            LoanApplicationViewModel model = new LoanApplicationViewModel
            {
                loanId = data.Id,
                pfmemberId = data.pfmemberId,
                employeeInfoId = data.EmployeeInfo.Id,
                loanAmount = data.loanAmount,
                installmentAmount = data.installmentAmount,
                installmentPeriod = data.installmentPeriod,
                interestRate = data.interestRate,
                expectedSettlementDate = data.expectedSettlementDate,
                expectedDate = data.disbursementDate,
                applicationDate = data.applicationDate,
                note = data.note,
                approveStatus = data.approveStatus,
                isActive = data.isActive,
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> PendingLoanPay([FromForm] LoanApplicationViewModel model)
        {
            try
            {
                await pFLoanManagementService.UpdatePFLoanManagementStatus(model.loanId, 3, HttpContext.User.Identity.Name);

                var pfCollection = new LoanCollection
                {
                    Id = 0,
                    collectionDate = model.expectedDate,
                    duration = null,
                    particular = "Loan Disbursement",
                    debit = model.loanAmount,
                    credit = null,
                    principal = model.loanAmount,
                    loanManagementId = model.loanId,
                    EmployeeInfoId= model.employeeInfoId,
                    pfmemberId= model.pfmemberId
                };

                await pFLoanManagementService.SaveLoanCollection(pfCollection);

                #region Auto Voucher

              //  await pFLoanManagementService.LoanDisburseAutoVoucher(model.loanId, model.applicationDate.ToString(), model.LedgerRelationId, model.note, HttpContext.User.Identity.Name);

                #endregion

                return Json(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IActionResult> LoanRepayment()
        {
            LoanApplicationViewModel model = new LoanApplicationViewModel
            {
                pFLoanManagements = await pFLoanManagementService.GetAllPFLoanManagement()
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
            LoanApplicationViewModel model = new LoanApplicationViewModel
            {
                pFLoanManagements = await pFLoanManagementService.GetAllPFLoanManagement()
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



        public async Task<IActionResult> CreateInterestProvision()
        {
            InterestProvisionViewModel model = new InterestProvisionViewModel()
            {
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateInterestProvision([FromForm] InterestProvisionViewModel model)
        {
            try
            {
                string employeeCode = HttpContext.User.Identity.Name;

                //LoanCollection data = new LoanCollection
                //{

                //    collectionAmount = model.collectionAmount,
                //    loanInterestAmount = model.loanInterestAmount,
                //    note = model.note,
                //    branchId = model.branchId,
                //    isJournal = 0,
                //    collectionDate = model.collectionDate,
                //    loanManagementId = model.loanManagementId,
                //    pfmemberId = model.pfmemberId
                //};

                var transactionDate = model.TransactionDate.ToString();

                var result = await pFLoanManagementService.CreateInterestProvision(null, transactionDate, employeeCode);

                string message = string.Empty;
                string status = string.Empty;

                if (result != null)
                {
                    message = result.Message;
                    status = result.Status;
                }
                else
                {
                    message = "Unable to Create Interest Provision.";
                    status = "NOK";
                }

                //return new OkObjectResult(new ResponseObject { Status = status, Message = message });


                //#region Auto Voucher

                //await pFLoanManagementService.LoanCollectionAutoVoucher(CollectionId, model.collectionDate.ToString(), model.LedgerRelationId, model.note, HttpContext.User.Identity.Name);

                //#endregion


                return RedirectToAction(nameof(CreateInterestProvision));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<IActionResult> LoanCollection()
        {
            LoanCollectionVm model = new LoanCollectionVm()
            {


            };
            return View(model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LoanCollection([FromForm] LoanCollectionVm model)
        {
            try
            {
                var previousCollection= await pFLoanManagementService.GetCollectionDataByLloanId(model.loanManagementId);
                LoanCollection data = new LoanCollection
                {

                    collectionAmount = model.collectionAmount,
                    branchId = model.branchId,
                    isJournal = 0,
                    collectionDate = model.collectionDate,
                    loanManagementId = model.loanManagementId,
                    pfmemberId = model.pfmemberId,
                    EmployeeInfoId = model.EmployeeInfoId,
                    credit = model.collectionAmount,
                    particular = "EMI Collection",
                    principal= previousCollection.principal - model.collectionAmount,


                };

                int CollectionId = await pFLoanManagementService.SaveLoanCollection(data);


                #region Auto Voucher

             //   await pFLoanManagementService.LoanCollectionAutoVoucher(CollectionId, model.collectionDate.ToString(), model.LedgerRelationId, model.note, HttpContext.User.Identity.Name);

                #endregion


                return RedirectToAction(nameof(LoanCollection));
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public async Task<IActionResult> LoanSettlement()
        {
            LoanCollectionVm model = new LoanCollectionVm()
            {


            };
            return View(model);
        }



        public async Task<IActionResult> GetEarlySettlementClosingBalance(string memberCode)
        {
            return Json(await pFLoanManagementService.GetEarlySettlementClosingBalance(memberCode));
        }


        [HttpGet]
        public async Task<IActionResult> InterestCharge()
        {
            LoanApplicationViewModel model = new LoanApplicationViewModel
            {

            };

            return View(model);

        }

        public async Task<IActionResult> ProcessAllPFInterest(DateTime processDate)
        {
            return Json(await pFLoanManagementService.ProcessAllPFInterest(processDate, HttpContext.User.Identity.Name));
        }


    }
}
