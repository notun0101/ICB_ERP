using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.ACR
{
    [Table("JobResponsibility", Schema = "ACR")]
    public class JobResponsibility:Base
    {
        public string ResponsibilityName { get; set; }
    }
}
