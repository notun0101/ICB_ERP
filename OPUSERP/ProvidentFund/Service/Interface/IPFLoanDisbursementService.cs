using OPUSERP.ProvidentFund.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OPUSERP.ProvidentFund.Service.Interface
{
    public interface IPFLoanDisbursementService
    {
        Task<PFLoanDisbursement> GetLoanDisbursementByEmployeeId(int employeeId);
        Task<bool> SaveLoanDisbursement(PFLoanDisbursement disbursement);
        Task<IEnumerable<PFLoanDisbursement>> GetAllLoanDisbursement();
        Task<IEnumerable<PFLoanDisbursement>> GetAllPendingLoanDisbursement();
        Task<IEnumerable<PFLoanDisbursement>> GetAllApproveLoanDisbursement();
        Task<PFLoanDisbursement> GetLoanDisbursementId(int id);
        Task<bool> DeleteLoanDisbursement(int id);
        Task<bool> UpdateLoanDisbursementStatus(int? id, int? status, string updateBy);
    }
}
