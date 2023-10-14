using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using OPUSERP.HRPMS.Data.Entity.Master;
using Microsoft.AspNetCore.Identity;
using OPUSERP.Data.Entity;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
    [Authorize]
    [Area("HRPMSEmployee")]
    public class MembershipController : Controller
    {
        private readonly IMembershipService membershipService;
        private readonly IPhotographService photographService;
        private readonly IMembershipLanguageService membershipLanguageService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        // private readonly LangGenerate<Membership> _lang;

        public MembershipController(IMembershipService membershipService, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IPhotographService photographService, IPersonalInfoService personalInfoService, IHostingEnvironment hostingEnvironment, IMembershipLanguageService membershipLanguageService)
        {
            this.membershipService = membershipService;
            this.photographService = photographService;
            this.personalInfoService = personalInfoService;
            this.membershipLanguageService = membershipLanguageService;
            _roleManager = roleManager;
            _userManager = userManager;
            // _lang = new LangGenerate<Membership>(hostingEnvironment.ContentRootPath);
        }

        // GET: Membership
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            var model = new MembershipViewModel
            {
                employeeId = id,
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employee = await personalInfoService.GetEmployeeInfoById(id),
                memberships = await membershipLanguageService.GetMembershipInfo(),
                employeeMemberships = await membershipService.GetMembershipInfoByEmpId(id),
                employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id),
                membershipOrganization = await membershipLanguageService.GetMembershipOrganizationInfo()
                //  fLang = _lang.PerseLang("Employee/MembershipEN.json")
            };
            return View(model);
        }

		public async Task<IActionResult> CreateMembershipOrganization()
		{
			var model = new MembershipViewModel
			{
				membershipOrganization = await membershipLanguageService.GetMembershipOrganizationInfo()
			};
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> CreateMembershipOrganization(MembershipViewModel model)
		{
			var data = new MembershipOrganization
			{
				Id = Convert.ToInt32(model.membershipId),
				organizationName = model.organizationName,
				organizationNameBn = model.organizationNameBn,
				organizationType = model.organizationType,
			};
			await membershipService.SaveMembershipOrganization(data);
			return RedirectToAction(nameof(CreateMembershipOrganization));
		}

		public async Task<IActionResult> DeleteMembershipOrg(int id)
		{
			await membershipService.DeleteMembershipOrgById(id);
			return RedirectToAction(nameof(CreateMembershipOrganization));
		}

		// POST: Membership/Create
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] MembershipViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            if (!ModelState.IsValid)
            {
                ViewBag.employeeID = model.employeeId;
                model.memberships = await membershipLanguageService.GetMembershipInfo();
                model.employeeMemberships = await membershipService.GetMembershipInfoByEmpId(model.employeeId);
                model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(model.employeeId);
                model.membershipOrganization = await membershipLanguageService.GetMembershipOrganizationInfo();

               // model.fLang = _lang.PerseLang("Employee/MembershipEN.json");
                return View(model);
            }

            EmployeeMembership data = new EmployeeMembership
            {
                Id = model.employeeMembershipID,
                employeeId = model.employeeId,
                membershipId = Int32.Parse(model.nameOrganization),
                membershipNo = model.membershipNo,
                //membershipId = Int32.Parse(model.membershipType),
                remarks = model.remarks

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
            await membershipService.SaveMembershipInfo(data);
            await personalInfoService.UpdateEmployeeinfoById(model.employeeId);
            return RedirectToAction("Index", "Membership", new
            {
                id = (model.employeeId)
            });
        }

        public async Task<IActionResult> Delete(int id, int empId)
        {
            await membershipService.DeleteMembershipInfoById(id);
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "Membership", new
            {
                id = empId
            });
        }

        [HttpPost]
        public async Task<IActionResult> MembershipOrganization(MembershipViewModel model)
        {
            var data = new MembershipOrganization
            {
                Id = 0,
                organizationName = model.organizationName,
                organizationType = model.organizationType,


            };
            await membershipService.SaveMembershipOrganization(data);
            return Json("saved");
        }

        public async Task<IActionResult> GetMembershipOrganization()
        {
            return Json(await membershipService.GetMembershipOrganization());
        }

    }
}