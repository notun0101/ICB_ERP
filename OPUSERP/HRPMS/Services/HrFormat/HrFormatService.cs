using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Formats;
using OPUSERP.HRPMS.Services.HrFormat.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.HrFormat
{
	public class HrFormatService: IHrFormat
	{
		private readonly ERPDbContext _context;

		public HrFormatService(ERPDbContext context)
		{
			_context = context;
		}

		public async Task<bool> SaveHrFormatMaster(HrFormatMaster hrFormatMaster)
		{
			if (hrFormatMaster.Id > 0)
				_context.hrFormatMasters.Update(hrFormatMaster);
			else
				_context.hrFormatMasters.Add(hrFormatMaster);
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<int> SaveHrFormatDetails(HrFormatDetails hrFormatDetails)
		{
			if (hrFormatDetails.Id != 0)
				_context.hrFormatDetails.Update(hrFormatDetails);
			else
				_context.hrFormatDetails.Add(hrFormatDetails);
			await _context.SaveChangesAsync();
			return hrFormatDetails.Id;
		}

		public async Task<IEnumerable<HrFormatMaster>> GetAllHrFormatMaster()
		{
			return await _context.hrFormatMasters.AsNoTracking().ToListAsync();
		}

		public async Task<HrFormatMaster> GetHrFormatMasterById(int id)
		{
			return await _context.hrFormatMasters.FindAsync(id);
		}
		public async Task<HrFormatDetails> GetFormatDetailsById(int id)
		{
			return await _context.hrFormatDetails.FindAsync(id);
		}

		public async Task<bool> DeleteHrFormatMasterById(int id)
		{
			_context.hrFormatMasters.Remove(_context.hrFormatMasters.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<HrFormatMaster> GetFormatByTypeId(string type)
		{
			return await _context.hrFormatMasters.Where(x => x.type == type).FirstOrDefaultAsync();
		}
		public async Task<EmployeeInfo> GetEmployeeInfoById(int id)
		{
			return await _context.employeeInfos.Include(x => x.department).Include(x => x.lastDesignation).Where(x => x.Id == id).FirstOrDefaultAsync();
		}
		public async Task<HrFormatMaster> GetHrFormatMasterByType(string type)
		{
			return await _context.hrFormatMasters.Where(x => x.type == type).FirstOrDefaultAsync();
		}
	}
}
