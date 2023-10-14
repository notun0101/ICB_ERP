using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.IELTS
{
	[Table("IeltsInfo", Schema = "IELTS")]
	public class IeltsInfo:Base
	{
		public int employeeId { get; set; }
		public EmployeeInfo employee { get; set; }

		public string examType { get; set; } //Academic,General
		public string centerNo { get; set; }
		public DateTime? date { get; set; }
		public string candidateNo { get; set; }

		public decimal? listeningScore { get; set; }
		public decimal? readingScore { get; set; }
		public decimal? writingScore { get; set; }
		public decimal? speakingScore { get; set; }
		public decimal? overallScore { get; set; }
		public decimal? cefrScore { get; set; }

		public string attachment { get; set; }

		public int? status { get; set; }
	}
}
