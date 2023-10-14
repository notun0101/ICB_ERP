using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Areas.Auth.Models;
using OPUSERP.Areas.HRPMSACR.Models;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.Payroll.Models;
using OPUSERP.Data.Entity;
using OPUSERP.Data.Entity.Auth;
using OPUSERP.Data.Entity.Matrix;
using OPUSERP.HRPMS.Data.Entity.ACR;
using OPUSERP.HRPMS.Data.Entity.Attendance;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.IELTS;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Payroll.Data.Entity.Fixation;
using OPUSERP.Payroll.Data.Entity.Salary;
using OPUSERP.ProvidentFund.Data.Entity;

namespace OPUSERP.HRPMS.Services.Employee.Interfaces
{
    public interface IPersonalInfoService
    {
        
        Task<int> GetTotalDegEmpCount();
        Task<int> GetTotalDepEmpCount();
        Task<int> GetEmpCountByDegId(int id);
        Task<int> GetEmpCountByDepId(int id);
		Task<int> DeleteEducationById(int id);
		Task<IEnumerable<ApplicationUser>> GetOnlineUsers();
		Task<IEnumerable<EmployeeInfo>> GetEmployeeInfos();
		Task<string> GetOfficeNameById(int id);
        Task<string> GetHrUnitNameById(int id);
        Task<string> GetZoneNameById(int id);
		Task<string> GetDepartmentNameById(int id);
		int CheckHolidayByDate(DateTime? dated);
        Task<FixationReportViewModel> GetFixationByFixId(int fixId);
        Task<int> GetTotalSubsideryEmpCount();
        Task<IEnumerable<FunctionInfo>> GetAllOffices1();
        Task<UserIsHRAdminVm> CheckUserIsHRAdmin(string username);
        Task<EmployeeAcrViewModel> GetEmployeeUserInfoByCodeBySp(string empCode);
		Task<IEnumerable<EmployeeInfo>> GetAllSalaryActiveEmployees();
		Task<IEnumerable<EmployeeInfo>> GetSalaryPaidEmployeeByEmployeeId(int periodId, int? employeeId);
		Task<IEnumerable<EmployeeInfo>> GetSalaryPaidEmployeeByDegignationId(int periodId, int?designationId);
		Task<IEnumerable<EmployeeInfo>> GetSalaryPaidEmployeeByBranchId(int periodId, int? branchId);
		Task<IEnumerable<EmployeeInfo>> GetAllSalaryActiveEmployeesByPeriodId(int periodId);
		Task<IEnumerable<FixationIntegration>> GetAllFixationEmployee(string type, int year);
		Task<IEnumerable<EmployeeInfo>> GetAllSalaryActiveEmployeesByPeriodAndDesig(int periodId, int desig);
		Task<EmployeeInfoSpLoanViewModel> GetEmployeeInfoByCodeSp(string empCode);
        Task<IEnumerable<BranchWithEmployeeCount>> GetAllBranchesWithEmployeesSp();
        Task<IEnumerable<DesignationWithEmployeeCount>> GetAllDesignationWithEmployeesSp();
        Task<IEnumerable<DepartmentWithEmployeeCount>> GetAllDeptWithEmployeesSp();
        Task<IEnumerable<DivisionWithEmployeeCount>> GetAllDivisionWithEmployeesSp();
        Task<IEnumerable<ZonesWithEmployeeCount>> GetAllZoneWithEmployeesSp();
        Task<IEnumerable<OfficeWithEmployeeCount>> GetAllOfficeWithEmployeesSp();
        Task<IEnumerable<HrUnitWithEmployeeCount>> GetAllUnitWithEmployeesSp();
        Task<IEnumerable<DateTime>> GetAllHolidaysByMonthYear(int month, int year);
        Task<IEnumerable<EmployeeInfo>> GetAllEmployessForLoan();
		Task<EmployeeInfoLoanVm> GetEmployeeInfoByCodeForLoan(string code);
		Task<EmployeeInfoLoanVmNew> GetEmployeeInfoByCodeForLoanNew(string code);
		Task<IEnumerable<DateTime>> GetAllHolidaysByDateRange(DateTime? startDate, DateTime? endDate);
		Task<EmployeeInfoByUsernameSp> EmployeeInfoByUsernameSp(string username);
		Task<IEnumerable<HrBranch>> GetAllBranchWithoutHeadOffice();
		Task<IEnumerable<GetAllActiveEmployeesVm>> GetAllActiveEmployees();
		Task<IEnumerable<EmployeeInfoWithPostingVm>> GetActiveEmployeeInfoWithPosting();
		Task<IEnumerable<EmployeeAccounts>> GetAllEmployeeAccountsByEmpId(int empId);
		Task<PFMemberInfo> GetPfMemberByEmpId(int empId);
		int UpdateEmployeeBasic(int empId, decimal? amount);
		Task<EmployeeInfo> GetEmployeeInfoByEmpId(int id);
        Task<EmployeeInfo> GetEmployeeInfoByCode1(string code);
        Task<decimal> GetNewIntegrationAmountByEmpCode(string code);
		Task<decimal> GetNewPromotionAmountByEmpCode(string code);
        Task<ApplicationUser> GetUserInfoByUserName(string username);
        Task<ApplicationUser> GetUserInfoByUserId(int userid);
        Task<IEnumerable<AllEmployeeJson>> GetAllActiveEmployeeInfo();
        Task<IEnumerable<AllEmployeeJsonNew>> GetAllActiveDraftEmployeeInfo();
        Task<IEnumerable<JobResponsibility>> GetAllResponsibilities();
        Task<bool> UpdateEmployeeinfoById1(int id);
        Task<Department> GetDepartmentById(int id);
        Task<HrDivision> GetHrDivisionById(int id);
        Task<int> GetNewPromotionGradeIdByEmpCode(string code);
        Task<string> GetBranchNameById(int? id);
		Task<EmployeeInformationByCodeVm> GetEmployeeInformationByCode(string code);
        Task<IEnumerable<AuditTrailViewModel>> GetAuditTrailsForLastSevenDays();
		Task<IEnumerable<AuditTables>> GetAuditTables();
        Task<int> GetZoneIdByBranchId(int BranchId);
        Task<List<string>> GetRoleNaturesByUserName(string username);
        Task<IEnumerable<AuditTrailViewModel>> GetAuditTrailsByParams(string tableName, string operationType, DateTime? startDate, DateTime? endDate);

