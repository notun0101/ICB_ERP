using OPUSERP.HRPMS.Data.Entity.Leave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSLeave.Models
{
	public class LeaveViewModel
	{
		public IEnumerable<LeaveRegister> leaveRegisters { get; set; }
        
	}
}
