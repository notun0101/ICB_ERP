using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.Payroll.Models;
using OPUSERP.Data.Entity;
using OPUSERP.Data.Entity.AttachmentComment;
using OPUSERP.ERPServices.AttachmentComment.Interfaces;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.Payroll.Data.Entity.carLoan;
using OPUSERP.Payroll.Data.Entity.homeLoan;
using OPUSERP.Payroll.Data.Entity.PF;
using OPUSERP.Payroll.Data.Entity.Salary;
using OPUSERP.Payroll.Services.PF.Interfaces;
using OPUSERP.Payroll.Services.Salary.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OPUSERP.Areas.Payroll.Controllers
{
    [Area("Payroll")]
    [Authorize]
    public class FDRInvestmentController : Controller
    {
        // GET: /<controller>/
        private readonly IERPCompanyService eRPCompanyService;
        private readonly IPFService pFService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        public readonly IAttachmentCommentService attachmentCommentService;
        private readonly ISalaryService salaryService;
        private readonly IPersonalInfoService personalInfoService;


        private readonly string rootPath;
        private readonly MyPDF myPDF;

        public FDRInvestmentController(ISalaryService salaryService, UserManager<ApplicationUser> userManager, IPFService pFService, IHostingEnvironment hostingEnvironment, IAttachmentCommentService attachmentCommentService, IPersonalInfoService personalInfoService, IConverter converter, IERPCompanyService eRPCompanyService)
        {
            this.pFService = pFService;
            _userManager = userManager;
            this._hostingEnvironment = hostingEnvironment;
            this.attachmentCommentService = attachmentCommentService;
            this.salaryService = salaryService;
            this.personalInfoService = personalInfoService;
            this.eRPCompanyService = eRPCompanyService;

            //For PDF
            myPDF = new MyPDF(hostingEnvironment, converter);
            rootPath = hostingEnvironment.ContentRootPath;
        }
        #region PFloan
        public async Task<ActionResult> PFLoan()
        {
            var salaryHead = await salaryService.GetAllSalaryHeadByFilter(string.Empty);
            PFLoanViewModel model = new PFLoanViewModel
            {
                pFLoans = await pFService.GetAllPFLoan(),
                salaryHeads = salaryHead.Where(a => a.isAdvance == "Yes"),
                salaryPeriods = await salaryService.GetAllSalaryPeriod(),
                salaryGradesList = await salaryService.GetAllSalaryGrade(),
                visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData()
            };
            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PFLoan([FromForm] PFLoanViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);

            var loandata = await pFService.GetAllPFLoan();
            var count = loandata.Count() + 1;
            string LoanNo = "PFLoan/" + Convert.ToDateTime(DateTime.Now).ToString("dd-MM-yyyy") + "/" + count.ToString();
            if (model.PfloanId > 0)
            {
                LoanNo = model.PfLoanNo;
            }
            PFLoan data = new PFLoan
            {
                Id = model.PfloanId,
                employeeInfoId = model.employeeInfoId,
                salaryPeriodId = model.salaryPeriodId,
                salaryHeadId = model.salaryHeadId,
                salaryGradeId = model.salaryGradeId,
                advanceAmount = model.advanceAmount,
                installmentAmount = model.installmentAmount,
                noOfInstallment = model.noOfInstallment,
                isFromSalary = model.isFromSalary,
                PFLoanNo = LoanNo,
                loanDate = model.loanDate,
                purpose = model.purpose,
                loanType = model.loanType,
                forwardedBy = user.userId
            };


            int pfid = await pFService.SavePFAdvance(data);

            var maxLoanApproverOrder = await pFService.GetMaxOrderByLoanId(pfid) + 1;
            LoanLog loanLog = new LoanLog
            {
                Id = 0,
                loanId = pfid,
                nextapprover = model.approver,
                approveOrder = maxLoanApproverOrder,
                ispassed = 0
            };
            await pFService.SaveLoanLog(loanLog);


            try
            {
                await pFService.DeletePFLoanScheduleByPFId(pfid);
                int periodid = model.salaryPeriodId;
                for (int i = 1; i <= model.noOfInstallment; i++)
                {
                    PFLoanSchedule datas = new PFLoanSchedule
                    {
                        Id = 0,
                        pFLoanId = pfid,
                        salaryPeriodId = periodid,
                        installmentAmount = model.installmentAmount,
                        noOfInstallment = i,
                        restAmount = model.advanceAmount - model.installmentAmount * i,
                        isComplete = 0

                    };
                    await pFService.SavePFLoanSchedule(datas);
                    periodid++;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction(nameof(PFLoan));
        }

        public async Task<IActionResult> GetLoanDetailsAPI(int loanId)
        {
            var model = new PFLoanViewModel
            {
                PfLoan = await pFService.GetPFLoanByIdNew(loanId)
            };
            return Json(model);
        }

        public async Task<IActionResult> GetUserByEmpCode(string code)
        {
            var data = await personalInfoService.GetUserInfoByEmpCode(code);
            return Json(data);
        }

        public async Task<IActionResult> GetOngoingPfLoan()
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var model = new PFLoanViewModel
            {
                loans = await pFService.GetOngoingLoanByTypeAndUserId("PF", user.userId)
            };

            return View(model);
        }
        #region PfLoanPDF
        [AllowAnonymous]
        public async Task<ActionResult> LoanDisbursmentSheet(int year)
        {
            var reportDate = "31 December " + year;
            var model = new PFLoanViewModel
            {
                rptDate = reportDate,
                companies = await eRPCompanyService.GetAllCompany(),
                pFLoans = await pFService.GetLoansByTypeAndStatus1("PF", 1, year)
            };

            return View(model);

        }
        [AllowAnonymous]
        public IActionResult LoanDisbursmentSheetPDF(int year)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string fileName;

            //if (rptType == "loanDisburse")
            //{
            //    url = scheme + "://" + host + "/Payroll/FDRInvestment/LoanDisbursmentSheet?year="+year;
            //}

            url = scheme + "://" + host + "/Payroll/FDRInvestment/LoanDisbursmentSheet?year=" + year;

            string status = myPDF.GeneratePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        [AllowAnonymous]
        public async Task<ActionResult> NonRefundableContribution(int year)
        {
            var reportDate = "31 December " + year;
            var model = new PFLoanViewModel
            {
                rptDate = reportDate,
                companies = await eRPCompanyService.GetAllCompany(),
                pfAccountsSchedules = await pFService.GetNonRefundableContribution(year)
            };

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult NonRefundableContributionPDF(int year)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string fileName;

            url = scheme + "://" + host + "/Payroll/FDRInvestment/NonRefundableContribution?year=" + year;


            string status = myPDF.GeneratePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        [AllowAnonymous]
        public async Task<ActionResult> FinalSettlementContribution(int year)
        {
            var reportDate = "31 December " + year;
            var model = new PFLoanViewModel
            {
                rptDate = reportDate,
                companies = await eRPCompanyService.GetAllCompany(),
                pfAccountsSchedules = await pFService.GetFinalSettlementContribution(year)
            };
            return View(model);
        }

        public IActionResult FinalSettlementContributionPDF(int year)
        {
            string schema = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string filename;

            url = schema+"://" + host + "/Payroll/FDRInvestment/FinalSettlementContribution?year=" + year;

            string status = myPDF.GeneratePDF(out filename, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [AllowAnonymous]
        public async Task<ActionResult> PfScheduleReport(int year)
        {
            var reportDate = "31 December " + year;
            var model = new PFLoanViewModel
            {
                rptDate = reportDate,
                companies = await eRPCompanyService.GetAllCompany(),
                pfAccountsSchedules = await eRPCompanyService.GetAllPfAccountsByYear(year)
            };
            return View(model);
        }



        public IActionResult PfScheduleReportPdf(int year)
        {
            string schema = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string filename;

            url = schema+"://" + host + "/Payroll/FDRInvestment/PfScheduleReport?year=" + year;

            string status = myPDF.GenerateLandscapePDF(out filename, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [AllowAnonymous]
        public async Task<ActionResult> PfAccountScheduleReport(int year)
        {
            var reportDate = "31 December " + year;
            var model = new PFLoanViewModel
            {
                rptDate = reportDate,
                companies = await eRPCompanyService.GetAllCompany(),
                pfAccountsSchedules = await eRPCompanyService.GetAllPfAccountsByYear(year)
            };
            return View(model);
        }

        public IActionResult PfAccountScheduleReportPdf(int year)
        {
            string schema = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string filename;

            url = schema+"://" + host + "/Payroll/FDRInvestment/PfAccountScheduleReport?year=" + year;

            string status = myPDF.GenerateLandscapePDF(out filename, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        public async Task<ActionResult> PfEmpWiseYearlyStatement(int year,int pfId)
        {
            var firstDate = new DateTime(year, 1, 1);
            var lastDate = new DateTime(year, 12, 31);
            var pfMemberInfo = await pFService.GetPfMemberById(pfId);

            var model = new PFAccountsViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                pFMember = pfMemberInfo,
                pfAccounts = await pFService.GetPfAccountByIdAndYear(pfId, year),
                firstDate = firstDate,
                lastDate = lastDate
            };
            return View(model);
        }
        public IActionResult PfEmpWiseYearlyStatementPDF(int pfId,int year)
        {
            string schema = Request.Scheme;
            var host = Request.Host;
            //string url = "";
            string filename;
            string url = schema + "://" + host + "/Payroll/FDRInvestment/PfEmpWiseYearlyStatement?pfId=" + pfId + "&year="+ year;
            string status = myPDF.GeneratePDF(out filename, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        public async Task<ActionResult> PfIndividualYearlyStatement(int pfId, int year)
        {
            var firstDate = new DateTime(year, 1, 1);
            var lastDate = new DateTime(year, 12, 31);
            var pfMemberInfo = await pFService.GetPfMemberById(pfId);

			var pfYearlyStement = await pFService.GetPfIndividualYearlyStatement(pfId, year);
            var model = new PFAccountsViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                pFMember = pfMemberInfo,
                pfAccounts = await pFService.GetPfAccountByIdAndYear(pfId, year),
                firstDate = firstDate,
                lastDate = lastDate,
				pfYearlyStatements = pfYearlyStement
            };
            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> PfIndividualYearlyStatementWithMonth(int pfId, int year, int startMonth, int endMonth)
		{
			var lastDay= DateTime.DaysInMonth(year, endMonth);
			var firstDate = new DateTime(year, startMonth, 1);
			var lastDate = new DateTime(year, endMonth, lastDay);
			var pfMemberInfo = await pFService.GetPfMemberById(pfId);

			var pfYearlyStement = await pFService.GetPfIndividualYearlyStatementWithMonth(pfId, year, firstDate, lastDate);
			var dif = endMonth - startMonth;
			var monthList = new List<string>();
			var month = startMonth;
			decimal totalPfBank = 0;
			decimal totalPfOwn = 0;
			for (int i = 0; i < dif+1; i++)
			{
					if (month == 1)
					{
						totalPfBank = totalPfBank + Convert.ToDecimal(pfYearlyStement?.januaryBank);
						totalPfOwn = totalPfOwn + Convert.ToDecimal(pfYearlyStement?.januaryOwn);
					}
					else if (month == 2)
					{
						totalPfBank = totalPfBank + Convert.ToDecimal(pfYearlyStement?.februaryBank);
						totalPfOwn = totalPfOwn + Convert.ToDecimal(pfYearlyStement?.februaryOwn);
					}
					else if (month == 3)
					{
						totalPfBank = totalPfBank + Convert.ToDecimal(pfYearlyStement?.marchBank);
						totalPfOwn = totalPfOwn + Convert.ToDecimal(pfYearlyStement?.marchOwn);
					}
					else if (month == 4)
					{
						totalPfBank = totalPfBank + Convert.ToDecimal(pfYearlyStement?.aprilBank);
						totalPfOwn = totalPfOwn + Convert.ToDecimal(pfYearlyStement?.aprilOwn);
					}
					else if (month == 5)
					{
						totalPfBank = totalPfBank + Convert.ToDecimal(pfYearlyStement?.mayBank);
						totalPfOwn = totalPfOwn + Convert.ToDecimal(pfYearlyStement?.mayOwn);
					}
					else if (month == 6)
					{
						totalPfBank = totalPfBank + Convert.ToDecimal(pfYearlyStement?.juneBank);
						totalPfOwn = totalPfOwn + Convert.ToDecimal(pfYearlyStement?.juneOwn);
					}
					else if (month == 7)
					{
						totalPfBank = totalPfBank + Convert.ToDecimal(pfYearlyStement?.julyBank);
						totalPfOwn = totalPfOwn + Convert.ToDecimal(pfYearlyStement?.julyOwn);
					}
					else if (month == 8)
					{
						totalPfBank = totalPfBank + Convert.ToDecimal(pfYearlyStement?.augustBank);
						totalPfOwn = totalPfOwn + Convert.ToDecimal(pfYearlyStement?.augustOwn);
					}
					else if (month == 9)
					{
						totalPfBank = totalPfBank + Convert.ToDecimal(pfYearlyStement?.septemberBank);
						totalPfOwn = totalPfOwn + Convert.ToDecimal(pfYearlyStement?.septemberOwn);
					}
					else if (month == 10)
					{
						totalPfBank = totalPfBank + Convert.ToDecimal(pfYearlyStement?.octoberBank);
						totalPfOwn = totalPfOwn + Convert.ToDecimal(pfYearlyStement?.octoberOwn);
					}
					else if (month == 11)
					{
						totalPfBank = totalPfBank + Convert.ToDecimal(pfYearlyStement?.novemberBank);
						totalPfOwn = totalPfOwn + Convert.ToDecimal(pfYearlyStement?.novemberOwn);
					}
					else if (month == 12)
					{
						totalPfBank = totalPfBank + Convert.ToDecimal(pfYearlyStement?.decemberBank);
						totalPfOwn = totalPfOwn + Convert.ToDecimal(pfYearlyStement?.decemberOwn);
					}
					month++;

			}

			var result = new PfYearlyStatement
			{
				designationName = pfYearlyStement.designationName,
				employeeCode = pfYearlyStement.employeeCode,
				empName = pfYearlyStement.empName,
				MemberId = pfYearlyStement.MemberId,
				Posting = pfYearlyStement.Posting,
				PfBank= totalPfBank,
				PfOwn=totalPfOwn
			};
			var model = new PFAccountsViewModel
			{
				companies = await eRPCompanyService.GetAllCompany(),
				pFMember = pfMemberInfo,
				pfAccounts = await pFService.GetPfAccountByIdAndYear(pfId, year),
				firstDate = firstDate,
				lastDate = lastDate,
				pfYearlyStatements = result
			};
			return View(model);
		}
		public async Task<ActionResult> PfEmployeeDetailsReport(int pfId, int year)
        {
            var firstDate = new DateTime(year, 1, 1);
            var lastDate = new DateTime(year, 12, 31);
            var pfMemberInfo = await pFService.GetPfMemberById(pfId);

            //var totalPfOwn = await pFService.GetTotalPfLoanByYearAndPfId(pfId, year);
            var model = new PFAccountsViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                pFMember = pfMemberInfo,
                PFEmployeeDetailList = await pFService.GetPfDetailByPfIdAndYear(pfId, year),
                firstDate = firstDate,
                lastDate = lastDate,
                //totalpfloan = totalPfOwn
            };
            decimal totDebit = 0;
            decimal totCredit = 0;
            foreach (var data in model.PFEmployeeDetailList)
            {
                totDebit = totDebit +  decimal.Parse(data.DebitAmount.ToString());
                totCredit = totCredit + decimal.Parse(data.creditAmount.ToString());
            }

            ViewBag.TotDebit = totDebit;
            ViewBag.TotCredit = totCredit;

            return View(model);
        }

        public async Task<ActionResult> PfBranchWiseMonthlySummery(int year, string month)
        {
            //var firstDate = new DateTime(year, 1, 1);
            //var lastDate = new DateTime(year, 12, 31);
            //var pfMemberInfo = await pFService.GetPfMemberById(pfId);

            //var totalPfOwn = await pFService.GetTotalPfLoanByYearAndPfId(pfId, year);
            var model = new PFAccountsViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                //pFMember = pfMemberInfo,
                PFBranchWiseMonthlySummeryList = await pFService.GetPfBranchWiseMonthlySummery(year, month),
                //firstDate = firstDate,
                //lastDate = lastDate,
                //totalpfloan = totalPfOwn
            };
            decimal totDebit = 0;
            decimal totCredit = 0;
            foreach (var data in model.PFBranchWiseMonthlySummeryList)
            {
                totDebit = totDebit + decimal.Parse(data.DebitAmount.ToString());
                totCredit = totCredit + decimal.Parse(data.creditAmount.ToString());
            }

            ViewBag.TotDebit = totDebit;
            ViewBag.TotCredit = totCredit;
            ViewBag.Month = month;
            ViewBag.Year = year;

            return View(model);
        }

        public IActionResult PfIndividualYearlyStatementPDF(int pfId, int year)
        {
            string schema = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string filename;

            url = schema+"://" + host + "/Payroll/FDRInvestment/PfIndividualYearlyStatement?pfId=" + pfId + "&year=" + year;

            string status = myPDF.GeneratePDF(out filename, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        [AllowAnonymous]
		public IActionResult PfIndividualYearlyStatementWithMonthPDF(int pfId, int year,int startMonth,int endMonth)
		{
			string schema = Request.Scheme;
			var host = Request.Host;
			//string url = "";
			string filename;

			string url = schema + "://" + host + "/Payroll/FDRInvestment/PfIndividualYearlyStatementWithMonth?pfId=" + pfId + "&year=" + year+ "&startMonth="+ startMonth + "&endMonth="+endMonth;

			string status = myPDF.GeneratePDF(out filename, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}
		public IActionResult PfEmployeeDetailsReportPDF(int pfId, int year)
        {
            string schema = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string filename;

            url = schema+"://" + host + "/Payroll/FDRInvestment/PfEmployeeDetailsReport?pfId=" + pfId + "&year=" + year;

            string status = myPDF.GeneratePDF(out filename, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        public IActionResult PfBranchWiseMonthlySummeryPDF(int year, string month)
        {
            string schema = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string filename;

            url = schema+"://" + host + "/Payroll/FDRInvestment/PfBranchWiseMonthlySummery?year=" + year + "&month=" + month;

            string status = myPDF.GeneratePDF(out filename, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        #endregion


        public async Task<IActionResult> GetForwardedPfLoan()
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var model = new PFLoanViewModel
            {
                loans = await pFService.GetForwardedLoanByTypeAndUserId("PF", user.userId)
            };

            return View(model);
        }
        public async Task<IActionResult> GetForwardedCarLoan()
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var model = new PFLoanViewModel
            {
                loans = await pFService.GetForwardedLoanByTypeAndUserId("Car", user.userId)
            };

            return View(model);
        }
        public async Task<IActionResult> GetForwardedHomeLoan()
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var model = new PFLoanViewModel
            {
                loans = await pFService.GetForwardedLoanByTypeAndUserId("Home", user.userId)
            };

            return View(model);
        }
        public async Task<IActionResult> ApprovedPFLoans()
        {
            var model = new PFLoanViewModel
            {
                pFLoans = await pFService.GetLoansByTypeAndStatus("PF", 1)
            };

            return View(model);
        }

        public async Task<IActionResult> ForwardToNewApproverBy(int loanId, string type, int approverId)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);

            var prevLog = await pFService.GetLoanLogByUserAndLoan(user.userId, loanId);
            prevLog.ispassed = 1;
            await pFService.SaveLoanLog(prevLog);

            var maxLoanApproverOrder = await pFService.GetMaxOrderByLoanId(loanId) + 1;
            LoanLog loanLog = new LoanLog
            {
                Id = 0,
                loanId = loanId,
                nextapprover = approverId,
                approveOrder = maxLoanApproverOrder,
                ispassed = 0
            };
            var logId = await pFService.SaveLoanLog(loanLog);
            if (logId > 0)
            {
                return Json("Ok");
            }
            else
            {
                return Json("Failed");
            }
        }

        public async Task<IActionResult> ApproveLoan(int id)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);

            var loan = await pFService.GetPFLoanById(id);
            loan.approver = user.userId;
            await pFService.SavePFAdvance(loan);

            var loanlog = await pFService.GetLoanIdAndUserId(loan.Id, user.userId);
            loanlog.ispassed = 1;
            await pFService.SaveLoanLog(loanlog);
            return Json("ok");
        }

        #endregion


        #region CarLoan
        public async Task<ActionResult> CarLoanApply()
        {
            var salaryHead = await salaryService.GetAllSalaryHeadByFilter(string.Empty);
            PFLoanViewModel model = new PFLoanViewModel
            {
                pFLoans = await pFService.GetAllPFLoan(),
                salaryHeads = salaryHead.Where(a => a.isAdvance == "Yes"),
                salaryPeriods = await salaryService.GetAllSalaryPeriod(),
                salaryGradesList = await salaryService.GetAllSalaryGrade(),
                visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData()
            };
            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CarLoanApply([FromForm] PFLoanViewModel model)
        {

            var loandata = await pFService.GetAllPFLoan();
            var count = loandata.Count() + 1;
            string LoanNo = "CarLoan/" + Convert.ToDateTime(DateTime.Now).ToString("dd-MM-yyyy") + "/" + count.ToString();
            if (model.PfloanId > 0)
            {
                LoanNo = model.PfLoanNo;
            }
            PFLoan data = new PFLoan
            {
                Id = model.PfloanId,
                employeeInfoId = model.employeeInfoId,
                salaryPeriodId = model.salaryPeriodId,
                salaryHeadId = model.salaryHeadId,
                salaryGradeId = model.salaryGradeId,
                advanceAmount = model.advanceAmount,
                installmentAmount = model.installmentAmount,
                noOfInstallment = model.noOfInstallment,
                isFromSalary = model.isFromSalary,
                PFLoanNo = LoanNo,
                loanDate = model.loanDate,
                purpose = model.purpose,
                loanType = model.loanType
            };


            int pfid = await pFService.SavePFAdvance(data);

            var maxLoanApproverOrder = await pFService.GetMaxOrderByLoanId(pfid) + 1;
            LoanLog loanLog = new LoanLog
            {
                Id = 0,
                loanId = pfid,
                nextapprover = model.approver,
                approveOrder = maxLoanApproverOrder,
                ispassed = 0
            };
            await pFService.SaveLoanLog(loanLog);


            try
            {
                await pFService.DeletePFLoanScheduleByPFId(pfid);
                int periodid = model.salaryPeriodId;
                for (int i = 1; i <= model.noOfInstallment; i++)
                {
                    PFLoanSchedule datas = new PFLoanSchedule
                    {
                        Id = 0,
                        pFLoanId = pfid,
                        salaryPeriodId = periodid,
                        installmentAmount = model.installmentAmount,
                        noOfInstallment = i,
                        restAmount = model.advanceAmount - model.installmentAmount * i,
                        isComplete = 0

                    };
                    await pFService.SavePFLoanSchedule(datas);
                    periodid++;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction(nameof(CarLoanApply));
        }

        public async Task<IActionResult> GetOngoingCarLoan()
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var model = new PFLoanViewModel
            {
                loans = await pFService.GetOngoingLoanByTypeAndUserId("car", user.userId)
            };

            return View(model);
        }

         public async Task<IActionResult> GetMyCarLoan()
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var emp = user.EmpCode;
            var model = new PFLoanViewModel
            {
                loans = await pFService.GetMyLoanByTypeAndEmpId("car", emp)
            };

            return View(model);
        }



        public async Task<IActionResult> ApprovedCarLoans()
        {
            var model = new PFLoanViewModel
            {
                pFLoans = await pFService.GetLoansByTypeAndStatus("car", 1)
            };

            return View(model);
        }

        //public async Task<IActionResult> ApproveCarLoan(int id)
        //{

        //    string userName = HttpContext.User.Identity.Name;
        //    var user = await _userManager.FindByNameAsync(userName);

        //    var loan = await pFService.GetPFLoanById(id);
        //    loan.approver = user.userId;
        //    await pFService.SavePFAdvance(loan);

        //    var loanlog = await pFService.GetLoanIdAndUserId(loan.Id, user.userId);
        //    loanlog.ispassed = 1;
        //    await pFService.SaveLoanLog(loanlog);
        //    return Json("ok");
        //}
        #endregion


        #region HomeLoan
        public async Task<ActionResult> HomeLoanApply()
        {
            var salaryHead = await salaryService.GetAllSalaryHeadByFilter(string.Empty);
            PFLoanViewModel model = new PFLoanViewModel
            {
                pFLoans = await pFService.GetAllPFLoan(),
                salaryHeads = salaryHead.Where(a => a.isAdvance == "Yes"),
                salaryPeriods = await salaryService.GetAllSalaryPeriod(),
                salaryGradesList = await salaryService.GetAllSalaryGrade(),
                visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData()
            };
            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HomeLoanApply([FromForm] PFLoanViewModel model)
        {

            var loandata = await pFService.GetAllPFLoan();
            var count = loandata.Count() + 1;
            string LoanNo = "HomeLoan/" + Convert.ToDateTime(DateTime.Now).ToString("dd-MM-yyyy") + "/" + count.ToString();
            if (model.PfloanId > 0)
            {
                LoanNo = model.PfLoanNo;
            }
            PFLoan data = new PFLoan
            {
                Id = model.PfloanId,
                employeeInfoId = model.employeeInfoId,
                salaryPeriodId = model.salaryPeriodId,
                salaryHeadId = model.salaryHeadId,
                salaryGradeId = model.salaryGradeId,
                advanceAmount = model.advanceAmount,
                installmentAmount = model.installmentAmount,
                noOfInstallment = model.noOfInstallment,
                isFromSalary = model.isFromSalary,
                PFLoanNo = LoanNo,
                loanDate = model.loanDate,
                purpose = model.purpose,
                loanType = model.loanType
            };


            int pfid = await pFService.SavePFAdvance(data);

            var maxLoanApproverOrder = await pFService.GetMaxOrderByLoanId(pfid) + 1;
            LoanLog loanLog = new LoanLog
            {
                Id = 0,
                loanId = pfid,
                nextapprover = model.approver,
                approveOrder = maxLoanApproverOrder,
                ispassed = 0
            };
            await pFService.SaveLoanLog(loanLog);


            try
            {
                await pFService.DeletePFLoanScheduleByPFId(pfid);
                int periodid = model.salaryPeriodId;
                for (int i = 1; i <= model.noOfInstallment; i++)
                {
                    PFLoanSchedule datas = new PFLoanSchedule
                    {
                        Id = 0,
                        pFLoanId = pfid,
                        salaryPeriodId = periodid,
                        installmentAmount = model.installmentAmount,
                        noOfInstallment = i,
                        restAmount = model.advanceAmount - model.installmentAmount * i,
                        isComplete = 0

                    };
                    await pFService.SavePFLoanSchedule(datas);
                    periodid++;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction(nameof(HomeLoanApply));
        }

        public async Task<IActionResult> GetOngoingHomeLoan()
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var model = new PFLoanViewModel
            {
                loans = await pFService.GetOngoingLoanByTypeAndUserId("home", user.userId)
            };

            return View(model);
        }
        public async Task<IActionResult> ApprovedHomeLoans()
        {
            var model = new PFLoanViewModel
            {
                pFLoans = await pFService.GetLoansByTypeAndStatus("home", 1)
            };
            return View(model);
        }

        public async Task<IActionResult> GetMyHomeLoan()
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);

            var emp = user.EmpCode;
            var model = new PFLoanViewModel
            {
                loans = await pFService.GetMyLoanByTypeAndEmpId("home", emp)
            };

            return View(model);
        }

        //public async Task<IActionResult> ApproveHomeLoan(int id)
        //{

        //    string userName = HttpContext.User.Identity.Name;
        //    var user = await _userManager.FindByNameAsync(userName);

        //    var loan = await pFService.GetPFLoanById(id);
        //    loan.approver = user.userId;
        //    await pFService.SavePFAdvance(loan);

        //    var loanlog = await pFService.GetLoanIdAndUserId(loan.Id, user.userId);
        //    loanlog.ispassed = 1;
        //    await pFService.SaveLoanLog(loanlog);
        //    return Json("ok");
        //}

        #endregion



        #region PFloanPayment
        public async Task<ActionResult> PFLoanPayment()
        {
            var salaryHead = await salaryService.GetAllSalaryHeadByFilter(string.Empty);
            PFLoanPaymentViewModel model = new PFLoanPaymentViewModel
            {
                pFLoanPayments = await pFService.GetAllPFLoanPayment(),
                visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData()
                //salaryHeads = salaryHead.Where(a => a.isAdvance == "Yes"),
                //salaryPeriods = await salaryService.GetAllSalaryPeriod()

            };
            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PFLoanPayment([FromForm] PFLoanPaymentViewModel model)
        {


            PFLoanPayment data = new PFLoanPayment
            {
                Id = model.PfloanpaymentId,
                employeeInfoId = model.employeeInfoId,
                paymentAmount = model.paymentAmount,
                paymentDate = model.paymentDate,
                pFLoanId = model.pFLoanId

            };

            int pfid = await pFService.SavePFLoanPayment(data);
            try
            {

                await pFService.UpdatePFScheduleByPaymentId(pfid);
                await pFService.UpdatePFSchedulePaymentByPaymentId((int)model.pFLoanId, (int)model.installmentno, pfid);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction(nameof(PFLoanPayment));
        }

        #endregion
        public async Task<ActionResult> Index()
        {
            var docInfo = await attachmentCommentService.GetDocumentAttachmentList("FDRInvestment", "ChecqueFile", 10);
            if (docInfo == null)
            {
                docInfo = new List<DocumentPhotoAttachment>();
            }
            FDRInvestmentViewModel model = new FDRInvestmentViewModel
            {

                fdrInvestments = await pFService.GetAllFdrInvestment(),
                documentPhotoAttachments = docInfo
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] FDRInvestmentViewModel model)
        {


            FdrInvestment data = new FdrInvestment
            {
                Id = (int)model.FdrInvestmentId,
                FromBank = model.FromBank,
                FromBranch = model.FromBranch,
                AccountNumber = model.AccountNumber,
                ToBank = model.ToBank,
                ToBranch = model.ToBranch,
                ChequeNo = model.ChequeNo,
                OpenDate = model.OpenDate,
                MaturityDate = model.MaturityDate,
                Amount = model.Amount,
                Rate = model.Rate,
                Interest = model.Interest,
                Tenure = model.Tenure
            };

            int fdrid = await pFService.SaveFdrInvestment(data);

            if (model.ChequeFile != null)
            {

                string userName = User.Identity.Name;
                string documentType = "ChecqueFile";
                var filesPath = $"{_hostingEnvironment.WebRootPath}/Upload/Photo";
                var fileName = ContentDispositionHeaderValue.Parse(model.ChequeFile.ContentDisposition).FileName;
                string fileType = Path.GetExtension(fileName);
                fileName = fileName.Contains("\\")
                    ? fileName.Trim('"').Substring(fileName.LastIndexOf("\\", StringComparison.Ordinal) + 1)
                    : fileName.Trim('"');

                string NewFileName = fdrid + "_" + documentType + "_" + fileName;
                string fileLocation = "/Upload/Photo/" + NewFileName;
                var fullFilePath = Path.Combine(filesPath, NewFileName);

                using (var stream = new FileStream(fullFilePath, FileMode.Create))
                {
                    await model.ChequeFile.CopyToAsync(stream);
                }

                DocumentPhotoAttachment dataF = new DocumentPhotoAttachment
                {
                    Id = 0,
                    actionType = "FDRInvestment",
                    actionTypeId = fdrid,
                    subject = "",
                    fileName = NewFileName,
                    filePath = fileLocation,
                    fileType = fileType,
                    description = "",
                    documentType = documentType,
                    moduleId = 10,
                    createdBy = userName
                };
                await attachmentCommentService.SaveDocumentAttachment(dataF);

            }

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> GetContactphotoByContactId(int Id)
        {
            return Json(await attachmentCommentService.GetDocumentAttachmentByActionIdContact(Id, "FDRInvestment", "ChecqueFile", 10));
        }
        [Route("global/api/GetAllScheduleByPFloanId/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetAllScheduleByPFloanId(int id)
        {
            return Json(await pFService.GetLoanScheduleViewModel(id));
        }
        [Route("global/api/GetAllPFLoanByPFEmployeeId/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetAllPFLoanByPFEmployeeId(int id)
        {
            return Json(await pFService.GetPFLoanByemployeeId(id));
        }
        [Route("global/api/GetPFLoanInfoByPFId/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetPFLoanInfoByPFId(int id)
        {
            return Json(await pFService.GetPFLoanById(id));
        }
    }
}
