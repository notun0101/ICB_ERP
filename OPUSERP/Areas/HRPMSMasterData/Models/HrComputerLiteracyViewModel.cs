using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.HrComputerLiteracy;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
    public class HrComputerLiteracyViewModel
    {
        public string employeeID { get; set; }

        public string LiteracyId { get; set; }
        public string subject { get; set; }

        public string competencyLevel { get; set; } //Beginer, Intermediate, Advance
        public string training { get; set; }
        public string diploma { get; set; }

        public int? status { get; set; }
        public int? isActive { get; set; }
        public string remarks { get; set; }
        public Photograph photograph { get; set; }
        public EmployeeInfo employee { get; set; }

        public IEnumerable<HrComputerLiteracy> computerLiteracy { get; set; }
        public IEnumerable<HrComputerLiteracy> computerLiteracies { get; set; }

        public IEnumerable<ComputerSubject> ComputerSubjectsList { get; set; }
    }
}
