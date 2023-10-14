using Microsoft.AspNetCore.Http;
using OPUSERP.SCM.Data.Entity.PurchaseProcess;
using OPUSERP.SCM.Data.Entity.Requisition;
using System;
using System.Collections.Generic;

namespace OPUSERP.Areas.SCMPurchaseProcess.Models
{
    public class PurchaseProcessViewModel
    {
        public int? procurementTypeId { get; set; }
        public int? procurementValueId { get; set; }
        public int? reqId { get; set; }
        public int? projectId { get; set; }

		public int csMasterId { get; set; }

		public int[] reqDetailsId { get; set; }
        public int[] itemCatId { get; set; }
        public int[] supplierId { get; set; }
        public decimal?[] csRate { get; set; }
        public decimal?[] csQty { get; set; }
        public int?[] justifyTypeId { get; set; }
        public int?[] isJustify { get; set; }
        public string[] justifyValue { get; set; }
        public DateTime? deliveryDate { get; set; }
        public string rfqReference { get; set; }
        public string attchSubject { get; set; }

		public int orgId { get; set; }
		public string organizationName { get; set; }
		public string orgCode { get; set; }
		public string LicenseNumber { get; set; }
		public string eTinNumber { get; set; }
		public string VATNumber { get; set; }

		public RequisitionMaster requisitionMaster { get; set; }
        public IEnumerable<RequisitionDetail> requisitionDetails { get; set; }
        public RequisitionDetail requisitionDetail { get; set; }
        public IEnumerable<ProcurementType> procurementTypes { get; set; }
        public IEnumerable<ProcurementValue> procurementValues { get; set; }
        public IEnumerable<JustificationType> justificationTypes { get; set; }
		public CSMaster cSMaster { get; set; }
		public IEnumerable<Justification> justifications { get; set; }
	}
}
