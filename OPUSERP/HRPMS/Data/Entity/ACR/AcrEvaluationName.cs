using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.ACR
{
	[Table("AcrEvaluationName")]
	public class AcrEvaluationName:Base
	{
		public int? acrForId { get; set; }
		public AcrFor acrFor { get; set; }
		public int? acrTypeId { get; set; }
		public AcrType acrType { get; set; }
		[MaxLength(150)]
		public string evaluationName { get; set; }
		[MaxLength(150)]
		public string evaluationNameBn { get; set; }
		[MaxLength(950)]
		public string description { get; set; }
		public int? serialNo { get; set; }
		public int? shortOrder { get; set; }
	}
}
