﻿using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
    [Table("Reference")]
    public class Reference:Base
    {
        public int employeeID { get; set; }
        public EmployeeInfo employee { get; set; }

        public string name { get; set; }
        public string designation { get; set; }
        public string organization { get; set; }
        public string contact { get; set; }
        public string email { get; set; }
    }
}
