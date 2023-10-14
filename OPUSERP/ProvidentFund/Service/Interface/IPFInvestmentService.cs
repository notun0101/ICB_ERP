using OPUSERP.ProvidentFund.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.ProvidentFund.Service.Interface
{
    public interface IPFInvestmentService
    {
        Task<bool> SavePFInvestment(PFInvestment pFInvestment);
        Task<IEnumerable<PFInvestment>> GetAllPFInvestment();
        Task<bool> DeletePFInvestmentById(int id);
        Task<PFInvestment> GetPFInvestmentById(int id);
        Task<decimal> GetTotalInvestment();
        Task<decimal> GetTotalNewYearInvestment();
    }
}
