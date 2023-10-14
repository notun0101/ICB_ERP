using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.ProvidentFund.Data.Entity;
using OPUSERP.ProvidentFund.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.ProvidentFund.Service
{
    public class PFInvestmentService: IPFInvestmentService
    {
        private readonly ERPDbContext _context;

        public PFInvestmentService(ERPDbContext context)
        {
            _context = context;
        }


        public async Task<bool> SavePFInvestment(PFInvestment pFInvestment)
        {
            if (pFInvestment.Id != 0)
                _context.pFInvestments.Update(pFInvestment);
            else
                _context.pFInvestments.Add(pFInvestment);

            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<PFInvestment>> GetAllPFInvestment()
        {
            return await _context.pFInvestments.AsNoTracking().ToListAsync();
        }

        public async Task<bool> DeletePFInvestmentById(int id)
        {
            _context.pFInvestments.Remove(_context.pFInvestments.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<PFInvestment> GetPFInvestmentById(int id)
        {
            return await _context.pFInvestments.FindAsync(id);
        }
        public async Task<decimal> GetTotalInvestment()
        {
            var data = await _context.pFInvestments.SumAsync(x => x.investmentAmount);
            return Convert.ToDecimal(data);
        }
        public async Task<decimal> GetTotalNewYearInvestment()
        {
            var data = await _context.pFInvestments.Where(x=> Convert.ToDateTime(x.investmentDate).Year== DateTime.Now.Year).SumAsync(x => x.investmentAmount);
            return Convert.ToDecimal(data);
        }
    }
}
