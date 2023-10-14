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
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
    [Authorize]
    [Area("HRPMSEmployee")]
    public class OtherQualificationsController : Controller
    {
        private readonly IPersonalInfoService personalInfoService;
        private readonly IPhotographService photographService;
        private readonly IResultService resultService;
        private readonly IOtherQualificationService otherQualificationService;
        private readonly IOtherQualificationHeadService otherQualificationHeadService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;


        public OtherQualificationsController(IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IPhotographService photographService, IResultService resultService, IOtherQualificationHeadService otherQualificationHeadService, IOtherQualificationService otherQualificationService, IPersonalInfoService personalInfoService)
        {
            this.personalInfoService = personalInfoService;
            this.photographService = photographService;
            this.resultService = resultService;
            this.otherQualificationService = otherQualificationService;
            this.otherQualificationHeadService = otherQualificationHeadService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            var model = new OtherQualificationsViewModel
            {
                employeeID = id,
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                otherQualificationHeads = await otherQualificationHeadService.GetOtherQualificationHead(),
                results = await resultService.GetAllResult(),
                otherQualifications = await otherQualificationService.GetOtherQualificationByEmpId(id),
                employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] OtherQualificationsViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            if (!ModelState.IsValid)
            {
                ViewBag.employeeID = model.employeeID;
                model.otherQualificationHeads = await otherQualificationHeadService.GetOtherQualificationHead();
                model.results = await resultService.GetAllResult();
                model.otherQualifications = await otherQualificationService.GetOtherQualificationByEmpId((int)model.employeeID);
                model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById((int)model.employeeID);
                return View(model);
            }

            OtherQualification data = new OtherQualification
            {
                Id = model.qualificationID ?? 0,
                employeeID = (int)model.employeeID,
                otherQualificationHeadId = model.qualificationHeadId,
                subject = model.subject,
                resultId = model.result,
                instituteName = model.instituteName,
                passingYear = model.passingYear,
                markGrade = model.markGrade,
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
            await otherQualificationService.SaveOtherQualification(data);
            await personalInfoService.UpdateEmployeeinfoById((int)model.employeeID);
            return RedirectToAction("Index", "OtherQualifications", new
            {
                id = (int)model.employeeID
            });
        }

        public async Task<IActionResult> Delete(int id, int empId)
        {
            await otherQualificationService.DeleteOtherQualificationById(id);
            return RedirectToAction("Index", "OtherQualifications", new
            {
                id = empId
            });
        }

        [HttpPost]
        public async Task<IActionResult> OtherQualificationHead(OtherQualificationsViewModel model)
        {
            var data = new OtherQualificationHead
            {
                Id = 0,
                name = model.OtherQualificationHeadName


            };
            await otherQualificationHeadService.SaveOtherQualificationHead(data);
            return Json("saved");
        }

        public async Task<IActionResult> GetAllOtherQualificationHead()
        {
           
            return Json(await otherQualificationHeadService.GetOtherQualificationHead());
        }

    }
}