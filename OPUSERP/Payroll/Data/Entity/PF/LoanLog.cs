using OPUSERP.Data.Entity;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Data.Entity.PF
{
	[Table("LoanLog")]
	public class LoanLog:Base
	{
		public int? loanId { get; set; }
		public PFLoan loan { get; set; }

		public int? nextapprover { get; set; } //userid
		public int? approveOrder { get; set; } = 1;
		public int? ispassed { get; set; } = 0;
	}
}
