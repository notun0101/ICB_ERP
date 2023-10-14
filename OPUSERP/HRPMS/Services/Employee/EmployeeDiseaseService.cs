using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Employee
{
    public class EmployeeDiseaseService : IEmployeeDiseaseService
    {
        private readonly ERPDbContext _context;

        public EmployeeDiseaseService (ERPDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteEmployeeDiseaseById(int id)
        {
            _context.employeeDiseases.Remove(_context.employeeDiseases.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
       

        public async Task<IEnumerable<EmployeeDisease>> GetEmployeeDisease()
        {
            return await _context.employeeDiseases.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<EmployeeDisease>> GetEmployeeDiseaseByEmpId(int empId)
        {
            return await _context.employeeDiseases.Where(x => x.employeeInfoId == empId).Include(x => x.employeeInfo).Include(x => x.medicalDisease)
                .OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        }
       
        public async Task<EmployeeDisease> GetEmployeeDiseaseById(int id)
        {
            return await _context.employeeDiseases.FindAsync(id);
        }

      
        public async Task<bool> SaveEmployeeDisease(EmployeeDisease employeeDisease)
        {
            if (employeeDisease.Id != 0)
                _context.employeeDiseases.Update(employeeDisease);
            else
                _context.employeeDiseases.Add(employeeDisease);

            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<int> IsThisNamePresent(int MDiseaseId)
        {
            return await _context.employeeDiseases.Where(x => x.medicalDiseaseId == MDiseaseId).AsNoTracking().Select(x => x.Id).FirstOrDefaultAsync();
        }
    }
}
