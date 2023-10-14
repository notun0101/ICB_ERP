using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Accounting.Data.Entity.Voucher
{
    [Table("IOUAdjustmentSpend", Schema = "ACCOUNT")]
    public class IOUAdjustmentSpend:Base
    {
        public int? iOUAdjustmentMasterId { get; set; }
        public IOUAdjustmentMaster iOUAdjustmentMaster { get; set; }

        public int? voucherMasterId { get; set; }
        public VoucherMaster voucherMaster { get; set; }

        public decimal? amount { get; set; }
        public string remarks { get; set; }
        public int? type { get; set; } // 1= UnderSpend 2= overspend


        [NotMapped]
        public int? ledgerRelationId { get; set; }
        [NotMapped]
        public int? ledgerId { get; set; }
        [NotMapped]
        public string ledgerRelationName { get; set; }

    }
}
