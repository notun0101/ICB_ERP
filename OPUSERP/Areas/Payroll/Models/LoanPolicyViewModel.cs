using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Payroll.Data.Entity.Salary;
using System.Collections.Generic;

namespace OPUSERP.Areas.Payroll.Models
{
    public class LoanPolicyViewModel
    {
        public int editId { get; set; }
        public int? salaryGradeId { get; set; }
        public int? salaryHeadId { get; set; }

        public string loanPolicyName { get; set; }
        public decimal? maximumLoanAmount { get; set; }
        public decimal? loanInterestRate { get; set; }
        public int? loanNoOfInstallment { get; set; }
        public string isActive { get; set; }

        public int? designationId { get; set; }
        public Designation designation { get; set; }
        public int? employeeInfoId { get; set; }
        public EmployeeInfo employeeInfo { get; set; }

        public int? jobDuration { get; set; }
        public int? loanDuration { get; set; }

        public int?[] desId { get; set; }
        public IEnumerable<LoanPolicy> loanPolicies { get; set; }
        public IEnumerable<LoanPolicyNew> loanPolicyNews { get; set; }
        public IEnumerable<SalaryGrade> salaryGradesList { get; set; }
        public IEnumerable<SalaryHead> salaryHeadsList { get; set; }
        public IEnumerable<Designation>  designations { get; set; }
        public IEnumerable<EmployeeInfo>  employeeInfos { get; set; }
    }
}
