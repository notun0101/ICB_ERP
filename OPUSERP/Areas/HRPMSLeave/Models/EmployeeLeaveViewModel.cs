using System;

namespace OPUSERP.Areas.HRPMSLeave.Models
{
	public class EmployeeLeaveViewModel
	{
		public string EmployeeCode { get; set; }
		public string EmployeeName { get; set; }
		public string LeaveTypeName { get; set; }

		public DateTime? ApplicationDate { get; set; }
		public DateTime? FromDate { get; set; }
		public DateTime? ToDate { get; set; }

		public string LeaveStatus { get; set; }
		public string ApproverCode { get; set; }
		public string ApproverName { get; set; }
	}
}
