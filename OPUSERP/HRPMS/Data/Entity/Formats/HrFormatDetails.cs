using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Formats
{
	[Table("HrFormatDetails")]
	public class HrFormatDetails:Base
	{
		public int formatMasterId { get; set; }
		public HrFormatMaster formatMaster { get; set; }

		public int employeeId { get; set; }
		public EmployeeInfo employee { get; set; }

		public string editedFormat { get; set; }
		public string fileName { get; set; }

		public int? status { get; set; }
		public int? isActive { get; set; }
		public string remarks { get; set; }
	}
}
