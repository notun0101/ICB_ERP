﻿using OPUSERP.Areas.HRPMSLeave.Models;
using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Leave;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Leave.Interfaces
{
   public interface ILeavePolicyService
    {
        #region Leave Policy

        Task<bool> SaveLeavePolicy(LeavePolicy leavePolicy);
        Task<IEnumerable<LeavePolicy>> GetLeavePolicy();
        Task<LeavePolicy> GetLeavePolicyById(int id);
        Task<LeavePolicy> GetLeavePolicyByTypeandYear(int typeId, int year);
        Task<bool> DeleteLeavePolicyById(int id);
        

        #endregion

        #region Leave Opening Balance

        Task<bool> SaveLeaveOpeningBalance(LeaveOpeningBalance leaveOpeningBalance);
        Task<IEnumerable<LeaveOpeningBalance>> GetLeaveOpeningBalance();
        Task<IEnumerable<LeaveOpeningBalance>> GetLeaveOpeningBalanceByEmpAndYear(int empId, int year);
        Task<bool> DeleteLeaveOpeningBalanceById(int id);
        Task<bool> OpeningBalanceProcess(int id);
        #endregion

        #region Leave Day
        Task<bool> SaveLeaveDay(LeaveDay leaveDay);
        Task<IEnumerable<LeaveDay>> GetAllLeaveDay();
        Task<LeaveDay> GetLeaveDayById(int id);
        Task<bool> DeleteLeaveDayById(int id);
		#endregion
		Task<IEnumerable<LeaveStatusViewModel>> GetLeaveStatus(int id);
		Task<IEnumerable<ApprovalDetail>> GetApprovalDetailsByUser(int id);
		Task<IEnumerable<EmployeeWithUser>> GetAllUserList();
		Task<EmployeeInfo> GetEmployeeByCode(string id);
		Task<ApplicationUser> GetUserByCode(string id);
		Task<ApplicationUser> UserByEmployeeId(int id);
		Task<bool> CheckPolicyExistence(int type, int year);
		Task<IEnumerable<ApprovalDetail>> GetApprovalDetailsByEmployeeId(int id);
	}
}
