using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Data.Entity
{
	[Table("MobileAllowanceStructure", Schema = "Payroll")]
	public class MobileAllowanceStructure : Base
	{
		public int? employeeInfoId { get; set; }
		public EmployeeInfo employeeInfo { get; set; }

		public int? salaryPeriodId { get; set; }
		public SalaryPeriod salaryPeriod { get; set; }

		public DateTime? effectiveDate { get; set; }
		public decimal? amount { get; set; }

		public int? isActive { get; set; } = 1;
	}
}
