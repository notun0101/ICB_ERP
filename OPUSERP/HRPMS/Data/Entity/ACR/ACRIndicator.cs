using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.ACR
{
	[Table("ACRIndicator", Schema = "HR")]
	public class ACRIndicator:Base
	{
		public int? headId { get; set; }
		public QuantitativeEvaluationHead head { get; set; }
		[MaxLength(200)]
		public string indicatorName { get; set; }
		[MaxLength(200)]
		public string indicatorNameBn { get; set; }

		public int? shortOrder { get; set; }
	}
}
