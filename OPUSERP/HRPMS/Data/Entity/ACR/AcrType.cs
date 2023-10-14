using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.ACR
{
	[Table("AcrType", Schema = "HR")]
	public class AcrType:Base
	{
		[MaxLength(150)]
		public string acrTypeName { get; set; }
		[MaxLength(150)]
		public string acrTypeNameBn { get; set; }
		public int? shortOrder { get; set; }
	}
}
