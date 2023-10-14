using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Models
{
	public class CBSVoucherViewModel
	{
		public DateTime? ProcessDate { get; set; }
		public string periodName { get; set; }
		public string Particular { get; set; }
		public string account { get; set; }
		public string DEBIT_CREDIT { get; set; }
		public string branchName { get; set; }
		public decimal? Amount { get; set; }
		public string Status { get; set; }
	}
}
