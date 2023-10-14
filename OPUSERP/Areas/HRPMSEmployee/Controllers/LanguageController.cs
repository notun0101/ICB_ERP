using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OPUSERP.Data.Entity;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
    [Area("HRPMSEmployee")]
    public class LanguageController : Controller
    {
        private readonly LangGenerate<LanguageLn> _lang;
        private readonly IAwardPublicationLanguageService awardPublicationService;
        private readonly IMembershipLanguageService membershipLanguageService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly IPhotographService photographService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public LanguageController(IHostingEnvironment hostingEnvironment, IPhotographService photographService, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IPersonalInfoService personalInfoService, IMembershipLanguageService membershipLanguageService, IAwardPublicationLanguageService awardPublicationService)
        {
            _lang = new LangGenerate<LanguageLn>(hostingEnvironment.ContentRootPath);
            this.awardPublicationService = awardPublicationService;
            this.membershipLanguageService = membershipLanguageService;
            this.personalInfoService = personalInfoService;
            this.photographService = photographService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: Language
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            var model = new LanguageViewModel
            {
                employeeID = id.ToString(),
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                fLang = _lang.PerseLang("Employee/LanguageEN.json", "Employee/LanguageBN.json", Request.Cookies["lang"]),
                employeeLanguages = await awardPublicationService.GetLanguageByEmpId(id),
                languages =  await membershipLanguageService.GetLanguageInfo(),
                employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id)
            };
            return View(model);
        }


        // POST: Language/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] LanguageViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            if (!ModelState.IsValid)
            {
                ViewBag.employeeID = model.employeeID;
                model.employeeLanguages = await awardPublicationService.GetLanguageByEmpId(Int32.Parse(model.employeeID));
                model.fLang = _lang.PerseLang("Employee/LanguageEN.json", "Employee/LanguageBN.json", Request.Cookies["lang"]);
                model.languages = await membershipLanguageService.GetLanguageInfo();
                return View(model);
            }

            EmployeeLanguage data = new EmployeeLanguage
            {
                Id = Int32.Parse(model.languageId),
                employeeId = Int32.Parse(model.employeeID),
                reading = model.reading,
                writing = model.writing,
                speaking = model.speaking,
                listening= model.listening,
                languageId = Int32.Parse(model.language),
                proficiency = model.proficiency

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
            await awardPublicationService.SaveLanguage(data);
            await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));
            return RedirectToAction(nameof(Index));
        }

        // Delete: Language
        public async Task<IActionResult> Delete(int id, int empId)
        {
            await awardPublicationService.DeleteLanguageById(id);
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "Language", new
            {
                id = empId
            });
        }

    }
}