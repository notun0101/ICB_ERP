using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.ACR
{
    [Table("DocumentAttachments")]
    public class DocumentAttachments:Base
    {
        public int? assesmentId { get; set; }

        [MaxLength(10)]
        public string empCode { get; set; }
        [MaxLength(300)]
        public string attachmentpath { get; set; }
        [MaxLength(100)]
        public string attachFor { get; set; }//TargetAchive
    }
}
