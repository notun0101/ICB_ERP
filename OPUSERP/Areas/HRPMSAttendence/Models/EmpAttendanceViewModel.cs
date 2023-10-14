using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Attendance;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.Areas.HRPMSAttendence.Models
{
    [NotMapped]
    public class EmpAttendanceViewModel
    {
		public int Id { get; set; }
		public string EmpCode { get; set; }
		[MaxLength(50)]
		public string punchCardNo { get; set; }
		[MaxLength(20)]
		public string workDate { get; set; }
		[MaxLength(20)]
		public string startTime { get; set; }
		public int lateMin { get; set; }
		[MaxLength(20)]
		public string endTime { get; set; }
		public int? summaryId { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		public decimal? workingTime { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		public decimal? latetime { get; set; }
		public string remarks { get; set; }
		[MaxLength(200)]
		public string attenUpdateBy { get; set; }
		[MaxLength(200)]
		public string attenUpdateAppBy { get; set; }
        public int? shiftGroupMasterId { get; set; }
        public string employeeCode { get; set; }
        public string notes { get; set; }

        public IEnumerable<EmpWithManualAttendence> empAttendances { get; set; }
		public IEnumerable<ShiftGroupMaster> shiftGroupMasterslist { get; set; }
		public IEnumerable<AttendenceApi> addedAttendence { get; set; }
		public IEnumerable<ShiftGroupDetail> shiftGroupDetail { get; set; }

	}

	public class ManualAttendanceForAPIViewModel
	{
		public string empCode { get; set; }
		public DateTime? punchDate { get; set; }
		public string punchTime { get; set; }
        public string notes { get; set; }
        public string location { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
    }

	public class EmpWithManualAttendence
	{
		public EmpAttendance empAttendance { get; set; }
		public EmployeeInfo employeeInfo { get; set; }
		public IEnumerable<EmpManualAttBySpVM> empManualAttBySp { get; set; }
        public IEnumerable<ShiftGroupMaster> shiftGroupMasterslist { get; set; }
		public IEnumerable<ManualAttendanceApproval> manualAttendanceApprovals { get; set; }
	}

	public class ManualAttendanceApproval
	{
		public int? Id { get; set; }
		public string employeeCode { get; set; }
		public string nameEnglish { get; set; }
		public string designationName { get; set; }
		public string workDate { get; set; }
		public string startTime { get; set; }
		public string endTime { get; set; }
		public int? summaryId { get; set; }
        public string reason { get; set; }
        public string remarks { get; set; }
	
	}
	public class ManualAttendanceApprovalApi
	{
		public int? Id { get; set; }
		public string employeeCode { get; set; }
		public string nameEnglish { get; set; }
		public string designationName { get; set; }
		public string workDate { get; set; }
		public string startTime { get; set; }
		public string endTime { get; set; }
		public int? summaryId { get; set; }
		public string reason { get; set; }
		public string location { get; set; }
		public double lat { get; set; }
		public double lon { get; set; }
		public string Remarks { get; set; }
		public int? AttendanceTypeId { get; set; }
	}
	public class ManualAttendanceRejectApi
	{
		public string empCode { get; set; }
		public int id { get; set; }
		public string approverEmpCode { get; set; }
		public string reasonforreject { get; set; }
		
	}
	public class ShiftGroupViewModel
	{
		public int ShiftGroupId { get; set; }
		public string GroupName { get; set; }
		public string EmpCode { get; set; }
		public int EmpId { get; set; }
		public string InTime { get; set; }
		public string OutTime { get; set; }
	}

    public class EmpManualAttBySpVM
    {
        public int? Id { get; set; }
        public string employeeCode { get; set; }
        
        public string nameEnglish { get; set; }
        public string designationName { get; set; }
        public string deptName { get; set; }
        public string designationCode { get; set; }
     
        public string workDate { get; set; }
       
        public string startTime { get; set; }
      
        public string endTime { get; set; }
        public int? summaryId { get; set; }
        public string notes { get; set; }
     
    }

}
