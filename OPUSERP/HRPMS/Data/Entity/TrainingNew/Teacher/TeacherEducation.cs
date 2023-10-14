using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.TrainingNew.Teacher
{
	[Table("TeacherEducation", Schema = "HR")]
	public class TeacherEducation:Base
	{
		public string universityName { get; set; }
		public string degree { get; set; }
		public string session { get; set; }

		public int teacherId { get; set; }
		public TeacherBasics teacher { get; set; }

		public string description { get; set; }
		public int? status { get; set; }
		public int? isActive { get; set; }
		public string remarks { get; set; }
	}
}
