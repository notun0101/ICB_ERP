using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.MasterData
{
    public class CvBlackListService : ICvBlackListService
    {
        private readonly ERPDbContext _context;

        public CvBlackListService(ERPDbContext context)
        {
            _context = context;
        }

        #region CvBlackList
        public async Task<CvBlackList> GetCvBlack()
        {
            return await _context.cvBlackLists.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CvBlackList>> GetCvBlackList()
        {
            return await _context.cvBlackLists.AsNoTracking().ToListAsync();
        }


        public async Task<CvBlackList> GetCvBlackListById(int id)
        {
            return await _context.cvBlackLists.FindAsync(id);
        }


        public async Task<bool> SaveCvBlackList(CvBlackList cvBlackList)
        {
            if (cvBlackList.Id != 0)
                _context.cvBlackLists.Update(cvBlackList);
            else
                _context.cvBlackLists.Add(cvBlackList);
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteCvBlackListById(int id)
        {
            _context.cvBlackLists.Remove(_context.cvBlackLists.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
        #endregion

    }
}
