using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.Data.Entity;
using Microsoft.AspNetCore.Identity;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
    [Authorize]
    [Area("HRPMSEmployee")]
    public class MobileBenifitController : Controller
    {
        private readonly IMobileBenifitService mobileBenifitService;
        private readonly IPhotographService photographService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public MobileBenifitController(IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IMobileBenifitService mobileBenifitService, IPhotographService photographService, IPersonalInfoService personalInfoService)
        {
            this.mobileBenifitService = mobileBenifitService;
            this.personalInfoService = personalInfoService;
            this.photographService = photographService;
            _roleManager = roleManager;
            _userManager = userManager;


        }

        // GET: MobileBenifit
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            try
            {
                var model = new MobileBenifitViewModel
                {
                    employeeId = id.ToString(),
                    photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                    employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                    mobileBenifits = await mobileBenifitService.GetMobileBenifitByEmpId(id),

                };
                //if (model.mobileBenifits == null) model.mobileBenifits = new MobileBenifit();
                return View(model);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        // POST: MobileBenifit/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] MobileBenifitViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            if (!ModelState.IsValid)
            {
                ViewBag.employeeID = model.employeeId;
                //model.photograph = await photographService.GetPhotographByEmpIdAndType(model.employeeId, "profile");
              //  model.employeeInfo = await personalInfoService.GetEmployeeInfoById(Int32.Parse(model.employeeId));
                model.mobileBenifits = await mobileBenifitService.GetMobileBenifitByEmpId(Int32.Parse(model.employeeId));
                return View(model);
            }
            MobileBenifit data = new MobileBenifit
            {
                Id = (Int32.Parse(model.MobileBenifitID)),
                employeeId = (Int32.Parse(model.employeeId)),
                type = model.type,
                amount = model.amount,
                date = model.date,
                status = 1,


            };
            if (roles.Contains("HRAdmin") || roles.Contains("admin"))
            {
               data.isDelete = 0;
            }
            else
            {
                data.isDelete = 1;
                //await employeeInfoService.ChangeEmployeeActivityStatus(model.employeeID, 3);
            }
            await mobileBenifitService.SaveMobileBenifit(data);              
            await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeId));
            return RedirectToAction("Index", "MobileBenifit", new
            {
                id = model.employeeId
            });

        }


        public async Task<IActionResult> Delete(int id, int empId)
        {
            await mobileBenifitService.DeleteMobileBenifitById(id);
            return RedirectToAction("Index", "MobileBenifit", new
            {
                id = empId
            });
        }

    }
}