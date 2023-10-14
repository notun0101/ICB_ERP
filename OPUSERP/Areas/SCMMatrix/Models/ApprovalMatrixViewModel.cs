using OPUSERP.Areas.Auth.Models;
using OPUSERP.Areas.SCMMatrix.Models.Lang;
using OPUSERP.Data.Entity.Matrix;
using OPUSERP.SCM.Data.Entity.MasterData;
using OPUSERP.SCM.Data.Entity.Requisition;
using System.Collections.Generic;

namespace OPUSERP.Areas.SCMMatrix.Models
{
    public class ApprovalMatrixViewModel
    {
        public int? reqId { get; set; }
        public int? matrixId { get; set; }
        public int? projectId { get; set; }
        public int? budgetBranchId { get; set; }

        public int? matrixTypeId { get; set; }

        public int? userId { get; set; }

        public int? nextApproverId { get; set; }

        public int? approverTypeId { get; set; }

        public int? isActive { get; set; }

        public int? sequenceNo { get; set; }

        public int? nProjectId { get; set; }
        public int? nRaiserId { get; set; }
        public int?[] nextApproversId { get; set; }
        public int? nFinalId { get; set; }
        public string approvalName { get; set; }
        public string photo { get; set; }

        public ApprovalMatrixForEditViewModel approvalMatrixForEdit { get; set; }
        public ApprovalMatrixLN flang { get; set; }

        public IEnumerable<Project> projects { get; set; }
        public IEnumerable<MatrixType> matrixTypes { get; set; }
        public IEnumerable<ApproverType> approverTypes { get; set; }
        public IEnumerable<MatrixInformationVM> matrixInformation { get; set; }
        public IEnumerable<AspNetUsersViewModel> aspNetUsersViews { get; set; }
        public IEnumerable<AspNetUsersApproverViewModel> aspNetUsersApproverViews { get; set; }
        public IEnumerable<RequisitionMaster> requisitionMasters { get; set; }
        public IEnumerable<ChangeOfDoa> changeOfDoas { get; set; }
        public IEnumerable<ChangeDoaViewModel> changeDoaViewModels { get; set; }
        public IEnumerable<MatrixChangeHistory> matrixChangeHistories { get; set; }


    }
    public class ApprovalMatrixForEditViewModel
    {
        public int? projectId { get; set; }
        public string projectName { get; set; }
        public int? typeId { get; set; }
        public string typeName { get; set; }
        public RaiserViewModel Raiser { get; set; }
        public IEnumerable<ApprovarViewModel> Approvars { get; set; }
        public FinalApprovarViewModel FinalApprovar { get; set; }
    }
    public class ApprovarViewModel
    {
        public int? id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public int sequence { get; set; }
    }
    public class RaiserViewModel
    {
        public int? id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
    }
    public class FinalApprovarViewModel
    {
        public int? id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
    }


}
