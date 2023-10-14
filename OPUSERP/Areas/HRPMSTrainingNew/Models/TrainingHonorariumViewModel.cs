using OPUSERP.Areas.HRPMSTrainingNew.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.TrainingNew;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSTrainingNew.Models
{
    public class TrainingHonorariumViewModel 
	{
		public int? trainingId { get; set; }
		public int? Id { get; set; }
		public string  trainngName { get; set; }
		public string designation { get; set; }
		public string trainerName { get; set; }

		public int? trainerId { get; set; }
		public ResourcePerson trainer { get; set; }

		public decimal? receivedMoney { get; set; }
		public decimal? taxPercentage { get; set; }
		public decimal? taxAmount { get; set; }

		public int? session { get; set; }
		public DateTime? effectiveDate { get; set; }

		public int? status { get; set; }
		public int? isPaid { get; set; }

        public int? CoOrdinatorId { get; set; }

        public decimal? coOrdinatorPayment { get; set; }
    }
}
