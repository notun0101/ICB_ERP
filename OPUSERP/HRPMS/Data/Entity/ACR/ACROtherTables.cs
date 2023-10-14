using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.ACR
{
    [Table("ACROtherTables")]
    public class ACROtherTables:Base
    {
        public int? assessmentId { get; set; }
        public Assessment assessment { get; set; }
        [MaxLength(3)]
        public string isPhysicallyCapable { get; set; }
        public string bankingExperience { get; set; }
        public string nobankingExperience { get; set; }
    }
}
