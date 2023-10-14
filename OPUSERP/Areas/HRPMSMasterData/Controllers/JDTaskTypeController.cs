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

namespace OPUSERP.Areas.HRPMSMasterData.Controllers
{
    [Authorize]
    [Area("HRPMSMasterData")]
    public class JDTaskTypeController : Controller
    {
        private readonly IDesignationDepartmentService designationDepartmentService;

        public JDTaskTypeController(IHostingEnvironment hostingEnvironment, IDesignationDepartmentService designationDepartmentService)
        {
            this.designationDepartmentService = designationDepartmentService;
        }

        public async Task<IActionResult> Index(int id)
        {
            JDTaskTypeViewModel model = new JDTaskTypeViewModel
            {
                jDTaskTypes = await designationDepartmentService.GetJDTaskTypes(id),
                jDTypes = await designationDepartmentService.GetAllJDTypes(id),
                jDTaskLists = await designationDepartmentService.GetAllJDTaskList(id)
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] JDTaskTypeViewModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                model.jDTaskTypes = await designationDepartmentService.GetJDTaskTypes(id);
                model.jDTypes = await designationDepartmentService.GetAllJDTypes(id);
                model.jDTaskLists = await designationDepartmentService.GetAllJDTaskList(id);
                return View(model);
            }

            JDTaskType data = new JDTaskType
            {
                Id = Int32.Parse(model.jdTaskTypeId),
                AdditionalDescription = model.AdditionalDescription,
                jdTypeId = model.jdTypeId,
                jdTaskListId = model.jdTaskListId,
            };

            await designationDepartmentService.SaveJDTaskType(data);

            return RedirectToAction(nameof(Index));
        }
        public async Task<JsonResult> DeleteJDTaskType(int Id)
        {
            await designationDepartmentService.DeleteJDTaskTypeById(Id);
            return Json(true);
        }

    }
}