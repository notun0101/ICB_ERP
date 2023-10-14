using OPUSERP.Data.Entity;
using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Attendance;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSAttendence.Models
{
	public class DailyAttendenceViewModel
	{
        public IEnumerable<Company> companies { get; set; }
        public IEnumerable<AllEmployeeWithAttendence> DailyAttendence { get; set; }

		public EmployeeInfo employeeInfo { get; set; }
		public IEnumerable<Department> departments { get; set; }
		public IEnumerable<ShiftGroupMaster> shifts { get; set; }
		public IEnumerable<EmployeeInfo> employeeInfosd { get; set; }
		public IEnumerable<EmployeeInfo> employeeInfoDepts { get; set; }
		public IEnumerable<Designation> designations { get; set; }
		public IEnumerable<Location> zone { get; set; }
		public IEnumerable<FunctionInfo> Office { get; set; }
		public string departmenta { get; set; }
		public string designationa { get; set; }
		public string shifta { get; set; }
		public int totalPresent { get; set; }
		public int totalAbsent { get; set; }
		public int totalLate { get; set; }
		public int totalLeave { get; set; }
		public int totalLeaveWP { get; set; }
        public string departmentName { get; set; }
        public string designationName { get; set; }
        public string shiftName { get; set; }
        public string zoneName { get; set; }
        public string officeName { get; set; }
        public string branchName { get; set; }
        public string divisionName { get; set; }
        public string locationName { get; set; }
        //public Department department { get; set; }
        //public EmployeeInfo employee { get; set; }
        //public ShiftGroupMaster shiftGroupMaster { get; set; }
        public IEnumerable<IndAttendenceByMonth> indAttendenceByMonths { get; set; }
        public IEnumerable<HrBranch> hrBranches { get; set; }
        public IEnumerable<HrDivision> hrDivisions { get; set; }
        public IEnumerable<OverTimeReportViewModel> overtimereportviewmodels { get; set; }
       

        //public IEnumerable<MonthlyAttendenceByMonthYear> monthlyAttendenceByMonthYears { get; set; }
    }




    public class OverTimeReportViewModel
    {
        public string empCode { get; set; }
        public string empName { get; set; }
        public string workDate { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public int? departmentId { get; set; }
        public int? hrBranchId { get; set; }

        public int? hrDivisionId { get; set; }
        public int? locationId { get; set; }
        public int? functionInfoId { get; set; }
        public string BOT { get; set; }
        public string AOT { get; set; }
        public string TOT { get; set; }
    }

    public class MonthWiseAttendance
    {
        public IEnumerable<MonthWiseViewModel> monthWiseViewAttendance { get; set; }
        public IEnumerable<AllEmployeeAttendanceViewModel> allEmployeeAttendanceVM { get; set; }
    }
	public class AllEmployeeWithAttendence
	{
		public EmployeeInfo employeeInfo { get; set; }
		public EmpAttendance empAttendance { get; set; }
        public int isLeaved { get; set; } = 0;
        public IEnumerable<Department> department { get; set; }
        public IEnumerable<Designation> designations { get; set; }

        public IEnumerable<ShiftGroupMaster> shift { get; set; }
		public IEnumerable<EmployeeInfo> employeeInfos { get; set; }
		public IEnumerable<EmployeeInfo> employeeInfoDept { get; set; }
		public string departments { get; set; }
		public string designation { get; set; }
		public string shifts { get; set; }
        public IEnumerable<Location> zone { get; set; }
        public IEnumerable<HrUnit> hrUnits { get; set; }
        public IEnumerable<HrDivision> hrDivitions { get; set; }
        public IEnumerable<FunctionInfo> Office { get; set; }
        public IEnumerable<HrBranch> hrBranches { get; set; }
        public List<MonthlyEmployeeAttendence> monthlyEmployeeAttendences { get; set; }
	}
	public class MonthlyAttendance
	{
		public string departmentName { get; set; }
		public string designationName { get; set; }
		public string shiftName { get; set; }
		public IEnumerable<MonthlyEmployeeAttendence> monthlyEmployeeAttendences { get; set; }
		
	}
    public class MonlyAttendanceReportVm
    {
        public IEnumerable<Company> companies { get; set; }
        public IEnumerable<MonthlyAttendanceVm> attendance { get; set; }
    }
	public class MonthlyAttendanceVm
	{
		public string employeeCode { get; set; }
		public string empName { get; set; }
		public string branchName { get; set; }
		public int? designationCode { get; set; }
		public int? seniorityLevel { get; set; }
		public string designationName { get; set; }

		public string D1 { get; set; }
		public string D2 { get; set; }
		public string D3 { get; set; }
		public string D4 { get; set; }
		public string D5 { get; set; }
		public string D6 { get; set; }
		public string D7 { get; set; }
		public string D8 { get; set; }
		public string D9 { get; set; }
		public string D10 { get; set; }
		public string D11 { get; set; }
		public string D12 { get; set; }
		public string D13 { get; set; }
		public string D14 { get; set; }
		public string D15 { get; set; }
		public string D16 { get; set; }
		public string D17 { get; set; }
		public string D18 { get; set; }
		public string D19 { get; set; }
		public string D20 { get; set; }
		public string D21 { get; set; }
		public string D22 { get; set; }
		public string D23 { get; set; }
		public string D24 { get; set; }
		public string D25 { get; set; }
		public string D26 { get; set; }
		public string D27 { get; set; }
		public string D28 { get; set; }
		public string D29 { get; set; }
		public string D30 { get; set; }
		public string D31 { get; set; }
	}
	public class MonthlyEmployeeAttendence
	{
		public int totalPresent { get; set; }
		public int totalAbsent { get; set; }
		public int totalLate { get; set; }
		public int totalLeave { get; set; }
		public int totalLeaveWP { get; set; }
		public string department { get; set; }
		public string designation { get; set; }
		public string shift { get; set; }
		public EmployeeInfo employeeInfo { get; set; }
		public EmployeeInfo employeeInfoDDS { get; set; }
		public IEnumerable<EmpAttendanceWithDates> empAttendances { get; set; }
	}
	public class EmpAttendanceWithDates
	{
		public DateTime? date { get; set; }
		public EmpAttendance empAttendance { get; set; }
	}
	public class IndAttendenceByMonth
	{
		public int totalLeave { get; set; }
		public DateTime? Date { get; set; }
		public EmpAttendance empAttendance { get; set; }

		public int? isHoliday { get; set; }
		public int? isLeave { get; set; }

	}
	//   public class MonthlyAttendenceByMonthYear
	//{
	//       public EmployeeInfo employeeInfo { get; set; }
	//       public DateTime? Date { get; set; }
	//	public EmpAttendance empAttendance { get; set; }
	//}
	public class AllEmployeeLateViewModel
	{
		public int totalPresent { get; set; }
		public int totalAbsent { get; set; }
		public string departmentName { get; set; }
		public string designationName { get; set; }
		public string shiftName { get; set; }
		public IEnumerable<Department> department { get; set; }
		public string departments { get; set; }
		public string designation { get; set; }
		public string shifts { get; set; }
		public int totalActive { get; set; }
		public int totalLate { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
        public IEnumerable<ShiftGroupMaster> shift { get; set; }
		public IEnumerable<EmployeeInfo> employeeInfoDept { get; set; }
		public IEnumerable<EmployeeWithAtt> empAttendances { get; set; }
        public IEnumerable<Designation> designations { get; set; }
    }
	public class EmployeeWithAtt
	{
		public EmployeeInfo employeeInfo { get; set; }
		public int totalWorks { get; set; }
		public int totalLate { get; set; }
        public IEnumerable<MonthlyEmployeeAttendence> monthlyEmployeeAttendences { get; set; }
    }


	public class MyLateAttendanceViewModel
	{
		public ApplicationUser applicationUser { get; set; }
		public IndLateAttendanceViewModel indLateAttendance { get; set; }
		public MyLateAttendanceViewModel myLateAttendance { get; set; }
	}
	public class IndLateAttendanceViewModel{
		public EmployeeInfo employeeInfo { get; set; }
		public EmpAttendance empAttendance { get; set; }
	}

	public class AllEmployeeLate
	{
		public ApplicationUser applicationUser { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
		public IEnumerable<Department> department { get; set; }
		public IEnumerable<ShiftGroupMaster> shift { get; set; }
		public IEnumerable<EmployeeInfo> employeeInfoDept { get; set; }
        public string departmentName { get; set; }
        public string designationName { get; set; }
        public string shiftName { get; set; }
        public IEnumerable<Designation> designations { get; set; }


        public int allActive { get; set; }
		public int allAbsent { get; set; }
		public IEnumerable<EmployeeLateAttendance> employeeLates { get; set; }
        public ApplicationUser applicationUsers { get; set; }

        public IndAbsent indAbsent { get; set; }
	}
	public class EmployeeLateAttendance
	{
		public EmployeeInfo employee { get; set; }
		public int totalPresent { get; set; }
		public int totalAbsent { get; set; }
        public int totalWork { get; set; }

    }

	public class IndAbsent
	{
		public int totalWorking { get; set; }
		public int totalAbsent { get; set; }
		public int tLeaves { get; set; }
		public EmployeeInfo employeeInfo { get; set; }
		public IEnumerable<EmpAttendance> empAttendances { get; set; }
		public IEnumerable<EmpAbsentDates> empAbsentDates { get; set; }
	}
	public class EmpAbsentDates {
		public DateTime? date { get; set; }
		public int isPresent { get; set; }
	}



    public class AllEmployeeAttendanceViewModel
    {
        public int Attend { get; set; }
        public string average { get; set; }
        public string desigName { get; set; }
        public string Ratings { get; set; }
        public int holidayCount { get; set; }
        public int actualDays { get; set; }
        public int workingDays { get; set; }
        public string empCode { get; set; }
        public string empName { get; set; }
        public int totalAbsent { get; set; }
    }
    public class LunchSubsidyReportViewModel
    {
        
        public string HeadCode { get; set; }
        public string HeadName { get; set; }
        public string HeadNameBn { get; set; }
        public string phoneOffice { get; set; }
        
        public string designationName { get; set; }
        public string designationNameBN { get; set; }
    }

}
