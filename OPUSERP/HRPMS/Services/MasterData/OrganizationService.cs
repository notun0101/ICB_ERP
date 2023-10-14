using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using Microsoft.EntityFrameworkCore;
using OPUSERP.Data.Entity.Master;
using OPUSERP.Areas.HRPMSReport.Models;
using DocumentFormat.OpenXml.Drawing.Charts;
using System.Reflection.Emit;
using System.Security.Policy;

namespace OPUSERP.HRPMS.Services.MasterData
{
    public class OrganizationService : IOrganizationService
    {
        private readonly ERPDbContext _context;

        public OrganizationService(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveOrganization(Organization organization)
        {
            if(organization.Id != 0)
                _context.organizations.Update(organization);
            else
                _context.organizations.Add(organization);
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Organization>> GetAllOrganization()
        {
            return await _context.organizations.OrderBy(x => x.organizationName).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Religion>> GetAllReligions()
        {
            return await _context.religions.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<EmployeeInfoApiViewModel>> GetAllEmployeesByTypeId(string type, int id)
        {
            var data = await _context.employeeInfoApiViews.FromSql($"sp_getAllEmployeeByTypeAndId @p0, @p1", type, id).ToListAsync();
            return data;
        }
        public async Task<IEnumerable<EmployeeInfoApiViewModel>> GetAllEmployeesByBranchAndDesignationId(int? branchId, int? departmentId, int? designationid)
        {
            var data = await _context.employeeInfoApiViews.FromSql($"sp_getAllEmployeeByBranchAndDesignation @p0, @p1, @p2", branchId, departmentId, designationid).ToListAsync();
			return data;
		}
        public async Task<IEnumerable<EmployeeInfoApiViewModel>> GetEmployeesByAnyKey(int? divisionId, int? unitId, int? officeId, int? zoneId, int? branchId, int? departmentId, int? designationId)
		{//unitId	int, officeId int,
						var data = await _context.employeeInfoApiViews.FromSql($"SP_GET_EMPLOYEES_BY_ANY_KEY {divisionId}, {unitId}, {officeId}, {zoneId}, {branchId}, {departmentId}, {designationId}").AsNoTracking().ToListAsync();
            return data;
        }
        public async Task<IEnumerable<Division>> GetAllDivisions()
        {
            return await _context.Divisions.AsNoTracking().ToListAsync();
        }
       public async Task<IEnumerable<District>> GetAllDistrict()
        {
            return await _context.Districts.AsNoTracking().ToListAsync();
        }
       public async Task<IEnumerable<Location>> GetAllBranch()
        {
            return await _context.Locations.AsNoTracking().ToListAsync();
        }
      public async Task<IEnumerable<HrBranch>> GetAllHrBranch()
        {
            return await _context.hrBranches.AsNoTracking().ToListAsync();
        }
      
         public async Task<IEnumerable<HrDivision>> GetAllHrDivision()
        {
            return await _context.hrDivisions.AsNoTracking().ToListAsync();
        }
      
         public async Task<IEnumerable<Department>> GetAllDepartments()
        {
            return await _context.departments.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<FunctionInfo>> GetAllOffices()
        {
            return await _context.FunctionInfos.AsNoTracking().ToListAsync();
        }



        public async Task<Organization> GetOrganizationById(int id)
        {
            return await _context.organizations.FindAsync(id);
        }

        public async Task<bool> DeleteOrganizationById(int id)
        {
            _context.organizations.Remove(_context.organizations.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

       
    }
}
