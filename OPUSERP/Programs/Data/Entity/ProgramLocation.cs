﻿using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Programs.Data.Entity
{
    [Table("ProgramLocation")]
    public class ProgramLocation:Base
    {
        public int? programMasterId { get; set; }
        public ProgramMaster programMaster { get; set; }
        public string location { get; set; }
    }
}
