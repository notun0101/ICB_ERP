using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.TrainingNew.Teacher
{
	[Table("TeacherExpertise", Schema = "HR")]
	public class TeacherExpertise:Base
	{
		public string title { get; set; }
		public string subtitle { get; set; }

		public int teacherId { get; set; }
		public TeacherBasics teacher { get; set; }

		public int? status { get; set; }
		public int? isActive { get; set; }
		public string remarks { get; set; }
	}
}
