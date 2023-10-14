using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace OPUSERP.ERPServices.MasterData.Interfaces
{
    public interface IHandoverTakeoverMasterService
    {
        Task<bool> SaveHTMaster(HandoverTakeoverMaster handoverTakeoverMaster);
        Task<IEnumerable<HandoverTakeoverMaster>> GetAllHTMaster();
        Task<HandoverTakeoverMaster> GetHTMasterById(int id);
        Task<bool> DeleteHTMasterById(int id);


        Task<bool> SaveHTDetails(HandoverTakeoverDetails handoverTakeoverDetails);
        Task<IEnumerable<HandoverTakeoverDetails>> GetAllHTDetails();
        Task<HandoverTakeoverDetails> GetHTDetailsById(int id);
        Task<bool> DeleteHTDetailsById(int id);
        Task<IEnumerable<HandoverTakeoverDetails>> GetDetailsBytakeoverId(int takeoverId);
        Task<string> GetDetailsImgUrlById(int id);


    }
}
