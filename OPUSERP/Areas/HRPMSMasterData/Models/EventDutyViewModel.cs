using OPUSERP.Areas.HRPMSMasterData.Models.Lang;
using OPUSERP.Data.Entity.MasterData;
using OPUSERP.HRPMS.Data.Entity.Attendance;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Models
{
    public class EventDutyViewModel
    {
        public int AllocationId { get; set; }
        public int? dutyId { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public string location { get; set; }
        public int? employeeInfoId { get; set; }
        public string name { get; set; }
        public int? status { get; set; }
        public IEnumerable<EmployeeAllocation>  employeeAllocations { get; set; }
        public IEnumerable<SpecialEventDutyMaster> dutyMasters { get; set; }
        public IEnumerable<EmployeeInfo>  employeeInfos { get; set; }

    }
}
