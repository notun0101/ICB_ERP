using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
//using OPUSERP.Areas.HRPMSAssignment.Helpers;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.Data.Entity;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
    [Authorize]
    [Area("HRPMSEmployee")]
    public class TransferLogController : Controller
    {
        //private readonly LangGenerate<TransferLogLn> _lang;
        private readonly LangGenerate<TransferLogLn> _lang;
        private readonly ISalaryGradeService salaryGradeService;
        private readonly IPhotographService photographService;
        private readonly IServiceHistoryService serviceHistoryService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly IDesignationDepartmentService designationDepartmentService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly string rootPath;
        private readonly MyPDF myPDF;


        public TransferLogController(IHostingEnvironment hostingEnvironment, IConverter converter, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IPhotographService photographService, IDesignationDepartmentService designationDepartmentService, IPersonalInfoService personalInfoService, ISalaryGradeService salaryGradeService, IServiceHistoryService serviceHistoryService)
        {
            _lang = new LangGenerate<TransferLogLn>(hostingEnvironment.ContentRootPath);
            this.salaryGradeService = salaryGradeService;
            this.photographService = photographService;
            this.serviceHistoryService = serviceHistoryService;
            this.personalInfoService = personalInfoService;
            this.designationDepartmentService = designationDepartmentService;
            _roleManager = roleManager;
            _userManager = userManager;
            myPDF = new MyPDF(hostingEnvironment, converter);
            rootPath = hostingEnvironment.ContentRootPath;
        }

        // GET: Transfar
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            var model = new TransferLogViewModel
            {
                employeeID = id.ToString(),
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                fLang = _lang.PerseLang("Employee/TransferLogEN.json", "Employee/TransferLogBN.json", Request.Cookies["lang"]),
                salaryGrade = await salaryGradeService.GetAllSalaryGrade(),
                transferLogs = await serviceHistoryService.GetServiceHistoryByEmpId(id),
                designations = await designationDepartmentService.GetDesignations(),
                departments = await designationDepartmentService.GetDepartment(),
                employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id)
            };
            return View(model);
        }


        // POST: Transfar/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] TransferLogViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            //if (!ModelState.IsValid)
            //{
            //    ViewBag.employeeID = model.employeeID;
            //    model.fLang = _lang.PerseLang("Employee/TransferLogEN.json", "Employee/TransferLogBN.json", Request.Cookies["lang"]);
            //    model.designations = await designationDepartmentService.GetDesignations();
            //    model.departments = await designationDepartmentService.GetDepartment();
            //    model.salaryGrade = await salaryGradeService.GetAllSalaryGrade();
            //    model.transferLogs = await serviceHistoryService.GetServiceHistoryByEmpId(Convert.ToInt32(model.employeeID));


            //    return View(model);
            //}

            TransferLog data = new TransferLog
            {
                Id = Int32.Parse(model.transfarID),
                employeeId = Int32.Parse(model.employeeID),
                //workStation = model.workStation,
                departmentId = model.departmentId,
                designatioId = model.designationId,
                from = model.fromDate,
                to = model.toDate,
                salaryGradeId = model.grade,
                designation = model.designation
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
            await serviceHistoryService.SaveServiceHistory(data);
            await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));
            return RedirectToAction("Index", "TransferLog", new
            {
                id = Int32.Parse(model.employeeID)
            });
        }

        // Delete: Language
        public async Task<IActionResult> Delete(int id, int empId)
        {
            await serviceHistoryService.DeleteServiceHistoryById(id);
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "TransferLog", new
            {
                id = empId
            });
        }


        public async Task<IActionResult> TransferEmployee()
        {
            // ViewBag.employeeID = id.ToString();
            var model = new TransferLogViewModel
            {
                // employeeID = id.ToString(),
                //  photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                //  employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                fLang = _lang.PerseLang("Employee/TransferLogEN.json", "Employee/TransferLogBN.json", Request.Cookies["lang"]),
                salaryGrade = await salaryGradeService.GetAllSalaryGrade(),
                transferLogs = await serviceHistoryService.GetServiceHistory(),
                designations = await designationDepartmentService.GetDesignations(),
                departments = await designationDepartmentService.GetDepartment(),
                hrBranches = await designationDepartmentService.GetBranch(),
                hrDivisions = await designationDepartmentService.GetHrdivision(),

                //  employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TransferEmployee([FromForm] TransferLogViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            var fullDate = model.banglaDate + " " + model.banglaMonth + " " + model.banglaYear;

            TransferLog data = new TransferLog
            {
                Id = Int32.Parse(model.transfarID),
                employeeId = Int32.Parse(model.employeeID),

                departmentId = model.departmentId,
                designatioId = model.designationId,
                toBranchId = model.toBranchId,
                from = model.fromDate,
                to = model.toDate,
                //salaryGradeId = model.grade,
                designation = model.designation,
                reportDateBn = fullDate,
                reportDateEn = model.reportDateEn,
                status = 1

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
            await serviceHistoryService.SaveServiceHistory(data);

            var branch = new HrBranch();
            var department = new Department();
            var division = new HrDivision();

            var empPosting = await serviceHistoryService.GetEmpPostingPlaceByEmpId(Convert.ToInt32(model.employeeID));
            if (empPosting != null)
            {
                empPosting.endDate = Convert.ToDateTime(model.fromDate).AddDays(-1);
                await serviceHistoryService.SaveEmpPostingPlace(empPosting);

                var posting = new EmployeePostingPlace
                {
                    employeeId = Convert.ToInt32(model.employeeID),

                    startDate = model.fromDate,
                    status = 2
                };

                if (model.toBranchId != null)
                {
                    branch = await personalInfoService.GetHrBranchById(Convert.ToInt32(model.toBranchId));
                    posting.placeName = branch.branchName;
                    posting.placeNameBn = branch.branchNameBn;
                    posting.hrBranchId = Convert.ToInt32(model.toBranchId);
                }
                if (model.departmentId != null)
                {
                    department = await personalInfoService.GetDepartmentById(Convert.ToInt32(model.departmentId));
                    posting.placeName = department.deptName;
                    posting.placeNameBn = department.deptNameBn;
                    posting.departmentId = Convert.ToInt32(model.departmentId);
                }
                if (model.hrDivisionId != null)
                {
                    division = await personalInfoService.GetHrDivisionById(Convert.ToInt32(model.hrDivisionId));
                    posting.placeName = division.divName;
                    posting.placeNameBn = division.divNameBn;
                    posting.hrDivisionId = Convert.ToInt32(model.hrDivisionId);
                }

                await serviceHistoryService.SaveEmpPostingPlace(posting);

                var emp = await personalInfoService.GetEmployeeInfoById(Convert.ToInt32(model.employeeID));
                emp.lastTransferDate = model.fromDate;

                if (model.departmentId != null)
                {
                    emp.presentPosting = branch.branchName;
                    emp.hrBranchId = null;
                    emp.hrBranch = null;
                    emp.departmentId = Convert.ToInt32(model.departmentId);
                    emp.hrDivisionId = null;
                    emp.hrDivision = null;
                }
                if (model.hrDivisionId != null)
                {
                    emp.presentPosting = branch.branchName;
                    emp.hrBranchId = null;
                    emp.hrBranch = null;
                    emp.departmentId = null;
                    emp.department = null;
                    emp.hrDivisionId = Convert.ToInt32(model.hrDivisionId);
                }
                if (model.toBranchId != null)
                {
                    emp.presentPosting = branch.branchName;
                    emp.hrBranchId = Convert.ToInt32(model.toBranchId);
                    emp.departmentId = null;
                    emp.department = null;
                    emp.hrDivisionId = null;
                    emp.hrDivision = null;
                }
                await personalInfoService.SaveEmployeeInfo(emp);
            }
            else
            {
                var posting = new EmployeePostingPlace
                {
                    employeeId = Convert.ToInt32(model.employeeID),

                    startDate = model.fromDate,
                    status = 2
                };

                if (model.toBranchId != null)
                {
                    branch = await personalInfoService.GetHrBranchById(Convert.ToInt32(model.toBranchId));
                    posting.placeName = branch.branchName;
                    posting.placeNameBn = branch.branchNameBn;
                    posting.hrBranchId = Convert.ToInt32(model.toBranchId);
                }
                if (model.departmentId != null)
                {
                    department = await personalInfoService.GetDepartmentById(Convert.ToInt32(model.departmentId));
                    posting.placeName = department.deptName;
                    posting.placeNameBn = department.deptNameBn;
                    posting.departmentId = Convert.ToInt32(model.departmentId);
                }
                if (model.hrDivisionId != null)
                {
                    division = await personalInfoService.GetHrDivisionById(Convert.ToInt32(model.hrDivisionId));
                    posting.placeName = division.divName;
                    posting.placeNameBn = division.divNameBn;
                    posting.hrDivisionId = Convert.ToInt32(model.hrDivisionId);
                }

                await serviceHistoryService.SaveEmpPostingPlace(posting);

                var emp = await personalInfoService.GetEmployeeInfoById(Convert.ToInt32(model.employeeID));
                emp.lastTransferDate = model.fromDate;

                if (model.departmentId != null)
                {
                    emp.presentPosting = branch.branchName;
                    emp.hrBranchId = null;
                    emp.hrBranch = null;
                    emp.departmentId = Convert.ToInt32(model.departmentId);
                    emp.hrDivisionId = null;
                    emp.hrDivision = null;
                }
                if (model.hrDivisionId != null)
                {
                    emp.presentPosting = branch.branchName;
                    emp.hrBranchId = null;
                    emp.hrBranch = null;
                    emp.departmentId = null;
                    emp.department = null;
                    emp.hrDivisionId = Convert.ToInt32(model.hrDivisionId);
                }
                if (model.toBranchId != null)
                {
                    emp.presentPosting = branch.branchName;
                    emp.hrBranchId = Convert.ToInt32(model.hrDivisionId);
                    emp.departmentId = null;
                    emp.department = null;
                    emp.hrDivisionId = null;
                    emp.hrDivision = null;
                }
                await personalInfoService.SaveEmployeeInfo(emp);
            }


            //await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));
            return RedirectToAction(nameof(TransferEmployee));
        }




        public async Task<IActionResult> AppliedTransferEmpList()
        {
            // ViewBag.employeeID = id.ToString();
            var model = new TransferLogViewModel
            {

                fLang = _lang.PerseLang("Employee/TransferLogEN.json", "Employee/TransferLogBN.json", Request.Cookies["lang"]),
                salaryGrade = await salaryGradeService.GetAllSalaryGrade(),
                transferLogs = await serviceHistoryService.GetServiceHistory(),
                designations = await designationDepartmentService.GetDesignations(),
                departments = await designationDepartmentService.GetDepartment(),
                hrBranches = await designationDepartmentService.GetBranch(),
                //  employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id)
            };
            return View(model);
        }
        // GET: Photograph
        public async Task<IActionResult> transferAttachment(int id)
        {
            ViewBag.employeeID = id.ToString();
            TransferLogViewModel model = new TransferLogViewModel
            {
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                //transferUrl = await photographService.GetTransferAttachmentByEmpId(id),

            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> transferAttachment([FromForm] TransferLogViewModel model)
        {
            string fileName;

            if (model.transferattach != null)
            {
                string message = FileSave.SaveEmpAttachmentNew(out fileName, model.transferattach);

            }
            else
            {
                if (model.transfarID != null)
                {
                    fileName = await personalInfoService.GetTransferAttachmentUrlById(Int32.Parse(model.transfarID));
                }
                else
                {
                    fileName = "";
                }

            }
            var data = await serviceHistoryService.GetServiceHistoryById(Int32.Parse(model.transfarID));
            data.url = fileName;
            data.status = 2;


            //TransferLog data = new TransferLog
            //{
            //    Id = Int32.Parse(model.transfarID),
            //    employeeId = Int32.Parse(model.employeeID),
            //    url=model.url,
            //    status =2

            //};
            await photographService.SaveTransferAttachment(data);

            var posting = await serviceHistoryService.GetEmpPostingPlaceByEmpIdAndFromDate(data.employeeId, data.from);
            photographService.UpdatePostingPlace(data.employeeId, data.from);
            return RedirectToAction("AppliedTransferEmpList");
        }
        //niloy
        public async Task<IActionResult> TransferApplication()
        {
            var model = new TransferLogViewModel
            {
                // employeeID = id.ToString(),
                //  photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                //  employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                fLang = _lang.PerseLang("Employee/TransferLogEN.json", "Employee/TransferLogBN.json", Request.Cookies["lang"]),
                salaryGrade = await salaryGradeService.GetAllSalaryGrade(),
                transferLogs = await serviceHistoryService.GetServiceHistory(),
                designations = await designationDepartmentService.GetDesignations(),
                departments = await designationDepartmentService.GetDepartment(),
                hrBranches = await designationDepartmentService.GetBranch(),
                hrDivisions = await designationDepartmentService.GetHrdivision(),

                //  employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id)
            };
            return View(model);
        }

        [Route("global/api/GetActiveAllEmployeeInfoDepWise/{depId}/{branchId}/{divId}/{unitId}/{officeId}/{zoneId}/{empId}")]
        [HttpGet]
        public async Task<IActionResult> GetActiveAllEmployeeInfoDepWise(int? depId, int? branchId, int? divId, int? unitId, int? officeId, int? zoneId, int empId)
        {
            return Json(await personalInfoService.GetActiveAllEmployeeInfoDepWise(depId, branchId, divId, unitId, officeId, zoneId, empId));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TransferApplication([FromForm] TransferLogViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            var fullDate = model.banglaDate + " " + model.banglaMonth + " " + model.banglaYear;


            // var fileName = GetTransferPdf(Int32.Parse(model.employeeID));


            TransferLog data = new TransferLog
            {
                Id = Int32.Parse(model.transfarID),
                employeeId = Int32.Parse(model.employeeID),

                departmentId = model.departmentId,
                designatioId = model.designationId,
                toBranchId = model.toBranchId,
                from = model.fromDate,
                to = model.lastJoinDate,
                //to = model.toDate,
                //salaryGradeId = model.grade,
                designation = model.designation,
                reportDateBn = fullDate,
                reportDateEn = model.reportDateEn,
                substitutionEmp = model.substitutionEmp,
                orderDate = model.orderDate,
                orderNo = model.orderNo,
                releaseSignatory = model.releaseSignatory,
                status = 1,



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
            var TranferId = await serviceHistoryService.SaveTransferLog(data);
            var empSub = await serviceHistoryService.GetServiceHistoryById(TranferId);
            var fileName = GetTransferPdf(Int32.Parse(model.employeeID));
            empSub.url = fileName;
            await serviceHistoryService.SaveTransferLog(empSub);

            //var branch = new HrBranch();
            //var department = new Department();
            //var division = new HrDivision();

            //var empPosting = await serviceHistoryService.GetEmpPostingPlaceByEmpId(Convert.ToInt32(model.employeeID));
            //if (empPosting != null)
            //{
            //    empPosting.endDate = Convert.ToDateTime(model.fromDate).AddDays(-1);
            //    await serviceHistoryService.SaveEmpPostingPlace(empPosting);

            //    var posting = new EmployeePostingPlace
            //    {
            //        employeeId = Convert.ToInt32(model.employeeID),

            //        startDate = model.fromDate,
            //        status = 2
            //    };

            //    if (model.toBranchId != null)
            //    {
            //        branch = await personalInfoService.GetHrBranchById(Convert.ToInt32(model.toBranchId));
            //        posting.placeName = branch.branchName;
            //        posting.placeNameBn = branch.branchNameBn;
            //        posting.hrBranchId = Convert.ToInt32(model.toBranchId);
            //    }
            //    if (model.departmentId != null)
            //    {
            //        department = await personalInfoService.GetDepartmentById(Convert.ToInt32(model.departmentId));
            //        posting.placeName = department.deptName;
            //        posting.placeNameBn = department.deptNameBn;
            //        posting.departmentId = Convert.ToInt32(model.departmentId);
            //    }
            //    if (model.hrDivisionId != null)
            //    {
            //        division = await personalInfoService.GetHrDivisionById(Convert.ToInt32(model.hrDivisionId));
            //        posting.placeName = division.divName;
            //        posting.placeNameBn = division.divNameBn;
            //        posting.hrDivisionId = Convert.ToInt32(model.hrDivisionId);
            //    }

            //    await serviceHistoryService.SaveEmpPostingPlace(posting);

            //    var emp = await personalInfoService.GetEmployeeInfoById(Convert.ToInt32(model.employeeID));
            //    emp.lastTransferDate = model.fromDate;

            //    if (model.departmentId != null)
            //    {
            //        emp.presentPosting = branch.branchName;
            //        emp.hrBranchId = null;
            //        emp.hrBranch = null;
            //        emp.departmentId = Convert.ToInt32(model.departmentId);
            //        emp.hrDivisionId = null;
            //        emp.hrDivision = null;
            //    }
            //    if (model.hrDivisionId != null)
            //    {
            //        emp.presentPosting = branch.branchName;
            //        emp.hrBranchId = null;
            //        emp.hrBranch = null;
            //        emp.departmentId = null;
            //        emp.department = null;
            //        emp.hrDivisionId = Convert.ToInt32(model.hrDivisionId);
            //    }
            //    if (model.toBranchId != null)
            //    {
            //        emp.presentPosting = branch.branchName;
            //        emp.hrBranchId = Convert.ToInt32(model.toBranchId);
            //        emp.departmentId = null;
            //        emp.department = null;
            //        emp.hrDivisionId = null;
            //        emp.hrDivision = null;
            //    }
            //    await personalInfoService.SaveEmployeeInfo(emp);
            //}
            //else
            //{
            //    var posting = new EmployeePostingPlace
            //    {
            //        employeeId = Convert.ToInt32(model.employeeID),

            //        startDate = model.fromDate,
            //        status = 2
            //    };

            //    if (model.toBranchId != null)
            //    {
            //        branch = await personalInfoService.GetHrBranchById(Convert.ToInt32(model.toBranchId));
            //        posting.placeName = branch.branchName;
            //        posting.placeNameBn = branch.branchNameBn;
            //        posting.hrBranchId = Convert.ToInt32(model.toBranchId);
            //    }
            //    if (model.departmentId != null)
            //    {
            //        department = await personalInfoService.GetDepartmentById(Convert.ToInt32(model.departmentId));
            //        posting.placeName = department.deptName;
            //        posting.placeNameBn = department.deptNameBn;
            //        posting.departmentId = Convert.ToInt32(model.departmentId);
            //    }
            //    if (model.hrDivisionId != null)
            //    {
            //        division = await personalInfoService.GetHrDivisionById(Convert.ToInt32(model.hrDivisionId));
            //        posting.placeName = division.divName;
            //        posting.placeNameBn = division.divNameBn;
            //        posting.hrDivisionId = Convert.ToInt32(model.hrDivisionId);
            //    }

            //    await serviceHistoryService.SaveEmpPostingPlace(posting);

            //    var emp = await personalInfoService.GetEmployeeInfoById(Convert.ToInt32(model.employeeID));
            //    emp.lastTransferDate = model.fromDate;

            //    if (model.departmentId != null)
            //    {
            //        emp.presentPosting = branch.branchName;
            //        emp.hrBranchId = null;
            //        emp.hrBranch = null;
            //        emp.departmentId = Convert.ToInt32(model.departmentId);
            //        emp.hrDivisionId = null;
            //        emp.hrDivision = null;
            //    }
            //    if (model.hrDivisionId != null)
            //    {
            //        emp.presentPosting = branch.branchName;
            //        emp.hrBranchId = null;
            //        emp.hrBranch = null;
            //        emp.departmentId = null;
            //        emp.department = null;
            //        emp.hrDivisionId = Convert.ToInt32(model.hrDivisionId);
            //    }
            //    if (model.toBranchId != null)
            //    {
            //        emp.presentPosting = branch.branchName;
            //        emp.hrBranchId = Convert.ToInt32(model.hrDivisionId);
            //        emp.departmentId = null;
            //        emp.department = null;
            //        emp.hrDivisionId = null;
            //        emp.hrDivision = null;
            //    }
            //    await personalInfoService.SaveEmployeeInfo(emp);
            //}


            //await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));
            return RedirectToAction(nameof(TransferApplication));
        }
       





















        public async Task<IActionResult> PendingTransferApplication()
        {
            var model = new TransferLogViewModel
            {
                fLang = _lang.PerseLang("Employee/TransferLogEN.json", "Employee/TransferLogBN.json", Request.Cookies["lang"]),
                salaryGrade = await salaryGradeService.GetAllSalaryGrade(),
                transferLogs = await serviceHistoryService.GetServiceHistory(),
                designations = await designationDepartmentService.GetDesignations(),
                departments = await designationDepartmentService.GetDepartment(),
                hrBranches = await designationDepartmentService.GetBranch(),
                //  employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id)
            };
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> JoiningAcceptanceView(int ids, string signatory)
        {
            ViewBag.employeeID = ids.ToString();
            var model = new TransferLogViewModel
            {
                employeeInfo = await personalInfoService.GetEmployeeInfoById(ids),
                transferLog = await serviceHistoryService.GetTransferLogByEmpId(ids),
                joiningSignatory = await serviceHistoryService.GetJoiningSignatoryByTransferId(ids, signatory)
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateTransfer(int id, string signatory)
        {

            var emp = await serviceHistoryService.GetTransferLogById(id);

            emp.status = 5;
            var fileName = JoiningAcceptancePDF(emp.employeeId, signatory);
            emp.clearenceUrl = fileName;
            emp.signatoryCode = signatory;

            await serviceHistoryService.SaveServiceHistory(emp);

            var empPosting = await serviceHistoryService.GetEmpPostingPlaceByEmpId(emp.employeeId);
            if (empPosting != null)
            {
                empPosting.endDate = Convert.ToDateTime(emp.reportDateEn);
                await serviceHistoryService.SaveEmpPostingPlace(empPosting);
            }

            var posting = new EmployeePostingPlace
            {
                employeeId = Convert.ToInt32(emp.employeeId),
                startDate = emp.reportDateEn,
                status = 1
            };

            var branch = new HrBranch();
            var department = new Department();
            var division = new HrDivision();

            if (emp.toBranchId != null)
            {
                branch = await personalInfoService.GetHrBranchById(Convert.ToInt32(emp.toBranchId));
                posting.placeName = branch.branchName;
                posting.placeNameBn = branch.branchNameBn;
                posting.hrBranchId = Convert.ToInt32(emp.toBranchId);
            }
            if (emp.departmentId != null)
            {
                department = await personalInfoService.GetDepartmentById(Convert.ToInt32(emp.departmentId));
                posting.placeName = department.deptName;
                posting.placeNameBn = department.deptNameBn;
                posting.departmentId = Convert.ToInt32(emp.departmentId);
            }
            //if (emp.hrDivisionId != null)
            //{
            //    division = await personalInfoService.GetHrDivisionById(Convert.ToInt32(emp.hrDivisionId));
            //    posting.placeName = division.divName;
            //    posting.placeNameBn = division.divNameBn;
            //    posting.hrDivisionId = Convert.ToInt32(emp.hrDivisionId);
            //}

            var postId = await serviceHistoryService.SaveEmpPostingPlace(posting);

            var d = personalInfoService.UpdateEmployeePosting(Convert.ToInt32(posting.employeeId), 1, posting.departmentId, posting.locationId, posting.officeId, posting.hrDivisionId, posting.hrUnitId, posting.hrBranchId, postId);

            //var employee = await personalInfoService.GetEmployeeInfoById(emp.employeeId);
            //employee.lastTransferDate = emp.reportDateEn;

            //if (employee.departmentId != null)
            //{
            //    employee.presentPosting = branch.branchName;
            //    employee.hrBranchId = null;
            //    employee.hrBranch = null;
            //    employee.departmentId = Convert.ToInt32(emp.departmentId);
            //    employee.hrDivisionId = null;
            //    employee.hrDivision = null;
            //}
            //if (employee.hrDivisionId != null)
            //{
            //    employee.presentPosting = branch.branchName;
            //    employee.hrBranchId = null;
            //    employee.hrBranch = null;
            //    employee.departmentId = null;
            //    employee.department = null;
            //    employee.hrDivisionId = Convert.ToInt32(emp.hrDivisionId);
            //}
            //if (employee.hrBranchId != null)
            //{
            //    employee.presentPosting = branch.branchName;
            //    employee.hrBranchId = Convert.ToInt32(emp.toBranchId);
            //    employee.departmentId = null;
            //    employee.department = null;
            //    employee.hrDivisionId = null;
            //    employee.hrDivision = null;
            //}
            //await personalInfoService.SaveEmployeeInfo(employee);

            return RedirectToAction(nameof(PendingTransferApplication));

        }
        //[HttpPost]
        //public async Task<ActionResult> JoiningAcceptanceView([FromForm] TransferLogViewModel model)
        //{
        //    string fileName;

        //    if (model.Clearanceattach != null)
        //    {
        //        string message = FileSave.SaveEmpAttachmentNew(out fileName, model.Clearanceattach);

        //    }
        //    else
        //    {
        //        if (model.transfarID != null)
        //        {
        //            fileName = await personalInfoService.GetClearanceAttachmentUrlById(Int32.Parse(model.transfarID));
        //        }
        //        else
        //        {
        //            fileName = "";
        //        }

        //    }
        //    var data = await serviceHistoryService.GetServiceHistoryById(Int32.Parse(model.transfarID));
        //    data.clearenceUrl = fileName;
        //    data.status = 3;
        //    await photographService.SaveTransferAttachment(data);
        //    return RedirectToAction("TransferEmployee");
        //}
        //[AllowAnonymous]
        //public string JoiningAcceptancePDF(int ids)
        //{
        //    string scheme = Request.Scheme;
        //    var host = Request.Host;
        //    string fileName;

        //    string url = scheme + "://" + host + "/HRPMSEmployee/TransferLog/JoiningAcceptanceView?ids=" + ids;
        //    string status = myPDF.GeneratePDF1(out fileName, url);

        //    if (status != "done")
        //    {
        //        //return Content("<h1>Something Went Wrong</h1>");
        //        return status;
        //    }

        //    var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
        //    //return new FileStreamResult(stream, "application/pdf");
        //    return fileName;
        //}



        [AllowAnonymous]
        public string JoiningAcceptancePDF(int ids, string code)
        {

            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/HRPMSEmployee/TransferLog/JoiningAcceptanceView?ids=" + ids + "&signatory=" + code;

            string fileName;
            string status = myPDF.GeneratePDF(out fileName, url);
            if (status != "done")
            {
                return status;
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            //return new FileStreamResult(stream, "application/pdf");
            return fileName;

        }



        //GET Clearance Attachment
        public async Task<IActionResult> ClearanceAttachment(int id)
        {
            ViewBag.employeeID = id.ToString();
            TransferLogViewModel model = new TransferLogViewModel
            {
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),

            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ClearanceAttachment([FromForm] TransferLogViewModel model)
        {
            string fileName;

            if (model.Clearanceattach != null)
            {
                string message = FileSave.SaveEmpAttachmentNew(out fileName, model.Clearanceattach);

            }
            else
            {
                if (model.transfarID != null)
                {
                    fileName = await personalInfoService.GetClearanceAttachmentUrlById(Int32.Parse(model.transfarID));
                }
                else
                {
                    fileName = "";
                }

            }
            var data = await serviceHistoryService.GetServiceHistoryById(Int32.Parse(model.transfarID));
            data.clearenceUrl = fileName;
            data.status = 3;
            await photographService.SaveTransferAttachment(data);
            return RedirectToAction("TransferEmployee");
        }

        // Delete: transfer
        public async Task<IActionResult> TransferEmployeeDelete(int id)
        {
            await serviceHistoryService.DeleteServiceHistoryById(id);
            return RedirectToAction(nameof(TransferEmployee));
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetTransferPdfView(int ids)
        {
            var emp = await serviceHistoryService.GetTransferLogByEmpId1(ids);
            var code = emp.substitutionEmp;
            var SigCode = emp.releaseSignatory;
            var data = new TransferLogViewModel
            {
                employeeInfo = await personalInfoService.GetEmployeeInfoById(ids),
                employeeInfotransfer = await personalInfoService.GetEmployeeInfoByCodetranfer(code),
                employeeInfoSig = await personalInfoService.GetEmployeeInfoByCodetranfer(SigCode),
                //salaryGrade = await salaryGradeService.GetAllSalaryGrade(),
                //transferLogs = await serviceHistoryService.GetServiceHistory(),
                transferLog = await serviceHistoryService.GetTransferLogByEmpId(ids),
                designations = await designationDepartmentService.GetDesignations(),
                departments = await designationDepartmentService.GetDepartment()
            };
            return View(data);
        }


        [AllowAnonymous]
        public string GetTransferPdf(int ids)
        {

            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/HRPMSEmployee/TransferLog/GetTransferPdfView?ids=" + ids;

            string fileName;
            string status = myPDF.GeneratePDF(out fileName, url);
            if (status != "done")
            {
                return status;
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            //return new FileStreamResult(stream, "application/pdf");
            return fileName;

        }



    }
}