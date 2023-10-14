using OPUSERP.Areas.SCMMatrix.Models;
using OPUSERP.Data.Entity.Matrix;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OPUSERP.SCM.Services.Matrix.Interfaces
{
    public interface IApprovalMatrixService
    {
        Task<bool> DeleteApprovalMatrixByprojectuserId(int projectId, int userId);
        Task<bool> UpdateApprovalMatrix(int projectId, int matrixTypeId, int newUserId, int oldUserId);
        Task<int> SaveApprovalMatrix(ApprovalMatrix approvalMatrix);
        Task<IEnumerable<ApprovalMatrix>> GetApprovalMatrixList();
        Task<IEnumerable<System.Object>> GetApprovalMatrixByRaiserId(int projectId, int matrixTypeId, int raiserId);
        Task<IEnumerable<ApprovalMatrixViewModel>> GetAllTypeApprovalMatrixByRaiserIdAndTypeId(int projectId, int matrixTypeId, int raiserId);
        Task<IEnumerable<System.Object>> GetPRApproverPanel(string userName);
        Task<IEnumerable<ApproverPanelViewModel>> GetPRApproverPanelList(string userName, int matrixTypeId, int projectId);
        Task<IEnumerable<ApprovalMatrixVMR>> GetApprovalMatrixVMR(int reqMasterId);
        Task<IEnumerable<ApproverPanelViewModel>> GetPRApproverPanelListFromApprovalLogs(string userName, int matrixTypeId, int reqMasterId);
        Task<IEnumerable<MatrixInformationVM>> GetAllMatrixInformation(string userName);
        Task<ApprovalMatrix> GetApprovalMatrixById(int id);
        Task<bool> DeleteApprovalMatrixById(int id);
        Task<IEnumerable<ApproverPanelViewModel>> GetPRApproverPanelByUserId(int userId, int reqId);
        Task<IEnumerable<ApprovalMatrix>> GetApprovalMatrixByUserId(int userid);
        Task<IEnumerable<ApprovalMatrixsVM>> GetApprovalMatrixs();
        Task<IEnumerable<ApproverPanelFViewModel>> GetPRApproverPanelFListFromApprovalLogs(string userName, int matrixTypeId, int reqMasterId);
        Task<IEnumerable<ProjectWithUserVm>> GetAllProjectsByMatrixType(string mTypeName);
		Task<IEnumerable<ApprovarsByProject>> GetApprovarsByUserIdAndProjectId(int userid, int projectid, string matrixTypeName);
        Task<ApprovalMatrixForEditViewModel> GetApprovalMatrixByProjectAndRaiserId(int? projectId, int? raiserId,int? matrixTypeId);
        Task<IEnumerable<ProjectWithUserVm>> GetAllProjectsByMatrixTypeByRaisar(string mTypeName);
		Task<IEnumerable<ApproverPanelViewModel>> GetPRApproverPanelList1(int csId, int matrixTypeId, int projectId);
		Task<int> DeleteApprovalMatrixByProjectRaiserMTypeId(int projectId, int raiser, int mTypeId);
		Task<IEnumerable<StatusLog>> GetStatusLogByReqId(int reqId);
	}
}
