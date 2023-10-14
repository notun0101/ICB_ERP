using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
    [Table("EmployeeRole")]
    public class EmployeeRole : Base
    {
        public int? employeeId { get; set; }
        public EmployeeInfo employee { get; set; }

        public int? roleId { get; set; }
        public CustomRole role { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }

        public int? status { get; set; }
    }
}
