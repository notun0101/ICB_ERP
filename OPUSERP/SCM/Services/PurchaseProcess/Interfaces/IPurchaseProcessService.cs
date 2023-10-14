using OPUSERP.Areas.SCMMatrix.Models;
using OPUSERP.Areas.SCMPurchaseOrder.Models;
using OPUSERP.Areas.SCMPurchaseProcess.Models;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.SCM.Data.Entity.PurchaseProcess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OPUSERP.SCM.Services.PurchaseProcess.Interfaces
{
    public interface IPurchaseProcessService
    {
        #region  CS Master

        Task<int> SaveCSMaster(CSMaster cSMaster);
        void UpdateCSMaster(int id, int status);
        Task<IEnumerable<CSMaster>> GetCSMasterList(int userId);
        Task<IEnumerable<CSMaster>> GetCSMasterListForPO(int userId);
        Task<IEnumerable<CSMaster>> GetCSMasterByReqId(int reqId);
        Task<CSMaster> GetCSMasterById(int id);
        Task<bool> DeleteCSMasterById(int id);
        Task<string> GetCSNumber();
        Task<IEnumerable<CSMaster>> GetAllCSMasterList();

        Task<IEnumerable<CSMaster>> GetCSListForApprove(int userId);


        #endregion
        
        #region  CS Details
        Task<int> SaveCSDetailsSingle(CSDetail cSDetail);
        void SaveCSDetails(List<CSDetail> cSDetails);

        Task<bool> UpdateCSDetailApproval(int? Id, decimal? qty, decimal? rate);


        Task<IEnumerable<CSDetail>> GetCSDetailListByMasterId(int masterId);
        Task<IEnumerable<CSItemVM>> GetCSDetailListBySupplierId(int supplierId);
        Task<IEnumerable<CSItemVM>> GetCSDetailListByCSAndSupplierId(int csMasterId,int supplierId);
        Task<IEnumerable<CSSupplierVM>> GetCSSupplierListBycsId(int csId);

        Task<IEnumerable<QuotationDetailsVM>> GetQuotationDetailView(int csMasterId);
        Task<IEnumerable<CSDetailsVM>> GetCSDetailView(int csMasterId);
        Task<IEnumerable<CSDetailsReportViewModel>> GetCSDetailInforReport(int csMasterId);
		Task<IEnumerable<RequisitionForCSViewModel>> GetRequisitionListForBuyerSingleSource(int userId);
		Task<IEnumerable<RequisitionForCSViewModel>> GetRequisitionListForBuyerSpotPurchase(int userId);
		//Task<int> SaveCSDetails(CSDetail cSDetail);
		//void UpdateCSDetails(int id, int status);
		//Task<IEnumerable<CSDetail>> GetCSDetailList(int userId);
		//Task<IEnumerable<CSDetail>> GetCSDetailsByCSId(int csId);
		//Task<CSDetail> GetCSDetailsById(int id);
		//Task<bool> DeleteCSDetailsById(int id);
		//Task<bool> DeleteCSDetailsByCSId(int id);

		#endregion

		#region PR WITH CS
		Task<IEnumerable<RequisitionForCSViewModel>> GetRequisitionListForBuyer(int userId);
        Task<IEnumerable<GetSupplierWiseItemDetailsViewModel>> GetSupplierWiseItemDetails(int reqDetailID);
        #endregion
        #region POMaster Approval
        Task<PurchaseOrderViewModel> GetPOMasterDetailsByPOMId(int pomid);
		#endregion
		Task<IEnumerable<CSMaster>> GetCSApprovedListForApprove(int userId);
		Task<IEnumerable<CSMaster>> GetCSApprovedListForApprove(int userId, DateTime? start, DateTime? end);
		Task<IEnumerable<CSMaster>> GetCSListForApproveSingle(int userId);
		Task<IEnumerable<CSMaster>> GetCSListForApproveSingleSource(int userId);
		Task<IEnumerable<CSMaster>> GetSingleApprovedListForApprove(int userId, DateTime? start, DateTime? end);
		Task<IEnumerable<ApproverPanelViewModel>> GetReqApprovalMatrixByMasterIdAndProjectId(int masterId, int projectId, string username);
		Task<string> GetUsernameByCsMasterId(int masterId);
        Task<string> GetUsernameByIOUMasterId(int masterId);
		Task<IEnumerable<EmployeeInfo>> GetReqApprovalMatrixByMasterIdAndProjectIdInWorkOrder(int projectId, string username);
		Task<IEnumerable<CSDetail>> GetAllCsDetailList();
        Task<string> GetUsernameByWorkOrderMasterId(int masterId);
		Task<int> UpdateCSMaster(CSMaster cSMaster);
		Task<IEnumerable<ApproverPanelViewModel>> GetWOMatrixByMasterIdAndProjectId(int masterId, int projectId, string username);
		Task<IEnumerable<ApproverPanelViewModel>> GetIOUApprovalMatrixByMasterIdAndProjectId(int masterId, int projectId, string username);

	}
}
