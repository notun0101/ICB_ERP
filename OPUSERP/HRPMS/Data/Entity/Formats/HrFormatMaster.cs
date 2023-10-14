using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Formats
{
	[Table("HrFormatMaster")]
	public class HrFormatMaster:Base
	{
		public string name { get; set; }
		public string type { get; set; } //NOC, Certificate
		public string body { get; set; }

		public int? status { get; set; }
		public int? isActive { get; set; }
		public string remarks { get; set; }
	}
}
