using OPUSERP.Data.Entity.Training;
using OPUSERP.HRPMS.Data.Entity.TrainingNew;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSTrainingNew.Models
{
    public class TrainingTAdaViewModel
    {
        public int? Id { get; set; }
        public int? trainingId { get; set; }
        public string purpose { get; set; }
        public decimal? cost { get; set; }
        public DateTime? date { get; set; }
        public TrainingTaDa trainingTAda { get; set; }
        public IEnumerable<TrainingTaDa> trainingTAdaList { get; set; }
        public IEnumerable<TrainingInfoNew> trainingList { get; set; }

    }
}
