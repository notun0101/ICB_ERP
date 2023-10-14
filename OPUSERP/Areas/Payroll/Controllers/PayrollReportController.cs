using ClosedXML.Excel;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.Payroll.Models;
using OPUSERP.Data;
using OPUSERP.Data.Entity;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.Payroll.Data.Entity.PF;
using OPUSERP.Payroll.Data.Entity.Salary;
using OPUSERP.Payroll.Services.Salary.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace OPUSERP.Areas.Payroll.Controllers
{
	[Area("Payroll")]
	[Authorize]
	public class PayrollReportController : Controller
	{
		private readonly ISalaryService salaryService;
		private readonly IPersonalInfoService personalInfoService;
		private readonly ISpecialBranchUnitService specialBranchUnitService;
		private readonly IERPCompanyService eRPCompanyService;
		private readonly IHostingEnvironment _hostingEnvironment;
		private readonly IUserInfoes userInfo;
		private readonly OPUSERP.ERPServices.MasterData.Interfaces.IDesignationDepartmentService designationDepartmentService;
		private readonly IEmployeePunchCardInfoService employeePunchCardInfoService;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ILocationService locationService;
		private readonly IStatusService statusService;
		private readonly IFunctionsInfoService functionsInfoService;
        private readonly ERPDbContext _context;

        private readonly string rootPath;
		private readonly MyPDF myPDF;
		public string FileName;

		public PayrollReportController(ISalaryService salaryService, IFunctionsInfoService functionsInfoService, IStatusService statusService, ILocationService locationService, UserManager<ApplicationUser> _userManager, IEmployeePunchCardInfoService employeePunchCardInfoService, IUserInfoes userInfo, IPersonalInfoService personalInfoService, ISpecialBranchUnitService specialBranchUnitService, IERPCompanyService eRPCompanyService, IHostingEnvironment _hostingEnvironment, IConverter converter, ERPDbContext context, OPUSERP.ERPServices.MasterData.Interfaces.IDesignationDepartmentService designationDepartmentService)
		{
			this.employeePunchCardInfoService = employeePunchCardInfoService;
			this.salaryService = salaryService;
			this.personalInfoService = personalInfoService;
			this.specialBranchUnitService = specialBranchUnitService;
			this.eRPCompanyService = eRPCompanyService;
			this.designationDepartmentService = designationDepartmentService;
			this.userInfo = userInfo;
			this._userManager = _userManager;
			this.locationService = locationService;
			this.statusService = statusService;
            _context = context;

            this._hostingEnvironment = _hostingEnvironment;
			myPDF = new MyPDF(_hostingEnvironment, converter);
			rootPath = _hostingEnvironment.ContentRootPath;
		}

		#region Gratuity

		//public async Task<ActionResult> GratuityView()
		public ActionResult GratuityView()
		{
			return View();
		}

		public ActionResult GratuityProcessedData()
		{
			ViewBag.datelist = salaryService.GetAllGratiutyProcessDataDates();
			return View();
		}

		public async Task<ActionResult> GratuityProcessed(DateTime datee)
		{
			var data = await salaryService.GetGratuityReport();
			foreach (var list in data)
			{
				var employee = await personalInfoService.GetEmployeeInfoByCode(list.employeeCode);
				GratiutyProcessData gratiutyProcessData = new GratiutyProcessData
				{
					employeeInfoId = employee.Id,
					designation = list.designation,
					date = datee,
					basic = list.basicAmount,
					year = list.fractionalYear,
					gratuity = list.gratuityAmount
				};
				await salaryService.SaveGratiutyProcessData(gratiutyProcessData);
			}

			return RedirectToAction(nameof(GratuityProcessedData));
		}

		[AllowAnonymous]
		[Route("global/api/GetGratuityData")]
		[HttpGet]
		public async Task<IActionResult> GetGratuityData()
		{
			return Json(await salaryService.GetGratuityReport());
		}

		[AllowAnonymous]
		[Route("global/api/GetAllGratuityReportViewModel")]
		[HttpGet]
		public async Task<IActionResult> GetAllGratuityReportViewModel()
		{
			return Json(await salaryService.GetAllGratuityReportViewModel());
		}

		[AllowAnonymous]
		[Route("global/api/GetAllGratuityReportViewModelByDate/{date}")]
		[HttpGet]
		public async Task<IActionResult> GetAllGratuityReportViewModelByDate(DateTime datee)
		{
			return Json(await salaryService.GetAllGratuityReportViewModelByDate(datee));
		}

		#endregion

		#region Reconciliation

		public async Task<ActionResult> Reconciliation()
		{
			PayrollReportViewModel model = new PayrollReportViewModel
			{
				salaryPeriods = await salaryService.GetAllSalaryPeriod()
			};

			return View(model);
		}

		#endregion

		#region Report
		public async Task<ActionResult> Index()
		{
			PayrollReportViewModel model = new PayrollReportViewModel
			{
				salaryPeriods = await salaryService.GetAllSalaryPeriod(),
				specialBranchUnits = await specialBranchUnitService.GetSpecialBranchUnit(),
				taxYears = await salaryService.GetAllTaxYear(),
				hrBranches = await salaryService.GetAllHrBranchs(),
				departments = await salaryService.GetAllDepartments(),
			};

			return View(model);
		}

		public async Task<ActionResult> PayslipView()
		{
			string userName = HttpContext.User.Identity.Name;
			var userInfos = await userInfo.GetUserInfoByUser(userName);
			ViewBag.EmployeeId = userInfos.EmployeeId;

			PayrollReportViewModel model = new PayrollReportViewModel
			{
				salaryPeriods = await salaryService.GetAllSalaryPeriod(),
				specialBranchUnits = await specialBranchUnitService.GetSpecialBranchUnit(),
				taxYears = await salaryService.GetAllTaxYear()
			};

			return View(model);
		}

		public async Task<ActionResult> TaxCertificateView()
		{
			string userName = HttpContext.User.Identity.Name;
			var userInfos = await userInfo.GetUserInfoByUser(userName);
			ViewBag.EmployeeId = userInfos.EmployeeId;

			PayrollReportViewModel model = new PayrollReportViewModel
			{
				taxYears = await salaryService.GetAllTaxYear()
			};

			return View(model);
		}

		public async Task<ActionResult> WagesIndex()
		{
			PayrollReportViewModel model = new PayrollReportViewModel
			{
				salaryPeriods = await salaryService.GetAllSalaryPeriod(),
				specialBranchUnits = await specialBranchUnitService.GetSpecialBranchUnit(),
				taxYears = await salaryService.GetAllTaxYear()
			};

			return View(model);
		}

		#endregion

		#region Grameen Report
		public async Task<ActionResult> IndexNew()
		{
			PayrollReportViewModel model = new PayrollReportViewModel
			{
				Departments = await designationDepartmentService.GetDepartment(),
				salaryPeriods = await salaryService.GetAllSalaryPeriod(),
				specialBranchUnits = await specialBranchUnitService.GetSpecialBranchUnit(),
				taxYears = await salaryService.GetAllTaxYear()
			};

			return View(model);
		}

		public async Task<IActionResult> DepartamentWiseReport(string rptType, int? sbuId, int? pnsId, int salaryPeriodId)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string fileName;

			if (rptType == "MWSS" || rptType == "MWSSBWC" || rptType == "MWBS")
			{
				var data = await salaryService.GetReportFormatByReportType(rptType);
				url = scheme + "://" + host + "/Payroll/PayrollReport/MonthlySalaryReportPdf_Grammen_Dept" + "?salaryPeriodId=" + salaryPeriodId + "&sbuId=" + sbuId + "&pnsId=" + pnsId;
				//url = scheme + "://" + host + "/Payroll/PayrollReport/" + data.FirstOrDefault().reportFormat + "?salaryPeriodId=" + salaryPeriodId + "&sbuId=" + sbuId + "&pnsId=" + pnsId;
				//url = scheme + "://" + host + "/Payroll/PayrollReport/MonthlySalaryReportPdf_RedCross?salaryPeriodId=" + salaryPeriodId;
			}

			string status = myPDF.GenerateLandscapePDF_WithPad(out fileName, url);

			FileName = fileName;
			if (status != "done")
			{
				return Content("<h1>" + status + "</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}

		public async Task<IActionResult> MonthlySalaryReportPdf_Grammen_Dept(int salaryPeriodId, int? sbuId, int? pnsId)
		{
			try
			{
				var model = new PayrollReportViewModel
				{
					monthlySalaryReportViewModels = await salaryService.GetMonthlySalaryReportByPeriodId(salaryPeriodId, sbuId, pnsId),
					companies = await eRPCompanyService.GetAllCompany(),
				};
				return PartialView(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		#endregion

		#region Report SubMonthlySalaryReportView

		[AllowAnonymous]
		public async Task<IActionResult> SubMonthlySalaryReportView(int salaryPeriodId, int? sbuId, int? pnsId)
		{
			try
			{
				var model = new PayrollReportViewModel
				{
					monthlySalaryReportViewModels = await salaryService.GetMonthlySalaryReportByPeriodId(salaryPeriodId, sbuId, pnsId),
					companies = await eRPCompanyService.GetAllCompany(),
				};
				return PartialView(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        public async Task<IActionResult> SalaryCertificate(string rptType)
        {
            var emp = await personalInfoService.GetEmployeeInfoByCode(User.Identity.Name);
            ViewBag.rptType = rptType;
            PayrollReportViewModel model = new PayrollReportViewModel
            {
                salaryPeriods = await salaryService.GetAllSalaryPeriod(),
                employeeInfo = emp
            };
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> SalaryCertificateReportView(int employeeInfoId, int salaryPeriodId)
        {
            try
            {
                var model = new PayrollReportViewModel
                {
                    payslipAdditionViewModels = await salaryService.GetPaySlipAdditionByEmpId(employeeInfoId, salaryPeriodId),
                    payslipDeductionViewModels = await salaryService.GetPaySlipDeductionByEmpId(employeeInfoId, salaryPeriodId),
                    companies = await eRPCompanyService.GetAllCompany(),
                };
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [AllowAnonymous]
        public async Task<IActionResult> SalaryCertificateReport(string rptType, int employeeInfoId, int salaryPeriodId)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string fileName;

            if (rptType == "SaCertificate")
            {
                var data = await salaryService.GetReportFormatByReportType(rptType);
                url = scheme + "://" + host + "/Payroll/PayrollReport/SalaryCertificateReportView?employeeInfoId=" + employeeInfoId + "&salaryPeriodId=" + salaryPeriodId;
            }

            string status = myPDF.GeneratePDF(out fileName, url);

            FileName = fileName;
            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }


        #endregion

        #region PDF Mehod
        [AllowAnonymous]
		public async Task<IActionResult> SalaryReport(string rptType, int employeeInfoId, int salaryPeriodId)
		{
            try
            {
                var username = HttpContext.User.Identity.Name;
                var empId = await personalInfoService.GetEmployeeIdByUsername(username);
                var isvalid = false;
                var arrearEmp = _context.EmployeeArrearInfo.Where(x => x.employeeId == employeeInfoId).Count();
                var arrearSLIP = "ARREARPSLIP";
                if (empId == employeeInfoId || User.IsInRole("UAT Admin") || User.IsInRole("HRAdmin") || User.IsInRole("UAT Payroll") || User.IsInRole("PayrollAdmin") || User.IsInRole("PaySlip"))
                {
                    isvalid = true;
                }
                string scheme = Request.Scheme;
                var host = Request.Host;
                string url = "";
                string fileName;
                string fileFormat = "";
                if (arrearEmp > 0)
                {
                    rptType = arrearSLIP;
                    fileFormat = "ArrearPayslipReportPdf";
                }
                else
                {
                    var data = await salaryService.GetReportFormatByReportType(rptType);
                    fileFormat = data.FirstOrDefault().reportFormat;
                }

                
               
                
                //var data = await salaryService.GetReportFormatByReportType(arrearSLIP);
                if (isvalid)
                {
                    url = scheme + "://" + host + "/Payroll/PayrollReport/" + fileFormat + "?employeeInfoId=" + employeeInfoId + "&salaryPeriodId=" + salaryPeriodId;
                }
                
                string status = myPDF.GeneratePDF(out fileName, url);

                FileName = fileName;
                if (status != "done")
                {
                    return Content("<h1>" + status + "</h1>");
                }

                var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
                return new FileStreamResult(stream, "application/pdf");
            }


            catch (Exception ex)
            {
                throw ex;
            }
		}


        [AllowAnonymous]
        public async Task<IActionResult> SalaryReportAPP(string username, string rptType, int employeeInfoId, int salaryPeriodId)
        {
            try
            {
                var empId = await personalInfoService.GetEmployeeIdByUsername(username);
                var isvalid = false;
                var arrearEmp = _context.EmployeeArrearInfo.Where(x => x.employeeId == employeeInfoId).Count();
                var arrearSLIP = "ARREARPSLIP";
                if (empId == employeeInfoId || User.IsInRole("UAT Admin") || User.IsInRole("HRAdmin") || User.IsInRole("UAT Payroll") || User.IsInRole("PayrollAdmin") || User.IsInRole("PaySlip"))
                {
                    isvalid = true;
                }
                string scheme = Request.Scheme;
                var host = Request.Host;
                string url = "";
                string fileName;
                string fileFormat = "";
                if (arrearEmp > 0)
                {
                    rptType = arrearSLIP;
                    fileFormat = "ArrearPayslipReportPdf";
                }
                else
                {
                    var data = await salaryService.GetReportFormatByReportType(rptType);
                    fileFormat = data.FirstOrDefault().reportFormat;
                }




                //var data = await salaryService.GetReportFormatByReportType(arrearSLIP);
                if (isvalid)
                {
                    url = scheme + "://" + host + "/Payroll/PayrollReport/" + fileFormat + "?employeeInfoId=" + employeeInfoId + "&salaryPeriodId=" + salaryPeriodId;
                }

                string status = myPDF.GeneratePDF(out fileName, url);

                FileName = fileName;
                if (status != "done")
                {
                    return Content("<h1>" + status + "</h1>");
                }

                var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
                return new FileStreamResult(stream, "application/pdf");
            }


            catch (Exception ex)
            {
                throw ex;
            }
        }


        [AllowAnonymous]
        public async Task<IActionResult> SalaryReportAPI(string rptType, int employeeInfoId, int salaryPeriodId)
        {
            var username = HttpContext.User.Identity.Name;
            var empId = await personalInfoService.GetEmployeeIdByUsername(username);
            var isvalid = true;

            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string fileName;

            if (rptType == "PSLIP")
            {
                var data = await salaryService.GetReportFormatByReportType(rptType);
                if (isvalid)
                {
                    url = scheme + "://" + host + "/Payroll/PayrollReport/" + data.FirstOrDefault().reportFormat + "?employeeInfoId=" + employeeInfoId + "&salaryPeriodId=" + salaryPeriodId;
                }
            }

            string status = myPDF.GeneratePDF(out fileName, url);

            FileName = fileName;
            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [AllowAnonymous]
		public async Task<IActionResult> WagesSalaryReport(string rptType, int employeeInfoId, int salaryPeriodId)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string fileName;

			if (rptType == "PSLIP")
			{
				var data = await salaryService.GetReportFormatByReportType(rptType);
				url = scheme + "://" + host + "/Payroll/PayrollReport/WagesPayslipReportPdf?employeeInfoId=" + employeeInfoId + "&salaryPeriodId=" + salaryPeriodId;
			}

			string status = myPDF.GeneratePDF(out fileName, url);

			FileName = fileName;
			if (status != "done")
			{
				return Content("<h1>" + status + "</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}

		[AllowAnonymous]
		public async Task<IActionResult> WagesMonthlySalaryReport(string rptType, int? sbuId, int? pnsId, int salaryPeriodId)
		{

			string scheme = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string fileName;

			if (rptType == "MWSS")
			{
				var data = await salaryService.GetReportFormatByReportType(rptType);
				url = scheme + "://" + host + "/Payroll/PayrollReport/WagesMonthlySalaryReportPdf?salaryPeriodId=" + salaryPeriodId + "&sbuId=" + sbuId + "&pnsId=" + pnsId;
				//url = scheme + "://" + host + "/Payroll/PayrollReport/MonthlySalaryReportPdf_RedCross?salaryPeriodId=" + salaryPeriodId;
			}

			string status = myPDF.GenerateLandscapePDF(out fileName, url);

			FileName = fileName;
			if (status != "done")
			{
				return Content("<h1>" + status + "</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}

		[AllowAnonymous]
		public async Task<IActionResult> MonthlySalaryReport(string rptType, int? sbuId, int? pnsId, int salaryPeriodId)
		{

			string scheme = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string fileName;

			if (rptType == "MWSS" || rptType == "MWSSBWC" || rptType == "MWBS" || rptType == "MWBSNew")
			{
				var data = await salaryService.GetReportFormatByReportType(rptType);
				url = scheme + "://" + host + "/Payroll/PayrollReport/" + data.FirstOrDefault().reportFormat + "?salaryPeriodId=" + salaryPeriodId + "&sbuId=" + sbuId + "&pnsId=" + pnsId;
				//url = scheme + "://" + host + "/Payroll/PayrollReport/MonthlySalaryReportPdf_RedCross?salaryPeriodId=" + salaryPeriodId;
			}

			string status = myPDF.GenerateLandscapePDF_WithPad(out fileName, url);

			FileName = fileName;
			if (status != "done")
			{
				return Content("<h1>" + status + "</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}

		[AllowAnonymous]
		public async Task<IActionResult> MonthlySalaryReportByDepartment(string rptType, int? departmentId, int? pnsId, int salaryPeriodId)
		{

			string scheme = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string fileName;

			if (rptType == "MWSS" || rptType == "MWSSD" || rptType == "MWSSB" || rptType == "MWSSBWC" || rptType == "MWBS")
			{
				var data = await salaryService.GetReportFormatByReportType(rptType);
				url = scheme + "://" + host + "/Payroll/PayrollReport/" + data.FirstOrDefault().reportFormat + "?salaryPeriodId=" + salaryPeriodId + "&departmentId=" + departmentId + "&pnsId=" + pnsId;
				//url = scheme + "://" + host + "/Payroll/PayrollReport/MonthlySalaryReportPdf_RedCross?salaryPeriodId=" + salaryPeriodId;
			}

			string status = myPDF.GenerateLandscapePDF_WithPad(out fileName, url);

			FileName = fileName;
			if (status != "done")
			{
				return Content("<h1>" + status + "</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}
		[AllowAnonymous]
		public async Task<IActionResult> MonthlySalaryReportByHrBranch(string rptType, int? hrbranchId, int? pnsId, int salaryPeriodId)
		{

			string scheme = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string fileName;

			if (rptType == "MWSS" || rptType == "MWSSD" || rptType == "MWSSB" || rptType == "MWSSBNew" || rptType == "MWSSONew" || rptType == "MWSSUNew" || rptType == "MWSSZNew" || rptType == "MWSSBWC" || rptType == "MWBS")
			{
				var data = await salaryService.GetReportFormatByReportType(rptType);
				url = scheme + "://" + host + "/Payroll/PayrollReport/" + data.FirstOrDefault().reportFormat + "?salaryPeriodId=" + salaryPeriodId + "&hrbranchId=" + hrbranchId + "&pnsId=" + pnsId;
			}
			if (rptType == "MWSSheet")
			{
				url = scheme + "://" + host + "/Payroll/PayrollReport/MonthlySalarySummaryReportPdf_BranchNew?salaryPeriodId=" + salaryPeriodId + "&hrbranchId=" + 0 + "&pnsId=" + pnsId;
			}

			string status = myPDF.GenerateLandscapePDF_ForSalarySheet(out fileName, url);

			FileName = fileName;
			if (status != "done")
			{
				return Content("<h1>" + status + "</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}

		[AllowAnonymous]
		public async Task<IActionResult> MonthlySalaryReportByDpt(string rptType, int? departmentId, int? pnsId, int salaryPeriodId)
		{

			string scheme = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string fileName;

			if (rptType == "MWSS" || rptType == "MWSSD" || rptType == "MWSSB" || rptType == "MWSSBNew" || rptType == "MWSSDNew" || rptType == "MWSSONew" || rptType == "MWSSUNew" || rptType == "MWSSZNew" || rptType == "MWSSBWC" || rptType == "MWBS")
			{
				var data = await salaryService.GetReportFormatByReportType(rptType);
				url = scheme + "://" + host + "/Payroll/PayrollReport/" + data.FirstOrDefault().reportFormat + "?salaryPeriodId=" + salaryPeriodId + "&departmentId=" + departmentId + "&pnsId=" + pnsId;
				//url = scheme + "://" + host + "/Payroll/PayrollReport/MonthlySalaryReportPdf_RedCross?salaryPeriodId=" + salaryPeriodId;
			}

			string status = myPDF.GenerateLandscapePDF_ForSalarySheet(out fileName, url);

			FileName = fileName;
			if (status != "done")
			{
				return Content("<h1>" + status + "</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}

		public async Task<IActionResult> PaySlip(string rptType)
		{
			var emp = await personalInfoService.GetEmployeeInfoByCode(User.Identity.Name);
			ViewBag.rptType = rptType;
			PayrollReportViewModel model = new PayrollReportViewModel
			{
				salaryPeriods = await salaryService.GetAllSalaryPeriod(),
				employeeInfo = emp
			};
			return View(model);
		}
        public async Task<IActionResult> PaySlipSecurities()
        {

            var emp = await personalInfoService.GetEmployeeInfoByCode(User.Identity.Name);
            PayrollReportViewModel model = new PayrollReportViewModel
            {
                salaryPeriods = await salaryService.GetAllSalaryPeriod(),
                employeeInfo = emp
            };
            return View(model);
        }
        [AllowAnonymous]
        public async Task<ActionResult> SpecialReportView(string rptType, int salaryPeriodId)
        {
            PayrollReportViewModel model = new PayrollReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                specialReportVms = await salaryService.GetSecuritiesLtdReprotByTypeAndPeriod(rptType, salaryPeriodId)

            };

            return View(model);
        }
        [AllowAnonymous]
        public IActionResult SpecialReportPdf(string rptType, int salaryPeriodId)
        {
            string schema = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string filename;

            url = schema+"://" + host + "/Payroll/PayrollReport/SpecialReportView?rptType=" + rptType + "&salaryPeriodId=" + salaryPeriodId;

            string status = myPDF.GeneratePDF(out filename, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }

        [AllowAnonymous]
        public async Task<ActionResult> BISLView(string rptType, int salaryPeriodId)
        {
            PayrollReportViewModel model = new PayrollReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                specialReportVms = await salaryService.GetSecuritiesLtdReprotByTypeAndPeriod(rptType, salaryPeriodId)

            };

            return View(model);
        }
        [AllowAnonymous]
        public IActionResult BISLReportPdf(string rptType, int salaryPeriodId)
        {
            string schema = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string filename;

            url = schema+"://" + host + "/Payroll/PayrollReport/BISLView?rptType=" + rptType + "&salaryPeriodId=" + salaryPeriodId;

            string status = myPDF.GeneratePDF(out filename, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }


        public async Task<IActionResult> MonthWiseSalarySheet(string rptType)
		{
			ViewBag.rptType = rptType;
			PayrollReportViewModel model = new PayrollReportViewModel
			{
				salaryPeriods = await salaryService.GetAllSalaryPeriod(),
				specialBranchUnits = await specialBranchUnitService.GetSpecialBranchUnit(),
				hrBranches = await salaryService.GetAllHrBranchs(),
				departments = await salaryService.GetAllDepartments(),
				functionInfos = await salaryService.GetAllOffices(),
				hrUnits = await salaryService.GetAllHrUnits(),
				locations = await salaryService.GetAllZones()
			};
			return View(model);
		}
		public async Task<IActionResult> MonthWiseSalaryDisburse(string rptType)
		{
			ViewBag.rptType = rptType;
			PayrollReportViewModel model = new PayrollReportViewModel
			{
				salaryPeriods = await salaryService.GetAllSalaryPeriod(),
				specialBranchUnits = await specialBranchUnitService.GetSpecialBranchUnit(),


			};
			return View(model);
		}

		public async Task<IActionResult> BonusReports() {
			var data = new PayrollReportViewModel
			{
				hrBranches = await salaryService.GetAllHrBranchs(),
				locations = await salaryService.GetAllZones(),
				salaryPeriods = await salaryService.GetAllSalaryPeriod()
			};
			return View(data);
		}
		public async Task<IActionResult> MonthWiseBonus(string rptType)
		{
			ViewBag.rptType = rptType;
			PayrollReportViewModel model = new PayrollReportViewModel
			{
				salaryPeriods = await salaryService.GetAllSalaryPeriod(),
				specialBranchUnits = await specialBranchUnitService.GetSpecialBranchUnit(),


			};
			return View(model);
		}




		[AllowAnonymous]
		public async Task<IActionResult> BankStatementReport(string rptType, int? sbuId, int? pnsId, int salaryPeriodId)
		{

			string scheme = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string fileName;

			if (rptType == "BNKSTMNT" || rptType == "WALLSTMNT" || rptType == "CASHSTMNT")
			{
				var data = await salaryService.GetReportFormatByReportType(rptType);
				url = scheme + "://" + host + "/Payroll/PayrollReport/" + data.FirstOrDefault().reportFormat + "?salaryPeriodId=" + salaryPeriodId + "&sbuId=" + sbuId + "&pnsId=" + pnsId;
			}

			string status = myPDF.GeneratePDF_WithPAD(out fileName, url);

			FileName = fileName;
			if (status != "done")
			{
				return Content("<h1>" + status + "</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}

		[AllowAnonymous]
		public async Task<IActionResult> BankStatementReport_PAD(string rptType, int? sbuId, int? pnsId, int salaryPeriodId)
		{

			string scheme = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string fileName;

			if (rptType == "BNKSTMNTWTHOUTPAD" || rptType == "BNKAPP")
			{
				var data = await salaryService.GetReportFormatByReportType(rptType);
				url = scheme + "://" + host + "/Payroll/PayrollReport/" + data.FirstOrDefault().reportFormat + "?salaryPeriodId=" + salaryPeriodId + "&sbuId=" + sbuId + "&pnsId=" + pnsId;
			}

			string status = myPDF.GeneratePDF_WithoutPAD(out fileName, url);

			FileName = fileName;
			if (status != "done")
			{
				return Content("<h1>" + status + "</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}

		[AllowAnonymous]
		public async Task<IActionResult> BankStatementReportExcel(string rptType, int? sbuId, int? pnsId, int salaryPeriodId)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string fileName;

			if (rptType == "BNKSTMNT" || rptType == "BNKSTMNTWTHOUTPAD" || rptType == "BNKAPP" || rptType == "WALLSTMNT" || rptType == "CASHSTMNT")
			{
				var data = await salaryService.GetReportFormatByReportType(rptType);
				url = scheme + "://" + host + "/Payroll/PayrollReport/" + data.FirstOrDefault().reportFormat + "?salaryPeriodId=" + salaryPeriodId + "&sbuId=" + sbuId + "&pnsId=" + pnsId;
			}

			string status = myPDF.GeneratePDF(out fileName, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}

			string pdfpath = rootPath + @"\wwwroot\pdf\" + fileName;
			//string wordpath = fileName.Replace(".pdf", ".xls");
			string wordpath = fileName.Replace(".pdf", ".csv");
			SautinSoft.PdfFocus f = new SautinSoft.PdfFocus();
			f.OpenPdf(rootPath + @"\wwwroot\pdf\" + fileName);
			f.ToExcel(rootPath + @"\wwwroot\pdf\" + wordpath);
			var model = wordpath;
			return Json(model);
		}

		// IFRC SUB OFFICE REPORT

		[AllowAnonymous]
		public async Task<IActionResult> SubMonthlySalaryReportPdf(string rptType, int? sbuId, int? pnsId, int salaryPeriodId)
		{

			string scheme = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string fileName;

			if (rptType == "MWSS789")
			{
				var data = await salaryService.GetReportFormatByReportType(rptType);
				url = scheme + "://" + host + "/Payroll/PayrollReport/" + data.FirstOrDefault().reportFormat + "?salaryPeriodId=" + salaryPeriodId + "&sbuId=" + sbuId + "&pnsId=" + pnsId;
				//url = scheme + "://" + host + "/Payroll/PayrollReport/MonthlySalaryReportPdf_RedCross?salaryPeriodId=" + salaryPeriodId;
			}

			string status = myPDF.GenerateLandscapePDF(out fileName, url);

			FileName = fileName;
			if (status != "done")
			{
				return Content("<h1>" + status + "</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}

		[AllowAnonymous]
		[Route("api/global/PayslipAPI/{empCode}/{salaryPeriodId}")]
		public async Task<IActionResult> PayslipReportAPI(string empCode, int salaryPeriodId)
		{
			try
			{
				var emp = await personalInfoService.GetEmployeeIdByCode(empCode);
				var model = new PaySlipApiVm
				{
					payslipReportViewModels = await salaryService.GetPaySlipByEmpId(emp, salaryPeriodId),
					payslipAdditionViewModels = await salaryService.GetPaySlipAdditionByEmpIdApi(emp, salaryPeriodId),
					payslipDeductionViewModels = await salaryService.GetPaySlipDeductionByEmpIdApi(emp, salaryPeriodId)
				};
				return Json(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[AllowAnonymous]
		public async Task<IActionResult> PayslipReportPdf(int employeeInfoId, int salaryPeriodId)
		{
			try
			{
				//ViewBag.postingPlace = await salaryService.GetPostingByEmpId(employeeInfoId);

				var model = new PayrollReportViewModel
				{
					payslipReportViewModels = await salaryService.GetPaySlipByEmpId(employeeInfoId, salaryPeriodId),
					payslipAdditionViewModels = await salaryService.GetPaySlipAdditionByEmpId(employeeInfoId, salaryPeriodId),
					payslipDeductionViewModels = await salaryService.GetPaySlipDeductionByEmpId(employeeInfoId, salaryPeriodId),
					companies = await eRPCompanyService.GetAllCompany(),
					//visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData()
				};
				return View(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


		[AllowAnonymous]
		public async Task<IActionResult> WagesPayslipReportPdf(int employeeInfoId, int salaryPeriodId)
		{
			try
			{
				var model = new PayrollReportViewModel
				{
					payslipReportViewModels = await salaryService.GetWagesPaySlipByEmpId(employeeInfoId, salaryPeriodId),
					payslipAdditionViewModels = await salaryService.GetWagesPaySlipAdditionByEmpId(employeeInfoId, salaryPeriodId),
					payslipDeductionViewModels = await salaryService.GetWagesPaySlipDeductionByEmpId(employeeInfoId, salaryPeriodId),
					companies = await eRPCompanyService.GetAllCompany(),
				};
				return PartialView(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


		[AllowAnonymous]
		public async Task<IActionResult> PayslipReportPdf_RedCross(int employeeInfoId, int salaryPeriodId)
		{
			try
			{
				var model = new PayrollReportViewModel
				{
					payslipReportViewModels = await salaryService.GetPaySlipByEmpId(employeeInfoId, salaryPeriodId),
					payslipAdditionViewModels = await salaryService.GetPaySlipAdditionByEmpId(employeeInfoId, salaryPeriodId),
					payslipDeductionViewModels = await salaryService.GetPaySlipDeductionByEmpId(employeeInfoId, salaryPeriodId),
					companies = await eRPCompanyService.GetAllCompany(),
				};
				return PartialView(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		[AllowAnonymous]
		public async Task<IActionResult> UniversalCodePdfAction(string rptType, int employeeInfoId, int salaryPeriodId)
		{

			string scheme = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string fileName;

			if (rptType == "UCP")
			{
				var data = await salaryService.GetReportFormatByReportType(rptType);
				url = scheme + "://" + host + "/Payroll/PayrollReport/" + data.FirstOrDefault().reportFormat + "?employeeInfoId=" + employeeInfoId + "&salaryPeriodId=" + salaryPeriodId;
				//url = scheme + "://" + host + "/Payroll/PayrollReport/MonthlySalaryReportPdf_RedCross?salaryPeriodId=" + salaryPeriodId;
			}

			string status = myPDF.GenerateLandscapePDF_A3(out fileName, url);

			FileName = fileName;
			if (status != "done")
			{
				return Content("<h1>" + status + "</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}

		#endregion

		#region Monthly Salary Report

		[AllowAnonymous]
		public async Task<IActionResult> UniversalCodePdf(int employeeInfoId, int salaryPeriodId)
		{
			try
			{
				var model = new PayrollReportViewModel
				{
					salaryPeriods = await salaryService.GetSalaryPeriods(),
					universalCodaXLTempleteViewModels = await salaryService.GetUniversalCodaXLTempleteViewModels(salaryPeriodId, employeeInfoId),
					companies = await eRPCompanyService.GetAllCompany(),
				};
				return PartialView(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[AllowAnonymous]
		public async Task<IActionResult> MonthlySalaryReportPdf(int salaryPeriodId, int? sbuId, int? pnsId)
		{
			try
			{
				var model = new PayrollReportViewModel
				{
					monthlySalaryReportViewModels = await salaryService.GetMonthlySalaryReportByPeriodId(salaryPeriodId, sbuId, pnsId),
					companies = await eRPCompanyService.GetAllCompany(),
				};
				return PartialView(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[AllowAnonymous]
		public async Task<IActionResult> WagesMonthlySalaryReportPdf(int salaryPeriodId, int? sbuId, int? pnsId)
		{
			try
			{
				var model = new PayrollReportViewModel
				{
					monthlySalaryReportViewModels = await salaryService.GetWagesMonthlySalaryReportByPeriodId(salaryPeriodId, sbuId, pnsId),
					companies = await eRPCompanyService.GetAllCompany(),
				};
				return PartialView(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


		[AllowAnonymous]
		public async Task<IActionResult> MonthlySalaryReportPdf_RedCross(int salaryPeriodId, int? sbuId, int? pnsId)
		{
			try
			{
				var model = new PayrollReportViewModel
				{
					monthlySalaryReportViewModels = await salaryService.GetMonthlySalaryReportByPeriodId(salaryPeriodId, sbuId, pnsId),
					companies = await eRPCompanyService.GetAllCompany(),
				};
				return PartialView(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[AllowAnonymous]
		public async Task<IActionResult> MonthlySalaryReportPdf_BnB(int salaryPeriodId, int? sbuId, int? pnsId)
		{
			try
			{
				var model = new PayrollReportViewModel
				{
					monthlySalaryReportViewModels = await salaryService.GetMonthlySalaryReportByPeriodId(salaryPeriodId, sbuId, pnsId),
					companies = await eRPCompanyService.GetAllCompany(),
				};
				return PartialView(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		//Designation Wise
		[AllowAnonymous]
		public async Task<IActionResult> MonthlySalaryReportPdf_BnB_V2(int salaryPeriodId, int? sbuId, int? pnsId)
		{
			try
			{
				var data = new List<SalaryWithDesignation>();
				var designations = await salaryService.GetAllDesignations();
				var monthlySalaryReportViewModels = await salaryService.GetMonthlySalaryReportByPeriodId(salaryPeriodId, sbuId, pnsId);
				foreach (var item in designations)
				{
					data.Add(new SalaryWithDesignation
					{
						designation = item,
						monthlySalaryReports = monthlySalaryReportViewModels.Where(x => x.designation == item.designationName)
					});
				}
				var model = new PayrollReportViewModel
				{
					companies = await eRPCompanyService.GetAllCompany(),
					designations = await salaryService.GetAllDesignations(),
					salaryWithDesignations = data,
					monthlySalaryReportViewModels = await salaryService.GetMonthlySalaryReportByPeriodId(salaryPeriodId, sbuId, pnsId)
				};
				return View(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		[AllowAnonymous]
		public async Task<IActionResult> MonthlySalaryReportPdf_Dept(int salaryPeriodId, int? departmentId, int? pnsId)
		{
			try
			{
				var data = new List<SalaryWithDesignation>();
				var designations = await salaryService.GetAllDesignations();
				var monthlySalaryReportViewModels = await salaryService.GetMonthlySalaryReportByPeriodIdDept(salaryPeriodId, departmentId, pnsId);
				foreach (var item in designations)
				{
					data.Add(new SalaryWithDesignation
					{
						designation = item,
						monthlySalaryReports = monthlySalaryReportViewModels.Where(x => x.designation == item.designationName)
					});
				}
				var model = new PayrollReportViewModel
				{
					companies = await eRPCompanyService.GetAllCompany(),
					designations = await salaryService.GetAllDesignations(),
					salaryWithDesignations = data,
					monthlySalaryReportViewModels = await salaryService.GetMonthlySalaryReportByPeriodIdDept(salaryPeriodId, departmentId, pnsId)
				};
				return View(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		[AllowAnonymous]
		public async Task<IActionResult> MonthlySalaryReportPdf_Branch(int salaryPeriodId, int? hrbranchId, int? pnsId)
		{
			try
			{
				if (hrbranchId > 0)
				{
					ViewBag.branchName = await personalInfoService.GetBranchNameById(Convert.ToInt32(hrbranchId));
				}
				else
				{
					ViewBag.branchName = null;
				}
				var data = new List<SalaryWithDesignation>();
				var designations = await salaryService.GetAllDesignations();
				var monthlySalaryReportViewModels = await salaryService.GetMonthlySalaryReportByPeriodIdBranch(salaryPeriodId, hrbranchId, pnsId);
				foreach (var item in designations)
				{
					data.Add(new SalaryWithDesignation
					{
						designation = item,
						monthlySalaryReports = monthlySalaryReportViewModels.Where(x => x.designation == item.designationName)
					});
				}
				var model = new PayrollReportViewModel
				{
					companies = await eRPCompanyService.GetAllCompany(),
					designations = await salaryService.GetAllDesignations(),
					salaryWithDesignations = data,
					monthlySalaryReportViewModels = await salaryService.GetMonthlySalaryReportByPeriodIdBranch(salaryPeriodId, hrbranchId, pnsId)
				};
				return View(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[AllowAnonymous]
		public async Task<IActionResult> MonthlySalaryReportView_AllInd(int salaryPeriodId, string type)
		{
			try
			{
				var data = new List<SalaryWithDesignation>();
				var designations = await salaryService.GetAllDesignations();
				
				var model = new PayrollReportViewModel
				{
					companies = await eRPCompanyService.GetAllCompany(),
					designations = designations,
					salaryWithDesignations = data,
					allSalarySheetVms = await salaryService.GetAllSalarySheetVms(salaryPeriodId, type)
					//branchMonthlySalaryReportViewModels = monthlySalaryReportViewModels
				};
				return View(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[AllowAnonymous]
		public IActionResult MonthlySalaryReportPdf_AllInd(int salaryPeriodId, string type)
		{
			string schema = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string filename;

			url = schema+"://" + host + "/Payroll/PayrollReport/MonthlySalaryReportView_AllInd?salaryPeriodId=" + salaryPeriodId + "&type=" + type;

			string status = myPDF.GenerateLandscapePDF(out filename, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}

		[AllowAnonymous]
		public async Task<IActionResult> MonthlySalaryReportPdf_BranchNew(int salaryPeriodId, int? hrbranchId, int? pnsId)
		{
			try
			{
				if (hrbranchId > 0)
				{
					ViewBag.branchName = await personalInfoService.GetBranchNameById(Convert.ToInt32(hrbranchId));
				}
				else
				{
					ViewBag.branchName = null;
				}
				var data = new List<SalaryWithDesignation>();
				var designations = await salaryService.GetAllDesignations();
				var monthlySalaryReportViewModels = await salaryService.GetMonthlySalaryReportByPeriodIdBranchNew(salaryPeriodId, hrbranchId, pnsId);
				foreach (var item in designations)
				{
					data.Add(new SalaryWithDesignation
					{
						designation = item,
						branchMonthlySalaryReports = monthlySalaryReportViewModels.Where(x => x.designation == item.designationName)
					});
				}
				var model = new PayrollReportViewModel
				{
					companies = await eRPCompanyService.GetAllCompany(),
					designations = designations,
					salaryWithDesignations = data,
					branchMonthlySalaryReportViewModels = monthlySalaryReportViewModels
				};
				return View(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[AllowAnonymous]
		public async Task<IActionResult> MonthlySalarySummaryReportPdf_BranchNew(int salaryPeriodId, int? hrbranchId, int? pnsId)
		{
			try
			{
				if (hrbranchId > 0)
				{
					ViewBag.branchName = await personalInfoService.GetBranchNameById(Convert.ToInt32(hrbranchId));
				}
				else
				{
					ViewBag.branchName = null;
				}
				var data = new List<SalaryWithDesignation>();
				var designations = await salaryService.GetAllDesignations();
				var monthlySalaryReportViewModels = await salaryService.GetMonthlySalaryReportByPeriodIdBranchNew(salaryPeriodId, hrbranchId, pnsId);
				foreach (var item in designations)
				{
					data.Add(new SalaryWithDesignation
					{
						designation = item,
						branchMonthlySalaryReports = monthlySalaryReportViewModels.Where(x => x.designation == item.designationName)
					});
				}
				var model = new PayrollReportViewModel
				{
					companies = await eRPCompanyService.GetAllCompany(),
					designations = designations,
					salaryWithDesignations = data,
					branchMonthlySalaryReportViewModels = monthlySalaryReportViewModels
				};
				return View(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[AllowAnonymous]
		public async Task<IActionResult> MonthlySalaryReportPdf_DeptNew(int salaryPeriodId, int? departmentId, int? pnsId)
		{
			try
			{
				if (departmentId > 0)
				{
					ViewBag.departmentName = await personalInfoService.GetDepartmentNameById(Convert.ToInt32(departmentId));
				}
				else
				{
					ViewBag.departmentName = null;
				}
				var data = new List<SalaryWithDesignation>();
				var designations = await salaryService.GetAllDesignations();
				var monthlySalaryReportViewModels = await salaryService.GetMonthlySalaryReportByPeriodIdDeptNew(salaryPeriodId, departmentId, pnsId);
				foreach (var item in designations)
				{
					data.Add(new SalaryWithDesignation
					{
						designation = item,
						branchMonthlySalaryReports = monthlySalaryReportViewModels.Where(x => x.designation == item.designationName)
					});
				}
				var model = new PayrollReportViewModel
				{
					companies = await eRPCompanyService.GetAllCompany(),
					designations = await salaryService.GetAllDesignations(),
					salaryWithDesignations = data,
					branchMonthlySalaryReportViewModels = await salaryService.GetMonthlySalaryReportByPeriodIdDeptNew(salaryPeriodId, departmentId, pnsId)
				};
				return View(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<IActionResult> MonthlySalaryReportPdf_OfficeNew(int salaryPeriodId, int? hrBranchId, int? pnsId)
		{
			try
			{
				if (hrBranchId > 0)
				{
					ViewBag.officeName = await personalInfoService.GetOfficeNameById(Convert.ToInt32(hrBranchId));
				}
				else
				{
					ViewBag.officeName = null;
				}
				var data = new List<SalaryWithDesignation>();
				var designations = await salaryService.GetAllDesignations();
				var monthlySalaryReportViewModels = await salaryService.GetMonthlySalaryReportByPeriodIdOfficeNew(salaryPeriodId, hrBranchId, pnsId);
				//foreach (var item in designations)
				//{
				//	data.Add(new SalaryWithDesignation
				//	{
				//		designation = item,
				//		branchMonthlySalaryReports = monthlySalaryReportViewModels.Where(x => x.designation == item.designationName)
				//	});
				//}

				var model = new PayrollReportViewModel
				{
					companies = await eRPCompanyService.GetAllCompany(),
					designations = await salaryService.GetAllDesignations(),
					salaryWithDesignations = data,
					branchMonthlySalaryReportViewModels = monthlySalaryReportViewModels
				};
				return View(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<IActionResult> MonthlySalaryReportPdf_UnitNew(int salaryPeriodId, int? hrBranchId, int? pnsId)
		{
			try
			{
				if (hrBranchId > 0)
				{
					ViewBag.hrunitName = await personalInfoService.GetHrUnitNameById(Convert.ToInt32(hrBranchId));
				}
				else
				{
					ViewBag.hrunitName = null;
				}
				var data = new List<SalaryWithDesignation>();
				var designations = await salaryService.GetAllDesignations();
				var monthlySalaryReportViewModels = await salaryService.GetMonthlySalaryReportByPeriodIdUnitNew(salaryPeriodId, hrBranchId, pnsId);
				foreach (var item in designations)
				{
					data.Add(new SalaryWithDesignation
					{
						designation = item,
						branchMonthlySalaryReports = monthlySalaryReportViewModels.Where(x => x.designation == item.designationName)
					});
				}

				var model = new PayrollReportViewModel
				{
					companies = await eRPCompanyService.GetAllCompany(),
					designations = await salaryService.GetAllDesignations(),
					salaryWithDesignations = data,
					branchMonthlySalaryReportViewModels = await salaryService.GetMonthlySalaryReportByPeriodIdUnitNew(salaryPeriodId, hrBranchId, pnsId)
				};
				return View(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public async Task<IActionResult> MonthlySalaryReportPdf_ZoneNew(int salaryPeriodId, int? hrBranchId, int? pnsId)
		{
			try
			{
				if (hrBranchId > 0)
				{
					ViewBag.zoneName = await personalInfoService.GetZoneNameById(Convert.ToInt32(hrBranchId));
				}
				else
				{
					ViewBag.zoneName = null;
				}
				var data = new List<SalaryWithDesignation>();
				var designations = await salaryService.GetAllDesignations();
				var monthlySalaryReportViewModels = await salaryService.GetMonthlySalaryReportByPeriodIdZoneNew(salaryPeriodId, hrBranchId, pnsId);
				foreach (var item in designations)
				{
					data.Add(new SalaryWithDesignation
					{
						designation = item,
						branchMonthlySalaryReports = monthlySalaryReportViewModels.Where(x => x.designation == item.designationName)
					});
				}

				var model = new PayrollReportViewModel
				{
					companies = await eRPCompanyService.GetAllCompany(),
					designations = await salaryService.GetAllDesignations(),
					salaryWithDesignations = data,
					branchMonthlySalaryReportViewModels = await salaryService.GetMonthlySalaryReportByPeriodIdZoneNew(salaryPeriodId, hrBranchId, pnsId)
				};
				return View(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		//Without Designation Wise
		//[AllowAnonymous]
		//public async Task<IActionResult> MonthlySalaryReportPdf_BnB_V2(int salaryPeriodId, int? sbuId, int? pnsId)
		//{
		//    try
		//    {
		//        var model = new PayrollReportViewModel
		//        {
		//            monthlySalaryReportViewModels = await salaryService.GetMonthlySalaryReportByPeriodId(salaryPeriodId, sbuId, pnsId),
		//            companies = await eRPCompanyService.GetAllCompany(),
		//        };
		//        return PartialView(model);
		//    }
		//    catch (Exception ex)
		//    {
		//        throw ex;
		//    }
		//}

		[AllowAnonymous]
		public async Task<IActionResult> MonthlySalaryReportPdf_BWC_BnB(int salaryPeriodId, int? sbuId, int? pnsId)
		{
			try
			{
				var model = new PayrollReportViewModel
				{
					monthlySalaryReportViewModels = await salaryService.GetMonthlySalaryReportByPeriodId(salaryPeriodId, sbuId, pnsId),
					companies = await eRPCompanyService.GetAllCompany(),
				};
				return PartialView(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[AllowAnonymous]
		public async Task<IActionResult> MonthlySalaryReportPdf_Brac(int salaryPeriodId, int? sbuId, int? pnsId)
		{
			try
			{
				var model = new PayrollReportViewModel
				{
					monthlySalaryReportViewModels = await salaryService.GetMonthlySalaryReportByPeriodId(salaryPeriodId, sbuId, pnsId),
					companies = await eRPCompanyService.GetAllCompany(),
				};
				return PartialView(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[AllowAnonymous]
		public async Task<IActionResult> MonthlySalaryReportPdf_DbSchenker(int salaryPeriodId, int? sbuId, int? pnsId)
		{
			try
			{
				var model = new PayrollReportViewModel
				{
					monthlySalaryReportViewModels = await salaryService.GetMonthlySalaryReportByPeriodId(salaryPeriodId, sbuId, pnsId),
					companies = await eRPCompanyService.GetAllCompany(),
				};
				return PartialView(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[AllowAnonymous]
		public async Task<IActionResult> BankStatementReportPdf_BnB(int salaryPeriodId, int? sbuId, int? pnsId)
		{
			try
			{
				var model = new PayrollReportViewModel
				{
					bankStatementReportViewModels = await salaryService.GetBankStatementByPeriodId(salaryPeriodId, sbuId, pnsId),
					companies = await eRPCompanyService.GetAllCompany(),
				};
				string Val = model.bankStatementReportViewModels.Where(a => a.bankPayable != 0 && a.bankAccountNo != "").Sum(x => x.bankPayable).ToString();
				ViewBag.InWord = AmountInWord.ConvertToWords(Val);

				return PartialView(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[AllowAnonymous]
		public async Task<IActionResult> BankStatementReportPdf_BnB_WithoutPad(int salaryPeriodId, int? sbuId, int? pnsId)
		{
			try
			{
				var model = new PayrollReportViewModel
				{
					bankStatementReportViewModels = await salaryService.GetBankStatementByPeriodId(salaryPeriodId, sbuId, pnsId),
					companies = await eRPCompanyService.GetAllCompany(),
				};
				string Val = model.bankStatementReportViewModels.Where(a => a.bankPayable != 0 && a.bankAccountNo != "").Sum(x => x.bankPayable).ToString();
				ViewBag.InWord = AmountInWord.ConvertToWords(Val);

				return PartialView(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[AllowAnonymous]
		public async Task<IActionResult> BankApplicationReportPdf_BnB(int salaryPeriodId, int? sbuId, int? pnsId)
		{
			try
			{
				var model = new PayrollReportViewModel
				{
					bankStatementReportViewModels = await salaryService.GetBankStatementByPeriodId(salaryPeriodId, sbuId, pnsId),
					companies = await eRPCompanyService.GetAllCompany(),
				};
				string Val = model.bankStatementReportViewModels.Where(a => a.bankPayable != 0 && a.bankAccountNo != "").Sum(x => x.bankPayable).ToString();
				ViewBag.InWord = AmountInWord.ConvertToWords(Val);

				return PartialView(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[AllowAnonymous]
		public async Task<IActionResult> WalletStatementReportPdf_BnB(int salaryPeriodId, int? sbuId, int? pnsId)
		{
			try
			{
				var model = new PayrollReportViewModel
				{
					bankStatementReportViewModels = await salaryService.GetBankStatementByPeriodId(salaryPeriodId, sbuId, pnsId),
					companies = await eRPCompanyService.GetAllCompany(),
				};
				string Val = model.bankStatementReportViewModels.Sum(x => x.walletPayable).ToString();
				ViewBag.InWord = AmountInWord.ConvertToWords(Val);

				return PartialView(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[AllowAnonymous]
		public async Task<IActionResult> CashStatementReportPdf_BnB(int salaryPeriodId, int? sbuId, int? pnsId)
		{
			try
			{
				var model = new PayrollReportViewModel
				{
					bankStatementReportViewModels = await salaryService.GetBankStatementByPeriodId(salaryPeriodId, sbuId, pnsId),
					companies = await eRPCompanyService.GetAllCompany(),
				};
				string Val = model.bankStatementReportViewModels.Sum(x => x.cashPayable).ToString();
				ViewBag.InWord = AmountInWord.ConvertToWords(Val);

				return PartialView(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		[AllowAnonymous]
		public async Task<IActionResult> MonthlySalaryReportPdf_Priyojon(int salaryPeriodId, int? sbuId, int? pnsId)
		{
			try
			{
				var model = new PayrollReportViewModel
				{
					monthlySalaryReportViewModels = await salaryService.GetMonthlySalaryReportByPeriodId(salaryPeriodId, sbuId, pnsId),
					companies = await eRPCompanyService.GetAllCompany(),
				};
				return PartialView(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[AllowAnonymous]
		public async Task<IActionResult> MonthlySalaryReportPdf_Opus(int salaryPeriodId, int? sbuId, int? pnsId)
		{
			try
			{
				var model = new PayrollReportViewModel
				{
					monthlySalaryReportViewModels = await salaryService.GetMonthlySalaryReportByPeriodId(salaryPeriodId, sbuId, pnsId),
					companies = await eRPCompanyService.GetAllCompany(),
				};
				return PartialView(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		[AllowAnonymous]
		public async Task<ActionResult> SalaryVoucher(int salaryPeriodId, int id, string type)
		{
			var salaryperiod = await salaryService.GetSalaryPeriodById(salaryPeriodId);
			ViewBag.salaryPeriod = salaryperiod.periodName;

			PayrollReportViewModel model = new PayrollReportViewModel
			{
				companies = await eRPCompanyService.GetAllCompany(),
				slaryVoucherVm = await salaryService.GetSalaryVoucherByPeriodId(salaryPeriodId, id, type)
			};

			return View(model);
		}


		[AllowAnonymous]
		public IActionResult SalaryVouchertPDF(int salaryPeriodId, int id, string type)
		{
			string schema = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string filename;

			url = schema+ "://" + host + "/Payroll/PayrollReport/SalaryVoucher?salaryPeriodId=" + salaryPeriodId + "&id=" + id + "&type=" + type;

			string status = myPDF.GeneratePDF(out filename, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}
		[AllowAnonymous]
		public async Task<IActionResult> MobileAllowanceReportViewInd(int periodId)
		{
			PayrollReportViewModel model = new PayrollReportViewModel
			{
				companies = await eRPCompanyService.GetAllCompany(),
				mobileOrCarAllowanceVms = await salaryService.GetMobileOrCarAllowanceVms(periodId, "MOBILE")
			};

			return View(model);
		}
		[AllowAnonymous]
		public async Task<IActionResult> MobileAllowanceReportView(int periodId)
		{
			PayrollReportViewModel model = new PayrollReportViewModel
			{
				companies = await eRPCompanyService.GetAllCompany(),
				mobileOrCarAllowanceVms = await salaryService.GetMobileOrCarAllowanceVms(periodId, "MOBILE")
			};

			return View(model);
		}
		[AllowAnonymous]
		public IActionResult MobileAllowanceReportPdf(int periodId, string type)
		{
			string schema = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string filename;
			if (type == "branchWise")
			{
				url = schema+ "://" + host + "/Payroll/PayrollReport/MobileAllowanceReportViewInd?periodId=" + periodId;
			}
			else
			{
				url = schema + "://" + host + "/Payroll/PayrollReport/MobileAllowanceReportView?periodId=" + periodId;
			}

			string status = myPDF.GeneratePDF(out filename, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}

		[AllowAnonymous]
		public async Task<ActionResult> SalaryVoucherAll(int salaryPeriodId, string type)
		{
			var salaryperiod = await salaryService.GetSalaryPeriodById(salaryPeriodId);
			ViewBag.salaryPeriod = salaryperiod.periodName;

			PayrollReportViewModel model = new PayrollReportViewModel
			{
				companies = await eRPCompanyService.GetAllCompany(),
				slaryVoucherAllVm = await salaryService.GetSalaryVoucherAllByPeriodId(salaryPeriodId, type)
			};

			return View(model);
		}


		[AllowAnonymous]
		public IActionResult SalaryVoucherAllPDF(int salaryPeriodId, string type)
		{
			string schema = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string filename;

			url = schema+"://" + host + "/Payroll/PayrollReport/SalaryVoucherAll?salaryPeriodId=" + salaryPeriodId + "&type=" + type;

			string status = myPDF.GeneratePDF(out filename, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}

		[AllowAnonymous]
		public async Task<ActionResult> BonusVoucher(int salaryPeriodId, int hrBranchId, int zoneId)
		{
			var salaryperiod = await salaryService.GetSalaryPeriodById(salaryPeriodId);
			//if (salaryperiod.Id == 56)
			//{
			//	ViewBag.salaryPeriod = "Noboborsho Allowance for the year " + salaryperiod.salaryYear?.yearName;
			//}
			//else if (salaryperiod.Id == 58)
			//{
			//	ViewBag.salaryPeriod = "Chaittra Sangcranti(Biju) Bonus for the year " + salaryperiod.salaryYear?.yearName;
			//}
			//else
			//{
			//	ViewBag.salaryPeriod = "Eid-Ul-Fitr Bonus for the year " + salaryperiod.salaryYear?.yearName;
			//}

			PayrollReportViewModel model = new PayrollReportViewModel
			{
				companies = await eRPCompanyService.GetAllCompany(),
                salaryPeriod = salaryperiod,
                bonusVouchers = await salaryService.GetBonusVoucherByPeriodId(salaryPeriodId, hrBranchId, zoneId)
			};

			return View(model);
		}


		[AllowAnonymous]
		public IActionResult BonusVoucherPDF(int salaryPeriodId, int hrBranchId, int zoneId)
		{
			string schema = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string filename;

			url = schema+"://" + host + "/Payroll/PayrollReport/BonusVoucher?salaryPeriodId=" + salaryPeriodId + "&hrBranchId=" + hrBranchId + "&zoneId=" + zoneId;

			string status = myPDF.GeneratePDF(out filename, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}


		[AllowAnonymous]
		public async Task<ActionResult> BonusVoucherByType(int salaryPeriodId, string type)
		{
			var salaryperiod = await salaryService.GetSalaryPeriodById(salaryPeriodId);
			//if (salaryperiod.Id == 56)
			//{
			//	ViewBag.salaryPeriod = "Noboborsho Allowance for the year " + salaryperiod.salaryYear?.yearName;
			//}
			//else if (salaryperiod.Id == 58)
			//{
			//	ViewBag.salaryPeriod = "Chaittra Sangcranti(Biju) Bonus for the year " + salaryperiod.salaryYear?.yearName;
			//}
			//else
			//{
			//	ViewBag.salaryPeriod = "Eid-Ul-Fitr Bonus for the year " + salaryperiod.salaryYear?.yearName;
			//}

            ViewBag.year = salaryperiod.salaryYear?.yearName;

            PayrollReportViewModel model = new PayrollReportViewModel
			{
                hrBranches = await salaryService.GetAllHrBranchs(),
				companies = await eRPCompanyService.GetAllCompany(),
				bonusVouchers = await salaryService.GetBonusVoucherByPeriodIdAndType(salaryPeriodId, type)
			};

			return View(model);
		}

		[AllowAnonymous]
		public IActionResult BonusVoucherPDFByType(int salaryPeriodId, string type)
		{
			string schema = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string filename;

			url = schema + "://" + host + "/Payroll/PayrollReport/BonusVoucherByType?salaryPeriodId=" + salaryPeriodId + "&type=" + type;

			string status = myPDF.GeneratePDF(out filename, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}

		[AllowAnonymous]
		public async Task<ActionResult> SalaryAdditionDiductionView(int salaryPeriodId, int branchId)
		{
			ViewBag.branchName = await personalInfoService.GetBranchNameById(branchId);
			PayrollReportViewModel model = new PayrollReportViewModel
			{
				companies = await eRPCompanyService.GetAllCompany(),
				salaryAddDiductionViewModels = await salaryService.GetSalaryAddiDidinfoByPeriodNbranchId(salaryPeriodId, branchId)
			};

			return View(model);
		}

		[AllowAnonymous]
		public IActionResult SalaryAdditionDiductionPDF(int salaryPeriodId, int branchId)
		{
			string schema = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string filename;

			url = schema + "://" + host + "/Payroll/PayrollReport/SalaryAdditionDiductionView?salaryPeriodId=" + salaryPeriodId + "&branchId=" + branchId;

			string status = myPDF.GeneratePDF(out filename, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}


		#endregion

		#region Loan
		[AllowAnonymous]
		public async Task<ActionResult> AllEmployeeLoanView(string type, int id)
		{
			ViewBag.type = await salaryService.GetTypeName(type, id);
			PayrollReportViewModel model = new PayrollReportViewModel
			{
				companies = await eRPCompanyService.GetAllCompany(),
				allLoans = await salaryService.GetAllLoanByTypeAndId(type, id)
			};

			return View(model);
		}
		[AllowAnonymous]
		public IActionResult AllEmployeeLoanPDF(string type, int id)
		{
			string schema = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string filename;

			url = schema + "://" + host + "/Payroll/PayrollReport/AllEmployeeLoanView?type=" + type + "&id=" + id;

			string status = myPDF.GeneratePDF(out filename, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}

		[AllowAnonymous]
		public async Task<ActionResult> IndividualLoanView(int empId)
		{
			var loans = await salaryService.GetIndividualLoanByEmpId(empId);
			//foreach (var item in loans)
			//{
			//	var data = new IndividualLoans
			//	{
			//		employee = await personalInfoService.GetEmployeeInfoById(empId),
			//		loans = await item.whe
			//	}
			////}
			var data = new IndividualLoans
			{
				employee = await personalInfoService.GetEmployeeInfoById(empId),
				loans = await salaryService.GetIndividualLoanByEmpId(empId)
			};
			PayrollReportViewModel model = new PayrollReportViewModel
			{
				companies = await eRPCompanyService.GetAllCompany(),
				indLoan = data
			};

			return View(model);
		}
		[AllowAnonymous]
		public IActionResult IndividualLoanPDF(int empId)
		{
			string schema = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string filename;

			url = schema + "://" + host + "/Payroll/PayrollReport/IndividualLoanView?empId=" + empId;

			string status = myPDF.GeneratePDF(out filename, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}

		public async Task<IActionResult> SalaryReports()
		{
			var model = new PayrollReportViewModel
			{
				salaryPeriods = await salaryService.GetAllSalaryPeriod()
			};

			return View(model);
		}

		public IActionResult StaffLoanReport()
		{
			return View();
		}

        public IActionResult StaffLoanReportNew()
        {
            return View();
        }
        //Asad added on 16.06.2023
        public IActionResult StaffLoanInterestCharge()
        {
            return View();
        }
        //Asad added on 15.06.2023
        public IActionResult StaffLoanVoucher()
        {
            return View();
        }

        public async Task<IActionResult> MyStaffLoanReport()
		{
			var username = User.Identity.Name;
			var user = await personalInfoService.GetEmployeeInformationByCode(username);
			ViewBag.empCode = username;
			ViewBag.empName = user.empName;
			return View();
		}

		public async Task<IActionResult> StaffLoanInterestProcess()
		{
			var data = new PayrollReportViewModel
			{
				hrBranches = await salaryService.GetAllHrBranchs(),
				locations = await personalInfoService.GetAllZones()
			};

			return View(data);
		}

		public async Task<IActionResult> ProcessStaffLoanInterest(DateTime? processDate, int hrBranchId, string type)
		{
			var data = await salaryService.ProcessStaffLoanInterest(processDate, hrBranchId, type);
			return Json(data);
		}

        public async Task<IActionResult> StaffLoanInterest(DateTime? processDate, string code, string type)
		{
			var data = await salaryService.StaffLoanInterest(processDate, code, type);
			return Json(data);
		}



		public async Task<IActionResult> ProcessStaffLoanInterestIndividual(DateTime? processDate, string empCode, string type)
		{
			var data = await salaryService.ProcessStaffLoanInterestIndividual(processDate, empCode, type);
			return Json(data);
		}

		public async Task<IActionResult> GetLoansByEmployeeCode(string empCode)
		{
			var data = await salaryService.GetLoansByEmployeeCode(empCode);
			return Json(data);
		}
		[AllowAnonymous]
		public async Task<ActionResult> StaffLoanIndScheduleView(DateTime? startDate, DateTime? endDate, int hrBranchId, int zoneId, string empCode)
		{
			ViewBag.startDate = startDate?.ToString("dd MMM yyyy");
			ViewBag.endDate = endDate?.ToString("dd MMM yyyy");
			PayrollReportViewModel model = new PayrollReportViewModel
			{
				companies = await eRPCompanyService.GetAllCompany(),
				staffLoanEmployees = await salaryService.GetEmpInfoByLoan(),
				staffLoanSchedules = await salaryService.GetStaffLoanScheduleByDateRange(startDate, endDate, hrBranchId, zoneId, empCode)
			};

			return View(model);
		}
		[AllowAnonymous]
		public async Task<ActionResult> StaffLoanIndScheduleViewNew(DateTime? startDate, DateTime? endDate, int hrBranchId, int zoneId, string empCode)
		{
			var branchName = "";
			if (hrBranchId == 0 && zoneId == 0)
			{
				if (empCode != "0")
				{
					branchName = empCode;
				}
				else
				{
					branchName = "All Branch";
				}
			}
			else if (hrBranchId != 0 && zoneId == 0)
			{
				var branch = await salaryService.GetHrBranchById(hrBranchId);
				branchName = branch.branchName;
			}
			else if (hrBranchId == 0 && zoneId != 0)
			{
				var zone = await salaryService.GetZoneById(zoneId);
				branchName = zone.branchUnitName;
			}
			ViewBag.branchName = branchName;
			ViewBag.startDate = startDate?.ToString("dd MMM yyyy");
			ViewBag.endDate = endDate?.ToString("dd MMM yyyy");
			PayrollReportViewModel model = new PayrollReportViewModel
			{
				companies = await eRPCompanyService.GetAllCompany(),
				staffLoanEmployees = await salaryService.GetEmpInfoByLoan(),
				staffLoanSchedules = await salaryService.GetStaffLoanScheduleByDateRange(startDate, endDate, hrBranchId, zoneId, empCode)
			};

			return View(model);
		}
		[AllowAnonymous]
		public IActionResult StaffLoanIndSchedulePdf(DateTime? startDate, DateTime? endDate, int hrBranchId, int zoneId, string empCode)
		{
			string schema = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string filename;

			url = schema + "://" + host + "/Payroll/PayrollReport/StaffLoanIndScheduleViewNew?startDate=" + startDate + "&endDate=" + endDate + "&hrBranchId=" + hrBranchId + "&zoneId=" + zoneId + "&empCode=" + empCode;

			string status = myPDF.GeneratePDF(out filename, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}

		[AllowAnonymous]
		public async Task<ActionResult> StaffLoanIndRecoveryView(DateTime? startDate, DateTime? endDate, int hrBranchId, int zoneId, string empCode, string loanType)
		{
			ViewBag.startDate = startDate?.ToString("dd MMM yyyy");
			ViewBag.endDate = endDate?.ToString("dd MMM yyyy");
			PayrollReportViewModel model = new PayrollReportViewModel
			{
				companies = await eRPCompanyService.GetAllCompany(),
				staffLoanEmployees = await salaryService.GetEmpInfoByLoan(),
				staffLoanSchedules = await salaryService.GetStaffLoanRecoveryScheduleByDateRange(startDate, endDate, hrBranchId, zoneId, empCode, loanType)
			};

			return View(model);
		}

		[AllowAnonymous]
		public IActionResult StaffLoanIndRecoveryPdf(DateTime? startDate, DateTime? endDate, int hrBranchId, int zoneId, string empCode, string loanType)
		{
			string schema = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string filename;

			url = schema + "://" + host + "/Payroll/PayrollReport/StaffLoanIndRecoveryView?startDate=" + startDate + "&endDate=" + endDate + "&hrBranchId=" + hrBranchId + "&zoneId=" + zoneId + "&empCode=" + empCode + "&loanType=" + loanType;

			string status = myPDF.GeneratePDF(out filename, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}

		[AllowAnonymous]
		public async Task<ActionResult> InterestReportView(DateTime? startDate, DateTime? endDate, int hrBranchId, int zoneId, string loanType)
		{
			ViewBag.startDate = startDate?.ToString("dd MMM yyyy");
			ViewBag.endDate = endDate?.ToString("dd MMM yyyy");
			PayrollReportViewModel model = new PayrollReportViewModel
			{
				companies = await eRPCompanyService.GetAllCompany(),
				staffLoanEmployees = await salaryService.GetEmpInfoByLoan(),
				staffLoanInterestVms = await salaryService.GetStaffLoanInterestScheduleByDateRange(startDate, endDate, hrBranchId, zoneId, loanType)
			};

			return View(model);
		}
		[AllowAnonymous]
		public async Task<ActionResult> InterestChargeReportView(DateTime? asOndate, int hrBranchId, string zoneorBranch)
		{
			ViewBag.asonDate = asOndate.ToString();
			var data = await salaryService.GetStaffLoanInterestChargeByDateRange(asOndate.ToString(), hrBranchId, zoneorBranch);
			PayrollReportViewModel model = new PayrollReportViewModel
			{
				companies = await eRPCompanyService.GetAllCompany(),
				designations = await personalInfoService.GetAllDesignation(),
				staffLoanInterestChargeVms = data,
				hrBranchinfo = await personalInfoService.GetHrBranchById(hrBranchId)

			};
			
			return View(model);
		}
		[AllowAnonymous]
        public async Task<ActionResult> InterestReportAllView(DateTime? startDate, DateTime? endDate, int hrBranchId, int zoneId, string loanType)
        {
            ViewBag.startDate = startDate?.ToString("dd MMM yyyy");
            ViewBag.endDate = endDate?.ToString("dd MMM yyyy");
            PayrollReportViewModel model = new PayrollReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                staffLoanEmployees = await salaryService.GetEmpInfoByLoan(),
				employeeInfos = await personalInfoService.GetAllEmployeeInfoWithPosting(),
                allstaffLoanInterestVms = await salaryService.GetAllInterestByDate(startDate, endDate)
            };

            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> AllStaffLoanView(DateTime? startDate, DateTime? endDate)
        {
            ViewBag.startDate = startDate?.ToString("dd MMM yyyy");
            ViewBag.endDate = endDate?.ToString("dd MMM yyyy");
            PayrollReportViewModel model = new PayrollReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                staffLoanEmployees = await salaryService.GetEmpInfoByLoan(),
                allStaffLoanVms = await salaryService.GetaLLStaffLoanByDateRange(startDate, endDate)
            };

            return View(model);
        }
        [AllowAnonymous]
        public IActionResult AllStaffLoanPdf(DateTime? startDate, DateTime? endDate)
        {
            string schema = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string filename;

            url = schema + "://" + host + "/Payroll/PayrollReport/AllStaffLoanView?startDate=" + startDate + "&endDate=" + endDate;

            string status = myPDF.GeneratePDF(out filename, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }
        [AllowAnonymous]
		public IActionResult InterestReportPdf(DateTime? startDate, DateTime? endDate, int hrBranchId, int zoneId, string loanType)
		{
			string schema = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string filename;

			url = schema + "://" + host + "/Payroll/PayrollReport/InterestReportView?startDate=" + startDate + "&endDate=" + endDate + "&hrBranchId=" + hrBranchId + "&zoneId=" + zoneId + "&loanType=" + loanType;

			string status = myPDF.GeneratePDF(out filename, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}
		[AllowAnonymous]
		public IActionResult InterestChargeReportPdf(DateTime? asOndate, int hrBranchId, string zoneorBranch)
		{
			string schema = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string filename;

			url = schema + "://" + host + "/Payroll/PayrollReport/InterestChargeReportView?asOndate=" + asOndate + "&hrBranchId=" + hrBranchId + "&zoneorBranch=" + zoneorBranch;

			string status = myPDF.GeneratePDF(out filename, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}
		[AllowAnonymous]
		public IActionResult InterestReportAllPdf(DateTime? startDate, DateTime? endDate, int hrBranchId, int zoneId, string loanType)
		{
			string schema = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string filename;

			url = schema + "://" + host + "/Payroll/PayrollReport/InterestReportAllView?startDate=" + startDate + "&endDate=" + endDate + "&hrBranchId=" + hrBranchId + "&zoneId=" + zoneId + "&loanType=" + loanType;

			string status = myPDF.GeneratePDF(out filename, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}
        [AllowAnonymous]
        public async Task<ActionResult> InterestReportAllByTypeExcel(DateTime? startDate, DateTime? endDate, int hrBranchId, int zoneId, string loanType)
        {
            ViewBag.startDate = startDate?.ToString("dd MMM yyyy");
            ViewBag.endDate = endDate?.ToString("dd MMM yyyy");
            PayrollReportViewModel model = new PayrollReportViewModel
            {
                //companies = await eRPCompanyService.GetAllCompany(),
                //staffLoanEmployees = await salaryService.GetEmpInfoByLoan(),
                //employeeInfos = await personalInfoService.GetAllEmployeeInfoWithPosting(),
                
            };

            var allstaffLoanInterestVms = await salaryService.GetAllInterestByDate(startDate, endDate);
            
            #region MCA
            DataTable dtMCA = new DataTable("MCA");
            dtMCA.Columns.AddRange(new DataColumn[10] {
                new DataColumn("SL"),
                new DataColumn("EmpId"),
                new DataColumn("Emp Name"),
                new DataColumn("Designation"),
                new DataColumn("Loan Type"),
                new DataColumn("Date"),
                new DataColumn("Interest Charged"),
                new DataColumn("Principal"),
                new DataColumn("Interest"),
                new DataColumn("Total")
            });

            int slmca = 0;
            foreach (var item in allstaffLoanInterestVms.Where(x => x.loanType == "MCA").OrderBy(x => x.loanType))
            {
                slmca++;
                dtMCA.Rows.Add(
                    slmca.ToString(),
                    item.employeeCode,
                    item.nameEnglish,
                    item.desig,
                    item.loanType,
                    item.effectiveDate,
                    item.interestCharged,
                    item.principalTotal,
                    item.interestTotal,
                    item.total
                    );
            }
            #endregion

            #region MCAR
            DataTable dtMCAR = new DataTable("MCAR");
            dtMCAR.Columns.AddRange(new DataColumn[10] {
                new DataColumn("SL"),
                new DataColumn("EmpId"),
                new DataColumn("Emp Name"),
                new DataColumn("Designation"),
                new DataColumn("Loan Type"),
                new DataColumn("Date"),
                new DataColumn("Interest Charged"),
                new DataColumn("Principal"),
                new DataColumn("Interest"),
                new DataColumn("Total")
            });

            int slmcar = 0;
            foreach (var item in allstaffLoanInterestVms.Where(x => x.loanType == "MCAR").OrderBy(x => x.loanType))
            {
                slmcar++;
                dtMCAR.Rows.Add(
                    slmcar.ToString(),
                    item.employeeCode,
                    item.nameEnglish,
                    item.desig,
                    item.loanType,
                    item.effectiveDate,
                    item.interestCharged,
                    item.principalTotal,
                    item.interestTotal,
                    item.total
                    );
            }
            #endregion

            #region CL
            DataTable dtCL = new DataTable("CL");
            dtCL.Columns.AddRange(new DataColumn[10] {
                new DataColumn("SL"),
                new DataColumn("EmpId"),
                new DataColumn("Emp Name"),
                new DataColumn("Designation"),
                new DataColumn("Loan Type"),
                new DataColumn("Date"),
                new DataColumn("Interest Charged"),
                new DataColumn("Principal"),
                new DataColumn("Interest"),
                new DataColumn("Total")
            });

            int slcl = 0;
            foreach (var item in allstaffLoanInterestVms.Where(x => x.loanType == "CL").OrderBy(x => x.loanType))
            {
                slcl++;
                dtCL.Rows.Add(
                    slcl.ToString(),
                    item.employeeCode,
                    item.nameEnglish,
                    item.desig,
                    item.loanType,
                    item.effectiveDate,
                    item.interestCharged,
                    item.principalTotal,
                    item.interestTotal,
                    item.total
                    );
            }
            #endregion

            #region HBA-B13
            DataTable dtHBAB13 = new DataTable("HBA-B13");
            dtHBAB13.Columns.AddRange(new DataColumn[10] {
                new DataColumn("SL"),
                new DataColumn("EmpId"),
                new DataColumn("Emp Name"),
                new DataColumn("Designation"),
                new DataColumn("Loan Type"),
                new DataColumn("Date"),
                new DataColumn("Interest Charged"),
                new DataColumn("Principal"),
                new DataColumn("Interest"),
                new DataColumn("Total")
            });

            int slHBAB13 = 0;
            foreach (var item in allstaffLoanInterestVms.Where(x => x.loanType == "HBA-B13").OrderBy(x => x.loanType))
            {
                slHBAB13++;
                dtHBAB13.Rows.Add(
                    slHBAB13.ToString(),
                    item.employeeCode,
                    item.nameEnglish,
                    item.desig,
                    item.loanType,
                    item.effectiveDate,
                    item.interestCharged,
                    item.principalTotal,
                    item.interestTotal,
                    item.total
                    );
            }
            #endregion

            #region HBA-A13
            DataTable dtHBAA13 = new DataTable("HBA-A13");
            dtHBAA13.Columns.AddRange(new DataColumn[10] {
                new DataColumn("SL"),
                new DataColumn("EmpId"),
                new DataColumn("Emp Name"),
                new DataColumn("Designation"),
                new DataColumn("Loan Type"),
                new DataColumn("Date"),
                new DataColumn("Interest Charged"),
                new DataColumn("Principal"),
                new DataColumn("Interest"),
                new DataColumn("Total")
            });

            int slHBAA13 = 0;
            foreach (var item in allstaffLoanInterestVms.Where(x => x.loanType == "HBA-A13").OrderBy(x => x.loanType))
            {
                slHBAA13++;
                dtHBAA13.Rows.Add(
                    slHBAA13.ToString(),
                    item.employeeCode,
                    item.nameEnglish,
                    item.desig,
                    item.loanType,
                    item.effectiveDate,
                    item.interestCharged,
                    item.principalTotal,
                    item.interestTotal,
                    item.total
                    );
            }
			#endregion

			#region BCA
			DataTable dtBCA = new DataTable("BCA");
			dtBCA.Columns.AddRange(new DataColumn[10] {
				new DataColumn("SL"),
				new DataColumn("EmpId"),
				new DataColumn("Emp Name"),
				new DataColumn("Designation"),
				new DataColumn("Loan Type"),
				new DataColumn("Date"),
				new DataColumn("Interest Charged"),
				new DataColumn("Principal"),
				new DataColumn("Interest"),
				new DataColumn("Total")
			});

			int slBCA = 0;
			foreach (var item in allstaffLoanInterestVms.Where(x => x.loanType == "BCA").OrderBy(x => x.loanType))
			{
				slBCA++;
				dtBCA.Rows.Add(
					slBCA.ToString(),
					item.employeeCode,
					item.nameEnglish,
					item.desig,
					item.loanType,
					item.effectiveDate,
					item.interestCharged,
					item.principalTotal,
					item.interestTotal,
					item.total
					);
			}
			#endregion


			using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dtMCA);
                wb.Worksheets.Add(dtMCAR);
                wb.Worksheets.Add(dtCL);
                wb.Worksheets.Add(dtHBAB13);
                wb.Worksheets.Add(dtHBAA13);
                wb.Worksheets.Add(dtBCA);

                IXLRange range = wb.Worksheet("MCA").Range(wb.Worksheet("MCA").Cell(1, 1).Address, wb.Worksheet("MCA").Cell(allstaffLoanInterestVms.Where(x => x.loanType == "MCA").Count() + 1, 33).Address);
                IXLRange rangeMCAR = wb.Worksheet("MCAR").Range(wb.Worksheet("MCAR").Cell(1, 1).Address, wb.Worksheet("MCAR").Cell(allstaffLoanInterestVms.Where(x => x.loanType == "MCAR").Count() + 1, 33).Address);
                IXLRange rangeCL = wb.Worksheet("CL").Range(wb.Worksheet("CL").Cell(1, 1).Address, wb.Worksheet("CL").Cell(allstaffLoanInterestVms.Where(x => x.loanType == "CL").Count() + 1, 33).Address);
                IXLRange rangeHBAB13 = wb.Worksheet("HBA-B13").Range(wb.Worksheet("HBA-B13").Cell(1, 1).Address, wb.Worksheet("HBA-B13").Cell(allstaffLoanInterestVms.Where(x => x.loanType == "HBA-B13").Count() + 1, 33).Address);
                IXLRange rangeHBAA13 = wb.Worksheet("HBA-A13").Range(wb.Worksheet("HBA-A13").Cell(1, 1).Address, wb.Worksheet("HBA-A13").Cell(allstaffLoanInterestVms.Where(x => x.loanType == "HBA-A13").Count() + 1, 33).Address);
                IXLRange rangeBCA = wb.Worksheet("BCA").Range(wb.Worksheet("BCA").Cell(1, 1).Address, wb.Worksheet("BCA").Cell(allstaffLoanInterestVms.Where(x => x.loanType == "BCA").Count() + 1, 33).Address);
                range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                rangeMCAR.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                rangeCL.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                rangeHBAB13.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                rangeHBAA13.Style.Border.InsideBorder = XLBorderStyleValues.Thin;


                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "InterestReport_" + startDate + "__" + DateTime.Now + ".xlsx");
                }
            }

        }

        [AllowAnonymous]
        public async Task<ActionResult> InterestReportAllByTypeView(DateTime? startDate, DateTime? endDate, int hrBranchId, int zoneId, string loanType)
        {
            ViewBag.startDate = startDate?.ToString("dd MMM yyyy");
            ViewBag.endDate = endDate?.ToString("dd MMM yyyy");
            PayrollReportViewModel model = new PayrollReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                staffLoanEmployees = await salaryService.GetEmpInfoByLoan(),
                employeeInfos = await personalInfoService.GetAllEmployeeInfoWithPosting(),
                allstaffLoanInterestVms = await salaryService.GetAllInterestByDate(startDate, endDate)
            };

            return View(model);
        }
        [AllowAnonymous]
        public IActionResult InterestReportAllByTypePdf(DateTime? startDate, DateTime? endDate, int hrBranchId, int zoneId, string loanType)
        {
            string schema = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string filename;

            url = schema + "://" + host + "/Payroll/PayrollReport/InterestReportAllByTypeView?startDate=" + startDate + "&endDate=" + endDate + "&hrBranchId=" + hrBranchId + "&zoneId=" + zoneId + "&loanType=" + loanType;

            string status = myPDF.GeneratePDF(out filename, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }

        [AllowAnonymous]
		public async Task<ActionResult> StaffLoanReportViewNew(string empCode, int loanId, DateTime? startDate)
		{
			ViewBag.startDate = startDate?.ToString("dd MMM yyyy");

			PayrollReportViewModel model = new PayrollReportViewModel
			{
				employeeInfoForLoan = await personalInfoService.GetEmployeeInfoByCodeForLoan(empCode),
				companies = await eRPCompanyService.GetAllCompany(),
				staffLoanTrans = await salaryService.GetStaffLoanTransactionsByDateRange(empCode, loanId, startDate)
			};

			return View(model);
		}


		[AllowAnonymous]
		public async Task<ActionResult> StaffLoanReportViewNew2(string empCode, int loanId, DateTime? startDate, DateTime? endDate)
		{
			ViewBag.startDate = startDate?.ToString("dd MMM yyyy");

			PayrollReportViewModel model = new PayrollReportViewModel
			{
				employeeInfoForLoanNew = await personalInfoService.GetEmployeeInfoByCodeForLoanNew(empCode),
				//employeeInfoForLoan = await personalInfoService.GetEmployeeInfoByCodeForLoan(empCode),
				companies = await eRPCompanyService.GetAllCompany(),
				staffLoanTrans = await salaryService.GetStaffLoanTransactionsByFormDateToDate(empCode, loanId, startDate, endDate)
			};

			return View(model);
		}

		[AllowAnonymous]
		public async Task<ActionResult> StaffLoanReportView(string empCode, int loanId, DateTime? startDate, DateTime? endDate)
		{
			ViewBag.startDate = startDate?.ToString("dd MMM yyyy");
			ViewBag.endDate = endDate?.ToString("dd MMM yyyy");
			PayrollReportViewModel model = new PayrollReportViewModel
			{
				employeeInfoForLoan = await personalInfoService.GetEmployeeInfoByCodeForLoan(empCode),
				companies = await eRPCompanyService.GetAllCompany(),
				staffLoans = await salaryService.GetStaffLoanLogByDateRange(empCode, loanId, startDate, endDate)
			};

			return View(model);
		}
		[AllowAnonymous]
		public IActionResult StaffLoanReportPdf(string empCode, int loanId, DateTime? startDate, DateTime? endDate)
		{
			string schema = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string filename;

			url = schema + "://" + host + "/Payroll/PayrollReport/StaffLoanReportViewNew2?empCode=" + empCode + "&loanId=" + loanId + "&startDate=" + startDate + "&endDate=" + endDate;

			string status = myPDF.GeneratePDF(out filename, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}

        [AllowAnonymous]
        public async Task<ActionResult> StaffLoanStatementReportView(string empCode, int loanId, DateTime? startDate, DateTime? endDate)
        {
            ViewBag.startDate = startDate?.ToString("dd MMM yyyy");
            ViewBag.endDate = endDate?.ToString("dd MMM yyyy");
			ViewBag.loanId = loanId;

			//Addedby Asad On 06.08.2023
			PayrollReportViewModel model = new PayrollReportViewModel();

			model.employeeInfoForLoan = await personalInfoService.GetEmployeeInfoByCodeForLoan(empCode);
			model.companies = await eRPCompanyService.GetAllCompany();
			model.staffLoanTrans = await salaryService.GetStaffLoanTransactionsByFormDateToDate(empCode, loanId, startDate, endDate);
						
            var clossingBalance = model.staffLoanTrans.OrderByDescending(cv => cv.effectiveDate).FirstOrDefault()?.total;

			foreach (var item in model.staffLoanTrans)
			{
				item.closingbalance = clossingBalance;
            }



			//Commented By Asad On 06.08.2023
			//PayrollReportViewModel model = new PayrollReportViewModel
			//{
			//employeeInfoForLoan = await personalInfoService.GetEmployeeInfoByCodeForLoan(empCode),
			//companies = await eRPCompanyService.GetAllCompany(),
			//staffLoanTrans = await salaryService.GetStaffLoanTransactionsByFormDateToDate(empCode, loanId, startDate, endDate)
			//};

			return View(model);
        }

        [AllowAnonymous]
        public IActionResult StaffLoanStatementReportPdf(string empCode, int loanId, DateTime? startDate, DateTime? endDate)
        {
            string schema = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string filename;

            url = schema + "://" + host + "/Payroll/PayrollReport/StaffLoanStatementReportView?empCode=" + empCode + "&loanId=" + loanId + "&startDate=" + startDate + "&endDate=" + endDate;

			var footerUrl = Path.Combine(_hostingEnvironment.WebRootPath, "Footer\\ReportFooter\\_LoanStatementFooter.html");

			//var footerUrl = Path.Combine(_hostingEnvironment.ContentRootPath, "Areas\\Payroll\\Views\\PayrollReport\\_LoanStatementFooter.html");

			//Asad added on 07.08.2023
			string status = myPDF.GeneratePDF(out filename, url, "", footerUrl);
			//Asad Commented on 07.08.2023
			//string status = myPDF.GeneratePDF(out filename, url);

			if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }
        //Asad added on 13.06.2023
        [AllowAnonymous]
        public async Task<IActionResult> GetIndividualLoanSummaryByChargeDateExcel(string chargeDate)
        {

            const string sheetName = "Loan Summary";
            var numberColumns = new List<int> { 0, 4, 5, 6 };
            var amountColumn = new List<int> { 20, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 };
            //var dateColumn = new List<int> { 8, 17 };

            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[7] {
                new DataColumn("Loan Id"),
                new DataColumn("Employee Code"),
                new DataColumn("Employee Name"),
                new DataColumn("Loan Type"),
                new DataColumn("Principal Amount"),
                new DataColumn("Interest Amount"),
                new DataColumn("Charge Amount")
            });

            dt.Rows.Add(
                    "1",
                    "2",
                    "3",
                    "4",
                    "5",
                    "6",
                    "7"
                    );

            var data = await salaryService.GetIndividualLoanSummaryByChargeDate(chargeDate);

            foreach (var item in data)
            {
                dt.Rows.Add(
                    item.LoanId,
                    item.EmployeeCode,
                    item.EmployeeName,
                    item.LoanType,
                    item.PrincipalAmount,
                    item.InterestAmount,
                    item.InterestCharge
                    );
            }
                                    

            var lastRow = data.Count() + 3;

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);

                IXLRange range = wb.Worksheet("Grid").Range(wb.Worksheet("Grid").Cell(1, 1).Address, wb.Worksheet("Grid").Cell(data.Count() + 1, 7).Address);
                range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                //Have to use later
                //foreach (var item in numberColumns)
                //{
                //    wb.Worksheet(sheetName).Range(3, item, lastRow, item).SetDataType(XLDataType.Number);
                //}

                //foreach (var item in amountColumn)
                //{
                //    wb.Worksheet(sheetName).Range(3, item, lastRow, item).SetDataType(XLDataType.Number);
                //    wb.Worksheet(sheetName).Range(3, item, lastRow, item).Style.NumberFormat.Format = "#,##0.00";
                //    wb.Worksheet(sheetName).Range(3, item, lastRow, item).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                //}

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Loan_Summary_" + chargeDate + "__" + DateTime.Now + ".xlsx");
                }
            }

        }

        //Asad added on 15.06.2023
        [AllowAnonymous]
        public async Task<IActionResult> GetVoucherByParticularExcel(string particular, string startDate, string endDate)
        {

            const string sheetName = "Voucher";
            var numberColumns = new List<int> { 0, 4, 5, 6 };
            var amountColumn = new List<int> { 20, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 };
            //var dateColumn = new List<int> { 8, 17 };


            //st.empCode,
            //st.empName,
            //st.loanType,
            //lg.credit,
            //lg.debit,
            //lg.particular,
            //lg.effectiveDate,
            //lg.principal

            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[8] {
                new DataColumn("Employee Code"),
                new DataColumn("Employee Name"),
                new DataColumn("Loan Type"),
                new DataColumn("Credit"),
                new DataColumn("Debit"),
                new DataColumn("Particular"),
                new DataColumn("Effective Date"),
                new DataColumn("Principal Amount")
            });

            dt.Rows.Add(
                    "1",
                    "2",
                    "3",
                    "4",
                    "5",
                    "6",
                    "7",
                    "8"
                    );

            var data = await salaryService.GetVoucherByParticular(particular, startDate, endDate);

            foreach (var item in data)
            {
                dt.Rows.Add(
                    item.EmployeeCode,
                    item.EmployeeName,
                    item.LoanType,
                    item.Credit,
                    item.Debit,
                    item.Particular,
                    item.EffectiveDate,
                    item.PrincipalAmount
                    );
            }


            var lastRow = data.Count() + 3;

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);

                IXLRange range = wb.Worksheet("Grid").Range(wb.Worksheet("Grid").Cell(1, 1).Address, wb.Worksheet("Grid").Cell(data.Count() + 1, 8).Address);
                range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                //Have to use later
                //foreach (var item in numberColumns)
                //{
                //    wb.Worksheet(sheetName).Range(3, item, lastRow, item).SetDataType(XLDataType.Number);
                //}

                //foreach (var item in amountColumn)
                //{
                //    wb.Worksheet(sheetName).Range(3, item, lastRow, item).SetDataType(XLDataType.Number);
                //    wb.Worksheet(sheetName).Range(3, item, lastRow, item).Style.NumberFormat.Format = "#,##0.00";
                //    wb.Worksheet(sheetName).Range(3, item, lastRow, item).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                //}

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Voucher" + "__" + DateTime.Now + ".xlsx");
                }
            }

        }


        [AllowAnonymous]
		public async Task<ActionResult> StaffLoanReportApi(string empCode, int loanId, DateTime? startDate, DateTime? endDate)
		{
			PayrollReportViewModel model = new PayrollReportViewModel
			{
				employeeInfoForLoan = await personalInfoService.GetEmployeeInfoByCodeForLoan(empCode),
				staffLoanTrans = await salaryService.GetStaffLoanTransactionsByFormDateToDate(empCode, loanId, startDate, endDate)
            };

			return Json(model);
		}
		public async Task<IActionResult> AllStaffLoans()
		{
			var model = new PayrollReportViewModel
			{
                branchandZoneVms = await salaryService.GetBranchandZone(),
				hrBranches = await salaryService.GetAllHrBranchs(),
				locations = await personalInfoService.GetAllZones(),
                salaryPeriods =  await salaryService.GetAllSalaryPeriod(),
                particulars = await salaryService.GetAllParticulars()
			};

			return View(model);
		}


		public async Task<IActionResult> SBSReportView(string startDate)
		{
			var model = new PayrollReportViewModel
			{
				sbsReport = await salaryService.GetSBSReportAsOn(startDate, DateTime.Now.ToString("yyyy-mm-dd"), "0")
			};
			return View(model);
		}

		public async Task<IActionResult> SBSReportExcel(string startDate, string endDate)
		{
			const string sheetName = "Grid";
			var numberColumns = new List<int> { 3, 4, 5, 8, 12, 13, 14, 15, 16, 17 , 18, 19 };
			var amountColumn = new List<int> { 20, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 };
			//var dateColumn = new List<int> { 8, 17 };

			DataTable dt = new DataTable("Grid");
			dt.Columns.AddRange(new DataColumn[33] {
				new DataColumn("Emp_Name"),
				new DataColumn("Dated"),
				new DataColumn("FI ID"),
				new DataColumn("FI Br.ID"),
				new DataColumn("SLNO"),
				new DataColumn("Account ID"),
				new DataColumn("Date of Birth"),
				new DataColumn("Gender Code"),
				new DataColumn("Unique ID"),
				new DataColumn("E-Tin"),
				new DataColumn("E-BIN"),
				new DataColumn("Nature of Loan"),
				new DataColumn("Eco Sec ID"),
				new DataColumn("Eco Pur.ID"),
				new DataColumn("Collateral ID"),
				new DataColumn("Coll.Value"),
				new DataColumn("Loan Class"),
				new DataColumn("Industry Scale ID"),
				new DataColumn("Product type"),
				new DataColumn("Intt.rate"),
				new DataColumn("San.limit"),
				new DataColumn("Disburse date"),
				new DataColumn("Expiry date"),
				new DataColumn("Opening bl."),
				new DataColumn("Disb. amount"),
				new DataColumn("Ch.Intt"),
				new DataColumn("Acc.Intt."),
				new DataColumn("Others"),
				new DataColumn("Rec. amt."),
				new DataColumn("Waiver amt."),
				new DataColumn("write-off amt."),
				new DataColumn("Outs.amt."),
				new DataColumn("Overdue amt.")
			});
			dt.Rows.Add(
					"1",
					"2",
					"3",
					"4",
					"5",
					"6",
					"7",
					"8",
					"9",
					"10",
					"11",
					"12",
					"13",
					"14",
					"15",
					"16",
					"17",
					"18",
					"19",
					"20",
					"21",
					"22",
					"23",
					"24",
					"25",
					"26",
					"27",
					"28",
					"29",
					"30",
					"31",
					"32",
					"33"
					);



			var sbsReport = await salaryService.GetSBSReportAsOn(startDate, endDate, "0");

			foreach (var sbs in sbsReport)
			{
				dt.Rows.Add(
					sbs.nameEnglish,
					sbs.Dated?.ToString("dd/MM/yyyy"),
					sbs.FIID,
					sbs.FIBranchId,
					sbs.SlNo,
					sbs.ACID,
					sbs.dateOfBirth?.ToString("dd/MM/yyyy"),
					sbs.GenderCode,
					sbs.UniqueId,
					sbs.eTin,
					sbs.eBin,
					sbs.NatureOfLoan,
					sbs.EcoSecId,
					sbs.EcoPurId,
					sbs.CollateralId,
					sbs.ColValue,
					sbs.LoanClass,
					sbs.IndustryScaleId,
					sbs.ProductType,
					sbs.InttRate,
					sbs.SancLimit,
					sbs.DisburseDate?.ToString("dd/MM/yyyy"),
					sbs.ExpiryDate?.ToString("dd/MM/yyyy"),
					sbs.OpeningBalance,
					sbs.DisburseAmount,
					sbs.ChIntt,
					sbs.AccIntt,
					sbs.Others,
					sbs.ReceiveAmount,
					sbs.WaiverAmount,
					sbs.WriteOffAmount,
					sbs.OutStanding,
					sbs.OverdueAmount
					);
			}


			dt.Rows.Add(
					"Total",
					"",
					"",
					"",
					"",
					"",
					"",
					"",
					"",
					"",
					"",
					"",
					"",
					"",
					"",
					"",
					"",
					"",
					"",
					"",
					"",
					"",
					"",
					sbsReport.Sum(x => x.OpeningBalance),
					sbsReport.Sum(x => x.DisburseAmount),
					sbsReport.Sum(x => x.ChIntt),
					sbsReport.Sum(x => x.AccIntt),
					sbsReport.Sum(x => x.Others),
					sbsReport.Sum(x => x.ReceiveAmount),
					sbsReport.Sum(x => x.WaiverAmount),
					sbsReport.Sum(x => x.WriteOffAmount),
					sbsReport.Sum(x => x.OutStanding),
					sbsReport.Sum(x => x.OverdueAmount)
					);

			var lastRow = sbsReport.Count() + 3;

			using (XLWorkbook wb = new XLWorkbook())
			{
				wb.Worksheets.Add(dt);

				IXLRange range = wb.Worksheet("Grid").Range(wb.Worksheet("Grid").Cell(1, 1).Address, wb.Worksheet("Grid").Cell(sbsReport.Count() + 1, 33).Address);
				range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;


				foreach (var item in numberColumns)
				{
					wb.Worksheet(sheetName).Range(3, item, lastRow, item).SetDataType(XLDataType.Number);
				}

				foreach (var item in amountColumn)
				{
					wb.Worksheet(sheetName).Range(3, item, lastRow, item).SetDataType(XLDataType.Number);
					wb.Worksheet(sheetName).Range(3, item, lastRow, item).Style.NumberFormat.Format = "#,##0.00";
					wb.Worksheet(sheetName).Range(3, item, lastRow, item).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
				}

				//foreach (var item in dateColumn)
				//{
				//	wb.Worksheet(sheetName).Range(2, item, lastRow + 1, item).SetDataType(XLDataType.DateTime);
				//}



				using (MemoryStream stream = new MemoryStream())
				{
					wb.SaveAs(stream);
					return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SBSReport_" + startDate + "__" + DateTime.Now + ".xlsx");
				}
			}
		}




		public async Task<IActionResult> ReconcilliationReportExcel(string particular, string fDate, string tDate)
		{
			const string sheetName = "Grid";
			var amountColumn = new List<int> { 4, 5, 8 };

			DataTable dt = new DataTable("Grid");
			dt.Columns.AddRange(new DataColumn[8] {
				new DataColumn("Emp Id"),
				new DataColumn("Emp Name"),
				new DataColumn("Loan Type"),
				new DataColumn("Credit"),
				new DataColumn("Debit"),
				new DataColumn("Particular"),
				new DataColumn("Effective Date"),
				new DataColumn("Principal")
			});


			var reconcialliations = await salaryService.GetReconcillationData(particular, fDate, tDate);

			foreach (var sbs in reconcialliations)
			{
				dt.Rows.Add(
					sbs.empCode,
					sbs.empName,
					sbs.loanType,
					sbs.credit,
					sbs.debit,
					sbs.particular,
					sbs.effectiveDate?.ToString("dd/MM/yyyy"),
					sbs.principal
					);
			}


			dt.Rows.Add(
					"Total",
					"",
					"",
					reconcialliations.Sum(x => x.credit),
					reconcialliations.Sum(x => x.debit),
					"",
					"",
					reconcialliations.Sum(x => x.principal)
					);

			var lastRow = reconcialliations.Count() + 1;

			using (XLWorkbook wb = new XLWorkbook())
			{
				wb.Worksheets.Add(dt);

				IXLRange range = wb.Worksheet("Grid").Range(wb.Worksheet("Grid").Cell(1, 1).Address, wb.Worksheet("Grid").Cell(reconcialliations.Count() + 1, 8).Address);
				range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;



				foreach (var item in amountColumn)
				{
					wb.Worksheet(sheetName).Range(2, item, lastRow, item).SetDataType(XLDataType.Number);
					wb.Worksheet(sheetName).Range(2, item, lastRow, item).Style.NumberFormat.Format = "#,##0.00";
					wb.Worksheet(sheetName).Range(2, item, lastRow, item).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
				}

				//foreach (var item in dateColumn)
				//{
				//	wb.Worksheet(sheetName).Range(2, item, lastRow + 1, item).SetDataType(XLDataType.DateTime);
				//}



				using (MemoryStream stream = new MemoryStream())
				{
					wb.SaveAs(stream);
					return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReconcilliationReport_" + "__" + DateTime.Now + ".xlsx");
				}
			}
		}


        public async Task<IActionResult> QuarterlyReportExcel(string type, string fDate, string tDate)
        {
            const string sheetName = "Grid";
            var amountColumn = new List<int> { 3, 4 };

            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[4] {
                new DataColumn("SL"),
                new DataColumn("Branch Name"),
                new DataColumn("Outstanding"),
                new DataColumn("Disbursement")
            });


            var quarterly = await salaryService.GetQuarterlyReport(type, fDate, tDate);

            foreach (var sbs in quarterly)
            {
                dt.Rows.Add(
                    sbs.Sl,
                    sbs.branchName,
                    sbs.OutStanding,
                    sbs.DisburseAmount
                    );
            }


            dt.Rows.Add(
                    "Total",
                    "",
                    quarterly.Sum(x => x.OutStanding),
                    quarterly.Sum(x => x.DisburseAmount)
                    );

            var lastRow = quarterly.Count() + 1;

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);

                IXLRange range = wb.Worksheet("Grid").Range(wb.Worksheet("Grid").Cell(1, 1).Address, wb.Worksheet("Grid").Cell(quarterly.Count() + 1, 4).Address);
                range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;



                foreach (var item in amountColumn)
                {
                    wb.Worksheet(sheetName).Range(2, item, lastRow, item).SetDataType(XLDataType.Number);
                    wb.Worksheet(sheetName).Range(2, item, lastRow, item).Style.NumberFormat.Format = "#,##0.00";
                    wb.Worksheet(sheetName).Range(2, item, lastRow, item).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                }

                //foreach (var item in dateColumn)
                //{
                //	wb.Worksheet(sheetName).Range(2, item, lastRow + 1, item).SetDataType(XLDataType.DateTime);
                //}



                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Quarterly_Report" + "__" + DateTime.Now + ".xlsx");
                }
            }
        }


        [AllowAnonymous]
		public async Task<ActionResult> StaffLoanBalanceAsOnDateView(DateTime? asOndate, int hrBranchId,string zoneorBranch)
		{
			ViewBag.asonDate = asOndate.ToString();
			var data = await salaryService.GetStaffLoanBalanceAsOn2(asOndate.ToString(), hrBranchId, zoneorBranch);
			PayrollReportViewModel model = new PayrollReportViewModel
			{
				companies = await eRPCompanyService.GetAllCompany(),
				designations = await personalInfoService.GetAllDesignation(),
				staffLoanAsOn = data,
				hrBranchinfo = await personalInfoService.GetHrBranchById(hrBranchId)

			};

			return View(model);
		}
		[AllowAnonymous]
		public IActionResult StaffLoanBalanceAsOnDatePdf(DateTime? asOndate, int hrBranchId,string zoneorBranch)
		{
			string schema = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string filename;

			url = schema + "://" + host + "/Payroll/PayrollReport/StaffLoanBalanceAsOnDateView?asOndate=" + asOndate + "&hrBranchId=" + hrBranchId + "&zoneorBranch=" + zoneorBranch;

			string status = myPDF.GeneratePDF(out filename, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}
		[AllowAnonymous]
		public async Task<ActionResult> StaffLoanBalanceAsOnDateByTypeView(DateTime? asOndate, int hrBranchId, string type)
		{
			ViewBag.asonDate = asOndate.ToString();
			ViewBag.type = type;
			var data = await salaryService.GetStaffLoanBalanceAsOnByType(asOndate.ToString(), hrBranchId, type);
			PayrollReportViewModel model = new PayrollReportViewModel
			{
				companies = await eRPCompanyService.GetAllCompany(),
				designations = await personalInfoService.GetAllDesignation(),
				staffLoanAsOn = data,
				hrBranchinfo = await personalInfoService.GetHrBranchById(hrBranchId)

			};

			return View(model);
		}
		[AllowAnonymous]
		public IActionResult StaffLoanBalanceAsOnDateByTypePdf(DateTime? asOndate, int hrBranchId,string type)
		{
			string schema = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string filename;

			url = schema + "://" + host + "/Payroll/PayrollReport/StaffLoanBalanceAsOnDateByTypeView?asOndate=" + asOndate + "&hrBranchId=" + hrBranchId+ "&type="+ type;

			string status = myPDF.GeneratePDF(out filename, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}
		[AllowAnonymous]
		public async Task<ActionResult> StaffLoanBalanceAsOnDateTypeView(string asOndate, int hrBranchId,string zoneorBranch)
		{
			ViewBag.asonDate = asOndate;
            var data = await salaryService.GetStaffLoanBalanceAsOn2(asOndate, hrBranchId, zoneorBranch);
            PayrollReportViewModel model = new PayrollReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                designations = await personalInfoService.GetAllDesignation(),
                staffLoanAsOn = data,
				hrBranchinfo = await personalInfoService.GetHrBranchById(hrBranchId)

			};

            return View(model);
        }
        [AllowAnonymous]
		public IActionResult StaffLoanBalanceAsOnDateTypePdf(DateTime? asOndate, int hrBranchId,string zoneorBranch)
		{
			string schema = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string filename;

            url = schema + "://" + host + "/Payroll/PayrollReport/StaffLoanBalanceAsOnDateTypeView?asOndate=" + asOndate?.ToString("yyyy-MM-dd") + "&hrBranchId=" + hrBranchId + "&zoneorBranch=" + zoneorBranch;

            string status = myPDF.GeneratePDF(out filename, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }

        [AllowAnonymous]
		public async Task<ActionResult> StaffLoanTypeWiseReportView(DateTime? asOndate, int hrBranchId)
		{
			ViewBag.asonDate = asOndate.ToString();
			var data = await salaryService.GetStaffLoanBalanceAsOn(asOndate.ToString(), hrBranchId);
			PayrollReportViewModel model = new PayrollReportViewModel
			{
				companies = await eRPCompanyService.GetAllCompany(),
				designations = await personalInfoService.GetAllDesignation(),
				staffLoanAsOn = data,
				hrBranchinfo = await personalInfoService.GetHrBranchById(hrBranchId)

			};

			return View(model);
		}
		[AllowAnonymous]
		public IActionResult StaffLoanTypeWiseReportPdf(DateTime? asOndate, int hrBranchId)
		{
			string schema = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string filename;

			url = schema + "://" + host + "/Payroll/PayrollReport/StaffLoanTypeWiseReportView?asOndate=" + asOndate + "&hrBranchId=" + hrBranchId;

			string status = myPDF.GeneratePDF(out filename, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}

        [AllowAnonymous]
        public async Task<ActionResult> BonusReportSundryView(int periodId, int hrBranchId, int zoneId)
        {
            var data = await salaryService.GetBonusSundryData(periodId, hrBranchId, zoneId);
            ViewBag.branchName = "";
            if (hrBranchId != 0)
            {
                var hrBranch = await salaryService.GetHrBranchById(hrBranchId);
                ViewBag.branchName = hrBranch.branchName + "(" + hrBranch.branchCode + ")";
            }
            if (zoneId != 0)
            {
                var zone = await salaryService.GetZoneById(zoneId);
                ViewBag.branchName = zone.branchUnitName + "(" + zone.branchCode + ")";
            }

            PayrollReportViewModel model = new PayrollReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                designations = await personalInfoService.GetAllDesignation(),
                bonuses = data
            };

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult BonusReportSundryPdf(int periodId, int hrBranchId, int zoneId)
        {
            string schema = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string filename;

            url = schema + "://" + host + "/Payroll/PayrollReport/BonusReportSundryView?periodId=" + periodId + "&hrBranchId=" + hrBranchId + "&zoneId=" + zoneId;

            string status = myPDF.GeneratePDF(out filename, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }

        [AllowAnonymous]
		public async Task<ActionResult> BonusReportView(int periodId, int hrBranchId, int zoneId)
		{
			var data = await salaryService.GetBonusData(periodId, hrBranchId, zoneId);
			ViewBag.branchName = "";
			if (hrBranchId != 0)
			{
				var hrBranch = await salaryService.GetHrBranchById(hrBranchId);
				ViewBag.branchName = hrBranch.branchName + "(" + hrBranch.branchCode + ")";
			}
			if (zoneId != 0)
			{
				var zone = await salaryService.GetZoneById(zoneId);
				ViewBag.branchName = zone.branchUnitName + "(" + zone.branchCode + ")";
            }

			PayrollReportViewModel model = new PayrollReportViewModel
			{
				companies = await eRPCompanyService.GetAllCompany(),
				designations = await personalInfoService.GetAllDesignation(),
				bonuses = data
			};

			return View(model);
		}
		[AllowAnonymous]
		public IActionResult BonusReportPdf(int periodId, int hrBranchId, int zoneId)
		{
			string schema = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string filename;

			url = schema + "://" + host + "/Payroll/PayrollReport/BonusReportView?periodId=" + periodId + "&hrBranchId=" + hrBranchId + "&zoneId=" + zoneId;

			string status = myPDF.GeneratePDF(out filename, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}

		[AllowAnonymous]
		public async Task<ActionResult> BonusReportAllView(int periodId, string type)
		{
			var data = await salaryService.GetAllBonusData(periodId, type);

			PayrollReportViewModel model = new PayrollReportViewModel
			{
				companies = await eRPCompanyService.GetAllCompany(),
				designations = await personalInfoService.GetAllDesignation(),
				allBonuses = data
			};

			return View(model);
		}
		[AllowAnonymous]
		public IActionResult BonusReportAllPdf(int periodId, string type)
		{
			string schema = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string filename;

			url = schema + "://" + host + "/Payroll/PayrollReport/BonusReportAllView?periodId=" + periodId + "&type=" + type;

			string status = myPDF.GeneratePDF(out filename, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}

		[AllowAnonymous]
		public async Task<ActionResult> BonusSummaryReportView(int periodId, int hrBranchId, int zoneId)
		{
            var period = await salaryService.GetSalaryPeriodById(periodId);
            var data = await salaryService.GetBonusSummaryData(periodId, hrBranchId, zoneId);
			PayrollReportViewModel model = new PayrollReportViewModel
			{
				companies = await eRPCompanyService.GetAllCompany(),
                salaryPeriod = period,
                //designations = await personalInfoService.GetAllDesignation(),
                bonusSummary = data
			};

			return View(model);
		}
		[AllowAnonymous]
		public IActionResult BonusSummaryReportPdf(int periodId, int hrBranchId, int zoneId)
		{
			string schema = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string filename;

			url = schema + "://" + host + "/Payroll/PayrollReport/BonusSummaryReportView?periodId=" + periodId + "&hrBranchId=" + hrBranchId + "&zoneId=" + zoneId;

			string status = myPDF.GeneratePDF(out filename, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}

		[AllowAnonymous]
		public async Task<ActionResult> BonusSummarySheetBranchOrZoneWise(int periodId, string type)
		{
            var period = await salaryService.GetSalaryPeriodById(periodId);
			var data = await salaryService.GetBonusSummaryDataByBranch(periodId, type);
			PayrollReportViewModel model = new PayrollReportViewModel
			{
				companies = await eRPCompanyService.GetAllCompany(),
                salaryPeriod = period,
                bonusSummaryByBranch = data
			};

			return View(model);
		}
		[AllowAnonymous]
		public IActionResult BonusSummarySheetBranchOrZoneWisePdf(int periodId, string type)
		{
			string schema = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string filename;

			url = schema+"://" + host + "/Payroll/PayrollReport/BonusSummarySheetBranchOrZoneWise?periodId=" + periodId + "&type=" + type;

			string status = myPDF.GeneratePDF(out filename, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}


        [AllowAnonymous]
        public async Task<ActionResult> HeadOfficeWiseBonusSummarySheet(int periodId, string type)
        {
			var salaryperiod = await salaryService.GetSalaryPeriodById(periodId);

			var data = await salaryService.GetBonusSummaryDataByHeadOffice(periodId, type);
            ViewBag.salaryPeriod = salaryperiod.mailText + " for the year " + salaryperiod.salaryYear?.yearName;
            PayrollReportViewModel model = new PayrollReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                headOfficeBonusSummaries = data,
                salaryPeriod = await salaryService.GetSalaryPeriodById(periodId)
            };

            return View(model);
        }
        [AllowAnonymous]
        public IActionResult HeadOfficeWiseBonusSummarySheetPdf(int periodId, string type)
        {
            string schema = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string filename;

            url = schema+"://" + host + "/Payroll/PayrollReport/HeadOfficeWiseBonusSummarySheet?periodId=" + periodId + "&type=" + type;

            string status = myPDF.GeneratePDF(out filename, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }







        [AllowAnonymous]
        public async Task<ActionResult> RecoveryforsalarizedView( int periodId)
        {
            PayrollReportViewModel model = new PayrollReportViewModel
            {
                //salaryPeriod = await salaryService.GetSalaryPeriodNameById(periodId),
                companies = await eRPCompanyService.GetAllCompany(),
                salariedLoanDeductionVms = await salaryService.GetSalariedLoanDeductionByperiodid(periodId)
            };

            return View(model);
        }
        [AllowAnonymous]
        public IActionResult RecoveryforsalarizedPdf(int periodId)
        {
            string schema = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string filename;

            url = schema+"://" + host + "/Payroll/PayrollReport/RecoveryforsalarizedView?periodId=" + periodId;

            string status = myPDF.GeneratePDF(out filename, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }

        [AllowAnonymous]
        public async Task<ActionResult> LoanRecoveryothersalaryView(DateTime? startDate, DateTime? endDate)
        {
            ViewBag.startDate = startDate?.ToString("dd MMM yyyy");
            ViewBag.endDate = endDate?.ToString("dd MMM yyyy");
            PayrollReportViewModel model = new PayrollReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                staffLoanEmployees = await salaryService.GetEmpInfoByLoan(),
                loanRecoveryotherSalaryVms = await salaryService.GetloanrecoveryotherthansalaryByDateRange(startDate, endDate)
            };

            return View(model);
        }
        [AllowAnonymous]
        public IActionResult LoanRecoveryothersalaryPdf(DateTime? startDate, DateTime? endDate)
        {
            string schema = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string filename;

            url = schema+"://" + host + "/Payroll/PayrollReport/LoanRecoveryothersalaryView?startDate=" + startDate + "&endDate=" + endDate;

            string status = myPDF.GeneratePDF(out filename, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }
        #endregion

        #region Bonus Report

        [AllowAnonymous]
		public async Task<IActionResult> MonthlyBonusReportPdf_BnB(int salaryPeriodId, int? sbuId, int? pnsId)
		{
			try
			{
				var model = new PayrollReportViewModel
				{
					monthlySalaryReportViewModels = await salaryService.GetMonthlySalaryReportByPeriodId(salaryPeriodId, sbuId, pnsId),
					companies = await eRPCompanyService.GetAllCompany(),
				};
				return PartialView(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		#endregion

		#region Tax Report

		[AllowAnonymous]
		public async Task<IActionResult> FinalTaxCertificateReportPdfAction(string rptType, int employeeInfoId, int taxYearId)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string fileName;

			if (rptType == "FTAXCERT")
			{
				var data = await salaryService.GetReportFormatByReportType(rptType);
				url = scheme + "://" + host + "/Payroll/PayrollReport/" + data.FirstOrDefault().reportFormat + "?employeeInfoId=" + employeeInfoId + "&taxYearId=" + taxYearId;
			}
			if (rptType == "TAXCAL")
			{
				var data = await salaryService.GetReportFormatByReportType(rptType);
				url = scheme + "://" + host + "/Payroll/PayrollReport/" + data.FirstOrDefault().reportFormat + "?employeeInfoId=" + employeeInfoId + "&taxYearId=" + taxYearId;
			}

			string status = myPDF.GeneratePDF(out fileName, url);

			FileName = fileName;
			if (status != "done")
			{
				return Content("<h1>" + status + "</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}

		[AllowAnonymous]
		public async Task<IActionResult> FinalTaxCertificateReportPdf(int employeeInfoId, int taxYearId)
		{
			try
			{
				var model = new PayrollReportViewModel
				{
					empTaxDetailsViewModels = await salaryService.GetEmpTaxDetails(employeeInfoId, taxYearId),
					empTaxSlabViewModels = await salaryService.GetEmpTaxSlab(employeeInfoId, taxYearId),
					//empRebatableTaxViewModels = await salaryService.GetEmpRebatableTax(employeeInfoId, taxYearId),
					//empTaxDeductFinalViewModels = await salaryService.GetEmpTaxDeductFinal(employeeInfoId, taxYearId),
					companies = await eRPCompanyService.GetAllCompany(),
				};
				return PartialView(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[AllowAnonymous]
		public async Task<IActionResult> TaxCalculationReport(int employeeInfoId, int taxYearId, int periodId)
		{
			try
			{
				//var model = new PayrollReportViewModel
				//{
				//    companies = await eRPCompanyService.GetAllCompany(),
				//};
				ViewBag.employeeId = employeeInfoId;
				ViewBag.taxyearId = taxYearId;
				ViewBag.periodId = periodId;
				var companys = await eRPCompanyService.GetAllCompany();
				TaxCertificateViewModel model = new TaxCertificateViewModel
				{
					company = companys.LastOrDefault(),
					employeeInfo = await personalInfoService.GetEmployeeInfoById(employeeInfoId),
					salaryPeriod = await salaryService.GetSalaryPeriodNameById(periodId),
					taxableamountViewModels = await salaryService.TaxableamountViewModels(employeeInfoId, taxYearId),
					taxableSlabViewModels = await salaryService.TaxableslabViewModels(employeeInfoId, taxYearId),
					taxablePFViewModels = await salaryService.TaxablePFViewModels(employeeInfoId, taxYearId),
					InvestmentRebateSettings = await salaryService.GetAllRebateSettingbytaxyearid(taxYearId),
					taxYear = await salaryService.TaxYearbyId(taxYearId)


				};
				return PartialView(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		[AllowAnonymous]
		public IActionResult TaxCalculationReportAction(int employeeInfoId, int taxYearId, int periodId)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string fileName;

			//url = scheme + "://" + host + "/Payroll/PayrollReport/" + data.FirstOrDefault().reportFormat + "?employeeInfoId=" + employeeInfoId + "&taxYearId=" + taxYearId;
			url = scheme + "://" + host + "/Payroll/PayrollReport/TaxCalculationReportPdf?employeeInfoId=" + employeeInfoId + "&taxYearId=" + taxYearId + "&periodId=" + periodId;

			string status = myPDF.GeneratePDF(out fileName, url);

			FileName = fileName;
			if (status != "done")
			{
				return Content("<h1>" + status + "</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}
		[AllowAnonymous]
		public async Task<IActionResult> TaxCalculationReportPdf(int employeeInfoId, int taxYearId, int periodId)
		{
			try
			{
				//var model = new PayrollReportViewModel
				//{
				//    companies = await eRPCompanyService.GetAllCompany(),
				//};
				var companys = await eRPCompanyService.GetAllCompany();
				TaxCertificateViewModel model = new TaxCertificateViewModel
				{
					company = companys.LastOrDefault(),
					employeeInfo = await personalInfoService.GetEmployeeInfoById(employeeInfoId),
					salaryPeriod = await salaryService.GetSalaryPeriodNameById(periodId),
					taxableamountViewModels = await salaryService.TaxableamountViewModels(employeeInfoId, taxYearId),
					taxableSlabViewModels = await salaryService.TaxableslabViewModels(employeeInfoId, taxYearId),
					taxablePFViewModels = await salaryService.TaxablePFViewModels(employeeInfoId, taxYearId),
					InvestmentRebateSettings = await salaryService.GetAllRebateSettingbytaxyearid(taxYearId),
					taxYear = await salaryService.TaxYearbyId(taxYearId)

				};

				return PartialView(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		#endregion

		[HttpGet]
		public async Task<ActionResult> AllLoan()
		{
			PayrollReportViewModel model = new PayrollReportViewModel
			{
				departments = await employeePunchCardInfoService.GetAllDepartment(),
				hrBranches = await employeePunchCardInfoService.GetAllHrBranches(),
				hrDivisions = await employeePunchCardInfoService.GetAllHrDivisions(),
				functionInfos = await employeePunchCardInfoService.GetFunctionInfo(),
				hrUnits = await statusService.GetHrUnit(),
				locations = await locationService.GetLocation(),
				specialBranchUnits = await specialBranchUnitService.GetSpecialBranchUnit(),
			};
			return View(model);
		}

		[HttpGet]
		public ActionResult IndividualLoan()
		{
			//string userName = HttpContext.User.Identity.Name;
			//var user = await _userManager.FindByNameAsync(userName);
			//var emp = await personalInfoService.GetEmployeeInfoByCode(user.EmpCode);
			//ViewBag.empcode = emp.employeeCode;
			//ViewBag.employeename = emp.nameEnglish;
			//var model = new PayrollReportViewModel
			//{

			//};
			return View();
		}
      
        #region Payroll Report 2
        public async Task<IActionResult> SalaryReportsNew()
		{
			var model = new SalaryLedgerReportView
			{
				hrBranches = await personalInfoService.GetAllBranch(),
				departments = await personalInfoService.GetAllDepartment(),
				hrDivisions = await personalInfoService.GetAllHrDivision(),
				hrUnits = await personalInfoService.GetAllHrUnits(),
				offices = await personalInfoService.GetAllOffices(),
				zones = await personalInfoService.GetAllZone(),
				salaryPeriods = await salaryService.GetAllSalaryPeriod(),
				salaryHeads = await salaryService.GetAllSalaryHead()
			};
			return View(model);
		}

		public async Task<IActionResult> NetPayReport()
		{
			var model = new SalaryLedgerReportView
			{
				hrBranches = await personalInfoService.GetAllBranch(),
				departments = await personalInfoService.GetAllDepartment(),
				hrDivisions = await personalInfoService.GetAllHrDivision(),
				hrUnits = await personalInfoService.GetAllHrUnits(),
				offices = await personalInfoService.GetAllOffices(),
				zones = await personalInfoService.GetAllZone(),
				salaryPeriods = await salaryService.GetAllSalaryPeriod(),
				salaryHeads = await salaryService.GetAllSalaryHead()
			};
			return View(model);
		}
		[AllowAnonymous]
		public async Task<IActionResult> PfTypeReportView(int? hrBranchId, int? departmentId, int? hrDivisionId, int? hrUnitId, int? officeId, int? zoneId, int? salaryPeriodId, int? salaryHeadId)
		{
			var branchName = await personalInfoService.GetBranchNameById(hrBranchId);
			ViewBag.branchName = branchName;
			var HeadName = await salaryService.GetHeadNameById(salaryHeadId);
			ViewBag.SHeadName = HeadName;

			var data = await salaryService.GetSalaryLedgerReports(salaryPeriodId, salaryHeadId);
			if (hrBranchId != 0)
			{
				data = data.Where(x => x.hrBranchId == hrBranchId).ToList();
			}
			else if (departmentId != 0)
			{
				data = data.Where(x => x.departmentId == departmentId).ToList();
			}
			else if (hrDivisionId != 0)
			{
				data = data.Where(x => x.hrDivisionId == hrDivisionId).ToList();
			}
			else if (hrUnitId != 0)
			{
				data = data.Where(x => x.hrUnitId == hrUnitId).ToList();
			}
			else if (officeId != 0)
			{
				data = data.Where(x => x.officeId == officeId).ToList();
			}
			else if (zoneId != 0)
			{
				data = data.Where(x => x.zoneId == zoneId).ToList();
			}
			else if (departmentId != 0)
			{
				data = data.ToList();
			}
			var model = new SalaryLedgerReportView
			{
				companies = await eRPCompanyService.GetAllCompany(),
				salaryLedgers = data
			};
			return View(model);
		}
		[AllowAnonymous]
		public IActionResult PfTypeReportPdf(int? hrBranchId, int? departmentId, int? hrDivisionId, int? hrUnitId, int? officeId, int? zoneId, int? salaryPeriodId, int? salaryHeadId)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string fileName;

			url = scheme + "://" + host + "/Payroll/PayrollReport/PfTypeReportView?hrBranchId=" + hrBranchId + "&departmentId=" + departmentId + "&hrDivisionId=" + hrDivisionId + "&hrUnitId=" + hrUnitId + "&officeId=" + officeId + "&zoneId=" + zoneId + "&salaryPeriodId=" + salaryPeriodId + "&salaryHeadId=" + salaryHeadId;

			string status = myPDF.GeneratePDF(out fileName, url);

			FileName = fileName;
			if (status != "done")
			{
				return Content("<h1>" + status + "</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}
		[AllowAnonymous]
		public async Task<IActionResult> NewPayReportView(int? hrBranchId, int? departmentId, int? hrDivisionId, int? hrUnitId, int? officeId, int? zoneId, int? salaryPeriodId, int? salaryHeadId)
		{
			var data = await salaryService.GetSalaryNetPayReports(salaryPeriodId);
			if (hrBranchId != 0)
			{
				data = data.Where(x => x.hrBranchId == hrBranchId).ToList();
			}
			else if (departmentId != 0)
			{
				data = data.Where(x => x.departmentId == departmentId).ToList();
			}
			else if (hrDivisionId != 0)
			{
				data = data.Where(x => x.hrDivisionId == hrDivisionId).ToList();
			}
			else if (hrUnitId != 0)
			{
				data = data.Where(x => x.hrUnitId == hrUnitId).ToList();
			}
			else if (officeId != 0)
			{
				data = data.Where(x => x.officeId == officeId).ToList();
			}
			else if (zoneId != 0)
			{
				data = data.Where(x => x.zoneId == zoneId).ToList();
			}
			else if (departmentId != 0)
			{
				data = data.ToList();
			}
			var model = new SalaryLedgerReportView
			{
				companies = await eRPCompanyService.GetAllCompany(),
				salaryLedgers = data
			};
			return View(model);
		}
		[AllowAnonymous]
		public IActionResult NewPayReportPdf(int? hrBranchId, int? departmentId, int? hrDivisionId, int? hrUnitId, int? officeId, int? zoneId, int? salaryPeriodId, int? salaryHeadId)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string fileName;

			url = scheme + "://" + host + "/Payroll/PayrollReport/NewPayReportView?hrBranchId=" + hrBranchId + "&departmentId=" + departmentId + "&hrDivisionId=" + hrDivisionId + "&hrUnitId=" + hrUnitId + "&officeId=" + officeId + "&zoneId=" + zoneId + "&salaryPeriodId=" + salaryPeriodId + "&salaryHeadId=" + salaryHeadId;

			string status = myPDF.GeneratePDF(out fileName, url);

			FileName = fileName;
			if (status != "done")
			{
				return Content("<h1>" + status + "</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}
		#endregion

		#region API
		[AllowAnonymous]
		[Route("global/api/GetUniverdata/{employeeInfoId}/{salaryPeriodId}")]
		[HttpGet]
		public async Task<IActionResult> GetUniverdata(int employeeInfoId, int salaryPeriodId)
		{
			return Json(await salaryService.GetUniversalCodaXLTempleteViewModels(salaryPeriodId, employeeInfoId));
		}

		[Route("global/api/GetBankStatementByPeriodId/{salaryPeriodId}/{sbuId}/{pnsId}")]
		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> GetBankStatementByPeriodId(int salaryPeriodId, int? sbuId, int? pnsId)
		{
			var data = await salaryService.GetBankStatementByPeriodId(salaryPeriodId, sbuId, pnsId);
			return Json(data.Where(a => a.bankAccountNo != "" && a.bankPayable != 0));
		}

		[Route("global/api/GetCashStatementByPeriodId/{salaryPeriodId}/{sbuId}/{pnsId}")]
		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> GetCashStatementByPeriodId(int salaryPeriodId, int? sbuId, int? pnsId)
		{
			var data = await salaryService.GetBankStatementByPeriodId(salaryPeriodId, sbuId, pnsId);
			return Json(data.Where(a => a.cashPayable != 0));
		}

		[Route("global/api/GetWalletStatementByPeriodId/{salaryPeriodId}/{sbuId}/{pnsId}")]
		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> GetWalletStatementByPeriodId(int salaryPeriodId, int? sbuId, int? pnsId)
		{
			var data = await salaryService.GetBankStatementByPeriodId(salaryPeriodId, sbuId, pnsId);
			return Json(data.Where(a => a.walletPayable != 0));
		}

		[Route("global/api/GetReconcilationStatement/{salaryPeriodId}/{presalaryPeriodId}/{typeId}")]
		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> GetReconcilationStatement(int? salaryPeriodId, int? presalaryPeriodId, int? typeId)
		{
			return Json(await salaryService.GetReconcilationStatement(0, salaryPeriodId, presalaryPeriodId, typeId));
		}

		#endregion


		#region SalarySummarySheet

		[AllowAnonymous]
		public IActionResult SalaryYearlySummarySheetHeadOfficePdf(string fromDate, string toDate)
		{

			string scheme = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string fileName;

			url = scheme + "://" + host + "/Payroll/PayrollReport/SalaryYearlySummarySheetHeadOfficeView?fromDate=" + fromDate + "&toDate=" + toDate;

			string status = myPDF.GenerateLandscapePDF(out fileName, url);

			FileName = fileName;
			if (status != "done")
			{
				return Content("<h1>" + status + "</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}


		[AllowAnonymous]
		public IActionResult BranchWiseSalarySummaryReport(string rptType, int salaryPeriodId)
		{

			string scheme = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string fileName;

			if (rptType == "BWSS")
			{
				//var data = await salaryService.GetReportFormatByReportType(rptType);
				url = scheme + "://" + host + "/Payroll/PayrollReport/BranchWiseSalarySummaryReportPdf?salaryPeriodId=" + salaryPeriodId;

			}
			if (rptType == "TBSS")
			{
				url = scheme + "://" + host + "/Payroll/PayrollReport/SalarySummaryReportPdf?salaryPeriodId=" + salaryPeriodId;
			}

			string status = myPDF.GenerateLandscapePDF(out fileName, url);

			FileName = fileName;
			if (status != "done")
			{
				return Content("<h1>" + status + "</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}


		[AllowAnonymous]
		public async Task<IActionResult> BranchWiseSalarySummaryReportPdf(int salaryPeriodId)
		{
			try
			{


				var data = new List<SalaryWithDesignation>();
				var Branches = await salaryService.GetAllHrBranchs();
				var branchSalaryReportViewModels = await salaryService.GetSalarySummarySheetBranch(salaryPeriodId);

				var model = new PayrollReportViewModel
				{
					companies = await eRPCompanyService.GetAllCompany(),

					branchSalarySummaryViewModels = branchSalaryReportViewModels
				};
				return View(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[AllowAnonymous]
		public async Task<IActionResult> SalarySummaryReportPdf(int salaryPeriodId)
		{
			try
			{


				var data = new List<SalaryWithDesignation>();
				var Branches = await salaryService.GetAllHrBranchs();
				var branchSalaryReportViewModels = await salaryService.GetSalarySummarySheetBranch(salaryPeriodId);

				var model = new PayrollReportViewModel
				{
					companies = await eRPCompanyService.GetAllCompany(),
					branchSalarySummaryViewModels = branchSalaryReportViewModels,
					salarySummarySheetSpViewModels = await salaryService.GetSalarySummarySheetHeadOffice(salaryPeriodId),
					branchSalarySummaryViewModelZone = await salaryService.GetSalarySummarySheetZone(salaryPeriodId)
				};
				return View(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		
		
		[AllowAnonymous]
		public async Task<IActionResult> SalaryYearlySummarySheetHeadOfficeView(string fromDate, string toDate)
		{
			try
			{
				ViewBag.fromDate = Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy");
				ViewBag.toDate = Convert.ToDateTime(toDate).ToString("dd-MMM-yyyy");

				var data = new List<SalaryWithDesignation>();
				var Branches = await salaryService.GetAllHrBranchs();
				var branchSalaryReportViewModels = await salaryService.GetYearlySalarySummarySheetBranch(fromDate, toDate);

				var model = new PayrollReportViewModel
				{
					companies = await eRPCompanyService.GetAllCompany(),
					branchSalarySummaryViewModels = branchSalaryReportViewModels,
				};
				return View(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}




		[AllowAnonymous]
		public async Task<IActionResult> ZoneWiseSalarySummaryReport(string rptType, int salaryPeriodId)
		{

			string scheme = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string fileName;

			if (rptType == "ZWSS")
			{
				var data = await salaryService.GetReportFormatByReportType(rptType);
				url = scheme + "://" + host + "/Payroll/PayrollReport/ZoneWiseSalarySummaryReportPdf?salaryPeriodId=" + salaryPeriodId;

			}

			string status = myPDF.GenerateLandscapePDF(out fileName, url);

			FileName = fileName;
			if (status != "done")
			{
				return Content("<h1>" + status + "</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}


		[AllowAnonymous]
		public async Task<IActionResult> ZoneWiseSalarySummaryReportPdf(int salaryPeriodId)
		{
			try
			{

				var zoneWiseSalaryReportViewModels = await salaryService.GetSalarySummarySheetZone(salaryPeriodId);

				var model = new PayrollReportViewModel
				{
					companies = await eRPCompanyService.GetAllCompany(),

					branchSalarySummaryViewModels = zoneWiseSalaryReportViewModels
				};
				return View(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}



		#endregion


		#region SalarySummarySheetHeadOffice

		[AllowAnonymous]
		public async Task<IActionResult> SalarySummarySheetHeadOfficePdf(int salaryPeriodId)
		{

			string scheme = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string fileName;

			//if (rptType == "BWSS")
			//{
			//    var data = await salaryService.GetReportFormatByReportType(rptType);
			//    url = scheme + "://" + host + "/Payroll/PayrollReport/SalarySummarySheetHeadOffice?salaryPeriodId=" + salaryPeriodId;

			//}

			url = scheme + "://" + host + "/Payroll/PayrollReport/SalarySummarySheetHeadOffice?salaryPeriodId=" + salaryPeriodId;

			string status = myPDF.GenerateLandscapePDF(out fileName, url);

			FileName = fileName;
			if (status != "done")
			{
				return Content("<h1>" + status + "</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}


		[AllowAnonymous]
		public async Task<IActionResult> SalarySummarySheetHeadOffice(int salaryPeriodId)
		{
			try
			{


				var data = new List<SalaryWithDesignation>();
				var designations = await salaryService.GetAllDesignations();
				var SalarySummarySheetHeadOffice = await salaryService.GetSalarySummarySheetHeadOffice(salaryPeriodId);
				//foreach (var item in designations)
				//{
				//    data.Add(new SalaryWithDesignation
				//    {
				//        designation = item,
				//        branchMonthlySalaryReports = SalarySummarySheetHeadOffice.Where(x => x.Designation == item.designationName)
				//    });
				//}
				var model = new PayrollReportViewModel
				{
					companies = await eRPCompanyService.GetAllCompany(),
					salarySummarySheetSpViewModels = SalarySummarySheetHeadOffice
				};
				return View(model);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        #endregion

        #region InDivisualSalaryReport
        public async Task<IActionResult> InDivisualSalaryReport()
        {
			var username = User.Identity.Name;
			var user = await personalInfoService.GetEmployeeInfoByCode(username);
			ViewBag.empCode = user.employeeCode;
			ViewBag.empName = user.nameEnglish;

            var emp = await personalInfoService.GetEmployeeInfoByCode(User.Identity.Name);
            PayrollReportViewModel model = new PayrollReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeInfo = emp,
				designations = await personalInfoService.GetAllDesignation()
            };
            return View(model);
        }

		public async Task<IActionResult> SalaryAllowanceReport()
		{
			PayrollReportViewModel model = new PayrollReportViewModel
			{
				companies = await eRPCompanyService.GetAllCompany(),
			};
			return View(model);
		}

		[AllowAnonymous]
        public async Task<IActionResult> IndivisualSalaryPDFView(string empCode, DateTime fDate, DateTime tDate)
        {
            ViewBag.fDate = fDate;
            ViewBag.tDate = tDate;
            var emp = await personalInfoService.GetEmployeeInfoByCode(empCode);
            ViewBag.Name = emp.nameEnglish;
            ViewBag.Code = emp.employeeCode;
            ViewBag.designation = emp.designation;

            PayrollReportViewModel model = new PayrollReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeInfo = emp,
                indivisualEmpSalaryReportViewModels = await salaryService.GetIndivisualSalaryReportByEmpIdandDate(empCode,fDate,tDate),
                bonusTaxReports = await salaryService.GetBonusTaxReportByEmpCode(empCode,fDate,tDate)
            };
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> AllTaxSalaryPDFView(int desigId, DateTime fDate, DateTime tDate)
        {
			
            ViewBag.fDate = fDate;
            ViewBag.tDate = tDate;

            var model = new PayrollReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeInfos = await personalInfoService.GetAllEmployeeInfoWithPosting(),
                indivisualEmpSalaryReportViewModels = await salaryService.GetIndivisualSalaryReportByDesigIdandDate(desigId,fDate,tDate),
                bonusTaxReports = await salaryService.GetBonusTaxReportByDesig(desigId,fDate,tDate)
            };
            return View(model);
        }


        [AllowAnonymous]
        public IActionResult IndivisualSalaryPDF(string empCode, DateTime fDate, DateTime tDate)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string fileName;

            url = scheme + "://" + host + "/Payroll/PayrollReport/IndivisualSalaryPDFView?empCode=" + empCode + "&fDate=" + fDate + "&tDate=" + tDate;


            string status = myPDF.GenerateLegalPDF(out fileName, url);

            FileName = fileName;
            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
		
		[AllowAnonymous]
        public IActionResult AllTaxSalaryPDF(int desigId, DateTime fDate, DateTime tDate)
        {
			fDate = fDate < new DateTime(2021, 12, 1) ? new DateTime(2021, 12, 1) : fDate;

			string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string fileName;

            url = scheme + "://" + host + "/Payroll/PayrollReport/AllTaxSalaryPDFView?desigId=" + desigId + "&fDate=" + fDate + "&tDate=" + tDate;

            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            FileName = fileName;
            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }


		[AllowAnonymous]
		public async Task<IActionResult> AllTaxSalaryPDFView1(int desigId, DateTime fDate, DateTime tDate)
		{

			ViewBag.fDate = fDate;
			ViewBag.tDate = tDate;

            var model = new PayrollReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeInfos = await personalInfoService.GetAllEmployeeInfoWithPosting(),
                indivisualEmpSalaryReportViewModels = await salaryService.GetIndivisualSalaryReportByDesigIdandDate(desigId, fDate, tDate),
                bonusTaxReports = await salaryService.GetBonusTaxReportByDesig(desigId, fDate, tDate),
                salaryPeriods = await salaryService.GetSalaryPeriodByDateRange(fDate, tDate)
            };
			return View(model);
		}
		[AllowAnonymous]
		public IActionResult AllTaxSalaryPDF1(int desigId, DateTime fDate, DateTime tDate)
		{
			fDate = fDate < new DateTime(2021, 12, 1) ? new DateTime(2021, 12, 1) : fDate;

			string scheme = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string fileName;

			url = scheme + "://" + host + "/Payroll/PayrollReport/AllTaxSalaryPDFView1?desigId=" + desigId + "&fDate=" + fDate + "&tDate=" + tDate;

			string status = myPDF.GenerateLegalPDF(out fileName, url);

			FileName = fileName;
			if (status != "done")
			{
				return Content("<h1>" + status + "</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}

		[AllowAnonymous]
		public async Task<IActionResult> BetonVataAllView(DateTime fDate, DateTime tDate)
		{
			PayrollReportViewModel model = new PayrollReportViewModel
			{
				companies = await eRPCompanyService.GetAllCompany(),
				betonVataViewModel = await salaryService.GetBetonVataAll(fDate, tDate)
			};
			return View(model);
		}


		[AllowAnonymous]
		public IActionResult BetonVataAllPDF(DateTime fDate, DateTime tDate)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string fileName;

			url = scheme + "://" + host + "/Payroll/PayrollReport/BetonVataAllView?fDate=" + fDate + "&tDate=" + tDate;

			string status = myPDF.GeneratePDF(out fileName, url);

			FileName = fileName;
			if (status != "done")
			{
				return Content("<h1>" + status + "</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}
        #endregion


        [AllowAnonymous]
        public async Task<IActionResult> ArrearPayslipReportPdf(int employeeInfoId, int salaryPeriodId)
        {
            try
            {
                var model = new PayrollReportViewModel
                {
                    payslipReportViewModels = await salaryService.GetPaySlipByEmpId(employeeInfoId, salaryPeriodId),
                    payslipAdditionViewModels = await salaryService.GetPaySlipAdditionByEmpId(employeeInfoId, salaryPeriodId),
                    payslipDeductionViewModels = await salaryService.GetPaySlipDeductionByEmpId(employeeInfoId, salaryPeriodId),
                    companies = await eRPCompanyService.GetAllCompany(),
                };
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }






    }
}