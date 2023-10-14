using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Payroll.Models
{
    public class BranchSalarySummaryViewModel
    {

       
        public int salaryPeriodId { get; set; }
        public string PeriodName { get; set; }
        public string branchName { get; set; }
        public string branchCode { get; set; }
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
        public int? TotalEmployee { get; set; }
		public decimal? SpecialIncentive { get; set; }
	}

    public class LoanNumberGenerate
    {
        public string loanNo { get; set; }
    }

    public class LoanPaid
    {
        public int IsPaid { get; set; }
    }
    public class FilterStaffLoanViewModel
    {
        public int Id { get; set; }
        public int employeeInfoId { get; set; }
        public string employeeCode { get; set; }
        public string nameEnglish { get; set; }
        public string loanType { get; set; }
        public decimal? totalDisbursement { get; set; }
        public decimal? principalAmount { get; set; }
        public decimal? interestAmount { get; set; }
        public decimal? chargeAmount { get; set; }
        public decimal? totalAmount { get; set; }
        public DateTime? loandate { get; set; }
        public string CBSLoanNo { get; set; }
        public string NewLoanNo { get; set; }
        public int? InstallmentType { get; set; }
        public decimal? DeductionAmount { get; set; }
    }
}
