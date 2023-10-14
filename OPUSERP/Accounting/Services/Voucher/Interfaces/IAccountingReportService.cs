using OPUSERP.Areas.Accounting.Models;
using OPUSERP.Areas.Budget.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OPUSERP.Accounting.Services.Voucher.Interfaces
{
    public interface IAccountingReportService
    {
        Task<IEnumerable<DailyBillReceiveReportViewModel>> GetdailyBillReceivePdf(int supplierId, DateTime fromDate, DateTime toDate);
        Task<IEnumerable<TrialBalanceViewModel>> GetTrialBalanceReportData(DateTime fromDate, DateTime toDate, int? sbuId, int? projectId, int? reportBy);
        Task<LedgerBookViewModel> GetDayBookReportData(int voucherTypeId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId);
        Task<LedgerBookViewModel> GetSubLedgerBookReportData(int subledgerId, DateTime fromDate, DateTime toDate);
        Task<LedgerBookViewModel> GetLedgerBookReportData(int ledgerId, int subledgerId, DateTime fromDate, DateTime toDate,int? sbuId,int? projectId);
        Task<LedgerBookViewModel> GetCashLedgerBookReportData(int ledgerId, int subledgerId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId);
        Task<LedgerBookViewModel> GetBankLedgerBookReportData(int ledgerId, int subledgerId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId);
        Task<ChartOfAccountViewModel> ChartOfAccountViewModels( int? sbuId, int? projectId);
        Task<IEnumerable<DailyBillReceiveReportViewModel>> GetdailyBillReceive(int supplierId, DateTime fromDate, DateTime toDate);
        Task<IEnumerable<TrialBalanceViewModel>> GetTrialBalanceReportDatad(DateTime fromDate, DateTime toDate , int? sbuId, int? projectId);
        Task<IEnumerable<LedgerBookReportViewModel>> GetDayBookReportDatad(int voucherTypeId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId);
        Task<IEnumerable<LedgerBookReportViewModel>> GetSubLedgerBookReportDatad(int subledgerId, DateTime fromDate, DateTime toDate);
        Task<IEnumerable<LedgerBookReportViewModel>> GetLedgerBookReportDatad(int ledgerId, int subledgerId, DateTime fromDate, DateTime toDate,int? sbuId,int? projectId);
     
        Task<IEnumerable<CCWiseLedgerReportdataViewModel>> GetCCWiseLedgerReportViewModels(int costcentreId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId);
        Task<IEnumerable<LedgerDetailsViewModel>> LedgerDetailsViewModels(int ledgerId, DateTime fromDate, DateTime toDate);
        Task<IEnumerable<ChartOfAccountdataViewModel>> ChartOfAccountdataViewModel(int? sbuId, int? projectId);
        Task<IEnumerable<CCWiseLedgerReportViewModel>> GetCCWiseLedgerReportViewModelsT(int costcentreId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId);
        //Task<IEnumerable<ProfitAndLossAccountViewModel>> GetProfitLossACData(int? salaryYearId);

        Task<IEnumerable<ProfitAndLossAccountViewModel>> GetProfitLossACData_Previous(string fromDate, string toDate, int? sbuId, int? projectId);
        Task<IEnumerable<ProfitAndLossAccountViewModel>> GetProfitLossACData(string fromDate, string toDate, int? sbuId, int? projectId);
        Task<IEnumerable<BalanceSheetViewModel>> GetBalanceSheetData(string fromDate, string toDate, int? sbuId, int? projectId);
        Task<IEnumerable<CFSMasterViewModel>> GetCFSMasterViewModels(string FDate, string TDate, int? sbuId, int? projectId);
        Task<IEnumerable<CFSMasterViewModel>> GetCFSIndirectMasterViewModels(string FDate, string TDate, int? sbuId, int? projectId);
        Task<IEnumerable<BudgetExpenseMasterViewModel>> GetBudgetExpenseMasterViewModels(string FDate, string TDate, int? sbuId, int? projectId);
        Task<IEnumerable<RVMasterViewModel>> GetRVMasterViewModels(string FDate, string TDate, int? sbuId, int? projectId, int? reportBy);
        Task<BudgetExpenditureViewModel> GetBudgetExpenditureData(DateTime fromDate, DateTime toDate, int? sbuId, int? projectId);
        Task<LedgerBookViewModel> GetLedgerBookReportDataWithoutSP(int ledgerId, int subledgerId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId);
        Task<AccountingDashboardModel> AccountGroupWiseVoucher(int? sbuId, int year);
        Task<AccountingDashboardModel> YearlyIncomeExpense(int? sbuId, int year);
        Task<IEnumerable<CashBankAccountBalanceDashboard>> BankAndCashBalance(int? sbuId, int year);

        Task<LedgerBookViewModel> GetLedgerForBankReconcilation(int ledgerId, int subledgerId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId);

        Task<LedgerBookViewModel> GetStateMentsBankReconcilation(int ledgerId, int subledgerId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId);

        Task<LedgerBookViewModel> GetStateMentsBankReconcilationByMasterId(int ledgerId, int subledgerId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId,int bankReconMasterId);

       // Task<LedgerBookViewModel> GetLedgerBookReportDataWithoutSP(int ledgerId, int subledgerId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId);
    }
}
