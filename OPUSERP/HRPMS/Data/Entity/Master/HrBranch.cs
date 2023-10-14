using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
	[Table("HrBranch")]
	public class HrBranch:Base
	{
		public string branchCode { get; set; }
		public string branchName { get; set; }
		public string branchNameBn { get; set; }
		public string address { get; set; }
        public int? locationId { get; set; } //zoneId
        public Location location { get; set; } //zone

        public int? branchTypeId { get; set; }
		public HrBranchType branchType { get; set; }

		public int? officeId { get; set; }
		public FunctionInfo office { get; set; }
        public int? branchPlace { get; set; }
    }
}
