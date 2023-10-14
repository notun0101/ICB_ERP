using Microsoft.AspNetCore.Http;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.HRPMSLeave.Models.Lang;
using OPUSERP.Areas.HRPMSLeave.Models.NotMapped;
using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Leave;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OPUSERP.Areas.HRPMSLeave.Models
{
    public class LeaveRegisterViewModel
    {
        public string userName { get; set; }
        public int id { get; set; }

        public int[] registerids { get; set; }
        public int?[] subsId { get; set; }

        public int? employeeId { get; set; }
        public int? substitutionUserId { get; set; }
        public decimal? leaveBalance { get; set; }
        public int leaveTypeId { get; set; }
        public int leaveregId { get; set; }
        public int? leaveDayId { get; set; }
        public string whenLeave { get; set; }
        public string empCode { get; set; }
        public string reason { get; set; }
        

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? leaveFrom { get; set; }
        public DateTime? leaveFromN { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? leaveTo { get; set; }
        public DateTime? leaveToN { get; set; }

        public decimal? daysLeave { get; set; }
        public decimal? daysLeaveN { get; set; }
        public string purposeOfLeave { get; set; }
        public string substitutionEmpCode { get; set; }
        public string emergencyContactNo { get; set; }
        public string leaveStatus { get; set; }
        public string address { get; set; }
        public string txtRemarks { get; set; }
        public string paymentType { get; set; }
        public string comment { get; set; }

        public IFormFile updateAttatchment { get; set; }
        public IFormFile fileUrl { get; set; }

        public LeaveLn fLang { get; set; }

        public LeaveRegister leaveRegister { get; set; }
        public EmployeeSignature employeeSignature { get; set; }
        public EmployeeSignature employeeGMSignature { get; set; }
        public EmployeeSignature employeeMDSignature { get; set; }
        public EmployeeSignature employeeDJBSignature { get; set; }
        public IEnumerable<ApprovalDetail> approvalDetails { get; set; }
        public IEnumerable<LeaveRegister> leaveRegisterslist { get; set; }
        public IEnumerable<WagesLeaveRegister> wagesLeaveRegisters { get; set; }
        public IEnumerable<LeaveType> leaveTypelist { get; set; }
        public IEnumerable<LeaveRoute> leaveRoutes { get; set; }
        public IEnumerable<LeaveReportModel> leaveReportModels { get; set; }
        public IEnumerable<Year> years { get; set; }
        public IEnumerable<Company> companies { get; set; }
        public IEnumerable<LeaveSummaryReport> leaveSummaryReports { get; set; }
        public IEnumerable<LeaveSupervisorRecomViewModel> leaveSupervisorRecomViewModels { get; set; }
        public IEnumerable<LeaveIndividualViewModel> leaveIndividualViewModels { get; set; }
        public Supervisor supervisor { get; set; }
        public ApprovalDetail approvalDetail { get; set; }
        public IEnumerable<LeaveDay> leaveDays { get; set; }
        public IEnumerable<LeaveStatusViewModel> leaveStatusViewModels { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
        public string visualEmpCodeName { get; set; }

        public IEnumerable<LeaveStatusLog> leaveStatusLogs { get; set; }

        public IEnumerable<EmployeeWithUser> applicationUsers { get; set; }
        public IEnumerable<LeaveStatusLogViewModel> leaveStatusLogViewModels { get; set; }
        public IEnumerable<AppDetailsWithLog> appDetailsWithLogs { get; set; }
        public IEnumerable<LeaveRegisterViewModelN> ProleaveRegisters { get; set; }
        public decimal availedThisYear { get; set; }
        public decimal availedThisYearByDate { get; set; }
        public decimal totalBalance { get; set; }
        public decimal presentBalance { get; set; }
        public decimal presentUsed { get; set; }
        public decimal presentBal { get; set; }
        public IEnumerable<EmployeeInfo> employeeInfos { get; set; }
        public EmployeeInfo employee { get; set; }
        public string employeeReportId { get; set; }
        public DepartBranchZoneHeadViewModel headViewModel { get; set; }


        public decimal AverageLeave { get; set; }
        public decimal FullLeave { get; set; }
        public decimal LeaveWithoutPay { get; set; }
        public decimal ExtraOrdinaryLeave { get; set; }
        public int RecreationLeave { get; set; }

        public IEnumerable<ProposedListForHeadViewModel> proposedlist { get; set; }
    }

    public class LeaveRegisterViewModelN
    {
        public LeaveRegister leaveRegisterslists { get; set; }
        public ProposedListForHeadViewModel proposedlists { get; set; }
        public ActualListForHeadViewModel actuallists { get; set; }
        public ReportProposedListViewModel reportlists { get; set; }
        public ReportActualListViewModel reportActuallists { get; set; }
        public ProposedListForZoneHeadViewModel proposedlistsForZone { get; set; }
      

    }

    public class EmployeeWithUser
    {
        public string EmpCode { get; set; }
        public string UserName { get; set; }
        public string EmployeeName { get; set; }
        public int UserId { get; set; }
    }
    public class LeaveStatusLogViewModel
    {
        public LeaveStatusLog leaveStatusLog { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
    }
    public class AppDetailsWithLog
    {
        public ApprovalDetail approvalDetail { get; set; }
        public int regId { get; set; }
        public LeaveStatusLog leaveStatusLog { get; set; }
    }

    public class LeaveRegisterViewModelAPI
    {
        public string userName { get; set; }
        public DateTime? leaveFrom { get; set; }
        public DateTime? leaveTo { get; set; }
        public string purposeOfLeave { get; set; }
        public decimal? daysLeave { get; set; }
        public string emergencyContactNo { get; set; }
        public string address { get; set; }
        public int? substitutionUserId { get; set; }
        public decimal? leaveBalance { get; set; }
        public int leaveTypeId { get; set; }
        public int? leaveDayId { get; set; }
        public string whenLeave { get; set; }
    }


    public class LeaveCommitteeViewModel
    {
        public string userName { get; set; }
        public int id { get; set; }

        public int[] registerids { get; set; }

        public int approvalMasterId { get; set; }
        public int? employeeInfoId { get; set; }
        public int? approvalTypeId { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
        public int employeeID { get; set; }

        public int?[] approverId { get; set; }


        public string[] empNameBn { get; set; }
        public string[] empNameEng { get; set; }

        //
        public int?[] employeeId { get; set; }
        public EmployeeInfo employee { get; set; }

        public int?[] leaveRegisterId { get; set; }
        public LeaveRegister leaveRegister { get; set; }

        public string[] designationBn { get; set; }
        public string[] designationEn { get; set; }

        public string[] departmentBn { get; set; }
        public string[] departmentEn { get; set; }

        public int?[] memberRoleId { get; set; }//সভাপতি=1,সদস্য=2,সদস্য সচিব=3
        public int?[] CoYear { get; set; }

        public IEnumerable<LeaveCommittee> leaveCommitteeList { get; set; }


    }
}
