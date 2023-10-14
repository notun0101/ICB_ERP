using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
	[Table("HandoverTakeoverMaster")]
	public class HandoverTakeoverMaster:Base
	{
		public int? handoverId { get; set; }
		public EmployeeInfo handover { get; set; }

		public int? takeoverId { get; set; }
		public EmployeeInfo takeover { get; set; }

		public DateTime? date { get; set; }
	}
}
