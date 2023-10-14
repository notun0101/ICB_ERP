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
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.Areas.MasterData.Models;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using Microsoft.AspNetCore.Identity;
using OPUSERP.Data.Entity;

namespace OPUSERP.Areas.HRPMSMasterData.Controllers
{
    [Authorize]
    [Area("HRPMSMasterData")]
    public class HandoverTakeoverMasterController : Controller
    {

        private readonly IHandoverTakeoverMasterService handoverTakeoverMasterServic;
        private readonly IPersonalInfoService personalInfoService;
        private readonly UserManager<ApplicationUser> _userManager;

        public HandoverTakeoverMasterController(IHandoverTakeoverMasterService handoverTakeoverMasterServic, UserManager<ApplicationUser> userManager, IPersonalInfoService personalInfoService)
        {

            this.handoverTakeoverMasterServic = handoverTakeoverMasterServic;
            this.personalInfoService = personalInfoService;
            _userManager = userManager;
        }
        // GET: Thana
        public async Task<IActionResult> Index()
        {
            HandoverTakeoverMasterViewModel model = new HandoverTakeoverMasterViewModel
            {

                hTMasters = await handoverTakeoverMasterServic.GetAllHTMaster(),
            };
            return View(model);
        }

        // POST: Thana/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] HandoverTakeoverMasterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.hTMasters = await handoverTakeoverMasterServic.GetAllHTMaster();
                return View(model);
            }

            HandoverTakeoverMaster data = new HandoverTakeoverMaster
            {
                Id = (int)model.HTMasterId,
                handoverId = model.handoverId,
                takeoverId = model.takeoverId,
                date = model.date,


            };

            await handoverTakeoverMasterServic.SaveHTMaster(data);

            return RedirectToAction(nameof(Index));

        }

        public async Task<JsonResult> Delete(int Id)
        {
            await handoverTakeoverMasterServic.DeleteHTMasterById(Id);
            return Json(true);
        }
        public async Task<IActionResult> HTDetail()
        {
            HandoverTakeoverMasterViewModel model = new HandoverTakeoverMasterViewModel
            {

                hTMasters = await handoverTakeoverMasterServic.GetAllHTMaster(),
              //  hTDetails = await handoverTakeoverMasterServic.GetDetailsByMasterId(id),
                hTDetails = await handoverTakeoverMasterServic.GetAllHTDetails(),

            };
            return View(model);
        }

        // POST: Thana/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HTDetail([FromForm] HandoverTakeoverMasterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.hTMasters = await handoverTakeoverMasterServic.GetAllHTMaster();
               
                return View(model);
            }

            string fileName;
            if (model.file != null)
            {
                string message = FileSave.SaveImageNew(out fileName, model.file);
            }
            else
            {
                fileName = await handoverTakeoverMasterServic.GetDetailsImgUrlById(model.HTDetailsId);
            };

            HandoverTakeoverDetails data = new HandoverTakeoverDetails
            {
                Id = (int)model.HTDetailsId,
               masterId = model.HTMasterId,
                taskName = model.taskName,
                taskDetails = model.taskDetails,
                comments = model.comments,
                file = fileName,
                status = 1,


            };

            await handoverTakeoverMasterServic.SaveHTDetails(data);

            return RedirectToAction(nameof(HTDetail));

        }

        public async Task<JsonResult> DeleteHTDetail(int Id)
        {
            await handoverTakeoverMasterServic.DeleteHTDetailsById(Id);
            return Json(true);
        }



        public async Task<IActionResult> GetHTDetailByTakeover()
        {
            // var user = await personalInfoService.GetUserIdByEmpId(id);

            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var employeeInfo = await personalInfoService.GetEmployeeInfoByApplicationId(user.Id);
            HandoverTakeoverMasterViewModel model = new HandoverTakeoverMasterViewModel
            {
                
                hTDetails = await handoverTakeoverMasterServic.GetDetailsBytakeoverId(employeeInfo.Id),
              
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DetailTaken(int Id, int status)
        {
            try
            {
                var a = await handoverTakeoverMasterServic.GetHTDetailsById(Id);
                a.status = status;

                await handoverTakeoverMasterServic.SaveHTDetails(a);
                return Json("updated");
            }
            catch (Exception ex)
            {

                return Json(ex.Message);
            }

        }

    }
}