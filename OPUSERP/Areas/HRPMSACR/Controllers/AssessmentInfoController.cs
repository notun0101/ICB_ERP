using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Accounting.Data.Entity.MasterData;
using OPUSERP.Accounting.Services.MasterData.Interfaces;
using OPUSERP.Areas.HRPMSACR.Models;
using OPUSERP.Areas.HRPMSLeave.Models;
using OPUSERP.Data.Entity;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.ACR;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Services.ACR.Interfaces;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.Leave;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.Utility.Models;

namespace OPUSERP.Areas.HRPMSACR.Controllers
{
    [Area("HRPMSACR")]
    [Authorize]
    public class AssessmentInfoController : Controller
    {
        private readonly IPersonalInfoService personalInfoService;
        private readonly IPhotographService photographService;
        private readonly ISignatureService signatureService;
        private readonly IAcrInfoService acrInfoService;
        private readonly IUserInfoes userInfoes;
        private readonly IEmployeePunchCardInfoService employeePunchCardInfoService;
        private readonly UserManager<ApplicationUser> _userManager;
       

        public AssessmentInfoController(
            IPersonalInfoService personalInfoService, 
            IPhotographService photographService,
            ISignatureService signatureService,
            IAcrInfoService acrInfoService,
            IUserInfoes userInfoes,
            IEmployeePunchCardInfoService employeePunchCardInfoService,
            UserManager<ApplicationUser> _userManager)
        {
            this.personalInfoService = personalInfoService;
            this.photographService = photographService;
            this.signatureService = signatureService;
            this.acrInfoService = acrInfoService;
            this.userInfoes = userInfoes;
            this.employeePunchCardInfoService = employeePunchCardInfoService;
            this._userManager = _userManager;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            string userName = HttpContext.User.Identity.Name;

            var userInfo = await _userManager.FindByNameAsync(userName);
            var emp = await personalInfoService.GetEmployeeUserInfoByCodeBySp(userName);
            var roles = await personalInfoService.GetRolesByUserId(userInfo.Id);

            //var assessments = await acrInfoService.GetAssessmentByEmployeeCode(userName);



            int? empId = emp.EmpId;
            ACRAssessmentVM model = new ACRAssessmentVM
            {
                empPostings = await acrInfoService.GetEmpPostings((int)empId, 0),
                AddAssessments = await acrInfoService.GetAssessmentByEmployeeCode(userName),

        };

            return View(model);
        }
 
            #region Authority

            [HttpGet]
        public async Task<ActionResult> Authority()
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;

                var userInfo = await _userManager.FindByNameAsync(userName);
                var emp = await personalInfoService.GetEmployeeUserInfoByCodeBySp(userName);
                var roles = await personalInfoService.GetRolesByUserId(userInfo.Id);
                int? empId = emp.EmpId;
                ViewBag.UserEmpCode = emp.EmpCode;
                //string roleId = roles.Contains("");
                //string roleId = roles.FirstOrDefault().Id==null|| roles.FirstOrDefault().Id == "" ? "2441663d-fed0-4528-b4af-14a9e5d06831": roles.FirstOrDefault().Id;

                var lstAssessment = await acrInfoService.GetLastAssessmentByUserName(userName);

                ACRAssessmentVM model = new ACRAssessmentVM
                {
                    ACRUsers = userInfo,
                    AssessmentBasic = lstAssessment.FirstOrDefault(),
                    empPostings = await acrInfoService.GetEmpPostings((int)empId, 0),
                };

