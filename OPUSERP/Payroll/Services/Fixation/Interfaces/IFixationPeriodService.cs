using OPUSERP.Payroll.Data.Entity.Fixation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Services.Fixation.Interfaces
{
    public interface IFixationPeriodService
    {
        Task<int> SaveFixationPeriod(FixationPeriod fixationPeriod);
       
        Task<IEnumerable<FixationPeriod>> GetAllFixationPeriod();

        Task<int> DeleteFixationPeriod(int id);
        Task<int> IsDuplicateFixationPeriod(FixationPeriod fixationPeriod);
    }
}
