using OPUSERP.Data.Entity.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
    public class JDTaskTypeViewModel
    {
        public string AdditionalDescription { get; set; }

        public int? jdTypeId { get; set; }

        public string jdTaskTypeId { get; set; }

        public JDType jdType { get; set; }

        public int? jdTaskListId { get; set; }

        public JDTaskList jdTaskList { get; set; }

        public IEnumerable<JDType> jDTypes { get; set; }

        public IEnumerable<JDTaskList> jDTaskLists { get; set; }

        public IEnumerable<JDTaskType> jDTaskTypes { get; set; }
        
    }
}
