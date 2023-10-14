using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.MasterData
{
    public class FunctionsInfoService : IFunctionsInfoService
    {
        private readonly ERPDbContext _context;

        public FunctionsInfoService(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FunctionInfo>> GetFunctionInfo()
        {
            return await _context.FunctionInfos.Include(x=>x.company).Include(x=>x.specialBranchUnit).OrderBy(x => x.branchUnitName).AsNoTracking().ToListAsync();
        }

        public async Task<FunctionInfo> GetFunctionInfoById(int id)
        {
            return await _context.FunctionInfos.FindAsync(id);
        }

       
        public async Task<bool> SaveFunctionInfo(FunctionInfo functionInfo)
        {
            if (functionInfo.Id != 0)
                _context.FunctionInfos.Update(functionInfo);
            else
                _context.FunctionInfos.Add(functionInfo);
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteFunctionInfoById(int id)
        {
            _context.FunctionInfos.Remove(_context.FunctionInfos.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Company>> GetAllCompany()
        {
            var result = await _context.Companies?.OrderBy(a => a.Id).Take(1).AsNoTracking().ToListAsync();
         
            return result;
        }
        public async Task<EmployeePostingPlace> GetpresentPosting(int empid)
        {
            var result = await _context.employeePostings?.Where(x=> x.employeeId == empid && x.endDate == null).OrderBy(x => Convert.ToDateTime(x.startDate).Date).AsNoTracking().LastOrDefaultAsync();

            return result;
        }
    }
}
