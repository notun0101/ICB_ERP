using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.ACR
{
    [Table("ACRComments", Schema = "ACR")]
    public class ACRComments:Base
    {
        public int? assessmentId { get; set; }
        public Assessment assessment { get; set; }
        [MaxLength(500)]
        public string remarks { get; set; }
        [MaxLength(300)]
        public string specificJob { get; set; }
        public int? livingStandard { get; set; }
        public string languageUsage { get; set; }
        public int? recommendation { get; set; }

        public string signatoryRemarks { get; set; }
        public string signlanguageUsage { get; set; }
        public int? signRecommendation { get; set; }
    }
}
