using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.ProvidentFund.Data.Entity
{
    [Table("PFContribution")]

    public class PFContribution : Base
    {

        public int? EmployeeInfoId { get; set; }
        public EmployeeInfo EmployeeInfo { get; set; }
        public int? pfmemberId { get; set; }
        public PFMemberInfo pfmember { get; set; }
        public int? SalaryPeriodId { get; set; }
        public SalaryPeriod SalaryPeriod { get; set; }
        public DateTime? transectionDate { get; set; }
        public decimal? companyContribution { get; set; }
        public decimal? employeeContribution { get; set; }
        public decimal? profitOnCompanyContribution { get; set; }
        public decimal? profitOnEmployeeContribution { get; set; }
        public decimal? totalProfit { get; set; }
        public decimal? withdrawn { get; set; }
        public decimal? debit { get; set; }
        public decimal? credit { get; set; }
        public string Particulars { get; set; }
        public string narration { get; set; }
        public int? isJournal { get; set; } //1==Voucher Posted Done

    }
}
