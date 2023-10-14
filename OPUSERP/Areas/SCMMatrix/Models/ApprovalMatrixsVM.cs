using Microsoft.CodeAnalysis;
using OPUSERP.Data.Entity.Matrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.SCMMatrix.Models
{
	public class ApprovalMatrixsVM
	{
		public string matrixTypeName { get; set; }
		public string projectName { get; set; }
		public string Raiser { get; set; }
		public string Approver { get; set; }
		public int projectId { get; set; }
		public int sequenceNo { get; set; }
	}
	public class ApprovalMatrixsVM1
	{
		public string projectName { get; set; }
		public int projectId { get; set; }
		public string raiserName { get; set; }
		public string matrixTypeName { get; set; }
		public int? matrixTypeId { get; set; }
		public int raiserId { get; set; }
		public string raiserPhoto { get; set; }
		public IEnumerable<ApprovarsByProject> Approvars { get; set; }

        public IEnumerable<Project> projects { get; set; }
       
    }
	public class ApprovarVm
	{
		public string approvarName { get; set; }
	}
	public class ProjectWithUserVm
	{
		public string projectName { get; set; }
		public string matrixTypeName { get; set; }
		public string Raiser { get; set; }
		public string Photo { get; set; }
		public int? matrixTypeId { get; set; }
		public int projectId { get; set; }
		public int userid { get; set; }
		public List<ApprovalMatrixViewModel> approvalList { get; set; }
	}
	public class ApprovarsByProject
	{
		public string ApprovarName { get; set; }
		public string Photo { get; set; }
		public int ApprovarType { get; set; }
		public int sequenceNo { get; set; }
	}

    public class ApprovalMatrixListViewModel
    {
        public IEnumerable<ApprovalMatrixsVM1> approvalMatricess { get; set; }
        public IEnumerable<MatrixType> matrixTypes { get; set; }
    }
}
