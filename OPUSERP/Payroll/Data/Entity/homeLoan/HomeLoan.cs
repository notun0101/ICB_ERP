using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Data.Entity.homeLoan
{
    [Table("HomeLoan")]
    public class HomeLoan:Base
    {
        public int employeeInfoId { get; set; }
        public EmployeeInfo employeeInfo { get; set; }

        public int salaryPeriodId { get; set; }
        public SalaryPeriod salaryPeriod { get; set; }

        public int salaryHeadId { get; set; }
        public SalaryHead salaryHead { get; set; }

        public int? salaryGradeId { get; set; }
        public SalaryGrade salaryGrade { get; set; }

        public string HomeLoanNo { get; set; }

        public decimal advanceAmount { get; set; }
        public decimal installmentAmount { get; set; }
        public int noOfInstallment { get; set; }

        [DefaultValue(0)]
        public int? isFromSalary { get; set; }

        [MaxLength(300)]
        public string purpose { get; set; }
        public DateTime? loanDate { get; set; }

        public string loanType { get; set; } //pf/home/car
        public int? approver { get; set; } //userid
    }
}
