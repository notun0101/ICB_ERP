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
    public class PostingController : Controller
    {
        private readonly IPersonalInfoService personalInfoService;
        private readonly IPhotographService photographService;
        private readonly ITaxService taxService;
        private readonly IDesignationDepartmentService designationDepartmentService;
        private readonly IFunctionsInfoService functionsInfoService;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILocationService locationService;
        private readonly ISpecialBranchUnitService specialBranchUnitService;
        private readonly IStatusService statusService;


        public PostingController(IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IPhotographService photographService,ITaxService taxService, IPersonalInfoService personalInfoService, IStatusService statusService, ILocationService locationService, ISpecialBranchUnitService specialBranchUnitService, IDesignationDepartmentService designationDepartmentService, IFunctionsInfoService functionsInfoService
)
        {
            this.personalInfoService = personalInfoService;
            this.photographService = photographService;
            this.taxService = taxService;
            this.statusService = statusService;
            this.locationService = locationService;
            this.specialBranchUnitService = specialBranchUnitService;
            this.designationDepartmentService = designationDepartmentService;
            this.functionsInfoService = functionsInfoService;


            _roleManager = roleManager;
            _userManager = userManager;
        }
        #region Posting
        // GET: Posting
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            var model = new PostingViewModel
            {
                employeeId = id,
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                employeePostingPlaces = await taxService.GetEmpPostingByEmpId(id),
                specialBranchUnits = await specialBranchUnitService.GetSpecialBranchUnit(),
                hrUnits = await statusService.GetHrUnit(),
                locations = await locationService.GetLocation(),
                hrDivisions = await statusService.GetHrDivision(),
                hrBranches = await statusService.GetHrBranch(),
                departments = await designationDepartmentService.GetDepartment(),
                functionInfos = await functionsInfoService.GetFunctionInfo(),
                employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id)





            };
            if (model.employeePosting == null) model.employeePosting = new EmployeePostingPlace();
            return View(model);
        }

        // POST: Posting/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] PostingViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

			var emp = await personalInfoService.GetEmployeeInfoById((int)model.employeeId);

            //if (ModelState.IsValid)
            //{
            //    //go on as normal
            //}
            //else
            //{
            //    var errors = ModelState.Select(x => x.Value.Errors)
            //                           .Where(y => y.Count > 0)
            //                           .ToList();
            //}

            //if (!ModelState.IsValid)
            //{
            //    ViewBag.employeeID = model.employeeId;
            //    model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById((int)model.employeeId);
            //    model.employeePostingPlaces = await taxService.GetEmpPostingByEmpId((int)model.employeeId);
            //    model.hrBranches = await statusService.GetHrBranch();
            //    model.hrDivisions = await statusService.GetHrDivision();
            //    model.hrUnits = await statusService.GetHrUnit();
            //    model.locations = await locationService.GetLocation();
            //    model.departments = await designationDepartmentService.GetDepartment();
            //    model.functionInfos = await functionsInfoService.GetFunctionInfo();
            //    model.specialBranchUnits = await specialBranchUnitService.GetSpecialBranchUnit();
            //    if (model.employeePosting == null) model.employeePosting = new EmployeePostingPlace();
            //    return View(model);
            //}

            EmployeePostingPlace data = new EmployeePostingPlace
            {
                Id = Convert.ToInt32(model.postingID),
                employeeId = model.employeeId,
                placeName = model.placeName,
                placeNameBn = model.placeNameBn,
                remarks = model.remarks,
                type = model.type,
                branchId=model.branchId,
                hrBranchId=model.hrBranchId,
                departmentId=model.departmentId,
                hrDivisionId=model.hrDivisionId,
                hrUnitId=model.hrUnitId,
                locationId=model.locationId,
                startDate=model.startDate,
                endDate=model.endDate,
                officeId=model.officeId,
                status = 1

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
            var postingId = await taxService.SavePosting(data);



			if (model.endDate == null && emp.lastDesignationId != 285) //GM
			{
				await personalInfoService.UpdatePreviousPostingDate((int)model.employeeId, postingId);
                personalInfoService.UpdateEmployeePosting(emp.Id, 1, model.departmentId, model.locationId, model.officeId, model.hrDivisionId, model.hrUnitId, model.hrBranchId, postingId);
			}

            //if (model.endDate == null && emp.lastDesignationId != 285)
            //{


            //    if (model.hrBranchId != null)
            //    {
            //        emp.departmentId = null;
            //        emp.locationId = null;
            //        emp.functionInfo = null;
            //        emp.hrDivisionId = null;
            //        emp.hrUnitId = null;
            //        emp.hrBranchId = model.hrBranchId;
            //    }
            //    if (model.departmentId != null)
            //    {
            //        emp.hrBranchId = 116;
            //        emp.locationId = null;
            //        emp.functionInfo = null;
            //        emp.hrDivisionId = null;
            //        emp.hrUnitId = null;
            //        emp.departmentId = 161;
            //    }
            //    if (model.locationId != null)
            //    {
            //        emp.hrBranchId = null;
            //        emp.departmentId = null;
            //        emp.functionInfo = null;
            //        emp.hrDivisionId = null;
            //        emp.hrUnitId = null;
            //        emp.locationId = model.locationId;
            //    }
            //    if (model.officeId != null)
            //    {
            //        emp.hrBranchId = null;
            //        emp.departmentId = null;
            //        emp.locationId = null;
            //        emp.hrDivisionId = null;
            //        emp.hrUnitId = null;
            //        emp.functionInfoId = model.officeId;
            //    }
            //    if (model.hrDivisionId != null)
            //    {
            //        emp.hrBranchId = null;
            //        emp.departmentId = null;
            //        emp.locationId = null;
            //        emp.functionInfo = null;
            //        emp.hrUnitId = null;
            //        emp.hrDivisionId = model.hrDivisionId;
            //    }
            //    if (model.hrUnitId != null)
            //    {
            //        emp.hrBranchId = null;
            //        emp.departmentId = null;
            //        emp.locationId = null;
            //        emp.functionInfo = null;
            //        emp.hrDivisionId = null;
            //        emp.hrUnitId = model.hrUnitId;
            //    }
            //    var empId = await personalInfoService.SaveEmployeeInfo(emp);
            //}


            //await personalInfoService.UpdateEmployeeinfoById((int)model.employeeId);

            return RedirectToAction("Index", "Posting", new
            {
                id = model.employeeId
            });

        }

        // Delete: Posting
        public async Task<IActionResult> Delete(int id, int empId)
        {
            var data = await taxService.DeletePostingById(id);
            if (data == 1)
            {
                return Json("deleted");
            }
            else
            {
                return Json("failed");
            }
        }
        public async Task<IActionResult> type(int id, int empId)
        {
            var posting = await taxService.GetEmpPostingById(id);
            if (posting.type == 1)
            {
                posting.type = 0;
            }
            else
            {
                posting.type = 1;
            }
            await taxService.SavePosting(posting);
            return Json("ok");
        }
        #endregion
    }
}