using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Data.Entity.Salary
{
    public class LoanPolicyNew: Base
    {
        public int? designationId { get; set; }
        public Designation designation { get; set; }
        public int? employeeInfoId { get; set; }
        public EmployeeInfo employeeInfo  { get; set; }
        [MaxLength(300)]
        public int? jobDuration { get; set; }
        public int? loanDuration { get; set; }

        public string loanPolicyName { get; set; }
        public decimal? maximumLoanAmount { get; set; }
        public decimal? loanInterestRate { get; set; }
        public int? loanNoOfInstallment { get; set; }
        [MaxLength(10)]
        public string isActive { get; set; }

    }
}
