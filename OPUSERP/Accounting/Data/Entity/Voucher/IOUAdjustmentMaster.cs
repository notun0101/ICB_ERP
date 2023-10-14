using OPUSERP.Accounting.Data.Entity.AccountingSettings;
using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Accounting.Data.Entity.Voucher
{
    [Table("IOUAdjustmentMaster")]
    public class IOUAdjustmentMaster : Base
    {
        public int? voucherMasterId { get; set; }
        public VoucherMaster voucherMaster { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        public string adjustmentNo { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? adjustmentAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? advanceAmount { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? adjustmentDate { get; set; }


        [Column(TypeName = "nvarchar(500)")]
        public string remarks { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public string iOUVoucherNos { get; set; }
        
        public int? advLedgerRelationId { get; set; }
        public LedgerRelation advLedgerRelation { get; set; }

        public int? adjustmentBy { get; set; }
     
        public int? status { get; set; }  // 1= Adjustment 2. 

      

    }
}
