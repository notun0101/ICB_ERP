using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSACR.Models
{
    public class EvaluationOfficerPartTwoViewModel
    {
        public string evaluationName { get; set; }
        public string description { get; set; }
        public int? Value { get; set; }
        public int? ValueSignatory { get; set; }
        public int? TotalValueSignatory { get; set; }
        public int? TotalValue { get; set; }
        public string Grade { get; set; }
        public int? fifthValue { get; set; }
        public int? forthValue { get; set; }
        public int? thirdValue { get; set; }
        public int? sencondValue { get; set; }
        public int? firstValue { get; set; }
        public decimal? assessmentValue { get; set; }
        public string InWordMarks { get; set; }
        public string InWordSignatoryMarks { get; set; }
        public string InWordAssessmentMarks { get; set; }
    }
}
