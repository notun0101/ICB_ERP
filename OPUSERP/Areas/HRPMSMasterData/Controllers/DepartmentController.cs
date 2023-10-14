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
    public class DepartmentController : Controller
    {
        private readonly LangGenerate<DepartmentInfoLn> _lang;
        // GET: Department
        private readonly IDesignationDepartmentService designationDepartmentService;

        public DepartmentController(IHostingEnvironment hostingEnvironment, IDesignationDepartmentService designationDepartmentService)
        {
            _lang = new LangGenerate<DepartmentInfoLn>(hostingEnvironment.ContentRootPath);
            this.designationDepartmentService = designationDepartmentService;
        }

        // GET: Department
        public async Task<IActionResult> Index()
        {
            Random generator = new Random();
            String ranNo = generator.Next(1, 1000000).ToString("D6");
            ViewBag.deptCode = "DI-" + ranNo;
            DepartmentViewModel model = new DepartmentViewModel
            {
                fLang = _lang.PerseLang("MasterData/DepartmentEN.json", "MasterData/DepartmentBN.json", Request.Cookies["lang"]),
                departments = await designationDepartmentService.GetDepartment(),
                hrDivisions= await designationDepartmentService.GetHrdivision(),
                hrBranch = await designationDepartmentService.GetAllHrBranch()
            };
            return View(model);
        }

        // POST: Department/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] DepartmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.fLang = _lang.PerseLang("MasterData/DepartmentEN.json", "MasterData/DepartmentBN.json", Request.Cookies["lang"]);
                model.departments = await designationDepartmentService.GetDepartment();
                model.hrDivisions = await designationDepartmentService.GetHrdivision();
                return View(model);
            }

            Department data = new Department
            {
                Id = Int32.Parse(model.departmentId),
                deptCode = model.deptCode,
                deptName = model.deptName,
                deptNameBn = model.deptNameBn,
                hrDivisionId=model.hrdivisionId,
                shortName = model.shortName,
                startDate = model.startDate,
                hrBranchId = model.hrBranchId,
				isDelete = model.status
            };

            await designationDepartmentService.SaveDepartment(data);

            return RedirectToAction(nameof(Index));
        }
        public async Task<JsonResult> DeleteDepartmentById(int Id)
        {
            await designationDepartmentService.DeleteDepartmentById(Id);
            return Json(true);
        }
    }
}