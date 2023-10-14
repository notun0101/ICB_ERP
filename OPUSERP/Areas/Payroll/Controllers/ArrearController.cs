using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.Payroll.Models;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.Payroll.Data;
using OPUSERP.Payroll.Data.Entity.Arrear;
using OPUSERP.Payroll.Data.Entity.Salary;
using OPUSERP.Payroll.Services.Salary.Interfaces;

namespace OPUSERP.Areas.Payroll.Controllers
{
    [Area("Payroll")]
    public class ArrearController : Controller
    {
        private readonly ISalaryService salaryService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly IERPCompanyService eRPCompanyService;
        private readonly IUserInfoes userInfo;
        private readonly IApprovalService approvalService;
        private readonly IArrearService arrearService;

        private readonly string rootPath;
        private readonly MyPDF myPDF;

        public ArrearController(ISalaryService salaryService, IArrearService arrearService, IPersonalInfoService personalInfoService, IERPCompanyService eRPCompanyService, IUserInfoes userInfo, IApprovalService approvalService, IHostingEnvironment hostingEnvironment, IConverter converter)
        {
            this.salaryService = salaryService;
            this.arrearService = arrearService;
            this.personalInfoService = personalInfoService;
            this.eRPCompanyService = eRPCompanyService;
            this.userInfo = userInfo;
            this.approvalService = approvalService;

            //For PDF
            myPDF = new MyPDF(hostingEnvironment, converter);
            rootPath = hostingEnvironment.ContentRootPath;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? employeeId)
        {
            ArrearViewModel model = new ArrearViewModel
            {
                employeeArrearList = await arrearService.GetAllArrearInfo(),
                salaryPeriods = await arrearService.GetSalaryPeriod(),
                employeesSalaryStructuresList = await salaryService.GetEmployeesSalaryStructureByEmpId(Convert.ToInt32(employeeId)),

            };
            return View(model);
        }

        public async Task<IActionResult> GetStructureByEmpId(int employeeId)
        {
            return Json( await salaryService.GetEmployeesSalaryStructureByEmpId(Convert.ToInt32(employeeId)));
        }
        [HttpPost]
        public async Task<IActionResult> Index([FromForm] ArrearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.employeeArrearList = await arrearService.GetAllArrearInfo();
                model.salaryPeriods = await arrearService.GetSalaryPeriod();

                return View(model);
            }

            EmployeeArrearInfo data = new EmployeeArrearInfo
            {
                Id = model.ArrearinfoId,
                employeeId = model.employeeId,
                currentBasic = model.currentBasic,
                oldBasic = model.oldBasic,
                periodId = model.periodId,
                //otherAddition = model.otherAddition,
                //otherDeduction = model.otherDeduction,
                //fromDate = model.fromDate,
                //toDate = model.toDate,
            };

            int masterId = await arrearService.SaveEmpArrearInfo(data);




            var employeeInfo = await personalInfoService.GetEmployeeInfoById(Convert.ToInt32(model.employeeId));


            IEnumerable<SalaryHead> lstSalaryHead = await salaryService.GetAllSalaryHead();

            var head = await arrearService.GetEmployeesArrearStructureByEmpId(Convert.ToInt32(model.employeeId));
            if (head.Count() > 0)
            {
                //await salaryService.SaveStructureHistory(Convert.ToInt32(model.employeeId), HttpContext.User.Identity.Name);
                await arrearService.DeleteEmployeesArrearStructureByempId(Convert.ToInt32(Convert.ToInt32(model.employeeId)));
            }

            foreach (var item in model.salaryStrustureList)
            {
                var isSalaryHead = lstSalaryHead.Where(x => x.Id == item.headId).FirstOrDefault();
                if (isSalaryHead != null)
                {

                    EmployeesArrearStructure data1 = new EmployeesArrearStructure
                    {
                        Id = 0,
                        employeeInfoId = Convert.ToInt32(model.employeeId),
                        arrearMasterId = masterId,
                        salarySlabId = Convert.ToInt32(model.salarySlab),
                        salaryHeadId = isSalaryHead.Id,
                        amount = Math.Ceiling((decimal)item.amount),
                        isActive = item.status,
                        effectiveDate = DateTime.Now
                    };
                    await arrearService.SaveEmployeesArrearStructure(data1);
                }
            }
            return Json("ok");
        }





        public async Task<JsonResult> DeleteArrearinfoById(int Id)
        {
            await arrearService.DeleteArrerById(Id);
            return Json(true);
        }
    }
}