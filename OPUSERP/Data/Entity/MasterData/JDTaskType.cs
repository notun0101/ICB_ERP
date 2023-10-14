using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Data.Entity.MasterData
{
    public class JDTaskType:Base
    {

        public string AdditionalDescription { get; set; }

        public int? jdTypeId { get; set; }

        public JDType jdType { get; set; }

        public int? jdTaskListId { get; set; }

        public JDTaskList jdTaskList { get; set; }

     
    }
}
