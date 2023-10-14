using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using Microsoft.EntityFrameworkCore;
using OPUSERP.Areas.HRPMSEmployee.Models;


namespace OPUSERP.HRPMS.Services.MasterData
{
    public class HolidayService : IHolidayService
    {
        private readonly ERPDbContext _context;

        public HolidayService(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveHoliday(Holiday holiday)
        {
            if (holiday.Id != 0)
                _context.holidays.Update(holiday);
            else
                _context.holidays.Add(holiday);

            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Holiday>> GetAllHoliday()
        {
            return await _context.holidays.AsNoTracking().ToListAsync();
        }
        public async Task<int> AddFridayAndSaturday(int year)
        {
            var firstDay = new DateTime(year, 10, 6);
            var lastDay = new DateTime(year, 12, 31);
            for (int i = 0; i < (lastDay - firstDay).Days; i++)
            {
                if (firstDay.DayOfWeek.ToString() == "Friday")
                {
                    var data = new Holiday
                    {
                        weeklyHoliday = firstDay,
                        holidayName = "Friday",
                        holidayNameBn = "শুক্রবার",
                        year = year
                    };
                    _context.holidays.Add(data);
                    await _context.SaveChangesAsync();
                }
                if (firstDay.DayOfWeek.ToString() == "Saturday")
                {
                    var data = new Holiday
                    {
                        weeklyHoliday = firstDay,
                        holidayName = "Saturday",
                        holidayNameBn = "শনিবার",
                        year = year
                    };
                    _context.holidays.Add(data);
                    await _context.SaveChangesAsync();
                }
                firstDay = firstDay.AddDays(1);
            }
            return year;
        }

        public async Task<Holiday> GetHolidayById(int id)
        {
            return await _context.holidays.FindAsync(id);
        }

        public async Task<bool> DeleteHolidayById(int id)
        {
            _context.holidays.Remove(_context.holidays.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public IEnumerable<DateTime> GetHolidaysByDayNameAndYear(string day, int year)
        {
            var dateList = new List<DateTime>();

            DateTime startdate = new DateTime(year, 1, 1);
            DateTime enddate = new DateTime(year, 12, 31);
            if (day == "Friday")
            {
                while (startdate.DayOfWeek != DayOfWeek.Friday)
                    startdate = startdate.AddDays(1);

                while (startdate < enddate)
                {
                    dateList.Add(startdate);
                    //yield return startdate;
                    startdate = startdate.AddDays(7);
                };
            }
            else if (day == "Saturday")
            {
                while (startdate.DayOfWeek != DayOfWeek.Saturday)
                    startdate = startdate.AddDays(1);

                while (startdate < enddate)
                {
                    dateList.Add(startdate);
                    //yield return startdate;
                    startdate = startdate.AddDays(7);
                };
            }
            else
            {

            }
            
            return dateList;
        }



    }
}
