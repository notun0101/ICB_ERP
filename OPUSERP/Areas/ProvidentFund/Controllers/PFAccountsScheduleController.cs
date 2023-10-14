using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.Payroll.Models;
using OPUSERP.Areas.ProvidentFund.Models;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.Helpers;
using OPUSERP.Payroll.Services.PF.Interfaces;
using OPUSERP.Payroll.Services.Salary.Interfaces;
using OPUSERP.ProvidentFund.Data.Entity;
using OPUSERP.ProvidentFund.Service;
using OPUSERP.ProvidentFund.Service.Interface;

namespace OPUSERP.Areas.ProvidentFund.Controllers
{
    [Area("ProvidentFund")]
    public class PFAccountsScheduleController : Controller
    {
        private readonly IPFTransactionService pFTransactionService;
        private readonly ISalaryService salaryService;
        private readonly IERPCompanyService eRPCompanyService;
        private readonly IPFService pFService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly MyPDF myPDF;

        private readonly string rootPath;
        public PFAccountsScheduleController(IPFTransactionService pFTransactionService, IERPCompanyService eRPCompanyService, ISalaryService salaryService, IPFService pFService, IHostingEnvironment hostingEnvironment, IConverter converter)
        {
            this._hostingEnvironment = hostingEnvironment;
            this.pFTransactionService = pFTransactionService;
            this.salaryService = salaryService;
            this.pFService = pFService;
            this.eRPCompanyService = eRPCompanyService;
            myPDF = new MyPDF(hostingEnvironment, converter);
            rootPath = hostingEnvironment.ContentRootPath;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> PfAccountsScheduleCreate()
        {
            return View();
        }
        public async Task<ActionResult> PfAccountsScheduleList()
        {
            PFTransactionViewModel model = new PFTransactionViewModel
            {
                pfAccountsSchedules = await pFTransactionService.GetAllPfAccountsSchedule(),
                years = await salaryService.GetAllSalaryYear()
            };
            return View(model);
        }

        public async Task<ActionResult> PfAccountsSchedule()
        {
            PFTransactionViewModel model = new PFTransactionViewModel
            {
                pfAccountsSchedules = await pFTransactionService.GetAllPfAccountsSchedule(),
                years = await salaryService.GetAllSalaryYear()
            };
            return View(model);
        }


        public async Task<ActionResult> PfTransactionList()
        {
            PFTransactionViewModel model = new PFTransactionViewModel
            {
                pfTransactions = await pFTransactionService.GetAllPfTransaction()

            };
            return View(model);
        }
        public async Task<ActionResult> PfTransactionProcess()
        {
            PFTransactionViewModel model = new PFTransactionViewModel
            {
                years = await salaryService.GetAllSalaryYear()
            };
            return View(model);
        }

        public async Task<IActionResult> ProcessPfTransaction(int memberId, int yearId)
        {
            var pfAccount = await pFTransactionService.GetPfAccountsByMemberAndYearId(memberId, yearId);
            if (pfAccount != null)
            {
                await pFTransactionService.RemovePfAccountsById(pfAccount.Id);
            }

            var pfTransactions = await pFTransactionService.GetAllPfTransactionByMemberAndYearId(memberId, yearId);
            if (pfTransactions != null)
            {
                var model = new PfAccountsSchedule
                {
                    Id = 0,
                    pfMemberId = pfTransactions.FirstOrDefault().pfMemberId,
                    employeeId = pfTransactions.FirstOrDefault().employeeId,
                    year = pfTransactions?.FirstOrDefault().year.yearName,
                    january = pfTransactions?.Where(x => x.month == "January").Select(x => x.pfOwn + x.pfCompany).FirstOrDefault(),
                    february = pfTransactions?.Where(x => x.month == "February").Select(x => x.pfOwn + x.pfCompany).FirstOrDefault(),
                    march = pfTransactions?.Where(x => x.month == "March").Select(x => x.pfOwn + x.pfCompany).FirstOrDefault(),
                    april = pfTransactions?.Where(x => x.month == "April").Select(x => x.pfOwn + x.pfCompany).FirstOrDefault(),
                    may = pfTransactions?.Where(x => x.month == "May").Select(x => x.pfOwn + x.pfCompany).FirstOrDefault(),
                    june = pfTransactions?.Where(x => x.month == "June").Select(x => x.pfOwn + x.pfCompany).FirstOrDefault(),
                    july = pfTransactions?.Where(x => x.month == "July").Select(x => x.pfOwn + x.pfCompany).FirstOrDefault(),
                    august = pfTransactions?.Where(x => x.month == "August").Select(x => x.pfOwn + x.pfCompany).FirstOrDefault(),
                    september = pfTransactions?.Where(x => x.month == "September").Select(x => x.pfOwn + x.pfCompany).FirstOrDefault(),
                    october = pfTransactions?.Where(x => x.month == "October").Select(x => x.pfOwn + x.pfCompany).FirstOrDefault(),
                    november = pfTransactions?.Where(x => x.month == "November").Select(x => x.pfOwn + x.pfCompany).FirstOrDefault(),
                    december = pfTransactions?.Where(x => x.month == "December").Select(x => x.pfOwn + x.pfCompany).FirstOrDefault(),
                };
                if (model.january == null)
                {
                    model.january = 0;
                }
                if (model.february == null)
                {
                    model.february = 0;
                }
                if (model.march == null)
                {
                    model.march = 0;
                }
                if (model.april == null)
                {
                    model.april = 0;
                }
                if (model.may == null)
                {
                    model.may = 0;
                }
                if (model.june == null)
                {
                    model.june = 0;
                }
                if (model.july == null)
                {
                    model.july = 0;
                }
                if (model.august == null)
                {
                    model.august = 0;
                }
                if (model.september == null)
                {
                    model.september = 0;
                }
                if (model.october == null)
                {
                    model.october = 0;
                }
                if (model.november == null)
                {
                    model.november = 0;
                }
                if (model.december == null)
                {
                    model.december = 0;
                }

                model.total = model.january + model.february + model.march + model.april + model.may + model.june + model.july + model.august + model.september + model.october + model.november + model.december + model.interest;
                await pFTransactionService.SavePfAccountsSchedule(model);
            }
            return Json("ok");
        }

        public async Task<IActionResult> GetPFTransactionDetailsByPeriodId(int salaryPeriodId, int yearId)
        {
            var data = await pFTransactionService.GetTransactionDetailsByPeriodId(salaryPeriodId, yearId);
            return Json(data);
        }

        public async Task<IActionResult> ProcessPfTransactionNew(int salaryPeriodId, int yearId)
        {
            var members = await pFTransactionService.GetPfMembersByPeriodIdAndYearId(salaryPeriodId, yearId);
            foreach (var item in members)
            {
                var pfAccount = await pFTransactionService.GetPfAccountsByMemberAndYearId(item.Id, yearId);
                if (pfAccount != null)
                {
                    await pFTransactionService.RemovePfAccountsById(pfAccount.Id);
                }

                var pfTransactions = await pFTransactionService.GetAllPfTransactionByMemberAndYearId(item.Id, yearId);
                if (pfTransactions != null)
                {
                    var model = new PfAccountsSchedule
                    {
                        Id = 0,
                        pfMemberId = pfTransactions.FirstOrDefault().pfMemberId,
                        employeeId = pfTransactions.FirstOrDefault().employeeId,
                        year = pfTransactions?.FirstOrDefault().year.yearName,
                        january = pfTransactions?.Where(x => x.month == "January").Select(x => x.pfOwn + x.pfCompany).FirstOrDefault(),
                        januaryOwn = pfTransactions?.Where(x => x.month == "January").Select(x => x.pfOwn).FirstOrDefault(),
                        januaryBank = pfTransactions?.Where(x => x.month == "January").Select(x => x.pfCompany).FirstOrDefault(),
                        february = pfTransactions?.Where(x => x.month == "February").Select(x => x.pfOwn + x.pfCompany).FirstOrDefault(),
                        februaryOwn = pfTransactions?.Where(x => x.month == "February").Select(x => x.pfOwn).FirstOrDefault(),
                        februaryBank = pfTransactions?.Where(x => x.month == "February").Select(x => x.pfCompany).FirstOrDefault(),
                        march = pfTransactions?.Where(x => x.month == "March").Select(x => x.pfOwn + x.pfCompany).FirstOrDefault(),
                        marchOwn = pfTransactions?.Where(x => x.month == "March").Select(x => x.pfOwn).FirstOrDefault(),
                        marchBank = pfTransactions?.Where(x => x.month == "March").Select(x => x.pfCompany).FirstOrDefault(),
                        april = pfTransactions?.Where(x => x.month == "April").Select(x => x.pfOwn + x.pfCompany).FirstOrDefault(),
                        aprilOwn = pfTransactions?.Where(x => x.month == "April").Select(x => x.pfOwn).FirstOrDefault(),
                        aprilBank = pfTransactions?.Where(x => x.month == "April").Select(x => x.pfCompany).FirstOrDefault(),
                        may = pfTransactions?.Where(x => x.month == "May").Select(x => x.pfOwn + x.pfCompany).FirstOrDefault(),
                        mayOwn = pfTransactions?.Where(x => x.month == "May").Select(x => x.pfOwn).FirstOrDefault(),
                        mayBank = pfTransactions?.Where(x => x.month == "May").Select(x => x.pfCompany).FirstOrDefault(),
                        june = pfTransactions?.Where(x => x.month == "June").Select(x => x.pfOwn + x.pfCompany).FirstOrDefault(),
                        juneOwn = pfTransactions?.Where(x => x.month == "June").Select(x => x.pfOwn).FirstOrDefault(),
                        juneBank = pfTransactions?.Where(x => x.month == "June").Select(x => x.pfCompany).FirstOrDefault(),
                        july = pfTransactions?.Where(x => x.month == "July").Select(x => x.pfOwn + x.pfCompany).FirstOrDefault(),
                        julyOwn = pfTransactions?.Where(x => x.month == "July").Select(x => x.pfOwn).FirstOrDefault(),
                        julyBank = pfTransactions?.Where(x => x.month == "July").Select(x => x.pfCompany).FirstOrDefault(),
                        august = pfTransactions?.Where(x => x.month == "August").Select(x => x.pfOwn + x.pfCompany).FirstOrDefault(),
                        augustOwn = pfTransactions?.Where(x => x.month == "August").Select(x => x.pfOwn).FirstOrDefault(),
                        augustBank = pfTransactions?.Where(x => x.month == "August").Select(x => x.pfCompany).FirstOrDefault(),
                        september = pfTransactions?.Where(x => x.month == "September").Select(x => x.pfOwn + x.pfCompany).FirstOrDefault(),
                        septemberOwn = pfTransactions?.Where(x => x.month == "September").Select(x => x.pfOwn).FirstOrDefault(),
                        septemberBank = pfTransactions?.Where(x => x.month == "September").Select(x => x.pfCompany).FirstOrDefault(),
                        october = pfTransactions?.Where(x => x.month == "October").Select(x => x.pfOwn + x.pfCompany).FirstOrDefault(),
                        octoberOwn = pfTransactions?.Where(x => x.month == "October").Select(x => x.pfOwn).FirstOrDefault(),
                        octoberBank = pfTransactions?.Where(x => x.month == "October").Select(x => x.pfCompany).FirstOrDefault(),
                        november = pfTransactions?.Where(x => x.month == "November").Select(x => x.pfOwn + x.pfCompany).FirstOrDefault(),
                        novemberOwn = pfTransactions?.Where(x => x.month == "November").Select(x => x.pfOwn).FirstOrDefault(),
                        novemberBank = pfTransactions?.Where(x => x.month == "November").Select(x => x.pfCompany).FirstOrDefault(),
                        december = pfTransactions?.Where(x => x.month == "December").Select(x => x.pfOwn + x.pfCompany).FirstOrDefault(),
                        decemberOwn = pfTransactions?.Where(x => x.month == "December").Select(x => x.pfOwn).FirstOrDefault(),
                        decemberBank = pfTransactions?.Where(x => x.month == "December").Select(x => x.pfCompany).FirstOrDefault(),
                    };
                    if (model.january == null)
                    {
                        model.january = 0;
                    }
                    if (model.february == null)
                    {
                        model.february = 0;
                    }
                    if (model.march == null)
                    {
                        model.march = 0;
                    }
                    if (model.april == null)
                    {
                        model.april = 0;
                    }
                    if (model.may == null)
                    {
                        model.may = 0;
                    }
                    if (model.june == null)
                    {
                        model.june = 0;
                    }
                    if (model.july == null)
                    {
                        model.july = 0;
                    }
                    if (model.august == null)
                    {
                        model.august = 0;
                    }
                    if (model.september == null)
                    {
                        model.september = 0;
                    }
                    if (model.october == null)
                    {
                        model.october = 0;
                    }
                    if (model.november == null)
                    {
                        model.november = 0;
                    }
                    if (model.december == null)
                    {
                        model.december = 0;
                    }
                    if (model.januaryOwn == null)
                    {
                        model.januaryOwn = 0;
                    }
                    if (model.februaryOwn == null)
                    {
                        model.februaryOwn = 0;
                    }
                    if (model.marchOwn == null)
                    {
                        model.marchOwn = 0;
                    }
                    if (model.aprilOwn == null)
                    {
                        model.aprilOwn = 0;
                    }
                    if (model.mayOwn == null)
                    {
                        model.mayOwn = 0;
                    }
                    if (model.juneOwn == null)
                    {
                        model.juneOwn = 0;
                    }
                    if (model.julyOwn == null)
                    {
                        model.julyOwn = 0;
                    }
                    if (model.augustOwn == null)
                    {
                        model.augustOwn = 0;
                    }
                    if (model.septemberOwn == null)
                    {
                        model.septemberOwn = 0;
                    }
                    if (model.octoberOwn == null)
                    {
                        model.octoberOwn = 0;
                    }
                    if (model.novemberOwn == null)
                    {
                        model.novemberOwn = 0;
                    }
                    if (model.decemberOwn == null)
                    {
                        model.decemberOwn = 0;
                    }
                    if (model.januaryBank == null)
                    {
                        model.januaryBank = 0;
                    }
                    if (model.februaryBank == null)
                    {
                        model.februaryBank = 0;
                    }
                    if (model.marchBank == null)
                    {
                        model.marchBank = 0;
                    }
                    if (model.aprilBank == null)
                    {
                        model.aprilBank = 0;
                    }
                    if (model.mayBank == null)
                    {
                        model.mayBank = 0;
                    }
                    if (model.juneBank == null)
                    {
                        model.juneBank = 0;
                    }
                    if (model.julyBank == null)
                    {
                        model.julyBank = 0;
                    }
                    if (model.augustBank == null)
                    {
                        model.augustBank = 0;
                    }
                    if (model.septemberBank == null)
                    {
                        model.septemberBank = 0;
                    }
                    if (model.octoberBank == null)
                    {
                        model.octoberBank = 0;
                    }
                    if (model.novemberBank == null)
                    {
                        model.novemberBank = 0;
                    }
                    if (model.decemberBank == null)
                    {
                        model.decemberBank = 0;
                    }




                    model.total = model.january + model.february + model.march + model.april + model.may + model.june + model.july + model.august + model.september + model.october + model.november + model.december + model.interest;
                    await pFTransactionService.SavePfAccountsSchedule(model);
                }

                var salaryPeriod = await pFService.GetSalaryPeriodById(salaryPeriodId);
                salaryPeriod.isPfProcessed = 1;
                await salaryService.SaveSalaryPeriod(salaryPeriod);
            }

            return Json("ok");
        }

        public async Task<ActionResult> PfInterestDistribution()
        {
            PFTransactionViewModel model = new PFTransactionViewModel
            {
                years = await salaryService.GetAllSalaryYear(),
                salaryPeriods = await salaryService.GetAllSalaryPeriod(),
            };
            return View(model);
        }

        public async Task<IActionResult> CalculateInterest(string year, decimal interest)
        {
            var allPfAcs = await pFTransactionService.GetAllPfAccountsScheduleByYear(year);
            foreach (var item in allPfAcs)
            {
                await pFTransactionService.ProcessPfAccountInterestByEmp(year, Convert.ToInt32(item.pfMemberId), interest);
            }
            return Json("ok");
        }

        public async Task<IActionResult> GetMonthlyPfProcessByPeriodId(int id) //salary period id
        {
            var model = new List<PFProcessViewModel>();

            var employees = await pFTransactionService.GetEmployeesHaveSalaryStructure();

            foreach (var item in employees)
            {
                var loan = await pFService.GetPfLoanByEmpIdAndPeriodId(item.employee.Id, id);
                model.Add(new PFProcessViewModel
                {
                    employee = item.employee,
                    pfCode = item.pFMember.memberCode,
                    own = await pFService.GetPfHeadAmountByEmpIdAndHeadName1(item.employee.Id, id, "PF Own"),
                    company = await pFService.GetPfHeadAmountByEmpIdAndHeadName1(item.employee.Id, id, "PF Company"),
                    totalDeduction = loan == null ? 0 : loan.advanceAmount
                });
            }
            var model1 = new PFTransactionProcessViewModel
            {
                pfProcess = model,
                salaryPeriod = await pFService.GetSalaryPeriodById(id)
            };
            return Json(model1);

        }

        public async Task<ActionResult> PfProcess()
        {
            SalaryPeriodViewModel model = new SalaryPeriodViewModel
            {
                salaryPeriodsList = await salaryService.GetSalaryPeriodByLockLavel(6),

            };

            return View(model);
        }
        public async Task<IActionResult> TotalInterest(string year)
        {
            return Json(await pFService.GetInterestByYear(year));
        }

        public async Task<IActionResult> PfInterestCreate()
        {
            PFTransactionViewModel model = new PFTransactionViewModel
            {
                years = await salaryService.GetAllSalaryYear(),
                pfInterests = await pFService.GetAllPfInterest()
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> PfInterestCreate([FromForm] PFTransactionViewModel model)
        {
            try
            {

                PfInterest data = new PfInterest()
                {
                    Id = model.editId,
                    year = model.year,
                    investmentAmount = model.investmentAmount,
                    interestRate = model.interestRate,
                    total = model.total
                };
                await pFService.SavePFInterest(data);
                return RedirectToAction("PfInterestCreate");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



        public async Task<IActionResult> DeletePfInterestById(int id)
        {
            if (id > 0)
            {

                try
                {
                    await pFService.DeletePfInterestById(id);
                    return Json(new { Success = true, Message = "Deleted Successfully!" });
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Message = "Unable to delete! please try again.. ", exception = ex.Message });
                }
            }

            return Json("ok");
        }
        public async Task<ActionResult> PfForfeitAccount()
        {
            PfForfeitAccountViewModel model = new PfForfeitAccountViewModel
            {
                forfeitAccounts = await salaryService.GetAllForfeitAccount()
            };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> PfForfeitAccount([FromForm] PfForfeitAccountViewModel model)
        {

            var pfmember = await salaryService.GetPFMemberInfoByEmployeeId(Convert.ToInt32(model.employeeId));
            ForfeitAccount data = new ForfeitAccount
            {
                Id = model.Id,
                employeeId = model.employeeId,
                pfId = pfmember.Id,
                forfeitDate = model.forfeitDate,
                reason = model.reason,
                pfOwn = model.pfOwn,
                pfCompany = model.pfCompany,
                loan = model.loan,
                outstandingLoan = model.outstandingLoan,
                balance = model.balance,
                status = model.status,
                remarks = model.remarks,

            };
            await salaryService.SaveForfeitAccount(data);
            return RedirectToAction(nameof(PfForfeitAccount));
        }

        public async Task<IActionResult> DeleteForfeitAccount(int id)
        {

            if (id > 0)
            {

                try
                {
                    await salaryService.DeleteForfeitAccountById(id);
                    return Json(new { Success = true, Message = "Deleted Successfully!" });
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Message = "Unable to delete! please try again.. ", exception = ex.Message });
                }
            }

            return Json("ok");
        }
        public async Task<IActionResult> DeleteFinalSettleMent(int id)
        {

            if (id > 0)
            {

                try
                {
                    await salaryService.DeletepfFinalSettlementById(id);
                    return Json(new { Success = true, Message = "Deleted Successfully!" });
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Message = "Unable to delete! please try again.. ", exception = ex.Message });
                }
            }

            return Json("ok");
        }

