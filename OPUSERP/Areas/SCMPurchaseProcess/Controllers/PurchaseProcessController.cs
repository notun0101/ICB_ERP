using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.SCMMatrix.Models;
using OPUSERP.Areas.SCMPurchaseProcess.Models;
using OPUSERP.Data.Entity.Matrix;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.Master;
//using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.SCM.Data.Entity.PurchaseProcess;
using OPUSERP.SCM.Data.Entity.Requisition;
using OPUSERP.SCM.Helpers;
using OPUSERP.SCM.Services.Matrix.Interfaces;
using OPUSERP.SCM.Services.PurchaseProcess.Interfaces;
using OPUSERP.SCM.Services.Requisition.Interfaces;
using OPUSERP.SCM.Services.SCMMail.interfaces;
using OPUSERP.SCM.Services.Supplier.Interface;

namespace OPUSERP.Areas.SCMPurchaseProcess.Controllers
{
    [Area("SCMPurchaseProcess")]
    public class PurchaseProcessController : Controller
    {
        private readonly IPurchaseProcessService purchaseProcessService;
        private readonly IProcurementService procurementService;
        private readonly IRequisitionService requisitionService;
        private readonly IUserInfoes userInfoes;
        private readonly IApprovalMatrixService approvalMatrixService;
        private readonly IApprovalLogService approverLogService;
        private readonly RequisitionStatusHistory requisitionStatusHistory;
        private readonly ISCMMailService sCMMailService;
        private readonly string rootPath;
		private readonly IOrganizationService organizationService;
		private readonly MyPDF myPDF;
		private readonly IERPCompanyService eRPCompanyService;

		public PurchaseProcessController(IPurchaseProcessService purchaseProcessService, RequisitionStatusHistory requisitionStatusHistory
            , IUserInfoes userInfoes, IRequisitionService requisitionService, IProcurementService procurementService, IApprovalMatrixService approvalMatrixService
            , IApprovalLogService approverLogService, IERPCompanyService eRPCompanyService, IOrganizationService organizationService, ISCMMailService sCMMailService, IHostingEnvironment hostingEnvironment, IConverter converter)
        {
            this.purchaseProcessService = purchaseProcessService;
            this.procurementService = procurementService;
            this.requisitionService = requisitionService;
            this.userInfoes = userInfoes;
			this.eRPCompanyService = eRPCompanyService;
			this.organizationService = organizationService;
			this.approvalMatrixService = approvalMatrixService;
            this.requisitionStatusHistory = requisitionStatusHistory;
            this.approverLogService = approverLogService;
            this.sCMMailService = sCMMailService;
            myPDF = new MyPDF(hostingEnvironment, converter);
            rootPath = hostingEnvironment.ContentRootPath;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfo = await userInfoes.GetUserInfoByUser(userName);
            var model = new CSRequisitionList
            {
                requisitionForCSViewModels = await purchaseProcessService.GetRequisitionListForBuyer(userInfo.UserId),
                cSMasters = await purchaseProcessService.GetCSMasterList(userInfo.UserId),
            };
			//model.requisitionForCSViewModels = model.requisitionForCSViewModels.Where(x => x.)
            return View(model);
        }


