
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSAssignment.Helpers;
using OPUSERP.Areas.HRPMSDisciplineInvestigation.Models;
using OPUSERP.Areas.HRPMSDisciplineInvestigation.Models.Lang;
using OPUSERP.Data.Entity;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.Suspensions;
using OPUSERP.HRPMS.Services.DisciplineInvestigation.Interfaces;
using OPUSERP.HRPMS.Services.Employee.Interfaces;

namespace OPUSERP.Areas.HRPMSDisciplineInvestigation.Controllers
{
	[Authorize]
	[Area("HRPMSDisciplineInvestigation")]
	public class AllegationController : Controller
    {
	//	private readonly LangGenerate<DisciplinaryLn> _lang;
		private readonly IDisciplineInvestigation disciplineInvestigation;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IPhotographService photographService;
        private readonly IPersonalInfoService personalInfoService;

        public AllegationController(IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IDisciplineInvestigation disciplineInvestigation, IPhotographService photographService, IPersonalInfoService personalInfoService)
		{
			//_lang = new LangGenerate<DisciplinaryLn>(hostingEnvironment.ContentRootPath);
			this.disciplineInvestigation = disciplineInvestigation;
            this.personalInfoService = personalInfoService;
            this.photographService = photographService;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        // GET: Allegation
        public async Task<ActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            AllegationViewModel model = new AllegationViewModel
            {
                employeeId = id,
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id),
                allegations = await disciplineInvestigation.GetAllAllegation(),
                allegationDetails = await disciplineInvestigation.GetAllegationById(id)

            };
            if (model.allegationDetails == null) model.allegationDetails = new Allegation();
            return View(model);
        }

        // POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] AllegationViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            if (!ModelState.IsValid)
            {
                //ViewBag.employeeID = model.employeeID;
                model.allegations = await disciplineInvestigation.GetAllAllegation();
                return View(model);
            }
            string fileName;

            if (model.allegationUrl != null)
            {
                string message = FileSave.SaveImageDiscipline(out fileName, model.allegationUrl);
            }
            else
            {
                fileName = await disciplineInvestigation.GetAllegationUrlById(model.allegationID ?? 0);
            };

            string fileNameC;

            if (model.clarificationUrl != null)
            {
                string message = FileSave.SaveImageDiscipline(out fileNameC, model.clarificationUrl);
            }
            else
            {
                fileNameC = await disciplineInvestigation.GetClarificationUrlById(model.allegationID ?? 0);
            };

            string fileNameM;

            if (model.managementUrl != null)
            {
                string message = FileSave.SaveImageDiscipline(out fileNameM, model.managementUrl);
            }
            else
            {
                fileNameM = await disciplineInvestigation.GetManagementUrlById(model.allegationID ?? 0);
            };

            Allegation data = new Allegation
            {
                Id = Convert.ToInt32(model.allegationID),
                allegationDetail = model.allegationDetail,
                allegationUrl = fileName,
                clarification = model.clarification,
                clarificationUrl = fileNameC,
                management = model.management,
                managementUrl = fileNameM,
                isActive = 1,
                status = 1,
                employeeId = model.employeeId
            };
            if (roles.Contains("HRAdmin") || roles.Contains("admin"))
            {
                data.isDelete = 0;
            }
            else
            {
                data.isDelete = 1;
            }
            await disciplineInvestigation.SaveAllegation(data);

            return RedirectToAction(nameof(Index));
        }

         public async Task<ActionResult> AllegationList()
        {
            AllegationViewModel model = new AllegationViewModel
            {
                allegations = await disciplineInvestigation.GetAllAllegation()
            };
            return View(model);
        }

        // POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AllegationList([FromForm] AllegationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.allegations = await disciplineInvestigation.GetAllAllegation();
                return View(model);
            }
            string fileName;

            if (model.allegationUrl != null)
            {
                string message = FileSave.SaveImageDiscipline(out fileName, model.allegationUrl);
            }
            else
            {
                fileName = await disciplineInvestigation.GetAllegationUrlById(model.allegationID ?? 0);
            };

            string fileNameC;

            if (model.clarificationUrl != null)
            {
                string message = FileSave.SaveImageDiscipline(out fileNameC, model.clarificationUrl);
            }
            else
            {
                fileNameC = await disciplineInvestigation.GetClarificationUrlById(model.allegationID ?? 0);
            };

            string fileNameM;

            if (model.managementUrl != null)
            {
                string message = FileSave.SaveImageDiscipline(out fileNameM, model.managementUrl);
            }
            else
            {
                fileNameM = await disciplineInvestigation.GetManagementUrlById(model.allegationID ?? 0);
            };

            Allegation data = new Allegation
            {
                Id = Convert.ToInt32(model.allegationID),
                allegationDetail = model.allegationDetail,
                allegationUrl = fileName,
                clarification = model.clarification,
                clarificationUrl = fileNameC,
                management = model.management,
                managementUrl = fileNameM,
                isActive = 1,
                status = 1,
                employeeId = model.employeeId
            };

            await disciplineInvestigation.SaveAllegation(data);

            return RedirectToAction(nameof(AllegationList));
        }




        #region API Section
        //[Route("global/api/allegation/{id}")]
        //[HttpDelete]
        [HttpPost]
        public async Task<IActionResult> DeleteAllegation(int Id)
        {
            await disciplineInvestigation.DeleteAllegationById(Id);
            return Json(true);
        }
        #endregion
    }
}