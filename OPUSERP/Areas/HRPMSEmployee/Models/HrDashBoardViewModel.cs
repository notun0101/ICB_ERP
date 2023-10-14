using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class HrDashBoardViewModel
    {
        public int? activeEmployee { get; set; }
        public int? todayPresent { get; set; }
        public int? todayAbsent { get; set; }
        public int? leaveToday { get; set; }
        public decimal? totalMalePercent { get; set; }
        public decimal? totalFemalePercent { get; set; }
        public int? totalBranches { get; set; }
        public int? totalBranchEmployees { get; set; }
        public int? totalDegs { get; set; }
        public int? totalDegEmployees { get; set; }
        public int? totalDep { get; set; }
        public int? totalDiv { get; set; }
        public int? totalZone { get; set; }
        public int? totalOffice { get; set; }
        public int? totalHrUnit { get; set; }
        public int? totalSubsidery { get; set; }
        public int? totalDepEmployees { get; set; }
        public int? totalDivEmployees { get; set; }
        public int? totalZoneEmployees { get; set; }
        public int? totalOfficeEmployees { get; set; }
        public int? totalSubsideryEmployees { get; set; }
        public int? totalHrUnitEmployees { get; set; }
        public IEnumerable<EmployeeInfoViewModel> todayLeave { get; set; }
        public IEnumerable<EmployeeInfoViewModel> presentEmployees { get; set; }
        public IEnumerable<EmployeeInfoViewModel> absentEmployees { get; set; }
        public IEnumerable<BranchWithEmployeeCount> branchEmps { get; set; }
        public IEnumerable<DesignationWithEmployeeCount> degEmps { get; set; }
        public IEnumerable<DepartmentWithEmployeeCount> depEmps { get; set; }
        public IEnumerable<DivisionWithEmployeeCount> divEmps { get; set; }
        public IEnumerable<ZonesWithEmployeeCount> zoneEmps { get; set; }
        public IEnumerable<OfficeWithEmployeeCount> officeEmps { get; set; }
        public IEnumerable<HrUnitWithEmployeeCount> hrUnitEmps { get; set; }
        public IEnumerable<HrBranch> hrBranches { get; set; }
        public IEnumerable<Designation> designations { get; set; }
        public IEnumerable<Department> departments { get; set; }
        public IEnumerable<EmployeeInfo> branchEmployee { get; set; }
        public IEnumerable<EmployeeInfo> DesignationEmployee { get; set; }
        public IEnumerable<EmployeeInfo> DepertmentEmployee { get; set; }
        public IEnumerable<AttendanceDataForChart> attendanceDataForCharts { get; set; }
    }

    public class AttendanceDataViewModel
    {
        public IEnumerable<string> workDays { get; set; }
        public IEnumerable<int?> presents { get; set; }
        public IEnumerable<int?> absents { get; set; }
    }
    public class AttendanceDataForChart
    {
        public DateTime? workDate { get; set; }
        public string workD { get; set; }
        public int? Present { get; set; }
        public int? Absent { get; set; }
    }
    public class JoningInfoViewModel
    {
        public string yearRange { get; set; }
        public int? empCount { get; set; }
    }
    public class BranchWithEmployeeCount
    {
        public int? branchId { get; set; }
        public string branchName { get; set; }
        public int? empCount { get; set; }
    }

    public class DesignationWithEmployeeCount
    {
        public int? designationId { get; set; }
        public string designationName { get; set; } 
        public string designationCode { get; set; } 
        public int? empCount { get; set; }
    }
    public class DepartmentWithEmployeeCount
    {
        public int? departmentId { get; set; }
        public string departmentName { get; set; }
        public int? empCount { get; set; }
    }
    public class DivisionWithEmployeeCount
	{
        public int? divisionId { get; set; }
        public string divisionName { get; set; }
        public int? empCount { get; set; }
    }
    public class ZonesWithEmployeeCount
	{
        public int? zoneId { get; set; }
        public string zoneName { get; set; }
        public int? empCount { get; set; }
    }
    public class OfficeWithEmployeeCount
	{
        public int? officeId { get; set; }
        public string officeName { get; set; }
        public int? empCount { get; set; }
    }
    public class HrUnitWithEmployeeCount
	{
        public int? hrUnitId { get; set; }
        public string hrUnitName { get; set; }
        public int? empCount { get; set; }
    }
}
