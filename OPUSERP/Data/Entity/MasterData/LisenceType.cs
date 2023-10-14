using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Data.Entity.MasterData
{
    [Table("LisenceType", Schema = "HR")]
    public class LisenceType:Base
    {
        public string typeName { get; set; }

        public int? isActive { get; set; }
    }
}
