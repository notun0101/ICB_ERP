using Microsoft.EntityFrameworkCore;
using OPUSERP.Areas.Auth.Models;
using OPUSERP.Areas.MasterData.Models;
using OPUSERP.Data;
using OPUSERP.Data.Entity;
using OPUSERP.Data.Entity.Auth;
using OPUSERP.Data.Entity.MasterData;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Models;
using OPUSERP.SCM.Data.Entity.ProjectStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace OPUSERP.ERPServices.AuthService
{
    public class UserInfoes : IUserInfoes
    {
        private readonly ERPDbContext _context;
        public UserInfoes(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationUser> GetAspnetuserByUser(string userName)
        {
            return await _context.Users.Where(x => x.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserType>> GetUserTypeList()
        {
            return await _context.UserTypes.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<UserBackup>> GetUserBackupList()
        {
            return await _context.userBackups.AsNoTracking().ToListAsync();
        }
        public async Task<int> SaveDailyProgressReport(DailyProgressReport dailyProgress)
        {
            if (dailyProgress.Id != 0)
            {
                _context.DailyProgressReports.Update(dailyProgress);
            }
            else
            {
                _context.DailyProgressReports.Add(dailyProgress);
            }

            await _context.SaveChangesAsync();
            return dailyProgress.Id;
        }
        public async Task<IEnumerable<UserBackUpViewModel>> GetUserBackupListWithEmp()
        {
            List<UserBackUpViewModel> User = new List<UserBackUpViewModel>();
            var backlist = await _context.userBackups.AsNoTracking().ToListAsync();
            foreach (var data in backlist)
            {
                User.Add(new UserBackUpViewModel
                {
                    userBackup = data,
                    employeeInfo = await _context.employeeInfos.Where(x => x.employeeCode == data.EmpCode).FirstOrDefaultAsync()
                });
            }
            return User;
        }

        public async Task<IEnumerable<string>> GetRoleListByUserId(string Id)
        {
            return await _context.UserRoles.Where(x => x.UserId == Id).Select(x => x.RoleId).ToListAsync();
        }

        public async Task<bool> DeleteUserRoleListByUserId(string Id)
        {
            _context.UserRoles.RemoveRange(_context.UserRoles.Where(x => x.UserId == Id).ToList());
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteRoleById(string Id)
        {
            _context.Roles.Remove(_context.Roles.Where(x => x.Id == Id).First());
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteMenuByRoleId(string Id)
        {
            _context.UserAccessPages.RemoveRange(_context.UserAccessPages.Where(x => x.applicationRoleId == Id).ToList());
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<ApplicationUser> GetUserBasicInfoes(string userName)
        {
            return await _context.Users.Where(x => x.UserName == userName).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<ApplicationUser>> GetUserInfoListByUser(string userName)
        {
            return await _context.Users.Where(x => x.UserName == userName).ToListAsync();
        }

        public async Task<AspNetUsersViewModel> GetUserInfoByUser(string userName)
        {
            var result = (from U in _context.Users

                          join E in _context.employeeInfos on U.Id equals E.ApplicationUserId into EE
                          from emp in EE.DefaultIfEmpty()
                          join pl in _context.ProjectConstructionLocations.Include(x => x.project) on U.Id equals pl.ApplicationUserId into pp
                          from PCL in pp.DefaultIfEmpty()
                          join D in _context.departments on emp.departmentId equals D.Id into DD
                          from dpt in DD.DefaultIfEmpty()
                          where U.UserName == userName

                          select new AspNetUsersViewModel
                          {
                              aspnetId = U.Id,
                              companyId = (U.companyId == null) ? 0 : U.companyId,
                              UserName = U.UserName,
                              UserTypeId = (U.userTypeId == null) ? 0 : U.userTypeId,
                              Email = U.Email,
                              EmpCode = U.EmpCode,
                              FinancialValue = U.MaxAmount,
                              UserId = (U.userId == null) ? 0 : U.userId,
                              isActive = (U.isActive == null) ? 0 : U.isActive,
                              EmpName = emp.nameEnglish,
                              EmployeeId = (emp.Id == null) ? 0 : emp.Id,
                              DivisionName = dpt.deptName,
                              projectId = (emp.branchId == null) ? 0 : emp.branchId,
                              departmentId = emp.departmentId,
                              DesignationName = emp.designation,
                              projId = PCL.projectId,
                              projectName = PCL.project.projectName,
                              //imageUrl= _context.photographs.Where(x => x.employeeId==emp.Id).Select(x=>x.url).FirstOrDefaultAsync(),
                              specialBranchUnitId = (U.specialBranchUnitId == null) ? 2 : U.specialBranchUnitId
                          }).FirstOrDefaultAsync();
            return await result;
        }

        public async Task<AspNetUsersViewModel> GetSbuIdByEmployeeEmail(string emailId)
        {
            var result = (from E in _context.employeeInfos
                          where E.emailAddress == emailId
                          select new AspNetUsersViewModel
                          {
                              specialBranchUnitId = E.branchId
                          }).FirstOrDefaultAsync();
            return await result;
        }

		public async Task<IEnumerable<GetAllUserViewModel>> GetAllUserInfos()
		{
			var result = await _context.getAllUserViewModels.FromSql($"sp_GetAllUserInfos").ToListAsync();
			return result;
		}

		public async Task<IEnumerable<ApplicationRoleViewModel>> GetAllRoles()
		{
			var data = await _context.Roles.Where(x => x.Name != "admin" && x.Name != "ERPAdmin").Select(x => new ApplicationRoleViewModel
			{
				RoleId = x.Id,
				RoleName = x.Name
			}).ToListAsync();
			return data;
		}

        public async Task<IEnumerable<AspNetUsersViewModel>> GetUserInfoList()
        {
            List<AspNetUsersViewModel> result = (from U in _context.Users
                                                     //join empInfo in _context.employeeInfos on U.Id equals empInfo.ApplicationUserId into Details
                                                     //join P in _context.photographs on U.EmpCode equals P.employee.employeeCode
                                                     //from m in Details.DefaultIfEmpty()
                                                 join E in _context.employeeInfos on U.Id equals E.ApplicationUserId
                                                 into EE
                                                 from m in EE.DefaultIfEmpty()
                                                 join PH in _context.photographs on m.employeeCode equals PH.employee.employeeCode
                                                 into PP
                                                 from P in EE.DefaultIfEmpty()
                                                 select new AspNetUsersViewModel
                                                 {
                                                     aspnetId = U.Id,
                                                     UserName = U.UserName,
                                                     UserTypeId = U.userTypeId,
                                                     Email = U.Email,
                                                     EmpCode = U.EmpCode,
                                                     FinancialValue = U.MaxAmount,
                                                     UserId = U.userId,
                                                     isActive = U.isActive,
                                                     departmentName = m.department.deptName,
                                                     empType = m.employeeType.empType,
                                                     joiningDate = m.joiningDateGovtService,
                                                     mobileNo = m.mobileNumberOffice,
                                                     email = m.emailAddress,
                                                     status = m.activityStatus,
                                                     photoId = P.Id,
                                                     EmpName = m.nameEnglish,
                                                     EmployeeId = (m.Id == null) ? 0 : m.Id,
                                                     DesignationName = m.designation
                                                 }).ToList();

            var aspnetolelist = _context.UserRoles.ToList();
            var aspnetrolenamelist = _context.Roles.ToList();
            List<AspNetUsersViewModel> aspNetUsersViewModels = new List<AspNetUsersViewModel>();
            foreach (AspNetUsersViewModel data in result)
            {
                var roleId = aspnetolelist.Where(x => x.UserId == data.aspnetId).ToList();
                List<string> role = new List<string>();
                foreach (var UserRole in roleId)
                {
                    string rnam = aspnetrolenamelist.Where(x => x.Id == UserRole.RoleId).Select(x => x.Name).First();
                    role.Add(rnam);
                }

                aspNetUsersViewModels.Add(new AspNetUsersViewModel
                {
                    aspnetId = data.aspnetId,
                    UserName = data.UserName,
                    UserTypeId = data.UserTypeId,
                    Email = data.Email,
                    EmpCode = data.EmpCode,
                    FinancialValue = data.FinancialValue,
                    UserId = data.UserId,
                    isActive = data.isActive,
                    departmentName = data.departmentName,
                    empType = data.empType,
                    joiningDate = data.joiningDate,
                    mobileNo = data.mobileNo,
                    email = data.email,
                    status = data.status,
                    photoId = data.photoId,
                    EmpName = data.EmpName,
                    EmployeeId = data.EmployeeId,
                    DesignationName = data.DesignationName,
                    roleId = string.Join(",", role),
                    DivisionName = await _context.photographs.Where(x => x.employeeId == data.EmployeeId).Select(x => x.url).FirstOrDefaultAsync()
                });

            }
            return aspNetUsersViewModels;
        }
		public async Task<AspNetUsersViewModel> GetUserInfoByUserNameNew(string userName)
		{
			var result = await (from U in _context.Users.Where(x => x.UserName == userName)
								join E in _context.employeeInfos.Include(x => x.lastDesignation) on U.Id equals E.ApplicationUserId
								into EE
								from m in EE.DefaultIfEmpty()
								join PH in _context.photographs on m.employeeCode equals PH.employee.employeeCode
								into PP
								from P in EE.DefaultIfEmpty()
								select new AspNetUsersViewModel
								{
									aspnetId = U.Id,
									UserName = U.UserName,
									UserTypeId = U.userTypeId,
									Email = U.Email,
									EmpCode = U.EmpCode,
									FinancialValue = U.MaxAmount,
									UserId = U.userId,
									isActive = U.isActive,
									departmentName = m.department.deptName,
									empType = m.employeeType.empType,
									joiningDate = m.joiningDateGovtService,
									mobileNo = m.mobileNumberOffice,
									email = m.emailAddress,
									status = m.activityStatus,
									photoId = PP.FirstOrDefault().Id,
									photoUrl = PP.FirstOrDefault().url,
									EmpName = m.nameEnglish,
									EmployeeId = (m.Id == null) ? 0 : m.Id,
									DesignationName = m.lastDesignation.designationName
								}).FirstOrDefaultAsync();
			return result;
		}

		public async Task<GetAllUserViewModelSp> GetUserInfoByUserNameSp(string userName)
		{
			var result = _context.getAllUserViewModelsp.FromSql($"sp_GetAllUserInfoByUserName {userName}").AsNoTracking().FirstOrDefaultAsync();
			return await result;
		}

		public async Task<IEnumerable<AspNetUsersViewModel>> GetUserInfoByUserName(string userName)
        {
            List<AspNetUsersViewModel> result = (from U in _context.Users.Where(x => x.UserName == userName)
                                                     //join empInfo in _context.employeeInfos on U.Id equals empInfo.ApplicationUserId into Details
                                                     //join P in _context.photographs on U.EmpCode equals P.employee.employeeCode
                                                     //from m in Details.DefaultIfEmpty()
                                                 join E in _context.employeeInfos on U.Id equals E.ApplicationUserId
                                                 into EE
                                                 from m in EE.DefaultIfEmpty()
                                                 join PH in _context.photographs on m.employeeCode equals PH.employee.employeeCode
                                                 into PP
                                                 from P in EE.DefaultIfEmpty()
                                                 select new AspNetUsersViewModel
                                                 {
                                                     aspnetId = U.Id,
                                                     UserName = U.UserName,
                                                     UserTypeId = U.userTypeId,
                                                     Email = U.Email,
                                                     EmpCode = U.EmpCode,
                                                     FinancialValue = U.MaxAmount,
                                                     UserId = U.userId,
                                                     isActive = U.isActive,
                                                     departmentName = m.department.deptName,
                                                     empType = m.employeeType.empType,
                                                     joiningDate = m.joiningDateGovtService,
                                                     mobileNo = m.mobileNumberOffice,
                                                     email = m.emailAddress,
                                                     status = m.activityStatus,
                                                     photoId = PP.FirstOrDefault().Id,
                                                     EmpName = m.nameEnglish,
                                                     EmployeeId = (m.Id == null) ? 0 : m.Id,
                                                     DesignationName = m.designation
                                                 }).ToList();

            var aspnetolelist = _context.UserRoles.ToList();
            var aspnetrolenamelist = _context.Roles.ToList();
            List<AspNetUsersViewModel> aspNetUsersViewModels = new List<AspNetUsersViewModel>();
            foreach (AspNetUsersViewModel data in result)
            {
                var roleId = aspnetolelist.Where(x => x.UserId == data.aspnetId).ToList();
                List<string> role = new List<string>();
                foreach (var UserRole in roleId)
                {
                    string rnam = aspnetrolenamelist.Where(x => x.Id == UserRole.RoleId).Select(x => x.Name).First();
                    role.Add(rnam);
                }
                aspNetUsersViewModels.Add(new AspNetUsersViewModel
                {
                    aspnetId = data.aspnetId,
                    UserName = data.UserName,
                    UserTypeId = data.UserTypeId,
                    Email = data.Email,
                    EmpCode = data.EmpCode,
                    FinancialValue = data.FinancialValue,
                    UserId = data.UserId,
                    isActive = data.isActive,
                    departmentName = data.departmentName,
                    empType = data.empType,
                    joiningDate = data.joiningDate,
                    mobileNo = data.mobileNo,
                    email = data.email,
                    status = data.status,
                    photoId = data.photoId,
                    EmpName = data.EmpName,
                    EmployeeId = data.EmployeeId,
                    DesignationName = data.DesignationName,
                    roleId = string.Join(",", role),
                    DivisionName = await _context.photographs.Where(x => x.employeeId == data.EmployeeId).Select(x => x.url).FirstOrDefaultAsync()
                });

            }
            return aspNetUsersViewModels;
        }

        public async Task<IEnumerable<AspNetUsersViewModel>> GetUserInfoListForProxyAdmin(string userRoleId, string userName)
        {
            List<AspNetUsersViewModel> result = (from U in _context.Users
                                                 join E in _context.employeeInfos on U.Id equals E.ApplicationUserId
                                                 into EE
                                                 from m in EE.DefaultIfEmpty()
                                                 join PH in _context.photographs on m.employeeCode equals PH.employee.employeeCode
                                                 into PP
                                                 from P in PP.DefaultIfEmpty()
                                                 where U.UserName == (userName == "NA" ? U.UserName : userName)
                                                 select new AspNetUsersViewModel
                                                 {
                                                     aspnetId = U.Id,
                                                     UserName = U.UserName,
                                                     UserTypeId = U.userTypeId,
                                                     Email = P.url,
                                                     EmpCode = U.EmpCode,
                                                     FinancialValue = U.MaxAmount,
                                                     UserId = U.userId,
                                                     isActive = U.isActive,
                                                     departmentName = m.department.deptName,
                                                     empType = m.employeeType.empType,
                                                     joiningDate = m.joiningDateGovtService,
                                                     mobileNo = m.mobileNumberOffice,
                                                     email = m.emailAddress,
                                                     status = m.activityStatus,
                                                     photoId = P.Id,
                                                     EmpName = m.nameEnglish,
                                                     EmployeeId = (m.Id == null) ? 0 : m.Id,
                                                     DesignationName = m.designation
                                                 }).ToList();

            var aspnetolelist = _context.UserRoles.Where(x => x.RoleId == (userRoleId == "NA" ? x.RoleId : userRoleId)).ToList();
            var aspnetrolenamelist = _context.Roles.ToList();
            List<AspNetUsersViewModel> aspNetUsersViewModels = new List<AspNetUsersViewModel>();
            foreach (AspNetUsersViewModel data in result)
            {
                var roleId = aspnetolelist.Where(x => x.UserId == data.aspnetId).ToList();
                List<string> role = new List<string>();
                foreach (var UserRole in roleId)
                {
                    string rnam = aspnetrolenamelist.Where(x => x.Id == UserRole.RoleId).Select(x => x.Name).First();
                    role.Add(rnam);
                }
                aspNetUsersViewModels.Add(new AspNetUsersViewModel
                {
                    aspnetId = data.aspnetId,
                    UserName = data.UserName,
                    UserTypeId = data.UserTypeId,
                    Email = data.Email,
                    EmpCode = data.EmpCode,
                    FinancialValue = data.FinancialValue,
                    UserId = data.UserId,
                    isActive = data.isActive,
                    departmentName = data.departmentName,
                    empType = data.empType,
                    joiningDate = data.joiningDate,
                    mobileNo = data.mobileNo,
                    email = data.email,
                    status = data.status,
                    photoId = data.photoId,
                    EmpName = data.EmpName,
                    EmployeeId = data.EmployeeId,
                    DesignationName = data.DesignationName,
                    roleId = string.Join(",", role),
                    DivisionName = await _context.photographs.Where(x => x.employeeId == data.EmployeeId).Select(x => x.url).FirstOrDefaultAsync()
                });

            }
            return aspNetUsersViewModels;
        }

        public async Task<AspNetUsersViewModel> GetUserInfoByUserId(int? UserId)
        {
            var result = (from U in _context.Users
                          join E in _context.employeeInfos on U.Id equals E.ApplicationUserId
                          join D in _context.departments on E.departmentId equals D.Id
                          where U.userId == UserId
                          select new AspNetUsersViewModel
                          {
                              aspnetId = U.Id,
                              UserName = U.UserName,
                              UserTypeId = U.userTypeId,
                              Email = U.Email,
                              EmpCode = U.EmpCode,
                              FinancialValue = U.MaxAmount,
                              UserId = U.userId,
                              isActive = U.isActive,
                              EmpName = E.nameEnglish,
                              DivisionName = D.deptName
                          }).FirstOrDefaultAsync();
            return await result;
        }

        public async Task<EmployeeInfoViewModel> GetEmployeeInfobyUser(string userName)
        {
            var result = _context.employeeInfoViewModels.FromSql($"sp_GetAspnetuserInfoByuser {userName}").AsNoTracking().FirstOrDefaultAsync();
            return await result;
        }

        public async Task<IEnumerable<AspNetUsersViewModel>> GetUserInfo()
        {
            var result = (from U in _context.Users
                          join E in _context.employeeInfos on U.Id equals E.ApplicationUserId
                          select new AspNetUsersViewModel
                          {
                              aspnetId = U.Id,
                              UserName = U.UserName,
                              UserTypeId = U.userTypeId,
                              Email = U.Email,
                              EmpCode = U.EmpCode,
                              FinancialValue = U.MaxAmount,
                              UserId = U.userId,
                              isActive = U.isActive,
                              EmpName = E.nameEnglish + " - " + E.employeeCode,
                              EmployeeId = E.Id,
                              //DivisionName = EEE.department.deptName

                          }).ToListAsync();
            return await result;
        }

        public async Task<int> GetMaxUserId()
        {
            var result = await _context.Users.MaxAsync(x => x.userId);
            return result;
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            var data = await _context.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
            return data;
        }

        public async Task<ApplicationUser> GetUserByUserName(string email)
        {
            var data = await _context.Users.Where(x => x.UserName == email).FirstOrDefaultAsync();
            return data;
        }

        public async Task<int> StoreForgotPassCode(string email, string username, string code, DateTime? expire)
        {
            var existHistory = await _context.forgotPasswordHistories.Where(x => x.email == email).FirstOrDefaultAsync();
            if (existHistory != null)
            {
                existHistory.recoverCode = code;
                existHistory.codeExpire = expire;
                existHistory.changeReqCount = existHistory.changeReqCount + 1;
                _context.forgotPasswordHistories.Update(existHistory);
                await _context.SaveChangesAsync();
            }
            else
            {
                var data = new ForgotPasswordHistory
                {
                    Id = 0,
                    ipAddress = GetLocalIPAddress(),
                    email = email,
                    username = username,
                    recoverCode = code,
                    codeExpire = expire,
                    changeReqCount = 1
                };
                _context.forgotPasswordHistories.Add(data);
                await _context.SaveChangesAsync();
            }
            return 1;
        }

        public async Task<ForgotPasswordHistory> GetPassResetCodeByEmail(string email)
        {
            var data = await _context.forgotPasswordHistories.Where(x => x.username == email).OrderByDescending(x=>x.codeExpire).FirstOrDefaultAsync();
            return data;
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public async Task<IEnumerable<AspNetUsersViewModel>> GetUsersByEmployeeInfo()
        {
            try
            {
                return await _context.aspNetUsersViews.FromSql(@"sp_GetUsersByEmployeeInfo").AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<AspNetUsersApproverViewModel>> GetUsersApproverByEmployeeInfo()
        {
            try
            {
                return await _context.aspNetUsersApproverViews.FromSql(@"sp_GetUsersByEmployeeInfoapprover").AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<AspNetUsersViewModel>> GetAllActiveEmployeeInfo()
        {
            try
            {
                return await _context.aspNetUsersViews.FromSql(@"sp_GetAllActiveEmployeeInfo").AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<AspNetUsersViewModel>> GetAllActiveEmployeeInfoForOwnerChange()
        {
            try
            {
                return await _context.aspNetUsersViews.FromSql(@"sp_GetAllActiveEmployeeInfoForOwnerChange").AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<AspNetUsersViewModel>> GetUserInfoByModule(int moduleId)
        {
            try
            {
                return await _context.aspNetUsersViews.FromSql($"sp_GetAspNetUsersByModule  {moduleId}").AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ERPModule>> GetAllERPModule()
        {
            return await _context.ERPModules.AsNoTracking().ToListAsync();
        }
        public async Task<EmployeeInfo> GetemployeebyempCode(string empcode)
        {
            return await _context.employeeInfos.Where(x => x.employeeCode == empcode).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<EmployeeInfo> Getemployeebyname(string empcode)
        {
            return await _context.employeeInfos.Where(x => x.nameEnglish == empcode).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<UpdateAspnetUser> UpdateAspNetUserByUserIdAndStatus(string userId, int status)
        {
            try
            {
                var user = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
                user.isActive = status;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                var data = new UpdateAspnetUser
                {
                    status = (int)user.isActive
                };
                return data;
                //return await _context.updateAspnetUsers.FromSql($"SP_UpdateAspNetUserByUserIdAndStatus {userId},{status}").FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UpdateAspnetUser> DeleteAspNetUserByUserId(string userId, string userName)
        {
            try
            {
                return await _context.updateAspnetUsers.FromSql($"SP_DeleteAspNetUserByUserId {userId},{userName}").FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AspNetUsersViewModel> GetUserInfoWinthEmpByUser(string userName)
        {
            var result = (from U in _context.Users

                          join E in _context.employeeInfos on U.Id equals E.ApplicationUserId into EE
                          from emp in EE.DefaultIfEmpty()

                          join D in _context.designations on emp.departmentId equals D.Id into DD
                          from dpt in DD.DefaultIfEmpty()
                          where U.UserName == userName

                          select new AspNetUsersViewModel
                          {
                              aspnetId = U.Id,
                              companyId = (U.companyId == null) ? 0 : U.companyId,
                              UserName = U.UserName,
                              UserTypeId = (U.userTypeId == null) ? 0 : U.userTypeId,
                              Email = U.Email,
                              EmpCode = U.EmpCode,
                              UserId = (U.userId == null) ? 0 : U.userId,
                              isActive = (U.isActive == null) ? 0 : U.isActive,
                              EmpName = emp.nameEnglish,
                              EmployeeId = (emp.Id == null) ? 0 : emp.Id,
                              departmentName = dpt.designationName,
                              projectId = (emp.branchId == null) ? 0 : emp.branchId,
                              DesignationName = emp.designation
                          }).FirstOrDefaultAsync();
            return await result;
        }

        public async Task<ApplicationUser> getUserByEmployeeId(int id)
        {
            var appUserId = _context.employeeInfos.Where(x => x.Id == id).Select(x => x.ApplicationUserId).FirstOrDefault();
            return await _context.Users.Where(x => x.Id == appUserId).FirstOrDefaultAsync();
        }
		public async Task<string> UpdateAspNetUser(ApplicationUser user)
		{
			_context.Users.Update(user);
			await _context.SaveChangesAsync();
			return user.EmpCode;
		}

        public async Task<IEnumerable<EmployeeInfo>> GetEmployeesWithoutUser()
        {
            var user = await _context.Users.Select(x => x.UserName).ToListAsync();
            var data = await _context.employeeInfos.Where(x => !user.Contains(x.employeeCode)).ToListAsync();
            return data;
        }


        public async Task<IEnumerable<ApplicationRoleViewModel>> GetAllUserRoles()
        {
            var data = await _context.Roles.Select(x => new ApplicationRoleViewModel
            {
                RoleId = x.Id,
                RoleName = x.Name

            }).ToListAsync();
            return data;
        }

        public async Task<IEnumerable<HrBranch>> GetAllBranch()
        {
            return await _context.hrBranches.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<GetAllUserViewNewModel>> GetAllUserInfosForProxyUser(string userName, string rolename, string branchId)
        {
            try
            {
                var data = await _context.getAllUserViewNewModels.FromSql($"sp_GetAllUserInfosForProxyUser {userName},{rolename},{branchId}").ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
    }
}
