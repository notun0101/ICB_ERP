
using DinkToPdf.Contracts;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSACR.Models;
using OPUSERP.Data;
using OPUSERP.Data.Entity;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Services.ACR.Interfaces;
using OPUSERP.HRPMS.Services.Employee;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace OPUSERP.Areas.HRPMSACR.Controllers
{
	[Area("HRPMSACR")]
	[Authorize]
	public class HtmlReportController : Controller
	{
		private readonly IPersonalInfoService personalInfoService;
        private readonly IProfessionalQualificationsService professionalQualificationsService;
        private readonly ERPDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IAcrInfoService acrInfoService;
		private readonly string rootPath;
		private readonly MyPDF myPDF;

		public HtmlReportController(IPersonalInfoService personalInfoService, ERPDbContext context, IAcrInfoService acrInfoService, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, IConverter converter,IProfessionalQualificationsService professionalQualificationsService)
		{
			this.personalInfoService = personalInfoService;
			this._context = context;
			this.acrInfoService = acrInfoService;
			this.professionalQualificationsService = professionalQualificationsService;
			_userManager = userManager;

			myPDF = new MyPDF(hostingEnvironment, converter);
			rootPath = hostingEnvironment.ContentRootPath;
		}

		#region Executive
		public async Task<IActionResult> AcrCompletedList()
		{
			string userName = HttpContext.User.Identity.Name;

			//List<AssessmentInfoViewModel> lstFinal = new List<AssessmentInfoViewModel>();
			//lstFinal = ObjectMapHelper<AssessmentViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetAssessmentInfoForSignatory", new object[] { userName, "final", "2020" }));
			var lstFinal = await acrInfoService.GetAssessmentInfoForSignatory(userName, "final", "2020");

			ACRAssessmentVM model = new ACRAssessmentVM
			{
				lstFinal = lstFinal,
			};
			return View(model);
		}

		[AllowAnonymous]
		public IActionResult AcrReportExcecutivePdf(int? assessmentId)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string fileName;


			url = scheme + "://" + host + "/HRPMSACR/HtmlReport/AcrReportForExecutive?assessmentId=" + assessmentId;

            string status = myPDF.GenerateOfficeLegalPDF(out fileName, url);

			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}

		[AllowAnonymous]
		public async Task<ActionResult> AcrReportForExecutive(int assessmentId)
		{
            try
            {
                //var aa = await acrInfoService.GetEmployeeAcrInfo(assessmentId);

                var empCode = await acrInfoService.GetEmployeeCodeByAssmentId(assessmentId);
                var EmpId = await personalInfoService.GetEmployeeIdByCode(empCode);
                ReportViewModel model = new ReportViewModel
                {
                    employeeInfoViewModel = await acrInfoService.GetEmployeeInfo(assessmentId),
                    employeeInfoViewModelAcr = await acrInfoService.GetEmployeeInfoForDGMtoAbove(assessmentId),
                    employeesAcrsViewModel = await acrInfoService.GetEmployeeAcrInfo(assessmentId),
                    employeesPerformanceViewModel = await acrInfoService.GetEmployeePerformanceInfo(assessmentId),
                    quantitativeEvaluationsViewModel = await acrInfoService.GetQuantativeEvaluation(assessmentId),
                    employeeTrainingViewModel = await acrInfoService.GetEmployeeTrainingInfo(assessmentId),
                    empPostings = await acrInfoService.GetEmpPostings(EmpId,assessmentId),
					empEducations = await acrInfoService.GetEmpEducations(EmpId),
                    professionalQualifications = await professionalQualificationsService.GetProfessionalQualificationsByEmpId(EmpId),
                    document = _context.docAttachment.Where(x => x.assesmentId == assessmentId && x.attachFor == "TargetAchive").FirstOrDefault()
                };
                return View(model);
            }
            catch (Exception ex )
            {

                throw ex;
            }
           
		}

		[AllowAnonymous]
		public IActionResult AcrReportOfficerPdf(int? assessmentId)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string fileName;


			url = scheme + "://" + host + "/HRPMSACR/HtmlReport/AcrReportOfficer?assessmentId=" + assessmentId;

			string status = myPDF.GenerateOfficeLegalPDF(out fileName, url);

			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}
		//#region Officer

		[AllowAnonymous]
		public async Task<ActionResult> AcrReportOfficer(int assessmentId)
		{
			var empInfo = await acrInfoService.GetEmployeeInfo(assessmentId);
			//var emp = await personalInfoService.GetEmployeeUserInfoByCodeBySp(empInfo.FirstOrDefault().EmpCode);
			var emp = await personalInfoService.GetEmployeeUserInfoByCodeBySp(empInfo.FirstOrDefault().EmpCode);
            var signature = await personalInfoService.GetSignatureByEmpId(Convert.ToInt32(empInfo.FirstOrDefault().EmpId));
            var totalqEvaluation = await acrInfoService.GetTotalQuantitativeEvaluationByAssId(assessmentId);
            ViewBag.qevaluation = totalqEvaluation;
            var jobDuration1 = await acrInfoService.GetJobDuration1ByAssessId(assessmentId);
            ViewBag.duration1 = jobDuration1;

            var jobDuration2 = await acrInfoService.GetJobDuration2ByAssessId(assessmentId);
            ViewBag.duration2 = jobDuration2;

            ReportViewModel model = new ReportViewModel
			{
				employeeInfoViewModel = empInfo,
				EmployeeAcrInfo = emp,
				acrType = await acrInfoService.GetAcrTypeByAssId(assessmentId),
				employeesAcrsViewModelN = await acrInfoService.GetEmployeeAcrInfoForOfficersNew(assessmentId),
				quantitativeEvaluationsViewModel = await acrInfoService.GetQuantativeEvaluation(assessmentId),
				employeeEducationInfoViewModel = await acrInfoService.GetEmployeeEducation(assessmentId),
				employeeTrainingViewModel = await acrInfoService.GetEmployeeTrainingInfo(assessmentId),
				employeePromotionViewModel = await acrInfoService.GetEmployeePromotion(assessmentId),
				employeeAssignmentViewModel = await acrInfoService.GetEmployeesAssignment(assessmentId),
				employeeLeaveInfoViewModels = await acrInfoService.GetEmployeesLeave(assessmentId),
				aCRRecommendation = await acrInfoService.GetACRRecommendationByAssId(assessmentId),
				document = _context.docAttachment.Where(x => x.assesmentId == assessmentId && x.attachFor == "TargetAchive").FirstOrDefault(),
				//evaluatedMarks = total,
				empEducations = await acrInfoService.GetEmpEducations(Convert.ToInt32(emp.EmpId)),
				empPostings = await acrInfoService.GetEmpPostings(Convert.ToInt32(emp.EmpId),assessmentId),
				//document = _context.docAttachment.Where(x => x.assesmentId == assessmentId && x.attachFor == "TargetAchive").FirstOrDefault()
			};
			return View(model);
		}

		[AllowAnonymous]
		public IActionResult AcrReportClericalPdf(int? assessmentId)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string fileName;


            url = scheme + "://" + host + "/HRPMSACR/HtmlReport/AcrReportClerical?assessmentId=" + assessmentId;
            //url = scheme + "://" + host + "/HRPMSACR/ThirdClericalReport/ClericalReportPdf?assessmentId=" + assessmentId;

            string status = myPDF.GenerateLegalPDF(out fileName, url);

			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}
		//#region Officer

		[AllowAnonymous]
		public async Task<ActionResult> AcrReportClerical(int assessmentId)
		{
            var empInfo = await acrInfoService.GetEmployeeInfo(assessmentId);
            var emp = await personalInfoService.GetEmployeeUserInfoByCodeBySp(empInfo.FirstOrDefault().EmpCode);
            ReportViewModel model = new ReportViewModel
			{
				prevJobHistoryVms = await acrInfoService.GetPrevJobHistoryByAssessmentId(assessmentId),
				employeeInfoViewModel = await acrInfoService.GetEmployeeInfo(assessmentId),
				employeesAcrsViewModel = await acrInfoService.GetEmployeeAcrInfoForClerks(assessmentId),
				//employeeTrainingViewModel = await acrInfoService.GetEmployeeTrainingInfo(assessmentId),
				acrOtherTableViewModel = await acrInfoService.GetAcrOtherTablesReport(assessmentId),
                empEducations = await acrInfoService.GetEmpEducations(Convert.ToInt32(emp.EmpId)),
                empPostings = await acrInfoService.GetEmpPostings(Convert.ToInt32(emp.EmpId),assessmentId)
            };
			return View(model);
		}

		[AllowAnonymous]
		public IActionResult AcrReportNonClericalPdf(int? assessmentId)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string url = "";
			string fileName;


			url = scheme + "://" + host + "/HRPMSACR/HtmlReport/AcrReportNonClerical?assessmentId=" + assessmentId;

			string status = myPDF.GenerateOfficeLegalPDF(out fileName, url);

			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}
		//#region Officer

		[AllowAnonymous]
		public async Task<ActionResult> AcrReportNonClerical(int assessmentId)
		{
			ReportViewModel model = new ReportViewModel
			{
				employeeInfoViewModel = await acrInfoService.GetEmployeeInfo(assessmentId),
				employeesAcrsViewModel = await acrInfoService.GetEmployeeAcrInfoForClerks(assessmentId),
				//employeeTrainingViewModel = await acrInfoService.GetEmployeeTrainingInfo(assessmentId),
				acrOtherTableViewModel = await acrInfoService.GetAcrOtherTablesReport(assessmentId)
			};
			return View(model);
		}

		//public ActionResult AcrReportClericalPdf(int assesmentId)
		//{
		//    var lstEmp = new List<EmployeeInfoViewModel>();
		//    lstEmp = ObjectMapHelper<EmployeeInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesByCode", new object[] { assesmentId }));

		//    var acr = new List<EmployeesAcrsViewModel>();
		//    acr = ObjectMapHelper<EmployeesAcrsViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_SUB_Clerical_Performance", new object[] { assesmentId }));

		//    var training = new List<EmployeeTrainingViewModel>();
		//    training = ObjectMapHelper<EmployeeTrainingViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeeTraining", new object[] { assesmentId }));

		//    var acrOtherTable = new List<ACROtherTableViewModel>();
		//    acrOtherTable = ObjectMapHelper<ACROtherTableViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_AcrOtherTablesValue", new object[] { assesmentId }));

		//    var model = new ReportViewModel
		//    {
		//        employeeInfoViewModel = lstEmp,
		//        employeesAcrsViewModel = acr,
		//        employeeTrainingViewModel = training,
		//        acrOtherTableViewModel = acrOtherTable
		//    };
		//    //return new Rotativa.ViewAsPdf("AcrReportClericalPdf", model);
		//    string userName = HttpContext.User.Identity.Name;
		//    int? userTypeId = _context.Users.Where(x => x.UserName == userName).FirstOrDefault().userTypeId;

		//    var data = _context.Assessments.Find(assesmentId);
		//    if (data.statusInfoId == 1 && data.empCode == userName)
		//    {
		//        return new Rotativa.ViewAsPdf("AcrReportClericalPdf", model);
		//    }
		//    else if (userTypeId == 1 || userTypeId == 2 || userTypeId == 9)
		//    {
		//        return new Rotativa.ViewAsPdf("AcrReportClericalPdf", model);
		//    }
		//    else
		//    {
		//        return View("UnauthorizedAccess");
		//    }
		//}
		//public ActionResult AcrReportExcecutive2019Pdf(int assesmentId)
		//{
		//    var lstEmp = new List<EmployeeInfoViewModel>();
		//    lstEmp = ObjectMapHelper<EmployeeInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesByCode", new object[] { assesmentId }));

		//    var acr = new List<EmployeesAcrsViewModel>();
		//    acr = ObjectMapHelper<EmployeesAcrsViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_SUB_DGM_PersonalCharacter_Html", new object[] { assesmentId }));

		//    var quan = new List<QuantitativeEvaluationsViewModel>();
		//    quan = ObjectMapHelper<QuantitativeEvaluationsViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_SUB_TargetEvaluation", new object[] { assesmentId }));

		//    var training = new List<EmployeeTrainingViewModel>();
		//    training = ObjectMapHelper<EmployeeTrainingViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeeTraining", new object[] { assesmentId }));

		//    var model = new ReportViewModel
		//    {
		//        employeeInfoViewModel = lstEmp,
		//        employeesAcrsViewModel = acr,
		//        quantitativeEvaluationsViewModel = quan,
		//        employeeTrainingViewModel = training,
		//        document = _context.DocumentAttachments.Where(x => x.assesmentId == assesmentId && x.attachFor == "TargetAchive").FirstOrDefault()
		//    };
		//    //return new Rotativa.ViewAsPdf("AcrReportExcecutivePdf", model);

		//    string userName = HttpContext.User.Identity.Name;
		//    int? userTypeId = _context.Users.Where(x => x.UserName == userName).FirstOrDefault().userTypeId;

		//    var data = _context.Assessments.Find(assesmentId);
		//    if (data.statusInfoId == 1 && data.empCode == userName)
		//    {
		//        return new Rotativa.ViewAsPdf("AcrReportExcecutive2019Pdf", model);
		//    }
		//    else if (userTypeId == 1 || userTypeId == 2 || userTypeId == 10)
		//    {
		//        return new Rotativa.ViewAsPdf("AcrReportExcecutive2019Pdf", model);
		//    }
		//    else
		//    {
		//        return View("UnauthorizedAccess");
		//    }
		//    //return View();
		//}

		//public ActionResult AcrReportExcecutiveOwnPdf(int assesmentId)
		//{
		//    var lstEmp = new List<EmployeeInfoViewModel>();
		//    lstEmp = ObjectMapHelper<EmployeeInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesByCode", new object[] { assesmentId }));

		//    var acr = new List<EmployeesAcrsViewModel>();
		//    acr = ObjectMapHelper<EmployeesAcrsViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_SUB_DGM_PersonalCharacter_Html", new object[] { assesmentId }));

		//    var quan = new List<QuantitativeEvaluationsViewModel>();
		//    quan = ObjectMapHelper<QuantitativeEvaluationsViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_SUB_TargetEvaluation", new object[] { assesmentId }));

		//    var training = new List<EmployeeTrainingViewModel>();
		//    training = ObjectMapHelper<EmployeeTrainingViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeeTraining", new object[] { assesmentId }));

		//    var model = new ReportViewModel
		//    {
		//        employeeInfoViewModel = lstEmp,
		//        employeesAcrsViewModel = acr,
		//        quantitativeEvaluationsViewModel = quan,
		//        employeeTrainingViewModel = training,
		//        document = _context.DocumentAttachments.Where(x => x.assesmentId == assesmentId && x.attachFor == "TargetAchive").FirstOrDefault()
		//    };
		//    //return new Rotativa.ViewAsPdf("AcrReportExcecutivePdf", model);

		//    string userName = HttpContext.User.Identity.Name;
		//    int? userTypeId = _context.Users.Where(x => x.UserName == userName).FirstOrDefault().userTypeId;

		//    var data = _context.Assessments.Find(assesmentId);
		//    if (data.statusInfoId == 1 && data.empCode == userName)
		//    {
		//        return new Rotativa.ViewAsPdf("AcrReportExcecutivePdf", model);
		//    }
		//    else if (userTypeId == 1 || userTypeId == 2 || userTypeId == 10)
		//    {
		//        return new Rotativa.ViewAsPdf("AcrReportExcecutivePdf", model);
		//    }
		//    else
		//    {
		//        return View("UnauthorizedAccess");
		//    }
		//    //return View();
		//}
		#endregion

		//#region Officer
		//public ActionResult AcrReportOfficerPdf(int assesmentId)
		//{
		//    var lstEmp = new List<EmployeeInfoViewModel>();
		//    lstEmp = ObjectMapHelper<EmployeeInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesByCode", new object[] { assesmentId }));

		//    var acr = new List<EmployeesAcrsViewModel>();
		//    acr = ObjectMapHelper<EmployeesAcrsViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_Getparttwobyassignmentid", new object[] { assesmentId }));

		//    var quan = new List<QuantitativeEvaluationsViewModel>();
		//    quan = ObjectMapHelper<QuantitativeEvaluationsViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_SUB_TargetEvaluation", new object[] { assesmentId }));

		//    var quan92 = new List<QuantitativeEvaluationsViewModel>();
		//    quan92 = ObjectMapHelper<QuantitativeEvaluationsViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_SUB_TargetEvaluation_Officer_4thPart", new object[] { assesmentId }));

		//    var training = new List<EmployeeTrainingViewModel>();
		//    training = ObjectMapHelper<EmployeeTrainingViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeeTraining", new object[] { assesmentId }));

		//    var education = new List<EmployeeEducationInfoViewModel>();
		//    education = ObjectMapHelper<EmployeeEducationInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeeEducation", new object[] { assesmentId }));

		//    var promotion = new List<EmployeePromotionViewModel>();
		//    promotion = ObjectMapHelper<EmployeePromotionViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesPromotion", new object[] { assesmentId }));

		//    var leave = new List<EmployeeLeaveInfoViewModel>();
		//    leave = ObjectMapHelper<EmployeeLeaveInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesLeave", new object[] { assesmentId }));

		//    var assignment = new List<EmployeeAssignmentViewModel>();
		//    assignment = ObjectMapHelper<EmployeeAssignmentViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesAssignment", new object[] { assesmentId }));

		//    var acrOtherTable = new List<ACROtherTableViewModel>();
		//    acrOtherTable = ObjectMapHelper<ACROtherTableViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_AcrOtherTablesValue", new object[] { assesmentId }));

		//    var model = new ReportViewModel
		//    {
		//        employeeInfoViewModel = lstEmp,
		//        employeesAcrsViewModel = acr,
		//        quantitativeEvaluationsViewModel = quan,
		//        quantitativeEvaluationsViewModel92 = quan92,
		//        employeeTrainingViewModel = training,
		//        employeeEducationInfoViewModel = education,
		//        employeePromotionViewModel = promotion,
		//        employeeLeaveInfoViewModel = leave,
		//        employeeAssignmentViewModel = assignment,
		//        acrOtherTableViewModel = acrOtherTable
		//    };

		//    string userName = HttpContext.User.Identity.Name;
		//    int? userTypeId = _context.Users.Where(x => x.UserName == userName).FirstOrDefault().userTypeId;

		//    var data = _context.Assessments.Find(assesmentId);
		//    if (data.statusInfoId == 1 && data.empCode == userName)
		//    {
		//        return new Rotativa.ViewAsPdf("AcrReportOfficerPdf", model);
		//    }
		//    else if (userTypeId == 1 || userTypeId == 2 || userTypeId == 8 || userTypeId == 10)
		//    {
		//        return new Rotativa.ViewAsPdf("AcrReportOfficerPdf", model);
		//    }
		//    //return new Rotativa.ViewAsPdf("AcrReportOfficerPdf", model);
		//    else
		//    {
		//        return View("UnauthorizedAccess");
		//    }
		//}

		//public ActionResult AcrReportOfficer2019Pdf(int assesmentId)
		//{
		//    var lstEmp = new List<EmployeeInfoViewModel>();
		//    lstEmp = ObjectMapHelper<EmployeeInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesByCode", new object[] { assesmentId }));

		//    var acr = new List<EmployeesAcrsViewModel>();
		//    acr = ObjectMapHelper<EmployeesAcrsViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_Getparttwobyassignmentid_2019", new object[] { assesmentId }));

		//    var quan = new List<QuantitativeEvaluationsViewModel>();
		//    quan = ObjectMapHelper<QuantitativeEvaluationsViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_SUB_TargetEvaluation", new object[] { assesmentId }));

		//    var quan92 = new List<QuantitativeEvaluationsViewModel>();
		//    quan92 = ObjectMapHelper<QuantitativeEvaluationsViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_SUB_TargetEvaluation_Officer_4thPart", new object[] { assesmentId }));

		//    var training = new List<EmployeeTrainingViewModel>();
		//    training = ObjectMapHelper<EmployeeTrainingViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeeTraining", new object[] { assesmentId }));

		//    var education = new List<EmployeeEducationInfoViewModel>();
		//    education = ObjectMapHelper<EmployeeEducationInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeeEducation", new object[] { assesmentId }));

		//    var promotion = new List<EmployeePromotionViewModel>();
		//    promotion = ObjectMapHelper<EmployeePromotionViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesPromotion", new object[] { assesmentId }));

		//    var leave = new List<EmployeeLeaveInfoViewModel>();
		//    leave = ObjectMapHelper<EmployeeLeaveInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesLeave", new object[] { assesmentId }));

		//    var assignment = new List<EmployeeAssignmentViewModel>();
		//    assignment = ObjectMapHelper<EmployeeAssignmentViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesAssignment", new object[] { assesmentId }));

		//    var acrOtherTable = new List<ACROtherTableViewModel>();
		//    acrOtherTable = ObjectMapHelper<ACROtherTableViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_AcrOtherTablesValue", new object[] { assesmentId }));

		//    var model = new ReportViewModel
		//    {
		//        employeeInfoViewModel = lstEmp,
		//        employeesAcrsViewModel = acr,
		//        quantitativeEvaluationsViewModel = quan,
		//        quantitativeEvaluationsViewModel92 = quan92,
		//        employeeTrainingViewModel = training,
		//        employeeEducationInfoViewModel = education,
		//        employeePromotionViewModel = promotion,
		//        employeeLeaveInfoViewModel = leave,
		//        employeeAssignmentViewModel = assignment,
		//        acrOtherTableViewModel = acrOtherTable
		//    };

		//    string userName = HttpContext.User.Identity.Name;
		//    int? userTypeId = _context.Users.Where(x => x.UserName == userName).FirstOrDefault().userTypeId;

		//    var data = _context.Assessments.Find(assesmentId);
		//    if (data.statusInfoId == 1 && data.empCode == userName)
		//    {
		//        return new Rotativa.ViewAsPdf("AcrReportOfficer2019Pdf", model);
		//    }
		//    else if (userTypeId == 1 || userTypeId == 2 || userTypeId == 8 || userTypeId == 10)
		//    {
		//        return new Rotativa.ViewAsPdf("AcrReportOfficer2019Pdf", model);
		//    }
		//    //return new Rotativa.ViewAsPdf("AcrReportOfficerPdf", model);
		//    else
		//    {
		//        return View("UnauthorizedAccess");
		//    }
		//}

		//public ActionResult AcrReportOfficerOwnPdf(int assesmentId)
		//{
		//    var lstEmp = new List<EmployeeInfoViewModel>();
		//    lstEmp = ObjectMapHelper<EmployeeInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesByCode", new object[] { assesmentId }));

		//    var acr = new List<EmployeesAcrsViewModel>();
		//    acr = ObjectMapHelper<EmployeesAcrsViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_Getparttwobyassignmentid", new object[] { assesmentId }));

		//    //var quan = new List<QuantitativeEvaluationsViewModel>();
		//    //quan = ObjectMapHelper<QuantitativeEvaluationsViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_SUB_TargetEvaluation", new object[] { assesmentId }));

		//    //var quan92 = new List<QuantitativeEvaluationsViewModel>();
		//    //quan92 = ObjectMapHelper<QuantitativeEvaluationsViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_SUB_TargetEvaluation_Officer_4thPart", new object[] { assesmentId }));

		//    //var training = new List<EmployeeTrainingViewModel>();
		//    //training = ObjectMapHelper<EmployeeTrainingViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeeTraining", new object[] { assesmentId }));

		//    //var education = new List<EmployeeEducationInfoViewModel>();
		//    //education = ObjectMapHelper<EmployeeEducationInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeeEducation", new object[] { assesmentId }));

		//    //var promotion = new List<EmployeePromotionViewModel>();
		//    //promotion = ObjectMapHelper<EmployeePromotionViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesPromotion", new object[] { assesmentId }));

		//    //var leave = new List<EmployeeLeaveInfoViewModel>();
		//    //leave = ObjectMapHelper<EmployeeLeaveInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesLeave", new object[] { assesmentId }));

		//    //var assignment = new List<EmployeeAssignmentViewModel>();
		//    //assignment = ObjectMapHelper<EmployeeAssignmentViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesAssignment", new object[] { assesmentId }));

		//    //var acrOtherTable = new List<ACROtherTableViewModel>();
		//    //acrOtherTable = ObjectMapHelper<ACROtherTableViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_AcrOtherTablesValue", new object[] { assesmentId }));

		//    var model = new ReportViewModel
		//    {
		//        employeeInfoViewModel = lstEmp,
		//        employeesAcrsViewModel = acr,
		//        //quantitativeEvaluationsViewModel = quan,
		//        //quantitativeEvaluationsViewModel92 = quan92,
		//        //employeeTrainingViewModel = training,
		//        //employeeEducationInfoViewModel = education,
		//        //employeePromotionViewModel = promotion,
		//        //employeeLeaveInfoViewModel = leave,
		//        //employeeAssignmentViewModel = assignment,
		//        //acrOtherTableViewModel = acrOtherTable
		//    };

		//    string userName = HttpContext.User.Identity.Name;
		//    int? userTypeId = _context.Users.Where(x => x.UserName == userName).FirstOrDefault().userTypeId;

		//    var data = _context.Assessments.Find(assesmentId);
		//    if (data.statusInfoId == 1 && data.empCode == userName)
		//    {
		//        return new Rotativa.ViewAsPdf("AcrReportOfficerOwnPdf", model);
		//    }
		//    //else if (userTypeId == 1 || userTypeId == 2 || userTypeId == 8 || userTypeId == 10)
		//    //{
		//    //    return new Rotativa.ViewAsPdf("AcrReportOfficerPdf", model);
		//    //}
		//    //return new Rotativa.ViewAsPdf("AcrReportOfficerPdf", model);
		//    else
		//    {
		//        return View("UnauthorizedAccess");
		//    }
		//}

		//#endregion

		//#region Clerical
		//public ActionResult AcrReportClerical2019Pdf(int assesmentId)
		//{
		//    var lstEmp = new List<EmployeeInfoViewModel>();
		//    lstEmp = ObjectMapHelper<EmployeeInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesByCode", new object[] { assesmentId }));

		//    var acr = new List<EmployeesAcrsViewModel>();
		//    acr = ObjectMapHelper<EmployeesAcrsViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_SUB_Clerical_Performance", new object[] { assesmentId }));

		//    var training = new List<EmployeeTrainingViewModel>();
		//    training = ObjectMapHelper<EmployeeTrainingViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeeTraining", new object[] { assesmentId }));

		//    var acrOtherTable = new List<ACROtherTableViewModel>();
		//    acrOtherTable = ObjectMapHelper<ACROtherTableViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_AcrOtherTablesValue", new object[] { assesmentId }));

		//    var model = new ReportViewModel
		//    {
		//        employeeInfoViewModel = lstEmp,
		//        employeesAcrsViewModel = acr,
		//        employeeTrainingViewModel = training,
		//        acrOtherTableViewModel = acrOtherTable
		//    };
		//    //return new Rotativa.ViewAsPdf("AcrReportClericalPdf", model);
		//    string userName = HttpContext.User.Identity.Name;
		//    int? userTypeId = _context.Users.Where(x => x.UserName == userName).FirstOrDefault().userTypeId;

		//    var data = _context.Assessments.Find(assesmentId);
		//    if (data.statusInfoId == 1 && data.empCode == userName)
		//    {
		//        return new Rotativa.ViewAsPdf("AcrReportClerical2019Pdf", model);
		//    }
		//    else if (userTypeId == 1 || userTypeId == 2 || userTypeId == 9)
		//    {
		//        return new Rotativa.ViewAsPdf("AcrReportClerical2019Pdf", model);
		//    }
		//    else
		//    {
		//        return View("UnauthorizedAccess");
		//    }
		//}
		//public ActionResult AcrReportClericalPdf(int assesmentId)
		//{
		//    var lstEmp = new List<EmployeeInfoViewModel>();
		//    lstEmp = ObjectMapHelper<EmployeeInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesByCode", new object[] { assesmentId }));

		//    var acr = new List<EmployeesAcrsViewModel>();
		//    acr = ObjectMapHelper<EmployeesAcrsViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_SUB_Clerical_Performance", new object[] { assesmentId }));

		//    var training = new List<EmployeeTrainingViewModel>();
		//    training = ObjectMapHelper<EmployeeTrainingViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeeTraining", new object[] { assesmentId }));

		//    var acrOtherTable = new List<ACROtherTableViewModel>();
		//    acrOtherTable = ObjectMapHelper<ACROtherTableViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_AcrOtherTablesValue", new object[] { assesmentId }));

		//    var model = new ReportViewModel
		//    {
		//        employeeInfoViewModel = lstEmp,
		//        employeesAcrsViewModel = acr,
		//        employeeTrainingViewModel = training,
		//        acrOtherTableViewModel = acrOtherTable
		//    };
		//    //return new Rotativa.ViewAsPdf("AcrReportClericalPdf", model);
		//    string userName = HttpContext.User.Identity.Name;
		//    int? userTypeId = _context.Users.Where(x => x.UserName == userName).FirstOrDefault().userTypeId;

		//    var data = _context.Assessments.Find(assesmentId);
		//    if (data.statusInfoId == 1 && data.empCode == userName)
		//    {
		//        return new Rotativa.ViewAsPdf("AcrReportClericalPdf", model);
		//    }
		//    else if (userTypeId == 1 || userTypeId == 2 || userTypeId == 9)
		//    {
		//        return new Rotativa.ViewAsPdf("AcrReportClericalPdf", model);
		//    }
		//    else
		//    {
		//        return View("UnauthorizedAccess");
		//    }
		//}

		//#endregion

		//#region Non Clerical       
		//public ActionResult AcrReportNonClerical2019Pdf(int assesmentId)
		//{
		//    var lstEmp = new List<EmployeeInfoViewModel>();
		//    lstEmp = ObjectMapHelper<EmployeeInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesByCode", new object[] { assesmentId }));

		//    var acr = new List<EmployeesAcrsViewModel>();
		//    acr = ObjectMapHelper<EmployeesAcrsViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_SUB_Clerical_Performance", new object[] { assesmentId }));

		//    var training = new List<EmployeeTrainingViewModel>();
		//    training = ObjectMapHelper<EmployeeTrainingViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeeTraining", new object[] { assesmentId }));

		//    var acrOtherTable = new List<ACROtherTableViewModel>();
		//    acrOtherTable = ObjectMapHelper<ACROtherTableViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_AcrOtherTablesValue", new object[] { assesmentId }));

		//    var model = new ReportViewModel
		//    {
		//        employeeInfoViewModel = lstEmp,
		//        employeesAcrsViewModel = acr,
		//        employeeTrainingViewModel = training,
		//        acrOtherTableViewModel = acrOtherTable
		//    };
		//    //return new Rotativa.ViewAsPdf("AcrReportNonClericalPdf", model);
		//    string userName = HttpContext.User.Identity.Name;
		//    int? userTypeId = _context.Users.Where(x => x.UserName == userName).FirstOrDefault().userTypeId;

		//    var data = _context.Assessments.Find(assesmentId);
		//    if (data.statusInfoId == 1 && data.empCode == userName)
		//    {
		//        return new Rotativa.ViewAsPdf("AcrReportNonClerical2019Pdf", model);
		//    }
		//    else if (userTypeId == 1 || userTypeId == 2 || userTypeId == 9)
		//    {
		//        return new Rotativa.ViewAsPdf("AcrReportNonClerical2019Pdf", model);
		//    }
		//    else
		//    {
		//        return View("UnauthorizedAccess");
		//    }
		//}

		//public ActionResult AcrReportNonClericalPdf(int assesmentId)
		//{
		//    var lstEmp = new List<EmployeeInfoViewModel>();
		//    lstEmp = ObjectMapHelper<EmployeeInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesByCode", new object[] { assesmentId }));

		//    var acr = new List<EmployeesAcrsViewModel>();
		//    acr = ObjectMapHelper<EmployeesAcrsViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_SUB_Clerical_Performance", new object[] { assesmentId }));

		//    var training = new List<EmployeeTrainingViewModel>();
		//    training = ObjectMapHelper<EmployeeTrainingViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeeTraining", new object[] { assesmentId }));

		//    var acrOtherTable = new List<ACROtherTableViewModel>();
		//    acrOtherTable = ObjectMapHelper<ACROtherTableViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_AcrOtherTablesValue", new object[] { assesmentId }));

		//    var model = new ReportViewModel
		//    {
		//        employeeInfoViewModel = lstEmp,
		//        employeesAcrsViewModel = acr,
		//        employeeTrainingViewModel = training,
		//        acrOtherTableViewModel = acrOtherTable
		//    };
		//    //return new Rotativa.ViewAsPdf("AcrReportNonClericalPdf", model);
		//    string userName = HttpContext.User.Identity.Name;
		//    int? userTypeId = _context.Users.Where(x => x.UserName == userName).FirstOrDefault().userTypeId;

		//    var data = _context.Assessments.Find(assesmentId);
		//    if (data.statusInfoId == 1 && data.empCode == userName)
		//    {
		//        return new Rotativa.ViewAsPdf("AcrReportNonClericalPdf", model);
		//    }
		//    else if (userTypeId == 1 || userTypeId == 2 || userTypeId == 9)
		//    {
		//        return new Rotativa.ViewAsPdf("AcrReportNonClericalPdf", model);
		//    }
		//    else
		//    {
		//        return View("UnauthorizedAccess");
		//    }
		//}


		//#endregion

		//#region Combine Report

		//[HttpGet]
		//public ActionResult GetAssesmentIdbyDesig(string desiCode,int fromFile,int toFile)
		//{
		//    List<BatchUploadViewModel> lstBatch = ObjectMapHelper<BatchUploadViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_AssesmentEmpByDesig", new object[] { desiCode, fromFile, toFile }));
		//    var pdfFile = new ViewAsPdf();
		//    List<string> filePath = new List<string>();
		//    List<byte[]> byteList = new List<byte[]>();
		//    if (lstBatch.Count > 0)
		//    {
		//        foreach (var item in lstBatch)
		//        {
		//            if (item.empType == "Top")
		//            {
		//                var lstEmp = new List<EmployeeInfoViewModel>();
		//                lstEmp = ObjectMapHelper<EmployeeInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesByCode", new object[] { item.assessmentId }));

		//                var acr = new List<EmployeesAcrsViewModel>();
		//                acr = ObjectMapHelper<EmployeesAcrsViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_SUB_DGM_PersonalCharacter_Html", new object[] { item.assessmentId }));

		//                var quan = new List<QuantitativeEvaluationsViewModel>();
		//                quan = ObjectMapHelper<QuantitativeEvaluationsViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_SUB_TargetEvaluation", new object[] { item.assessmentId }));

		//                var training = new List<EmployeeTrainingViewModel>();
		//                training = ObjectMapHelper<EmployeeTrainingViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeeTraining", new object[] { item.assessmentId }));

		//                var model = new ReportViewModel
		//                {
		//                    employeeInfoViewModel = lstEmp,
		//                    employeesAcrsViewModel = acr,
		//                    quantitativeEvaluationsViewModel = quan,
		//                    employeeTrainingViewModel = training,
		//                    document = _context.DocumentAttachments.Where(x => x.assesmentId == item.assessmentId && x.attachFor == "TargetAchive").FirstOrDefault()
		//                };

		//                string userName = HttpContext.User.Identity.Name;
		//                int? userTypeId = _context.Users.Where(x => x.UserName == userName).FirstOrDefault().userTypeId;

		//                var data = _context.Assessments.Find(item.assessmentId);
		//                if (data.statusInfoId == 1 && data.empCode == userName)
		//                {
		//                    pdfFile = new ViewAsPdf("AcrReportExcecutivePdf", model) { FileName = DateTime.Now.Ticks + "_" + lstEmp.FirstOrDefault().FileNo.ToString() + ".pdf" + "" };
		//                }
		//                else if (userTypeId == 1 || userTypeId == 2 || userTypeId == 10)
		//                {
		//                    pdfFile = new ViewAsPdf("AcrReportExcecutivePdf", model) { FileName = DateTime.Now.Ticks + "_" + lstEmp.FirstOrDefault().FileNo.ToString() + ".pdf" + "" };
		//                }
		//                else
		//                {

		//                }
		//                byte[] pdfData = pdfFile.BuildPdf(ControllerContext);
		//                string path = Server.MapPath("~/Upload/ACRFiles/" + pdfFile.FileName);
		//                System.IO.File.WriteAllBytes(path, pdfData);
		//                filePath.Add(path);
		//                byteList.Add(pdfData);
		//            }
		//            else if (item.empType == "Officer")
		//            {
		//                var lstEmp = new List<EmployeeInfoViewModel>();
		//                lstEmp = ObjectMapHelper<EmployeeInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesByCode", new object[] { item.assessmentId }));

		//                var acr = new List<EmployeesAcrsViewModel>();
		//                acr = ObjectMapHelper<EmployeesAcrsViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_Getparttwobyassignmentid", new object[] { item.assessmentId }));

		//                var quan = new List<QuantitativeEvaluationsViewModel>();
		//                quan = ObjectMapHelper<QuantitativeEvaluationsViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_SUB_TargetEvaluation", new object[] { item.assessmentId }));

		//                var quan92 = new List<QuantitativeEvaluationsViewModel>();
		//                quan92 = ObjectMapHelper<QuantitativeEvaluationsViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_SUB_TargetEvaluation_Officer_4thPart", new object[] { item.assessmentId }));

		//                var training = new List<EmployeeTrainingViewModel>();
		//                training = ObjectMapHelper<EmployeeTrainingViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeeTraining", new object[] { item.assessmentId }));

		//                var education = new List<EmployeeEducationInfoViewModel>();
		//                education = ObjectMapHelper<EmployeeEducationInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeeEducation", new object[] { item.assessmentId }));

		//                var promotion = new List<EmployeePromotionViewModel>();
		//                promotion = ObjectMapHelper<EmployeePromotionViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesPromotion", new object[] { item.assessmentId }));

		//                var leave = new List<EmployeeLeaveInfoViewModel>();
		//                leave = ObjectMapHelper<EmployeeLeaveInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesLeave", new object[] { item.assessmentId }));

		//                var assignment = new List<EmployeeAssignmentViewModel>();
		//                assignment = ObjectMapHelper<EmployeeAssignmentViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesAssignment", new object[] { item.assessmentId }));

		//                var acrOtherTable = new List<ACROtherTableViewModel>();
		//                acrOtherTable = ObjectMapHelper<ACROtherTableViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_AcrOtherTablesValue", new object[] { item.assessmentId }));

		//                var model = new ReportViewModel
		//                {
		//                    employeeInfoViewModel = lstEmp,
		//                    employeesAcrsViewModel = acr,
		//                    quantitativeEvaluationsViewModel = quan,
		//                    quantitativeEvaluationsViewModel92 = quan92,
		//                    employeeTrainingViewModel = training,
		//                    employeeEducationInfoViewModel = education,
		//                    employeePromotionViewModel = promotion,
		//                    employeeLeaveInfoViewModel = leave,
		//                    employeeAssignmentViewModel = assignment,
		//                    acrOtherTableViewModel = acrOtherTable
		//                };

		//                string userName = HttpContext.User.Identity.Name;
		//                int? userTypeId = _context.Users.Where(x => x.UserName == userName).FirstOrDefault().userTypeId;

		//                var data = _context.Assessments.Find(item.assessmentId);
		//                if (data.statusInfoId == 1 && data.empCode == userName)
		//                {
		//                    pdfFile = new ViewAsPdf("AcrReportOfficerPdf", model) { FileName = DateTime.Now.Ticks + "_" + lstEmp.FirstOrDefault().FileNo.ToString() + ".pdf" + "" };
		//                }
		//                else if (userTypeId == 1 || userTypeId == 2 || userTypeId == 8 || userTypeId == 10)
		//                {
		//                    pdfFile = new ViewAsPdf("AcrReportOfficerPdf", model) { FileName = DateTime.Now.Ticks + "_" + lstEmp.FirstOrDefault().FileNo.ToString() + ".pdf" + "" };
		//                }
		//                else
		//                {

		//                }
		//                byte[] pdfData = pdfFile.BuildPdf(ControllerContext);
		//                string path = Server.MapPath("~/Upload/ACRFiles/" + pdfFile.FileName);
		//                System.IO.File.WriteAllBytes(path, pdfData);
		//                filePath.Add(path);
		//                byteList.Add(pdfData);
		//            }
		//            else if (item.empType == "Clerk")
		//            {
		//                var lstEmp = new List<EmployeeInfoViewModel>();
		//                lstEmp = ObjectMapHelper<EmployeeInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesByCode", new object[] { item.assessmentId }));

		//                var acr = new List<EmployeesAcrsViewModel>();
		//                acr = ObjectMapHelper<EmployeesAcrsViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_SUB_Clerical_Performance", new object[] { item.assessmentId }));

		//                var training = new List<EmployeeTrainingViewModel>();
		//                training = ObjectMapHelper<EmployeeTrainingViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeeTraining", new object[] { item.assessmentId }));

		//                var acrOtherTable = new List<ACROtherTableViewModel>();
		//                acrOtherTable = ObjectMapHelper<ACROtherTableViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_AcrOtherTablesValue", new object[] { item.assessmentId }));

		//                var model = new ReportViewModel
		//                {
		//                    employeeInfoViewModel = lstEmp,
		//                    employeesAcrsViewModel = acr,
		//                    employeeTrainingViewModel = training,
		//                    acrOtherTableViewModel = acrOtherTable
		//                };
		//                string userName = HttpContext.User.Identity.Name;
		//                int? userTypeId = _context.Users.Where(x => x.UserName == userName).FirstOrDefault().userTypeId;

		//                var data = _context.Assessments.Find(item.assessmentId);
		//                if (data.statusInfoId == 1 && data.empCode == userName)
		//                {
		//                    pdfFile = new ViewAsPdf("AcrReportClericalPdf", model) { FileName = DateTime.Now.Ticks + "_" + lstEmp.FirstOrDefault().FileNo.ToString() + ".pdf" + "" };
		//                }
		//                else if (userTypeId == 1 || userTypeId == 2 || userTypeId == 9)
		//                {
		//                    pdfFile = new ViewAsPdf("AcrReportClericalPdf", model) { FileName = DateTime.Now.Ticks + "_" + lstEmp.FirstOrDefault().FileNo.ToString() + ".pdf" + "" };
		//                }
		//                else
		//                {

		//                }
		//                byte[] pdfData = pdfFile.BuildPdf(ControllerContext);
		//                string path = Server.MapPath("~/Upload/ACRFiles/" + pdfFile.FileName);
		//                System.IO.File.WriteAllBytes(path, pdfData);
		//                filePath.Add(path);
		//                byteList.Add(pdfData);
		//            }
		//            else if (item.empType == "NonClerk")
		//            {
		//                var lstEmp = new List<EmployeeInfoViewModel>();
		//                lstEmp = ObjectMapHelper<EmployeeInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesByCode", new object[] { item.assessmentId }));

		//                var acr = new List<EmployeesAcrsViewModel>();
		//                acr = ObjectMapHelper<EmployeesAcrsViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_SUB_Clerical_Performance", new object[] { item.assessmentId }));

		//                var training = new List<EmployeeTrainingViewModel>();
		//                training = ObjectMapHelper<EmployeeTrainingViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeeTraining", new object[] { item.assessmentId }));

		//                var acrOtherTable = new List<ACROtherTableViewModel>();
		//                acrOtherTable = ObjectMapHelper<ACROtherTableViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_AcrOtherTablesValue", new object[] { item.assessmentId }));

		//                var model = new ReportViewModel
		//                {
		//                    employeeInfoViewModel = lstEmp,
		//                    employeesAcrsViewModel = acr,
		//                    employeeTrainingViewModel = training,
		//                    acrOtherTableViewModel = acrOtherTable
		//                };
		//                string userName = HttpContext.User.Identity.Name;
		//                int? userTypeId = _context.Users.Where(x => x.UserName == userName).FirstOrDefault().userTypeId;

		//                var data = _context.Assessments.Find(item.assessmentId);
		//                if (data.statusInfoId == 1 && data.empCode == userName)
		//                {
		//                    pdfFile = new ViewAsPdf("AcrReportNonClericalPdf", model) { FileName = DateTime.Now.Ticks + "_" + lstEmp.FirstOrDefault().FileNo.ToString() + ".pdf" + "" };
		//                }
		//                else if (userTypeId == 1 || userTypeId == 2 || userTypeId == 9)
		//                {
		//                    pdfFile = new ViewAsPdf("AcrReportNonClericalPdf", model) { FileName = DateTime.Now.Ticks + "_" + lstEmp.FirstOrDefault().FileNo.ToString() + ".pdf" + "" };
		//                }
		//                else
		//                {

		//                }
		//                byte[] pdfData = pdfFile.BuildPdf(ControllerContext);
		//                string path = Server.MapPath("~/Upload/ACRFiles/" + pdfFile.FileName);
		//                System.IO.File.WriteAllBytes(path, pdfData);
		//                filePath.Add(path);
		//                byteList.Add(pdfData);
		//            }

		//        }

		//        FileStream[] streamArray = new FileStream[filePath.Count()];
		//        for (int i = 0; i < filePath.Count(); i++)
		//        {
		//            streamArray[i] = new FileStream(filePath[i], FileMode.Open);
		//        }
		//        PdfLibrary pdfMerger = new PdfLibrary();
		//        string mergePath = Server.MapPath("~/Upload/ACRFiles/" + DateTime.Now.Ticks + "_" + "Combine.pdf");

		//        // merge files
		//        var fileContent = pdfMerger.MergeFiles(byteList);
		//        System.IO.File.WriteAllBytes(mergePath, fileContent);
		//        return File(mergePath, "application/pdf");
		//    }

		//    return View("UnauthorizedAccess");
		//}

		//#endregion

		//#region Marking Worksheet
		//public ActionResult MarkingWorksheetForGMPdf(int assesmentId, string empCode)
		//{
		//    var lstEmp = new List<EmployeeInfoViewModel>();
		//    lstEmp = ObjectMapHelper<EmployeeInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesByCode", new object[] { assesmentId }));

		//    var education = new List<EducationalEvaluationViewModel>();
		//    education = ObjectMapHelper<EducationalEvaluationViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEducationAbilityFiveMarks", new object[] { empCode }));

		//    var merit = new List<MeritEvaluationViewModel>();
		//    merit = ObjectMapHelper<MeritEvaluationViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_ACR_Evaluation_Three_Marks", new object[] { empCode }));

		//    var profQ = new List<ProfessionalEvaluationViewModel>();
		//    profQ = ObjectMapHelper<ProfessionalEvaluationViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_ProfessionalQualificationSIXMarks", new object[] { empCode }));

		//    var jobLength = new List<JobLengthViewModel>();
		//    jobLength = ObjectMapHelper<JobLengthViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_JobLenth_TENMarks", new object[] { empCode }));

		//    var branchZoneFF = new List<BranchZonalHeadViewModel>();
		//    branchZoneFF = ObjectMapHelper<BranchZonalHeadViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_BranchHeadZonalHeadTowMarks", new object[] { empCode }));

		//    var model = new MarkingWorksheetReportViewModel
		//    {
		//        employeeInfoViewModel = lstEmp,
		//        last = education.FirstOrDefault().lastLevelEducationMarks,
		//        meritEvaluationViewModel = merit,
		//        professionalEvaluationViewModel = profQ,
		//        jobLengthViewModel = jobLength,
		//        branchZonalHeadViewModel = branchZoneFF
		//    };
		//    string designation = lstEmp.FirstOrDefault().DesignationName;

		//    string userName = HttpContext.User.Identity.Name;
		//    int? userTypeId = _context.Users.Where(x => x.UserName == userName).FirstOrDefault().userTypeId;

		//    var data = _context.Assessments.Find(assesmentId);
		//    if (data.statusInfoId == 1 && data.empCode == userName)
		//    {
		//        return new Rotativa.ViewAsPdf("MarkingWorksheetForGMPdf", model);
		//    }
		//    else if (userTypeId == 1 || userTypeId == 2 || userTypeId == 8 || userTypeId == 10)
		//    {
		//        return new Rotativa.ViewAsPdf("MarkingWorksheetForGMPdf", model);
		//    }
		//    //return new Rotativa.ViewAsPdf("AcrReportOfficerPdf", model);
		//    else
		//    {
		//        return View("UnauthorizedAccess");
		//    }
		//}

		//public ActionResult MarkingWorksheetForDGMPdf(int assesmentId, string empCode)
		//{
		//    var lstEmp = new List<EmployeeInfoViewModel>();
		//    lstEmp = ObjectMapHelper<EmployeeInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesByCode", new object[] { assesmentId }));

		//    var education = new List<EducationalEvaluationViewModel>();
		//    education = ObjectMapHelper<EducationalEvaluationViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEducationAbilityFiveMarks", new object[] { empCode }));

		//    var merit = new List<MeritEvaluationViewModel>();
		//    merit = ObjectMapHelper<MeritEvaluationViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_ACR_Evaluation_Three_Marks", new object[] { empCode }));

		//    var profQ = new List<ProfessionalEvaluationViewModel>();
		//    profQ = ObjectMapHelper<ProfessionalEvaluationViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_ProfessionalQualificationSIXMarks", new object[] { empCode }));

		//    var jobLength = new List<JobLengthViewModel>();
		//    jobLength = ObjectMapHelper<JobLengthViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_JobLenth_TENMarks", new object[] { empCode }));

		//    var branchZoneFF = new List<BranchZonalHeadViewModel>();
		//    branchZoneFF = ObjectMapHelper<BranchZonalHeadViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_BranchHeadZonalHeadTowMarks", new object[] { empCode }));

		//    var model = new MarkingWorksheetReportViewModel
		//    {
		//        employeeInfoViewModel = lstEmp,
		//        last = education.FirstOrDefault().lastLevelEducationMarks,
		//        meritEvaluationViewModel = merit,
		//        professionalEvaluationViewModel = profQ,
		//        jobLengthViewModel = jobLength,
		//        branchZonalHeadViewModel = branchZoneFF
		//    };
		//    string designation = lstEmp.FirstOrDefault().DesignationName;

		//    string userName = HttpContext.User.Identity.Name;
		//    int? userTypeId = _context.Users.Where(x => x.UserName == userName).FirstOrDefault().userTypeId;

		//    var data = _context.Assessments.Find(assesmentId);
		//    if (data.statusInfoId == 1 && data.empCode == userName)
		//    {
		//        return new Rotativa.ViewAsPdf("MarkingWorksheetForDGMPdf", model);
		//    }
		//    else if (userTypeId == 1 || userTypeId == 2 || userTypeId == 8 || userTypeId == 10)
		//    {
		//        return new Rotativa.ViewAsPdf("MarkingWorksheetForDGMPdf", model);
		//    }
		//    //return new Rotativa.ViewAsPdf("AcrReportOfficerPdf", model);
		//    else
		//    {
		//        return View("UnauthorizedAccess");
		//    }
		//}

		//public ActionResult MarkingWorksheetForAGMPdf(int assesmentId, string empCode)
		//{
		//    var lstEmp = new List<EmployeeInfoViewModel>();
		//    lstEmp = ObjectMapHelper<EmployeeInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesByCode", new object[] { assesmentId }));

		//    var education = new List<EducationalEvaluationViewModel>();
		//    education = ObjectMapHelper<EducationalEvaluationViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEducationAbilityFiveMarks", new object[] { empCode }));

		//    var merit = new List<MeritEvaluationViewModel>();
		//    merit = ObjectMapHelper<MeritEvaluationViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_ACR_Evaluation_Three_Marks", new object[] { empCode }));

		//    var profQ = new List<ProfessionalEvaluationViewModel>();
		//    profQ = ObjectMapHelper<ProfessionalEvaluationViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_ProfessionalQualificationSIXMarks", new object[] { empCode }));

		//    var jobLength = new List<JobLengthViewModel>();
		//    jobLength = ObjectMapHelper<JobLengthViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_JobLenth_TENMarks", new object[] { empCode }));

		//    var branchZoneFF = new List<BranchZonalHeadViewModel>();
		//    branchZoneFF = ObjectMapHelper<BranchZonalHeadViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_BranchHeadZonalHeadTowMarks", new object[] { empCode }));

		//    var model = new MarkingWorksheetReportViewModel
		//    {
		//        employeeInfoViewModel = lstEmp,
		//        last = education.FirstOrDefault().lastLevelEducationMarks,
		//        meritEvaluationViewModel = merit,
		//        professionalEvaluationViewModel = profQ,
		//        jobLengthViewModel = jobLength,
		//        branchZonalHeadViewModel = branchZoneFF
		//    };
		//    string designation = lstEmp.FirstOrDefault().DesignationName;

		//    string userName = HttpContext.User.Identity.Name;
		//    int? userTypeId = _context.Users.Where(x => x.UserName == userName).FirstOrDefault().userTypeId;

		//    var data = _context.Assessments.Find(assesmentId);
		//    if (data.statusInfoId == 1 && data.empCode == userName)
		//    {
		//        return new Rotativa.ViewAsPdf("MarkingWorksheetForAGMPdf", model);
		//    }
		//    else if (userTypeId == 1 || userTypeId == 2 || userTypeId == 8 || userTypeId == 10)
		//    {
		//        return new Rotativa.ViewAsPdf("MarkingWorksheetForAGMPdf", model);
		//    }
		//    //return new Rotativa.ViewAsPdf("AcrReportOfficerPdf", model);
		//    else
		//    {
		//        return View("UnauthorizedAccess");
		//    }
		//}

		//public ActionResult MarkingWorksheetForSPOPdf(int assesmentId, string empCode)
		//{
		//    var lstEmp = new List<EmployeeInfoViewModel>();
		//    lstEmp = ObjectMapHelper<EmployeeInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesByCode", new object[] { assesmentId }));

		//    var education = new List<EducationalEvaluationViewModel>();
		//    education = ObjectMapHelper<EducationalEvaluationViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEducationAbilityFiveMarks", new object[] { empCode }));

		//    var merit = new List<MeritEvaluationViewModel>();
		//    merit = ObjectMapHelper<MeritEvaluationViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_ACR_Evaluation_Three_Marks", new object[] { empCode }));

		//    var profQ = new List<ProfessionalEvaluationViewModel>();
		//    profQ = ObjectMapHelper<ProfessionalEvaluationViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_ProfessionalQualificationSIXMarks", new object[] { empCode }));

		//    var jobLength = new List<JobLengthViewModel>();
		//    jobLength = ObjectMapHelper<JobLengthViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_JobLenth_TENMarks", new object[] { empCode }));

		//    var branchZoneFF = new List<BranchZonalHeadViewModel>();
		//    branchZoneFF = ObjectMapHelper<BranchZonalHeadViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_BranchHeadZonalHeadTowMarks", new object[] { empCode }));

		//    var model = new MarkingWorksheetReportViewModel
		//    {
		//        employeeInfoViewModel = lstEmp,
		//        last = education.FirstOrDefault().lastLevelEducationMarks,
		//        meritEvaluationViewModel = merit,
		//        professionalEvaluationViewModel = profQ,
		//        jobLengthViewModel = jobLength,
		//        branchZonalHeadViewModel = branchZoneFF
		//    };
		//    string designation = lstEmp.FirstOrDefault().DesignationName;

		//    string userName = HttpContext.User.Identity.Name;
		//    int? userTypeId = _context.Users.Where(x => x.UserName == userName).FirstOrDefault().userTypeId;

		//    var data = _context.Assessments.Find(assesmentId);
		//    if (data.statusInfoId == 1 && data.empCode == userName)
		//    {
		//        return new Rotativa.ViewAsPdf("MarkingWorksheetForSPOPdf", model);
		//    }
		//    else if (userTypeId == 1 || userTypeId == 2 || userTypeId == 8 || userTypeId == 10)
		//    {
		//        return new Rotativa.ViewAsPdf("MarkingWorksheetForSPOPdf", model);
		//    }
		//    //return new Rotativa.ViewAsPdf("AcrReportOfficerPdf", model);
		//    else
		//    {
		//        return View("UnauthorizedAccess");
		//    }
		//}

		//public ActionResult MarkingWorksheetForPOPdf(int assesmentId, string empCode)
		//{
		//    var lstEmp = new List<EmployeeInfoViewModel>();
		//    lstEmp = ObjectMapHelper<EmployeeInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesByCode", new object[] { assesmentId }));

		//    var education = new List<EducationalEvaluationViewModel>();
		//    education = ObjectMapHelper<EducationalEvaluationViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEducationAbilityFiveMarks", new object[] { empCode }));

		//    var merit = new List<MeritEvaluationViewModel>();
		//    merit = ObjectMapHelper<MeritEvaluationViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_ACR_Evaluation_Three_Marks", new object[] { empCode }));

		//    var profQ = new List<ProfessionalEvaluationViewModel>();
		//    profQ = ObjectMapHelper<ProfessionalEvaluationViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_ProfessionalQualificationSIXMarks", new object[] { empCode }));

		//    var jobLength = new List<JobLengthViewModel>();
		//    jobLength = ObjectMapHelper<JobLengthViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_JobLenth_TENMarks", new object[] { empCode }));

		//    var branchZoneFF = new List<BranchZonalHeadViewModel>();
		//    branchZoneFF = ObjectMapHelper<BranchZonalHeadViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_BranchHeadZonalHeadTowMarks", new object[] { empCode }));

		//    var model = new MarkingWorksheetReportViewModel
		//    {
		//        employeeInfoViewModel = lstEmp,
		//        last = education.FirstOrDefault().lastLevelEducationMarks,
		//        meritEvaluationViewModel = merit,
		//        professionalEvaluationViewModel = profQ,
		//        jobLengthViewModel = jobLength,
		//        branchZonalHeadViewModel = branchZoneFF
		//    };
		//    string designation = lstEmp.FirstOrDefault().DesignationName;

		//    string userName = HttpContext.User.Identity.Name;
		//    int? userTypeId = _context.Users.Where(x => x.UserName == userName).FirstOrDefault().userTypeId;

		//    var data = _context.Assessments.Find(assesmentId);
		//    if (data.statusInfoId == 1 && data.empCode == userName)
		//    {
		//        return new Rotativa.ViewAsPdf("MarkingWorksheetForPOPdf", model);
		//    }
		//    else if (userTypeId == 1 || userTypeId == 2 || userTypeId == 8 || userTypeId == 10)
		//    {
		//        return new Rotativa.ViewAsPdf("MarkingWorksheetForPOPdf", model);
		//    }
		//    //return new Rotativa.ViewAsPdf("AcrReportOfficerPdf", model);
		//    else
		//    {
		//        return View("UnauthorizedAccess");
		//    }
		//}

		//public ActionResult MarkingWorksheetForSOPdf(int assesmentId, string empCode)
		//{
		//    var lstEmp = new List<EmployeeInfoViewModel>();
		//    lstEmp = ObjectMapHelper<EmployeeInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesByCode", new object[] { assesmentId }));

		//    var education = new List<EducationalEvaluationViewModel>();
		//    education = ObjectMapHelper<EducationalEvaluationViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEducationAbilityFiveMarks", new object[] { empCode }));

		//    var merit = new List<MeritEvaluationViewModel>();
		//    merit = ObjectMapHelper<MeritEvaluationViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_ACR_Evaluation_Three_Marks", new object[] { empCode }));

		//    var profQ = new List<ProfessionalEvaluationViewModel>();
		//    profQ = ObjectMapHelper<ProfessionalEvaluationViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_ProfessionalQualificationSIXMarks", new object[] { empCode }));

		//    var jobLength = new List<JobLengthViewModel>();
		//    jobLength = ObjectMapHelper<JobLengthViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_JobLenth_TENMarks", new object[] { empCode }));

		//    var branchZoneFF = new List<BranchZonalHeadViewModel>();
		//    branchZoneFF = ObjectMapHelper<BranchZonalHeadViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_BranchHeadZonalHeadTowMarks", new object[] { empCode }));

		//    var model = new MarkingWorksheetReportViewModel
		//    {
		//        employeeInfoViewModel = lstEmp,
		//        last = education.FirstOrDefault().lastLevelEducationMarks,
		//        meritEvaluationViewModel = merit,
		//        professionalEvaluationViewModel = profQ,
		//        jobLengthViewModel = jobLength,
		//        branchZonalHeadViewModel = branchZoneFF
		//    };
		//    string designation = lstEmp.FirstOrDefault().DesignationName;

		//    string userName = HttpContext.User.Identity.Name;
		//    int? userTypeId = _context.Users.Where(x => x.UserName == userName).FirstOrDefault().userTypeId;

		//    var data = _context.Assessments.Find(assesmentId);
		//    if (data.statusInfoId == 1 && data.empCode == userName)
		//    {
		//        return new Rotativa.ViewAsPdf("MarkingWorksheetForSOPdf", model);
		//    }
		//    else if (userTypeId == 1 || userTypeId == 2 || userTypeId == 8 || userTypeId == 10)
		//    {
		//        return new Rotativa.ViewAsPdf("MarkingWorksheetForSOPdf", model);
		//    }
		//    //return new Rotativa.ViewAsPdf("AcrReportOfficerPdf", model);
		//    else
		//    {
		//        return View("UnauthorizedAccess");
		//    }
		//}

		//public ActionResult MarkingWorksheetForOfficerPdf(int assesmentId, string empCode)
		//{
		//    var lstEmp = new List<EmployeeInfoViewModel>();
		//    lstEmp = ObjectMapHelper<EmployeeInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesByCode", new object[] { assesmentId }));

		//    var education = new List<EducationalEvaluationViewModel>();
		//    education = ObjectMapHelper<EducationalEvaluationViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEducationAbilityFiveMarks", new object[] { empCode }));

		//    var merit = new List<MeritEvaluationViewModel>();
		//    merit = ObjectMapHelper<MeritEvaluationViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_ACR_Evaluation_Three_Marks", new object[] { empCode }));

		//    var profQ = new List<ProfessionalEvaluationViewModel>();
		//    profQ = ObjectMapHelper<ProfessionalEvaluationViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_ProfessionalQualificationSIXMarks", new object[] { empCode }));

		//    var jobLength = new List<JobLengthViewModel>();
		//    jobLength = ObjectMapHelper<JobLengthViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_JobLenth_TENMarks", new object[] { empCode }));

		//    var branchZoneFF = new List<BranchZonalHeadViewModel>();
		//    branchZoneFF = ObjectMapHelper<BranchZonalHeadViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_BranchHeadZonalHeadTowMarks", new object[] { empCode }));

		//    var model = new MarkingWorksheetReportViewModel
		//    {
		//        employeeInfoViewModel = lstEmp,
		//        last = education.FirstOrDefault().lastLevelEducationMarks,
		//        meritEvaluationViewModel = merit,
		//        professionalEvaluationViewModel = profQ,
		//        jobLengthViewModel = jobLength,
		//        branchZonalHeadViewModel = branchZoneFF
		//    };
		//    string designation = lstEmp.FirstOrDefault().DesignationName;

		//    string userName = HttpContext.User.Identity.Name;
		//    int? userTypeId = _context.Users.Where(x => x.UserName == userName).FirstOrDefault().userTypeId;

		//    var data = _context.Assessments.Find(assesmentId);
		//    if (data.statusInfoId == 1 && data.empCode == userName)
		//    {
		//        return new Rotativa.ViewAsPdf("MarkingWorksheetForOfficerPdf", model);
		//    }
		//    else if (userTypeId == 1 || userTypeId == 2 || userTypeId == 8 || userTypeId == 10)
		//    {
		//        return new Rotativa.ViewAsPdf("MarkingWorksheetForOfficerPdf", model);
		//    }
		//    //return new Rotativa.ViewAsPdf("AcrReportOfficerPdf", model);
		//    else
		//    {
		//        return View("UnauthorizedAccess");
		//    }
		//}

		//#endregion
	}
}