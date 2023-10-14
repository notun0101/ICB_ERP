using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.CRM.Services.MasterData.Interfaces;
using OPUSERP.Data.Entity;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.Payroll.Services.Salary.Interfaces;
using System;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
    [Authorize]
    [Area("HRPMSEmployee")]
    public class EmployeeDiseaseController : Controller
    {
       
        private readonly IEmployeeDiseaseService employeeDiseaseService;
        private readonly IPhotographService photographService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly IReligionMunicipalityService religionMunicipalityService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;


        public EmployeeDiseaseController(IHostingEnvironment hostingEnvironment, IPhotographService photographService, IPersonalInfoService personalInfoService, IEmployeeDiseaseService employeeDiseaseService, IReligionMunicipalityService religionMunicipalityService, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
          
            this.employeeDiseaseService = employeeDiseaseService;
            this.photographService = photographService;
            this.personalInfoService = personalInfoService;
            this.religionMunicipalityService = religionMunicipalityService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        #region Employee Disease
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            var model = new EmployeeDiseaseViewModel
            {
                employeeID = id,
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                medicalDiseases = await religionMunicipalityService.GetMedicalDiseases(),
                employeeDiseases = await employeeDiseaseService.GetEmployeeDiseaseByEmpId(id),
               
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] EmployeeDiseaseViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user1 = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user1);

            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            //bool flag = false;

            //try
            //{

            //    int temp = await employeeDiseaseService.IsThisNamePresent((int)model.medicalDiseaseId);
            //    if (temp != 0 && temp != (model.employeeID))
            //    {
            //        ModelState.AddModelError(string.Empty, "This Medical Disease is Already Taken. Please Try Another Medical Disease");
            //        flag = true;
            //    }
            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}
            if (!ModelState.IsValid)
            {
                ViewBag.employeeID = model.employeeID;
                model.medicalDiseases = await religionMunicipalityService.GetMedicalDiseases();
                model.employeeDiseases = await employeeDiseaseService.GetEmployeeDiseaseByEmpId(model.employeeID);
               
                return View(model);
            }

            EmployeeDisease data = new EmployeeDisease
            {
                Id = model.EmployeeDiseaseId,
                employeeInfoId = model.employeeID,
                medicalDiseaseId = model.medicalDiseaseId,
                status = model.status,
                hospitalised=model.hospitalised,
                underTreatment=model.underTreatment,
                vaccinated=model.vaccinated,
                observation=model.observation
               
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
            await employeeDiseaseService.SaveEmployeeDisease(data);
            await personalInfoService.UpdateEmployeeinfoById(model.employeeID);
            return RedirectToAction("Index", "EmployeeDisease", new
            {
                id = model.employeeID
            });
        }

        public async Task<IActionResult> Delete(int id, int empId)
        {
            await employeeDiseaseService.DeleteEmployeeDiseaseById(id);
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "EmployeeDisease", new
            {
                id = empId
            });
        }

        #endregion

       
    }
}