﻿using OPUSERP.CRM.Data.Entity.Lead;
using OPUSERP.CRM.Data.Entity.MasterData;
using OPUSERP.Data.Entity.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace OPUSERP.Areas.CRMLead.Models
{
    public class ActivityMasterCAViewModel
    {
        
        public string number { get; set; }
        public int parentId { get; set; }
        public ActivityMaster activityMasters { get; set; }
        public List<ActivityDetails> activityDetails { get; set; }
        public List<ActivityTeam> activityTeam { get; set; }

        
    }
}
