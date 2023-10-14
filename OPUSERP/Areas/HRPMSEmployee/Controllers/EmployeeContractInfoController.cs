using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Data.Entity;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Services.Employee.Interfaces;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
    [Authorize]
    [Area("HRPMSEmployee")]
    public class EmployeeContractInfoController : Controller
    {
        private readonly IEmployeeContratInfoService employeeContratInfoService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly IPhotographService photographService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public EmployeeContractInfoController(IEmployeeContratInfoService employeeContratInfoService, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IPhotographService photographService, IPersonalInfoService personalInfoService)
        {
            this.employeeContratInfoService = employeeContratInfoService;
            this.personalInfoService = personalInfoService;
            this.photographService = photographService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int id)
        {
            EmployeeContractInfoViewModel model = new EmployeeContractInfoViewModel
            {
                employeeID = id,
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                employeeContractInfos =await employeeContratInfoService.GetContractInfoByEmpId(id),
                employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id)
            };
            return View(model);
        }

        // POST: Contract/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] EmployeeContractInfoViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string fileName;
            if (model.attachmentUrl != null)
            {
                string message = FileSave.SaveImageNew(out fileName, model.attachmentUrl);
            }
            else
            {
                fileName = await employeeContratInfoService.GetEmployeeContractImgUrlById(Convert.ToInt32(model.contractId));
            };

            EmployeeContractInfo data = new EmployeeContractInfo
            {
                Id = Convert.ToInt32(model.contractId),
                employeeId = model.employeeID,
                contractStartDate = model.ContractStartDate,
                contractEndDate = model.ContractEndDate,
                contractStatus = model.contractStatus,
                remarks = model.remarks,
                attachmentUrl = fileName,
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
            await employeeContratInfoService.SaveContractInfo(data);
            await personalInfoService.UpdateEmployeeinfoById((int)model.employeeID);
            return RedirectToAction("Index", "EmployeeContractInfo", new
            {
                id = model.employeeID
            });
        }

        // Delete: Contract
        public async Task<IActionResult> Delete(int id, int empId)
        {
            await employeeContratInfoService.DeletContractInfoById(id);
            return RedirectToAction("Index", "EmployeeContractInfo", new
            {
                id = empId
            });
        }
    }
}