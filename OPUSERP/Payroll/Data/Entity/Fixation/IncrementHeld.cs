using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Data.Entity.Fixation
{
    [Table("IncrementHeld")]
    public class IncrementHeld:Base
    {
        public int? employeeId { get; set; }
        public EmployeeInfo employee { get; set; }

        public string empCode { get; set; }
        public string empName { get; set; }
        public string reason { get; set; }
        public DateTime? expireDate { get; set; }

        public string orderNo { get; set; }
    }
}
