using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
	[Table("HandoverTakeoverDetails")]
	public class HandoverTakeoverDetails:Base
	{
		public int? masterId { get; set; }
		public HandoverTakeoverMaster master { get; set; }

		public string taskName { get; set; }
		public string taskDetails { get; set; }

		public int? status { get; set; } //1=ongoing
		public string file { get; set; }
		public string comments { get; set; }
	}
}
