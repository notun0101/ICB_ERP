using ACR.Data.ViewModel.Evaluation;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.HRPMS.Data.Entity.ACR;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Leave;
using OPUSERP.Workflow.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSACR.Models
{
	public class ACREvaluationViewModel
	{
		public string actionType { get; set; }
		public string acrType { get; set; }
        public int? isHRAdmin { get; set; }
        public string empCode { get; set; }
		public int? assessmentId { get; set; }
		public string evaluationName { get; set; }
		public int? headId { get; set; }
		public AssessmentViewModel AssessmentBasic { get; set; }
		public IEnumerable<DesignationViewModel> Designations { get; set; }
		public List<EmployeesAcr> EmployeesAcr { get; set; }
		//public List<EmployeeLeaveInfoViewModel> leaveInfo { get; set; }
		public IEnumerable<AcrEvaluationName> AcrEvaluationList { get; set; }
		public IEnumerable<EmployeeLeaveInfoViewModel> leaveInfo { get; set; }
		public EmployeeUserViewModel ACRUsers { get; set; }
		public AcrUserViewModel ACRUser { get; set; }
		public IEnumerable<LeaveType> LeaveTypes { get; set; }
		public IEnumerable<QuantitativeEvaluationHead> heads { get; set; }
		public IEnumerable<EvaluationCommentsHead> commentsHead { get; set; }
		public IEnumerable<EmployeeEducationInfoViewModel> employeeEducationInfoViewModels { get; set; }
		public IEnumerable<EmployeeTrainingViewModel> employeeTrainingViewModels { get; set; }
		public IEnumerable<PrevJobHistory> prevJobHistories { get; set; }
		public IEnumerable<EmployeePromotionViewModel> employeePromotionViewModels { get; set; }
		public IEnumerable<EmployeeAssignmentViewModel> employeeAssignmentViewModels { get; set; }
		public IEnumerable<ACRQuantitativeEvaluationViewModel> aCRQuantitativeEvaluationViewModels { get; set; }
		public IEnumerable<QuantitativeEvaluation> quantitativeEvaluations { get; set; }
		public IEnumerable<OfficerPart5ViewModel> officerPart5ViewModels { get; set; }
		public IEnumerable<EmployeesAcrsViewModel> employeesAcrsViewModels { get; set; }
		public IEnumerable<EmployeesAcrsViewModel> evaluationOfficerPartTwoViewModels { get; set; }
		public IEnumerable<ACRIndicator> aCRIndicators { get; set; }
		public IEnumerable<EmpEducationSpViewModel> empEducations { get; set; }
		public IEnumerable<EmpPostingsSpViewModel> empPostings { get; set; }
		public IEnumerable<ProfessionalQualifications> professionalQualifications { get; set; }
		public IEnumerable<DocumentAttachments> documentAttachments { get; set; }
        
        public EmployeeACRInfoViewModel empAcr { get; set; }
        public ACREmployeeGradeViewModel empAcrGrade { get; set; }
        public EmployeeSignature empSignature { get; set; }
        public ACRDetailsInfoViewModel aCRDetails { get; set; }
        public ExecutiveOfficerPart5ViewModel executiveOfficerPart5ViewModel { get; set; }

        public int childCount { get; set; }
        public int? assessmentId5thPart { get; set; }
        public string specialRemarks { get; set; }

		public AcrTotal acrTotal { get; set; }
		public ACRRecommendations ACRRecommendations { get; set; }
	}

    public class ACREmployeeGradeViewModel
    {
        public int? EmpId { get; set; }
        public string currentGrade { get; set; }
        public decimal? currentBasic { get; set; }
    }

        public class AcrTotal
	{
		public int? TotalMarks { get; set; }
	}
	public class EmpEducationSpViewModel
	{
		public string examName { get; set; }
		public string groupOrSubject { get; set; }
		public string result { get; set; }
		public int? passingYear { get; set; }
		public string organizationName { get; set; }
		public int? type { get; set; }
	}
    public class ReCommendatorViewModel
    {
        public EmployeeInfo employee { get; set; }
        public Assessment assessment { get; set; }
        public EmployeeSignature signatureUrl { get; set; }

        public IEnumerable<EmployeeUserViewModel> lstDrawer { get; set; }

    }
	public class EmpPostingsSpViewModel
	{
		public string designationName { get; set; }
		public string designationNameBN { get; set; }
		public string placeNameBn { get; set; }
		public string placeName { get; set; }
		public DateTime? startDate { get; set; }
		public DateTime? endDate { get; set; }

		public int? totalDaysInBranch { get; set; }
	}
}
