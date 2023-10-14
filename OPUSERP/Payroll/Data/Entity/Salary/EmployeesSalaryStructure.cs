using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.Payroll.Data.Entity.Salary
{
    [Table("EmployeesSalaryStructure", Schema = "Payroll")]
    public class EmployeesSalaryStructure : Base
    {
        public int employeeInfoId { get; set; }
        public EmployeeInfo employeeInfo { get; set; }

        public int? salarySlabId { get; set; }
        public SalarySlab salarySlab { get; set; }

        public int salaryHeadId { get; set; }
        public SalaryHead salaryHead { get; set; }

        public decimal amount { get; set; }
        [MaxLength(10)]
        public string isActive { get; set; }

        public DateTime? effectiveDate { get; set; }

        public string pfType { get; set; } //CPF, GPF
        public int? pfPercentage { get; set; }

		public int? isPartial { get; set; }
		
		public DateTime? expiryDate { get; set; }

		public string loanDeduction { get; set; }
		public int? loanDedAmount { get; set; }
	}
}
