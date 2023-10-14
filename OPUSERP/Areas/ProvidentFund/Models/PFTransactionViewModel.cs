using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Payroll.Data.Entity.Salary;
using OPUSERP.ProvidentFund.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.ProvidentFund.Models
{
    public class PFTransactionViewModel
    {

        public IEnumerable<PfTransaction> pfTransactions { get; set; }
        public IEnumerable<SalaryYear> years { get; set; }
        public IEnumerable<PfAccountsSchedule> pfAccountsSchedules { get; set; }
        //pfinterest
        public int editId { get; set; }
        public string year { get; set; }
        public decimal? investmentAmount { get; set; }
        public decimal? interestRate { get; set; }
        public decimal? total { get; set; } //calculated

        public IEnumerable<PfInterest> pfInterests { get; set; }
        public IEnumerable<SalaryPeriod> salaryPeriods { get; set; }
    }
    public class PFTransactionProcessViewModel
    {
        public IEnumerable<PFProcessViewModel> pfProcess { get; set; }
        public SalaryPeriod salaryPeriod { get; set; }
    }
    public class PFTotalPfAmountByYear
    {
        public decimal? Total { get; set; }

    }
    public class PFProcessViewModel
    {
        public EmployeeInfo employee { get; set; }
        public string pfCode { get; set; }
        public decimal? own { get; set; }
        public decimal? company { get; set; }
        public decimal? totalDeduction { get; set; }
    }
    public class EmployeeWithPfMember
    {
        public EmployeeInfo employee { get; set; }
        public PFMemberInfo pFMember { get; set; }
    }

}
