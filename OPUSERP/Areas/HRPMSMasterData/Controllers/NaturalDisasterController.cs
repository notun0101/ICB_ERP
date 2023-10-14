using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSMasterData.Models;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;

namespace OPUSERP.Areas.HRPMSMasterData.Controllers
{
    [Authorize]
    [Area("HRPMSMasterData")]

    public class NaturalDisasterController : Controller
    {
        private readonly IMembershipLanguageService membershipLanguageService;
        public NaturalDisasterController(IHostingEnvironment hostingEnvironment, IMembershipLanguageService membershipLanguageService)
        {
            this.membershipLanguageService = membershipLanguageService;
        }
        // GET: Language
        public async Task<IActionResult> Index()
        {
            NaturalDisasterViewModel model = new NaturalDisasterViewModel
            {
                naturalDigesters = await membershipLanguageService.GetNaturalDisasterInfo()
            };
            return View(model);
        }

        // POST: Language/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] NaturalDisasterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.naturalDigesters = await membershipLanguageService.GetNaturalDisasterInfo();
                return View(model);
            }

            NaturalDigester data = new NaturalDigester
            {
                Id = model.naturaldisasterId,
                name =model.name,
                description = model.description,
              
            };

            await membershipLanguageService.SaveNaturalDisasterInfo(data);

            return RedirectToAction(nameof(Index));
        }
        public async Task<JsonResult> DeleteNaturalDisasterInfoById(int id)
        {
            await membershipLanguageService.DeleteNaturalDisasterInfoById(id);
            return Json(true);
        }
    }
}