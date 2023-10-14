using OPUSERP.Data.Entity;
using OPUSERP.SCM.Data.Entity.PurchaseProcess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.SCM.Data.Entity.PurchaseOrder
{
	[Table("DeliveryStructure", Schema = "SCM")]
	public class DeliveryStructure:Base
	{
		public int? cSDetailId { get; set; }
		public CSDetail cSDetail { get; set; }
		public string DeliveryDate { get; set; }
		public int LocationId { get; set; }
		public int Qty { get; set; }
	}
}
