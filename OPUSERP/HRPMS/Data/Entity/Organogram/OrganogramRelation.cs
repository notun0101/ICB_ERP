using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Organogram
{
	[Table("OrganogramRelation", Schema = "HR")]
	public class OrganogramRelation:Base
	{
        public string title { get; set; }

        public int? officeId { get; set; }
        public FunctionInfo office { get; set; }

        public int? departmentId { get; set; }
        public Department department { get; set; }

        public int? divisionId { get; set; }
        public HrDivision division { get; set; }


        public string subTitle { get; set; }
		public int? parentId { get; set; }
		public string url { get; set; }
		public int? headId { get; set; } //employeeId
	}
}
