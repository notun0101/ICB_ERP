using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.SCM.Data.Entity.Hospital
{
	[Table("Cabin", Schema = "SCM")]
	public class Cabin:Base
	{
		public int? floorId { get; set; }
		public Floor floor { get; set; }

		public string cabinNo { get; set; }
		public string nameEn { get; set; }
		public string nameBn { get; set; }
		public string type { get; set; }
		public int bed { get; set; }
		public int booked { get; set; }
		public int? status { get; set; }
		public string remarks { get; set; }
	}
}
