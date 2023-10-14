using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.Leave
{
    [Table("LeaveRoute")]
    public class LeaveRoute : Base
    {
        public int? leaveRegisterId { get; set; }
        public LeaveRegister leaveRegister { get; set; }

        //Supervisor Id
        public int? employeeId { get; set; }
        public EmployeeInfo employee { get; set; }

        public int? routeOrder { get; set; }

        public int? isActive { get; set; }
    }
}
