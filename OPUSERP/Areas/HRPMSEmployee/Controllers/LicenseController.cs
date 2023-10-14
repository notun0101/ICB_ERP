using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OPUSERP.Data.Entity;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
    [Authorize]
    [Area("HRPMSEmployee")]
    public class LicenseController : Controller
    {
        private readonly IDrivingLicenseService drivingLicenseService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly IPhotographService photographService;
        private readonly LangGenerate<License> _lang;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public LicenseController(IHostingEnvironment hostingEnvironment, IPhotographService photographService, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IPersonalInfoService personalInfoService, IDrivingLicenseService drivingLicenseService)
        {
            this.drivingLicenseService = drivingLicenseService;
            this.photographService = photographService;
            this.personalInfoService = personalInfoService;
            _lang = new LangGenerate<License>(hostingEnvironment.ContentRootPath);
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: License
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            var model = new LicenseViewModel
            {
                employeeID = id.ToString(),
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                fLang = _lang.PerseLang("Employee/LicenseEN.json", "Employee/LicenseBN.json", Request.Cookies["lang"]),
                licenses = await drivingLicenseService.GetDrivingLicenseByEmpId(id),
                employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id)
            };
            return View(model);
        }

        // POST: License/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] LicenseViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);
            if (!ModelState.IsValid)
            {
                ViewBag.employeeID = model.employeeID;
                model.licenses = await drivingLicenseService.GetDrivingLicenseByEmpId(Int32.Parse(model.employeeID));
                model.fLang = _lang.PerseLang("Employee/LicenseEN.json", "Employee/LicenseBN.json", Request.Cookies["lang"]);
                return View(model);
            }

            DrivingLicense data = new DrivingLicense
            {
                Id = Int32.Parse(model.licenseId),
                employeeId = Int32.Parse(model.employeeID),
                licenseNumber = model.licenseNumber,
                category = model.licenseCategory,
                placeOfIssue = model.place,
                dateOfIssue = model.dateOfIssue,
                dateOfExpair = model.dateOfExpair
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
            await drivingLicenseService.SaveDrivingLicenseInfo(data);
            await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));
            //return RedirectToAction("Index", "License", new
            //{
            //    // id = Int32.Parse(model.employeeID)
            //    id = model.employeeID
            //});
           // return RedirectToAction("Index", new { id = data.Id });

            return RedirectToAction("Index", "License", new
            {
                id = model.employeeID
            });
        }

        // Delete: Language
        public async Task<IActionResult> Delete(int id, int empId)
        {
            await drivingLicenseService.DeleteDrivingLicenseById(id);
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "License", new
            {
                id = empId
            });
        }

    }
}