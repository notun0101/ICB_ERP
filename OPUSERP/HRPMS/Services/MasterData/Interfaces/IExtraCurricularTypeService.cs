using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.MasterData.Interfaces
{
   public interface IExtraCurricularTypeService
    {
        Task<bool> SaveExtraCurricularType(ExtraCurricularType extraCurricularType);
        Task<IEnumerable<ExtraCurricularType>> GetExtraCurricularType();
        Task<ExtraCurricularType> GetExtraCurricularTypeId(int id);
        Task<bool> DeleteExtraCurricularTypeId(int id);
    }
}
