using OPUSERP.Accounting.Data.Entity.MasterData;
using OPUSERP.Areas.SCMRequisition.Models.Lang;
using OPUSERP.CRO.Data.Entity.DistributeJob;
using OPUSERP.Data.Entity.Matrix;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.SCM.Data.Entity.MasterData;
using OPUSERP.SCM.Data.Entity.PurchaseProcess;
using OPUSERP.SCM.Data.Entity.Requisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.SCMRequisition.Models
{
    public class RequisitionViewModel
    {
        public string userName { get; set; }
        public int reqMasterId { get; set; }
        public string reqNo { get; set; }
        public DateTime? reqDate { get; set; }
        public DateTime? targetDate { get; set; }
        //public DateTime? targetDate { get; set; }
        public string departmentName { get; set; }
        public string subject { get; set; }
        public string reqDept { get; set; }
        public int projectId { get; set; }
        public string reqType { get; set; }

        

        public string itemName { get; set; }
        public string specName { get; set; }
        public int detailsId { get; set; }
        public int itemId { get; set; }
        public string itemSpecificationId { get; set; }
        public string itemSpecificationName { get; set; }
        public string budgetCode { get; set; }
        public decimal? reqQty { get; set; }
        public decimal? totalAmmount { get; set; }
        public decimal? reqRate { get; set; }
        public string justification { get; set; }
        public int? employeeId { get; set; }
        public string deliveryLocation { get; set; }

        
        public int? isCompWaiver { get; set; }
        public int? supplierId { get; set; }
        public int? foundsourceId { get; set; }
        public string isPostPR { get; set; }
        public string deliveryaddress { get; set; }
       
        public int? statusInfoId { get; set; }
        public IEnumerable<RequisitionMaster> requisitionMaster { get; set; }
        public IEnumerable<RequisitionDetail> requisitionDetails { get; set; }
        public IEnumerable<RequisitionMaster> activePR { get; set; }
        public IEnumerable<RequisitionMaster> inActivePR { get; set; }
        public IEnumerable<RequisitionViewModel> requisitionViewModels { get; set; }
        public List<Details> Details { get; set; }
        public RequisitionAutoNumberViewModel GetReqAutoNumberBySp { get; set; }
        public List<ApprovalPanel> panels { get; set; }
        public List<PreferableVendorViewModel> preferableVendors { get; set; }
        public RequisitionLN fLang { get; set; }
        public IEnumerable<Project> projects { get; set; }
        public IEnumerable<SpecificationCategory> specificationCategories { get; set; }
        public IEnumerable<FundSource> fundSources { get; set; }
        public IEnumerable<EmployeeInfo> employeeInfos { get; set; }

        public RequisitionMaster rMaster { get; set; }
        public DocumentAttachmentDetail DocumentAttachmentDetails { get; set; }
        public IEnumerable<ApproverVM> approverVMs { get; set; }
        public IEnumerable<SpecificationDetail> specifications { get; set; }
        public IEnumerable<DocumentAttachmentDetail> DocumentAttachmentDetail { get; set; }
		public IEnumerable<OPUSERP.Data.Entity.Matrix.StatusLog> statusLogs { get; set; }
	}
    public class ApproverVM
    {
        public int? userId { get; set; }
        public string Name { get; set; }
        public int SequenceNo { get; set; }
        public string imgPath { get; set; }
    }
    public class Details
    {
        public int detailsId { get; set; }
        public int itemId { get; set; }
        public string itemSpecificationId { get; set; }
        public string itemSpecificationName { get; set; }
        public string budgetCode { get; set; }
        public decimal? reqQty { get; set; }
        public decimal? reqRate { get; set; }
        public DateTime? targetDate { get; set; }
        public string justification { get; set; }
        public int? employeeId { get; set; }
        public string deliveryLocation { get; set; }
    }
    public class PreferableVendorViewModel
    {
        public int Id { get; set; }
        public int itemId { get; set; }
        public string itemSpecificationId { get; set; }
        public int? supplierId { get; set; }
    }

    public class ApprovalPanel
    {
        public int sequenceNo { get; set; }
        public int userId { get; set; }
    }


	public class RequisitionDetailReportViewModel
	{
		public IEnumerable<RequistionReportVm> requistionReports { get; set; }
	}
	public class RequistionReportVm
	{
		public GetAllReqReportViewModel getAllReqReport { get; set; }

		public DateTime? PrDate { get; set; }
		public string PrNo { get; set; }
		public string PrRaisedBy { get; set; }
		public string PrPurpose { get; set; }
		public DateTime? ExpectedDeliveryDate { get; set; }
		public DateTime? CsDate { get; set; }
		public string CSRef { get; set; }
		public DateTime? PoDate { get; set; }
		public string PoRef { get; set; }
		public string SupplierName{ get; set; }
		public decimal? PoAmount { get; set; }
		public string StatusUpdate { get; set; }
	}
	public class GetAllReqReportViewModel
	{
		public DateTime? PrDate { get; set; }
		public string PrNo { get; set; }
		public string PrRaisedBy { get; set; }
		public string PrPurpose { get; set; }
		public DateTime? ExpectedDeliveryDate { get; set; }
		public DateTime? CsDate { get; set; }
		public string CSRef { get; set; }
		public DateTime? PoDate { get; set; }
		public string PoRef { get; set; }
		public string SupplierName { get; set; }
	}
}
