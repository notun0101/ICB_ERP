using OPUSERP.Areas.HRPMSAttendence.Models.Lang;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Attendance;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Wages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace OPUSERP.Areas.HRPMSAttendence.Models
{
    public class EmployeePunchCardInfoViewModel
    {
        public int editId { get; set; }
        [Required]
        [Display(Name = "PunchCardNo")]
        public string punchCardNo { get; set; }

        public int? shiftGroupMasterId { get; set; }

        public int? employeeId { get; set; }
        
        public string employeeCode { get; set; }
        public DateTime? attendanceProcessDate { get; set; }
        public AttendanceLn fLang { get; set; }

        public IEnumerable<ShiftGroupMaster> shiftGroupMasterlist { get; set; }
        public IEnumerable<Department> departments { get; set; }
        public IEnumerable<EmployeePunchCardInfo> employeePunchCardInfoslist { get; set; }
        public IEnumerable<EmployeePunchCardInfoViewModel> employeePunchCardInfoslistNew { get; set; }
        public IEnumerable<WagesPunchCardInfo> wagesPunchCardInfos { get; set; }
        public IEnumerable<ShiftGroupMaster> shiftGroupMasterslist { get; set; }
        public IEnumerable<EmployeeInfo> employeeInfos { get; set; }
        public IEnumerable<EmpAttendanceViewModel> empAttendanceViewModels { get; set; }
        public IEnumerable<JobCardReportViewModel> jobCardReportViewModels { get; set; }
        public IEnumerable<IndividualAttendanceReportViewModel> individualAttendanceReportViewModels { get; set; }
        public IEnumerable<Company> companies { get; set; }
        public IEnumerable<IndividualAttendanceSummaryReportViewModel> individualAttendanceSummaryReportViewModels { get; set; }
        public string visualEmpCodeName { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
		public EmployeePunchCardInfo employeePunchCard { get; set; }

		public DateTime? Fdate { get; set; }
		public DateTime? Tdate { get; set; }
	}
    public class MonthWiseViewModel
    {
        public AllEmployeeJson employee { get; set; }

        public int totalPresent { get; set; }
        public int totalAbsent { get; set; }
        public int totalOnLeave { get; set; }
        public int totalHolidays { get; set; }
        public int actuallyDays { get; set; }
        public int totalWorkingDays { get; set; }
        public int extraWork { get; set; }
        public int ratings { get; set; }
        public decimal? average { get; set; }
    }

    public class SubsidyViewModel
    {
        public EmployeeInfo employee { get; set; }
        public int? attendDays { get; set; }
        public decimal? amount { get; set; }
    }

    public class LunchSubsidyViewModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public int? AttendDays { get; set; }
        public decimal? Amount { get; set; }
    }

    public class SubsidyCountViewModel
    {
        public int attendDays { get; set; }
    }
}
