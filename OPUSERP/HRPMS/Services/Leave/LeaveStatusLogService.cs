using Microsoft.EntityFrameworkCore;
using OPUSERP.Areas.HRPMSLeave.Models;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Leave;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Leave
{
    public class LeaveStatusLogService : ILeaveStatusLogService
    {
        private readonly ERPDbContext _context;

        public LeaveStatusLogService(ERPDbContext context)
        {
            _context = context;
        }

        public async  Task<bool> SaveLeaveStatusLog(LeaveStatusLog leaveStatusLog)
        {
            if (leaveStatusLog.Id != 0)
                _context.leaveStatusLogs.Update(leaveStatusLog);
            else
                _context.leaveStatusLogs.Add(leaveStatusLog);
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LeaveStatusLog>> GetAllLeaveStatusLog()
        {
            return await _context.leaveStatusLogs.AsNoTracking().ToListAsync();
        }

        //public async Task<IEnumerable<LeaveStatusLog>> GetAllLeaveStatusLogByLeaveId(int id)
        //{
        //    return await _context.leaveStatusLogs.Where(x=>x.leaveRegisterId==id).Include(x=>x.leaveRegister.leaveType).Include(x=>x.leaveRegister.employee).Include(x=>x.employee).AsNoTracking().ToListAsync();
        //}
		public async Task<IEnumerable<LeaveStatusLogViewModel>> GetAllLeaveStatusLogByLeaveId(int id)
        {
			var log = await _context.leaveStatusLogs.Where(x => x.leaveRegisterId == id).Include(x => x.leaveRegister.leaveType).Include(x => x.leaveRegister.employee).Include(x => x.employee).AsNoTracking().ToListAsync();
			var data = new List<LeaveStatusLogViewModel>();
			foreach (var item in log)
			{
				data.Add(new LeaveStatusLogViewModel
				{
					leaveStatusLog = item,
					employeeInfo = await _context.employeeInfos.Where(x => x.ApplicationUser.UserName == item.createdBy).FirstOrDefaultAsync()
				});
			}

			return data;
        }

        public async Task<LeaveStatusLog> GetLeaveStatusLogById(int id)
        {
            return await _context.leaveStatusLogs.FindAsync(id);
        }

        public async  Task<bool> DeleteLeaveStatusLogById(int id)
        {
            _context.leaveStatusLogs.Remove(_context.leaveStatusLogs.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateLeaveRegisterStatus(int? leaveRegisterId, int? status)
        {
            var data = await _context.leaveRegisters.FindAsync(leaveRegisterId);

            data.leaveStatus = status;

            _context.leaveRegisters.Update(data);

            return await _context.SaveChangesAsync();
        }

    }
}
