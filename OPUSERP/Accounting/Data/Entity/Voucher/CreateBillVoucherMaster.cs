using OPUSERP.Accounting.Data.Entity.AccountingSettings;
using OPUSERP.Data.Entity;
using OPUSERP.SCM.Data.Entity.Stock;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Accounting.Data.Entity.Voucher
{
    [Table("CreateBillVoucherMaster", Schema = "ACCOUNT")]
    public class CreateBillVoucherMaster:Base
    {
        public int? stockMasterId { get; set; }
        public StockMaster stockMaster { get; set; }

        public int? voucherMasterId { get; set; }
        public VoucherMaster voucherMaster { get; set; }

        public int? ledgerRelationId { get; set; } // Account Payable Ledger 
        public LedgerRelation ledgerRelation { get; set; }

        public int? subledgerRelationId { get; set; } // supplier sub ledger Relation Ledger 
        public LedgerRelation subledgerRelation { get; set; }

        public string billNumber { get; set; }

        public DateTime? date { get; set; }

        public int? status { get; set; } // DUE,

        public string remarks { get; set; }
    }
}
