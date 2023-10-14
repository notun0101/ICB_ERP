using OPUSERP.Data.Entity;
using OPUSERP.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Suspensions
{
	[Table("BoardOfDirector", Schema = "HR")]
	public class BoardOfDirector:Base
	{
		public string name { get; set; }
		public string designation { get; set; }
		public string mobile { get; set; }
		public string email { get; set; }
		public string address { get; set; }
		public string bio { get; set; }
		public string photoUrl { get; set; }

		public int companyId { get; set; }
		public Company company { get; set; }

		public int? year { get; set; }

		public int? isActive { get; set; }
		public int? status { get; set; }
		public DateTime? date { get; set; }
	}
}
