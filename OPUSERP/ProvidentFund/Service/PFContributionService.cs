using Microsoft.EntityFrameworkCore;
using OPUSERP.Areas.ProvidentFund.Models;
using OPUSERP.Data;
using OPUSERP.ProvidentFund.Data.Entity;
using OPUSERP.ProvidentFund.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.ProvidentFund.Service
{
    public class PFContributionService : IPFContributionService
    {
        private readonly ERPDbContext _context;

        public PFContributionService(ERPDbContext context)
        {
            _context = context;
        }


        public async Task<bool> SavePFContribution(PFContribution pFContribution)
        {
            if (pFContribution.Id != 0)
                _context.pFContributions.Update(pFContribution);
            else
                _context.pFContributions.Add(pFContribution);

            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PFContribution>> GetAllPFContribution()
        {
            return await _context.pFContributions.Include(x=>x.pfmember).AsNoTracking().ToListAsync();
        }
        public async Task<decimal> GetTotalEmployeeContribution()
        {
            var data = await _context.pFContributions.SumAsync(x => x.employeeContribution);
            return Convert.ToDecimal(data);
        }
        public async Task<decimal> GetTotalCompanyContribution()
        {
            var data = await _context.pFContributions.SumAsync(x => x.companyContribution);
            return Convert.ToDecimal(data);
        }

        public async Task<bool> DeletePFContributionById(int id)
        {
            _context.pFContributions.Remove(_context.pFContributions.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<PFContribution> GetPFContributionById(int id)
        {
            return await _context.pFContributions.FindAsync(id);
        }
        public async Task<ContributionAmountVM> GetContributionAmountByMemberId(int id)
        {
            var data = new ContributionAmountVM();

            try
            {
                data = await _context.contributionAmountVMs.FromSql($"SP_GetContributionAmountByMemberId {id}").FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

            }

            return data;

        }

        public async Task<decimal> GetTotalContribution()
        {
            var contribution = await _context.pFContributions.SumAsync(x => x.companyContribution + x.employeeContribution);
            var DistributedInterest = await _context.pFInterestDistributionMasters.SumAsync(x => x.totaldistributedInterest);
            var data = contribution + DistributedInterest;
            return Convert.ToDecimal(data);
        }

        public async Task<int> PFInterestDistribution(string processBy, int periodId, decimal? interest)
        {
            _context.Database.ExecuteSqlCommand($"Sp_PFInterestDistribution {processBy},{periodId},{interest}");
            return 1;
        }
    }
}