        public async Task<ActionResult> PfFinalSettlement()
        {
            PfForfeitAccountViewModel model = new PfForfeitAccountViewModel
            {
                finalSettlements = await salaryService.GetAllpfFinalSettlement()
            };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> PfFinalSettlement([FromForm] PfForfeitAccountViewModel model)
        {
            var data = await pFService.GetPfAccountByMemberId(Convert.ToInt32(model.pfMemberId));

            if (data != null)
            {
                data.pfMemberId = model.pfMemberId;
                data.pfMemberCode = model.pfMemberCode;
                data.pfTotalOwn = model.pfTotalOwn;
                data.pfTotalBank = model.pfTotalBank;
                data.pfTotalInterest = model.pfTotalInterest;
                data.pfTotalLoan = model.pfTotalLoan;
                data.pfTotal = model.pfTotal;
                await salaryService.SavepfFinalSettleMent(data);
                return RedirectToAction(nameof(PfFinalSettlement));
            }
            else
            {
                data = new PfFinalSettlement
                {
                    Id = model.Id,
                    pfMemberId = model.pfMemberId,
                    pfMemberCode = model.pfMemberCode,
                    pfTotalOwn = model.pfTotalOwn,
                    pfTotalBank = model.pfTotalBank,
                    pfTotalInterest = model.pfTotalInterest,
                    pfTotalLoan = model.pfTotalLoan,
                    pfTotal = model.pfTotal

                };
                await salaryService.SavepfFinalSettleMent(data);
                return RedirectToAction(nameof(PfFinalSettlement));
            }


        }
        public async Task<IActionResult> StatementOfForfeitAccountView(int year1, int year2)
        {
            var years = new List<int>();
            var data = new List<ForfietAccountsByYear>();

            for (int i = year2; i >= year1; i--)
            {
                years.Add(i);
            }
            foreach (var item in years)
            {
                data.Add(new ForfietAccountsByYear
                {
                    year = item,
                    forfeitAccounts = await pFService.GetPfAccountsByYear(item)
                });
            }
            ViewBag.year1 = year1;
            ViewBag.year2 = year2;
            var model = new ForfeitAccountReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                forfietAccountsByYears = data
            };
            return View(model);

        }
        public IActionResult StatementOfForfeitAccountViewPdf(int year1, int year2)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string fileName;


