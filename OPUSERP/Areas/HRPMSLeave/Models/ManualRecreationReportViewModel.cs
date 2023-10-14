using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Leave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSLeave.Models
{
    public class ManualRecreationReportViewModel
    {
        public string employeeCode { get; set; }
        public int? designationId { get; set; }
        public string designationCode { get; set; }
        public string seniorityLevel { get; set; }
        public string nameEnglish { get; set; }
        public string nameBangla { get; set; }
        public string designationName { get; set; }
        public string designationNameBN { get; set; }
        public DateTime? leaveFrom { get; set; }
        public DateTime? leaveTo { get; set; }
        public DateTime? lastLeaveFrom { get; set; }
        public DateTime? lastLeaveTo { get; set; }
        public string postingBn { get; set; }
        public string postingEn { get; set; }
        public int? sortOrder { get; set; }
    }



    public class ActualRecreationReportHRViewModel
    {
        public string employeeCode { get; set; }
        public int? designationId { get; set; }
        public string designationCode { get; set; }
        public string seniorityLevel { get; set; }
        public string nameEnglish { get; set; }
        public string nameBangla { get; set; }
        public string designationName { get; set; }
        public string designationNameBN { get; set; }
        public DateTime? leaveFrom { get; set; }
        public DateTime? leaveTo { get; set; }

        public DateTime? ActualLeaveFrom { get; set; }
        public DateTime? ActualLeaveTo { get; set; }
        public DateTime? lastLeaveFrom { get; set; }
        public DateTime? lastLeaveTo { get; set; }
        public string postingBn { get; set; }
        public string postingEn { get; set; }
        public int? sortOrder { get; set; }
    }

    public class ManualRecreationReportVM
    {
        public IEnumerable<ManualRecreationReportViewModel> ManualRecreationReportViewModel { get; set; }
        public IEnumerable<ActualRecreationReportHRViewModel> actualReportHRViewModel { get; set; }
        public IEnumerable<LeaveCommittee> LeaveCommitteList { get; set; }
    }

    public class LateAttendanceReportVM
    {
        public IEnumerable<Company> companies { get; set; }
        public IEnumerable<LateAttandenceDataVM> lateAttandenceDataVMs { get; set; }
    }

    public class LateAttandenceDataVM
    {
        public string employeeCode { get; set; }
        public string EmpName { get; set; }
        public string designationName { get; set; }
        public string workDate { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public int? summaryId { get; set; }
        public string reason { get; set; }
        public string lateOrEarly { get; set; }
    }

    public class ProposedListForHeadViewModel
    {
        public int? leaveId { get; set; }
        public int? employeeId { get; set; }
        public int? leaveStatus { get; set; }
        public int? locationId { get; set; }
        public int? hrBranchId { get; set; }

        public string employeeCode { get; set; }
        public string nameEnglish { get; set; }
        public string designationName { get; set; }
        public DateTime? leaveFrom { get; set; }
        public DateTime? leaveTo { get; set; }
        public decimal? daysLeave { get; set; }
      
        public string purposeOfLeave { get; set; }
        public string nameEn { get; set; }
        public string paymentOption { get; set; }
    }
    #region
    public class ProposedListForZoneHeadViewModel
    {
        public int? leaveId { get; set; }
        public int? employeeId { get; set; }
        public int? leaveStatus { get; set; }
        public int? locationId { get; set; }
        public int? hrBranchId { get; set; }

        public string employeeCode { get; set; }
        public string nameEnglish { get; set; }
        public string designationName { get; set; }
        public DateTime? leaveFrom { get; set; }
        public DateTime? leaveTo { get; set; }
        public decimal? daysLeave { get; set; }

        public string purposeOfLeave { get; set; }
        public string nameEn { get; set; }
        public string paymentOption { get; set; }
        public string reason { get; set; }
    }
    #endregion
    public class ReportProposedListViewModel
    {
        public int? leaveId { get; set; }
        public int? employeeId { get; set; }
        public int? leaveStatus { get; set; }
        public string employeeCode { get; set; }
        public string nameEnglish { get; set; }
        public string nameBangla { get; set; }
        public string designationName { get; set; }
        public string designationNameBN { get; set; }
        public string DepBrZnName { get; set; }
        public string DepBrZnNameBN { get; set; }
        public DateTime? leaveFrom { get; set; }
        public DateTime? leaveTo { get; set; }
        public decimal? daysLeave { get; set; }
      
        public string purposeOfLeave { get; set; }
        public string nameEn { get; set; }
        public string paymentOption { get; set; }
    }



    public class ActualListForHeadViewModel
    {
        public int? leaveId { get; set; }
        public int? employeeId { get; set; }
        public int? leaveStatus { get; set; }
        public int? locationId { get; set; }
        public int? hrBranchId { get; set; }

        public string employeeCode { get; set; }
        public string nameEnglish { get; set; }
        public string designationName { get; set; }
        public DateTime? leaveFrom { get; set; }
        public DateTime? leaveTo { get; set; }
         public DateTime? ActualLeaveFrom { get; set; }
        public DateTime? ActualLeaveTo { get; set; }


        public decimal? daysLeave { get; set; }

        public string purposeOfLeave { get; set; }
        public string nameEn { get; set; }
        public string paymentOption { get; set; }



        public int? SubsEmId { get; set; }
        public string subEmpCode { get; set; }
        public string subsEmpName { get; set; }
        public string subEmDesignation { get; set; }

    }


    public class ReportActualListViewModel
    {
        public int? leaveId { get; set; }
        public int? employeeId { get; set; }
        public int? leaveStatus { get; set; }
        public string employeeCode { get; set; }
        public string nameEnglish { get; set; }
        public string nameBangla { get; set; }
        public string designationName { get; set; }
        public string designationNameBN { get; set; }
        public string DepBrZnName { get; set; }
        public string DepBrZnNameBN { get; set; }
        public DateTime? leaveFrom { get; set; }
        public DateTime? leaveTo { get; set; }

        public DateTime? ActualLeaveFrom { get; set; }
        public DateTime? ActualLeaveTo { get; set; }

        public DateTime? lastLeaveFrom { get; set; }
        public DateTime? lastLeaveTo { get; set; }
        public decimal? daysLeave { get; set; }

        public string purposeOfLeave { get; set; }
        public string nameEn { get; set; }
        public string paymentOption { get; set; }
    }

}
