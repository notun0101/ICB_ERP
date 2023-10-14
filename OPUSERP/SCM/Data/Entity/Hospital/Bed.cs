using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.SCM.Data.Entity.Hospital
{
	[Table("Bed", Schema = "SCM")]
	public class Bed:Base
	{
		public string nameEn { get; set; }
		public string nameBn { get; set; }

		public int? cabinId { get; set; }
		public Cabin cabin { get; set; }
		public int? isBooked { get; set; }
		public int status { get; set; }
		public string Description { get; set; }
	}
}
