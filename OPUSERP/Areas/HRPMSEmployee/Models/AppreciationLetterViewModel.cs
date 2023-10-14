using Microsoft.AspNetCore.Http;
using OPUSERP.Areas.HRPMSRetirementAndTermination.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.RetirementAndTermination;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class AppreciationLetterViewModel
    {
        public int? employeeId { get; set; }
        public EmployeeInfo employee { get; set; }
        public int appreciationId { get; set; }
        public string letterNo { get; set; }
        public DateTime? date { get; set; }
        public IFormFile imageUrl { get; set; }
        public int? status { get; set; }
        public string remarks { get; set; }

        public AppreciationLetter appreciationLetter { get; set; }
        public IEnumerable<AppreciationLetter> appreciationLetters { get; set; }

    }
}
