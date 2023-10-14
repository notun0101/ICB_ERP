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
    [Table("PFUploadData", Schema = "PF")]
    public class PFUploadData : Base
    {
        public int? salaryPeriodID { get; set; }
        public SalaryPeriod salaryPeriod { get; set; }
        public int? salaryYearId { get; set; }
        public SalaryYear salaryYear { get; set; }
        public string memberCode { get; set; }
        public decimal? memberContribution { get; set; }
        public decimal? companyContribution { get; set; }
        public int? branchId { get; set; }
        public int? pNSId { get; set; }
        public PNS pNS { get; set; }
        public int? ledgerId { get; set; }
        public Ledger ledger { get; set; }
        public int? isJournal { get; set; } // 0 = Data Upload ,1 = AutoVoucher Create

        public decimal? ifrcTranjaction  { get; set; }
        public DateTime? date { get; set; }
    }
}
