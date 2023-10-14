using OPUSERP.Data.Entity;
using OPUSERP.Data.Entity.MasterData;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Attendance
{
	[Table("EmployeeAllocation", Schema = "HR")]
	public class EmployeeAllocation:Base
	{
		public int? dutyId { get; set; }
		public SpecialEventDutyMaster duty { get; set; }

		public DateTime? fromDate { get; set; }
		public DateTime? toDate { get; set; }
		public string location { get; set; }

		public int? employeeInfoId { get; set; }
		public EmployeeInfo employeeInfo { get; set; }
	}
}
