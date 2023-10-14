using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
    [Table("EmployeeJobDescription", Schema = "HR")]
    public class EmployeeJobDescription
    {
        public int employeeId { get; set; }
        public EmployeeInfo employee { get; set; }
        public string title { get; set; }
        public string atttachment { get; set; }

        [MaxLength(20)]
        public string status { get; set; }
     
        public DateTime? effectiveDate { get; set; }

        public DateTime? endDate { get; set; }

    }
}
