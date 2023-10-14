using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Payroll.Models
{
    public class FsSalaryStructureViewModel
    {
        public string salaryHeadType { get; set; }
        public string salaryHeadName { get; set; }
        public int? salaryHeadId { get; set; }
        public decimal? amount { get; set; }       
    }

	public class SalarySheetByYearMonthAndSBU
	{
		public int? EmployeeId { get; set; }
		public string EmployeeCode { get; set; }
		public string EmployeeName { get; set; }
		public string Designation { get; set; }
		public string SBAccount { get; set; }
		public int? SalaryPeriodId { get; set; }
		public string PeriodName { get; set; }
		public string LockLabel { get; set; }
		public string SBUName { get; set; }
		public string JoiningDate { get; set; }
		public decimal? Basic { get; set; }
		public decimal? HouseRent { get; set; }
		public decimal? PensionFundBank { get; set; }
		public decimal? Sumptuary { get; set; }
		public decimal? Medical { get; set; }
		public decimal? Conveyance { get; set; }
		public decimal? Dearness { get; set; }
		public decimal? OtherAllowance { get; set; }
		public decimal? HouseHold { get; set; }
		public decimal? MobileAllowance { get; set; }
		public decimal? Cook { get; set; }
		public decimal? Education { get; set; }
		public decimal? Entertainment { get; set; }
		public decimal? ArrearAddition { get; set; }
		public decimal? Charge { get; set; }
		public decimal? Misc { get; set; }
		public decimal? PFOwn { get; set; }
		public decimal? PFBank { get; set; }
		public decimal? PFLoan { get; set; }
		public decimal? HBL { get; set; }
		public decimal? ComputerLoan { get; set; }
		public decimal? GratuityFundBank { get; set; }
		public decimal? GratuityFundDeduction { get; set; }
		public decimal? WelfareFund { get; set; }
		public decimal? PersonalPay { get; set; }
		public decimal? MotorCycleLoan { get; set; }
		public decimal? MotorCarLoan { get; set; }
		public decimal? GroupInsurance { get; set; }
		public decimal? IncomeTax { get; set; }
		public decimal? Telephone { get; set; }
		public decimal? ArrearDeduction { get; set; }
		public decimal? UnionSubscription { get; set; }
		public decimal? OfficerAssociation { get; set; }
		public decimal? BenevolentFund { get; set; }
		public decimal? BusFare { get; set; }
		public decimal? PickAndDrop { get; set; }
		public decimal? PersonalUseBankCar { get; set; }
		public decimal? Sundry { get; set; }
		public decimal? OtherDeduction { get; set; }
		public decimal? TiffinFee { get; set; }
		public decimal? NewsPaper { get; set; }
		public decimal? RevenueStamp { get; set; }
		public decimal? TOTALPAIDBYBANK { get; set; }
		public decimal? TOTALDEDUCT { get; set; }
		public decimal? NETPAY { get; set; }
		public string Status { get; set; }
	}

	public class NetPayByYearMonthAndSBU
	{
		public string EmployeeCode { get; set; }
		public string EmployeeName { get; set; }
		public string Designation { get; set; }
		public string SBAccount { get; set; }
		public int? SalaryPeriodId { get; set; }
		public string PeriodName { get; set; }
		public string LockLabel { get; set; }
		public string SBUName { get; set; }
		public decimal? NETPAY { get; set; }
		public string Status { get; set; }
	}

	public class PostSalary
	{
		public int? salaryPeriodId { get; set; }
		public int? hrBranchId { get; set; }
	}

	public class PostedVoucherVm
	{
		public int? MasterId { get; set; }
		public int? hrBranchId { get; set; }
		public string DebitCode { get; set; }
		public string CreditCode { get; set; }
		public decimal? DebitAmount { get; set; }
		public decimal? CreditAmount { get; set; }
		public DateTime? PostingDate { get; set; }
		public int? salaryPeriodId { get; set; }
		public int? status { get; set; }
		public string uniqueId { get; set; }
	}

	public class PostedVoucherViewModel
	{
		public int? hrBranchId { get; set; }
		public IEnumerable<VoucherMasterDetailVm> voucherMasterDetailVms { get; set; }
	}

	public class VoucherMasterDetailVm
	{
		public int? MasterId { get; set; }
		public string DebitCode { get; set; }
		public decimal? DebitAmount { get; set; }
		public DateTime? PostingDate { get; set; }
		public int? salaryPeriodId { get; set; }
		public int? status { get; set; }
		public string uniqueId { get; set; }

		public IEnumerable<VoucherDetailsVm> voucherDetailsVms { get; set; }
	}
	public class VoucherDetailsVm
	{
		public string CreditCode { get; set; }
		public decimal? CreditAmount { get; set; }
	}
}
