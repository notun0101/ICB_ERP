using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.CRM.Data.Entity.MasterData;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class EmployeeSportsViewModel
    {
        public int EmployeeSportsId { get; set; }
        public int employeeID { get; set; }
        public string skillType { get; set; } // National, Reasonal Player
        public string skillLevel { get; set; } //Beginer, Intermediate, Advanced
        public Photograph photograph { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
        public IEnumerable<EmployeeSports> employeeSports { get; set; }
    }
}
