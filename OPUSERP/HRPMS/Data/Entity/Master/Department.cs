using OPUSERP.Data.Entity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
    [Table("Department")]
    public class Department:Base
    {
        public string deptCode { get; set; }

        [Required]
        public string deptName { get; set; }
        public string deptNameBn { get; set; }

        public string shortName { get; set; }
        public DateTime? startDate { get; set; }
        public int? hrDivisionId { get; set; }
        public HrDivision hrDivision { get; set; }
        public int? hrBranchId { get; set; }
        public HrBranch hrBranch { get; set; }

		public int? sortOrder { get; set; }

		public int? isActive { get; set; } = 1;
		public string type { get; set; }

	}
}
