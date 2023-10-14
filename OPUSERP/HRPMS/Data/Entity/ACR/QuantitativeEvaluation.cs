using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.ACR
{
	[Table("QuantitativeEvaluation")]
	public class QuantitativeEvaluation:Base
	{
		public int? assessmentId { get; set; }
		public Assessment assessment { get; set; }
		public int? aCRIndicatorId { get; set; }
		public ACRIndicator aCRIndicator { get; set; }
		public decimal? target { get; set; }
		public decimal? achievement { get; set; }
		public decimal? achievementPercent { get; set; }
		public decimal? achievementSign { get; set; }
		public decimal? achievementPercentSign { get; set; }
		public decimal? acrCounter { get; set; }
		public string posting { get; set; }
	}
}
