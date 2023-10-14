using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.Areas.HRPMSMasterData.Models.Lang;
using OPUSERP.Areas.HRPMSReport.Models;
using OPUSERP.Data.Entity;
using OPUSERP.Data.Entity.Master;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.Leave.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.HRPMS.Services.Report;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IDesignationDepartmentService = OPUSERP.HRPMS.Services.MasterData.Interfaces.IDesignationDepartmentService;

namespace OPUSERP.Areas.HRPMSReport.Controllers
{
    [Authorize]
    [Area("HRPMSReport")]
    public class HrAdminReportController : Controller
    {
		

		private readonly IReports reports;
        private readonly IERPCompanyService eRPCompanyService;
        private readonly LangGenerate<EmployeeInfoLn> _lang;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ERPServices.MasterData.Interfaces.IDesignationDepartmentService designationDepartmentService;
        private readonly ISpecialBranchUnitService specialBranchUnitService;
        private readonly IYearCourseTitleService yearCourseTitleService;
        private readonly IOrganizationService organizationService;
        private readonly ILevelofEducationService levelofEducationService;
        private readonly IBelongingsItemService belongingsItemService;
        private readonly ISubjectService subjectService;
        private readonly ICustomRoleService customRoleService;
        private readonly ITravelService travelService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly IEmployeePunchCardInfoService hrBranchService;
        private readonly ILocationService locationService;
		private readonly IStatusService _statusService;
		private readonly IFunctionsInfoService _functionsInfoService;
		private readonly OPUSERP.HRPMS.Services.MasterData.Interfaces.IAddressService addressService;
        private readonly IDesignationDepartmentService designationDepartService;

        private readonly string rootPath;
        private readonly MyPDF myPDF;

        public HrAdminReportController(ICustomRoleService customRoleService, IPersonalInfoService personalInfoService, IEmployeePunchCardInfoService hrBranchService, IReports reports, ITravelService travelService, IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, ERPServices.MasterData.Interfaces.IDesignationDepartmentService designationDepartmentService, IERPCompanyService eRPCompanyService, ISpecialBranchUnitService specialBranchUnitService, IBelongingsItemService belongingsItemService, ISubjectService subjectService, IOrganizationService organizationService, IYearCourseTitleService yearCourseTitleService, ILevelofEducationService levelofEducationService, IConverter converter, ILocationService locationService, IStatusService statusService, IFunctionsInfoService functionsInfoService, OPUSERP.HRPMS.Services.MasterData.Interfaces.IAddressService addressService, IDesignationDepartmentService designationDepartService)
        {
            _lang = new LangGenerate<EmployeeInfoLn>(hostingEnvironment.ContentRootPath);

            this.designationDepartmentService = designationDepartmentService;
            this.specialBranchUnitService = specialBranchUnitService;
            this.subjectService = subjectService;
            this.belongingsItemService = belongingsItemService;
            this.organizationService = organizationService;
            this.levelofEducationService = levelofEducationService;
            this.yearCourseTitleService = yearCourseTitleService;
            this.eRPCompanyService = eRPCompanyService;
            this.personalInfoService = personalInfoService;
            this.hrBranchService = hrBranchService;
            this.customRoleService = customRoleService;
            _userManager = userManager;
            this.reports = reports;
            this.travelService = travelService;


            //For PDF
            myPDF = new MyPDF(hostingEnvironment, converter);
            rootPath = hostingEnvironment.ContentRootPath;
            this.locationService = locationService;
            this._statusService = statusService;
            this._functionsInfoService = functionsInfoService;
			this.addressService = addressService;
            this.designationDepartService = designationDepartService;
        }

        public async Task<ActionResult> Index()
        {
            var model = new AllHrReportViewModel
            {
                specialBranchUnits = await specialBranchUnitService.GetSpecialBranchUnit(),
                subjects = await subjectService.GetAllSubject(),
                organizations = await organizationService.GetAllOrganization(),
                religions = await organizationService.GetAllReligions(),
                divisions = await organizationService.GetAllDivisions(),
                districts = await organizationService.GetAllDistrict(),
                branches = await organizationService.GetAllBranch(),
                courseTitles = await yearCourseTitleService.GetCourseTitle(),
                levelofEducations = await levelofEducationService.GetAllLevelofEducation(),
                belongingItems = await belongingsItemService.GetBelongingItem(),
                designations = await designationDepartmentService.GetDesignations(),
                customRoles = await customRoleService.GetCustomRole(),
                hrBranches = await organizationService.GetAllHrBranch(),
                hrDivisions = await organizationService.GetAllHrDivision(),
                departments = await organizationService.GetAllDepartments(),
                functionInfos = await organizationService.GetAllOffices(),

            };
            return View(model);
        }

        #region Report
        [AllowAnonymous]
        public IActionResult HrReportDataByBelongingItem(string rptType, int? belongingId)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string fileName;

            if (rptType == "9")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportByBelongingPdf?belongingId=" + belongingId;
            }

            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        [AllowAnonymous]
        public IActionResult HrReportDataByBelongingItemExcel(string rptType, int? belongingId)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string fileName;

