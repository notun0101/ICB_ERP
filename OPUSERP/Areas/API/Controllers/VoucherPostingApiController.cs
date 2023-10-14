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
using OPUSERP.Payroll.Services.Salary.Interfaces;

namespace OPUSERP.Areas.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VoucherPostingApiController : ControllerBase
    {
        private readonly ISalaryService salaryService;
        private readonly IERPCompanyService eRPCompanyService;

        public VoucherPostingApiController(ISalaryService salaryService, IERPCompanyService eRPCompanyService)
        {
            this.salaryService = salaryService;
            this.eRPCompanyService = eRPCompanyService;
        }


        [HttpGet("~/api/bdbl/SalaryVoucher/{year}/{month}/{sbu}/{type}")]
        public async Task<ActionResult> SalaryVoucherJson(int year, string month, string sbu, string type)
        {
            var salaryperiod = await salaryService.GetSalaryPeriodByYearAndMonth(year, month);
			var model = await salaryService.GetSalaryVoucherByPeriodIdAndSbu(salaryperiod.Id, sbu, type);

			return Ok(model);
        }

		[HttpGet("~/api/bdbl/SalarySheetByYearMonthAndSBU/{year}/{month}/{sbu}/{empId}")]
		public async Task<IActionResult> GetSalarySheetByYearMonthAndSBU(string year, string month, string sbu, string empId)
		{
			var data = await salaryService.sp_GetSalarySheetByYearMonthAndSBU(year, month, sbu.ToString(), empId.ToString());
			return Ok(data);
		}

		[HttpGet("~/api/bdbl/NetPayByYearMonthAndSBU/{year}/{month}/{sbu}/{empId}")]
		public async Task<IActionResult> NetPayByYearMonthAndSBU(string year, string month, string sbu, string empId)
		{
			var data = await salaryService.sp_GetNetPayByYearMonthAndSBU(year, month, sbu.ToString(), empId.ToString());
			return Ok(data);
		}

		[HttpGet("~/api/bdbl/PostSalaryVoucher/{periodId}/{branchId}")]
		public async Task<IActionResult> PostSalaryVoucherByBranch(int periodId, int branchId)
		{
			var data = await salaryService.sp_SaveVoucherDataByBranchId(periodId, branchId);
			return Ok(data);
		}


		[HttpGet("~/api/bdbl/GetPostedVoucher/{periodId}/{branchId}")]
		public async Task<IActionResult> GetPostedVoucher(int periodId, int branchId)
		{
			var data = await salaryService.sp_GetPostedVoucher(periodId, branchId);
			return Ok(data);
		}

		[HttpGet("~/api/bdbl/GetPostedVouchers/{periodId}/{branchId}")]
		public async Task<IActionResult> GetPostedVouchers(int periodId, int branchId)
		{
			var data = await salaryService.sp_GetPostedVoucher(periodId, branchId);
			var topOuter = new List<PostedVoucherViewModel>();

			foreach (var item in data.Select(x => x.hrBranchId).Distinct())
			{
				topOuter.Add(new PostedVoucherViewModel
				{
					hrBranchId = item,
					voucherMasterDetailVms = data.Where(x => x.hrBranchId == item).Select(x => new VoucherMasterDetailVm
					{
						MasterId = x.MasterId,
						DebitCode = x.DebitCode,
						DebitAmount = x.DebitAmount,
						PostingDate = x.PostingDate,
						salaryPeriodId = x.salaryPeriodId,
						status = x.status,
						uniqueId = x.uniqueId,
						voucherDetailsVms = data.Where(y => y.hrBranchId == item && y.MasterId == x.MasterId).Select(y => new VoucherDetailsVm {
							CreditCode = y.CreditCode,
							CreditAmount = y.CreditAmount
						})
					}).ToList()
				});
			}
			var model = new List<PostedVoucherViewModel>();
			foreach (var item in topOuter.Select(x => x.hrBranchId))
			{
				
			}
			return Ok(topOuter);
		}
	}
}