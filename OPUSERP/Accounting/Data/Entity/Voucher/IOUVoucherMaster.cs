using OPUSERP.Accounting.Data.Entity.AccountingSettings;
using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Accounting.Data.Entity.Voucher
{
    [Table("IOUVoucherMaster")]
    public class IOUVoucherMaster : Base
    {
        public int? voucherMasterId { get; set; }
        public VoucherMaster voucherMaster { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? voucherDate { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        public string voucherNo { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? voucherAmount { get; set; }
        

        [Column(TypeName = "nvarchar(500)")]
        public string remarks { get; set; }

        public int? paymentBy { get; set; }
        public DateTime? paymentDate { get; set; }

        public int? status { get; set; }  // 1= IOU pending 2. Adjustment

        public int? autoVoucherNameId { get; set; }
        public AutoVoucherName autoVoucherName { get; set; }

        public int? ledgerRelationId { get; set; }
        public LedgerRelation ledgerRelation { get; set; }

        public int? subLedgerRelationId { get; set; }
        public LedgerRelation subLedgerRelation { get; set; }
    }
}