		[HttpGet]
        public async Task<IActionResult> IndexSingle()
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfo = await userInfoes.GetUserInfoByUser(userName);
            var model = new CSRequisitionList
            {
                requisitionForCSViewModels = await purchaseProcessService.GetRequisitionListForBuyerSingleSource(userInfo.UserId),
                cSMasters = await purchaseProcessService.GetCSMasterList(userInfo.UserId),
            };
            return View(model);
        }
		public async Task<IActionResult> GetReqApprovalMatrixByMasterAndProjectId(int masterId, int projectId)
		{
			var username = HttpContext.User.Identity.Name;
			var data = await purchaseProcessService.GetReqApprovalMatrixByMasterIdAndProjectId(masterId, projectId, username);
			return Json(data);
		}
		public async Task<IActionResult> GetIOUApprovalMatrixByMasterAndProjectId(int masterId, int projectId)
		{
			var username = HttpContext.User.Identity.Name;
			var data = await purchaseProcessService.GetIOUApprovalMatrixByMasterIdAndProjectId(masterId, projectId, username);
			return Json(data);
		}
		public async Task<IActionResult> GetReqApprovalMatrixByMasterAndProjectIdR(int masterId, int projectId)
		{
			var username = await purchaseProcessService.GetUsernameByCsMasterId(masterId);
			var data = await purchaseProcessService.GetReqApprovalMatrixByMasterIdAndProjectId(masterId, projectId, username);
			return Json(data);
		}
        public async Task<IActionResult> GetReqApprovalMatrixByMasterAndProjectIds(int masterId, int projectId)
		{
			var username = await purchaseProcessService.GetUsernameByIOUMasterId(masterId);
            var data = await purchaseProcessService.GetIOUApprovalMatrixByMasterIdAndProjectId(masterId, projectId, username);

            //var data = await purchaseProcessService.GetReqApprovalMatrixByMasterIdAndProjectId(masterId, projectId, username);
            return Json(data);
		}
        public async Task<IActionResult> GetReqApprovalMatrixByMasterAndProjectIdWO(int masterId, int projectId)
		{
			var username = await purchaseProcessService.GetUsernameByWorkOrderMasterId(masterId);
			var data = await purchaseProcessService.GetWOMatrixByMasterIdAndProjectId(masterId, projectId, username);
			return Json(data);
		}
        public async Task<IActionResult> GetReqApprovalMatrixInWorkOrderByMasterAndProjectId(int projectId)
		{
            var username = HttpContext.User.Identity.Name;
            var data = await purchaseProcessService.GetReqApprovalMatrixByMasterIdAndProjectIdInWorkOrder(projectId, username);
            return Json(data);
        }
		public async Task<IActionResult> SaveOrganizationInfo(PurchaseProcessViewModel model)
		{
			var data = new OPUSERP.SCM.Data.Entity.Supplier.Organization
			{
				Id = model.orgId,
				organizationName = model.organizationName,
				orgCode = model.orgCode,
				LicenseNumber = model.LicenseNumber,
				eTinNumber = model.eTinNumber,
				VATNumber = model.VATNumber
			};
			var d = await organizationService.SaveOrganization(data);
			if (d > 0)
			{
				return Json("Saved");
			}
			else
			{
				return Json("Failed");
			}
		}

