using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Data.Entity.PF
{
	[Table("PFSalaryContribution")]
	public class PFSalaryContribution:Base
	{
		public int? employeeId { get; set; }
		public EmployeeInfo employee { get; set; }

		public string employeeCode { get; set; }

		public string year { get; set; }
		public string month { get; set; }
		public decimal? pfOwn { get; set; }
		public decimal? pfCompany { get; set; }
	}
}
