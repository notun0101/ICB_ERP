using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.Auth.Models;
using OPUSERP.Areas.SCMPurchaseOrder.Models;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.Helpers;
using OPUSERP.SCM.Data.Entity.PurchaseOrder;
using OPUSERP.SCM.Helpers;
using OPUSERP.SCM.Services.PurchaseOrder.Interface;
using OPUSERP.SCM.Services.PurchaseProcess.Interfaces;
using OPUSERP.SCM.Services.SCMMail.interfaces;

namespace OPUSERP.Areas.SCMPurchaseOrder.Controllers
{
    [Area("SCMPurchaseOrder")]
    public class PurchaseOrderController : Controller
    {
        private readonly IAddressService addressService;
        private readonly IPurchaseProcessService purchaseProcessService;
        private readonly IPurchaseOrderService purchaseOrderService;
        private readonly RequisitionStatusHistory requisitionStatusHistory;
        private readonly IUserInfoes userInfo;
        private readonly ISCMMailService sCMMailService;
        private readonly string rootPath;
        private readonly MyPDF myPDF;

        public PurchaseOrderController(IAddressService addressService, RequisitionStatusHistory requisitionStatusHistory, IHostingEnvironment hostingEnvironment, IConverter converter, IPurchaseProcessService purchaseProcessService, IUserInfoes userInfo, IPurchaseOrderService purchaseOrderService, ISCMMailService sCMMailService)
        {
            this.addressService = addressService;
            this.purchaseProcessService = purchaseProcessService;
            this.purchaseOrderService = purchaseOrderService;
            this.requisitionStatusHistory = requisitionStatusHistory;
            this.userInfo = userInfo;
            this.sCMMailService = sCMMailService;
            myPDF = new MyPDF(hostingEnvironment, converter);
            rootPath = hostingEnvironment.ContentRootPath;
        }
		[HttpPost]
		public async Task<IActionResult> SaveDeliveryStructures(DeliveryStructureViewModel model)
		{
			for (int i = 0; i < model.structureDate.Length; i++)
			{
				var data = new DeliveryStructure
				{
					cSDetailId = model.structureCsDetailId[i],
					DeliveryDate = model.structureDate[i],
					LocationId = model.structureLoc[i],
					Qty = model.structureQty[i]
				};
				await purchaseOrderService.SaveDeliveryStructure(data);
			}
			return Json("Ok");
		}
		// GET: PurchaseOrder
		public async Task<ActionResult> Index()
        {
            string userName = HttpContext.User.Identity.Name;
            PurchaseOrderViewModel model = new PurchaseOrderViewModel
            {
                purchaseOrderMasters = await purchaseOrderService.GetPurchaseOrderMaster(userName),
                issuedpurchaseOrder = await purchaseOrderService.GetIssuedPurchaseOrderMaster(userName)
            };
            return View(model);
        }

        public async Task<ActionResult> CreatePO()
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos =await userInfo.GetUserInfoByUser(userName);
            PurchaseOrderViewModel model = new PurchaseOrderViewModel
            {
                cSMasters = await purchaseProcessService.GetCSMasterListForPO(userInfos.UserId),
                deliveryLocations = await purchaseOrderService.GetDeliveryLocation(),
                deliveryModes = await purchaseOrderService.GetDeliveryMode(),
                paymentModes = await purchaseOrderService.GetPaymentMode(),
                paymentTerms = await purchaseOrderService.GetpaymentTerms(),
                currencies = await purchaseOrderService.GetCurrency(),
            };
            return View(model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] PurchaseOrderViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            //if (!ModelState.IsValid || model.csDetailsall == null)
            //{
            //    model.cSMasters = await purchaseProcessService.GetCSMasterList(userInfos.UserId);
            //    model.deliveryLocations = await purchaseOrderService.GetDeliveryLocation();
            //    model.deliveryModes = await purchaseOrderService.GetDeliveryMode();
            //    model.paymentModes = await purchaseOrderService.GetPaymentMode();
            //    model.paymentTerms = await purchaseOrderService.GetpaymentTerms();
            //    model.currencies = await purchaseOrderService.GetCurrency();

            //    model.purchaseOrderMasters = await purchaseOrderService.GetPurchaseOrderMaster(userName);
            //    model.issuedpurchaseOrder = await purchaseOrderService.GetIssuedPurchaseOrderMaster(userName);

            //    if (model.csDetailsall == null)
            //    {
            //        ModelState.AddModelError(string.Empty,"You have to Add minimum 1 Item");
            //    }
            //    return View(model);
            //}

            var poNoInfo = purchaseOrderService.GetPuerchaseOrderNo((int)model.txtRequsitionId);
            PurchaseOrderMaster data = new PurchaseOrderMaster
            {
                Id = (int)model.PurchaseOrderId,
                poNo = poNoInfo.autoNumber,
                poDate = model.poDate,
                csMasterId = model.txtCsmasterId,
                deliveryDate = model.deliveryDate,
                supplierId= model.txtSupplierId,
                poStatus = 5,
                paymentModeId = model.paymentType,
                paymentTermsId = model.paymenTerm,
                deliveryModeId = model.deliveryType,
                billToLocation = model.billToLocation,
                receiveStatus = 0,
                userId = userInfos.UserId,
                savingAmount = model.savingsAmount,
                savingPercent = model.savingsPercent,
                remarks = model.remarks
            };

