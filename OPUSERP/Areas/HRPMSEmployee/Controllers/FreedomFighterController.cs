using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    public class FreedomFighterController : Controller
    {
        private readonly IPersonalInfoService personalInfoService;
        private readonly IFreedomFighterService freedomFighterService;
        private readonly IPhotographService photographService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public FreedomFighterController(IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IPhotographService photographService, IFreedomFighterService freedomFighterService, IPersonalInfoService personalInfoService)
        {
            this.personalInfoService = personalInfoService;
            this.photographService = photographService;
            this.freedomFighterService = freedomFighterService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            var model = new FreedomFighterViewModel
            {
                employeeID = id,
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                freedomFighter = await freedomFighterService.GetFreedomFighterByEmpId(id),
                employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id),
                freedomFighters = await freedomFighterService.GetFreedomFighterlistByEmpId(id)
            };
            return View(model);
        }


        // POST: Award/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] FreedomFighterViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            if (!ModelState.IsValid)
            {
                ViewBag.employeeID = model.employeeID;
                model.freedomFighter = await freedomFighterService.GetFreedomFighterByEmpId((int)model.employeeID);
                model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById((int)model.employeeID);
                return View(model);
            }

            FreedomFighter data = new FreedomFighter
            {
                Id = model.FFID ?? 0,
                employeeID = (int)model.employeeID,
                number = model.ffNo,
                owner = model.owner,
                relationship = model.relationShip,
                sectorNo = model.sectorNo,
                remarks = model.remark
            };
            if (roles.Contains("HRAdmin") || roles.Contains("admin"))
            {
                data.isDelete = 0;
            }
            else
            {
                data.isDelete = 1;
            }
            await freedomFighterService.SaveFreedomFighter(data);

            return RedirectToAction("Index", "FreedomFighter", new
            {
                id = (int)model.employeeID
            });
        }

        // Delete: FreedomFighter
        public async Task<IActionResult> Delete(int id, int empId)
        {
            await freedomFighterService.DeleteFreedomFighterById(id);
            return RedirectToAction("Index", "FreedomFighter", new
            {
                id = empId
            });
        }
    }
}