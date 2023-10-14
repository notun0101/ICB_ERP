using OPUSERP.HRPMS.Data.Entity.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
	public class HrFormatDetailsViewModel
	{
		public int formatDetailsId { get; set; }
		public int formatMasterId { get; set; }
		public int employeeId { get; set; }
		public string editedFormat { get; set; }

		public IEnumerable<HrFormatMaster> hrFormatMasters { get; set; }
		public HrFormatDetails hrFormatDetail { get; set; }

		public int? status { get; set; }
		public int? isActive { get; set; }
		public string remarks { get; set; }
	}
}
