using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Payroll.Models
{
    public class SalaryWithDesignation
    {
        public Designation designation { get; set; }
        public HrBranch hrBranch { get; set; }
        public IEnumerable<MonthlySalaryReportViewModel> monthlySalaryReports { get; set; }
        public IEnumerable<BranchMonthlySalaryReportViewModel> branchMonthlySalaryReports { get; set; }
        
    }
}