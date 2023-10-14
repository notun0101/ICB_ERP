﻿using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
    [Table("NaturalDigester", Schema = "HR")]
    public class NaturalDigester:Base
    {
        public string name { get; set; }
        public string description { get; set; }
    }
}
