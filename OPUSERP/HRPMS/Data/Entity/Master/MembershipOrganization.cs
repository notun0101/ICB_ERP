using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
	[Table("MembershipOrganization")]
	public class MembershipOrganization : Base
	{
		[Required]
		public string organizationName { get; set; }
		public string organizationNameBn { get; set; }

		public string organizationType { get; set; }
	}
}
