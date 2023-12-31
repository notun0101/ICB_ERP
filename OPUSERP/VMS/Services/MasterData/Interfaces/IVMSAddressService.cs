﻿
using System.Collections.Generic;
using System.Threading.Tasks;
using OPUSERP.Data.Entity.Master;
using OPUSERP.VMS.Data.Entity.FuelStation;
using OPUSERP.VMS.Data.Entity.Master;

namespace OPUSERP.VMS.Services.MasterData.Interfaces
{
    public interface IVMSAddressService
    {
        Task<bool> SaveCountry(Country country);
        Task<IEnumerable<Country>> GetAllContry();
        Task<Country> GetContryById(int id);
        Task<bool> DeleteContryById(int id);

        Task<bool> SaveDivision(Division division);
        Task<IEnumerable<Division>> GetAllDivision();
        Task<Division> GetDivisionById(int id);
        Task<bool> DeleteDivisionById(int id);
        Task<IEnumerable<Division>> GetDivisionsByCountryId(int CntId);

        Task<bool> SaveDistrict(District district);
        Task<IEnumerable<District>> GetAllDistrict();
        Task<District> GetDistrictById(int id);
        Task<bool> DeleteDistrictById(int id);
        Task<IEnumerable<District>> GetDistrictsByDivisonId(int DivisionId);

        Task<bool> SaveThana(Thana thana);
        Task<IEnumerable<Thana>> GetAllThana();
        Task<Thana> GetThanaById(int id);
        Task<bool> DeleteThanaById(int id);
        Task<IEnumerable<Thana>> GetThanasByDistrictId(int DistrictId);
        #region Address
        Task<int> SaveAddress(VMSAddress address);
        Task<IEnumerable<VMSAddress>> GetAddress();
        Task<VMSAddress> GetAddressById(int id);
        Task<VMSAddress> GetAddressBystationId(int id);
        Task<bool> DeleteAddressById(int id);
        #endregion

    }
}
