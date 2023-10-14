using OPUSERP.HRPMS.Data.Entity.TrainingNew;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Data.Entity.Training
{
    [Table("TrainingAttendance")]
    public class TrainingAttendance:Base
    {
        public int? trainingId { get; set; }
        public TrainingInfoNew training { get; set; }

        public int? traineeId { get; set; }
        public EnrolledTrainee trainee { get; set; }

        public int? isPresent { get; set; }
    }
}
