using OPUSERP.Budget.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Accounting.Models
{
    public class AccountingDashboardViewModel
    {
        public IEnumerable<FiscalYear> fiscalYears { get; set; }
    }
}
