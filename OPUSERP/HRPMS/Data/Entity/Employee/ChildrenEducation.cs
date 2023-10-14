using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
	[Table("ChildrenEducation", Schema = "HR")]
	public class ChildrenEducation : Base
	{
		public int? childrenId { get; set; }
		public Children children { get; set; }

		public string institution { get; set; }

        public int? levelOfEducationId { get; set; }
        public LevelofEducation levelOfEducation { get; set; }

        public int? degreeId { get; set; }
		public Degree degree { get; set; }

		public string majorSubject { get; set; }
		public string passingYear { get; set; }

		public string resultType { get; set; } //1st, 2nd, 3rd Class, Grade
		public string result { get; set; }
	}
}
