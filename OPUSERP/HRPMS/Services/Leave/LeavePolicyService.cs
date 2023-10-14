using Microsoft.EntityFrameworkCore;
using OPUSERP.Areas.HRPMSLeave.Models;
using OPUSERP.Data;
using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Leave;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.Leave.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Leave
{
    public class LeavePolicyService : ILeavePolicyService
    {
        private readonly ERPDbContext _context;

        public LeavePolicyService(ERPDbContext context)
        {
            _context = context;
        }

        #region Leave Policy

        public async Task<bool> SaveLeavePolicy(LeavePolicy leavePolicy)
        {
            if (leavePolicy.Id != 0)
                _context.leavePolicies.Update(leavePolicy);
            else
                _context.leavePolicies.Add(leavePolicy);
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LeavePolicy>> GetLeavePolicy()
        {
            return await _context.leavePolicies.Include(x => x.leaveType).Include(x => x.year).AsNoTracking().ToListAsync();
        }

        public async Task<LeavePolicy> GetLeavePolicyById(int id)
        {

            return await _context.leavePolicies.FindAsync(id);
        }

        public async Task<LeavePolicy> GetLeavePolicyByTypeandYear(int typeId, int year)
        {
            return await _context.leavePolicies.Where(x => x.leaveTypeId == typeId && x.yearId == year).Include(x => x.leaveType).Include(x => x.year).FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteLeavePolicyById(int id)
        {
            _context.leavePolicies.Remove(_context.leavePolicies.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        #endregion

        #region Leave Opening Balance

        public async Task<bool> SaveLeaveOpeningBalance(LeaveOpeningBalance leaveOpeningBalance)
        {
            if (leaveOpeningBalance.Id != 0)
                _context.leaveOpeningBalances.Update(leaveOpeningBalance);
            else
                _context.leaveOpeningBalances.Add(leaveOpeningBalance);
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LeaveOpeningBalance>> GetLeaveOpeningBalance()
        {
            return await _context.leaveOpeningBalances.Include(x => x.employee).Include(x=> x.employee.lastDesignation).Include(x => x.leaveType).Include(x => x.year).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<LeaveOpeningBalance>> GetLeaveOpeningBalanceByEmpAndYear(int empId, int year)
        {
            return await _context.leaveOpeningBalances.Where(x => x.employeeId == empId && x.yearId == year).Include(x => x.employee).Include(x => x.leaveType).Include(x => x.year).AsNoTracking().ToListAsync();
        }

        public async Task<bool> DeleteLeaveOpeningBalanceById(int id)
        {
            _context.leaveOpeningBalances.Remove(_context.leaveOpeningBalances.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
		public async Task<bool> CheckPolicyExistence(int type, int year)
        {
			var data = await _context.leavePolicies.Where(x => x.yearId == year && x.leaveTypeId == type).FirstOrDefaultAsync();
			if (data != null)
			{
				return true;
			}
			else
			{
				return false;
			}
        }

        public async Task<bool> OpeningBalanceProcess(int id)
        {
            LeavePolicy leavePolicy = await _context.leavePolicies.FindAsync(id);
            IEnumerable<EmployeeInfo> employeeInfos = await _context.employeeInfos.ToListAsync();

            foreach (var data in employeeInfos)
            {
                LeaveOpeningBalance leaveOpeningBalance = new LeaveOpeningBalance
                {
                    yearId = leavePolicy.yearId,
                    leaveTypeId = leavePolicy.leaveTypeId,
                    employeeId = data.Id,
                    leaveDays = leavePolicy.yearlyMaxLeave,
                    leaveCarryDays = leavePolicy.yearlyMaxCarry
                };
                _context.leaveOpeningBalances.Add(leaveOpeningBalance);
                await _context.SaveChangesAsync();
            }

            return false;
        }

        #endregion

        #region Leave Day
        public async Task<bool> SaveLeaveDay(LeaveDay leaveDay)
        {
            if (leaveDay.Id != 0)
                _context.leaveDays.Update(leaveDay);
            else
                _context.leaveDays.Add(leaveDay);
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LeaveDay>> GetAllLeaveDay()
        {
            return await _context.leaveDays.AsNoTracking().ToListAsync();
        }
		public async Task<IEnumerable<LeaveStatusViewModel>> GetLeaveStatus(int id)
        {
			var data = await _context.sp_leaveStatus.FromSql($"spLoadIndividualLeaveSummary {id}").ToListAsync();
			return data;
		}
		public async Task<EmployeeInfo> GetEmployeeByCode(string id)
        {
			return await _context.employeeInfos.Where(x => x.employeeCode == id).FirstOrDefaultAsync();
		}
		public async Task<ApplicationUser> GetUserByCode(string id)
        {
			return await _context.Users.Where(x => x.EmpCode == id).FirstOrDefaultAsync();
		}
		public async Task<IEnumerable<EmployeeWithUser>> GetAllUserList()
        {
			var data = await (from u in _context.Users
						join e in _context.employeeInfos on u.EmpCode equals e.employeeCode
						select new EmployeeWithUser
						{
							EmpCode = e.employeeCode,
							EmployeeName = e.nameEnglish,
							UserId = u.userId,
							UserName = u.UserName
						}).ToListAsync();
			return data;
		}

		public async Task<IEnumerable<ApprovalDetail>> GetApprovalDetailsByEmployeeId(int id)
		{
			var userId = await _context.employeeInfos.AsNoTracking().Where(x => x.Id == id).Select(x => x.ApplicationUser.userId).FirstOrDefaultAsync();

			var approvalMaster = _context.approvalMasters.Include(x => x.employeeInfo).Include(x => x.employeeInfo.lastDesignation).Include(x => x.employeeInfo.ApplicationUser).Where(x => x.employeeInfo.ApplicationUser.userId == userId).AsNoTracking().FirstOrDefault();
			if (approvalMaster == null)
			{
				return new List<ApprovalDetail>();
			}
			else
			{
				return await _context.approvalDetails.Include(x => x.approvalMaster).Include(x => x.approver).Include(x => x.approver.lastDesignation).Include(x => x.approver.ApplicationUser).Where(x => x.approvalMasterId == approvalMaster.Id).AsNoTracking().ToListAsync();
			}
		}
		public async Task<IEnumerable<ApprovalDetail>> GetApprovalDetailsByUser(int id)
        {
			var approvalMaster = _context.approvalMasters.Include(x => x.employeeInfo).Include(x => x.employeeInfo.lastDesignation).Include(x => x.employeeInfo.ApplicationUser).Where(x => x.employeeInfo.ApplicationUser.userId == id).AsNoTracking().FirstOrDefault();
			if (approvalMaster == null)
			{
				return new List<ApprovalDetail>();
			}
			else
			{
				return await _context.approvalDetails.Include(x => x.approvalMaster).Include(x => x.approver).Include(x => x.approver.lastDesignation).Include(x => x.approver.ApplicationUser).Where(x => x.approvalMasterId == approvalMaster.Id).AsNoTracking().ToListAsync();
			}
		}
		public async Task<ApplicationUser> UserByEmployeeId(int id)
        {
			var appuserId = _context.employeeInfos.Where(x => x.Id == id).Select(x => x.ApplicationUserId).AsNoTracking().FirstOrDefault();
			return await _context.Users.FindAsync(appuserId);
		}

        public async Task<LeaveDay> GetLeaveDayById(int id)
        {

            return await _context.leaveDays.FindAsync(id);
        }

        public async Task<bool> DeleteLeaveDayById(int id)
        {
            _context.leaveDays.Remove(_context.leaveDays.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
        

        #endregion
    }
}
