using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.MasterData.Interfaces
{
    public interface ICvBlackListService
    {
        Task<bool> SaveCvBlackList(CvBlackList cvBlackList);
        Task<IEnumerable<CvBlackList>> GetCvBlackList();
        Task<CvBlackList> GetCvBlackListById(int id);
        Task<CvBlackList> GetCvBlack();
        Task<bool> DeleteCvBlackListById(int id);
    }
}
