using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Data.Entity.MasterData;
using OPUSERP.Data.Entity.Training;
using OPUSERP.HRPMS.Data.Entity.Attendance;
using OPUSERP.HRPMS.Data.Entity.Master;

namespace OPUSERP.HRPMS.Services.MasterData.Interfaces
{
    public interface IEventDutyService
    {
        Task<bool> SaveEmployeeAllocation(EmployeeAllocation employeeAllocation);
        Task<IEnumerable<EmployeeAllocation>> GetEmployeeAllocation();
        Task<EmployeeAllocation> GetEmployeeAllocationById(int id);
        Task<bool> DeleteEmployeeAllocationById(int id);


        Task<bool> Saveduty(SpecialEventDutyMaster duty);
        Task<IEnumerable<SpecialEventDutyMaster>> Getduty();
        Task<SpecialEventDutyMaster> GetdutyById(int id);
        Task<bool> DeletedutyById(int id);

        

    }
}
