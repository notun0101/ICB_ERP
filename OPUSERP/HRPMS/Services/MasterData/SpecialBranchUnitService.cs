using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.MasterData
{
    public class SpecialBranchUnitService: ISpecialBranchUnitService
    {
        private readonly ERPDbContext _context;

        public SpecialBranchUnitService(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SpecialBranchUnit>> GetSpecialBranchUnit()
        {
            return await _context.SpecialBranchUnits.Where(x=> x.isDelete != 1).Include(x=>x.company).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<EmpExpertise>> GetEmpExpertise()
        {
            return await _context.empExpertises.AsNoTracking().ToListAsync();
        }

        public async Task<SpecialBranchUnit> GetSpecialBranchUnitById(int id)
        {
            return await _context.SpecialBranchUnits.FindAsync(id);
        }

        public async Task<IEnumerable<SpecialBranchUnit>> GetSpecialBranchUnitByUserBranchId(int id)
        {
            return await _context.SpecialBranchUnits.Where(a => a.Id == id && a.isDelete != 1).AsNoTracking().ToListAsync();
        }

        public async Task<bool> SaveSpecialBranchUnit(SpecialBranchUnit specialBranchUnit)
        {
            if (specialBranchUnit.Id != 0)
                _context.SpecialBranchUnits.Update(specialBranchUnit);
            else
                _context.SpecialBranchUnits.Add(specialBranchUnit);
            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<bool> SaveExpertisee(EmpExpertise empExpertise)
        {
            if (empExpertise.Id != 0)
                _context.empExpertises.Update(empExpertise);
            else
                _context.empExpertises.Add(empExpertise);
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteSpecialBranchUnitById(int id)
        {
            _context.SpecialBranchUnits.Remove(_context.SpecialBranchUnits.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<bool> DeleteExpertiseById(int id)
        {
            _context.empExpertises.Remove(_context.empExpertises.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
    }
}
