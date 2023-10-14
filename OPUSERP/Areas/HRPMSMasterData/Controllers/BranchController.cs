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
    public class BranchController : Controller
    {
        private readonly LangGenerate<DepartmentInfoLn> _lang;
        // GET: Branch
        private readonly IDesignationDepartmentService designationDepartmentService;
        private readonly IFunctionsInfoService functionsInfoService;

        public BranchController(IHostingEnvironment hostingEnvironment, IDesignationDepartmentService designationDepartmentService, IFunctionsInfoService functionsInfoService)
        {
            _lang = new LangGenerate<DepartmentInfoLn>(hostingEnvironment.ContentRootPath);
            this.designationDepartmentService = designationDepartmentService;
            this.functionsInfoService = functionsInfoService;
        }

        // GET: Branch
        public async Task<IActionResult> Index()
        {
            BranchViewModel model = new BranchViewModel
            {
                hrBranchTypes = await designationDepartmentService.GetAllBranchType(),
                //hrBranches = await designationDepartmentService.GetBranch(),
                branchesWithManager = await designationDepartmentService.GetAllBranchWithManagers(),
                zones = await designationDepartmentService.GetAllZones(),
                offices = await functionsInfoService.GetFunctionInfo()
            };
            return View(model);
        }

        // POST: Branch/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] BranchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.hrBranchTypes = await designationDepartmentService.GetAllBranchType();
                model.hrBranches = await designationDepartmentService.GetBranch();
                return View(model);
            }

            HrBranch data = new HrBranch
            {
                Id = Int32.Parse(model.branchId),
                branchTypeId = Convert.ToInt32(model.branchTypeId),
                branchCode = model.branchCode,
                branchName = model.branchName,
                branchNameBn=model.branchNameBn,
                address = model.address,
				locationId = model.zoneId,
				officeId = model.officeId,
                branchPlace = model.branchPlace,
				isDelete = model.status
            };

            await designationDepartmentService.SaveBranch(data);

            return RedirectToAction(nameof(Index));
        }
        public async Task<JsonResult> DeleteBranch(int Id)
        {
            await designationDepartmentService.DeleteBranchById(Id);
            return Json(true);
        }

		// GET: SubBranch
		public async Task<IActionResult> SubBranch()
        {
            SubBranchViewModel model = new SubBranchViewModel
            {
                hrBranchTypes = await designationDepartmentService.GetAllBranchType(),
                hrSubBranches = await designationDepartmentService.GetSubBranch(),
                hrBranches = await designationDepartmentService.GetBranch()
            };
            return View(model);
        }

		// POST: SubBranch/Edit
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubBranch([FromForm] SubBranchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.hrBranchTypes = await designationDepartmentService.GetAllBranchType();
                model.hrSubBranches = await designationDepartmentService.GetSubBranch();
                return View(model);
            }

            HrSubBranch data = new HrSubBranch
            {
                Id = Int32.Parse(model.subbranchId),
                subbranchTypeId = model.subbranchTypeId,
                subbranchCode = model.subbranchCode,
                subbranchName = model.subbranchName,
                subaddress = model.subaddress,
                status = model.status,
                isActive = model.isActive,
                remarks = model.remarks,
                hrBranchId = model.hrBranchId,


            };

            await designationDepartmentService.SaveSubBranch(data);

            return RedirectToAction(nameof(SubBranch));
        }

        public async Task<JsonResult> DeleteSubBranchById(int Id)
        {
            await designationDepartmentService.DeleteSubBranchById(Id);
            return Json(true);
        }
    }
}