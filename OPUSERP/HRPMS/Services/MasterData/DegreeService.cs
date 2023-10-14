using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace OPUSERP.HRPMS.Services.MasterData
{
    public class DegreeService : IDegreeService
    {
        private readonly ERPDbContext _context;

        public DegreeService(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveDegree(Degree degree)
        {
            if(degree.Id != 0)
                _context.degrees.Update(degree);
            else
                _context.degrees.Add(degree);
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Degree>> GetAllDegree()
        {
            return await _context.degrees.Include(a => a.levelofeducation).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<Organization>> GetAllUniversity()
        {
            return await _context.organizations.OrderBy(x => x.organizationName).Where(x => x.organizationType == "University" && x.type == "UGC Approved").AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<Organization>> GetAllboard()
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return await _context.organizations.OrderBy(x => x.organizationName).Where(x => x.organizationType == "Board").ToListAsync();
        }
        public async Task<Degree> GetDegreeById(int id)
        {
            return await _context.degrees.FindAsync(id);
        }

        public async Task<bool> DeleteDegreeById(int id)
        {
            _context.degrees.Remove(_context.degrees.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Degree>> GetDegreeByLOEId(int loEId)
        {
            return await _context.degrees.OrderBy(x => x.degreeName).Where(x => x.levelofeducationId == loEId).ToListAsync();
        }
    }
}
