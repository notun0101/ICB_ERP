using OPUSERP.Areas.Auth.Models;
using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.ProvidentFund.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OPUSERP.ERPServices.MasterData.Interfaces
{
    public interface IERPCompanyService
    {
        Task<int> SaveERPCompany(Company company);

        void UpdateCompanyLogoById(int compId, string fileName, string fileLocation);

        Task<IEnumerable<Company>> GetAllCompany();
		Task<NavbarViewModel> AssignPage(string userName);

		Task<Company> GetCompanyById(int id);

        Task<bool> DeleteCompanyById(int id);

        Task<Company> GetCompany();
        Task<IEnumerable<Department>> GetDepartment();
		Task<IEnumerable<EmployeeInfo>> GetEmployeesByAge(int age);
        Task<IEnumerable<EmployeeInfo>> GetEmployeesByRoleAndDesig(int? Role, int? Designation);
        Task<IEnumerable<EmployeeInfo>> GetEmployeesByRole(int? Role);
        Task<CustomRole> GetCustomRollById(int Id);
        Task<IEnumerable<PfAccountsSchedule>> GetAllPfAccountsByYear(int year);

    }
}
