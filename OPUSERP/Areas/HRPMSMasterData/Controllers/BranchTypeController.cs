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
    public class BranchTypeController : Controller
    {
        private readonly IStatusService statusService;

        public BranchTypeController(IHostingEnvironment hostingEnvironment, IStatusService statusService)
        {
            this.statusService = statusService;
        }
        //  GET: BranchType
        public async Task<IActionResult> Index()
        {

            BranchTypeViewModel model = new BranchTypeViewModel
            {
                hrBranchTypes = await statusService.GetHrBranchType()
        };
            
            return View(model);
        }

        // POST: DesignationInfo/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] BranchTypeViewModel model)
        {
            
            //if (model.BranchTypeId > 0)
            //{
            //    Branch = model.designationCode;
            //}

            if (!ModelState.IsValid)
            {

                model.hrBranchTypes = await statusService.GetHrBranchType();
                return View(model);
            }

            HrBranchType data = new HrBranchType
            {
               
                Id = model.BranchTypeId,
                name = model.name,
                status = 1
               
            };

            await statusService.SaveHrBranchType(data);

            return RedirectToAction(nameof(Index));
        }
        public async Task<JsonResult> DeleteHrBranchTypeById(int Id)
        {
            await statusService.DeleteHrBranchTypeById(Id);
            return Json(true);
        }

        

    }
}