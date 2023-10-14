using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.ACR
{
	[Table("EmployeesAcr")]
	public class EmployeesAcr:Base
	{
		public int? assessmentId { get; set; }
		public Assessment assessment { get; set; }
		public int? acrEvaluationNameId { get; set; }
		public virtual AcrEvaluationName acrEvaluationName { get; set; }
		public string empCode { get; set; }
		public long? firstValue { get; set; }
		public long? sencondValue { get; set; }
		public long? thirdValue { get; set; }
		public long? forthValue { get; set; }
		public long? fifthValue { get; set; }
	}
}
