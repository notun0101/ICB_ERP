using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Accounting.Data.Entity.Voucher
{
    [Table("CreateBillVoucherAttachment")]
    public class CreateBillVoucherAttachment:Base
    {
        public int? createBillVoucherMasterId { get; set; }
        public CreateBillVoucherMaster createBillVoucherMaster { get; set; }

        public string fileName { get; set; }

        public string url { get; set; }

        public int? status { get; set; }

        public string remarks { get; set; }
    }
}
