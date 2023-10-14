using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.Data.Entity;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.SCM.Services.MasterData.Interfaces;
using System;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
    [Authorize]
    [Area("HRPMSEmployee")]
    public class VacancyController : Controller
    {
        private readonly IProjectService projectService;
        private readonly ITravelService travelService;
        private readonly ITravelInfoService travelInfoService;
        private readonly IPhotographService photographService;

        private readonly IAddressService addressService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly IStatusService statusService;
        private readonly IDesignationDepartmentService designationDepartmentService;

        private readonly LangGenerate<Travel> _lang;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public VacancyController(IHostingEnvironment hostingEnvironment, IPhotographService photographService, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IPersonalInfoService personalInfoService, IAddressService addressService, ITravelInfoService travelInfoService, ITravelService travelService, IStatusService statusService, IProjectService projectService, IDesignationDepartmentService designationDepartmentService)
        {
            _lang = new LangGenerate<Travel>(hostingEnvironment.ContentRootPath);
            this.travelInfoService = travelInfoService;
            this.photographService = photographService;
            this.travelService = travelService;
            this.addressService = addressService;
            this.personalInfoService = personalInfoService;
            this.statusService = statusService;
            this.projectService = projectService;
            this.designationDepartmentService = designationDepartmentService;

            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: Vacancy
        public async Task<IActionResult> Index( )
        {
            var model = new VacancyViewModel
            {
                vacancies = await travelService.GetVacancy(),
                designations = await designationDepartmentService.GetDesignations(),


            };
            return View(model);
        }

        //POST: Vacancy/Create
        [HttpPost]
        public async Task<IActionResult> Index([FromForm] VacancyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.vacancies = await travelService.GetVacancy();
                model.designations = await designationDepartmentService.GetDesignations();
                return View(model);
            }
          
            var data = new Vacancy
            {
                Id = model.vacancyId,
                designationId =model.designationId,
                type = model.type,
                sanction = model.sanction,
                existing = model.existing,
                vacancy = model.sanction- model.existing,
                //status = model.status,
                //remarks = model.remarks,
           


            };
            await travelInfoService.SaveVacancy(data);

            return RedirectToAction(nameof(Index));
        }

        // Delete: Vacancy
        [HttpPost]
        public async Task<JsonResult> DeleteVacancyById(int Id)
        {
            await travelInfoService.DeleteVacancyById(Id);
            return Json(true);
        }
    }
}