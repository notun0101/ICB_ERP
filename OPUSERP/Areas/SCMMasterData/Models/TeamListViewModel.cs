using OPUSERP.CRM.Data.Entity.Team;
using OPUSERP.Data.Entity.MasterData;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.SCM.Data.Entity.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.SCMMasterData.Models
{
    public class TeamListViewModel
    {
        public int tmId { get; set; }
        public string teamName { get; set; }
        public string teamCode { get; set; }
        //public string teamleader { get; set; }
        public string leaderName { get; set; }
        public string mamberName { get; set; }
        public string[] mamber { get; set; }
        public int memberId { get; set; }
        public int teamMasterId { get; set; }


        public string empPhoto { get; set; }
        public string leaderphoto { get; set; }
        public int teamId { get; set; }

        //public IEnumerable<TeamListViewModel> GetTeams { get; set; }
        public IEnumerable<TeamMaster> teamMasters { get; set; }
        public IEnumerable<TeamMember> teamMembers { get; set; }
        public IEnumerable<CRMTeamMaster> crmTeamMasters { get; set; }
        public IEnumerable<CRMTeamMember> crmTeamMembers { get; set; }
    }
    public class TeamListViewModel1
    {
        public int teamId { get; set; }
        public int? reqDetailsId { get; set; }
        public string teamMembers { get; set; }
        public string LeaderName { get; set; }
        public string teamLeader{ get; set; }
        public string teamName { get; set; }
        public string teamCode { get; set; }
        public int? leaderId { get; set; }
        public string[] mamber { get; set; }
        public string mamberName { get; set; }
        public string leaderphoto { get; set; }
        public IEnumerable<EmployeeInfo> employeeInfos { get; set; }
        public IEnumerable<Photograph> Photographs { get; set; }

        public int? memberId { get; set; }
  
        public string leaderName { get; set; }
        public string memberName { get; set; }
        public string leaderPhoto { get; set; }
        public string memberPhotoPhoto { get; set; }
        public int?[] teamMemberById { get; set; }
        public IEnumerable<TeamMember> teamMembersList { get; set; }
        public IEnumerable<CRMTeamMember> crmTeamMembersList { get; set; }






    }

    public class TeamInfoViewModel
    {
        public IEnumerable<TeamListViewModel> TeamMembers { get; set; }
        public TeamMaster TeamMaster { get; set; }
        public IEnumerable<TeamListViewModel> TeamMasterlist { get; set; }
        public IEnumerable<TeamListViewModel> CRMTeamMembers { get; set; }
        public CRMTeamMaster CRMTeamMaster { get; set; }
        public IEnumerable<TeamListViewModel> CRMTeamMasterlist { get; set; }

    }

}