            url = scheme + "://" + host + "/ProvidentFund/PFAccountsSchedule/StatementOfForfeitAccountView?year1=" + year1 + "&year2=" + year2;


            string status = myPDF.GeneratePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        public async Task<IActionResult> statementReport()
        {
            PFTransactionViewModel model = new PFTransactionViewModel
            {
                years = await salaryService.GetAllSalaryYear()

            };
            return View(model);
        }
        public async Task<IActionResult> PfReport()
        {
            PFTransactionViewModel model = new PFTransactionViewModel
            {
                years = await salaryService.GetAllSalaryYear()

            };
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> IndividualYearlyPfView(int pfMemberId, string year)
        {
            var pfMemberInfo = await pFService.GetPfMemberById(pfMemberId);
            var model = new PFLoanViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                pFMember = pfMemberInfo,
                pFYearlyIndividuals = await salaryService.GetIndividualYearlyPF(pfMemberId, year)
            };
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult IndividualYearlyPfPdf(int pfMemberId, string year)
        {
            string schema = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string filename;

            url = schema + "://" + host + "/ProvidentFund/PFAccountsSchedule/IndividualYearlyPfView?pfMemberId=" + pfMemberId + "&year=" + year;

            string status = myPDF.GenerateLandscapePDF(out filename, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        public async Task<IActionResult> GetFinalSettlementByMemberId(int memberId)
        {
            var account = await pFService.GetLastAccountScheduleByMemberId(memberId);
            var totalPfOwn = account.januaryOwn + account.februaryOwn + account.marchOwn + account.marchOwn + account.aprilOwn + account.mayOwn + account.juneOwn + account.julyOwn + account.augustOwn + account.septemberOwn + account.octoberOwn + account.novemberOwn + account.decemberOwn;
            var totalPfBank = account.januaryBank + account.februaryBank + account.marchBank + account.marchBank + account.aprilBank + account.mayBank + account.juneBank + account.julyBank + account.augustBank + account.septemberBank + account.octoberBank + account.novemberBank + account.decemberBank;
            var model = new FinalSettlementAcViewModel
            {
                memberId = Convert.ToInt32(account.pfMemberId),
                employeeId = Convert.ToInt32(account.employeeId),
                pfTotalOwn = totalPfOwn,
                pfTotalBank = totalPfBank,
                pfTotalInterest = account.interest,
                pfTotalLoan = account.loan,
                pfTotal = account.total
            };
            return Json(model);
        }

        //public async Task<IActionResult> TotalInterest(string year)
        //{
        //    return Json(await pFService.GetInterestByYear(year));
        //}
    }
}