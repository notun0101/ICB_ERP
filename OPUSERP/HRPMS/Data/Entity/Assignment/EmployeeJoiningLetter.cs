using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Assignment
{
    [Table("EmployeeJoiningLetter")]
    public class EmployeeJoiningLetter : Base
    {
        public int? employeeReleaseInfoId { get; set; }
        public EmployeeReleaseInfo employeeReleaseInfo { get; set; }

        public int? joinedEmpId { get; set; }

        public string empName { get; set; }
        public string designation { get; set; }

        public int? joingDepartmentId { get; set; }
        public string joingDepartment { get; set; }
    
        public string remarks { get; set; }

        public DateTime? joiningDate { get; set; }
        public string joiningTime { get; set; }
    }
}
