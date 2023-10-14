using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
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
    public class PublicationController : Controller
    {
        private readonly LangGenerate<Publication> _lang;
        private readonly IAwardPublicationLanguageService awardPublicationService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly IPhotographService photographService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public PublicationController(IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IPhotographService photographService, IPersonalInfoService personalInfoService, IAwardPublicationLanguageService awardPublicationService)
        {
            _lang = new LangGenerate<Publication>(hostingEnvironment.ContentRootPath);
            this.awardPublicationService = awardPublicationService;
            this.photographService = photographService;
            this.personalInfoService = personalInfoService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: Publication
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            var model = new PublicationViewModel
            {
                employeeID = id.ToString(),
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                fLang = _lang.PerseLang("Employee/PublicationEN.json", "Employee/PublicationBN.json", Request.Cookies["lang"]),
                publications = await awardPublicationService.GetPublicationsByEmpId(id),
                employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id)
            };
            return View(model);
        }

        // POST: Publication/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] PublicationViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            if (!ModelState.IsValid)
            {
                ViewBag.employeeID = model.employeeID;
                model.publications = await awardPublicationService.GetPublicationsByEmpId(Int32.Parse(model.employeeID));
                model.fLang = _lang.PerseLang("Employee/PublicationEN.json", "Employee/PublicationBN.json", Request.Cookies["lang"]);
                return View(model);
            }

            HRPMS.Data.Entity.Employee.Publication data = new HRPMS.Data.Entity.Employee.Publication
            {
                Id = Int32.Parse(model.publicationId),
                employeeId = Int32.Parse(model.employeeID),
                publicationName = model.publicationName,
                publicationType = model.publicationType,
                publicationPlace = model.publicationPlace,
                publicationDate = model.publicationDate

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
            await awardPublicationService.SavePublication(data);
            await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));
            return RedirectToAction(nameof(Index));
        }

        // Delete: Publication
        public async Task<IActionResult> Delete(int id, int empId)
        {
            await awardPublicationService.DeletePublicationById(id);
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "Publication", new
            {
                id = empId
            });
        }

    }
}