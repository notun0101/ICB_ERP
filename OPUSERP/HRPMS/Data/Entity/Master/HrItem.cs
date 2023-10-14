using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
    [Table("HrItem", Schema = "HR")]
    public class HrItem:Base
    {
        public string name { get; set; }
        public decimal price { get; set; }
        public string supplier { get; set; }
        public DateTime? expireDate { get; set; }

        public int status { get; set; }
    }
}
