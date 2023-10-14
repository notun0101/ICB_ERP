using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Areas.HRPMSACR.Models;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.ACR.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Areas.HRPMSACR.Models.Lang;
using Microsoft.AspNetCore.Hosting;
using OPUSERP.Helpers;
using ACRLogViewModel = OPUSERP.Areas.HRPMSEmployee.Models.ACRLogViewModel;
using Microsoft.AspNetCore.Authorization;
using OPUSERP.Data.Entity;
using Microsoft.AspNetCore.Identity;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
    [Authorize]
    [Area("HRPMSEmployee")]
    public class ACRLogController : Controller
    {
        private readonly LangGenerate<ACRLn> _lang;
        private readonly IPersonalInfoService personalInfoService;
        private readonly IAcrInfoService acrInfoService;
        private readonly IPhotographService photographService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public ACRLogController(IHostingEnvironment hostingEnvironment, IPhotographService photographService, IPersonalInfoService personalInfoService, IAcrInfoService acrInfoService, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _lang = new LangGenerate<ACRLn>(hostingEnvironment.ContentRootPath);
            this.personalInfoService = personalInfoService;
            this.acrInfoService = acrInfoService;
            this.photographService = photographService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: ACRLog
        public async Task<IActionResult> ACREmployeeList()
        {
            var model = new EmployeeInfoViewModel
            {
                fLang4 = _lang.PerseLang("ACR/ACREmpListEN.json", "ACR/ACREmpListBN.json", Request.Cookies["lang"]),
                allEmployeeInfos = await personalInfoService.GetEmployeeInfo(),
            };
            return View(model);
        }

        public async Task<IActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            var model = new ACRLogViewModel
            {
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                fLang2 = _lang.PerseLang("ACR/ACRDetailsEN.json", "ACR/ACRDetailsBN.json", Request.Cookies["lang"]),
                acrInfos = await acrInfoService.GetAcrInfoByEmpId(id),
                employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] ACRLogViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);
            if (!ModelState.IsValid)
            {
                model.fLang2 = _lang.PerseLang("ACR/ACRDetailsEN.json", "ACR/ACRDetailsBN.json", Request.Cookies["lang"]);
                model.acrInfos = await acrInfoService.GetAcrInfoByEmpId(model.employeeId);
                model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(model.employeeId);
                return View(model);
            }

            AcrInfo data = new AcrInfo
            {
                Id = Convert.ToInt32(model.ID),
                employeeId = Convert.ToInt32(model.employeeId),
                signatoryName = model.signatoryName,
                signatoryDesg = model.signatoryDesignation,
                supervisorName = model.supervisorName,
                supervisorDesg = model.supervisorDesignation,
                startDate = model.startDate,
                endDate = model.endDate,
                remarks = model.remarks,
                year = model.year,
                advanceComment= model.advanceComment,
                score = Convert.ToInt32(model.score),
                supervisorCode = model.supervisorCode,
                signatoryCode = model.signatoryCode,
                finalStatus = model.finalStatus,
                effectiveDate = model.effectiveDate
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
            int lstId = await acrInfoService.SaveACRInfo(data);

            return RedirectToAction(nameof(Index), new { @id = model.employeeId });
        }

        // Delete: Language
        public async Task<IActionResult> Delete(int id, int empId)
        {
            await acrInfoService.DeleteAcrInfoById(id);
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "ACRLog", new
            {
                id = empId
            });
        }


    }
}