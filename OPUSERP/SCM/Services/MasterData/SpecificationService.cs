using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.SCM.Data.Entity.MasterData;
using OPUSERP.SCM.Services.MasterData.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.SCM.Services.MasterData
{
    public class SpecificationService : ISpecificationService
    {
        private readonly ERPDbContext _context;

        public SpecificationService(ERPDbContext context)
        {
            _context = context;
        }

      

        public async Task<IEnumerable<ItemSpecification>> GetSpecifications()
        {
            return await _context.ItemSpecifications.Include(x=>x.Item).AsNoTracking().ToListAsync();
        }
        public async Task<ItemSpecification> GetSpecificationsById(int id)
        {
            return await _context.ItemSpecifications.Include(x=>x.Item).Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<ItemSpecification> SaveSpecification(ItemSpecification specification)
        {
            if (specification.Id > 0)
            {
                _context.Entry(specification).State = EntityState.Modified;
            }
            else
            {
                _context.ItemSpecifications.Add(specification);
            }
            await _context.SaveChangesAsync();
            return specification;
        }
    }
}
