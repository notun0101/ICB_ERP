﻿using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.CRM.Data.Entity.MasterData
{
    [Table("ActivityNature")]
    public class ActivityNature : Base
    {
       
        [Required]
        public string activityNatureName { get; set; }
    }
}
