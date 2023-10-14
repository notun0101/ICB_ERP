using Microsoft.AspNetCore.Http;
using OPUSERP.Areas.HRPMSRetirementAndTermination.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.RetirementAndTermination;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSRetirementAndTermination.Models
{
    public class PRLApplicationViewModel
    {
        [Required]
        [Display(Name ="Employee ID")]
        public int? employeeId { get; set; }
        public string applicationName { get; set; }
        public string applicationDate { get; set; }
        public string type { get; set; }
        public string fileTitle { get; set; }
        public string uploadFile { get; set; }
        public IFormFile resignPhoto { get; set; }

        public string resignId { get; set; }
        public DateTime? resignDate { get; set; }
        public DateTime? lastWorkingDate { get; set; }
        public string resignReason { get; set; }
        public int nextApprover { get; set; } //userid
        
        public IEnumerable<PRLApplication> pRLApplications { get; set; }
        public IEnumerable<ResignInformation> resignInformation { get; set; }
        public ResignInformation resignInformations { get; set; }
        public PRLApplication pRLApplicationsById { get; set; }
        public EmployeeInfo  employeeInfo { get; set; }
        public PRLApplicationLn flang { get; set; }


        public string bodyText { get; set; }

        public string letterNo { get; set; }
        public IFormFile clearanceFile { get; set; }
        public DateTime? clearanceDate { get; set; }
    }
}
