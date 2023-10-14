using OPUSERP.Data.Entity.Master;
using OPUSERP.Data.Entity.MasterData;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
    public class JDTypeViewModel
    {
        public int JDTypeId { get; set; }
        public string roleId { get; set; }
        public string JDRoleName { get; set; }
        public string departmentname { get; set; }
        public string divisionname { get; set; }
        public string designationname { get; set; }


        public string departmnetTypeId { get; set; }
        public Department departmentType { get; set; }
        public IEnumerable<Department> departments { get; set; }

        public string divisionTypeId { get; set; }
        public Division divisionType { get; set; }
        public IEnumerable<Division> divisions { get; set; }

        public string designationTypeId { get; set; }
        public Designation designationType { get; set; }
        public IEnumerable<Designation> designations { get; set; }

        public IEnumerable<JDType> jDTypes { get; set; }

    }
}
