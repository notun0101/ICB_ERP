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
    public class EmployeeHobbyViewModel
    {
        public string EmployeeHobbyID { get; set; }
        public int employeeInfoId { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
        public Photograph photograph { get; set; }
        public string[] hobbyName { get; set; }
        public int? isActive { get; set; }
        public int? status { get; set; }
        public DateTime? date { get; set; }
        public IEnumerable<EmployeeHobby> employeeHobbies { get; set; }
    }
}
