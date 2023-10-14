﻿using OPUSERP.Areas.SCMDashboard.Models;
using OPUSERP.Areas.SCMJobAssign.Models;
using OPUSERP.Areas.SCMPurchaseProcess.Models;
using OPUSERP.Areas.SCMReport.Models;
using OPUSERP.Areas.SCMRequisition.Models;
using OPUSERP.Data.Entity.Matrix;
using OPUSERP.SCM.Data.Entity.MasterData;
using OPUSERP.SCM.Data.Entity.PurchaseProcess;
using OPUSERP.SCM.Data.Entity.Requisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.SCM.Services.Requisition.Interfaces
{
    public interface IRequisitionService
    {


        #region  RequisitionMaster

        Task<int> SaveRequisitionMaster(RequisitionMaster requisitionMaster);
        void UpdateRequisitionMasterStatusById(int reqId, int status);
        Task<IEnumerable<RequisitionMaster>> GetRequisitionMasterList(int reqBy);
        Task<IEnumerable<RequisitionMaster>> GetRequisitionMasterListNew(int reqBy);
        Task<IEnumerable<RequisitionMaster>> GetAllRequisitionDhasboard();
        Task<IEnumerable<RequisitionMaster>> GetRequisitionMasterListByPRStatus(int reqBy,int status);
        Task<IEnumerable<RequisitionMaster>> GetRequisitionMasterByReqId(int reqId);
        Task<IEnumerable<StatusLog>> GetStatusLogByRequisitionId(int id);
        Task<IEnumerable<RequisitionMaster>> GetRequisitionMasterListForAssign();
        Task<IEnumerable<Project>> GetProjectByUser(string userName);
        Task<RequisitionMaster> GetRequisitionMasterById(int id);
        Task<bool> DeleteRequisitionMasterById(int id);
        Task<RequisitionAutoNumberViewModel> GetReqAutoNumberBySp(int projectId,string reqBy);
        Task<CumulativeGRQtyBySpecIdViewModel> GetCumulativeGRQtyBySpecId(int itemspecId);
        Task<IEnumerable<RequisitionMaster>> GetRequisitionMasterListByStatus(int status);
        void AssignTeamInRequisitionMasterById(int masterId, int status, int teamId);
        void ReturnTeamInRequisitionMasterById(int masterId);
        Task<IEnumerable<RequisitionMaster>> GetAllRequisitionMasterList();
        Task<IEnumerable<RequisitionMaster>> GetAllRequisitionMasterForIOUList(string userName);
        Task<IEnumerable<RequisitionMaster>> GetRequisitionMasterListByActivityStatus(int status);

        Task<IEnumerable<RequisitionMasterForStatusVM>> GetAllRequisitionMasterListForViewStatus(DateTime? fromDate, DateTime? toDate, string type,int?userId);
        Task<DashboardModel> GetRequisitionMasterListForDashboard(int reqBy);
        Task<IEnumerable<PurchaseOrderSuppReportViewModel>> PurchaseOrderSuppReportViewModels(DateTime fromdate, DateTime todate, int supplierId);
        Task<IEnumerable<PurchaseOrderItemReportViewModel>> PurchaseOrderItemReportViewModels(DateTime fromdate, DateTime todate, int itemId);
        Task<IEnumerable<PurchaseOrderProjectReportViewModel>> PurchaseOrderProjectReportViewModels(DateTime fromdate, DateTime todate, int projectId);
        Task<IEnumerable<PurchaseOrderDiffReportViewModel>> PurchaseOrderDiffReportViewModels(DateTime fromdate, DateTime todate);
        Task<IEnumerable<PurchaseOrderBuyerReportViewModel>> PurchaseOrderBuyerReportViewModels(DateTime fromdate, DateTime todate);
        #endregion

        #region RequisitionDetail
        Task<int> SaveRequisitionDetail(RequisitionDetail requisitionDetail);
        Task<IEnumerable<RequisitionDetail>> GetRequisitionDetailListByReqId(int reqId);
        Task<IEnumerable<RequisitionDetailViewModel>> GetLinqRequisitionDetailListByReqId(int reqId,int userId);
        Task<IEnumerable<RequisitionDetail>> GetRequisitionDetailByitemIdandspec(int ItemId, int spec);
        Task<IEnumerable<RequisitionDetail>> GetRequisitionDetailByitemIdandspecandReqId(int ItemId, int spec,int reqId);
        Task<RequisitionDetail> GetRequisitionDetailById(int id);
        Task<bool> DeleteRequisitionDetailById(int id);
        Task<bool> DeleteRequisitionDetailByreqId(int reqId);
		void AssignTeamInRequisitionDetailsById(int Id, int status, int memberId, int proType, int purchaseType);

		Task<IEnumerable<RequisitionDetailsWithCSViewModel>> GetRequisitionDetailListForBuyer(int reqId,int userId);
        Task<IEnumerable<DashboardVM>> GetTopFiveUsedItem(int userId);
        Task<IEnumerable<DashboardVM>> GetCurrentMonthPRStatus(int userId);
        Task<IEnumerable<RequisitionGRCumulativeViewModel>> GetItemWiseCumutativeQTY(int itemId,int specId);
        Task<IEnumerable<RequisitionDetail>> GetRequisitionDetailListWithPurchaseTypeByReqId(int reqId, int purchaseType);
        #endregion

        #region Requisition Approve

        //Task<IEnumerable<RequisitionMaster>> GetRequisitionApproveList(int userId);
        Task<IEnumerable<GetRequisitionListForApprovedViewModel>> GetRequisitionApproveList(int userId);
        Task<IEnumerable<RequisitionTotalNumberForApproveViewModel>> GetRequisitionApproveCount(int reqId);
        Task<IEnumerable<RequisitionDetail>> GetItemListByRequisitionId(int reqId);
        Task<IEnumerable<RequisitionGRCumulativeViewModel>> GetRequisitionByReqId(int reqId);
        Task<IEnumerable<RequisitionDetailViewModel>> GetAllRequisitionDetailListByRequisitionId(int reqId);
        Task<IEnumerable<RequisitionDetailVM>> GetAllRequisitionDetailListByReqId(int reqId);
        Task<IEnumerable<System.Object>> GetAllItemListByRequisitionId(int reqId);
        Task<System.Object> GetApprovalLogByRequisitionId(int reqId);
        Task<System.Object> GetRequisitorInfoByRequisitionId(int reqId);
        #endregion
        Task<IEnumerable<RequisitionDetail>> GetItemListByrfqId(int reqId);
        Task<IEnumerable<RFQReqDetail>> GetRFQReqDetailListByrfqId(int reqId);
        #region DocumentAttachment
        Task<int> SaveDocumentAttachment(DocumentAttachment documentAttachment);
        Task<IEnumerable<DocumentAttachment>> GetDocumentAttachmentList(int masterId);
        Task<IEnumerable<DocumentAttachment>> GetAttachmentListByMatrixType(int masterId, int matrixTypeId);
        Task<DocumentAttachment> GetDocumentAttachmentById(int id);
        Task<bool> DeleteDocumentAttachmentById(int id);
        Task<bool> DeleteDocumentAttachmentBymasterId(int masterId, int matrixTypeId);
        Task<int> SaveDocumentAttachmentDetails(DocumentAttachmentDetail documentAttachment);
        Task<IEnumerable<DocumentAttachmentDetail>> GetDocumentAttachmentDetailList(int detailid, int itemspecid);
        Task<IEnumerable<DocumentAttachmentDetail>> GetDocumentAttachmentDetailListreqid(int reqId);
        #endregion

        #region PreferableVendor
        Task<int> SavePreferableVendor(PreferableVendor preferableVendor);
        Task<IEnumerable<PreferableVendor>> GetPreferableVendorList(int masterId);
        Task<IEnumerable<PreferableVendor>> GetPreferableVendorListbySpecId(int SpecId);
        Task<PreferableVendor> GetPreferableVendorById(int id);
        Task<bool> DeletePreferableVendorById(int id);
        Task<bool> DeletePreferableVendorByreqId(int reqId);
        #endregion

        Task<IEnumerable<RequisitionDetail>> GetRequisitionDetailList();

        Task<int> GetCreatedRequisition(string statusName);
		Task<IEnumerable<MostRecentRequisitionViewModel>> GetMostRecentReq();
        Task<IEnumerable<TopTenRequisitionViewModel>> GetTopTenReq();
        Task<IEnumerable<FeaturedItemViewModel>> GetFeaturedReq();
        Task<IEnumerable<MostRecentRequisitionNewViewModel>> GetMostRecentRequisition(int dptId, string username);
        Task<IEnumerable<TopTenRequisitionViewModel>> GetTopTenReq(int dptId);
        Task<IEnumerable<FeaturedItemViewModel>> GetFeaturedReq(string username, int id);
        //add/////////////
        //Task<int> GetRequisitionApproverByUserId(int userId);
        Task<IEnumerable<ApproverVM>> GetApproversByProjectIdandUserId(int? projectId, int? userId);
        //end/////////////
        Task<RequisitionDetail> GetRequisitionDetailByMemberId(int? id);
        #region PO Report Press Club
        Task<IEnumerable<POSupplierReportViewModel>> POSuppReportViewModels(DateTime fromdate, DateTime todate, int supplierId);
		#endregion
		Task<RequisitionDetail> GetRequisitionDetailListWithPurchaseTypeByReq(int reqId, int purchaseType);
		Task<IEnumerable<ReqDetailWithAssignedMember>> GetAllItemListByRequisitionMasterId(int reqId);
		Task<int> UpdateRequisitionDetasilStatus(int masterId, int detailsId, int memberId);
		Task<int> UpdateRequisitionDetasilStatus1(int masterId, int detailsId, int memberId, int purchaseType, int processType);
		Task<int> checkReqApproverSeqByUserId(int masterId, int userId);
		Task<int> GetStatusLogIdByMasterId(int masterId);
		Task<IEnumerable<CSDetail>> GetCsDetailsByReqMasterId(int reqId);
		Task<IEnumerable<RequisitionDetail>> GetAllRequisitionDetailList();
        Task<IEnumerable<ReqDetailWithAssignedMember>> GetAllItemListByRequisitionMaster();

        Task<IEnumerable<RequisitionDetail>> GetAllRequisitionDetailListByReqMasterId(int reqId);
		Task<IEnumerable<CSDetail>> GetCsDetailsByCsMasterId(int csId);
		Task<IEnumerable<CSDetail>> GetSingleCsDetailsByCsMasterId(int csId);
		Task<IEnumerable<Notification>> GetAllNotifications();
		Task<bool> SaveNotification(Notification notification);
		Task<IEnumerable<Notification>> GetAllNotificationsByReceiverId(string aspnetid);
		Task<int> ReadNotificationById(int id);
		Task<int> GetAllNotificationsCountByReceiverId(string aspnetid);
		Task<int> GetEmpIdByTeamMemberId(int id);
		void AssignTeamInRequisitionDetailsById(int Id, int status, int memberId, int empId, int proType, int purchaseType);
		Task<IEnumerable<GetAllReqReportViewModel>> GetAllReqReports();
	}
}
