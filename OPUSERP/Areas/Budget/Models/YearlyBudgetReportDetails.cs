using OPUSERP.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Budget.Models
{
    public class YearlyBudgetReportDetails
    {
        public Company company { get; set; }

        public IEnumerable<YearlyBudgetReport> yearlyBudgetReports { get; set; }

    }
}
