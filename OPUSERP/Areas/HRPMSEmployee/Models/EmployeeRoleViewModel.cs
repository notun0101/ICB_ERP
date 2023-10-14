using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class EmployeeRoleViewModel
    {
        public int emproleid { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public int employeeID { get; set; }

        public IEnumerable<EmployeeRole> employeeRoles { get; set; }
        public int? roleId { get; set; }



        public int? status { get; set; }
        public Photograph photograph { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
        public string employeeNameCode { get; set; }
        public IEnumerable<CustomRole> customRoles { get; set; }
        public EmployeeRole employeeRole { get; set; }


    }
}
