﻿using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Accounting.Data.Entity.NonPoTransaction
{
    [Table("RateType")]
    public class RateType : Base
    {
        public string RateTypeName { get; set; }
    }
}