        Task<int> GetEmployeeIdByCode(string code);
		Task<int> GetEmployeeIdByUsername(string username);
		int UpdateOnlineStatus(string username, int status);
		int BackupDatabaseCopy(string dbName, string path);
		int PopulateAcrNotify(string username);

		Task<IEnumerable<AttendanceDataForChart>> GetAttendanceChart();

        Task<EmployeeInfo> GetEmployeeIdByCode2(string code);
        Task<int> SaveEmployeeInfo(EmployeeInfo employeeInfo);
        Task<int> SaveIeltsInfo(IeltsInfo ieltsInfo);
        Task<string> GetNomineeImgUrlById(int id);
        Task<bool> UpdateEmployeeinfoById(int id);
        Task<IEnumerable<EmployeeInfo>> GetEmployeeInfo();
        Task<IEnumerable<EmployeeInfo>> GetActiveAllEmployeeInfo();
        Task<EmployeeInfo> GetEmployeeInfoById(int id);
        Task<bool> DeleteEmployeeInfoById(int id);
        Task<IEnumerable<EmployeeWithDesignationVM>> GetEmployeeInfoDetailsList(int empId);
		Task<EmpManualAttendance> GetManualAttendanceByEmpCode(string empCode, DateTime? punchDate);
        Task<EmpManualAttendance> GetManualAttendanceByType(string empCode, DateTime? punchDate, int? attendanceTypeId);

        Task<int> SaveManualAttendance(EmpManualAttendance model);

