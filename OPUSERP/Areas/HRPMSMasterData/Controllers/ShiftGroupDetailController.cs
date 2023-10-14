﻿using OPUSERP.Areas.HRPMSMasterData.Models;
using OPUSERP.Areas.HRPMSMasterData.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace OPUSERP.Areas.HRPMSMasterData.Controllers
{
    [Authorize]
    [Area("HRPMSMasterData")]
    public class ShiftGroupDetailController : Controller
    {
        private readonly LangGenerate<ShiftGroupDetailsLn> _lang;
        private readonly IShiftGroupDetailService shiftGroupDetailService;
        private readonly IShiftGroupMasterService shiftGroupMasterService;

        public ShiftGroupDetailController(IHostingEnvironment hostingEnvironment, IShiftGroupDetailService shiftGroupDetailService, IShiftGroupMasterService shiftGroupMasterService)
        {
            _lang = new LangGenerate<ShiftGroupDetailsLn>(hostingEnvironment.ContentRootPath);
            this.shiftGroupDetailService = shiftGroupDetailService;
            this.shiftGroupMasterService = shiftGroupMasterService;
        }
        // GET: ShiftGroupDetail
        public async Task<ActionResult> Index()
        {
			var shiftMasters = await shiftGroupMasterService.GetAllShiftGroupMaster();
			var masterDetails = new List<ShiftGroupWithDetails>();
			foreach (var item in shiftMasters)
			{
				masterDetails.Add(new ShiftGroupWithDetails
				{
					shiftGroupMaster = item,
					shiftGroupDetail = await shiftGroupDetailService.GetAllShiftGroupDetailByMasterId(item.Id)
				});
			}
            ShiftGroupDetailViewModel model = new ShiftGroupDetailViewModel
            {
                fLang = _lang.PerseLang("MasterData/ShiftGroupDetailsEN.json", "MasterData/ShiftGroupDetailsBN.json", Request.Cookies["lang"]),
                shiftGroupDetailslist = await shiftGroupDetailService.GetAllShiftGroupDetail(),
                shiftGroupMasterslist = await shiftGroupMasterService.GetAllShiftGroupMaster(),
				shiftGroupWithDetails = masterDetails
			};
            return View(model);
        }

        // POST: ShiftGroupDetail/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] ShiftGroupDetailViewModel model)
        {
            //return Json(model);

            if (!ModelState.IsValid)
            {
                model.fLang = _lang.PerseLang("MasterData/ShiftGroupDetailsEN.json", "MasterData/ShiftGroupDetailsBN.json", Request.Cookies["lang"]);
                model.shiftGroupDetailslist = await shiftGroupDetailService.GetAllShiftGroupDetail();
                model.shiftGroupMasterslist = await shiftGroupMasterService.GetAllShiftGroupMaster();
                return View(model);
            }

            ShiftGroupDetail data = new ShiftGroupDetail
            {
                Id = model.shiftMasterId,
                shiftGroupMasterId = model.shiftGroupMasterId,
                weekDay=model.weekDay,
                startTime=model.startTime,
                endTime=model.endTime,
                holiday=bool.Parse(model.holiday),
				holidayType = model.holidayType
            };

            await shiftGroupDetailService.SaveShiftGroupDetail(data);

            return RedirectToAction(nameof(Index));
        }
        public async Task<JsonResult> DeleteShiftById(int Id)
        {
            await shiftGroupDetailService.DeleteShiftGroupDetailById(Id);
            return Json(true);
        }
    }
}