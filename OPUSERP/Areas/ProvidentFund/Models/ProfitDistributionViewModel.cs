using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.ProvidentFund.Models
{
    public class ProfitDistributionViewModel
    {
        public int profitDistributionId { get; set; }
        public string memberName { get; set; }
        public int contribution { get; set; }
        public int accumulatedProfit { get; set; }
        public int surplus { get; set; }
        public string profitPeriod { get; set; }

        public IEnumerable<ProfitDistributionViewModel> profitDistributionViewModels { get; set; }
         

    }
}
