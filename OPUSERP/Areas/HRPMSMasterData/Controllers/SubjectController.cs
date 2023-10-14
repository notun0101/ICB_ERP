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
using System;

namespace OPUSERP.Areas.HRPMSMasterData.Controllers
{
    [Authorize]
    [Area("HRPMSMasterData")]
    public class SubjectController : Controller
    {
        private readonly LangGenerate<SubjectInfoLn> _lang;
        private readonly ISubjectService subjectService;

        public SubjectController(IHostingEnvironment hostingEnvironment, ISubjectService subjectService)
        {
            _lang = new LangGenerate<SubjectInfoLn>(hostingEnvironment.ContentRootPath);
            this.subjectService = subjectService;
        }

        // GET: Subject
        public async Task<IActionResult> Index()
        {
            SubjectViewModel model = new SubjectViewModel
            {
                fLang = _lang.PerseLang("MasterData/SubjectEN.json", "MasterData/SubjectBN.json", Request.Cookies["lang"]),
				subjectvm = await subjectService.GetAllSubjectVm(),
				subjects = await subjectService.GetAllSubject(),
                degrees = await subjectService.GetAllDegrees()
            };

            return View(model);
        }


        // POST: Subject/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] SubjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.fLang = _lang.PerseLang("MasterData/SubjectEN.json", "MasterData/SubjectBN.json", Request.Cookies["lang"]);
                model.subjects = await subjectService.GetAllSubject();
                return View(model);
            }

            Subject data = new Subject
            {
                Id = model.subjectId,
                subjectName = model.subjectName,
                subjectNameBn = model.subjectNameBn,
                subjectShortName = model.subjectShortName
            };

            var subjectid = await subjectService.SaveSubject1(data);

            await subjectService.UpdateSubjectDegreeRelation(subjectid, Convert.ToInt32(model.degreeId));

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<JsonResult> DeleteSubjectById(int id)
        {
            await subjectService.DeleteSubjectById(id);
            return Json(true);
        }

        public async Task<IActionResult> GetDegreeBySubjectId(int id)
        {
            var data = await subjectService.GetDegreeBySubjectId(id);
            return Json(data);
        }
    }
}