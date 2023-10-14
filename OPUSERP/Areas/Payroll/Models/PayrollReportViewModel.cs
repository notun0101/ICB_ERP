using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Models;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Payroll.Models
{
	public class PayrollReportViewModel
	{
        public SalaryPeriod salaryPeriod { get; set; }
        public IEnumerable<SalaryPeriod> salaryPeriods { get; set; }
        public IEnumerable<EmployeeInfoWithPostingVm> employeeInfos { get; set; }
		public IEnumerable<SpecialBranchUnit> specialBranchUnits { get; set; }
		public IEnumerable<PayslipReportViewModel> payslipReportViewModels { get; set; }
		public IEnumerable<PayslipReportViewModel> payslipAdditionViewModels { get; set; }
		public IEnumerable<PayslipReportViewModel> payslipDeductionViewModels { get; set; }
		public IEnumerable<MonthlySalaryReportViewModel> monthlySalaryReportViewModels { get; set; }
		public IEnumerable<BranchMonthlySalaryReportViewModel> branchMonthlySalaryReportViewModels { get; set; }
		public IEnumerable<BankStatementReportViewModel> bankStatementReportViewModels { get; set; }
		public IEnumerable<GratuityReportViewModel> gratuityReportViewModels { get; set; }
		public IEnumerable<Company> companies { get; set; }
		public IEnumerable<UniversalCodaXLTempleteViewModel> universalCodaXLTempleteViewModels { get; set; }
		public IEnumerable<TaxYear> taxYears { get; set; }
		public IEnumerable<HrDivision> hrDivisions { get; set; }
		public IEnumerable<ShiftGroupMaster> shifts { get; set; }
		public IEnumerable<EmployeeInfo> employeeInfosd { get; set; }
		public IEnumerable<EmployeeInfo> employeeInfoDepts { get; set; }
		public IEnumerable<Designation> designationss { get; set; }
		public IEnumerable<FunctionInfo> functionInfos { get; set; }
		public IEnumerable<Location> locations { get; set; }
		public IEnumerable<HrUnit> hrUnits { get; set; }
		public IEnumerable<StaffLoanAsOnViewModel> staffLoanAsOn { get; set; }
		public IEnumerable<SBSReportViewModel> sbsReport { get; set; }
		public IEnumerable<BonusSummaryReportViewModel> bonusSummary { get; set; }
		public IEnumerable<BranchWiseBonusSummary> bonusSummaryByBranch { get; set; }
		public IEnumerable<HeadOfficeWiseBonusSummary> headOfficeBonusSummaries { get; set; }
        public IEnumerable<SalaryCertificateReportViewModel> salaryCertificateReportVM { get; set; }
        public IEnumerable<AllBranchSalarySheetVm> allSalarySheetVms { get; set; }
        public IEnumerable<ParticularNamesVm> particulars { get; set; }

        public IEnumerable<BranchSalarySummaryViewModel> branchSalarySummaryViewModels { get; set; }
		public IEnumerable<BranchSalarySummaryViewModel> branchSalarySummaryViewModelZone { get; set; }

		public EmployeeInfo employeeInfo { get; set; }
		public EmployeeInfoLoanVm employeeInfoForLoan { get; set; }
		public EmployeeInfoLoanVmNew employeeInfoForLoanNew { get; set; }


		public IEnumerable<BonusReportViewModel> bonuses { get; set; }
		public IEnumerable<BonusVoucherViewModel> bonusVouchers { get; set; }
		public IEnumerable<AllSheetBonusVm> allBonuses { get; set; }
		public IEnumerable<IndivisualEmpSalaryReportViewModel> indivisualEmpSalaryReportViewModels { get; set; }


		public IEnumerable<Department> Departments { get; set; }
		public IEnumerable<HrBranch> hrBranches { get; set; }
		public IEnumerable<BranchandZoneVm> branchandZoneVms { get; set; }
		public HrBranch hrBranchinfo { get; set; }
		public IEnumerable<Department> departments { get; set; }

		public IEnumerable<EmpTaxDetailsViewModel> empTaxDetailsViewModels { get; set; }
		public IEnumerable<EmpTaxSlabViewModel> empTaxSlabViewModels { get; set; }
		public IEnumerable<EmpRebatableTaxViewModel> empRebatableTaxViewModels { get; set; }
		public IEnumerable<EmpTaxDeductFinalViewModel> empTaxDeductFinalViewModels { get; set; }

		public string visualEmpCodeName { get; set; }

		public IEnumerable<Designation> designations { get; set; }
		public IEnumerable<SalaryWithDesignation> salaryWithDesignations { get; set; }
		public IEnumerable<SalaryVoucherViewModel> slaryVoucherVm { get; set; }
		public IEnumerable<SalaryVoucherAllViewModel> slaryVoucherAllVm { get; set; }


		public IEnumerable<AllLoanViewModel> allLoans { get; set; }
		public IEnumerable<salaryAddDiductionViewModel> salaryAddDiductionViewModels { get; set; }
		public IndividualLoans indLoan { get; set; }

		public IEnumerable<StaffLoanViewModel> staffLoans { get; set; }
		public IEnumerable<StaffLoanTransactions> staffLoanTrans { get; set; }
		public IEnumerable<StaffLoanScheduleViewModel> staffLoanSchedules { get; set; }
		public IEnumerable<StaffLoanInterestVm> staffLoanInterestVms { get; set; }
		public IEnumerable<StaffLoanInterestChargeVm> staffLoanInterestChargeVms { get; set; }
		public IEnumerable<StaffLoanAllInterestVm> allstaffLoanInterestVms { get; set; }
		public IEnumerable<AllStaffLoanVm> allStaffLoanVms { get; set; }
		//public IEnumerable<SpecialReport> allStaffLoanVms { get; set; }
		public IEnumerable<LoanRecoveryotherSalaryVm> loanRecoveryotherSalaryVms { get; set; }
		public IEnumerable<SpecialReportVm> specialReportVms { get; set; }
		public IEnumerable<SalariedLoanDeductionVm> salariedLoanDeductionVms { get; set; }
		public IEnumerable<SalarySummarySheetSpViewModel> salarySummarySheetSpViewModels { get; set; }
		public IEnumerable<StaffLoanEmployeesVm> staffLoanEmployees { get; set; }
		public BetonVataViewModel betonVataViewModel { get; set; }

		public IEnumerable<GetMobileOrCarAllowanceVm> mobileOrCarAllowanceVms { get; set; }
		public IEnumerable<BonusTaxReport> bonusTaxReports { get; set; }

	}

    public class BonusTaxReport
    {
        public int? employeeId { get; set; }
        public string SBAccount { get; set; }
        public int? salaryPeriodId { get; set; }
        public string monthName { get; set; }
        public string yearName { get; set; }
        public string employeeCode { get; set; }
        public string empName { get; set; }
        public string designation { get; set; }
        public string periodName { get; set; }
        public int? lockLabel { get; set; }
        public string sbuName { get; set; }
        public decimal? BonusAmount { get; set; }
        public decimal? Deduction { get; set; }
        public decimal? NetAmount { get; set; }
    }

	public class AllBranchSalarySheetVm {
		public string branchCode { get; set; }
		public string branchName { get; set; }
		public string SBAccount { get; set; }
		public int? salaryPeriodId { get; set; }
		public string periodName { get; set; }
		public string employeeCode { get; set; }
		public string nameEnglish { get; set; }
		public string designation { get; set; }
		public string lockLabel { get; set; }
		public DateTime? joiningDate { get; set; }
		public decimal? structureBasic { get; set; }
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
		public decimal? misc { get; set; }
		public decimal? PFSubscription { get; set; }
		public decimal? PfBankCont { get; set; }
		public decimal? PFAdvance { get; set; }
		public decimal? HouseBuilding { get; set; }
		public decimal? ComputerLoan { get; set; }
		public decimal? GratuityFundBank { get; set; }
		public decimal? GratuityFundDed { get; set; }
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
		public decimal? GROSS { get; set; }
		public decimal? TOTALPAIDBYBANK { get; set; }
		public decimal? TOTALDEDUCT { get; set; }
		public decimal? TOTALALLOWANCE { get; set; }
		public decimal? NET { get; set; }
		public decimal? SpecialIncentive { get; set; }

    }
	public class PaySlipApiVm
	{
		public IEnumerable<PayslipReportViewModel> payslipReportViewModels { get; set; }
		public IEnumerable<PayslipReportViewModelApi> payslipAdditionViewModels { get; set; }
		public IEnumerable<PayslipReportViewModelApi> payslipDeductionViewModels { get; set; }
	}
	public class StaffLoanEmployeesVm
	{
		public int? loanId { get; set; }
		public string empCode { get; set; }
		public string empName { get; set; }
		public string loanType { get; set; }
		public string loanNo { get; set; }
	}

	public class EmployeeInfoLoanVm
	{
		public string nameEnglish { get; set; }
		public string designation { get; set; }
		public string employeeCode { get; set; }
		public string mobilePersonal { get; set; }
	}

	public class EmployeeInfoLoanVmNew
	{
		public string nameEnglish { get; set; }
		public string designation { get; set; }
		public string employeeCode { get; set; }
		public string mobilePersonal { get; set; }
		public string postingPlace { get; set; }
		public string status { get; set; }
	}
	public class IndividualLoans
	{
		public EmployeeInfo employee { get; set; }
		public IEnumerable<IndLoanViewModel> loans { get; set; }
	}
	public class SalaryVoucherAllViewModel
	{
		public string heading { get; set; }
		public string accountCode { get; set; }
		public decimal? amount { get; set; }
		public string type { get; set; }
		public int? periodId { get; set; }
		public int? postingBranchId { get; set; }
		public string branchName { get; set; }
		public string branchCode { get; set; }
	}
	public class AllLoanViewModel
	{
		public int? designationCode { get; set; }
		public int? seniorityLevel { get; set; }
		public int? branchId { get; set; }
		public string employeeCode { get; set; }
		public string employeeName { get; set; }
		public string designationName { get; set; }
		public string loanType { get; set; }
		public decimal? principalAmount { get; set; }
		public decimal? interestAmount { get; set; }
		public decimal? chargeAmount { get; set; }
		public decimal? totalDisbursement { get; set; }
		public string LoanNo { get; set; }
		public decimal? totalAmount { get; set; }
		public DateTime? loanDate { get; set; }
		public int? departmentId { get; set; }
		public int? hrBranchId { get; set; }
		public int? zoneId { get; set; }
		public int? officeId { get; set; }
		public int? hrUnitId { get; set; }
		public int? hrDivisionId { get; set; }
		public IEnumerable<Department> departments { get; set; }
		public IEnumerable<ShiftGroupMaster> shifts { get; set; }
		public IEnumerable<EmployeeInfo> employeeInfosd { get; set; }
		public IEnumerable<EmployeeInfo> employeeInfoDepts { get; set; }
	}
	public class IndLoanViewModel
	{
		public int? designationCode { get; set; }
		public int? seniorityLevel { get; set; }
		public string loanType { get; set; }
		public string employeeCode { get; set; }
		public string employeeName { get; set; }
		public string designationName { get; set; }
		public string periodName { get; set; }
		public int? noOfInstallment { get; set; }
		public decimal? installmentAmount { get; set; }
		public decimal? restAmount { get; set; }
		public int? isComplete { get; set; }
		public int? loanId { get; set; }
		public DateTime? installmentDate { get; set; }
	}

	public class salaryAddDiductionViewModel
	{
		public int headId { get; set; }
		public string salaryHeadName { get; set; }
		public decimal TotalAddition { get; set; }
		public int sortOrder { get; set; }
		public string salaryHeadType { get; set; }
		public string accountNo { get; set; }


	}

    public class SalaryVoucherViewModel
    {
        public string heading { get; set; }
        public string accountCode { get; set; }
        public decimal? amount { get; set; }
        public string type { get; set; }
        public string branchName { get; set; }
        public int? periodId { get; set; }
        public int? postingBranchId { get; set; }
        public string branchType { get; set; }
        //public string bonusName { get; set; }
        //public string branchCode { get; set; }
    }
    public class BonusVoucherViewModel
    {
        public string heading { get; set; }
        public string type { get; set; }
        public string accountCode { get; set; }
        public decimal? amount { get; set; }
        public string branchName { get; set; }
        public string bonusName { get; set; }
        public string branchCode { get; set; }
    }
    public class GetMobileOrCarAllowanceVm
	{
		public string empCode { get; set; }
		public string empName { get; set; }
		public int? designationId { get; set; }
		public string designationCode { get; set; }
		public string designationName { get; set; }
		public string sbAccount { get; set; }
		public decimal? amount { get; set; }
		public decimal? Revenue { get; set; }
		public decimal? NetPay { get; set; }
		public string salaryHeadName { get; set; }
		public int? salaryPeriodId { get; set; }
		public string periodName { get; set; }
		public string monthName { get; set; }
	}
	public class StaffLoanViewModel
	{
		public int? staffLoanId { get; set; }

		public DateTime? effectiveDate { get; set; }
		public DateTime? createdAt { get; set; }

		public string loanType { get; set; }
		public string particular { get; set; }

		public decimal? debit { get; set; }
		public decimal? credit { get; set; }

		public decimal? principal { get; set; }
		public decimal? interest { get; set; }
		public decimal? charge { get; set; }

		public int? salaryPeriodId { get; set; }
		public string periodName { get; set; }
		public DateTime? periodLockDate { get; set; }

		public decimal? total { get; set; }
		public string posting { get; set; }
		public int? isActive { get; set; }
		public int? status { get; set; }
	}
	public class StaffLoanScheduleViewModel
	{
		public string empCode { get; set; }
		public string branchName { get; set; }
		public string empName { get; set; }
		public int? staffLoanId { get; set; }

		public DateTime? effectiveDate { get; set; }
		public DateTime? createdAt { get; set; }

		public string loanType { get; set; }
		public string particular { get; set; }

		public decimal? debit { get; set; }
		public decimal? credit { get; set; }

		public decimal? principal { get; set; }
		public decimal? interest { get; set; }
		public decimal? charge { get; set; }

		public int? salaryPeriodId { get; set; }
		public string periodName { get; set; }
		public DateTime? periodLockDate { get; set; }

		public decimal? total { get; set; }
		public string posting { get; set; }
		public int? isActive { get; set; }
		public int? status { get; set; }
	}
	public class StaffLoanAsOnViewModel
	{
		public string employeeName { get; set; }
		public string branchName { get; set; }
		public string designationName { get; set; }
		public string accountNo { get; set; }
		public string empCode { get; set; }
		public string loanType { get; set; }
		public decimal? interestRate { get; set; }
		public decimal? Principal { get; set; }
		public decimal? Interest { get; set; }
		public decimal? Charge { get; set; }
		public decimal? Total { get; set; }
	}

	public class BonusReportViewModel
	{
		public string employeeCode { get; set; }
		public string employeeName { get; set; }
		public string designationName { get; set; }
		public string designationCode { get; set; }
		public string accountNo { get; set; }
		public int? seniorityLevel { get; set; }
		public decimal? BasicSalary { get; set; }
		public decimal? BonusAmount { get; set; }
		public decimal? Revenue { get; set; }
		public decimal? TotalAmount { get; set; }
		public string branchName { get; set; }
	}
	public class BonusSummaryReportViewModel
	{
		public string designationName { get; set; }
		public int? designationCode { get; set; }
		public int? TotalEmp { get; set; }
		public decimal? Basic { get; set; }
		public decimal? GrossBonus { get; set; }
		public decimal? Rev { get; set; }
		public string branchName { get; set; }
		public string reportHead { get; set; }
	}
	public class BranchWiseBonusSummary
	{
		public int? Id { get; set; }
		public string branchCode { get; set; }
		public string branchName { get; set; }
		public string bonusName { get; set; }
		public string yearName { get; set; }
		public int? EmployeeCount { get; set; }
		public string branchType { get; set; }
		public int? salaryPeriodId { get; set; }
		public int? salaryHeadId { get; set; }
		public decimal? TotalBasic { get; set; }
		public decimal? TotalBonus { get; set; }
		public decimal? TotalRev { get; set; }
		public decimal? NetBonus { get; set; }
	}
	public class EmployeeLoans
	{
		public int? loanId { get; set; }
		public string loanName { get; set; }
		public string status { get; set; }
	}
	public class StaffLoanTransactions
	{
		public int? loanId { get; set; }
		public string newLoanNo { get; set; }
		public string loanType { get; set; }
		public string posting { get; set; }
		public DateTime? effectiveDate { get; set; }
		public string salaryPeriod { get; set; }
		public string particular { get; set; }
		public decimal? debit { get; set; }
		public decimal? credit { get; set; }
		public decimal? principal { get; set; }
		public decimal? interest { get; set; }
		public decimal? charge { get; set; }
		public decimal? total { get; set; }
		public decimal? openingbalance { get; set; }
		public decimal? closingbalance { get; set; }
		public int? creditcount { get; set; }
		public int? debitcount { get; set; }
        public decimal? actualOpening { get; set; }
    }

	public class StaffLoanInterestVm
	{
		public string employeeCode { get; set; }
		public string nameEnglish { get; set; }
		public string designationName { get; set; }
		public string NewLoanNo { get; set; }
		public string loanType { get; set; }
		public DateTime? effectiveDate { get; set; }
		public DateTime? loanDate { get; set; }
		public decimal? debit { get; set; }
		public int? hrBranchId { get; set; }
		public string branchName { get; set; }

	}
	public class StaffLoanInterestChargeVm
	{
		public string empCode { get; set; }
		public string employeeName { get; set; }
		public string loanType { get; set; }
		public decimal? interestRate { get; set; }
		public decimal? principal { get; set; }
		public decimal? interest { get; set; }
		public DateTime? effectiveDate { get; set; }
		public decimal? interestCharged { get; set; }

	}
	public class StaffLoanAllInterestVm
    {
        public string employeeCode { get; set; }
        public string nameEnglish { get; set; }
        public string designationName { get; set; }
        public string NewLoanNo { get; set; }
        public string loanType { get; set; }
        public DateTime? effectiveDate { get; set; }
        public DateTime? loanDate { get; set; }
        public decimal? interestCharged { get; set; }
        public decimal? principalTotal { get; set; }
        public decimal? interestTotal { get; set; }
        public decimal? total { get; set; }
        public int? hrBranchId { get; set; }
        public string branchCode { get; set; }
        public string designationCode { get; set; }
        public string desig { get; set; }
        public string branchName { get; set; }
        public int? seniorityLevel { get; set; }
        public string type { get; set; }

    }

    public class EmployeeInfoMinimum
    {
        public int? Id { get; set; }
        public string empName { get; set; }
        public string empCode { get; set; }
        public string designation { get; set; }
        public int? activityStatus { get; set; }
        public int? seniorityLevel { get; set; }
    }
	public class EmployeeInfoMinimumPF
	{
		public int? Id { get; set; }
		public string empName { get; set; }
		public string empCode { get; set; }
		public string designation { get; set; }
		public int? activityStatus { get; set; }
		public int? seniorityLevel { get; set; }
		public int? pfID { get; set; }
	}
	public class HeadOfficeWiseBonusSummary
    {
        public int? hrBranchId { get; set; }
        public int? salaryHeadId { get; set; }
        public int? salaryPeriodId { get; set; }
        public int? Id { get; set; }
        public int? EmployeeCount { get; set; }
        public string designationCode { get; set; }
        public string designationName { get; set; }
        public decimal? BasicAmount { get; set; }
        public decimal? BonusAmount { get; set; }
        public decimal? Revenue { get; set; }
        public decimal? NetBonus { get; set; }
        public string branchName { get; set; }
        public string bonusName { get; set; }
    }

	public class AllSheetBonusVm
	{
		public string employeeCode { get; set; }
		public string employeeName { get; set; }
		public string accountNo { get; set; }
		public string designationCode { get; set; }
		public string designationName { get; set; }
		public string branchCode { get; set; }
		public string branchName { get; set; }
		public int? branchId { get; set; }
		public int? salaryPeriodId { get; set; }
		public decimal? BasicSalary { get; set; }
		public decimal? BonusAmount { get; set; }
		public decimal? Revenue { get; set; }
		public decimal? TotalAmount { get; set; }
		public string bonusName { get; set; }
	}

	public class EmployeeFixatonAuto
	{
		public int? Id { get; set; }
		public string employeeCode { get; set; }
		public string nameEnglish { get; set; }
		public string nameBangla { get; set; }
		public string designationName { get; set; }
		public string shortName { get; set; }
		public int? activityStatus { get; set; }
		public int? salaryStatus { get; set; }
		public DateTime? joiningDate { get; set; }
		public DateTime? dateOfBirth { get; set; }
		public string placeOfPosting { get; set; }
		public decimal? currentBasic { get; set; }
		public int? currentGradeId { get; set; }
		public string currentGrade { get; set; }
		public int? hrBranchId { get; set; }
		public int? locationId { get; set; }
		public string branchName { get; set; }
		public DateTime? incrementDate { get; set; }
		public DateTime? promotionDate { get; set; }
		public DateTime? nextIncrementDate { get; set; }
	}
    public class AllStaffLoanVm
    {
        public string employeeCode { get; set; }
        public string nameEnglish { get; set; }
        public string shortName { get; set; }
        public string loanType { get; set; }
        public DateTime? effectiveDate { get; set; }
        public string particular { get; set; }
        public decimal? principal { get; set; }
        public decimal? interest { get; set; }
        public decimal? charge { get; set; }
        public decimal? total { get; set; }
        public int? Installment { get; set; }
    }
    public class SalariedLoanDeductionVm
    {
        public string periodName { get; set; }
        public string employeeCode { get; set; }
        public string employeeName { get; set; }
        public string shortName { get; set; }
        public string designationCode { get; set; }
        public int? senioritylevel { get; set; }

        public string loanType { get; set; }
        public decimal? Salary { get; set; }
        public decimal? Loan { get; set; }
       
    }
    public class LoanRecoveryotherSalaryVm
    {
        public string empCode { get; set; }
        public string empName { get; set; }
        public string designationEn { get; set; }
        public DateTime? effectiveDate { get; set; }
        public string particular { get; set; }
        public string loanType { get; set; }
        public decimal? credit { get; set; }


    }
    public class SpecialReportVm
    {
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string Designation { get; set; }
        public decimal? BasicPay { get; set; }
        public decimal? NetPay { get; set; }
        public decimal? Gratuity { get; set; }
        public decimal? PFBank { get; set; }
        public decimal? Bonus { get; set; }
        public decimal? lunch { get; set; }
        public decimal? mobileAllowance { get; set; }
        public string Year { get; set; }
        public string MonthName { get; set; }
       
        public decimal? Pension { get; set; }
    }


    public class BranchandZoneVm
    {
        public int? BranchandZoneId { get; set; }
        public string BranchandZoneName { get; set; }
        public string Type { get; set; }
    }

    public class SP_GetEmpOverTime
    {
        public int? employeeInfoId { get; set; }
        public string empCode { get; set; }
        public string empName { get; set; }
    }

    public class EmployeeInfoMinimumNew
    {
        public int? Id { get; set; }
        public string empName { get; set; }
        public string empCode { get; set; }
        public string designation { get; set; }
        public int? activityStatus { get; set; }
        public int? seniorityLevel { get; set; }
    }
}
