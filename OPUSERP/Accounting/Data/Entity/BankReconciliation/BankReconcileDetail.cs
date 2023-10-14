using OPUSERP.Accounting.Data.Entity.AccountingSettings;
using OPUSERP.Accounting.Data.Entity.Voucher;
using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Accounting.Data.Entity.BankReconciliation
{
    [Table("BankReconcileDetail", Schema = "ACCOUNT")]
    public class BankReconcileDetail : Base
    {

        public int? bankReconcileMasterId { get; set; }
        public BankReconcileMaster bankReconcileMaster { get; set; }
        public int? voucherDetailId { get; set; }
        public VoucherDetail voucherDetail { get; set; }
        public int? voucherMasterId { get; set; }
        public VoucherMaster voucherMaster { get; set; }
        public string chequeNo { get; set; }
       
        public int? isChecked { get; set; }

        public DateTime? chackDate { get; set; }
        public DateTime? bankClearingDate { get; set; }
        public DateTime? unCheckDate { get; set; }
        public decimal? drAmount { get; set; }
        public decimal? crAmount { get; set; }
        public int? transectionMode { get; set; } // 2= payment/Credit 1 = Deposit/debit

        public int? specialBranchUnitId { get; set; }
        public SpecialBranchUnit specialBranchUnit { get; set; }
    }
}
