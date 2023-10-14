using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Leave;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSLeave.Models
{
    public class YearlyLeaveProcessViewModel
    {
        public int? employeeId { get; set; }
        public EmployeeInfo employee { get; set; }

        public int? leaveTypeId { get; set; }
        public LeaveType leaveType { get; set; }

        public int? yearId { get; set; }
        public Year year { get; set; }

        public decimal? leaveDays { get; set; }
        public decimal? leaveCarryDays { get; set; }
        public decimal? LeaveAvailed { get; set; }

        public int? status { get; set; }
        public string remarks { get; set; }
        public IEnumerable<YearlyLeaveProcess> yearlyLeaveProcesses { get; set; }
        public IEnumerable<Year> AllYears { get; set; }
    }
}
