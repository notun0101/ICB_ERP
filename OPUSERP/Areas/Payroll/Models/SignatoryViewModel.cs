using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Payroll.Data.Entity.Fixation;
using System.Collections.Generic;

namespace OPUSERP.Areas.Payroll.Models
{
    public class SignatoryViewModel
    {
        public int Id { get; set; }
        public int DesignationId { get; set; }
        public Designation Designation { get; set; }
        public int EmployeeId { get; set; }
        public string SignatoryName { get; set; }
        public string SignatoryDesignation { get; set; }
        public string SignatoryPhone { get; set; }
        public IEnumerable<Signatory> SignatoryList { get; set; }
        public IEnumerable<Designation> Designations { get; set; }
    }
}
