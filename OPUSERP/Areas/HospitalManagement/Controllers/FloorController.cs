using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HospitalManagement.Models;
using OPUSERP.SCM.Data.Entity.Hospital;
using OPUSERP.SCM.Services.Hospital;
using OPUSERP.SCM.Services.Hospital.Interfaces;


namespace OPUSERP.Areas.HospitalManagement.Controllers
{
    [Area("HospitalManagement")]
    public class FloorController : Controller
    {
		private readonly IHospitalSettings _hospitalSettings;

		public FloorController(IHospitalSettings hospitalSettings)
		{
			_hospitalSettings = hospitalSettings;
		}

		#region Floor
		public async Task<ActionResult> FloorCreate()
		{
			FloorViewModel model = new FloorViewModel
			{
				floorList = await _hospitalSettings.GetAllFloor()
			};
			if (model.floor == null) model.floor = new Floor();
			return View(model);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FloorCreate([FromForm] FloorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.floorList = await _hospitalSettings.GetAllFloor();
                return View(model);
            }

            Floor data = new Floor
            {
                Id = model.Id,
                buildingId = model.buildingId,
                nameEn = model.nameEn,
                nameBn = model.nameBn,
                type = model.type,
                status = model.status,
                remarks = model.remarks
                
            };


            await _hospitalSettings.SaveFloor(data);
            return RedirectToAction(nameof(FloorCreate));
        }

        [HttpPost]
        public async Task<JsonResult> DeleteFloorById(int Id)
        {
            await _hospitalSettings.DeleteFloorById(Id);
            return Json(true);
        }

        #endregion


    }
}