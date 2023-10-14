using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Payroll.Data.Entity;
using OPUSERP.Payroll.Data.Entity.Loan;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Payroll.Models
{

    public class StaffLoanDeductionTypeVM
    {
        public int? staffLoanId { get; set; }
        public int? employeeInfoId { get; set; }
        public int? InstallmentType { get; set; }
        public decimal? DeductionAmount { get; set; }
    }
    public class StaffLoanVm
    {
        public int? staffLoanId { get; set; }
        public int? staffLoanLogId { get; set; }
        public int? employeeInfoId { get; set; }
        public string empCode { get; set; }
        public string employeeName { get; set; }
        public string loanType { get; set; }
        public string loanNo { get; set; }
        public string loanAc { get; set; }
        public decimal? totalDisbursement { get; set; }
        public decimal? amount { get; set; }
        public decimal? principal { get; set; }
        public decimal? interest { get; set; }
        public decimal? charge { get; set; }
        public decimal? totalAmount { get; set; }
        public DateTime? loanDate { get; set; }

        public DateTime? expiryDate { get; set; }
        public DateTime? DisburseDate { get; set; }
        public string trxType { get; set; }
		public string remarks { get; set; }
		public string trxNo { get; set; }
		public string loanNewType { get; set; }
		public decimal? sanctionAmount { get; set; }
		public DateTime? sanctionDate { get; set; }
        public DateTime? firstInstallmentDate { get; set; }
        public DateTime? loanTenure { get; set; }

        public int? InstallmentType { get; set; }

        public decimal? DeductionAmount { get; set; }
		public string Particular { get; set; }
        //public int? StaffLoanLogId { get; set; }
		//public int? salaryHeadId { get; set; }
		//public int? salaryGradeId { get; set; }

		public IEnumerable<StaffLoan> staffLoans { get; set; }
        public IEnumerable<SalaryPeriod> salaryPeriods { get; set; }
        public IEnumerable<StaffLoanLog> staffLoanLogs { get; set; }
    }

    public class UploadLoanViewModel
    {
        public IEnumerable<UploadExcelLog> uploadExcelLogs { get; set; }
    }

    public class StaffLoanFilterVm
    {
        public IEnumerable<HrBranch> hrBranches { get; set; }
        public IEnumerable<Location> zones { get; set; }
        public IEnumerable<StaffLoan> staffLoans { get; set; }
    }

	public class UploadLoanDataExcelFormat
	{
		public string EmpId { get; set; }
		public string Name { get; set; }
		public string Designation  { get; set; }
		public string LoanType { get; set; }
		public string TotalDisbursement { get; set; }
		public string CBSAc { get; set; }
		public string LoanAc { get; set; }
		public string Principal { get; set; }
		public string Interest { get; set; }
		public string Charge { get; set; }
		public string Total { get; set; }
        public string loanDate { get; set; }
    }
}
