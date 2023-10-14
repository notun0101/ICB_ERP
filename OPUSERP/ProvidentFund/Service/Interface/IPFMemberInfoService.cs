//new
using OPUSERP.Budget.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.ProvidentFund.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.ProvidentFund.Service.Interface
{
    public interface IPFMemberInfoService
    {

        Task<bool> SavePFMemberInfo(PFMemberInfo pFMemberInfo);
        Task<IEnumerable<PFMemberInfo>> GetAllPFMemberInfo();
        Task<IEnumerable<LoanCollection>> GetLoanStatementByEmpCode(string empCode);
        Task<IEnumerable<PFMemberInfo>> GetPendingPFMemberInfo();
        Task<IEnumerable<PFMemberInfo>> GetApprovePFMemberInfo();
        Task<PFMemberInfo> GetPFMemberInfoById(int id);
        Task<PFMemberInfo> GetPFMemberInfoByEmployeeCode(string employeeCode);
        Task<bool> DeletePFMemberInfoById(int id);
        Task<bool> UpdateMemberAplicationStatus(int? id, int? status, string updateBy);
        Task<IEnumerable<SpecialBranchUnit>> GetAllSbu();
        Task<string> GetMemberInfoById(string code);
        Task<decimal> CalculateTotalContributionByMemberId(int id);
        Task<IEnumerable<PFMemberInfo>> GetNewPFMemberInfo();
        Task<int> getLastMemberCode();
    }
}



//Old
//using OPUSERP.Budget.Data.Entity;
//using OPUSERP.HRPMS.Data.Entity.Master;
//using OPUSERP.ProvidentFund.Data.Entity;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace OPUSERP.ProvidentFund.Service.Interface
//{
//   public interface IPFMemberInfoService
//    {
//        Task<PFMemberInfo> GetPFMemberInfoByEmployeeCode(string employeeCode);
//        Task<bool> SavePFMemberInfo(PFMemberInfo pFMemberInfo);
//        Task<IEnumerable<PFMemberInfo>> GetAllPFMemberInfo();
//        Task<IEnumerable<PFMemberInfo>> GetPendingPFMemberInfo();
//        Task<IEnumerable<PFMemberInfo>> GetApprovePFMemberInfo();
//        Task<PFMemberInfo> GetPFMemberInfoById(int id);
//        Task<bool> DeletePFMemberInfoById(int id);
//        Task<bool> UpdateMemberAplicationStatus(int? id, int? status, string updateBy);
//        Task<IEnumerable<SpecialBranchUnit>> GetAllSbu();
//        Task<string> GetMemberInfoById(string code);        
//        Task<decimal> CalculateTotalContributionByMemberId(int id);
//        Task<IEnumerable<PFMemberInfo>> GetNewPFMemberInfo();
//        Task<int> getLastMemberCode();
//    }
//}
