using OPUSERP.Accounting.Data.Entity.Voucher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Accounting.Models
{
    public class LedgerBookViewModelWithoutSP
    {
        public int? VoucherId { get; set; }
        public int? VoucherDetailId { get; set; }
        public int? tranMode { get; set; }
        public string voucherDate { get; set; }
        public DateTime? vDate { get; set; }
        public string voucherNo { get; set; }
        public string accountName { get; set; }
        public string accountCode { get; set; }
        public string subledgerName { get; set; }
        public string voucherType { get; set; }
        public string action { get; set; }
        public string narration { get; set; }
        public string chequeNo { get; set; }

        public decimal? debit { get; set; }
        public decimal? credit { get; set; }

        public IEnumerable<VoucherDetail> voucherDetails { get; set; }
    }
}
