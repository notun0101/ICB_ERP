using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
    [Authorize]
    [Area("HRPMSEmployee")]
    public class PreviousEmploymentController : Controller
    {
        private readonly IPersonalInfoService personalInfoService;
        private readonly IPhotographService photographService;
        private readonly IWagesPersonalInfoService wagesPersonalInfoService;
        private readonly IPriviousEmploymentService priviousEmploymentService;
        private readonly IWagesPriviousEmploymentService wagesPriviousEmploymentService;
        private readonly IHRPMSOrganizationTypeService iHRPMSOrganizationTypeService;
        private readonly IDesignationDepartmentService designationDepartmentService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public PreviousEmploymentController(IPersonalInfoService personalInfoService, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IPhotographService photographService, IPriviousEmploymentService priviousEmploymentService, IHRPMSOrganizationTypeService iHRPMSOrganizationTypeService, IWagesPriviousEmploymentService wagesPriviousEmploymentService, IWagesPersonalInfoService wagesPersonalInfoService , IDesignationDepartmentService designationDepartmentService)
        {
            this.personalInfoService = personalInfoService;
            this.photographService = photographService;
            this.priviousEmploymentService = priviousEmploymentService;
            this.iHRPMSOrganizationTypeService = iHRPMSOrganizationTypeService;
            this.wagesPriviousEmploymentService = wagesPriviousEmploymentService;
            this.wagesPersonalInfoService = wagesPersonalInfoService;
            this.designationDepartmentService = designationDepartmentService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            var model = new PreviousEmploymentViewModel
            {
                employeeID = id,
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                priviousEmployments = await priviousEmploymentService.GetPriviousEmploymentByEmpId(id),
                hRPMSOrganizationTypes = await iHRPMSOrganizationTypeService.GetHRPMSOrganizationType(),
                employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id),
                designations = await designationDepartmentService.GetDesignations()
        };
            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] PreviousEmploymentViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            if (!ModelState.IsValid)
            {
                ViewBag.employeeID = model.employeeID;
                model.hRPMSOrganizationTypes = await iHRPMSOrganizationTypeService.GetHRPMSOrganizationType();
                model.priviousEmployments = await priviousEmploymentService.GetPriviousEmploymentByEmpId((int)model.employeeID);
                model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById((int)model.employeeID);
                return View(model);
            }

            PriviousEmployment data = new PriviousEmployment
            {

                Id = model.previousEmploymentID ?? 0,
                employeeID = (int)model.employeeID,
                organizationName = model.organizationName,
                organizationTypeId = model.organizationType,
                companyLocation = model.companyLocation,
                startDate = model.startDate,
                joiningDesignationId = model.joiningDesignationId,
                endDate = model.endDate,
                lastDesignationId = model.lastDesignationId,
                jobTitle = model.jobTitle,
                lastSalary = model.lastSalary,
                lengthOfService = model.lengthOfService,
                responsibilities = model.responsibilities,
                proficiency = model.proficiency,
                achivments = model.achivments,
                employer = model.employer,

                companyName = model.companyName,
                position = model.position,
                department = model.department,
                companyBusiness = model.companyBusiness,



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
            await priviousEmploymentService.SavePriviousEmployment(data);

            return RedirectToAction("Index", "PreviousEmployment", new
            {
                id = (int)model.employeeID
            });
        }

        public async Task<IActionResult> Delete(int id, int empId)
        {
            await priviousEmploymentService.DeletePriviousEmploymentById(id);
            return RedirectToAction("Index", "PreviousEmployment", new
            {
                id = empId
            });
        }

        public async Task<IActionResult> WagesDelete(int id, int empId)
        {
            await wagesPriviousEmploymentService.DeletePriviousEmploymentById(id);
            return RedirectToAction("WagesIndex", "PreviousEmployment", new
            {
                id = empId
            });
        }

    }
}