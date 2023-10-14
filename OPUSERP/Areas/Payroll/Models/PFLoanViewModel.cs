using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Payroll.Data.Entity.Salary;
using OPUSERP.ProvidentFund.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Payroll.Models
{
    public class PFLoanViewModel
    {

        public int PfloanId { get; set; }
        public int employeeInfoId { get; set; }
        public int salaryPeriodId { get; set; }
        public int salaryHeadId { get; set; }
        public int? salaryGradeId { get; set; }
        public string PfLoanNo { get; set; }
        public PFLoan PfLoan { get; set; }

        public string rptDate { get; set; }
        public string EmpName { get; set; }
        public string Designation { get; set; }



        public int? salarySlab { get; set; }//new
    
        public decimal? amount { get; set; }//new

        public decimal advanceAmount { get; set; }
        public decimal installmentAmount { get; set; }
        public int noOfInstallment { get; set; }       
        public int? isFromSalary { get; set; }
        public DateTime? loanDate { get; set; }
        public string purpose { get; set; }

        public IEnumerable<PFLoan> pFLoans { get; set; }
        public IEnumerable<PFLoanSchedule> pFLoanSchedules { get; set; }
        public IEnumerable<SalaryPeriod> salaryPeriods { get; set; }
        public IEnumerable<SalaryHead> salaryHeads { get; set; }
        public IEnumerable<SalaryGrade> salaryGradesList { get; set; }

        public string visualEmpCodeName { get; set; }

		public int? approver { get; set; }
		public string approverName { get; set; }
		public string loanType { get; set; }

		public IEnumerable<LoansViewModel> loans { get; set; }
        public IEnumerable<Company> companies { get; set; }
        public IEnumerable<PfAccountsSchedule> pfAccountsSchedules { get; set; }
        public IEnumerable<EmployeeInfo> employeeInfos { get; set; }

		public PFMemberInfo pFMember { get; set; }
		public IEnumerable<PFYearlyIndividualVm> pFYearlyIndividuals { get; set; }
        
    }
	public class PFYearlyIndividualVm
	{
		public string effectiveDate { get; set; }
		public string description { get; set; }
		public decimal? pfAmount { get; set; }
		public int? MonthOrder { get; set; }
	}
	public class LoansViewModel
	{
		public PFLoan loan { get; set; }
		public EmployeeInfo employee { get; set; }
	}

    public class PFAccountsViewModel
    {
        public IEnumerable<Company> companies { get; set; }
        public EmployeeInfo employee { get; set; }
        public PFMemberInfo pFMember { get; set; }
        public PfAccountsSchedule pfAccounts { get; set; }
        public DateTime? firstDate { get; set; }
        public DateTime? lastDate { get; set; }
		public decimal totalpfloan { get; set; }
        public IEnumerable<PFEmployeeDetailViewModel> PFEmployeeDetailList { get; set; }
        public IEnumerable<PFBranchWiseMonthlySummeryViewModel> PFBranchWiseMonthlySummeryList { get; set; }
        public PfYearlyStatement pfYearlyStatements { get; set; }

    }

    public class PFEmployeeDetailViewModel {
        public int pfMemberId { get; set; }
        public string nameEnglish { get; set; }
        public string DisburseDate { get; set; }
        public decimal? creditAmount { get; set; }
        public string designationName { get; set; }
        public decimal? DebitAmount { get; set; }
        public string branchName { get; set; }
        public string Descriptions { get; set; }
        public string BranchCode {get; set;}
    }
	public class PfYearlyStatement
	{
		public int? MemberId { get; set; }
		public string employeeCode { get; set; }
		public string empName { get; set; }
		public string designationName { get; set; }
		public string Posting { get; set; }
		public decimal? PfBank { get; set; }
		public decimal? PfOwn { get; set; }
	}
	public class PfYearlyStatementWithMonth
	{
		public int? MemberId { get; set; }
		public string employeeCode { get; set; }
		public string empName { get; set; }
		public string designationName { get; set; }
		public string Posting { get; set; }
		public decimal? januaryBank { get; set; }
		public decimal? januaryOwn { get; set; }
		public decimal? februaryBank { get; set; }
		public decimal? februaryOwn { get; set; }
		public decimal? marchBank { get; set; }
		public decimal? marchOwn { get; set; }
		public decimal? aprilBank { get; set; }
		public decimal? aprilOwn { get; set; }
		public decimal? mayBank { get; set; }
		public decimal? mayOwn { get; set; }
		public decimal? juneBank { get; set; }
		public decimal? juneOwn { get; set; }
		public decimal? julyBank { get; set; }
		public decimal? julyOwn { get; set; }
		public decimal? augustBank { get; set; }
		public decimal? augustOwn { get; set; }
		public decimal? septemberBank { get; set; }
		public decimal? septemberOwn { get; set; }
		public decimal? octoberBank { get; set; }
		public decimal? octoberOwn { get; set; }
		public decimal? novemberBank { get; set; }
		public decimal? novemberOwn { get; set; }
		public decimal? decemberBank { get; set; }
		public decimal? decemberOwn { get; set; }
	}

	public class PFBranchWiseMonthlySummeryViewModel
    {

        public string BranchCode { get; set; }
        public string branchName { get; set; }
        public decimal? creditAmount { get; set; }
        public decimal? DebitAmount { get; set; }
    }


}
