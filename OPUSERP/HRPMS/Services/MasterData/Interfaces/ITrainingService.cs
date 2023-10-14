using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Data.Entity.Training;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.TrainingNew;

namespace OPUSERP.HRPMS.Services.MasterData.Interfaces
{
    public interface ITrainingService
    {
        Task<bool> SaveTrainingCategory(TrainingCategory trainingCategory);
        Task<IEnumerable<TrainingCategory>> GetTrainingCategories();
        Task<TrainingCategory> GetTrainingCategoryById(int id);
        Task<bool> DeleteTrainingCategoryById(int id);

        Task<bool> SaveTrainingInstitute(TrainingInstitute trainingInstitute);
        Task<IEnumerable<TrainingInstitute>> GetTrainingInstitute();
        Task<TrainingInstitute> GetTrainingInstituteById(int id);
        Task<bool> DeleteTrainingInstituteById(int id);
        Task<IEnumerable<TrainingSubject>> GetTrainingSubject();
        Task<bool> SaveTrainingSubject(TrainingSubject trainingSubject);
        Task<bool> DeleteTrainingSubjectById(int id);
		Task<IEnumerable<TrainingScheduleDetails>> GetTrainingScheduleDetails(int scheduleId);
		Task<IEnumerable<TrainingScheduleTrainer>> GetTrainingScheduleTrainer(int scheduleId);
		Task<IEnumerable<TrainingScheduleTrainee>> GetTrainingScheduleTrainee(int scheduleId);
		Task<int> SaveTrainingScheduleDetails(List<TrainingScheduleDetails> trainingScheduleDetails);
		Task<int> SaveTrainingScheduleTrainer(List<TrainingScheduleTrainer> trainingScheduleTrainer);
		Task<int> SaveTrainingScheduleTrainee(List<TrainingScheduleTrainee> trainingScheduleTrainee);

	}
}
