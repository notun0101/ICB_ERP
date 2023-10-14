using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
	[Table("EmployeeHobby", Schema = "HR")]
	public class EmployeeHobby:Base
	{
		public int employeeInfoId { get; set; }
		public EmployeeInfo employeeInfo { get; set; }

		public string hobbyName { get; set; }

		public int? isActive { get; set; }
		public int? status { get; set; }
		public DateTime? date { get; set; }
	}
}
