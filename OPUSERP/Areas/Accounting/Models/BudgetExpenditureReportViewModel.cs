using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Accounting.Models
{
    public class BudgetExpenditureReportViewModel
    {
        public string code { get; set; }
        public string name { get; set; }
        public decimal? TotalBudget { get; set; }
        public decimal? TotalActualExpense { get; set; }
        public decimal? CurrentBudgetTotal { get; set; }
        public decimal? ActualAmount { get; set; }
    }
}
