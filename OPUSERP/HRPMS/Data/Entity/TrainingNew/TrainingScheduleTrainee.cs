using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.TrainingNew
{
    [Table("TrainingScheduleTrainee")]
    public class TrainingScheduleTrainee: Base
    {
		public int? trainingScheduleId { get; set; }
		public TrainingSchedule trainingSchedule { get; set; }
		public int? employeeInfoId { get; set; }
		public EmployeeInfo employeeInfo { get; set; }
	}
}
