using OPUSERP.Areas.ProvidentFund.Models;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.ProvidentFund.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.ProvidentFund.Service.Interface
{
    public interface IPFTransactionService
    {
        Task<IEnumerable<PfTransaction>> GetAllPfTransaction();
        Task<PfAccountsSchedule> GetPfAccountsByMemberAndYearId(int memberId, int yearId);
        Task<int> RemovePfAccountsById(int id);
        Task<IEnumerable<PfTransaction>> GetAllPfTransactionByMemberAndYearId(int memberId, int yearId);
        Task<int> SavePfAccountsSchedule(PfAccountsSchedule model);
        Task<IEnumerable<PfAccountsSchedule>> GetAllPfAccountsSchedule();
        Task<IEnumerable<PfAccountsSchedule>> GetAllPfAccountsScheduleByYear(string year);
        Task<int> ProcessPfAccountInterestByEmp(string year, int memberId, decimal interest);
        //Task<decimal> GetBalanceByTotalMonth();
        Task<IEnumerable<PFTotalPfAmountByYear>> GetTotalPfAmountByYear(int year);
        Task<IEnumerable<EmployeeWithPfMember>> GetEmployeesHaveSalaryStructure();
        Task<IEnumerable<PFMemberInfo>> GetPfMembersByPeriodIdAndYearId(int salaryPeriodId, int yearId);
        Task<IEnumerable<PfTransaction>> GetTransactionDetailsByPeriodId(int salaryPeriodId, int yearId);
    }
}
