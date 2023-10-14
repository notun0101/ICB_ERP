using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Accounting.Models
{
    public class CashBankAccountBalanceDashboard
    {
        public string name { get; set; }
        public decimal? amount { get; set; }
        public string  code { get; set; }
        public string  type { get; set; }
    }
}
