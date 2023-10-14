using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.HRPMS.Data.Entity.Employee;

namespace OPUSERP.HRPMS.Services.Employee.Interfaces
{
    public interface IMobileBenifitService
    {
        
        Task<int> SaveMobileBenifit(MobileBenifit mobileBenifit);
        Task<IEnumerable<MobileBenifit>> GetMobileBenifit();
        Task<MobileBenifit> GetMobileBenifitById(int id);
        Task<bool> DeleteMobileBenifitById(int id);
        Task<IEnumerable<MobileBenifit>> GetMobileBenifitByEmpId(int empId);


    }
}
