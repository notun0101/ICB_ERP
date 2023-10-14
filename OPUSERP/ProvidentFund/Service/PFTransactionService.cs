using Microsoft.EntityFrameworkCore;
using OPUSERP.Areas.ProvidentFund.Models;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.ProvidentFund.Data.Entity;
using OPUSERP.ProvidentFund.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.ProvidentFund.Service
{
    public class PFTransactionService: IPFTransactionService
    {
        private readonly ERPDbContext _context;

        public PFTransactionService(ERPDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<PfTransaction>> GetAllPfTransaction()
        {
            return await _context.pfTransactions.Include(x=>x.employee).Include(x=>x.year).Include(x => x.pfMember).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<PfAccountsSchedule>> GetAllPfAccountsSchedule()
        {
            return await _context.pfAccountsSchedules.Include(x => x.employee).Include(x=>x.pfMember).Include(x => x.employee.lastDesignation).AsNoTracking().ToListAsync();
        }

        public async Task<PfAccountsSchedule> GetPfAccountsByMemberAndYearId(int memberId, int yearId)
        {
            var year = await _context.salaryYears.FindAsync(yearId);
            var data = await _context.pfAccountsSchedules.Where(x => x.pfMemberId == memberId && x.year == year.yearName).FirstOrDefaultAsync();
            return data;
        }
        public async Task<IEnumerable<PFMemberInfo>> GetPfMembersByPeriodIdAndYearId(int salaryPeriodId, int yearId)
        {
            var data = await _context.pfTransactions.Where(x => x.salaryPeriodId == salaryPeriodId && x.yearId == yearId).Select(x => x.pfMember).ToListAsync();
            return data;
        }
        public async Task<IEnumerable<PfTransaction>> GetTransactionDetailsByPeriodId(int salaryPeriodId, int yearId)
        {
            var data = await _context.pfTransactions.Include(x => x.employee).Include(x => x.pfMember).Include(x => x.salaryPeriod).Where(x => x.salaryPeriodId == salaryPeriodId && x.yearId == yearId).ToListAsync();
            return data;
        }
        public async Task<int> RemovePfAccountsById(int id)
        {
            _context.pfAccountsSchedules.Remove(await _context.pfAccountsSchedules.FindAsync(id));
            return await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<PfTransaction>> GetAllPfTransactionByMemberAndYearId(int memberId, int yearId)
        {
            var data = await _context.pfTransactions.Include(x => x.year).Where(x => x.pfMemberId == memberId && x.yearId == yearId).ToListAsync();
            return data;
        }
        public async Task<int> SavePfAccountsSchedule(PfAccountsSchedule model)
        {
            if (model.Id != 0)
            {
                _context.pfAccountsSchedules.Update(model);
            }
            else
            {
                _context.pfAccountsSchedules.Add(model);
            }
            await _context.SaveChangesAsync();
            return model.Id;
        }

        public async Task<IEnumerable<PfAccountsSchedule>> GetAllPfAccountsScheduleByYear(string year)
        {
            return await _context.pfAccountsSchedules.Include(x => x.employee).Include(x => x.pfMember).Include(x => x.employee.lastDesignation).Where(x => x.year == year).AsNoTracking().ToListAsync();
        }

        public async Task<int> ProcessPfAccountInterestByEmp(string year, int memberId, decimal interest)
        {
            _context.Database.ExecuteSqlCommand($"sp_ProcessPfAccountByYearMemberIdAndInterestAmount {year}, {memberId}, {interest}");
            return 1;
        }

        //public async Task<decimal> GetBalanceByTotalMonth()
        //{
        //    var data = await _context.pfAccountsSchedules.SumAsync(x =>(decimal?)x.january ?? 0 + x.february + x.march + x.april + x.may + x.june +x.july + x.august + x.september + x.october + x.november + x.december);
        //    var data= 
        //    return Convert.ToDecimal(data);
        //}

        public async Task<IEnumerable<PFTotalPfAmountByYear>> GetTotalPfAmountByYear(int year)
        {
            var data =  _context.pFTotalPfAmountByYears.FromSql($"sp_TotalPfAmountByYear @p",year);
            return data;
        }

        public async Task<IEnumerable<EmployeeWithPfMember>> GetEmployeesHaveSalaryStructure()
        {
            var data = await (from e in _context.employeeInfos
                              join s in _context.employeesSalaryStructures
                              on e.Id equals s.employeeInfoId
                              join p in _context.pfMemberInfos
                              on e.Id equals p.employeeInfoId
                              select new EmployeeWithPfMember{
                                  employee = e,
                                  pFMember = p
                              }).Distinct().ToListAsync();
            return data;
        }
    }
}
