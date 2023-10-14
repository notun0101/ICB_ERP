
//New
using OPUSERP.Areas.Accounting.Models;
using OPUSERP.ProvidentFund.Data.Entity;
using OPUSERP.ProvidentFund.VMS;
using OPUSERP.Utility.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.ProvidentFund.Service.Interface
{
    public interface IPFLoanManagementService
    {
        Task<bool> SavePFLoanManagement(PFLoanManagement pFLoanManagement);
        Task<IEnumerable<PFLoanManagement>> GetAllPFLoanManagement();
        Task<IEnumerable<PFLoanManagement>> GetAllPendingPFLoanManagement();
        Task<IEnumerable<PFLoanManagement>> GetAllApprovePFLoanManagement();
        Task<PFLoanManagement> GetPFLoanManagementId(int id);
        Task<bool> DeletePFLoanManagement(int id);
        Task<bool> UpdatePFLoanManagementStatus(int? id, int? status, string updateBy);
        Task<IEnumerable<DeleteVoucherViewModel>> LoanDisburseAutoVoucher(int loanId, string applyDate, int? ledgerId, string note, string createBy);
        Task<IEnumerable<PFLoanManagement>> GetAllDisbursedLoanList();
        Task<int> SaveLoanCollection(LoanCollection loanCollection);
        Task<EarlySettlementClosingBalanceVM> GetEarlySettlementClosingBalance(string memberCode);
        Task<IEnumerable<DeleteVoucherViewModel>> LoanCollectionAutoVoucher(int CollectionId, string collectionDate, int? ledgerId, string note, string createBy);
        Task<ResponseObject> CreateInterestProvision(string employeeId, string transactionDate, string createBy);
        Task<IEnumerable<PFReportVm>> GetPFReportByEmpId(int empId, int year);
        Task<IEnumerable<PFReportVm>> GetDataForPFSummaryReport();
        Task<DeleteVoucherViewModel> ProcessAllPFInterest(DateTime processDate, string userName);
        Task<IEnumerable<PFStatementVM>> GetPFStatementByEmpIdAndDate(int empId, DateTime fromDate, DateTime Todate);
        Task<IEnumerable<PFStatementVM>> GetPFDailyTransactionReport(DateTime fromDate, DateTime Todate);
        Task<IEnumerable<LoanStatementVM>> GetTrusteLoanStatementByEpmId(int empId, DateTime fromDate, DateTime Todate);
        Task<IEnumerable<LoanStatementVM>> GetLoanDailyTransactionReport(DateTime fromDate, DateTime Todate);
        Task<BalanceConfirmationVM> GetBalanceConfirmationReportByEmpId(int empId, DateTime fromDate, DateTime Todate);
        Task<LoanCollection> GetCollectionDataByLloanId(int? id);


    }
}

