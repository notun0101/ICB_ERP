using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Leave
{
    [Table("LeaveCommittee")]
    public class LeaveCommittee:Base
    {
        public int? employeeId { get; set; }
        public EmployeeInfo employee { get; set; }

        public int? leaveRegisterId { get; set; }
        public LeaveRegister leaveRegister { get; set; }

        public int? memberRoleId { get; set; }//সভাপতি=1,সদস্য=2,সদস্য সচিব=3
        public string empNameBn { get; set; }
        public string empNameEng { get; set; }

        public string designationBn { get; set; }
        public string designationEn { get; set; }

        public string departmentBn { get; set; }
        public string  departmentEn { get; set; }

        public int? CoYear { get; set; }
    }
}
