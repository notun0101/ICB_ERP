using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
	[Table("EmployeeDeath", Schema = "HR")]
	public class EmployeeDeath:Base
	{
		public int? employeeInfoId { get; set; }
		public EmployeeInfo employeeInfo { get; set; }

		public DateTime? date { get; set; }
		public string reason { get; set; } //1=Normal, 2=Accidental
	}
}
