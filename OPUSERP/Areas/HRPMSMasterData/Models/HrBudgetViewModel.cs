using OPUSERP.HRPMS.Data.Entity.HrBudget;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
	public class HrBudgetViewModel
	{
		public int hrbudgetId { get; set; }
		public int departmentId { get; set; }
		public int designationId { get; set; }
		public int branchId { get; set; }
		public int? person { get; set; }

		public int? status { get; set; }
		public int? isActive { get; set; }
		public string remarks { get; set; }


		public IEnumerable<Department> allDepartment { get; set; }
		public IEnumerable<Designation> allDesignation { get; set; }
		public IEnumerable<HrBranch> AllBranch { get; set; }

		public IEnumerable<DepartmentWithBudget> departmentWithBudgets { get; set; }
	}
	public class DepartmentWithBudget
	{
		public Department department { get; set; }
		public IEnumerable<HrBudgetDpt> hrBudgetDpts { get; set; }
	}
}
