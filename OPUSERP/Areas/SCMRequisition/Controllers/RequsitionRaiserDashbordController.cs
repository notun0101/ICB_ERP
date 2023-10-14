using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.SCMRequisition.Models;
using OPUSERP.Data.Entity;
using OPUSERP.SCM.Services.Requisition.Interfaces;

namespace OPUSERP.Areas.SCMRequisition.Controllers
{
    [Area("SCMRequisition")]
    public class RequsitionRaiserDashbordController : Controller
    {
		public readonly IRequisitionService _requisitionService;
        private readonly UserManager<ApplicationUser> _userManager;
        public RequsitionRaiserDashbordController(IRequisitionService requisitionService, UserManager<ApplicationUser> userManager)
		{
			_requisitionService = requisitionService;
            _userManager = userManager;
		}

		public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Dashboard()
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

		public async Task<IActionResult> OnGoingRequisition()
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

		//[Route("api/SCMRequisition/RequsitionRaiserDashbord/RequisitionList/{status}")]
  //      public async Task<IActionResult> RequisitionList(string status)
  //      {
  //          string userName = HttpContext.User.Identity.Name;
  //          var result = await _requisitionService.GetRequisitionListByStatus(status,userName);

  //          return Json(result);
  //      }
    }
}
