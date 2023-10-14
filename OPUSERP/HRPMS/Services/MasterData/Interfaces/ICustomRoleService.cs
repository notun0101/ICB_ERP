using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.MasterData.Interfaces
{
   public interface ICustomRoleService
    {
        Task<bool> SaveCustomRole(CustomRole customRole);
        Task<IEnumerable<CustomRole>> GetCustomRole();
        Task<CustomRole> GetCustomRoleById(int id);
        Task<bool> DeleteCustomRoleById(int id);
    }
}
