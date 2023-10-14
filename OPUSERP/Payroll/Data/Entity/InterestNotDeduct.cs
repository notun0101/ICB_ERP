using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Data.Entity
{
	[Table("InterestNotDeduct", Schema = "Payroll")]
	public class InterestNotDeduct:Base
	{
		public string empCode { get; set; }

		public int? empId { get; set; }
		public EmployeeInfo emp { get; set; }

		public string loanType { get; set; } //MCA,MCAR,CL,HBA-B13,HBA-A13

		public int? status { get; set; } = 1;
	}
}
