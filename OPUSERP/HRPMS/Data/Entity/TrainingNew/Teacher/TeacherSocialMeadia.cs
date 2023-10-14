using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.TrainingNew.Teacher
{
	[Table("TeacherSocialMeadia", Schema = "HR")]
	public class TeacherSocialMeadia:Base
	{
		public string name { get; set; }
		public string username { get; set; }
		public string url { get; set; }

		public int teacherId { get; set; }
		public TeacherBasics teacher { get; set; }

		public int? status { get; set; }
		public int? isActive { get; set; }
		public string remarks { get; set; }
	}
}
