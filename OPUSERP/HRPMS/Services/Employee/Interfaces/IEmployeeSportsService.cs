using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Employee.Interfaces
{
    public interface IEmployeeSportsService
    {
        Task<bool> SaveEmployeeSports(EmployeeSports employeeSports);
        Task<IEnumerable<EmployeeSports>> GetEmployeeSports();
        Task<EmployeeSports> GetEmployeeSportsById(int id);
        Task<bool> DeleteEmployeeSportsById(int id);
        Task<IEnumerable<EmployeeSports>> GetEmployeeSportsByEmpId(int empId);
    }
}
