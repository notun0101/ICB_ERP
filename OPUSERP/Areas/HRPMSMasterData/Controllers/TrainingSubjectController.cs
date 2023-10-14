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
using OPUSERP.Data.Entity.Training;

namespace OPUSERP.Areas.HRPMSMasterData.Controllers
{
    [Authorize]
    [Area("HRPMSMasterData")]
    public class TrainingSubjectController : Controller
    {
        private readonly LangGenerate<TrainigInstituteLn> _lang;
        private readonly ITrainingService trainingService;

        public TrainingSubjectController(IHostingEnvironment hostingEnvironment, ITrainingService trainingService)
        {
            _lang = new LangGenerate<TrainigInstituteLn>(hostingEnvironment.ContentRootPath);
            this.trainingService = trainingService;
        }
        // GET: TrainingSubject
        public async Task<IActionResult> Index()
        {
            TrainingSubjectViewModel model = new TrainingSubjectViewModel
            {
                trainingSubjects = await trainingService.GetTrainingSubject()
            };
            return View(model);
        }

        // POST: TrainingSubject/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] TrainingSubjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.trainingSubjects = await trainingService.GetTrainingSubject();
                return View(model);
            }

            TrainingSubject data = new TrainingSubject
            {
                Id = model.tSubjectId,
                name = model.name
               
            };

            await trainingService.SaveTrainingSubject(data);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<JsonResult> DeleteSubjectById(int id)
        {
            await trainingService.DeleteTrainingSubjectById(id);
            return Json(true);
        }
    }
}