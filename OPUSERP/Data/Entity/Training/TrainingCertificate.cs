using OPUSERP.HRPMS.Data.Entity.TrainingNew;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Data.Entity.Training
{
	[Table("TrainingCertificate", Schema = "HR")]
	public class TrainingCertificate:Base
	{
		public int? trainingId { get; set; }
		public TrainingInfoNew training { get; set; }

		public int? traineeId { get; set; }
		public EnrolledTrainee trainee { get; set; }

		public string certificateUrl { get; set; }
		public DateTime? generateDate { get; set; }
	}
}
