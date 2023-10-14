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
    public class AdditionalInformationViewModel
    {
      
        public string employeeID { get; set; }
        [Required]
        [Display(Name = "Favourite Color")]
        public string favouriteColor { get; set; }
        public string employeeCode { get; set; }
        public string nameEnglish { get; set; }
        public string employeeNameCode { get; set; }
        public string visualEmpCodeName { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
        public Photograph photograph { get; set; }


    }
}
