using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSMasterData.Models;
using OPUSERP.Data.Entity.MasterData;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.HRPMS.Data.Entity.Master;

namespace OPUSERP.Areas.HRPMSMasterData.Controllers
{
    [Authorize]
    [Area("HRPMSMasterData")]
    public class JDTypeController : Controller
    {
        // GET: JDTYpe
        private readonly IDesignationDepartmentService designationDepartmentService;

        public JDTypeController(IHostingEnvironment hostingEnvironment, IDesignationDepartmentService designationDepartmentService)
        {
            this.designationDepartmentService = designationDepartmentService;
        }

        // GET: JDTYpe
        public async Task<IActionResult> Index()
        {
            JDTypeViewModel model = new JDTypeViewModel
            {
                departments = await designationDepartmentService.GetDepartment(),
                designations = await designationDepartmentService.GetDesignations(),
                divisions = await designationDepartmentService.GetDivision(),
                jDTypes = await designationDepartmentService.GetJDType()
            };
            return View(model);
        }

        // POST: JDTYpe/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] JDTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.departments = await designationDepartmentService.GetDepartment();
                model.designations = await designationDepartmentService.GetDesignations();
                model.divisions = await designationDepartmentService.GetDivision();
                model.jDTypes = await designationDepartmentService.GetJDType();
                return View(model);
            }

            JDType data = new JDType
            {
                Id = model.JDTypeId,
                JDRoleName = model.JDRoleName,
                roleId = Int32.Parse(model.roleId),
                departmentId = Int32.Parse(model.departmnetTypeId),
                designationId = Int32.Parse(model.designationTypeId),
                divisionId = Int32.Parse(model.divisionTypeId),

            };

            await designationDepartmentService.SaveJDType(data);

            return RedirectToAction(nameof(Index));
        }
        public async Task<JsonResult> DeleteJDType(int Id)
        {
            await designationDepartmentService.DeleteJDTypeById(Id);
            return Json(true);
        }
    }
}


      