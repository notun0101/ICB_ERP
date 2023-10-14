using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.SCM.Data.Entity.MasterData;
using OPUSERP.SCM.Data.Entity.PurchaseOrder;
using OPUSERP.SCM.Data.Entity.Requisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.SCMJobAssign.Models
{
    public class JobAssignViewModel
    {
        public int?[] masterIds { get; set; }
        public int?[] teamIds { get; set; }

        public int rBuyer { get; set; }
		public IEnumerable<PurchaseOrderMaster> purchaseOrderMasters { get; set; }
		public int rType { get; set; }
        public int purchaseType { get; set; }//1= Spoc Purchase 2=CS Purchase
        public int?[] mPurchaseType { get; set; }//1= Spoc Purchase 2=CS Purchase
        public int?[] mProcessType { get; set; }//1= Spoc Purchase 2=CS Purchase

        public int? singleMemberIds { get; set; }
        public int?[] MemberIds { get; set; }
        public int?[] reqDetailIds { get; set; }

        public IEnumerable<RequisitionMaster> requisitionMasters { get; set; }
        public IEnumerable<RequisitionMaster> assignRequisitionMasters { get; set; }
        public IEnumerable<TeamMaster> teamMasters { get; set; }

		public int reqMasterIds { get; set; }
		public int Aassigneds { get; set; }
		public string Remarkss { get; set; }
	}
	public class ReqDetailWithAssignedMember
	{
		public RequisitionDetail requisitionDetail { get; set; }
		public EmployeeInfo employeeInfo { get; set; }
	}
	public class AssignMemberMasterViewModel
	{
		public int AmasterId { get; set; }
		public int AdetailsId { get; set; }
		public int Aassigned { get; set; }
		public int purchaseTypes { get; set; }
		public int processTypes { get; set; }
	}
}
