﻿using OPUSERP.Areas.Auth.Models;
using OPUSERP.SCM.Data.Entity.MasterData;
using System.Collections.Generic;

namespace OPUSERP.Areas.SCMMasterData.Models
{
    public class TeamMemberViewModel
    {
        public int? teamMemberId { get; set; }
        public int? teamMasterId { get; set; }

        public string teamName { get; set; }
        public string memberName { get; set; }

        public IEnumerable<TeamMaster> teamMasters { get; set; }
        public IEnumerable<TeamListViewModel> AllTeams { get; set; }
        public IEnumerable<TeamMemberViewModel> teamMembers { get; set; }
        public IEnumerable<AspNetUsersViewModel> aspNetUsersViews { get; set; }
	//	public IEnumerable<TeamListViewModel> AllTeams { get; set; }
	}
	//public class TeamWithMasterVm
	//{
	//	public string TeamName { get; set; }
	//	public string LeaderName { get; set; }
	//	public string MemberName { get; set; }
	//}
}
