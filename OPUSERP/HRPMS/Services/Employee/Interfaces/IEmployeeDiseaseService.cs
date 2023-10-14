using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Employee.Interfaces
{
    public interface IEmployeeDiseaseService
    {
        Task<bool> SaveEmployeeDisease(EmployeeDisease employeeDisease);
        Task<IEnumerable<EmployeeDisease>> GetEmployeeDisease();
        Task<EmployeeDisease> GetEmployeeDiseaseById(int id);
        Task<bool> DeleteEmployeeDiseaseById(int id);
        Task<IEnumerable<EmployeeDisease>> GetEmployeeDiseaseByEmpId(int empId);
        Task<int> IsThisNamePresent(int MDiseaseId);
    }
}
