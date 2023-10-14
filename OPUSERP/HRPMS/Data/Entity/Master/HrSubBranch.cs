using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
	[Table("HrSubBranch")]
	public class HrSubBranch : Base
	{
		public string subbranchCode { get; set; }
		public string subbranchName { get; set; }
		public string subbranchNameBangla { get; set; }
		public string subaddress { get; set; }

		public int? subbranchTypeId { get; set; }
		public HrBranchType subbranchType { get; set; }

        public int? hrBranchId { get; set; }
		public HrBranch hrBranch { get; set; }

		public int? status { get; set; }
		public int? isActive { get; set; }
		public string remarks { get; set; }
	}
}
