using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Payroll.Models
{
    public class SalarySummarySheetSpViewModel
    {
        public string Designation { get; set; }
        public string designationCode { get; set; }
        public int? DesignationCount { get; set; }
        public int? salaryPeriodId { get; set; }
        public string PeriodName { get; set; }
        public decimal? Basic { get; set; }
        public decimal? HouseRent { get; set; }
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
        public decimal? PensionFundBank { get; set; }
        public decimal? PFSubscription { get; set; }
        public decimal? PfBankCont { get; set; }
        public decimal? PFAdvance { get; set; }
        public decimal? HouseBuilding { get; set; }
        public decimal? ComputerLoan { get; set; }
        public decimal? GratuityFundBank { get; set; }
        public decimal? GratuityFundDed { get; set; }
        public decimal? WelfareFund { get; set; }
        public decimal? PersonalPay { get; set; }
        public decimal? MotorCycle { get; set; }
        public decimal? GroupInsurance { get; set; }
        public decimal? IncomeTax { get; set; }
        public decimal? ArrearDeduction { get; set; }
        public decimal? UnionSubscription { get; set; }
        public decimal? Benevolent { get; set; }
        public decimal? BusFare { get; set; }
        public decimal? OtherDed { get; set; }
        public decimal? NewsPaper { get; set; }
        public decimal? RevenueStamp { get; set; }
        public decimal? TOTALPAIDBYBANK { get; set; }
        public decimal? TOTALDEDUCT { get; set; }
        public decimal? NETPAY { get; set; }
        public decimal? BasicBDBL { get; set; }
        public decimal? ArrearBDBL { get; set; }
        public decimal? BasicBSB { get; set; }
        public decimal? ArrearBSB { get; set; }
        public decimal? BasicBSRS { get; set; }
        public decimal? ArrearBSRS { get; set; }
		public decimal? SpecialIncentive { get; set; }



	}
	public class GenerateTrxNumberForLoan
	{
		public string TrxNum { get; set; }
	}
	public class CheckLoanDedVm
	{
		public decimal? Principal { get; set; }
		public decimal? Interest { get; set; }
		public decimal? Charge { get; set; }
	}
	public class SalaryLockVm
	{
		public int? totalRow { get; set; }
	}
	public class SalaryLockWithStatusVm
	{
		public int? Id { get; set; }
		public int? periodId { get; set; }
		public string branchCode { get; set; }
		public int? branchId { get; set; }
		public string periodName { get; set; }
		public string branchName { get; set; }
		public string status { get; set; }
	}
}
