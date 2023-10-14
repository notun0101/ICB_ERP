using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Payroll.Models
{
	public class EmpContributionViewModel
	{
		public int? employeeId { get; set; }

		public string employeeCode { get; set; }
		public string employeeName { get; set; }
		public string department { get; set; }
		public string lastDesignation { get; set; }

		public string year { get; set; }
		public string month { get; set; }
		public decimal? own { get; set; }
		public decimal? company { get; set; }
        public decimal? Total { get; set; }
        

    }
}
