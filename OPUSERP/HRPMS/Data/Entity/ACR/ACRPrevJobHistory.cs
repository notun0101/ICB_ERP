using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.ACR
{
	[Table("ACRPrevJobHistory", Schema = "ACR")]
	public class ACRPrevJobHistory:Base
	{
		public int? assessmentId { get; set; }
		public Assessment assessment { get; set; }
		public int? employeeId { get; set; }
		public EmployeeInfo employee { get; set; }

		public string company { get; set; }
		public string position { get; set; }
		public DateTime? jobstart { get; set; }
		public DateTime? jobend { get; set; }
		public int? type { get; set; } //1 = Bank, 2 = Others
	}
}
