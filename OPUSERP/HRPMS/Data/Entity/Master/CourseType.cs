using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
    [Table("CourseType", Schema = "HR")]
    public class CourseType:Base
    {
        public string typeNameEN { get; set; }

        public string typeNameBN { get; set; }

        public string remarks { get; set; }

        public int status { get; set; }
    }
}
