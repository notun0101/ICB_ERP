using Microsoft.AspNetCore.Http;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Suspensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSDisciplineInvestigation.Models
{
    public class SuspensionViewModel
    {
        public string suspensionID { get; set; }
        public string susDesc { get; set; }
        //public string susFile { get; set; }
        public string chargeSheetDesc { get; set; }
        //public string chargeSheetFile { get; set; }
        public string hearingReport { get; set; }
        //public string hearingReportFile { get; set; }
        public string punishmentType { get; set; }
        public DateTime? effectiveForm { get; set; }

        public int? isActive { get; set; }
        public int? status { get; set; }
        public IFormFile suspensionFile { get; set; }
        public IFormFile chargeFile { get; set; }
        public IFormFile hearingRepoFile { get; set; }
       
        public IEnumerable<Suspension> supensionDetails { get; set; }
        public Suspension supensionDetail { get; set; }
        public Suspension suspensionInfo { get; set; }

        public Photograph photograph { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
        public int? employeeId { get; set; }
        public string employeeNameCode { get; set; }


    }
}
