﻿using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.CRM.Data.Entity.MasterData
{
    [Table("ActivitySession")]
    public class ActivitySession : Base
    {
       
        [Required]
        public string activitySessionName { get; set; }
    }
}
