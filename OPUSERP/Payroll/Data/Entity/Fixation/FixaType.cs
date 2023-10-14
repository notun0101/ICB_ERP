using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Data.Entity.Fixation
{
    [Table("FixaType", Schema = "Fixation")]
    public class FixaType:Base
    {
        public string FixationTypeName { get; set; }
    }
}
