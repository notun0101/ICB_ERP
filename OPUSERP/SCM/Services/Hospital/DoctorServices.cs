using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.SCM.Services.Hospital.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.SCM.Services.Hospital
{
	public class DoctorServices:IDoctor
	{
		private readonly ERPDbContext _context;

		public DoctorServices(ERPDbContext context)
		{
			_context = context;
		}

		public async Task<bool> DeletDoctorInfoById(int id)
		{
			_context.EmployeeContractInfos.Remove(_context.EmployeeContractInfos.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<EmployeeContractInfo>> GetDoctorInfoByDoctId(int empId)
		{
			return await _context.EmployeeContractInfos.Where(x => x.employeeId == empId).Include(x => x.employee).AsNoTracking().ToListAsync();
		}

		public async Task<EmployeeContractInfo> GetDoctorInfoById(int id)
		{
			return await _context.EmployeeContractInfos.FindAsync(id);
		}

		public async Task<bool> SaveDoctorInfo(EmployeeContractInfo DoctorInfo)
		{
			if (DoctorInfo.Id != 0)
				_context.EmployeeContractInfos.Update(DoctorInfo);
			else
				_context.EmployeeContractInfos.Add(DoctorInfo);

			return 1 == await _context.SaveChangesAsync();
		}
	}
}
