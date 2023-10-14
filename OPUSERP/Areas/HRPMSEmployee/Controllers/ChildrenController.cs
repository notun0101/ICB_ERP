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
using System.Linq;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
    [Authorize]
    [Area("HRPMSEmployee")]
    public class ChildrenController : Controller
    {
        private readonly LangGenerate<ChildrenLn> _lang;
        private readonly ISpouseChildrenService spouseChildrenService;
        private readonly IPhotographService photographService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly INomineeService nomineeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public ChildrenController(IHostingEnvironment hostingEnvironment, INomineeService nomineeService, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IPhotographService photographService, IPersonalInfoService personalInfoService, ISpouseChildrenService spouseChildrenService)
        {
            _lang = new LangGenerate<ChildrenLn>(hostingEnvironment.ContentRootPath);
            this.spouseChildrenService = spouseChildrenService;
            this.personalInfoService = personalInfoService;
            this.photographService = photographService;
            this.nomineeService = nomineeService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: Children
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            var model = new ChildrenViewModel
            {
                employeeID = id.ToString(),
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                fLang = _lang.PerseLang("Employee/ChildrenEN.json", "Employee/ChildrenBN.json", Request.Cookies["lang"]),
                children = await spouseChildrenService.GetChildrenByEmpId(id),
                child = await spouseChildrenService.GetchildByEmpId1(id),
                occupations = await nomineeService.GetOccupation(),
                degrees = await nomineeService.GetDegree(),
                levelofEducations = await nomineeService.GetLevelofEducation(),
                employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id)
            };
            return View(model);
        }

        // POST: Children/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] ChildrenViewModel model)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                var user = await _userManager.FindByNameAsync(userName);
                var roles = await _userManager.GetRolesAsync(user);
                if (ModelState.IsValid)
                {
                    //go on as normal
                }
                else
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                                           .Where(y => y.Count > 0)
                                           .ToList();
                }


                if (!ModelState.IsValid)
                {
                    ViewBag.employeeID = model.employeeID;
                    model.children = await spouseChildrenService.GetChildrenByEmpId(Int32.Parse(model.childrenID));
                    model.fLang = _lang.PerseLang("Employee/ChildrenEN.json", "Employee/ChildrenBN.json", Request.Cookies["lang"]);
                    return View(model);
                }
                string fileName;

                if (model.childrenPhoto != null)
                {
                    string message = FileSave.SaveImageNew(out fileName, model.childrenPhoto);
                }
                else
                {
                    fileName = await spouseChildrenService.GetChildrenImgUrlById(Int32.Parse(model.childrenID));
                };

                Children data = new Children
                {
                    Id = Int32.Parse(model.childrenID),
                    employeeId = Int32.Parse(model.employeeID),
                    childName = model.childName,
                    childNameBN = model.childNameBN,
                    dateOfBirth = model.dateOfBirth,
                    education = model.education,
                    occupationId = model.occupationId,
                    gender = model.gender,
                    designation = model.designation,
                    organization = model.organization,
                    bin = model.bin,
                    nid = model.nid,
                    bloodGroup = model.bloodGroup,
                    disability = model.disability,
                    nationality = model.nationality,
                    relationship = model.relationship,
                    emailAddressPersonal = model.emailAddressPersonal,
                    presentEducation = model.presentEducation,
                    disablityType = model.disablityType,
                    phone = model.contact,
                    childNo =model.childNo,
                    maritalstatus=model.maritalstatus,

                    imageUrl = fileName
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

                var chilId = await spouseChildrenService.SaveChildren(data);
                await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));
                if (model.childEduId != null)
                {
                    for (int i = 0; i < model.childEduId.Length; i++)
                    {
                        var childEdu = new ChildrenEducation
                        {
                            Id = model.childEduId[i],
                            childrenId = chilId,
                            institution = model.institution[i],
                            levelOfEducationId = model.levelOfEducationId[i],
                            degreeId = model.degreeId[i],
                            majorSubject = model.majorSubject[i],
                            passingYear = model.passingYear[i],
                            resultType = model.resultType[i],
                            result = model.result[i],
                        };
                        if (roles.Contains("HRAdmin") || roles.Contains("admin"))
                        {
                            childEdu.isDelete = 0;
                        }
                        else
                        {
                            childEdu.isDelete = 1;
                            //await employeeInfoService.ChangeEmployeeActivityStatus(model.employeeID, 3);
                        }
                        await spouseChildrenService.SaveChildrenEducation(childEdu);

                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            
            return RedirectToAction("Index", "Children", new
            {
                id = model.employeeID
            });
        }

        public async Task<IActionResult> Delete(int id, int empId)
        {
            await spouseChildrenService.DeleteChildrenById(id);
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "Children", new
            {
                id = empId
            });
        }
        public async Task<IActionResult> GetChildrenEduById(int id)
        {
            var data = await spouseChildrenService.GetChildEduById(id);
            return Json(data);
        }

        public async Task<IActionResult> DeleteChildrenEducation(int id)
        {
            var data = await spouseChildrenService.DeleteChildrenEducationById(id);
            return Json(data);
        }

        public async Task<IActionResult> GetDegreesByLOEId(int id)
        {
            var data = await spouseChildrenService.GetDegreesByLoeId(id);
            return Json(data);
        }

    }
}