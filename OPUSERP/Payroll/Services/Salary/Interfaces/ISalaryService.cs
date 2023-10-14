using OPUSERP.Areas.HRPMSAttendence;
using OPUSERP.Areas.HRPMSAttendence.Models;
using OPUSERP.Areas.Payroll.Models;
using OPUSERP.Data.Entity.MasterData;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Wages;
using OPUSERP.Models;
using OPUSERP.Payroll.Data.Entity;
using OPUSERP.Payroll.Data.Entity.Arrear;
using OPUSERP.Payroll.Data.Entity.Loan;
using OPUSERP.Payroll.Data.Entity.PF;
using OPUSERP.Payroll.Data.Entity.Salary;
using OPUSERP.ProvidentFund.Data.Entity;
using OPUSERP.Utility.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Services.Salary.Interfaces
{
	public interface ISalaryService
	{
		Task<SalaryPeriod> GetSalaryPeriodById1(int PeriodId);
        Task<IEnumerable<SalaryPeriod>> GetSalaryPeriodByDateRange(DateTime fDate, DateTime tDate);
        Task<decimal> UpdateEmployeeStructureByBasic(int empId, decimal basic);
		Task<string> GetPostingByEmpId(int employeeId);
		Task<Location> GetZoneById(int zoneId);
        Task<StaffLoanLog> GetStaffLoanLogByLoanId(int loanId);
        Task<int> saveUploadLoanExcelLog(UploadExcelLog model);
        Task<IEnumerable<UploadExcelLog>> GetAllExcelUploadLogsByType(string type);
		Task<CBSProcessSp> ProcessLoanByType(string loanType, int periodId, string empCode);
        Task<IEnumerable<CBSProcessLogViewModel>> GetProcessedSalaryLog(int periodId);
        Task<ArrearAmountVm> GetArrearAmountByEmpCode(int periodId, string empCode);
        Task<IEnumerable<StaffLoanScheduleViewModel>> GetStaffLoanScheduleByDateRange(DateTime? startDate, DateTime? endDate, int hrBranchId, int zoneId, string empCode);
        Task<IEnumerable<StaffLoanEmployeesVm>> GetEmpInfoByLoan();
        Task<IEnumerable<StaffLoanTransactions>> GetStaffLoanTransactionsByDateRange(string empCode, int loanId, DateTime? startDate);
        Task<IEnumerable<StaffLoanTransactions>> GetStaffLoanTransactionsByFormDateToDate(string empCode, int loanId, DateTime? startDate, DateTime? endDate);       
        Task<IEnumerable<PRLEmployeeViewModel>> GetAllPrlEmployees();
        Task<IEnumerable<SalarySheetByYearMonthAndSBU>> sp_GetSalarySheetByYearMonthAndSBU(string year, string month, string sbu, string empId);
		Task<IEnumerable<NetPayByYearMonthAndSBU>> sp_GetNetPayByYearMonthAndSBU(string year, string month, string sbu, string empId);
		Task<IEnumerable<SalaryPeriod>> GetAllSalaryPeriodByUserId(int userid, string type);
        Task<IEnumerable<EmployeeInfo>> GetIndividualEmployeeFromSalaryStructure(int branchId, int departmentId, int designationId);
        Task<IEnumerable<SalaryVoucherViewModel>> GetSalaryVoucherByPeriodId(int periodId, int id, string type);
		Task<IEnumerable<GetMobileOrCarAllowanceVm>> GetMobileOrCarAllowanceVms(int periodId, string headName);
		Task<IEnumerable<SalaryVoucherAllViewModel>> GetSalaryVoucherAllByPeriodId(int periodId, string type);
		Task<IEnumerable<SalaryVoucherViewModel>> GetSalaryVoucherByPeriodIdAndSbu(int periodId, string sbu, string type);
		Task<IEnumerable<PostSalary>> sp_SaveVoucherDataByBranchId(int periodId, int branchId);
		Task<IEnumerable<PostedVoucherVm>> sp_GetPostedVoucher(int periodId, int branchId);
        Task<int> DeleteStaffLoanLogsByLoanId(int id);
        Task<int> DeleteStaffLoanByLoanId(int id);
		Task<IEnumerable<BranchWiseBonusSummary>> GetBonusSummaryDataByBranch(int periodId, string type);
		Task<LoanNumberGenerate> DeleteStaffLoanMasterDetail(int loanId);

		Task<LoanPaid> IsFullPaid(int loanId);
		Task<LoanPaid> CloseFullPaidStaffLoan(int loanId);
        
        Task<IEnumerable<FilterStaffLoanViewModel>> FilterStaffLoan(int branchId, int zoneId, int empId, string loanType);
        Task<string> GetTypeName(string type, int id);
        Task<decimal> GetBasicCalculation(int empId, decimal basic);
		Task<decimal> GetBasicCalculationForArrear(int empId, decimal basic, decimal total);
        Task<SalaryProcessDataViewModel> UpdatePfTransactionByPeriodId(int salaryPeriodId);
        Task<IEnumerable<SalaryStatusLog>> GetLockStatusLogs();
        Task<IEnumerable<SalaryProcessDataViewModel>> ZoneWiseEmpSalaryProcess(int salaryPeriodId, int locationId);
        Task<decimal> GetBasicFromSalaryStructure(int empId);
        Task<IEnumerable<Location>> GetAllZones();
		Task<IEnumerable<SpecialBranchUnit>> GetAllSbus();
        Task<CBSProcessSp> CBSProcessAllVoucher(int periodId);
        Task<CBSProcessSp> CBSProcessAllSalary(int periodId);
        Task<int> SaveStaffLoanErrors(StaffLoanUploadFailed model);
        Task<StaffLoan> GetLoanNoByLoanId(int loanId);
        Task<IEnumerable<BranchMonthlySalaryReportViewModel>> GetMonthlySalaryReportByPeriodIdZoneNew(int salaryPeriodId, int? unitId, int? pnsId);
        Task<decimal> GetPFSubsByEmpId(int empId, decimal basic, string pfType, int? pfPercentage);
        Task<EmployeesSalaryStructure> GetSalaryStructureByEmpAndHeadId(int empId, int headId);
        Task<IEnumerable<BranchMonthlySalaryReportViewModel>> GetMonthlySalaryReportByPeriodIdOfficeNew(int salaryPeriodId, int? officeId, int? pnsId);
        Task<IEnumerable<BranchMonthlySalaryReportViewModel>> GetMonthlySalaryReportByPeriodIdUnitNew(int salaryPeriodId, int? unitId, int? pnsId);
        Task<IEnumerable<salaryAddDiductionViewModel>> GetSalaryAddiDidinfoByPeriodNbranchId(int salaryPeriodId, int branchId);
		Task<IEnumerable<BranchMonthlySalaryReportViewModel>> GetMonthlySalaryReportByPeriodIdDeptNew(int salaryPeriodId, int? departmentId, int? pnsId);
        Task<IEnumerable<EmployeeLoans>> GetLoansByEmployeeCode(string empCode);
        Task<IEnumerable<EmployeeInfoMinimum>> GetEmployeesAll();
        Task<IEnumerable<SalaryLedgerViewModel>> GetSalaryLedgerReports(int? salaryPeriodId, int? salaryHeadId);
        Task<IEnumerable<SalaryLedgerViewModel>> GetSalaryNetPayReports(int? salaryPeriodId);
        Task<int> SaveLoanPolicyDetail(LoanPolicyDetail loanPolicyDetail);
        Task<int> DeleteLoanPolicyDetailsByMasterId(int id);
		Task<int> ProcessStaffLoanInterest(DateTime? processDate, int hrBranchId, string type);
		Task<int> ProcessStaffLoanInterestIndividual(DateTime? processDate, string empCode, string type);
		Task<PartialResultViewModel> SavePartialSalary(int type, int empId, int periodId, string fromDate, string toDate, int days);
		Task<IEnumerable<PartialSalaryLog>> GetAllPartialSalaryLogs();
		Task<PartialResultViewModel> DeletePartialSalary(int empId, int periodId);
		Task<IEnumerable<PartialSalaryLogViewModel>> GetAllPartialSalaryLog();
		Task<IEnumerable<SalaryHead>> GetAllSalaryHeadForStruct();
		Task<GetLoansByEmpCodeAndTypeViewModel> GetLoansByEmpCodeAndType(string code, string type);
        Task<IEnumerable<StaffLoanAsOnViewModel>> GetStaffLoanBalanceAsOn(string asOndate, int hrBranchId);
        Task<IEnumerable<StaffLoanAsOnViewModel>> GetStaffLoanBalanceAsOn2(string asOndate, int hrBranchId,string zoneorBranch);
		Task<IEnumerable<StaffLoanAsOnViewModel>> GetStaffLoanBalanceAsOnByType(string asOndate, int hrBranchId,string type);
		Task<IEnumerable<BonusReportViewModel>> GetBonusData(int periodId, int hrBranchId, int zoneId);
        Task<IEnumerable<BonusReportViewModel>> GetBonusSundryData(int periodId, int hrBranchId, int zoneId);
        Task<IEnumerable<AllSheetBonusVm>> GetAllBonusData(int periodId, string type);
		Task<int> SaveCarMaintenance(CarMaintenance model);
		Task<int> SaveMobileAllowanceStructure(MobileAllowanceStructure model);
		Task<IEnumerable<EmployeeInfoCarMaintanceVm>> GetEmployeeInfoById(int id);
		Task<IEnumerable<CarMaintenance>> GetAllCarMaintenance();
		Task<IEnumerable<MobileAllowanceStructure>> GetAllMobileAllowanceStructure();
		Task<int> DeleteCarMaintenance(int id);
		Task<int> DeleteMobileAllowanceStructure(int id);
		Task<IEnumerable<PFYearlyIndividualVm>> GetIndividualYearlyPF(int pfMemberId, string year);
		Task<bool> SaveEmployeeArrearInfo(EmployeeArrearInfo employeeArrear);
		Task<IEnumerable<EmployeeArrearInfo>> GetAllEmployeeArrearInfo();
		Task<int> DeleteArrearInfoById(int id);
        Task<SalaryPeriod> GetSalaryPeriodByYearAndMonth(int year, string month);
		Task<EmployeesSalaryStructure> GetPFByEmployeeId(int empId);
        Task<IEnumerable<LoanNumberGenerate>> DeleteExistingStaffLoan(int empId, string loanType, string CBSAc, string LoanAc);
        Task<LoanNumberGenerate> GetStaffLoanNo(string empCode, string loanType);
        Task<StaffLoan> GetStaffLoanByEmpIdAndType(int empId, string type);
        Task<string> GetHeadNameById(int? id);
		Task<IEnumerable<StaffLoan>> GetAvailableLoans(string empcode);
		Task<GenerateTrxNumberForLoan> GenerateLoanTransactionNo(string type, string loanName);
		Task<string> GetLoanNameById(int id);
		Task<IEnumerable<StaffLoanScheduleViewModel>> GetStaffLoanRecoveryScheduleByDateRange(DateTime? startDate, DateTime? endDate, int hrBranchId, int zoneId, string empCode, string loanType);
		Task<StaffLoanInfoViewModel> GetStaffLoanInfoExpSelfById(string transactionNo, int loanId);
		Task<StaffLoanInfoViewModel> GetStaffLoanInfoById(int loanId);
		Task<IEnumerable<SBSReportViewModel>> GetSBSReportAsOn(string startDate, string endDate, string loanType);
		Task<IEnumerable<BonusVoucherViewModel>> GetBonusVoucherByPeriodId(int periodId, int hrBranchId, int zoneId);
		Task<IEnumerable<BonusVoucherViewModel>> GetBonusVoucherByPeriodIdAndType(int periodId, string type);
		Task<IEnumerable<BonusSummaryReportViewModel>> GetBonusSummaryData(int periodId, int hrBranchId, int zoneId);
        Task<IEnumerable<SalaryCertificateReportViewModel>> GetSalaryCertificateByEmpId(int employeeInfoId, int salaryPeriodId);
		Task<BetonVataViewModel> GetBetonVataAll(DateTime fDate, DateTime tDate);
        Task<ResolveAllVm> ResolveStructureWithLoan();
        //Asad added on 08.06.2023
        Task<ResolveAllVm> SetAllToSalaryStructure(int salaryPeriodId);
        Task<ResolveAllVm> ProcessPartialHouseRent(int periodId);
        Task<IEnumerable<PartialHouseRentVm>> GetPartialHouseRent(int periodId);
        Task<IEnumerable<AllStaffLoanVm>> GetaLLStaffLoanByDateRange(DateTime? startDate, DateTime? endDate);
        Task<IEnumerable<SpecialReportVm>> GetSecuritiesLtdReprotByTypeAndPeriod(string type, int periodId);
        Task<IEnumerable<IndivisualEmpSalaryReportViewModel>> GetIndivisualSalaryReportByEmpIdandDate(string empCode, DateTime fDate, DateTime tDate);
		Task<IEnumerable<IndivisualEmpSalaryReportViewModel>> GetIndivisualSalaryReportByDesigIdandDate(int desigId, DateTime fDate, DateTime tDate);

		Task<IEnumerable<BonusTaxReport>> GetBonusTaxReportByEmpCode(string empCode, DateTime fDate, DateTime tDate);
		Task<IEnumerable<BonusTaxReport>> GetBonusTaxReportByDesig(int desigId, DateTime fDate, DateTime tDate);

		Task<IEnumerable<ReconciallationVm>> GetReconcillationData(string particular, string fDate, string tDate);
		Task<IEnumerable<QuarterlyVm>> GetQuarterlyReport(string type, string fDate, string tDate);

        Task<IEnumerable<StaffLoan>> GetAllStaffLoansByDate(DateTime? fromDate, DateTime toDate);
		Task<StaffLoanLog> GetStaffLoanLogById(int id);

		Task<IEnumerable<StaffLoanLog>> GetStaffLoansLogAllByDate(DateTime? fromDate, DateTime toDate);
		Task<IEnumerable<StaffLoanLog>> GetStaffLoanLogByTrxNo(string trxNo);
		Task<IEnumerable<StaffLoanLog>> GetStaffLoanLog();

		Task<IEnumerable<StaffLoanLog>> GetAllStaffLoansLog();
        #region Gratuity process
        Task<bool> SaveSendEmailLogStatus(SendEmailLogStatus sendmail);
		Task<IEnumerable<SendEmailLogStatus>> GetSendEmailLogStatus();

		Task<bool> SaveGratiutyProcessData(GratiutyProcessData salaryYear);
		Task<IEnumerable<GratiutyProcessData>> GetAllGratiutyProcessData();
		Task<IEnumerable<GratuityReportViewModel>> GetAllGratuityReportViewModel();
		List<DateTime?> GetAllGratiutyProcessDataDates();
		Task<IEnumerable<GratuityReportViewModel>> GetAllGratuityReportViewModelByDate(DateTime date);
		#endregion

		#region Salary Year

		Task<bool> SaveSalaryYear(SalaryYear salaryYear);
		Task<IEnumerable<SalaryYear>> GetAllSalaryYear();
		Task<SalaryYear> GetSalaryYearById(int id);
		Task<bool> DeleteSalaryYearById(int id);
		#endregion

		#region Employee Tax
		Task<bool> SaveEmployeesTax(EmployeesTax employeesTax);
		Task<IEnumerable<EmployeesTax>> GetAllEmployeesTax();
		Task<EmployeesTax> GetEmployeesTaxById(int id);
		Task<bool> DeleteEmployeeTaxById(int id);
		Task<bool> UpdateEmployeesStatus(int? id, int taxyearid);
		#endregion

		#region Salary Grade
		Task<bool> SaveSalaryGrade(SalaryGrade orga);
		Task<IEnumerable<SalaryGrade>> GetAllSalaryGrade();
		Task<SalaryGrade> GetSalaryGradeById(int id);
		Task<bool> DeleteSalaryGradeById(int id);
		#endregion

		#region Tax Year
		Task<bool> SaveTaxYear(TaxYear taxYear);
		Task<IEnumerable<TaxYear>> GetAllTaxYear();
		Task<bool> UpdateTaxInSalaryall(int PeriodId);
		Task<bool> UpdateTaxInSalaryallstruc();
		Task<bool> UpdateTaxInSalary(int PeriodId, int employeeId, int Id);
		Task<bool> UpdateTaxInstruc(int employeeId, int Id);
		Task<TaxYear> TaxYearbyId(int id);
		#endregion

		#region Tax slab Type
		Task<bool> SaveTaxSlab(SlabType slabType);
		Task<IEnumerable<SlabType>> GetAllSlabType();
		#endregion

		#region Rebate slab Type
		Task<bool> SaveRebateSlab(RebateSlabType rebateSlabType);
		Task<IEnumerable<RebateSlabType>> GetAllRebateSlabType();
		#endregion

		#region Rebate Setting
		Task<bool> SaveRebateSetting(InvestmentRebateSettings investmentRebateSettings);
		Task<IEnumerable<InvestmentRebateSettings>> GetAllRebateSetting();
		Task<IEnumerable<InvestmentRebateSettings>> GetAllRebateSettingbytaxyearid(int Id);
		#endregion

		#region Salary Head
		Task<bool> SaveSalaryHead(SalaryHead salaryHead);
		Task<IEnumerable<SalaryHead>> GetAllSalaryHead();
		Task<IEnumerable<SalaryHead>> GetAllSalaryHeadByFilter(string filter);
		Task<bool> DeleteSalaryHeadById(int id);
		#endregion

		#region Salary Type
		Task<bool> SaveSalaryType(SalaryType salaryType);
		Task<IEnumerable<SalaryType>> GetAllSalaryType();
		#endregion

		#region Bonus Type
		Task<bool> SaveBonusType(BonusType bonusType);
		Task<IEnumerable<BonusType>> GetAllBonusType();
		#endregion

		#region Bonus Category

		Task<bool> SaveBonusCategory(BonousCategory bonusCategory);
		Task<IEnumerable<BonousCategory>> GetAllBonusCategory();
		Task<bool> DeleteBonusCategoryById(int id);

		#endregion

		#region Bonus Sub Category

		Task<bool> SaveBonusSubCategory(BonousSubCategory bonusSubCategory);
		Task<IEnumerable<BonousSubCategory>> GetAllBonusSubCategory();
		Task<IEnumerable<BonousSubCategory>> GetBonusSubCategoryByMasterId(int masterId);
		Task<bool> DeleteBonusSubCategoryById(int id);

		#endregion

		#region Bonous Structure

		Task<int> SaveBonousStructure(BonousStructure bonousStructure);
		Task<IEnumerable<BonousStructure>> GetAllBonousStructure();
		Task<bool> DeleteBonousStructureById(int id);

		#endregion

		#region Employees Bonous Structure

		Task<int> SaveEmployeesBonusStructure(EmployeesBonusStructure bonousStructure);
		Task<IEnumerable<EmployeesBonusStructure>> GetEmployeesBonusStructureByBonusId(int bonusId);
		Task<bool> DeleteEmployeesBonusStructureBybonusId(int id);

		#endregion

		#region Salary Calulation Type
		Task<bool> SaveSalaryCalulationType(SalaryCalulationType salaryCalulationType);
		Task<IEnumerable<SalaryCalulationType>> GetAllSalaryCalulationType();
		#endregion

		#region Wallet Type
		Task<bool> SaveWalletType(WalletType walletType);
		Task<IEnumerable<WalletType>> GetAllWalletType();
		Task<bool> DeleteWalletTypeById(int id);
		#endregion

		#region EmployeesCashSetup
		Task<int> SaveEmployeesCashSetup(EmployeesCashSetup employeesCashSetup);
		Task<IEnumerable<EmployeesCashSetup>> GetAllEmployeesCashSetup();
		Task<IEnumerable<EmployeesCashSetup>> GetEmployeesCashSetupByEmployeeId(int empId);
		Task<bool> DeleteEmployeesCashSetupById(int id);
		#endregion

		#region Salary Period
		Task<int> SaveSalaryPeriod(SalaryPeriod salaryPeriod);
		Task<DateTime> GetDisburseDate(int yearid, string monthname);
		Task<IEnumerable<SalaryPeriod>> GetAllSalaryPeriod();
		Task<IEnumerable<SalaryPeriod>> GetAllSalaryPeriodbyid(int userid);
		Task<SalaryPeriod> GetSalaryPeriodById(int PeriodId);
		Task<IEnumerable<SalaryPeriod>> GetSalaryPeriods();
		Task<IEnumerable<SalaryPeriod>> GetDuplicateSalaryPeriodById(int PeriodId, int yearId, int typeId, string month);
		Task<bool> EditSalaryPeriodForlockLabel(int Id, int lockLabel);
		Task<bool> SetSalaryPeriodLock(int Id, int lockLabel, string lockBy);
		Task<SalaryPeriod> GetSalaryPeriodNameById(int periodId);
		Task<SalaryPeriod> GetSalaryPeriodmax();
		Task<bool> DeleteSalaryPeriodById(int id);
		Task<IEnumerable<PayslipReportViewModelApi>> GetPaySlipAdditionByEmpIdApi(int employeeInfoId, int salaryPeriodId);
		Task<IEnumerable<PayslipReportViewModelApi>> GetPaySlipDeductionByEmpIdApi(int employeeInfoId, int salaryPeriodId);
		//new
		Task<IEnumerable<SalaryPeriod>> GetSalaryPeriodByName(int periodName);
		Task<IEnumerable<CheckLoansVm>> CheckLoans();
        Task<IEnumerable<ParticularNamesVm>> GetAllParticulars();
        #endregion

        #region Salary Slab
        Task<bool> SaveSalarySlab(SalarySlab salarySlab);
		Task<IEnumerable<SalarySlab>> GetAllSalarySlab();
		Task<IEnumerable<SalarySlab>> GetSalarySlabBysalaryGradeId(int salaryGradeId);
		Task<SalarySlab> GetSalarySlabById(int Id);
		#endregion

		#region Salary Grade Percent
		Task<bool> SaveSalaryGradePercent(SalaryGradePercent salaryGradePercent);
		Task<IEnumerable<SalaryGradePercent>> GetAllSalaryGradePercent();
		Task<SalaryGradePercent> GetSalaryGradePercentBysalaryHeadId(int salaryGradeId, int salaryHeadId);
		//salaryGrade
		Task<IEnumerable<SalaryGradePercent>> GetSalaryHeadBysalaryGradeId(int salaryGradeId);
		#endregion

		#region Employees Salary Structure
		Task<bool> SaveEmployeesSalaryStructure(EmployeesSalaryStructure employeesSalaryStructure);
		Task<IEnumerable<EmployeesSalaryStructure>> GetAllEmployeesSalaryStructure();
		Task<IEnumerable<EmployeesSalaryStructure>> GetEmployeesSalaryStructureByEmpId(int empId);
		Task<EmployeesSalaryStructure> GetEmpStructureByEmpId(int empId);
		Task<bool> DeleteEmployeesSalaryStructureByempId(int empId);
		Task<bool> EditEmployeesSalaryStructure(int Id, decimal amount, string status);
		Task<IEnumerable<FsSalaryStructureViewModel>> GetFsStructure(int empId, int day);
		#endregion

		#region Salary Structure History

		Task<IEnumerable<SalaryProcessDataViewModel>> SaveStructureHistory(int employeeInfoId, string changeBy);
		Task<IEnumerable<SalaryProcessDataViewModel>> UpdateStructureHistory(int structureId, string changeBy);
		Task<IEnumerable<System.Object>> GetStructureHistoryByEmpId(int empId);

		#endregion

		#region Wages Salary Structure
		Task<bool> SaveWagesSalaryStructure(WagesSalaryStructure wagesSalaryStructure);
		Task<IEnumerable<WagesSalaryStructure>> GetAllWagesSalaryStructure();
		Task<IEnumerable<WagesSalaryStructure>> GetWagesSalaryStructureByEmpId(int empId);
		Task<bool> DeleteWagesSalaryStructureByempId(int empId);
		Task<bool> EditWagesSalaryStructure(int Id, decimal amount, string status);
		Task<bool> DeleteWagesSalaryStructureById(int Id);
		#endregion

		#region Process Emp Salary Structure
		Task<bool> SaveProcessEmpSalaryStructure(ProcessEmpSalaryStructure processEmpSalaryStructure);
		Task<IEnumerable<ProcessEmpSalaryStructure>> GetProcessEmpSalaryStructureByemployeeInfoId(int salaryPeriodId, int employeeInfoId);
		Task<IEnumerable<ProcessEmpSalaryStructure>> GetProcessEmpSalaryStructureBysalaryPeriodId(int salaryPeriodId);
		Task<bool> DeleteProcessEmpSalaryStructureByempId(int empId, int salaryPeriodId);
		Task<bool> DeleteProcessEmpSalaryStructureBysalaryPeriodId(int salaryPeriodId);
		Task<decimal> GetNetPayableByemployeeInfoId(int employeeInfoId, int salaryPeriodId);
		Task<bool> EditProcessEmpSalaryStructureForLeaveWithoutPay(int employeeInfoId, int salaryPeriodId, int noOfDays);
		Task<bool> EditProcessEmpSalaryStructureForAdvanceAdjustment(int employeeInfoId, int salaryPeriodId, int salaryHeadId, decimal amount);
		Task<bool> EditProcessEmpSalaryStructureForEmployeeArrear(int employeeInfoId, int salaryPeriodId, decimal totalamount, decimal amount);
		#endregion

		#region Process Emp Salary Master
		Task<bool> SaveProcessEmpSalaryMaster(ProcessEmpSalaryMaster processEmpSalaryMaster);
		Task<IEnumerable<ProcessEmpSalaryMaster>> GetProcessEmpSalaryMasterByemployeeInfoId(int salaryPeriodId, int employeeInfoId);
		Task<IEnumerable<ProcessEmpSalaryMaster>> GetProcessEmpSalaryMasterBysalaryPeriodId(int salaryPeriodId);
		Task<bool> DeleteProcessEmpSalaryMasterByempId(int empId, int salaryPeriodId);
		Task<bool> DeleteProcessEmpSalaryMasterBysalaryPeriodId(int salaryPeriodId);
		Task<IEnumerable<SalaryProcessDataViewModel>> ProcessEmpSalaryMasterBySp(int? salaryPeriodId, int? employeeInfoId);
		#endregion

		#region Salary Process Log
		Task<bool> SaveSalaryProcessLog(SalaryProcessLog salaryProcessLog);
		Task<IEnumerable<SalaryProcessLog>> GetAllSalaryProcessLog();
		#endregion

		#region Salary Status Log
		Task<int> SaveSalaryStatusLog(SalaryStatusLog salaryStatusLog);
		Task<IEnumerable<SalaryStatusLog>> GetSalaryStatusLogByPeriodId(int periodId);
		#endregion

		#region Process Salary Remarks

		Task<int> SaveProcessSalaryRemarks(ProcessSalaryRemarks processSalaryRemarks);
		Task<bool> DeleteProcessSalaryRemarks(int? empId, int? periodId);
		#endregion

		#region Leave Without Pay
		Task<bool> SaveLeaveWithoutPay(LeaveWithoutPay leaveWithoutPay);
		Task<IEnumerable<LeaveWithoutPay>> GetAllLeaveWithoutPay();
		Task<LeaveWithoutPay> GetLeaveWithoutPayById(int id);
		Task<bool> DeleteLeaveWithoutPayById(int id);
		Task<IEnumerable<LeaveWithoutPay>> GetLeaveWithoutPayBysalaryPeriodId(int salaryPeriodId);
		Task<IEnumerable<LeaveWithoutPay>> GetLeaveWithoutPayByemployeeInfoId(int salaryPeriodId, int employeeInfoId);

		#endregion

		#region Employee Arrear
		Task<bool> SaveEmployeeArrear(EmployeeArrear employeeArrear);
		Task<IEnumerable<EmployeeArrear>> GetAllEmployeeArrear();
		Task<IEnumerable<EmployeeArrear>> GetEmployeeArrearBysalaryPeriodId(int salaryPeriodId);
		Task<IEnumerable<EmployeeArrear>> GetEmployeeArrearByEmpAndPeriodId(int empId, int periodId);
		Task<EmployeeArrear> GetEmployeeArrearById(int id);
		Task<bool> DeleteEmployeeArrearByEmpAndPeriodId(int empId, int periodId);
		Task<IEnumerable<EmployeeArrear>> EmployeeArrearCalculationByEmpId(int empId, int periodId, decimal? totalAmount, decimal? bonusAmount);
		#endregion

		#region Advance Adjustment
		Task<int> SaveAdvanceAdjustment(AdvanceAdjustment advanceAdjustment);
		Task<IEnumerable<AdvanceAdjustment>> GetAllAdvanceAdjustment();
		Task<AdvanceAdjustment> GetAdvanceAdjustmentById(int id);
		Task<bool> DeleteAdvanceAdjustmentById(int id);
		Task<IEnumerable<AdvanceAdjustment>> GetAdvanceAdjustmentBysalaryPeriodId(int salaryPeriodId);
		Task<IEnumerable<AdvanceAdjustment>> GetAdvanceAdjustmentByemployeeInfoId(int salaryPeriodId, int employeeInfoId);

		Task<bool> SaveAdvanceAdjustmentDetail(AdvanceAdjustmentDetail advanceAdjustment);
		Task<IEnumerable<AdvanceAdjustmentDetail>> GetAllAdvanceAdjustmentDetailByMasterId(int id);
		void UpdateAdvanceAdjustmentDetail(int docId, decimal? amount);

		#endregion

		#region Loan Schedule

		Task<int> SaveLoanScheduleMaster(LoanScheduleMaster advanceAdjustment);
		Task<bool> SaveLoanScheduleDetail(LoanScheduleDetail advanceAdjustment);
		Task<IEnumerable<LoanScheduleMaster>> GetAllLoanScheduleMaster();
		Task<IEnumerable<LoanScheduleMaster>> GetAllLoanScheduleMasterByEmpId(int? empId);
		Task<IEnumerable<LoanScheduleDetail>> GetAllLoanScheduleDetailByMasterId(int id);
		Task<bool> DeleteLoanScheduleDetailById(int id);
		void UpdateLoanScheduleDetail(int docId, decimal? amount);
		Task<bool> UpdateLoanScheduleMasterApproval(int Id, int status);
		Task<IEnumerable<LoanScheduleReportViewModel>> GetLoanReportById(int masterId);

		#endregion

		#region Loan Policy

		Task<bool> SaveLoanPolicy(LoanPolicy loanPolicy);
		Task<IEnumerable<LoanPolicy>> GetAllLoanPolicy();
		Task<LoanPolicy> GetLoanPolicyById(int id);
		Task<bool> DeleteLoanPolicyById(int id);
		Task<IEnumerable<AllLoanViewModel>> GetAllLoanByTypeAndId(string type, int id);
		Task<IEnumerable<IndLoanViewModel>> GetIndividualLoanByEmpId(int empId);
		int LoanDeductionByPeriodId(int salaryPeriodId);
		#endregion

		#region Loan Route

		Task<bool> SaveLoanRoute(LoanRoute loanRoute);
		Task<IEnumerable<LoanRoute>> GetLoanRouteByEmpId(int empId);
		Task<LoanRoute> GetLoanRouteById(int id);
		Task<bool> UpdateLoanRoute(int Id, int Type);
		Task<LoanRoute> GetLoanRouteByRouteOrder(int id, int order);

		#endregion

		#region Monthly Allowance
		Task<bool> SaveMonthlyAllowance(MonthlyAllowance monthlyAllowance);
		Task<IEnumerable<MonthlyAllowance>> GetAllMonthlyAllowance();
		Task<MonthlyAllowance> GetMonthlyAllowanceById(int id);
		Task<bool> DeleteMonthlyAllowanceById(int id);
		Task<IEnumerable<MonthlyAllowance>> GetMonthlyAllowanceBysalaryPeriodId(int salaryPeriodId);
		Task<IEnumerable<MonthlyAllowance>> GetMonthlyAllowanceByemployeeInfoId(int salaryPeriodId, int employeeInfoId);
		Task<IEnumerable<MealChargeViewModel>> GetMealChargeByPeriod(int salaryPeriodId);
		#endregion

		#region Attendance Summary

		Task<int> SaveAttendanceSummary(EmpAttendanceSummary monthlyAllowance);
		Task<IEnumerable<EmpAttendanceSummary>> GetAttendanceSummary();
		Task<EmpAttendanceSummary> GetAttendanceSummaryById(int id);
		Task<bool> DeleteAttendanceSummaryById(int id);
		Task<IEnumerable<EmpAttendanceSummary>> GetAttendanceSummaryBysalaryPeriodId(int salaryPeriodId);
		Task<IEnumerable<EmpAttendanceSummary>> GetDuplicateAttendanceSummary(int id, int salaryPeriodId, int employeeInfoId);
		Task<IEnumerable<Areas.HRPMSAttendence.Models.AttendanceSummaryViewModel>> GetAttendanceSummaryByPeriod(int? id, int? salaryPeriodId);

		//Task<IEnumerable<ManualAttendanceViewModel>> GetManualAttendanceByAnyKey(string employeeCode, int? approverId, string fromDate, string toDate);
		#endregion

		#region Lfa Info
		Task<bool> SaveLfaInfo(LfaInfo lfaInfo);
		Task<IEnumerable<LfaInfo>> GetAllLfaInfo();
		Task<LfaInfo> GetLfaInfoByEmpId(int empId);
		Task<bool> DeleteLfaInfoById(int id);
		Task<ProcessEmpSalaryStructure> GetLastYearBasic(int empId, int periodId);
		#endregion

		#region Payroll Report
		Task<IEnumerable<PayslipReportViewModel>> GetPaySlipByEmpId(int employeeInfoId, int salaryPeriodId);
		Task<IEnumerable<PayslipReportViewModel>> GetPaySlipAdditionByEmpId(int employeeInfoId, int salaryPeriodId);
		Task<IEnumerable<PayslipReportViewModel>> GetPaySlipDeductionByEmpId(int employeeInfoId, int salaryPeriodId);
		Task<IEnumerable<MonthlySalaryReportViewModel>> GetMonthlySalaryReportByPeriodId(int salaryPeriodId, int? sbuId, int? pnsId);
        Task<IEnumerable<MonthlySalaryReportViewModel>> GetMonthlySalaryReportByPeriodIdDept(int salaryPeriodId, int? departmentId, int? pnsId);
        Task<IEnumerable<MonthlySalaryReportViewModel>> GetMonthlySalaryReportByPeriodIdBranch(int salaryPeriodId, int? departmentId, int? pnsId);
        Task<IEnumerable<BranchMonthlySalaryReportViewModel>> GetMonthlySalaryReportByPeriodIdBranchNew(int salaryPeriodId, int? hrBranchId, int? pnsId);

        Task<IEnumerable<BankStatementReportViewModel>> GetBankStatementByPeriodId(int salaryPeriodId, int? sbuId, int? pnsId);
		Task<IEnumerable<GratuityReportViewModel>> GetGratuityReport();
		Task<IEnumerable<ReconciliationReportViewModel>> GetReconcilationStatement(int? empId, int? salaryPeriodId, int? presalaryPeriodId, int? typeId);
		Task<IEnumerable<EmpTaxDetailsViewModel>> GetEmpTaxDetails(int employeeInfoId, int taxYearId);
		Task<IEnumerable<EmpTaxSlabViewModel>> GetEmpTaxSlab(int employeeInfoId, int taxYearId);
		Task<IEnumerable<EmpRebatableTaxViewModel>> GetEmpRebatableTax(int employeeInfoId, int taxYearId);
		Task<IEnumerable<EmpTaxDeductFinalViewModel>> GetEmpTaxDeductFinal(int employeeInfoId, int taxYearId);
		Task<IEnumerable<TaxableamountViewModel>> TaxableamountViewModels(int employeeInfoId, int taxYearId);
		Task<IEnumerable<TaxableSlabViewModel>> TaxableslabViewModels(int employeeInfoId, int taxYearId);
		Task<IEnumerable<TaxablePFViewModel>> TaxablePFViewModels(int employeeInfoId, int taxYearId);
		Task<IEnumerable<EmployeesTax>> TaxCalculateforall(int taxYearId);
        Task<IEnumerable<Designation>> GetAllDesignations();
		Task<EmployeeInfo> GetDesignationByEmployeeId(int employeeId);

        Task<IEnumerable<AllBranchSalarySheetVm>> GetAllSalarySheetVms(int periodId, string type);
		#endregion

		#region Advance Payment
		Task<bool> SaveAdvancePayment(AdvancePayment advancePayment);
		Task<IEnumerable<AdvancePayment>> GetAllAdvancePayment();
		Task<IEnumerable<AdvancePayment>> GetAdvancePaymentBysalaryPeriodId(int salaryPeriodId);
		Task<IEnumerable<AdvancePayment>> GetAdvancePaymentByadvanceAdjustmentId(int advanceAdjustmentId);
		Task<IEnumerable<AdvancePayment>> GetAdvancePaymentByemployeeInfoId(int salaryPeriodId, int employeeInfoId);
		Task<AdvancePayment> GetAdvancePaymentById(int id);
		Task<bool> DeleteAdvancePaymentBysalaryPeriodId(int salaryPeriodId);
		Task<bool> DeleteAdvancePaymentByemployeeInfoId(int employeeInfoId, int salaryPeriodId);

		#endregion

		#region reportformat
		Task<bool> SaveReportFormat(ReportFormat reportFormat);
		Task<IEnumerable<ReportFormat>> GetReportFormat();
		Task<IEnumerable<ReportFormat>> GetReportFormatByReportType(string reportType);
		Task<bool> DeleteformatById(string reportType);
		Task<IEnumerable<UniversalCodaXLTempleteViewModel>> GetUniversalCodaXLTempleteViewModels(int PeriodId, int EmployeeId);
		#endregion

		#region salaryActivityPerchect
		Task<bool> SavesalaryActivityPercent(SalaryActivityPercent salaryActivityPercent);
		Task<IEnumerable<SalaryActivityPercent>> GetsalaryActivityPercent();
		Task<IEnumerable<SalaryActivityPercent>> GetsalaryActivityPercentByEmpId(int empId);
		Task<bool> DeletesalaryActivityPercentById(int empId);
		#endregion

		#region salaryActivityPerchect
		Task<bool> SaveCodeManagement(CodeManagement codeManagement);
		Task<IEnumerable<CodeManagement>> GetCodeManagement();
		Task<IEnumerable<CodeManagement>> GetCodeManagementByEmpPeriodId(int empId, int PeriodId);
		Task<IEnumerable<CodeManagement>> GetCodeManagementByPeriodId(int PeriodId);
		Task<bool> DeleteCodeManagementsByEmpPeriodId(int empId, int PeriodId);
		Task<bool> DeleteCodeManagementsByPeriodId(int PeriodId);
		#endregion

		#region Salary/Bonus Process
		Task<IEnumerable<SalaryProcessDataViewModel>> EmpSalaryProcess(int salaryPeriodId, int employeeInfoId);
        Task<IEnumerable<SalaryProcessDataViewModel>> BranchWiseEmpSalaryProcess(int salaryPeriodId, int hrBranchId);

        Task<IEnumerable<SalaryProcessDataViewModel>> EmpBonusProcess(int salaryPeriodId, int salaryHeadId, int employeeInfoId, DateTime? lastDayofPeriod, string userName, string bonusFor);
		Task<IEnumerable<SalaryProcessDataViewModel>> WagesSalaryProcess(int salaryPeriodId, int employeeInfoId);
		Task<IEnumerable<SalaryProcessDataViewModel>> BonusProcess(string username, int salaryPeriodId, int hrBranch, int zoneId, string empCode);
		#endregion

		#region Wages Pay slip

		Task<IEnumerable<PayslipReportViewModel>> GetWagesPaySlipByEmpId(int employeeInfoId, int salaryPeriodId);
		Task<IEnumerable<PayslipReportViewModel>> GetWagesPaySlipAdditionByEmpId(int employeeInfoId, int salaryPeriodId);
		Task<IEnumerable<PayslipReportViewModel>> GetWagesPaySlipDeductionByEmpId(int employeeInfoId, int salaryPeriodId);

		Task<IEnumerable<MonthlySalaryReportViewModel>> GetWagesMonthlySalaryReportByPeriodId(int salaryPeriodId, int? sbuId, int? pnsId);

		#endregion

		Task<bool> DeleteSalarySlabById(int id);
		Task<bool> DeleteSalaryGradeHeadById(int id);
		Task<IEnumerable<EmployeeInfo>> GetIndividualEmployeeFromSalaryStructure();
		Task<IEnumerable<EmployeesSalaryStructure>> GetSalaryStructureByEmp(int empId);
		Task<IEnumerable<SalaryProcessDataViewModel>> ProcessPartialSalary(int salaryPeriodId, int type, int employeeInfoId);
		Task<IEnumerable<SalaryProcessDataViewModel>> GetAllEmployeesByType(int salaryPeriodId, int type);
		Task<SalaryProcessDataViewModel> ProcessIndividualEmployeeSalaryForLongReport(int salaryPeriodId, int empId);
		Task<bool> SaveHomeLoanPolicy(LoanPolicy loanPolicy);
		Task<bool> SaveCarLoanPolicy(LoanPolicy loanPolicy);
		Task<bool> SavePFLoanPolicy(LoanPolicy loanPolicy);
		Task<bool> DeleteCarLoanPolicyById(int id);
		Task<bool> DeleteHomeLoanPolicyById(int id);

        Task<ExecuteNoneQuery> CollectPayrollContributionByPeriodId(int salaryPeriodId, string createBy);
        Task<IEnumerable<EmpContributionViewModel>> GetPayrollContributionByPeriodId(int salaryPeriodId);
        Task<IEnumerable<EmpContributionViewModel>> GetProcessedDataByYearAndMonth(string year, string month);
		Task<bool> DeletePFLoanPolicyById(int id);
		Task<int> SaveSalaryContribution(PFSalaryContribution pFSalaryContribution);
        Task<IEnumerable<PFSalaryContribution>> GetAllpfContribution();
        Task<SalarySlab> GetSalarySlabByAmount(decimal amount);
        Task<IEnumerable<SalaryPeriod>> GetSalaryPeriodByLockLavel(int locklabel);
        Task<PFMemberInfo> GetPFMemberInfoByEmployeeId(int empId);



        Task<IEnumerable<SalaryPeriod>> GetAllSalaryPeriodApproved(int userId, int lockLabel);
        Task<IEnumerable<SalaryPeriod>> GetAllSalaryPeriodDisbursePending(int userId, int lockLabel);
        Task<IEnumerable<SalaryPeriod>> GetAllSalaryDisburse(int userId);
        Task<List<int>> GetEmployeesWithSalaryStructure();
        Task<List<int>> GetBranchEmployeesWithSalaryStructure(int branchId);
        Task<IEnumerable<FunctionInfo>> GetAllOffices();
        Task<IEnumerable<HrUnit>> GetAllHrUnits();


        #region ForfeitAccount
        Task<bool> SaveForfeitAccount(ForfeitAccount forfeitAccount);
        Task<IEnumerable<ForfeitAccount>> GetAllForfeitAccount();
        Task<ForfeitAccount> GetForfeitAccountById(int id);
        Task<bool> DeleteForfeitAccountById(int id);


        #endregion
        #region FinalSettlement
        Task<bool> SavepfFinalSettleMent(PfFinalSettlement pfFinalSettlement);
        Task<IEnumerable<PfFinalSettlement>> GetAllpfFinalSettlement();
        Task<PfFinalSettlement> GetpfFinalSettlementById(int id);
        Task<bool> DeletepfFinalSettlementById(int id);


        #endregion

        #region masterData
        Task<IEnumerable<HrBranch>> GetAllHrBranchs();
		Task<HrBranch> GetHrBranchById(int hrBranchId);
		Task<CBSProcessSp> CBSProcessSalarySheetByYearMonthAndSBU(int periodId, string sbu, string empCode, int hrBranchId);
		Task<CBSProcessSp> CBSProcessSalaryVoucher(int periodId, int hrBranchId, string type);
		Task<CBSProcessSp> CBSProcessLoan(int periodId, int hrBranchId);
		Task<IEnumerable<Department>> GetAllDepartments();
        #endregion
        Task<int> SaveCarLoanPolicyNew(LoanPolicyNew loanPolicyNew);
        Task<IEnumerable<LoanPolicyNew>> GetAllLoanPolicyNew();
        Task<IEnumerable<StaffLoanViewModel>> GetStaffLoanLogByDateRange(string empCode, int loanId, DateTime? startDate, DateTime? endDate);
        Task<IEnumerable<StaffLoan>> GetAllStaffLoans();
        Task<DateTime?> GetPRLDateByEmpId(int empId);
        Task<int> SaveStaffLoan(StaffLoan model);
		Task<int> DeleteStaffLoanLogById(int id);

		Task<int> SaveStaffLoanLog(StaffLoanLog model);
		Task<CheckLoanDedVm> RecoverFromPrincipalOrInterestExpSelf(string transactionNo, int loanId);
		Task<List<StaffLoanLog>> GetStaffLoanLogByTransactionNo(string transactionNo);
		Task<CheckLoanDedVm> RecoverFromPrincipalOrInterest(int loanId);
		Task<UpdateSalaryStructureExpire> UpdateSalaryStructureExpire();
		Task<IEnumerable<SalaryLockVm>> PopulateBranchSalaryLock(int periodId);
		Task<IEnumerable<HrBranchSalary>> GetHrBranchForSalaryProcess(int periodId);
		Task<IEnumerable<HrBranchSalary>> GetHrZonesForSalaryProcess(int periodId);
		Task<IEnumerable<SalaryLockWithStatusVm>> GetAllSalaryLocksWithStatus();
		Task<SalaryLockVm> UpdateBadgeLock(int periodId, string type, int status);
		Task<IEnumerable<SalaryLockWithStatusVm>> GetAllBadgeLock(int periodId, string type);
		Task<int> UpdateLockStatusBranchWise(int lockId);
        int ProcessSalaryBackgroundTasks(int periodId, int empId);

        Task<IEnumerable<StaffLoanInterestVm>> GetStaffLoanInterestScheduleByDateRange(DateTime? startDate, DateTime? endDate, int hrBranchId, int zoneId, string loanType);
        Task<IEnumerable<StaffLoanAllInterestVm>> GetAllInterestByDate(DateTime? startDate, DateTime? endDate);
        Task<IEnumerable<HeadOfficeWiseBonusSummary>> GetBonusSummaryDataByHeadOffice(int periodId, string type);
        #region SalarySummarySheet
        Task<IEnumerable<SalarySummarySheetSpViewModel>> GetSalarySummarySheetHeadOffice(int salaryPeriodId);
        Task<IEnumerable<BranchSalarySummaryViewModel>> GetSalarySummarySheetBranch(int salaryPeriodId);
        Task<IEnumerable<BranchSalarySummaryViewModel>> GetSalarySummarySheetZone(int salaryPeriodId);
		Task<IEnumerable<BranchSalarySummaryViewModel>> GetYearlySalarySummarySheetBranch(string fromDate, string toDate);
		#endregion

		Task<IEnumerable<SalariedLoanDeductionVm>> GetSalariedLoanDeductionByperiodid(int periodid);
        Task<IEnumerable<LoanRecoveryotherSalaryVm>> GetloanrecoveryotherthansalaryByDateRange(DateTime? startDate, DateTime? endDate);
        Task<IEnumerable<BranchandZoneVm>> GetBranchandZone();
		Task<IEnumerable<StaffLoanInterestChargeVm>> GetStaffLoanInterestChargeByDateRange(string asOndate, int hrBranchId,string zoneorBranch);
		Task<IEnumerable<EmployeeInfoMinimumPF>> GetEmployeesforPFAll();
        Task<LoanInterest> StaffLoanInterest(DateTime? processDate, string code, string type);
		Task<IEnumerable<EmployeeInfoMinimumNew>> GetUserWiseEmployee(string username);

        Task<bool> UpdateHouseLoanDeductAmountBasedOnType(StaffLoan loan);

        //Asad added on 08.06.2023
        Task<IEnumerable<StaffLoanInfoVm>> GetNewLoans(int salaryPeriodId);
        Task<ResolveAllVm> GetNumberOfNewLoans(int salaryPeriodId);
        Task<IEnumerable<IndividualLoanSummaryVm>> GetIndividualLoanSummaryByChargeDate(string chargeDate);
        Task<IEnumerable<VoucherVm>> GetVoucherByParticular(string particular, string startDate, string endDate);
		Task<ResponseObject> CollectPayrollLoanEmiByPeriodId(int salaryPeriodId, string createBy);
		Task<IEnumerable<PayrollLoanEmiViewModel>> GetPayrollLoanEmiByPeriodId(int salaryPeriodId);
        Task<ResponseObject> GenerateVoucherOnPayrollLoanEmiByPeriodId(int salaryPeriodId, string createBy);


    }
}
