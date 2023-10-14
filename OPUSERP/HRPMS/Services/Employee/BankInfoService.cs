using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Employee
{
	public class BankInfoService : IBankInfoService
	{
		private readonly ERPDbContext _context;

		public BankInfoService(ERPDbContext context)
		{
			_context = context;
		}

		public async Task<bool> DeleteBankInfoById(int id)
		{
			_context.bankInfos.Remove(_context.bankInfos.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<bool> DeleteEmpAccountInfoById(int id)
		{
			_context.employeeAccounts.Remove(_context.employeeAccounts.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}
		public async Task<bool> DeleteBankDiplomaById(int id)
		{
			var data = _context.bankingDiplomas.Find(id);
			_context.bankingDiplomas.Remove(data);
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<BankInfo>> GetBankInfo()
		{
			return await _context.bankInfos.AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<BankInfo>> GetBankInfoByEmpId(int empId)
		{
			return await _context.bankInfos.Where(x => x.employeeId == empId).Include(x => x.employee).Include(x => x.bank).Include(x => x.walletType).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<BankingDiploma>> GetBankDiplomaInfoByEmpId(int empId)
		{
			return await _context.bankingDiplomas.Where(x => x.employeeId == empId).Include(x => x.employee).AsNoTracking().ToListAsync();
		}

		public async Task<BankInfo> GetBankInfoById(int id)
		{
			return await _context.bankInfos.FindAsync(id);
		}

		public async Task<bool> SaveBankInfo(BankInfo bankInfo)
		{
			if (bankInfo.Id != 0)
				_context.bankInfos.Update(bankInfo);
			else
				_context.bankInfos.Add(bankInfo);

			await _context.SaveChangesAsync();
			return 1 == 1;
		}
		public async Task<bool> SaveEmployeeAccounts(EmployeeAccounts acInfo)
		{
			if (acInfo.Id != 0)
				_context.employeeAccounts.Update(acInfo);
			else
				_context.employeeAccounts.Add(acInfo);

			await _context.SaveChangesAsync();
			return 1 == 1;
		}
		public async Task<bool> SaveBankingDiplomaInfo(BankingDiploma bankingDiploma)
		{
			if (bankingDiploma.Id != 0)
				_context.bankingDiplomas.Update(bankingDiploma);
			else
				_context.bankingDiplomas.Add(bankingDiploma);

			return 1 == await _context.SaveChangesAsync();
		}
	}
}
