﻿using System;
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
using OPUSERP.Data.Entity.MasterData;

namespace OPUSERP.Areas.HRPMSMasterData.Controllers
{
    [Authorize]
    [Area("HRPMSMasterData")]
    public class ReligionController : Controller
    {
        private readonly LangGenerate<ReligionLn> _lang;
        // GET: Religion
        private readonly IReligionMunicipalityService religionMunicipalityService;

        public ReligionController(IHostingEnvironment hostingEnvironment, IReligionMunicipalityService religionMunicipalityService)
        {
            _lang = new LangGenerate<ReligionLn>(hostingEnvironment.ContentRootPath);
            this.religionMunicipalityService = religionMunicipalityService;
        }

        // GET: Religion
        public async Task<IActionResult> Index()
        {
            ReligionViewModel model = new ReligionViewModel
            {
                fLang = _lang.PerseLang("MasterData/ReligionEN.json", "MasterData/ReligionBN.json", Request.Cookies["lang"]),
                religions = await religionMunicipalityService.GetReligions()
            };
            return View(model);
        }

        // POST: Religion/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] ReligionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.fLang = _lang.PerseLang("MasterData/ReligionEN.json", "MasterData/ReligionBN.json", Request.Cookies["lang"]);
                model.religions = await religionMunicipalityService.GetReligions();
                return View(model);
            }

            Religion data = new Religion
            {
                Id = model.religionId,
                name = model.name,
                nameBn = model.nameBn,
                shortName = model.shortName
            };

            await religionMunicipalityService.SaveReligion(data);

            return RedirectToAction(nameof(Index));
        }
        public async Task<JsonResult> DeleteReligionById(int id)
        {
            await religionMunicipalityService.DeleteReligionById(id);
            return Json(true);
        }


        // GET: MedicalDisease
        public async Task<IActionResult> MedicalDisease()
        {
            ReligionViewModel model = new ReligionViewModel
            {
                medicalDiseases = await religionMunicipalityService.GetMedicalDiseases()
            };
            return View(model);
        }

        // POST: Religion/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MedicalDisease([FromForm] ReligionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.medicalDiseases = await religionMunicipalityService.GetMedicalDiseases();
                return View(model);
            }

            MedicalDisease data = new MedicalDisease
            {
                Id = model.religionId,
                name = model.name,
               
            };

            await religionMunicipalityService.SaveMedicalDisease(data);

            return RedirectToAction(nameof(MedicalDisease));
        }
        public async Task<JsonResult> DeleteMedicalDisease(int id)
        {
            await religionMunicipalityService.DeleteMedicalDiseaseById(id);
            return Json(true);
        }
    }
}