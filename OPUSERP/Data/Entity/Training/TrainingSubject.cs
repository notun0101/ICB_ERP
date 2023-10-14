using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Data.Entity.Training
{
	[Table("TrainingSubject", Schema = "HR")]
	public class TrainingSubject:Base
	{
		public string name { get; set; }
	}
}
