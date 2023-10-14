using OPUSERP.Data.Entity.Master;
using System.Collections.Generic;

namespace OPUSERP.Areas.Accounting.Models
{
    public class LedgerBookViewModel
    {
        public decimal? obDebit { get; set; }
        public decimal? obCredit { get; set; }
        public string AccountName { get; set; }
        public string VoucherTypeName { get; set; }

        public IEnumerable<LedgerBookReportViewModel> ledgerBookReportViewModels { get; set; }

        public IEnumerable<LedgerBookViewModelWithoutSP> ledgerBookViewModelWithoutSPs { get; set; }
        public IEnumerable<LedgerBookViewModelWithoutSP> ledgerBookPreviousUnRecons { get; set; }
        public IEnumerable<LedgerBookViewModelWithoutSP> ledgerBookCurrentUnRecons { get; set; }
        public IEnumerable<BankBookReportViewModel> bankBookReportViewModels { get; set; }
        public Company Company { get; set; }
    }
}
