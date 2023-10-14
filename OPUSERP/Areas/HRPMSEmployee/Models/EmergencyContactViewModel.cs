using Microsoft.AspNetCore.Http;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class EmergencyContactViewModel
    {
        public int? employeeID { get; set; }
        public int? refID { get; set; }
        public string refName { get; set; }
        public string refRelation { get; set; }
        public string refOrganization { get; set; }
        public string refDesignation { get; set; }
        public string refEmail { get; set; }
        public string refContact { get; set; }
        public string Occupation { get; set; }

        public string employeeNameCode { get; set; }
        [Display(Name = "Employee Photo")]
        public IFormFile empPhoto { get; set; }

        public Photograph photograph { get; set; }
        public EmployeeInfo employeeInfo { get; set; }

        public string OfficeAddress { get; set; }
        public string HomeAddress { get; set; }

        public IEnumerable<EmergencyContact> emergencyContacts { get; set; }
        public IEnumerable<WagesEmergencyContact> wagesEmergencyContacts { get; set; }
        public IEnumerable<Occupation> occupations { get; set; }
        public IEnumerable<Relation> relations { get; set; }
    }
}
