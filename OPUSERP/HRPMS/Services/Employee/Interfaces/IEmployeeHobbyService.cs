using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.HRPMS.Data.Entity.Employee;

namespace OPUSERP.HRPMS.Services.Employee.Interfaces
{
    public interface IEmployeeHobbyService
    {
        
        Task<bool> SaveEmployeeHobby(EmployeeHobby employeeHobby);
        Task<IEnumerable<EmployeeHobby>> GetEmployeeHobby();
        Task<EmployeeHobby> GetEmployeeHobbyById(int id);
        Task<bool> DeleteEmployeeHobbyById(int id);
        Task<IEnumerable<EmployeeHobby>> GetEmployeeHobbyByEmpId(int empId);

        
    }
}
