using OPUSERP.Data.Entity;
using OPUSERP.Data.Entity.Matrix;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.SCM.Data.Entity.IOU;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.Accounting.Data.Entity.Voucher
{
    [Table("IOUVoucherLog", Schema = "ACCOUNT")]
    public class IOUVoucherLog : Base
    {
        public int? IOUVoucherMasterId { get; set; }
        public IOUVoucherMaster IOUVoucherMaster { get; set; }
        public int? IOUId { get; set; }
        public IOUMaster IOU { get; set; }


        //public int? voucherMasterId { get; set; }
        //public VoucherMaster voucherMaster { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? iouAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? paymentAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? adjustmentAmount { get; set; }
        //public int? paymentBy { get; set; }
        //public DateTime? paymentDate { get; set; }
    }


}
