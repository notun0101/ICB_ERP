using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OPUSERP.Areas.Auth.Models;
using OPUSERP.Data;
using OPUSERP.Data.Entity.Auth;
using OPUSERP.Data.Entity.Master;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.ProvidentFund.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.ERPServices.MasterData
{
    public class ERPCompanyService: IERPCompanyService
    {
        private readonly ERPDbContext _context;

        public ERPCompanyService(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveERPCompany(Company company)
        {
            if (company.Id != 0)
                _context.Companies.Update(company);
            else
                _context.Companies.Add(company);
             await _context.SaveChangesAsync();
            return company.Id;
        }

        public void UpdateCompanyLogoById(int compId, string fileName,string fileLocation)
        {
            var user = _context.Companies.Find(compId);
            user.fileName = fileName;
            user.filePath = fileLocation;
            _context.Entry(user).State = EntityState.Modified;
            
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Company>> GetAllCompany()
        {
           var result= await _context.Companies?.OrderBy(a => a.Id).Take(1).AsNoTracking().ToListAsync();
            //var result= await _context.Companies?.OrderBy(a => a.Id).AsNoTracking().ToListAsync();
            return result;
        }

		public async Task<NavbarViewModel> AssignPage(string userName)
		{
			var userId = _context.Users.Where(x => x.UserName == userName).Select(x => x.Id).FirstOrDefault();
			List<string> roleids = _context.UserRoles.Where(x => x.UserId == userId).Select(x => x.RoleId).ToList();
			List<int?> lstmodule = _context.UserAccessPages.Where(x => roleids.Contains(x.applicationRoleId)).Select(x => x.navbarId).ToList();

			List<int> lstparentId = _context.Navbars.Where(x => lstmodule.Contains(x.Id)).Select(x => x.parentID).ToList();
			List<int> lstparentIdF = _context.Navbars.Where(x => lstparentId.Contains(x.Id)).Select(x => x.parentID).ToList();

			IEnumerable<Navbar> navdata = await _context.Navbars.Include(x => x.module).Where(x => x.status == true).OrderBy(x => x.displayOrder).ToListAsync();

			var adminrole = _context.UserRoles.Where(x => x.UserId == userId && x.RoleId == "0583d54e-74a8-46a3-b880-e13698723f69").ToList();
			if (adminrole.Count() == 0)
			{
				navdata = navdata.Where(x => lstmodule.Contains(x.Id) || lstparentId.Contains(x.Id) || lstparentIdF.Contains(x.Id));
			}
			List<int?> modid = navdata.Select(x => x.moduleId).ToList();
			IEnumerable<ERPModule> modules = await _context.ERPModules.OrderBy(x => x.shortOrder).ToListAsync();
			if (adminrole.Count() == 0)
			{
				modules = modules.Where(x => modid.Contains(x.Id));
			}
			NavbarViewModel model = new NavbarViewModel
			{
				navbars = navdata,//await pageAssignService.GetNavbars(userName),
				ERPModules = modules,//await pageAssignService.GetERPModules(),
				userRoles = await _context.UserRoles.Where(x => x.UserId == userId).Select(x => x.RoleId).ToListAsync()
			};


			return model;
		}

		public async Task<IEnumerable<EmployeeInfo>> GetEmployeesByAge(int age)
		{
			var today = DateTime.Now;
			var data = await _context.employeeInfos.Include(x => x.lastDesignation).Include(x => x.department).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => (today - Convert.ToDateTime(x.dateOfBirth)).TotalDays <= age * 365 && x.lastDesignationId != null).ToListAsync();
			return data;
		}

        public async Task<IEnumerable<EmployeeInfo>> GetEmployeesByRoleAndDesig(int? Role , int? Designation)
		{
			
			var data = await _context.employeeInfos.Include(x => x.customRole).Include(x => x.lastDesignation).Include(x => x.department).Where(x => x.lastDesignation.customRoleId == Role && x.lastDesignationId == Designation).OrderBy(x => x.seniorityLevel).ToListAsync();
			return data;
		}
        public async Task<IEnumerable<EmployeeInfo>> GetEmployeesByRole(int? Role)
		{
			
			var data = await _context.employeeInfos.Include(x => x.customRole).Include(x => x.lastDesignation).Include(x => x.department).OrderBy(x => Convert.ToInt32(x.lastDesignation.designationCode)).ThenBy(x => Convert.ToInt32(x.seniorityLevel)).Where(x => x.lastDesignation.customRoleId == Role && x.lastDesignationId != null).ToListAsync();
			return data;
		}


		public async Task<Company> GetCompanyById(int Id)
        {
            return await _context.Companies.FindAsync(Id);
        }
        public async Task<CustomRole> GetCustomRollById(int Id)
        {
            return await _context.customRoles.FindAsync(Id);
        }

        public async Task<bool> DeleteCompanyById(int id)
        {
            _context.Companies.Remove(_context.Companies.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<Company> GetCompany()
        {
            return await _context.Companies.AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Department>> GetDepartment()
        {
            return await _context.departments.Include(x => x.hrDivision).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<PfAccountsSchedule>> GetAllPfAccountsByYear(int year)
        {
            var data = await _context.pfAccountsSchedules.Include(x => x.pfMember).Include(x => x.employee).Include(x => x.employee.lastDesignation).Where(x => x.year == year.ToString()).AsNoTracking().ToListAsync();
            return data;
        }

    }
}
