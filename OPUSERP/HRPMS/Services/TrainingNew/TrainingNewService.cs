using Microsoft.EntityFrameworkCore;
using OPUSERP.Areas.Auth.Models;
using OPUSERP.Areas.HRPMSTrainingNew.Models;
using OPUSERP.Data;
using OPUSERP.Data.Entity.Training;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.TrainingNew;
using OPUSERP.HRPMS.Services.TrainingNew.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.TrainingNew
{
	public class TrainingNewService : ITrainingNewService
	{
		private readonly ERPDbContext _context;

		public TrainingNewService(ERPDbContext context)
		{
			_context = context;
		}

		//ApplicationForm
		public async Task<bool> DeleteTrainingInfoNewById(int id)
		{
			_context.trainingInfoNews.Remove(_context.trainingInfoNews.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<bool> DeleteFeedback(int id)
		{
			_context.trainingFeedbacks.Remove(_context.trainingFeedbacks.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoNew()
		{
			return await _context.trainingInfoNews.Include(x => x.employeeType).AsNoTracking().ToListAsync();
		}



		public async Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoNewByType(int type, string org)
		{
			return await _context.trainingInfoNews.Include(x => x.employeeType).Where(x => x.trainingType == type).Where(x => x.orgType == org).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoNews()
		{
			var result = await _context.trainingInfoNews.Include(x => x.employeeType).Include(x => x.courseType).Include(x => x.sourceOfFund).OrderByDescending(x => x.Id).Where(x => x.status != 255).AsNoTracking().ToListAsync();
			return result;
			//			sourceOfFund
			//courseType
		}

		public async Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoNewByStatus(int statu)
		{
			return await _context.trainingInfoNews.Include(x => x.employeeType).Where(x => x.status == statu).AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoNewByStatusandType(int statu, int type, string org)
		{
			return await _context.trainingInfoNews.Include(x => x.employeeType).Where(x => x.status == statu).Where(x => x.trainingType == type).Where(x => x.orgType == org).AsNoTracking().ToListAsync();
		}

		public async Task<TrainingInfoNew> GetTrainingInfoNewById(int id)
		{
			var data=await _context.trainingInfoNews.Include(x => x.employeeType).Include(x => x.country).Include(x => x.trainer).Include(x => x.subject).Where(x => x.Id == id).FirstOrDefaultAsync();
            return data;
		}


        public async Task<TrainingInfoNew> GetTrainingInfoNewByIdNew(int trainingId)
		{
			var data=await _context.trainingInfoNews.Include(x => x.employeeType).Include(x => x.country).Include(x => x.trainer).Include(x => x.subject).Where(x => x.Id==trainingId).FirstOrDefaultAsync();
            return data;
		}
         public async Task<EnrolledTrainee> GetEnrollTraineeById(int id,int trainingId)
		{
			var data=await _context.enrolledTrainees.Where(x => x.employeeId == id && x.trainingInfoNewId==trainingId).FirstOrDefaultAsync();
            return data;
		}


		public async Task<IEnumerable<ResourcePerson>> GetTrainingResourcePersonById(int id)
		{
			var result = from person in _context.resourcePersons
						 join training in _context.trainingResourcePersons on person.Id equals training.resourcePersonId
						 where training.trainingInfoNewId == id
						 select person;

			return await result.ToListAsync();
		}
		public async Task<IEnumerable<EnrolledTrainee>> GetEnrolledTrainingByEmpId(int id)
		{
			return await _context.enrolledTrainees.Include(x => x.employee).Include(x => x.trainingInfoNew).Where(x => x.employeeId == id).ToListAsync();
		}
        public async Task<IEnumerable<CourseCoordinator>> GetcourseCoordinatorByTrId(int id)
		{
			return await _context.courseCoordinators.Include(x => x.employeeInfo.lastDesignation).Include(x => x.employeeInfo).Include(x => x.trainingInfoNew).Where(x => x.trainingInfoNewId == id).ToListAsync();
		}

        public bool checkShortOrder(int no,int Tid)
        {
            return 0 < _context.trainingSchedules.Where(x => x.sortOrder == no && x.trainingId == Tid).Count();
        }

        public async Task<IEnumerable<IndTrainingReportSPViewModel>> GetTraingInfoSPByeempId(int id)
        {
            return await _context.indTrainingReportSPViewModels.FromSql($"SP_TraingHistoryEmpId @p0", id).ToListAsync();
        }

        public async Task<EnrolledTrainee> GetTrainingPerticipantsById(int enrollId)
		{
			return await _context.enrolledTrainees.Include(x => x.trainingInfoNew).Include(x => x.employee).Where(x => x.Id == enrollId).FirstOrDefaultAsync();
		}
		public async Task<IEnumerable<TraningAndPerticipantsVm>> GetTrainingWithPerticipantsById(int id)
		{
			return await _context.trainingAndPerticipants.FromSql($"sp_getTraningWithPerticipants @p0", id).ToListAsync();
		}
		public async Task<IEnumerable<TraningAndPerticipantsVm>> GetTrainingWithPerticipantsByIdNew(int id)
		{
			return await _context.trainingAndPerticipants.FromSql($"sp_getTraningWithPerticipantsNew @p0", id).ToListAsync();
		}
		public async Task<IEnumerable<YearlyTraining>> GetYealyTrainingsByTrainingYear(int id)
		{
			return await _context.yearlyTrainings.FromSql($"sp_getYearlyTrainingReport @p0", id).ToListAsync();
		}
		public async Task<IEnumerable<InstituteTrainingReport>> GetTrainingsByYearAndMonth(int year, int month)
		{
			return await _context.instituteTrainings.FromSql($"sp_GetTrainingsByYearAndMonth @p0, @p1", year, month).ToListAsync();
		}
        public async Task<IEnumerable<TrainingofonlyBDViewModel>> GetTrainingsByYearAndMonthforBank(string type,int trainingid,int localOforeign, int year, int month)
        {
            return await _context.TrainingofonlyBDViewModels.FromSql($"sp_GetOutsideBankTrainings @p0, @p1, @p2,@p3,@p4", type, trainingid, localOforeign, year, month).ToListAsync();
        }
        public async Task<IEnumerable<InstituteTrainingReport>> GetTrainingsByYear(int year)
		{
			return await _context.instituteTrainings.FromSql($"sp_GetTrainingsByYear @p0", year).ToListAsync();
		}
        
        public async Task<IEnumerable<InstituteTrainingReportViewModel>> GetTrainingsByInstituteId()
		{
			return await _context.instituteTrainingReportViewModels.FromSql($"sp_GetTrainingsByInstituteId").ToListAsync();
		}

		public async Task<IEnumerable<CourseWiseTraining>> GetCourseWiseTrainingDateRange(int courseId, DateTime? startDate, DateTime? endDate)
		{
			return await _context.courseWiseTrainings.FromSql($"sp_GetTrainingsByCourseAndRange @p0, @p1, @p2", courseId, startDate, endDate).ToListAsync();
		}
		public async Task<IEnumerable<TrainingSchedule>> GetAllTrainingSchedulesByTrainingId(int id)
		{
			var data = await _context.trainingSchedules.Include(x => x.trainer).Include(x => x.training).Where(x => x.trainingId == id).OrderBy(x => x.sortOrder).AsNoTracking().ToListAsync();
			return data;
		}
		public async Task<IEnumerable<TrainingSchedule>> GetAllTrainingSchedules()
		{
			var data = await _context.trainingSchedules.Include(x => x.trainer).Include(x => x.training).OrderBy(x => x.sortOrder).AsNoTracking().ToListAsync();
			return data;
		}


		public async Task<IEnumerable<TrainingScheduleVm>> GetAllTrainingSchedulesByTrainingIdSp(int id)
		{
			return await _context.trainingScheduleVms.FromSql($"sp_getAllTrainingScheduleByTrainingId @p0", id).ToListAsync();
		}

		public async Task<int> DeleteScheduleById(int id)
		{
			var trainingId = await _context.trainingSchedules.Where(x => x.Id == id).Select(x => x.trainingId).FirstOrDefaultAsync();
			_context.trainingSchedules.Remove(await _context.trainingSchedules.FindAsync(id));
			await _context.SaveChangesAsync();
			return Convert.ToInt32(trainingId);
		}

		public async Task<IEnumerable<CourseWiseParticipents>> GetCourseWisePerticipientsDateRange(int courseId, DateTime? startDate, DateTime? endDate)
		{
			return await _context.courseWiseParticipents.FromSql($"sp_GetPercipantsByCourseAndRange @p0, @p1, @p2", courseId, startDate, endDate).ToListAsync();
		}


		public async Task<IEnumerable<ResourcePerson>> GetAllTrainer()
		{
			return await _context.resourcePersons.ToListAsync();
		}

		public async Task<int> SaveEnrollTrainee(EnrolledTrainee model)
		{
			if (model.Id != 0)
			{
				_context.enrolledTrainees.Update(model);
			}
			else
			{
				_context.enrolledTrainees.Add(model);
			}
			await _context.SaveChangesAsync();
			return model.Id;
		}
		public async Task<int> SaveTrainingSchedule(TrainingSchedule model)
		{
			if (model.Id != 0)
			{
				_context.trainingSchedules.Update(model);
			}
			else
			{
				_context.trainingSchedules.Add(model);
			}
			await _context.SaveChangesAsync();
			return model.Id;
		}
		public async Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoNewByDate(DateTime? start, DateTime? end)
		{
			var data = await _context.trainingInfoNews.Include(x => x.country).Include(x => x.institute).Include(x => x.subject).Include(x => x.trainer).Where(x => Convert.ToDateTime(x.startDate) >= start && Convert.ToDateTime(x.startDate) <= end).AsNoTracking().ToListAsync();
			return data;
		}
		public async Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoNewByDateAndTrainerId(DateTime? start, DateTime? end, int? trainerId)
		{
			var data = await _context.trainingInfoNews.Include(x => x.country).Include(x => x.institute).Include(x => x.subject).Include(x => x.trainer).Where(x => Convert.ToDateTime(x.startDate) >= start && Convert.ToDateTime(x.startDate) <= end && x.trainerId == trainerId).AsNoTracking().ToListAsync();
			return data;
		}
		public async Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoNewByDateAndTrainingId(DateTime? start, DateTime? end, int? TrainingId)
		{
			var data = await _context.trainingInfoNews.Include(x => x.country).Include(x => x.institute).Include(x => x.subject).Include(x => x.trainer).Where(x => Convert.ToDateTime(x.startDate) >= start && Convert.ToDateTime(x.startDate) <= end && x.Id == TrainingId).AsNoTracking().ToListAsync();
			return data;
		}
		public async Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoNewByDateAndSubjectId(DateTime? start, DateTime? end, int? SubjectId)
		{
			var data = await _context.trainingInfoNews.Include(x => x.country).Include(x => x.institute).Include(x => x.subject).Include(x => x.trainer).Where(x => Convert.ToDateTime(x.startDate) >= start && Convert.ToDateTime(x.startDate) <= end && x.Id == SubjectId).AsNoTracking().ToListAsync();
			return data;
		}

		public async Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoByDateTrainingIdandTrainerId(DateTime? start, DateTime? end, int? TrainingId, int? trainerId)
		{
			var data = await _context.trainingInfoNews.Include(x => x.country).Include(x => x.institute).Include(x => x.subject).Include(x => x.trainer).Where(x => Convert.ToDateTime(x.startDate) >= start && Convert.ToDateTime(x.startDate) <= end && x.Id == TrainingId && x.trainerId == trainerId).AsNoTracking().ToListAsync();
			return data;
		}
		public async Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoByDateTrainingIdandTrainerIdSubjectId(DateTime? start, DateTime? end, int? TrainingId, int? trainerId, int? SubjectId)
		{
			var data = await _context.trainingInfoNews.Include(x => x.country).Include(x => x.institute).Include(x => x.subject).Include(x => x.trainer).Where(x => Convert.ToDateTime(x.startDate) >= start && Convert.ToDateTime(x.startDate) <= end && x.Id == TrainingId && x.trainerId == trainerId && x.subjectId == SubjectId).AsNoTracking().ToListAsync();
			return data;
		}

		public async Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoNewByTrainerId(int? trainerId)
		{
			var data = await _context.trainingInfoNews.Include(x => x.country).Include(x => x.institute).Include(x => x.subject).Include(x => x.trainer).Where(x => x.trainerId == trainerId).AsNoTracking().ToListAsync();
			return data;
		}
		public async Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoNewByTrainerIdandTrainingId(int? trainerId, int? TrainingId)
		{
			var data = await _context.trainingInfoNews.Include(x => x.country).Include(x => x.institute).Include(x => x.subject).Include(x => x.trainer).Where(x => x.trainerId == trainerId && x.Id == TrainingId).AsNoTracking().ToListAsync();
			return data;
		}
		public async Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoNewByTrainerIdandSubjectId(int? trainerId, int? SubjectId)
		{
			var data = await _context.trainingInfoNews.Include(x => x.country).Include(x => x.institute).Include(x => x.subject).Include(x => x.trainer).Where(x => x.trainerId == trainerId && x.subjectId == SubjectId).AsNoTracking().ToListAsync();
			return data;
		}
		//new
		public async Task<IEnumerable<TrainingInfoNew>> GetTrainerInfoNewByTrainingId(int? Id)
		{
			var data = await _context.trainingInfoNews.Include(x => x.country).Include(x => x.institute).Include(x => x.subject).Include(x => x.trainer).Include(x => x.courseType).Where(x => x.Id == Id).AsNoTracking().ToListAsync();
			return data;
		}

		public async Task<IEnumerable<ResourcePerson>> GetTrainerInfoNewByTraining(int? Id)
		{
			var data = await _context.trainingResourcePersons.Where(x => x.trainingInfoNewId == Id).Select(x => x.resourcePerson).ToListAsync();
			return data;
		}

		public async Task<IEnumerable<TrainingInfoNew>> GetAllTrainingInfoNew()
		{
			var data = await _context.trainingInfoNews.Include(x => x.country).Include(x => x.institute).Include(x => x.subject).Include(x => x.trainer).Where(x => x.status != 255).OrderByDescending(x => x.Id).AsNoTracking().ToListAsync();
			return data;
		}
		public async Task<IEnumerable<TrainingInfoNew>> GetAllTrainingInfoNewcomplited()
		{
			var data = await _context.trainingInfoNews.Include(x => x.country).Include(x => x.institute).Include(x => x.subject).Include(x => x.trainer).Where(x => x.status == 255).OrderByDescending(x => x.Id).AsNoTracking().ToListAsync();
			return data;
		}



		public async Task<IEnumerable<TrainingInfoNew>> GetAllTrainingInfoNewReport()
		{
			var data = await _context.trainingInfoNews.Include(x => x.country).Include(x => x.institute).Include(x => x.subject).Include(x => x.trainer).OrderByDescending(x => x.Id).AsNoTracking().ToListAsync();
			return data;
		}
		public async Task<IEnumerable<TrainingInfoNew>> GetAllTrainingInfoWithSubject(int? SubjectId)
		{
			var data = await _context.trainingInfoNews.Include(x => x.country).Include(x => x.institute).Include(x => x.subject).Include(x => x.trainer).Where(x => x.subjectId == SubjectId).AsNoTracking().ToListAsync();
			return data;

		}
		public async Task<IEnumerable<TrainingInfoNew>> GetMyTrainingInfoNew(string empCode)
		{

			var data = await _context.trainingInfoNews.Include(x => x.country).Include(x => x.institute).Include(x => x.subject).Include(x => x.trainer).AsNoTracking().ToListAsync();
			var data1 = await (from t in _context.enrolledTrainees
							   join e in _context.employeeInfos on t.employeeId equals e.Id
							   join d in data on t.trainingInfoNewId equals d.Id
							   where e.employeeCode == empCode
							   select d).ToListAsync();

			return data1;
		}

		public async Task<IEnumerable<EnrolledTrainee>> GetTrainingPerticipantsByTrainingId(int id)
		{
			return await _context.enrolledTrainees.Include(x => x.trainingInfoNew).Include(x => x.employee).Include(x => x.trainingInfoNew.subject).Include(x => x.trainingInfoNew).Include(x => x.employee.department).Include(x => x.employee.lastDesignation).Where(x => x.trainingInfoNewId == id).ToListAsync();
		}
		public async Task<IEnumerable<EnrolledTrainee>> GetTrainingPerticipantsBySubjectId(int id)
		{
			return await _context.enrolledTrainees.Include(x => x.trainingInfoNew).Include(x => x.employee).Include(x => x.trainingInfoNew.subject).Include(x => x.trainingInfoNew).Include(x => x.employee.department).Include(x => x.employee.lastDesignation).Where(x => x.trainingInfoNew.Id == id).ToListAsync();
		}

		public async Task<bool> UpdateTrainingInfoNewById(TrainingInfoNew trainingInfoNew)
		{
			TrainingInfoNew trainingInfoNew1 = _context.trainingInfoNews.Find(trainingInfoNew.Id);
			trainingInfoNew1.startDateActual = trainingInfoNew.startDateActual;
			trainingInfoNew1.endDateActual = trainingInfoNew.endDateActual;
			trainingInfoNew1.noOfParticipantsActual = trainingInfoNew.noOfParticipantsActual;
			trainingInfoNew1.status = trainingInfoNew.status;
			trainingInfoNew1.amountActual = trainingInfoNew.amountActual;
			return 1 == await _context.SaveChangesAsync();
		}


		public async Task<int> SaveTrainingInfoNew(TrainingInfoNew trainingInfoNew)
		{
			if (trainingInfoNew.Id != 0)
				_context.trainingInfoNews.Update(trainingInfoNew);
			else
				_context.trainingInfoNews.Add(trainingInfoNew);
			await _context.SaveChangesAsync();
			return trainingInfoNew.Id;
		}
		public async Task<int> SaveTrainingResourcePerson(List<TrainingResourcePerson> trainingResourcePerson)
		{
			_context.trainingResourcePersons.AddRange(trainingResourcePerson);
			await _context.SaveChangesAsync();
			return trainingResourcePerson[0].Id;
		}
        public async Task<int> UpdateTrainingResourcePerson(List<TrainingResourcePerson> trainingResourcePerson)
        {
            _context.trainingResourcePersons.RemoveRange(_context.trainingResourcePersons.Where(x => x.trainingInfoNewId == trainingResourcePerson[0].trainingInfoNewId).ToList());
            _context.trainingResourcePersons.AddRange(trainingResourcePerson);
            await _context.SaveChangesAsync();
            return trainingResourcePerson[0].Id;
        }


        public async Task<int> SaveCoordinator(List<CourseCoordinator> courseCoordinator)
		{
			_context.courseCoordinators.AddRange(courseCoordinator);
			await _context.SaveChangesAsync();
			return courseCoordinator[0].Id;
		}



		public async Task<int> UpdateCoordinator(List<CourseCoordinator> courseCoordinator)
		{
			_context.courseCoordinators.RemoveRange(_context.courseCoordinators.Where(x => x.trainingInfoNewId == courseCoordinator[0].trainingInfoNewId).ToList());
			_context.courseCoordinators.AddRange(courseCoordinator);
			await _context.SaveChangesAsync();
			return courseCoordinator[0].Id;
		}



		public async Task<int> SaveTrainingFeedBack(TrainingFeedback trainingFeedback)
		{
			if (trainingFeedback.Id != 0)
				_context.trainingFeedbacks.Update(trainingFeedback);
			else
				_context.trainingFeedbacks.Add(trainingFeedback);
			await _context.SaveChangesAsync();
			return trainingFeedback.Id;
		}

		public async Task<IEnumerable<TrainingFeedback>> GetTrainingFeedback()
		{
			return await _context.trainingFeedbacks.Include(x => x.training).AsNoTracking().ToListAsync();
		}

		//public async Task<string> CheckTrainingOrTrainer(string empCode)
		//{
		//	var isTrainee = await (from e in _context.employeeInfos
		//					  join p in _context.enrolledTrainees
		//					  on e.Id equals p.employeeId
		//					  select e).FirstOrDefaultAsync();
		//	var isTrainer = await (from e in _context.employeeInfos
		//					  join r in _context.resourcePersons
		//					  on e.Id equals r.
		//					  select e).FirstOrDefaultAsync();
		//}

		//For DashBoard
		public async Task<IEnumerable<TrainingInfoNew>> AllTrainingOfLastYearByOrg(string org)
		{
			DateTime to = DateTime.Now;
			DateTime frm = to.AddYears(-1);
			return await _context.trainingInfoNews.Where(x => x.orgType == org && x.startDate >= frm && x.startDate <= to).AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<TrainingPerticipants>> GetTrainingPerticipantsByEmpId(int empId)
		{
			return await _context.trainingPerticipants.Include(x => x.training).Where(x => x.traineeId == empId).AsNoTracking().ToListAsync();
		}

		//public async Task<IEnumerable<EmployeeAward>> GetAwardsByEmpId(int empId)
		//{
		//    return await _context.employeeAwards.Include(x => x.award).Where(x => x.employeeId == empId).AsNoTracking().ToListAsync();
		//}

		public async Task<AspNetUsersViewModel> GetUserInfoByEmpCode(string code)
		{
			var result = (from U in _context.Users
						  join E in _context.employeeInfos on U.Id equals E.ApplicationUserId
						  where E.employeeCode == code
						  select new AspNetUsersViewModel
						  {
							  aspnetId = U.Id,
							  UserName = U.UserName,
							  Email = U.Email,
							  EmpName = E.nameEnglish
						  }).FirstOrDefaultAsync();
			return await result;
		}


		public async Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoNewForManpower(string year)
		{

			var data = await _context.trainingInfoNews.Include(x => x.country).Include(x => x.trainingInstitute).Include(x => x.subject).Include(x => x.trainer).AsNoTracking().ToListAsync();
			var data1 = await (from t in _context.enrolledTrainees
							   join e in _context.employeeInfos on t.employeeId equals e.Id
							   join d in data on t.trainingInfoNewId equals d.Id
							   where d.year == year
							   select d).ToListAsync();

			return data1;
		}

		public async Task<IEnumerable<BangladeshBankManpowerReportVM>> GetTrainingInfoForBangladeshBankManPower(int year, int month)
		{
			return await _context.bangladeshBankManpowerReportVMs.FromSql($"sp_GetBangladeshBankManpowerTrainingInfo @p0, @p1", year, month).ToListAsync();
		}
		#region TrainingHonorarium Date:08/12/2021
		//public async Task<IEnumerable<TrainingInfoNew>> GetTrainingInfoNew()
		//{
		//	return await _context.trainingInfoNews.Include(x => x.employeeType).AsNoTracking().ToListAsync();
		//}
		public async Task<IEnumerable<TrainingHonorariumViewModel>> GetTrainingHonorarium()
		{

			var r = (from h in _context.trainingHonorariums
					 join t in _context.trainingInfoNews on h.trainingId equals t.Id
					 join d in _context.resourcePersons on h.trainerId equals d.Id
					 where h.CoOrdinatorId == null

					 select new TrainingHonorariumViewModel
					 {
						 Id = h.Id,
						 trainngName = t.course,
						 trainingId = t.Id,
						 trainerId = h.trainerId,
						 receivedMoney = h.receivedMoney,
						 taxPercentage = h.taxPercentage,
						 taxAmount = h.taxAmount,
						 session = h.session,
						 effectiveDate = h.effectiveDate,
						 status = h.status,
						 isPaid = h.isPaid,
						 trainerName = d.name,
						 CoOrdinatorId = h.CoOrdinatorId,
						 coOrdinatorPayment = h.coOrdinatorPayment
					 }).OrderByDescending(i => i.Id);//.ToListAsync();

			var y = (from h in _context.trainingHonorariums
					 join t in _context.trainingInfoNews on h.trainingId equals t.Id
					 join d in _context.employeeInfos on h.CoOrdinatorId equals d.Id
					 where h.CoOrdinatorId > 0
					 select new TrainingHonorariumViewModel
					 {
						 Id = h.Id,
						 trainngName = t.course,
						 trainingId = t.Id,
						 trainerId = h.trainerId,
						 receivedMoney = h.receivedMoney,
						 taxPercentage = h.taxPercentage,
						 taxAmount = h.taxAmount,
						 session = h.session,
						 effectiveDate = h.effectiveDate,
						 status = h.status,
						 isPaid = h.isPaid,
						 trainerName = d.nameEnglish,
						 CoOrdinatorId = h.CoOrdinatorId,
						 coOrdinatorPayment = h.coOrdinatorPayment
					 }).OrderByDescending(i => i.Id);//.ToListAsync();

			var results = r.ToList().Union(y.ToList()).OrderByDescending(z => z.Id);


			return results;

		}
		public async Task<int> SaveUpdateTrainingHonorarium(TrainingHonorarium trainingHonorarium)
		{
			if (trainingHonorarium.Id != 0)
				_context.trainingHonorariums.Update(trainingHonorarium);
			else
				_context.trainingHonorariums.Add(trainingHonorarium);
			await _context.SaveChangesAsync();
			return trainingHonorarium.Id;
		}
		public async Task<bool> DeleteTrainingHonorariumById(int id)
		{
			_context.trainingHonorariums.Remove(_context.trainingHonorariums.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<IEnumerable<TrainingHonorariumViewModel>> MoneyReceptReportPdf(int id)
		{
			var emp = _context.trainingHonorariums.Where(x => x.Id == id).ToList();
			if (emp[0].CoOrdinatorId != null)
			{
				var r = (from h in _context.trainingHonorariums
						 join t in _context.trainingInfoNews on h.trainingId equals t.Id
						 join d in _context.employeeInfos on h.CoOrdinatorId equals d.Id
						 join de in _context.designations on d.lastDesignationId equals de.Id
						 where h.Id == id

						 select new TrainingHonorariumViewModel
						 {
							 trainngName = t.course,
							 effectiveDate = h.effectiveDate,
							 session = h.session,
							 taxAmount = h.taxAmount,
							 taxPercentage = h.taxPercentage,
							 receivedMoney = h.receivedMoney,
							 trainerName = d.nameEnglish,
							 designation = de.designationName
						 }).OrderByDescending(i => i.Id).ToListAsync();
				return await r;
			}
			else
			{
				var r2 = (from h in _context.trainingHonorariums
						  join t in _context.trainingInfoNews on h.trainingId equals t.Id
						  join d in _context.resourcePersons on h.trainerId equals d.Id
						  where h.Id == id

						  select new TrainingHonorariumViewModel
						  {
							  trainngName = t.course,
							  effectiveDate = h.effectiveDate,
							  session = h.session,
							  taxAmount = h.taxAmount,
							  taxPercentage = h.taxPercentage,
							  receivedMoney = h.receivedMoney,
							  trainerName = d.name,
							  designation = d.designation
						  }).OrderByDescending(i => i.Id).ToListAsync();
				return await r2;
			}
		}

		public async Task<IEnumerable<EmployeeInfo>> GetemployeeInfos()
		{
			var s = (from h in _context.employeeInfos
					 select new EmployeeInfo
					 {
						 Id = h.Id,
						 employeeCode = h.employeeCode,
						 nameBangla = h.nameBangla,
						 nameEnglish = h.nameEnglish,
						 departmentId = h.departmentId
					 }
					).OrderBy(x => x.employeeCode).ToListAsync();
			return await s;

		}
		#endregion
		#region SourceOfFund Date:11/12/2021
		public async Task<int> SaveUpdateSourceOfFund(SourceOfFund sourceOfFund)
		{
			if (sourceOfFund.Id != 0)
				_context.sourceOfFunds.Update(sourceOfFund);
			else
				_context.sourceOfFunds.Add(sourceOfFund);
			await _context.SaveChangesAsync();
			return sourceOfFund.Id;
		}
		public async Task<bool> DeletesourceOfFundById(int id)
		{
			_context.sourceOfFunds.Remove(_context.sourceOfFunds.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<IEnumerable<SourceOfFund>> GetSourceOfFund()
		{
			return await _context.sourceOfFunds.Where(x => (x.isDelete == null || x.isDelete == 0)).ToListAsync();
		}
		#endregion
		#region CourseType Date:11/12/2021
		public async Task<int> SaveUpdateCourseType(CourseType courseType)
		{
			if (courseType.Id != 0)
				_context.courseTypes.Update(courseType);
			else
				_context.courseTypes.Add(courseType);
			await _context.SaveChangesAsync();
			return courseType.Id;
		}
		public async Task<bool> DeletecourseTypeById(int id)
		{
			_context.courseTypes.Remove(_context.courseTypes.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<IEnumerable<CourseType>> GetCourseType()
		{
			return await _context.courseTypes.Where(x => (x.isDelete == null || x.isDelete == 0)).ToListAsync();
		}
		#endregion
		public async Task<IEnumerable<ManpowerSummary>> GetManpowerSummaryList()
		{
			return await _context.manpowerSummaryVm.FromSql($"sp_getEmployeeSummaryReport").ToListAsync();
		}
		public async Task<IEnumerable<ScheduleResourcePersonVm>> GetScheduleResourcePerson(int trainingId)
		{
			return await _context.scheduleResourcePersonsVMs.FromSql($"sp_GetScheduleResourcePerson @p0", trainingId).ToListAsync();
		}
		public async Task<string> GetEmployeePostingPlace(int? EmpId)
		{
			var z = await _context.employeePostings.Where(x => x.employeeId == EmpId && x.type == 1 && x.status == 1).Select(y => y.placeName).FirstOrDefaultAsync();
			return z;
		}
		public async Task<int> SaveIsCompleteTraining(TrainingInfoNew data)
		{
			var record6 = new TrainingInfoNew { Id = data.Id, status = 255 };
			_context.Attach(record6);
			_context.Entry(record6).Property(r => r.status).IsModified = true;
			await _context.SaveChangesAsync();
			return data.Id;
		}

		public async Task<IEnumerable<TrainingScheduleTrainersVm>> GetTrainerListByTrainingId(int? trainingId)
		{
			var data = await _context.trainingScheduleTrainersVms.FromSql($"sp_getTrainerListTrainingId @p0", trainingId).ToListAsync();
			return data;
		}


	}
}
