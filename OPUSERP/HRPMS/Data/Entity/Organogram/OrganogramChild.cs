using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Organogram
{
	[Table("OrganogramChild")]
	public class OrganogramChild:Base
	{
		public int? designationId { get; set; }
		public Designation designation { get; set; }

		public int? memberCount { get; set; }

		public int? OrgRelationId { get; set; }
		public OrganogramRelation OrgRelation { get; set; }
	}
}
