using OPUSERP.Areas.HRPMSTrainingNew.Models.Lang;
using OPUSERP.Data.Entity.Master;
using OPUSERP.Data.Entity.Training;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Training;
using OPUSERP.HRPMS.Data.Entity.TrainingNew;
using System;
using System.Collections.Generic;

namespace OPUSERP.Areas.HRPMSTrainingNew.Models
{
    public class TrainingInfoFilterViewModel
    {
        public int? trainingId { get; set; }
        public string subjects { get; set; }
        public int? trainerId { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public int? instituteId { get; set; }
       


        public IEnumerable<TrainingInfoNew> trainingInfoNews { get; set; }
        public IEnumerable<TrainingInstitute> trainingInstitutes { get; set; }
        public IEnumerable<Trainer>  trainers { get; set; }
        public IEnumerable<TrainingSubject> trainingSubjects { get; set; }
        public IEnumerable<ResourcePerson> resourcePeople { get; set; }
      //  public IEnumerable<TrainingInstitute> trainingInstitutes { get; set; }




    }
}
