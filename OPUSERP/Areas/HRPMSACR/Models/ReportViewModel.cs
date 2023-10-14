using ACR.Data.ViewModel.Evaluation;
using OPUSERP.HRPMS.Data.Entity.ACR;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSACR.Models
{
    public class ReportViewModel
    {
		public string acrType { get; set; }
		public IEnumerable<EmployeesInfoViewModel> employeeInfoViewModel { get; set; }
		public IEnumerable<EmployeesAcrDGMtoAboveViewModel> employeeInfoViewModelAcr { get; set; }
        public IEnumerable<EmployeesAcrsViewModel> employeesAcrsViewModel { get; set; }
        public ACRRecommendations aCRRecommendation { get; set; }
        public IEnumerable<EmployeesAcrsViewModelNew2> employeesAcrsViewModelN { get; set; }
        public IEnumerable<EmployeesAcrsViewModel> employeesPerformanceViewModel { get; set; }
        public IEnumerable<QuantitativeEvaluationsViewModel> quantitativeEvaluationsViewModel { get; set; }
        public IEnumerable<QuantitativeEvaluationsViewModel> quantitativeEvaluationsViewModel92 { get; set; }
        public IEnumerable<EmployeeTrainingViewModel> employeeTrainingViewModel { get; set; }
        public IEnumerable<EmployeeEducationInfoViewModel> employeeEducationInfoViewModel { get; set; }
        public IEnumerable<EmployeePromotionViewModel> employeePromotionViewModel { get; set; }
        public IEnumerable<EmployeeLeaveInfoViewModel> employeeLeaveInfoViewModel { get; set; }
        public IEnumerable<EmployeeAssignmentViewModel> employeeAssignmentViewModel { get; set; }
        public IEnumerable<ACROtherTableViewModel> acrOtherTableViewModel { get; set; }
        public IEnumerable<EmployeePromotionViewModel> employeePromotionViewModels { get; set; }
        public IEnumerable<EmployeeLeaveInfoViewModel> employeeLeaveInfoViewModels { get; set; }
        public IEnumerable<EvaluationOfficerPartTwoViewModel> evaluationOfficerPartTwoViewModels { get; set; }
        public DocumentAttachments document { get; set; }
        public int? evaluatedMarks { get; set; }
        public EmployeeAcrViewModel EmployeeAcrInfo { get; set; }


		public IEnumerable<EmpEducationSpViewModel> empEducations { get; set; }
		public IEnumerable<EmpPostingsSpViewModel> empPostings { get; set; }
		public IEnumerable<PrevJobHistoryVm> prevJobHistoryVms { get; set; }
        public IEnumerable<ProfessionalQualifications> professionalQualifications { get; set; }
    }
	public class PrevJobHistoryVm
	{
		public string company { get; set; }
		public string position { get; set; }
		public DateTime? jobstart { get; set; }
		public DateTime? jobend { get; set; }
		public int? totalDays { get; set; }
	}
}
