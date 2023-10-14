using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.CRM.Services.MasterData.Interfaces;
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
    public class EmployeeSportsController : Controller
    {
        private readonly LangGenerate<BankLn> _lang;
        private readonly IEmployeeSportsService EmployeeSportsService;
        private readonly IPhotographService photographService;
        private readonly IPersonalInfoService personalInfoService;
     

        public EmployeeSportsController(IHostingEnvironment hostingEnvironment, IPhotographService photographService, IPersonalInfoService personalInfoService, IEmployeeSportsService EmployeeSportsService)
        {
            _lang = new LangGenerate<BankLn>(hostingEnvironment.ContentRootPath);
            this.EmployeeSportsService = EmployeeSportsService;
            this.photographService = photographService;
            this.personalInfoService = personalInfoService;
           
        }

        #region Employee Sports
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            var model = new EmployeeSportsViewModel
            {
                employeeID = id,
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                employeeSports = await EmployeeSportsService.GetEmployeeSportsByEmpId(id),
               
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] EmployeeSportsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.employeeID = model.employeeID;
               model.employeeSports = await EmployeeSportsService.GetEmployeeSportsByEmpId(model.employeeID);
                return View(model);
            }

            EmployeeSports data = new EmployeeSports
            {
                Id = model.EmployeeSportsId,
                employeeId = model.employeeID,
                skillType = model.skillType,
                skillLevel = model.skillLevel,
            
            };

            await EmployeeSportsService.SaveEmployeeSports(data);
            await personalInfoService.UpdateEmployeeinfoById(model.employeeID);
            return RedirectToAction("Index", "EmployeeSports", new
            {
                id = model.employeeID
            });
        }

        public async Task<IActionResult> Delete(int id, int empId)
        {
            await EmployeeSportsService.DeleteEmployeeSportsById(id);
            return RedirectToAction("Index", "EmployeeSports", new
            {
                id = empId
            });
        }

        #endregion

      
    }
}