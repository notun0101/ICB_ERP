using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Data.Entity.Salary
{
	[Table("ProcessEmpSalaryStructure", Schema = "Payroll")]
	public class ProcessEmpSalaryStructure : Base
	{
		public int employeeInfoId { get; set; }
		public EmployeeInfo employeeInfo { get; set; }
        public string employeeCode { get; set; }
        public string employeeName { get; set; }
        public int? seniorityLevel { get; set; }
        public DateTime? joiningDateGovtService { get; set; }

        public int? sbuId { get; set; }
        public int? employeeTypeId { get; set; }
        public string branchType { get; set; } //branch, department, division, unit, zone, office
		public string postingPlaceName { get; set; }

		public int salaryPeriodId { get; set; }
		public SalaryPeriod salaryPeriod { get; set; }

		public int salaryHeadId { get; set; }
		public SalaryHead salaryHead { get; set; }

		public string desiCode { get; set; }
		public string accountNo { get; set; }

		public int? hrBranchId { get; set; }
		public HrBranch hrBranch { get; set; }

		public int? departmentId { get; set; }
		public Department department { get; set; }

		public int? hrDivisionId { get; set; }
		public HrDivision hrDivision { get; set; }

		public int? hrUnitId { get; set; }
		public HrUnit hrUnit { get; set; }

		public int? officeId { get; set; }
		public FunctionInfo office { get; set; }

		public int? zoneId { get; set; }
		public Location zone { get; set; }

		public int? designationId { get; set; }
		public Designation designation { get; set; }

		public decimal amount { get; set; }

        public string pfType { get; set; } //CPF, GPF
        public int? pfPercentage { get; set; }

		public int? days { get; set; }

		public int? status { get; set; } //posted-lock = 1; posted-final = 2;
	}
}
