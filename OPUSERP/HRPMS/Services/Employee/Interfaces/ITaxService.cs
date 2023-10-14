using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.HRPMS.Data.Entity.Employee;

namespace OPUSERP.HRPMS.Services.Employee.Interfaces
{
    public interface ITaxService
    {
        
        Task<int> SaveTax(Tax tax);
        Task<IEnumerable<Tax>> GetTax();
        Task<Tax> GetTaxById(int id);
        Task<bool> DeleteTaxById(int id);
        Task<int> DeletePostingById(int id);
        Task<IEnumerable<Tax>> GetTaxByEmpId(int empId);

        Task<int> SaveDuelResidence(DuelResidence  duelResidence);
        Task<IEnumerable<DuelResidence>> GetDuelResidence();
        Task<DuelResidence> GetDuelResidenceById(int id);
        Task<bool> DeleteDuelResidenceById(int id);
        Task<IEnumerable<DuelResidence>> GetDuelResidenceByEmpId(int empId);

        Task<IEnumerable<EmployeePostingPlace>> GetEmpPostingByEmpId(int empId);
        Task<int> SavePosting(EmployeePostingPlace employeePosting);
        Task<EmployeePostingPlace> GetEmpPostingById(int id);



    }
}
