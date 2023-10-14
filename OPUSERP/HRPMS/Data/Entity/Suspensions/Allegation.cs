using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Suspensions
{
	[Table("Allegation", Schema = "HR")]
	public class Allegation:Base
	{
        public int? employeeId { get; set; }
        public EmployeeInfo employee { get; set; }

        public string allegationDetail { get; set; }
		public string allegationUrl { get; set; }
		public string clarification { get; set; }
		public string clarificationUrl { get; set; }
		public string management { get; set; }
		public string managementUrl { get; set; }

		public int? isActive { get; set; }
		public int? status { get; set; }
	}
}
