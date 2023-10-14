using OPUSERP.Data.Entity;
using OPUSERP.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
	[Table("DuelResidence", Schema = "HR")]
	public class DuelResidence:Base
	{
		public int employeeId { get; set; }
		public EmployeeInfo employee { get; set; }

		public int? duelCountryId { get; set; }
		public Country duelCountry { get; set; }

		public string duelPassport { get; set; }
	}
}
