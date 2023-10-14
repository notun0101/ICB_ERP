using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Suspensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSDisciplineInvestigation.Models
{
    public class EmpSuspensionViewModel
    {
        public EmployeeInfo employeeInfo { get; set; }
        public Suspension suspension { get; set; }
        public Designation designation { get; set; }
    }
}
