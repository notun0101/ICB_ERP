using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.CRM.Data.Entity.Team
{
	[Table("CRMTeamMember")]
	public class CRMTeamMember : Base
	{
		public int? cRMTeamMasterId { get; set; }
		public CRMTeamMaster cRMTeamMaster { get; set; }

		public int? memberId { get; set; }

		public int? isActive { get; set; }

		public int? shortOrder { get; set; }
	}
}
