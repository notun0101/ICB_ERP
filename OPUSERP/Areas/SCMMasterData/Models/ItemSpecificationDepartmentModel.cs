using OPUSERP.Areas.SCMRequisition.Models;
using OPUSERP.SCM.Data.Entity.Requisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.SCMMasterData.Models
{
    public class ItemSpecificationDepartmentModel
    {
        public int? itemId { get; set; }
        public int? itemSpecificationId { get; set; }
        public string SKUNumber { get; set; }
        public string specificationName { get; set; }
        public string specImageUrl { get; set; }
        public decimal? qty { get; set; }
        public int? storeDepartmentId { get; set; }

		//public string[] validItems { get; set; }
		//public int[] reqQty { get; set; }

		//public string reqNo { get; set; }
		//public DateTime reqDate { get; set; }
		//public string subject { get; set; }
		//public string status { get; set; }

		public IEnumerable<ItemSpecificationDepartmentModel> itemSpecificationDepartmentModels { get; set; }

		public IEnumerable<MostRecentRequisitionViewModel> MostRecent { get; set; }
		public IEnumerable<TopTenRequisitionViewModel> TopTen { get; set; }
		public IEnumerable<FeaturedItemViewModel> Featured { get; set; }
		public IEnumerable<RequisitionDetail> requisitionDetails { get; set; }
        public RequisitionMaster requisitionMaster { get; set; }
    }
}
