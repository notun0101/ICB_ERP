using OPUSERP.Data.Entity.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
    public class TrainingSubjectViewModel
    {
        public int tSubjectId { get; set; }

        public string name { get; set; }

        public IEnumerable<TrainingSubject> trainingSubjects { get; set; }
    }
}
