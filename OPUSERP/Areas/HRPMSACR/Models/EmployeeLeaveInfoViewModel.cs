using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSACR.Models
{
	public class EmployeeLeaveInfoViewModel
	{
		public int? Id { get; set; }
		public string EmpCode { get; set; }
		public string LeaveTypeName { get; set; }
		public int LeaveConsume { get; set; }
		public int LeaveOB { get; set; }
		public int LeaveCB { get; set; }
		public int SerialNo { get; set; }
		public int totalLeave { get; set; }
		public int consumptionLeave { get; set; }
	}
}
