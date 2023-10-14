using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.CRM.Services.MasterData.Interfaces;
using OPUSERP.Data.Entity;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.Payroll.Services.Salary.Interfaces;
using System;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
    [Authorize]
    [Area("HRPMSEmployee")]
    public class BankingDiplomaController : Controller
    {
        private readonly LangGenerate<BankLn> _lang;
        private readonly IBankInfoService bankInfoService;
        private readonly IPhotographService photographService;
        private readonly IWagesBankInfoService wagesBankInfoService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly IWagesPersonalInfoService wagesPersonalInfoService;
        private readonly IBankBranchService bankBranchService;
        private readonly ISalaryService salaryService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public BankingDiplomaController(IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IPhotographService photographService, IPersonalInfoService personalInfoService, IBankInfoService bankInfoService, IWagesBankInfoService wagesBankInfoService, IWagesPersonalInfoService wagesPersonalInfoService, IBankBranchService bankBranchService, ISalaryService salaryService)
        {
            _lang = new LangGenerate<BankLn>(hostingEnvironment.ContentRootPath);
            this.bankInfoService = bankInfoService;
            this.photographService = photographService;
            this.personalInfoService = personalInfoService;
            this.wagesBankInfoService = wagesBankInfoService;
            this.wagesPersonalInfoService = wagesPersonalInfoService;
            this.bankBranchService = bankBranchService;
            this.salaryService = salaryService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        #region Employee Banking Diploma
        public async Task<IActionResult> Index(int id)
        {
           
            ViewBag.employeeID = id.ToString();
            var model = new BankingDiplomaViewModel
            {
                employeeID = id.ToString(),
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                bankingDiplomas = await bankInfoService.GetBankDiplomaInfoByEmpId(id),
                employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id),
               
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] BankingDiplomaViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            if (!ModelState.IsValid)
            {
                ViewBag.employeeID = model.employeeID;
                model.bankingDiplomas = await bankInfoService.GetBankDiplomaInfoByEmpId(Int32.Parse(model.employeeID));
                model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(Int32.Parse(model.employeeID));
                return View(model);
            }

            BankingDiploma data = new BankingDiploma
            {
                Id = model.bankDiplomaId,
                employeeId = Int32.Parse(model.employeeID),
                diplomaPart = model.diplomaPart,
                //diplomaName = model.diplomaName,
                passingYear=model.passingYear,
                //resultType=model.resultType,
                result=model.result,
                session=model.session
               
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
            await bankInfoService.SaveBankingDiplomaInfo(data);
            await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));
            return RedirectToAction("Index", "BankingDiploma", new
            {
                id = model.employeeID
            });
        }

        public async Task<IActionResult> Delete(int id, int empId)
        {
            await bankInfoService.DeleteBankDiplomaById(id);
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "BankingDiploma", new
            {
                id = empId
            });
        }

        #endregion

        
    }
}