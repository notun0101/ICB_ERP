using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.ProvidentFund.Models
{
    public class PFAccountingViewModel
    {
        //AddCoA
        public int coaId { get; set; }
        public string accountType { get; set; }
        public string accountCode { get; set; }
        public string subAccountOf { get; set; }
        public string accountName { get; set; }
        public string description { get; set; }
        public IEnumerable<PFAccountingViewModel> pFAccountingViewModels { get; set; }
    }
}
