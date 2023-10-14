using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Data.Entity.Salary
{
	[Table("ProcessEmployeesMobileAndCarAllowance", Schema = "Payroll")]
	public class ProcessEmployeesMobileAndCarAllowance : Base
	{
		public int? employeeInfoId { get; set; }
		public EmployeeInfo employeeInfo { get; set; }

		public int? salaryPeriodId { get; set; }
		public SalaryPeriod salaryPeriod { get; set; }

		public string salaryHeadName { get; set; }
		public string empCode { get; set; }
		public string empName { get; set; }
		public string sbAccount { get; set; }

		public DateTime? effectiveDate { get; set; }
		public decimal? amount { get; set; }
	}
}
