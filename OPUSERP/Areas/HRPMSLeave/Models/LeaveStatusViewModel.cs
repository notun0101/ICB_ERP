using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSLeave.Models
{
	public class LeaveStatusViewModel
	{
		public string LeaveTypeName { get; set; }
		public decimal OpeningLeaveBalance { get; set; }
		public decimal yearlyMaxLeave { get; set; }
		public decimal leaveCarryDays { get; set; }
		public decimal leaveAvailed { get; set; }
		public decimal leaveBalance { get; set; }
		public decimal cumLeaveBalance { get; set; }
		public decimal userLeaveBalance { get; set; }
	}
}