            int masterId = await purchaseOrderService.SavePurchaseOrderMaster(data);

            if (model.PurchaseOrderId > 0)
            {
                await purchaseOrderService.DeletePurchaseOrderDetailsByMasterId((int)model.PurchaseOrderId);
                await purchaseOrderService.DeletePOTermAndConditionByMasterId((int)model.PurchaseOrderId);
            }

            POTermAndCondition pOTermAndCondition = new POTermAndCondition
            {
                purchaseId = masterId,
                tarmsType = "Purchase Order",
                description = model.content,
            };

            await purchaseOrderService.SavePOTermAndCondition(pOTermAndCondition);

            for (int i = 0; i < model.csDetailsall.Length; i++)
            {
                PurchaseOrderDetails data1 = new PurchaseOrderDetails
                {
                    purchaseId = masterId,
                    cSDetailId = model.csDetailsall[i],
                    poQty = model.poQntall[i],
                    poRate = model.txtUnitRateall[i],
                    vat = model.txtVatAmountall[i],
                    vatPercent = model.txtVatall[i],
                    tax = model.txtAitAmountall[i],
                    taxPercent = model.txtAitall[i],
                    currencyId = 1,
                    deliveryLocationId= model.txtLocationall[i],
                    otherDeliveryLocation = model.txtOtherLocationall[i],
                };
                await purchaseOrderService.SavePurchaseOrderDetails(data1);
            }

            var cSMaster = await purchaseProcessService.GetCSMasterById(Convert.ToInt32(model.txtCsmasterId));
            string empNameCode = userInfos.EmpCode + "-" + userInfos.EmpName;
            //string nextEmpNameCode = nextUserInfo.EmpCode + "-" + nextUserInfo.EmpName;
            //await requisitionStatusHistory.SaveRequisitionStatusLog(Convert.ToInt32(cSMaster.requisitionId), 9, Convert.ToInt32(userInfos.UserTypeId), userInfos.UserId, empNameCode, "", "", 14, "PO", masterId, poNoInfo.autoNumber);
            await requisitionStatusHistory.SaveRequisitionStatusLog(Convert.ToInt32(cSMaster.requisitionId), 9, Convert.ToInt32(userInfos.UserTypeId), userInfos.UserId, empNameCode, "", "", 14, "Work Order", masterId, poNoInfo.autoNumber);

            //string host = HttpContext.Request.Host.ToString();
            //string scheme = Request.Scheme;
            //string baseUrl = $"" + scheme + "://" + host + "/Auth/Account/Login";
            //await sCMMailService.MailMessage(nextEmail, poNoInfo.autoNumber, logStatusId, empNameCode, baseUrl);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> POListForApprove()
        {
            string userName = HttpContext.User.Identity.Name;
			
            PurchaseOrderViewModel model = new PurchaseOrderViewModel
            {
                purchaseOrderMasters = await purchaseOrderService.GetPurchaseOrderMasterForApprove(userName, 1),
                issuedpurchaseOrder = await purchaseOrderService.GetIssuedPurchaseOrderMaster(userName)
            };
			if (User.IsInRole("PO Approval"))
			{
				model.purchaseOrderMasters = await purchaseOrderService.GetPurchaseOrderMasterForApprove(userName, 5);
			}
			if (User.IsInRole("PO Approval") && User.IsInRole("PO Admin"))
			{
				model.purchaseOrderMasters = await purchaseOrderService.GetPurchaseOrderMasterForApprove(userName, 5);
			}
			return View(model);
        }
		public async Task<IActionResult> POListForApproveFinal()
        {
            string userName = HttpContext.User.Identity.Name;
            PurchaseOrderViewModel model = new PurchaseOrderViewModel
            {
                purchaseOrderMasters = await purchaseOrderService.GetPurchaseOrderMasterForApprove(userName, 1),
                issuedpurchaseOrder = await purchaseOrderService.GetIssuedPurchaseOrderMaster(userName)
            };
            return View(model);
        }
		public async Task<IActionResult> ApprovedWorkOrder()
        {
            string userName = HttpContext.User.Identity.Name;
            PurchaseOrderViewModel model = new PurchaseOrderViewModel
            {
                purchaseOrderMasters = await purchaseOrderService.GetPurchaseOrderMasterForApprove(userName,1),
                issuedpurchaseOrder = await purchaseOrderService.GetIssuedPurchaseOrderMaster(userName)
            };
            return View(model);
        }
		public async Task<IActionResult> AllWorkOrder()
        {
            string userName = HttpContext.User.Identity.Name;
            PurchaseOrderViewModel model = new PurchaseOrderViewModel
            {
                purchaseOrderMasters = await purchaseOrderService.GetPurchaseOrderMasterForAll(userName,1),
                issuedpurchaseOrder = await purchaseOrderService.GetIssuedPurchaseOrderMaster(userName)
            };
            return View(model);
        }
		public async Task<IActionResult> OnGoingWorkOrder()
        {
            string userName = HttpContext.User.Identity.Name;
            PurchaseOrderViewModel model = new PurchaseOrderViewModel
            {
                purchaseOrderMasters = await purchaseOrderService.GetPurchaseOrderMasterForApprove1(userName,1),
                issuedpurchaseOrder = await purchaseOrderService.GetIssuedPurchaseOrderMaster(userName)
            };
            return View(model);
        }
		public async Task<IActionResult> ReturnedWorkOrder()
        {
            string userName = HttpContext.User.Identity.Name;
            PurchaseOrderViewModel model = new PurchaseOrderViewModel
            {
                purchaseOrderMasters = await purchaseOrderService.GetPurchaseOrderMasterForApprove(userName,4),
                issuedpurchaseOrder = await purchaseOrderService.GetIssuedPurchaseOrderMaster(userName)
            };
            return View(model);
        }
		public async Task<IActionResult> RejectedWorkOrder()
        {
            string userName = HttpContext.User.Identity.Name;
            PurchaseOrderViewModel model = new PurchaseOrderViewModel
            {
                purchaseOrderMasters = await purchaseOrderService.GetPurchaseOrderMasterForApprove(userName,-1),
                issuedpurchaseOrder = await purchaseOrderService.GetIssuedPurchaseOrderMaster(userName)
            };
            return View(model);
        }
		//public async Task<IActionResult> AllWorkOrder()
  //      {
  //          string userName = HttpContext.User.Identity.Name;
  //          PurchaseOrderViewModel model = new PurchaseOrderViewModel
  //          {
  //              purchaseOrderMasters = await purchaseOrderService.GetPurchaseOrderMasterForApprove2(userName,3),
  //              issuedpurchaseOrder = await purchaseOrderService.GetIssuedPurchaseOrderMaster(userName)
  //          };
  //          return View(model);
  //      }

