using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.CRM.Data.Entity.Team
{
	[Table("CRMTeamMaster ", Schema = "CRM")]
	public class CRMTeamMaster : Base
	{
		[StringLength(50)]
		public string teamCode { get; set; }

		[StringLength(250)]
		public string teamName { get; set; }

		public int? leaderId { get; set; }

		public int? isActive { get; set; }

		public int? shortOrder { get; set; }
		[NotMapped]
		public string teamLeader { get; set; }
		[NotMapped]
		public int? userId { get; set; }
	}
}
