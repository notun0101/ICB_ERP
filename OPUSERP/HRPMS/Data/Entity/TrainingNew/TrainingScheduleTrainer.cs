using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.TrainingNew
{
    [Table("TrainingScheduleTrainer")]
    public class TrainingScheduleTrainer : Base
    {
		public int? trainingScheduleId { get; set; }
		public TrainingSchedule trainingSchedule { get; set; }
		public int? resourcePersonId { get; set; }
		public ResourcePerson resourcePerson { get; set; } 
	}
}
