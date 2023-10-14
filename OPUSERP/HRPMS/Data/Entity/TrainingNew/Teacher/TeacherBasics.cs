using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.TrainingNew.Teacher
{
	[Table("TeacherBasics")]
	public class TeacherBasics:Base
	{
		public string teacherCode { get; set; }
		public string nameEnglish { get; set; }
		public string nameBangla { get; set; }
		public string maritalStatus { get; set; }
		public string addressPresent { get; set; }
		public string addressPermanant { get; set; }
		public string bloodGroup { get; set; }
		public string mobilePersonal { get; set; }
		public string mobileOffice { get; set; }
		public string emailPersonal { get; set; }
		public DateTime? dateOfBirth { get; set; }
		public string eTin { get; set; }
		public string nid { get; set; }
		public int heightCm { get; set; }
		public int weightKg { get; set; }
		public string gender { get; set; }

		public int? religionId { get; set; }
		public Religion religion { get; set; }

		public string photoUrl { get; set; }

		public string about { get; set; }

		public int? status { get; set; }
		public int? isActive { get; set; }
		public string remarks { get; set; }
	}
}
