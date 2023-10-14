using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Data.Entity
{
    public class StaffLoanUploadFailed:Base
    {
        public string empCode { get; set; }
        public string empName { get; set; }
        public string postingPlace { get; set; }
        public string loanType { get; set; }
        public string LoanNo { get; set; }
        public string NewLoanNo { get; set; }
        public decimal? interestRate { get; set; }
        public decimal? principalAmount { get; set; }
        public decimal? interestAmount { get; set; }
        public decimal? chargeAmount { get; set; }
        public decimal? totalAmount { get; set; }
        public decimal? totalDisbursement { get; set; }
        public DateTime? loanDate { get; set; }
    }
}
