using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using Microsoft.EntityFrameworkCore;
using OPUSERP.Data.Entity.MasterData;

namespace OPUSERP.HRPMS.Services.MasterData
{
    public class BookAwardService : IBookAwardService
    {
        private readonly ERPDbContext _context;

        public BookAwardService(ERPDbContext context)
        {
            _context = context;
        }
        //Book Info

        public async Task<IEnumerable<BookCategory>> GetBookCategory()
        {
            return await _context.bookCategories.AsNoTracking().ToListAsync();
        }

        public async Task<BookCategory> GetBookCategoryById(int id)
        {
            return await _context.bookCategories.FindAsync(id);
        }

        public async Task<bool> SaveBookCategory(BookCategory bookCategory)
        {
            if (bookCategory.Id != 0)
                _context.bookCategories.Update(bookCategory);
            else
                _context.bookCategories.Add(bookCategory);
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteBookCategoryById(int id)
        {
            _context.bookCategories.Remove(_context.bookCategories.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        //Award Info

        public async Task<bool> SaveAwardInfo(Award award)
        {
            if (award.Id != 0)
                _context.awards.Update(award);
            else
                _context.awards.Add(award);

            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<bool> SaveJDTaskListInfo(JDTaskList jDTaskList)
        {
            if (jDTaskList.Id != 0)
                _context.jDTaskLists.Update(jDTaskList);
            else
                _context.jDTaskLists.Add(jDTaskList);

            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<bool> SaveAtmInfo(HrAtmBooth hrAtmBooth)
        {
            if (hrAtmBooth.Id != 0)
                _context.hrAtmBooths.Update(hrAtmBooth);
            else
                _context.hrAtmBooths.Add(hrAtmBooth);

            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Award>> GetAwardInfo()
        {
            return await _context.awards.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<JDTaskList>> GetJDTaskListInfo()
        {
            return await _context.jDTaskLists.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<HrAtmBooth>> GetatmboothInfo()
        {
            return await _context.hrAtmBooths.Include(x=>x.branch).Include(x=>x.subBranch).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<HrBranch>> Getbranches()
        {
            return await _context.hrBranches.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<HrSubBranch>> Getsubbranches()
        {
            return await _context.hrSubBranches.AsNoTracking().ToListAsync();
        }

        public async Task<Award> GetAwardInfoById(int id)
        {
            return await _context.awards.FindAsync(id);
        }

        public async Task<JDTaskList> GetJDTaskListById(int id)
        {
            return await _context.jDTaskLists.FindAsync(id);
        }

        public async Task<HrAtmBooth> GetATMInfoById(int id)
        {
            return await _context.hrAtmBooths.FindAsync(id);
        }

        public async Task<bool> DeleteAwardInfoById(int id)
        {
            _context.awards.Remove(_context.awards.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<bool> DeletJDTaskListInfoById(int id)
        {
            _context.jDTaskLists.Remove(_context.jDTaskLists.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<bool> DeleteatmboothInfobyId(int id)
        {
            _context.hrAtmBooths.Remove(_context.hrAtmBooths.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
    }
}
