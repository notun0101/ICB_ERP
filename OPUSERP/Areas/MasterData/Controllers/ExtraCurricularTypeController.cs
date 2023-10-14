using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.Data.Entity.Master;
using OPUSERP.Areas.MasterData.Models;
using Microsoft.AspNetCore.Authorization;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.HRPMS.Data.Entity.Master;

namespace OPUSERP.Areas.MasterData.Controllers
{
    [Area("MasterData")]
    public class ExtraCurricularTypeController : Controller
    {
       
        
        private readonly IExtraCurricularTypeService extraCurricularTypeService;

        public ExtraCurricularTypeController(IHostingEnvironment hostingEnvironment,IExtraCurricularTypeService extraCurricularTypeService)
        {
            this.extraCurricularTypeService = extraCurricularTypeService;
        }
        // GET: ExtraCurricularType
        public async Task<IActionResult> Index()
        {

            ExtraCurricularTypeViewModel model = new ExtraCurricularTypeViewModel
            {

                extraCurricularTypes = await extraCurricularTypeService.GetExtraCurricularType(),


            };

            return View(model);
        }

        // POST: ExtraCurricularType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] ExtraCurricularTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.extraCurricularTypes = await extraCurricularTypeService.GetExtraCurricularType();
                return View(model);
            }

            ExtraCurricularType data = new ExtraCurricularType
            {

                Id = model.ExtraCurricularTypeId,
                name = model.name,
 
            };

            await extraCurricularTypeService.SaveExtraCurricularType(data);

            return RedirectToAction(nameof(Index));

        }

        // GET: Thana/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            await extraCurricularTypeService.DeleteExtraCurricularTypeId(id);
            return RedirectToAction(nameof(Index));
        }


    }
}