using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
    [Table("EmployeeExtraCurricular")]
    public class EmployeeExtraCurricular:Base
    {
        public int? employeeId { get; set; }
        public EmployeeInfo employee { get; set; }

        public int? extraCurricularTypeId { get; set; }
        public ExtraCurricularType extraCurricularType { get; set; }

        public string skillLevel { get; set; } // Primary, Mid-Level, Expert

        public string skillType { get; set; } //National, Regional

        public string description { get; set; }
    }
}
