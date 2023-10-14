using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class EmployeeDeathViewModel
    {
        public int employeeInfoId { get; set; }
        //public EmployeeInfo employeeInfo { get; set; }
        public int deathId { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string Depeartment { get; set; }
        public string employeeNameCode { get; set; }
        public Photograph photograph { get; set; }
        public DateTime? date { get; set; }
        public string reason { get; set; } //1=Normal, 2=Accidental
        public IEnumerable<EmployeeDeath> employeeDeaths { get; set; }
    }
}
