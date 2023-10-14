using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using Microsoft.EntityFrameworkCore;
using OPUSERP.HRPMS.Data.Entity.Attendance;
using OPUSERP.Data.Entity.MasterData;

namespace OPUSERP.HRPMS.Services.MasterData
{
    public class EventDutyService : IEventDutyService
    {
        private readonly ERPDbContext _context;

        public EventDutyService(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeeAllocation>> GetEmployeeAllocation()
        {
            return await _context.employeeAllocations.Include(x => x.employeeInfo).Include(x => x.employeeInfo.lastDesignation).Include(x => x.employeeInfo.department).Include(x => x.duty).AsNoTracking().ToListAsync();
        }

        public async Task<EmployeeAllocation> GetEmployeeAllocationById(int id)
        {
            return await _context.employeeAllocations.FindAsync(id);
        }

        public async Task<bool> SaveEmployeeAllocation(EmployeeAllocation employeeAllocation)
        {
            if(employeeAllocation.Id != 0)
                _context.employeeAllocations.Update(employeeAllocation);
            else
                _context.employeeAllocations.Add(employeeAllocation);
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteEmployeeAllocationById(int id)
        {
            _context.employeeAllocations.Remove(_context.employeeAllocations.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
        // Cadre Info

        public async Task<IEnumerable<SpecialEventDutyMaster>> Getduty()
        {
            return await _context.eventDutyMasters.AsNoTracking().ToListAsync();
        }

        public async Task<SpecialEventDutyMaster> GetdutyById(int id)
        {
            return await _context.eventDutyMasters.FindAsync(id);
        }

        public async Task<bool> Saveduty(SpecialEventDutyMaster duty)
        {
            if(duty.Id != 0)
                _context.eventDutyMasters.Update(duty);
            else
                _context.eventDutyMasters.Add(duty);
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<bool> DeletedutyById(int id)
        {
            _context.eventDutyMasters.Remove(_context.eventDutyMasters.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
    }
}
