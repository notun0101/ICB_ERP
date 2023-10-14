using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
    [Table("EmployeeSports", Schema = "HR")]
    public class EmployeeSports:Base
    {
        public int? employeeId { get; set; }
        public EmployeeInfo employee { get; set; }

        public string skillType { get; set; } // National, Reasonal Player
        public string skillLevel { get; set; } //Beginer, Intermediate, Advanced
    }


}
