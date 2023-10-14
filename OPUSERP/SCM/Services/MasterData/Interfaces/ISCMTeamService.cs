using OPUSERP.Areas.SCMMasterData.Models;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.SCM.Data.Entity.MasterData;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OPUSERP.SCM.Services.MasterData.Interfaces
{
    public interface ISCMTeamService
    {
        bool DuplicateData(string memberName);
        Task<int> SaveTeamMaster(TeamMaster teamMaster);
        Task<IEnumerable<TeamMaster>> GetAllTeamMaster();
        Task<TeamMaster> GetTeamMasterById(int id);
        Task<bool> DeleteTeamMasterById(int id);

        Task<int> SaveTeamMember(TeamMember teamMember);
        Task<IEnumerable<TeamMember>> GetAllTeamMember();
        Task<TeamMember> GetTeamMemberById(int id);
        Task<bool> DeleteTeamMemberById(int id);

        Task<IEnumerable<TeamListViewModel>> GetTeamMembersByTeamMasterId(int id);
        Task<IEnumerable<TeamListViewModel>> GetAllTeamList(int id);
        Task<IEnumerable<TeamListViewModel>> GetAllTeamInfoList();
        Task<IEnumerable<EmployeeInfo>> GetAllTeamMemberByMasterId1(int id);
        Task<IEnumerable<Photograph>> getAllphoto();
        Task<IEnumerable<TeamListViewModel>> GetAllTeamInfoList1();
        Task<string> getLeaderNameByLeaderId(int id);
        Task<string> getLeaderPhotoByLeaderId(int id);
		Task<IEnumerable<TeamListViewModel>> GetTeamMembersByTeamMasterIdNew(int id);
        Task<bool> DeleteTeamInfoById(int id);


    }
}
