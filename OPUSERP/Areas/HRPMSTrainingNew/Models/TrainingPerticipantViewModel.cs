using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.TrainingNew;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSTrainingNew.Models
{
	public class TrainingPerticipantViewModel
	{
		public int? trainingId { get; set; }
		public int[] perticipants { get; set; }

		public TrainingWithPerticipants trainingWithPerticipants { get; set; }

        public IEnumerable<EmployeeInfo> employeeInfos { get; set; }
    }
	public class TrainingWithPerticipants
	{
		public TrainingInfoNew training { get; set; }
		public IEnumerable<EmployeeInfo> employees { get; set; }
	}
}
