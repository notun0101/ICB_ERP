using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Services.Employee.Interfaces;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
    [Authorize]
    [Area("HRPMSEmployee")]
    public class FoodLikingController : Controller
    {
        private readonly IPhotographService photographService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public FoodLikingController(IPhotographService photographService, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IPersonalInfoService personalInfoService)
        {
            this.photographService = photographService;
            this.personalInfoService = personalInfoService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            var model = new FoodLikingViewModel
            {
                employeeID = id,
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                foodLiking = await personalInfoService.GetFoodLikingById(id)
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(FoodLikingViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            var data = new FoodLiking
            {
                Id = model.foodLikingId,
                employeeId = model.employeeID,
                vegiterian = model.vegiterian,
                nonVegiterian = model.nonVegiterian
            };
            if (roles.Contains("HRAdmin") || roles.Contains("admin"))
            {
               data.isDelete = 0;
            }
            else
            {
                data.isDelete = 1;
                //await employeeInfoService.ChangeEmployeeActivityStatus(model.employeeID, 3);
            }
            await personalInfoService.SaveFoodLiking(data);
            return RedirectToAction("Index");
        }
    }
}