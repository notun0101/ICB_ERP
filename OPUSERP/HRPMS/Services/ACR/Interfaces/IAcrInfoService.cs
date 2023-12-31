﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACR.Data.ViewModel.Evaluation;
using OPUSERP.Areas.HRPMSACR.Models;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.ACR;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Leave;

namespace OPUSERP.HRPMS.Services.ACR.Interfaces
{
    public interface IAcrInfoService
    {
        Task<Assessment> AssaignAcrcancel(int assId);
        Task<int> SaveACRInfo(AcrInfo acrInfo);
        Task<IEnumerable<AcrInfo>> GetAcrInfo();
		Task<EmployeeACRInfoViewModel> GetEmployeeACRInfoByAssessmentId(int id);
		Task<EmployeeInfo> GetEmployeeByEmpCode(string empcode);
		Task<AcrInfo> GetAcrInfoById(int id);
        Task<bool> DeleteAcrInfoById(int id);
		Task<string> GetAcrTypeByAssId(int id);
		Task<Assessment> GetAssessmentInfoById(int id);
		Task<int> SaveAssessment(Assessment model);
		Task<IEnumerable<AcrInfo>> GetAcrInfoByEmpId(int empId);
        Task<IEnumerable<AssessmentViewModel>> GetLastAssessmentByUserName(string username);
		Task<IEnumerable<AssessmentViewModel>> AssessmentInfoById(string username, int id);
        Task<int> GetHeadIdByAssIdAndIndicator(int id, int indicator);
        Task<IEnumerable<LeaveType>> GetAllLeaveTypes();
		Task<IEnumerable<ACRIndicator>> GetAllAcrIndicators();
		Task<IEnumerable<EmpEducationSpViewModel>> GetEmpEducations(int empId);
		Task<IEnumerable<EmpPostingsSpViewModel>> GetEmpPostings(int empId,int id);
        Task<IEnumerable<QuantitativeEvaluation>> GetQuantitativeEvaluationsByAssId(int id);
		Task<IEnumerable<GetQuantitativeEvaByAssIdVm>> GetQuantitativeEvaByAssId(string username, int id);
		Task<ACRRecommendations> GetACRRecommendationByAssId(int id);
		Task<IEnumerable<EmployeesAcrsViewModelNew2>> GetEmployeeAcrInfoForOfficersNew(int? assesmentId);
        Task<IEnumerable<NotificationVmAcr>> SendNotification(string username, string sender, string receiver, string title, string description, string type, string url);
        Task<int> SaveACRComments(ACRComments acrComments);
		Task<int> GetTotalEmployeesAcrByAssId(int id);
		Task<int> GetTotalQuantitativeEvaluationByAssId(int id);
        Task<Assessment> GetAcrEmployeeInformationByYear(string ecode, string atype, string ayear);
        Task<IEnumerable<AssessmentInfoNewViewModel>> GetAssessmentInfoForSignatoryNew(string username, string type, string year);

        Task<int> SaveClericalAcr(EmployeesAcr model);
        Task<IEnumerable<EmployeesInfoViewModel>> GetEmployeeInfoNonClerk(int assessmentId);

