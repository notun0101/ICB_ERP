﻿using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.CRM.Data.Entity.MasterData
{
    [Table("ActivityCategory")]
    public class ActivityCategory : Base
    {
       
        [Required]
        public string activityCategoryName { get; set; }
    }
}
