using Microsoft.EntityFrameworkCore;
using OPUSERP.Areas.Payroll.Models;
using OPUSERP.Data;
using OPUSERP.Data.Entity.MasterData;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Wages;
using OPUSERP.Payroll.Data;
using OPUSERP.Payroll.Data.Entity.Arrear;
using OPUSERP.Payroll.Data.Entity.PF;
using OPUSERP.Payroll.Data.Entity.Salary;
using OPUSERP.Payroll.Services.Salary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Payroll.Services.Salary
{
    public class ArrearService : IArrearService
    {
        private readonly ERPDbContext _context;

        public ArrearService(ERPDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<EmployeeArrearInfo>> GetAllArrearInfo()
        {
            return await _context.EmployeeArrearInfo.Include(a => a.employee).Include(x => x.employee.lastDesignation).Include(x => x.employee.department).Include(x => x.employee.currentGrade).Include(x=> x.period).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<SalaryPeriod>> GetSalaryPeriod()
        {
            return await _context.salaryPeriods.AsNoTracking().ToListAsync();
        }
        public async Task<int> SaveEmpArrearInfo(EmployeeArrearInfo employeeArrearInfo)
        {
            if (employeeArrearInfo.Id != 0)
                _context.EmployeeArrearInfo.Update(employeeArrearInfo);
            else
                _context.EmployeeArrearInfo.Add(employeeArrearInfo);
            await _context.SaveChangesAsync();
            return employeeArrearInfo.Id;
        }
        public async Task<int> SaveEmployeesArrearStructure(EmployeesArrearStructure arrearDetails)
        {
            if (arrearDetails.Id != 0)
                _context.employeesArrearStructures.Update(arrearDetails);
            else
                _context.employeesArrearStructures.Add(arrearDetails);
            await _context.SaveChangesAsync();
            return arrearDetails.Id;
        }
        public async Task<bool> DeleteArrerById(int id)
        {
            _context.EmployeeArrearInfo.Remove(_context.EmployeeArrearInfo.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EmployeesArrearStructure>> GetEmployeesArrearStructureByEmpId(int empId)
        {
            return await _context.employeesArrearStructures.Include(x => x.salaryHead).Include(x => x.salarySlab).Where(x => x.employeeInfoId == empId).AsNoTracking().ToListAsync();
        }

        public async Task<bool> DeleteEmployeesArrearStructureByempId(int empId)
        {
            _context.employeesArrearStructures.RemoveRange(_context.employeesArrearStructures.Where(x => x.employeeInfoId == empId));
            return 1 == await _context.SaveChangesAsync();
        }

    }
}