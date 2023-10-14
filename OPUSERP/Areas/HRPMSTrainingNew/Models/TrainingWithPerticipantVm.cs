using OPUSERP.HRPMS.Data.Entity.TrainingNew;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSTrainingNew.Models
{
    public class TrainingWithPerticipantVm
    {
        public TrainingInfoNew training { get; set; }
        public IEnumerable<EnrolledTrainee> trainees { get; set; }
    }
}
