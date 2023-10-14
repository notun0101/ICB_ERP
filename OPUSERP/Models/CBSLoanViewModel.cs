using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Models
{
	public class CBSLoanViewModel
	{
		public DateTime? ProcessDate { get; set; }
		public string periodName { get; set; }
		public string LoanType { get; set; }
		public string account { get; set; }
		public string branchName { get; set; }
		public decimal? Amount { get; set; }
		public string Status { get; set; }
	}
}
