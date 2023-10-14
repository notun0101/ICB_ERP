using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Employee.Interfaces
{
    public interface IBankInfoService
    {
        Task<bool> SaveBankInfo(BankInfo bankInfo);
        Task<IEnumerable<BankInfo>> GetBankInfo();
        Task<BankInfo> GetBankInfoById(int id);
        Task<bool> DeleteBankInfoById(int id);
        Task<IEnumerable<BankInfo>> GetBankInfoByEmpId(int empId);
        Task<IEnumerable<BankingDiploma>> GetBankDiplomaInfoByEmpId(int empId);
        Task<bool> SaveBankingDiplomaInfo(BankingDiploma bankingDiploma);
        Task<bool> DeleteBankDiplomaById(int id);
		Task<bool> SaveEmployeeAccounts(EmployeeAccounts acInfo);
		Task<bool> DeleteEmpAccountInfoById(int id);
	}
}
