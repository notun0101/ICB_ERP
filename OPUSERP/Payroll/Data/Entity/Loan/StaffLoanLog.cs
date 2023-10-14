using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Data.Entity.Loan
{
    [Table("StaffLoanLog")]
    public class StaffLoanLog:Base
    {
        public int? staffLoanId { get; set; }
        public StaffLoan staffLoan { get; set; }

        public DateTime? effectiveDate { get; set; }
        public string particular { get; set; }

        public decimal? debit { get; set; }
        public decimal? credit { get; set; }

        public decimal? principal { get; set; }
        public decimal? interest { get; set; }
        public decimal? charge { get; set; }

        public decimal? total { get; set; }

        public int? isActive { get; set; }
        public int? status { get; set; }
		public int? salaryPeriodId { get; set; }

		public DateTime? interestProcessDate { get; set; }

		public string type { get; set; }
		public string remarks { get; set; }
		public string trxNo { get; set; }
		//public int? TracerNo { get; set; }
	}
}
