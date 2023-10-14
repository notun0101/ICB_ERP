using OPUSERP.Accounting.Data.Entity.AccountingSettings;
using OPUSERP.Accounting.Data.Entity.Voucher;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OPUSERP.Accounting.Services.AccountingSettings.Interfaces
{
    public interface IOpeningBalanceService
    {
        Task<int> SaveopeningBalance(OpeningBalance openingBalance);
        Task<IEnumerable<OpeningBalance>> GetOpeningBalance();
        Task<IEnumerable<OpeningBalance>> GetOpeningBalanceBranchUnitWise(int branchUnitId);
        Task<OpeningBalance> GetopeningBalanceById(int id);
        Task<IEnumerable<OpeningBalance>> GetOpeningBalancebyLedgerRelId(int id);
        Task<bool> DeleteopeningBalanceById(int id);
        Task<IEnumerable<VoucherDetail>> GetOpeningBalancebyVoucherNo(int branchUnitId);
        Task<bool> DeleteopeningBalanceByVoucherMasterId(int id);
        Task<bool> DeleteopeningOpeningBalanceDetailsByVoucherMasterId(int id);
    }
}
