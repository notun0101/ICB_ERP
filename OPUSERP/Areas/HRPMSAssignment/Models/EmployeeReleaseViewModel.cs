using OPUSERP.HRPMS.Data.Entity.Assignment;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSAssignment.Models
{
    public class EmployeeReleaseViewModel
    {
        public int? employeeInfoId { get; set; }
        public string employeeName { get; set; }
        public string designation { get; set; }
        public string department { get; set; }
        public int? departmentId { get; set; }
        public int? toDepartmentId { get; set; }
        public int? officialEmpId { get; set; }
        public string transferOrderNo { get; set; }
        public string releaseOrderNo { get; set; }
        public DateTime? trOrderDate { get; set; }
        public DateTime? releaseOrderDate { get; set; }

        public IEnumerable<Department> departments { get; set; }
        public IEnumerable<EmployeeReleaseInfo> employeeReleaseInfos { get; set; }
    }
}
