using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
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
    public class ChildrenViewModel
    {
        public string childrenID { get; set; }
        public string employeeID { get; set; }
        public string childName { get; set; }
        public string childNameBN { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string education { get; set; }
        public string gender { get; set; }
        public string contact { get; set; }
        public string bin { get; set; }
        public int? occupation { get; set; }
        public int? occupationId { get; set; }
        public string designation { get; set; }
        public string organization { get; set; }
       // public string nid { get; set; }
        public string bloodGroup { get; set; }
        public string presentEducation { get; set; }
        public string nationality { get; set; }
        public string relationship { get; set; }
        public string emailAddressPersonal { get; set; }
        public string employeeNameCode { get; set; }
        public int disability { get; set; }
        public string disablityType { get; set; }
        public string nid { get; set; }
        public int? childNo { get; set; }
        public string maritalstatus { get; set; }

        public int[] childEduId { get; set; }
        public string[] institution { get; set; }

        public int[] degreeId { get; set; }
        public int[] levelOfEducationId { get; set; }

        public string[] majorSubject { get; set; }
        public string[] passingYear { get; set; }

        public string[] resultType { get; set; } //1st, 2nd, 3rd Class, Grade
        public string[] result { get; set; }


        public Photograph photograph { get; set; }
        public IFormFile childrenPhoto { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
        public ChildrenLn fLang { get; set; }

        public IEnumerable<Children> children { get; set; }
        public Children child { get; set; }
        public IEnumerable<Occupation> occupations { get; set; }
        public IEnumerable<Degree> degrees { get; set; }
        public IEnumerable<LevelofEducation> levelofEducations { get; set; }
        public LevelofEducation levelOfEducation { get; set; }
    }
}
