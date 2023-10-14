using DinkToPdf.Contracts;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Services.DisciplineInvestigation.Interfaces;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OPUSERP.Data.Entity;
using System.IO;
using OPUSERP.ERPServices.MasterData.Interfaces;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
    [Authorize]
    [Area("HRPMSEmployee")]
    public class DisciplinaryController : Controller
    {
        private readonly LangGenerate<Disciplinary> _lang;
        private readonly IERPCompanyService eRPCompanyService;
        private readonly IDisciplineInvestigation disciplineInvestigation;
        private readonly IDisciplinaryService disciplinaryService;
        private readonly IPhotographService photographService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly string rootPath;
        private readonly MyPDF myPDF;

        public DisciplinaryController(IERPCompanyService eRPCompanyService, IHostingEnvironment hostingEnvironment, IPhotographService photographService, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IDisciplineInvestigation disciplineInvestigation, IDisciplinaryService disciplinaryService, IPersonalInfoService personalInfoService, IConverter converter)
        {
            _lang = new LangGenerate<Disciplinary>(hostingEnvironment.ContentRootPath);
            this.disciplineInvestigation = disciplineInvestigation;
            this.photographService = photographService;
            this.disciplinaryService = disciplinaryService;
            this.personalInfoService = personalInfoService;
            this.eRPCompanyService = eRPCompanyService;
            _roleManager = roleManager;
            _userManager = userManager;
            //For PDF
            myPDF = new MyPDF(hostingEnvironment, converter);
            rootPath = hostingEnvironment.ContentRootPath;
        }

        // GET: Disciplinary
        public async Task<ActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            var model = new DisciplinaryViewModel
            {
                employeeId = id,
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                fLang = _lang.PerseLang("Employee/DisciplinaryEN.json", "Employee/DisciplinaryBN.json", Request.Cookies["lang"]),
                offenses = await disciplineInvestigation.GetAllOffense(),
                punishments = await disciplineInvestigation.GetAllPunishment(),
                disciplinaries = await disciplinaryService.GetDisciplinaryLogByEmpId(id),
                employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id)
            };
            return View(model);
        }

        // POST: Disciplinary/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index([FromForm] DisciplinaryViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            if (!ModelState.IsValid)
            {
                ViewBag.employeeID = model.employeeId;
                model.fLang = _lang.PerseLang("Employee/DisciplinaryEN.json", "Employee/DisciplinaryBN.json", Request.Cookies["lang"]);
                model.offenses = await disciplineInvestigation.GetAllOffense();
                model.punishments = await disciplineInvestigation.GetAllPunishment();
                model.disciplinaries = await disciplinaryService.GetDisciplinaryLogByEmpId(model.employeeId);
                model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(model.employeeId);
                return View(model);
            }

            string fileName = "";
            if (model.goAttachment != null)
            {
                FileSave.SaveFile(out fileName, model.goAttachment, "DecActionFiles");
            }
            DisciplinaryLog data = new DisciplinaryLog
            {
                Id = Convert.ToInt32(model.disciplinaryId),
                employeeId = model.employeeId,
                OffenseId = model.offenseId,
                naturalPunishmentId = Convert.ToInt32(model.punishmentId),
                punishmentDate = model.punishmentDate,
                startingDate = model.startFrom,
                endDate = model.endTo,
                goNumberWithDate = model.goNo,
                remarks = model.remarks,
                goFileURL = fileName
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
            await disciplinaryService.SaveDisciplinaryLog(data);

            return RedirectToAction(nameof(Index));

        }

        // Delete: Disciplinary
        public async Task<IActionResult> Delete(int id, int empId)
        {
            await disciplinaryService.DeleteDisciplinaryLogById(id);
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "Disciplinary", new
            {
                id = empId
            });
        }
        [AllowAnonymous]
        public IActionResult pdspdf(int idr)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;

            string url = scheme + "://" + host + "/HRPMSEmployee/InfoView/PrintView1?idr=" + idr;
            string status = myPDF.GeneratePDF1(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [AllowAnonymous]
        public IActionResult DisciplinaryAllEmployeePDF(string type, DateTime toDates)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;

            //string url = scheme + "://" + host + "/HRPMSEmployee/InfoView/PrintView1?idr=" + idr;
            string url = scheme + "://" + host + "/HRPMSEmployee/Disciplinary/DisciplinaryAllEmployeeReport?toDates=" + toDates;
            string status = myPDF.GeneratePDF1(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
       
        }
        [AllowAnonymous]
        public IActionResult DisciplinaryEmployeePDF(string type, int id)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;

            //string url = scheme + "://" + host + "/HRPMSEmployee/InfoView/PrintView1?idr=" + idr;
            string url = scheme + "://" + host + "/HRPMSEmployee/Disciplinary/DisciplinaryEmployeeReport?id=" + id;
            string status = myPDF.GeneratePDF1(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
       
        }

        [AllowAnonymous]
        public async Task<IActionResult> DisciplinaryAllEmployeeReport(DateTime? toDates)
        {
            ViewBag.date = toDates;

            DisciplinaryViewModel model = new DisciplinaryViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                fLang = _lang.PerseLang("Employee/DisciplinaryEN.json", "Employee/DisciplinaryBN.json", Request.Cookies["lang"]),
                offenses = await disciplineInvestigation.GetAllOffense(),
                punishments = await disciplineInvestigation.GetAllPunishment(),
                disciplinaries = await disciplinaryService.GetDisciplinaryLog(),
                //employeeNameCode = await personalInfoService.GetEmployeeNameCodeById()
            };
            return View(model);

        }
        [AllowAnonymous]
        public async Task<IActionResult> DisciplinaryEmployeeReport(int id)
        {
            //ViewBag.date = toDates;

            DisciplinaryViewModel model = new DisciplinaryViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                fLang = _lang.PerseLang("Employee/DisciplinaryEN.json", "Employee/DisciplinaryBN.json", Request.Cookies["lang"]),
                offenses = await disciplineInvestigation.GetOffenseByempId(id),
                punishments = await disciplineInvestigation.GetPunishmentbyId(id),
                disciplinaries = await disciplinaryService.GetDisciplinaryLogbyemployee(id),
                employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id)
        };
            return View(model);

        }

        public async Task<ActionResult> DisciplinaryEmployee()
        {
          //  ViewBag.employeeID = id.ToString();
            var model = new DisciplinaryViewModel
            {
               // employeeId = id,
              //  photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
              //  employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                fLang = _lang.PerseLang("Employee/DisciplinaryEN.json", "Employee/DisciplinaryBN.json", Request.Cookies["lang"]),
                offenses = await disciplineInvestigation.GetAllOffense(),
                punishments = await disciplineInvestigation.GetAllPunishment(),
                disciplinaries = await disciplinaryService.GetDisciplinaryLog(),
               // employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id)
            };
            return View(model);
        }

        // POST: Disciplinary/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisciplinaryEmployee([FromForm] DisciplinaryViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            if (!ModelState.IsValid)
            {
                ViewBag.employeeID = model.employeeId;
                model.fLang = _lang.PerseLang("Employee/DisciplinaryEN.json", "Employee/DisciplinaryBN.json", Request.Cookies["lang"]);
                model.offenses = await disciplineInvestigation.GetAllOffense();
                model.punishments = await disciplineInvestigation.GetAllPunishment();
                model.disciplinaries = await disciplinaryService.GetDisciplinaryLogByEmpId(model.employeeId);
                model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(model.employeeId);
                return View(model);
            }

            string fileName = "";
            if (model.goAttachment != null)
            {
                FileSave.SaveImageNew(out fileName, model.goAttachment);
                // FileSave.SaveFile(out fileName, model.goAttachment, "DecActionFiles");
            }
			else
			{
				fileName = model.goAttachmentExist;
			}
            DisciplinaryLog data = new DisciplinaryLog
            {
                Id = Convert.ToInt32(model.disciplinaryId),
                employeeId = model.employeeId,
                OffenseId = model.offenseId,
                naturalPunishmentId = Convert.ToInt32(model.punishmentId),
                punishmentDate = model.punishmentDate,
                startingDate = model.startFrom,
                endDate = model.endTo,
                goNumberWithDate = model.goNo,
                remarks = model.remarks,
                goFileURL = fileName
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
            await disciplinaryService.SaveDisciplinaryLog(data);

            return RedirectToAction(nameof(DisciplinaryEmployee));

        }

        // Delete: Disciplinary
        public async Task<IActionResult> DisciplinaryEmployeeDelete(int id)
        {
            await disciplinaryService.DeleteDisciplinaryLogById(id);
            return RedirectToAction(nameof(DisciplinaryEmployee));
        }


    }
}