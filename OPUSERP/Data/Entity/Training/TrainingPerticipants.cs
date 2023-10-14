using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.TrainingNew;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Data.Entity.Training
{
	[Table("TrainingPerticipants")]
	public class TrainingPerticipants:Base
	{
		public int? trainingId { get; set; }
		public TrainingInfoNew training { get; set; }

		public int? traineeId { get; set; }
		public EmployeeInfo trainee { get; set; }

		public int? attendance { get; set; } //0=absent, 1=present

		public int? marking { get; set; }
	}
}
