﻿using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
    [Table("HrUnit")]
    public class HrUnit : Base
    {
        [MaxLength(250)]
        public string unitName { get; set; }
        [MaxLength(250)]
        public string unitNameBn { get; set; }
        [MaxLength(150)]
        public string shortName { get; set; }
        public int? departmentId { get; set; }
        public Department department { get; set; }

        public int? hrBranchId { get; set; }
        public HrBranch hrBranch { get; set; }

		public int? sortOrder { get; set; }
	}
}
