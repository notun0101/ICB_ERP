using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Accounting.Data.Entity.Voucher;
using OPUSERP.Accounting.Services.AccountingSettings.Interfaces;
using OPUSERP.Accounting.Services.MasterData.Interfaces;
using OPUSERP.Accounting.Services.Voucher.Interfaces;
using OPUSERP.Areas.Accounting.Models;
using OPUSERP.Areas.SCMIOU.Models;
using OPUSERP.Areas.SCMMatrix.Models;
using OPUSERP.Data;
using OPUSERP.Data.Entity.Matrix;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.Payroll.Services.Salary.Interfaces;
using OPUSERP.SCM.Data.Entity.IOU;
using OPUSERP.SCM.Helpers;
using OPUSERP.SCM.Services.IOU.Interface;
using OPUSERP.SCM.Services.Matrix.Interfaces;

namespace OPUSERP.Areas.SCMIOU.Controllers
{
    [Area("SCMIOU")]
    public class DisbarseController : Controller
    {
        private readonly IIOUService iOUService;
        private readonly IUserInfoes userInfo;
        private readonly IApprovalLogService approverLogService;
        private readonly RequisitionStatusHistory requisitionStatusHistory;
        private readonly IApprovalMatrixService approvalMatrixService;
        private readonly ICostCentreService costCentreService;
        private readonly ILedgerService ledgerService;
        private readonly IVoucherService voucherService;
        private readonly ISalaryService salaryService;
        private readonly IERPCompanyService eRPCompanyService;

        public DisbarseController(IIOUService iOUService, IUserInfoes userInfo, IApprovalLogService approverLogService, RequisitionStatusHistory requisitionStatusHistory, IApprovalMatrixService approvalMatrixService, ICostCentreService costCentreService, ILedgerService ledgerService, IVoucherService voucherService, ISalaryService salaryService, IERPCompanyService eRPCompanyService, ERPDbContext _context)
        {
            this.iOUService = iOUService;
            this.userInfo = userInfo;
            this.approverLogService = approverLogService;
            this.requisitionStatusHistory = requisitionStatusHistory;
            this.approvalMatrixService = approvalMatrixService;
            this.costCentreService = costCentreService;
            this.ledgerService = ledgerService;
            this.voucherService = voucherService;
            this.salaryService = salaryService;
            this.eRPCompanyService = eRPCompanyService;

        }


