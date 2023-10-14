using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Employee
{
    public class EmployeeSportsService : IEmployeeSportsService
    {
        private readonly ERPDbContext _context;

        public EmployeeSportsService(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteEmployeeSportsById(int id)
        {
            _context.employeeSports.Remove(_context.employeeSports.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
        

        public async Task<IEnumerable<EmployeeSports>> GetEmployeeSports()
        {
            return await _context.employeeSports.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<EmployeeSports>> GetEmployeeSportsByEmpId(int empId)
        {
            return await _context.employeeSports.Where(x => x.employeeId == empId).Include(x => x.employee).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        }


        public async Task<EmployeeSports> GetEmployeeSportsById(int id)
        {
            return await _context.employeeSports.FindAsync(id);
        }

        public async Task<bool> SaveEmployeeSports(EmployeeSports employeeSports)
        {
            if (employeeSports.Id != 0)
                _context.employeeSports.Update(employeeSports);
            else
                _context.employeeSports.Add(employeeSports);

            return 1 == await _context.SaveChangesAsync();
        }
       
    }
}
