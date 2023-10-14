using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Data.Entity
{
	[Table("StaffLoan")]
	public class StaffLoan:Base
	{
		public int? employeeInfoId { get; set; }
		public EmployeeInfo employeeInfo { get; set; }

        public string empCode { get; set; }
        public string empName { get; set; }
        public string postingPlace { get; set; }

        public int? designationId { get; set; }
        public Designation designation { get; set; }

        public int? salaryPeriodId { get; set; }
		public SalaryPeriod salaryPeriod { get; set; }

		public int? salaryHeadId { get; set; }
		public SalaryHead salaryHead { get; set; }

		public int? salaryGradeId { get; set; }
		public SalaryGrade salaryGrade { get; set; }

		public string LoanNo { get; set; }
		public string NewLoanNo { get; set; }
		public decimal? interestRate { get; set; }

		public decimal? principalAmount { get; set; }
		public decimal? interestAmount { get; set; }
		public decimal? chargeAmount { get; set; }
		public decimal? totalAmount { get; set; }
		public decimal? totalDisbursement { get; set; }
		public decimal? sanctionLimit { get; set; }

		public decimal? installmentAmount { get; set; }
		public int? noOfInstallment { get; set; }

		[DefaultValue(0)]
		public int? isFromSalary { get; set; }

		[MaxLength(300)]
		public string purpose { get; set; }
		public DateTime? loanDate { get; set; }

		public string name { get; set; }
		public string designationEn { get; set; }


		public int? forwardedBy { get; set; } //userid
		public string loanType { get; set; } //Car/Home/MCA
		public string homeLoanType { get; set; } //HBA1/HBA2
		public int? approver { get; set; } //userid

		public DateTime? expiryDate { get; set; }
		public DateTime? DisburseDate { get; set; }
		public string loanNewType { get; set; }
		public decimal? sanctionAmount { get; set; }
		public DateTime? sanctionDate { get; set; }
		public DateTime? firstInstallmentDate { get; set; }
		public DateTime? loanTenure { get; set; }


		public int? InstallmentType { get; set; }

		public decimal? DeductionAmount { get; set; }
        public int? IsComplete { get; set; }

    }
}
