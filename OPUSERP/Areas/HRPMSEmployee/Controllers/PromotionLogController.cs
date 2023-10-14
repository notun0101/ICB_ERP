using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using OPUSERP.Areas.HRPMSAssignment.Helpers;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using OPUSERP.Data.Entity;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
    [Authorize]
    [Area("HRPMSEmployee")]
    public class PromotionLogController : Controller
    {
        private readonly LangGenerate<PromotionLogLn> _lang;
        private readonly IPhotographService photographService;
        private readonly IPromotionLogService promotionLogService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly ISalaryGradeService salaryGradeService;
        private readonly IDesignationDepartmentService designationDepartmentService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public PromotionLogController(IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IPhotographService photographService, ISalaryGradeService salaryGradeService, IPersonalInfoService personalInfoService, IPromotionLogService promotionLogService, IDesignationDepartmentService designationDepartmentService)
        {
            _lang = new LangGenerate<PromotionLogLn>(hostingEnvironment.ContentRootPath);
            this.promotionLogService = promotionLogService;
            this.photographService = photographService;
            this.personalInfoService = personalInfoService;
            this.salaryGradeService = salaryGradeService;
            this.designationDepartmentService = designationDepartmentService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: PromotionLog
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            var data = await personalInfoService.GetEmployeeInfoById(id);
            string desigName = data.designation;
            PromotionLogViewModel model = new PromotionLogViewModel
            {
                employeeID = id.ToString(),
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                fLang = _lang.PerseLang("Employee/PromotionLogEN.json", "Employee/PromotionLogBN.json", Request.Cookies["lang"]),
                promotionLogs = await promotionLogService.GetPromotionLogByEmpId(id),
                employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id),
                salaryGrades = await salaryGradeService.GetAllSalaryGrade(),
                designations = await designationDepartmentService.GetDesignations(),
                designationName = await designationDepartmentService.GetDesignationIdByName(desigName),
            };

            return View(model);
        }

        // POST: PromotionLog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] PromotionLogViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            if (!ModelState.IsValid)
            {
                model.fLang = _lang.PerseLang("Employee/PromotionLogEN.json", "Employee/PromotionLogBN.json", Request.Cookies["lang"]);
                model.designations = await designationDepartmentService.GetDesignations();
                model.promotionLogs = await promotionLogService.GetPromotionLogByEmpId(Int32.Parse(model.employeeID));
                model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(Int32.Parse(model.employeeID));
                model.salaryGrades = await salaryGradeService.GetAllSalaryGrade();
                return View(model);
            }

            PromotionLog data = new PromotionLog
            {
                Id = Int32.Parse(model.promotionId),
                employeeId = Int32.Parse(model.employeeID),
                date = model.date,
                designationNewId = model.designationNewId,
                designationOldId = model.designationOldId,
                remark = model.remark,
                goNumber = model.goNumber,
                goDate = model.goDate,
                payScaleId = Int32.Parse(model.payScale)
                //nature = model.nature,
                //basic = model.basic,
                //rank = model.rank,

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
            await promotionLogService.SavePromotionLog(data);
      


            return RedirectToAction("Index", "PromotionLog", new
            {
                id = Int32.Parse(model.employeeID)
            });

        }

        // Delete: Language
        public async Task<IActionResult> Delete(int id, int empId)
        {
            await promotionLogService.DeletePromotionLogById(id);
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "PromotionLog", new
            {
                id = empId
            });
        }

        public async Task<IActionResult> PromotionEmployee()
        {
          //  ViewBag.employeeID = id.ToString();
          //  var data = await personalInfoService.GetEmployeeInfoById(id);
         //  string desigName = data.designation;
            PromotionLogViewModel model = new PromotionLogViewModel
            {
              //  employeeID = id.ToString(),
              //  photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
               // employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                fLang = _lang.PerseLang("Employee/PromotionLogEN.json", "Employee/PromotionLogBN.json", Request.Cookies["lang"]),
               // promotionLogs = await promotionLogService.GetPromotionLogByEmpId(id),
               // employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id),
                salaryGrades = await salaryGradeService.GetAllSalaryGrade(),
                designations = await designationDepartmentService.GetDesignations(),
                //  designationName = await designationDepartmentService.GetDesignationIdByName(desigName),
                 promotionLogs = await promotionLogService.GetPromotionLog(),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PromotionEmployee([FromForm] PromotionLogViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            if (!ModelState.IsValid)
            {
                model.fLang = _lang.PerseLang("Employee/PromotionLogEN.json", "Employee/PromotionLogBN.json", Request.Cookies["lang"]);
                model.designations = await designationDepartmentService.GetDesignations();
                model.promotionLogs = await promotionLogService.GetPromotionLogByEmpId(Int32.Parse(model.employeeID));
                model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(Int32.Parse(model.employeeID));
                model.salaryGrades = await salaryGradeService.GetAllSalaryGrade();
                return View(model);
            }

            //EmployeeInfo emp = new EmployeeInfo
            //{
            //    Id = Int32.Parse(model.employeeID),
            //    lastDesignationId = model.designationNewId,
            //    employeeCode = model.employeeCode
            //};

            //var b = await personalInfoService.SaveEmployeeInfo(emp);
            //await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));
            PromotionLog data = new PromotionLog
            {
                Id = Int32.Parse(model.promotionId),
                employeeId = Int32.Parse(model.employeeID),
               // employeeId = b,
                date = model.date,
                designationNewId = model.designationNewId,
               designationOldId = model.designationOldId,
                remark = model.remark,
                goNumber = model.goNumber,
                goDate = model.goDate,
                payScaleId = Int32.Parse(model.payScale)
                //nature = model.nature,
                //basic = model.basic,
                //rank = model.rank,

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
            await promotionLogService.SavePromotionLog(data);

            //var emp = await personalInfoService.GetEmployeeInfoByEmpId(Convert.ToInt32(model.employeeID));
            //emp.lastDesignationId = model.designationNewId;
            //var empId = await personalInfoService.SaveEmployeeInfo(emp);

            var promotion = await promotionLogService.GetLastPromotionByEmpId(Int32.Parse(model.employeeID));
            var employee = await personalInfoService.GetEmployeeInfoById(Int32.Parse(model.employeeID));
            employee.lastDesignationId = promotion.designationNewId;
            await personalInfoService.SaveEmployeeInfo(employee);
            //   await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));


            return RedirectToAction(nameof(PromotionEmployee));

        }


        public async Task<IActionResult> DeletePromotionEmployee(int id)
        {
            await promotionLogService.DeletePromotionLogById(id);
            return RedirectToAction(nameof(PromotionEmployee));
        }


        public IActionResult PromotionHistory()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetPromotionHistoryByEmpSearchApi(int id)
        {

            PromotionLogViewModel model = new PromotionLogViewModel
            {
                promotionLogs = await promotionLogService.GetPromotionHistoryByEmpId(id),
            };

            return Json(model);
        }

       
        //[HttpGet]
        //public async Task<IActionResult> GetPromotionHistoryByEmpSearchApi(int id)
        //{
        //    return Json(await promotionLogService.GetPromotionHistoryByEmpId(id));
        //}
    }
}