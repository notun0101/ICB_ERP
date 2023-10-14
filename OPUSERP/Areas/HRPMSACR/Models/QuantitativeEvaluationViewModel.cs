namespace OPUSERP.Areas.HRPMSACR.Models
{
    public class QuantitativeEvaluationViewModel
    {
        public int? qaassessmentId { get; set; }
        public int?[] qaId { get; set; }
        public int?[] aCRIndicatorId { get; set; }
        public decimal?[] target { get; set; }
        public decimal?[] achievement { get; set; }
        public decimal?[] achievementPercent { get; set; }
        public decimal?[] achievementSign { get; set; }
        public decimal?[] achievementPercentSign { get; set; }
        public decimal? acrCounter { get; set; }
        public decimal? acrCounterNinety { get; set; }
        public int? headId { get; set; }

        public string evaluationComments { get; set; }
        public int? evaluationCommentsHeadId { get; set; }
        public int? PerformanceCategory { get; set; }
    }
}
