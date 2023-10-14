using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSMasterData.Models;
using OPUSERP.Data.Entity.MasterData;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;

namespace OPUSERP.Areas.HRPMSMasterData.Controllers
{
    [Authorize]
    [Area("HRPMSMasterData")]
    public class JDTaskListController : Controller
    {
     
        private readonly IBookAwardService bookAwardService;

        public JDTaskListController(IHostingEnvironment hostingEnvironment, IBookAwardService bookAwardService)
        {
            this.bookAwardService = bookAwardService;
        }
        // GET: Award
        public async Task<IActionResult> Index()
        {
            JDTaskListViewModel model = new JDTaskListViewModel
            {
                jDTaskLists = await bookAwardService.GetJDTaskListInfo()
            };
            return View(model);
        }

        // POST: Award/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] JDTaskListViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.jDTaskLists = await bookAwardService.GetJDTaskListInfo();
                return View(model);
            }

            JDTaskList data = new JDTaskList
            {
                Id = Int32.Parse(model.JDListId),
                TaskName = model.JdListName,

            };

            await bookAwardService.SaveJDTaskListInfo(data);

            return RedirectToAction(nameof(Index));
        }
        public async Task<JsonResult> DeletJDTaskListInfoById(int id)
        {
            await bookAwardService.DeletJDTaskListInfoById(id);
            return Json(true);
        }
    }
}