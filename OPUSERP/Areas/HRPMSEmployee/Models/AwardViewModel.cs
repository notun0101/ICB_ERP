using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.HRPMS.Data.Entity.Employee;
using Microsoft.AspNetCore.Http;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class AwardViewModel
    {
        public string employeeID { get; set; }

        public string awardId { get; set; }

        //[Required]
        //[Display(Name = "Award Name")]
        //public string awardName { get; set; }
        public int? awardd { get; set; }
        public IEnumerable<OPUSERP.HRPMS.Data.Entity.Master.Award> awardlist { get; set; }

        [Display(Name = "Perpose")]
        public string perpose { get; set; }

        [Display(Name = "Date")]
        public DateTime? txtAwardDate { get; set; }

        public string action { get; set; }

        public string employeeNameCode { get; set; }
        public IFormFile awardPhoto { get; set; }

        public Photograph photograph { get; set; }
        public EmployeeInfo employeeInfo { get; set; }

        public Award fLang { get; set; }

        public IEnumerable<EmployeeAward> awards { get; set; }
       
    }
}
