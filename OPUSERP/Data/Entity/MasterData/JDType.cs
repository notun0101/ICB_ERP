using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Data.Entity.MasterData
{
    public class JDType:Base
    {
      
        public string JDRoleName { get; set; }

        public int roleId { get; set; }

        public int? departmentId { get; set; }

        public Department department { get; set; }

        public int? designationId { get; set; }

        public Designation designation { get; set; }

        public int? divisionId { get; set; }

        public Division division { get; set; }

       
    }
}
