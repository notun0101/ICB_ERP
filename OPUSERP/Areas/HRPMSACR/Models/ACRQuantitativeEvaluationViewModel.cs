namespace ACR.Data.ViewModel.Evaluation
{
    public class ACRQuantitativeEvaluationViewModel
    {
        public int? indicatorId { get; set; }
        public string indicatorNameBn { get; set; }
        public int? headId { get; set; }
        public int? evaluationId { get; set; }
        public decimal? target { get; set; }
        public decimal? achievement { get; set; }
        public decimal? achievementPercent { get; set; }
        public decimal? achievementSign { get; set; }
        public decimal? achievementPercentSign { get; set; }
        public decimal? acrCounter { get; set; }
        public int? assessmentId { get; set; }
        public int? evaluationCommentsHeadId { get; set; }
    }
}
