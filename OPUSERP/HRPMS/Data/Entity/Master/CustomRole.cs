using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
	[Table("CustomRole", Schema = "HR")]
	public class CustomRole:Base
	{
		public string roleName { get; set; }
		public string roleNameBn { get; set; }
		public string remarks { get; set; }
	}
}
