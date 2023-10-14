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
    public class EmployeeHobbyController : Controller
    {
        
       // private readonly IEmployeeHobbyService employeeHobbyService;
        private readonly ISpouseChildrenService spouseChildrenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IPersonalInfoService personalInfoService;
        private readonly IPhotographService photographService;

        public EmployeeHobbyController(IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ISpouseChildrenService spouseChildrenService, IPersonalInfoService personalInfoService, IPhotographService photographService)
        {
           
          //  this.employeeHobbyService = employeeHobbyService;
            this.spouseChildrenService = spouseChildrenService;
            this.personalInfoService = personalInfoService;
            this.photographService = photographService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

		// GET: Spouse
		public async Task<IActionResult> Index(int id)
		{
			ViewBag.empId = id;
            //return View();
            //  ViewBag.employeeID = id.ToString();
            var model = new EmployeeHobbyViewModel
            {
                employeeInfoId = id,
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                employeeHobbies = await personalInfoService.GetAllHobbiesByEmp(id)

            };
            return View(model);
        }

		[HttpPost]
		public async Task<IActionResult> SaveHobby(EmployeeHobbyViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            var count = model.hobbyName;

			foreach (var item in model.hobbyName)
			{
				var data = new EmployeeHobby
				{
                    Id = Int32.Parse(model.EmployeeHobbyID),
                    employeeInfoId = model.employeeInfoId,
					hobbyName = item,
					isActive = 1,
					status = 1,
					date = DateTime.Now
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
                //service for save
                await spouseChildrenService.SaveEmployeeHobby(data);
            }
          
            return Json("Saved");
        } 
       
    }
}