        public async Task<IActionResult> ApprovePO(int poId)
        {
            
            PurchaseOrderViewModel model = new PurchaseOrderViewModel
            {
                poMaster = await purchaseOrderService.GetPurchaseOrderInfoMasterById(poId),

                PurchaseOrderDetails = await purchaseOrderService.GetPurchaseDetailInfoByPOId(poId),
                pOTermAndCondition = await purchaseOrderService.GetPOTermAndConditionByPOId(poId)

            };
            return View(model);
        }
		public async Task<IActionResult> ApprovePOFinal(int poId)
        {
            
            PurchaseOrderViewModel model = new PurchaseOrderViewModel
            {
                poMaster = await purchaseOrderService.GetPurchaseOrderInfoMasterById(poId),

                PurchaseOrderDetails = await purchaseOrderService.GetPurchaseDetailInfoByPOId(poId),
                pOTermAndCondition = await purchaseOrderService.GetPOTermAndConditionByPOId(poId)

            };
            return View(model);
        }

		public async Task<IActionResult> ApprovePO1(int poId)
        {
            
            PurchaseOrderViewModel model = new PurchaseOrderViewModel
            {
                poMaster = await purchaseOrderService.GetPurchaseOrderInfoMasterById(poId),

                PurchaseOrderDetails = await purchaseOrderService.GetPurchaseDetailInfoByPOId(poId),
                pOTermAndCondition = await purchaseOrderService.GetPOTermAndConditionByPOId(poId)

            };
            return View(model);
        }

        [HttpPost]
        public JsonResult ApprovePOByPOAdmin(int poId,int status)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                purchaseOrderService.UpdatePOMasterStatusById(poId,status,userName);
                return Json(true);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
        
        #region api

        [Route("global/api/GetPurchaseOrderNo/{reqId}")]
        [HttpGet]
        public IActionResult GetPurchaseOrderNo(int reqId)
        {
            return Json(purchaseOrderService.GetPuerchaseOrderNo(reqId));
        }

        [Route("global/api/GetCSDetailListByMasterId/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetCSDetailListByMasterId(int id)
        {
            return Json(await purchaseProcessService.GetCSSupplierListBycsId(id));
        }

        [Route("global/api/GetPOInfoAndItemInfoDetailsByMasterId/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetPOInfoAndItemInfoDetailsByMasterId(int id)
        {
            return Json(await purchaseProcessService.GetPOMasterDetailsByPOMId(id));
        }

        [Route("global/api/GetCSDetailListBySupplierId/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetCSDetailListBySupplierId(int id)
        {
            return Json(await purchaseProcessService.GetCSDetailListBySupplierId(id));
        }

        [Route("global/api/GetCSDetailListByCSAndSupplierId/{csId}/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetCSDetailListBySupplierId(int csId,int id)
        {
            return Json(await purchaseProcessService.GetCSDetailListByCSAndSupplierId(csId,id));
        }

        [Route("global/api/TestRPTData/{id}")]
        [HttpGet]
        public async Task<IActionResult> TestRPTData(int id)
        {
            var result = await purchaseOrderService.GetRPTPurchaseOrderDetailsByMasterId(id);
            return Json(result);
        }
        #endregion

    }
}