		Task<EmployeeInfo> GetEmployeeInfoByCode(string code);
        Task<EmployeeUserViewModel> GetEmployeeUserInfoByCode(string code);
        Task<EmployeeInfo> GetEmployeeInfoByCodeAndOrg(string code,string orgType);
        Task<EmployeeInfo> GetFreeEmployeeByCode(string code);
        Task<string> GetEmployeeNameCodeById(int Id);
        //Task<IEnumerable<Occupation>> GetOccupation();
        Task<bool> UpdateEmployee(int Id, string authId, string org);
        Task<bool> ApproveEmployeeinfoById(int id);
        Task<IEnumerable<EmployeeInfo>> GetEmployeeInfoByOrg(string org);
        Task<string> GetAuthCodeByUserId(int empId);
        Task<IEnumerable<EmployeeInfo>> GetDuplicateEmpCode(int empId, string empCode);
        Task<int> IsThisEmpIDPresent(string employeeId);
        Task<IEnumerable<AllEmployeeJson>> GetActiveEmployeeInfoAsQueryAble(string queryString, string org);
        Task<IEnumerable<AllEmployeeJson>> GetInactiveEmployeeInfoAsQueryAble(string queryString, string org);
        Task<IEnumerable<AllEmployeeJson>> GetEmployeeInfoAsQueryAble(string queryString, string org);
        Task<IEnumerable<AllEmployeeJson>> GetEmployeeInfoAsQueryAbleSingle(string queryString, string org, string empode);
        Task<IEnumerable<AllEmployeeJson>> GetEmployeeInfoAsQueryAbleApprove(string queryString, string org, string empode);
        Task<IEnumerable<PeerSearchViewModel>> GetEmployeeInfoBySearch(string searchKey);
        //newly added
        Task<IEnumerable<AllEmployeeJson>> GetAllEmployeeInfo(int empStatus, string org);
        Task<IEnumerable<AllEmployeeJson>> GetEmployeeInfoSingle(int queryString, string org, string empode);
        Task<IEnumerable<EmployeeInfo>> GetEmployeeInfoList();
        // for DashBoard
        Task<IEnumerable<EmployeeInfo>> PRLInNextSixMonthByOrg(string org);
        Task<IEnumerable<EmployeeInfo>> LeaveInLastOneMonthByOrg(string org);
        Task<EmployeeInfo> GetEmployeeInfoByApplicationId(string userId);
        Task<IEnumerable<EmployeeInfo>> GetEmployeeInfoCROAnalyst();
        Task<IEnumerable<EmployeeInfo>> GetEmployeeInfoCROTeamLeader();
        Task<IEnumerable<EmployeeInfo>> GetEmployeeInfoCROAnalystByTeamId(int teamId);
        Task<IEnumerable<EmployeeInfo>> GetEmployeeInfoCROReview();
        Task<string> GetEmployeeIDByAuthID(string empId);
        Task<IEnumerable<EmployeeInfo>> GetEmployeeInfoCRMAnalyst();
        Task<IEnumerable<AllEmployeeJson>> GetAllEmployeeInfoJson();
        Task<IEnumerable<AllEmployeeJson>> GetAllInactiveEmployeeInfoJson();
        Task<AspNetUsersViewModel> GetUserInfoByEmpCode(string code);
        Task<IEnumerable<AspNetUsersViewModel>> GetUserAllInfoByEmpCode(string code);

        Task<IEnumerable<AllUsersOnlineStatusVm>> GetAllUsersOnlineStatus();

		Task<string> GetCompanyIdByEmpId(int empId);
        Task<EmployeeInfo> GetEmployeeInfoByApplicationId1(int userId);
        //Visual Data
        Task<string> GetEmpCodeNameVisualData();
		Task<IEnumerable<ApplicationRole>> GetRolesByUserId(string id);
		Task<IEnumerable<PageAssingInfo>> GetPagesByRoleNames(string[] roles);
		Task<AllEmployeeJson> GetEmployeeInfoJson(string username);
		Task<int> DeleteEmployeeById(int id);
		Task<IEnumerable<EmployeeInfo>> GetUserEmployeeInfo();
		Task<IEnumerable<StatusLog>> GetStatusLogsByReqMasterId(int masterid);
		Task<EmployeeInfo> GetEmployeeInfoByAspnetId(string Id);
        Task<IEnumerable<IeltsInfo>> GetIeltsInfos();
        Task<IEnumerable<IeltsInfo>> GetIeltsInfoByEmpId(int empId);
        Task<bool> DeleteIeltsById(int id);
        Task<string> GetIeltsImgUrlById(int id);
        Task<string> GetPassportImgUrlById(int id);
        Task<int> SaveFoodLiking(FoodLiking foodLiking);
        Task<FoodLiking> GetFoodLikingById(int id);
        Task<IEnumerable<EmployeeInfo>> GetEmployeeInfoAI();
        Task<bool> UpdateEmployeeinfoApproveStatusById(string username, int id);
        Task<IEnumerable<EmployeeHobby>> GetAllHobbiesByEmp(int empId);
        Task<int> CheckUserLoginCount(string username);
        Task<int> saveUserLogHistory(UserLogHistory model);
        Task<IEnumerable<ApplicationUser>> GetAllUsers();
        Task<ApplicationUser> GetUserIdByEmpId(int Id);
        Task<bool> SavePRL(EmployeeInfo employeeInfo);
		Task<IEnumerable<EmployeeInfo>> GetPRLEmployeeList();
        Task<IEnumerable<EmployeeInfo>> GetPensionEmployeeList();
        Task<string> GetTransferAttachmentUrlById(int id);
        Task<string> GetClearanceAttachmentUrlById(int id);

