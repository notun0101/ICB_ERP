using OPUSERP.Accounting.Data.Entity.AccountingSettings;
using OPUSERP.Data.Entity;
using OPUSERP.SCM.Data.Entity.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Accounting.Data.Entity.Voucher
{
    [Table("IOUAdjustmentDetail")]
    public class IOUAdjustmentDetail:Base
    {
        public int? iOUAdjustmentMasterId { get; set; }
        public IOUAdjustmentMaster iOUAdjustmentMaster { get; set; }

        public int? voucherDetailId { get; set; }
        public VoucherDetail voucherDetail { get; set; }

        //public int? iOUVoucherMasterId { get; set; }
        //public IOUVoucherMaster iOUVoucherMaster { get; set; }

        public int? itemSpecificationId { get; set; }
        public ItemSpecification itemSpecification { get; set; }

        public int? ledgerRelationId { get; set; }
        public LedgerRelation ledgerRelation { get; set; }

        public decimal? amount { get; set; } // iouAmount
        public decimal? adjAmount { get; set; }

    }
}
