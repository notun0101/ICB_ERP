using OPUSERP.Areas.Auth.Models;
using OPUSERP.HRPMS.Data.Entity.Attendance;
using System.Collections;
using System.Collections.Generic;

namespace OPUSERP.Areas.HRPMSAttendence.Models
{
	public class LunchPageViewModel
    {
        public IEnumerable<AspNetUsersViewModel> userInfo { get; set; }
	}
}
