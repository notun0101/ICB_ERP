using OPUSERP.Accounting.Data.Entity.AccountingSettings;
using OPUSERP.CRM.Data.Entity.MasterData;
using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.ProvidentFund.Data.Entity
{
    [Table("UploadLoanData", Schema = "PF")]
    public class UploadLoanData : Base
    {
        public int? salaryPeriodID { get; set; }
        public SalaryPeriod salaryPeriod { get; set; }
        public int? salaryYearId { get; set; }
        public SalaryYear salaryYear { get; set; }
        public string memberCode { get; set; }
        //public int? bankId { get; set; }
        //public Bank bank { get; set; }

        public decimal? loanPrincipalAmount { get; set; }
        public decimal? loanInterestAmount { get; set; }
        public int? interestRate { get; set; }
        public int? installmentNumber { get; set; }
        public string note { get; set; }
        public int? branchId { get; set; }
        public int? pNSId { get; set; }
        public PNS pNS { get; set; }
        public int? ledgerId { get; set; }
        public Ledger ledger { get; set; }
        public int? isJournal { get; set; } // 0 = Data Upload ,1 = AutoVoucher Create
        public DateTime? transactionDate { get; set; }
        public decimal? settlementCharge { get; set; }
        public decimal? settlementDiscount { get; set; }
        public int? loanManagementId { get; set; }

    }
}
