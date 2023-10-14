using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
	public class AllEmployeeJson
	{
		public int employeeId { get; set; }
		public string employeeCode { get; set; }
		public string nameEnglish { get; set; }
		public string designation { get; set; }
		public string department { get; set; }
		public string branch { get; set; }
		public string division { get; set; }
		public string mobileNumberOffice { get; set; }
		public string emailAddress { get; set; }
		public string imageUrl { get; set; }
		public string companies { get; set; }
		public int? activityStatus { get; set; }
		public string lastDesignation { get; set; }
		public string action { get; set; }
	}

	public class AllEmployeeJsonNew
	{
		public int employeeId { get; set; }
		public string employeeCode { get; set; }
		public string nameEnglish { get; set; }
		public string designation { get; set; }
		public string gender { get; set; }
		public string department { get; set; }
		public string branch { get; set; }
		public string division { get; set; }
		public string mobileNumberOffice { get; set; }
		public string emailAddress { get; set; }
		public string imageUrl { get; set; }
		public string companies { get; set; }
		public int? activityStatus { get; set; }
		public string lastDesignation { get; set; }
		public DateTime? joiningDateGovtService { get; set; }

		public string Places { get; set; }
	}
    public class AllEmployeeJsonNewForPresent
	{
		public int employeeId { get; set; }
		public string employeeCode { get; set; }
		public string nameEnglish { get; set; }
		public string designation { get; set; }
		public string gender { get; set; }
		public string department { get; set; }
		public string branch { get; set; }
		public string division { get; set; }
		public string mobileNumberOffice { get; set; }
		public string emailAddress { get; set; }
		public string startTime { get; set; }
		public string endTime { get; set; }
		public string imageUrl { get; set; }
		public string companies { get; set; }
		public int? activityStatus { get; set; }
		public string lastDesignation { get; set; }
		public DateTime? joiningDateGovtService { get; set; }

		public string Places { get; set; }
	}

      public class AllEmployeeJsonForLeave
	{
		public int employeeId { get; set; }
		public string employeeCode { get; set; }
		public string nameEnglish { get; set; }

		public string designation { get; set; }

		public string gender { get; set; }
		public string department { get; set; }
		public string branch { get; set; }
		public string division { get; set; }

		public string mobileNumberOffice { get; set; }
		public string emailAddress { get; set; }
		
		public string imageUrl { get; set; }
		public string companies { get; set; }
		public int? activityStatus { get; set; }

		public string lastDesignation { get; set; }

		public DateTime? joiningDateGovtService { get; set; }
		public DateTime? leaveFrom { get; set; }
		public DateTime? leaveTo { get; set; }

		public string Places { get; set; }
	}




}
