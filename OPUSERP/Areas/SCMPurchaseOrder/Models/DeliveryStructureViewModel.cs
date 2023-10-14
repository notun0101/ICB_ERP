using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.SCMPurchaseOrder.Models
{
	public class DeliveryStructureViewModel
	{
		public int?[] structureCsDetailId { get; set; }
		public string[] structureDate { get; set; }
		public int[] structureLoc { get; set; }
		public int[] structureQty { get; set; }
	}
}
