﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.Data.Entity;
using OPUSERP.Data.Entity.Auth;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.ERPServices.AuthService.Interfaces;
using OPUSERP.ERPServices.Interfaces;
using OPUSERP.ERPServices.MasterData.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Auth.Controllers
{
    [Area("Auth")]
    public class UserAssignPageController : Controller
    {
        private readonly IPageAssignService pageAssignService;
        private readonly IModuleAssignService moduleAssignService;
        private readonly IUserInfoes userInfoes;
        private readonly IERPModuleService eRPModuleService;
        private readonly INavbarService navbarService;

        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private ERPDbContext _db;

        public UserAssignPageController(IPageAssignService pageAssignService, INavbarService navbarService, IERPModuleService eRPModuleService, ERPDbContext db, IModuleAssignService moduleAssignService
            , IUserInfoes userInfoes, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.pageAssignService = pageAssignService;
            this.userInfoes = userInfoes;
            this.moduleAssignService = moduleAssignService;
            this.eRPModuleService = eRPModuleService;
            this.navbarService = navbarService;
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult IndexModule()
        {
            return View();
        }
        public IActionResult ModuleAssign()
        {
            return View();
        }
        public IActionResult ModuleAssignERP()
        {
            return View();
        }

        public async Task<JsonResult> GetUserMenuAccess(string userTypeId)
        {
            try
            {
                var data = await pageAssignService.GetUserMenuAccessByUserType(userTypeId);
                return Json(data);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<JsonResult> GetUserMenuAccessByModule(string userTypeId,int moduleId)
        {
            try
            {
                var data = await pageAssignService.GetUserMenuAccessByUserTypeByModule(userTypeId, moduleId);
                return Json(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<JsonResult> GetUserMenuAccessByUserTypeByMenu(string userTypeId,int moduleId)
        {
            try
            {
                var data = await pageAssignService.GetUserMenuAccessByUserTypeByMenu(userTypeId, moduleId);
                return Json(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<JsonResult> GetUserModuleAccess(string userTypeId)
        {
            try
            {
                var data = await moduleAssignService.GetModuleAccessPagesactive(userTypeId);
                return Json(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<JsonResult> GetUserModuleAccessERP(string userTypeId)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                var userId = _db.Users.Where(x => x.UserName == userName).Select(x => x.Id).FirstOrDefault();
                // var data = _context.Users.Where(x => x.UserName == userName).FirstOrDefaultAsync();
                List<string> roleids = _db.UserRoles.Where(x => x.UserId == userId).Select(x => x.RoleId).ToList();
                var datamodule = await moduleAssignService.GetModuleAccessPages();
               List<int?> datamoduleId = datamodule.Where(x => roleids.Contains(x.applicationRoleId)).Select(x=>x.eRPModuleId).ToList();
                var data = await moduleAssignService.GetModuleAccessPagesactive(userTypeId);
                return Json(data.Where(x=> datamoduleId.Contains(x.eRPModuleId)));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<JsonResult> GetUserMenuAccessModule(string userTypeId)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                var userId = _db.Users.Where(x => x.UserName == userName).Select(x => x.Id).FirstOrDefault();
                // var data = _context.Users.Where(x => x.UserName == userName).FirstOrDefaultAsync();
				string roleids = _db.UserRoles.Where(x => x.UserId == userId).Select(x=>x.RoleId).FirstOrDefault();
                var data = await pageAssignService.GetUserMenuAccessByUserTypeModule(userTypeId,roleids);
          
                return Json(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<JsonResult> SaveUserMenuAccess(string UserTypeIds, int[] AllMenuIds)
        {
            await pageAssignService.DeleteUserAccesspageByUserTypeId(UserTypeIds);
           
            foreach (var app in AllMenuIds)
            {
                UserAccessPage UAP = new UserAccessPage();
                UAP.Id = 0;
                UAP.applicationRoleId = UserTypeIds;
                UAP.isAccess = 1;
                UAP.navbarId = Convert.ToInt32(app);

                await pageAssignService.SaveUserAccessPage(UAP);
                UAP = new UserAccessPage();

            }


            return Json(new { result = "1" });
        }
        [HttpPost]
        public async Task<JsonResult> SaveUserModuleAccess(string UserTypeIds, int[] AllMenuIds)
        {
            await moduleAssignService.DeleteModuleAccesspageByUserTypeId(UserTypeIds);

            foreach (var app in AllMenuIds)
            {
                ModuleAccessPage UAP = new ModuleAccessPage();
                UAP.Id = 0;
                UAP.applicationRoleId = UserTypeIds;
          
                UAP.eRPModuleId = Convert.ToInt32(app);

                await moduleAssignService.SaveModuleAccessPage(UAP);
                UAP = new ModuleAccessPage();

            }


            return Json(new { result = "1" });
        }

        public async  Task<IActionResult> GetUserType()
        {
            string userName = HttpContext.User.Identity.Name;
            string[] adminRoles = { "0583d54e-74a8-46a3-b880-e13698723f69", "31a586e8-f6d8-498f-bc8c-42ed33c7182f"};
            var userId = _db.Users.Where(x => x.UserName == userName).Select(x => x.Id).FirstOrDefault();
            // var data = _context.Users.Where(x => x.UserName == userName).FirstOrDefaultAsync();
            List<string> roleids = _db.UserRoles.Where(x => x.UserId == userId).Select(x => x.RoleId).ToList();
            var roles = await _roleManager.Roles.Where(x=>!adminRoles.Contains(x.Id)).ToListAsync();
            var result = roles; //await userInfoes.GetUserTypeList();
            return Json(result);
        }

        public async Task<IActionResult> GetUserModule()
        {
            var result = await eRPModuleService.GetAllERPModule();
            return Json(result);
        }

        public async Task<IActionResult> GetUserMenus(int id)
        {
            var result = await navbarService.GetNavbarItemByModule(id);
            return Json(result);
        }

        public async Task<IActionResult> GetUserTypeERP()
        {
            string userName = HttpContext.User.Identity.Name;
            var userId = _db.Users.Where(x => x.UserName == userName).Select(x => x.Id).FirstOrDefault();
            // var data = _context.Users.Where(x => x.UserName == userName).FirstOrDefaultAsync();
            List<string> roleids = _db.UserRoles.Where(x => x.UserId == userId).Select(x => x.RoleId).ToList();

            var roles = await _roleManager.Roles.Where(x => !roleids.Contains(x.Id)&&x.Name!= "admin" && x.Name != "OPUSAdmin").ToListAsync();
            var result = roles; //await userInfoes.GetUserTypeList();
            return Json(result);
        }

        [Route("global/api/getAllUserAccessPages/{Id}")]
        [HttpGet]
        public async Task<IActionResult> getAllUserAccessPages(string Id)
        {
            return Json(await _db.UserAccessPages.Where(x=>x.applicationRoleId==Id).Include(x=>x.navbar).Include(x=>x.applicationRole).ToListAsync());
        }
    }
}