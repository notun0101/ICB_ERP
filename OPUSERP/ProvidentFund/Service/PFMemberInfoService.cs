//New
using Microsoft.EntityFrameworkCore;
using OPUSERP.Budget.Data.Entity;
using OPUSERP.Budget.Service.Interface;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.ProvidentFund.Data.Entity;
using OPUSERP.ProvidentFund.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//ProvidentFund
namespace OPUSERP.ProvidentFund.Service
{
    public class PFMemberInfoService : IPFMemberInfoService
    {
        private readonly ERPDbContext _context;

        public PFMemberInfoService(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SavePFMemberInfo(PFMemberInfo pFMemberInfo)
        {
            if (pFMemberInfo.Id != 0)
                _context.pfMemberInfos.Update(pFMemberInfo);
            else
                _context.pfMemberInfos.Add(pFMemberInfo);

            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PFMemberInfo>> GetAllPFMemberInfo()
        {
            return await _context.pfMemberInfos.Include(x => x.employeeInfo).Include(x => x.employeeInfo).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<LoanCollection>> GetLoanStatementByEmpCode(string empCode)
        {
            return await _context.loanCollections.Include(x => x.loanManagement).Include(x => x.pfmember).Where(x => x.loanManagement.EmployeeInfo.employeeCode == empCode).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<PFMemberInfo>> GetPendingPFMemberInfo()
        {
            return await _context.pfMemberInfos.Where(x => x.approveStatus == 0).Include(x => x.employeeInfo).Include(x => x.employeeInfo.lastDesignation).Include(x => x.employeeInfo.department).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<PFMemberInfo>> GetNewPFMemberInfo()
        {
            return await _context.pfMemberInfos.Where(x => x.approveStatus == 2 && Convert.ToDateTime(x.applicationDate).Year == DateTime.Now.Year).Include(x => x.employeeInfo).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<PFMemberInfo>> GetApprovePFMemberInfo()
        {
            var data = new List<PFMemberInfo>();
            try
            {
                data = await _context.pfMemberInfos.Include(x => x.employeeInfo).Where(x => x.approveStatus == 2).Include(x => x.employeeInfo).AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {

            }
            return data;
        }
        public async Task<decimal> CalculateTotalContributionByMemberId(int id)
        {
            var data = await _context.pFContributions.Where(x => x.pfmemberId == id).SumAsync(x => x.employeeContribution);
            return Convert.ToDecimal(data);
        }

        public async Task<PFMemberInfo> GetPFMemberInfoById(int id)
        {
            var data= await _context.pfMemberInfos.Include(x => x.employeeInfo).Include(x => x.employeeInfo.lastDesignation).Include(x => x.employeeInfo.department).Where(x => x.Id == id).FirstOrDefaultAsync();
            return data;
        }

        public async Task<PFMemberInfo> GetPFMemberInfoByEmployeeCode(string employeeCode)
        {
            return await _context.pfMemberInfos.Include(x => x.employeeInfo).Where(x => x.employeeInfo.employeeCode == employeeCode).FirstOrDefaultAsync();
        }

        public async Task<bool> DeletePFMemberInfoById(int id)
        {
            _context.pfMemberInfos.Remove(_context.pfMemberInfos.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateMemberAplicationStatus(int? id, int? status, string updateBy)
        {
            var pfMemberInfos = _context.pfMemberInfos.Find(id);
            pfMemberInfos.approveStatus = status;
            pfMemberInfos.updatedBy = updateBy;
            pfMemberInfos.updatedAt = DateTime.Now;

            _context.Entry(pfMemberInfos).State = EntityState.Modified;

            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<SpecialBranchUnit>> GetAllSbu()
        {
            return await _context.SpecialBranchUnits.AsNoTracking().ToListAsync();
        }

        public async Task<string> GetMemberInfoById(string code)
        {
            var data = await _context.pfMemberInfos.Where(x => x.memberCode == code).Select(x => x.memberCode).AsNoTracking().FirstOrDefaultAsync();
            return data;
        }

        public async Task<int> getLastMemberCode()
        {
            var result = await _context.pfMemberInfos.MaxAsync(x => x.memberCode);
            return Convert.ToInt32(result);
        }




    }
}


//Old
//using Microsoft.EntityFrameworkCore;
//using OPUSERP.Budget.Data.Entity;
//using OPUSERP.Budget.Service.Interface;
//using OPUSERP.Data;
//using OPUSERP.HRPMS.Data.Entity.Master;
//using OPUSERP.ProvidentFund.Data.Entity;
//using OPUSERP.ProvidentFund.Service.Interface;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

////ProvidentFund
//namespace OPUSERP.ProvidentFund.Service
//{
//    public class PFMemberInfoService : IPFMemberInfoService
//    {
//        private readonly ERPDbContext _context;

//        public PFMemberInfoService(ERPDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<PFMemberInfo> GetPFMemberInfoByEmployeeCode(string employeeCode)
//        {
//            return await _context.pfMemberInfos.Include(x => x.department).Include(x => x.designation).Include(x => x.memberType).Where(x => x.employeeInfo.employeeCode == employeeCode).FirstOrDefaultAsync();
//        }
//        public async Task<bool> SavePFMemberInfo(PFMemberInfo pFMemberInfo)
//        {
//            if (pFMemberInfo.Id != 0)
//                _context.pfMemberInfos.Update(pFMemberInfo);
//            else
//                _context.pfMemberInfos.Add(pFMemberInfo);

//            return 1 == await _context.SaveChangesAsync();
//        }

//        public async Task<IEnumerable<PFMemberInfo>> GetAllPFMemberInfo()
//        {
//            return await _context.pfMemberInfos.Include(x=>x.designation).Include(x=>x.department).Include(x=>x.employeeInfo).AsNoTracking().ToListAsync();
//        }

//        public async Task<IEnumerable<PFMemberInfo>> GetPendingPFMemberInfo()
//        {
//            return await _context.pfMemberInfos.Where(x=>x.approveStatus==0).Include(x => x.designation).Include(x => x.department).AsNoTracking().ToListAsync();
//        }

//        public async Task<IEnumerable<PFMemberInfo>> GetNewPFMemberInfo()
//        {
//            return await _context.pfMemberInfos.Where(x => x.approveStatus == 2 && Convert.ToDateTime(x.applicationDate).Year==DateTime.Now.Year).Include(x => x.designation).Include(x => x.department).AsNoTracking().ToListAsync();
//        }
//        public async Task<IEnumerable<PFMemberInfo>> GetApprovePFMemberInfo()
//        {
//            return await _context.pfMemberInfos.Where(x=>x.approveStatus==2).Include(x => x.designation).Include(x => x.department).AsNoTracking().ToListAsync();
//        }
//        public async Task<decimal> CalculateTotalContributionByMemberId(int id)
//        {
//            var data = await _context.pFContributions.Where(x => x.pfmemberId == id).SumAsync(x => x.employeeContribution);
//            return Convert.ToDecimal(data);
//        }

//        public async Task<PFMemberInfo> GetPFMemberInfoById(int id)
//        {
//            return await _context.pfMemberInfos.Include(x=>x.department).Include(x=>x.designation).Include(x=>x.memberType).Where(x=>x.Id==id).FirstOrDefaultAsync();
//        }

//        public async Task<bool> DeletePFMemberInfoById(int id)
//        {
//            _context.pfMemberInfos.Remove(_context.pfMemberInfos.Find(id));
//            return 1 == await _context.SaveChangesAsync();
//        }

//        public async Task<bool> UpdateMemberAplicationStatus(int? id, int? status, string updateBy)
//        {
//            var pfMemberInfos = _context.pfMemberInfos.Find(id);
//            pfMemberInfos.approveStatus = status;
//            pfMemberInfos.updatedBy = updateBy;
//            pfMemberInfos.updatedAt = DateTime.Now;

//            _context.Entry(pfMemberInfos).State = EntityState.Modified;

//            return 1 == await _context.SaveChangesAsync();
//        }

//        public async Task<IEnumerable<SpecialBranchUnit>> GetAllSbu()
//        {
//            return await _context.SpecialBranchUnits.AsNoTracking().ToListAsync();
//        }

//        public async Task<string> GetMemberInfoById(string code)
//        {
//            var data = await _context.pfMemberInfos.Where(x => x.memberCode == code).Select(x => x.memberCode).AsNoTracking().FirstOrDefaultAsync();
//            return data;
//        }

//        public async Task<int> getLastMemberCode()
//        {
//            var result = await _context.pfMemberInfos.MaxAsync(x => x.memberCode);
//            return Convert.ToInt32(result);
//        }


//    }
//}
