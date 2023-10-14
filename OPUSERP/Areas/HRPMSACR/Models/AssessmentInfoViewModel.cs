using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSACR.Models
{
    public class AssessmentInfoViewModel
    {
        public int? assessmentId { get; set; }
        public DateTime? assessmentDate { get; set; }
        
        public string assessmentNo { get; set; }
        public string assessmentYear { get; set; }
        public string empCode { get; set; }
        
        public string fDate { get; set; }
        public string tDate { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        
        public int? Id { get; set; }
        public string recommendator { get; set; }
        public string recommendatorName { get; set; }
        public string recommendatorDesig { get; set; }
        public string recomSign { get; set; }

        public string signatory { get; set; }
        public string signatoryDate { get; set; }
        public string signatoryName { get; set; }
        public string signatoryDesig { get; set; }
        public string signatorySign { get; set; }

        public string FinalSignatoryName { get; set; } //Asad Added

        public string empName { get; set; }       
        public string DesiCode { get; set; }
        public string designation { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string branch { get; set; }
        
        public int? statusInfoId { get; set; }
        public string statusInfo { get; set; }
        public string actionType { get; set; }
		public string empType { get; set; }
    }

	public class AcrForAdminViewModel
	{
		public int? assessmentId { get; set; }
		public string empCode { get; set; }
		public string empName { get; set; }
		public string designationCode { get; set; }
		public string assessmentNo { get; set; }
		public string assessmentYear { get; set; }
		public DateTime? assessmentDate { get; set; }
		public DateTime? fromDate { get; set; }
		public DateTime? toDate { get; set; }
		public int? statusInfoId { get; set; }
		public string recommendator { get; set; }
		public string recommendatorName { get; set; }
		public string signatory { get; set; }
		public string signatory2 { get; set; }
		public string signatoryName { get; set; }
		public string acrType { get; set; }
		public int? routeOrder { get; set; }
		public string empType { get; set; }
		public string signatory2Name { get; set; }
	}

	public class AllAcrsViewModel
	{
		public IEnumerable<AllAcrSp> allAcrSps { get; set; }
	}

	public class AllAcrSp
	{
		public int? assessmentId { get; set; }
		public DateTime? assessmentDate { get; set; }

		public string assessmentNo { get; set; }
		public string assessmentYear { get; set; }
		public string empCode { get; set; }

		public string fDate { get; set; }
		public string tDate { get; set; }
		public DateTime? fromDate { get; set; }
		public DateTime? toDate { get; set; }

		public int? Id { get; set; }
		public string recommendator { get; set; }
		public string recommendatorName { get; set; }
		public string recommendatorDesig { get; set; }
		public string recomSign { get; set; }

		public string signatory { get; set; }
		public string signatoryDate { get; set; }
		public string signatoryName { get; set; }
		public string signatoryDesig { get; set; }
		public string signatorySign { get; set; }

		public string empName { get; set; }
		public string DesiCode { get; set; }
		public string designation { get; set; }
		public string BranchCode { get; set; }
		public string BranchName { get; set; }
		public string branch { get; set; }

		public int? statusInfoId { get; set; }
		public string statusInfo { get; set; }
		public string actionType { get; set; }
		public string empType { get; set; }
		public string ApproverType { get; set; }
	}

	public class NotificationVmAcr
	{
		public int? NotificationSendStatus { get; set; } = 0;
	}



    public class AssessmentInfoNewViewModel
    {
        public int? assessmentId { get; set; }
        public DateTime? assessmentDate { get; set; }

        public string assessmentNo { get; set; }
        public string assessmentYear { get; set; }
        public string empCode { get; set; }

        public string fDate { get; set; }
        public string tDate { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }

        public int? Id { get; set; }
        public string recommendator { get; set; }
        public string recommendatorName { get; set; }
        public string recommendatorDesig { get; set; }
        public string recomSign { get; set; }

        public string signatory { get; set; }
        public string signatoryDate { get; set; }
        public string signatoryName { get; set; }
        public string signatoryDesig { get; set; }
        public string signatorySign { get; set; }

        public string empName { get; set; }
        public string DesiCode { get; set; }
        public string designation { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string branch { get; set; }

        public int? statusInfoId { get; set; }
        public string statusInfo { get; set; }
        public string actionType { get; set; }
        public string empType { get; set; }
        public string reasonOfLate { get; set; }
        public string responsibleUser { get; set; }
    }
}
