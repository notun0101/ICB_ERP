using OPUSERP.Data.Entity;
using OPUSERP.Data.Entity.Matrix;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.ACR
{
    [Table("Assessment")]
    public class Assessment : Base
    {
        [MaxLength(50)]
        public string empCode { get; set; }
        [MaxLength(100)]
        public string assessmentNo { get; set; }
        public string assessmentYear { get; set; }
        public DateTime? assessmentDate { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        [MaxLength(100)]
        public string signatory { get; set; }
        public DateTime? signatoryDate { get; set; }
        [MaxLength(100)]
        public string recommendator { get; set; }
        public DateTime? recommendationDate { get; set; }
        public string durationInRecommendetor { get; set; }
        public DateTime? duration1 { get; set; }
        public DateTime? duration2 { get; set; }
        public string goalAchievement { get; set; }
        public int? statusInfoId { get; set; }  // 8 = cancle , 7 = report , 1 =  submit
        public StatusInfo statusInfo { get; set; }
        public DateTime? assignToRecomDate { get; set; }
        [StringLength(50)]
        public string empType { get; set; }
        public decimal? assessmentValue { get; set; }
        [NotMapped]
        public string empName { get; set; }
		public string acrType { get; set; }

		public int? routeOrder { get; set; } //1=way to recomendator, 2=way to signatory, 3=to the md
		public string recomAssignedBy { get; set; }
		public string signatoryAssignedBy { get; set; }
        //For Sixth Part
        public DateTime? receivingDate { get; set; }
        public DateTime? hrDate { get; set; }
        public string reasonOfLate { get; set; }
        public string actionOnApplication { get; set; }
		public string responsibleUser { get; set; }

        public string newSignatory { get; set; } //not used
    }
}
