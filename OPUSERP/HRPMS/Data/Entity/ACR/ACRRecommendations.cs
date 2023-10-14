using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.ACR
{
    [Table("ACRRecommendations", Schema = "ACR")]
    public class ACRRecommendations:Base
    {
        public int? assessmentId { get; set; }
        public Assessment assessment { get; set; }
        [MaxLength(500)]
        public string specialComment { get; set; }
        [MaxLength(500)]
        public string moralComment { get; set; }
        [MaxLength(500)]
        public string intellectualComment { get; set; }
        [MaxLength(500)]
        public string subjectiveComment { get; set; }
        [MaxLength(500)]
        public string trainingComment { get; set; }
        public int? promotionalQualification { get; set; }
        [MaxLength(1000)]
        public string otherQualification { get; set; }
        public int? promotionalQualificationSignatory { get; set; }
        [MaxLength(10)]
        public string moralCommentSignatory { get; set; }
        [MaxLength(10)]
        public string intellectualCommentSignatory { get; set; }
        [MaxLength(10)]
        public string subjectiveCommentSignatory { get; set; }

		public int? PromotionValue { get; set; }
		public int? IncrementValue { get; set; }
		public int? PermanentValue { get; set; }
		public string logical { get; set; }
		public int? type { get; set; }

		public int? evaluationValueFirstRecom { get; set; }
		public int? evaluationValueSecondRecom { get; set; }

		public DateTime? evaluationDateFirstRecom { get; set; }
		public DateTime? evaluationDateSecondRecom { get; set; }

		public decimal? evaluationTotalFirstRecom { get; set; }
		public decimal? evaluationTotalSecondRecom { get; set; }

		public string evaluationTextFirstRecom { get; set; }
		public string evaluationSecondFirstRecom { get; set; }

		public string Recom5Name { get; set; }
		public string Recom5Desig { get; set; }
		public string Recom5Code { get; set; }

		public string Recom6Name { get; set; }
		public string Recom6Desig { get; set; }
		public string Recom6Code { get; set; }
	}
}
