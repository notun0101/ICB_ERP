using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Areas.HRPMSMasterData.Models;
using OPUSERP.Areas.HRPMSMasterData.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OPUSERP.Areas.HRPMSMasterData.Controllers
{
    [Authorize]
    [Area("HRPMSMasterData")]
    public class HrDivisionController : Controller
    {
        private readonly IStatusService statusService;
        private readonly IDesignationDepartmentService designationDepartmentService;

        public HrDivisionController(IHostingEnvironment hostingEnvironment, IStatusService statusService, IDesignationDepartmentService designationDepartmentService)
        {
            this.statusService = statusService;
            this.designationDepartmentService = designationDepartmentService;
        }
        //  GET: BranchType
        public async Task<IActionResult> Index()
        {

            HrDivisionViewModel model = new HrDivisionViewModel
            {
                hrDivisions = await statusService.GetHrDivision(),
                functionInfos = await statusService.GetFunctionInfo(),
                hrBranch = await designationDepartmentService.GetAllHrBranch()
            };
            
            return View(model);
        }

        // POST: DesignationInfo/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] HrDivisionViewModel model)
        {
            
           

            if (!ModelState.IsValid)
            {

                model.hrDivisions = await statusService.GetHrDivision();
                model.functionInfos = await statusService.GetFunctionInfo();

                return View(model);
            }

            HrDivision data = new HrDivision
            {
               
                Id = model.divId,
                divCode = model.divCode,
                divName = model.divName,
                divNameBn = model.divNameBn,
                shortName = model.shortName,
                startDate = model.startDate,
                functionId=model.functionId,
                hrBranchId = model.hrBranchId,
				isDelete = model.status
               
            };

            await statusService.SaveHrDivision(data);

            return RedirectToAction(nameof(Index));
        }
        public async Task<JsonResult> DeleteHrDivisionById(int Id)
        {
            await statusService.DeleteHrDivisionById(Id);
            return Json(true);
        }

        

    }
}