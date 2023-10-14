using OPUSERP.HRPMS.Data.Entity.HrBudget;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.HrBudget.Interface
{
	public interface IClientServeLost
    {
        Task<bool> SaveClientServeLost(ClientServeLost clientServeLost);
        Task<IEnumerable<ClientServeLost>> GetClientServeLost();
        Task<ClientServeLost> GetClientServeLostById(int id);
        Task<bool> DeleteClientServeLostById(int id);
    }
}
