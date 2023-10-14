using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSACR.Models
{
    public class EmployeesJobDurationViewModel
    {
        public int? assessmentId { get; set; }
        public int? statusInfoId { get; set; }
        public string empCode { get; set; }
        public string JobDuration { get; set; }
        public string JobDurationAsBranchManger { get; set; }
        public string JobDurationAsOthers { get; set; }
        public string JobDurationAsZonalHead { get; set; }
        public string yearName { get; set; }      
        public string empType { get; set; }
    }
}
