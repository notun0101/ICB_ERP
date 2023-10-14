using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
	[Table("HrDivision", Schema = "HR")]
	public class HrDivision : Base
	{
		public string divCode { get; set; }

		[Required]
		public string divName { get; set; }
		public string divNameBn { get; set; }
        public int? functionId { get; set; }  //officeId
        public FunctionInfo functionInfo { get; set; } //Office
        public string shortName { get; set; }
		public DateTime? startDate { get; set; }
        public int? hrBranchId { get; set; }
        public HrBranch hrBranch { get; set; }

		public int? sortOrder { get; set; }
	}
}
