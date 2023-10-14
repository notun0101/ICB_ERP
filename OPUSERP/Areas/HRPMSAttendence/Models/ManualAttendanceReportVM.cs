using OPUSERP.Areas.HRPMSLeave.Models;
using OPUSERP.Data.Entity.Master;
using System.Collections.Generic;

namespace OPUSERP.Areas.HRPMSAttendence.Models
{
	public class ManualAttendanceReportVM
	{
		public IEnumerable<Company> companies { get; set; }
		public IEnumerable<ManualAttendanceViewModel> ManualAttendanceViewModels { get; set; }
	}
}
