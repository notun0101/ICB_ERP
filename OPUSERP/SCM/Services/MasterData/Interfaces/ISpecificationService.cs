using OPUSERP.SCM.Data.Entity.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.SCM.Services.MasterData.Interfaces
{
    public  interface ISpecificationService
    {
        Task<IEnumerable<ItemSpecification>> GetSpecifications();
        Task<ItemSpecification> GetSpecificationsById(int id);
        Task<ItemSpecification> SaveSpecification(ItemSpecification specification);
    }
}
