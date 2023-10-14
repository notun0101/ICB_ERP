using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Leave;
using OPUSERP.Areas.HRPMSLeave.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Employee;
using Microsoft.AspNetCore.Http;
using OPUSERP.Areas.HRPMSLeave.Models.NotMapped;
using OPUSERP.Data.Entity.Master;
using OPUSERP.Data.Entity;
using OPUSERP.Areas.SCMMatrix.Models.Lang;
using OPUSERP.Data.Entity.Matrix;
using OPUSERP.Areas.SCMMatrix.Models;
using OPUSERP.Areas.Auth.Models;
using OPUSERP.Distribution.Data.Entity.Requisition;
using OPUSERP.SCM.Data.Entity.MasterData;

namespace OPUSERP.Areas.HRPMSLeave.Models
{
    public class LeaveRegisterNewVM
    {
        public int? reqId { get; set; }
        public int? matrixId { get; set; }
        public int? leaveTypeId { get; set; }
        public int? approvalMasterId { get; set; }
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

        public IEnumerable<Project> projects { get; set; }
        public IEnumerable<MatrixType> matrixTypes { get; set; }
        public IEnumerable<ApprovalType> approvalTypes { get; set; }
        public IEnumerable<ApprovalMaster> approvalMasters { get; set; }
        public ApprovalMaster approvalMaster { get; set; }
        public IEnumerable<ApprovalDetail> approvalDetails { get; set; }
        
    }

    public class ApprovalMastersVM
    {
        public int projectId { get; set; }
        public string projectName { get; set; }
        public int? raiserId { get; set; }
        public string raiserName { get; set; }
        public string raiserEmployeeCode { get; set; }
        public string approverTypeName { get; set; }
        public int? approverTypeId { get; set; }
        public int? approvalMasterId { get; set; }
        public string raiserPhoto { get; set; }
        public IEnumerable<ApprovalDetail> approvalDetails { get; set; }

        public IEnumerable<Project> projects { get; set; }

    }

    public class ApprovalListViewModel
    {
        public IEnumerable<ApprovalMastersVM> approvalMasters { get; set; }
        public IEnumerable<ApprovalType> approvalTypes { get; set; }
    }
}
