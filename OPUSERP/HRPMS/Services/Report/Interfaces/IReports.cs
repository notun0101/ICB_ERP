using OPUSERP.Areas.HRPMSAttendence.Models;
using OPUSERP.Areas.HRPMSDisciplineInvestigation.Models;
using OPUSERP.Areas.HRPMSReport.Models;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Report
{
    public interface IReports
    {
        IEnumerable<IndividualAttendanceReportViewModel> GetJobCardReport(string empCode, string fromDate, string toDate);
        Task<IEnumerable<EmployeeReport>> GetEmployeeInfoAsQueryAble(string queryString, string org);
        Task<IEnumerable<EmployeeReport>> GetWagesAsQueryAble(string queryString, string org);
        Task<IEnumerable<HrReportViewModel>> GetHrDataByDesig(int? desigId, int? deptId, string bloodGroup, int? sbuId);
        Task<IEnumerable<SignatureReportViewModel>> GetSignatureLists();
        Task<IEnumerable<HrEducationReportViewModel>> GetHrDataByEducation(int? subjectId, int? universityId, int? loeId);
        Task<IEnumerable<HrTrainingReportViewModel>> GetHrDataByTrCourse(int? courseId);
        Task<IEnumerable<HrBelongingReportViewModel>> GetHrDataByBelongingItem(int? belongingId);
        Task<IEnumerable<EmpDiplomaVm>> GetAllEmployeesByDiploma(string part);
        Task<IEnumerable<HrSummaryReportViewModel>> GetHrSummaryData(string callName);
        Task<IEnumerable<EmployeeInfo>> GetReligionWiseEmployee(int id);
        Task<Religion> GetReligionById(int id);
        Task<IEnumerable<EmployeeInfo>> GetGenderWiseEmployee(string gender);
        Task<IEnumerable<EmployeeInfo>> GetEmployeeByJoiningDateRange(DateTime? from, DateTime? to);
        Task<IEnumerable<EmployeeInfo>> GetEmployeeByConfirmationDateRange(DateTime? from, DateTime? to);
        Task<IEnumerable<Award>> GetAllAwardList();
        //Task<IEnumerable<EmployeeInfo>> GetAllEmployeeByAwardId(int id);
        Task<IEnumerable<EmployeeInfo>> GetEmployeeByRetirementDateRange(DateTime? from, DateTime? to);
        Task<IEnumerable<EmployeeInfo>> GetEmployeeByResignationDateRange(DateTime? from, DateTime? to);
        // Task<IEnumerable<EmployeeInfo>> GetEmployeeBySuspensionDateRange(DateTime? fromdate, DateTime? todate);
        Task<IEnumerable<EmpSuspensionViewModel>> GetEmployeeBySuspensionDateRange(DateTime? fromdate, DateTime? todate);
        Task<IEnumerable<EmployeeInfo>> GetDivisionWiseEmployee(int id);
        Task<IEnumerable<EmployeeInfo>> GetDistrictWiseEmployee(int id);
        Task<IEnumerable<EmployeeInfo>> GetBranchWiseEmployee(int id);
        Task<IEnumerable<EmpDiplomaViewModel>> GetAllEmployeesByPart(string part);
        Task<IEnumerable<EmpDiplomaViewModel>> GetAllEmployeesByPart();
        Task<IEnumerable<EmployeeInfo>> GetAllEmployeeByAwardId1(int id);
        Task<IEnumerable<EmpDiplomaViewModel>> GetAllEmployeeByAwardId(int id);
        Task<IEnumerable<DesignationSeniorityViewModel>> GetSeniorityDesigByJoiningDate(DateTime joiningDate);
        Task<IEnumerable<EmployeeInfo>> GetSeniorityByPRLDate(DateTime prlDate);
        Task<IEnumerable<EmployeeInfo>> GetSeniorityByEqAndDate(DateTime joiningDate, string qualification);
        Task<IEnumerable<EmployeeWithYearsTransfer>> GetSeniorityBy3YAndAbove(DateTime transferDate, int years);
        Task<IEnumerable<EducationalQualification>> GetSeniorityByEqAndDate1(DateTime joiningDate, int qualification);
        Task<IEnumerable<EmployeeInfo>> GetEmployeesByPostingDate(DateTime date, int deptId);
        Task<IEnumerable<EmployeePostingPlaceViewModel>> GetPostingsByEmployeeIdAndDate(int id, DateTime date);
        Task<IEnumerable<EmployeeInfo>> GetEmployeeByBranch(DateTime joiningDate, int Id);
        Task<IEnumerable<DesignationSeniorityViewModel>> GetSeniorityDesigBylastPromotionDate(DateTime lastPromotionDate);
        Task<HrBranch> GetBranchNameById(int Id);
        Task<HrDivision> GetDivisionNameById(int divisionId);
        Task<IEnumerable<EmployeeInfo>> GetEmployeeByDivision(DateTime joiningDate, int divisionId);
        Task<IEnumerable<EmployeeInfo>> GetEmployeesDivisionByPostingDate(DateTime date, int divId);
        Task<IEnumerable<EmployeeInfo>> GetEmployeesBranchByPostingDate(DateTime date, int branchId);
        Task<string> GetDeisignationNameById(int id);
        Task<Department> GetDepNameById(int Id);
        Task<Designation> GetDegNameById(int Id);
        Task<FunctionInfo> GetOfficeNameById(int Id);
        Task<IEnumerable<EmployeeInfo>> GetEmployeesOfficeByPostingDate(DateTime date, int officeId);
        Task<IEnumerable<CustomRole>> GetAllRoles();
        Task<Designation> GetDesignationById(int Id);
        Task<EducationalQualification> GetEduCationalSubjectNameById(int subjectId);
        Task<Location> GetLocationById(int Id);
        Task<IEnumerable<EmpDiplomaViewModel>> GetAllEmployeesByBoth();
        Task<string> GetHrBranchNameById(int id);
        Task<string> GetHrBranchNameById1(int id);
        Task<string> GetDeisignationNameById1(int id);
        Task<IEnumerable<EmployeePostingPlaceViewModel>> GetPostingsByEmployeeIdAndDate1(int id, DateTime date);
        Task<IEnumerable<DesignationSeniorityViewModel>> GetSeniorityDepByDate(DateTime postingDate);
        Task<IEnumerable<EmployeeInfo>> GetEmployeesDepartmentByPostingDate(DateTime date, int DepartmentId);
        Task<string> GetDeisignationShortNameById(int id);
        Task<EmployeePostingPlace> GetPostingsById(int id, DateTime date);
        Task<IEnumerable<EmployeeInfo>> GetDegWiseEmpLisr(int Id, int depId);

        Task<int> GetOfficerCountByOfficeId(int id, DateTime date);
        Task<int> GetStaffCountByOfficeId(int id, DateTime date);
        Task<int> GetTotalOfficerAndStaffCountByOfficeId(int id, DateTime date);
        Task<int> GetOfficerCountByBranchId(int id, DateTime date);
        Task<int> GetStaffCountByBranchId(int id, DateTime date);
        Task<int> GetTotalOfficerAndStaffCountByBranchId(int id, DateTime date);
        Task<IEnumerable<HrSummaryReportViewModel>> GetHrSummaryData1(string toDate);

        Task<IEnumerable<GManViewModel>> GetHrGenderSummaryData1(string toDate);
        Task<int> GetDegWiseEmpLisrCount(int Id, int depId);
        Task<IEnumerable<EmployeeInfo>> GetOfficeWiseEmpLisr(int Id, int OfficeId);
        Task<IEnumerable<EmployeeInfo>> GetDivWiseEmpLisr(int Id, int DivId);
        Task<IEnumerable<EmployeeInfo>> GetAllEmployees();
        Task<IEnumerable<EducationManpowerViewModel>> GetHrEducationSummaryData1(string todate);
        Task<IEnumerable<EmployeeInfo>> GetEmployeesOfficeByPostingDateNew(DateTime date, int officeId);
        Task<IEnumerable<EmployeeInfo>> GetEmployeesDivisionByPostingDateNew(DateTime date, int divId);

		Task<IEnumerable<SeniorityListViewModelNew>> GetAllSeniorityList();
		Task<IEnumerable<SeniorityListViewModelNew>> GetAllByJoiningDateSeniorityList(DateTime? joiningDate);
        Task<IEnumerable<BranchManagerViewModel>> GetBranchManagerByBranchIdDate(int hrBranchId, string fromDate, string toDate);


    }
}
