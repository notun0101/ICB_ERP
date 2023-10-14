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
using OPUSERP.HRPMS.Services.MasterData.Interfaces;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
    [Authorize]
    [Area("HRPMSEmployee")]
    public class EmployeeRoleController : Controller
    {
        private readonly IPhotographService photographService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IAddressService addressService;


        public EmployeeRoleController(IHostingEnvironment hostingEnvironment, IPhotographService photographService, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, IPersonalInfoService personalInfoService, IAddressService addressService)
        {
            this.photographService = photographService;
            this.personalInfoService = personalInfoService;
            _roleManager = roleManager;
            _userManager = userManager;
            this.addressService = addressService;
        }

        // GET: Role
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.employeeID = id;
            var model = new EmployeeRoleViewModel
            {
                employeeID = id,
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id),
                customRoles = await addressService.GetCustomRoleName(),
                employeeRoles = await addressService.GetEmployeeRoleByEmpId(id),
                employeeRole =await addressService.GetEmployeeRoleByEmpId1(id)


            };
            if (model.employeeRole == null) model.employeeRole = new EmployeeRole();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] EmployeeRoleViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            if (!ModelState.IsValid)
            {
                ViewBag.employeeID = model.employeeID;
                model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(model.employeeID);
                if (model.employeeRole == null) model.employeeRole = new EmployeeRole();
                return View(model);
            }

            EmployeeRole data = new EmployeeRole
            {
                employeeId = model.employeeID,
                Id = model.emproleid,
                startDate = model.startDate,
                endDate = model.endDate,
                roleId = model.roleId,

            };
            if (roles.Contains("HRAdmin") || roles.Contains("admin"))
            {
                data.isDelete = 0;
            }
            else
            {
                data.isDelete = 1;
            }
            await addressService.SaveEmployeeRole(data);
            await personalInfoService.UpdateEmployeeinfoById(model.employeeID);
            return RedirectToAction("Index", "EmployeeRole", new
            {
                id = model.employeeID
            });
        }
        public async Task<IActionResult> Delete(int id, int empId)
        {
            await addressService.DeleteEmployeeRoleById(id);
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "EmployeeRole", new
            {
                id = empId
            });
        }
    }
}