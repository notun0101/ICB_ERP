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
    public class EmployeeDeathRecordController : Controller
    {
        private readonly IPhotographService photographService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly IWagesPersonalInfoService wagesPersonalInfoService;
  
    
        public EmployeeDeathRecordController( IPhotographService photographService, IPersonalInfoService personalInfoService,  IWagesPersonalInfoService wagesPersonalInfoService)
        {
            this.photographService = photographService;
            this.personalInfoService = personalInfoService;
            this.wagesPersonalInfoService = wagesPersonalInfoService;
       
        }

        #region Employee Death Record
        public async Task<IActionResult> Index()
        {
            var model = new EmployeeDeathViewModel
            {
                //employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                employeeDeaths = await personalInfoService.GetDeathRecordOfEmployee(),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] EmployeeDeathViewModel model)
        {
            var data = new EmployeeDeath
            {
                Id = model.deathId,
                employeeInfoId = model.employeeInfoId,
                reason=model.reason,
                date=model.date
            };
            
            await personalInfoService.SaveEmpDeathRecord(data);
            var emp = await personalInfoService.GetEmployeeInfoById(model.employeeInfoId);
            emp.activityStatus = 0;
            await personalInfoService.SaveEmployeeInfo(emp);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await personalInfoService.DeleteEmpDeathRecordById(id);

            return RedirectToAction("Index");
        }

        #endregion

        
    }
}