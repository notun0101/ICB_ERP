using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Data.Entity.Salary
{
	[Table("BranchWiseSalaryLock")]
	public class BranchWiseSalaryLock:Base
	{
		public int? periodId { get; set; }
		public SalaryPeriod period { get; set; }

		public int? hrBranchId { get; set; }
		public HrBranch hrBranch { get; set; }

		public int? zoneId { get; set; }
		public Location zone { get; set; }

		public int? status { get; set; } = 0;
	}
}
