using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.RetirementAndTermination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSRetirementAndTermination.Models
{
    public class ShowAllResignationViewModel
    {
        public IEnumerable<ResignInformation> resignInformationList { get; set; }
        public IEnumerable<EmployeeInfo> employeeInfoList { get; set; }
        public IEnumerable<Designation> designationList { get; set; }
    }
}
