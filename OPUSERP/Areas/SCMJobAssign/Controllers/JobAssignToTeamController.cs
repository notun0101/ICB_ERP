using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.SCMJobAssign.Models;
using OPUSERP.CRO.Services.DistributeJob.Interfaces;
using OPUSERP.Data.Entity.Matrix;
using OPUSERP.Distribution.Data.Entity.Requisition;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.SCM.Helpers;
using OPUSERP.SCM.Services.MasterData.Interfaces;
using OPUSERP.SCM.Services.Matrix.Interfaces;
using OPUSERP.SCM.Services.PurchaseOrder.Interface;
using OPUSERP.SCM.Services.Requisition.Interfaces;
using OPUSERP.SCM.Services.SCMMail.interfaces;

namespace OPUSERP.Areas.SCMJobAssign.Controllers
{
	[Area("SCMJobAssign")]
	public class JobAssignToTeamController : Controller
	{
		private readonly IStatusLogService statuslogService;
		private readonly IRequisitionService requisitionService;
		private readonly IUserInfoes userInfoes;
		private readonly RequisitionStatusHistory requisitionStatusHistory;
		private readonly ISCMTeamService teamService;
		private readonly ISCMMailService sCMMailService;
		private readonly IPurchaseOrderService purchaseOrderService;
		private readonly IApprovalMatrixService approvalMatrixService;

		public JobAssignToTeamController(IStatusLogService statuslogService, IApprovalMatrixService approvalMatrixService, IRequisitionService requisitionService, IPurchaseOrderService purchaseOrderService, RequisitionStatusHistory requisitionStatusHistory, IUserInfoes userInfoes, ISCMTeamService teamService, ISCMMailService sCMMailService)
		{
			this.approvalMatrixService = approvalMatrixService;
			this.statuslogService = statuslogService;
			this.requisitionService = requisitionService;
			this.purchaseOrderService = purchaseOrderService;
			this.userInfoes = userInfoes;
			this.requisitionStatusHistory = requisitionStatusHistory;
			this.teamService = teamService;
			this.sCMMailService = sCMMailService;
		}

