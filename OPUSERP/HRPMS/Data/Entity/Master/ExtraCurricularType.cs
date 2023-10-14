using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
    [Table("ExtraCurricularType", Schema = "HR")]
    public class ExtraCurricularType:Base
    {
        public string name { get; set; }
    }
}
