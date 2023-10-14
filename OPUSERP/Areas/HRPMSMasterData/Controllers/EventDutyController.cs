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
using OPUSERP.Data.Entity.MasterData;
using OPUSERP.HRPMS.Data.Entity.Attendance;

namespace OPUSERP.Areas.HRPMSMasterData.Controllers
{
    [Authorize]
    [Area("HRPMSMasterData")]
    public class EventDutyController : Controller
    {
       
        private readonly IEventDutyService EventDutyService;
    

        public EventDutyController(IHostingEnvironment hostingEnvironment, IEventDutyService EventDutyService)
        {
            
            this.EventDutyService = EventDutyService;
           
        }

        // GET: Branch
        public async Task<IActionResult> Index()
        {
            EventDutyViewModel model = new EventDutyViewModel
            {
                employeeAllocations = await EventDutyService.GetEmployeeAllocation(),
                dutyMasters = await EventDutyService.Getduty()

            };
            return View(model);
        }

        // POST: Branch/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] EventDutyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.employeeAllocations = await EventDutyService.GetEmployeeAllocation();
                model.dutyMasters = await EventDutyService.Getduty();
                return View(model);
            }

            EmployeeAllocation data = new EmployeeAllocation
            {
                Id = model.AllocationId,
                dutyId = model.dutyId,
                fromDate = model.fromDate,
                toDate = model.toDate,
                location =model.location,
                employeeInfoId = model.employeeInfoId,
				
            };

            await EventDutyService.SaveEmployeeAllocation(data);

            return RedirectToAction(nameof(Index));
        }
        public async Task<JsonResult> Delete(int Id)
        {
            await EventDutyService.DeleteEmployeeAllocationById(Id);
            return Json(true);
        }

		// GET: SubBranch
		public async Task<IActionResult> Duty()
        {
            EventDutyViewModel model = new EventDutyViewModel
            {
                dutyMasters = await EventDutyService.Getduty(),
               
            };
            return View(model);
        }

		// POST: SubBranch/Edit
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Duty([FromForm] EventDutyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.dutyMasters = await EventDutyService.Getduty();
             
                return View(model);
            }

            SpecialEventDutyMaster data = new SpecialEventDutyMaster
            {
                Id = (int)model.dutyId,
                name = model.name,
                status = model.status,
               


            };

            await EventDutyService.Saveduty(data);

            return RedirectToAction(nameof(Duty));
        }

        public async Task<JsonResult> DeleteDuty(int Id)
        {
            await EventDutyService.DeletedutyById(Id);
            return Json(true);
        }
    }
}