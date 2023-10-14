using OPUSERP.Data.Entity;
using OPUSERP.SCM.Data.Entity.IOU;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Accounting.Data.Entity.Voucher
{
    [Table("IOUAdjustmentDetaillog")]
    public class IOUAdjustmentDetaillog:Base
    {
        public int? iOUAdjustmentDetailId { get; set; }
        public IOUAdjustmentDetail iOUAdjustmentDetail { get; set; }

        public int? iOUDetailId { get; set; }
        public IOUDetails iOUDetail { get; set; }

        public string remarks { get; set; }
        public int? status { get; set; }
    }
}
