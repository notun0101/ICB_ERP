using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
    [Table("OutsourcesCompany", Schema = "HR")]
    public class OutsourcesCompany:Base
    {
        public string name { get; set; }
        public string termsandcondition { get; set; }
        public string url { get; set; }
    }
}
