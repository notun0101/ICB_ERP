using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Leave;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.ACR
{
    [Table("LeaveDetail")]
    public class LeaveDetail:Base
    {
        public int? assessmentId { get; set; }
        public Assessment assessment { get; set; }
        public int? leaveTypeId { get; set; }
        public LeaveType leaveType { get; set; }
        public int? totalLeave { get; set; }
        public int? consumptionLeave { get; set; }
        public int? leaveBalance { get; set; }
    }
}
