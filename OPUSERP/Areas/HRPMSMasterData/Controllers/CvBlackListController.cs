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
using Microsoft.AspNetCore.Mvc;
using OPUSERP.HRPMS.Services.Employee.Interfaces;

namespace OPUSERP.Areas.HRPMSMasterData.Controllers
{
    [Authorize]
    [Area("HRPMSMasterData")]
    public class CvBlackListController : Controller
    {
        private readonly IHRPMSAttachmentService hRPMSAttachmentService;

        private readonly ICvBlackListService cvBlackListService;

        public CvBlackListController(IHRPMSAttachmentService hRPMSAttachmentService,IHostingEnvironment hostingEnvironment, ICvBlackListService cvBlackListService)
        {
            this.hRPMSAttachmentService = hRPMSAttachmentService;

            this.cvBlackListService = cvBlackListService;
        }
        // GET: Award
        public async Task<IActionResult> Index()
        {
            CvBlackListViewModel model = new CvBlackListViewModel
            {
                cvBlackLists = await cvBlackListService.GetCvBlackList(),
                cvBlackList = await cvBlackListService.GetCvBlack()
            };
            return View(model);
        }

        // POST: Award/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] CvBlackListViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.cvBlackLists = await cvBlackListService.GetCvBlackList();
                return View(model);
            }
            //string fileName;
            //string message = FileSave.SaveImageNew(out fileName, model.attatchment);
            string fileName;

            if (model.attatchment != null)
            {
                string message = FileSave.SaveImageNew(out fileName, model.attatchment);
            }
            else
            {
                fileName = await hRPMSAttachmentService.GetAttachmentUrlByIdcv(model.CvBlackListId);
            };

            CvBlackList data = new CvBlackList
            {
                Id = model.CvBlackListId,
                sscRoll = model.sscRoll,
                sscReg = model.sscReg,
                date = model.date,
                attatchment = fileName,
                reason = model.reason,
            };

            await cvBlackListService.SaveCvBlackList(data);

            return RedirectToAction(nameof(Index));
        }
        public async Task<JsonResult> Delete(int id)
        {
            await cvBlackListService.DeleteCvBlackListById
                (id);
            return Json(true);
        }
    }
}