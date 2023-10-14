using OPUSERP.HRPMS.Data.Entity.TrainingNew;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Data.Entity.Training
{
	[Table("TrainingTaDa")]
	public class TrainingTaDa:Base
	{
		public int? trainingId { get; set; }
		public TrainingInfoNew training { get; set; }

		public string purpose { get; set; }
		public decimal? cost { get; set; }
		public DateTime? date { get; set; }
	}
}
