using OPUSERP.Areas.Auth.Models;
using OPUSERP.Areas.HRPMSTrainingNew.Models;
using OPUSERP.Data.Entity.Training;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.TrainingNew;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.TrainingNew.Interfaces
{
    public interface ITrainingNewService
    {
        Task<int> SaveTrainingInfoNew(TrainingInfoNew trainingInfoNew);
		Task<int> SaveTrainingResourcePerson(List<TrainingResourcePerson> trainingResourcePerson);
		Task<int> UpdateTrainingResourcePerson(List<TrainingResourcePerson> trainingResourcePerson);
		Task<IEnumerable<ResourcePerson>> GetTrainingResourcePersonById(int id);
		Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoNew();
        Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoNewByType(int type, string org);
        Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoNewByStatus(int statu);
        Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoNewByStatusandType(int statu, int type, string org);
        Task<TrainingInfoNew> GetTrainingInfoNewById(int id);
        Task<TrainingInfoNew> GetTrainingInfoNewByIdNew(int trainingId);
        Task<bool> DeleteTrainingInfoNewById(int id);
        Task<bool> UpdateTrainingInfoNewById(TrainingInfoNew trainingInfoNew);
        Task<IEnumerable<EnrolledTrainee>> GetTrainingPerticipantsByTrainingId(int id);
        Task<int> SaveTrainingSchedule(TrainingSchedule model);
        Task<EnrolledTrainee> GetTrainingPerticipantsById(int enrollId);
        Task<int> SaveEnrollTrainee(EnrolledTrainee model);
        Task<IEnumerable<TrainingSchedule>> GetAllTrainingSchedulesByTrainingId(int id);
        Task<IEnumerable<TrainingScheduleVm>> GetAllTrainingSchedulesByTrainingIdSp(int id);
        Task<int> DeleteScheduleById(int id);

        Task<IEnumerable<TrainingInfoNew>> GetMyTrainingInfoNew(string empCode);
        Task<IEnumerable<YearlyTraining>> GetYealyTrainingsByTrainingYear(int id);
        Task<IEnumerable<InstituteTrainingReport>> GetTrainingsByYearAndMonth(int year, int month);
        Task<IEnumerable<InstituteTrainingReport>> GetTrainingsByYear(int year);
        Task<IEnumerable<CourseWiseTraining>> GetCourseWiseTrainingDateRange(int courseId, DateTime? startDate, DateTime? endDate);
        Task<IEnumerable<ResourcePerson>> GetTrainerInfoNewByTraining(int? Id);
        Task<AspNetUsersViewModel> GetUserInfoByEmpCode(string code);
        Task<IEnumerable<TraningAndPerticipantsVm>> GetTrainingWithPerticipantsById(int id);
		Task<IEnumerable<TraningAndPerticipantsVm>> GetTrainingWithPerticipantsByIdNew(int id);

		//For Dashboard
		Task<IEnumerable<TrainingInfoNew>> AllTrainingOfLastYearByOrg(string org);
        Task<IEnumerable<EnrolledTrainee>> GetEnrolledTrainingByEmpId(int id);
        //For Feedback

        Task<int> SaveTrainingFeedBack(TrainingFeedback trainingFeedback);
        Task<bool> DeleteFeedback(int id);

        Task<IEnumerable<TrainingFeedback>> GetTrainingFeedback();
        Task<IEnumerable<TrainingPerticipants>> GetTrainingPerticipantsByEmpId(int empId);
        Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoNews();
        Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoNewByDate(DateTime? start, DateTime? end);
        Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoNewByDateAndTrainerId(DateTime? start, DateTime? end, int? trainerId);
        Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoNewByDateAndTrainingId(DateTime? start, DateTime? end, int? TrainingId);
        Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoByDateTrainingIdandTrainerId(DateTime? start, DateTime? end, int? TrainingId, int? trainerId);
        Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoNewByTrainerIdandTrainingId(int? trainerId, int? TrainingId);
        Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoNewByTrainerId(int? trainerId);
        Task<IEnumerable<TrainingInfoNew>> GetAllTrainingInfoNew();

		Task<IEnumerable<TrainingInfoNew>> GetAllTrainingInfoNewReport();

		Task<IEnumerable<ResourcePerson>> GetAllTrainer();
        Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoByDateTrainingIdandTrainerIdSubjectId(DateTime? start, DateTime? end, int? TrainingId, int? trainerId, int? SubjectId);
        Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoNewByTrainerIdandSubjectId(int? trainerId, int? SubjectId);
        Task<IEnumerable<EnrolledTrainee>> GetTrainingPerticipantsBySubjectId(int id);
        Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoNewByDateAndSubjectId(DateTime? start, DateTime? end, int? SubjectId);
        Task<IEnumerable<TrainingInfoNew>> GetAllTrainingInfoWithSubject(int? SubjectId);


        Task<IEnumerable<TrainingInfoNew>> GetTrainerInfoNewByTrainingId(int? Id);



        Task<IEnumerable<TrainingHonorariumViewModel>> GetTrainingHonorarium();
        Task<int> SaveUpdateTrainingHonorarium(TrainingHonorarium trainingHonorarium);
        Task<bool> DeleteTrainingHonorariumById(int id);
        Task<IEnumerable<TrainingHonorariumViewModel>> MoneyReceptReportPdf(int id);



        Task<IEnumerable<BangladeshBankManpowerReportVM>> GetTrainingInfoForBangladeshBankManPower(int year, int month);
       
        Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoNewForManpower(string year);
        Task<IEnumerable<CourseWiseParticipents>> GetCourseWisePerticipientsDateRange(int courseId, DateTime? startDate, DateTime? endDate);
        Task<IEnumerable<InstituteTrainingReportViewModel>> GetTrainingsByInstituteId();
		Task<IEnumerable<EmployeeInfo>> GetemployeeInfos();
        Task<EnrolledTrainee> GetEnrollTraineeById(int id, int trainingId);
        #region GetSourceOfFund Date:11/12/2021
        Task<IEnumerable<SourceOfFund>> GetSourceOfFund();
		Task<int> SaveUpdateSourceOfFund(SourceOfFund sourceOfFund);
		Task<bool> DeletesourceOfFundById(int id);
		#endregion

		#region CourseType  Date:11/12/2021
		Task<IEnumerable<CourseType>> GetCourseType();
		Task<int> SaveUpdateCourseType(CourseType courseType);
		Task<bool> DeletecourseTypeById(int id);
		#endregion
		Task<IEnumerable<ManpowerSummary>> GetManpowerSummaryList();
		Task<IEnumerable<ScheduleResourcePersonVm>> GetScheduleResourcePerson(int trainingId);
		Task<string> GetEmployeePostingPlace(int? EmpId);
		Task<int> SaveIsCompleteTraining(TrainingInfoNew data);
		Task<IEnumerable<TrainingScheduleTrainersVm>> GetTrainerListByTrainingId(int? trainingId);
        Task<IEnumerable<TrainingofonlyBDViewModel>> GetTrainingsByYearAndMonthforBank(string type, int trainingid, int localOforeign, int year, int month);
        Task<IEnumerable<IndTrainingReportSPViewModel>> GetTraingInfoSPByeempId(int id);
        Task<int> UpdateCoordinator(List<CourseCoordinator> courseCoordinator);
        Task<int> SaveCoordinator(List<CourseCoordinator> courseCoordinator);
        Task<IEnumerable<CourseCoordinator>> GetcourseCoordinatorByTrId(int id);
		Task<IEnumerable<TrainingSchedule>> GetAllTrainingSchedules();
		Task<IEnumerable<TrainingInfoNew>> GetAllTrainingInfoNewcomplited();

        bool checkShortOrder(int no, int Tid);


    }
}
