using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSMasterData.Models;
using OPUSERP.Areas.HRPMSMasterData.Models.Lang;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Controllers
{
    [Authorize]
    [Area("HRPMSMasterData")]
    public class ShiftGroupAssignController : Controller
    {
        private readonly LangGenerate<ShiftGroupLn> _lang;
        private readonly IShiftGroupMasterService shiftGroupMasterService;
        private readonly ISpecialBranchUnitService specialBranchUnitService;
        private readonly IDesignationDepartmentService designationDepartmentService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly IShiftGroupDetailService shiftGroupDetailService;

        public ShiftGroupAssignController(IHostingEnvironment hostingEnvironment, IShiftGroupMasterService shiftGroupMasterService, ISpecialBranchUnitService specialBranchUnitService, IDesignationDepartmentService designationDepartmentService, IPersonalInfoService personalInfoService, IShiftGroupDetailService shiftGroupDetailService)
        {
            _lang = new LangGenerate<ShiftGroupLn>(hostingEnvironment.ContentRootPath);
            this.shiftGroupMasterService = shiftGroupMasterService;
            this.specialBranchUnitService = specialBranchUnitService;
            this.designationDepartmentService = designationDepartmentService;
            this.personalInfoService = personalInfoService;
            this.shiftGroupDetailService = shiftGroupDetailService;
        }

        // GET: ShiftGroupMaster
        public async Task<ActionResult> Index()
        {
            //var shiftMasters = await shiftGroupMasterService.GetAllShiftGroupMaster();
            //var masterDetails = new List<ShiftGroupWithDetails>();
            //foreach (var item in shiftMasters)
            //{
            //    masterDetails.Add(new ShiftGroupWithDetails
            //    {
            //        shiftGroupMaster = item,
            //        shiftGroupDetail = await shiftGroupDetailService.GetAllShiftGroupDetailByMasterId(item.Id)
            //    });
            //}
            ShiftGroupAssignViewModel model = new ShiftGroupAssignViewModel
            {
                fLang = _lang.PerseLang("MasterData/ShiftGroupMasterEN.json", "MasterData/ShiftGroupMasterBN.json", Request.Cookies["lang"]),
                shiftGroupMasterlist = await shiftGroupMasterService.GetAllShiftGroupMaster(),
                shiftGroupDetailslist = await shiftGroupMasterService.GetAllShiftGroupDetail(),
                specialBranchUnits = await specialBranchUnitService.GetSpecialBranchUnit(),
                departments = await designationDepartmentService.GetDepartment(),
                visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
				employeeWithShiftGroups = await shiftGroupMasterService.GetEmployeeWithShiftGroup(),
              //  shiftGroupWithDetails = masterDetails,
                shiftGroupWithDetails = await shiftGroupDetailService.GetAllShiftGroupDetail()
               // shiftGroupWithDetails = await shiftGroupDetailService.GetAllShiftGroupDetailByMasterId(Id)
                
            };
            return View(model);
        }

      
        public async Task<JsonResult> GetShiftDetails(int id)
        {
            ShiftGroupAssignViewModel model = new ShiftGroupAssignViewModel();
            var a = await shiftGroupDetailService.GetAllShiftGroupDetailByMasterId(id);

            return Json(a);
        }


        // GET: ShiftGroupMaster
        public async Task<ActionResult> WagesIndex()
        {
            ShiftGroupAssignViewModel model = new ShiftGroupAssignViewModel
            {
                fLang = _lang.PerseLang("MasterData/ShiftGroupMasterEN.json", "MasterData/ShiftGroupMasterBN.json", Request.Cookies["lang"]),
                shiftGroupMasterlist = await shiftGroupMasterService.GetAllShiftGroupMaster(),
                specialBranchUnits = await specialBranchUnitService.GetSpecialBranchUnit(),
                departments = await designationDepartmentService.GetDepartment(),
            };
            return View(model);
        }


        // POST: ShiftGroupMaster/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] ShiftGroupAssignViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.fLang = _lang.PerseLang("MasterData/ShiftGroupMasterEN.json", "MasterData/ShiftGroupMasterBN.json", Request.Cookies["lang"]);
                model.shiftGroupMasterlist = await shiftGroupMasterService.GetAllShiftGroupMaster();
                return View(model);
            }

            await shiftGroupMasterService.UpdateShiftGroupId(model.ShiftType, model.sbu, model.department, model.employeeInfoId, model.shiftGroup);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WagesIndex([FromForm] ShiftGroupAssignViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.fLang = _lang.PerseLang("MasterData/ShiftGroupMasterEN.json", "MasterData/ShiftGroupMasterBN.json", Request.Cookies["lang"]);
                model.shiftGroupMasterlist = await shiftGroupMasterService.GetAllShiftGroupMaster();
                return View(model);
            }

            await shiftGroupMasterService.UpdateShiftGroupIdForWages(model.ShiftType, model.sbu, model.department, model.employeeInfoId, model.shiftGroup);

            return RedirectToAction(nameof(Index));
        }


    }
}