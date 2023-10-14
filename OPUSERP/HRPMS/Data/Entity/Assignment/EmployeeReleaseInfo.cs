using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Data.Entity.Assignment
{
    [Table("EmployeeReleaseInfo")]
    public class EmployeeReleaseInfo : Base
    {
        public int? employeeInfoId { get; set; }
        public EmployeeInfo employeeInfo { get; set; }

        public int? officialEmpId { get; set; }
        public string officialEmpName { get; set; }

        public string employeeName { get; set; }
        public string designation { get; set; }

        public int? fromDepartmentId { get; set; }
        public int? toDepartmentId { get; set; }

        public string fromDepartment { get; set; }
        public string toDepartment { get; set; }

        public string transferOrderNo { get; set; }
        public string releaseOrderNo { get; set; }
        public string transferToOfficial { get; set; }// Transfer to Employee
        public string remarks { get; set; }

        public DateTime? transferOrderDate { get; set; }
        public DateTime? releaseOrderDate { get; set; }
    }
}
