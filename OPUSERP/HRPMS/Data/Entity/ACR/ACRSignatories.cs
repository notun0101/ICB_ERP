using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.ACR
{
    [Table("ACRSignatories")]
    public class ACRSignatories:Base
    {
        public int? assessmentId { get; set; }
        public Assessment assessment { get; set; }
        public int? evaluationType { get; set; }
        public string disAgreementComment { get; set; }
        public string additionalComment { get; set; }
    }
}
