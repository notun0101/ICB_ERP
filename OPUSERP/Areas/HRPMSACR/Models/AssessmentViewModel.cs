using OPUSERP.HRPMS.Data.Entity.ACR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSACR.Models
{
    public class AssessmentViewModel
    {
        public int? assessmentId { get; set; }
        public string empCode { get; set; }
        public string assessmentNo { get; set; }
        public string assessmentYear { get; set; }
        public DateTime? assessmentDate { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public string fDate { get; set; }
        public string tDate { get; set; }
        public string recommendator { get; set; }
        public DateTime? recommendationDate { get; set; }
        public string recommendatorName { get; set; }
        public string recommendatorDesig { get; set; }
        public string recomSign { get; set; }
        public string signatory { get; set; }
        public DateTime? signatoryDate { get; set; }
        public DateTime? duration1 { get; set; }
        public DateTime? duration2 { get; set; }
        public string signatoryName { get; set; }
        public string signatoryDesig { get; set; }
        public string signatorySign { get; set; }
        public string Signatory2 { get; set; }
        public string fsignatoryName { get; set; }
        public string fsignatoryDesig { get; set; }
        public string fsignatorySign { get; set; }
        public string ownSign { get; set; }
        public int? EmpId { get; set; }
        public string empName { get; set; }
        public string DesignationName { get; set; }
        public string DesiCode { get; set; }
        public string BranchCode { get; set; }
        
        public string BranchName { get; set; }
        public string actionType { get; set; }
        public int? statusInfoId { get; set; }
		public string statusInfo { get; set; }
		public string empType { get; set; }
        public string attachmentPath { get; set; }
        public string certificationPath { get; set; }
        public string certificationPath92 { get; set; }
        public int? SlNo { get; set; }
        public IEnumerable<Assessment> Assessments { get; set; }
    }
    public class LeaveDetailsViewModel
    {
        public int? leaveAssessId { get; set; }
        public int?[] leaveId { get; set; }
        public int?[] leaveTypeId { get; set; }
        public int?[] totalLeave { get; set; }
        public int?[] consumptionLeave { get; set; }
        public int?[] leaveBalance { get; set; }

        //Additional
        public string drawerCode { get; set; }
        public string durationInRecommendetor { get; set; }
        public DateTime? duration1 { get; set; }
        public DateTime? duration2 { get; set; }
        public string expBanking { get; set; }

        public string signatoryF { get; set; }
        

    }

    public class ACROtherTableViewModel
    {
        public int? assessmentId { get; set; }
        public string isPhysicallyCapable { get; set; }
        public string bankingExperience { get; set; }
        public string nobankingExperience { get; set; }

    }
    public class DeleteAcrDataViewModel
    {
        public int? assessmentId6thPart { get; set; }
    }

	public class GetQuantitativeEvaByAssIdVm
	{
		public int? assessmentId { get; set; }
		public int? aCRIndicatorId { get; set; }
		public int? headId { get; set; }
		public decimal? target { get; set; }
		public decimal? achievement { get; set; }
		public decimal? achievementPercent { get; set; }
		public decimal? achievementSign { get; set; }
		public decimal? achievementPercentSign { get; set; }
		public decimal? acrCounter { get; set; }
		public string posting { get; set; }
	}
}
