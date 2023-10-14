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
    public class CustomRoleService : ICustomRoleService
    {
        private readonly ERPDbContext _context;

        public CustomRoleService(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveCustomRole(CustomRole customRole)
        {
            if (customRole.Id != 0)
                _context.customRoles.Update(customRole);
            else
                _context.customRoles.Add(customRole);

            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CustomRole>> GetCustomRole()
        {
            return await _context.customRoles.AsNoTracking().ToListAsync();
        }


        public async Task<CustomRole> GetCustomRoleById(int id)
        {
            return await _context.customRoles.FindAsync(id);
        }

        public async Task<bool> DeleteCustomRoleById(int id)
        {
            _context.customRoles.Remove(_context.customRoles.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
    }
}
