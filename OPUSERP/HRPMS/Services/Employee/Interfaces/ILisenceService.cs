using OPUSERP.Data.Entity.MasterData;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Employee.Interfaces
{
    public interface ILisenceService
    {
        Task<int> SaveLisence(Lisence  lisence);
        Task<IEnumerable<Lisence>> GetLisence();
        Task<Lisence> GetLisenceById(int id);
        Task<bool> DeleteLisenceById(int id);
        Task<Lisence> GetLisence1();
      //  Task<IEnumerable<Lisence>> GetLisenceByEmpId(int empId);


        Task<int> SaveLisenceType(LisenceType  lisenceType);
        Task<IEnumerable<LisenceType>> GetLisenceType();
        Task<bool> DeleteLisenceTypeById(int id);
      
    }
}
