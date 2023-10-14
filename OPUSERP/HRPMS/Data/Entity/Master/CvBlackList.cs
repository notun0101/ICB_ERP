using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
	[Table("CvBlackList")]
	public class CvBlackList:Base
	{
		public string sscRoll { get; set; }
		public string sscReg { get; set; }
		public DateTime? date { get; set; }
		public string attatchment { get; set; }
		public string reason { get; set; }
	}
}
