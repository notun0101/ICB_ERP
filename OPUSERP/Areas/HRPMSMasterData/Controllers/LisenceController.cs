using OPUSERP.Areas.HRPMSMasterData.Models;
using OPUSERP.Areas.HRPMSMasterData.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.Data.Entity.MasterData;

namespace OPUSERP.Areas.HRPMSMasterData.Controllers
{
    [Authorize]
    [Area("HRPMSMasterData")]
    public class LisenceController : Controller
    {
        
        private readonly ILisenceService lisenceService;

        public LisenceController(IHostingEnvironment hostingEnvironment, ILisenceService lisenceService)
        {
           
            this.lisenceService = lisenceService;
        }

        // GET: Lisence
        public async Task<IActionResult> Index()
        {
            LisenceViewModel model = new LisenceViewModel
            {
                lisenceTypes = await lisenceService.GetLisenceType(),
                lisences = await lisenceService.GetLisence(),
                lisence = await lisenceService.GetLisence1()
            };
            return View(model);
        }


        // POST: Lisence
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] LisenceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.lisences = await lisenceService.GetLisence();
                model.lisenceTypes = await lisenceService.GetLisenceType();
                model.lisence = await lisenceService.GetLisence1();
                return View(model);
            }
            string fileName;
            string message = FileSave.SaveImageNew(out fileName, model.attatchment);
            

            Lisence data = new Lisence
            {
                Id = model.LisenceId,
                lisenceTypeId = model.lisenceTypeId,
                lisenceNo = model.lisenceNo,
                issueDate = model.issueDate,
                expDate = model.expDate,
                attatchment = fileName
            };

            await lisenceService.SaveLisence(data);

            return RedirectToAction(nameof(Index));
        }
        public async Task<JsonResult> DeleteLisence(int Id)
        {
            await lisenceService.DeleteLisenceById(Id);
            return Json(true);
        }

        //Lisence
        public async Task<IActionResult> LisenceType()
        {
            LisenceViewModel model = new LisenceViewModel
            {
                lisenceTypes = await lisenceService.GetLisenceType()
            };

            return View(model);
          
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LisenceType([FromForm] LisenceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.lisenceTypes = await lisenceService.GetLisenceType();
                return View(model);
            }

            LisenceType data = new LisenceType
            {
                Id = model.LisenceId,
                typeName = model.typeName,
               
            };

            await lisenceService.SaveLisenceType(data);

            return RedirectToAction(nameof(LisenceType));
        }

        public async Task<JsonResult> DeleteLisenceType(int Id)
        {
            await lisenceService.DeleteLisenceTypeById(Id);
            return Json(true);
        }
    }
}