using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.ACR
{
	[Table("QuantitativeEvaluationHead")]
	public class QuantitativeEvaluationHead:Base
	{
		[MaxLength(400)]
		public string HeadName { get; set; }
	}
}
