using OPUSERP.Accounting.Data.Entity.AccountingSettings;
using OPUSERP.Accounting.Data.Entity.MasterData;
using OPUSERP.CRM.Data.Entity.MasterData;
using OPUSERP.Data.Entity;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Accounting.Data.Entity.BankReconciliation
{
    [Table("BankReconciliationMaster", Schema = "ACCOUNT")]
    public class BankReconcileMaster : Base
    {
        public int? sbuId { get; set; }
       
        public string ReconRef { get; set; }
        public decimal? closingBalance { get; set; }
        public decimal? bankStatementClosingBalance { get; set; }
        public DateTime? closingDate { get; set; }
        public DateTime? reconDate { get; set; }

        public DateTime? reconFromDate { get; set; }
        public DateTime? reconToDate { get; set; }

        public int? ledgerId { get; set; }
        public Ledger ledger { get; set; }
        
        public int? fiscalYearId { get; set; }
        public virtual SalaryYear fiscalYear { get; set; }

        public int? taxYearId { get; set; }
        public virtual TaxYear taxYear { get; set; }
        
        public int? fundSourceId { get; set; }
        public virtual FundSource fundSource { get; set; }
        public int? companyId { get; set; }

        public int? ledgerRelationId { get; set; }
        public LedgerRelation ledgerRelation { get; set; }
    }
}
