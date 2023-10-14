using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using Microsoft.EntityFrameworkCore;
using OPUSERP.Data.Entity.MasterData;
using OPUSERP.Data.Entity.Master;
using OPUSERP.Areas.HRPMSMasterData.Models;

namespace OPUSERP.HRPMS.Services.MasterData
{
    public class DesignationDepartmentService : IDesignationDepartmentService
    {
        private readonly ERPDbContext _context;

        public DesignationDepartmentService(ERPDbContext context)
        {
            _context = context;
        }

        //Designation
        public async Task<bool> DeleteDesignationById(int id)
        {
            _context.designations.Remove(_context.designations.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Designation>> GetDesignations()
        {
            return await _context.designations.Include(x => x.customRole).OrderBy(x => x.designationName).AsNoTracking().ToListAsync();
        }

        public async Task<Designation> GetDesignationById(int id)
        {
            return await _context.designations.FindAsync(id);
        }

        public async Task<Designation> GetDesignationIdByName(string name)
        {
            return await _context.designations.Where(x => x.designationName == name).FirstOrDefaultAsync();
        }
        public async Task<Department> GetDepartmentIdByName(string name)
        {
            return await _context.departments.Where(x => x.deptName == name).FirstOrDefaultAsync();
        }
        public async Task<Division> GetDivisionIdByName(string name)
        {
            return await _context.Divisions.Where(x => x.divisionName == name).FirstOrDefaultAsync();
        }


        public async Task<bool> SaveDesignation(Designation designation)
        {
            if (designation.Id != 0)
                _context.designations.Update(designation);
            else
                _context.designations.Add(designation);

            return 1 == await _context.SaveChangesAsync();
        }

        //Department
        public async Task<bool> DeleteDepartmentById(int id)
        {
            _context.departments.Remove(_context.departments.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<bool> DeleteDivisionById(int id)
        {
            _context.Divisions.Remove(_context.Divisions.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<bool> DeleteBranchById(int id)
        {
            _context.hrBranches.Remove(_context.hrBranches.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<bool> DeleteJDTypeById(int id)
        {
            _context.jDTypes.Remove(_context.jDTypes.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Department>> GetDepartment()
        {
            return await _context.departments.Include(x => x.hrDivision).OrderBy(x => x.deptName).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Division>> GetDivision()
        {
            return await _context.Divisions.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<HrBranch>> GetBranch()
        {
            return await _context.hrBranches.Include(x => x.office).Include(x => x.branchType).Include(x => x.location).OrderBy(x => x.branchName).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<BranchWithManagerViewModel>> GetAllBranchWithManagers()
        {
            var data = await _context.branchWithManagers.FromSql($"sp_GetBranchesWithManager").ToListAsync();

            return data;
        }
        public async Task<IEnumerable<Location>> GetAllZones()
        {
            return await _context.Locations.Include(x => x.company).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<JDType>> GetJDType()
        {
            return await _context.jDTypes.Include(x => x.designation).Include(x =>x.division).Include(x=> x.department).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<HrBranchType>> GetAllBranchType()
        {
            return await _context.hrBranchTypes.AsNoTracking().ToListAsync();
        }

        public async Task<Department> GetDepartmentById(int id)
        {
            return await _context.departments.FindAsync(id);
        }
        public async Task<Division> GetDivisionById(int id)
        {
            return await _context.Divisions.FindAsync(id);
        }
         public async Task<HrDivision> GetHrDivisionById(int id)
        {
            return await _context.hrDivisions.FindAsync(id);
        }

        public async Task<JDType> GetJDTypeById(int id)
        {
            return await _context.jDTypes.FindAsync(id);
        }


        public async Task<bool> SaveDepartment(Department department)
        {
            if(department.Id != 0)
                _context.departments.Update(department);
            else
                _context.departments.Add(department);
            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<bool> SaveDivision(Division division)
        {
            if (division.Id != 0)
                _context.Divisions.Update(division);
            else
                _context.Divisions.Add(division);
            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<bool> SaveJDType(JDType jDType)
        {
            if (jDType.Id != 0)
                _context.jDTypes.Update(jDType);
            else
                _context.jDTypes.Add(jDType);
            return 1 == await _context.SaveChangesAsync();


        }
        public async Task<bool> SaveBranch(HrBranch hrBranch)
        {
            if(hrBranch.Id != 0)
                _context.hrBranches.Update(hrBranch);
            else
                _context.hrBranches.Add(hrBranch);
            return 1 == await _context.SaveChangesAsync();

        
        }

        //subBranch
        public async Task<bool> SaveSubBranch(HrSubBranch hrSubBranch)
        {
            if(hrSubBranch.Id != 0)
                _context.hrSubBranches.Update(hrSubBranch);
            else
                _context.hrSubBranches.Add(hrSubBranch);
            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<HrSubBranch>> GetSubBranch()
        {
            return await _context.hrSubBranches.Include(x => x.hrBranch).Include(x => x.subbranchType).AsNoTracking().ToListAsync();
        }
        public async Task<bool> DeleteSubBranchById(int id)
        {
            _context.hrSubBranches.Remove(_context.hrSubBranches.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
        
        
        //JDTaskType
        public async Task<bool> SaveJDTaskType(JDTaskType jDTaskType)
        {
            if (jDTaskType.Id != 0)
                _context.jDTaskTypes.Update(jDTaskType);
            else
                _context.jDTaskTypes.Add(jDTaskType);
            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<JDTaskType>> GetJDTaskTypes(int id)
        {
            return await _context.jDTaskTypes.Include(x => x.jdType).Include(x => x.jdTaskList).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<JDType>> GetAllJDTypes(int id)
        {
            return await _context.jDTypes.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<JDTaskList>> GetAllJDTaskList(int id)
        {
            return await _context.jDTaskLists.AsNoTracking().ToListAsync();
        }
        public async Task<bool> DeleteJDTaskTypeById (int id)
        {
            _context.jDTaskTypes.Remove(_context.jDTaskTypes.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<HrDivision>> GetHrdivision()
        {
            return await _context.hrDivisions.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<HrBranch>> GetAllHrBranch()
        {
            return await _context.hrBranches.AsNoTracking().ToListAsync();
        }

    }
}


