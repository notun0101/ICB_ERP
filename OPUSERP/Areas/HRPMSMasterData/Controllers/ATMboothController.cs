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
    public class ATMboothController : Controller
    {

        private readonly IBookAwardService bookAwardService;

        public ATMboothController(IHostingEnvironment hostingEnvironment, IBookAwardService bookAwardService)
        {

            this.bookAwardService = bookAwardService;
        }
        // GET: ATM
        public async Task<IActionResult> Index()
        {
            AtmboothViewModel model = new AtmboothViewModel
            {
                hrAtmBooths = await bookAwardService.GetatmboothInfo(),
                hrBranches = await bookAwardService.Getbranches(),
                hrSubBranches = await bookAwardService.Getsubbranches()
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Index([FromForm] AtmboothViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.hrAtmBooths = await bookAwardService.GetatmboothInfo();
                model.hrBranches = await bookAwardService.Getbranches();
                model.hrSubBranches = await bookAwardService.Getsubbranches();
                return View(model);
            }
            var data = new HrAtmBooth
            {
                Id=model.atmId,
                location = model.location,
                contactNo = model.contactNo,
                contactPerson = model.contactPerson,
                status = model.status,
                noOfMachine=model.noOfMachine,
                branchId = model.branchId,
                subBranchId = model.subBranchId
            };
            await bookAwardService.SaveAtmInfo(data);
            return RedirectToAction("Index");
        }
        public async Task<JsonResult> DeleteatmboothInfobyId(int Id)
        {
            await bookAwardService.DeleteatmboothInfobyId(Id);
            return Json(true);
        }
    }
}