		public async Task<IActionResult> GetCsDetailsByReqMasterId(int id)
		 {
			//var data = await requisitionService.GetCsDetailsByReqMasterId(id);
			var data = await requisitionService.GetCsDetailsByCsMasterId(id);
			return Json(data);
		}
		public async Task<IActionResult> GetSingleCsDetailsByReqMasterId(int id)
		 {
			//var data = await requisitionService.GetCsDetailsByReqMasterId(id);
			var data = await requisitionService.GetSingleCsDetailsByCsMasterId(id);
			return Json(data);
		}
		// Get: PurchaseProcess
		[HttpGet]
        public async Task<IActionResult> QutaionProcess(int reqId,int projeectId)
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfo = await userInfoes.GetUserInfoByUser(userName);
            ViewBag.reqMasterId = reqId;
            ViewBag.CSNumber = await purchaseProcessService.GetCSNumber();
            var model = new PurchaseProcessViewModel
            {
                projectId = projeectId,
                reqId=reqId,
                requisitionMaster = await requisitionService.GetRequisitionMasterById(reqId),
                requisitionDetails = await requisitionService.GetRequisitionDetailListWithPurchaseTypeByReqId(reqId,1),
                requisitionDetail = await requisitionService.GetRequisitionDetailListWithPurchaseTypeByReq(reqId,3),
                procurementTypes=await procurementService.GetProcurementTypeList(),
                procurementValues=await procurementService.GetProcurementValueList(),
                justificationTypes=await procurementService.GetJustificationTypeList(),
            };
            return View(model);
        }

		[HttpGet]
        public async Task<IActionResult> QutaionProcessEdit(int reqId,int projeectId)
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfo = await userInfoes.GetUserInfoByUser(userName);
            ViewBag.reqMasterId = reqId;
            ViewBag.CSNumber = await purchaseProcessService.GetCSNumber();
			var csMaster = await procurementService.GetCsMasterByReqId(reqId);

			var model = new PurchaseProcessViewModel
            {
                projectId = projeectId,
                reqId=reqId,
                requisitionMaster = await requisitionService.GetRequisitionMasterById(reqId),
                requisitionDetails = await requisitionService.GetRequisitionDetailListWithPurchaseTypeByReqId(reqId,1),
                requisitionDetail = await requisitionService.GetRequisitionDetailListWithPurchaseTypeByReq(reqId,3),
                procurementTypes=await procurementService.GetProcurementTypeList(),
                procurementValues=await procurementService.GetProcurementValueList(),
                justificationTypes=await procurementService.GetJustificationTypeList(),
				cSMaster = csMaster,
				justifications = await procurementService.GetJustificationListByCSMasterId(csMaster.Id)
            };
            return View(model);
        }
		[HttpGet]
        public async Task<IActionResult> SingleQutaionProcessEdit(int reqId,int projeectId)
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfo = await userInfoes.GetUserInfoByUser(userName);
            ViewBag.reqMasterId = reqId;
            ViewBag.CSNumber = await purchaseProcessService.GetCSNumber();
			var csMaster = await procurementService.GetSingleCsMasterByReqId(reqId);

			var model = new PurchaseProcessViewModel
            {
                projectId = projeectId,
                reqId=reqId,
                requisitionMaster = await requisitionService.GetRequisitionMasterById(reqId),
                requisitionDetails = await requisitionService.GetRequisitionDetailListWithPurchaseTypeByReqId(reqId,1),
                requisitionDetail = await requisitionService.GetRequisitionDetailListWithPurchaseTypeByReq(reqId,3),
                procurementTypes=await procurementService.GetProcurementTypeList(),
                procurementValues=await procurementService.GetProcurementValueList(),
                justificationTypes=await procurementService.GetJustificationTypeList(),
				cSMaster = csMaster,
				justifications = await procurementService.GetJustificationListByCSMasterId(csMaster.Id)
            };
            return View(model);
        }

		[HttpGet]
        public async Task<IActionResult> QutaionProcessSingleSource(int reqId,int projeectId)
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfo = await userInfoes.GetUserInfoByUser(userName);
            ViewBag.reqMasterId = reqId;
            ViewBag.CSNumber = await purchaseProcessService.GetCSNumber();
            var model = new PurchaseProcessViewModel
            {
                projectId = projeectId,
                reqId=reqId,
                requisitionMaster = await requisitionService.GetRequisitionMasterById(reqId),
                requisitionDetails = await requisitionService.GetRequisitionDetailListWithPurchaseTypeByReqId(reqId,3),
                requisitionDetail = await requisitionService.GetRequisitionDetailListWithPurchaseTypeByReq(reqId,3),
                procurementTypes=await procurementService.GetProcurementTypeList(),
                procurementValues=await procurementService.GetProcurementValueList(),
                justificationTypes=await procurementService.GetJustificationTypeList(),
            };
            return View(model);
        }

        // POST: PurchaseProcess
        [HttpPost]
        public async Task<JsonResult> QutaionProcess([FromForm] PurchaseProcessViewModel model)
        {
            try
            {
				string userName = HttpContext.User.Identity.Name;
                var userInfo = await userInfoes.GetUserInfoByUser(userName);

                IEnumerable<ApprovalMatrixViewModel> approvarInfo = await approvalMatrixService.GetAllTypeApprovalMatrixByRaiserIdAndTypeId(Convert.ToInt32(model.projectId), 8, userInfo.UserId);
                List<ApprovalMatrixViewModel> lstApproval = approvarInfo.ToList();
                string csNo = await purchaseProcessService.GetCSNumber();
                CSMaster cSMaster = new CSMaster
                {
					Id = model.csMasterId,
                    csNo = csNo,
                    csDate = DateTime.Now.Date,
                    requisitionId = model.reqId,
                    csStatus = 1,
                    expectedDeliveryDate = model.deliveryDate,
                    rfqRefNo = model.rfqReference,
                    userId = userInfo.UserId,
                    procurementTypeId = model.procurementTypeId,
                    procurementValueId = model.procurementValueId
                };
                int csMasterId = await purchaseProcessService.SaveCSMaster(cSMaster);

                if (model.reqDetailsId != null)
                {
                    int index = 0;
                    List<CSDetail> lstDetails = new List<CSDetail>();
                    for (int i = 0; i < model.reqDetailsId.Length; i++)
                    {
                        for (int j = 0; j < model.supplierId.Length; j++)
                        {
                            CSDetail detail = new CSDetail
                            {
                                Id = 0,
                                cSMasterId = csMasterId,
                                requisitionDetailId = model.reqDetailsId[i],
                                itemCategoryId = model.itemCatId[i],
                                currentStatus = 1,
                                supplierId = model.supplierId[j],
                                qty = model.csQty[index]??0,
                                rate = model.csRate[index],
                            };
                            lstDetails.Add(detail);
                            index++;
                            //int csDetailsId = await purchaseProcessService.SaveCSDetailsSingle(detail);
                        }
                    }
                    purchaseProcessService.SaveCSDetails(lstDetails);
                }

                List<ApprovalLog> approvalLogs = new List<ApprovalLog>();
                ApprovalLog approvalLog = new ApprovalLog
                {
                    masterId = csMasterId,
                    matrixTypeId = 8,
                    isActive = 0,
                    nextApproverId = lstApproval[0].nextApproverId,
                    userId = userInfo.UserId,
                    sequenceNo = 0
                };
                approvalLogs.Add(approvalLog);

                for (int i = 0; i < lstApproval.Count(); i++)
                {
                    ApprovalLog log = new ApprovalLog();
                    log.masterId = csMasterId;
                    log.matrixTypeId = 8;
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

                if (model.justifyTypeId.Length > 0)
                {
                    List<Justification> lstJustify = new List<Justification>();
                    for (int i = 0; i < model.justifyTypeId.Length; i++)
                    {
                        Justification justification = new Justification
                        {
                            cSMasterId = csMasterId,
                            justificationTypeId = model.justifyTypeId[i],
                            isJustify = model.isJustify[i],
                            justificationValue = model.justifyValue[i],
                        };
                        lstJustify.Add(justification);
                    }
                    procurementService.SaveJustificationList(lstJustify);
                }

                var nextUserInfo = await userInfoes.GetUserInfoByUserId(lstApproval[0].nextApproverId);
                string empNameCode = userInfo.EmpCode + "-" + userInfo.EmpName;
                string nextEmpNameCode = nextUserInfo.EmpCode + "-" + nextUserInfo.EmpName;
                await requisitionStatusHistory.SaveRequisitionStatusLog(Convert.ToInt32(model.reqId), 2, Convert.ToInt32(userInfo.UserTypeId), userInfo.UserId, empNameCode, nextEmpNameCode, "", 9, "CS", csMasterId, csNo);

                string host = HttpContext.Request.Host.ToString();
                string scheme = Request.Scheme;
                string baseUrl = $"" + scheme + "://" + host + "/Auth/Account/Login";
                //await sCMMailService.MailMessage(nextUserInfo.Email, csNo, 9, empNameCode, baseUrl);

                return Json(csMasterId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

		[HttpPost]
        public async Task<JsonResult> UpdateQutaionProcess([FromForm] PurchaseProcessViewModel model)
        {
            try
            {
				string userName = HttpContext.User.Identity.Name;
                var userInfo = await userInfoes.GetUserInfoByUser(userName);

                IEnumerable<ApprovalMatrixViewModel> approvarInfo = await approvalMatrixService.GetAllTypeApprovalMatrixByRaiserIdAndTypeId(Convert.ToInt32(model.projectId), 8, userInfo.UserId);
                List<ApprovalMatrixViewModel> lstApproval = approvarInfo.ToList();
                string csNo = await purchaseProcessService.GetCSNumber();
				var csMasterData = await purchaseProcessService.GetCSMasterById(model.csMasterId);
				csMasterData.csDate = DateTime.Now.Date;
				csMasterData.csStatus = 1;
				csMasterData.expectedDeliveryDate = model.deliveryDate;
				csMasterData.rfqRefNo = model.rfqReference;
				csMasterData.procurementTypeId = model.procurementTypeId;
				csMasterData.procurementValueId = model.procurementValueId;

                int csMasterId = await purchaseProcessService.UpdateCSMaster(csMasterData);

                if (model.reqDetailsId != null)
                {
                    int index = 0;
                    List<CSDetail> lstDetails = new List<CSDetail>();
                    for (int i = 0; i < model.reqDetailsId.Length; i++)
                    {
                        for (int j = 0; j < model.supplierId.Length; j++)
                        {
                            CSDetail detail = new CSDetail
                            {
                                Id = 0,
                                cSMasterId = csMasterId,
                                requisitionDetailId = model.reqDetailsId[i],
                                itemCategoryId = model.itemCatId[i],
                                currentStatus = 1,
                                supplierId = model.supplierId[j],
                                qty = model.csQty[index]??0,
                                rate = model.csRate[index],
                            };
                            lstDetails.Add(detail);
                            index++;
                            //int csDetailsId = await purchaseProcessService.SaveCSDetailsSingle(detail);
                        }
                    }
                    purchaseProcessService.SaveCSDetails(lstDetails);
                }

                List<ApprovalLog> approvalLogs = new List<ApprovalLog>();
                ApprovalLog approvalLog = new ApprovalLog
                {
                    masterId = csMasterId,
                    matrixTypeId = 8,
                    isActive = 0,
                    nextApproverId = lstApproval[0].nextApproverId,
                    userId = userInfo.UserId,
                    sequenceNo = 0
                };
                approvalLogs.Add(approvalLog);

                for (int i = 0; i < lstApproval.Count(); i++)
                {
                    ApprovalLog log = new ApprovalLog();
                    log.masterId = csMasterId;
                    log.matrixTypeId = 8;
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

                if (model.justifyTypeId.Length > 0)
                {
                    List<Justification> lstJustify = new List<Justification>();
                    for (int i = 0; i < model.justifyTypeId.Length; i++)
                    {
                        Justification justification = new Justification
                        {
                            cSMasterId = csMasterId,
                            justificationTypeId = model.justifyTypeId[i],
                            isJustify = model.isJustify[i],
                            justificationValue = model.justifyValue[i],
                        };
                        lstJustify.Add(justification);
                    }
                    procurementService.UpdateJustificationList(lstJustify);
                }

                var nextUserInfo = await userInfoes.GetUserInfoByUserId(lstApproval[0].nextApproverId);
                string empNameCode = userInfo.EmpCode + "-" + userInfo.EmpName;
                string nextEmpNameCode = nextUserInfo.EmpCode + "-" + nextUserInfo.EmpName;
                await requisitionStatusHistory.SaveRequisitionStatusLog(Convert.ToInt32(model.reqId), 2, Convert.ToInt32(userInfo.UserTypeId), userInfo.UserId, empNameCode, nextEmpNameCode, "", 9, "CS", csMasterId, csNo);

                string host = HttpContext.Request.Host.ToString();
                string scheme = Request.Scheme;
                string baseUrl = $"" + scheme + "://" + host + "/Auth/Account/Login";
                //await sCMMailService.MailMessage(nextUserInfo.Email, csNo, 9, empNameCode, baseUrl);

                return Json(csMasterId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

      
        public async Task<IActionResult> GetAutoOrganizationCode()
		{
			var data = await organizationService.GetAutoOrganizationCode();
			return Json(data);
		}
		[HttpPost]
        public async Task<JsonResult> QutaionProcessDraft([FromForm] PurchaseProcessViewModel model)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                var userInfo = await userInfoes.GetUserInfoByUser(userName);

                IEnumerable<ApprovalMatrixViewModel> approvarInfo = await approvalMatrixService.GetAllTypeApprovalMatrixByRaiserIdAndTypeId(Convert.ToInt32(model.projectId), 8, userInfo.UserId);
                List<ApprovalMatrixViewModel> lstApproval = approvarInfo.ToList();
                string csNo = await purchaseProcessService.GetCSNumber();
                CSMaster cSMaster = new CSMaster
                {
                    csNo = csNo,
                    csDate = DateTime.Now.Date,
                    requisitionId = model.reqId,
                    csStatus = 2,
                    expectedDeliveryDate = model.deliveryDate,
                    rfqRefNo = model.rfqReference,
                    userId = userInfo.UserId,
                    procurementTypeId = model.procurementTypeId,
                    procurementValueId = model.procurementValueId
                };
                int csMasterId = await purchaseProcessService.SaveCSMaster(cSMaster);

                if (model.reqDetailsId != null)
                {
                    int index = 0;
                    List<CSDetail> lstDetails = new List<CSDetail>();
                    for (int i = 0; i < model.reqDetailsId.Length; i++)
                    {
                        for (int j = 0; j < model.supplierId.Length; j++)
                        {
                            CSDetail detail = new CSDetail
                            {
                                Id = 0,
                                cSMasterId = csMasterId,
                                requisitionDetailId = model.reqDetailsId[i],
                                itemCategoryId = model.itemCatId[i],
                                currentStatus = 2,
                                supplierId = model.supplierId[j],
                                qty = model.csQty[index]??0,
                                rate = model.csRate[index],
                            };
                            lstDetails.Add(detail);
                            index++;
                            //int csDetailsId = await purchaseProcessService.SaveCSDetailsSingle(detail);
                        }
                    }
                    purchaseProcessService.SaveCSDetails(lstDetails);
                }

                List<ApprovalLog> approvalLogs = new List<ApprovalLog>();
                ApprovalLog approvalLog = new ApprovalLog
                {
                    masterId = csMasterId,
                    matrixTypeId = 8,
                    isActive = 0,
                    nextApproverId = lstApproval[0].nextApproverId,
                    userId = userInfo.UserId,
                    sequenceNo = 0
                };
                approvalLogs.Add(approvalLog);

                for (int i = 0; i < lstApproval.Count(); i++)
                {
                    ApprovalLog log = new ApprovalLog();
                    log.masterId = csMasterId;
                    log.matrixTypeId = 8;
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

                if (model.justifyTypeId.Length > 0)
                {
                    List<Justification> lstJustify = new List<Justification>();
                    for (int i = 0; i < model.justifyTypeId.Length; i++)
                    {
                        Justification justification = new Justification
                        {
                            cSMasterId = csMasterId,
                            justificationTypeId = model.justifyTypeId[i],
                            isJustify = model.isJustify[i],
                            justificationValue = model.justifyValue[i],
                        };
                        lstJustify.Add(justification);
                    }
                    procurementService.SaveJustificationList(lstJustify);
                }

                var nextUserInfo = await userInfoes.GetUserInfoByUserId(lstApproval[0].nextApproverId);
                string empNameCode = userInfo.EmpCode + "-" + userInfo.EmpName;
                string nextEmpNameCode = nextUserInfo.EmpCode + "-" + nextUserInfo.EmpName;
                await requisitionStatusHistory.SaveRequisitionStatusLog(Convert.ToInt32(model.reqId), 2, Convert.ToInt32(userInfo.UserTypeId), userInfo.UserId, empNameCode, nextEmpNameCode, "", 9, "CS", csMasterId, csNo);

                string host = HttpContext.Request.Host.ToString();
                string scheme = Request.Scheme;
                string baseUrl = $"" + scheme + "://" + host + "/Auth/Account/Login";
                //await sCMMailService.MailMessage(nextUserInfo.Email, csNo, 9, empNameCode, baseUrl);

                return Json(csMasterId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public async Task<JsonResult> UploadCSAttachment(string id, string[] arrayFileAtach)
        {
            string userName = HttpContext.User.Identity.Name;

            if (Request.Form.Files.Count > 0)
            {
                for (int i = 0; i < Request.Form.Files.Count; i++)
                {
                    int _min = 10000;
                    int _max = 99999;
                    Random _rdm = new Random();
                    int rnd = _rdm.Next(_min, _max);

                    string filePath = string.Empty;
                    string fileName = string.Empty;
                    string fileType = string.Empty;

                    IFormFile file = Request.Form.Files[i];
                    fileType = file.ContentType;
                    fileName = rnd + file.FileName;
                    filePath = "wwwroot/Upload/CS/" + fileName;
                    //var fileD = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Attachments\\RoutingFile", fileName);
                    var fileD = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    using (var fileSrteam = new FileStream(fileD, FileMode.Create))
                    {
                        await file.CopyToAsync(fileSrteam);
                    }

                    DocumentAttachment attachment = new DocumentAttachment
                    {
                        Id = 0,
                        masterId = Convert.ToInt32(id),
                        filePath = filePath,
                        fileName = fileName,
                        fileType = fileType,
                        subject = arrayFileAtach[i],
                        matrixTypeId = 2
                    };
                    await requisitionService.SaveDocumentAttachment(attachment);
                }
            }
            return Json(true);
        }

        // Get: PurchaseProcess
        [HttpGet]
        public async Task<IActionResult> RFQQutaionProcess(int rfqId)
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfo = await userInfoes.GetUserInfoByUser(userName);
            ViewBag.reqMasterId = rfqId;
            ViewBag.CSNumber = await purchaseProcessService.GetCSNumber();
            var model = new PurchaseProcessViewModel
            {
                //requisitionMaster = await requisitionService.GetRequisitionMasterById(reqId),
                //requisitionDetails = await requisitionService.GetRequisitionDetailListByReqId(reqId),
                procurementTypes = await procurementService.GetProcurementTypeList(),
                procurementValues = await procurementService.GetProcurementValueList(),
                justificationTypes = await procurementService.GetJustificationTypeList(),
            };
            return View(model);
        }

        #region CS Report
        [AllowAnonymous]
        public async Task<IActionResult> CSReportView(int csId)
        {
           // string userName = HttpContext.User.Identity.Name;
            //var userInfo = await userInfoes.GetUserInfoByUser(uName);

            var model = new CSRequisitionList
            {
                //cSMaster = await purchaseProcessService.GetCSMasterById(csId),
                //quotationDetailsVMs = await purchaseProcessService.GetQuotationDetailView(csId),
                cSDetailsReports = await purchaseProcessService.GetCSDetailInforReport(csId),
				//justifications = await procurementService.GetJustificationListByCSMasterId(csId),
				//approverPanel = await approverLogService.GetNextApproverInfo(uName, csId, 2),
				//approerPanelList = await approverLogService.GetApprovedListByApprovar(userInfo.UserId, csId, 2),
				approerPanelList = await approvalMatrixService.GetPRApproverPanelList1(csId, 8, 3111),
				companies = await eRPCompanyService.GetAllCompany()
			};
            return View(model);
        }
        [AllowAnonymous]
        public IActionResult CSReportPdf(int csId)
        {
            //string userName = HttpContext.User.Identity.Name;
            string fileName;
            string host = HttpContext.Request.Host.ToString();
            string scheme = Request.Scheme;
            string url = string.Empty;
            url = $"" + scheme + "://" + host + "/SCMPurchaseProcess/PurchaseProcess/CSReportView?csId=" + csId +"&uName="+ User.Identity.Name;

           // string status = myPDF.GenerateLandscapePDF(out fileName, url);
            string status = myPDF.GeneratePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        #endregion

        [Route("global/api/GetRequisitionDetailListForBuyer/{reqId}")]
        [HttpGet]
        public async Task<IActionResult> GetRequisitionDetailListForBuyer(int reqId)
        {
			string userName = HttpContext.User.Identity.Name;
            var userInfo = await userInfoes.GetUserInfoByUser(userName);
            var result = await requisitionService.GetRequisitionDetailListForBuyer(reqId, userInfo.UserId);
            return Json(result);
        }

    }
}