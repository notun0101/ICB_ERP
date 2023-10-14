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
using Microsoft.AspNetCore.Identity;
using OPUSERP.Data.Entity;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
    [Authorize]
    [Area("HRPMSEmployee")]
    public class TaxController : Controller
    {
        
        private readonly IPhotographService photographService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly ITaxService taxService;
        private readonly IAddressService addressService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public TaxController(IHostingEnvironment hostingEnvironment,IPhotographService photographService, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IPersonalInfoService personalInfoService, ITaxService taxService, IAddressService addressService)
        {
            
            this.personalInfoService = personalInfoService;
            this.photographService = photographService;
            this.taxService = taxService;
            this.addressService = addressService;
            _roleManager = roleManager;
            _userManager = userManager;

        }
        #region tax
        // GET: tax
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            var model = new TaxViewModel
            {
                employeeID = id.ToString(),
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                taxes = await taxService.GetTaxByEmpId(id),

            };
            if (model.tax == null) model.tax = new Tax();
            return View(model);
        }

        // POST: tax/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] TaxViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            if (!ModelState.IsValid)
            {
                ViewBag.employeeID = model.employeeID;
                model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(Int32.Parse(model.employeeID));
                model.taxes = await taxService.GetTaxByEmpId(Int32.Parse(model.employeeID));

                if (model.tax == null) model.tax = new Tax();
                return View(model);
            }

            Tax data = new Tax
            {
                Id = Int32.Parse(model.taxID),
                employeeId = Int32.Parse(model.employeeID),
                taxZone = model.taxZone,
                taxCircle = model.taxCircle,

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
            await taxService.SaveTax(data);

            await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));

            return RedirectToAction("Index", "Tax", new
            {
                id = model.employeeID
            });

        }

        // Delete: tax
        public async Task<IActionResult> Delete(int id, int empId)
        {
            await taxService.DeleteTaxById(id);

            return RedirectToAction("Index", "Tax", new
            {
                id = empId
            });
        }

        #endregion

        #region DuelResidence

        // GET: DuelResidence
        public async Task<IActionResult> DuelResidence(int id)
        {
            ViewBag.employeeID = id.ToString();
            var model = new DuelResidenceViewModel
            {
                employeeID = id.ToString(),
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                duelResidences = await taxService.GetDuelResidenceByEmpId(id),
                duelCountry = await addressService.GetAllContry(),

            };
            if (model.duelResidence == null) model.duelResidence = new DuelResidence();
            return View(model);
        }

        // POST: DuelResidence/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DuelResidence([FromForm] DuelResidenceViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            if (!ModelState.IsValid)
            {
                ViewBag.employeeID = model.employeeID;
                model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(Int32.Parse(model.employeeID));
                model.duelResidences = await taxService.GetDuelResidenceByEmpId(Int32.Parse(model.employeeID));
                model.duelCountry = await addressService.GetAllContry();

                if (model.duelResidence == null) model.duelResidence = new DuelResidence();
                return View(model);
            }

            DuelResidence data = new DuelResidence
            {
                Id = Int32.Parse(model.DuelResidenceID),
                employeeId = Int32.Parse(model.employeeID),
                duelCountryId = model.duelCountryId,
                duelPassport = model.duelPassport,

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
            await taxService.SaveDuelResidence(data);

            await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));

            return RedirectToAction("DuelResidence", "Tax", new
            {
                id = model.employeeID
            });

        }

        // Delete: DuelResidence
        public async Task<IActionResult> DeleteDuelResidence(int id, int empId)
        {
            await taxService.DeleteDuelResidenceById(id);

            return RedirectToAction("DuelResidence", "Tax", new
            {
                id = empId
            });
        }

        #endregion


    }
}