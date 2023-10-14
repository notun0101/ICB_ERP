using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Employee
{
    public class PromotionLogService : IPromotionLogService
    {
        private readonly ERPDbContext _context;

        public PromotionLogService(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeletePromotionLogById(int id)
        {
            _context.promotionLogs.Remove(_context.promotionLogs.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PromotionLog>> GetPromotionLog()
        {
            return await _context.promotionLogs.Include(x => x.designationNew).Include(x => x.designationOld).Include(x => x.employee).Include(x => x.employee.department).Include(x => x.employee.lastDesignation).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<PromotionLog>> GetPromotionLogByEmpId(int empId)
        {
            return await _context.promotionLogs.Where(x => x.employeeId == empId).Include(x=>x.designationNew).Include(x => x.designationOld).AsNoTracking().ToListAsync();
        }

        public async Task<PromotionLog> GetLastPromotionByEmpId(int empId)
        {
            return await _context.promotionLogs.Where(x => x.employeeId == empId).OrderBy(x => Convert.ToDateTime(x.date).Date).LastOrDefaultAsync();
        }

        public async Task<PromotionLog> GetPromotionLogById(int id)
        {
            return await _context.promotionLogs.FindAsync(id);
        }
        public async Task<IEnumerable<PromotionLog>> GetPromotionHistoryByEmpId(int Id)
        {
            return await _context.promotionLogs.Include(x => x.designationNew).Include(x => x.designationOld).Include(x => x.employee.department).Include(x=>x.employee).Where(x => x.employeeId == Id).AsNoTracking().ToListAsync();
        }
        public async Task<bool> SavePromotionLog(PromotionLog promotionLog)
        {
            if (promotionLog.Id != 0)
                _context.promotionLogs.Update(promotionLog);
            else
                _context.promotionLogs.Add(promotionLog);

            return 1 == await _context.SaveChangesAsync();
        }
    }
}
