﻿using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.Master
{
    [Table("TempDay")]
    public class TempDay :Base
    {        
        public string daysInBetween { get; set; }
    }
}
