using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.TrainingNew
{
    [Table("TrainingSchedule", Schema = "HR")]
    public class TrainingSchedule:Base
    {
        public DateTime? effectiveDate { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string topic { get; set; }

        public int? trainingId { get; set; }
        public TrainingInfoNew training { get; set; }
        public string trainers { get; set; }

        public int? isBreak { get; set; }

        public int? trainerId { get; set; }
        public ResourcePerson trainer { get; set; }

        public int? sortOrder { get; set; }
    }
}
