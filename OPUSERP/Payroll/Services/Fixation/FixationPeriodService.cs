using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Payroll.Data.Entity.Fixation;
using OPUSERP.Payroll.Services.Fixation.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
namespace OPUSERP.Payroll.Services.Fixation
{
    public class FixationPeriodService : IFixationPeriodService
    {
        private readonly ERPDbContext _context;

        public FixationPeriodService(ERPDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<FixationPeriod>> GetAllFixationPeriod()
        {
            return await _context.FixationPeriods.AsNoTracking().ToListAsync();
        }

        public async Task<int> IsDuplicateFixationPeriod(FixationPeriod fixationPeriod)
        {
            var obj = new FixationPeriod();
            if (fixationPeriod.Id != 0)
            {
                obj = await _context.FixationPeriods
               .Where(x =>
                x.PeriodName == fixationPeriod.PeriodName && 
                x.FixationYear == fixationPeriod.FixationYear && 
                x.FixationTypeId == fixationPeriod.FixationTypeId && 
                x.ContextId == fixationPeriod.ContextId &&
                x.Id != fixationPeriod.Id).FirstOrDefaultAsync();
            }
            else
            {
                obj = await _context.FixationPeriods
                .Where(x => 
                x.PeriodName == fixationPeriod.PeriodName && 
                x.FixationYear == fixationPeriod.FixationYear && 
                x.FixationTypeId == fixationPeriod.FixationTypeId && 
                x.ContextId == fixationPeriod.ContextId).FirstOrDefaultAsync();
            }

            if(obj == null)
            {
                return 0;
            }
            else
            {
                return 1;
            }           
        }
       
        public async Task<int> SaveFixationPeriod(FixationPeriod fixationPeriod)
        {
            if (fixationPeriod.Id != 0)
                _context.FixationPeriods.Update(fixationPeriod);
            else
                _context.FixationPeriods.Add(fixationPeriod);
            await _context.SaveChangesAsync();
            return fixationPeriod.Id;
        }

        public async Task<int> DeleteFixationPeriod(int id)
        {
            var data = _context.FixationPeriods.Find(id);
            _context.FixationPeriods.Remove(data);
            return await _context.SaveChangesAsync();
        }
    }
}
