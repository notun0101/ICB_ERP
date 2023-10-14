using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.Leave
{
    [Table("LeaveOpeningBalance")]
    public class LeaveOpeningBalance: Base
    {
        public int? employeeId { get; set; }
        public EmployeeInfo employee { get; set; }

        public int? leaveTypeId { get; set; }
        public LeaveType leaveType { get; set; }

        public int? yearId { get; set; }
        public Year year { get; set; }

        public decimal? leaveDays { get; set; }

        public decimal? leaveCarryDays { get; set; }
    }
}
