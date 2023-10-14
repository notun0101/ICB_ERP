using OPUSERP.Data.Entity.MasterData;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
    public class JDTaskListViewModel
    {
        public string JDListId { get; set; }
        public string JdListName { get; set; }
        public IEnumerable<JDTaskList> jDTaskLists { get; set; }
    }
}
