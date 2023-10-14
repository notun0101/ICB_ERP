using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.Payroll.Models;
using OPUSERP.CLUB.Services.Member.Interfaces;
using OPUSERP.Data.Entity;
using OPUSERP.Payroll.Data.Entity.Fixation;
using OPUSERP.Payroll.Data.Entity.Salary;
using OPUSERP.Payroll.Services.Fixation;
using OPUSERP.Payroll.Services.Fixation.Interfaces;
using OPUSERP.Payroll.Services.IncomeTax;
using OPUSERP.Payroll.Services.IncomeTax.Interfaces;
using OPUSERP.Utility.Models;
using System.Linq;
using System.Threading.Tasks;


namespace OPUSERP.Areas.Payroll.Controllers
{
    [Area("Payroll")]
    [Authorize]
    public class FixationPeriodController : Controller
    {

        private readonly IPersonalInfoService _personalInfoService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFixationPeriodService _fixationPeriodService;
       
        public FixationPeriodController(IPersonalInfoService personalInfoService, UserManager<ApplicationUser> userManager, IFixationPeriodService fixationPeriodService)
        {
            this._userManager = userManager;
            this._personalInfoService = personalInfoService;
            this._fixationPeriodService = fixationPeriodService;
        }
        public async Task<IActionResult> CreateFixationPeriod()
        {
            var model = new FixationPeriodViewModel();
            {
                model.fixationPeriods = await _fixationPeriodService.GetAllFixationPeriod();
            };

           
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFixationPeriod([FromForm] FixationPeriodViewModel model)
        {
            if (ModelState.IsValid)
            {
                //go on as normal
            }
            else
            {


                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
            }

            //if (!ModelState.IsValid)
            //{
            //    model.fixationperiods = await _fixationperiodservice.getallfixationperiod();
            //    return RedirectToAction("fixationperiod");
            //}

            FixationPeriod data = new FixationPeriod
            {
                Id = model.Id,
                PeriodName = model.PeriodName,
                FixationTypeId = model.FixationTypeId,
                FixationYear = model.FixationYear,
                ContextId = model.ContextId,
                LockLevel = model.LockLevel,
                LockDate = model.LockDate,
                LockedBy = model.LockedBy,
            };

            var result = await _fixationPeriodService.IsDuplicateFixationPeriod(data);
            if (result == 1)

            return new OkObjectResult(new ResponseObject { Status = "NOK", Message = "Duplicate Record Found." });
        
        

        await _fixationPeriodService.SaveFixationPeriod(data);
            return RedirectToAction("CreateFixationPeriod");
        }

        public async Task<IActionResult> DeleteFixationPeriod(int Id)
        {
            var data = await _fixationPeriodService.DeleteFixationPeriod(Id);
            return Json("ok");
        }

       
    }
}
