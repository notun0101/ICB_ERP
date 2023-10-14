using OPUSERP.HRPMS.Data.Entity.Suspensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.SuspensionReport.Interfaces
{
    public interface ISuspension
    {
        Task<int> SaveSuspension(Suspension suspension);
        Task<IEnumerable<Suspension>> GetAllSuspension();
        Task<bool> DeleteSuspensionById(int id);
        Task<Suspension> GetSuspensionById(int id);
        Task<string> GetsuspensionImgUrlById(int id);
        Task<string> GetChargeImgUrlById(int id);
        Task<string> GetHearingImgUrlById(int id);
        Task<IEnumerable<Suspension>> GetSuspensionsByEmployeeId(int id);
        Task<Suspension> GetSuspensionsByEmpId(int id);
    }
}
