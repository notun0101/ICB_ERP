using OPUSERP.Helpers;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using Microsoft.AspNetCore.Identity;
using OPUSERP.Data.Entity;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
    [Authorize]
    [Area("HRPMSEmployee")]
    public class EducationalQualificationController : Controller
    {
        private readonly LangGenerate<EducationalQualificationLn> _lang;
        private readonly IEmployeeInfoService employeeInfoService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly IPhotographService photographService;
        private readonly ILevelofEducationService levelofEducationService;
        private readonly IDegreeService degreeService;
        private readonly IRelDegreeSubjectService degreeSubjectService;
        private readonly ISubjectService subjectService;
        private readonly IOrganizationService organizationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public EducationalQualificationController(IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IPhotographService photographService, IPersonalInfoService personalInfoService, IEmployeeInfoService employeeInfoService, ILevelofEducationService levelofEducationService, IDegreeService degreeService, IRelDegreeSubjectService degreeSubjectService , ISubjectService subjectService, IOrganizationService organizationService)
        {
            _lang = new LangGenerate<EducationalQualificationLn>(hostingEnvironment.ContentRootPath);
            this.employeeInfoService = employeeInfoService;
            this.personalInfoService = personalInfoService;
            this.photographService = photographService;
            this.levelofEducationService = levelofEducationService;
            this.degreeService = degreeService;
            this.degreeSubjectService = degreeSubjectService;
            this.subjectService = subjectService;
            this.organizationService = organizationService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: EducationQualification
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            var model = new EducationalQualificationViewModel
            {
                employeeId =id,
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                fLang = _lang.PerseLang("Employee/EducationalQualificationEN.json", "Employee/EducationalQualificationBN.json", Request.Cookies["lang"]),
                educationalQualifications = await employeeInfoService.GetEducationalQualificationByEmpId(id),
                employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id),
                levelofeducationNamelist = await levelofEducationService.GetAllLevelofEducation(),
                degreeslist = await degreeService.GetAllDegree(),
                subjectslist = await subjectService.GetAllSubject()
            };
            return View(model);
        }

        // POST: EducationQualification/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] EducationalQualificationViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);
            //if (!ModelState.IsValid)
            //{
            //    ViewBag.employeeID = model.employeeId;
            //    model.educationalQualifications = await employeeInfoService.GetEducationalQualificationByEmpId(model.employeeId);
            //    model.levelofeducationNamelist = await levelofEducationService.GetAllLevelofEducation();
            //    model.fLang = _lang.PerseLang("Employee/EducationalQualificationEN.json", "Employee/EducationalQualificationBN.json", Request.Cookies["lang"]);
            //    return View(model);
            //}

            EducationalQualification data = new EducationalQualification
            {
                Id = model.educationId,
                employeeId = model.employeeId,
                institution = model.institution,
                resultId = model.resultId,
                grade = model.grade,
                passingYear = model.passingYear,
                degreeId = model.degreeId,
				majorGroup = model.majorGroup,
                organizationId = model.organizationId,
				organizationName = model.organizationName,
                reldegreesubjectId = model.reldegreesubjectId
            };
            if (roles.Contains("HRAdmin") || roles.Contains("admin") || roles.Contains("UAT PIMS"))
            {
                data.isDelete = 0;
            }
            else
            {
               data.isDelete = 1;
                //await employeeInfoService.ChangeEmployeeActivityStatus(model.employeeID, 3);
            }
            await employeeInfoService.SaveEducationalQualification(data);
            await personalInfoService.UpdateEmployeeinfoById(model.employeeId);
            return RedirectToAction("Index", "EducationalQualification", new
            {
                id = model.employeeId
            });

        }
        
        // Delete: Language
        public async Task<IActionResult> Delete(int id, int empId)
        {
            await employeeInfoService.DeleteEducationalQualificationById(id);
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "EducationalQualification", new
            {
                id = empId
            });
        }

        [HttpPost]
        public async Task<IActionResult> LevelEducation(EducationalQualificationViewModel model)
        {
            var data = new LevelofEducation
            {
                Id = 0,
                levelofeducationName = model.levelofeducationName
                
            };
            await levelofEducationService.SaveLevelofEducation(data);
            return Json("saved");
        }

        [HttpPost]
        public async Task<IActionResult> Degree(EducationalQualificationViewModel model)
        {
            var data = new Degree
            {
                Id = 0,
                degreeName = model.degreeName,
                levelofeducationId = model.levelofeducationId

            };
            await degreeService.SaveDegree(data);
            return Json("saved");
        }
        [HttpPost]
        public async Task<IActionResult> Subject(EducationalQualificationViewModel model)
        {
            var data = new RelDegreeSubject
            {
                Id = 0,
                degreeId = model.degreeSubjectId,
                subjectId = model.subjectId

            };
            await degreeSubjectService.SaveDegreeSubject(data);
            return Json("saved");
        }

        [HttpPost]
        public async Task<IActionResult> Organization(EducationalQualificationViewModel model)
        {
            var data = new Organization
            {
                Id = 0,
                organizationType = model.organizationType,
                organizationName = model.organizationName,
              

            };
            await organizationService.SaveOrganization(data);
            return Json("saved");
        }

        #region API Section
        [Route("global/api/academicLogById/{id}")]
        [HttpGet]
        public async Task<IActionResult> academicLogById(int id)
        {
            return Json(await employeeInfoService.GetEducationalQualificationByEmpId(id));
        }

		public async Task<IActionResult> GetAllLevelOfEducation()
        {
          //  return Json(await employeeInfoService.GetLevelOfEducationList());
            return Json( await levelofEducationService.GetAllLevelofEducation());
        }
        public async Task<IActionResult> GetAllDegree()
        {
          //  return Json(await employeeInfoService.GetLevelOfEducationList());
            return Json( await degreeService.GetAllDegree());
        }
        public async Task<IActionResult> GetAllOrgs(string type)
        {
            if (type == "board")
            {
                var data = await degreeService.GetAllboard();
                return Json(data);
            }
            else
            {
                var data = await degreeService.GetAllUniversity();
                return Json(data);
            }
        }
        #endregion

    }
}