        public async Task<IActionResult> Index()
        {
            string userName = HttpContext.User.Identity.Name;

            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var user = userInfo.GetUserInfo();
            ViewBag.masterId = 0;
            IOUPaymentMasterViewModel model = new IOUPaymentMasterViewModel
            {
                iOUMasters = await iOUService.GetIOUMasterByUserNameNDateTime(userName, DateTime.Now.ToString(), DateTime.Now.ToString())
            };
            ViewBag.IOUNO = await iOUService.GetIOUPaymentNo();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveIOUPayment([FromForm] IOUPaymentMasterViewModel model)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                var userInfos = await userInfo.GetUserInfoByUser(userName);

                var IOUNoInfo = await iOUService.GetIOUPaymentNo();

                IOUPaymentMaster data = new IOUPaymentMaster
                {
                    Id = 0,
                    iouPaymentNo = IOUNoInfo,
                    iouPaymentDate = model.iouPaymentDate,
                    //projectId = model.projectId[0],
                    //attentionTo = model.attentionTo,
                    //attentionToId = model.attentionToId,
                    iouPaymentStatus = 1,
                    userId = userInfos.UserId,
                    paymentBy = userName,
                    remarks = model.remarks
                };
                int masterId = await iOUService.SaveIOUPaymentMaster(data);

                for (int i = 0; i < model.ioumasterId.Length; i++)
                {
                    IOUPayment details = new IOUPayment
                    {
                        Id = 0,
                        iOUPaymentMasterId = masterId,
                        IOUId = model.ioumasterId[i],
                        iouAmount = model.iouValue[i],
                        paymentAmount = model.subTotal[i],
                        paymentBy = userInfos.UserId,
                        paymentDate = model.iouPaymentDate,
                        statusInfoId = 1,
                    };
                    await iOUService.SaveIOUPayment(details);
                }


                IEnumerable<ApprovalMatrixViewModel> approvarInfo = await approvalMatrixService.GetAllTypeApprovalMatrixByRaiserIdAndTypeId(Convert.ToInt32(model.projectId[0]), 7, userInfos.UserId);
                List<ApprovalMatrixViewModel> lstApproval = approvarInfo.ToList();

                List<ApprovalLog> approvalLogs = new List<ApprovalLog>();

                ApprovalLog approvalLog = new ApprovalLog
                {
                    masterId = masterId,
                    matrixTypeId = 7,
                    isActive = 0,
                    nextApproverId = lstApproval[0].nextApproverId,
                    userId = userInfos.UserId,
                    sequenceNo = 0
                };
                approvalLogs.Add(approvalLog);

                for (int i = 0; i < lstApproval.Count(); i++)
                {
                    ApprovalLog log = new ApprovalLog();
                    log.masterId = masterId;
                    log.matrixTypeId = 7;
                    log.isActive = 0;
                    log.nextApproverId = 0;
                    log.userId = (int)lstApproval[i].nextApproverId;
                    log.sequenceNo = i + 1;
                    if (i == 0)
                    {
                        log.isActive = 1;
                    }
                    approvalLogs.Add(log);
                    log = new ApprovalLog();
                }
                approverLogService.SaveApproverLogList(approvalLogs);

                var nextUserInfo = await userInfo.GetUserInfoByUserId(lstApproval[0].nextApproverId);
                string empNameCode = userInfos.EmpCode + "-" + userInfos.EmpName;
                string nextEmpNameCode = nextUserInfo.EmpCode + "-" + nextUserInfo.EmpName;
                int reqId = 0;
                for (int i = 0; i < model.ioumasterId.Length; i++)
                {
                    var req = await iOUService.GetIOUDetailsByMasterId(Convert.ToInt32(model.ioumasterId[i]));
                    reqId = req.FirstOrDefault().requisitionDetail.requisitionMasterId;
                    await requisitionStatusHistory.SaveRequisitionStatusLog(Convert.ToInt32(reqId), 7, Convert.ToInt32(userInfos.UserTypeId), userInfos.UserId, empNameCode, nextEmpNameCode, "", 21, "IOUPayment", masterId, IOUNoInfo);
                }

                return Json(masterId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<ActionResult> IOUDisburseList()
        {
            string userName = HttpContext.User.Identity.Name;
            IOUPaymentMasterViewModel model = new IOUPaymentMasterViewModel
            {
                iOUPaymentMasters = await iOUService.GetIOUPaymentMasterByUserName(userName),
                issuedIOUPaymentMasters = await iOUService.GetIssuedIOUPaymentMasterByUserName(userName)
            };
            return View(model);
        }

        public async Task<ActionResult> IOUDisburseListForApprove()
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var user = userInfo.GetUserInfo();

            IOUPaymentMasterViewModel model = new IOUPaymentMasterViewModel
            {
                iOUPaymentMasters = await iOUService.GetIOUPaymentMasterListForApprove(userInfos.UserId)
            };
            return View(model);
        }

        public async Task<ActionResult> IOUDisburseApprove(int iOUPaymentMasterId)
        {
            string userName = HttpContext.User.Identity.Name;
            IOUPaymentMasterViewModel model = new IOUPaymentMasterViewModel();

            try
            {
                model.IOUPaymentMaster = await iOUService.GetIOUPaymentMasterById(Convert.ToInt32(iOUPaymentMasterId));
                model.iOUPayments = await iOUService.GetIOUPaymentByiOUPaymentMasterId(iOUPaymentMasterId);
                //model.approverPanel = await approverLogService.GetNextApproverInfo(userName, iOUPaymentMasterId, 7);

                // username
                var userInfos = await userInfo.GetUserInfoByUser(model.IOUPaymentMaster.createdBy);

                ViewBag.EmpName = userInfos.EmpName;
            }catch (Exception ex)
            {
                throw ex;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveIOUDisburseApprove(IOUPaymentMasterViewModel model)
        {

            try
            {
                string userName = HttpContext.User.Identity.Name;
                var userInfos = await userInfo.GetUserInfoByUser(userName);
                var status = 0;

                if (userInfos.UserId == 7)
                {
                    status = 2;
                }
                else
                {
                    status = 3;
                }
                // quantity and unit rate update
                //string userName = HttpContext.User.Identity.Name;
                //var currUserInfo = await userInfo.GetUserInfoByUser(userName);
                //var currentStatus = await approverLogService.GetNextApproverInfo(userName, Convert.ToInt32(model.iOUPaymentMasterId), 7);
                //var iOUMaster = await iOUService.GetIOUPaymentMasterById(Convert.ToInt32(model.iOUPaymentMasterId));
                int statusId = 0;
                //int logStatusId = 0;

                if (model.approveType == -1)
                {
                    statusId = -1;
                    //logStatusId = 25;
                }
                else if (model.approveType == 4)
                {
                    statusId = 4;
                    //logStatusId = 24;
                }
                else
                {
                    //if (currentStatus != null)
                    //{
                    //    var userInfo = await approverLogService.UpdateApprovalLogStatus(userName, Convert.ToInt32(model.iOUPaymentMasterId), 7, model.remarks);
                    //    statusId = 2;
                    //    logStatusId = 22;
                    //}
                    //else
                    //{
                    //statusId = 3;
                    statusId = status;
                    //logStatusId = 23;
                    //}
                }
                for (int i = 0; i < model.ioumasterId.Length; i++)
                {
                    await iOUService.UpdateIOUPaymentForApprove(Convert.ToInt32(model.ioumasterId[i]), model.iouValue[i], statusId);

                }
                iOUService.UpdateIOUPaymentMaster(Convert.ToInt32(model.iOUPaymentMasterId), statusId);

                //string empNameCode = currUserInfo.EmpCode + "-" + currUserInfo.EmpName;
                //string nextEmpNameCode = "";
                //if (logStatusId != 23)
                //{
                //    nextEmpNameCode = currentStatus.EmpCode + "-" + currentStatus.EmpName;
                //}
                //else
                //{
                //    var nextUserInfo = await userInfo.GetUserInfoByUserId(iOUMaster.userId);
                //    nextEmpNameCode = nextUserInfo.EmpCode + "-" + nextUserInfo.EmpName;
                //}
                //int reqId = 0;
                //for (int i = 0; i < model.ioumasterId.Length; i++)
                //{
                //    var req = await iOUService.GetIOUDetailsByMasterId(Convert.ToInt32(model.ioumasterId[i]));
                //    reqId = req.FirstOrDefault().requisitionDetail.requisitionMasterId;
                //    await requisitionStatusHistory.SaveRequisitionStatusLog(Convert.ToInt32(reqId), 7, Convert.ToInt32(currUserInfo.UserTypeId), currUserInfo.UserId, empNameCode, nextEmpNameCode, model.remarks, logStatusId, "IOUPayment", model.iOUPaymentMasterId, iOUMaster.iouPaymentNo);
                //}


                return Json(statusId);
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }
        public async Task<ActionResult> IOUListForPayment()
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var user = userInfo.GetUserInfo();
            IOUViewModel model = new IOUViewModel
            {
                iOUPayments = await iOUService.GetIOUPaymentByType(3),
                //iOUAdjustments = await iOUService.GetIOUPaymentByType(3),
            };
            return View(model);
        }

        public async Task<ActionResult> IOUPaymentEntry(int id)
        {
            IOUViewModel model = new IOUViewModel
            {
                iOUPayment = await iOUService.GetIOUPaymentById(id),
            };
            model.iOUDetails = await iOUService.GetIOUDetailsByMasterId(Convert.ToInt32(model.iOUPayment.IOUId));
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> SaveIOUPaymentStatus(int? IOUPayId, decimal? paymentAmount, int? receivebyId, DateTime? ReceiveDate, string PaymentMode, string ChequeNo, string bankName)
        {
            try
            {
                await iOUService.UpdateIOUPaymentForReceivedPayment(Convert.ToInt32(IOUPayId), paymentAmount, receivebyId, ReceiveDate, PaymentMode, ChequeNo, bankName);
                return Json(IOUPayId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region SP Disbarse
        public async Task<IActionResult> DisbarsePayment(int? id)
        {
            string userName = HttpContext.User.Identity.Name;

            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var user = userInfo.GetUserInfo();
            ViewBag.masterId = 0;
            IOUPaymentMasterViewModel model = new IOUPaymentMasterViewModel
            {
                //iOUMasters = await iOUService.GetIOUMasterForPayment(),
                iOUMasters = await iOUService.GetIOUMasterByUserNameNDateTime(userName, DateTime.Now.ToString(), DateTime.Now.ToString()),
                autoVoucherNames = await costCentreService.GetAllAutoVoucherName(),
            };
            ViewBag.IOUNO = await iOUService.GetIOUPaymentNo();

            var allVoucherlogs = await voucherService.GetIOUVoucherLog();
            int[] ids = allVoucherlogs.Select(x => Convert.ToInt32(x.IOUId)).Distinct().ToArray();
            if (model.iOUMasters.Count() > 0)
            {
                model.iOUMasters = model.iOUMasters.Where(x => !ids.Contains(x.Id));
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SaveDisbarsePayment([FromForm] IOUPaymentMasterViewModel model)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                var userInfos = await userInfo.GetUserInfoByUser(userName);
                var salaryYear = await salaryService.GetAllSalaryYear();
                var taxYear = await salaryService.GetAllTaxYear();
                var company = await eRPCompanyService.GetAllCompany();

                var IOUNoInfo = await iOUService.GetIOUPaymentNo();

                VoucherMaster voucherMaster = new VoucherMaster
                {
                    Id = 0,
                    voucherNo = model.voucherNo,
                    refNo = model.refNo,
                    voucherDate = model.voucherDate,
                    voucherTypeId = 6,
                    remarks = model.remarks,
                    //  voucherAmount = model.voucherAmount,
                    voucherAmount = model.grandTotal,
                    isPosted = 1,
                    companyId = company.OrderByDescending(a => a.Id).FirstOrDefault().Id,
                    fiscalYearId = salaryYear.OrderByDescending(a => a.Id).FirstOrDefault().Id,
                    taxYearId = taxYear.OrderByDescending(a => a.Id).FirstOrDefault().Id,
                    fundSourceId = 1,
                    projectId = model.project_Id,
                    chequeNumber = model.chequeNumber,
                    chequeDate = model.chequeDate,
                    autoVoucherNameId = model.autoVoucherNameId

                };
                int VMID = await voucherService.SavevoucherMaster(voucherMaster);

                VoucherDetail vD_Perticular = new VoucherDetail
                {
                    Id = 0,
                    voucherId = VMID,
                    ledgerRelationId = Convert.ToInt32(model.txtLedgerRelationId),
                    transectionModeId = model.transectionModeId,
                    amount = model.grandTotal,
                    accountName = model.ledgerRelationName, 
                    isPrincAcc = 0,
                    specialBranchUnitId = userInfos?.specialBranchUnitId
                };

                int vid_per = await voucherService.SavevoucherDetail(vD_Perticular);

                VoucherDetail VD_payAccount = new VoucherDetail
                {
                    Id = 0,
                    voucherId = VMID,
                    ledgerRelationId = Convert.ToInt32(model.hdnPaymentAccId),
                    transectionModeId = (model.transectionModeId == 1) ? 2 : (model.transectionModeId == 2) ? 1 : model.transectionModeId,
                    amount = model.grandTotal,
                    accountName = model.txtPaymentAccount,
                    isPrincAcc = 1,
                    specialBranchUnitId = userInfos?.specialBranchUnitId
                };

                int vid_Pay = await voucherService.SavevoucherDetail(VD_payAccount);


                var allIouVouchers = await voucherService.GetIOUVoucherMaster();
                var voucherNo = "IOU-" + DateTime.Now.ToString("ddMMyyyy") + allIouVouchers.Count() + 1;
                IOUVoucherMaster iOUVoucherMaster = new IOUVoucherMaster
                {
                    Id = 0,
                    voucherMasterId = VMID,
                    voucherNo = voucherNo,
                    voucherDate = model.voucherDate,
                    remarks = model.ledgerRelationName,
                    paymentBy = userInfos.UserId,
                    paymentDate = model.iouPaymentDate,
                    status = 1,
                    autoVoucherNameId = model.autoVoucherNameId,
                    ledgerRelationId = model.ledgerRelationId,
                    subLedgerRelationId = Convert.ToInt32(model.txtLedgerRelationId)

                };
                int iOUVoucherMasterID = await voucherService.SaveIOUvoucherMaster(iOUVoucherMaster);

                for (int i = 0; i < model.ioumasterId.Length; i++)
                {
                    IOUVoucherLog iOUVoucherLog = new IOUVoucherLog
                    {
                        Id = 0,
                        IOUVoucherMasterId = iOUVoucherMasterID,
                        IOUId = model.ioumasterId[i],
                        iouAmount = model.iouValue[i],
                        paymentAmount = model.subTotal[i]
                    };
                    await voucherService.SaveIOUVoucherLog(iOUVoucherLog);
                }

                return Json(VMID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }


        [HttpGet]
        public async Task<IActionResult> AllDisbarseVoucherList()
        {
            string username = HttpContext.User.Identity.Name;
            var userinfo = await userInfo.GetUserInfoByUser(username);
            string userName = userinfo?.UserName;
            int? branchid = 0;
            var model = new VoucherMasterViewModel();
            ViewBag.isPIU = false;

            if (userName == "biplab" || userName == "suza")
            {
                branchid = 0;
                model.voucherLists = await voucherService.GetAllAutotVoucherMasterDetails();
            }
            else if (userName == "piu")
            {
                ViewBag.isPIU = true;
                model.voucherLists = await voucherService.GetAllAutotVoucherMasterDetails();

            }
            else
            {
                branchid = userinfo?.specialBranchUnitId;
                model.voucherLists = await voucherService.GetAutoVoucherMasterDetailsBybranchUnitWise(6, Convert.ToInt32(branchid));

            }

            return View(model);
        }

        public async Task<IActionResult> DisbarseDetails(int id)
        {
            try
            {
                ViewBag.masterId = id;
                var data = await voucherService.GetvoucherDetailByMasterId(id);
                var ledgerRelationId = data.FirstOrDefault().ledgerRelationId;
                var dataLedger = await ledgerService.GetledgerRelationByLedgerRelationId(ledgerRelationId);
                //var companyId = dataLedger.FirstOrDefault().ledger.companyId;
                //var CommentInfo = await attachmentCommentService.GetCommentByActionTypeId(id, "Voucher", 4);
                //if (CommentInfo == null)
                //{
                //    CommentInfo = new List<Comment>();
                //}
                //var photoInfo = await attachmentCommentService.GetDocumentAttachmentByActionId(id, "Voucher", "photo", 4);
                //if (photoInfo == null)
                //{
                //    photoInfo = new List<DocumentPhotoAttachment>();
                //}
                //var docInfo = await attachmentCommentService.GetDocumentAttachmentByActionId(id, "Voucher", "Document", 4);
                //if (docInfo == null)
                //{
                //    docInfo = new List<DocumentPhotoAttachment>();
                //}
                //var costInf = await voucherService.GetCostCentreAllocationsByDetailId(id);
                //if (costInf == null)
                //{
                //    costInf = new List<CostCentreAllocation>();
                //}

                var model = new VoucherMasterViewModel
                {
                    GetGetvoucherMasterById = await voucherService.GetGetvoucherMasterById(id),
                    VoucherDetails = await voucherService.GetvoucherDetailByMasterId(id),
                    GetVoucherDetailsByMasterId = await voucherService.GetvoucherDetailByMasterId(id),
                    //company = await ledgerService.Company(Convert.ToInt32(companyId)),
                    //costCentreAllocations = costInf,
                    //comments = CommentInfo,
                    //photoes = photoInfo,
                    //documents = docInfo
                };
                string Val = model.GetVoucherDetailsByMasterId.Where(x => x.transectionModeId == 1).Sum(x => x.amount).ToString();

                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}