using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Employee;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.HRPMS.Data.Entity.Master;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class TaxViewModel
    {
        public string taxID { get; set; }
        public string employeeID { get; set; }
        public string taxZone { get; set; }
        public string taxCircle { get; set; }
        public string employeeNameCode { get; set; }
        public Photograph photograph { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
        public Tax tax { get; set; }
        public IEnumerable<Tax> taxes  { get; set; }
      
        
    }
}
