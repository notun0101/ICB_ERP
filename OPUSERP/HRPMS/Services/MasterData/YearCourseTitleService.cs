using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OPUSERP.HRPMS.Data.Entity.TrainingNew;
using OPUSERP.Data.Entity.Training;
using OPUSERP.HRPMS.Data.Entity.Employee;

namespace OPUSERP.HRPMS.Services.MasterData
{
    public class YearCourseTitleService: IYearCourseTitleService
    {
        private readonly ERPDbContext _context;

        public YearCourseTitleService(ERPDbContext context)
        {
            _context = context;
        }

        //Year Region

        public async Task<bool> SaveYear(Year year)
        {
            if (year.Id != 0)
                _context.years.Update(year);
            else
                _context.years.Add(year);

            return 1 == await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Year>> GetYear()
        {
           return await _context.years.AsNoTracking().OrderByDescending(x=>x.year).ToListAsync();
        }

        public async Task<Year> GetYearById(int id)
        {
            return await _context.years.FindAsync(id);
        }

        public async Task<Year> GetYearByName(string name)
        {
            return await _context.years.Where(a => a.year == name).FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteYearById(int id)
        {
            _context.years.Remove(_context.years.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        //Course Title Region

        public async Task<bool> SaveCourseTitle(CourseTitle courseTitle)
        {
            if (courseTitle.Id != 0)
                _context.courseTitles.Update(courseTitle);
            else
                _context.courseTitles.Add(courseTitle);

            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CourseTitle>> GetCourseTitle()
        {
            return await _context.courseTitles.AsNoTracking().ToListAsync();
        }

		public async Task<IEnumerable<TrainingInfoNew>> GetAllFinishedTrainingList()
        {
            return await _context.trainingInfoNews.Where(x => Convert.ToDateTime(x.endDate) <= DateTime.Now).AsNoTracking().ToListAsync();
        }
		public async Task<TrainingInfoNew> GetTrainingInfoById(int id)
        {
            return await _context.trainingInfoNews.Include(x => x.trainer).Include(x => x.trainingInstitute).Include(x => x.subject).Include(x => x.country).Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }
		public async Task<IEnumerable<TrainingInfoNew>> GetAllTrainingList()
        {
            return await _context.trainingInfoNews.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TrainingPerticipants>> GetAllTrainingPerticipants()
        {
            return await _context.trainingPerticipants.Include(x => x.trainee).Include(x => x.training).AsNoTracking().ToListAsync();
        }
		public async Task<int> SaveTrainingPerticipant(TrainingPerticipants model)
		{
			if (model.Id != 0)
			{
				_context.trainingPerticipants.Update(model);
			}
			else
			{
				_context.trainingPerticipants.Add(model);
			}
			await _context.SaveChangesAsync();
			return model.Id;
		}
		public async Task<IEnumerable<EmployeeInfo>> GetPerticipantsByTrainingId(int id) //trainingId
		{
			var employees = await (from p in _context.trainingPerticipants.Where( x => x.trainingId == id)
								   join e in _context.employeeInfos
								   on p.traineeId equals e.Id 
								   select e).ToListAsync();
			return employees;
		}


		public async Task<CourseTitle> GetCourseTitleById(int id)
        {
            return await _context.courseTitles.FindAsync(id);
        }

        public async Task<bool> DeleteCourseTitleById(int id)
        {
            _context.courseTitles.Remove(_context.courseTitles.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
    }
}
