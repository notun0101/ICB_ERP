using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Data.Entity.Salary
{
	[Table("PartialSalaryLog", Schema = "Payroll")]
	public class PartialSalaryLog:Base
	{
		public int? employeeInfoId { get; set; }
		public EmployeeInfo employeeInfo { get; set; }

		public int? salaryPeriodId { get; set; }
		public SalaryPeriod salaryPeriod { get; set; }

		public DateTime? fromDate { get; set; }
		public DateTime? toDate { get; set; }
		public int? totalDays { get; set; }

		public int? type { get; set; } //1=new emp, 2=prl
		public int? status { get; set; }
	}
}
