using ACR.Data.ViewModel.Evaluation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using OPUSERP.Areas.HRPMSACR.Models;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Data;
using OPUSERP.Data.Entity;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.ACR;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Leave;
using OPUSERP.HRPMS.Services.ACR.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.ACR
{
	public class AcrInfoService : IAcrInfoService
	{
		private readonly ERPDbContext _context;

		public AcrInfoService(ERPDbContext context)
		{
			_context = context;
		}

		public async Task<bool> DeleteAcrInfoById(int id)
		{
			_context.acrInfos.Remove(_context.acrInfos.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<AcrInfo>> GetAcrInfo()
		{
			return await _context.acrInfos.AsNoTracking().AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<AcrInfo>> GetAcrInfoByEmpId(int empId)
		{
			return await _context.acrInfos.Where(x => x.employeeId == empId).AsNoTracking().ToListAsync();
		}

		public async Task<AcrInfo> GetAcrInfoById(int id)
		{
			return await _context.acrInfos.FindAsync();
		}

		public async Task<int> SaveACRInfo(AcrInfo acrInfo)
		{
			if (acrInfo.Id != 0)
				_context.acrInfos.Update(acrInfo);
			else
				_context.acrInfos.Add(acrInfo);

			await _context.SaveChangesAsync();
			return acrInfo.Id;
		}

		public async Task<int> SaveACRComments(ACRComments acrComments)
		{
			if (acrComments.Id != 0)
				_context.aCRComments.Update(acrComments);
			else
				_context.aCRComments.Add(acrComments);

			await _context.SaveChangesAsync();
			return acrComments.Id;
		}

		public async Task<IEnumerable<AssessmentViewModel>> GetLastAssessmentByUserName(string username)
		{
			var data = await _context.assessmentViewModels.FromSql($"SP_GetAssessmentInfoByEmpCode {username}").AsNoTracking().ToListAsync();
			return data;
		}
		public async Task<IEnumerable<EmpEducationSpViewModel>> GetEmpEducations(int empId)
		{
			var data = await _context.empEducationSpViewModels.FromSql($"sp_EducationByEmpId {empId}").AsNoTracking().ToListAsync();
			return data;
		}
		public async Task<IEnumerable<EmpPostingsSpViewModel>> GetEmpPostings(int empId, int id)
		{
			var data = await _context.empPostingsSpViewModels.FromSql($"sp_GetPostingsByEmpId {empId},{id}").AsNoTracking().ToListAsync();
			return data;
		}
		public async Task<int> GetTotalEmployeesAcrByAssId(int id)
		{
			var first = await _context.employeesAcrs.Where(x => x.assessmentId == id).SumAsync(x => x.firstValue);
			var second = await _context.employeesAcrs.Where(x => x.assessmentId == id).SumAsync(x => x.sencondValue);
			var third = await _context.employeesAcrs.Where(x => x.assessmentId == id).SumAsync(x => x.thirdValue);
			var fourth = await _context.employeesAcrs.Where(x => x.assessmentId == id).SumAsync(x => x.forthValue);
			var fifth = await _context.employeesAcrs.Where(x => x.assessmentId == id).SumAsync(x => x.fifthValue);
			return Convert.ToInt32(first + second + third + fourth + fifth);
		}
		public async Task<int> GetTotalQuantitativeEvaluationByAssId(int id)
		{
			var ass = _context.assessments.Where(x => x.Id == id).FirstOrDefault();
			var datalist = await _context.quantitativeEvaluations.Where(x => x.assessmentId == id).Select(x => x.achievement).ToListAsync();

			int dTotal = 0;
			foreach (var item in datalist)
			{
				dTotal += DecryptACRNumber(ass.Id, ass.empCode, Convert.ToInt32(item));
			}
			//var data = await _context.quantitativeEvaluations.Where(x => x.assessmentId == id).SumAsync(x => x.achievement);
			return dTotal;
		}
		public async Task<IEnumerable<QuantitativeEvaluation>> GetQuantitativeEvaluationsByAssId(int id)
        {
            var data = await _context.quantitativeEvaluations.Include(x => x.aCRIndicator).AsNoTracking().Where(x => x.assessmentId == id).ToListAsync();
            return data;
		}
		public async Task<IEnumerable<GetQuantitativeEvaByAssIdVm>> GetQuantitativeEvaByAssId(string username, int id)
		{
			var data = await _context.quantitativeEvaByAssIdVms.FromSql($"SP_GetQuantitativeEvaByAssId {id}, {username}").AsNoTracking().ToListAsync();
			return data;
		}
		public async Task<IEnumerable<AssessmentViewModel>> AssessmentInfoById(string username, int id)
        {
            var data = await _context.assessmentViewModels.FromSql($"SP_GetAssessmentInfoById {id}, {username}").AsNoTracking().ToListAsync();
            return data;
        }
        public async Task<IEnumerable<AssessmentViewModel>> GetAcrUser(string username, int id)
        {
            var data = await _context.assessmentViewModels.FromSql($"SP_GetAssessmentInfoById {id}, {username}").AsNoTracking().ToListAsync();
            return data;
        }
        public async Task<EmployeeInfo> GetEmployeeByEmpCode(string empcode)
		{
			var data = await _context.employeeInfos.Include(x => x.lastDesignation).AsNoTracking().Where(x => x.employeeCode == empcode).FirstOrDefaultAsync();
			return data;
		}
		public async Task<ACRRecommendations> GetACRRecommendationByAssId(int id)
		{
			var data = await _context.aCRRecommendations.AsNoTracking().Where(x => x.assessmentId == id && x.type == 2).FirstOrDefaultAsync();
			return data;
		}

		public async Task<int> SaveAssessment(Assessment model)
		{
			if (model.Id != 0)
			{
				_context.assessments.Update(model);
			}
			else
			{
				_context.assessments.Add(model);
			}
			await _context.SaveChangesAsync();
			return model.Id;
		}
        public async Task<string> SaveUserInfo(ApplicationUser model)
		{
			if (model.Id != null)
			{
                _context.Users.Update(model);
			}
			else
			{
				_context.Users.Add(model);
			}
			await _context.SaveChangesAsync();
			return model.Id;
		}
		public async Task<Assessment> GetAssessmentInfoById(int id)
		{
			var data = await _context.assessments.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
			return data;
		}


        public async Task<IEnumerable<Assessment>> GetAssessmentByEmployeeCode(string employeeCode)
        {
            var data = await _context.assessments.Where(x => x.empCode == employeeCode).AsNoTracking().ToListAsync();
            return data;
        }

        public async Task<AssessmentViewModel> GetAssessmentBasicInfo(int? id)
        {
            var data = await (from a in _context.assessments
                              join e in _context.employeeInfos on a.empCode equals e.employeeCode
                              join re in _context.employeeInfos on a.empCode equals re.employeeCode into recom
                              from erecom in recom.DefaultIfEmpty()
                              join sig in _context.employeeInfos on a.empCode equals sig.employeeCode into signatory
                              from esign in signatory.DefaultIfEmpty()
                                  //join userOwn in _context.Users on a.empCode equals userOwn.EmpCode into own
                                  //from ownUser in own.DefaultIfEmpty()
                                  //join userRecom in _context.Users on a.empCode equals userRecom.EmpCode into recomUser
                                  //from reUser in recomUser.DefaultIfEmpty()
                                  //join usersign in _context.Users on a.empCode equals usersign.EmpCode into sign
                                  //from signUser in sign.DefaultIfEmpty()

                              where a.Id == id
                              select new AssessmentViewModel
                              {
                                  assessmentId = a.Id,
                                  assessmentDate = a.assessmentDate != null && a.assignToRecomDate != null ? a.assignToRecomDate : a.assessmentDate != null && a.assignToRecomDate == null ? a.assessmentDate : a.assessmentDate,
                                  assessmentNo = a.assessmentNo,
                                  assessmentYear = a.assessmentYear,
                                  empCode = a.empCode,
                                  fromDate = a.fromDate,
                                  toDate = a.toDate,
                                  signatory = a.signatory,
                                  BranchName = e.BranchName,
                                  EmpId = e.Id,
                                  empName = e.nameEnglish,
                                  DesiCode = e.DesiCode,
                                  DesignationName = e.designation,
                                  BranchCode = e.branch.branchCode,
                                  recomSign = "",
                                  signatorySign = "",
                                  ownSign = "",
                                  recommendator = a.recommendator,
                                  recommendatorName = erecom.nameEnglish,
                                  recommendatorDesig = erecom.designation,
                                  recommendationDate = a.recommendationDate,
                                  signatoryName = esign.nameEnglish,
                                  signatoryDesig = esign.designation,
                                  signatoryDate = a.signatoryDate,
                                  statusInfoId = a.statusInfoId
                              }).FirstOrDefaultAsync();
            return data;
        }
        public async Task<IEnumerable<ACROtherTables>> GetAcrOtherByAsssId(int id)
		{
			var data = await _context.aCROtherTables.Where(x => x.assessmentId == id).AsNoTracking().ToListAsync();
			return data;
		}

		public async Task<int> GetChildrenNoByAssId(int id)
		{
			var child = await (from e in _context.employeeInfos
						 join c in _context.childrens on e.Id equals c.employeeId
						 join a in _context.assessments on e.employeeCode equals a.empCode
						 where a.Id == id
						 select c.Id).CountAsync();
			return child;
		}

        public async Task<EmployeeSignature> GetSignatureByEmpCode(string empCode)
        {
            var data = await _context.employeeSignatures.AsNoTracking().Where(x => x.employee.employeeCode == empCode).FirstOrDefaultAsync();
            return data;
        }
        public async Task<IEnumerable<PrevJobHistory>> GetOtherEmployment(int id)
        {
            var emp = await _context.assessments.Where(x => x.Id == id).FirstOrDefaultAsync();
            var data = await _context.prevJobHistories.Include(x=>x.employee).AsNoTracking().Where(x => x.employee.employeeCode == emp.empCode).ToListAsync();
            return data;
        }
        public async Task<ACREmployeeGradeViewModel> GetEmployeeGradeBySp(string empCode)
        {
            return await _context.aCREmployeeGradeVM.FromSql($"sp_employeeAcrSalaryGrade  {empCode}").FirstOrDefaultAsync();
        }

        public async Task<EmployeeACRInfoViewModel> GetEmployeeACRInfoByAssessmentId(int id)
		{
            try
            {
                var data = await (from e in _context.employeeInfos.Include(x => x.lastDesignation).Include(x => x.currentGrade)
                                  join a in _context.assessments
                                  on e.employeeCode equals a.empCode
                                  join b in _context.SpecialBranchUnits
                                  on e.branchId equals b.Id
                                  join d in _context.designations
                                  on e.lastDesignationId equals d.Id
                                  join p in _context.photographs
                                  on e.Id equals p.employeeId into ePhoto
                                  from Eephoto in ePhoto.DefaultIfEmpty()
                                  join edn in _context.educationalQualifications.Include(x => x.degree).Include(x => x.reldegreesubject).OrderByDescending(x => x.passingYear)
                                  on e.Id equals edn.employeeId 
                                  into empEdu
                                  from EempEdu in empEdu.DefaultIfEmpty()
                                  join slab in _context.salarySlabs
                                  on e.currentGradeId equals slab.salaryGradeId
                                  where a.Id == id
                                  select new EmployeeACRInfoViewModel
                                  {
                                      yearRange = Convert.ToDateTime(a.fromDate).ToString("dd/MM/yyyy") + "-" + Convert.ToDateTime(a.toDate).ToString("dd/MM/yyyy"),
                                      employeeInfo = e,
                                      assessment = a,
                                      lastDesignation = d.designationNameBN,
                                      //lastEducation = edn != null ? edn.degree.degreeName: " " + " " + edn != null ? edn.reldegreesubject.subject.subjectName : "",
                                      lastEducation = EempEdu != null ? EempEdu.degree.degreeName: " " + " " + EempEdu != null ? EempEdu.reldegreesubject.subject.subjectName : "",
                                      gradeSlab = slab.slabName,
                                      jobDuration = _context.employeesJobDurations.Where(x => x.empCode == e.employeeCode && x.yearName == Convert.ToDateTime(a.toDate).ToString("yyyy")).Select(x => x.JobDuration).FirstOrDefault(),
                                      jobDurationAsManager = _context.employeesJobDurations.Where(x => x.empCode == e.employeeCode && x.yearName == Convert.ToDateTime(a.toDate).ToString("yyyy")).Select(x => x.JobDurationAsBranchManger).FirstOrDefault(),
                                      jobDurationAsZonalHead = _context.employeesJobDurations.Where(x => x.empCode == e.employeeCode && x.yearName == Convert.ToDateTime(a.toDate).ToString("yyyy")).Select(x => x.JobDurationAsZonalHead).FirstOrDefault(),
                                      jobDurationAsOthers = _context.employeesJobDurations.Where(x => x.empCode == e.employeeCode && x.yearName == Convert.ToDateTime(a.toDate).ToString("yyyy")).Select(x => x.JobDurationAsOthers).FirstOrDefault(),
                                      jobUnderSupervisor = _context.AcrEmployeeInfos.Where(x => x.assessmentId == a.Id).Select(x => x.JobUnderSupervisor).FirstOrDefault(),
                                      diploma1 = _context.bankingDiplomas.Where(x => x.diplomaPart == "Part 1" && x.employeeId == e.Id).Select(x => x.diplomaPart).FirstOrDefault(),
                                      diploma2 = _context.bankingDiplomas.Where(x => x.diplomaPart == "Part 2" && x.employeeId == e.Id).Select(x => x.diplomaPart).FirstOrDefault(),
                                      profQ = _context.professionalQualifications.Where(x => x.employeeID == e.Id).Select(x => x.qualificationHead.name + ' ' + x.subject).FirstOrDefault(),
                                      othersQ = _context.otherQualifications.Where(x => x.employeeID == e.Id).Select(x => x.otherQualificationHead.name + ' ' + x.subject).FirstOrDefault(),
                                      photoUrl = Eephoto.url,
                                      joiningDesignation = _context.designations.Where(x => x.designationName == e.joiningDesignation).Select(x => x.designationNameBN).FirstOrDefault(),
                                      superVisor = _context.employeeInfos.AsNoTracking().Where(x => x.employeeCode == e.SupervisorCode).Include(x => x.lastDesignation).Select(x => new SuperVisorViewModel
                                      {
                                          supervisorCode = x.employeeCode,
                                          supervisorName = x.nameBangla,
                                          supervisorDesig = x.lastDesignation.designationNameBN
                                      }).FirstOrDefault(),
                                      postingPlaceMainDuty = string.Join(", ", _context.employeePostings.Where(x => x.employeeId == e.Id && x.type == 1 && x.status == 1).Select(x => x.placeName).ToList())
                                  }).FirstOrDefaultAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
		}
        //public async Task<IEnumerable<EmployeeEducationInfoViewModel>> GetEmployeeEducation(int? assessmentId)
        //{
        //    var data = await (from e in _context.EmployeeEducations
        //                      join a in _context.AcrProfessionalQualifications
        //                      on e.assessmentId equals a.assessmentId into aa from aaa in aa.DefaultIfEmpty()
        //                      //join b in _context.SpecialBranchUnits
        //                      //on e.branchId equals b.Id
        //                      //join d in _context.designations
        //                      //on e.lastDesignationId equals d.Id
        //                      //join p in _context.photographs
        //                      //on e.Id equals p.employeeId
        //                      where e.assessmentId == assessmentId
        //                      select new EmployeeEducationInfoViewModel
        //                      {
        //                          LavelOfEducation = e.LavelOfEducation,
        //                          LOECat = e.LOECat,
        //                          PassingYear = e.PassingYear,
        //                          MajorGroup = e.MajorGroup,
        //                          ResultClass = e.ResultClass,
        //                          UniversityName=e.UniversityName

        //                      }).ToListAsync();
        //    return data;
        //}

        //public async Task<IEnumerable<ACRQuantitativeEvaluationViewModel>> GetQuantativeEvaluationAssmentId(int? id,int? headId)
        //{
        //    var records = (from i in _context.aCRIndicators
        //                   join h in _context.quantitativeEvaluationHeads on i.headId equals h.Id
        //                   join q in _context.quantitativeEvaluations on i.Id equals q.aCRIndicatorId into que
        //                   from qe in que.DefaultIfEmpty()
        //                   join a in _context.assessments on qe.assessmentId equals a.Id into aa
        //                   from ass in aa.DefaultIfEmpty()
        //                   where ass.Id == id && h.Id == headId && h.Id != 6
        //                   select new ACRQuantitativeEvaluationViewModel
        //                   {
        //                       indicatorId = i.Id,
        //                       indicatorNameBn = i.indicatorNameBn,
        //                       headId = h.Id,
        //                       evaluationId = qe.Id,
        //                       target = qe.target == null ? 0 : qe.target,
        //                       achievement = qe.achievement == null ? 0 : qe.achievement,
        //                       achievementPercent = qe.achievementPercent == null ? 0 : qe.achievementPercent,
        //                       achievementSign = qe.achievementSign == null ? 0 : qe.achievementSign,
        //                       achievementPercentSign = qe.achievementPercentSign == null ? 0 : qe.achievementPercentSign,
        //                       acrCounter = qe.acrCounter,
        //                       assessmentId = ass.Id
        //                   }).ToList();
        //    return records;
        //}

        public async Task<IEnumerable<ACRQuantitativeEvaluationViewModel>> GetQuantativeEvaluation(int? id, int? headId)
        {
            var data = await _context.aCRQuantitativeEvaluationViewModels.FromSql($"SP_GetQuantativeEvaluation {id}").AsNoTracking().ToListAsync();
            return data;
        }

        public async Task<IEnumerable<NotificationVmAcr>> SendNotification(string username, string sender, string receiver, string title, string description, string type, string url)
        {
            var data = await _context.notificationVmAcrs.FromSql($"sp_SendNotification {username}, {sender}, {receiver}, {title}, {description}, {type}, {url}").AsNoTracking().ToListAsync();
            return data;
        }

        public async Task<IEnumerable<AssessmentInfoViewModel>> GetAssessmentInfoForSignatory(string username, string type, string year)
        {
            var data = await _context.assessmentInfoViewModels.FromSql($"SP_GetAssessmentInfoForSignatory {username}, {type}, {year}").AsNoTracking().ToListAsync();
            return data;
        }
        public async Task<IEnumerable<AssessmentInfoNewViewModel>> GetAssessmentInfoForSignatoryNew(string username, string type, string year)
        {
            var data = await _context.assessmentInfoNewViewModels.FromSql($"SP_GetAssessmentInfoForSignatory {username}, {type}, {year}").AsNoTracking().ToListAsync();
            return data;
        }



		public async Task<IEnumerable<AcrForAdminViewModel>> ACRPendingListForHRAdmin()
		{
			var data = await _context.acrForAdminViewModels.FromSql($"SP_GetPendingListForHRAdmin").AsNoTracking().ToListAsync();
			return data;
		}
		public async Task<NotificationVmAcr> AssaignAcrApprover(int assId, string recom, string signatory, string signatory2)
		{
			var data = await _context.notificationVmAcrs.FromSql($"SP_AssaignAcrApprover {assId}, {recom}, {signatory}, {signatory2}").AsNoTracking().FirstOrDefaultAsync();
			return data;
		}
        public async Task<Assessment> AssaignAcrcancel(int assId)
		{
			var data = await _context.assessments.Where(x=>x.Id == assId).AsNoTracking().FirstOrDefaultAsync();
           
			return data;
		}


		public int EncryptACRNumber(int assId, string empCode, int number)
		{
			var data = number + (assId * Convert.ToInt32(empCode) * 12);
			return data;
		}

		public int DecryptACRNumber(int assId, string empCode, int number)
		{
			var data = number - (assId * Convert.ToInt32(empCode) * 12);
			return data;
		}

		public async Task<IEnumerable<AssessmentInfoViewModel>> GetAssessmentInfoForRecom(string username, string type, string year)
        {
            var data = await _context.assessmentInfoViewModels.FromSql($"SP_GetAssessmentInfoForSignatory {username}, {type}, {year}").AsNoTracking().ToListAsync();
            return data;
        }
        public async Task<IEnumerable<AllAcrSp>> GetAllMyAcrs(string username)
        {
            var data = await _context.allAcrSps.FromSql($"sp_GetACRNotify {username}").AsNoTracking().ToListAsync();
            return data;
        }
        public async Task<IEnumerable<DeleteAcrDataViewModel>> DeleteAcrDataByChangeDrawer(int id)
        {
            var data = await _context.deleteAcrDataViews.FromSql($"SP_DeleteAcrData_ByChangeDrawer {id}").AsNoTracking().ToListAsync();
            return data;
        }

        public async Task<MessageViewModel> ValidAcrDate(string userName, string acrYear, string acrType, string fromDate, string toDate)
        {
            var reult = new MessageViewModel();

            try
            {
                reult = await _context.MessageViewModels.FromSql($"ValidAcrDate {userName},{acrYear},{acrType},{fromDate},{toDate}").AsNoTracking().SingleOrDefaultAsync();
            }
            catch(Exception exp)
            {

            }
            return reult;
        }

        public async Task<IEnumerable<UserProfileViewModel>> GetUserProfileByUserName(string username, string toDate)
        {
            var data = await _context.userProfileViewModels.FromSql($"SP_GET_UserProfile {username},{toDate}").AsNoTracking().ToListAsync();
            return data;
        }
        public async Task<IEnumerable<EmployeesJobDurationViewModel>> SaveEmployeesJobDurations(string signaturePath, string imagePath, string empcode, string jbDura, string jbBranch, string jbZonal, string jbOthers,string type, string fromDate, string toDate, string year)
        {
            var data = new List<EmployeesJobDurationViewModel>();
            try
            {
                data = await _context.employeesJobDurationsVm.FromSql($"SP_Save_EmployeesJobDurations {empcode}, {jbDura}, {jbBranch}, {jbZonal},{jbOthers},{type}, {fromDate}, {toDate},{year}").AsNoTracking().ToListAsync();
            }
            catch(Exception ex)
            {

            }
            return data;
        }
        public int UpdateACRHRMData(int assId, DateTime? receivingdate,string empCode,string reason, string action)
        {
            _context.Database.ExecuteSqlCommand($"SP_Update_ACR_HRM_Data {assId},{receivingdate}, {reason}, {action}, {empCode}");
            return 1;
        }
        public async Task<IEnumerable<DocumentAttachments>> GetDocumentAttachmentByAssId(int? Id)
        {
            return await _context.docAttachment.Where(x => x.assesmentId == Id).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<EmployeeLeaveInfoViewModel>> GetEmployeeLeaveDetailsForAcr(int? assessmentId)
        {
            var data = await _context.employeeLeaveInfoViewModels.FromSql($"SP_GetEmployeesLeave {assessmentId}").AsNoTracking().ToListAsync();
            return data;
        }
        public async Task<EmployeeUserViewModel> GetUserInfo(string username)
        {
            var data = await _context.employeeUserViewModels.FromSql($"SP_GETUserInformation {username}").AsNoTracking().FirstOrDefaultAsync();
            return data;
        }
        public async Task<AcrUserViewModel> GetAcrUserInfo(string username)
        {
            var data = await _context.acrUserViewModels.FromSql($"SP_GETUserInformation {username}").AsNoTracking().FirstOrDefaultAsync();
            return data;
        }
        public async Task<IEnumerable<UserProfileViewModel>> GetUserProfile(int assessmentId)
        {
            var data = await _context.userProfileViewModels.FromSql($"SP_GET_UserProfilebyempcode {assessmentId}").AsNoTracking().ToListAsync();
            return data;
        }
        public async Task<IEnumerable<EmployeeEducationInfoViewModel>> GetEmployeeEducation(int? assessmentId)
        {
            var data = await _context.employeeEducationViewModels.FromSql($"SP_GetEmployeeEducation {assessmentId}").AsNoTracking().ToListAsync();
            return data;
        }
        public async Task<IEnumerable<EmployeeTrainingViewModel>> GetEmployeeTraining(int? assessmentId)
        {
            var data = await _context.employeeTrainingViewModels.FromSql($"SP_GetEmployeeTraining {assessmentId}").AsNoTracking().ToListAsync();
            return data;
        }
        public async Task<IEnumerable<EmployeesInfoViewModel>> GetEmployeeInfo(int assessmentId)
        {
            try
            {
                var data = await _context.employeeInfoVMs.FromSql($"SP_GetEmployeesByCode {assessmentId}").AsNoTracking().ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }
         public async Task<IEnumerable<EmployeesAcrDGMtoAboveViewModel>> GetEmployeeInfoForDGMtoAbove(int assessmentId)
        {
            try
            {
                var data = await _context.employeesAcrDGMtoAbovesVM.FromSql($"SP_GetEmployeesByCodeDGM {assessmentId}").AsNoTracking().ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        public async Task<string> GetEmployeeCodeByAssmentId(int assessmentId)
        {
            Assessment data = await _context.assessments.Where(x => x.Id == assessmentId).FirstOrDefaultAsync();
            return data.empCode;
           
        }


        public async Task<IEnumerable<PrevJobHistoryVm>> GetPrevJobHistoryByAssessmentId(int assessmentId)
        {
            var data = await _context.prevJobHistoryVms.FromSql($"sp_GetPrevJobHistory {assessmentId}").AsNoTracking().ToListAsync();
            return data;
        }
        public async Task<IEnumerable<EmployeesInfoViewModel>> GetEmployeeInfoNonClerk(int assessmentId)
        {
            var data = await _context.employeeInfoVMs.FromSql($"SP_GetEmployeesByCodeNonClerk {assessmentId}").AsNoTracking().ToListAsync();
            return data;
        }
        public async Task<IEnumerable<QuantitativeEvaluationsViewModel>> GetQuantativeEvaluation(int? assessmentId)
        {
            var data = await _context.quantitativeEvaluationsVMs.FromSql($"SP_RPT_SUB_TargetEvaluation {assessmentId}").AsNoTracking().ToListAsync();
            return data;
        }
        public async Task<IEnumerable<OfficerPart5ViewModel>> GetRecommendationComments(int? assessmentId)
        {
            var data = await _context.officerPart5ViewModels.FromSql($"SP_GetAcrRecommendationComments {assessmentId}").AsNoTracking().ToListAsync();
            return data;
        }
        //public async Task<IEnumerable<EmployeeEducationInfoViewModel>> GetEmployeeEducation(int? assessmentId)
        //{
        //    var data = await _context.employeeEducationViewModels.FromSql($"SP_GetEmployeeEducation {assessmentId}").AsNoTracking().ToListAsync();
        //    return data;
        //}
        public async Task<IEnumerable<EmployeePromotionViewModel>> GetEmployeePromotion(int? assessmentId)
        {
            var data = await _context.employeePromotionViewModels.FromSql($"SP_GetEmployeesPromotion {assessmentId}").AsNoTracking().ToListAsync();
            return data;
        }
        public async Task<IEnumerable<EmployeeAssignmentViewModel>> GetEmployeeAssignment(int? assessmentId)
        {
            var data = await _context.employeeAssignmentViewModels.FromSql($"SP_GetEmployeesAssignment {assessmentId}").AsNoTracking().ToListAsync();
            return data;
        }
        public async Task<IEnumerable<EmployeeLeaveInfoViewModel>> GetEmployeesLeave(int? assessmentId)
        {
            var data = await _context.employeeLeaveInfoViewModels.FromSql($"SP_GetEmployeesLeave {assessmentId}").AsNoTracking().ToListAsync();
            return data;
        }
        public async Task<IEnumerable<EmployeeAssignmentViewModel>> GetEmployeesAssignment(int? assessmentId)
        {
            var data = await _context.employeeAssignmentViewModels.FromSql($"SP_GetEmployeesAssignment {assessmentId}").AsNoTracking().ToListAsync();
            return data;
        }
        public async Task<IEnumerable<ACROtherTableViewModel>> GetAcrOtherTablesReport(int? assessmentId)
        {
            //var data = await _context.aCROtherTableViewModels.FromSql($"SP_RPT_AcrOtherTablesValue {assessmentId}").AsNoTracking().ToListAsync();
            //return data;
            var result = (from C in _context.aCROtherTables
                          where C.assessmentId == assessmentId
                          select new ACROtherTableViewModel
                          {
                              isPhysicallyCapable = C.isPhysicallyCapable,
                              bankingExperience = C.bankingExperience,
                              nobankingExperience = C.nobankingExperience
                          }).ToList();
            return result;
        }
        public async Task<IEnumerable<EmployeesAcrsViewModel>> GetEmployeeAcrInfo(int? assesmentId)
        {
            try
            {
                var data = await _context.employeesAcrsViewModels.FromSql($"SP_RPT_SUB_DGM_PersonalCharacter_Html {assesmentId}").AsNoTracking().ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
      
        }
        public async Task<IEnumerable<EmployeesAcrsViewModel>> GetEmployeePerformanceInfo(int? assesmentId)
        {
            try
            {
                var data = await _context.employeesAcrsViewModels.FromSql($"SP_RPT_Executive_Performance_Html {assesmentId}").AsNoTracking().ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }
        //public async Task<IEnumerable<EvaluationOfficerPartTwoViewModel>> GetEmployeeAcrInfoForOfficers(int? assesmentId)
        //{
        //    var data = await _context.evaluationOfficerPartTwoViewModels.FromSql($"SP_Getparttwobyassignmentid {assesmentId}").AsNoTracking().ToListAsync();
        //    return data;
        //}
        public async Task<IEnumerable<EmployeesAcrsViewModel>> GetEmployeeAcrInfoForOfficers(int? assesmentId)
        {
            var data = await _context.employeesAcrsViewModels.FromSql($"SP_Getparttwobyassignmentid {assesmentId}").AsNoTracking().ToListAsync();
            return data;
        }
        public async Task<IEnumerable<EmployeesAcrsViewModelNew2>> GetEmployeeAcrInfoForOfficersNew (int? assesmentId)
        {
            try
            {
                var data = await _context.employeesAcrsViewModelNews2.FromSql($"SP_GetparttwobyassignmentidNew {assesmentId}").AsNoTracking().ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<IEnumerable<EmployeesAcrsViewModel>> GetEmployeeAcrInfoForClerks(int? assesmentId)
        {
            var data = await _context.employeesAcrsViewModels.FromSql($"SP_RPT_SUB_Clerical_Performance {assesmentId}").AsNoTracking().ToListAsync();
            return data;
        }
        public async Task<IEnumerable<EmployeeTrainingViewModel>> GetEmployeeTrainingInfo(int? assesmentId)
        {
            var data = await _context.employeeTrainingViewModels.FromSql($"SP_GetEmployeeTraining {assesmentId}").AsNoTracking().ToListAsync();
            return data;
        }
        public async Task<IEnumerable<DeleteAcrDataViewModel>> UpdateAcrRecommendations(int? assesmentId, string logically,int? promotionalQualification, string moralComment, string intellectualComment, string subjectiveComment, string specialComment, string trainingComment, string otherQualification,string userName)
        {
            var data = await _context.deleteAcrDataViews.FromSql($"SP_Update_ACRRecommendations {assesmentId},{logically},{promotionalQualification},{moralComment},{intellectualComment},{subjectiveComment},{specialComment},{trainingComment},{otherQualification},{userName}").AsNoTracking().ToListAsync();
            return data;
        }
        //public async Task<IEnumerable<AssessmentViewModel>> GetAssessmentBasicInfo(int? assId)
        //{
        //    var data = await _context.assessmentVM.FromSql($"SP_GetAssessmentInfoById {assId}").AsNoTracking().ToListAsync();
        //    return data;
        //}
        public async Task<int> GetHeadIdByAssId(int id)
		{
			var data = await _context.quantitativeEvaluations.AsNoTracking().Where(x => x.assessmentId == id).Include(x => x.aCRIndicator).Select(x => x.aCRIndicator.Id).FirstOrDefaultAsync();
			return data;
		}
        public async Task<int> GetHeadIdByAssIdAndIndicator(int id, int indicator)
		{
			var data = await _context.quantitativeEvaluations.AsNoTracking().Where(x => x.assessmentId == id && x.aCRIndicator.indicatorName != indicator.ToString()).Include(x => x.aCRIndicator).Select(x => x.aCRIndicator.Id).FirstOrDefaultAsync();
			return data;
		}
		public async Task<string> GetAcrTypeByAssId(int id)
		{
			var data = await _context.assessments.Where(x => x.Id == id).Select(x => x.acrType).FirstOrDefaultAsync();
			return data;
		}
		public async Task<IEnumerable<AcrEvaluationName>> AcrEvaluationNameByacrForId(int id)
		{
			var data = await _context.acrEvaluationNames.AsNoTracking().Where(x => x.acrForId == id).ToListAsync();
			return data;
		}
        public async Task<DateTime?> GetJobDuration1ByAssessId(int id)
		{
			var data = await _context.assessments.Where(x => x.Id == id).Select(x => x.duration1).FirstOrDefaultAsync();
			return data;
		}
         public async Task<DateTime?> GetJobDuration2ByAssessId(int id)
		{
			var data = await _context.assessments.Where(x => x.Id == id).Select(x => x.duration2).FirstOrDefaultAsync();
			return data;
		}




		public async Task<AcrTotal> GetAcrTotalByAssessmentId(int? id)
		{
			var data = await _context.acrTotals.FromSql($"sp_GetMarkAcrByAssId {id}").AsNoTracking().FirstOrDefaultAsync();
			return data;
		}

		public async Task<IEnumerable<QuantitativeEvaluationHead>> GetAllQuantitativeEvaluationHeads()
		{
			var data = await _context.quantitativeEvaluationHeads.AsNoTracking().ToListAsync();
			return data;
		}
		public async Task<IEnumerable<EvaluationCommentsHead>> GetAllEvaluationCommentsHeads()
		{
			var data = await _context.evaluationCommentsHeads.AsNoTracking().ToListAsync();
			return data;
		}
		public async Task<IEnumerable<ACRIndicator>> GetAllAcrIndicators()
		{
			var data = await _context.aCRIndicators.AsNoTracking().ToListAsync();
			return data;
		}
        public async Task<IEnumerable<LeaveType>> GetAllLeaveTypes()
		{
			var data = await _context.leaveTypes.AsNoTracking().ToListAsync();
			return data;
		}
        public async Task<LeaveDetail> GetLeaveDetailsById (int id)
		{
			var data = await _context.leaveDetails.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
			return data;
		}
        public async Task<int> SaveLeaveDetails(LeaveDetail model)
        {
            if (model.Id != 0)
            {
                _context.leaveDetails.Update(model);
            }
            else
            {
                _context.leaveDetails.Add(model);
            }
            await _context.SaveChangesAsync();
            return model.Id;
        }
        public async Task<int> SaveACROther(ACROtherTables model)
        {
            if (model.Id != 0)
            {
                _context.aCROtherTables.Update(model);
            }
            else
            {
                _context.aCROtherTables.Add(model);
            }
            await _context.SaveChangesAsync();
            return model.Id;
        }
        public async Task<int> SaveEmployeesAcr(EmployeesAcr model)
        {
            if (model.Id != 0)
            {
                _context.employeesAcrs.Update(model);
            }
            else
            {
                _context.employeesAcrs.Add(model);
            }
            await _context.SaveChangesAsync();
            return model.Id;
        }

        public async Task<int> SaveClericalAcr(EmployeesAcr model)
        {
            if (model.Id != 0)
            {
                _context.employeesAcrs.Update(model);
            }
            else
            {
                _context.employeesAcrs.Add(model);
            }
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> DeleteAcrOtherByAssId(int id)
        {
            var data = await _context.aCROtherTables.AsNoTracking().Where(x => x.assessmentId == id).ToListAsync();
            _context.aCROtherTables.RemoveRange(data);
            await _context.SaveChangesAsync();
            return 1;
        }

        public async Task<EmployeesAcr> GetEmployeesAcrById(int id)
        {
            var data = await _context.employeesAcrs.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
            return data;
        }
       
        public async Task<ACRDetailsInfoViewModel> GetEmployeeACRDetailsInfo(int? id, int? acrTypeId)
        {
            var marks = _context.employeesAcrs.Where(x => x.assessmentId == id).Sum(x => (x.firstValue == 2 ? 0 : x.firstValue) + (x.sencondValue == 2 ? 0 : x.sencondValue * 2) + (x.thirdValue == 2 ? 0 : x.thirdValue * 3) + (x.forthValue == 2 ? 0 : x.forthValue * 4) + (x.fifthValue == 2 ? 0 : x.fifthValue * 5));
            var amountInWord = AmountInWord.ConvertWholeNumber(marks.ToString());
            var result = (from e in _context.employeesAcrs
                          join a in _context.acrEvaluationNames on e.acrEvaluationNameId equals a.Id
                          where e.assessmentId == id && a.acrTypeId == acrTypeId
                          select new ACRDetailsInfoViewModel
                          {
                              acrId = e.Id,
                              empCode = e.empCode,
                              acrEvaluationNameId = e.acrEvaluationNameId,
                              firstValue = e.firstValue,
                              sencondValue = e.sencondValue,
                              thirdValue = e.thirdValue,
                              forthValue = e.forthValue,
                              fifthValue = e.fifthValue,
                              marks = marks.ToString(),
                              marksInWord = amountInWord
                          }).FirstOrDefaultAsync();
            return await result;
            //return Json(result, JsonRequestBehavior.AllowGet);
        }
     

        public async Task<Assessment> GetAcrEmployeeInformation(string ecode,string atype,DateTime afromDate,DateTime atoDate,string ayear)
        {
            Assessment data = await _context.assessments.Where(x => x.empCode == ecode && x.acrType==atype && ((x.fromDate <= afromDate && x.toDate >= atoDate) || (x.fromDate >= afromDate && x.toDate <= atoDate)) && x.assessmentYear== ayear).FirstOrDefaultAsync();
           // Assessment data = await _context.assessments.Where(x => x.empCode == ecode && x.acrType==atype && ((x.fromDate <= afromDate && x.toDate >= atoDate) || (x.fromDate >= afromDate && x.toDate <= atoDate)|| (x.fromDate >= afromDate && x.toDate >= atoDate)|| (x.fromDate <= afromDate && x.toDate <= atoDate)) && x.assessmentYear== ayear).FirstOrDefaultAsync();


           // Assessment data = await _context.assessments.Where(x => x.empCode == ecode && x.acrType==atype && (x.fromDate <= afromDate || x.toDate >= atoDate) && x.assessmentYear== ayear).FirstOrDefaultAsync();
            //Assessment data = await _context.assessments.Where(x => x.empCode == ecode && x.acrType==atype && ((x.fromDate >= afromDate && x.fromDate <= atoDate) ||(x.toDate >= atoDate && x.toDate <= atoDate)) && x.assessmentYear== ayear).FirstOrDefaultAsync();
            return data;
        }
        public async Task<Assessment> GetAcrEmployeeInformationByYear(string ecode,string atype,string ayear)
        {
            Assessment data = await _context.assessments.Where(x => x.empCode == ecode && x.acrType==atype && x.assessmentYear== ayear).FirstOrDefaultAsync();
            return data;
        }


    }
}
