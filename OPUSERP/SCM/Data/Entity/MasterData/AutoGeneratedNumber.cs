﻿using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.SCM.Data.Entity.MasterData
{
    [Table("AutoGeneratedNumber", Schema = "SCM")]
    public class AutoGeneratedNumber:Base
    {
        public string Name { get; set; }
        public int isEmpCode { get; set; }
        public int isDate { get; set; }
        public int digit { get; set; }
        public string alphaNumeric { get; set; }
        public string numericStartPoint { get; set; }
        public string speratedSign { get; set; }
        public string format { get; set; }
    }
}