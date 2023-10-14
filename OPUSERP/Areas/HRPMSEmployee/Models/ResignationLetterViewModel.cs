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
using OPUSERP.HRPMS.Data.Entity.RetirementAndTermination;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class ResignationLetterViewModel
    {
        public int ResignationLetterID { get; set; }
        public string employeeID { get; set; }
       // public int employeeId { get; set; }
        public DateTime? submissionDate { get; set; }
        public DateTime? lastworkingDate { get; set; }
        public string reason { get; set; }
        public string totalWorkingDays { get; set; }
        public string fileUrl { get; set; }
        public IFormFile imageUrl { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
        public IEnumerable<ResignationLetter> resignationLetters { get; set; }


        public ResignInformation resignInformations { get; set; }
    }
}
