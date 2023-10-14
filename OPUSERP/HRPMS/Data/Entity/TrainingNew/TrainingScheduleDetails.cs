using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.TrainingNew
{
    [Table("TrainingScheduleDetails")]
    public class TrainingScheduleDetails : Base
    {
        public string startTime { get; set; }
        public string endTime { get; set; }
        public int? trainingScheduleId { get; set; }
        public TrainingSchedule trainingSchedule { get; set; }
        public int? isBreak { get; set; }
        public int? sortOrder { get; set; }
    }
}
