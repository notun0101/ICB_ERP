using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.SCMMasterData.Models;
using OPUSERP.Areas.SCMRequisition.Models;
using OPUSERP.Data.Entity;
using OPUSERP.SCM.Services.MasterData.Interfaces;
using OPUSERP.SCM.Services.Requisition.Interfaces;

namespace OPUSERP.Areas.SCMRequisition.Controllers
{
	[Area("SCMRequisition")]
    public class RequisitionNewController : Controller
    {
        private readonly IItemsService itemsService;
        private readonly IRequisitionService _requisitionService;
		private readonly UserManager<ApplicationUser> _userManager;

		public RequisitionNewController(IItemsService itemsService, IRequisitionService requisitionService, UserManager<ApplicationUser> userManager)
        {
            this.itemsService = itemsService;
            this._requisitionService = requisitionService;
            this._userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {

            var data = new RequisitionRaiserDashViewModel
            {
                CreateRequisition = await _requisitionService.GetCreatedRequisition("PR Raise"),
                OnGoingRequisition = await _requisitionService.GetCreatedRequisition("Approval process"),
                ApprovedRequisition = await _requisitionService.GetCreatedRequisition("Final Approved"),
                CompleteRequisition = await _requisitionService.GetCreatedRequisition("Final Approved"),
                RejectRequisition = await _requisitionService.GetCreatedRequisition("Reject"),
                itemSpecificationDepartmentModels = await itemsService.GetItemByDepartmentWise(),
                MostRecent = await _requisitionService.GetMostRecentReq(),
                TopTen = await _requisitionService.GetTopTenReq(),
                Featured = await _requisitionService.GetFeaturedReq()
            };

            return View(data);
            //         ItemSpecificationDepartmentModel model = new ItemSpecificationDepartmentModel
            //         {
            //             itemSpecificationDepartmentModels = await itemsService.GetItemByDepartmentWise(),
            //	MostRecent = await _requisitionService.GetMostRecentReq(),
            //	TopTen = await _requisitionService.GetTopTenReq(),
            //	Featured = await _requisitionService.GetFeaturedReq()
            //};
            //         return View(model);
        }

		public async Task<IActionResult> CreateRequisition()
        {
			ItemSpecificationDepartmentModel model = new ItemSpecificationDepartmentModel
			{
				itemSpecificationDepartmentModels = await itemsService.GetItemByDepartmentWise(),
				MostRecent = await _requisitionService.GetMostRecentReq(),
				TopTen = await _requisitionService.GetTopTenReq(),
				Featured = await _requisitionService.GetFeaturedReq()
			};
			return View(model);
		}

		public async Task<IActionResult> RequisitionStockInIndex()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> GetRecentItems(int id)
		{
			var data = await _requisitionService.GetMostRecentReq();
			return Json(data);
		}
		[HttpGet]
		public async Task<IActionResult> GetTopTen(int id)
		{
			var data = await _requisitionService.GetTopTenReq();
			return Json(data);
		}
		[HttpGet]
		public async Task<IActionResult> GetFeatured(int id)
		{
			var data = await _requisitionService.GetFeaturedReq();
			return Json(data);
		}

		[HttpGet]
		public async Task<IActionResult> ApproveRequisitionNew(int id)
		{
			var requisition = await _requisitionService.GetRequisitionMasterById(id);
			var reqDetails = await _requisitionService.GetRequisitionDetailListByReqId(id);
			ItemSpecificationDepartmentModel model = new ItemSpecificationDepartmentModel
			{
				itemSpecificationDepartmentModels = await itemsService.GetItemByDepartmentWise(),
				MostRecent = await _requisitionService.GetMostRecentReq(),
				TopTen = await _requisitionService.GetTopTenReq(),
				Featured = await _requisitionService.GetFeaturedReq(),
				requisitionMaster=requisition,
				requisitionDetails=reqDetails
			};
			return View(model);
		}

		public async Task<IActionResult> StoreKeeper()
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);
			string roleName = "";
			if (roles.Contains("Requisition Raiser"))
			{
				roleName = "Requisition Raiser";
			}
			else
			{
				roleName = "";
			}
			ViewBag.roleName = roleName;
			var model = new RequisitionRaiserDashViewModel
			{
				CreateRequisition = await _requisitionService.GetCreatedRequisition("PR Raise"),
				OnGoingRequisition = await _requisitionService.GetCreatedRequisition("Approval process"),
				ApprovedRequisition = await _requisitionService.GetCreatedRequisition("Final Approved"),
				CompleteRequisition = await _requisitionService.GetCreatedRequisition("Final Approved"),
				RejectRequisition = await _requisitionService.GetCreatedRequisition("Reject"),
				MostRecent = await _requisitionService.GetMostRecentReq(),
				TopTen = await _requisitionService.GetTopTenReq(),
				Featured = await _requisitionService.GetFeaturedReq(),
				//requisitionMasters = await _requisitionService.GetAllRequisitionMasterList()
			};
			return View(model);
		}

		//[Route("api/SCMRequisition/RequsitionNew/RequisitionListForStockIn/{status}")]
		//public async Task<IActionResult> RequisitionListForStockIn(string status)
		//{
		//	string userName = HttpContext.User.Identity.Name;
		//	var result = await _requisitionService.GetRequisitionListByStatusForStockIn(status, userName);

		//	return Json(result);
		//}

		[HttpGet]
		public async Task<IActionResult> RequisitionInfoForStockOut(int id)
		{
			var requisition = await _requisitionService.GetRequisitionMasterById(id);
			var reqDetails = await _requisitionService.GetRequisitionDetailListByReqId(id);
			ItemSpecificationDepartmentModel model = new ItemSpecificationDepartmentModel
			{
				requisitionMaster = requisition,
				requisitionDetails = reqDetails
			};
			return Json(model);
		}

		[HttpPost]
		public async Task<IActionResult> SaveCartItems([FromForm] ItemSpecificationDepartmentModel model)
		{
			return Json(model);
		}

        public IActionResult Items()
        {
            return View();
        }

        public IActionResult Settings()
        {
            return View();
        }
        public IActionResult Supplier()
        {
            return View();
        }
    }
}
