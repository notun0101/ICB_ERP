using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.SCM.Data.Entity.Hospital
{
	[Table("Floor")]
	public class Floor:Base
	{
		public int? buildingId { get; set; }
		public Building building { get; set; }

		public string nameEn { get; set; }
		public string nameBn { get; set; }
		public string type { get; set; }
		public int? status { get; set; }
		public string remarks { get; set; }
	}
}
