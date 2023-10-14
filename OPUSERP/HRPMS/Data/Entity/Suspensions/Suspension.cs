using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Suspensions
{
	[Table("Suspension", Schema = "HR")]
	public class Suspension:Base
	{
        public int? employeeId { get; set; }
        public EmployeeInfo employee { get; set; }

        public string susDesc { get; set; }
		public string susFile { get; set; }
		public string chargeSheetDesc { get; set; }
		public string chargeSheetFile { get; set; }
		public string hearingReport { get; set; }
		public string hearingReportFile { get; set; }
		public string punishmentType { get; set; }
		public DateTime? effectiveForm { get; set; }

		public int? isActive { get; set; }
		public int? status { get; set; }
	}
}
