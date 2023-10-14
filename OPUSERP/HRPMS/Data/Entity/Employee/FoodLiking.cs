using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
	[Table("FoodLiking")]
	public class FoodLiking: Base
	{
		public int employeeId { get; set; }
		public EmployeeInfo employee { get; set; }

		public int? vegiterian { get; set; }
		public int? nonVegiterian { get; set; }
	}
}
