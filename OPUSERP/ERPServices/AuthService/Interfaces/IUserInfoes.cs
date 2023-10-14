using OPUSERP.Areas.Auth.Models;
using OPUSERP.Areas.MasterData.Models;
using OPUSERP.Data.Entity;
using OPUSERP.Data.Entity.Auth;
using OPUSERP.Data.Entity.MasterData;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Models;
using OPUSERP.SCM.Data.Entity.ProjectStatus;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OPUSERP.ERPService.AuthService.Interfaces
{
    public interface IUserInfoes
    {
        Task<ApplicationUser> GetAspnetuserByUser(string userName);
        Task<EmployeeInfoViewModel> GetEmployeeInfobyUser(string userName);
        Task<IEnumerable<UserType>> GetUserTypeList();
        Task<IEnumerable<ApplicationUser>> GetUserInfoListByUser(string userName);
        Task<AspNetUsersViewModel> GetUserInfoByUser(string userName);
        Task<ApplicationUser> GetUserBasicInfoes(string userName);
        Task<IEnumerable<AspNetUsersViewModel>> GetUserInfo();
        Task<int> SaveDailyProgressReport(DailyProgressReport dailyProgress);
        Task<int> GetMaxUserId();
        Task<IEnumerable<AspNetUsersViewModel>> GetUsersByEmployeeInfo();
        Task<IEnumerable<AspNetUsersViewModel>> GetUserInfoByModule(int moduleId);
        Task<AspNetUsersViewModel> GetUserInfoByUserId(int? UserId);
        Task<IEnumerable<ERPModule>> GetAllERPModule();
        Task<IEnumerable<AspNetUsersViewModel>> GetUserInfoList();
        Task<IEnumerable<AspNetUsersViewModel>> GetUserInfoByUserName(string userName);
		Task<AspNetUsersViewModel> GetUserInfoByUserNameNew(string userName);
		Task<GetAllUserViewModelSp> GetUserInfoByUserNameSp(string userName);
		Task<IEnumerable<GetAllUserViewModel>> GetAllUserInfos();
		Task<IEnumerable<ApplicationRoleViewModel>> GetAllRoles();
        Task<IEnumerable<ApplicationRoleViewModel>> GetAllUserRoles();
        Task<IEnumerable<HrBranch>> GetAllBranch();
        Task<IEnumerable<AspNetUsersViewModel>> GetAllActiveEmployeeInfo();
        Task<IEnumerable<AspNetUsersViewModel>> GetAllActiveEmployeeInfoForOwnerChange();
        Task<AspNetUsersViewModel> GetSbuIdByEmployeeEmail(string emailId);
        Task<EmployeeInfo> GetemployeebyempCode(string empcode);
        Task<IEnumerable<AspNetUsersViewModel>> GetUserInfoListForProxyAdmin(string userRoleId, string userName);
        Task<UpdateAspnetUser> UpdateAspNetUserByUserIdAndStatus(string userId, int status);
        Task<UpdateAspnetUser> DeleteAspNetUserByUserId(string userId, string userName);
        Task<IEnumerable<string>> GetRoleListByUserId(string Id);
        Task<bool> DeleteUserRoleListByUserId(string Id);
        Task<IEnumerable<UserBackup>> GetUserBackupList();
        Task<IEnumerable<UserBackUpViewModel>> GetUserBackupListWithEmp();
        Task<bool> DeleteRoleById(string Id);
        Task<bool> DeleteMenuByRoleId(string Id);
        Task<EmployeeInfo> Getemployeebyname(string empcode);
        Task<IEnumerable<AspNetUsersApproverViewModel>> GetUsersApproverByEmployeeInfo();
        Task<AspNetUsersViewModel> GetUserInfoWinthEmpByUser(string userName);
		Task<ApplicationUser> getUserByEmployeeId(int id);
        Task<ApplicationUser> GetUserByEmail(string email);
        Task<ApplicationUser> GetUserByUserName(string email);
        Task<int> StoreForgotPassCode(string email, string username, string code, DateTime? expire);
        Task<ForgotPasswordHistory> GetPassResetCodeByEmail(string email);
		Task<string> UpdateAspNetUser(ApplicationUser user);
        Task<IEnumerable<EmployeeInfo>> GetEmployeesWithoutUser();
        Task<IEnumerable<GetAllUserViewNewModel>> GetAllUserInfosForProxyUser(string userName, string rolename, string branchId);
    }
}
