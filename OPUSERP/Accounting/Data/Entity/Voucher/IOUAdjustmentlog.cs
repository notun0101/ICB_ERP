using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Accounting.Data.Entity.Voucher
{
    [Table("IOUAdjustmentlog", Schema = "ACCOUNT")]
    public class IOUAdjustmentlog:Base
    {
        public int? iOUAdjustmentMasterId { get; set; }
        public IOUAdjustmentMaster iOUAdjustmentMaster { get; set; }

        public int? iOUVoucherMasterId { get; set; }
        public IOUVoucherMaster iOUVoucherMaster { get; set; }

        public int? status { get; set; }
        public string remarks { get; set; }
    }
}
