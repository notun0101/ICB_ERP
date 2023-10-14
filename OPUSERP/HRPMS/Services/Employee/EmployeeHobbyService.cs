using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Employee
{
    public class EmployeeHobbyService : IEmployeeHobbyService
    {
        private readonly ERPDbContext _context;

        public EmployeeHobbyService(ERPDbContext context)
        {
            _context = context;
        }



        public async Task<bool> DeleteEmployeeHobbyById(int id)
        {
            _context.employeeHobbies.Remove(_context.employeeHobbies.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EmployeeHobby>> GetEmployeeHobby()
        {
            return await _context.employeeHobbies.AsNoTracking().ToListAsync();
        }

        public async Task<EmployeeHobby> GetEmployeeHobbyById(int id)
        {
            return await _context.employeeHobbies.FindAsync(id);
        }
       
        

        public async Task<bool> SaveEmployeeHobby(EmployeeHobby employeeHobby)
        {
            if (employeeHobby.Id != 0)
                _context.employeeHobbies.Update(employeeHobby);
            else
                _context.employeeHobbies.Add(employeeHobby);
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EmployeeHobby>> GetEmployeeHobbyByEmpId(int empId)
        {
            return await _context.employeeHobbies.Where(x => x.employeeInfoId == empId).AsNoTracking().ToListAsync();
        }
    
       
       
    }
}
