using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OPUSERP.Accounting.Services.AccountingSettings.Interfaces;
using OPUSERP.Accounting.Services.MasterData.Interfaces;
using OPUSERP.Accounting.Services.Voucher.Interfaces;
using OPUSERP.Areas.Accounting.Models;
using OPUSERP.Areas.Budget.Models;
using OPUSERP.Budget.Service.Interface;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.Payroll.Services.Salary.Interfaces;
using OPUSERP.SCM.Services.MasterData.Interfaces;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Accounting.Controllers
{
    [Authorize]
    [Area("Accounting")]
    public class AccountingReportController : Controller
    {
        private readonly IVoucherTypeService voucherTypeService;
        private readonly ILedgerService ledgerService;
        private readonly IAccountingReportService reportingService;
        private readonly ICostCentreService costCentreService;
        private readonly IERPCompanyService eRPCompanyService;
        private readonly ISpecialBranchUnitService specialBranchUnitService;
        private readonly IProjectService projectService;
        private readonly IUserInfoes userInfo;
        private readonly IBudgetRequsitionMasterService budgetRequsitionMasterService;
        private readonly ISalaryService salaryService;
        private readonly IVoucherService voucherService;

        private readonly string rootPath;
        private readonly MyPDF myPDF;
        public string FileName;

        public AccountingReportController(IVoucherTypeService voucherTypeService,
            IUserInfoes userInfo, IBudgetRequsitionMasterService budgetRequsitionMasterService,
            ISpecialBranchUnitService specialBranchUnitService,
            IProjectService projectService, ILedgerService ledgerService,
            IAccountingReportService reportingService, ICostCentreService costCentreService,
            IHostingEnvironment hostingEnvironment, IConverter converter,
            IERPCompanyService eRPCompanyService, ISalaryService salaryService, IVoucherService voucherService)
        {
            this.voucherTypeService = voucherTypeService;
            this.ledgerService = ledgerService;
            this.reportingService = reportingService;
            this.costCentreService = costCentreService;
            this.eRPCompanyService = eRPCompanyService;
            this.specialBranchUnitService = specialBranchUnitService;
            this.projectService = projectService;
            this.userInfo = userInfo;
            this.budgetRequsitionMasterService = budgetRequsitionMasterService;
            this.salaryService = salaryService;
            this.voucherService = voucherService;

            //For PDF
            myPDF = new MyPDF(hostingEnvironment, converter);
            rootPath = hostingEnvironment.ContentRootPath;
        }
        // GET: AccountingReport
        public ActionResult Index()
        {
            return View();
        }

        #region Cash Book

        public ActionResult CashBook()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult CashBookReportDataViewpdf(int ledgerId, int subledgerId, DateTime fromDate, DateTime toDate, int sbuId, int projectId)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/Accounting/AccountingReport/CashBookReportDataView?ledgerId=" + ledgerId + "&subledgerId=" + subledgerId + "&fromDate=" + fromDate + "&toDate=" + toDate + "&sbuId=" + sbuId + "&projectId=" + projectId;

            string fileName;
            string status = myPDF.GenerateLandscapePDFA4(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        #endregion

        #region Bank Book

        public ActionResult BankBook()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult BankBookReportDataViewpdf(int ledgerId, int subledgerId, DateTime fromDate, DateTime toDate, int sbuId, int projectId)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/Accounting/AccountingReport/BankBookReportDataView?ledgerId=" + ledgerId + "&subledgerId=" + subledgerId + "&fromDate=" + fromDate + "&toDate=" + toDate + "&sbuId=" + sbuId + "&projectId=" + projectId;

            string fileName;
            string status = myPDF.GenerateLandscapePDFA4(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        #endregion


        #region LedgerBook

        public ActionResult LedgerBook()
        {
            return View();
        }

        #endregion

        #region SubLedgerBook

        public ActionResult SubLedgerBook()
        {
            return View();
        }

        #endregion

        #region DayBook

        public ActionResult DayBook()
        {
            return View();
        }

        #endregion

        #region TrialBalance

        public ActionResult TrialBalance()
        {
            return View();
        }

        #endregion

        #region Financial Reports

        public ActionResult ProfitAndLossAccount()
        {
            return View();
        }
        public ActionResult BalanceSheet()
        {
            return View();
        }
        public ActionResult CFS()
        {
            return View();
        }
        public ActionResult CFSIndirect()
        {
            return View();
        }
        public ActionResult RV()
        {
            return View();
        }

        public ActionResult BudgetExpenseReport()
        {
            return View();
        }
        public async Task<ActionResult> BudgetExpenseReportP()
        {
            BudgetExpenseMasterViewModel model = new BudgetExpenseMasterViewModel
            {
                fiscalYears = await budgetRequsitionMasterService.GetFiscalYear()
            };

            return View(model);
        }
        #endregion

        #region DailyBillReceive

        public ActionResult DailyBillReceive()
        {

            return View();
        }

        #endregion

        #region CCWiseReport

        public ActionResult CcWiseReport()
        {
            return View();
        }

        #endregion

        #region LedgerDetails

        public ActionResult LedgerDetails()
        {
            return View();
        }
        public ActionResult ChartOAcc()
        {
            return View();
        }
        public ActionResult BudgetVsExpenditureView()
        {
            return View();
        }
        public ActionResult YearlyBudgetDetails()
        {
            return View();
        }





        #endregion



        [AllowAnonymous]
        public async Task<ActionResult> LedgerBookReportDataView(int ledgerId, int subledgerId, DateTime fromDate, DateTime toDate, int sbuId, int projectId)
        {
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            var SBU = await specialBranchUnitService.GetSpecialBranchUnitById(sbuId);
            var project = await projectService.GetProjectById(projectId);
            ViewBag.projectName = project?.projectName;
            ViewBag.SBUName = SBU?.branchUnitName;

            var model = await reportingService.GetLedgerBookReportData(ledgerId, subledgerId, fromDate, toDate, sbuId, projectId);
            return View(model);
        }



        [AllowAnonymous]
        public async Task<ActionResult> BudgetVsExpenditure(DateTime fromDate, DateTime toDate, int sbuId, int projectId)
        {
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            var SBU = await specialBranchUnitService.GetSpecialBranchUnitById(sbuId);
            var project = await projectService.GetProjectById(projectId);
            ViewBag.projectName = project?.projectName;
            ViewBag.SBUName = SBU?.branchUnitName;

            var model = await reportingService.GetBudgetExpenditureData(fromDate, toDate, sbuId, projectId);
            return View(model);
        }
        

        [AllowAnonymous]
        public async Task<ActionResult> LedgerBookReportView(int ledgerId, int subledgerId, DateTime fromDate, DateTime toDate, int sbuId, int projectId)
        {
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            var SBU = await specialBranchUnitService.GetSpecialBranchUnitById(sbuId);
            var project = await projectService.GetProjectById(projectId);
            ViewBag.projectName = project?.projectName;
            ViewBag.SBUName = SBU?.branchUnitName;

            var model = await reportingService.GetLedgerBookReportDataWithoutSP(ledgerId, subledgerId, fromDate, toDate, sbuId, projectId);
            return View(model);
        }


        [AllowAnonymous]
        public async Task<ActionResult> CashBookReportDataView(int ledgerId, int subledgerId, DateTime fromDate, DateTime toDate, int sbuId, int projectId)
        {
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            var model = await reportingService.GetCashLedgerBookReportData(ledgerId, subledgerId, fromDate, toDate, sbuId, projectId);
            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> BankBookReportDataView(int ledgerId, int subledgerId, DateTime fromDate, DateTime toDate, int sbuId, int projectId)
        {
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            var model = await reportingService.GetBankLedgerBookReportData(ledgerId, subledgerId, fromDate, toDate, sbuId, projectId);
            return View(model);
        }


        [AllowAnonymous]
        public async Task<IActionResult> LedgerDetailsDataView(int ledgerId, DateTime fromDate, DateTime toDate)
        {
            var model = await reportingService.LedgerDetailsViewModels(ledgerId, fromDate, toDate);
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult LedgerBookReportDataViewpdf(int ledgerId, int subledgerId, DateTime fromDate, DateTime toDate, int sbuId, int projectId)
        {
            string scheme = Request.Scheme;

            //var host = "localhost:61333";
            var host = Request.Host;

            string url = scheme + "://" + host + "/Accounting/AccountingReport/LedgerBookReportView?ledgerId=" + ledgerId + "&subledgerId=" + subledgerId + "&fromDate=" + fromDate + "&toDate=" + toDate + "&sbuId=" + sbuId + "&projectId=" + projectId;

            string fileName;
            string status = myPDF.GenerateLandscapePDFA4(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }


        [AllowAnonymous]
        public async Task<ActionResult> SubLedgerBookReportDataView(int ledgerId, int subledgerId, DateTime fromDate, DateTime toDate, int sbuId, int projectId)
        {
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            var SBU = await specialBranchUnitService.GetSpecialBranchUnitById(sbuId);
            var project = await projectService.GetProjectById(projectId);
            ViewBag.projectName = project?.projectName;
            ViewBag.SBUName = SBU?.branchUnitName;
            //var model = await reportingService.GetSubLedgerBookReportData(subledgerId, fromDate, toDate);
            var model = await reportingService.GetLedgerBookReportData(ledgerId, subledgerId, fromDate, toDate, sbuId, projectId);
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult SubLedgerBookReportDataViewpdf(int ledgerId, int subledgerId, DateTime fromDate, DateTime toDate, int sbuId, int projectId)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/Accounting/AccountingReport/SubLedgerBookReportDataView?ledgerId=" + ledgerId + "&subledgerId=" + subledgerId + "&fromDate=" + fromDate + "&toDate=" + toDate + "&sbuId=" + sbuId + "&projectId=" + projectId;

            string fileName;
            string status = myPDF.GenerateLandscapePDFA4(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [AllowAnonymous]
        public async Task<ActionResult> DayBookReportDataView(int voucherTypeId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId)
        {
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            var SBU = await specialBranchUnitService.GetSpecialBranchUnitById(Convert.ToInt32(sbuId));
            var project = await projectService.GetProjectById(Convert.ToInt32(projectId));
            ViewBag.projectName = project?.projectName;
            ViewBag.SBUName = SBU?.branchUnitName;
            var model = await reportingService.GetDayBookReportData(voucherTypeId, fromDate, toDate, sbuId, projectId);
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult DayBookReportDataViewpdf(int voucherTypeId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/Accounting/AccountingReport/DayBookReportDataView?voucherTypeId=" + voucherTypeId + "&fromDate=" + fromDate + "&toDate=" + toDate + "&sbuId=" + sbuId + "&projectId=" + projectId;

            string fileName;
            string status = myPDF.GenerateLandscapePDFA4(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [AllowAnonymous]
        public IActionResult BudgetVsExpenditurePdf(int? sbuId, int? projectId, DateTime fromDate, DateTime toDate)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/Accounting/AccountingReport/BudgetVsExpenditure?sbuId=" + sbuId + "&projectId=" + projectId + " &fromDate=" + fromDate + "&toDate=" + toDate;

            string fileName;
            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [AllowAnonymous]
        public IActionResult YearlyBudgetDetailsPdf(int? sbuId, int? projectId, DateTime fromDate, DateTime toDate)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/Accounting/AccountingReport/YearlyBudgetDetailsView?sbuId=" + sbuId + "&projectId=" + projectId + " &fromDate=" + fromDate + "&toDate=" + toDate;

            string fileName;
            string status = myPDF.GeneratePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }



        [AllowAnonymous]
        public async Task<ActionResult> TrialBalanceReportData(DateTime fromDate, DateTime toDate, int? sbuId, int? projectId, int? reportBy)
        {
            var SBU = await specialBranchUnitService.GetSpecialBranchUnitById(Convert.ToInt32(sbuId));
            var project = await projectService.GetProjectById(Convert.ToInt32(projectId));
            ViewBag.projectName = project?.projectName;
            ViewBag.SBUName = SBU?.branchUnitName;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            var model = await reportingService.GetTrialBalanceReportData(fromDate, toDate, sbuId, projectId, reportBy);
            return View(model);
        }


        [AllowAnonymous]
        public async Task<ActionResult> CCReportDataView(int costcentreId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId)
        {
            var SBU = await specialBranchUnitService.GetSpecialBranchUnitById(Convert.ToInt32(sbuId));
            var project = await projectService.GetProjectById(Convert.ToInt32(projectId));
            ViewBag.projectName = project?.projectName;
            ViewBag.SBUName = SBU?.branchUnitName;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            var model = await reportingService.GetCCWiseLedgerReportViewModels(costcentreId, fromDate, toDate, sbuId, projectId);
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult CCReportDataViewpdf(int costcentreId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/Accounting/AccountingReport/CCReportDataView?costcentreId=" + costcentreId + "&fromDate=" + fromDate + "&toDate=" + toDate + "&sbuId=" + sbuId + "&projectId=" + projectId;

            string fileName;
            string status = myPDF.GenerateLandscapePDFA4(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        [AllowAnonymous]
        public async Task<ActionResult> chartofAccounts(int? projectId, int? sbuId)
        {
            var SBU = await specialBranchUnitService.GetSpecialBranchUnitById(Convert.ToInt32(sbuId));
            var project = await projectService.GetProjectById(Convert.ToInt32(projectId));
            ViewBag.projectName = project?.projectName;
            ViewBag.SBUName = SBU?.branchUnitName;
            var model = await reportingService.ChartOfAccountViewModels(sbuId, projectId);
            return View(model);
        }
        [AllowAnonymous]
        public IActionResult chartofAccountspdf(int? projectId, int? sbuId)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/Accounting/AccountingReport/chartofAccounts?projectId=" + projectId + "&sbuId=" + sbuId;

            string fileName;
            string status = myPDF.GeneratePDFVoucher(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [AllowAnonymous]
        public IActionResult TrialBalanceReportDatapdf(DateTime fromDate, DateTime toDate, int? sbuId, int? projectId, int? reportBy)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/Accounting/AccountingReport/TrialBalanceReportData?fromDate=" + fromDate + "&toDate=" + toDate + "&sbuId=" + sbuId + "&projectId=" + projectId + "&reportBy=" + reportBy;

            string fileName;
            string status = myPDF.GeneratePDFVoucher(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [AllowAnonymous]
        public IActionResult LedgerDetailsPdf(int ledgerId, DateTime fromDate, DateTime toDate)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/Accounting/AccountingReport/LedgerDetailsDataView?ledgerId=" + ledgerId + "&fromDate=" + fromDate + "&toDate=" + toDate;

            string fileName;
            string status = myPDF.GenerateLandscapePDFA4(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }


        #region Profit & Loss

        [AllowAnonymous]
        //public async Task<ActionResult> PLReportData(int? salaryYearId)
        public async Task<ActionResult> PLReportData(string fromDate, string toDate, int? sbuId, int? projectId)
        {
            ViewBag.toDate = Convert.ToDateTime(toDate).Year - 1;
            ViewBag.fromDate = Convert.ToDateTime(fromDate).Year;
            var model = new FinancialReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                profitAndLossAccountViewModels = await reportingService.GetProfitLossACData(fromDate, toDate, sbuId, projectId)
                //profitAndLossAccountViewModels = await reportingService.GetProfitLossACData(salaryYearId)
            };
            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> PLReportDataBrac(string fromDate, string toDate, int? sbuId, int? projectId)
        {
            var SBU = await specialBranchUnitService.GetSpecialBranchUnitById(Convert.ToInt32(sbuId));
            var project = await projectService.GetProjectById(Convert.ToInt32(projectId));
            ViewBag.projectName = project?.projectName;
            ViewBag.SBUName = SBU?.branchUnitName;
            ViewBag.previousYear = Convert.ToDateTime(toDate).Year - 1;
            ViewBag.currentYear = Convert.ToDateTime(fromDate).Year;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            var model = new FinancialReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                profitAndLossAccountViewModels = await reportingService.GetProfitLossACData(fromDate, toDate, sbuId, projectId)
            };
            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> CFSReportDataBrac(string fromDate, string toDate, int? sbuId, int? projectId)
        {
            var SBU = await specialBranchUnitService.GetSpecialBranchUnitById(Convert.ToInt32(sbuId));
            var project = await projectService.GetProjectById(Convert.ToInt32(projectId));
            ViewBag.projectName = project?.projectName;
            ViewBag.SBUName = SBU?.branchUnitName;
            var model = new FinancialReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                cFSMasterViewModels = await reportingService.GetCFSMasterViewModels(fromDate, toDate, sbuId, projectId)
            };
            // return Json(model.cFSMasterViewModels);
            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> CFSReportIndirectDataBrac(string fromDate, string toDate, int? sbuId, int? projectId)
        {
            var SBU = await specialBranchUnitService.GetSpecialBranchUnitById(Convert.ToInt32(sbuId));
            var project = await projectService.GetProjectById(Convert.ToInt32(projectId));
            ViewBag.projectName = project?.projectName;
            ViewBag.SBUName = SBU?.branchUnitName;
            var model = new FinancialReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                cFSMasterViewModels = await reportingService.GetCFSIndirectMasterViewModels(fromDate, toDate, sbuId, projectId)
            };
            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> RVReportDataBrac(string fromDate, string toDate, int? sbuId, int? projectId)
        {
            var SBU = await specialBranchUnitService.GetSpecialBranchUnitById(Convert.ToInt32(sbuId));
            var project = await projectService.GetProjectById(Convert.ToInt32(projectId));
            ViewBag.projectName = project?.projectName;
            ViewBag.SBUName = SBU?.branchUnitName;
            var model = new FinancialReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                rVMasterViewModels = await reportingService.GetRVMasterViewModels(fromDate, toDate, sbuId, projectId, 1)
            };
            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> RVReportDataUrbn(string fromDate, string toDate, int? sbuId, int? projectId, int? reportBy)
        {

            var SBU = await specialBranchUnitService.GetSpecialBranchUnitById(Convert.ToInt32(sbuId));
            var project = await projectService.GetProjectById(Convert.ToInt32(projectId));
            ViewBag.projectName = project?.projectName;
            ViewBag.SBUName = SBU?.branchUnitName;
            var model = new FinancialReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                rVMasterViewModels = await reportingService.GetRVMasterViewModels(fromDate, toDate, sbuId, projectId, reportBy)
            };
            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> RVReportDataPolwel(string fromDate, string toDate, int? sbuId, int? projectId)
        {

            var model = new FinancialReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                rVMasterViewModels = await reportingService.GetRVMasterViewModels(fromDate, toDate, sbuId, projectId, 1)
            };
            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> BEReportDataBrac(string fromDate, string toDate, int? sbuId, int? projectId)
        {
            var SBU = await specialBranchUnitService.GetSpecialBranchUnitById(Convert.ToInt32(sbuId));
            var project = await projectService.GetProjectById(Convert.ToInt32(projectId));
            ViewBag.projectName = project?.projectName;
            ViewBag.SBUName = SBU?.branchUnitName;
            var model = new FinancialReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                budgetExpenseMasterViewModels = await reportingService.GetBudgetExpenseMasterViewModels(fromDate, toDate, sbuId, projectId)
            };
            // return Json(model.cFSMasterViewModels);
            return View(model);
        }


        [AllowAnonymous]
        public IActionResult PLReportDatapdf(string fromDate, string toDate, int? sbuId, int? projectId)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;

            //string url = scheme + "://" + host + "/Accounting/AccountingReport/PLReportData?salaryYearId=" + salaryYearId;
            string url = scheme + "://" + host + "/Accounting/AccountingReport/PLReportDataBrac?fromDate=" + fromDate + "&toDate=" + toDate + "&sbuId=" + sbuId + "&projectId=" + projectId;

            string fileName;
            string status = myPDF.GeneratePDFVoucher(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [AllowAnonymous]
        public async Task<IActionResult> PLReportDataExcel(string fromDate, string toDate, int? sbuId, int? projectId)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;

            //string url = scheme + "://" + host + "/Accounting/AccountingReport/PLReportData?salaryYearId=" + salaryYearId;
            string url = scheme + "://" + host + "/Accounting/AccountingReport/PLReportDataBrac?fromDate=" + fromDate + "&toDate=" + toDate + "&sbuId=" + sbuId + "&projectId=" + projectId;

            string fileName;
            string status = myPDF.GeneratePDFVoucher(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            string pdfpath = rootPath + @"\wwwroot\pdf\" + fileName;
            string wordpath = fileName.Replace(".pdf", ".csv");
            SautinSoft.PdfFocus f = new SautinSoft.PdfFocus();
            f.OpenPdf(rootPath + @"\wwwroot\pdf\" + fileName);
            f.ToExcel(rootPath + @"\wwwroot\pdf\" + wordpath);
            var model = wordpath;
            return Json(model);
        }

        [AllowAnonymous]
        public IActionResult CFSReportDatapdf(string fromDate, string toDate, int? sbuId, int? projectId)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;

            //string url = scheme + "://" + host + "/Accounting/AccountingReport/PLReportData?salaryYearId=" + salaryYearId;
            string url = scheme + "://" + host + "/Accounting/AccountingReport/CFSReportDataBrac?fromDate=" + fromDate + "&toDate=" + toDate + "&sbuId=" + sbuId + "&projectId=" + projectId;

            string fileName;
            string status = myPDF.GeneratePDFVoucher(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [AllowAnonymous]
        public IActionResult CFSIndirectReportDatapdf(string fromDate, string toDate, int? sbuId, int? projectId)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/Accounting/AccountingReport/CFSReportIndirectDataBrac?fromDate=" + fromDate + "&toDate=" + toDate + "&sbuId=" + sbuId + "&projectId=" + projectId;

            string fileName;
            string status = myPDF.GeneratePDFVoucher(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [AllowAnonymous]
        public async Task<IActionResult> RVReportDatapdf(string fromDate, string toDate, int? sbuId, int? projectId, int? reportBy)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string rptType = "RECEIVEPAYMENT";


            var data = await salaryService.GetReportFormatByReportType(rptType);
            string url = scheme + "://" + host + "/Accounting/AccountingReport/" + data.FirstOrDefault().reportFormat + "?fromDate=" + fromDate + "&toDate=" + toDate + "&sbuId=" + sbuId + "&projectId=" + projectId + "&reportBy=" + reportBy;

            string fileName;
            string status = myPDF.GenerateLandscapePDFA4(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        public IActionResult BEReportDatapdf(string fromDate, string toDate, int? sbuId, int? projectId)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/Accounting/AccountingReport/BEReportDataBrac?fromDate=" + fromDate + "&toDate=" + toDate + "&sbuId=" + sbuId + "&projectId=" + projectId;

            string fileName;
            string status = myPDF.GeneratePDFVoucher(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        public IActionResult BEReportDatapdfP(int Id, int? partnerId, int? sbuId, int? projectId)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;

            //string url = scheme + "://" + host + "/Accounting/AccountingReport/PLReportData?salaryYearId=" + salaryYearId;
            string url = scheme + "://" + host + "/Accounting/AccountingReport/BEReportDataBracp?Id=" + Id + "&partnerId=" + partnerId + "&sbuId=" + sbuId + "&projectId=" + projectId;

            string fileName;
            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }




        [AllowAnonymous]
        public async Task<ActionResult> FinancialReport()
        {

            var model = new FinancialReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
            };
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult FinancialReportPDF()
        {
            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/Accounting/AccountingReport/FinancialReport";
            string fileName;
            string status = myPDF.GenerateLandscapePDFA4(out fileName, url);

            FileName = fileName;
            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        #endregion

        #region Balance Sheet

        [AllowAnonymous]
        public async Task<ActionResult> BalanceSheetReportData(string fromDate, string toDate, int? sbuId, int? projectId)
        //public async Task<ActionResult> BalanceSheetReportData(int? salaryYearId)
        {
            //ViewBag.toDate = toDate;
            //ViewBag.lasttoDate = ltoDate;
            var model = new FinancialReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                balanceSheetViewModels = await reportingService.GetBalanceSheetData(fromDate, toDate, sbuId, projectId),
                profitAndLossAccountViewModels = await reportingService.GetProfitLossACData(fromDate, toDate, sbuId, projectId)
            };
            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> BalanceSheetReportDataBrac(string fromDate, string toDate, int? sbuId, int? projectId)
        {
            try
            {
                var SBU = await specialBranchUnitService.GetSpecialBranchUnitById(Convert.ToInt32(sbuId));
                var project = await projectService.GetProjectById(Convert.ToInt32(projectId));
                ViewBag.projectName = project?.projectName;
                ViewBag.SBUName = SBU?.branchUnitName;
                ViewBag.previousYear = Convert.ToDateTime(toDate).Year - 1;
                ViewBag.currentYear = Convert.ToDateTime(fromDate).Year;

                DateTime FromDate = DateTime.Parse(fromDate);
                FromDate = FromDate.AddDays(-1);
                ViewBag.frmDate = FromDate.ToString("dd-MM-yyyy");

                DateTime FromDate_Old = DateTime.Parse("1900-01-01");
                DateTime ToDate_Old = FromDate;

                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                var model = new FinancialReportViewModel
                {
                    companies = await eRPCompanyService.GetAllCompany(),
                    balanceSheetViewModels = await reportingService.GetBalanceSheetData(fromDate, toDate, sbuId, projectId),
                    profitAndLossAccountViewModels = await reportingService.GetProfitLossACData(fromDate, toDate, sbuId, projectId),
                    profitAndLossAccountViewModels_old = await reportingService.GetProfitLossACData(FromDate_Old.ToString(), ToDate_Old.ToString(), sbuId, projectId),
                    profitAndLossAccountViewModels_net = await reportingService.GetProfitLossACData_Previous(fromDate, toDate, sbuId, projectId)
                };
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [AllowAnonymous]
        public IActionResult BalanceSheetReportDatapdf(string fromDate, string toDate, int? sbuId, int? projectId)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/Accounting/AccountingReport/BalanceSheetReportDataBrac?fromDate=" + fromDate + "&toDate=" + toDate + "&sbuId=" + sbuId + "&projectId=" + projectId;

            string fileName;
            string status = myPDF.GeneratePDFVoucher(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        #endregion

        #region API Section
        [Route("global/api/GetVoucherType/")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetVoucherType()
        {
            return Json(await voucherTypeService.GetVoucherType());
        }

        [Route("global/api/GetLedger/")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetLedger()
        {

            return Json(await ledgerService.GetLedger());
        }

        [Route("global/api/GetLedgerbysbuId/{id}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetLedgerbysbuId(int id)
        {
            return Json(await ledgerService.GetLedgerLedgerBySbu(id));
        }
        

        [Route("global/api/GetCOA/{sbuId}/{projectId}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCOA(int? sbuId, int? projectId)
        {
            return Json(await reportingService.ChartOfAccountdataViewModel(sbuId, projectId));
        }

        [Route("global/api/GetAllSubLedger/")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllSubLedger()
        {
            return Json(await ledgerService.GetAllSubLedger());
        }

        [Route("global/api/GetAllSubLedgerBySbu/{id}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllSubLedgerBySbu(int id)
        {
            return Json(await ledgerService.GetAllSubLedgerBySbu(id));
        }

        [Route("global/api/GetAllSubLedgerbyledger/{id}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllSubLedgerbyledger(int id)
        {
            return Json(await ledgerService.GetAllSubLedgerbyledger(id));
        }

        [Route("global/api/GetLedgerBookReportData/{ledgerId}/{subledgerId}/{fromDate}/{toDate}/{sbuId}/{projectId}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetLedgerBookReportData(int ledgerId, int subledgerId, DateTime fromDate, DateTime toDate, int sbuId, int projectId)
        {
            return Json(await reportingService.GetLedgerBookReportDatad(ledgerId, subledgerId, fromDate, toDate, sbuId, projectId));
        }




        [Route("global/api/GetSubLedgerBookReportData/{subledgerId}/{fromDate}/{toDate}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetSubLedgerBookReportData(int subledgerId, DateTime fromDate, DateTime toDate)
        {
            return Json(await reportingService.GetSubLedgerBookReportDatad(subledgerId, fromDate, toDate));
        }

        [Route("global/api/GetDayBookReportData/{voucherTypeId}/{fromDate}/{toDate}/{sbuId}/{projectId}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetDayBookReportData(int voucherTypeId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId)
        {
            return Json(await reportingService.GetDayBookReportDatad(voucherTypeId, fromDate, toDate, sbuId, projectId));
        }

        [Route("global/api/GetTrialBalanceReportData/{fromDate}/{toDate}/{sbuId}/{projectId}/{reportBy}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetTrialBalanceReportData(DateTime fromDate, DateTime toDate, int? sbuId, int? projectId, int? reportBy)
        {
            return Json(await reportingService.GetTrialBalanceReportData(fromDate, toDate, sbuId, projectId, reportBy));
        }



        [Route("global/api/GetProfitLossACReportData/{fromDate}/{toDate}/{sbuId}/{projectId}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProfitLossACReportData(string fromDate, string toDate, int? sbuId, int? projectId)
        {
            return Json(await reportingService.GetProfitLossACData(fromDate, toDate, sbuId, projectId));
        }

        [Route("global/api/GetBalanceSheetData/{fromDate}/{toDate}/{sbuId}/{projectId}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetBalanceSheetData(string fromDate, string toDate, int? sbuId, int? projectId)
        {
            return Json(await reportingService.GetBalanceSheetData(fromDate, toDate, sbuId, projectId));
        }

        [Route("global/api/GetdailyBillReceive/{supplierId}/{fromDate}/{toDate}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetdailyBillReceive(int supplierId, DateTime fromDate, DateTime toDate)
        {
            return Json(await reportingService.GetdailyBillReceive(supplierId, fromDate, toDate));
        }
        [Route("global/api/GetCCWiseReport/{ccId}/{fromDate}/{toDate}/{sbuId}/{projectId}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCCWiseReport(int ccId, DateTime fromDate, DateTime toDate, int? sbuId, int? projectId)
        {
            return Json(await reportingService.GetCCWiseLedgerReportViewModelsT(ccId, fromDate, toDate, sbuId, projectId));
        }

        [Route("global/api/GetLedgerDetailsData/{ledgerId}/{fromDate}/{toDate}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetLedgerDetailsData(int ledgerId, DateTime fromDate, DateTime toDate)
        {
            return Json(await reportingService.LedgerDetailsViewModels(ledgerId, fromDate, toDate));
        }

        [Route("global/api/GetCostCentres/")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCostCentres()
        {
            return Json(await costCentreService.GetCostCentres());
        }

        [Route("global/api/GetSBU/")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetSBU()
        {
            string username = HttpContext.User.Identity.Name;
            var userinfo = await userInfo.GetUserInfoByUser(username);
            ViewBag.branchid = userinfo?.specialBranchUnitId;

            string userName = userinfo?.UserName;
            if (userName == "biplab" || userName == "suza" || userName == "piu" || userName == "2321" || userName == "opus.admin")
            {
                IEnumerable<SpecialBranchUnit> subs = await specialBranchUnitService.GetSpecialBranchUnit();
                if (userName == "piu" || userName == "2321" || userName == "opus.admin")
                {
                    subs = subs.Where(x => x.Id != 1);
                }
                return Json(subs);
            }
            else
            {
                IEnumerable<SpecialBranchUnit> subs = await specialBranchUnitService.GetSpecialBranchUnitByUserBranchId(ViewBag.branchid);
                return Json(subs);
            }

        }

        [Route("global/api/GetProject/{Id}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProject(int Id)
        {
            return Json(await projectService.GetProjectBySBUId(Id));
        }


        [Route("global/api/GetBankReconcilData/{ledgerId}/{subledgerId}/{fromDate}/{toDate}/{sbuId}/{projectId}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetBankReconcilData(int ledgerId, int subledgerId, DateTime fromDate, DateTime toDate, int sbuId, int projectId)
        {
            //  var model = await reportingService.GetLedgerForBankReconcilation(ledgerId, subledgerId, fromDate, toDate, sbuId, projectId);

            return Json(await reportingService.GetLedgerForBankReconcilation(ledgerId, subledgerId, fromDate, toDate, sbuId, projectId));
        }
        #endregion


        #region Bank Reconciliation
        [AllowAnonymous]
        public async Task<ActionResult> BankReconciliationView(int id)
        {
            ViewBag.masterId = id;
            BankReconciliationModel model = new BankReconciliationModel();
            if (id > 0)
            {
                model.bankReconcileMaster = await voucherService.GetBankReconcileMasterById((int)id);
                if (model.bankReconcileMaster != null)
                {
                    //model.ledgerBookViewModel = await reportingService.GetLedgerForBankReconcilation(Convert.ToInt32(model.bankReconcileMaster?.ledgerId), 0, Convert.ToDateTime(model.bankReconcileMaster?.reconFromDate), Convert.ToDateTime(model.bankReconcileMaster?.reconToDate), model.bankReconcileMaster?.sbuId, 0);

                    model.ledgerBookViewModel = await reportingService.GetStateMentsBankReconcilationByMasterId(Convert.ToInt32(model.bankReconcileMaster?.ledgerId), 0, Convert.ToDateTime(model.bankReconcileMaster?.reconFromDate), Convert.ToDateTime(model.bankReconcileMaster?.reconToDate), model.bankReconcileMaster?.sbuId, 0, model.bankReconcileMaster.Id);


                    //var bankReconDetails = await voucherService.GetBankReconcileDetail();
                    //int?[] voucherDetailIds = bankReconDetails.Select(x => x.voucherDetailId).ToArray();
                    //model.ledgerBookViewModel.ledgerBookViewModelWithoutSPs =
                    //    model.ledgerBookViewModel?.ledgerBookViewModelWithoutSPs.Where(x => !voucherDetailIds.Contains(x.VoucherDetailId));
                }
            }


            return View(model);
        }

        [AllowAnonymous]
        public IActionResult BankReconciliationViewpdf(int id)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/Accounting/AccountingReport/BankReconciliationView?id=" + id;

            string fileName;
            string status = myPDF.GenerateLandscapePDFA4(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        #endregion

        #region Financial Budget Reports
        [AllowAnonymous]
        public IActionResult FinancialBudgetReportPdf(int? sbuId, int? projectId, DateTime fromDate, DateTime toDate, int year)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/Accounting/AccountingReport/FinancialBudgetReport?sbuId=" + sbuId + "&projectId=" + projectId + " &fromDate=" + fromDate + "&toDate=" + toDate + "&year=" + year;

            string fileName;
            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");



        }


        


        [AllowAnonymous]
        public IActionResult FinancialBudgetReportExcel(int? sbuId, int? projectId, DateTime fromDate, DateTime toDate, int year)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/Accounting/AccountingReport/FinancialBudgetExcelReport?sbuId=" + sbuId + "&projectId=" + projectId + " &fromDate=" + fromDate + "&toDate=" + toDate + "&year=" + year;

            string fileName;
            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            //var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            //return new FileStreamResult(stream, "application/pdf");

            string pdfpath = rootPath + @"\wwwroot\pdf\" + fileName;
            string wordpath = fileName.Replace(".pdf", ".xls");
            SautinSoft.PdfFocus f = new SautinSoft.PdfFocus();
            f.OpenPdf(rootPath + @"\wwwroot\pdf\" + fileName);
            f.ToExcel(rootPath + @"\wwwroot\pdf\" + wordpath);
            var model = wordpath;
            //  var model = wordpath;
            return Json(model);
            //var filepath = rootPath + @"\wwwroot\pdf\" + wordpath;
            //var filepath = rootPath + "/wwwroot/pdf/" + wordpath;
            //byte[] bytes = System.IO.File.ReadAllBytes(filepath);
            //return File(bytes, "application/octet-stream", wordpath);

        }
        #endregion
    }
}