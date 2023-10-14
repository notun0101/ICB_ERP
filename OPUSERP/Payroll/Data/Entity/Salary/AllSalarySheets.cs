using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Data.Entity.Salary
{
	[Table("AllSalarySheets")]
	public class AllSalarySheets
	{
		public int? Id { get; set; }
		public string SBAccount { get; set; }
		public int? salaryPeriodId { get; set; }
		public string employeeCode { get; set; }
		public string nameEnglish { get; set; }
		public string designation { get; set; }
		public string empType { get; set; }
		public string periodName { get; set; }
		public string lockLabel { get; set; }
		public string branchUnitName { get; set; }
		public string projectName { get; set; }
		public decimal? structureBasic { get; set; }
		[DataType(DataType.Date)]
		public DateTime? joiningDatePresentWorkstation { get; set; }
		public string walletNo { get; set; }
		public decimal? walletPayable { get; set; }
		public string bankName { get; set; }
		public string bankAccountNo { get; set; }
		public decimal? bankPayable { get; set; }
		public decimal? cashPayable { get; set; }
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
		public decimal? PFSubscription { get; set; }
		public decimal? PfBankCont { get; set; }
		public decimal? SpecialIncentive { get; set; }
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
		public decimal? SalaryAdjustment { get; set; }
		public decimal? Advance { get; set; }
		public decimal? FestivalBonus { get; set; }
		public decimal? PerformanceBonus { get; set; }
		public decimal? OtherAddition { get; set; }
		public decimal? PFEmployer { get; set; }
		public decimal? DailyAllowance { get; set; }
		public decimal? MobileBillAllowance { get; set; }
		public decimal? InternetAllowance { get; set; }
		public decimal? LeaveEncashment { get; set; }
		public decimal? OverTime { get; set; }
		public decimal? UpKeepAllowance { get; set; }
		public decimal? UtilityAllowance { get; set; }
		public decimal? Annuity { get; set; }
		public decimal? Wages { get; set; }
		public decimal? hardshipAllowance { get; set; }
		public decimal? PFOwn { get; set; }
		public decimal? ExceedCellBill { get; set; }
		public decimal? InstallmentDeduction { get; set; }
		public decimal? AdvanceDeduction { get; set; }
		public decimal? ExcessFuelBill { get; set; }
		public decimal? FamilyPackage { get; set; }
		public decimal? AbsentDeduction { get; set; }
		public decimal? MealCharge { get; set; }
		public decimal? OtherDeduction { get; set; }
		public decimal? HouseLoan { get; set; }
		public decimal? VehicleTax { get; set; }
		public decimal? LastYearTaxAdjustment { get; set; }
		public decimal? ThisYearAdjustment { get; set; }
		public decimal? TransportDeduction { get; set; }
		public decimal? DPSDeduction { get; set; }
		public string Remarks { get; set; }
		public decimal? proposedAmount { get; set; }
		public decimal? PFOC { get; set; }
		public decimal? GROSSFour { get; set; }
		public decimal? GROSS { get; set; }
		public decimal? TOTALPAIDBYBANK { get; set; }
		public decimal? GROSSWPF { get; set; }
		public decimal? TOTALALLOWANCE { get; set; }
		public decimal? TOTALDEDUCT { get; set; }
		public decimal? NET { get; set; }
		public string deptName { get; set; }
	}
}
