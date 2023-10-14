using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.TrainingNew;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSTrainingNew.Models
{
	public class CertificateViewModel
	{
		public int? trainingId { get; set; }
        public TrainingInfoNew training { get; set; }
        public int? traineeId { get; set; }
        public EnrolledTrainee trainee { get; set; }

        public string certificateUrl { get; set; }

        public DateTime? generateDate { get; set; }

		public EmployeeInfo employee { get; set; }
		public IEnumerable<EmployeeInfo> employeeInfos { get; set; }
        public IEnumerable<EnrolledTrainee> enrolledTrainees { get; set; }
        public EnrolledTrainee enrolledTraineeinfo { get; set; }
        public TrainingInfoNew trainingInfoNews { get; set; }
        public IEnumerable<CourseTitle> courseTitles { get; set; }
    }

}
