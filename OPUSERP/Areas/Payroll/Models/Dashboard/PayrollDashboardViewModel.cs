using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Payroll.Models.Dashboard
{
    public class PayrollDashboardViewModel
    {
        public int? pendingForSalaryApproval { get; set; }
        public int? SalaryApproved { get; set; }
        public int? appliedLoan { get; set; }
        public int? approvedLoan { get; set; }
    }
}
