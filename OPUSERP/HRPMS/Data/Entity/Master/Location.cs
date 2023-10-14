using OPUSERP.Data.Entity;
using OPUSERP.Data.Entity.Master;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
    [Table("Location", Schema = "HR")]
    public class Location: Base //zone table
    {
        public string branchUnitName { get; set; }

        public string branchUnitNameBN { get; set; }

        public string branchCode { get; set; }

        public int? companyId { get; set; }
        public Company company { get; set; }

        public int? shortOrder { get; set; }

		public int? hrDepartmentId { get; set; }
		public Department hrDepartment { get; set; }

		public int? branchPlace { get; set; }

		//public int? hrBranchId { get; set; }
		//public HrBranch hrBranch { get; set; }
	}
}
