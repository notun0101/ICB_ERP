using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.TrainingNew;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Data.Entity.Training
{
	[Table("TrainingFeedback")]
	public class TrainingFeedback:Base
	{
		public int? trainingId { get; set; }
		public TrainingInfoNew training { get; set; }

		public string feedback { get; set; }

		public int? employeeId { get; set; }
		public EmployeeInfo employee { get; set; }

		public string type { get; set; } //trainee/trainer
	}
}
