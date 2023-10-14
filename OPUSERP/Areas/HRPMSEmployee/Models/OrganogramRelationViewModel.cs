using OPUSERP.HRPMS.Data.Entity.Organogram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
	public class OrganogramRelationViewModel
	{
		public int? Id { get; set; }
		public string title { get; set; }
		public string subTitle { get; set; }
		public int? parentId { get; set; }
		public string url { get; set; }
		public IEnumerable<OrganogramChild> children { get; set; }
	}
}