            if (rptType == "9")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportByBelongingPdf?belongingId=" + belongingId;
            }

            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }


            string pdfpath = rootPath + @"\wwwroot\pdf\" + fileName;
            string wordpath = fileName.Replace(".pdf", ".xls");
            SautinSoft.PdfFocus f = new SautinSoft.PdfFocus();
            f.OpenPdf(rootPath + @"\wwwroot\pdf\" + fileName);
            f.ToExcel(rootPath + @"\wwwroot\pdf\" + wordpath);
            //  var stream = new FileStream(rootPath + "/wwwroot/pdf/" + wordpath, FileMode.Open);
            var model = wordpath;
            return Json(model);
        }

        [AllowAnonymous]
        public IActionResult HrReportDataByTrainingCourse(string rptType, int? courseId)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string fileName;

            if (rptType == "8")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportByTrainingPdf?courseId=" + courseId;
            }

            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        [AllowAnonymous]
        public IActionResult HrReportDataByTrainingCourseExcel(string rptType, int? courseId)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string fileName;

            if (rptType == "8")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportByTrainingPdf?courseId=" + courseId;
            }

            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            string pdfpath = rootPath + @"\wwwroot\pdf\" + fileName;
            string wordpath = fileName.Replace(".pdf", ".xls");
            SautinSoft.PdfFocus f = new SautinSoft.PdfFocus();
            f.OpenPdf(rootPath + @"\wwwroot\pdf\" + fileName);
            f.ToExcel(rootPath + @"\wwwroot\pdf\" + wordpath);
            //  var stream = new FileStream(rootPath + "/wwwroot/pdf/" + wordpath, FileMode.Open);
            var model = wordpath;
            return Json(model);
        }
        [AllowAnonymous]
        public IActionResult HrReportDataByEducation(string rptType, int? subjectId, int? universityId, int? loeId)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string fileName;

            if (rptType == "5")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportBySubjectPdf?subjectId=" + subjectId + "&universityId=" + universityId + "&loeId=" + loeId;
            }
            if (rptType == "6")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportByUniversityPdf?subjectId=" + subjectId + "&universityId=" + universityId + "&loeId=" + loeId;
            }
            if (rptType == "7")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportByLoePdf?subjectId=" + subjectId + "&universityId=" + universityId + "&loeId=" + loeId;
            }

            string status = myPDF.GeneratePDF1(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        [AllowAnonymous]
        public IActionResult HrReportDataByEducationExcel(string rptType, int? subjectId, int? universityId, int? loeId)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string fileName;

            if (rptType == "5")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportBySubjectPdf?subjectId=" + subjectId + "&universityId=" + universityId + "&loeId=" + loeId;
            }
            if (rptType == "6")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportByUniversityPdf?subjectId=" + subjectId + "&universityId=" + universityId + "&loeId=" + loeId;
            }
            if (rptType == "7")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportByLoePdf?subjectId=" + subjectId + "&universityId=" + universityId + "&loeId=" + loeId;
            }

            string status = myPDF.GeneratePDF1(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            string pdfpath = rootPath + @"\wwwroot\pdf\" + fileName;
            string wordpath = fileName.Replace(".pdf", ".xls");
            SautinSoft.PdfFocus f = new SautinSoft.PdfFocus();
            f.OpenPdf(rootPath + @"\wwwroot\pdf\" + fileName);
            f.ToExcel(rootPath + @"\wwwroot\pdf\" + wordpath);
            //  var stream = new FileStream(rootPath + "/wwwroot/pdf/" + wordpath, FileMode.Open);
            var model = wordpath;
            return Json(model);
        }

        [AllowAnonymous]
        public IActionResult HrReportDataByDesig(string rptType, int? desigId, int? deptId, string bloodGroup, int? sbuId)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string fileName;

			string status = "";
			if (rptType == "3")
			{
				url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportDataByBloodPdf?desigId=" + desigId + "&deptId=" + deptId + "&bloodGroup=" + bloodGroup + "&sbuId=" + sbuId;
				status = myPDF.GeneratePDF(out fileName, url);
			}
			else
			{
				if (rptType == "1")
				{
					url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportByDepartmentPdf?desigId=" + desigId + "&deptId=" + deptId + "&bloodGroup=" + bloodGroup + "&sbuId=" + sbuId;
				}
				if (rptType == "2")
				{
					url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportByDesigPdf?desigId=" + desigId + "&deptId=" + deptId + "&bloodGroup=" + bloodGroup + "&sbuId=" + sbuId;
				}
				else if (rptType == "4")
				{
					url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportDataBySbuPdf?desigId=" + desigId + "&deptId=" + deptId + "&bloodGroup=" + bloodGroup + "&sbuId=" + sbuId;
				}
				else if (rptType == "10")
				{
					url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportDataByDesigSummaryPdf?desigId=" + desigId + "&deptId=" + deptId + "&bloodGroup=" + bloodGroup + "&sbuId=" + sbuId;
				}
				status = myPDF.GenerateLandscapePDF(out fileName, url);
			}



			if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [AllowAnonymous]
        public async Task<IActionResult> HRReportForSignaturesView()
        {
            var data = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                signatureReports = await reports.GetSignatureLists()
            };
            return View(data);
        }

        [AllowAnonymous]
        public IActionResult HRReportForSignatures()
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string fileName;

            string status = "";

            url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HRReportForSignaturesView";
            status = myPDF.GeneratePDF(out fileName, url);


            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }


        [AllowAnonymous]
        public IActionResult HrReportDataByDesigExcel(string rptType, string desigId, int? deptId, string bloodGroup, int? sbuId)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string fileName;

            if (rptType == "1")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportByDepartmentPdf?desigId=" + desigId + "&deptId=" + deptId + "&bloodGroup=" + bloodGroup + "&sbuId=" + sbuId;
            }
            if (rptType == "2")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportByDesigPdf?desigId=" + desigId + "&deptId=" + deptId + "&bloodGroup=" + bloodGroup + "&sbuId=" + sbuId;
            }
            else if (rptType == "3")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportDataByBloodPdf?desigId=" + desigId + "&deptId=" + deptId + "&bloodGroup=" + bloodGroup + "&sbuId=" + sbuId;
            }
            else if (rptType == "4")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportDataBySbuPdf?desigId=" + desigId + "&deptId=" + deptId + "&bloodGroup=" + bloodGroup + "&sbuId=" + sbuId;
            }
            else if (rptType == "10")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportDataByDesigSummaryPdf?desigId=" + desigId + "&deptId=" + deptId + "&bloodGroup=" + bloodGroup + "&sbuId=" + sbuId;
            }

            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            string pdfpath = rootPath + @"\wwwroot\pdf\" + fileName;
            string wordpath = fileName.Replace(".pdf", ".xls");
            SautinSoft.PdfFocus f = new SautinSoft.PdfFocus();
            f.OpenPdf(rootPath + @"\wwwroot\pdf\" + fileName);
            f.ToExcel(rootPath + @"\wwwroot\pdf\" + wordpath);
            //  var stream = new FileStream(rootPath + "/wwwroot/pdf/" + wordpath, FileMode.Open);
            var model = wordpath;
            return Json(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> HrReportByBelongingPdf(int? belongingId)
        {
            AllHrReportViewModel model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                hrBelongingReportViewModels = await reports.GetHrDataByBelongingItem(belongingId)
            };
            return View(model);
        }
        [AllowAnonymous]
        public async Task<ActionResult> HrReportByTrainingPdf(int? courseId)
        {
            AllHrReportViewModel model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                hrTrainingReportViewModels = await reports.GetHrDataByTrCourse(courseId)
            };
            return View(model);
        }
        [AllowAnonymous]
        public async Task<ActionResult> HrReportBySubjectPdf(int? subjectId, int? universityId, int? loeId)
        {
            //var subject1 = await reports.GetEduCationalSubjectNameById(subjectId);
            //ViewBag.majorGroup = subject1.reldegreesubject?.subject;
            AllHrReportViewModel model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                hrEducationReportViewModels = await reports.GetHrDataByEducation(subjectId, universityId, loeId)
            };
            return View(model);
        }
        
        public async Task<ActionResult> HrReportByEducationApi(int? subjectId, int? universityId, int? loeId)
        {
            var model = new HrReportApiViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                hrEducationReportViewModels = await reports.GetHrDataByEducation(subjectId, universityId, loeId)
            };
            return Json(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> HrReportByUniversityPdf(int? subjectId, int? universityId, int? loeId)
        {
            AllHrReportViewModel model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                hrEducationReportViewModels = await reports.GetHrDataByEducation(subjectId, universityId, loeId)
            };
            return View(model);
        }
        [AllowAnonymous]
        public async Task<ActionResult> HrReportByLoePdf(int? subjectId, int? universityId, int? loeId)
        {
            AllHrReportViewModel model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                hrEducationReportViewModels = await reports.GetHrDataByEducation(subjectId, universityId, loeId)
            };
            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> HrReportByDepartmentPdf(int? desigId, int? deptId, string bloodGroup, int? sbuId)
        {

            AllHrReportViewModel model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                hrReportViewModels = await reports.GetHrDataByDesig(desigId, deptId, bloodGroup, sbuId)
            };

            return View(model);
        }
        [AllowAnonymous]
        public IActionResult HrReportDataBySummary(string rptType, string callName)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string fileName;

            if (rptType == "10")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportDataByDesigSummaryPdf?callName=" + callName;
            }
            if (rptType == "11")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportDataByDeptSummaryPdf?callName=" + callName;
            }
            if (rptType == "12")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportDataByManpowerSummaryPdf?callName=" + callName;
            }

            string status = myPDF.GeneratePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        [AllowAnonymous]
        public IActionResult HrReportDataBySummaryExcel(string rptType, string callName)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string fileName;

            if (rptType == "10")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportDataByDesigSummaryPdf?callName=" + callName;
            }
            if (rptType == "11")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportDataByDeptSummaryPdf?callName=" + callName;
            }
            if (rptType == "12")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportDataByManpowerSummaryPdf?callName=" + callName;
            }

            string status = myPDF.GeneratePDF(out fileName, url);

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
            //  var stream = new FileStream(rootPath + "/wwwroot/pdf/" + wordpath, FileMode.Open);
            var model = wordpath;
            return Json(model);
        }
        [AllowAnonymous]
        public async Task<ActionResult> HrReportDataByDesigSummaryPdf(string callName)
        {
            AllHrReportViewModel model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                hrSummaryReportViewModels = await reports.GetHrSummaryData(callName)
            };
            return View(model);
        }
        [AllowAnonymous]
        public async Task<ActionResult> HrReportDataByDeptSummaryPdf(string callName)
        {
            AllHrReportViewModel model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                hrSummaryReportViewModels = await reports.GetHrSummaryData(callName)
            };
            return View(model);
        }
        [AllowAnonymous]
        public async Task<ActionResult> HrReportDataByManpowerSummaryPdf(string callName)
        {
            AllHrReportViewModel model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                hrSummaryReportViewModels = await reports.GetHrSummaryData(callName)
            };
            return View(model);
        }
        [AllowAnonymous]
        public async Task<ActionResult> HrReportByDesigPdf(int? desigId, int? deptId, string bloodGroup, int? sbuId)
        {
            AllHrReportViewModel model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                hrReportViewModels = await reports.GetHrDataByDesig(desigId, deptId, bloodGroup, sbuId)

            };
            return View(model);
        }

        
        public async Task<ActionResult> HrReportByDesignationApi(int? desigId, int? deptId, string bloodGroup, int? sbuId)
        {
            var model = new HrReportApiViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                hrReportViewModels = await reports.GetHrDataByDesig(desigId, deptId, bloodGroup, sbuId)

            };
            return Json(model);
        }

        //[AllowAnonymous]
        //public IActionResult HrReportDataByBlood(string desigId, int? deptId, string bloodGroup)
        //{
        //    string scheme = Request.Scheme;
        //    var host = Request.Host;

        //    string url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportDataByBloodPdf?desigId=" + desigId + "&deptId=" + deptId + "&bloodGroup=" + bloodGroup;

        //    string fileName;
        //    string status = myPDF.GenerateLandscapePDF(out fileName, url);

        //    if (status != "done")
        //    {
        //        return Content("<h1>Something Went Wrong</h1>");
        //    }

        //    var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
        //    return new FileStreamResult(stream, "application/pdf");
        //}

        [AllowAnonymous]
        public async Task<ActionResult> HrReportDataByBloodPdf(int? desigId, int? deptId, string bloodGroup, int? SbuId)
        {
            AllHrReportViewModel model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                hrReportViewModels = await reports.GetHrDataByDesig(desigId, deptId, bloodGroup, SbuId)
            };
            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> HrReportDataBySbuPdf(int? desigId, int? deptId, string bloodGroup, int? SbuId)
        {
            AllHrReportViewModel model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                hrReportViewModels = await reports.GetHrDataByDesig(desigId, deptId, bloodGroup, SbuId)
            };
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportAllReligionPage()
        {
            var religions = await organizationService.GetAllReligions();
            var data = new List<EmployeeReligion>();
            foreach (var item in religions)
            {
                data.Add(new EmployeeReligion
                {
                    religion = item,
                    employeeInfos = await reports.GetReligionWiseEmployee(item.Id)
                });
            }
            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeReligions = data
            };
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportByReligionPage(int id)
        {
            AllHrReportViewModel model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeInfos = await reports.GetReligionWiseEmployee(id),
            };
            return View(model);
            //var data = await reports.GetReligionWiseEmployee(id);

            //return View(data);
        }

        
        public async Task<IActionResult> HrReportByReligionApi(int id)
        {
            var model = new HrReportApiViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeInfos = await reports.GetReligionWiseEmployee(id),
            };
            return Json(model);
        }

        public IActionResult HrReportDataPdf(string type, int id)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";

            if (type == "Religion")
            {
                if (id == 0)
                {
                    url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportAllReligionPage";
                }
                else
                {
                    url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportByReligionPage?id=" + id;
                }
            }

            string fileName;
            string status = myPDF.GenerateLandscapePDF(out fileName, url);
            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }



        #endregion

        #region genderPdf
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportAllGender()
        {
            var genders = new List<string>();
            genders.Add("Male");
            genders.Add("Female");
            genders.Add("Other");

            var data = new List<EmployeeGender>();
            foreach (var item in genders)
            {
                data.Add(new EmployeeGender
                {
                    gender = item,
                    employeeInfos = await reports.GetGenderWiseEmployee(item)
                });
            }
            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeGenders = data
            };
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportIndGenderPage(string name)
        {
            var data = await reports.GetGenderWiseEmployee(name);

            //var genders = new List<string>();
            //genders.Add("Male");
            //genders.Add("Female");
            //genders.Add("Other");

            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeGender = new EmployeeGender
                {
                    gender = name,
                    employeeInfos = data
                }
            };
            return View(model);
        }

        public async Task<IActionResult> HrReportIndGenderApi(string name)
        {
            var data = await reports.GetGenderWiseEmployee(name);
            var model = new HrReportApiViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeGender = new EmployeeGender
                {
                    gender = name,
                    employeeInfos = data
                }
            };
            return Json(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportByAwardPage()
        {
            var awards = await reports.GetAllAwardList();
            var data = new List<AwardWiseEmployee>();

            foreach (var item in awards)
            {
                data.Add(new AwardWiseEmployee
                {
                    award = item,
                    employeeInfos = await reports.GetAllEmployeeByAwardId1(item.Id)
                });
            }
            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                awardWiseEmployees = data
            };
            return View(model);
        }



        public IActionResult HrReportGenderPdf(string type, string name)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";

            if (type == "Gender")
            {
                if (name == "All")
                {
                    url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportAllGender";
                }
                else
                {
                    url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportIndGenderPage?name=" + name;
                }
            }

            string fileName;
            string status = myPDF.GenerateLandscapePDF(out fileName, url);
            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        #endregion


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportByJoiningPage(string from, string to)
        {
            ViewBag.to = to;
            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeInfos = await reports.GetEmployeeByJoiningDateRange(Convert.ToDateTime(from), Convert.ToDateTime(to))
            };
            return View(model);
        }

        public async Task<IActionResult> HrReportByJoiningDateApi(string from, string to)
        {
            var model = new HrReportApiViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeInfos = await reports.GetEmployeeByJoiningDateRange(Convert.ToDateTime(from), Convert.ToDateTime(to))
            };
            return Json(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportByAge(int age)
        {
            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeInfos = await eRPCompanyService.GetEmployeesByAge(age)
            };
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportByRole(int? Role)
        {
            var RoleName = await eRPCompanyService.GetCustomRollById((int)Role);
            ViewBag.Role = RoleName.roleName;
            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeInfos = await eRPCompanyService.GetEmployeesByRole(Role)
            };
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportByRoleAndDesig(int? Role, int? Designation)
        {
            var RoleName = await eRPCompanyService.GetCustomRollById((int)Role);
            var deg = await reports.GetDesignationById((int)Designation);
            ViewBag.Role = RoleName.roleName;
            ViewBag.deg = deg.designationName;
            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeInfos = await eRPCompanyService.GetEmployeesByRoleAndDesig(Role, Designation)
            };
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportByAllRole(int? Role)
        {
            var roles = await reports.GetAllRoles();
            var data = new List<RoleWithEmployees>();

            foreach (var item in roles)
            {
                data.Add(new RoleWithEmployees
                {
                    customRole = item,
                    employees = await eRPCompanyService.GetEmployeesByRole(item.Id)
                });
            }

            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                roleWithEmployees = data
            };
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportByAllRoleAndDesig(int? Role, int? Designation)
        {
            var roles = await reports.GetAllRoles();
            var data = new List<RoleWithEmployees>();

            foreach (var item in roles)
            {
                data.Add(new RoleWithEmployees
                {
                    customRole = item,
                    employees = await eRPCompanyService.GetEmployeesByRoleAndDesig(item.Id, Designation)
                });
            }

            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                roleWithEmployees = data
            };
            return View(model);
        }

        public IActionResult HrReportByRolePdf(string type, int? Role, int? Designation)

        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";

            if (type == "Role")
            {
                if (Role == 0)
                {
                    if (Designation == 0)
                    {
                        url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportByAllRole?Role=" + Role;
                    }
                    else
                    {
                        url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportByAllRoleAndDesig?Role=" + Role + "&Designation=" + Designation;
                    };
                }
                else
                {
                    if (Designation == 0)
                    {
                        url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportByRole?Role=" + Role;
                    }
                    else
                    {
                        url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportByRoleAndDesig?Role=" + Role + "&Designation=" + Designation;
                    };
                }
            }

            string fileName;
            string status = myPDF.GeneratePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportByAge1(int age)
        {
            ViewBag.age = age;

            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeInfos = await eRPCompanyService.GetEmployeesByAge(age)
            };
            return View(model);
        }

        public async Task<IActionResult> HrReportByAgeApi(int age)
        {
            var model = new HrReportApiViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeInfos = await eRPCompanyService.GetEmployeesByAge(age)
            };
            return Json(model);
        }

        public IActionResult HrReportByAge1Pdf(string type, int age)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";

            if (type == "Age1")
            {

                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportByAge1?age=" + age;

            }

            string fileName;
            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportByConfirmationPage(string from, string to)
        {
            ViewBag.to = to;
            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeInfos = await reports.GetEmployeeByConfirmationDateRange(Convert.ToDateTime(from), Convert.ToDateTime(to))
            };
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportByRetirementPage(string from, string to)
        {
            ViewBag.to = to;
            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeInfos = await reports.GetEmployeeByRetirementDateRange(Convert.ToDateTime(from), Convert.ToDateTime(to))
            };
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportByResignationPage(string from, string to)
        {
            ViewBag.to = to;
            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeInfos = await reports.GetEmployeeByResignationDateRange(Convert.ToDateTime(from), Convert.ToDateTime(to))
            };
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportBySuspensionPage(string from, string to)
        {
            ViewBag.to = to;
            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeInfos1 = await reports.GetEmployeeBySuspensionDateRange(Convert.ToDateTime(from), Convert.ToDateTime(to))
            };
            return View(model);
        }

        public IActionResult HrReportJoiningDatePdf(string type, string from, string to)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";

            if (type == "JoiningDate")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportByJoiningPage?from=" + from + "&to=" + to;
            }
            if (type == "ConfirmationDate")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportByConfirmationPage?from=" + from + "&to=" + to;
            }
            if (type == "Retirement")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportByRetirementPage?from=" + from + "&to=" + to;
            }
            if (type == "Resignation")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportByResignationPage?from=" + from + "&to=" + to;
            }
            if (type == "Suspension")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportBySuspensionPage?from=" + from + "&to=" + to;
            }
            if (type == "seniorityListJoiningDate")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportBySeniorityWise?joiningDate=" + from;
            }

            string fileName;
            string status = myPDF.GeneratePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportByAllDiplomaPage(string part)
        {
            //var data = await reports.GetAllEmployeesByBoth();
            var data = await reports.GetAllEmployeesByDiploma(part);
            //var newdata = new List<EmpDiplomaViewModel>();

            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeDiplomas = data
            };

            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportByDiplomaPage(string part)
        {
            var data = await reports.GetAllEmployeesByDiploma(part);
            //var newdata = new List<EmpDiplomaViewModel>();

            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeDiplomas = data
            };

            return View(model);
        }

        public IActionResult HrReportBankingDiplomaPdf(string type, string part)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";

            if (type == "Diploma")
            {
                if (part == "0")
                {
                    url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportByAllDiplomaPage?part=" + part;
                }
                else
                {
                    url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportByDiplomaPage?part=" + part;
                }
            }

            string fileName;
            string status = myPDF.GeneratePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }



        public IActionResult HrReportAwardDatePdf(string type)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";

            if (type == "Award")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportByAwardPage";
            }
            //if (type == "Retirement")
            //{
            //    url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportByRetirementPage";
            //}
            //if (type == "Resignation")
            //{
            //    url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportByResignationPage";
            //}
            //if (type == "Suspension")
            //{
            //    url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportBySuspensionPage";
            //}

            string fileName;
            string status = myPDF.GeneratePDF(out fileName, url);
            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }



        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportAllEmployeeByDivision()
        {
            var divisions = await organizationService.GetAllDivisions();
            var data = new List<EmployeeDivision>();
            foreach (var item in divisions)
            {
                data.Add(new EmployeeDivision
                {
                    division = item,
                    employeeInfos = await reports.GetDivisionWiseEmployee(item.Id)
                });
            }
            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeDivisions = data
            };
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportIndEmpByDivision(int id)
        {
            AllHrReportViewModel model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeInfos = await reports.GetDivisionWiseEmployee(id),
            };
            return View(model);
            //var data = await reports.GetReligionWiseEmployee(id);

            //return View(data);
        }




        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportAllEmployeeByDistrict()
        {
            var districts = await organizationService.GetAllDistrict();
            var data = new List<EmployeeDistrict>();
            foreach (var item in districts)
            {
                data.Add(new EmployeeDistrict
                {
                    district = item,
                    employeeInfos = await reports.GetDistrictWiseEmployee(item.Id)
                });
            }
            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeDistricts = data
            };
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportIndEmpByDistrict(int id)
        {
            AllHrReportViewModel model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeInfos = await reports.GetDistrictWiseEmployee(id),
            };
            return View(model);
            //var data = await reports.GetReligionWiseEmployee(id);

            //return View(data);
        }

        public async Task<IActionResult> HrReportEmployeesByDistrictApi(int id)
        {
            var model = new HrReportApiViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeInfos = await reports.GetDistrictWiseEmployee(id),
            };
            return Json(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportAllEmployeeByBranch()
        {
            var branches = await organizationService.GetAllBranch();
            var data = new List<EmployeeBranch>();
            foreach (var item in branches)
            {
                data.Add(new EmployeeBranch
                {
                    branch = item,
                    employeeInfos = await reports.GetBranchWiseEmployee(item.Id)
                });
            }
            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeBranches = data
            };
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportIndEmpBybranch(int id)
        {
            var branch = await reports.GetLocationById(id);
            ViewBag.branchName = branch.branchUnitName;
            AllHrReportViewModel model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeInfos = await reports.GetBranchWiseEmployee(id),


            };
            return View(model);
            //var data = await reports.GetReligionWiseEmployee(id);

            //return View(data);
        }

        public async Task<IActionResult> HrReportEmployeesBybranchApi(int id)
        {
            var model = new HrReportApiViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeInfos = await reports.GetBranchWiseEmployee(id)
            };
            return Json(model);
        }





        public IActionResult HrReportDivisionDistrictBranchPdf(string type, int id)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";

            if (type == "Division")
            {
                if (id == 0)
                {
                    url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportAllEmployeeByDivision";
                }
                else
                {
                    url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportIndEmpByDivision?id=" + id;
                }
            }
            if (type == "District")
            {
                if (id == 0)
                {
                    url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportAllEmployeeByDistrict";
                }
                else
                {
                    url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportIndEmpByDistrict?id=" + id;
                }
            }
            if (type == "Branch")
            {
                if (id == 0)
                {
                    url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportAllEmployeeByBranch";
                }
                else
                {
                    url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportIndEmpBybranch?id=" + id;
                }
            }

            string fileName;
            string status = myPDF.GeneratePDF(out fileName, url);
            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportBySeniorityWise(DateTime joiningDate)
        {
            ViewBag.date = joiningDate;
            var model = new AllHrReportViewModel
            {
				designations = await personalInfoService.GetAllDesignation(),
				seniorityListViewModelNews = await reports.GetAllByJoiningDateSeniorityList(joiningDate),
				//seniorityWiseDesig = await reports.GetSeniorityDesigByJoiningDate(joiningDate),
				companies = await eRPCompanyService.GetAllCompany(),
            };
            return View(model);
            //  return Json(model);
        }


        public IActionResult HrReportBySeniorityWisePDF(string type, DateTime joiningDate)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";

            if (type == "SeniorityList")
            {

                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportBySeniorityWise?joiningDate=" + joiningDate;

            }

            string fileName;
            string status = myPDF.GenerateLandscapePDFCustom2(out fileName, url, "SeniorityList_");

            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportBylastPromotionDate(DateTime lastPromotionDate)
        {
            ViewBag.date = lastPromotionDate;
            var model = new AllHrReportViewModel
            {
                seniorityWiseDesig = await reports.GetSeniorityDesigBylastPromotionDate(lastPromotionDate),
                companies = await eRPCompanyService.GetAllCompany(),
            };
            return View(model);
            // return Json(model);
        }
        public IActionResult HrReportBylastPromotionDatePDF(string type, DateTime lastPromotionDate)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";

            if (type == "lastPromotionDate")
            {

                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportBylastPromotionDate?lastPromotionDate=" + lastPromotionDate;

            }

            string fileName;
            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportBySeniorityWisePRL(DateTime prlDate)
        {
            ViewBag.prlDate = prlDate;
            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeInfos = await reports.GetSeniorityByPRLDate(prlDate)
            };
            return View(model);
        }
        public IActionResult HrReportBySeniorityWisePRLPDF(string type, DateTime prlDate)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";

            if (type == "SeniorityPRL")
            {

                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportBySeniorityWisePRL?prlDate=" + prlDate;

            }


            string fileName;
            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        public IActionResult HrReportBySeniorityPDF(string type, DateTime transferDate, int? years)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";


            if (type == "3Years")
            {

                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportBySeniority3YAndAbove?transferDate=" + transferDate + "&years=" + years;

            }

            string fileName;
            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> HrReportBySeniorityEQ(DateTime joiningDate, string qualification)
        //{
        //    ViewBag.date = joiningDate;
        //    var model = new AllHrReportViewModel
        //    {
        //        companies = await eRPCompanyService.GetAllCompany(),
        //        employeeInfos = await reports.GetSeniorityByEqAndDate(joiningDate, qualification)
        //    };
        //     return View(model);
        // //   return Json(model);
        //}
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportBySeniorityEQ(DateTime joiningDate, int qualification)
        {
            ViewBag.date = joiningDate;
            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                educationalQualifications = await reports.GetSeniorityByEqAndDate1(joiningDate, qualification)
            };
            return View(model);
            // return Json(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportBySeniority3YAndAbove(DateTime transferDate, int years)
        {
            ViewBag.transferDate = transferDate;
            ViewBag.years = years;
            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                EmployeeTransferYears = await reports.GetSeniorityBy3YAndAbove(transferDate, years)
            };
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrReportBranchWise(DateTime joiningDate, int Id)
        {
            var branch = await reports.GetBranchNameById(Id);
			if (branch == null)
			{
				ViewBag.branchName = "";
			}
			else
			{
				ViewBag.branchName = branch.branchName;
			}
			ViewBag.joiningDate = joiningDate;
            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeInfos = await reports.GetEmployeeByBranch(joiningDate, Id)
            };
            return View(model);
            //return Json(model);
        }
        public IActionResult HrReportBranchWisePDF(string type, DateTime joiningDate, int? Id)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";


            if (type == "BranchWise")
            {

                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportBranchWise?joiningDate=" + joiningDate + "&Id=" + Id;

            }

            string fileName;
            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> DivisionWiseEmpReportByPostingDate(DateTime postingdate, int divId)
        {
            var division = await reports.GetDivisionNameById(divId);
            ViewBag.divName = division.divName;
            ViewBag.postingdate = postingdate;

            var emp = await reports.GetEmployeesDivisionByPostingDate(postingdate, divId);
            var p = new List<EmployeeWithPostingPlacesViewModel>();
            foreach (var item in emp)
            {
                p.Add(new EmployeeWithPostingPlacesViewModel
                {
                    employee = item,
                    designation = item.lastDesignation?.designationName,
                    //designation = await reports.GetDeisignationNameById((int)item.lastDesignationId),
                    postings = await reports.GetPostingsByEmployeeIdAndDate(item.Id, postingdate),
                    age = CalculateYearsByDays((Convert.ToDateTime(postingdate).Date - Convert.ToDateTime(item.dateOfBirth).Date).Days)
                });
            }
            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeWithPostingPlaces = p
            };
            return View(model);
        }

        public IActionResult HrReportDivisionWisePDF(string type, DateTime postingdate, int? divId)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";


            if (type == "DivisionWise")
            {
                //url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/DivisionWiseEmpReportByPostingDate?postingdate=" + postingdate + "&divId=" + divId;

                if (divId != 2000)
                {
                    url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/DivisionWiseEmpReportByPostingDate?postingdate=" + postingdate + "&divId=" + divId;

                }
                else
                {
                    url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/AllDivisionWiseEmpReportByPostingDate?postingdate=" + postingdate;

                }

            }

            string fileName;
            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetDepartmentWiseEmpReportByPostingDate(DateTime postingdate, int deptId)
        {
            ViewBag.Date = postingdate;
            var emp = await reports.GetEmployeesByPostingDate(postingdate, deptId);
            //  var deg = await reports.GetDegNameById();
            var dep = await reports.GetDepNameById(deptId);
            ViewBag.dep = dep.deptName;
            var p = new List<EmployeeWithPostingPlacesViewModel>();
            foreach (var item in emp)
            {
                p.Add(new EmployeeWithPostingPlacesViewModel
                {
                    designation = await reports.GetDeisignationNameById((int)item.lastDesignationId),
                    employee = item,
                    postings = await reports.GetPostingsByEmployeeIdAndDate(item.Id, postingdate),
                    age = CalculateYearsByDays((Convert.ToDateTime(postingdate).Date - Convert.ToDateTime(item.dateOfBirth).Date).Days)
                });
            }
            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeWithPostingPlaces = p
            };
            //  return Json(model);
            return View(model);
        }

        public async Task<IActionResult> GetDepartmentWiseEmpReportByPostingDateApi(DateTime postingdate, int deptId)
        {
            var emp = await reports.GetEmployeesByPostingDate(postingdate, deptId);
            var dep = await reports.GetDepNameById(deptId);
            var p = new List<EmployeeWithPostingPlacesViewModel>();
            foreach (var item in emp)
            {
                p.Add(new EmployeeWithPostingPlacesViewModel
                {
                    designation = await reports.GetDeisignationNameById((int)item.lastDesignationId),
                    employee = item,
                    postings = await reports.GetPostingsByEmployeeIdAndDate(item.Id, postingdate),
                    age = CalculateYearsByDays((Convert.ToDateTime(postingdate).Date - Convert.ToDateTime(item.dateOfBirth).Date).Days)
                });
            }
            var model = new HrReportApiViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeWithPostingPlaces = p
            };
            return Json(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> AllDepartmentWiseEmpReportByPostingDate(DateTime postingdate)
        {

            var department = await organizationService.GetAllDepartments();
            ViewBag.postingdate = postingdate;


            var p = new List<AllDivisionEmpViewModel>();
            foreach (var item in department)
            {
                var empDiv1 = new List<EmployeeWithPostingPlacesViewModel>();
                var employees = await reports.GetEmployeesByPostingDate(postingdate, item.Id);
                foreach (var emp in employees)
                {
                    empDiv1.Add(new EmployeeWithPostingPlacesViewModel
                    {
                        employee = emp,
                        designation = await reports.GetDeisignationNameById((int)emp.lastDesignationId),
                        postings = await reports.GetPostingsByEmployeeIdAndDate(emp.Id, postingdate),
                        age = CalculateYearsByDays((Convert.ToDateTime(postingdate).Date - Convert.ToDateTime(emp.dateOfBirth).Date).Days)
                    });

                }

                p.Add(new AllDivisionEmpViewModel
                {
                    department = item,
                    divEmpListVM = empDiv1

                });
            }
            var model = new AllHrReportViewModel
            {

                companies = await eRPCompanyService.GetAllCompany(),
                allDivisionEmpViewModels = p
            };
            return View(model);

            //  return Json(model);
        }

        [Route("global/api/GetDegNameById/{Id}")]
        [HttpGet]
        public async Task<IActionResult> GetDegNameById(int Id)
        {
            return Json(await reports.GetDegNameById(Id));
        }


        public string CalculateYearsByDays(int days)
        {
            int y = days / 365;
            int m = (days - (y * 365)) / 30;

            return y + "Y-" + m + "M";
        }

        public IActionResult HrReportDepartmentWisePDF(string type, DateTime postingdate, int? deptId)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";


            if (type == "DepartmenthWise")
            {

                if (deptId != 5000)
                {
                    url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/GetDepartmentWiseEmpReportByPostingDate?postingdate=" + postingdate + "&deptId=" + deptId;

                }
                else
                {
                    url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/AllDepartmentWiseEmpReportByPostingDate?postingdate=" + postingdate;

                }

            }

            string fileName;
            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrBranchWiseEmpReportByPostingDate(DateTime postingdate, int branchId)
        {
            var branch = await reports.GetBranchNameById(branchId);
            ViewBag.branchName = branch.branchName;
            ViewBag.postingdate = postingdate;

            var emp = await reports.GetEmployeesBranchByPostingDate(postingdate, branchId);
            var p = new List<EmployeeWithPostingPlacesViewModel>();
            foreach (var item in emp)
            {
                p.Add(new EmployeeWithPostingPlacesViewModel
                {
                    designation = await reports.GetDeisignationNameById((int)item.lastDesignationId),
                    employee = item,
                    postings = await reports.GetPostingsByEmployeeIdAndDate(item.Id, postingdate),
                    age = CalculateYearsByDays((Convert.ToDateTime(postingdate).Date - Convert.ToDateTime(item.dateOfBirth).Date).Days)
                });
            }
            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeWithPostingPlaces = p
            };
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> AllDepWiseEmpReportByPostingDate(DateTime postingdate)
        {

            var Department = await organizationService.GetAllDepartments();
            ViewBag.postingdate = postingdate;


            var p = new List<AllDivisionEmpViewModel>();
            foreach (var item in Department)
            {
                var empDiv1 = new List<EmployeeWithPostingPlacesViewModel>();
                var employees = await reports.GetEmployeesDepartmentByPostingDate(postingdate, item.Id);
                foreach (var emp in employees)
                {
                

                    empDiv1.Add(new EmployeeWithPostingPlacesViewModel
                    {
                        employee = emp,
                        designation = await reports.GetDeisignationShortNameById((int)emp.lastDesignationId),
                        DegWiseEmpList = await reports.GetDegWiseEmpLisr((int)emp.lastDesignationId, (int)emp.departmentId),
                        count = await reports.GetDegWiseEmpLisrCount((int)emp.lastDesignationId, (int)emp.departmentId),

                });

                }

                p.Add(new AllDivisionEmpViewModel
                {
                    department = item,
                    divEmpListVM = empDiv1,
                   


                   

                });
            }
            var model = new AllHrReportViewModel
            {

                companies = await eRPCompanyService.GetAllCompany(),
                allDivisionEmpViewModels = p
            };
            return View(model);

            // return Json(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> OfficeWiseEmpReportByPostingDate(DateTime postingdate)
        {

            var Office = await organizationService.GetAllOffices();
            ViewBag.postingdate = postingdate;


            var p = new List<AllDivisionEmpViewModel>();
            foreach (var item in Office)
            {
                var empDiv1 = new List<EmployeeWithPostingPlacesViewModel>();
                var employees = await reports.GetEmployeesOfficeByPostingDateNew(postingdate, item.Id);
                foreach (var emp in employees)
                {
                

                    empDiv1.Add(new EmployeeWithPostingPlacesViewModel
                    {
                        employee = emp,
                        designation = await reports.GetDeisignationShortNameById((int)emp.lastDesignationId),
                        DegWiseEmpList = await reports.GetOfficeWiseEmpLisr((int)emp.lastDesignationId, (int)emp.functionInfoId),
                        

                });

                }

                p.Add(new AllDivisionEmpViewModel
                {
                    office = item,
                    divEmpListVM = empDiv1,
                   


                   

                });
            }
            var model = new AllHrReportViewModel
            {

                companies = await eRPCompanyService.GetAllCompany(),
                allDivisionEmpViewModels = p
            };
            return View(model);

            // return Json(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> DivWiseEmpReportByPostingDate(DateTime postingdate)
        {

            var Division = await organizationService.GetAllHrDivision();
            ViewBag.postingdate = postingdate;


            var p = new List<AllDivisionEmpViewModel>();
            foreach (var item in Division)
            {
                var empDiv1 = new List<EmployeeWithPostingPlacesViewModel>();
                var employees = await reports.GetEmployeesDivisionByPostingDateNew(postingdate, item.Id);
                foreach (var emp in employees)
                {
                

                    empDiv1.Add(new EmployeeWithPostingPlacesViewModel
                    {
                        employee = emp,
                        designation = await reports.GetDeisignationShortNameById((int)emp.lastDesignationId),
                        DegWiseEmpList = await reports.GetDivWiseEmpLisr((int)emp.lastDesignationId, (int)emp.hrDivisionId),
                       

                });

                }

                p.Add(new AllDivisionEmpViewModel
                {
                    hrDivision = item,
                    divEmpListVM = empDiv1,
                   


                   

                });
            }
            var model = new AllHrReportViewModel
            {

                companies = await eRPCompanyService.GetAllCompany(),
                allDivisionEmpViewModels = p
            };
            return View(model);

            // return Json(model);
        }

        [AllowAnonymous]
        public IActionResult AllDepWiseEmpReportByPostingDatePdf(string type, DateTime postingdate)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";

            if (type == "AllDepWise")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/AllDepWiseEmpReportByPostingDate?postingdate=" + postingdate;
            }


            string fileName;
            string status = myPDF.GeneratePDF1(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        [AllowAnonymous]
        public IActionResult OfficeWiseEmpReportByPostingDatePdf(string type, DateTime postingdate)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";

            if (type == "OfficeWise")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/OfficeWiseEmpReportByPostingDate?postingdate=" + postingdate;
            }


            string fileName;
            string status = myPDF.GeneratePDF1(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        [AllowAnonymous]
        public IActionResult DivWiseEmpReportByPostingDatePdf(string type, DateTime postingdate)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";

            if (type == "DivWise")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/DivWiseEmpReportByPostingDate?postingdate=" + postingdate;
            }


            string fileName;
            string status = myPDF.GeneratePDF1(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }




        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> AllHrBranchWiseEmpReportByPostingDate(DateTime postingdate)
        {

            var HrBranch = await organizationService.GetAllHrBranch();
            ViewBag.postingdate = postingdate;


            var p = new List<AllDivisionEmpViewModel>();
            foreach (var item in HrBranch)
            {
                var empDiv1 = new List<EmployeeWithPostingPlacesViewModel>();
                var employees = await reports.GetEmployeesBranchByPostingDate(postingdate, item.Id);
                foreach (var emp in employees)
                {
                    empDiv1.Add(new EmployeeWithPostingPlacesViewModel
                    {
                        employee = emp,
                        designation = await reports.GetDeisignationNameById((int)emp.lastDesignationId),
                        postings = await reports.GetPostingsByEmployeeIdAndDate(emp.Id, postingdate),
                        age = CalculateYearsByDays((Convert.ToDateTime(postingdate).Date - Convert.ToDateTime(emp.dateOfBirth).Date).Days)
                    });

                }

                p.Add(new AllDivisionEmpViewModel
                {
                    hrBranch = item,
                    divEmpListVM = empDiv1

                });
            }
            var model = new AllHrReportViewModel
            {

                companies = await eRPCompanyService.GetAllCompany(),
                allDivisionEmpViewModels = p
            };
            return View(model);

            //  return Json(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> HrOfficeWiseEmpReportByPostingDate(DateTime postingdate, int officeId)
        {
            var office = await reports.GetOfficeNameById(officeId);
            ViewBag.branchUnitName = office.branchUnitName;
            ViewBag.postingdate = postingdate;

            var emp = await reports.GetEmployeesOfficeByPostingDate(postingdate, officeId);
            var p = new List<EmployeeWithPostingPlacesViewModel>();
            foreach (var item in emp)
            {
                p.Add(new EmployeeWithPostingPlacesViewModel
                {
                    designation = await reports.GetDeisignationNameById((int)item.lastDesignationId),
                    employee = item,
                    postings = await reports.GetPostingsByEmployeeIdAndDate(item.Id, postingdate),
                    age = CalculateYearsByDays((Convert.ToDateTime(postingdate).Date - Convert.ToDateTime(item.dateOfBirth).Date).Days)
                });
            }
            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeWithPostingPlaces = p
            };
            return View(model);
        }
        public IActionResult HrBranchWiseEmpReportByPostingDatePDF(string type, DateTime postingdate, int? branchId)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";


            if (type == "HrBranchWise")
            {

                //url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrBranchWiseEmpReportByPostingDate?postingdate=" + postingdate + "&branchId=" + branchId;
                if (branchId != 5000)
                {
                    url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrBranchWiseEmpReportByPostingDate?postingdate=" + postingdate + "&branchId=" + branchId;

                }
                else
                {
                    url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/AllHrBranchWiseEmpReportByPostingDate?postingdate=" + postingdate;

                }
            }

            string fileName;
            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }



        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> AllOfficeWiseEmpReportByPostingDate(DateTime postingdate)
        {

            var office1 = await organizationService.GetAllOffices();
            ViewBag.postingdate = postingdate;


            var p = new List<AllDivisionEmpViewModel>();
            foreach (var item in office1)
            {
                var empDiv1 = new List<EmployeeWithPostingPlacesViewModel>();
                var employees = await reports.GetEmployeesOfficeByPostingDate(postingdate, item.Id);
                foreach (var emp in employees)
                {
                    empDiv1.Add(new EmployeeWithPostingPlacesViewModel
                    {
                        employee = emp,
                        designation = await reports.GetDeisignationNameById((int)emp.lastDesignationId),
                        postings = await reports.GetPostingsByEmployeeIdAndDate(emp.Id, postingdate),
                        age = CalculateYearsByDays((Convert.ToDateTime(postingdate).Date - Convert.ToDateTime(emp.dateOfBirth).Date).Days)
                    });

                }

                p.Add(new AllDivisionEmpViewModel
                {
                    office = item,
                    divEmpListVM = empDiv1

                });
            }
            var model = new AllHrReportViewModel
            {

                companies = await eRPCompanyService.GetAllCompany(),
                allDivisionEmpViewModels = p
            };
            return View(model);
        }
        public IActionResult HrOfficeWiseEmpReportByPostingDatePDF(string type, DateTime postingdate, int? officeId)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";


            if (type == "HrOfficeWise")
            {

                //url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrOfficeWiseEmpReportByPostingDate?postingdate=" + postingdate + "&officeId=" + officeId;
                if (officeId != 5000)
                {
                    url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrOfficeWiseEmpReportByPostingDate?postingdate=" + postingdate + "&officeId=" + officeId;

                }
                else
                {
                    url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/AllOfficeWiseEmpReportByPostingDate?postingdate=" + postingdate;

                }
            }

            string fileName;
            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }



        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> AllDivisionWiseEmpReportByPostingDate(DateTime postingdate)
        {

            var division = await organizationService.GetAllHrDivision();
            ViewBag.postingdate = postingdate;


            var p = new List<AllDivisionEmpViewModel>();
            foreach (var item in division)
            {
                var empDiv1 = new List<EmployeeWithPostingPlacesViewModel>();
                var employees = await reports.GetEmployeesDivisionByPostingDate(postingdate, item.Id);
                foreach (var emp in employees)
                {
                    empDiv1.Add(new EmployeeWithPostingPlacesViewModel
                    {
                        employee = emp,
                        designation = await reports.GetDeisignationNameById((int)emp.lastDesignationId),
                        postings = await reports.GetPostingsByEmployeeIdAndDate(emp.Id, postingdate),
                        age = CalculateYearsByDays((Convert.ToDateTime(postingdate).Date - Convert.ToDateTime(emp.dateOfBirth).Date).Days)
                    });

                }

                p.Add(new AllDivisionEmpViewModel
                {
                    hrDivision = item,
                    divEmpListVM = empDiv1

                });
            }
            var model = new AllHrReportViewModel
            {

                companies = await eRPCompanyService.GetAllCompany(),
                allDivisionEmpViewModels = p
            };
            return View(model);
        }













        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> BranchWorkingExplanation(DateTime postingdate)
        {

            var HrBranch = await organizationService.GetAllHrBranch();
            ViewBag.postingdate = postingdate;


            var p = new List<AllDivisionEmpViewModel>();
            foreach (var item in HrBranch)
            {
                var empDiv1 = new List<EmployeeWithPostingPlacesViewModel>();
                var employees = await reports.GetEmployeesBranchByPostingDate(postingdate, item.Id);
                foreach (var emp in employees)
                {
                    empDiv1.Add(new EmployeeWithPostingPlacesViewModel
                    {
                        employee = emp,
                        designation = await reports.GetDeisignationNameById1((int)emp.lastDesignationId),
                        branch = await reports.GetHrBranchNameById1((int)emp.hrBranchId),
                        postings = await reports.GetPostingsByEmployeeIdAndDate1(emp.Id, postingdate),
                        // duration = endDate != null ? CalculateYearsByDays((Convert.ToDateTime(item.endDate).Date - Convert.ToDateTime(item.startDate).Date).Days) : CalculateYearsByDays((Convert.ToDateTime(date).Date - Convert.ToDateTime(item.startDate).Date).Days)
                    });

                }

                p.Add(new AllDivisionEmpViewModel
                {
                    hrBranch = item,
                    divEmpListVM = empDiv1

                });
            }
            var model = new AllHrReportViewModel
            {

                companies = await eRPCompanyService.GetAllCompany(),
                allDivisionEmpViewModels = p
            };
            return View(model);
        }
        public IActionResult BranchWorkingExplanationPDF(string type, DateTime postingdate)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";


            if (type == "BranchExp")
            {

                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/BranchWorkingExplanation?postingdate=" + postingdate;

            }

            string fileName;
            string status = myPDF.GeneratePDF1(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }



        [AllowAnonymous]
        public async Task<IActionResult> OfficewisePositionByPostDateView(DateTime? date)
        {
            ViewBag.date = date;
            var officeBranches = new OfficeBranch
            {
                office = await organizationService.GetAllOffices(),
                hrBranches = await organizationService.GetAllHrBranch()
            };

            var model = new List<OfficeBranchPosition>();

            foreach (var item in officeBranches.office)
            {
                model.Add(new OfficeBranchPosition
                {
                    officeName = item.branchUnitName,
                    Officers = await reports.GetOfficerCountByOfficeId(item.Id, Convert.ToDateTime(date)),
                    Staffs = await reports.GetStaffCountByOfficeId(item.Id, Convert.ToDateTime(date)),
                    Total = await reports.GetTotalOfficerAndStaffCountByOfficeId(item.Id, Convert.ToDateTime(date))
                });
            }
            foreach (var item in officeBranches.hrBranches)
            {
                model.Add(new OfficeBranchPosition
                {
                    officeName = item.branchName,
                    Officers = await reports.GetOfficerCountByBranchId(item.Id, Convert.ToDateTime(date)),
                    Staffs = await reports.GetStaffCountByBranchId(item.Id, Convert.ToDateTime(date)),
                    Total = await reports.GetTotalOfficerAndStaffCountByBranchId(item.Id, Convert.ToDateTime(date))
                });
            }

            var model1 = new AllHrReportViewModel
            {

                companies = await eRPCompanyService.GetAllCompany(),
                officeBranchPositions = model
            };

            return View(model1);
        }

        [AllowAnonymous]
        public IActionResult OfficewisePositionByPostDatePdf(string type, DateTime postingdate)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";

            if (type == "OfficeWisePosting")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/OfficewisePositionByPostDateView?date=" + postingdate;
            }


            string fileName;
            string status = myPDF.GeneratePDF1(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [AllowAnonymous]
        public async Task<ActionResult> HrReportManpowerByAgeGroup(DateTime toDate)
        {
            var month6 = toDate.AddMonths(-6);
            var month12 = toDate.AddMonths(-12);
            ViewBag.month = toDate;
            ViewBag.months = month6;
            ViewBag.monthTw = month12;
            AllHrReportViewModel model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                hrSummaryReportViewModels = await reports.GetHrSummaryData1(toDate.ToString()),
                hrSummaryReportViewModels6 = await reports.GetHrSummaryData1(month6.ToString()),
                hrSummaryReportViewModels12 = await reports.GetHrSummaryData1(month12.ToString()),
            };
            return View(model);
        }
        [AllowAnonymous]
        public async Task<IActionResult> GetAllBranchesApi()
        {
            var data = await hrBranchService.GetAllHrBranches();
            return Json(data);
        }
        [AllowAnonymous]
        public async Task<IActionResult> GetAllDesignationsApi()
        {
            var data = await hrBranchService.GetAllDesignation();
            return Json(data);
        }
        [AllowAnonymous]
        public async Task<IActionResult> GetAllDepartmentApi()
        {
            var data = await hrBranchService.GetAllDepartment();
            return Json(data);
        }
        public async Task<IActionResult> GetAllEmployeesByTypeId(string type, int id)
        {
            var data = await organizationService.GetAllEmployeesByTypeId(type, id);
            return Json(data);
        }
        [AllowAnonymous]
        public async Task<IActionResult> GetAllEmployeesByBranchAndDesignationId(int? branchId, int? departmentId, int? designationId)
        {
            var data = await organizationService.GetAllEmployeesByBranchAndDesignationId(Convert.ToInt32(branchId), Convert.ToInt32(departmentId), Convert.ToInt32(designationId));
            return Json(data);
        }
        [AllowAnonymous]
        public async Task<IActionResult> GetEmployeesByAnyKey(int? divisionId, int? unitId, int? officeId, int? zoneId, int? branchId, int? departmentId, int? designationId)
        {
            var data = await organizationService.GetEmployeesByAnyKey(divisionId, unitId, officeId, zoneId, branchId, departmentId, designationId);
            return Json(data);
        }

        public async Task<ActionResult> HrReportManpowerByAgeGroupApi(DateTime toDate)
        {
            var month6 = toDate.AddMonths(-6);
            var month12 = toDate.AddMonths(-12);
            var model = new HrReportApiViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                hrSummaryReportViewModels = await reports.GetHrSummaryData1(toDate.ToString()),
                hrSummaryReportViewModels6 = await reports.GetHrSummaryData1(month6.ToString()),
                hrSummaryReportViewModels12 = await reports.GetHrSummaryData1(month12.ToString()),
            };
            return Json(model);
        }

        [AllowAnonymous]
        public IActionResult HrReportManpowerByAgeGroupPDF(string type, DateTime toDate)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";

            if (type == "MPAge")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportManpowerByAgeGroup?toDate=" + toDate;
            }


            string fileName;
            string status = myPDF.GeneratePDF1(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [AllowAnonymous]
        public async Task<IActionResult> HrReportManpowerByGender(DateTime toDates)
        {
            var todate = toDates;
            var date6 = toDates.AddMonths(-6);
            var date12 = toDates.AddMonths(-12);
            ViewBag.month = toDates;
            ViewBag.months = date6;
            ViewBag.monthTw = date12;
            AllHrReportViewModel model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                genderManpowerViewModels = await reports.GetHrGenderSummaryData1(todate.ToString()),
                genderManpowerViewModels6 = await reports.GetHrGenderSummaryData1(date6.ToString()),
                genderManpowerViewModels12 = await reports.GetHrGenderSummaryData1(date12.ToString()),
            };
            return View(model);

        }
        [AllowAnonymous]
        public IActionResult HrReportManpowerByGenderPDF(string type, DateTime toDates)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";

            if (type == "GD")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportManpowerByGender?toDates=" + toDates;
            }


            string fileName;
            string status = myPDF.GeneratePDF1(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        [AllowAnonymous]
        public async Task<IActionResult> CSRAgeReport(DateTime birthDate)
        {
            var allEmployees = await reports.GetAllEmployees();
            var data = new RoleWiseAgeViewModel
            {
                entryMale = allEmployees?.Where(x => x.lastDesignation?.customRoleId == 3 && x.gender == "Male").Count(),
                entryFemale=allEmployees?.Where(x=>x.lastDesignation?.customRoleId==3 && x.gender== "Female").Count(),
                midMale=allEmployees?.Where(x=>x.lastDesignation?.customRoleId==2 && x.gender =="Male").Count(),
                midFemale = allEmployees?.Where(x => x.lastDesignation?.customRoleId == 2 && x.gender == "Female").Count(),
                seniorMale=allEmployees?.Where(x=>x.lastDesignation?.customRoleId==1 && x.gender =="Male").Count(),
                seniorFemale=allEmployees?.Where(x=>x.lastDesignation?.customRoleId==1 && x.gender=="Female").Count(),

                less30Male=allEmployees?.Where(x=>x.gender=="Male" && (new DateTime(1,1,1) + (Convert.ToDateTime(birthDate)-Convert.ToDateTime(x.dateOfBirth))).Year <= 30).Count(),
                less30Female=allEmployees?.Where(x=>x.gender=="Female" && (new DateTime(1,1,1) + (Convert.ToDateTime(birthDate)-Convert.ToDateTime(x.dateOfBirth))).Year <= 30).Count(),

                thirtyTofiftyMale=allEmployees?.Where(x=>x.gender=="Male" && (new DateTime(1,1,1) + (Convert.ToDateTime(birthDate)-Convert.ToDateTime(x.dateOfBirth))).Year >= 30 && (new DateTime(1, 1, 1) + (Convert.ToDateTime(birthDate) - Convert.ToDateTime(x.dateOfBirth))).Year <= 50).Count(),
                thirtyTofiftyFemale=allEmployees?.Where(x=>x.gender=="Female" && (new DateTime(1, 1, 1) + (Convert.ToDateTime(birthDate) - Convert.ToDateTime(x.dateOfBirth))).Year >= 30 && (new DateTime(1, 1, 1) + (Convert.ToDateTime(birthDate) - Convert.ToDateTime(x.dateOfBirth))).Year <= 50).Count(),


                upto50Male =allEmployees?.Where(x=>x.gender=="Male" && (new DateTime(1,1,1) + (Convert.ToDateTime(birthDate)-Convert.ToDateTime(x.dateOfBirth))).Year >= 50).Count(),
                upto50Female=allEmployees?.Where(x=>x.gender=="Female" && (new DateTime(1,1,1) + (Convert.ToDateTime(birthDate)-Convert.ToDateTime(x.dateOfBirth))).Year >= 50).Count(),



            };
            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                roleWiseAge = data
            };
            return View(model);
        }


        [AllowAnonymous]
        public IActionResult CSRAgeReportPDF(string type, DateTime birthDate)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";

            if (type == "CSRAge")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/CSRAgeReport?birthDate=" + birthDate;
            }


            string fileName;
            string status = myPDF.GeneratePDF1(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        [AllowAnonymous]
        public async Task<IActionResult> VacancySummaryReport(DateTime? toDates)
        {
            ViewBag.date = toDates;

            AllHrReportViewModel model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                vacancies = await travelService.GetVacancy()
            };
            return View(model);
        
        }
        [AllowAnonymous]
        public IActionResult HrReportVacancyPDF(string type, DateTime toDates)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";

            if (type == "Vac")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/VacancySummaryReport?toDates=" + toDates;
            }


            string fileName;
            string status = myPDF.GeneratePDF1(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [AllowAnonymous]
        public async Task<ActionResult> HrReportEducationManpower(DateTime toDate)
        {
            var month1 = toDate.AddMonths(-1);
            var month2 = toDate.AddMonths(-2);
            var year1 = toDate.AddYears(-1);
            ViewBag.month = toDate;
            ViewBag.months = month1;
            ViewBag.monthTw = month2;
            ViewBag.year1 = year1;
            AllHrReportViewModel model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                educationManpowerViewModels = await reports.GetHrEducationSummaryData1(toDate.ToString()),
                educationManpowerViewModels1 = await reports.GetHrEducationSummaryData1(month1.ToString()),
                educationManpowerViewModels2 = await reports.GetHrEducationSummaryData1(month2.ToString()),
                educationManpowerViewModels1y = await reports.GetHrEducationSummaryData1(year1.ToString())
            };
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult HrReportEducationManpowerPDF(string type, DateTime? toDate)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";

            if (type == "RptEducation")
            {
                url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/HrReportEducationManpower?toDate=" + toDate;
            }

            string fileName;
            string status = myPDF.GeneratePDF1(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        #region BranchManagerReport
        public async Task<IActionResult> BranchManagerView()
        {
            var model = new AllHrReportViewModel
            {
                hrBranches = await organizationService.GetAllHrBranch(),
            };
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetBranchManagerByDatePDFView(int hrBranchId, DateTime? fromDate, DateTime? toDate)
        {
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            var model = new AllHrReportViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                branchManagerViewModels = await reports.GetBranchManagerByBranchIdDate(hrBranchId, fromDate?.ToString("yyyy-MM-dd"), toDate?.ToString("yyyy-MM-dd"))
            };
            return View(model);
        }

        public IActionResult GetBranchManagerByDatePDF(int hrBranchId, DateTime? fromDate, DateTime? toDate)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";

            url = scheme + "://" + host + "/HRPMSReport/HrAdminReport/GetBranchManagerByDatePDFView?hrBranchId="+ hrBranchId + "&fromDate="+ fromDate+ "&toDate=" + toDate;

            string fileName;
            string status = myPDF.GeneratePDF(out fileName, url);
            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }


        #endregion
        #region API

        [Route("global/api/GetAllHrDesignations/")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllHrDesignations()
        {
            return Json(await designationDepartmentService.GetDesignations());
        }

        [Route("global/api/GetHrDataByDesig/{desigId}/{deptId}/{bloodGroup}/{sbuId}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetHrDataByDesig(int? desigId, int? deptId, string bloodGroup, int? sbuId)
        {
            return Json(await reports.GetHrDataByDesig(desigId, deptId, bloodGroup, sbuId));
        }
        [Route("global/api/GetHrDataByEducation/{subjectId}/{universityId}/{loeId}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetHrDataByEducation(int? subjectId, int? universityId, int? loeId)
        {
            return Json(await reports.GetHrDataByEducation(subjectId, universityId, loeId));
        }
        [Route("global/api/GetHrDataByTrCourse/{courseId}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetHrDataByTrCourse(int? courseId)
        {
            return Json(await reports.GetHrDataByTrCourse(courseId));
        }

        [Route("global/api/GetHrDataByBelongingItem/{belongingId}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetHrDataByBelongingItem(int? belongingId)
        {
            return Json(await reports.GetHrDataByBelongingItem(belongingId));
        }

        #endregion


        [AllowAnonymous]
        public async Task<IActionResult> GetAllDivision()
        {
            var divisions = await addressService.GetAllDivision();
            return Json(divisions);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GetAllHrDivision()
        {
            var division = await organizationService.GetAllHrDivision();
            return Json(division);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GetAllLocation()
        {
            var locations = await locationService.GetLocation();
            return Json(locations);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GetAllDepartment()
        {
            var departments = await designationDepartService.GetDepartment();
            return Json(departments);
        }
        //HRPMSReport/HrAdminReport/GetAllBranchesApi
        [AllowAnonymous]
        public async Task<IActionResult> GetHrBranchesByZoneId(int? zoneId)
        {
            var data = await hrBranchService.GetHrBranchesByZoneId(zoneId);
            return Json(data);
        }

		[AllowAnonymous]
		public async Task<IActionResult> GetAllUnit()
		{
            var units = from f in await _statusService.GetHrUnit() select new { f.Id, f.unitName };
			return Json(units);
		}

		[AllowAnonymous]
		public async Task<IActionResult> GetAllOffice()
		{
			var offices = from f in await _functionsInfoService.GetFunctionInfo() select new { f.Id, f.branchUnitName };
			return Json(offices);
		}
	}
}