using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.HrComputerLiteracy
{
	[Table("HrComputerLiteracy")]
	public class HrComputerLiteracy:Base
	{
		public string subject { get; set; }
		//public ComputerSubject subject { get; set; }

		public int employeeId { get; set; }
		public EmployeeInfo employee { get; set; }

		public string competencyLevel { get; set; } //Beginer, Intermediate, Advance
		public string training { get; set; }
		public string diploma { get; set; }

		public int? status { get; set; }
		public int? isActive { get; set; }
		public string remarks { get; set; }
	}
}
