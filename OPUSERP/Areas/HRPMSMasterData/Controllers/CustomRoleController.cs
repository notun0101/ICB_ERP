using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Areas.HRPMSMasterData.Models;
using OPUSERP.Areas.HRPMSMasterData.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OPUSERP.Areas.HRPMSMasterData.Controllers
{
    [Authorize]
    [Area("HRPMSMasterData")]
    public class CustomRoleController : Controller
    {
        private readonly ICustomRoleService customRoleService;

        public CustomRoleController(IHostingEnvironment hostingEnvironment, ICustomRoleService customRoleService)
        {
            this.customRoleService = customRoleService;
        }

        // GET: CustomRole
        public async Task<IActionResult> Index( )
        {

            CustomRoleViewModel model = new CustomRoleViewModel
            {
                
                customRoles = await customRoleService.GetCustomRole(),


            };

            return View(model);
        }

        // POST: CustomRole/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] CustomRoleViewModel model)
        {
            

            if (!ModelState.IsValid)
            {
                model.customRoles = await customRoleService.GetCustomRole();

                return View(model);
            }
            CustomRole data = new CustomRole
            {

               Id = model.CustomRoleId,
               roleName = model.roleName,
               remarks = model.remarks,

            };

            await customRoleService.SaveCustomRole(data);

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id )
        {
            await customRoleService.DeleteCustomRoleById(id);
            return RedirectToAction(nameof(Index));
        }
    }
}