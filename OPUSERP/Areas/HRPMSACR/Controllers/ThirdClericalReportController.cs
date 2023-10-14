using ACR.Data;
using ACR.Data.ViewModel.Evaluation;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OPUSERP.Areas.HRPMSACR.Models;
using OPUSERP.Data;
using OPUSERP.Data.Entity;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.ACR;
using OPUSERP.HRPMS.Services.ACR.Interfaces;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web;

namespace OPUSERP.Areas.HRPMSACR.Controllers
{
    [Authorize]
    [Area("HRPMSACR")]
    public class ThirdClericalReportController : Controller
    {
        private readonly ERPDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAcrInfoService acrInfoService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly IUserInfoes userInfoes;
        private readonly IERPCompanyService eRPCompanyService;
        private readonly string rootPath;
        private readonly MyPDF myPDF;

        // GET: Clerical
        public ThirdClericalReportController(ERPDbContext context, IERPCompanyService eRPCompanyService, IUserInfoes userInfoes, UserManager<ApplicationUser> userManager, IAcrInfoService acrInfoService, IPersonalInfoService personalInfoService, IHostingEnvironment hostingEnvironment, IConverter converter)
        {
            this._context = context;
            _userManager = userManager;
            this.acrInfoService = acrInfoService;
            this.personalInfoService = personalInfoService;
            this.userInfoes = userInfoes;
            this.eRPCompanyService = eRPCompanyService;
            myPDF = new MyPDF(hostingEnvironment, converter);
            rootPath = hostingEnvironment.ContentRootPath;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ClericalReportPdfView(int assessmentId)
        {

            var empInfo = await acrInfoService.GetEmployeeInfo(assessmentId);
            var emp = await personalInfoService.GetEmployeeUserInfoByCodeBySp(empInfo.FirstOrDefault().EmpCode);
            var signature = await personalInfoService.GetSignatureByEmpId(Convert.ToInt32(emp.EmpId));

            ReportViewModel model = new ReportViewModel
            {
                employeeInfoViewModel = await acrInfoService.GetEmployeeInfo(assessmentId),
                employeesAcrsViewModel = await acrInfoService.GetEmployeeAcrInfoForClerks(assessmentId),
                //employeeTrainingViewModel = await acrInfoService.GetEmployeeTrainingInfo(assessmentId),
                acrOtherTableViewModel = await acrInfoService.GetAcrOtherTablesReport(assessmentId)
            };
            return View(model);

            //ReportViewModel model = new ReportViewModel
            //{
            //    employeeInfoViewModel = empInfo,
            //    EmployeeAcrInfo = emp,
            //    //employeesAcrsViewModelN = await acrInfoService.GetEmployeeAcrInfoForOfficersNew(assessmentId),
            //    quantitativeEvaluationsViewModel = await acrInfoService.GetQuantativeEvaluation(assessmentId),
            //    employeeEducationInfoViewModel = await acrInfoService.GetEmployeeEducation(assessmentId),
            //    //employeeTrainingViewModel = await acrInfoService.GetEmployeeTrainingInfo(assessmentId),
            //    employeePromotionViewModel = await acrInfoService.GetEmployeePromotion(assessmentId),
            //    employeeAssignmentViewModel = await acrInfoService.GetEmployeesAssignment(assessmentId),
            //    employeeLeaveInfoViewModels = await acrInfoService.GetEmployeesLeave(assessmentId),
            //    aCRRecommendation = await acrInfoService.GetACRRecommendationByAssId(assessmentId),
            //    document = _context.docAttachment.Where(x => x.assesmentId == assessmentId && x.attachFor == "TargetAchive").FirstOrDefault(),
            //    //evaluatedMarks = total,
            //    empEducations = await acrInfoService.GetEmpEducations(Convert.ToInt32(emp.EmpId)),
            //    empPostings = await acrInfoService.GetEmpPostings(Convert.ToInt32(emp.EmpId)),
            //    //document = _context.docAttachment.Where(x => x.assesmentId == assessmentId && x.attachFor == "TargetAchive").FirstOrDefault()
            //};
            //return View(model);


        }
        
        [AllowAnonymous]

        public IActionResult ClericalReportPdf(int? assessmentId)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName = "";
            var url = scheme + "://" + host + "/HRPMSACR/ThirdClericalReport/ClericalReportPdfView?assessmentId=" + assessmentId;
            var status = myPDF.GeneratePDF(out fileName, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }
  


        public async Task<ActionResult> GetAssessmentBasicInfo(int? assId)
        {
            string userName = User.Identity.Name;
            var lstEmp = await acrInfoService.AssessmentInfoById(userName, Convert.ToInt32(assId));

            return Json(lstEmp);
        }

     

      
        [HttpGet]
        public JsonResult GetACRSignatories(int? id)
        {
            var result = (from C in _context.aCRSignatories
                          where C.assessmentId == id
                          select new ClerkPart2CommentViewModel
                          {
                              additionalComment = C.additionalComment
                          }).ToList();
            return Json(result.FirstOrDefault());
        }
        [HttpGet]
        public JsonResult GetACRComments(int? id)
        {
            var result = (from C in _context.aCRComments
                          where C.assessmentId == id
                          select new ClerkPart2CommentViewModel
                          {
                              remarks = C.remarks,
                              specificJob = C.specificJob

                          }).ToList();
            return Json(result.FirstOrDefault());
        }

        [HttpGet]
        public JsonResult GetACROtherTables(int? id)
        {
            var result = (from C in _context.aCROtherTables
                          where C.assessmentId == id
                          select new ACROtherTableViewModel
                          {
                              isPhysicallyCapable = C.isPhysicallyCapable,
                              bankingExperience = C.bankingExperience,
                              nobankingExperience = C.nobankingExperience
                          }).ToList();
            return Json(result.FirstOrDefault());
        }

        public async Task<IActionResult> GetUserInfo(string userName)
        {
            var userInfo = await personalInfoService.GetEmployeeInfoByCode(userName);
            return Json(userInfo);
        }

        [HttpGet]
        public JsonResult GetClericalEmployeeACRDetailsInfo(int? id, int? acrTypeId)
        {
            var marks = _context.employeesAcrs.Where(x => x.assessmentId == id).Sum(x => (x.firstValue == 2 ? 0 : x.firstValue * 2) + (x.sencondValue == 2 ? 0 : x.sencondValue * 2) + (x.thirdValue == 2 ? 0 : x.thirdValue * 4) + (x.forthValue == 2 ? 0 : x.forthValue * 5));
            var amountInWord = AmountInWord.ConvertWholeNumber(marks.ToString());
            var result = (from e in _context.employeesAcrs
                          join a in _context.acrEvaluationNames on e.acrEvaluationNameId equals a.Id
                          where e.assessmentId == id && a.acrTypeId == acrTypeId
                          select new ACRDetailsInfoViewModel
                          {
                              acrId = e.Id,
                              empCode = e.empCode,
                              acrEvaluationNameId = e.acrEvaluationNameId,
                              firstValue = e.firstValue,
                              sencondValue = e.sencondValue,
                              thirdValue = e.thirdValue,
                              forthValue = e.forthValue,
                              marks = marks.ToString(),
                              marksInWord = amountInWord
                          }).ToList();
            return Json(result);
            //return Json(result, JsonRequestBehavior.AllowGet);
        }

       

        [HttpGet]
        public async Task<IActionResult> GetUsersProfile(int id)
        {
           
            var lstUser = await acrInfoService.GetUserProfile(id);
            return Json(lstUser.FirstOrDefault());
        }
       
        [HttpGet]
        public async Task<IActionResult> GetClericalEmployeeInfo(int id)
        {
            var lstClericalEmp = await acrInfoService.GetEmployeeInfo(id);
            return Json(lstClericalEmp.FirstOrDefault());
        }


        [AllowAnonymous]
        //public IActionResult Clerical4Th_ReportPdf()
        //{
        //    string scheme = Request.Scheme;
        //    var host = Request.Host;
        //    string fileName = "";
        //    var url = scheme + "://" + host + "/HRPMSACR/ThirdClericalReport/Clerical_4Th_ReportPdfView";
        //    // url = scheme + "://" + host + "/HRPMSACR/ThirdClericalReport/ClericalReportPdfView?joiningDate=" + joiningDate;
        //    var status = myPDF.GeneratePDF(out fileName, url);
        //    if (status != "done")
        //    {
        //        return Content("<h1>Something Went Wrong</h1>");
        //    }
        //    var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
        //    return new FileStreamResult(stream, "application/pdf");

        //}
        public IActionResult Clerical4Th_ReportPdf(int? assessmentId)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName = "";
            var url = scheme + "://" + host + "/HRPMSACR/ThirdClericalReport/Clerical_4Th_ReportPdfView?assessmentId=" + assessmentId;
            var status = myPDF.GeneratePDF(out fileName, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }

        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> Clerical_4Th_ReportPdfView()
        //{
        //    ACREvaluationViewModel model = new ACREvaluationViewModel
        //    {
        //        AcrEvaluationList = await acrInfoService.AcrEvaluationNameByacrForId(2),
        //        LeaveTypes = await acrInfoService.GetAllLeaveTypes(),
        //        heads = await acrInfoService.GetAllQuantitativeEvaluationHeads(),
        //        commentsHead = await acrInfoService.GetAllEvaluationCommentsHeads(),
        //        aCRIndicators = await acrInfoService.GetAllAcrIndicators(),
        //    };


        //    var host = Dns.GetHostEntry(Dns.GetHostName());
        //    var ipaddress = "";
        //    foreach (var ip in host.AddressList)
        //    {
        //        if (ip.AddressFamily == AddressFamily.InterNetwork)
        //        {
        //            ipaddress = ip.ToString();
        //        }
        //    }


        //    return View();
        //}
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Clerical_4Th_ReportPdfView(int assessmentId)
        {

            var empInfo = await acrInfoService.GetEmployeeInfo(assessmentId);
            var emp = await personalInfoService.GetEmployeeUserInfoByCodeBySp(empInfo.FirstOrDefault().EmpCode);
            var signature = await personalInfoService.GetSignatureByEmpId(Convert.ToInt32(emp.EmpId));

            //thirdClaricalReportViewModel model = new thirdClaricalReportViewModel
            //{
            //    companies = await eRPCompanyService.GetAllCompany() 
            //};


            ReportViewModel model = new ReportViewModel
            {
                employeeInfoViewModel = empInfo,
                EmployeeAcrInfo = emp,
                //employeesAcrsViewModelN = await acrInfoService.GetEmployeeAcrInfoForOfficersNew(assessmentId),
                quantitativeEvaluationsViewModel = await acrInfoService.GetQuantativeEvaluation(assessmentId),
                employeeEducationInfoViewModel = await acrInfoService.GetEmployeeEducation(assessmentId),
                //employeeTrainingViewModel = await acrInfoService.GetEmployeeTrainingInfo(assessmentId),
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


    }
}