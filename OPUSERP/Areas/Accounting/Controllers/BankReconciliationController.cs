using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Accounting.Data.Entity.BankReconciliation;
using OPUSERP.Accounting.Services.Voucher.Interfaces;
using OPUSERP.Areas.Accounting.Models;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.Helpers;

namespace OPUSERP.Areas.Accounting.Controllers
{
    [Authorize]
    [Area("Accounting")]
    public class BankReconciliationController : Controller
    {
        private readonly IVoucherService voucherService;
        private readonly IUserInfoes userInfo;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly string rootPath;
        private readonly MyPDF myPDF;
        private readonly IAccountingReportService reportingService;
        public BankReconciliationController(IVoucherService voucherService, IUserInfoes userInfo, IHostingEnvironment _hostingEnvironment, IAccountingReportService reportingService)
        {
            this.voucherService = voucherService;
            this.userInfo = userInfo;
            this._hostingEnvironment = _hostingEnvironment;
            this.reportingService = reportingService;
        }
        public async Task<IActionResult> ReconciliationList()
        {
            string userName = User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);

            BankReconciliationModel model = new BankReconciliationModel();
            model.bankReconcileMasters = await voucherService.GetAllBankReconcileMasters(userInfos?.specialBranchUnitId);
            return View(model);
        }