        Task<IEnumerable<AppreciationLetter>> GetAppreciationLetters();
        Task<int> SaveAppreciationLetter(AppreciationLetter appreciationLetter);
        Task<string> GetAppreciationImgUrlById(int id);
        Task<AppreciationLetter> GetAppreciationInformationById(int id);
        Task<bool> DeleteAppreciationLetterById(int id);

        Task<IEnumerable<ExperianceLetter>> GetExprienceLetterEmployeeList();
        Task<int> SaveExperienceLetter(ExperianceLetter model);
        Task<string> GetExperienceLetterById(int id);
        Task<bool> DeleteExperienceLetterById(int id);

        Task<IEnumerable<BondLetter>> GetBondLetters();
        Task<string> GetBondFileUrlById(int id);
        Task<int> SaveBondLetter(BondLetter bondLetter);
        Task<bool> DeleteBondLetterById(int id);
        Task<IEnumerable<BondLetter>> GetBondLetterEmployeeList();
        //Added By Asad
        Task<IEnumerable<EmployeePostingPlace>> GetEmployeePostingPlace(int? isActive);

        Task<IEnumerable<EmployeeInfo>> GetActiveAllEmployeeInfo1();
		Task<IEnumerable<EmployeeFixatonAuto>> GetActiveAllEmployeeInfoForFixation();
		Task<int> saveUser(ApplicationUser user);

        Task<IEnumerable<EmployeeDeath>> GetDeathRecordByEmpId(int empId);
        Task<int> SaveEmpDeathRecord(EmployeeDeath model);
        Task<bool> DeleteEmpDeathRecordById(int id);
        Task<IEnumerable<EmployeeDeath>> GetDeathRecordOfEmployee();
        Task<IEnumerable<HrBranch>> GetAllBranch();
        Task<IEnumerable<Designation>> GetAllDesignation();
        Task<IEnumerable<Department>> GetAllDepartment();
        Task<IEnumerable<EmployeeInfo>> GetEmployeeInfoLoan();
        Task<Department> GetAllDepartmentById(int id);
        Task<int> IsThisApplicantIdPresent(int? ApplicantId);
        Task<HrBranch> GetHrBranchById(int id);
        Task<IEnumerable<EmployeeInfo>> GetActiveEmployeeAndPfMemberInfo();
        Task<int> GetTotalPresentToday();
        Task<int> GetTotalLeaveToday();
        Task<IEnumerable<Department>> GetAllDepartments1();
        Task<IEnumerable<HrDivision>> GetAllDivisions();
        Task<IEnumerable<FunctionInfo>> GetAllOffices();
        Task<IEnumerable<Location>> GetAllZone();


        Task<IEnumerable<OPUSERP.Areas.HRPMSEmployee.Models.EmployeeInfoViewModel>> GetTodayLeavePersons();
        Task<int> GetEmpCountByBranchId(int id);
        Task<int> GetTotalBranchEmpCount();
        Task<IEnumerable<EmployeeInfoSpViewModel>> GetEmployeeInfoByHrBranchId(int branchId);
        Task<IEnumerable<EmployeeInfoSpViewModel>> GetEmployeeInfoByDesignationId(int designationId);
        Task<IEnumerable<EmployeeInfoSpViewModel>> GetEmployeeInfoByDepartmentId(int departmentId);
        Task<IEnumerable<OPUSERP.Areas.HRPMSEmployee.Models.EmployeeInfoViewModel>> GetAttendsToday();
        Task<IEnumerable<OPUSERP.Areas.HRPMSEmployee.Models.EmployeeInfoViewModel>> GetAbsentsToday();
        Task<IEnumerable<HrBranch>> GetBranch();
        Task<IEnumerable<EmployeeInfo>> GetTotalBranchEmployee();
        Task<IEnumerable<EmployeeInfo>> GetTotalDegEmp();
        Task<IEnumerable<EmployeeInfo>> GetTotalDepEmp();
        Task<EmployeeSignature> GetSignatureByEmpId(int id);
        Task<EmployeeSignature> GetLeaveApproverSignatureByEmpId(int id);

