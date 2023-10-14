using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.Payroll.Models;
using OPUSERP.Controllers;
using OPUSERP.CRM.Data.Entity.MasterData;
using OPUSERP.ERPServices.AuthService.Interfaces;
using OPUSERP.Payroll.Data.Entity.Salary;
using OPUSERP.Payroll.Services.Salary.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static OPUSERP.Areas.Payroll.Models.SalaryGradePercentViewModel;

namespace OPUSERP.Areas.Payroll.Controllers
{
    [Area("Payroll")]
    [Authorize]
    public class EmployeeLoanController : Controller
    {
        private readonly ISalaryService salaryService;
        private readonly INavbarService navbarService;

        public EmployeeLoanController(ISalaryService salaryService, INavbarService navbarService)
        {
            this.salaryService = salaryService;
            this.navbarService = navbarService;
        }



        #region Car Loan Policy
        public async Task<ActionResult> CarLoanPolicy()
        {
            var salary = await salaryService.GetAllSalaryHead();
            LoanPolicyViewModel model = new LoanPolicyViewModel
            {
                loanPolicies = await salaryService.GetAllLoanPolicy(),
                salaryGradesList = await salaryService.GetAllSalaryGrade(),
                salaryHeadsList = salary.Where(a => a.isLoan == "Yes"),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CarLoanPolicy([FromForm] LoanPolicyViewModel model)
        {
            var salary = await salaryService.GetAllSalaryHead();
            if (!ModelState.IsValid)
            {
                model.loanPolicies = await salaryService.GetAllLoanPolicy();
                model.salaryGradesList = await salaryService.GetAllSalaryGrade();
                model.salaryHeadsList = salary.Where(a => a.isLoan == "Yes");
                return View(model);
            }

            LoanPolicy data = new LoanPolicy
            {
                Id = model.editId,
                salaryHeadId = 32,
                salaryGradeId = model.salaryGradeId,
                loanPolicyName = model.loanPolicyName,
                maximumLoanAmount = model.maximumLoanAmount,
                loanInterestRate = model.loanInterestRate,
                loanNoOfInstallment = model.loanNoOfInstallment,
                isActive = model.isActive,
            };

            await salaryService.SaveCarLoanPolicy(data);
            return RedirectToAction(nameof(CarLoanPolicy));
        }

        public async Task<ActionResult> CarLoanPolicyNew()
        {
            var salary = await salaryService.GetAllSalaryHead();
            LoanPolicyViewModel model = new LoanPolicyViewModel
            {
                loanPolicyNews = await salaryService.GetAllLoanPolicyNew(),
              
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CarLoanPolicyNew([FromForm] LoanPolicyViewModel model)
        {
            //var salary = await salaryService.GetAllSalaryHead();
            //if (!ModelState.IsValid)
            //{
            //    model.loanPolicies = await salaryService.GetAllLoanPolicy();
            //    model.salaryGradesList = await salaryService.GetAllSalaryGrade();
            //    model.salaryHeadsList = salary.Where(a => a.isLoan == "Yes");
            //    return View(model);
            //}

            LoanPolicyNew data = new LoanPolicyNew
            {
                Id = model.editId,
                jobDuration = model.jobDuration,
                loanDuration = model.loanDuration,
                employeeInfoId = model.employeeInfoId,
                designationId = model.designationId,
                loanPolicyName = model.loanPolicyName,
                maximumLoanAmount = model.maximumLoanAmount,
                loanInterestRate = model.loanInterestRate,
                loanNoOfInstallment = model.loanNoOfInstallment,
                isActive = model.isActive,
            };

            var loanid = await salaryService.SaveCarLoanPolicyNew(data);

            if (model.desId != null)
            {
                if (loanid > 0)
                {
                    await salaryService.DeleteLoanPolicyDetailsByMasterId(loanid);
                    foreach (var item in model.desId)
                    {
                        var loanPolicyDetail = new LoanPolicyDetail
                        {
                            loanPolicyId = loanid,
                            designationId = item
                        };
                        await salaryService.SaveLoanPolicyDetail(loanPolicyDetail);
                    }
                }
            }
            
            return RedirectToAction(nameof(CarLoanPolicyNew));
        }

        #endregion

        public async Task<IActionResult> DeleteCarLoanPolicyById(int id)
        {
            var data = await salaryService.DeleteCarLoanPolicyById(id);
            if (data)
            {
                return Json("deleted");
            }
            else
            {
                return Json("failed");
            }
        }


        #region Home Policy
        public async Task<ActionResult> HomeLoanPolicy()
        {
            var salary = await salaryService.GetAllSalaryHead();
            LoanPolicyViewModel model = new LoanPolicyViewModel
            {
                loanPolicies = await salaryService.GetAllLoanPolicy(),
                salaryGradesList = await salaryService.GetAllSalaryGrade(),
                salaryHeadsList = salary.Where(a => a.isLoan == "Yes"),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HomeLoanPolicy([FromForm] LoanPolicyViewModel model)
        {
            var salary = await salaryService.GetAllSalaryHead();
            if (!ModelState.IsValid)
            {
                model.loanPolicies = await salaryService.GetAllLoanPolicy();
                model.salaryGradesList = await salaryService.GetAllSalaryGrade();
                model.salaryHeadsList = salary.Where(a => a.isLoan == "Yes");
                return View(model);
            }

            LoanPolicy data = new LoanPolicy
            {
                Id = model.editId,
                salaryHeadId = 33,
                salaryGradeId = model.salaryGradeId,
                loanPolicyName = model.loanPolicyName,
                maximumLoanAmount = model.maximumLoanAmount,
                loanInterestRate = model.loanInterestRate,
                loanNoOfInstallment = model.loanNoOfInstallment,
                isActive = model.isActive,
            };

            await salaryService.SaveHomeLoanPolicy(data);
            return RedirectToAction(nameof(HomeLoanPolicy));
        }
        [HttpPost]
        public async Task<JsonResult> DeleteHomeLoanPolicyById(int Id)
        {
            await salaryService.DeleteHomeLoanPolicyById(Id);
            return Json(true);
        }
        #endregion


        #region PFLoan Policy
        public async Task<ActionResult> PFLoanPolicy()
        {
            var salary = await salaryService.GetAllSalaryHead();
            LoanPolicyViewModel model = new LoanPolicyViewModel
            {
                loanPolicies = await salaryService.GetAllLoanPolicy(),
                salaryGradesList = await salaryService.GetAllSalaryGrade(),
                salaryHeadsList = salary.Where(a => a.isLoan == "Yes"),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PFLoanPolicy([FromForm] LoanPolicyViewModel model)
        {
            var salary = await salaryService.GetAllSalaryHead();
            if (!ModelState.IsValid)
            {
                model.loanPolicies = await salaryService.GetAllLoanPolicy();
                model.salaryGradesList = await salaryService.GetAllSalaryGrade();
                model.salaryHeadsList = salary.Where(a => a.isLoan == "Yes");
                return View(model);
            }

            LoanPolicy data = new LoanPolicy
            {
                Id = model.editId,
                salaryHeadId = 34,
                salaryGradeId = model.salaryGradeId,
                loanPolicyName = model.loanPolicyName,
                maximumLoanAmount = model.maximumLoanAmount,
                loanInterestRate = model.loanInterestRate,
                loanNoOfInstallment = model.loanNoOfInstallment,
                isActive = model.isActive,
            };

            await salaryService.SavePFLoanPolicy(data);
            return RedirectToAction(nameof(PFLoanPolicy));
        }

        [HttpPost]
        public async Task<JsonResult> DeletePFLoanPolicyById(int Id)
        {
            await salaryService.DeletePFLoanPolicyById(Id);
            return Json(true);
        }

        #endregion



        public async Task<ActionResult> LoanPolicyList()
        {
            var salary = await salaryService.GetAllSalaryHead();
            LoanPolicyViewModel model = new LoanPolicyViewModel
            {
                loanPolicies = await salaryService.GetAllLoanPolicy(),
                salaryGradesList = await salaryService.GetAllSalaryGrade(),
                salaryHeadsList = salary.Where(a => a.isLoan == "Yes"),
            };
            return View(model);
        }

        #region API Section
        [Route("global/api/SalaryMasterController/GetAllSalaryYear/")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllSalaryYear()
        {
            return Json(await salaryService.GetAllSalaryYear());
        }

		[Route("/api/global/GetBonusSubCategoryByCategoryMasterId/{id}")]
		public async Task<JsonResult> GetBonusSubCategoryByCategoryMasterId(int id)
		{
			var subLedgers = await salaryService.GetBonusSubCategoryByMasterId(id);
			return Json(subLedgers);
		}

		[Route("/api/global/GetBonusSubCategoryByMasterId/{id}")]
        public async Task<JsonResult> GetBonusSubCategoryByMasterId(int id)
        {
            var subLedgers = await salaryService.GetBonusSubCategoryByMasterId(id);
            return Json(subLedgers);
        }
        [Route("global/api/GetEmployeesBonusStructureByBonusId/{id}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetEmployeesBonusStructureByBonusId(int Id)
        {
            return Json(await salaryService.GetEmployeesBonusStructureByBonusId(Id));
        }
        #endregion

    }
}