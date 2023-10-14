using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Models
{
	public class SalaryPostingLog:Base
	{
        public int? periodId { get; set; }
        public SalaryPeriod period { get; set; }

        public int? hrBranchId { get; set; }
		public HrBranch hrBranch { get; set; }

		public int? zoneId { get; set; }
		public Location zone { get; set; }

		public int? officeId { get; set; }
		public FunctionInfo office { get; set; }

		public string code { get; set; }

		public string type { get; set; } //VOUCHER, SALARY, OFFICE_UNION, SALARY65, SALARY66, SALARY67
		public string key { get; set; }
		public DateTime? postingDate { get; set; } = DateTime.Now;
		public string successStatus { get; set; } //Y/N
        public string refCode { get; set; }
        public string message { get; set; }

        public string remarks { get; set; }
	}
}