                var host = Dns.GetHostEntry(Dns.GetHostName());
                var ipaddress = "";
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipaddress = ip.ToString();
                    }
                }

                ViewBag.BaseUrl = ipaddress;
                return View(model);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAcrEmployeeDate(string ecode, string atype, DateTime afromDate, DateTime atoDate, string ayear)
        {
            if (atype== "Yearly")
            {
                var data = await acrInfoService.GetAcrEmployeeInformationByYear(ecode, atype, ayear);
                return Json(data);

            }
            else
            {
                var data = await acrInfoService.GetAcrEmployeeInformation(ecode, atype, afromDate, atoDate, ayear);
                return Json(data);
            }
        }



        


        //[HttpGet]
        //public JsonResult GetUsersNextProfile(string empcode)
        //{
        //try
        //{
        //    var saveJob = ObjectMapHelper<EmployeesJobDurationViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_NextUserInfo", new object[] { empcode }));

        //    return Json(saveJob.FirstOrDefault(), JsonRequestBehavior.AllowGet);
        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}

        //}

        [HttpGet]
        public async Task<ActionResult> GetAssessmentInfo(string year)
        {
            string userName = HttpContext.User.Identity.Name;
            //List<AssessmentViewModel> lstRecom = new List<AssessmentViewModel>();
            var lstRecom = await acrInfoService.GetAssessmentInfoForRecom(userName, "recome", year);


            //ViewBag.BaseUrl = DomainIP.GetIPAddress();
            return Json(lstRecom);
        }
		public async Task<IActionResult> GetMyPendingAcrs() {
			string userName = HttpContext.User.Identity.Name;
			var data = new AllAcrsViewModel
			{
				allAcrSps = await acrInfoService.GetAllMyAcrs(userName)
			};
			return View(data);
		}
		[HttpGet]
		public async Task<ActionResult> GetAssessmentInfo_Sign(string year)
		{
			IEnumerable<AssessmentInfoViewModel> lstSign;
			string userName = HttpContext.User.Identity.Name;

			lstSign = await acrInfoService.GetAssessmentInfoForSignatory(userName, "signatory", year);

			//lstSign = ObjectMapHelper<AssessmentViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetAssessmentInfoForSignatory", new object[] { userName, "signatory", year }));
			var host = Dns.GetHostEntry(Dns.GetHostName());
			var ipaddress = "";
			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
					ipaddress = ip.ToString();
				}
			}
			ViewBag.BaseUrl = ipaddress;
			return Json(lstSign);
		}
        [HttpGet]
        public async Task<ActionResult> GetAssessmentInfo_FSign(string year)
        {
            IEnumerable<AssessmentInfoViewModel> lstSign;
            string userName = HttpContext.User.Identity.Name;

            lstSign = await acrInfoService.GetAssessmentInfoForSignatory(userName, "signatory2", year);

            //lstSign = ObjectMapHelper<AssessmentViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetAssessmentInfoForSignatory", new object[] { userName, "signatory", year }));
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ipaddress = "";
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipaddress = ip.ToString();
                }
            }
            ViewBag.BaseUrl = ipaddress;
            return Json(lstSign);
        }

        [HttpGet]
		public async Task<ActionResult> GetAssessmentInfo_Final(string year)
		{
			string userName = HttpContext.User.Identity.Name;
			//List<AssessmentViewModel> lstFinal = new List<AssessmentViewModel>();
			var lstFinal = await acrInfoService.GetAssessmentInfoForSignatoryNew(userName, "final", year);
			var host = Dns.GetHostEntry(Dns.GetHostName());
			var ipaddress = "";
			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
					ipaddress = ip.ToString();
				}
			}
			ViewBag.BaseUrl = ipaddress;
			return Json(lstFinal);
		}


        #endregion

        [HttpGet]
        
        public async Task<IActionResult> ValidAcrDate(string acrYear, string acrType, string fromDate, string toDate)
        {
            string userName = HttpContext.User.Identity.Name;
            var result = await acrInfoService.ValidAcrDate(userName, acrYear, acrType, fromDate, toDate);

            string status = string.Empty;
            string message = string.Empty;

            if (result.Message == "OK")
            {
                status = "OK";
                message = "";
            }
            else
            {
                status = "NOK";
                message = result.Message;
            }

            return new OkObjectResult(new ResponseObject { Status = status, Message = message });

            //return Json(lstEmp);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersProfile(string toDate)
        {                        
            string userName = HttpContext.User.Identity.Name;
            var lstEmp = await acrInfoService.GetUserProfileByUserName(userName, toDate);
            return Json(lstEmp);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUsersProfile(string signaturePath, string imagePath, string empcode, string jbDura, string jbBranch, string jbZonal, string jbOthers, string type, string fromDate, string toDate, string year)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                var employee = await employeePunchCardInfoService.GetEmployeeByUsername(userName);
                var signature = await photographService.GetEmployeeSignatureByEmpId(employee.Id);

				//if (signaturePath != null)
				//{
				//	if (signature != null)
				//	{
				//		signature.url = signaturePath;
				//		await photographService.SaveEmployeeSignature(signature);
				//	}
				//	else
				//	{
				//		var data = new EmployeeSignature
				//		{
				//			Id = 0,
				//			url = signaturePath,
				//			employeeId = employee.Id
				//		};
				//		await photographService.SaveEmployeeSignature(data);
				//	}
				//}
				//else
				//{
				//	signaturePath = signature.url;
				//}

    //            var photo = await photographService.GetPhotographByEmpIdAndType(employee.Id, "profile");
				//if (imagePath != null)
				//{
				//	if (photo != null)
				//	{
				//		photo.url = "EmpImages\\" + imagePath;
				//		await photographService.SavePhotograph(photo);
				//	}
				//	else
				//	{
				//		var data = new Photograph
				//		{
				//			Id = 0,
				//			url = imagePath,
				//			employeeId = employee.Id,
				//			type = "Profile"
				//		};
				//		await photographService.SavePhotograph(data);
				//	}
				//}
				//else
				//{
				//	imagePath = photo.url;
				//}

                var job = await acrInfoService.SaveEmployeesJobDurations(signaturePath, imagePath, empcode, jbDura, jbBranch, jbZonal, jbOthers,type, fromDate, toDate, year);
                //var ff = ObjectMapHelper<ApplicationUser>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_Update_UserSignature", new object[] { userName, signaturePath, imagePath }));

                //var saveJob = ObjectMapHelper<EmployeesJobDurationViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_Save_EmployeesJobDurations", new object[] { empcode, jbDura, jbBranch, jbZonal, jbOthers }));

                return Json(job.FirstOrDefault());
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public async Task<ActionResult> UploadImageFile(string ID)
        {
            try
            {

                //string ID = Request.Form.Keys;
                List<ApplicationUser> lstshareinfo = new List<ApplicationUser>();
                ApplicationUser shareinfo = new ApplicationUser();

                //if (Request.Files.Count > 0)
                //{
                var files = Request.Form.Files;

                string fileName;
                string message = FileSave.SaveImage(out fileName, files[0]);

                var data = await photographService.GetEmployeeSignatureByEmpCode(ID);
                if (message == "success")
                {
                    data.url = fileName;
                }
                await photographService.SaveEmployeeSignature(data);
            }
            catch (Exception ex)
            {
                return Json("Error occurred. Error details: " + ex.Message);
            }
            return Json("File Uploaded Successfully!");
        }
        [HttpPost]
        public async Task<ActionResult> UploadPhotoFile(string ID)
        {
            try
            {

                //string ID = Request.Form.Keys;
                List<ApplicationUser> lstshareinfo = new List<ApplicationUser>();
                ApplicationUser shareinfo = new ApplicationUser();

                //if (Request.Files.Count > 0)
                //{
                var files = Request.Form.Files;

                string fileName;
                string message = FileSave.SaveImage(out fileName, files[0]);

                var data = await photographService.GetPhotographByEmpCodeAndType(ID, "profile");
                if (message == "success")
                {
                    data.url = fileName;
                }
                await photographService.SavePhotograph(data);
            }
            catch (Exception ex)
            {
                return Json("Error occurred. Error details: " + ex.Message);
            }
            return Json("File Uploaded Successfully!");
        }

        [HttpGet]
        public async Task<IActionResult> SaveHRMInfo(int assId, DateTime receivingdate,string empCode, string reason, string actionOn)
        {
            var d = acrInfoService.UpdateACRHRMData(assId, receivingdate,empCode, reason, actionOn);

            //return RedirectToAction(nameof(AcrCompletedList));
            return  Json("ok");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateSignatory(int? assId, string newSignatory) {
            var assessment = await acrInfoService.GetAssessmentInfoById(Convert.ToInt32(assId));

            assessment.newSignatory = newSignatory;

            await acrInfoService.SaveAssessment(assessment);

            return Json("saved");
        }


        #region Acr Completed List
        public async Task<IActionResult> AcrCompletedList()
        {
            string userName = HttpContext.User.Identity.Name;

            var userInfo = await _userManager.FindByNameAsync(userName);
            var emp = await personalInfoService.GetEmployeeUserInfoByCodeBySp(userName);
            var roles = await personalInfoService.GetRolesByUserId(userInfo.Id);
            int? empId = emp.EmpId;
            ViewBag.UserEmpCode = emp.EmpCode;
            //List<AssessmentInfoViewModel> lstFinal = new List<AssessmentInfoViewModel>();
            //lstFinal = ObjectMapHelper<AssessmentViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetAssessmentInfoForSignatory", new object[] { userName, "final", "2020" }));
            var lstFinal = await acrInfoService.GetAssessmentInfoForSignatory(userName, "final", "2020");

            ACRAssessmentVM model = new ACRAssessmentVM
            {
                ACRUsers = userInfo,
                lstFinal = lstFinal,
            };
            return View(model);
        }

		public async Task<IActionResult> ACRPendingListForHRAdmin()
		{
			var model = new ACRAssessmentVM
			{
				AllEmployees = await personalInfoService.GetAllActiveEmployees(),
				assessments = await acrInfoService.ACRPendingListForHRAdmin()
			};
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> SaveAcrAssaign(ACRAssaignVm model)
		{
			var assId = new NotificationVmAcr();
			for (int i = 0; i < model.assId.Length; i++)
			{
				assId = await acrInfoService.AssaignAcrApprover(Convert.ToInt32(model.assId[i]), model.recommendator[i], model.signatory[i], model.signatory2[i]);
			}
			return Json(1);
		}

        [HttpPost]
        public async Task<IActionResult> SaveAcrAssaignSingle(int assId, string recom, string signatory, string signatory2)
        {

            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);

            var ass = await acrInfoService.AssaignAcrApprover(assId, recom, signatory, signatory2);

            var userDetails = await personalInfoService.GetEmployeeInfoByCode(userName);
            await acrInfoService.SendNotification(userName, userName, recom, "ACR From " + userDetails.nameEnglish, "Waiting for ACR to recommendator", "ACR", "/HRPMSACR/AssessmentInfo/Authority");

            return Json(1);
        }


         [HttpPost]
        public async Task<IActionResult> SaveAcrAssaignCancel(int assId)
        {

            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);

            var ass = await acrInfoService.AssaignAcrcancel(assId);
            ass.statusInfoId = 8;
            var id = await acrInfoService.SaveAssessment(ass);
            //  var userDetails = await personalInfoService.GetEmployeeInfoByCode(userName);
            //  await acrInfoService.SendNotification(userName, userName, recom, "ACR From " + userDetails.nameEnglish, "Waiting for ACR to recommendator", "ACR", "/HRPMSACR/AssessmentInfo/Authority");

            return Json(id);
        }




        [HttpGet]
        public async Task<IActionResult> GetAssessmentById(int assId)
        {
            var ass = await acrInfoService.GetAssessmentInfoById(assId);

            return Json(ass);
        }



        #endregion
    }
}