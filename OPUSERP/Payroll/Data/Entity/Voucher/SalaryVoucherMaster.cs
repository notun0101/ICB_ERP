using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Data.Entity.Voucher
{
	[Table("SalaryVoucherMaster", Schema = "Payroll")]
	public class SalaryVoucherMaster:Base
	{
		public string particular { get; set; }

		public string accountCode { get; set; }
		public string type { get; set; } //Debit, Credit
		public decimal? amount { get; set; }

		public int? status { get; set; }

		public int? salaryPeriodId { get; set; }
		public SalaryPeriod salaryPeriod { get; set; }

		public int? hrBranchId { get; set; }
		public HrBranch hrBranch { get; set; }

		public int? departmentId { get; set; }
		public Department department { get; set; }

		public int? hrUnitId { get; set; }
		public HrUnit hrUnit { get; set; }

		public int? zoneId { get; set; }
		public Location zone { get; set; }

		public int? officeId { get; set; }
		public FunctionInfo office { get; set; }

		public int? divisionId { get; set; }
		public HrDivision division { get; set; }

		public string uniqueId { get; set; }
	}
}
