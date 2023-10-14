using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.Data.Entity;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Services.Employee.Interfaces;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
    [Authorize]
    [Area("HRPMSEmployee")]
    public class OfficeAssignController : Controller
    {
        private readonly IPersonalInfoService personalInfoService;
        private readonly IPhotographService photographService;
        private readonly IWagesPersonalInfoService wagesPersonalInfoService;
        private readonly IWagesReferenceService wagesReferenceService;
        private readonly IReferenceService referenceService;
        private readonly IOfficeAssignService officeAssignService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public OfficeAssignController(IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IPhotographService photographService,IOfficeAssignService officeAssignService, IReferenceService referenceService, IPersonalInfoService personalInfoService, IWagesPersonalInfoService wagesPersonalInfoService, IWagesReferenceService wagesReferenceService)
        {
            this.personalInfoService = personalInfoService;
            this.photographService = photographService;
            this.referenceService = referenceService;
            this.wagesPersonalInfoService = wagesPersonalInfoService;
            this.wagesReferenceService = wagesReferenceService;
            this.officeAssignService = officeAssignService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            var model = new OfficeAssignViewModel
            {
                employeeID = id,
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                officeAssigns = await officeAssignService.GetOfficeAssignByEmpId(id),
                employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id)
            };
            return View(model);
        }

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] OfficeAssignViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);
            if (!ModelState.IsValid)
            {
                ViewBag.employeeID = model.employeeID;
                model.officeAssigns = await officeAssignService.GetOfficeAssignByEmpId((int)model.employeeID);
                model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById((int)model.employeeID);
                return View(model);
            }

           OfficeAssign data = new OfficeAssign
           {
                Id = model.officeAssignID ?? 0,
                employeeInfoId = (int)model.employeeID,
                roomNo = model.roomNo,
                floorNo = model.floorNo,
                deskNo = model.deskNo,
                
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
            await officeAssignService.SaveofficeAssign(data);
            
            return RedirectToAction("Index", "OfficeAssign", new
            {
                id = (int)model.employeeID
            });
        }

        public async Task<IActionResult> Delete(int id, int empId)
        {
            await officeAssignService.DeleteOfficeAssignById(id);
            return RedirectToAction("Index", "OfficeAssign", new
            {
                id = empId
            });
        }

        
    }
}