using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
    [Table("ClientServeLost", Schema = "HR")]
    public class ClientServeLost:Base
    {
        public string year { get; set; }
        public int? clientServe { get; set; }
        public int? clientLost { get; set; }

        public string businessTarget { get; set; }
        public string businessGrowth { get; set; }

        public string profit { get; set; }
        public string dividend { get; set; }
    }
}
