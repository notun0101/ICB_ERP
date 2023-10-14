//New
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Accounting.Services.AccountingSettings.Interfaces;
using OPUSERP.Accounting.Services.MasterData.Interfaces;
using OPUSERP.Accounting.Services.Voucher.Interfaces;
using OPUSERP.Budget.Service.Interface;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.Payroll.Services.Salary.Interfaces;
using OPUSERP.ProvidentFund.Service.Interface;
using OPUSERP.ProvidentFund.VMS;
using OPUSERP.SCM.Services.MasterData.Interfaces;

namespace OPUSERP.Areas.ProvidentFund.Controllers
{
    [Authorize]
    [Area("ProvidentFund")]
    public class PFReportController : Controller
    {
        private readonly IERPCompanyService eRPCompanyService;
        private readonly string rootPath;
        private readonly IPFLoanManagementService pFLoanManagementService;
        private readonly MyPDF myPDF;
        public string FileName;
        public PFReportController(IHostingEnvironment hostingEnvironment, IConverter converter, IPFLoanManagementService pFLoanManagementService, IERPCompanyService eRPCompanyService)
        {


            //For PDF
            myPDF = new MyPDF(hostingEnvironment, converter);
            rootPath = hostingEnvironment.ContentRootPath;
            this.pFLoanManagementService = pFLoanManagementService;
            this.eRPCompanyService = eRPCompanyService;
        }


        public IActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> ReportCenter()
        {
            return View();
        }
        public async Task<ActionResult> ReportMemberFund()
        {
            return View();
        }
        public async Task<ActionResult> ReportExMemberFund()
        {
            return View();
        }
        public async Task<ActionResult> ReportInvestmentSchedule()
        {
            return View();
        }
        public async Task<ActionResult> ReportPFStatement()
        {
            return View();
        }


        #region PF Report
        [AllowAnonymous]
        public async Task<IActionResult> PFReportView(int empId, int year)
        {
            ViewBag.year = year;
            PFReportViewModel data = new PFReportViewModel()
            {
                companies = await eRPCompanyService.GetAllCompany(),
                pfreport = await pFLoanManagementService.GetPFReportByEmpId(empId, year)
            };
            return View(data);
        }

        [AllowAnonymous]
        public IActionResult PFReportByEmpId(int empId, int year)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/ProvidentFund/PFReport/PFReportView?empId=" + empId + "&year=" + year;

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
        public async Task<IActionResult> PFStatementByEmpIdAndDateVIEW(int empId, DateTime fromDate, DateTime toDate)
        {

            ViewBag.fromDate = fromDate.ToString("dd-MMM-yyyy");
            ViewBag.toDate = toDate.ToString("dd-MMM-yyyy");
            PFReportViewModel data = new PFReportViewModel()
            {
                companies = await eRPCompanyService.GetAllCompany(),
                pFStatementVMs = await pFLoanManagementService.GetPFStatementByEmpIdAndDate(empId, fromDate, toDate)
            };
            return View(data);
        }

        [AllowAnonymous]
        public IActionResult PFStatementByEmpIdAndDate(int empId, DateTime fromDate, DateTime toDate)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/ProvidentFund/PFReport/PFStatementByEmpIdAndDateVIEW?empId=" + empId + "&fromDate=" + fromDate + "&toDate=" + toDate;

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
        public async Task<IActionResult> PFSummaryView()
        {
            // ViewBag.year = year;
            PFReportViewModel data = new PFReportViewModel()
            {
                companies = await eRPCompanyService.GetAllCompany(),
                pfreport = await pFLoanManagementService.GetDataForPFSummaryReport()
            };
            return View(data);
        }

        [AllowAnonymous]
        public IActionResult PFsummarypdf()
        {
            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/ProvidentFund/PFReport/PFsummaryView";

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
        public async Task<IActionResult> PFDailyTransactionReportVIEW(DateTime fromDate, DateTime toDate)
        {

            ViewBag.fromDate = fromDate.ToString("dd-MMM-yyyy");
            ViewBag.toDate = toDate.ToString("dd-MMM-yyyy");
            PFReportViewModel data = new PFReportViewModel()
            {
                companies = await eRPCompanyService.GetAllCompany(),
                pFStatementVMs = await pFLoanManagementService.GetPFDailyTransactionReport(fromDate, toDate)
            };
            return View(data);
        }

        [AllowAnonymous]
        public IActionResult PFDailyTransactionReport(DateTime fromDate, DateTime toDate)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/ProvidentFund/PFReport/PFDailyTransactionReportVIEW?fromDate=" + fromDate + "&toDate=" + toDate;

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
        public async Task<IActionResult> LoanStatementByEmpIdAndDateVIEW(int empId, DateTime fromDate, DateTime toDate)
        {

            ViewBag.fromDate = fromDate.ToString("dd-MMM-yyyy");
            ViewBag.toDate = toDate.ToString("dd-MMM-yyyy");
            PFReportViewModel data = new PFReportViewModel()
            {
                companies = await eRPCompanyService.GetAllCompany(),
                loanStatementVMs = await pFLoanManagementService.GetTrusteLoanStatementByEpmId(empId, fromDate, toDate)
            };
            return View(data);
        }

        [AllowAnonymous]
        public IActionResult LoanStatementByEmpIdAndDate(int empId, DateTime fromDate, DateTime toDate)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/ProvidentFund/PFReport/LoanStatementByEmpIdAndDateVIEW?empId=" + empId + "&fromDate=" + fromDate + "&toDate=" + toDate;

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
        public async Task<IActionResult> LoanDailyTransactionReportVIEW(DateTime fromDate, DateTime toDate)
        {

            ViewBag.fromDate = fromDate.ToString("dd-MMM-yyyy");
            ViewBag.toDate = toDate.ToString("dd-MMM-yyyy");
            PFReportViewModel data = new PFReportViewModel()
            {
                companies = await eRPCompanyService.GetAllCompany(),
                loanStatementVMs = await pFLoanManagementService.GetLoanDailyTransactionReport(fromDate, toDate)
            };
            return View(data);
        }

        [AllowAnonymous]
        public IActionResult LoanDailyTransactionReport(DateTime fromDate, DateTime toDate)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/ProvidentFund/PFReport/LoanDailyTransactionReportVIEW?fromDate=" + fromDate + "&toDate=" + toDate;

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
        public async Task<IActionResult> BalanceConfirmationReportView(int empId, DateTime fromDate, DateTime toDate)
        {

            ViewBag.fromDate = fromDate.ToString("dd-MMM-yyyy");
            ViewBag.toDate = toDate.ToString("dd-MMM-yyyy");
            PFReportViewModel data = new PFReportViewModel()
            {
                companies = await eRPCompanyService.GetAllCompany(),
                BalanceConfirmationVM = await pFLoanManagementService.GetBalanceConfirmationReportByEmpId(empId, fromDate, toDate)
            };
            return View(data);
        }

        [AllowAnonymous]
        public IActionResult BalanceConfirmationReport(int empId, DateTime fromDate, DateTime toDate)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/ProvidentFund/PFReport/BalanceConfirmationReportView?empId=" + empId + "&fromDate=" + fromDate + "&toDate=" + toDate;

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

    }
}
