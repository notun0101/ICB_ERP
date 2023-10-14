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
    public class AwardController : Controller
    {
        private readonly LangGenerate<Award> _lang;
        private readonly IAwardPublicationLanguageService awardPublicationService;
        private readonly IPhotographService photographService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AwardController(IHostingEnvironment hostingEnvironment, IPhotographService photographService, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IPersonalInfoService personalInfoService, IAwardPublicationLanguageService awardPublicationService)
        {
            _lang = new LangGenerate<Award>(hostingEnvironment.ContentRootPath);
            this.awardPublicationService = awardPublicationService;
            this.photographService = photographService;
            this.personalInfoService = personalInfoService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: Award
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            var model = new AwardViewModel
            {
                employeeID = id.ToString(),
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                fLang = _lang.PerseLang("Employee/AwardEN.json", "Employee/AwardBN.json", Request.Cookies["lang"]),
                awards = await awardPublicationService.GetAwardsByEmpId(id),
                awardlist = await awardPublicationService.GetAllAward(),
                employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id)
            };
            return View(model);
        }

        // POST: Award/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] AwardViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            if (!ModelState.IsValid)
            {
                ViewBag.employeeID = model.employeeID;
                model.awards = await awardPublicationService.GetAwardsByEmpId(Int32.Parse(model.employeeID));
                model.awardlist = await awardPublicationService.GetAllAward();
                model.fLang = _lang.PerseLang("Employee/AwardEN.json", "Employee/AwardBN.json", Request.Cookies["lang"]);
                return View(model);
            }
            string fileName;
            if (model.awardPhoto != null)
            {
                string message = FileSave.SaveEmpAttachmentNew(out fileName, model.awardPhoto);
            }
            else
            {
                fileName = await awardPublicationService.GetAwardImgUrlById(Int32.Parse(model.awardId));
            };
            EmployeeAward data = new EmployeeAward
            {               
                Id = Int32.Parse(model.awardId),
                employeeId = Int32.Parse(model.employeeID),
                awardId = Convert.ToInt32(model.awardd),
                purpose = model.perpose,
                awardDate= model.txtAwardDate,
                url=fileName

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

            await awardPublicationService.SaveAward(data);
            await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));
            return RedirectToAction(nameof(Index));
        }

        // Delete: Award
        public async Task<IActionResult> Delete(int id, int empId)
        {
            await awardPublicationService.DeleteAwardById(id);
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "Award", new
            {
                id = empId
            });
        }
    }
}