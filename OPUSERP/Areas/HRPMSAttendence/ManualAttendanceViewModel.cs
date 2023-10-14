namespace OPUSERP.Areas.HRPMSAttendence
{
	public class ManualAttendanceViewModel
	{
		public string EmployeeCode { get; set; }
		public string EmployeeName { get; set; }
		public string WorkDate { get; set; }
		public int? AttendanceTypeId { get; set; }
		public string AttendanceType { get; set; }
		public string StartTime { get; set; }
		public string EndTime { get; set; }
		public int? ApproveStatus { get; set; }
		public string ApproveStatusShort { get; set; }
		public string ApproveStatusFull { get; set; }
		public string ApproverCode { get; set; }
		public string ApproverName { get; set; }
				
	}
}
