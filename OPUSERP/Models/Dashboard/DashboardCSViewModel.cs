using OPUSERP.SCM.Data.Entity.IOU;
using OPUSERP.SCM.Data.Entity.PurchaseOrder;
using OPUSERP.SCM.Data.Entity.PurchaseProcess;
using OPUSERP.SCM.Data.Entity.Requisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Models.Dashboard
{
	public class DashboardCSViewModel
	{
		public int PendingRequisitionCount { get; set; }
		public int AssignPendingRequisitionCount { get; set; }
		public int CSPendingCount { get; set; }
		public int SingelPendingCount { get; set; }
		public int POPendingCount { get; set; }
		public int SpotPendingCount { get; set; }

		public IEnumerable<RequisitionMaster> AllRequisition { get; set; }
		public IEnumerable<RequisitionDetail> AllRequisitionDetails { get; set; }
		public IEnumerable<CSDetail> AllCSDetails { get; set; }
		public IEnumerable<CSMaster> AllCS { get; set; }
		public IEnumerable<PurchaseOrderMaster> AllPurchase { get; set; }
		public IEnumerable<IOUMaster> AllSpotPurchase { get; set; }

		public decimal AllReqAmount { get; set; }
		public decimal AllApReqAmount { get; set; }
		public decimal AllPendReqAmount { get; set; }

		public decimal MonthReqAmount { get; set; }
		public decimal MonthApReqAmount { get; set; }
		public decimal MonthPendReqAmount { get; set; }

		public decimal YearReqAmount { get; set; }
		public decimal YearApReqAmount { get; set; }
		public decimal YearPendReqAmount { get; set; }

		public decimal AllCsAmount { get; set; }
		public decimal AllApCsAmount { get; set; }
		public decimal AllPendCsAmount { get; set; }

		public decimal MonthCsAmount { get; set; }
		public decimal MonthApCsAmount { get; set; }
		public decimal MonthPendCsAmount { get; set; }

		public decimal YearCsAmount { get; set; }
		public decimal YearApCsAmount { get; set; }
		public decimal YearPendCsAmount { get; set; }

		public decimal AllSpotAmount { get; set; }
		public decimal AllApSpotAmount { get; set; }
		public decimal AllPendSpotAmount { get; set; }

		public decimal MonthSpotAmount { get; set; }
		public decimal MonthApSpotAmount { get; set; }
		public decimal MonthPendSpotAmount { get; set; }

		public decimal YearSpotAmount { get; set; }
		public decimal YearApSpotAmount { get; set; }
		public decimal YearPendSpotAmount { get; set; }

		public decimal AllPOAmount { get; set; }
		public decimal AllApPOAmount { get; set; }
		public decimal AllPendPOAmount { get; set; }

		public decimal MonthPOAmount { get; set; }
		public decimal MonthApPOAmount { get; set; }
		public decimal MonthPendPOAmount { get; set; }

		public decimal YearPOAmount { get; set; }
		public decimal YearApPOAmount { get; set; }
		public decimal YearPendPOAmount { get; set; }
	}
	public class ReqWithAmount
	{
		public RequisitionMaster requisitionMaster { get; set; }
		public decimal amount { get; set; }
	}
}
