using OPUSERP.HRPMS.Data.Entity.Assignment;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSAssignment.Models
{
    public class EmployeeJoiningViewModel
    {
        public int? releaseInfoId { get; set; }
        public int? joinedEmpId { get; set; }
        public int? joiningDepartmentId { get; set; }
        public string empName { get; set; }
        public string designation { get; set; }
        public string joingDepartment { get; set; }
        public string joiningTime { get; set; }
       
        public DateTime? joiningDate { get; set; }
        public DateTime? releaseOrderDate { get; set; }

        public IEnumerable<Department> departments { get; set; }
        public IEnumerable<EmployeeJoiningLetter> employeeJoiningLetters { get; set; }
        public IEnumerable<EmployeeReleaseInfo> employeeReleaseInfos { get; set; }
    }
}
