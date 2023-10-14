using System;
using System.Collections.Generic;

namespace OPUSERP.Areas.HRPMSACR.Models
{
    public class OfficerPart5ViewModel
    {
        public int? assessmentId5thPart { get; set; }
        public int? livingStandard { get; set; }

        public int? promotionalQualification { get; set; }
        public int? IncrementValue { get; set; }
        public int? PermanentValue { get; set; }
		
        public string disAgreementComment { get; set; }
        public string additionalComment { get; set; }
        public string specialRemarks { get; set; }
        public int? promotionalQualificationSignatory { get; set; }

		public string signEmpId { get; set; }
		public string signEmpName { get; set; }
		public string signDesig { get; set; }

		public string logically { get; set; }

		public int? evaluationType { get; set; }
		public int? evaluationValueSecondRecom { get; set; }

		public DateTime? evaluationDateFirstRecom { get; set; }
		public DateTime? evaluationDateSecondRecom { get; set; }
		public DateTime? evaluationDateSecondRecomNew { get; set; }

		public decimal? evaluationTotalFirstRecom { get; set; }
		public decimal? evaluationTotalSecondRecom { get; set; }
		public int? evaluationTotalRecommendator { get; set; }

		public string evaluationTextFirstRecom { get; set; }
		public string evaluationTextSecondRecom { get; set; }

		public string Recom5Name { get; set; }
		public string Recom5Desig { get; set; }
		public string Recom5Code { get; set; }

		public string Recom6Name { get; set; }
		public string Recom6Desig { get; set; }
		public string Recom6Code { get; set; }

        public string finalApprover { get; set; }
    }
}
