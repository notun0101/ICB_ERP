using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.MasterData.Interfaces
{
    public interface IEmployeeExtraCurricularService
    {
        Task<int> SaveEmployeeExtraCurricular(EmployeeExtraCurricular employeeExtraCurricular);
        Task<IEnumerable<EmployeeExtraCurricular>> GetEmployeeExtraCurricular();
        Task<EmployeeExtraCurricular> GetEmployeeExtraCurricularId(int id);
        Task<bool> DeleteEmployeeExtraCurricularId(int id);
        Task<IEnumerable<EmployeeExtraCurricular>> GetEmployeeExtraCurricularEmpId(int empId);
    }
}