        public async Task<IActionResult> Index(int? id)
        {
            BankReconciliationModel model = new BankReconciliationModel();
            try
            {
                ViewBag.masterId = id;
                var recouncil = await voucherService.GetAllBankReconcileMaster();

                int Cpurchase = 0;
                if (recouncil != null)
                {
                    Cpurchase = recouncil.Count();
                }
                var issueNo = "ReConRef-" + DateTime.Now.ToString("ddMMyyyy") + Cpurchase + 1;

                if (id != 0)
                {
                    model.bankReconcileMaster = await voucherService.GetBankReconcileMasterById((int)id);
                    issueNo = model.bankReconcileMaster?.ReconRef;
                }

                ViewBag.refno = issueNo;

            }
            catch (Exception)
            {

                throw;
            }



            return View(model);
            //var model = new FiscalYearViewModel
            //{
            //    fiscalYear = await budgetRequsitionMasterService.GetFiscalYearById((int)Id),
            //}; if (model.fiscalYear == null) model.fiscalYear = new FiscalYear();
            //return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Index([FromForm] BankReconciliationModel model)
        {

            string userName = User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            //  ViewBag.sbuId = userInfos.projectId;
            var recouncil = await voucherService.GetAllBankReconcileMaster();
            int Cpurchase = 0;
            if (recouncil != null)
            {
                Cpurchase = recouncil.Count();
            }
            var issueNo = "ReConRef-" + model.closingDate?.ToString("ddMMyyyy") + Cpurchase + 1;


            if (Convert.ToInt32(model.bankReconciliationId) != 0)
            {
                var bankReconciliation = await voucherService.GetBankReconcileMasterById((int)model.bankReconciliationId);
                issueNo = bankReconciliation.ReconRef;
                await voucherService.DeleteBankReconcileDetailsByMasterId((int)model.bankReconciliationId);
            }
            ViewBag.bankReconciliationId = issueNo;

            try
            {

                BankReconcileMaster bankReconcileMaster = new BankReconcileMaster
                {
                    Id = Convert.ToInt32(model.bankReconciliationId),
                    ReconRef = issueNo,
                    reconFromDate = model.reconFromDate,
                    reconToDate = model.reconToDate,
                    ledgerId = model.txtPaymentAccountId,
                    ledgerRelationId = model.hdnPaymentAccId,
                    closingDate = model.closingDate,
                    bankStatementClosingBalance = model.bankStatementClosingBalance,
                    closingBalance = model.currentBalance,
                    sbuId = userInfos?.specialBranchUnitId

                };
                int bCID = await voucherService.SaveBankReconcilation(bankReconcileMaster);

                return Json(new { message = "Saved Successfully", id = bCID, Success = true });
                //return RedirectToAction("ReconciliationDetails", new { id = bCID });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, id = 0, Success = false });
                //throw;
            }
        }

        public async Task<IActionResult> ReconciliationDetails(int? id)
        {
            ViewBag.masterId = id;
            BankReconciliationModel model = new BankReconciliationModel();
            if (id > 0)
            {
                model.bankReconcileMaster = await voucherService.GetBankReconcileMasterById((int)id);
                if (model.bankReconcileMaster != null)
                {
                    //  model.ledgerBookViewModel = await voucherService.GetBankReconcileMasterById((int)id);
                    model.ledgerBookViewModel = await reportingService.GetLedgerForBankReconcilation(Convert.ToInt32(model.bankReconcileMaster?.ledgerId), 0, Convert.ToDateTime(model.bankReconcileMaster?.reconFromDate), Convert.ToDateTime(model.bankReconcileMaster?.reconToDate), model.bankReconcileMaster?.sbuId, 0);

                    //var bankReconDetails = await voucherService.GetBankReconcileDetail();
                    //int?[] voucherDetailIds = bankReconDetails.Select(x=>x.voucherDetailId).ToArray();
                    //model.ledgerBookViewModel.ledgerBookViewModelWithoutSPs =
                    //    model.ledgerBookViewModel?.ledgerBookViewModelWithoutSPs.Where(x => !voucherDetailIds.Contains(x.VoucherDetailId));
                }
            }


            return View(model);
        }

        [HttpGet]
        public IActionResult PaymentStatus()
        {
            var model = new PaymentStatusViewModel
            {
                voucherMasters = voucherService.GetvoucherMasterByTypeId(6),
            };
            return View(model);
        }

        public IActionResult ChequeDelivery()
        {
            return View();
        }

        public IActionResult BankReconciliation()
        {
            return View();
        }

        public IActionResult BankReconciliationOD()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> SaveReconcilData([FromForm] BankReconciliationModel model)
        {

            string userName = User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            if (Convert.ToInt32(model.bankReconciliationId) != 0)
            {
                var bankReconciliation = await voucherService.GetBankReconcileMasterById((int)model.bankReconciliationId);
                await voucherService.DeleteBankReconcileDetailsByMasterId((int)model.bankReconciliationId);
            }

            try
            {
                if (model.voucherIds.Count() > 0)
                {

                    for (int i = 0; i < model.voucherIds.Length; i++)
                    {
                        DateTime? date = null;
                        DateTime? unCheckdate = null;

                        if (model.isCheck[i] == 1)
                        {
                            if (model.reconDate[i] != null)
                            {
                                date = Convert.ToDateTime(model.reconDate[i]);
                               // date = DateTime.Parse(model.reconDate[i]);
                              //  date =  DateTime.ParseExact(model.reconDate[i], "dd/MM/yyyy", null);
                            }

                        }
                        else
                        {
                            if (model.reconDate[i] != null)
                            {
                               unCheckdate = Convert.ToDateTime(model.reconDate[i]);
                               //unCheckdate = DateTime.Parse(model.reconDate[i]);
                             //   unCheckdate = DateTime.ParseExact(model.reconDate[i], "dd/MM/yyyy", null);
                            }

                        }

                        BankReconcileDetail BankReconcileDetail = new BankReconcileDetail
                        {
                            Id = 0,
                            bankReconcileMasterId = model.bankReconciliationId,
                            voucherDetailId = model.voucherIds[i],
                            voucherMasterId = model.voucherMasterIds[i],
                            chequeNo = model.chequeNo[i],
                            isChecked = (model.isCheck[i] == 1) ? model.isCheck[i] : 0,
                            chackDate = date,
                             unCheckDate = unCheckdate,
                            transectionMode = (model.diposit[i] > 0) ? 1 : 2,
                            drAmount = model.diposit[i],
                            crAmount = model.payment[i],
                            specialBranchUnitId = userInfos?.specialBranchUnitId
                        };
                        int reconDetailId = await voucherService.SaveBankReconcilationDetail(BankReconcileDetail);

                        //}

                    }

                }
                return Json(new { message = "Saved Successfully", id = model.bankReconciliationId, Success = true });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, id = 0, Success = false });
                //throw;
            }
        }

    }
}