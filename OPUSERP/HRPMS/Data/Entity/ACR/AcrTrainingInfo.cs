using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.ACR
{
    [Table("AcrTrainingInfo", Schema = "ACR")]
    public class AcrTrainingInfo : Base
    {
        public int? assessmentId { get; set; }
        public Assessment assessment { get; set; }
        [MaxLength(10)]
        public string EmpCode { get; set; }
        [MaxLength(100)]
        public string FromDate { get; set; }
        [MaxLength(100)]
        public string ToDate { get; set; }
        [MaxLength(200)]
        public string TraingInstitute { get; set; }
        [MaxLength(500)]
        public string TrainingTitle { get; set; }
        [MaxLength(4)]
        public string TrainingYear { get; set; }
        [MaxLength(200)]
        public string TrainingCountry { get; set; }
        public string Remarks { get; set; }
    }
}
