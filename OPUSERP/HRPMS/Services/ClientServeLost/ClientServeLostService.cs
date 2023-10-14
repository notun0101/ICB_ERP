using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.HrBudget;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.HrBudget.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.HrBudget
{
	public class ClientServeLostService : IClientServeLost
    {
		private readonly ERPDbContext _context;

		public ClientServeLostService(ERPDbContext context)
		{
			_context = context;
		}

        public async Task<bool> SaveClientServeLost(ClientServeLost clientServeLost)
        {
            if (clientServeLost.Id != 0)
                _context.clientServeLosts.Update(clientServeLost);
            else
                _context.clientServeLosts.Add(clientServeLost);

            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ClientServeLost>> GetClientServeLost()
        {
            return await _context.clientServeLosts.AsNoTracking().ToListAsync();
        }


        public async Task<ClientServeLost> GetClientServeLostById(int id)
        {
            return await _context.clientServeLosts.FindAsync(id);
        }

        public async Task<bool> DeleteClientServeLostById(int id)
        {
            _context.clientServeLosts.Remove(_context.clientServeLosts.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
    }
}
