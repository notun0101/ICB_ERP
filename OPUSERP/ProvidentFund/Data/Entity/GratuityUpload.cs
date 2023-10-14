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
    [Table("GratuityUpload", Schema = "PF")]
    public class GratuityUpload : Base
    {
        //public int? pfmemberId { get; set; }
        //public PFMemberInfo pfmember { get; set; }

        public int? salaryPeriodId { get; set; }
        public SalaryPeriod salaryPeriod { get; set; }
        public int? salaryYearId { get; set; }
        public SalaryYear salaryYear { get; set; }
        public string memberCode { get; set; }
        public decimal? gratuityAmount { get; set; }
        public int? branchId { get; set; }
        public int? pNSId { get; set; }
        public PNS pNS { get; set; }
        public int? ledgerId { get; set; }
        public Ledger ledger { get; set; }
        public int? isJournal { get; set; } // 0 = Data Upload ,1 = AutoVoucher Create
        public DateTime? date { get; set; }
    }
}
