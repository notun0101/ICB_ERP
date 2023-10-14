using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Accounting.Services.Voucher;
using OPUSERP.Areas.Accounting.Models;
using OPUSERP.Areas.Payroll.Models;
using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Payroll.Services.Fixation;
using OPUSERP.Payroll.Services.Fixation.Interfaces;
using OPUSERP.Payroll.Services.Salary.Interfaces;
using OPUSERP.Utility.Models;
using System;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Payroll.Controllers
{
    
    [Area("HR")]
    [Authorize]
    public class SignatoryController : Controller
    {

        //private readonly IFixationService fixationService;
        private readonly ISalaryService _salaryService;
        private readonly IFixationService _fixationService;
        private readonly UserManager<ApplicationUser> _userManager;
        public SignatoryController(ISalaryService salaryService, IFixationService fixationService, UserManager<ApplicationUser> userManager)
        {
            this._salaryService = salaryService;
            this._fixationService = fixationService;
            this._userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> CreateSignatory()
        {
            SignatoryViewModel model = new SignatoryViewModel
            {
                Designations = await _salaryService.GetAllDesignations(),
                SignatoryList = await _fixationService.GetAllSignatory()
            };
            //if (model.fixationIntegration == null) model.fixationIntegration = new FixationIntegration();
            return View(model);
        }

        public async Task<IActionResult> GetDesignationByEmployeeId(int employeeId)
        {
            var data = await _salaryService.GetDesignationByEmployeeId(employeeId);
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSignatory([FromForm] SignatoryViewModel model)
        {
            try
            {
                var entity = new Signatory
                {
                    Id = model.Id,
                    DesignationId = model.DesignationId,
                    EmployeeInfoId = model.EmployeeId
                };

                var isDuplicate = await _fixationService.IsDuplicateSignatory(entity);

                if (isDuplicate)
                {
					return new OkObjectResult(new ResponseObject { Status = "NOK", Message = "Duplicate Recoed Found." });
				}


				var id = await _fixationService.SaveSignatory(entity);

                //var masterId = await voucherService.UpdateVoucherNo(model.voucherMasterId, model.remarks);
                //return Json(masterId);
                return new OkObjectResult(new ResponseObject { Status = "OK", Message = "Saved Successfully" });
			}
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public async Task<IActionResult> GetSalaryAndGradeByEmpCode(string code)
        //{
        //    var emp = await personalInfoService.GetEmployeeInfoByCode(code);
        //    var grade = await fixationService.GetSalarygradeByEmpId(code);
        //    var jobDuration = (DateTime.Now.Date - Convert.ToDateTime(emp.joiningDateGovtService).Date).TotalDays / 365;
        //    var data = new SalaryGradeJoiningDateViewModel
        //    {
        //        employee = emp,
        //        gradeId = Convert.ToInt32(grade),
        //        jobDuration = Convert.ToDecimal(jobDuration)
        //    };
        //    return Json(data);
        //}

        //[HttpGet]
        //public async Task<IActionResult> CreatePromotion()
        //{
        //    FixationIntegrationViewModel model = new FixationIntegrationViewModel
        //    {
        //        fixationIntegrationList = await fixationService.GetAllFixationIntegration(),
        //        salaryGrades = await fixationService.GetSalarygrade(),
        //        designations = await personalInfoService.GetAllDesignation()
        //    };
        //    if (model.fixationIntegration == null) model.fixationIntegration = new FixationIntegration();
        //    return View(model);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([FromForm] FixationIntegrationViewModel model)
        //{
        //    var errors = ModelState.Select(x => x.Value.Errors)
        //                   .Where(y => y.Count > 0)
        //                   .ToList();

        //    if (!ModelState.IsValid)
        //    {

        //        model.fixationIntegrationList = await fixationService.GetAllFixationIntegration();
        //        return View(model);
        //    }

        //    var signatory = await fixationService.GetSignatoryByDesignationId((int)model.lastDesignationId);


        //    FixationIntegration data = new FixationIntegration
        //    {
        //        Id = model.fixationintegrationId,
        //        employeeId = model.employeeId,
        //        empCode = model.empCode,
        //        brachCode = model.hrBranchId,
        //        designation = model.designation,
        //        lastDesignationId = model.lastDesignationId,
        //        joiningDate = model.joiningDate,
        //        dateOfBirth = model.dateOfBirth,
        //        fixationType = model.fixationType,
        //        fixationGradeId = Int32.Parse(model.fixationGrade),
        //        currentGrade = model.currentGrade,
        //        amount = model.amount,
        //        refNo = model.refNo,
        //        postingPlace = model.postingPlace,
        //        letterDate = model.letterDate,
        //        fileNo = model.fileNo,
        //        effectiveDate = model.effectiveDate,
        //        lastIncrementDate = model.lastIncrementDate,
        //        lastPromotionDate = model.lastPromotionDate,
        //        lastPromotionDatePrev = model.lastPromotionDate,
        //        nextIncrementDate = model.nextIncrementDate,
        //        status = 1,
        //        categoryId = model.categoryId,
        //        newSignatory = signatory?.SignatoryId,
        //        newSignatoryName = signatory?.SignatoryName,
        //        newSignatoryDesig = signatory?.SignatoryDesignation,
        //        newSignatoryPhone = signatory?.SignatoryPhone

        //    };

        //    await fixationService.SaveFixationIntegration(data);

        //    //var employee = await personalInfoService.GetEmployeeInfoByCode(model.empCode);
        //    //employee.currentGradeId = Int32.Parse(model.fixationGrade);
        //    //employee.currentBasic = model.amount;
        //    //await personalInfoService.SaveEmployeeInfo(employee);

        //    if (model.fixationType == "Promotion")
        //    {
        //        return RedirectToAction("AllPendingPromotion");
        //    }
        //    else
        //    {
        //        return RedirectToAction("FixationIntegrationList");
        //    }
        //}

        //public async Task<IActionResult> AllPendingPromotion()
        //{
        //    var model = new FixationIntegrationViewModel
        //    {
        //        fixationIntegrationList = await fixationService.GetAllFixationIntegration(),
        //    };
        //    return View(model);
        //}

    }
}
