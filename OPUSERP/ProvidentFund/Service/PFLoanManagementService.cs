//New
using Microsoft.EntityFrameworkCore;
using OPUSERP.Areas.Accounting.Models;
using OPUSERP.Areas.Payroll.Models;
using OPUSERP.Data;
using OPUSERP.ProvidentFund.Data.Entity;
using OPUSERP.ProvidentFund.Service.Interface;
using OPUSERP.ProvidentFund.VMS;
using OPUSERP.Utility.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.ProvidentFund.Service
{
    public class PFLoanManagementService : IPFLoanManagementService
    {
        private readonly ERPDbContext _context;
        public PFLoanManagementService(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SavePFLoanManagement(PFLoanManagement pFLoanManagement)
        {
            if (pFLoanManagement.Id != 0)
                _context.pFLoanManagements.Update(pFLoanManagement);
            else
                _context.pFLoanManagements.Add(pFLoanManagement);

            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<bool> DeletePFLoanManagement(int id)
        {
            _context.pFLoanManagements.Remove(_context.pFLoanManagements.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PFLoanManagement>> GetAllApprovePFLoanManagement()
        {
            return await _context.pFLoanManagements.Include(x => x.pfmember).Include(x=>x.EmployeeInfo).Where(x => x.approveStatus == 2).AsNoTracking().ToListAsync();

        }
        public async Task<IEnumerable<PFLoanManagement>> GetAllDisbursedLoanList()
        {
            return await _context.pFLoanManagements.Include(x => x.pfmember).Where(x => x.approveStatus == 3).AsNoTracking().ToListAsync();

        }

        public async Task<IEnumerable<PFLoanManagement>> GetAllPendingPFLoanManagement()
        {
            return await _context.pFLoanManagements.Include(x => x.pfmember).Include(x=>x.EmployeeInfo).Where(x => x.approveStatus == 0).AsNoTracking().ToListAsync();

        }

        public async Task<IEnumerable<PFLoanManagement>> GetAllPFLoanManagement()
        {
            return await _context.pFLoanManagements.Include(x => x.pfmember).ToListAsync();
        }

        public async Task<PFLoanManagement> GetPFLoanManagementId(int id)
        {
            return await _context.pFLoanManagements.Include(x=>x.pfmember).Include(x=>x.EmployeeInfo).Where(x=>x.Id==id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdatePFLoanManagementStatus(int? id, int? status, string updateBy)
        {
            var pFLoanManagements = _context.pFLoanManagements.Find(id);
            pFLoanManagements.approveStatus = status;
            pFLoanManagements.isActive = 1;
            pFLoanManagements.updatedBy = updateBy;
            pFLoanManagements.updatedAt = DateTime.Now;

            _context.Entry(pFLoanManagements).State = EntityState.Modified;

            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<DeleteVoucherViewModel>> LoanDisburseAutoVoucher(int loanId, string applyDate, int? ledgerId, string note, string createBy)
        {
            try
            {
                var data = await _context.deleteVoucherViewModels.FromSql($"SP_DisburseLoan {loanId},{Convert.ToDateTime(applyDate).ToString("yyyyMMdd")},{ledgerId},{note},{createBy}").AsNoTracking().ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IEnumerable<DeleteVoucherViewModel>> LoanCollectionAutoVoucher(int CollectionId, string collectionDate, int? ledgerId, string note, string createBy)
        {
            try
            {
                var data = await _context.deleteVoucherViewModels.FromSql($"SP_CollectionLoan {CollectionId},{Convert.ToDateTime(collectionDate).ToString("yyyyMMdd")},{ledgerId},{note},{createBy}").AsNoTracking().ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public async Task<EarlySettlementClosingBalanceVM> GetEarlySettlementClosingBalance(string memberCode)
        {
            try
            {
                var data = await _context.earlySettlementClosingBalanceVMs.FromSql($"Sp_EarlySettlementClosingBalance {memberCode}").FirstOrDefaultAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }



        }

        public async Task<DeleteVoucherViewModel> ProcessAllPFInterest(DateTime processDate, string username)
        {
            try
            {
                var data = await _context.deleteVoucherViewModels.FromSql($"SP_ProcessAllPFInterest {processDate},{username}").FirstOrDefaultAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }



        }

        public async Task<IEnumerable<PFReportVm>> GetPFReportByEmpId(int empId, int year)
        {
            try
            {
                var data = await _context.pFReportVms.FromSql($"sp_GetPFReportByEmpId {empId},{year}").ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<IEnumerable<PFReportVm>> GetDataForPFSummaryReport()
        {
            try
            {
                var data = await _context.pFReportVms.FromSql($"sp_GetDataForPFSummaryReport").ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IEnumerable<PFStatementVM>> GetPFDailyTransactionReport(DateTime fromDate, DateTime Todate)
        {
            try
            {
                var data = await _context.pFStatementVMs.FromSql($"sp_GetPFDailyTransactionReport {fromDate},{Todate}").ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<IEnumerable<LoanStatementVM>> GetLoanDailyTransactionReport(DateTime fromDate, DateTime Todate)
        {
            try
            {
                var data = await _context.loanStatementVMs.FromSql($"sp_GetLoanDailyTransactionReport {fromDate},{Todate}").ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<BalanceConfirmationVM> GetBalanceConfirmationReportByEmpId(int empId, DateTime fromDate, DateTime Todate)
        {
            try
            {
                var data = await _context.balanceConfirmationVMs.FromSql($"sp_GetBalanceConfirmationReportByEmpId {empId},{fromDate},{Todate}").FirstOrDefaultAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IEnumerable<PFStatementVM>> GetPFStatementByEmpIdAndDate(int empId, DateTime fromDate, DateTime Todate)
        {
            try
            {
                var data = await _context.pFStatementVMs.FromSql($"sp_GetPFStatementByEmpIdAndDate {empId},{fromDate},{Todate}").ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<IEnumerable<LoanStatementVM>> GetTrusteLoanStatementByEpmId(int empId, DateTime fromDate, DateTime Todate)
        {
            try
            {
                var data = await _context.loanStatementVMs.FromSql($"Sp_GetTrusteLoanStatementByEpmId {empId},{fromDate},{Todate}").ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }




        #region LoanCollection

        public async Task<int> SaveLoanCollection(LoanCollection loanCollection)
        {

            if (loanCollection.Id != 0)
            {

                _context.loanCollections.Update(loanCollection);
            }
            else
            {

                _context.loanCollections.Add(loanCollection);
            }

            await _context.SaveChangesAsync();
            return loanCollection.Id;
        }

        public async Task<LoanCollection> GetCollectionDataByLloanId(int? id)
        {
            var data = await _context.loanCollections.Where(x => x.loanManagementId == id).LastOrDefaultAsync();
            return data;
        }


        public async Task<ResponseObject> CreateInterestProvision(string employeeId, string transactionDate, string createBy)
        {
            //string vDate = DateTime.Now.ToString();
            var result = new ExecuteNoneQuery();

            try
            {
                result = await _context.ExecuteNoneQuery.FromSql($"SP_CreateInterestProvision {employeeId},{transactionDate}, {createBy}").AsNoTracking().FirstOrDefaultAsync();  //.ToListAsync()
            }
            catch (Exception exp)
            {

            }


            string message = string.Empty;
            string status = string.Empty;

            if (result != null)
            {
                message = result.Message;
                status = result.Status;
            }
            else
            {
                message = "Unable to Make Interest Provision.";
                status = "NOK";
            }
            return new ResponseObject { Status = status, Message = message };
        }



        #endregion
    }
}




