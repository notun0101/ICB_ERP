using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Accounting.Data.Entity.AccountingSettings;
using OPUSERP.Accounting.Services.AccountingSettings.Interfaces;
using OPUSERP.Areas.Accounting.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Data.Entity.AttachmentComment;
using OPUSERP.ERPServices.AttachmentComment.Interfaces;
using OPUSERP.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using OPUSERP.Areas.MasterData.Models;
using DinkToPdf.Contracts;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.Payroll.Services.Salary.Interfaces;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.Accounting.Data.Entity.Voucher;
using OPUSERP.Accounting.Services.Voucher.Interfaces;

namespace OPUSERP.Areas.Accounting.Controllers
{
    [Authorize]
    [Area("Accounting")]
    public class LedgerOpeningBalanceController : Controller
    {
        private readonly ILedgerService ledgerService;
        private readonly IAccountGroupService accountGroupService;
        private readonly IAttachmentCommentService attachmentCommentService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IOpeningBalanceService openingBalanceService;
        private readonly ISalaryService salaryService;
        private readonly IERPCompanyService eRPCompanyService;
        private readonly IVoucherService voucherService;
        private readonly IUserInfoes userInfoes;
        private readonly string rootPath;
        private readonly MyPDF myPDF;
        public string FileName;

        public LedgerOpeningBalanceController(ILedgerService ledgerService,IUserInfoes userInfoes, IAccountGroupService accountGroupService, IHostingEnvironment hostingEnvironment, IAttachmentCommentService attachmentCommentService, IConverter converter, IOpeningBalanceService openingBalanceService, ISalaryService salaryService, IERPCompanyService eRPCompanyService, IVoucherService voucherService )
        {
            this.ledgerService = ledgerService;
            this.accountGroupService = accountGroupService;
            this._hostingEnvironment = hostingEnvironment;
            this.attachmentCommentService = attachmentCommentService;
            this.openingBalanceService = openingBalanceService;
            this.salaryService = salaryService;
            this.eRPCompanyService = eRPCompanyService;
            this.voucherService = voucherService;
            this.userInfoes = userInfoes;

            myPDF = new MyPDF(hostingEnvironment, converter);

            rootPath = hostingEnvironment.ContentRootPath;
        }

        // GET: Ledger
        public async Task<IActionResult> Index()
        {
            string username = HttpContext.User.Identity.Name;
            var userinfo = await userInfoes.GetUserInfoByUser(username);
            string userName = userinfo?.UserName;
            int? branchid = 0;
            if (userName == "biplab" || userName == "suza")
            {
                branchid = 0;
            }
            else
            {
                branchid = userinfo?.specialBranchUnitId;
            }

            LedgerOpeningBalanceViewModel model = new LedgerOpeningBalanceViewModel
            {
               // openingBalances = await openingBalanceService.GetOpeningBalance(),
                voucherMasters = await openingBalanceService.GetOpeningBalancebyVoucherNo(Convert.ToInt32(branchid)),
             
                
            };

            ViewBag.branchId = branchid;
            return View(model);
        }

        // POST: Ledger/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] LedgerOpeningBalanceViewModel model)
        {
            try
            {
                string userName = User.Identity.Name;
                var userInfos = await userInfoes.GetUserInfoByUser(userName);
                var salaryYear = await salaryService.GetAllSalaryYear();
                var taxYear = await salaryService.GetAllTaxYear();
                var company = await eRPCompanyService.GetAllCompany();
                if (model.voucherId > 0)
                {
                  await  openingBalanceService.DeleteopeningOpeningBalanceDetailsByVoucherMasterId((int)model.voucherId);
                }
                VoucherMaster voucherMaster = new VoucherMaster
                {

                    Id = Convert.ToInt32(model.voucherId),
                    voucherNo = "Opening Balance",
                    voucherDate = model.balanceUpTo,
                    voucherTypeId = 7,
                    voucherAmount = model.amount,
                    isPosted = 2,
                    companyId = company.OrderByDescending(a => a.Id).FirstOrDefault().Id,
                    fiscalYearId = salaryYear.OrderByDescending(a => a.Id).FirstOrDefault().Id,
                    taxYearId = taxYear.OrderByDescending(a => a.Id).FirstOrDefault().Id,
                    fundSourceId = 1
                    //projectId = model.projectId
                };

                int VMID = await voucherService.SavevoucherMaster(voucherMaster);

                int? RelationId = 0;
                if (model.subledgerRelationId > 0)
                {
                    RelationId = model.subledgerRelationId;
                }
                else
                {
                    RelationId = model.ledgerRelationId;
                }
                string username = HttpContext.User.Identity.Name;
                var userinfo = await userInfoes.GetUserInfoByUser(username);

                var ledgerinfo = await ledgerService.GetDefaultledgerById();
                var ledgerRelation = new LedgerRelation();
                if (ledgerinfo == null)
                {
                    int? branchid = 0;
                    if (username == "biplab" || username == "suza")
                    {
                        branchid = 0;
                    }
                    else
                    {
                        branchid = userinfo?.specialBranchUnitId;
                    }
                    model.openingBalances = await openingBalanceService.GetOpeningBalanceBranchUnitWise(Convert.ToInt32(branchid));
                    ModelState.AddModelError(string.Empty, "You must Entry Defult Ledeger first.");
                    return View(model);
                }
                else
                {
                    ledgerRelation = await ledgerService.GetledgerRelationByLedgerIdSingle(ledgerinfo.Id);
                }

                int dr = 1;
                if (model.transectionModeId == 1)
                {
                    dr = 2;
                }

                VoucherDetail  data1 = new VoucherDetail
                {
                  
                    Id =0,
                    ledgerRelationId = ledgerRelation.Id,
                    amount = model.amount,
                    transectionModeId = dr,
                    voucherId = VMID,
                    accountName = model.particular,
                    specialBranchUnitId = userinfo?.specialBranchUnitId,
                };

                int defult = await voucherService.SavevoucherDetail(data1);


                VoucherDetail data = new VoucherDetail
                {
                    Id = 0,
                    ledgerRelationId = (int)RelationId,
                    amount = model.amount,
                    transectionModeId = model.transectionModeId,
                    voucherId = VMID,
                    accountName = model.particular,
                    specialBranchUnitId = userinfo?.specialBranchUnitId,
                };

                int ledgerOpeningBalanceId = await voucherService.SavevoucherDetail(data);


                return Json(ledgerOpeningBalanceId);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction("LedgerDetails", "Ledger", new { id = ledgerId, Area = "Accounting" });
        }

        [HttpPost]
        public async Task<JsonResult> DeleteopeningBalanceByVoucherMasterId(int Id)
        {
            await openingBalanceService.DeleteopeningBalanceByVoucherMasterId(Id);
            return Json(true);
        }

    }
}