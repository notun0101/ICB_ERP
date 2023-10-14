using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Payroll.Data.Entity.Salary;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.ProvidentFund.Data.Entity
{
    [Table("PfTransaction")]
    public class PfTransaction:Base
    {
        public int? employeeId { get; set; }
        public EmployeeInfo employee { get; set; }

        public int? pfMemberId { get; set; }
        public PFMemberInfo pfMember { get; set; }

        public int? yearId { get; set; }
        public SalaryYear year { get; set; }

        public int? salaryPeriodId { get; set; }
        public SalaryPeriod salaryPeriod { get; set; }

        public string month { get; set; }
        public decimal? pfOwn { get; set; }
        public decimal? pfCompany { get; set; }
        public decimal? pfLoanAmount { get; set; }
        public decimal? installmentPay { get; set; }


        public int? status { get; set; }
        public string remarks { get; set; }
    }
}
