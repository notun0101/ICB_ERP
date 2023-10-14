using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Accounting.Models
{
    public class AccountingDashboardModel
    {
        public List<string> months { get; set; }

        public List<decimal?> monthWiseIncome { get; set; }

        public List<decimal?> monthWiseExpense { get; set; }

        public List<string> accountGroup { get; set; }

        public List<decimal?> accountGroupWiseVouture { get; set; }
    }
}
