using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.Payroll.Models;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.Payroll.Services.Salary.Interfaces;

namespace OPUSERP.Areas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PayrollApiController : ControllerBase
    {
        private readonly ISalaryService salaryService;
        private readonly IERPCompanyService eRPCompanyService;
        private readonly IPersonalInfoService personalInfoService;

        public PayrollApiController(ISalaryService salaryService, IERPCompanyService eRPCompanyService, IPersonalInfoService personalInfoService)
        {
            this.salaryService = salaryService;
            this.eRPCompanyService = eRPCompanyService;
            this.personalInfoService = personalInfoService;
        }

        [HttpGet("/api/global/GetAllSalaryPeriods")]
        public async Task<IActionResult> GetAllSalaryPeriods()
        {
            var data = await salaryService.GetAllSalaryPeriod();
            return Ok(data);
        }

        [HttpGet("/api/global/GetAllEmployeesList")]
        public async Task<IActionResult> AllEmployeeListApi()
        {
            var allEmployee = await personalInfoService.GetAllActiveDraftEmployeeInfo();
            return Ok(allEmployee);
        }

        [HttpGet("/api/global/IndividualStaffLoanApi/{empCode}/{loanId}/{startDate}")]
        public async Task<ActionResult> IndividualStaffLoanApi(string empCode, int loanId, DateTime? startDate)
        {
            var model = new PayrollReportViewModel
            {
                employeeInfoForLoan = await personalInfoService.GetEmployeeInfoByCodeForLoan(empCode),
                staffLoanTrans = await salaryService.GetStaffLoanTransactionsByDateRange(empCode, loanId, startDate)
            };

            return Ok(model);
        }

        [HttpGet("/api/global/GetLoansByEmployeeCodeApi/{empCode}")]
        public async Task<IActionResult> GetLoansByEmployeeCodeApi(string empCode)
        {
            var data = await salaryService.GetLoansByEmployeeCode(empCode);
            return Ok(data);
        }

        [HttpGet("/api/global/GetEmployeesAll")]
        public async Task<IActionResult> GetEmployeesAll()
        {
            var data = await salaryService.GetEmployeesAll();
            return Ok(data);
        }
        [HttpGet("/api/global/GetUserWiseEmployee/{username}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserWiseEmployee(string username)
        {
            var data = await salaryService.GetUserWiseEmployee(username);
            return Ok(data);
        }
		[HttpGet("/api/global/GetEmployeesforPFAll")]
		public async Task<IActionResult> GetEmployeesforPFAll()
		{
			var data = await salaryService.GetEmployeesforPFAll();
			return Ok(data);
		}
	}
}