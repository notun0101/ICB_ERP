using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
	[Table("HrCommunication")]
	public class HrCommunication:Base
	{
		public int? employeeId { get; set; }
		public EmployeeInfo employee { get; set; }

		public string refNo { get; set; }
		public string email { get; set; }
		public DateTime? date { get; set; }

		public int? status { get; set; }
		public string remarks { get; set; }

		public string mailBody { get; set; }
		public DateTime? mailSendingDate { get; set; }
	}
}
