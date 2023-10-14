using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.TrainingNew.Teacher
{
	[Table("TeacherCareer", Schema = "HR")]
	public class TeacherCareer:Base
	{
		public int startYear { get; set; }
		public int endYear { get; set; }
		public string role { get; set; }
		public string companyName { get; set; }
		public string description { get; set; }

		public int teacherId { get; set; }
		public TeacherBasics teacher { get; set; }

		public int? status { get; set; }
		public int? isActive { get; set; }
		public string remarks { get; set; }
	}
}
