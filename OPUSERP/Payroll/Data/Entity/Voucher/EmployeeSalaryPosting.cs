using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Data.Entity.Voucher
{
	[Table("EmployeeSalaryPosting")]
	public class EmployeeSalaryPosting:Base
	{
		public int? employeeId { get; set; }
		public EmployeeInfo employee { get; set; }
        public int? salaryHeadId { get; set; }
        public SalaryHead salaryHead { get; set; }

        public string accountNo { get; set; }

		public decimal? amount { get; set; }

		public int? salaryVoucherDetailsId { get; set; }
		public SalaryVoucherDetails salaryVoucherDetails { get; set; }

		public int? status { get; set; }
	}
}
