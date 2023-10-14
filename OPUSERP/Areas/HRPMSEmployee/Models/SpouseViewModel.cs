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
    public class SpouseViewModel
    {
        public string spouseID { get; set; }
        public string employeeID { get; set; }
        [Required]
        [Display(Name = "Spouse Name")]
        public string spouseName { get; set; }

        public string emailAddress { get; set; }

        public string spouseNameBN { get; set; }


    
        public DateTime? dateOfBirth { get; set; }
        public DateTime? dateOfMarriage { get; set; }

        public int? occupationId { get; set; }

        public string gender { get; set; }

        public string maritalstatus { get; set; }

        public string designation { get; set; }

        public string organization { get; set; }

        public string bin { get; set; }

        public string nid { get; set; }

        public string bloodGroup { get; set; }

        public string homeDistrict { get; set; }
        public string relationship { get; set; }
        public string nationality { get; set; }


        [Display(Name ="Mobile Number")]        
        public string contact { get; set; }

        public string higherEducation { get; set; }

        public string employeeNameCode { get; set; }

        public Photograph photograph { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
        public IFormFile spousePhoto { get; set; }
        public IFormFile MarriagePhoto { get; set; }

        public SpouseLn fLang { get; set; }

        public Spouse spouse { get; set; }

        public IEnumerable<Spouse> spouses { get; set; }
        public IEnumerable<District> districts { get; set; }
        public IEnumerable<Occupation> occupations { get; set; }
    }
}
