using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Data.Entity.Arrear
{
    [Table("EmployeeArrearInfo")]
    public class EmployeeArrearInfo : Base
    {
        public int? employeeId { get; set; }
        public EmployeeInfo employee { get; set; }

		public string employeeName { get; set; }
		public string Designation { get; set; }
		public string posting { get; set; }

		public int? periodId { get; set; }
		public SalaryPeriod period { get; set; }

		public DateTime? Fromdate { get; set; }
		public DateTime? ToDate { get; set; }
		public decimal? amount { get; set; }
		public decimal? CalculatedArrear { get; set; }
		public decimal? oldBasic { get; set; }
		public decimal? currentBasic { get; set; }
		public decimal? houseRent { get; set; }
		public decimal? subs { get; set; }

		public int? status { get; set; }
		public int? type { get; set; } = 1; //custom = 2, regular = 1
	}
}
