using Microsoft.EntityFrameworkCore;
using OPUSERP.Areas.HRPMSMasterData.Models;
using OPUSERP.Data;
using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Suspensions;
using OPUSERP.HRPMS.Services.BoardofDirector.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.BoardofDirector
{
    public class BoardOfDirectorService: IBoardofDirector
    {
        private readonly ERPDbContext _context;

        public BoardOfDirectorService(ERPDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Company>> GetAllCompanyList()
        {
            return await _context.Companies.OrderBy(x => x.companyName).AsNoTracking().ToListAsync();
        }
        public async Task<string> GetPhotoUrlById(int id)
        {
            return await _context.boardOfDirectors.Where(x => x.Id == id).Select(x => x.photoUrl).FirstOrDefaultAsync();
        }
        public async Task<string> GetDirectorPhotoUrlById(int id)
        {
            return await _context.boardOfDirectors.Where(x => x.Id == id).Select(x => x.photoUrl).FirstOrDefaultAsync();
        }

        public async Task<int> SaveBoardOfDirector(BoardOfDirector boardofDirector)
        {
            if (boardofDirector.Id != 0)
                _context.boardOfDirectors.Update(boardofDirector);
            else
                _context.boardOfDirectors.Add(boardofDirector);

            await _context.SaveChangesAsync();
            return boardofDirector.Id;
        }
		public async Task<IEnumerable<ComapnyAndYear>> GetAllCompanyListWithBOD()
		{
			var companies = await (from b in _context.boardOfDirectors
								   group b by b.year into bg
								   join c in _context.Companies
								   on bg.FirstOrDefault().companyId equals c.Id
								   select new ComapnyAndYear {
									   companyId = c.Id,
									   companyName = c.companyName,
									   managerName = c.managerName,
									   year = Convert.ToInt32(bg.FirstOrDefault().year)
								   }).ToListAsync();
			return companies;
		}
		public async Task<int> DeleteBoardOfDirectorByCompanyIdAndYear(int companyId, int year)
		{
			var data = await _context.boardOfDirectors.Where(x => x.companyId == companyId && x.year == year).AsNoTracking().ToListAsync();
			foreach (var item in data)
			{
				_context.boardOfDirectors.Remove(_context.boardOfDirectors.Find(item.Id));
				await _context.SaveChangesAsync();
			}
			return year;
		}
		public async Task<IEnumerable<ComapnyAndYear>> GetAllCompanyListWithBOD(int companyId, int year)
		{
			var companies = await (from b in _context.boardOfDirectors
								   group b by b.year into bg
								   join c in _context.Companies
								   on bg.FirstOrDefault().companyId equals c.Id
								   where c.Id == companyId && bg.FirstOrDefault().year == year
								   select new ComapnyAndYear {
									   companyId = c.Id,
									   companyName = c.companyName,
									   managerName = c.managerName,
									   year = Convert.ToInt32(bg.FirstOrDefault().year)
								   }).ToListAsync();
			return companies;
		}
		public async Task<ComapnyAndBOD> GetChairmenByCompanyIdAndYear(int companyId, int year)
		{
			var chairmen = await (from c in _context.Companies
								   join b in _context.boardOfDirectors
								   on c.Id equals b.companyId
								   where c.Id == companyId && b.year == year && b.status == 1
								   select new ComapnyAndBOD
								   {
									   company = c,
									   boardOfDirector = b
								   }).FirstOrDefaultAsync();
			return chairmen;
		}
		public async Task<IEnumerable<ComapnyAndBOD>> GetDirectorsByCompanyIdAndYear(int companyId, int year)
		{
			var directors = await (from c in _context.Companies
								   join b in _context.boardOfDirectors
								   on c.Id equals b.companyId
								   where c.Id == companyId && b.year == year && b.status == 2
								   select new ComapnyAndBOD {
									   company = c,
									   boardOfDirector = b
								   }).ToListAsync();
			return directors;
		}
		public async Task<ComapnyAndBOD> GetCeoByCompanyIdAndYear(int companyId, int year)
		{
			var ceo = await (from c in _context.Companies
								   join b in _context.boardOfDirectors
								   on c.Id equals b.companyId
								   where c.Id == companyId && b.year == year && b.status == 3
								   select new ComapnyAndBOD
								   {
									   company = c,
									   boardOfDirector = b
								   }).FirstOrDefaultAsync();
			return ceo;
		}
        public async Task<bool> DeleteBoardOfDirectorById(int companyId, int year)
        {
            var bods = await _context.boardOfDirectors.Where(x => x.companyId == companyId && x.year == year).ToListAsync();
            foreach (var item in bods)
            {
                _context.boardOfDirectors.Remove(_context.boardOfDirectors.Find(item.Id));
                await _context.SaveChangesAsync();
            }
            return true;
        }
        //public async Task<IEnumerable<ComapnyAndBOD>> DeletetDirectorsByCompanyIdAndYear(int companyId, int year)
        //{
        //    var directors = await (from c in _context.Companies
        //                           join b in _context.boardOfDirectors
        //                           on c.Id equals b.companyId
        //                           where c.Id == companyId && b.year == year && b.status == 2
        //                           select new ComapnyAndBOD
        //                           {
        //                               company = c,
        //                               boardOfDirector = b
        //                           }).ToListAsync();
        //    return directors;
        //}

    }
}
