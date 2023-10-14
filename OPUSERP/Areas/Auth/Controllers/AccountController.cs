using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OPUSERP.Areas.Auth.Models;
using OPUSERP.Controllers;
using OPUSERP.Data;
using OPUSERP.Data.Entity;
using OPUSERP.Data.Entity.Auth;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.ERPServices.AuthService.Interfaces;
using OPUSERP.ERPServices.EmailService.interfaces;
using OPUSERP.ERPServices.Interfaces;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Services.Attendance.Interfaces;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Auth.Controllers
{
    [Authorize]
    [Area("Auth")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IPersonalInfoService personalInfoService;
        private readonly IDbChangeService dbChangeService;
        private readonly IUserInfoes userInfoes;
        private readonly IAttendanceProcessService attendanceProcessService;
        private readonly IPageAssignService pageAssignService;
        private readonly IEmailSenderService emailSenderService;

		private ERPDbContext _db;
        public AccountController(UserManager<ApplicationUser> userManager, IEmailSenderService emailSenderService, IPageAssignService pageAssignService, ERPDbContext db, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IUserInfoes userInfoes, IPersonalInfoService personalInfoService, IDbChangeService dbChangeService, IAttendanceProcessService attendanceProcessService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            this.userInfoes = userInfoes;
            _db = db;
            this.personalInfoService = personalInfoService;
            this.dbChangeService = dbChangeService;
            this.emailSenderService = emailSenderService;

            this.attendanceProcessService = attendanceProcessService;
            this.pageAssignService = pageAssignService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LogInViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {

                var userInfos = await userInfoes.GetUserInfoByUser(model.Name);
                if (userInfos != null)
                {
                    if (userInfos.isActive == 1)
                    {
                        var result = await _signInManager.PasswordSignInAsync(model.Name, model.Password, model.RememberMe, lockoutOnFailure: true);
                        if (result.Succeeded)
                        {
                            var ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                            var userAgent = Request.Headers["User-Agent"].ToString();
                            var mechineName = Environment.MachineName;
                            var rip = Dns.GetHostEntry(HttpContext.Connection.RemoteIpAddress.ToString()).ToString();
							var WebBrowserName = Request.Headers["User-Agent"].ToString();
							UserLogHistory userLog = new UserLogHistory
                            {
                                userId = model.Name,
                                logTime = DateTime.Now,
                                status = 1,
                                ipAddress = ip,
                                pcName = mechineName,
                                browserName = userAgent
                            };

                            await dbChangeService.SaveUserLogHistory(userLog);


                            return RedirectToLocal(returnUrl);

                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            return View(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }


            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Loginnew(string returnUrl = null)
        {

            //var notuser = await userInfoes.GetEmployeesWithoutUser();
            //foreach (var item in notuser)
            //{
            //    int maxUserId = await userInfoes.GetMaxUserId() + 1;
            //    var user = new ApplicationUser { UserName = item.employeeCode, isActive = 1, Email = item.emailAddress, userId = maxUserId, EmpCode = item.employeeCode, specialBranchUnitId = 2, userTypeId = 9, isPassChange = 0 };
            //    var result = await _userManager.CreateAsync(user, "123456");

            //    await _userManager.AddToRoleAsync(user, "General User");
            //}


            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);


            //var data = await personalInfoService.GetActiveAllEmployeeInfo();
            //foreach (var item in data)
            //{
            //    int maxUserId = await userInfoes.GetMaxUserId() + 1;
            //    if (Convert.ToInt32(item.employeeCode) >= 1000)
            //    {
            //        var user = new ApplicationUser { UserName = item.employeeCode, isActive = 1, Email = item.emailAddress, userId = maxUserId, EmpCode = item.employeeCode, specialBranchUnitId = item.branchId, userTypeId = 9 };
            //        var result = await _userManager.CreateAsync(user, "123456");
            //        if (result.Succeeded)
            //        {
            //            var userinfo = await userInfoes.GetUserInfoByUser(item.employeeCode);

            //            await _userManager.AddToRoleAsync(user, "General User");
            //        }
            //    }

            //    var empuser = await personalInfoService.GetAllUsers();
            //    foreach (var item1 in empuser)
            //    {
            //        if (item1.UserName != "opus.admin")
            //        {
            //            var emp = await personalInfoService.GetEmployeeInfoByCode(item1.UserName);
            //            emp.ApplicationUserId = item1.Id;
            //            await personalInfoService.SaveEmployeeInfo(emp);
            //        }
            //    }
            //}




            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        public async Task<IActionResult> CheckUserLoginStatus()
        {
            string userName = HttpContext.User.Identity.Name;
            var data = await personalInfoService.CheckUserLoginCount(userName);
            return Json(data);
        }
        public async Task<IActionResult> GetLastPassChange()
        {
            string userName = HttpContext.User.Identity.Name;
            var data = await _userManager.FindByNameAsync(userName);
            return Json(data);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Loginnew(LogInViewModel model, string returnUrl = null)
        {

            try
            {

                ViewData["ReturnUrl"] = returnUrl;
                if (ModelState.IsValid)
                {

                    var userInfos = await userInfoes.GetUserInfoByUser(model.Name);
                    if (userInfos != null)
                    {
                        if (userInfos.isActive == 1)
                        {
                            try
                            {
                                personalInfoService.PopulateAcrNotify(userInfos.UserName);
                            }
                            catch (Exception)
                            {

                            }

                            var result = await _signInManager.PasswordSignInAsync(model.Name, model.Password, model.RememberMe, lockoutOnFailure: true);
                            if (result.Succeeded)
                            {
                                var ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                                var userAgent = Request.Headers["User-Agent"].ToString();
                                var mechineName = Environment.MachineName;
                                var rip = Dns.GetHostEntry(HttpContext.Connection.RemoteIpAddress.ToString()).ToString();

                                var mac = Securities.GetMacAddress();
                                var ips = Securities.GetIPAddress();
                                var pcname = Securities.GetPCName();



                                UserLogHistory userLog = new UserLogHistory
                                {
                                    userId = model.Name,
                                    logTime = DateTime.Now,
                                    status = 1,
                                    ipAddress = ips.Count() >= 1 ? ips[0] : null,
                                    pcName = mechineName,
                                    browserName = userAgent,
                                    mac = mac,
                                    ipAddress2 = ips.Count() >= 2 ? ips[1] : null,
                                    deviceName = pcname,
                                    Latitude = model.latitude,
                                    Longitude = model.longitude,
                                    Address = model.city
                                };

                                await dbChangeService.SaveUserLogHistory(userLog);

                                personalInfoService.UpdateOnlineStatus(model.Name, 1);

                                var empID = await personalInfoService.GetEmployeeIdByUsername(model.Name);
                                return LocalRedirect("~/HRPMSEmployee/formview/formview?id=" + empID);

                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                                return View(model);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "User is Inactive.");
                            return View(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return View(model);
                    }


                }
            }
            catch (Exception ex)
            {

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public IActionResult GetAllUserLogs(string empCode)
        {
			try
			{
				var data = dbChangeService.GetUserLogs(empCode);
				return Json(data);
			}
			catch (Exception ex)
			{
				return Json(ex.Message);
			}
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }
        public async Task<IActionResult> UpdateLoginStatus()
        {
            var username = HttpContext.User.Identity.Name;
            personalInfoService.UpdateOnlineStatus(username, 1);

            var data = await personalInfoService.GetAllUsersOnlineStatus();
            return Json(data.Where(x => x.OnlineStatus == "Online"));
        }

        public async Task<IActionResult> GetOnlineUsers()
        {
            var data = await personalInfoService.GetOnlineUsers();
            return Json(data);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var roles = await _roleManager.Roles.ToListAsync();
            List<ApplicationRoleViewModel> lstRole = new List<ApplicationRoleViewModel>();
            foreach (var data in roles)
            {
                ApplicationRoleViewModel modelr = new ApplicationRoleViewModel
                {
                    RoleId = data.Id,
                    RoleName = data.Name
                };
                lstRole.Add(modelr);
            }
            var model = new RegisterViewModel
            {
                userRoles = lstRole.OrderBy(x => x.RoleName),
            };
            return View(model);
        }
        public async Task<IActionResult> getRoleByUserId(string id)
        {
            return Json(await personalInfoService.GetRolesByUserId(id));
        }
        public async Task<IActionResult> getPagesByRoleName(string[] roles)
        {
            return Json(await personalInfoService.GetPagesByRoleNames(roles));
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                string username = HttpContext.User.Identity.Name;
                //var userinfo = await userInfoes.GetUserInfoByUser(username);
                var userinfo = await userInfoes.GetSbuIdByEmployeeEmail(model.Email);

                int maxUserId = await userInfoes.GetMaxUserId() + 1;
                var user = new ApplicationUser { UserName = model.Name, isActive = 1, Email = model.Email, userId = maxUserId, EmpCode = model.EmpCode, specialBranchUnitId = userinfo.specialBranchUnitId, userTypeId = 9, isPassChange = 0 };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (model.RoleId != "General User")
                    {
                        await _userManager.AddToRoleAsync(user, "General User");
                    }
                    await _userManager.AddToRoleAsync(user, model.RoleId);
                    IdentityUser temp = await _userManager.FindByNameAsync(model.Name);
                    var emp = await personalInfoService.GetEmployeeInfoByCode(model.EmpCode);
                    emp.ApplicationUserId = temp.Id;
                    await personalInfoService.SaveEmployeeInfo(emp);
                    //return RedirectToLocal(returnUrl);
                    return RedirectToAction(nameof(UserList));
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterModuleAdmin(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var roles = await _roleManager.Roles.ToListAsync();
            List<ApplicationRoleViewModel> lstRole = new List<ApplicationRoleViewModel>();
            foreach (var data in roles)
            {
                ApplicationRoleViewModel modelr = new ApplicationRoleViewModel
                {
                    RoleId = data.Id,
                    RoleName = data.Name
                };
                lstRole.Add(modelr);
            }
            var model = new RegisterViewModel
            {
                userRoles = lstRole,
            };
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> UpdateStatus(string Id, int status)
        {
            await userInfoes.UpdateAspNetUserByUserIdAndStatus(Id, status);
            return RedirectToAction(nameof(UserList));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
            string userName = HttpContext.User.Identity.Name;
            await userInfoes.DeleteAspNetUserByUserId(Id, userName);
            return RedirectToAction(nameof(UserList));
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterModuleAdmin(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                int maxUserId = await userInfoes.GetMaxUserId() + 1;
                var user = new ApplicationUser { UserName = model.Name, Email = model.Email, EmpCode = model.EmpCode, userId = maxUserId };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.RoleId);
                    IdentityUser temp = await _userManager.FindByNameAsync(model.Name);
                    return RedirectToLocal(returnUrl);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> UserRoleCreate()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            List<ApplicationRoleViewModel> lstRole = new List<ApplicationRoleViewModel>();
            foreach (var data in roles)
            {
                ApplicationRoleViewModel model = new ApplicationRoleViewModel
                {
                    RoleId = data.Id,
                    RoleName = data.Name
                };
                lstRole.Add(model);
            }
            ApplicationRoleViewModel viewModel = new ApplicationRoleViewModel
            {
                eRPModules = await userInfoes.GetAllERPModule(),
                roleViewModels = lstRole
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UserIdentityRoleCreate([FromForm] ApplicationRoleViewModel model)
        {
            var user = new ApplicationRole(model.RoleName);
            IdentityResult result = await _roleManager.CreateAsync(user);

            return RedirectToAction(nameof(UserRoleCreate));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> UserList()
        {
            //var roles = await _roleManager.Roles.ToListAsync();
            //List<ApplicationRoleViewModel> lstRole = new List<ApplicationRoleViewModel>();
            //foreach (var data in roles)
            //{
            //    ApplicationRoleViewModel modelr = new ApplicationRoleViewModel
            //    {
            //        RoleId = data.Id,
            //        RoleName = data.Name
            //    };
            //    if (data.Name== "admin"|| data.Name == "ERPAdmin")
            //    {

            //    }
            //    else
            //    {
            //        lstRole.Add(modelr);
            //    }
            //}

            UserListViewModel model = new UserListViewModel
            {
                aspNetUsersViewModels = await userInfoes.GetAllUserInfos(),
                //aspNetUsersViewModels = await userInfoes.GetUserInfoList(),
                userRoles = await userInfoes.GetAllRoles(),
            };
            return View(model);
        }

        public async Task<IActionResult> DeleteRoles(string id)
        {
            try
            {
                await userInfoes.DeleteRoleById(id);
                return Json("Success");
            }
            catch
            {
                return Json("Roles is Already Assigned Someone!!");
            }
        }


        public async Task<IActionResult> DeleteMenu(string id)
        {
            try
            {
                await userInfoes.DeleteMenuByRoleId(id);
                return RedirectToAction(nameof(UserRoleCreate));
            }
            catch
            {
                return RedirectToAction(nameof(UserRoleCreate));
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteUserList()
        {
            UserListViewModel model = new UserListViewModel
            {
                userBackUpViewModels = await userInfoes.GetUserBackupListWithEmp()
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EditRole([FromForm] ApplicationRoleViewModel model)
        {

            ApplicationUser user = await _userManager.FindByNameAsync(model.EuserName);
            //var oldRoleId = _userManager.GetUsersInRoleAsync(model.userName);
            //var oldRoleName = _roleManager.Roles.SingleOrDefault(r => r.Id == oldRoleId).Name;
            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles.ToArray());

            if (model.roleIdList != null)
            {
                for (int i = 0; i < model.roleIdList.Length; i++)
                {
                    if (!await _userManager.IsInRoleAsync(user, model.roleIdList[i]))
                    {
                        await _userManager.AddToRoleAsync(user, model.roleIdList[i]);
                    }
                }
                //await _userManager.RemoveFromRoleAsync(user, model.PreRoleId);
            }

            return RedirectToAction(nameof(UserList));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            var ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            //var userinofs = await userInfoes.GetUserInfoByUser(HttpContext.User.Identity.Name);
            //var employeeinfo = await personalInfoService.GetEmployeeInfoByApplicationId(userinofs?.aspnetId);
            UserLogHistory userLog = new UserLogHistory
            {
                userId = HttpContext.User.Identity.Name,
                logTime = DateTime.Now,
                status = 0,
                ipAddress = ip
            };
            await dbChangeService.SaveUserLogHistory(userLog);

            personalInfoService.UpdateOnlineStatus(HttpContext.User.Identity.Name, 0);
            //EmpAttendance empAttendance = new EmpAttendance
            //{
            //    punchCardNo = employeeinfo.employeeCode,
            //    startTime = DateTime.Now,
            //    summaryId = 1
            //};
            //await attendanceProcessService.SaveEmpAttendence(empAttendance);
            //_logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePsswordViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);

            string message = "Fail To Update Password";
            if (ModelState.IsValid)
            {
                var data = await _userManager.ChangePasswordAsync(await _userManager.FindByNameAsync(HttpContext.User.Identity.Name), model.OldPassword, model.Password);
                message = data.ToString();

                var userInfo = await personalInfoService.GetUserInfoByUserName(userName);
                userInfo.isPassChange = 1;
                userInfo.passChangeDate = DateTime.Now;
                await personalInfoService.saveUser(userInfo);

                var data1 = new UserLogHistory
                {
                    createdAt = DateTime.Now,
                    userId = user.UserName,
                    logTime = DateTime.Now,
                    status = 1
                };
                await personalInfoService.saveUserLogHistory(data1);
            }
            return RedirectToAction(nameof(HomeController.Index), "Home", new { Message = message });
        }

        public async Task<IActionResult> ChangePasswordNew(ChangePsswordViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);

            string message = "Fail To Update Password";
            if (ModelState.IsValid)
            {
                var data = await _userManager.ChangePasswordAsync(await _userManager.FindByNameAsync(HttpContext.User.Identity.Name), model.OldPassword, model.Password);
                message = data.ToString();
                if (data.Succeeded)
                {
                    var userInfo = await personalInfoService.GetUserInfoByUserName(userName);
                    userInfo.isPassChange = 1;
                    userInfo.passChangeDate = DateTime.Now;
                    await personalInfoService.saveUser(userInfo);

                    var data1 = new UserLogHistory
                    {
                        createdAt = DateTime.Now,
                        userId = user.UserName,
                        logTime = DateTime.Now,
                        status = 1
                    };
                    var st = await personalInfoService.saveUserLogHistory(data1);
                    if (st == 1)
                    {
                        await _signInManager.SignOutAsync();
                        var ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                        UserLogHistory userLog = new UserLogHistory
                        {
                            userId = HttpContext.User.Identity.Name,
                            logTime = DateTime.Now,
                            status = 0,
                            ipAddress = ip
                        };
                        await dbChangeService.SaveUserLogHistory(userLog);
                    }
                    return Json("ok");
                }
                else
                {
                    return Json("failed");
                }
            }
            else
            {
                return Json("failed");
            }

        }
		public async Task<IActionResult> AutoLogout(ChangePsswordViewModel model)
		{
			await _signInManager.SignOutAsync();
			var ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
			UserLogHistory userLog = new UserLogHistory
			{
				userId = HttpContext.User.Identity.Name,
				logTime = DateTime.Now,
				status = 0,
				ipAddress = ip
			};
			await dbChangeService.SaveUserLogHistory(userLog);

			personalInfoService.UpdateOnlineStatus(HttpContext.User.Identity.Name, 0);
			
			return RedirectToAction(nameof(AccountController.Loginnew));
		}			
		[HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassMailSent(string email)
        {
            var message = "";
            if (email != null)
            {
                var rand = new Random();
                var auto = rand.Next(123456, 954321);
                //check user
                var user = await userInfoes.GetUserByUserName(email);
                if (user != null)
                {
                    //Uncomment: Asad: 17.07.2023
                    await emailSenderService.SendEmailWithFrom(user.Email, "BDBL", "Your Password Reset Code is ", auto.ToString() + ". Don't share with anyone. This code is valid for 5 minutes.");
                    await userInfoes.StoreForgotPassCode(user.Email, user.UserName, auto.ToString(), DateTime.Now.AddMinutes(5));
                    message = "success";
                }
                else
                {
                    message = "failed";
                }
                //save email, code, expiretime, reqcount

            }
            return Json(message);
        }

        public async Task<IActionResult> OnlineUsers()
        {
            var data = await personalInfoService.GetAllUsersOnlineStatus();
            return View(data);
        }

        public async Task<IActionResult> OnlineUsersJson()
        {
            var data = await personalInfoService.GetAllUsersOnlineStatus();
            return Json(data);
        }

        [AllowAnonymous]
        public async Task<IActionResult> VarifyPassCode(string email, string code)
        {            
            var msg = "";
            var check = await userInfoes.GetPassResetCodeByEmail(email);
            if (check.recoverCode == code && check.codeExpire >= DateTime.Now)
            {
                msg = "success";
            }
            else
            {
                msg = "failed";
            }
            return Json(msg);
        }
        [AllowAnonymous]
        public async Task<IActionResult> SetNewPassword(string email, string code, string pass1, string pass2)
        {
            var msg = "";
            var check = await userInfoes.GetPassResetCodeByEmail(email);
            if (check.username == email && check.recoverCode == code)
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(check.email);
                if (pass1 == pass2)
                {
                    var result = await _userManager.RemovePasswordAsync(user);
                    var results = await _userManager.AddPasswordAsync(user, pass1);
                    if (results.Succeeded)
                    {
                        msg = "success";
                    }
                    else
                    {
                        msg = "failed";
                    }
                }
                else
                {
                    msg = "mismatch";
                }
            }
            return Json(msg);
        }

		[HttpGet]
		//[ValidateAntiForgeryToken]
		public async Task<ActionResult> ResetPassword1(ResetPasswordViewModel model)
		{
			//if (ModelState.IsValid)
			//{
			var userName = await personalInfoService.GetUserInfoByEmpCode(model.Name);
			//var userName = await userInfoes.GetemployeebyempCode(model.Name);
			ApplicationUser user = await _userManager.FindByNameAsync(userName.UserName);
			var result = await _userManager.RemovePasswordAsync(user);
			var results = await _userManager.AddPasswordAsync(user, model.Password);
			string filter = model.Name;
			if (results.Succeeded)
			{
				TempData["Success"] = "Password Changed Successfully!";
			}
			else
			{
				TempData["Success"] = "Password Reset Successful!";
				//AddErrors(results);
			}
			//}
			//return View(model);
			return RedirectToAction(nameof(AccountController.ResetPassword));
		}

		//    [HttpPost]
		//    [ValidateAntiForgeryToken]
		//    public async Task<ActionResult> ResetPassword1(ResetPasswordViewModel model)
		//    {
		//        if (ModelState.IsValid)
		//        {
		//            var userName = await personalInfoService.GetUserInfoByEmpCode(model.Name);
		//            //var userName = await userInfoes.GetemployeebyempCode(model.Name);
		//            ApplicationUser user = await _userManager.FindByNameAsync(userName.UserName);
		//            var result = await _userManager.RemovePasswordAsync(user);
		//            var results = await _userManager.AddPasswordAsync(user, model.Password);
		//            string filter = model.Name;
		//            if (results.Succeeded)
		//            {
		//                TempData["Success"] = "Password Reset Successful!";
		//            }
		//            else
		//            {
		//	TempData["Success"] = "Password Reset Successful!";
		//	AddErrors(results);
		//}
		//        }
		//        //return View(model);
		//        return RedirectToAction(nameof(AccountController.ResetPassword));
		//    }

		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userName = await personalInfoService.GetUserInfoByEmpCode(model.Name);
                //var userName = await userInfoes.GetemployeebyempCode(model.Name);
                ApplicationUser user = await _userManager.FindByNameAsync(userName.UserName);
                var result = await _userManager.RemovePasswordAsync(user);
                var results = await _userManager.AddPasswordAsync(user, model.Password);
                string filter = model.Name;
                if (results.Succeeded)
                {
                    TempData["Success"] = "Password Changed Successfully!";
                }
                else
                {
                    AddErrors(results);
                }
            }
            //return View(model);
            return RedirectToAction(nameof(AccountController.UserList));
        }

        [HttpGet]
        public async Task<IActionResult> UserProxyByAdmin()
        {
            string userName = HttpContext.User.Identity.Name;
            var roles = await _roleManager.Roles.ToListAsync();
            List<ApplicationRoleViewModel> lstRole = new List<ApplicationRoleViewModel>();
            foreach (var data in roles)
            {
                ApplicationRoleViewModel modelr = new ApplicationRoleViewModel
                {
                    RoleId = data.Id,
                    RoleName = data.Name
                };
                lstRole.Add(modelr);
            }
            UserListViewModel model = new UserListViewModel
            {
                userRoles = lstRole,
            };

            return View(model);
        }

        //[HttpPost]
        //public async Task<IActionResult> SwitchedUser(string userId, string securityCode)
        //{
        //    string userName = HttpContext.User.Identity.Name;
        //    string returnUrl = "/";
        //    ApplicationUser user = await _userManager.FindByNameAsync(userId);
        //    if (user != null && securityCode == "OPUS")
        //    {
        //        await _signInManager.SignOutAsync();
        //        await _signInManager.SignInAsync(user, isPersistent: false);

        //        return RedirectToLocal(returnUrl);
        //    }
        //    else
        //    {
        //        return RedirectToAction(nameof(UserProxyByAdmin));
        //    }
        //}

		public async Task<IActionResult> SomeAdminSettings()
		{
			return View();
		}

		[HttpPost]
		public IActionResult BackupDatabase()
		{
			var path = @"D:\AutoBackup\" + "db_BDBL_" + DateTime.Now.ToString() + ".bak";
			personalInfoService.BackupDatabaseCopy("db_BDBL", path);
			return RedirectToAction(nameof(SomeAdminSettings));
		}

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                //return Redirect(returnUrl);
                var userId = HttpContext.User.Identity.Name;
                if (userId == "sabbir@emergingrating.com")
                {
                    return RedirectToAction(nameof(HomeController.CrmDashboard), "Home");
                }
                else
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            else
            {
                var userId = HttpContext.User.Identity.Name;
                if (userId == "sabbir@emergingrating.com")
                {
                    return RedirectToAction(nameof(HomeController.CrmDashboard), "Home");
                }
                else
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
        }
        #endregion

        #region api
        [AllowAnonymous]
        [Route("api/Account/AllUserListByFiltering/{userRoleId}/{userName}")]
        [HttpGet]
        public async Task<IActionResult> AllUserListByFiltering(string userRoleId, string userName)
        {
            var result = await userInfoes.GetUserInfoListForProxyAdmin(userRoleId, userName);
            return Json(result);
        }

        [AllowAnonymous]
        [Route("api/Account/GetUserInfoList/")]
        [HttpGet]
        public async Task<IActionResult> GetUserInfoList()
        {
            var result = await userInfoes.GetUserInfoList();
            return Json(result);
        }

        public async Task<JsonResult> GetUserMenuAccessByRole(string roles)
        {
            try
            {
                var roleArr = roles.Split(',');
                var data = await pageAssignService.GetUserMenuAccessByRoles(roleArr);
                return Json(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Mobile api

        //Mobile api

        [HttpPost]
        [AllowAnonymous]
        [Route("global/api/AppsLogin")]
        public async Task<IActionResult> AppsLogin([FromBody] LogInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userinfo = await userInfoes.GetUserInfoByUser(model.Name);


                var result = await _signInManager.PasswordSignInAsync(model.Name, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    string code = userinfo.EmpCode;

                    var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(code);
                    return Json(userinfo);

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");

                    return Json("Invalid login attempt.");
                }
            }
            // If we got this far, something failed, redisplay form
            return Json(model);
        }

        public async Task<IActionResult> GetEmployeeInfoByCode(string code)
        {
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(code);
            return Json(EmpInfo);
        }

        #endregion



        #region Proxy_User
        public async Task<IActionResult> ProxyUserList(string rolename, string branchId)
        {
            UserListViewModel model = new UserListViewModel
            {
                aspNetUsersViewModels = await userInfoes.GetAllUserInfos(),
                userRoles = await userInfoes.GetAllUserRoles(),
                branches = await userInfoes.GetAllBranch(),
            };
            return View(model);
        }

        public async Task<IActionResult> GetProxyUserList(string rolename, string branchId)
        {
            try
            {
                var username = User.Identity.Name;
                var data = await userInfoes.GetAllUserInfosForProxyUser(username, rolename, branchId);
                return Json(data);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        [HttpPost]
        public async Task<IActionResult> SwitchedUser(UserListViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            string returnUrl = "/";
            ApplicationUser user = await _userManager.FindByNameAsync(model.userId);
            if (user != null && model.securityCode == "U.OPUS")
            {
                await _signInManager.SignOutAsync();
                await _signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToLocal(returnUrl);

            }
            else
            {
                return RedirectToAction(nameof(UserProxyByAdmin));
            }


        }
        #endregion
    }
}