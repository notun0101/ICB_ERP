using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Payroll.Data.Entity.Arrear;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Payroll.Models
{
    public class EmployeeArrearViewModel
    {
        public int employeeArrearId { get; set; }
        public int employeeInfoId { get; set; }
		public string employeeName { get; set; }
		public string Designation { get; set; }
		public string posting { get; set; }
		public int? periodId { get; set; }

		public DateTime? Fromdate { get; set; }
		public DateTime? ToDate { get; set; }
		public decimal? amount { get; set; }
		public decimal? CalculatedArrear { get; set; }
		public decimal? oldBasic { get; set; }
		public decimal? currentBasic { get; set; }
		

        public IEnumerable<EmployeeArrear> employeeArrears { get; set; }
        public IEnumerable<SalaryPeriod> salaryPeriods { get; set; }
        public IEnumerable<HrBranch> hrBranches { get; set; }
       
        public string visualEmpCodeName { get; set; }
    }

	public class EmployeeArrearInfoViewModel
	{
		public int? employeeArrearId { get; set; }

		public int? employeeInfoId { get; set; }

		public decimal? currentBasic { get; set; }
		public decimal? oldBasic { get; set; }

		public int? periodId { get; set; }

		public int? salaryHeadId { get; set; }

		public decimal? amount { get; set; }

		public IEnumerable<SalaryPeriod> salaryPeriods { get; set; }
		public IEnumerable<EmployeeArrearInfo> employeeArrearInfos { get; set; }
	}
}
