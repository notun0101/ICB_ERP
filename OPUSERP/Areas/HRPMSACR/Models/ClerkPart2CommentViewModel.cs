using System;
using System.Collections.Generic;

namespace OPUSERP.Areas.HRPMSACR.Models
{
    public class ClerkPart2CommentViewModel
    {
        public int? assessmentId { get; set; }
        
        public string remarks { get; set; }
        public string signatoryRemarks { get; set; }
        public string specificJob { get; set; }
        
        public string additionalComment { get; set; }
        public DateTime? recomDate { get; set; }
        public string signDate { get; set; }
        public string signatoryCode { get; set; }
        public int? recommendation { get; set; }
        public int? signRecommendation { get; set; }
        public string languageUsage { get; set; }
        public string signlanguageUsage { get; set; }
    }
}
