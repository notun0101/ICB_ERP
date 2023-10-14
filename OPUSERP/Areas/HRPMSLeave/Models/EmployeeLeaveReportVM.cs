using OPUSERP.Data.Entity.Master;
using System.Collections.Generic;

namespace OPUSERP.Areas.HRPMSLeave.Models
{
    public class EmployeeLeaveReportVM
    {
        public IEnumerable<Company> companies { get; set; }
        public IEnumerable<EmployeeLeaveViewModel> EmployeeLeaveViewModels { get; set; }
    }
}
