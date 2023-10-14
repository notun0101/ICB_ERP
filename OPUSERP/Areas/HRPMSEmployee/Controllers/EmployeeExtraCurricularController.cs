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
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.Data.Entity;
using Microsoft.AspNetCore.Identity;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
    [Authorize]
    [Area("HRPMSEmployee")]
    public class EmployeeExtraCurricularController : Controller
    {
        private readonly IEmployeeExtraCurricularService employeeExtraCurricularService;
        private readonly IExtraCurricularTypeService extraCurricularTypeService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly IPhotographService photographService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public EmployeeExtraCurricularController(IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IEmployeeExtraCurricularService employeeExtraCurricularService, IExtraCurricularTypeService extraCurricularTypeService, IPersonalInfoService personalInfoService, IPhotographService photographService)
        {
            this.employeeExtraCurricularService = employeeExtraCurricularService;
            this.extraCurricularTypeService = extraCurricularTypeService;
            this.personalInfoService = personalInfoService;
            this.photographService = photographService;
            _roleManager = roleManager;
            _userManager = userManager;


        }

        // GET: EmployeeExtraCurricular
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            try
            {
                var model = new EmployeeExtraCurricularViewModel
                {
                    employeeId = id.ToString(),
                    employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                    photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                    extraCurricularType = await extraCurricularTypeService.GetExtraCurricularType(),
                    employeeExtraCurriculars = await employeeExtraCurricularService.GetEmployeeExtraCurricularEmpId(id),

                };
                if (model.employeeExtraCurricular == null) model.employeeExtraCurricular = new EmployeeExtraCurricular();
                return View(model);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        // POST: EmployeeExtraCurricular/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] EmployeeExtraCurricularViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            if (!ModelState.IsValid)
            {
                ViewBag.employeeID = model.employeeId;
                model.extraCurricularType = await extraCurricularTypeService.GetExtraCurricularType();
                //model.photograph = await photographService.GetPhotographByEmpIdAndType(model.employeeId, "profile");
                model.employeeInfo = await personalInfoService.GetEmployeeInfoById(Int32.Parse(model.employeeId));
                model.employeeExtraCurriculars = await employeeExtraCurricularService.GetEmployeeExtraCurricularEmpId(Int32.Parse(model.employeeId));
                return View(model);
            }

            EmployeeExtraCurricular data = new EmployeeExtraCurricular
            {
                Id = (Int32.Parse(model.ExtraCurricularId)),
                employeeId = (Int32.Parse(model.employeeId)),
                extraCurricularTypeId = model.extraCurricularTypeId,
               
                skillLevel = model.skillLevel,
                skillType = model.skillType,
                description = model.description


            };
            if (roles.Contains("HRAdmin") || roles.Contains("admin"))
            {
                data.isDelete = 0;
            }
            else
            {
                data.isDelete = 1;
              
            }
           
           
            await employeeExtraCurricularService.SaveEmployeeExtraCurricular(data);
            await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeId));
            return RedirectToAction("Index", "EmployeeExtraCurricular", new
            {
                id = model.employeeId
            });

        }


        public async Task<IActionResult> Delete(int id, int empId)
        {
            await employeeExtraCurricularService.DeleteEmployeeExtraCurricularId(id);
            return RedirectToAction("Index", "EmployeeExtraCurricular", new
            {
                id = empId
            });
        }

    }
}