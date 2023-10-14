using OPUSERP.Data.Entity.Training;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.TrainingNew;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.MasterData.Interfaces
{
    public interface IYearCourseTitleService
    {
        Task<bool> SaveYear( Year year);
        Task<IEnumerable<Year>> GetYear();
        Task<Year> GetYearById(int id);
        Task<Year> GetYearByName(string name);
        Task<bool> DeleteYearById(int id);

        Task<bool> SaveCourseTitle(CourseTitle courseTitle);
        Task<IEnumerable<CourseTitle>> GetCourseTitle();
        Task<CourseTitle> GetCourseTitleById(int id);
        Task<bool> DeleteCourseTitleById(int id);
		Task<IEnumerable<TrainingInfoNew>> GetAllFinishedTrainingList();
		Task<TrainingInfoNew> GetTrainingInfoById(int id);
		Task<IEnumerable<TrainingInfoNew>> GetAllTrainingList();
		Task<int> SaveTrainingPerticipant(TrainingPerticipants model);
		Task<IEnumerable<EmployeeInfo>> GetPerticipantsByTrainingId(int id);
        Task<IEnumerable<TrainingPerticipants>> GetAllTrainingPerticipants();

    }
}
