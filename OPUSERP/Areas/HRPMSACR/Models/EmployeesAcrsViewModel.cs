using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSACR.Models
{
    public class EmployeesAcrsViewModel
    {
        [NotMapped]
        public int? acrEvaluationNameId { get; set; }
        [NotMapped]
        public string empCode { get; set; }
        public string evaluationName { get; set; }
        [NotMapped]
        public string description { get; set; }
        public int? fifthValue { get; set; }
        public int? forthValue { get; set; }
        public int? thirdValue { get; set; }
        public int? sencondValue { get; set; }
        public int? firstValue { get; set; }
        //[NotMapped]
        public string signaturePath { get; set; }
        //[NotMapped]
        public string signaturePath2 { get; set; }
        [NotMapped]
        public int? acrForId { get; set; }
        [NotMapped]
        public int? acrTypeId { get; set; }
        [NotMapped]
        public int? TotalMarks { get; set; }
        public string Grade { get; set; }
       
        public string InWordMarks { get; set; }
        
        public string InWordSignatoryMarks { get; set; }
        public string InWordAssessmentMarks { get; set; }

        //
        public decimal? assessmentValue { get; set; }
        [NotMapped]
        public int? Value { get; set; }
        [NotMapped]
        public int? ValueSignatory { get; set; }
        public int? TotalValueSignatory { get; set; }
        [NotMapped]
        public int? TotalValue { get; set; }
    }

	public class EmployeesAcrsViewModelNew
	{
		[NotMapped]
		public int? acrEvaluationNameId { get; set; }
		[NotMapped]
		public string empCode { get; set; }
		public string evaluationName { get; set; }
		[NotMapped]
		public string description { get; set; }
		[NotMapped]
		public int? fifthValue { get; set; }
		public int? forthValue { get; set; }
		public int? thirdValue { get; set; }
		public int? sencondValue { get; set; }
		public int? firstValue { get; set; }
		//[NotMapped]
		public string signaturePath { get; set; }
		//[NotMapped]
		public string signaturePath2 { get; set; }

		[NotMapped]
		public int? acrTypeId { get; set; }
		[NotMapped]
		public int? TotalMarks { get; set; }
		public string Grade { get; set; }

		public string InWordMarks { get; set; }

		public string InWordSignatoryMarks { get; set; }
		public string InWordAssessmentMarks { get; set; }

		//
		public decimal? assessmentValue { get; set; }
		public int? Value { get; set; }
		[NotMapped]
		public int? ValueSignatory { get; set; }
		public int? TotalValueSignatory { get; set; }
		[NotMapped]
		public int? TotalValue { get; set; }
	}


    public class EmployeesAcrsViewModelNew2
    {
        public string evaluationName { get; set; }
        public string description { get; set; }
        public int? Value { get; set; }
        public int? ValueSignatory { get; set; }
        public int? TotalValue { get; set; }
        public string Grade { get; set; }
        public int? TotalValueSignatory { get; set; }
        public int? forthValue { get; set; }
        public int? thirdValue { get; set; }
        public int? sencondValue { get; set; }
        public int? firstValue { get; set; }
        public int? fifthValue { get; set; }
        public int? assessmentValue { get; set; }
        public string InWordAssessmentMarks { get; set; }
        public string InWordMarks { get; set; }
        public string InWordSignatoryMarks { get; set; }
    }
}
