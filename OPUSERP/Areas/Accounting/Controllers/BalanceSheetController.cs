using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Accounting.Data.Entity.AccountingSettings;
using OPUSERP.Accounting.Data.Entity.MasterData;
using OPUSERP.Accounting.Data.Entity.Voucher;
using OPUSERP.Accounting.Services.AccountingSettings.Interfaces;
using OPUSERP.Accounting.Services.MasterData.Interfaces;
using OPUSERP.Accounting.Services.Voucher.Interfaces;
using OPUSERP.Areas.Accounting.Models;
using OPUSERP.Areas.MasterData.Models;
using OPUSERP.Data.Entity.AttachmentComment;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.ERPServices.AttachmentComment.Interfaces;
using OPUSERP.Helpers;
using OPUSERP.SCM.Services.PurchaseOrder.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace OPUSERP.Areas.Accounting.Controllers
{
    [Authorize]
    [Area("Accounting")]
    public class BalanceSheetController : Controller
    {
        private readonly IVoucherService VoucherService;
        private readonly ILedgerService ledgerService;
        private readonly IPurchaseOrderService purchaseOrderService;
        private readonly IGroupNatureService groupNatureService;
        private readonly IAccountGroupService accountGroupService;
        private readonly ICostCentreService costCentreService;
        private readonly IUserInfoes userInfo;
        private readonly IAttachmentCommentService attachmentCommentService;
        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly string rootPath;
        private readonly MyPDF myPDF;
        public string FileName;
        public BalanceSheetController(IVoucherService VoucherService, ILedgerService ledgerService, IPurchaseOrderService purchaseOrderService, IGroupNatureService groupNatureService, IAccountGroupService accountGroupService, ICostCentreService costCentreService, IHostingEnvironment hostingEnvironment, IAttachmentCommentService attachmentCommentService, IUserInfoes userInfo, IConverter converter)
        {
            this.VoucherService = VoucherService;
            this.ledgerService = ledgerService;
            this.purchaseOrderService = purchaseOrderService;
            this.groupNatureService = groupNatureService;
            this.accountGroupService = accountGroupService;
            this.costCentreService = costCentreService;
            this._hostingEnvironment = hostingEnvironment;
            this.attachmentCommentService = attachmentCommentService;
            this.userInfo = userInfo;

            myPDF = new MyPDF(hostingEnvironment, converter);

            rootPath = hostingEnvironment.ContentRootPath;
        }

        #region Balance Sheet Master

        public async Task<IActionResult> Index()
        {
            BalanceSheetMasterViewModel model = new BalanceSheetMasterViewModel
            {
                groupNatures = await groupNatureService.GetgroupNature(),
                balanceSheetMasters = await accountGroupService.GetBalanceSheetMaster()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] BalanceSheetMasterViewModel model)
        {
            //return Json(model);
            if (!ModelState.IsValid)
            {
                model.balanceSheetMasters = await accountGroupService.GetBalanceSheetMaster();
               
            }

            BalanceSheetMaster data = new BalanceSheetMaster
            {
                Id = model.balanceSheetMasterId,
                accountGroupId = model.accountGroupId,
                fsLineName = model.fsLineName,
                noteNo = model.noteNo,
                serialNo = model.serialNo,
                fsLineFor = "BSS"

            };

            await accountGroupService.SaveBalanceSheetMaster(data);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<JsonResult> DeleteBalanceSheetMaster(int Id)
        {
            await accountGroupService.DeleteBalanceSheetMasterById(Id);
            return Json(true);
        }
        

        [HttpGet]
        public async Task<IActionResult> GetLedgerCountByGorupId(int Id)
        {
            return Json(await ledgerService.GetLedgerCountByGorupId(Id));
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountGroupByNatureId(int? natureId)
        {
            return Json(await accountGroupService.GetAccountGroupByNatureId(natureId));
        }

        #endregion

        #region Balance Sheet Note Master

        public async Task<IActionResult> NoteMaster()
        {
            var user = await userInfo.GetAspnetuserByUser(User.Identity.Name);
            NoteMasterViewModel model = new NoteMasterViewModel
            {
                balanceSheetMasters = await accountGroupService.GetBalanceSheetMaster(),
                noteMasters = await accountGroupService.GetNoteMasterByBranchId(user?.specialBranchUnitId)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NoteMaster([FromForm] NoteMasterViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    model.noteMasters = await accountGroupService.GetNoteMaster();

            //}
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var branchId = userInfos?.specialBranchUnitId;
            NoteMaster data = new NoteMaster
            {
                Id = model.noteMasterId,
                noteName = model.noteName,
                noteHead = model.noteHead,
                nmSerialNo=model.nmSerialNo,
                balanceSheetMasterId = model.balanceSheetMasterId,
                specialBranchUnitId = branchId
            };

            await accountGroupService.SavenoteMaster(data);

            return RedirectToAction(nameof(NoteMaster));
        }

        [HttpPost]
        public async Task<JsonResult> DeleteNoteMaster(int Id)
        {
            var flag = await accountGroupService.DeleteNoteMasterById(Id);

            return Json(flag);
        }
        [HttpPost]
        public async Task<JsonResult> DeleteBalanceSheetDetail(int Id)
        {
            await accountGroupService.DeleteBalanceSheetDetailsById(Id);
            return Json(true);
        }




        #endregion

        #region Balance Sheet Detail

        public async Task<IActionResult> BalanceSheetDetail()
        {
            var user = await userInfo.GetAspnetuserByUser(User.Identity.Name);
            BalanceSheetDetailViewModel model = new BalanceSheetDetailViewModel
            {
                balanceSheetMasters=await accountGroupService.GetBalanceSheetMaster(),
                noteMasters = await accountGroupService.GetNoteMaster(),
                balanceSheetDetails=await accountGroupService.GetBalanceSheetDetailsByBranchId((int)user.specialBranchUnitId),
                ledgers= await ledgerService.GetLedgerBySbuId((int)user.specialBranchUnitId),
                noteMasterId = user.specialBranchUnitId
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BalanceSheetDetail([FromForm] BalanceSheetDetailViewModel model)
        {
            //return Json(model);
            if (!ModelState.IsValid)
            {
                model.noteMasters = await accountGroupService.GetNoteMaster();

            }

            BalanceSheetDetails data = new BalanceSheetDetails
            {
                Id = model.balanceSheetDetailId,
                noteMasterId = model.noteMasterId,
                ledgerId = model.ledgerId,
              
            };

            await accountGroupService.SaveBalanceSheetDetails(data);

            return RedirectToAction(nameof(BalanceSheetDetail));
        }

        [HttpPost]
        public async Task<JsonResult> DeleteBalanceSheetDetails(int Id)
        {
            await accountGroupService.DeleteBalanceSheetDetailsById(Id);
            return Json(true);
        }

        [Route("/api/global/GetNoteMasterbyMasterId/{id}")]
        public async Task<JsonResult> GetNoteMasterbyMasterId(int id)
        {
            var subLedgers = await accountGroupService.GetNoteMasterByMasterId(id);
            return Json(subLedgers);
        }

        [Route("/api/global/GetNoteMasterbyMasterIdAndBranchId/{id}/{branchId}")]
        public async Task<JsonResult> GetNoteMasterbyMasterIdAndBranchId(int id,int branchId)
        {
            var subLedgers = await accountGroupService.GetNoteMasterByMasterIdAndBranchId(id,branchId);
            return Json(subLedgers);
        }




        #endregion

        #region Profit Loss Account Master

        public async Task<IActionResult> ProfitLossMaster()
        {
            BalanceSheetMasterViewModel model = new BalanceSheetMasterViewModel
            {
                groupNatures = await groupNatureService.GetgroupNature(),
                balanceSheetMasters = await accountGroupService.GetBalanceSheetMaster()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfitLossMaster([FromForm] BalanceSheetMasterViewModel model)
        {
            //return Json(model);
            if (!ModelState.IsValid)
            {
                model.balanceSheetMasters = await accountGroupService.GetBalanceSheetMaster();

            }

            BalanceSheetMaster data = new BalanceSheetMaster
            {
                Id = model.balanceSheetMasterId,
                accountGroupId = model.accountGroupId,
                fsLineName = model.fsLineName,
                noteNo = model.noteNo,
                serialNo = model.serialNo,
                fsLineFor = "PLA"

            };

            await accountGroupService.SaveBalanceSheetMaster(data);

            return RedirectToAction(nameof(ProfitLossMaster));
        }
        #endregion

        #region Profit Loss Note Master

        public async Task<IActionResult> NoteMasterProfitLoss()
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            NoteMasterViewModel model = new NoteMasterViewModel
            {
                balanceSheetMasters = await accountGroupService.GetBalanceSheetMaster(),
                noteMasters = await accountGroupService.GetNoteMasterByBranchId(userInfos?.specialBranchUnitId)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NoteMasterProfitLoss([FromForm] NoteMasterViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var branchId = userInfos?.specialBranchUnitId;
            NoteMaster data = new NoteMaster
            {
                Id = model.noteMasterId,
                noteName = model.noteName,
                noteHead = model.noteHead,
                balanceSheetMasterId = model.balanceSheetMasterId,
                specialBranchUnitId = branchId
            };

            await accountGroupService.SavenoteMaster(data);

            return RedirectToAction(nameof(NoteMasterProfitLoss));
        }

        #endregion

        #region Profit & Loss Detail

        public async Task<IActionResult> ProfitLossDetail()
        {
            var user = await userInfo.GetAspnetuserByUser(User.Identity.Name);
            BalanceSheetDetailViewModel model = new BalanceSheetDetailViewModel
            {
                balanceSheetMasters = await accountGroupService.GetBalanceSheetMaster(),
                noteMasters = await accountGroupService.GetNoteMaster(),
                balanceSheetDetails = await accountGroupService.GetBalanceSheetDetailsByBranchId((int)user.specialBranchUnitId),
                ledgers = await ledgerService.GetLedgerBySbuId((int)user.specialBranchUnitId),
                noteMasterId = user.specialBranchUnitId,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfitLossDetail([FromForm] BalanceSheetDetailViewModel model)
        {
            //return Json(model);
            if (!ModelState.IsValid)
            {
                model.noteMasters = await accountGroupService.GetNoteMaster();

            }

            BalanceSheetDetails data = new BalanceSheetDetails
            {
                Id = model.balanceSheetDetailId,
                noteMasterId = model.noteMasterId,
                ledgerId = model.ledgerId,

            };

            await accountGroupService.SaveBalanceSheetDetails(data);

            return RedirectToAction(nameof(ProfitLossDetail));
        }

        #endregion

        #region CFS Account Master

        public async Task<IActionResult> CFSMaster()
        {
            BalanceSheetMasterViewModel model = new BalanceSheetMasterViewModel
            {
                groupNatures = await groupNatureService.GetgroupNature(),
                balanceSheetMasters = await accountGroupService.GetBalanceSheetMaster()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CFSMaster([FromForm] BalanceSheetMasterViewModel model)
        {
            //return Json(model);
            if (!ModelState.IsValid)
            {
                model.balanceSheetMasters = await accountGroupService.GetBalanceSheetMaster();

            }

            BalanceSheetMaster data = new BalanceSheetMaster
            {
                Id = model.balanceSheetMasterId,
                accountGroupId = model.accountGroupId,
                fsLineName = model.fsLineName,
                noteNo = model.noteNo,
                serialNo = model.serialNo,
                fsLineFor = "CFS"

            };

            await accountGroupService.SaveBalanceSheetMaster(data);

            return RedirectToAction(nameof(CFSMaster));
        }
        #endregion

        #region CFS Indirect Account Master 

        public async Task<IActionResult> CFSMasterIND()
        {
            BalanceSheetMasterViewModel model = new BalanceSheetMasterViewModel
            {
                groupNatures = await groupNatureService.GetgroupNature(),
                balanceSheetMasters = await accountGroupService.GetBalanceSheetMaster()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CFSMasterIND([FromForm] BalanceSheetMasterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.groupNatures = await groupNatureService.GetgroupNature();
                model.balanceSheetMasters = await accountGroupService.GetBalanceSheetMaster();
            }

            BalanceSheetMaster data = new BalanceSheetMaster
            {
                Id = model.balanceSheetMasterId,
                accountGroupId = model.accountGroupId,
                fsLineName = model.fsLineName,
                noteNo = model.noteNo,
                serialNo = model.serialNo,
                fsLineFor = "CFSIND"
            };
            await accountGroupService.SaveBalanceSheetMaster(data);
            return RedirectToAction(nameof(CFSMasterIND));
        }
        #endregion

        #region CFS Note Master

        public async Task<IActionResult> NoteMasterCFS()
        {
            var user = await userInfo.GetAspnetuserByUser(User.Identity.Name);
            NoteMasterViewModel model = new NoteMasterViewModel
            {
                balanceSheetMasters = await accountGroupService.GetBalanceSheetMaster(),
                noteMasters = await accountGroupService.GetNoteMasterByBranchId(user?.specialBranchUnitId)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NoteMasterCFS([FromForm] NoteMasterViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var branchId = userInfos?.specialBranchUnitId;
            NoteMaster data = new NoteMaster
            {
                Id = model.noteMasterId,
                noteName = model.noteName,
                noteHead = model.noteHead,
                balanceSheetMasterId = model.balanceSheetMasterId,
                specialBranchUnitId = branchId
            };

            await accountGroupService.SavenoteMaster(data);

            return RedirectToAction(nameof(NoteMasterCFS));
        }
        #endregion

        #region CFS Indirect Note Master

        public async Task<IActionResult> NoteMasterCFSIND()
        {
            var user = await userInfo.GetAspnetuserByUser(User.Identity.Name);
            NoteMasterViewModel model = new NoteMasterViewModel
            {
                balanceSheetMasters = await accountGroupService.GetBalanceSheetMaster(),
                noteMasters = await accountGroupService.GetNoteMasterByBranchId(user?.specialBranchUnitId)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NoteMasterCFSIND([FromForm] NoteMasterViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var branchId = userInfos?.specialBranchUnitId;
            NoteMaster data = new NoteMaster
            {
                Id = model.noteMasterId,
                noteName = model.noteName,
                noteHead = model.noteHead,
                balanceSheetMasterId = model.balanceSheetMasterId,
                nmSerialNo = model.nmSerialNo,
                specialBranchUnitId = branchId
            };
            await accountGroupService.SavenoteMaster(data);
            return RedirectToAction(nameof(NoteMasterCFSIND));
        }
        #endregion
        
        #region CFS Note Details

        public async Task<IActionResult> CFSDetail()
        {
            var user = await userInfo.GetAspnetuserByUser(User.Identity.Name);
            BalanceSheetDetailViewModel model = new BalanceSheetDetailViewModel
            {
                balanceSheetMasters = await accountGroupService.GetBalanceSheetMaster(),
                noteMasters = await accountGroupService.GetNoteMaster(),
                balanceSheetDetails = await accountGroupService.GetBalanceSheetDetailsByBranchId((int)user.specialBranchUnitId),
                ledgers = await ledgerService.GetLedgerBySbuId((int)user.specialBranchUnitId),
                noteMasterId = user.specialBranchUnitId,
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CFSDetail([FromForm] BalanceSheetDetailViewModel model)
        {
            //return Json(model);
            if (!ModelState.IsValid)
            {
                model.noteMasters = await accountGroupService.GetNoteMaster();

            }

            BalanceSheetDetails data = new BalanceSheetDetails
            {
                Id = model.balanceSheetDetailId,
                noteMasterId = model.noteMasterId,
                ledgerId = model.ledgerId,

            };

            await accountGroupService.SaveBalanceSheetDetails(data);

            return RedirectToAction(nameof(CFSDetail));
        }
        #endregion

        #region CFS Indirect Note Details

        public async Task<IActionResult> CFSDetailIND()
        {
            var user = await userInfo.GetAspnetuserByUser(User.Identity.Name);
            BalanceSheetDetailViewModel model = new BalanceSheetDetailViewModel
            {
                balanceSheetMasters = await accountGroupService.GetBalanceSheetMaster(),
                noteMasters = await accountGroupService.GetNoteMaster(),
                balanceSheetDetails = await accountGroupService.GetBalanceSheetDetailsByBranchId((int)user.specialBranchUnitId),
                ledgers = await ledgerService.GetLedgerBySbuId((int)user.specialBranchUnitId),
                noteMasterId = user.specialBranchUnitId,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CFSDetailIND([FromForm] BalanceSheetDetailViewModel model)
        {
            var user = await userInfo.GetAspnetuserByUser(User.Identity.Name);
            if (!ModelState.IsValid)
            {
                model.noteMasters = await accountGroupService.GetNoteMaster();
                model.balanceSheetMasters = await accountGroupService.GetBalanceSheetMaster();
                model.balanceSheetDetails = await accountGroupService.GetBalanceSheetDetailsByBranchId((int)user.specialBranchUnitId);
                model.ledgers = await ledgerService.GetLedgerBySbuId((int)user.specialBranchUnitId);
                model.noteMasterId = user.specialBranchUnitId;
            }

            BalanceSheetDetails data = new BalanceSheetDetails
            {
                Id = model.balanceSheetDetailId,
                noteMasterId = model.noteMasterId,
                ledgerId = model.ledgerId,
            };
            await accountGroupService.SaveBalanceSheetDetails(data);
            return RedirectToAction(nameof(CFSDetailIND));
        }
        #endregion

        #region RV Account Master

        public async Task<IActionResult> RVMaster()
        {
            BalanceSheetMasterViewModel model = new BalanceSheetMasterViewModel
            {
                groupNatures = await groupNatureService.GetgroupNature(),
                balanceSheetMasters = await accountGroupService.GetBalanceSheetMaster()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RVMaster([FromForm] BalanceSheetMasterViewModel model)
        {
            //return Json(model);
            if (!ModelState.IsValid)
            {
                model.balanceSheetMasters = await accountGroupService.GetBalanceSheetMaster();

            }

            BalanceSheetMaster data = new BalanceSheetMaster
            {
                Id = model.balanceSheetMasterId,
                accountGroupId = model.accountGroupId,
                fsLineName = model.fsLineName,
                noteNo = model.noteNo,
                serialNo = model.serialNo,
                fsLineFor = "RV"

            };

            await accountGroupService.SaveBalanceSheetMaster(data);

            return RedirectToAction(nameof(RVMaster));
        }
        #endregion

        #region RV Note Master

        public async Task<IActionResult> NoteMasterRV()
        {
            var user = await userInfo.GetAspnetuserByUser(User.Identity.Name);
            NoteMasterViewModel model = new NoteMasterViewModel
            {
                balanceSheetMasters = await accountGroupService.GetBalanceSheetMaster(),
                noteMasters = await accountGroupService.GetNoteMasterByBranchId(user?.specialBranchUnitId)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NoteMasterRV([FromForm] NoteMasterViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var branchId = userInfos?.specialBranchUnitId;
            NoteMaster data = new NoteMaster
            {
                Id = model.noteMasterId,
                noteName = model.noteName,
                noteHead = model.noteHead,
                balanceSheetMasterId = model.balanceSheetMasterId,
                specialBranchUnitId = branchId
            };

            await accountGroupService.SavenoteMaster(data);

            return RedirectToAction(nameof(NoteMasterRV));
        }
        #endregion

        #region RV Details Detail

        public async Task<IActionResult> RVDetail()
        {
            var user = await userInfo.GetAspnetuserByUser(User.Identity.Name);
            BalanceSheetDetailViewModel model = new BalanceSheetDetailViewModel
            {
                balanceSheetMasters = await accountGroupService.GetBalanceSheetMaster(),
                noteMasters = await accountGroupService.GetNoteMaster(),
                balanceSheetDetails = await accountGroupService.GetBalanceSheetDetailsByBranchId((int)user.specialBranchUnitId),
                ledgers = await ledgerService.GetLedgerBySbuId((int)user.specialBranchUnitId),
                noteMasterId = user.specialBranchUnitId,

            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RVDetail([FromForm] BalanceSheetDetailViewModel model)
        {
            //return Json(model);
            if (!ModelState.IsValid)
            {
                model.noteMasters = await accountGroupService.GetNoteMaster();

            }

            BalanceSheetDetails data = new BalanceSheetDetails
            {
                Id = model.balanceSheetDetailId,
                noteMasterId = model.noteMasterId,
                ledgerId = model.ledgerId,

            };

            await accountGroupService.SaveBalanceSheetDetails(data);

            return RedirectToAction(nameof(RVDetail));
        }
        #endregion
    }
}