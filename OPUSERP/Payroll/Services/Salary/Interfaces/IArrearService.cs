using OPUSERP.Areas.HRPMSAttendence.Models;
using OPUSERP.Areas.Payroll.Models;
using OPUSERP.Data.Entity.MasterData;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Wages;
using OPUSERP.Payroll.Data;
using OPUSERP.Payroll.Data.Entity.Arrear;
using OPUSERP.Payroll.Data.Entity.PF;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Services.Salary.Interfaces
{
	public interface IArrearService
    {
        Task<IEnumerable<EmployeeArrearInfo>> GetAllArrearInfo();
        Task<IEnumerable<SalaryPeriod>> GetSalaryPeriod();
        Task<int> SaveEmpArrearInfo(EmployeeArrearInfo employeeArrearInfo);
        Task<bool> DeleteArrerById(int id);
        Task<int> SaveEmployeesArrearStructure(EmployeesArrearStructure arrearDetails);
        Task<IEnumerable<EmployeesArrearStructure>> GetEmployeesArrearStructureByEmpId(int empId);
        Task<bool> DeleteEmployeesArrearStructureByempId(int empId);
    }
}
