using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Areas.HRPMSReport.Models;
using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Master;

namespace OPUSERP.HRPMS.Services.MasterData.Interfaces
{
    public interface IOrganizationService
    {
        Task<bool> SaveOrganization(Organization orga);
        Task<IEnumerable<Organization>> GetAllOrganization();
        Task<Organization> GetOrganizationById(int id);
        Task<bool> DeleteOrganizationById(int id);
        Task<IEnumerable<Religion>> GetAllReligions();
        Task<IEnumerable<Division>> GetAllDivisions();
        Task<IEnumerable<District>> GetAllDistrict();
        Task<IEnumerable<Location>> GetAllBranch();
        Task<IEnumerable<HrBranch>> GetAllHrBranch();
        Task<IEnumerable<HrDivision>> GetAllHrDivision();
        Task<IEnumerable<Department>> GetAllDepartments();
        Task<IEnumerable<FunctionInfo>> GetAllOffices();
        Task<IEnumerable<EmployeeInfoApiViewModel>> GetAllEmployeesByBranchAndDesignationId(int? branchId, int? departmentId, int? designationid);
        Task<IEnumerable<EmployeeInfoApiViewModel>> GetEmployeesByAnyKey(int? divisionId, int? unitId, int? officeId, int? zoneId, int? branchId, int? departmentId, int? designationId);

		//Task<IEnumerable<EmployeeInfoApiViewModel>> GetEmployeesByAnyKey(int? divisionId, int? zoneId, int? branchId, int? departmentId, int? designationId);

		Task<IEnumerable<EmployeeInfoApiViewModel>> GetAllEmployeesByTypeId(string type, int id);

    }
}
