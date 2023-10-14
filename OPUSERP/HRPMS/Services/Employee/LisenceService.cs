using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.Data.Entity.MasterData;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Employee
{
    public class LisenceService : ILisenceService
    {
        private readonly ERPDbContext _context;

        public LisenceService(ERPDbContext context)
        {
            _context = context;
        }

        #region Tax
        public async Task<bool> DeleteLisenceById(int id)
        {
            _context.lisences.Remove(_context.lisences.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<int> SaveLisence(Lisence lisence)
        {
            if (lisence.Id != 0)
                _context.lisences.Update(lisence);
            else
                _context.lisences.Add(lisence);
            await _context.SaveChangesAsync();
            return lisence.Id;
        }

        public async Task<IEnumerable<Lisence>> GetLisence()
        {
            return await _context.lisences.Include( x=> x.lisenceType).AsNoTracking().ToListAsync();
        }

        public async Task<Lisence> GetLisence1()
        {
            return await _context.lisences.Include(x => x.lisenceType).AsNoTracking().FirstOrDefaultAsync();
        }


        public async Task<Lisence> GetLisenceById(int id)
        {
            var data = await _context.lisences.Where(x => x.Id == id).FirstOrDefaultAsync();
            return data;
        }


        #endregion

        #region DuelResidence


        public async Task<bool> DeleteLisenceTypeById(int id)
        {
            _context.lisenceTypes.Remove(_context.lisenceTypes.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<int> SaveLisenceType(LisenceType lisenceType)
        {
            if (lisenceType.Id != 0)
                _context.lisenceTypes.Update(lisenceType);
            else
                _context.lisenceTypes.Add(lisenceType);
            await _context.SaveChangesAsync();
            return lisenceType.Id;
        }

        public async Task<IEnumerable<LisenceType>> GetLisenceType()
        {
            return await _context.lisenceTypes.AsNoTracking().ToListAsync();
        }




        #endregion

    }
}