        Task<int> UpdatePreviousPostingDate(int empId, int postingId);
        Task<IEnumerable<PrevJobHistory>> GetAllPrevJobHistoryByEmpId(int empId);
		Task<IEnumerable<HrDivision>> GetAllHrDivision();
		Task<int> GetTotalDivEmpCount();
		Task<int> GetEmpCountByDivId(int id);
		Task<IEnumerable<HrUnit>> GetAllHrUnits();
		Task<IEnumerable<Location>> GetAllZones();
		Task<int> GetEmpCountByZoneId(int id);
		Task<int> GetEmpCountByOfficeId(int id);
		Task<int> GetEmpCountByHrUnitId(int id);
		Task<int> GetTotalZoneEmpCount();
		Task<int> GetTotalOfficeEmpCount();
		Task<int> GetTotalHrUnitEmpCount();
        Task<string> GetFixAmountByEmpCode(string code);
        Task<IEnumerable<EmployeeInfoSpViewModel>> GetEmployeeInfoByZoneId(int locationId);
		Task<IEnumerable<EmployeeInfoSpViewModel>> GetEmployeeInfoByOfficeId(int officeId);
		Task<IEnumerable<EmployeeInfoSpViewModel>> GetEmployeeInfoByHrDivisionId(int divisionId);
		Task<IEnumerable<EmployeeInfoSpViewModel>> GetEmployeeInfoByHrUnitId(int hrUnitId);
		int UpdateEmployeePosting(int empId, int status, int? departmentId, int? locationId, int? functionInfo, int? hrDivisionId, int? hrUnitId, int? hrBranchId, int? postid);
        //Task<decimal> GetFixAmountByEmpCodeForBnWord(int Id);
        Task<string> GetEmpBranchByempCode(string code);
        Task<string> GetNewPromotionGradeNameByEmpCode(string code);
        Task<IEnumerable<EmployeeInfo>> GetActiveAllEmployeeInfoDepWise(int? depId, int? branchId, int? divId, int? unitId, int? officeId, int? zoneId, int empId);
        Task<EmployeeInfo> GetEmployeeInfoByCodetranfer(string code);
        Task<IEnumerable<LoanPolicyDetail>> GetDesignationsByLoanPolicyId(int? id);

        Task<IEnumerable<AllEmployeeJsonNewForPresent>> GetAllPresentEmployeeInfo();
        Task<IEnumerable<AllEmployeeJsonNew>> GetAllAbsentEmployeeInfo();

		Task<IEnumerable<SeniorityListVm>> GetSeniorityList(int desigId);
		Task<int> UpdateEmployeeNew(EmployeeInfo model);
		Task<SeniorityStatus> UpdateSeniorityNullByDesignation(int desigId);
		Task<MarkingInfoVm> GetMarkingInfosByEmpId(int employeeId);
		Task<IEnumerable<MarkingEducation>> GetMarkingEducationByEmpId(int employeeId);
		Task<IEnumerable<EmployeeAward>> GetAllAwardsByEmpId(int employeeId);
		Task<IEnumerable<EmployeeInfoWithPostingVm>> GetAllEmployeeInfoWithPosting();
        string GetPostingByEmpId(int empId);
        Task<string> GetProfilePhotoByEmpId(int empId);
        Task<IEnumerable<EmployeeInfoWithPostingVm>> GetAllEmployeeInfoWithPostingForVoucherEntry();

        Task<IEnumerable<AllEmployeeJsonForLeave>> GetTodayLeaveEmployeeInfo();
        Task<EmployeeInfoUserDash> GetEmployeeInfoForUserDashboard(int empId);
        Task<IEnumerable<GetAttendanceLogByEmpIdVm>> GetAttendanceByEmpId(int empId);
        Task<FixationReportPreviewViewModel> GetFixationByEmpId(int empId, int fixationGrade);
        Task<DepartBranchZoneHeadViewModel> GetHeadByDepartBranchZoneId(int? depId, int? branchId, int? zoneId);
		Task<DepartBranchZoneHeadViewModel> GetApproverByEmployeeId(int employeeId);
        Task<DepartBranchZoneHeadViewModel> GetSecondLevelApproverByEmployeeId(int employeeId);
		Task<string> GetDesignationNameBNBydesigId(int desigid);
		Task<DepartBranchZoneHeadViewModel> GetHeadForHeadByDepartBranchZoneId(int? depId, int? branchId, int? zoneId);


        Task<BranchZoneDepHeadInfoViewModel> GetHeadInfoByEmpOrDepartBranchZoneId(int? empId, int? depId, int? branchId, int? zoneId);
        Task<IEnumerable<EmployeeInfo>> GetActiveEmployeeInfo(string code);
        Task<IEnumerable<SP_GetEmpOverTime>> GetEmployeeInfoByDesignation();
        Task<EmployeeSignature> GetEmployeeInfoByDesignation(string designation, int branchId, int deprtId);
        Task<int> CheckLeaveApprover(int approverId);
        Task<IEnumerable<AllEmployeeJsonNew>> GetNotPfMemberEmployeeList();
    }
}
