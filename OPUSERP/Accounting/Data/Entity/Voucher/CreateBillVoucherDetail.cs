using OPUSERP.Data.Entity;
using OPUSERP.SCM.Data.Entity.Stock;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Accounting.Data.Entity.Voucher
{
    [Table("CreateBillVoucherDetail")]
    public class CreateBillVoucherDetail:Base
    {
        public int? createBillVoucherMasterId { get; set; }
        public CreateBillVoucherMaster createBillVoucherMaster { get; set; }

        public int? stockDetailsId { get; set; }
        public StockDetails stockDetails { get; set; }

        public int? voucherDetailId { get; set; }
        public VoucherDetail voucherDetail { get; set; }

        public decimal? qty { get; set; }

        public decimal? rate { get; set; }

        public decimal? amount { get; set; }

        public decimal? vatAmount { get; set; }

        public decimal? totalAmount { get; set; }

        public int? status { get; set; }

        public string remarks { get; set; }
    }
}
