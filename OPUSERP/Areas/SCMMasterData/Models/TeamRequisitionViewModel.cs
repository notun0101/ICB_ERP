using OPUSERP.SCM.Data.Entity.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.SCMMasterData.Models
{
    public class TeamRequisitionViewModel
    {
        public int? teamId { get; set; }
        public int? memberId { get; set; }
        public string teamCode { get; set; }
        public string teamName { get; set; }
        public int? leaderId { get; set; }
        public string leaderName { get; set; }
        public string memberName { get; set; }
        public string leaderPhoto { get; set; }
        public string memberPhotoPhoto { get; set; }
        public int?[] teamMemberById { get; set; }
        public IEnumerable<TeamMember> teamMembers { get; set; }
    }
}
