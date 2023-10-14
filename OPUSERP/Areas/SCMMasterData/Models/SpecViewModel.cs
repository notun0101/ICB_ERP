using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.SCMMasterData.Models
{
	public class SpecViewModel
	{
		public IEnumerable<ItemWithSpecViewModel> itemWithSpecViewModels { get; set; }
	}
	public class ItemWithSpecViewModel
	{
		public string specificationName { get; set; }
		public string SKUNumber { get; set; }
		public string itemCode { get; set; }
		public string itemName { get; set; }
	}
}
