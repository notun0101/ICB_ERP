using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
	[Table("EmployeeLanguageSkill")]
	public class EmployeeLanguageSkill:Base
	{
		public int employeeInfoId { get; set; }
		public EmployeeInfo employeeInfo { get; set; }

		public string LanguageName { get; set; }
		public int? skill { get; set; } //percent
		public int? isNative { get; set; }

		public int? isActive { get; set; }
		public int? status { get; set; }
		public DateTime? date { get; set; }
	}
}
