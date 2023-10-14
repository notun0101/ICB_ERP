using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Data.Entity.Salary
{
    [Table("SpecialBonusEmp", Schema = "Payroll")]
    public class SpecialBonusEmp:Base
    {
        public int? employeeId { get; set; }
        public string empName { get; set; }
        public string empCode { get; set; }

        public int? branchId { get; set; }
        public HrBranch branch { get; set; }

        public int? zoneId { get; set; }
        public Location zone { get; set; }

        public int? designationId { get; set; }
        public Designation designation { get; set; }

		public string designationName { get; set; }
		public string accountNo { get; set; }
		public string EmpStatus { get; set; } //Retired, Dead, Prl, Resign
		public string Sundry { get; set; } //Yes, No

		public decimal? basicSalary { get; set; }

        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }

        public int? totalDays { get; set; }

        public int? isActive { get; set; }
    }
}