		// GET: JobAssignToTeam
		public async Task<ActionResult> Index()
		{
			JobAssignViewModel model = new JobAssignViewModel
			{
				requisitionMasters = await requisitionService.GetRequisitionMasterListForAssign(),
				assignRequisitionMasters = await requisitionService.GetRequisitionMasterListByStatus(6),
				teamMasters = await teamService.GetAllTeamMaster()
			};
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Index([FromForm] JobAssignViewModel model)
		{
			string userName = HttpContext.User.Identity.Name;
			var currUserInfo = await userInfoes.GetUserInfoByUser(userName);

			if (!ModelState.IsValid || model.masterIds == null)
			{
				model.requisitionMasters = await requisitionService.GetRequisitionMasterListForAssign();
				model.assignRequisitionMasters = await requisitionService.GetRequisitionMasterListByStatus(6);
				model.teamMasters = await teamService.GetAllTeamMaster();
				if (model.masterIds == null)
				{
					ModelState.AddModelError(string.Empty, "You have to assign minimum 1 team");
				}

				return View(model);
			}
			//return Json(model);
			List<int?> lstLeader = new List<int?>();
			for (int i = 0; i < model.masterIds.Length; i++)
			{
				requisitionService.AssignTeamInRequisitionMasterById((int)model.masterIds[i], 6, (int)model.teamIds[i]);

				var requisitionMasters = await requisitionService.GetRequisitionMasterById((int)model.masterIds[i]);

				var teamMasters = await teamService.GetTeamMasterById((int)model.teamIds[i]);
				var nextUserInfo = await userInfoes.GetUserInfoByUserId(teamMasters.leaderId);

				string empNameCode = nextUserInfo.EmpCode + "-" + nextUserInfo.EmpName;
				await requisitionStatusHistory.SaveRequisitionStatusLog((int)model.masterIds[i], 1, Convert.ToInt32(currUserInfo.UserTypeId), currUserInfo.UserId, empNameCode, "", "", 6, "PR", (int)model.masterIds[i], requisitionMasters.reqNo);
				lstLeader.Add(teamMasters.leaderId);
				if (!lstLeader.Contains(teamMasters.leaderId))
				{
					string host = HttpContext.Request.Host.ToString();
					string scheme = Request.Scheme;
					string baseUrl = $"" + scheme + "://" + host + "/Auth/Account/Login";
					await sCMMailService.MailMessage(nextUserInfo.Email, requisitionMasters.reqNo, 6, empNameCode, baseUrl);
				}

			}
			TempData["Success"] = "Assigned Successfully!";
			return RedirectToAction(nameof(Index));
		}


		public async Task<ActionResult> JobAssignToBuyer()
		{
			string userName = HttpContext.User.Identity.Name;
			JobAssignViewModel model = new JobAssignViewModel
			{
				//assignRequisitionMasters = await requisitionService.GetRequisitionMasterListByStatus(6),
				assignRequisitionMasters = await requisitionService.GetRequisitionMasterListForAssign(),
				purchaseOrderMasters = await purchaseOrderService.GetPurchaseOrderMasterForApprove(userName, 1),
				requisitionMasters = await requisitionService.GetRequisitionMasterListByStatus(7)

			};
			if (User.IsInRole("PO Approval") && User.IsInRole("PO Admin"))
			{
				model.purchaseOrderMasters = await purchaseOrderService.GetPurchaseOrderMasterForApprove(userName, 5);
			}
			return View(model);
		}

		public async Task<ActionResult> AssignRequisitionList()
		{
			JobAssignViewModel model = new JobAssignViewModel
			{
				//assignRequisitionMasters = await requisitionService.GetRequisitionMasterListByStatus(6),
				assignRequisitionMasters = await requisitionService.GetRequisitionMasterListForAssign(),
				requisitionMasters = await requisitionService.GetRequisitionMasterListByStatus(7)

			};
			return View(model);
		}
		public async Task<IActionResult> GetAllReqDetails()
		{
			var data = await requisitionService.GetAllRequisitionDetailList();
			return Json(data);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> JobAssignToBuyer([FromForm] JobAssignViewModel model)
		{
			string userName = HttpContext.User.Identity.Name;
			var currUserInfo = await userInfoes.GetUserInfoByUser(userName);

			if (!ModelState.IsValid)
			{
				model.assignRequisitionMasters = await requisitionService.GetRequisitionMasterListByStatus(6);
				return View(model);
			}
			//if (model.rType == 1)
			//{
			//    if (model.rBuyer == 1)
			//    {
			//        for (int i = 0; i < model.reqDetailIds.Length; i++)
			//        {
			//            requisitionService.AssignTeamInRequisitionDetailsById((int)model.reqDetailIds[i], 7, (int)model.singleMemberIds, model.purchaseType);
			//        }
			//        var requisitionDetail = await requisitionService.GetRequisitionDetailById((int)model.reqDetailIds[0]);

			//        var memberUsers = await teamService.GetTeamMemberById((int)model.singleMemberIds);
			//        var nextUserInfo = await userInfoes.GetUserInfoByUserId(memberUsers.memberId);
			//        string empNameCode = nextUserInfo.EmpCode + "-" + nextUserInfo.EmpName;

			//        await requisitionStatusHistory.SaveRequisitionStatusLog(requisitionDetail.requisitionMasterId, 1, Convert.ToInt32(currUserInfo.UserTypeId), currUserInfo.UserId, empNameCode, "", "", 7, "PR", requisitionDetail.requisitionMasterId, requisitionDetail.requisitionMaster.reqNo);

			//        string host = HttpContext.Request.Host.ToString();
			//        string scheme = Request.Scheme;
			//    }
			//    else
			//    {
			//        for (int i = 0; i < model.reqDetailIds.Length; i++)
			//        {
			//            requisitionService.AssignTeamInRequisitionDetailsById((int)model.reqDetailIds[i], 7, (int)model.MemberIds[i], (int)model.purchaseType);
			//        }

			//        var requisitionDetail = await requisitionService.GetRequisitionDetailById((int)model.reqDetailIds[0]);

			//        var memberUsers = await teamService.GetTeamMemberById((int)model.MemberIds[0]);
			//        var nextUserInfo = await userInfoes.GetUserInfoByUserId(memberUsers.memberId);
			//        string empNameCode = nextUserInfo.EmpCode + "-" + nextUserInfo.EmpName;

			//        await requisitionStatusHistory.SaveRequisitionStatusLog(requisitionDetail.requisitionMasterId, 1, Convert.ToInt32(currUserInfo.UserTypeId), currUserInfo.UserId, empNameCode, "", "", 7, "PR", requisitionDetail.requisitionMasterId, requisitionDetail.requisitionMaster.reqNo);

			//        string host = HttpContext.Request.Host.ToString();
			//        string scheme = Request.Scheme;
			//    }
			//}
			//else
			//{
			//    if (model.rBuyer == 1)
			//    {
			//        for (int i = 0; i < model.reqDetailIds.Length; i++)
			//        {
			//            requisitionService.AssignTeamInRequisitionDetailsById((int)model.reqDetailIds[i], 7, (int)model.singleMemberIds, (int)model.mPurchaseType[i]);
			//        }
			//        var requisitionDetail = await requisitionService.GetRequisitionDetailById((int)model.reqDetailIds[0]);

			//        var memberUsers = await teamService.GetTeamMemberById((int)model.singleMemberIds);
			//        var nextUserInfo = await userInfoes.GetUserInfoByUserId(memberUsers.memberId);
			//        string empNameCode = nextUserInfo.EmpCode + "-" + nextUserInfo.EmpName;

			//        await requisitionStatusHistory.SaveRequisitionStatusLog(requisitionDetail.requisitionMasterId, 1, Convert.ToInt32(currUserInfo.UserTypeId), currUserInfo.UserId, empNameCode, "", "", 7, "PR", requisitionDetail.requisitionMasterId, requisitionDetail.requisitionMaster.reqNo);

			//        string host = HttpContext.Request.Host.ToString();
			//        string scheme = Request.Scheme;
			//    }
			//    else
			//    {
			//        for (int i = 0; i < model.reqDetailIds.Length; i++)
			//        {
			//            requisitionService.AssignTeamInRequisitionDetailsById((int)model.reqDetailIds[i], 7, (int)model.MemberIds[i], (int)model.mPurchaseType[i]);
			//        }

			//        var requisitionDetail = await requisitionService.GetRequisitionDetailById((int)model.reqDetailIds[0]);

			//        var memberUsers = await teamService.GetTeamMemberById((int)model.MemberIds[0]);
			//        var nextUserInfo = await userInfoes.GetUserInfoByUserId(memberUsers.memberId);
			//        string empNameCode = nextUserInfo.EmpCode + "-" + nextUserInfo.EmpName;

			//        await requisitionStatusHistory.SaveRequisitionStatusLog(requisitionDetail.requisitionMasterId, 1, Convert.ToInt32(currUserInfo.UserTypeId), currUserInfo.UserId, empNameCode, "", "", 7, "PR", requisitionDetail.requisitionMasterId, requisitionDetail.requisitionMaster.reqNo);

			//        string host = HttpContext.Request.Host.ToString();
			//        string scheme = Request.Scheme;
			//    }
			//}

			for (int i = 0; i < model.reqDetailIds.Length; i++)
			{
				var id = (int)model.reqDetailIds[i];
				var memberid = (int)model.MemberIds[i];
				var proType = (int)model.mProcessType[i];
				var pType = (int)model.mPurchaseType[i];

				requisitionService.AssignTeamInRequisitionDetailsById(id, 7, memberid, proType, pType);
			}

			var requisitionDetail = await requisitionService.GetRequisitionDetailById((int)model.reqDetailIds[0]);

			var memberUsers = await teamService.GetTeamMemberById((int)model.MemberIds[0]);
			var nextUserInfo = await userInfoes.GetUserInfoByUserId(memberUsers.memberId);
			string empNameCode = nextUserInfo.EmpCode + "-" + nextUserInfo.EmpName;

			await requisitionStatusHistory.SaveRequisitionStatusLog(requisitionDetail.requisitionMasterId, 1, Convert.ToInt32(currUserInfo.UserTypeId), currUserInfo.UserId, empNameCode, "", "", 7, "PR", requisitionDetail.requisitionMasterId, requisitionDetail.requisitionMaster.reqNo);

			string host = HttpContext.Request.Host.ToString();
			string scheme = Request.Scheme;

			return RedirectToAction(nameof(JobAssignToBuyer));
		}

		public ActionResult JobReturnToTeam(int id)
		{
			requisitionService.ReturnTeamInRequisitionMasterById(id);
			return RedirectToAction(nameof(JobAssignToBuyer));
		}


		#region API

		[HttpGet]
		public async Task<IActionResult> GetMasterWiseRequisitionDetails(string MasterId)
		{
			var Re_DetailList = await requisitionService.GetAllItemListByRequisitionId(Convert.ToInt32(MasterId));
			return Json(Re_DetailList);
		}

		[Route("global/api/AssignRequisitionListData")]
		public async Task<ActionResult> AssignRequisitionListData()
		{
			JobAssignViewModel model = new JobAssignViewModel
			{
				//assignRequisitionMasters = await requisitionService.GetRequisitionMasterListByStatus(6),
				assignRequisitionMasters = await requisitionService.GetRequisitionMasterListForAssign(),
				requisitionMasters = await requisitionService.GetRequisitionMasterListByStatus(7)

			};
			return Json(model);
		}

		[HttpGet]
		public async Task<IActionResult> GetRequisitionDetailsByMasterId(string MasterId)
		{
			var Re_DetailList = await requisitionService.GetAllItemListByRequisitionMasterId(Convert.ToInt32(MasterId));
			return Json(Re_DetailList);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllItemOfRequisitionDetails()
		{
			return Json(await requisitionService.GetAllItemListByRequisitionMaster());
		}




		public async Task<IActionResult> AssignMemberMasterId(AssignMemberMasterViewModel model)
		{
			await requisitionService.UpdateRequisitionDetasilStatus(model.AmasterId, model.AdetailsId, model.Aassigned);
			return RedirectToAction("AssignRequisitionList");
		}

		[HttpPost]
		public async Task<IActionResult> ReturnPurchaseProcessor(JobAssignViewModel model)
		{
			var checkSeq = await requisitionService.checkReqApproverSeqByUserId(model.reqMasterIds, model.Aassigneds);
			var result = await approvalMatrixService.GetPRApproverPanelList("sabil.haider", 1, 3111);
			var stat = 7;
			if (checkSeq == 0)
			{
				stat = 4;
				var statuslog = new StatusLog
				{
					Id = 0,
					createdBy = "oyzuddin.ahammed",
					remarks = model.Remarkss,
					Status = "PR Returned by 0148-OYZUDDIN AHAMMED and PR is backed to  PR raiser end.",
					requisitionId = model.reqMasterIds,
					statusInfoId = 4,
					empName = "0148-OYZUDDIN AHAMMED"
				};
				await statuslogService.DeleteApprovalLogByReqId(model.reqMasterIds);
				await statuslogService.DeleteStatusLogByReqAndStatusId(model.reqMasterIds, 3);
				await statuslogService.SaveStatusLog(statuslog);
			}
			else
			{
				stat = 2;
				var statuslog = new StatusLog
				{
					Id = 0,
					createdBy = "oyzuddin.ahammed",
					nextEmpName = "0148-OYZUDDIN AHAMMED",
					remarks = model.Remarkss,
					Status = "PR is created by 0186-SYED SABIL HAIDER. Waiting at 0148-OYZUDDIN AHAMMED end for approval.",
					requisitionId = model.reqMasterIds,
					statusInfoId = 4,
					empName = "0186-SYED SABIL HAIDER"
				};
				await statuslogService.SaveStatusLog(statuslog);

				//for (int i = 0; i < result.Count(); i++)
				//{
				//	int isactive = 0;
				//	int nextAppId = 0;
				//	if (i == 1)
				//	{
				//		isactive = 1;
				//		nextAppId = 0;
				//	}
				//	else if (i == 0)
				//	{
				//		isactive = 0;
				//		nextAppId = model.panels[1].userId;
				//	}
				//	else
				//	{
				//		isactive = 0;
				//		nextAppId = 0;
				//	}

				//	ApprovalLog appLog = new ApprovalLog
				//	{
				//		masterId = masterId,
				//		matrixTypeId = 1,
				//		userId = model.panels[i].userId,
				//		sequenceNo = model.panels[i].sequenceNo,
				//		isActive = isactive,
				//		nextApproverId = nextAppId
				//	};
				//	await approvalLogService.SaveApproverLog(appLog);
				//}
			}
			var reqMaster = await requisitionService.GetRequisitionMasterById(model.reqMasterIds);
			reqMaster.statusInfoId = stat;
			reqMaster.isActive = 1;
			await requisitionService.SaveRequisitionMaster(reqMaster);
			return RedirectToAction("JobAssignToBuyer");
		}
		public async Task<IActionResult> ReAssignMemberMasterId(AssignMemberMasterViewModel model)
		{
			await requisitionService.UpdateRequisitionDetasilStatus1(model.AmasterId, model.AdetailsId, model.Aassigned, model.purchaseTypes, model.processTypes);
			return Json("Updated");
		}

		[HttpGet]
		public async Task<IActionResult> GetTeamMemberForJobAssignByUser(string teamId)
		{
			var Re_DetailList = await teamService.GetTeamMembersByTeamMasterId(Convert.ToInt32(teamId));
			return Json(Re_DetailList);
		}

		[HttpGet]
		public IActionResult GetReqApproversByReqMasterId(int masterId)
		{
			var data = purchaseOrderService.GetAllApproverByReqMasterId(masterId);
			return Json(data);
		}
		[HttpGet]
		//public async Task<IActionResult> GetApproverForJobAssignByUser(string teamId)
		//{
		//    var DetailList = await teamService.GetApproverByApproverId(Convert.ToInt32(teamId));
		//    return Json(DetailList);
		//}

		[HttpGet]
		public async Task<IActionResult> GetMasterWiseRequisitionAttachment(string MasterId)
		{
			var Re_DetailList = await requisitionService.GetDocumentAttachmentList(Convert.ToInt32(MasterId));
			return Json(Re_DetailList);
		}

		[Route("global/api/GetRequisitorInfoByRequisitionId/{reqId}")]
		[HttpGet]
		public async Task<IActionResult> GetRequisitorInfoByRequisitionId(int reqId)
		{
			return Json(await requisitionService.GetRequisitorInfoByRequisitionId(reqId));
		}

		#endregion

	}
}