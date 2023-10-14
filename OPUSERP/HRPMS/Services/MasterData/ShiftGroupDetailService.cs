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
    public class ShiftGroupDetailService : IShiftGroupDetailService
    {
        private readonly ERPDbContext _context;

        public ShiftGroupDetailService(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveShiftGroupDetail(ShiftGroupDetail shiftGroupDetail)
        {
            if(shiftGroupDetail.Id != 0)
                _context.shiftGroupDetails.Update(shiftGroupDetail);
            else
                _context.shiftGroupDetails.Add(shiftGroupDetail);
            return 1 == await _context.SaveChangesAsync();
        }
        

        public async Task<IEnumerable<ShiftGroupDetail>> GetAllShiftGroupDetail()
        {
            return await _context.shiftGroupDetails.Include(x=>x.shiftGroupMaster).AsNoTracking().ToListAsync();
        }
		public async Task<IEnumerable<ShiftGroupDetail>> GetAllShiftGroupDetailByMasterId(int id)
        {
            return await _context.shiftGroupDetails.Include(x=>x.shiftGroupMaster).Where(x => x.shiftGroupMasterId == id).AsNoTracking().ToListAsync();
        }
		public async Task<bool> CheckEmployeeInShiftGroup(int? id, int empCode)
        {
			var id1 = await _context.employeeInfos.Where(x => x.Id == empCode).Select(x => x.shiftGroupId).FirstOrDefaultAsync();
			if (id1 == id)
			{
				return true;
			}
			else
			{
				return false;
			}
        }

        public async Task<ShiftGroupDetail> GetShiftGroupDetailById(int id)
        {
            return await _context.shiftGroupDetails.FindAsync(id);
        }

        public async Task<bool> DeleteShiftGroupDetailById(int id)
        {
            _context.shiftGroupDetails.Remove(_context.shiftGroupDetails.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<bool> DeleteShiftById(int id)
        {
            _context.shiftGroupMasters.Remove(_context.shiftGroupMasters.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }


    }
}
