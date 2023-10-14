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
	public class HrBudgetService:IHrBudget
	{
		private readonly ERPDbContext _context;

		public HrBudgetService(ERPDbContext context)
		{
			_context = context;
		}

		public async Task<bool> SaveHrBudget(HrBudgetDpt hrBudget)
		{
			if (hrBudget.Id != 0)
				_context.hrBudgetDpts.Update(hrBudget);
			else
				_context.hrBudgetDpts.Add(hrBudget);
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<HrBudgetDpt>> GetAllHrBudget()
		{
			return await _context.hrBudgetDpts.Include(a => a.department).Include(a => a.designation).AsNoTracking().ToListAsync();
		}

		public async Task<HrBudgetDpt> GetHrBudgetById(int id)
		{
			return await _context.hrBudgetDpts.FindAsync(id);
		}

		public async Task<bool> DeleteHrBudgetById(int id)
		{
			_context.hrBudgetDpts.Remove(_context.hrBudgetDpts.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}



		public async Task<IEnumerable<HrBudgetDpt>> GetAllDepartments()
		{
			return await _context.hrBudgetDpts.Include(x => x.department).Include(x => x.designation).GroupBy(x => x.departmentId).Select(x => x.First()).ToListAsync();
		}

		public async Task<IEnumerable<Department>> GetAllHrDepartments()
		{
			return await _context.departments.ToListAsync();
		}
		public async Task<IEnumerable<HrBranch>> GetAllHrBranch()
		{
			return await _context.hrBranches.ToListAsync();
		}
		public async Task<IEnumerable<Designation>> GetAllHrDesignations()
		{
			return await _context.designations.ToListAsync();
		}

		public async Task<IEnumerable<HrBudgetDpt>> GetHrBudgetsByDeptId(int deptId)
		{
			return await _context.hrBudgetDpts.Include(x => x.department).Include(x => x.designation).Where(x => x.departmentId == deptId).ToListAsync();
		}
	}
}
