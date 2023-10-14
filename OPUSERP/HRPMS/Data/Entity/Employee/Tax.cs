using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
	[Table("Tax")]
	public class Tax:Base
	{
		public int employeeId { get; set; }
		public EmployeeInfo employee { get; set; }

		[MaxLength(150)]
		public string taxZone { get; set; }
		[MaxLength(150)]
		public string taxCircle { get; set; }

		public string eTin { get; set; }
	}
}
