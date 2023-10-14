using OPUSERP.Accounting.Data.Entity.BankReconciliation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Accounting.Models
{
    public class BankReconciliationModel
    {
        public int? bankReconciliationId { get; set; }
        public string bankReconciliationNo { get; set; }

        public int? sbuId { get; set; }
        public string ReconRef { get; set; }

        public int? txtPaymentAccountId { get; set; }
        public int? hdnPaymentAccId { get; set; }
        public int? txtPaymentAccountCode { get; set; }
        public string txtPaymentnatureId { get; set; }
        public decimal? currentBalance { get; set; }
        public decimal? bankStatementClosingBalance { get; set; }
        public int? projectId { get; set; }

        public DateTime? closingDate { get; set; }
        
        public string remarks { get; set; }
        public DateTime? reconFromDate { get; set; }
        public DateTime? reconToDate { get; set; }



        public int?[] voucherIds { get; set; }
        public int?[] voucherMasterIds { get; set; }
        public decimal?[] diposit { get; set; }
        public decimal?[] payment { get; set; }
       //ublic DateTime?[] reconDate { get; set; }
        public string[] reconDate { get; set; }
        public string[] isChecked { get; set; }
        public int?[] isCheck { get; set; }
        public string[] accountCode { get; set; }
        public string[] chequeNo { get; set; }
        public string[] voucherNo { get; set; }
        
        

        public virtual IEnumerable<BankReconcileDetail> bankReconcileDetails { get; set; }
        public virtual IEnumerable<BankReconcileMaster> bankReconcileMasters { get; set; }
        public BankReconcileMaster bankReconcileMaster { get; set; }
        public LedgerBookViewModel ledgerBookViewModel { get; set; }
        
    }

    
}
