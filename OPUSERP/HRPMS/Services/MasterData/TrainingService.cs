using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using Microsoft.EntityFrameworkCore;
using OPUSERP.Data.Entity.Training;
using OPUSERP.HRPMS.Data.Entity.TrainingNew;

namespace OPUSERP.HRPMS.Services.MasterData
{
    public class TrainingService : ITrainingService
    {
        private readonly ERPDbContext _context;

        public TrainingService(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteTrainingCategoryById(int id)
        {
            _context.trainingCategories.Remove(_context.trainingCategories.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TrainingCategory>> GetTrainingCategories()
        {
            return await _context.trainingCategories.OrderBy(x => x.trainingCategoryName).AsNoTracking().ToListAsync();
        }

        public async Task<TrainingCategory> GetTrainingCategoryById(int id)
        {
            return await _context.trainingCategories.FindAsync(id);
        }

        public async Task<bool> SaveTrainingCategory(TrainingCategory trainingCategory)
        {
            if(trainingCategory.Id != 0)
                _context.trainingCategories.Update(trainingCategory);
            else
                _context.trainingCategories.Add(trainingCategory);
            return 1 == await _context.SaveChangesAsync();
        }
        //Training Institute
        public async Task<bool> DeleteTrainingInstituteById(int id)
        {
            _context.trainingInstitutes.Remove(_context.trainingInstitutes.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TrainingInstitute>> GetTrainingInstitute()
        {
            return await _context.trainingInstitutes.OrderBy(x => x.trainingInstituteName).AsNoTracking().ToListAsync();
        }
          public async Task<IEnumerable<TrainingSubject>> GetTrainingSubject()
        {
            return await _context.trainingSubjects.AsNoTracking().ToListAsync();
        }


        public async Task<TrainingInstitute> GetTrainingInstituteById(int id)
        {
            return await _context.trainingInstitutes.FindAsync(id);
        }

        public async Task<bool> SaveTrainingInstitute(TrainingInstitute trainingInstitute)
        {
            if(trainingInstitute.Id != 0)
                _context.trainingInstitutes.Update(trainingInstitute);
            else
                _context.trainingInstitutes.Add(trainingInstitute);
            return 1 == await _context.SaveChangesAsync();
        }
         public async Task<bool> SaveTrainingSubject(TrainingSubject trainingSubject)
        {
            if(trainingSubject.Id != 0)
                _context.trainingSubjects.Update(trainingSubject);
            else
                _context.trainingSubjects.Add(trainingSubject);
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteTrainingSubjectById(int id)
        {
            _context.trainingSubjects.Remove(_context.trainingSubjects.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
		#region TrainingSchedule Date:19/12/2021
		public async Task<IEnumerable<TrainingScheduleDetails>> GetTrainingScheduleDetails(int scheduleId )
		{
			//return await _context.trainingScheduleDetails.OrderBy(x => x.trainingInstituteName).AsNoTracking().ToListAsync();
			return await _context.trainingScheduleDetails.Include(x => x.trainingSchedule).Where(x=>x.trainingScheduleId==scheduleId).OrderBy(x => x.Id).ToListAsync();
		}
		public async Task<IEnumerable<TrainingScheduleTrainer>> GetTrainingScheduleTrainer(int scheduleId)
		{
			return await _context.trainingScheduleTrainers.Include(x => x.trainingSchedule).Include(x=>x.resourcePerson).Where(x => x.trainingScheduleId == scheduleId).OrderBy(x => x.Id).ToListAsync(); 
		}
		public async Task<IEnumerable<TrainingScheduleTrainee>> GetTrainingScheduleTrainee(int scheduleId)
		{
			return await _context.trainingScheduleTrainee.Include(x => x.trainingSchedule).Include(x => x.employeeInfo).Where(x => x.trainingScheduleId == scheduleId).OrderBy(x => x.Id).ToListAsync();
		}
		public async Task<int> SaveTrainingScheduleDetails(List<TrainingScheduleDetails> trainingScheduleDetails)
		{
			var list = _context.trainingScheduleDetails.Where(x => x.trainingScheduleId == trainingScheduleDetails[0].trainingScheduleId).ToList();
			_context.trainingScheduleDetails.RemoveRange(list);
			await _context.SaveChangesAsync();

			_context.trainingScheduleDetails.AddRange(trainingScheduleDetails);
			await _context.SaveChangesAsync();
			return trainingScheduleDetails[0].Id;
		}
		public async Task<int> SaveTrainingScheduleTrainer(List<TrainingScheduleTrainer> trainingScheduleTrainer)
		{
			var list = _context.trainingScheduleTrainers.Where(x => x.trainingScheduleId == trainingScheduleTrainer[0].trainingScheduleId).ToList();
			_context.trainingScheduleTrainers.RemoveRange(list);
			await _context.SaveChangesAsync();

			_context.trainingScheduleTrainers.AddRange(trainingScheduleTrainer);
			await _context.SaveChangesAsync();
			return trainingScheduleTrainer[0].Id;
		}
		public async Task<int> SaveTrainingScheduleTrainee(List<TrainingScheduleTrainee> trainingScheduleTrainee)
		{
			var list = _context.trainingScheduleTrainee.Where(x => x.trainingScheduleId == trainingScheduleTrainee[0].trainingScheduleId).ToList();
			_context.trainingScheduleTrainee.RemoveRange(list);
			await _context.SaveChangesAsync();

			_context.trainingScheduleTrainee.AddRange(trainingScheduleTrainee);
			await _context.SaveChangesAsync();
			return trainingScheduleTrainee[0].Id;
		}

		#endregion

	}
}
