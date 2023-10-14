using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Models
{
	public class CBSSalarySheetViewModel
	{
		public DateTime? ProcessDate { get; set; }
		public string PeriodName { get; set; }
		public string EmployeeCode { get; set; }
		public string EmployeeName { get; set; }
		public string Designation { get; set; }
		public string SalaryAccount { get; set; }
		public string SBUName { get; set; }
		public decimal? NetPay { get; set; }
		public string Status { get; set; }
	}
}
