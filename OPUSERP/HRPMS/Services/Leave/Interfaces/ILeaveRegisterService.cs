using OPUSERP.Areas.HRPMSACR.Models;
using OPUSERP.Areas.HRPMSLeave.Models;
using OPUSERP.Areas.HRPMSLeave.Models.NotMapped;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Leave;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.MasterData.Interfaces
{
    public interface ILeaveRegisterService
    {
        Task<IEnumerable<ProposedListForZoneHeadViewModel>> GetAllProLeaveByDepBrZId(int? depId, int? branchId, int? zoneId);
        Task<IEnumerable<ProposedListForZoneHeadViewModel>> GetAllProposedZoneForZoneHead(int? zoneId);
        Task<int> SaveLeaveRegister(LeaveRegister leaveRegister);
        Task<bool> UpdateReasonStatus(int? id, string reason);
        Task<bool> DeleteRecordById(int id);
        Task<IEnumerable<LeaveRegister>> GetAllLeaveRegister();
        Task<LeaveRegister> GetLeaveRegisterById(int id);
        Task<bool> DeleteLeaveRegisterById(int id);
        Task<IEnumerable<LeaveRegister>> GetLeavesByType(int typeId, int empId);
        Task<bool> UpdateLeaveApproval(int Id, int Type);
        Task<int> UpdateLeaveStatus(int id, int status);
        Task<IEnumerable<LeaveRegister>> GetAllLeaveByUserId(int userid);
        Task<IEnumerable<LeaveRegister>> GetAvailedLeaveByUser(int userid);
        Task<decimal> GetLeaveBalanceByempId(int empId, int leaveType);
        Task<IEnumerable<LeaveRegister>> GetAllLeaveRegisterByEmpIdAndStatus(int empId, int status);
        Task<IEnumerable<LeaveRegister>> GetAllLeaveRegisterByEmpId(int empId);
        Task<IEnumerable<LeaveRegisterNotMapped>> GetAllLeaveRegisterByEmpIdAndDate(int empId, DateTime? from, DateTime? to);
        Task<IEnumerable<ManualRecreationReportViewModel>> GetAllManualRecreationBranchReportByDate(DateTime? from, DateTime? to);
        Task<IEnumerable<ManualRecreationReportViewModel>> GetAllManualRecreationHeadOfficeReportByDate(DateTime? from, DateTime? to);
        Task<DayLeaveNotMapped> GetAllLeaveRegisterByEmpIdAndDateWithType(int empId, DateTime? from, DateTime? to, int typeId);
        Task<IEnumerable<LeaveRegister>> GetAllLeaveRegisterByEmpIdAndDateRange(int empId, DateTime? from, DateTime? to);
        Task<IEnumerable<LeaveReportModel>> GetLeaveReport(int year, int empId);
        Task<IEnumerable<LeaveSummaryReport>> GetAllLeaveSummaryReport(string year);
        Task<IEnumerable<LeaveSupervisorRecomViewModel>> GetSupervisorRecomForLeaveByEmpId(int leaveId, int empId);
        Task<DayLeaveNotMapped> GetTotalLeaveDaysBetweenDate(DateTime? frmDate, DateTime? toDate, int leaveType, int ShiftGrpID);
        Task<IEnumerable<LeaveBalanceViewModel>> GetLeaveBalanceSummaryByEmpId(int empId);
        Task<IEnumerable<LeaveIndividualViewModel>> GetIndividualLeaveReport(int empId, string fdate, string tdate);
        Task<ApprovalDetail> CheckFinalByEmpIdAndRegId(int? empId, int? regId);
        //Task<IEnumerable<LeaveRegister>> GetAllApproveLeaveByEmpid();
        Task<IEnumerable<LeaveRegister>> GetAllLeaveApprovByEmpIdAndStatus(int status);
        Task<IEnumerable<LeaveRegister>> GetAllLeaveByUserIdAndStatus(int userid, int status);
        Task<IEnumerable<LeaveRegister>> GetAllLeaveRegisterByUser(int userid);
        Task<int> DeleteLeaveStatusByRegId(int id);
        Task<int> DeleteLeaveRouteByRegId(int id);
        Task<int> CancelApprovedLeave(int id);
        Task<string> GetEmpNameByUserName(string username);
        Task<LeaveStatusLog> GetStatusLogByAppUserName(string appusername, int id, int regId);
        Task<IEnumerable<LeaveRegister>> GetAllLeaveByEmpCode(string empId);
        Task<IEnumerable<LeaveRegister>> GetAllPendingLeaveByEmpId(int empId);
        Task<IEnumerable<LeaveRegister>> GetTodayAprovedLeave(int userid, int status);
        Task<IEnumerable<LeaveStatusLog>> GetLeaveStatusLogByRegId(int id);
        Task<Photograph> GetPhotoByEmpId(int id);
        Task<IEnumerable<LeaveRegister>> GetAllLeaveByEmpId(int id);
        Task<IEnumerable<LeaveRegister>> GetAllApproveLeaveByUserIdStatus(int UserId, int status);
        Task<IEnumerable<LeaveRegister>> GetLeaveRequesterByApproverId(int userId);
        Task<IEnumerable<LeaveRegister>> GetAllApproveLeaveByUserId(int UserId);
        Task<IEnumerable<YearlyLeaveProcess>> GetAllYearlyLeaveProcessByEmpIdYearType(int id, int yearId, int typeId);
        Task<IEnumerable<YearlyLeaveProcess>> GetAllLeaveProcess();
        Task<IEnumerable<EmployeeLeaveViewModel>> GetEmployeeLeaveByAnyKey(string employeeCode, int? leaveTypeId, int? approverId, string fromDate, string toDate);


		Task<int> SaveYearlyLeaveProcess(YearlyLeaveProcess model);
        //Task<IEnumerable<LeaveOpeningBalance>> GetOpeningBalanceByYearId(int id);
        //Task<IEnumerable<YearlyLeaveProcessViewModel>> GetOpeningBalanceByYearId(int id);
        Task<string> GetYearById(int id);
        Task<int> DeleteYearlyLeaveProcessByYearId(int yearid);
        Task<decimal> GetLeaveAvailedByTypeEmpIdAndYear(int type, int empId, int year);
        Task<IEnumerable<Year>> GetAllYear();
        Task<int> GetOpeningBalanceByYearId(int id);
        Task<decimal> GetTotalAvailedLeaveByYear(int id, int year);
        Task<decimal> GetTotalAvailedLeaveByDate(int id, DateTime? date);
        Task<decimal> GetOpeningBalanceByRegId(int id, int year);
        Task<EmployeeSignature> GetSignatureByEmpId(int id);
        Task<EmployeeInfo> GetGeneralLeaveSummarybyId(string id);
        Task<decimal> GetLeaveCountByTypeShortNameAndEmpId(string leaveType, int empId);
        Task<bool> CheckLeave(string empCode, DateTime? fromDate, DateTime? toDate);
        Task<IEnumerable<LeaveRegister>> GetAllReCreationLeave();
        Task<IEnumerable<LeaveRegister>> GetAllLeaveRegisterByEmpIdForRecreation(int empId);
        Task<IEnumerable<LeaveRegister>> GetAllReCreationLeaveByDateRange(DateTime? from, DateTime? to);
        Task<IEnumerable<LeaveRegister>> GetAllReCreationLeaveByDateAndTypeRange(DateTime? from, DateTime? to, string type, int? typeId);
        Task<IEnumerable<LeaveRegister>> GetAllLeaveRegisterByEmpIdLeaveApply(int empId);
        Task<int> GetReCreationLeaveCountByEmpId(string leaveType, int empId);
        Task<IEnumerable<LeaveRegister>> ManualRecrationListForAppList(int empId);
        Task<int> UpdateManualLeaveStatus(int id);
        Task<IEnumerable<LeaveRegister>> ProposedRecrationListForAppList(int empId);
        Task<IEnumerable<ProposedListForHeadViewModel>> GetAllProposedLeaveByDepBrZId(int? depId, int? branchId, int? zoneId);
        Task<IEnumerable<NotificationVmAcr>> SendNotification(string username, string sender, string receiver, string title, string description, string type, string url);
        Task<int> UpdateProposedLeaveStatusByHead(int id);
        Task<int> UpdateProposedLeaveStatusByHead1(int id, string reason);
        Task<IEnumerable<LeaveRegister>> ProposedRecrationListForHrAdmin();
        Task<int> RejectProposedLeaveStatusByHead(int id);
        Task<int> ApprovedProposedStatusByHRadmin(int id);
        Task<int> RejectProposedStatusByHrAdmin(int id);
        Task<decimal> GetTotalAvailedLeaveByLeaveDate(int? id, DateTime? ToLeave);
        Task<decimal> GetTotalYearAvailedLeaveByLeaveDate(int? id, DateTime? ToLeave);
        Task<bool> SaveCommetitteDetail(LeaveCommittee leaveCommittee);
        Task<IEnumerable<LeaveCommittee>> GetleaveRegisterList();
        Task<bool> DeleteLeaveCommitteeById(int id);
        Task<IEnumerable<LeaveCommittee>> GetleaveRegisterListByYear(int? Year);
        Task<IEnumerable<ManualRecreationReportViewModel>> GetAllProposedReportByDepIdForHrAdmin(int? deptId, int? hrBranchId, int? LZoneId, int? empID, int? yearId);
        Task<IEnumerable<LeaveRegister>> ApprovedProposedListForHrAdminReportByYear(int? year);
        Task<IEnumerable<LeaveRegister>> ApprovedProposedListForHrAdmin();
        Task<IEnumerable<LeaveRegister>> ApprovedProposedListForHrAdminReportByIndEmp(int? empId);
        Task<IEnumerable<ProposedListForHeadViewModel>> GetAPrrovedProposedLeaveByDepBrZIdZForHead(int? depId, int? branchId, int? zoneId);
        Task<IEnumerable<LeaveRegister>> ApprovedProposedRecrationListEmployee(int empId);
        Task<IEnumerable<ProposedListForHeadViewModel>> GetAPrrovedProposedLeaveByDepBrZIdZForHeadByYear(int? depId, int? branchId, int? zoneId, int? year);
        Task<IEnumerable<ReportProposedListViewModel>> GetAPrrovedProposedLeaveByYearReport(int? depId, int? branchId, int? zoneId, int? year);

        Task<IEnumerable<ProposedListForHeadViewModel>> GetAllProposedLForZoneHead(int? zoneId);
        Task<IEnumerable<LeaveRegister>> FinalAnumodonProposedListForHrAdmin();
        Task<int> AnumoditoRejectProposedStatusByHrAdmin(int id);
        Task<int> AnumoditoProposedStatusByHRadmin(int id);

        Task<IEnumerable<LeaveRegister>> AnumoditoProposedListForHrAdmin();
        Task<IEnumerable<LeaveRegister>> AnumoditoRejectProposedForHrAdmin();
        Task<IEnumerable<LeaveRegister>> ActualLeaveApplyFromProposedEmployee(int empId, int year);
        Task<IEnumerable<ManualRecreationReportViewModel>> GetAnumoditoLeaveForHrAdminByYear(int? year);
        Task<IEnumerable<ManualRecreationReportViewModel>> GetFinalRejectProposedLeaveForHrAdminByYear(int? year);

        #region Actual Leave

        Task<IEnumerable<LeaveRegister>> ApprovedActualListEmployee(int empId);
        Task<int> UpdateActualLeaveStatusByHead(int id);
        Task<int> ActualLeaveRejectStatusByHead(int id);
        Task<IEnumerable<ActualListForHeadViewModel>> GetAllActualLeaveByDepBrZId(int? depId, int? branchId, int? zoneId);
        Task<IEnumerable<ActualListForHeadViewModel>> GetAllActualLForZoneHead(int? zoneId);
        Task<IEnumerable<ActualListForHeadViewModel>> GetActualAprrovedLeaveByDepBrZIdZForHead(int? depId, int? branchId, int? zoneId);
        Task<IEnumerable<ActualListForHeadViewModel>> GetActualAPrrovedLeaveByDepBrZIdZForHeadByYear(int? depId, int? branchId, int? zoneId, int? year);
        Task<IEnumerable<ReportActualListViewModel>> GetActualAPrrovedLeaveByYearReport(int? depId, int? branchId, int? zoneId, int? year);

        Task<IEnumerable<LeaveRegister>> ActualRecrationListForHrAdmin();

        Task<int> ApprovedActualStatusByHRadmin(int id);
        Task<int> RejectActualStatusByHrAdmin(int id);
        Task<IEnumerable<LeaveRegister>> ApprovedActualListForHrAdmin();
        Task<IEnumerable<ActualRecreationReportHRViewModel>> GetActualReportByDepIdForHrAdmin(int? deptId, int? hrBranchId, int? LZoneId, int? empID, int? yearId);
        #endregion

        #region Leave Attendance Report
        Task<IEnumerable<LateAttandenceDataVM>> GetLateAttendanceReports(string userName);
        #endregion

        Task<IEnumerable<LeaveRegister>> GetAllLeaveRegisterByEmpIdAndStatusAPI(int empId, int status);
        Task<IEnumerable<LeaveRegister>> GetAllPendingLeaveBySupervisorUserId(int UserId);
        Task<IEnumerable<LateAttandenceDataVM>> MyEarlyLeaveAttendance(string userName);
    }
}
