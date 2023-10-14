using OPUSERP.CRM.Data.Entity.Lead;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.CRMLead.Models
{
    public class ActivityMasterViewModelForTree
    {
        public string text { get; set; }
        public string icon { get; set; }
        public int parentId { get; set; }
        public ActivityMaster activityMaster { get; set; }
        public IEnumerable<ActivityMaster> activityMasters { get; set; }
        public IEnumerable<ActivityMasterViewModelForTree> activityMastersVM { get; set; }
    }
}
