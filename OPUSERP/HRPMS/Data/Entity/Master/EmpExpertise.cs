using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
    [Table("EmpExpertise")]
    public class EmpExpertise : Base
    {
        public string nameEn { get; set; }
        public string nameBn { get; set; }
        public string remarks { get; set; }
    }
}
