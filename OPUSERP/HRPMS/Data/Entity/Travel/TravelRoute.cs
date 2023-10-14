using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Travel
{
    [Table("TravelRoute")]
    public class TravelRoute:Base
    {
        public int? travelMasterId { get; set; }
        public TravelMaster travelMaster { get; set; }

        //Supervisor Id
        public int? employeeId { get; set; }
        public EmployeeInfo employee { get; set; }

        public int? routeOrder { get; set; }

        public int? isActive { get; set; }
    }
}
