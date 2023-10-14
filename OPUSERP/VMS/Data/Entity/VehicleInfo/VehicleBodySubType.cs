﻿using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.VMS.Data.Entity.VehicleInfo
{
    [Table("VehicleBodySubType")]
    public class VehicleBodySubType : Base
    {
        [MaxLength(250)]
        public string vehicleBodySubTypeName { get; set; }
        public int? sortOrder { get; set; }
    }
}
