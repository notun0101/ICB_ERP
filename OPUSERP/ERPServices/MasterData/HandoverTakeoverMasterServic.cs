using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.Data.Entity.Master;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace OPUSERP.ERPServices.MasterData
{
    public class HandoverTakeoverMasterServic : IHandoverTakeoverMasterService
    {
        private readonly ERPDbContext _context;

        public HandoverTakeoverMasterServic(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveHTMaster(HandoverTakeoverMaster handoverTakeoverMaster)
        {
            if (handoverTakeoverMaster.Id != 0)
                _context.handoverTakeoverMasters.Update(handoverTakeoverMaster);
            else
                _context.handoverTakeoverMasters.Add(handoverTakeoverMaster);
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<HandoverTakeoverMaster>> GetAllHTMaster()
        {
            return await _context.handoverTakeoverMasters.Include(x => x.handover).Include(x => x.takeover).ToListAsync();
        }

        public async Task<HandoverTakeoverMaster> GetHTMasterById(int id)
        {
            return await _context.handoverTakeoverMasters.FindAsync(id);
        }

        public async Task<bool> DeleteHTMasterById(int id)
        {
            _context.handoverTakeoverMasters.Remove(_context.handoverTakeoverMasters.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }


        public async Task<bool> SaveHTDetails(HandoverTakeoverDetails handoverTakeoverDetails)
        {
            if (handoverTakeoverDetails.Id != 0)
                _context.handoverTakeoverDetails.Update(handoverTakeoverDetails);
            else
                _context.handoverTakeoverDetails.Add(handoverTakeoverDetails);
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<HandoverTakeoverDetails>> GetAllHTDetails()
        {
            return await _context.handoverTakeoverDetails.Include(x => x.master).ToListAsync();
        }

        public async Task<HandoverTakeoverDetails> GetHTDetailsById(int id)
        {
            return await _context.handoverTakeoverDetails.FindAsync(id);

        }

        public async Task<IEnumerable<HandoverTakeoverDetails>> GetDetailsBytakeoverId(int takeoverId)
        {
            return await _context.handoverTakeoverDetails.Include(x => x.master).Include(x => x.master.takeover).Include(x => x.master.handover).Where(x => x.master.takeoverId == takeoverId).AsNoTracking().ToListAsync();
        }

        public async Task<bool> DeleteHTDetailsById(int id)
        {
            _context.handoverTakeoverDetails.Remove(_context.handoverTakeoverDetails.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<string> GetDetailsImgUrlById(int id)
        {
            return await _context.handoverTakeoverDetails.Where(x => x.Id == id).Select(x => x.file).FirstOrDefaultAsync();
        }
    }
}
