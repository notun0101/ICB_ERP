using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using Microsoft.EntityFrameworkCore;
using OPUSERP.HRPMS.Data.Entity.Employee;

namespace OPUSERP.HRPMS.Services.MasterData
{
    public class ShiftGroupMasterService : IShiftGroupMasterService
    {
        private readonly ERPDbContext _context;

        public ShiftGroupMasterService(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveShiftGroupMaster(ShiftGroupMaster shiftGroupMaster)
        {
            if(shiftGroupMaster.Id != 0)
                _context.shiftGroupMasters.Update(shiftGroupMaster);
            else
                _context.shiftGroupMasters.Add(shiftGroupMaster);
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ShiftGroupMaster>> GetAllShiftGroupMaster()
        {
            return await _context.shiftGroupMasters.AsNoTracking().ToListAsync();
        }
		public async Task<int> AssignShiftToEmployeeSer(int empCode, int masterId)
        {
			var emp = _context.employeeInfos.Where(x => x.Id == empCode).FirstOrDefault();
			emp.shiftGroupId = masterId;
			_context.employeeInfos.Update(emp);
			return await _context.SaveChangesAsync();
        }
		public async Task<IEnumerable<ShiftGroupDetail>> GetAllShiftGroupDetail()
        {
            return await _context.shiftGroupDetails.Include(x => x.shiftGroupMaster).AsNoTracking().ToListAsync();
        }
		public async Task<IEnumerable<EmployeeInfo>> GetEmployeeWithShiftGroup()
        {
            return await _context.employeeInfos.Include(x => x.shiftGroup).AsNoTracking().ToListAsync();
        }

        public async Task<ShiftGroupMaster> GetShiftGroupMasterById(int id)
        {
            return await _context.shiftGroupMasters.FindAsync(id);
        }

        public async Task<bool> DeleteShiftGroupMasterById(int id)
        {
            _context.shiftGroupMasters.Remove(_context.shiftGroupMasters.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<ShiftGroupMaster>> UpdateShiftGroupId(string ShiftType, int? sbu, int? department, int? employeeInfoId, int? shiftGroup)
        {
            return await _context.shiftGroupMasters.FromSql($"spUpdateShiftAssign {ShiftType},{sbu},{department},{employeeInfoId},{shiftGroup}").AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<ShiftGroupMaster>> UpdateShiftGroupIdForWages(string ShiftType, int? sbu, int? department, int? employeeInfoId, int? shiftGroup)
        {
            return await _context.shiftGroupMasters.FromSql($"spUpdateShiftAssignForWages {ShiftType},{sbu},{department},{employeeInfoId},{shiftGroup}").AsNoTracking().ToListAsync();
        }

    }
}
