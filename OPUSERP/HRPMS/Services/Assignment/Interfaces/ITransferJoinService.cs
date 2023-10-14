using OPUSERP.HRPMS.Data.Entity.Assignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Assignment.Interfaces
{
    public interface ITransferJoinService
    {
        Task<int> SaveEmployeeRelease(EmployeeReleaseInfo employeeReleaseInfo);
        Task<EmployeeReleaseInfo> GetEmployeeReleaseInfoById(int id);
        Task<bool> DeleteEmployeeReleaseById(int id);
        Task<IEnumerable<EmployeeReleaseInfo>> GetAllReleasedEmployee(int? departmentId);
        //Task<EmployeeReleaseInfo> GetAllReleasedEmployee(int? departmentId);

        Task<int> SaveEmployeeJoining(EmployeeJoiningLetter employeeJoiningLetter);
        Task<IEnumerable<EmployeeJoiningLetter>> GetAllJoiningEmployee(int? departmentId);

    }
}
