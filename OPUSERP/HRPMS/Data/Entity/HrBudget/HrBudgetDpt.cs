using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.HrBudget
{
	[Table("HrBudgetDpt", Schema = "HR")]
	public class HrBudgetDpt:Base
	{
		public int departmentId { get; set; }
		public Department department { get; set; }

		public int designationId { get; set; }
		public Designation designation { get; set; }

		public int branchId { get; set; }
		public HrBranch branch { get; set; }

		public int? person { get; set; }

		public int? status { get; set; }
		public int? isActive { get; set; }
		public string remarks { get; set; }
	}
}
