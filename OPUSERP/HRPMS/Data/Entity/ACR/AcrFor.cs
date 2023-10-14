using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.ACR
{
	[Table("AcrFor", Schema = "HR")]
	public class AcrFor:Base
	{
		[MaxLength(150)]
		public string acrForName { get; set; }
		[MaxLength(150)]
		public string acrForNameBn { get; set; }
		public int? shortOrder { get; set; }
	}
}
