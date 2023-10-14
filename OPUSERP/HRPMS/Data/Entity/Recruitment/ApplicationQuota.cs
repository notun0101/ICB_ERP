using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Recruitment
{
    [Table("ApplicationQuota", Schema = "HR")]
    public class ApplicationQuota : Base
    {
        public int? isFredomFighter { get; set; } //1=yes, 0=no
        public string relation { get; set; }
        public string document { get; set; } //file
        public int? isDisable { get; set; } //1=yes, 0=no
        public int? isTrible { get; set; } //1=yes, 0=no
        public int? isAnser { get; set; } //1=yes, 0=no
        public int? isOther { get; set; } //1=yes, 0=no

        public string otherQuotaName { get; set; }
        public string otherQuotaDoc { get; set; } //file

        public int applicationFormId { get; set; }
        public ApplicationForm applicationForm { get; set; }
    }
}
