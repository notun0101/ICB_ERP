using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.ACR
{
	[Table("EvaluationCommentsHead")]
	public class EvaluationCommentsHead:Base
	{
		[MaxLength(400)]
		public string commentsName { get; set; }
		public decimal? commentsMark { get; set; }
	}
}
