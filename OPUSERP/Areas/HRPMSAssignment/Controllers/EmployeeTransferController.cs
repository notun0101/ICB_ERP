using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSAssignment.Helpers;
using OPUSERP.Areas.HRPMSAssignment.Models;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.HRPMS.Data.Entity.Assignment;
using OPUSERP.HRPMS.Services.Assignment.Interfaces;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;

namespace OPUSERP.Areas.HRPMSAssignment.Controllers
{
    [Area("HRPMSAssignment")]
    public class EmployeeTransferController : Controller
    {
        private readonly LangGenerate<EmployeeInfoLn> _lang;
        private readonly IPersonalInfoService personalInfoService;
        private readonly IDesignationDepartmentService designationDepartmentService;
        private readonly ITransferJoinService transferJoinService;
        private readonly IUserInfoes userInfoes;

        public EmployeeTransferController(IHostingEnvironment hostingEnvironment, IPersonalInfoService personalInfoService, IDesignationDepartmentService designationDepartmentService, ITransferJoinService transferJoinService, IUserInfoes userInfoes)
        {
            _lang = new LangGenerate<EmployeeInfoLn>(hostingEnvironment.ContentRootPath);
            this.personalInfoService = personalInfoService;
            this.designationDepartmentService = designationDepartmentService;
            this.transferJoinService = transferJoinService;
            this.userInfoes = userInfoes;
        }


        public async Task<IActionResult> Index()
        {
            string userName = HttpContext.User.Identity.Name;
            var userinfo = await userInfoes.GetUserInfoByUser(userName);
            var deptId = userinfo.departmentId;
            var model = new EmployeeReleaseViewModel
            {
                //assignments = await assignmentService.GetAssignmentByTypeAndEmpId(id, 1),
                //designations = await designationDepartmentService.GetDesignations(),
                departmentId = deptId,
                departments = await designationDepartmentService.GetDepartment()
            };
            return View(model);
        }

        // POST: EmployeeTransfer/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] EmployeeReleaseViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var userinfo = await userInfoes.GetUserInfoByUser(userName);
            var deptId = userinfo.departmentId;
            if (!ModelState.IsValid)
            {
                //ViewBag.employeeID = model.employeeId.ToString();
                //ViewBag.employeeName = model.employeeName;

                //fLang = _lang.PerseLang("Assignment/AssignmentEN.json"), 
                //model.assignments = await assignmentService.GetAssignmentByTypeAndEmpId(Convert.ToInt32(model.employeeId), 1);
                //model.designations = await designationDepartmentService.GetDesignations();
                model.departments = await designationDepartmentService.GetDepartment();

                return View(model);
            }

            EmployeeReleaseInfo data = new EmployeeReleaseInfo
            {
                Id = 0,
                employeeInfoId = Convert.ToInt32(model.employeeInfoId),
                employeeName = model.employeeName,
                designation = model.designation,
                fromDepartmentId = deptId,
                toDepartmentId = model.toDepartmentId,
                fromDepartment = "",
                toDepartment = "",
                transferOrderNo = model.transferOrderNo,
                releaseOrderNo = model.releaseOrderNo,
                transferOrderDate = model.trOrderDate,
                releaseOrderDate = model.releaseOrderDate,
                remarks = ""
            };

            int lstId = await transferJoinService.SaveEmployeeRelease(data);
            return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index), new { @id = model.employeeId });
        }

        public async Task<IActionResult> JoiningLetter()
        {
            string userName = HttpContext.User.Identity.Name;
            var userinfo = await userInfoes.GetUserInfoByUser(userName);
            var departmentId = userinfo.departmentId;
            var model = new EmployeeJoiningViewModel
            {
                //assignments = await assignmentService.GetAssignmentByTypeAndEmpId(id, 1),
                //designations = await designationDepartmentService.GetDesignations(),
                //departmentId = departmentId,
                departments = await designationDepartmentService.GetDepartment(),
                employeeReleaseInfos = await transferJoinService.GetAllReleasedEmployee(departmentId),
                employeeJoiningLetters = await transferJoinService.GetAllJoiningEmployee(departmentId)
            };
            return View(model);
        }

        // POST: EmployeeTransfer/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JoiningLetter([FromForm] EmployeeJoiningViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var userinfo = await userInfoes.GetUserInfoByUser(userName);
            var deptId = userinfo.departmentId;
            if (!ModelState.IsValid)
            {
                model.departments = await designationDepartmentService.GetDepartment();

                return View(model);
            }

            EmployeeJoiningLetter data = new EmployeeJoiningLetter
            {
                Id = 0,
                employeeReleaseInfoId = Convert.ToInt32(model.releaseInfoId),
                joinedEmpId = model.joinedEmpId,
                empName = model.empName,
                designation = model.designation,
                joingDepartment = model.joingDepartment,
                joingDepartmentId= model.joiningDepartmentId,
                joiningDate = model.joiningDate,
                joiningTime = model.joiningTime,
                remarks = ""
            };

            int lstId = await transferJoinService.SaveEmployeeJoining(data);
            return RedirectToAction(nameof(JoiningLetter));
            //return RedirectToAction(nameof(Index), new { @id = model.employeeId });
        }
    }
}