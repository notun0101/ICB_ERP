using OPUSERP.HRPMS.Data.Entity.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
	public class HrFormatMasterViewModel
	{
		public int formatMasterId { get; set; }
		public string name { get; set; }
		public string type { get; set; } //NOC, Certificate
		public string body { get; set; }

		public int? status { get; set; }
		public int? isActive { get; set; }
		public string remarks { get; set; }
		public IEnumerable<HrFormatMaster> hrFormatMasters{ get; set; }
	}
}
