using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSMasterData.Models;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.Payroll.Services.Salary.Interfaces;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Controllers
{
    [Authorize]
    [Area("HRPMSMasterData")]
    public class HolidayController : Controller
    {
        private readonly IHolidayService holidayService;
        private readonly ISalaryService salaryService;

        public HolidayController(IHolidayService holidayService, ISalaryService salaryService)
        {
            this.holidayService = holidayService;
            this.salaryService = salaryService;
        }

        // GET: LevelofEducation
        public async Task<IActionResult> Index()
        {
            await holidayService.AddFridayAndSaturday(2021);
            HolidayViewModel model = new HolidayViewModel
            {
                holidays = await holidayService.GetAllHoliday(),
                salaryYearsList = await salaryService.GetAllSalaryYear(),
            };
            return View(model);
        }

        public async Task<IActionResult> GetAllHoliday()
        {
            return Json(await holidayService.GetAllHoliday());
        }

        // POST: LevelofEducation/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] HolidayViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.holidays = await holidayService.GetAllHoliday();
                return View(model);
            }

            Holiday data = new Holiday
            {
                Id = model.holidayId,
                holidayName = model.holidayName,
                holidayNameBn = model.holidayNameBn,
                weeklyHoliday = model.weeklyHoliday,
                holiday = model.weeklyHoliday,
                year = model.year
            };

            await holidayService.SaveHoliday(data);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<JsonResult> DeleteHolidayById(int Id)
        {
            await holidayService.DeleteHolidayById(Id);
            return Json(true);
        }

        public async Task<IActionResult> AddHolidaysByDayNameAndYear(string day, int year)
        {
            var data = holidayService.GetHolidaysByDayNameAndYear(day, year);
            foreach (var item in data)
            {
                var model = new Holiday
                {
                    Id = 0,
                    weeklyHoliday = item,
                    holidayName = day,
                    year = year,
                    holiday = item
                };
                await holidayService.SaveHoliday(model);
            }
            return Json(data);
        }
    }
}
