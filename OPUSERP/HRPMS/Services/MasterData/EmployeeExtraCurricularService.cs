using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.MasterData
{
    public class EmployeeExtraCurricularService : IEmployeeExtraCurricularService
    {
        private readonly ERPDbContext _context;

        public EmployeeExtraCurricularService(ERPDbContext context)
        {
            _context = context;
        }

        //public async Task<bool> SaveEmployeeExtraCurricular(EmployeeExtraCurricular employeeExtraCurricular)
        //{  


        //    if (employeeExtraCurricular.Id != 0)
        //        _context.employeeExtraCurriculars.Update(employeeExtraCurricular);
        //    else
        //        _context.employeeExtraCurriculars.Add(employeeExtraCurricular);

        //    return 1 == await _context.SaveChangesAsync();
        //}

        public async Task<int> SaveEmployeeExtraCurricular(EmployeeExtraCurricular employeeExtraCurricular)
        {
            if (employeeExtraCurricular.Id != 0)
                _context.employeeExtraCurriculars.Update(employeeExtraCurricular);
            else
                _context.employeeExtraCurriculars.Add(employeeExtraCurricular);
            await _context.SaveChangesAsync();
            return employeeExtraCurricular.Id;
        }

        public async Task<IEnumerable<EmployeeExtraCurricular>> GetEmployeeExtraCurricular()
        {
            return await _context.employeeExtraCurriculars.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<EmployeeExtraCurricular>> GetEmployeeExtraCurricularEmpId(int empId)
        {
            // return await _context.employeeExtraCurriculars.AsNoTracking().ToListAsync();
            return await _context.employeeExtraCurriculars.Include(x => x.extraCurricularType).Where(x => x.employeeId == empId).AsNoTracking().ToListAsync();
        }

        public async Task<EmployeeExtraCurricular> GetEmployeeExtraCurricularId(int id)
        {
            return await _context.employeeExtraCurriculars.FindAsync(id);
        }

        public async Task<bool> DeleteEmployeeExtraCurricularId(int id)
        {
            _context.employeeExtraCurriculars.Remove(_context.employeeExtraCurriculars.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
    }
}
