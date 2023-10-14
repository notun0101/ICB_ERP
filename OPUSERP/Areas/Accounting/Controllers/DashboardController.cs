using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Accounting.Services.Voucher.Interfaces;
using OPUSERP.Areas.Accounting.Models;
using OPUSERP.Budget.Service.Interface;
using OPUSERP.ERPService.AuthService.Interfaces;

namespace OPUSERP.Areas.Accounting.Controllers
{
    [Area("Accounting")]
    public class DashboardController : Controller
    {
        private readonly IUserInfoes userInfo;
        private readonly IAccountingReportService accountingReportService;
        private readonly IBudgetRequsitionMasterService budgetRequsitionMasterService;
        private readonly IAccountingReportService reportingService;

        public DashboardController(IUserInfoes userInfo, IAccountingReportService accountingReportService, IBudgetRequsitionMasterService budgetRequsitionMasterService, IAccountingReportService reportingService)
        {
            this.userInfo = userInfo;
            this.accountingReportService = accountingReportService;
            this.budgetRequsitionMasterService = budgetRequsitionMasterService;
            this.reportingService = reportingService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new AccountingDashboardViewModel
            {
                fiscalYears = await budgetRequsitionMasterService.GetFiscalYear(),
            };
            return View(model);
        }


        public async Task<IActionResult> AccountGroupWiseVoucher(int id)
        {
            var user = await userInfo.GetAspnetuserByUser(User.Identity.Name);
            return Json(await accountingReportService.AccountGroupWiseVoucher((int)user.specialBranchUnitId, id));
        }


        public async Task<IActionResult> YearlyIncomeExpense(int id)
        {
            var user = await userInfo.GetAspnetuserByUser(User.Identity.Name);
            return Json(await accountingReportService.YearlyIncomeExpense((int)user.specialBranchUnitId, id));
        }


        public async Task<IActionResult> IncomeExpense(int id)
        {
            var user = await userInfo.GetAspnetuserByUser(User.Identity.Name);
            AccountingDashboardModel incomeex = await accountingReportService.YearlyIncomeExpense((int)user.specialBranchUnitId, id);
            
            var response = new
            {
                income = incomeex.monthWiseIncome.Sum(),
                expense = incomeex.monthWiseExpense.Sum(),
                balance = (incomeex.monthWiseIncome.Sum() - incomeex.monthWiseExpense.Sum())
            };

            return Json(response);
        }
        
        public async Task<IActionResult> BankAndCashBalance(int id)
        {
            var user = await userInfo.GetAspnetuserByUser(User.Identity.Name);
            return Json(await accountingReportService.BankAndCashBalance((int)user.specialBranchUnitId, id));
        }


        public async Task<IActionResult> RecepiptAndPayment(int id)
        {
            var user = await userInfo.GetAspnetuserByUser(User.Identity.Name);
            var data = await reportingService.GetRVMasterViewModels("01-Jan-2018",DateTime.Now.ToString("dd-MMM-yyyy"), user.specialBranchUnitId, 0,1);

            if (id != 0)
            {
                var year = await budgetRequsitionMasterService.GetFiscalYearByAlias(id);
                data = await reportingService.GetRVMasterViewModels(year.yearStartFrom.ToString(), year.yearEndOn.ToString(), user.specialBranchUnitId, 0,1);
            }
            

            decimal? totalopening = 0;
            decimal? totalsunone = 0;
            decimal? totalsuntwo = 0;
            decimal? totalclosing = 0;
            decimal? total1 = 0;
            decimal? total2 = 0;

            foreach (var value in data.FirstOrDefault().cashOpeningBalance.Where(x => x.noteHead == "OB"))
            {
                totalopening = totalopening + value.Amount;
            }
            total1 = total1 + totalopening;

            foreach (var value in data.FirstOrDefault().rVDetailViewModels.Where(x => x.fsLineName == "Received"))
            {

                totalsunone = totalsunone + value.Amount;
            }
            total1 = total1 + totalsunone;

            foreach (var value in data.FirstOrDefault().rVDetailViewModels.Where(x => x.fsLineName == "Payment"))
            {
                totalsuntwo = totalsuntwo + value.Amount;
            }

            total2 = total2 + totalsuntwo;

            foreach (var value in data.FirstOrDefault().cashOpeningBalance.Where(x => x.noteHead == "CB"))
            {
                totalclosing = totalclosing + value.Amount;
            }

            total2 = total2 + totalclosing;

            var response = new
            {
                Opening = totalopening,
                Receive = totalsunone,
                Payment = totalsuntwo,
                Closing = totalclosing,
            };

            return Json(response);

        }

    }
}