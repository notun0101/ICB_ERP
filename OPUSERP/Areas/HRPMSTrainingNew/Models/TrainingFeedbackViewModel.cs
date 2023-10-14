using OPUSERP.Areas.HRPMSTrainingNew.Models.Lang;
using OPUSERP.Data.Entity.Master;
using OPUSERP.Data.Entity.Training;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.TrainingNew;
using System;
using System.Collections.Generic;

namespace OPUSERP.Areas.HRPMSTrainingNew.Models
{
    public class TrainingFeedbackViewModel
    {
        public int trainingfeedbackId { get; set; }
        public int? trainingId { get; set; }
        public TrainingInfoNew training { get; set; }
        public TrainingFeedback trainingfd { get; set; }
        //public int trainingname { get; set; }
        public string feedback { get; set; }

        public int? employeeId { get; set; }
        public EmployeeInfo employee { get; set; }

        public string employeeIdUser { get; set; }
        public string type { get; set; } //trainee/trainer

        public IEnumerable<EmployeeInfo> trainee { get; set; }
        public IEnumerable<TrainingFeedback> trainingFeedbackList { get; set; }
        public IEnumerable<EmployeeType> employeeTypes { get; set; }
        public IEnumerable<TrainingInfoNew> trainingInfoNewList { get; set; }


    }
}
