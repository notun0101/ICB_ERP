using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HospitalManagement.Models;
using OPUSERP.SCM.Services.Hospital.Interfaces;

namespace OPUSERP.Areas.HospitalManagement.Controllers
{
	[Area("HospitalManagement")]
    public class HospitalSettingController : Controller
    {
		private readonly IHospitalSettings _hospitalSettings;

		public HospitalSettingController(IHospitalSettings hospitalSettings)
		{
			_hospitalSettings = hospitalSettings;
		}

		public async Task<IActionResult> Index()
        {
			var data = new List<HospitalBuildingsWithFloor>();
			var buildings = await _hospitalSettings.GetAllBuildings();
			foreach (var item in buildings)
			{
				var model1 = new HospitalBuildingsWithFloor
				{
					building = item,
					floors = await _hospitalSettings.GetFloorsWithBuildingId(item.Id)
				};
				data.Add(model1);
			}

			return View(data);
        }

		public async Task<IActionResult> GetBedDescriptionById(int id)
		{
			return Json(await _hospitalSettings.GetBedInfoById(id));
		}
    }
}