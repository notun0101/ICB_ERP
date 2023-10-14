using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class PensionViewModel
    {
        public int EmployeeId { get; set; }
        public string nameEnglish { get; set; }
        public int? activityStatus { get; set; }
        public DateTime? prlDate { get; set; }
        public IEnumerable<EmployeeInfo> PRLInfos { get; set; }
        public IEnumerable<EmployeeInfo> PensionInfos { get; set; }
        public string PRLreportDateBn { get; set; }
       
        public string banglaDate { get; set; }
        public string banglaMonth { get; set; }
        public string banglaYear { get; set; }
    
        public DateTime? PRLreportDateEn { get; set; }

    }
}
