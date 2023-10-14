using OPUSERP.Areas.ProvidentFund.Models;
using OPUSERP.ProvidentFund.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.ProvidentFund.Service.Interface
{
    public interface IPFContributionService
    {
        Task<bool> SavePFContribution(PFContribution pFContribution);
        Task<IEnumerable<PFContribution>> GetAllPFContribution();
        Task<PFContribution> GetPFContributionById(int id);
        Task<bool> DeletePFContributionById(int id);
        Task<decimal> GetTotalEmployeeContribution();
        Task<decimal> GetTotalCompanyContribution();
        Task<ContributionAmountVM> GetContributionAmountByMemberId(int id);
        Task<decimal> GetTotalContribution();
        Task<int> PFInterestDistribution(string processBy, int periodId, decimal? interest);


    }
}
