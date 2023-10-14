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
using OPUSERP.HRPMS.Services.SuspensionReport.Interfaces;

namespace OPUSERP.Areas.HRPMSDisciplineInvestigation.Controllers
{
	[Authorize]
	[Area("HRPMSDisciplineInvestigation")]
	public class SuspensionController : Controller
    {
		private readonly IDisciplineInvestigation disciplineInvestigation;
		private readonly ISuspension suspensionReportService;
        private readonly IPhotographService photographService;
        private readonly IPersonalInfoService personalInfoService;
        //private readonly LangGenerate<DisciplinaryLn> _lang;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public SuspensionController(IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IDisciplineInvestigation disciplineInvestigation, ISuspension suspensionReportService, IPhotographService photographService, IPersonalInfoService personalInfoService)
		{
			//_lang = new LangGenerate<DisciplinaryLn>(hostingEnvironment.ContentRootPath);
			this.disciplineInvestigation = disciplineInvestigation;
            this.suspensionReportService = suspensionReportService;
            this.photographService = photographService;
            this.personalInfoService = personalInfoService;
            _roleManager = roleManager;
            _userManager = userManager;

        }
        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            SuspensionViewModel model = new SuspensionViewModel
            {
                //suspensionInfo = await suspensionReportService.GetSuspensionById(id),
                //supensionDetails = await suspensionReportService.GetAllSuspension(),
                employeeId = id,
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id),
                supensionDetails = await suspensionReportService.GetSuspensionsByEmployeeId(id),
                supensionDetail = await suspensionReportService.GetSuspensionById(id)
            };
            if (model.supensionDetail == null) model.supensionDetail = new Suspension();
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] SuspensionViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            if (!ModelState.IsValid)
            {
                model.supensionDetails = await suspensionReportService.GetAllSuspension();
                return View(model);
            }
            string fileName;

            if (model.suspensionFile != null)
            {
                string message = FileSave.SaveEmpAttachmentNew(out fileName, model.suspensionFile);

            }
            else
            {
                if (model.suspensionID != null)
                {
                    fileName = await suspensionReportService.GetsuspensionImgUrlById(Int32.Parse(model.suspensionID));
                }
                else
                {
                    fileName = "";
                }
              
            };
            string chargeFileName;
            if (model.chargeFile != null)
            {
                string message = FileSave.SaveEmpAttachmentNew(out chargeFileName, model.chargeFile);

            }
            else
            {
                if (model.suspensionID != null)
                {
                    chargeFileName = await suspensionReportService.GetChargeImgUrlById(Int32.Parse(model.suspensionID));
                }
                else
                {
                    chargeFileName = "";
                }

            };
            string hearingFileName;
            if (model.hearingRepoFile != null)
            {
                string message = FileSave.SaveEmpAttachmentNew(out hearingFileName, model.hearingRepoFile);

            }
            else
            {
                if (model.suspensionID != null)
                {
                    hearingFileName = await suspensionReportService.GetHearingImgUrlById(Int32.Parse(model.suspensionID));
                }
                else
                {
                    hearingFileName = "";
                }

            };

            Suspension data = new Suspension
            {
                Id = Convert.ToInt32(model.suspensionID),
                susDesc = model.susDesc,
                hearingReport = model.hearingReport,
                chargeSheetDesc = model.chargeSheetDesc,
                punishmentType = model.punishmentType,
                effectiveForm = model.effectiveForm,
                status = 1,
                isActive = 1,
                employeeId = model.employeeId
            };
            if (fileName != null)
            {
                data.susFile = fileName;
            }
            else
            {
                data.susFile = "";
            }
            if (chargeFileName != null)
            {
                data.chargeSheetFile = chargeFileName;
            }
            else
            {
                data.chargeSheetFile = "";
            }
            if (hearingFileName != null)
            {
                data.hearingReportFile = hearingFileName;
            }
            else
            {
                data.hearingReportFile = "";
            }
            if (roles.Contains("HRAdmin") || roles.Contains("admin"))
            {
                data.isDelete = 0;
            }
            else
            {
                data.isDelete = 1;
                //await employeeInfoService.ChangeEmployeeActivityStatus(model.employeeID, 3);
            }
            await suspensionReportService.SaveSuspension(data);
            return RedirectToAction(nameof(Index));

        }
        public async Task<JsonResult> DeleteSuspension(int Id)
        {
            await suspensionReportService.DeleteSuspensionById(Id);
            return Json(true);
        }
    }
}