        Task<int> GetHeadIdByAssId(int id);
		Task<IEnumerable<AcrEvaluationName>> AcrEvaluationNameByacrForId(int id);
		Task<IEnumerable<QuantitativeEvaluationHead>> GetAllQuantitativeEvaluationHeads();
		Task<IEnumerable<EvaluationCommentsHead>> GetAllEvaluationCommentsHeads();
		Task<IEnumerable<AcrForAdminViewModel>> ACRPendingListForHRAdmin();
		Task<NotificationVmAcr> AssaignAcrApprover(int assId, string recom, string signatory, string signatory2);
		Task<AcrTotal> GetAcrTotalByAssessmentId(int? id);
        Task<MessageViewModel> ValidAcrDate(string userName, string acrYear, string acrType, string fromDate, string toDate);
        Task<IEnumerable<UserProfileViewModel>> GetUserProfileByUserName(string username, string toDate);
        Task<IEnumerable<EmployeesJobDurationViewModel>> SaveEmployeesJobDurations(string signaturePath, string imagePath, string empcode, string jbDura, string jbBranch, string jbZonal, string jbOthers,string type, string fromDate, string toDate, string year);
        Task<IEnumerable<AssessmentInfoViewModel>> GetAssessmentInfoForSignatory(string username, string type, string year);
		Task<int> GetChildrenNoByAssId(int id);
        Task<IEnumerable<AssessmentInfoViewModel>> GetAssessmentInfoForRecom(string username, string type, string year);
		Task<IEnumerable<AllAcrSp>> GetAllMyAcrs(string username);
		int EncryptACRNumber(int assId, string empCode, int number);
		int DecryptACRNumber(int assId, string empCode, int number);
		Task<EmployeeSignature> GetSignatureByEmpCode(string empCode);
        Task<LeaveDetail> GetLeaveDetailsById(int id);
        Task<int> SaveLeaveDetails(LeaveDetail model);
        Task<IEnumerable<DeleteAcrDataViewModel>> DeleteAcrDataByChangeDrawer(int id);
        Task<string> SaveUserInfo(ApplicationUser model);
        Task<IEnumerable<ACROtherTables>> GetAcrOtherByAsssId(int id);
        Task<int> DeleteAcrOtherByAssId(int id);
        Task<int> SaveACROther(ACROtherTables model);
        Task<EmployeesAcr> GetEmployeesAcrById(int id);
        Task<int> SaveEmployeesAcr(EmployeesAcr model);
        Task<ACRDetailsInfoViewModel> GetEmployeeACRDetailsInfo(int? id, int? acrTypeId);
        Task<IEnumerable<EmployeeLeaveInfoViewModel>> GetEmployeeLeaveDetailsForAcr(int? assessmentId);
        Task<EmployeeUserViewModel> GetUserInfo(string username);
        Task<AcrUserViewModel> GetAcrUserInfo(string username);
        Task<AssessmentViewModel> GetAssessmentBasicInfo(int? id);
        //Task<IEnumerable<EmployeeInfoViewModel>> GetEmployeeInfoForSignatory(int assessmentId);
        Task<IEnumerable<EmployeesInfoViewModel>> GetEmployeeInfo(int assessmentId);
		Task<IEnumerable<PrevJobHistoryVm>> GetPrevJobHistoryByAssessmentId(int assessmentId);
		//Task<IEnumerable<EmployeeEducationInfoViewModel>> GetEmployeeEducation(int assessmentId);
		Task<IEnumerable<QuantitativeEvaluationsViewModel>> GetQuantativeEvaluation(int? assessmentId);
        Task<IEnumerable<EmployeesAcrsViewModel>> GetEmployeeAcrInfo(int? assesmentId);
        Task<IEnumerable<UserProfileViewModel>> GetUserProfile(int assessmentId);
        Task<IEnumerable<EmployeesAcrsViewModel>> GetEmployeePerformanceInfo(int? assesmentId);
        Task<IEnumerable<EmployeesAcrsViewModel>> GetEmployeeAcrInfoForOfficers(int? assesmentId);
        Task<IEnumerable<EmployeesAcrsViewModel>> GetEmployeeAcrInfoForClerks(int? assesmentId);
        Task<IEnumerable<EmployeeTrainingViewModel>> GetEmployeeTrainingInfo(int? assesmentId);
        Task<IEnumerable<EmployeeEducationInfoViewModel>> GetEmployeeEducation(int? assessmentId);
        Task<IEnumerable<EmployeeTrainingViewModel>> GetEmployeeTraining(int? assessmentId);
        Task<IEnumerable<EmployeePromotionViewModel>> GetEmployeePromotion(int? assessmentId);
        Task<IEnumerable<EmployeeLeaveInfoViewModel>> GetEmployeesLeave(int? assessmentId);
        Task<IEnumerable<EmployeeAssignmentViewModel>> GetEmployeesAssignment(int? assessmentId);
        Task<IEnumerable<ACROtherTableViewModel>> GetAcrOtherTablesReport(int? assessmentId);
        Task<IEnumerable<EmployeeAssignmentViewModel>> GetEmployeeAssignment(int? assessmentId);
        Task<IEnumerable<OfficerPart5ViewModel>> GetRecommendationComments(int? assessmentId);
        Task<IEnumerable<ACRQuantitativeEvaluationViewModel>> GetQuantativeEvaluation(int? id, int? headId);
        Task<IEnumerable<PrevJobHistory>> GetOtherEmployment(int id);
        Task<DateTime?> GetJobDuration1ByAssessId(int id);
        Task<DateTime?> GetJobDuration2ByAssessId(int id);
        int UpdateACRHRMData(int assId, DateTime? receivingdate, string empCode, string reason, string action);
        Task<IEnumerable<DocumentAttachments>> GetDocumentAttachmentByAssId(int? Id);
        Task<IEnumerable<DeleteAcrDataViewModel>> UpdateAcrRecommendations(int? assesmentId, string logically,int? promotionalQualification, string moralComment, string intellectualComment, string subjectiveComment, string specialComment, string trainingComment, string otherQualification, string userName);
        Task<Assessment> GetAcrEmployeeInformation(string ecode, string atype, DateTime afromDate, DateTime atoDate, string ayear);
        Task<string> GetEmployeeCodeByAssmentId(int assessmentId);
        Task<ACREmployeeGradeViewModel> GetEmployeeGradeBySp(string empCode);
        Task<IEnumerable<EmployeesAcrDGMtoAboveViewModel>> GetEmployeeInfoForDGMtoAbove(int assessmentId);
        Task<IEnumerable<Assessment>> GetAssessmentByEmployeeCode(string employeeCode);
    }
}
