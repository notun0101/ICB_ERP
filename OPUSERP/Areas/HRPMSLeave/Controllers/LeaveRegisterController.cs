using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSAttendence.Models;
using OPUSERP.Areas.HRPMSLeave.Models;
using OPUSERP.Areas.HRPMSLeave.Models.Lang;
using OPUSERP.Areas.HRPMSReport.Models;
using OPUSERP.Data.Entity;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.ERPServices.AuthService;
using OPUSERP.ERPServices.AuthService.Interfaces;
using OPUSERP.ERPServices.EmailService.interfaces;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.Leave;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.Attendance;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.Leave.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.Payroll.Services.Salary.Interfaces;
using OPUSERP.PushNotification.Models;
using OPUSERP.PushNotification.Services;
using OPUSERP.SCM.Data.Entity.Requisition;
using OPUSERP.SCM.Services.Requisition.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSLeave.Controllers
{
    [Authorize]
    [Area("HRPMSLeave")]
    public class LeaveRegisterController : Controller
    {
        private readonly IRequisitionService requisitionService;
        private readonly LangGenerate<LeaveLn> _lang;
        // GET: LeaveRegister
        private readonly ILeaveRegisterService leaveRegisterService;
        private readonly IWagesLeaveRegisterService wagesLeaveRegisterService;
        private readonly ILeaveTypeService leaveTypeService;
        private readonly ILeaveRouteService leaveRouteService;
        private readonly IUserInfoes userInfo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPersonalInfoService personalInfoService;
        private readonly ISupervisorService supervisorService;
        private readonly ILeaveStatusLogService leaveStatusLogService;
        private readonly IEmailSenderService emailSenderService;
        private readonly ILeavePolicyService leavePolicyService;
        private readonly IApprovalService approvalService;
        private readonly IYearCourseTitleService yearCourseTitleService;
        private readonly ISalaryService salaryService;
        private readonly IERPCompanyService eRPCompanyService;
        private readonly IOrganizationService organizationService;
        private readonly INotificationService pushNotificationService;
        private readonly IDbChangeService dbChangeService;
		//Asad added on 25.06.2023
		private readonly IEmployeePunchCardInfoService employeePunchCardInfoService;

		private readonly string rootPath;
        private readonly MyPDF myPDF;

        public LeaveRegisterController(IHostingEnvironment hostingEnvironment,IOrganizationService organizationService, IRequisitionService requisitionService, UserManager<ApplicationUser> _userManager, IERPCompanyService eRPCompanyService, IConverter converter, ILeaveRegisterService leaveRegisterService, ILeaveTypeService leaveTypeService, ILeaveRouteService leaveRouteService, IUserInfoes userInfo, IPersonalInfoService personalInfoService, ISupervisorService supervisorService, ILeaveStatusLogService leaveStatusLogService, IEmailSenderService emailSenderService, ILeavePolicyService leavePolicyService, IApprovalService approvalService, IYearCourseTitleService yearCourseTitleService, IWagesLeaveRegisterService wagesLeaveRegisterService, ISalaryService salaryService, INotificationService pushNotificationService, IDbChangeService dbChangeService, IEmployeePunchCardInfoService employeePunchCardInfoService)
        {
            _lang = new LangGenerate<LeaveLn>(hostingEnvironment.ContentRootPath);
            this.leaveRegisterService = leaveRegisterService;
            this.leaveTypeService = leaveTypeService;
            this.leaveRouteService = leaveRouteService;
            this.userInfo = userInfo;
            this.personalInfoService = personalInfoService;
            this.supervisorService = supervisorService;
            this.leaveStatusLogService = leaveStatusLogService;
            this.requisitionService = requisitionService;
            this.emailSenderService = emailSenderService;
            this.leavePolicyService = leavePolicyService;
            this.approvalService = approvalService;
            this.yearCourseTitleService = yearCourseTitleService;
            this.wagesLeaveRegisterService = wagesLeaveRegisterService;
            this.eRPCompanyService = eRPCompanyService;
            this.salaryService = salaryService;
            this._userManager = _userManager;
            this.organizationService = organizationService;
            this.pushNotificationService = pushNotificationService;
            this.dbChangeService = dbChangeService;
            this.employeePunchCardInfoService = employeePunchCardInfoService;
			//For PDF
			myPDF = new MyPDF(hostingEnvironment, converter);
            rootPath = hostingEnvironment.ContentRootPath;
        }

        #region Leave Apply
        public async Task<IActionResult> GetLeaveDates()
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpId(EmpInfo.Id)
            };
            return Json(model);
        }
        public async Task<IActionResult> GetPendingLeaveDates()
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                leaveRegisterslist = await leaveRegisterService.GetAllPendingLeaveByEmpId(EmpInfo.Id)
            };
            return Json(model);
        }
        public async Task<IActionResult> GetLeaveDatesByEmpId(int id)
        {
            var userInfos = await userInfo.getUserByEmployeeId(id);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpId(EmpInfo.Id)
            };
            return Json(model);
        }

        public async Task<IActionResult> GetLeaveDatesAll()
        {
            //string userName = HttpContext.User.Identity.Name;
            //var userInfos = await userInfo.GetUserInfoByUser(userName);
            //var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegister()
            };
            return Json(model);
        }
        public async Task<IActionResult> GetAllLeaveByEmpCode(string id)
        {
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                leaveRegisterslist = await leaveRegisterService.GetAllLeaveByEmpCode(id)
            };
            return Json(model);
        }

        public async Task<IActionResult> LeaveApply()
        {

            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            int empId = 0;
            if (EmpInfo != null)
            {
                empId = EmpInfo.Id;
            }
            ViewBag.employeeId = empId;
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpIdLeaveApply(empId),
                leaveTypelist = await leaveTypeService.GetAllLeaveType(),
                //leaveTypelist = await leaveTypeService.GetAllLeaveTypeBySP(empId),
                //supervisor = await supervisorService.GetFirstSupervisorByEmpId(empId),
                approvalDetail = await approvalService.GetSingleApprovalDetailByEmpAndType(empId, "Leave"),
                leaveDays = await leavePolicyService.GetAllLeaveDay(),
                leaveStatusViewModels = await leavePolicyService.GetLeaveStatus(empId),
                approvalDetails = await leavePolicyService.GetApprovalDetailsByUser(userInfos.UserId)
                //leaveOpeningBalances = await leaveOpeningBalanceService.GetLeaveOpeningBalanceByEmpCode(employeeId)
            };
            return View(model);
        }

        public async Task<IActionResult> EditLeave(int id)
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            int empId = 0;
            if (EmpInfo != null)
            {
                empId = EmpInfo.Id;
            }
            ViewBag.employeeId = empId;
            var appDetails = await leavePolicyService.GetApprovalDetailsByUser(userInfos.UserId);
            var appDetailsWithLog = new List<AppDetailsWithLog>();

            foreach (var item in appDetails)
            {
                var data = new AppDetailsWithLog
                {
                    approvalDetail = item,
                    regId = id,
                    leaveStatusLog = await leaveRegisterService.GetStatusLogByAppUserName(item.approver.ApplicationUser.UserName, item.approvalMaster.Id, id)
                };
                appDetailsWithLog.Add(data);
            }


            var leaveRegList = await leaveRegisterService.GetLeaveRegisterById(id);
            DateTime leaveYear = Convert.ToDateTime(leaveRegList.leaveFrom);
            var year = leaveYear.Year;
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpId(empId),
                leaveTypelist = await leaveTypeService.GetAllLeaveType(),
                approvalDetail = await approvalService.GetSingleApprovalDetailByEmpAndType(empId, "Leave"),
                leaveDays = await leavePolicyService.GetAllLeaveDay(),
                leaveStatusViewModels = await leavePolicyService.GetLeaveStatus(empId),
                approvalDetails = await leavePolicyService.GetApprovalDetailsByUser(userInfos.UserId),
                appDetailsWithLogs = appDetailsWithLog,
                companies = await eRPCompanyService.GetAllCompany(),
                leaveRegister = await leaveRegisterService.GetLeaveRegisterById(id),
                leaveReportModels = await leaveRegisterService.GetLeaveReport(year, EmpInfo.Id),
                leaveSupervisorRecomViewModels = await leaveRegisterService.GetSupervisorRecomForLeaveByEmpId(id, EmpInfo.Id),
                visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
                leaveStatusLogs = await leaveRegisterService.GetLeaveStatusLogByRegId(id)
            };
            return View(model);
        }

        public async Task<IActionResult> GetEmployessPhotoById(int id)
        {
            var data = await leaveRegisterService.GetPhotoByEmpId(id);
            return Json(data);
        }

        public async Task<IActionResult> LeaveStatus()
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            int empId = 0;
            if (EmpInfo != null)
            {
                empId = EmpInfo.Id;
            }
            ViewBag.employeeId = empId;

            //var appDetails = await leavePolicyService.GetApprovalDetailsByUser(userInfos.UserId);
            //var appDetailsWithLog = new List<AppDetailsWithLog>();

            //foreach (var item in appDetails)
            //{
            //	var data = new AppDetailsWithLog
            //	{
            //		approvalDetail = item,
            //		regId = id,
            //		leaveStatusLog = await leaveRegisterService.GetStatusLogByAppUserName(item.approver.ApplicationUser.UserName, item.approvalMaster.Id, id)
            //	};
            //	appDetailsWithLog.Add(data);
            //}

            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpId(empId),
                leaveTypelist = await leaveTypeService.GetAllLeaveType(),
                //leaveTypelist = await leaveTypeService.GetAllLeaveTypeBySP(empId),
                //supervisor = await supervisorService.GetFirstSupervisorByEmpId(empId),
                approvalDetail = await approvalService.GetSingleApprovalDetailByEmpAndType(empId, "Leave"),
                //appDetailsWithLogs = appDetailsWithLog,
                leaveDays = await leavePolicyService.GetAllLeaveDay(),
                leaveStatusViewModels = await leavePolicyService.GetLeaveStatus(empId),
                approvalDetails = await leavePolicyService.GetApprovalDetailsByUser(userInfos.UserId)
                //leaveOpeningBalances = await leaveOpeningBalanceService.GetLeaveOpeningBalanceByEmpCode(employeeId)
            };
            return View(model);
        }
        public async Task<IActionResult> GetLeaveStatusByEmpId(int id)
        {
            var userInfos = await userInfo.getUserByEmployeeId(id);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            int empId = 0;
            if (EmpInfo != null)
            {
                empId = EmpInfo.Id;
            }
            ViewBag.employeeId = empId;
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpId(empId),
                leaveTypelist = await leaveTypeService.GetAllLeaveType(),
                //leaveTypelist = await leaveTypeService.GetAllLeaveTypeBySP(empId),
                //supervisor = await supervisorService.GetFirstSupervisorByEmpId(empId),
                approvalDetail = await approvalService.GetSingleApprovalDetailByEmpAndType(empId, "Leave"),
                //appDetailsWithLogs = appDetailsWithLog,
                leaveDays = await leavePolicyService.GetAllLeaveDay(),
                leaveStatusViewModels = await leavePolicyService.GetLeaveStatus(empId),
                approvalDetails = await leavePolicyService.GetApprovalDetailsByUser(userInfos.userId)
                //leaveOpeningBalances = await leaveOpeningBalanceService.GetLeaveOpeningBalanceByEmpCode(employeeId)
            };
            return Json(model);
        }
        public async Task<IActionResult> LeaveStatusForAdmin()
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            int empId = 0;
            if (EmpInfo != null)
            {
                empId = EmpInfo.Id;
            }
            ViewBag.employeeId = empId;

            //var appDetails = await leavePolicyService.GetApprovalDetailsByUser(userInfos.UserId);
            //var appDetailsWithLog = new List<AppDetailsWithLog>();

            //foreach (var item in appDetails)
            //{
            //	var data = new AppDetailsWithLog
            //	{
            //		approvalDetail = item,
            //		regId = id,
            //		leaveStatusLog = await leaveRegisterService.GetStatusLogByAppUserName(item.approver.ApplicationUser.UserName, item.approvalMaster.Id, id)
            //	};
            //	appDetailsWithLog.Add(data);
            //}

            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpId(empId),
                leaveTypelist = await leaveTypeService.GetAllLeaveType(),
                //leaveTypelist = await leaveTypeService.GetAllLeaveTypeBySP(empId),
                //supervisor = await supervisorService.GetFirstSupervisorByEmpId(empId),
                approvalDetail = await approvalService.GetSingleApprovalDetailByEmpAndType(empId, "Leave"),
                //appDetailsWithLogs = appDetailsWithLog,
                leaveDays = await leavePolicyService.GetAllLeaveDay(),
                leaveStatusViewModels = await leavePolicyService.GetLeaveStatus(empId),
                approvalDetails = await leavePolicyService.GetApprovalDetailsByUser(userInfos.UserId)
                //leaveOpeningBalances = await leaveOpeningBalanceService.GetLeaveOpeningBalanceByEmpCode(employeeId)
            };
            return View(model);
        }



        public async Task<IActionResult> LeaveStatusAll()
        {
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                applicationUsers = await leavePolicyService.GetAllUserList()
            };
            return View(model);
        }

        public async Task<IActionResult> GetLeaveStatusByUser(string id)
        {
            var employee = await leavePolicyService.GetEmployeeByCode(id);
            var user = await leavePolicyService.GetUserByCode(id);
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                leaveStatusViewModels = await leavePolicyService.GetLeaveStatus(employee.Id),
                approvalDetails = await leavePolicyService.GetApprovalDetailsByUser(user.userId)
            };
            return Json(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LeaveApplyAPI([FromBody]LeaveRegisterViewModelAPI model)
        {
            string userName = model.userName;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);

            var isApplied = await leaveRegisterService.CheckLeave(userInfos?.EmpCode, model.leaveFrom, model.leaveTo);
            if (isApplied)
            {
                return Json("Already Applied");
            }

            int empId = 0;
            string email = "";
            string name = "";
            if (EmpInfo != null)
            {
                empId = EmpInfo.Id;
                email = EmpInfo.emailAddress;
                name = EmpInfo.nameEnglish;
            }
            ViewBag.employeeId = empId;
            var leaveCheck = await leaveRegisterService.GetAllLeaveRegisterByEmpIdAndDate(empId, model.leaveFrom, model.leaveTo);



            if (!ModelState.IsValid || leaveCheck.Count() > 0)
            {
                return Json("Invalid Request");
            }

            LeaveRegister data = new LeaveRegister
            {
                Id = 0,
                employeeId = EmpInfo.Id,
                leaveTypeId = model.leaveTypeId,
                whenLeave = model.whenLeave,
                leaveStatus = 1,
                leaveFrom = model.leaveFrom,
                leaveTo = model.leaveTo,
                daysLeave = model.daysLeave,
                purposeOfLeave = model.purposeOfLeave,
                emergencyContactNo = model.emergencyContactNo,
                address = model.address,
                substitutionUserId = model.substitutionUserId,
                leaveDayId = model.leaveDayId,
                fileUrl = null
            };

            int id = await leaveRegisterService.SaveLeaveRegister(data);

            IEnumerable<ApprovalDetail> approvalDetails = await approvalService.GetApprovalDetailByEmpAndType(EmpInfo.Id, "Leave");

            var i = 1;
            var Active = 1;
            foreach (ApprovalDetail supervisor in approvalDetails)
            {
                LeaveRoute leaveRoute = new LeaveRoute
                {
                    leaveRegisterId = id,
                    employeeId = supervisor.approverId,
                    routeOrder = i,
                    isActive = Active,
                };
                await leaveRouteService.SaveLeaveRoute(leaveRoute);
                i++;
                Active = 0;
            }

            LeaveStatusLog leaveStatusLog = new LeaveStatusLog
            {
                employeeId = EmpInfo.Id,
                leaveRegisterId = id,
                date = DateTime.Now,
                remarks = "On Approval",
                Status = 1
            };
            await leaveStatusLogService.SaveLeaveStatusLog(leaveStatusLog);




			#region Asad added on 26.06.2023 for sending SMS

			//var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(model.empCode);

			int? depId = 0;
			int? brId = 0;
			int? zoneLId = 0;
			if (EmpInfo.departmentId != null && EmpInfo.hrBranchId == 205 && EmpInfo.locationId == null)
			{
				depId = EmpInfo.departmentId;
				brId = 0;
				zoneLId = 0;
			}
			else if (EmpInfo.hrBranchId != null && EmpInfo.departmentId == null && EmpInfo.locationId == null)
			{
				brId = EmpInfo.hrBranchId;
				depId = 0;
				zoneLId = 0;
			}
			else if (EmpInfo.locationId != null && EmpInfo.hrBranchId == null && EmpInfo.departmentId == null)
			{
				zoneLId = EmpInfo.locationId;
				depId = 0;
				brId = 0;

			}

			var HeadInfo = await personalInfoService.GetHeadByDepartBranchZoneId(depId, brId, zoneLId);
			var Aprover = "";
			var ApproverPhone = "";

			if (EmpInfo.Id == HeadInfo.HeadId)
			{
				var HeadInfoForHead = await personalInfoService.GetHeadForHeadByDepartBranchZoneId(depId, brId, zoneLId);
				var senderId = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(HeadInfoForHead.HeadCode);
				Aprover = senderId.ApplicationUserId;
				ApproverPhone = senderId.mobileNumberOffice.Length == 13 ? senderId.mobileNumberOffice : "88" + senderId.mobileNumberOffice;
			}
			else
			{
				var senderId = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(HeadInfo.HeadCode);
				Aprover = senderId.ApplicationUserId;
				ApproverPhone = senderId.mobileNumberOffice.Length == 13 ? senderId.mobileNumberOffice : "88" + senderId.mobileNumberOffice;
			}
			var Emp = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(userInfos?.EmpCode);
			
			try
			{
				var message = "A leave is waiting for your approval.\nApplicant: " + Emp.nameEnglish + "(" + Emp.employeeCode + ")" + "\nLeave From: " + model.leaveFrom?.ToString("dd-MMM-yyyy") + "\nLeave To: " + model.leaveTo?.ToString("dd-MMM-yyyy") + "\nLeave Days: " + model.daysLeave + "\nReason: " + model.purposeOfLeave;
				//var message = "A leave request is waiting for your approval.\nApplicant:";
				var smsResponse = Messaging.SendSMSMessage(ApproverPhone, message);

				await employeePunchCardInfoService.SaveSMSResponseLog(smsResponse);
			}
			catch (Exception ex)
			{
			}
			#endregion


			//Save Notification
			var notification = new Notification
            {
                Id = 0,
                senderId = userInfos.aspnetId,
                receiverId = approvalDetails.FirstOrDefault().approver.ApplicationUserId,
                date = DateTime.Now,
                title = "Leave application from " + userInfos.EmpName,
                description = userInfos.EmpName + " apply leave for " + model.purposeOfLeave,
                type = "Leave",
                isRead = 0,
                url = "/HRPMSLeave/LeaveApproval/Index/" + id,
                status = 1,
                isDelete = id //registerId
            };
            await requisitionService.SaveNotification(notification);



            var ActiveLeave = await leaveRouteService.GetLeaveRouteByLeaveRegisterId(id);

            string html1 = "<div><strong>Application for leave.</strong></div>"
                + " <br/>"
                + "<p>Dear Sir,</p>"
                + " <br/>"
                + " This is to inform you that you have sent one leave application to " + ActiveLeave?.employee?.nameEnglish + " just now."
                + "<br/>"

                + "<div><p> Thank You & Best Regards</p><p style = 'font-weight:bold' > Human Resource Department.</p></div>"
                + " <br/>";

            string host = HttpContext.Request.Host.ToString();
            string scheme = Request.Scheme;
            string baseUrl = $"" + scheme + "://" + host + "/HRPMSLeave/LeaveApproval";

            string html = "<div><strong>Leave Recommendation/Approval.</strong></div>"
                    + " <br/>"
                    + "<p>Dear Sir,</p>"
                    + " <br/>"
                    + " This is to inform you that one leave application is waiting for your recommendation/approval."
                    + "<br/>"
                    + "<div><a href='" + baseUrl + "'><button>Login</button></a></div>"
                    + " <br/>"
                    + "<div><p> Thank You & Best Regards</p><p style = 'font-weight:bold' > Human Resource Department.</p></div>"
                    + " <br/>";

            if (email != null)
            {
                //await emailSenderService.SendEmailWithFrom(email, name, "Leave Application", html1);
            }

            if (ActiveLeave?.employee?.emailAddress != null)
            {
                //await emailSenderService.SendEmailWithFrom(ActiveLeave?.employee?.emailAddress, ActiveLeave?.employee?.nameEnglish, "Leave Application", html);
            }

            return Json("Saved");
        }

		//[AllowAnonymous]
		//public async Task<IActionResult> GetEmployeeLeaveByAnyKey(string employeeCode, int? leaveTypeId, int? approverId, string fromDate, string toDate)
  //      {
		//	var data = await leaveRegisterService.GetEmployeeLeaveByAnyKey(employeeCode, leaveTypeId,  approverId, fromDate, toDate);
		//	return Json(data);
		//}
		

		[HttpPost]
        public async Task<IActionResult> LeaveApply([FromForm] LeaveRegisterViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);

            var isApplied = await leaveRegisterService.CheckLeave(userInfos?.EmpCode, model.leaveFrom, model.leaveTo);
            if (isApplied)
            {
                return Json("exist");
            }

            int empId = 0;
            string email = "";
            string name = "";
            if (EmpInfo != null)
            {
                empId = EmpInfo.Id;
                email = EmpInfo.emailAddress;
                name = EmpInfo.nameEnglish;
            }
            ViewBag.employeeId = empId;
            var leaveCheck = await leaveRegisterService.GetAllLeaveRegisterByEmpIdAndDate(empId, model.leaveFrom, model.leaveTo);



            if (!ModelState.IsValid || leaveCheck.Count() > 0)
            {
				var errors = ModelState.Select(x => x.Value.Errors);

                model.leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpId(empId);
                model.leaveTypelist = await leaveTypeService.GetAllLeaveTypeBySP(empId);
                //model.supervisor = await supervisorService.GetFirstSupervisorByEmpId(EmpInfo.Id);
                model.approvalDetail = await approvalService.GetSingleApprovalDetailByEmpAndType(empId, "Leave");
                model.leaveDays = await leavePolicyService.GetAllLeaveDay();
                model.leaveStatusViewModels = await leavePolicyService.GetLeaveStatus(empId);
                model.approvalDetails = await leavePolicyService.GetApprovalDetailsByUser(userInfos.UserId);

                model.fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]);


				#region Asad added on 26.06.2023 for sending SMS
				//var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(model.empCode);

				int? depId = 0;
				int? brId = 0;
				int? zoneLId = 0;
				if (EmpInfo.departmentId != null && EmpInfo.hrBranchId == 205 && EmpInfo.locationId == null)
				{
					depId = EmpInfo.departmentId;
					brId = 0;
					zoneLId = 0;
				}
				else if (EmpInfo.hrBranchId != null && EmpInfo.departmentId == null && EmpInfo.locationId == null)
				{
					brId = EmpInfo.hrBranchId;
					depId = 0;
					zoneLId = 0;
				}
				else if (EmpInfo.locationId != null && EmpInfo.hrBranchId == null && EmpInfo.departmentId == null)
				{
					zoneLId = EmpInfo.locationId;
					depId = 0;
					brId = 0;

				}

				var HeadInfo = await personalInfoService.GetHeadByDepartBranchZoneId(depId, brId, zoneLId);
				var Aprover = "";
				var ApproverPhone = "";

				if (EmpInfo.Id == HeadInfo.HeadId)
				{
					var HeadInfoForHead = await personalInfoService.GetHeadForHeadByDepartBranchZoneId(depId, brId, zoneLId);
					var senderId = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(HeadInfoForHead.HeadCode);
					Aprover = senderId.ApplicationUserId;
					ApproverPhone = senderId.mobileNumberOffice.Length == 13 ? senderId.mobileNumberOffice : "88" + senderId.mobileNumberOffice;
				}
				else
				{
					var senderId = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(HeadInfo.HeadCode);
					Aprover = senderId.ApplicationUserId;
					ApproverPhone = senderId.mobileNumberOffice.Length == 13 ? senderId.mobileNumberOffice : "88" + senderId.mobileNumberOffice;
				}
				var Emp = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(userInfos?.EmpCode);
				
				try
				{
                    //var message = "An attendance is waiting for your approval.\nApplicant: " + Emp.nameEnglish + "(" + Emp.employeeCode + ")" + "\nAtt. Date: " + model.punchDate?.ToString("dd-MMM-yyyy") + "\nAtt. Time: " + model.punchTime + "\nRemarks: " + model.notes;
                    var message = "A leave is waiting for your approval.\nApplicant: " + Emp.nameEnglish + "(" + Emp.employeeCode + ")" + "\nLeave From: " + model.leaveFrom?.ToString("dd-MMM-yyyy") + "\nLeave To: " + model.leaveTo?.ToString("dd-MMM-yyyy") + "\nLeave Days: " + model.daysLeave + "\nReason: " + model.purposeOfLeave;
                    //var message = "A leave request is waiting for your approval.\nApplicant:";
                    var smsResponse = Messaging.SendSMSMessage(ApproverPhone, message);

					await employeePunchCardInfoService.SaveSMSResponseLog(smsResponse);
				}
				catch (Exception ex)
				{
				}
				
				#endregion



				if (leaveCheck.Count() > 0)
                {
                    ModelState.AddModelError(string.Empty, "Another Leave has already applied in same Date !!");
                }
                if (model.leaveBalance < model.daysLeave)
                {
                    ModelState.AddModelError(string.Empty, "Sorry You Haven't Enough Leave Balance !!");
                }
                return View(model);
            }


            string fileName = String.Empty;
            string fileNameMain = String.Empty;
            if (model.fileUrl != null)
            {
                string message = FileSave.SaveEmpAttachment(out fileName, model.fileUrl);

                if (message == "success")
                {
                    fileNameMain = fileName;
                }
                else
                {
                    fileNameMain = "";
                }
            }
            else
            {
                fileNameMain = "";
            }

            LeaveRegister data = new LeaveRegister
            {
                Id = model.id,
                employeeId = EmpInfo.Id,
                leaveTypeId = model.leaveTypeId,
                whenLeave = model.whenLeave,
                leaveStatus = 1,
                leaveFrom = model.leaveFrom,
                leaveTo = model.leaveTo,
                daysLeave = model.daysLeave,
                purposeOfLeave = model.purposeOfLeave,
                emergencyContactNo = model.emergencyContactNo,
                address = model.address,
                substitutionUserId = model.substitutionUserId,
                leaveDayId = model.leaveDayId,
                fileUrl = fileNameMain
            };

            int id = await leaveRegisterService.SaveLeaveRegister(data);

            //IEnumerable<Supervisor> supervisors = await supervisorService.GetSupervisorByEmpId(EmpInfo.Id);
            IEnumerable<ApprovalDetail> approvalDetails = await approvalService.GetApprovalDetailByEmpAndType(EmpInfo.Id, "Leave");

            var i = 1;
            var Active = 1;
            foreach (ApprovalDetail supervisor in approvalDetails)
            {
                LeaveRoute leaveRoute = new LeaveRoute
                {
                    leaveRegisterId = id,
                    employeeId = supervisor.approverId,
                    routeOrder = i,
                    isActive = Active,
                };
                await leaveRouteService.SaveLeaveRoute(leaveRoute);
                i++;
                Active = 0;
            }

            LeaveStatusLog leaveStatusLog = new LeaveStatusLog
            {
                employeeId = EmpInfo.Id,
                leaveRegisterId = id,
                date = DateTime.Now,
                remarks = "On Approval",
                Status = 1
            };
            await leaveStatusLogService.SaveLeaveStatusLog(leaveStatusLog);


            //Save Notification
            var notification = new Notification
            {
                Id = 0,
                senderId = userInfos.aspnetId,
                receiverId = approvalDetails.FirstOrDefault().approver.ApplicationUserId,
                date = DateTime.Now,
                title = "Leave application from " + userInfos.EmpName,
                description = userInfos.EmpName + " apply leave for " + model.purposeOfLeave,
                type = "Leave",
                isRead = 0,
                url = "/HRPMSLeave/LeaveApproval/Index/" + id,
                status = 1,
                isDelete = id //registerId
            };
            await requisitionService.SaveNotification(notification);
            var device = await dbChangeService.GetUserLastLogHistory(approvalDetails.FirstOrDefault().approver?.employeeCode);
            if (device != null) {
                var pushNotify = new NotificationModel
                {
                    DeviceId = device?.deviceId,
                    IsAndroiodDevice =true,
                    Title = "Leave application from " + userInfos.EmpName,
                    Body= userInfos.EmpName + " apply leave for " + model.purposeOfLeave,
                };
                await pushNotificationService.SendNotification(pushNotify);
            }

            var ActiveLeave = await leaveRouteService.GetLeaveRouteByLeaveRegisterId(id);

            string html1 = "<div><strong>Application for leave.</strong></div>"
                + " <br/>"
                + "<p>Dear Sir,</p>"
                + " <br/>"
                + " This is to inform you that you have sent one leave application to " + ActiveLeave?.employee?.nameEnglish + " just now."
                + "<br/>"

                + "<div><p> Thank You & Best Regards</p><p style = 'font-weight:bold' > Human Resource Department.</p></div>"
                + " <br/>";

            string host = HttpContext.Request.Host.ToString();
            string scheme = Request.Scheme;
            string baseUrl = $"" + scheme + "://" + host + "/HRPMSLeave/LeaveApproval";

            string html = "<div><strong>Leave Recommendation/Approval.</strong></div>"
                    + " <br/>"
                    + "<p>Dear Sir,</p>"
                    + " <br/>"
                    + " This is to inform you that one leave application is waiting for your recommendation/approval."
                    + "<br/>"
                    + "<div><a href='" + baseUrl + "'><button>Login</button></a></div>"
                    + " <br/>"
                    + "<div><p> Thank You & Best Regards</p><p style = 'font-weight:bold' > Human Resource Department.</p></div>"
                    + " <br/>";

            if (email != null)
            {
                await emailSenderService.SendEmailWithFrom(email, name, "Leave Application", html1);
            }

            if (ActiveLeave?.employee?.emailAddress != null)
            {
                await emailSenderService.SendEmailWithFrom(ActiveLeave?.employee?.emailAddress, ActiveLeave?.employee?.nameEnglish, "Leave Application", html);
            }

            return Json("Saved");
        }

        public async Task<IActionResult> SendEmail(string to, string name, string subject, string body)
        {
            await emailSenderService.SendEmailWithFrom(to, name, subject, body);
            return Json("ok");
        }


        public async Task<IActionResult> GetAllLeaveForReCreation()
        {
            //string userName = HttpContext.User.Identity.Name;
            //var userInfos = await userInfo.GetUserInfoByUser(userName);
            //var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                leaveRegisterslist = await leaveRegisterService.GetAllReCreationLeave()
            };
            return View(model);
        }
        public async Task<IActionResult> ReCreationLeaveListApi(string from, string to, string type, int? typeId)
        {

            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                leaveRegisterslist = await leaveRegisterService.GetAllReCreationLeaveByDateAndTypeRange(Convert.ToDateTime(from), Convert.ToDateTime(to), type, typeId)
            };

            return Json(model);
        }
        [AllowAnonymous]
        public async Task<IActionResult> AllReCreationLeaveByDateView(string from, string to)
        {

            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                leaveRegisterslist = await leaveRegisterService.GetAllReCreationLeaveByDateRange(Convert.ToDateTime(from), Convert.ToDateTime(to))
            };

            return View(model);
        }




        [AllowAnonymous]
        public IActionResult AllReCreationLeaveByDate(string from, string to)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;

            string url = scheme + "://" + host + "/HRPMSLeave/LeaveRegister/AllReCreationLeaveByDateView?from=" + from +"&to="+to;
            string status = myPDF.GeneratePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }


        public async Task<IActionResult> ActionAllNew([FromForm] LeaveRegisterViewModel model)
        {
            //  return Json(model);
            string fileName = String.Empty;

            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(user?.EmpCode);
           
            var regLeave = await leaveRegisterService.GetLeaveRegisterById(Convert.ToInt32(model.id));
            regLeave.leaveStatus = 3;
            regLeave.leaveFrom = model.leaveFrom;
            regLeave.leaveTo = model.leaveTo;
            regLeave.daysLeave = model.daysLeave;
            regLeave.comment = model.comment;

            if (model.updateAttatchment != null)
            {
                string message = FileSave.SaveEmpAttachment(out fileName, model.updateAttatchment);
                if (message == "success")
                {
                    regLeave.updateAttatchment = fileName;
                }
            }

            await leaveRegisterService.SaveLeaveRegister(regLeave);
            

            return RedirectToAction(nameof(GetAllLeaveForReCreation));

        }

        public async Task<IActionResult> ManualRecreationLeaveApply()
        {

            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            int empId = 0;
            if (EmpInfo != null)
            {
                empId = EmpInfo.Id;
            }
            ViewBag.employeeId = empId;
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                //leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpId(empId),
                leaveRegisterslist = await leaveRegisterService.ManualRecrationListForAppList(empId),
                leaveTypelist = await leaveTypeService.GetAllLeaveType(),
                //leaveTypelist = await leaveTypeService.GetAllLeaveTypeBySP(empId),
                //supervisor = await supervisorService.GetFirstSupervisorByEmpId(empId),
                approvalDetail = await approvalService.GetSingleApprovalDetailByEmpAndType(empId, "Leave"),
                leaveDays = await leavePolicyService.GetAllLeaveDay(),
                leaveStatusViewModels = await leavePolicyService.GetLeaveStatus(empId),
                approvalDetails = await leavePolicyService.GetApprovalDetailsByUser(userInfos.UserId)
                //leaveOpeningBalances = await leaveOpeningBalanceService.GetLeaveOpeningBalanceByEmpCode(employeeId)
            };
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManualRecreationLeaveApply([FromForm] LeaveRegisterViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(model.empCode);
            
            int empId = 0;
            string email = "";
            string name = "";
            if (EmpInfo != null)
            {
                empId = EmpInfo.Id;
                email = EmpInfo.emailAddress;
                name = EmpInfo.nameEnglish;
            }
            ViewBag.employeeId = empId;
            
            string fileName = String.Empty;
            string fileNameMain = String.Empty;
            if (model.fileUrl != null)
            {
                string message = FileSave.SaveEmpAttachment(out fileName, model.fileUrl);

                if (message == "success")
                {
                    fileNameMain = fileName;
                }
                else
                {
                    fileNameMain = "";
                }
            }
            else
            {
                fileNameMain = "";
            }

            LeaveRegister data = new LeaveRegister
            {
                Id = model.id,
                employeeId = EmpInfo.Id,
                leaveTypeId = model.leaveTypeId,
                whenLeave = model.whenLeave,
                //leaveStatus = 3,//manual leave approve status=3
                leaveStatus = 33,//inintal Manual Leave status=33
                leaveFrom = model.leaveFrom,
                leaveTo = model.leaveTo,
                daysLeave = model.daysLeave,
                purposeOfLeave = model.purposeOfLeave,
                emergencyContactNo = model.emergencyContactNo,
                address = model.address,
                substitutionUserId = model.substitutionUserId,
                leaveDayId = model.leaveDayId,
                fileUrl = fileNameMain,
                paymentOption = "Manual Recreation"
            };

            int id = await leaveRegisterService.SaveLeaveRegister(data);

            if (model.leaveFromN != null)
            {
                LeaveRegister data1 = new LeaveRegister
                {
                    Id = model.id,
                    employeeId = EmpInfo.Id,
                    leaveTypeId = model.leaveTypeId,
                    whenLeave = model.whenLeave,
                    leaveStatus = 3,
                    leaveFrom = model.leaveFromN,
                    leaveTo = model.leaveToN,
                    daysLeave = model.daysLeaveN,
                    purposeOfLeave = model.purposeOfLeave,
                    emergencyContactNo = model.emergencyContactNo,
                    address = model.address,
                    substitutionUserId = model.substitutionUserId,
                    leaveDayId = model.leaveDayId,
                    fileUrl = fileNameMain,
                    paymentOption = "Manual Recreation"
                };

                int ids = await leaveRegisterService.SaveLeaveRegister(data1);

            }


            return Json("Saved");
        }

        [HttpPost]
        public async Task<IActionResult> ApprovedManualRecreation(LeaveRegisterViewModel model)
        {
            try
            {
                for (int i = 0; i < model.registerids.Length; i++)
                {
                    await leaveRegisterService.UpdateManualLeaveStatus(Convert.ToInt32(model.registerids[i]));

                }
                return RedirectToAction("ManualRecreationLeaveApply");
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }



        [HttpGet]
        public async Task<IActionResult> DeleteRecord(int id)
        {
            await leaveRegisterService.DeleteRecordById(id);
            return Json("ok");
        }

        public async Task<ActionResult> ManualRecreationReport()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> ManualRecreationBranchReport(DateTime fromDate, DateTime toDate)
        {
            ManualRecreationReportVM model = new ManualRecreationReportVM
            {
                ManualRecreationReportViewModel = await leaveRegisterService.GetAllManualRecreationBranchReportByDate(fromDate, toDate)
            };
            return View(model);
        }
        [AllowAnonymous]
        public IActionResult ManualRecreationBranchReportPDF(DateTime fromDate, DateTime toDate)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;
            string url = "";

            url = scheme + "://" + host + "/HRPMSLeave/LeaveRegister/ManualRecreationBranchReport?fromDate=" + fromDate + "&toDate=" + toDate;
            string status = myPDF.GenerateLandscapePDF(out fileName, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [AllowAnonymous]
        public async Task<IActionResult> ManualRecreationHeadOfficeReport(DateTime fromDate, DateTime toDate)
        {
            ManualRecreationReportVM model = new ManualRecreationReportVM
            {
                ManualRecreationReportViewModel = await leaveRegisterService.GetAllManualRecreationHeadOfficeReportByDate(fromDate, toDate)
            };
            return View(model);
        }
        [AllowAnonymous]
        public IActionResult ManualRecreationHeadOfficeReportPDF(DateTime fromDate, DateTime toDate)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;
            string url = "";

            url = scheme + "://" + host + "/HRPMSLeave/LeaveRegister/ManualRecreationHeadOfficeReport?fromDate=" + fromDate + "&toDate=" + toDate;
            string status = myPDF.GenerateLandscapePDF(out fileName, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        #endregion

        #region Leave Report with Apply

        public async Task<IActionResult> AttendanceReport()
        {
            var userName = HttpContext.User.Identity.Name;
            var model = new LateAttandenceDataVM
            {
                employeeCode= userName
            };
            return View(model);
        }

        //Early Leave Accepted
        [AllowAnonymous]
        public async Task<IActionResult> EarlyLeaveAttendance(string userName)
        {
            var data = await leaveRegisterService.GetLateAttendanceReports(userName);
            var model = new LateAttendanceReportVM
            {
                companies = await eRPCompanyService.GetAllCompany(),
                lateAttandenceDataVMs=data.Where(y => y.summaryId == 9 && y.lateOrEarly == "Early Leave")
            };

            return View(model);
        }
        [AllowAnonymous]
        public IActionResult EarlyLeaveAttendancePDF(string userName)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;
            string url = "";

            url = scheme + "://" + host + "/HRPMSLeave/LeaveRegister/EarlyLeaveAttendance?userName=" + userName;
            string status = myPDF.GeneratePDF(out fileName, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong. </h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }


        [AllowAnonymous]
        public async Task<IActionResult> MyEarlyLeaveAttendanceView(string userName)
        {
            var data = await leaveRegisterService.MyEarlyLeaveAttendance(userName);
            var model = new LateAttendanceReportVM
            {
                companies = await eRPCompanyService.GetAllCompany(),
                lateAttandenceDataVMs = data.Where(y => y.summaryId == 9 && y.lateOrEarly == "Early Leave")
            };

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult MyEarlyLeaveAttendance(string userName)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;
            string url = "";

            url = scheme + "://" + host + "/HRPMSLeave/LeaveRegister/MyEarlyLeaveAttendanceView?userName=" + userName;
            string status = myPDF.GeneratePDF(out fileName, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong. </h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }


        //Late Attendance Accepted
        [AllowAnonymous]
        public async Task<IActionResult> LateAttendanceAccepted(string userName)
        {
            var data = await leaveRegisterService.GetLateAttendanceReports(userName);
            var model = new LateAttendanceReportVM
            {
                companies = await eRPCompanyService.GetAllCompany(),
                lateAttandenceDataVMs = data.Where(y => y.summaryId == 9 && y.lateOrEarly == "Late Reminder")
            };

            return View(model);
        }
        [AllowAnonymous]
        public IActionResult LateAttendanceAcceptedPDF(string userName)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;
            string url = "";

            url = scheme + "://" + host + "/HRPMSLeave/LeaveRegister/LateAttendanceAccepted?userName=" + userName;
            string status = myPDF.GeneratePDF(out fileName, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong. </h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        //Early Leave Rejected
        [AllowAnonymous]
        public async Task<IActionResult> RejectEarlyLeaveAttendance(string userName)
        {
            var data = await leaveRegisterService.GetLateAttendanceReports(userName);
            var model = new LateAttendanceReportVM
            {
                companies = await eRPCompanyService.GetAllCompany(),
                lateAttandenceDataVMs = data.Where(y => y.summaryId == 10 && y.lateOrEarly == "Early Leave")
            };

            return View(model);
        }
        [AllowAnonymous]
        public IActionResult RejectEarlyLeaveAttendancePDF(string userName)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;
            string url = "";

            url = scheme + "://" + host + "/HRPMSLeave/LeaveRegister/RejectEarlyLeaveAttendance?userName=" + userName;
            string status = myPDF.GeneratePDF(out fileName, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong. </h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        //Late Attendance Rejected
        [AllowAnonymous]
        public async Task<IActionResult> LateAttendanceRejected(string userName)
        {
            var data = await leaveRegisterService.GetLateAttendanceReports(userName);
            var model = new LateAttendanceReportVM
            {
                companies = await eRPCompanyService.GetAllCompany(),
                lateAttandenceDataVMs = data.Where(y => y.summaryId == 10 && y.lateOrEarly == "Late Reminder")
            };

            return View(model);
        }
        [AllowAnonymous]
        public IActionResult LateAttendanceRejectedPDF(string userName)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;
            string url = "";

            url = scheme + "://" + host + "/HRPMSLeave/LeaveRegister/LateAttendanceRejected?userName=" + userName;
            string status = myPDF.GeneratePDF(out fileName, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong. </h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        #endregion

        #region Leave Cancel

        public async Task<IActionResult> LeaveCancel()
        {
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegister()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LeaveCancel([FromForm] LeaveRegisterViewModel model)
        {
            //return Json(model);
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            int? empId = null;
            if (EmpInfo != null)
            {
                empId = EmpInfo.Id;
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            LeaveStatusLog leaveStatusLog = new LeaveStatusLog
            {
                employeeId = empId,
                leaveRegisterId = model.id,
                date = DateTime.Now,
                remarks = model.txtRemarks,
                Status = 6
            };
            await leaveStatusLogService.SaveLeaveStatusLog(leaveStatusLog);

            await leaveRegisterService.UpdateLeaveApproval(model.id, 6);

            return RedirectToAction(nameof(LeaveCancel));
        }

        #endregion

        #region Leave Return
        public async Task<IActionResult> LeaveReturn()
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            int empId = 0;
            if (EmpInfo != null)
            {
                empId = EmpInfo.Id;
            }
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpIdAndStatus(empId, 4)
            };
            return View(model);
        }
        #endregion

        #region LeaveReject
        public async Task<IActionResult> LeaveReject(int? id)
        {
            ViewBag.masterId = id;

            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            int empId = 0;
            if (EmpInfo != null)
            {
                empId = EmpInfo.Id;
            }
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpIdAndStatus(empId, 5)
            };
            return View(model);
        }

        public async Task<IActionResult> RejectLeaveByLeaveId(int id)
        {
            var data = await leaveRegisterService.UpdateLeaveStatus(id, 5);
            if (data > 0)
            {
                return Json("rejected");
            }
            else
            {
                return Json("failed");
            }
        }
        #endregion

        #region Manual Leave Apply
        public async Task<IActionResult> ManualLeaveApply()
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            int empId = 0;
            if (EmpInfo != null)
            {
                empId = EmpInfo.Id;
            }

            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpId(empId),
                leaveTypelist = await leaveTypeService.GetAllLeaveType(),
                leaveDays = await leavePolicyService.GetAllLeaveDay(),
                visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManualLeaveApply([FromForm] LeaveRegisterViewModel model)
        {
            //return Json(model);
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            //var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            int empId = (int)model.employeeId;
            //if (EmpInfo != null)
            //{
            //    empId = EmpInfo.Id;
            //}

            var leaveCheck = await leaveRegisterService.GetAllLeaveRegisterByEmpIdAndDate(empId, model.leaveFrom, model.leaveTo);

            if (!ModelState.IsValid || leaveCheck.Count() > 0)
            {
                model.leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpId(empId);
                model.leaveTypelist = await leaveTypeService.GetAllLeaveType();
                model.leaveDays = await leavePolicyService.GetAllLeaveDay();
                model.visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData();
                model.fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]);
                if (leaveCheck.Count() > 0)
                {
                    ModelState.AddModelError(string.Empty, "Another Leave has already applied in same Date !!");
                }
                if (model.leaveTypeId == 3 || model.leaveTypeId == 8)
                {
                }
                else
                {
                    if (model.leaveBalance < model.daysLeave)
                    {
                        ModelState.AddModelError(string.Empty, "Sorry You Haven't Enough Leave Balance !!");
                    }
                }
                return View(model);
            }

            LeaveRegister data = new LeaveRegister
            {
                Id = model.id,
                employeeId = model.employeeId,
                leaveTypeId = model.leaveTypeId,
                whenLeave = model.whenLeave,
                leaveStatus = 3,
                leaveFrom = model.leaveFrom,
                leaveTo = model.leaveTo,
                daysLeave = model.daysLeave,
                purposeOfLeave = model.purposeOfLeave,
                emergencyContactNo = model.emergencyContactNo,
                address = model.address,
                leaveDayId = model.leaveDayId,
                paymentOption = model.paymentType
            };

            int masterId = await leaveRegisterService.SaveLeaveRegister(data);

            LeaveStatusLog leaveStatusLog = new LeaveStatusLog
            {
                employeeId = model.employeeId,
                leaveRegisterId = masterId,
                date = DateTime.Now,
                remarks = "Leave Applied Manually by Admin",
                Status = 3
            };
            await leaveStatusLogService.SaveLeaveStatusLog(leaveStatusLog);

            //return RedirectToAction(nameof(ManualLeaveApply));
            return RedirectToAction(nameof(ManualLeaveApplyList), new { employeeId = empId });
        }

        public async Task<IActionResult> ManualLeaveApplyList(int employeeId)
        {
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(employeeId),
            };
            ViewBag.empId = employeeId;
            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> GetTotalLeaveDaysBetweenDate(string frmDate, string toDate, int leaveType)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                var userInfos = await userInfo.GetUserInfoByUser(userName);
                var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
                if (EmpInfo.shiftGroupId == null)
                {
                   // int shiftGroupId = 0;
                    var daysLeave = await leaveRegisterService.GetTotalLeaveDaysBetweenDate(Convert.ToDateTime(frmDate), Convert.ToDateTime(toDate), leaveType, 0);
                   // var daysLeave = await leaveRegisterService.GetTotalLeaveDaysBetweenDate(Convert.ToDateTime(frmDate), Convert.ToDateTime(toDate), leaveType, (int)EmpInfo.shiftGroupId);
                    return Json(daysLeave);
                    //return Json("Error");
                }
                else
                {
                    var daysLeave = await leaveRegisterService.GetTotalLeaveDaysBetweenDate(Convert.ToDateTime(frmDate), Convert.ToDateTime(toDate), leaveType, (int)EmpInfo.shiftGroupId);
                    return Json(daysLeave);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        public async Task<IActionResult> GetTypeNamesByType(string type)
        {
            if (type == "Department")
            {
                return Json(await personalInfoService.GetAllDepartment());
            }
            else if (type == "Branch")
            {
                return Json(await personalInfoService.GetAllBranch());
            }
            else if (type == "Zone")
            {
                return Json(await personalInfoService.GetAllZone());
            }
            else
            {
                return Json(false);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetLeaveBalanceSummaryByEmpId(int empId)
        {
            var daysLeave = await leaveRegisterService.GetLeaveBalanceSummaryByEmpId(empId);
            return Json(daysLeave);
        }

        [HttpGet]
        public async Task<JsonResult> GetAllLeaveApplicationByEmpId(int empId)
        {
            var daysLeave = await leaveRegisterService.GetAllLeaveRegisterByEmpId(empId);
            return Json(daysLeave);
        }

        #endregion

        #region Wages Leave Apply
        public async Task<IActionResult> WagesLeaveApply()
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            int empId = 0;
            if (EmpInfo != null)
            {
                empId = EmpInfo.Id;
            }
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                wagesLeaveRegisters = await wagesLeaveRegisterService.GetAllLeaveRegister(),
                leaveTypelist = await leaveTypeService.GetAllLeaveType(),
                //leaveOpeningBalances = await leaveOpeningBalanceService.GetLeaveOpeningBalanceByEmpCode(employeeId)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WagesLeaveApply([FromForm] LeaveRegisterViewModel model)
        {
            //return Json(model);
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            int empId = 0;
            if (EmpInfo != null)
            {
                empId = EmpInfo.Id;
            }

            if (!ModelState.IsValid)
            {
                model.wagesLeaveRegisters = await wagesLeaveRegisterService.GetAllLeaveRegister();
                model.leaveTypelist = await leaveTypeService.GetAllLeaveType();
                model.fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]);
                return View(model);
            }

            WagesLeaveRegister data = new WagesLeaveRegister
            {
                Id = model.id,
                employeeId = model.employeeId,
                leaveTypeId = model.leaveTypeId,
                whenLeave = model.whenLeave,
                leaveStatus = 1,
                leaveFrom = model.leaveFrom,
                leaveTo = model.leaveTo,
                daysLeave = model.daysLeave,
                purposeOfLeave = model.purposeOfLeave,
                emergencyContactNo = model.emergencyContactNo,
                address = model.address,
            };

            int masterId = await wagesLeaveRegisterService.SaveLeaveRegister(data);

            return RedirectToAction(nameof(WagesLeaveApply));
        }

        #endregion

        #region Leave Report

        public async Task<IActionResult> LeaveReportView()
        {
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                years = await yearCourseTitleService.GetYear()
            };
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> LeaveReportSummaryPdf(string rptType, string year)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;
            string url = "";

            if (rptType == "LEAVESUMMARY")
            {
                var data = await salaryService.GetReportFormatByReportType(rptType);
                url = scheme + "://" + host + "/HRPMSLeave/LeaveRegister/" + data.FirstOrDefault().reportFormat + "?year=" + year;
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
        public async Task<IActionResult> LeaveReportPdf(string rptType, int empId, string fdate, string tdate)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;
            string url = "";

            if (rptType == "LEAVEINDIVL")
            {
                var data = await salaryService.GetReportFormatByReportType(rptType);
                url = scheme + "://" + host + "/HRPMSLeave/LeaveRegister/" + data.FirstOrDefault().reportFormat + "?empId=" + empId + "&&fdate=" + fdate + "&&tdate=" + tdate;
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
        public IActionResult LeaveCasualPdf(int id)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;

            string url = scheme + "://" + host + "/HRPMSLeave/LeaveRegister/LeaveReportCasual/" + id;
            string status = myPDF.GeneratePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        [AllowAnonymous]
        public async Task<IActionResult> LeaveReportCasual(int id)
        {
            var leaveRegList = await leaveRegisterService.GetLeaveRegisterById(id);
            DateTime leaveYear = Convert.ToDateTime(leaveRegList.leaveFrom);
            var year = leaveYear.Year;
            DateTime leaveFrom = Convert.ToDateTime(leaveRegList.leaveFrom);
            DateTime ToLeave = Convert.ToDateTime(leaveRegList.leaveTo);
            var presentUsed = await leaveRegisterService.GetTotalAvailedLeaveByDate(id, leaveFrom);
            decimal? expence=Convert.ToDecimal(leaveRegList.daysLeave) + Convert.ToDecimal(presentUsed);
            var approver = await personalInfoService.GetLeaveApproverSignatureByEmpId((int)leaveRegList.Id);
            var DH = new HRPMS.Data.Entity.Employee.EmployeeSignature();
            if (approver != null)
            {
                DH = approver;
            }
            var gmId = 0;
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                leaveRegister = await leaveRegisterService.GetLeaveRegisterById(id),

                employeeSignature = await leaveRegisterService.GetSignatureByEmpId(Convert.ToInt32(leaveRegList.employeeId)),
                employeeDJBSignature = DH,
                leaveReportModels = await leaveRegisterService.GetLeaveReport(year, Convert.ToInt32(leaveRegList.employeeId)),
                leaveSupervisorRecomViewModels = await leaveRegisterService.GetSupervisorRecomForLeaveByEmpId(id, Convert.ToInt32(leaveRegList.employeeId)),
                availedThisYear = await leaveRegisterService.GetTotalAvailedLeaveByYear(id, year),
                availedThisYearByDate = await leaveRegisterService.GetTotalYearAvailedLeaveByLeaveDate(id, leaveFrom),
                totalBalance = await leaveRegisterService.GetOpeningBalanceByRegId(id, year),
                presentBal = await leaveRegisterService.GetOpeningBalanceByRegId(id, year)- Convert.ToDecimal(presentUsed),
                //totalBalance = await leaveRegisterService.GetLeaveBalanceByempId(id, Convert.ToInt32(leaveRegList.leaveTypeId)),
                //presentBalance = await leaveRegisterService.GetOpeningBalanceByRegId(id, year) - Convert.ToDecimal(leaveRegList.daysLeave)
                presentBalance = await leaveRegisterService.GetOpeningBalanceByRegId(id, year) - Convert.ToDecimal(expence)
            };
            ViewBag.availedThisYear = AmountInWord.ConvertEnNumToBnWord(Convert.ToInt32(model.availedThisYear).ToString());
            ViewBag.thisTimeBalance = AmountInWord.ConvertEnNumToBnWord(Convert.ToInt32(model.presentBal).ToString());
            ViewBag.availedThisYearByDate = AmountInWord.ConvertEnNumToBnWord(Convert.ToInt32(model.availedThisYearByDate).ToString());
            ViewBag.totalBalance = AmountInWord.ConvertEnNumToBnWord(Convert.ToInt32(model.totalBalance).ToString());
            ViewBag.presentBalance = AmountInWord.ConvertEnNumToBnWord(Convert.ToInt32(model.presentBalance).ToString());
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> LeaveReport(int year, int empId)
        {
            var EmpInfo = await personalInfoService.GetEmployeeInfoById(empId);
            ViewBag.name = EmpInfo.nameEnglish;
            ViewBag.year = year.ToString();
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                leaveReportModels = await leaveRegisterService.GetLeaveReport(year, empId)
            };
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> LeaveReportIndividual_CBF(int empId, string fdate, string tdate)
        {
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                leaveIndividualViewModels = await leaveRegisterService.GetIndividualLeaveReport(empId, fdate, tdate),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(empId),
                visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData()
            };
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> LeaveReportSummary(string year)
        {
            ViewBag.year = year.ToString();
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                leaveSummaryReports = await leaveRegisterService.GetAllLeaveSummaryReport(year)
            };
            return View(model);
        }
        [AllowAnonymous]
        public async Task<IActionResult> PreviewLeave(int idr, string user)
        {
            var userInfos = await userInfo.GetUserInfoByUser(user);

            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            int empId = 0;
            if (EmpInfo != null)
            {
                empId = EmpInfo.Id;
            }
            ViewBag.employeeId = empId;
            var appDetails = await leavePolicyService.GetApprovalDetailsByUser(userInfos.UserId);
            var appDetailsWithLog = new List<AppDetailsWithLog>();

            foreach (var item in appDetails)
            {
                var data = new AppDetailsWithLog
                {
                    approvalDetail = item,
                    regId = idr,
                    leaveStatusLog = await leaveRegisterService.GetStatusLogByAppUserName(item.approver.ApplicationUser.UserName, item.approvalMaster.Id, idr)
                };
                appDetailsWithLog.Add(data);
            }


            var leaveRegList = await leaveRegisterService.GetLeaveRegisterById(idr);
            DateTime leaveYear = Convert.ToDateTime(leaveRegList.leaveFrom);
            var year = leaveYear.Year;
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpId(empId),
                leaveTypelist = await leaveTypeService.GetAllLeaveType(),
                approvalDetail = await approvalService.GetSingleApprovalDetailByEmpAndType(empId, "Leave"),
                leaveDays = await leavePolicyService.GetAllLeaveDay(),
                leaveStatusViewModels = await leavePolicyService.GetLeaveStatus(empId),
                approvalDetails = await leavePolicyService.GetApprovalDetailsByUser(userInfos.UserId),
                appDetailsWithLogs = appDetailsWithLog,
                companies = await eRPCompanyService.GetAllCompany(),
                leaveRegister = await leaveRegisterService.GetLeaveRegisterById(idr),
                leaveReportModels = await leaveRegisterService.GetLeaveReport(year, EmpInfo.Id),
                leaveSupervisorRecomViewModels = await leaveRegisterService.GetSupervisorRecomForLeaveByEmpId(idr, EmpInfo.Id),
                visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData()
            };
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult PreviewLeavePdf(int idr)
        {
            string userName = HttpContext.User.Identity.Name;

            string fileName;
            string host = HttpContext.Request.Host.ToString();
            string scheme = Request.Scheme;
            string url = string.Empty;
            url = $"" + scheme + "://" + host + "/HRPMSLeave/LeaveRegister/PreviewLeave?idr=" + idr + "&user=" + userName;

            string status = myPDF.GeneratePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        public async Task<IActionResult> PreviewApi(int id)
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            int empId = 0;
            if (EmpInfo != null)
            {
                empId = EmpInfo.Id;
            }
            ViewBag.employeeId = empId;

            var appDetails = await leavePolicyService.GetApprovalDetailsByUser(userInfos.UserId);
            var appDetailsWithLog = new List<AppDetailsWithLog>();

            foreach (var item in appDetails)
            {
                var data = new AppDetailsWithLog
                {
                    approvalDetail = item,
                    regId = id,
                    leaveStatusLog = await leaveRegisterService.GetStatusLogByAppUserName(item.approver.ApplicationUser.UserName, item.approvalMaster.Id, id)
                };
                appDetailsWithLog.Add(data);
            }

            var leaveRegList = await leaveRegisterService.GetLeaveRegisterById(id);
            DateTime leaveYear = Convert.ToDateTime(leaveRegList.leaveFrom);
            var year = leaveYear.Year;
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpId(empId),
                leaveTypelist = await leaveTypeService.GetAllLeaveType(),
                approvalDetail = await approvalService.GetSingleApprovalDetailByEmpAndType(empId, "Leave"),
                leaveDays = await leavePolicyService.GetAllLeaveDay(),
                leaveStatusViewModels = await leavePolicyService.GetLeaveStatus(empId),
                approvalDetails = await leavePolicyService.GetApprovalDetailsByUser(userInfos.UserId),
                appDetailsWithLogs = appDetailsWithLog,
                companies = await eRPCompanyService.GetAllCompany(),
                leaveRegister = await leaveRegisterService.GetLeaveRegisterById(id),
                leaveReportModels = await leaveRegisterService.GetLeaveReport(year, EmpInfo.Id),
                leaveSupervisorRecomViewModels = await leaveRegisterService.GetSupervisorRecomForLeaveByEmpId(id, EmpInfo.Id),
                visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData()
            };
            return Json(model);
        }

        //PrintPDF
        [AllowAnonymous]
        public IActionResult LeaveViewPdf(int id)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;

            string url = scheme + "://" + host + "/HRPMSLeave/LeaveRegister/LeavePrintView/" + id;
            string status = myPDF.GeneratePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [AllowAnonymous]
        public async Task<IActionResult> LeavePrintView(int id)
        {
            //string userName = HttpContext.User.Identity.Name;
            //var userInfos = await userInfo.GetUserInfoByUser(userName);
            //var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            //int empId = 0;
            //if (EmpInfo != null)
            //{
            //    empId = EmpInfo.Id;
            //}


            var leaveRegList = await leaveRegisterService.GetLeaveRegisterById(id);
            DateTime leaveYear = Convert.ToDateTime(leaveRegList.leaveFrom);
            var year = leaveYear.Year;
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                leaveRegister = await leaveRegisterService.GetLeaveRegisterById(id),
                leaveReportModels = await leaveRegisterService.GetLeaveReport(year, Convert.ToInt32(leaveRegList.employeeId)),
                leaveSupervisorRecomViewModels = await leaveRegisterService.GetSupervisorRecomForLeaveByEmpId(id, Convert.ToInt32(leaveRegList.employeeId))

            };
            return View(model);
        }

        public async Task<IActionResult> AllIndividualLeave()
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);

            var model = new LeaveViewModel
            {
                leaveRegisters = await leaveRegisterService.GetAllLeaveByUserId(user.userId)
            };
            return View(model);
        }
        [AllowAnonymous]
        public async Task<IActionResult> GeneralLeaveSummaryReport(string id)
        {
            var employee = await leaveRegisterService.GetGeneralLeaveSummarybyId(id);
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employee = await leaveRegisterService.GetGeneralLeaveSummarybyId(id),
                AverageLeave = await leaveRegisterService.GetLeaveCountByTypeShortNameAndEmpId("AverageLeave", employee.Id),
                FullLeave = await leaveRegisterService.GetLeaveCountByTypeShortNameAndEmpId("FullLeave", employee.Id),
                LeaveWithoutPay = await leaveRegisterService.GetLeaveCountByTypeShortNameAndEmpId("LeaveWithoutPay", employee.Id),
                ExtraOrdinaryLeave = await leaveRegisterService.GetLeaveCountByTypeShortNameAndEmpId("ExtraOrdinaryLeave", employee.Id),
                RecreationLeave = await leaveRegisterService.GetReCreationLeaveCountByEmpId("Recreation Leave", employee.Id)
            };
            return View(model);
        }
        [AllowAnonymous]
        public IActionResult GeneralLeaveSummaryReportPDF(string id)
        {
            string schema = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string filename;

            url = schema+"://" + host + "/HRPMSLeave/LeaveRegister/GeneralLeaveSummaryReport?id=" + id;

            string status = myPDF.GeneratePDF(out filename, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }

        public async Task<IActionResult> CheckIsLeaved()
        {
            string msg = "valid";
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var emp = await personalInfoService.GetEmployeeInfoByCode(user.EmpCode);
            var recreationleave = await leaveRegisterService.GetLeavesByType(16, emp.Id); //recreation leave type id
            var lastleaveyear = Convert.ToDateTime(recreationleave.FirstOrDefault()?.createdAt).Year;
            if (lastleaveyear == DateTime.Now.Year)
            {
                msg = "same year";
            }
            else if (lastleaveyear + 1 == DateTime.Now.Year)
            {
                msg = "one year";
            }
            else if (lastleaveyear + 2 == DateTime.Now.Year)
            {
                msg = "two year";
            }
            else
            {
                msg = "valid";
            }
            return Json(msg);
        }

        public async Task<IActionResult> CheckMaternityLeave()
        {
            string msg = "valid";
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var emp = await personalInfoService.GetEmployeeInfoByCode(user.EmpCode);
            var maternityLeave = await leaveRegisterService.GetLeavesByType(3, emp.Id); //maternity leave type id
            if (emp.gender != "Female")
            {
                msg = "male";
            }
            else if (maternityLeave.Count() >= 2)
            {
                msg = "two";
            }
            else
            {
                msg = "valid";
            }
            return Json(msg);
        }


        #endregion

        #region API Section
        [Route("global/api/GetLeaveBalanceByempId/{id}/{statusId}")]
        [HttpGet]
        public async Task<IActionResult> GetLeaveBalanceByempId(int id, int statusId)
        {
            return Json(await leaveRegisterService.GetLeaveBalanceByempId(id, statusId));
        }

        [Route("global/api/GetLeaveReport/{year}/{empId}")]
        [HttpGet]
        public async Task<IActionResult> GetLeaveReport(int year, int empId)
        {
            return Json(await leaveRegisterService.GetLeaveReport(year, empId));
        }

        [Route("global/api/GetLeaveOpeningBalanceByEmpAndYear/{id}/{yearId}")]
        [HttpGet]
        public async Task<IActionResult> GetLeaveOpeningBalanceByEmpAndYear(int id, int yearId)
        {
            return Json(await leavePolicyService.GetLeaveOpeningBalanceByEmpAndYear(id, yearId));
        }

        [Route("global/api/GetAllLeaveStatusLogByLeaveId/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetAllLeaveStatusLogByLeaveId(int id)
        {
            return Json(await leaveStatusLogService.GetAllLeaveStatusLogByLeaveId(id));
        }

        [Route("global/api/GetAllLeaveRegisterByEmpId/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetAllLeaveRegisterByEmpId(int id)
        {
            return Json(await leaveRegisterService.GetAllLeaveRegisterByEmpId(id));
        }

        [Route("global/api/GetAllLeaveRegisterByEmpIdAndDateRange/{id}/{from}/{to}")]
        [HttpGet]
        public async Task<IActionResult> GetAllLeaveRegisterByEmpIdAndDateRange(int id, DateTime from, DateTime to)
        {
            return Json(await leaveRegisterService.GetAllLeaveRegisterByEmpIdAndDateRange(id, from, to));
        }

        [AllowAnonymous]
        [Route("global/api/SendEmailWithFrom/{email}/{from}/{subject}/{body}")]
        [HttpGet]
        public IActionResult SendEmailWithFrom(string email, string from, string subject, string body)
        {
            emailSenderService.SendEmailWithFrom(email, from, subject, body);
            return Ok("SuccessFull");
        }

        [Route("global/api/GetLeavePolicyByTypeandYear/{leaveTypeId}")]
        [HttpGet]
        public async Task<IActionResult> GetLeavePolicyByTypeandYear(int leaveTypeId)
        {
            var year = DateTime.Now.Year;
            var yearList = await yearCourseTitleService.GetYearByName(year.ToString());
            int yearId = yearList.Id;
            return Json(await leavePolicyService.GetLeavePolicyByTypeandYear(leaveTypeId, yearId));
        }

        #endregion

        #region MobileAPI Section

        [Route("global/api/GetUserAllInfo")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserAllInfo()
        {
            var userInfos = await userInfo.GetUserInfo();

            return Json(userInfos);
        }

        [Route("global/api/GetUserInfoByUser/{userName}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserInfoByUser(string userName)
        {
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            return Json(EmpInfo);
        }


        [Route("global/api/GetLeaveBalanceById/{id}/{statusId}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetLeaveBalanceById(int id, int statusId)
        {
            return Json(await leaveRegisterService.GetLeaveBalanceByempId(id, statusId));
        }

        [Route("global/api/GetLeaveOpeningBalanceEmpIDAndYear/{id}/{yearId}")]
        [HttpGet]
        public async Task<IActionResult> GetLeaveOpeningBalanceByEmpIDAndYear(int id, int yearId)
        {
            return Json(await leavePolicyService.GetLeaveOpeningBalanceByEmpAndYear(id, yearId));
        }

        [Route("global/api/GetAllLeaveStatusByLeaveid/{id}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllLeaveStatusLogByLeaveid(int id)
        {
            return Json(await leaveStatusLogService.GetAllLeaveStatusLogByLeaveId(id));
        }

        [Route("global/api/GetAllLeaveApplyEmpid/{id}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllLeaveRegisterByEmpid(int id)
        {
            return Json(await leaveRegisterService.GetAllLeaveRegisterByEmpId(id));
        }

        [Route("global/api/GetAllLeaveRegisterEmpIdandDate/{id}/{from}/{to}")]
        [HttpGet]
        public async Task<IActionResult> GetAllLeaveRegisterByEmpIdandDateRange(int id, DateTime from, DateTime to)
        {
            return Json(await leaveRegisterService.GetAllLeaveRegisterByEmpIdAndDateRange(id, from, to));
        }


        [Route("global/api/GetApprovarByEmpId/{id}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetSupervisorByEmpId(int id)
        {
            return Json(await approvalService.GetApprovalDetailByEmpAndType(id, "Leave"));
        }




        [Route("global/api/SendEmailWithFromBody/{email}/{from}/{subject}/{body}")]
        [HttpGet]
        public IActionResult SendEmailWithFromBody(string email, string from, string subject, string body)
        {
            emailSenderService.SendEmailWithFrom(email, from, subject, body);
            return Ok("SuccessFull");
        }

        //Leave Status

        [Route("global/api/GetAllLeaveReject/{empId}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllLeaveReject(int empId)
        {
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpIdAndStatus(empId, 5)
            };
            return Json(model.leaveRegisterslist);
        }

        [Route("global/api/GetAllLeaveReturn/{empId}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllLeaveReturn(int empId)
        {
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {

                leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpIdAndStatus(empId, 4)
            };
            return Json(model.leaveRegisterslist);
        }


        [Route("global/api/GetAllLeaveCancle/{empId}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllLeaveCancle(int empId)
        {
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpIdAndStatus(empId, 6)
            };
            return Json(model.leaveRegisterslist);
        }

        // POST: LeaveRegister/Edit
        [HttpPost]
        [AllowAnonymous]
        [Route("api/LeaveApplybyEmo")]
        public async Task<IActionResult> LeaveApplybyEmo([FromBody] LeaveRegisterViewModel model)
        {
            string userName = model.userName;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            int empId = 0;
            string email = "";
            string name = "";
            if (EmpInfo != null)
            {
                empId = EmpInfo.Id;
                email = EmpInfo.emailAddress;
                name = EmpInfo.nameEnglish;
            }
            //return Json(model);
            if (empId == 0)
            {
                return Json("Invalid Employee !! please check your employeeId..");
            }
            var leaveCheck = await leaveRegisterService.GetAllLeaveRegisterByEmpIdAndDate(empId, model.leaveFrom, model.leaveTo);
            //return Json(leaveCheck);

            if (!ModelState.IsValid || leaveCheck.Count() >= 0)
            {
                model.leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpId(empId);
                model.leaveTypelist = await leaveTypeService.GetAllLeaveType();
                model.supervisor = await supervisorService.GetFirstSupervisorByEmpId(empId);
                model.fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]);
                if (leaveCheck.Count() > 0)
                {
                    ModelState.AddModelError(string.Empty, "Another Leave has already applied in same Date !!");
                }
                // return Json("Dont left any leave !!");
            }
            var subEmpId = await personalInfoService.GetEmployeeIdByCode(model.substitutionEmpCode);
            LeaveRegister data = new LeaveRegister
            {
                //Id = model.id,
                employeeId = EmpInfo.Id,
                leaveTypeId = model.leaveTypeId,
                whenLeave = model.whenLeave,
                leaveStatus = 1,
                leaveFrom = model.leaveFrom,
                leaveTo = model.leaveTo,
                daysLeave = model.daysLeave,
                purposeOfLeave = model.purposeOfLeave,
                emergencyContactNo = model.emergencyContactNo,
                address = model.address,
                substitutionUserId = subEmpId
            };

            int id = await leaveRegisterService.SaveLeaveRegister(data);

            IEnumerable<ApprovalDetail> approvalDetails = await approvalService.GetApprovalDetailByEmpAndType(EmpInfo.Id, "Leave");

            var i = 1;
            var Active = 1;
            foreach (ApprovalDetail supervisor in approvalDetails)
            {
                LeaveRoute leaveRoute = new LeaveRoute
                {
                    leaveRegisterId = id,
                    employeeId = supervisor.approverId,
                    routeOrder = i,
                    isActive = Active,
                };
                await leaveRouteService.SaveLeaveRoute(leaveRoute);
                i++;
                Active = 0;
            }

            LeaveStatusLog leaveStatusLog = new LeaveStatusLog
            {
                employeeId = EmpInfo.Id,
                leaveRegisterId = id,
                date = DateTime.Now,
                remarks = "On Approval",
                Status = 1
            };
            await leaveStatusLogService.SaveLeaveStatusLog(leaveStatusLog);

            var ActiveLeave = await leaveRouteService.GetLeaveRouteByLeaveRegisterId(id);

            string html1 = "<div><strong>Leave Application.</strong></div>"
                + " <br/>"
                + "<p>Dear Sir,</p>"
                + " <br/>"
                + " This is to inform you that you have sent one leave application to " + ActiveLeave?.employee?.nameEnglish + " just now."
                + "<br/>"

                + "<div><p> Thank You & Best Regards</p><p style = 'font-weight:bold' > Human Resource Department.</p></div>"
                + " <br/>";

            string htmlShow = ActiveLeave?.employee?.nameEnglish;

            string host = HttpContext.Request.Host.ToString();
            string scheme = Request.Scheme;
            string baseUrl = $"" + scheme + "://" + host + "/HRPMSLeave/LeaveApproval";

            string html = "<div><strong>Leave Recommendation/Approval.</strong></div>"
                    + " <br/>"
                    + "<p>Dear Sir,</p>"
                    + " <br/>"
                    + " This is to inform you that one leave application is waiting for your recommendation/approval."
                    + "<br/>"
                    + "<div><a href='" + baseUrl + "'><button>Login</button></a></div>"
                    + " <br/>"
                    + "<div><p> Thank You & Best Regards</p><p style = 'font-weight:bold' > Human Resource Department.</p></div>"
                    + " <br/>";

            //  await emailSenderService.SendEmailWithFrom(email, name, "Leave Application", html1);
            //await emailSenderService.SendEmailWithFrom(ActiveLeave?.employee?.emailAddress, ActiveLeave?.employee?.nameEnglish, "Leave Application", html);

			if (id > 0)
			{
				return Json("success");
			}
			else
			{
				return Json("error");
			}
		}
        #endregion

        #region Leave
        //public async Task<IActionResult> PendingApproval()
        //{
        //	string userName = HttpContext.User.Identity.Name;
        //	var user = await _userManager.FindByNameAsync(userName);

        //	var model = new LeaveViewModel
        //	{
        //		leaveRegisters = await leaveRegisterService.GetAllLeaveByUserIdAndStatus(user.userId, 1)
        //	};
        //	return View(model);
        //}
        public async Task<IActionResult> PendingApproval(int? id)
        {
            ViewBag.masterId = id;

            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            int empId = 0;
            if (EmpInfo != null)
            {
                empId = EmpInfo.Id;
            }
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpIdAndStatus(empId, 1)
            };
            return View(model);
        }

        [HttpGet("api/global/MyPendingLeave/{id}")]
        public async Task<IActionResult> MyPendingLeaves(int id) {
            var leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpIdAndStatus(id, 1);
            return Json(leaveRegisterslist);
        }


        public async Task<IActionResult> TodayLeave()
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);

            var model = new LeaveViewModel
            {
                leaveRegisters = await leaveRegisterService.GetTodayAprovedLeave(user.userId, 3)
            };
            return View(model);
        }
        //      public async Task<IActionResult> ReturnApproval(int? id)
        //{
        //	ViewBag.masterId = id;
        //	string userName = HttpContext.User.Identity.Name;
        //	var user = await _userManager.FindByNameAsync(userName);

        //	var model = new LeaveViewModel
        //	{
        //		leaveRegisters = await leaveRegisterService.GetAllLeaveByUserIdAndStatus(user.userId, 4)
        //	};
        //	return View(model);
        //}
        public async Task<IActionResult> ReturnApproval(int? id)
        {
            ViewBag.masterId = id;

            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            int empId = 0;
            if (EmpInfo != null)
            {
                empId = EmpInfo.Id;
            }
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpIdAndStatus(empId, 4)
            };
            return View(model);
        }


        public async Task<IActionResult> AvailedLeave()
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);

            var model = new LeaveViewModel
            {
                leaveRegisters = await leaveRegisterService.GetAvailedLeaveByUser(user.userId)
            };
            return View(model);
        }


        //public async Task<IActionResult> ApprovedIndLeave(int? id)
        //{
        //	ViewBag.masterId = id;
        //	string userName = HttpContext.User.Identity.Name;
        //	var user = await _userManager.FindByNameAsync(userName);

        //	var model = new LeaveViewModel
        //	{
        //		leaveRegisters = await leaveRegisterService.GetAllLeaveByUserIdAndStatus(user.userId, 3)
        //	};
        //	return View(model);
        //}
        public async Task<IActionResult> ApprovedIndLeave(int? id)
        {
            ViewBag.masterId = id;

            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            int empId = 0;
            if (EmpInfo != null)
            {
                empId = EmpInfo.Id;
            }
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpIdAndStatus(empId, 3)
            };
            return View(model);
        }


        public async Task<IActionResult> CancelLeave()
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                var user = await _userManager.FindByNameAsync(userName);

                var model = new LeaveViewModel
                {
                    leaveRegisters = await leaveRegisterService.GetAllLeaveRegisterByUser(user.userId)
                };
                return View(model);
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public async Task<IActionResult> CancelLeaveByRegisterId(int id)
        {
            //string userName = HttpContext.User.Identity.Name;
            //var user = await _userManager.FindByNameAsync(userName);
            var leaveReg = await leaveRegisterService.GetLeaveRegisterById(id);
            if (leaveReg.leaveStatus != 3)
            {
                await leaveRegisterService.DeleteLeaveStatusByRegId(id);
                await leaveRegisterService.DeleteLeaveRouteByRegId(id);
                await leaveRegisterService.DeleteLeaveRegisterById(id);
                return Json("cancelled");
            }
            else
            {
                return Json("approved");
            }
        }

        public async Task<IActionResult> GetEmpNameByUserName(string username)
        {
            return Json(await leaveRegisterService.GetEmpNameByUserName(username));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/GetAllLeaveTypes")]
        public async Task<IActionResult> GetAllLeaveTypes()
        {
            var data = await leaveTypeService.GetAllLeaveType();
            return Json(data);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/GetLeaveStatusByEmpIdApi/{id}")]
        public async Task<IActionResult> GetLeaveStatusByEmpIdApi(int id)
        {
            var data = await leavePolicyService.GetLeaveStatus(id);
            return Json(data);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/GetApprovedLeaveByApproverEmpIdApi/{id}")]
        public async Task<IActionResult> GetApprovedLeaveByApproverEmpIdApi(int id)
        {
            var user = await personalInfoService.GetUserIdByEmpId(id);
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                leaveRegisterslist = await leaveRegisterService.GetAllApproveLeaveByUserIdStatus(user.userId, 2)
            };
            return Json(model.leaveRegisterslist);
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("api/GetReturnedLeaveByApproverEmpIdApi/{id}")]
        public async Task<IActionResult> GetReturnedLeaveByApproverEmpIdApi(int id)
        {
            var user = await personalInfoService.GetUserIdByEmpId(id);
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                leaveRegisterslist = await leaveRegisterService.GetAllApproveLeaveByUserIdStatus(user.userId, 4)

            };
            return Json(model.leaveRegisterslist);
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("api/GetRejectedLeaveByApproverEmpIdApi/{id}")]
        public async Task<IActionResult> GetRejectedLeaveByApproverEmpIdApi(int id)
        {
            var user = await personalInfoService.GetUserIdByEmpId(id);
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                leaveRegisterslist = await leaveRegisterService.GetAllApproveLeaveByUserIdStatus(user.userId, 5)

            };
            return Json(model.leaveRegisterslist);
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("api/GetAllLeaveByApproverEmpIdApi2/{id}")]
        public async Task<IActionResult> GetAllLeaveByApproverEmpIdApi(int id)
        {
            var user = await personalInfoService.GetUserIdByEmpId(id);
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                leaveRegisterslist = await leaveRegisterService.GetAllApproveLeaveByUserId(user.userId)

            };
            return Json(model.leaveRegisterslist);
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("api/GetPendingLeaveByApproverEmpIdApi/{id}")]
        public async Task<IActionResult> GetPendingLeaveByApproverEmpIdApi(int id)
        {
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                leaveRoutes = await leaveRouteService.GetLeaveRouteByEmpId(id)
            };
            return Json(model.leaveRoutes);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ApproveLeaveAPI([FromBody] LeaveApproveApiViewModel model)
        {
            var leaveRegister = await leaveRegisterService.GetLeaveRegisterById(model.leaveRegisterId);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(leaveRegister.employee?.employeeCode);

            int Status = 2;
            string mailtext = "recommended";

            LeaveRoute leaveRoute = await leaveRouteService.GetLeaveRouteById(model.leaveRouteId);
            int? nextOrder = leaveRoute.routeOrder + 1;
            await leaveRouteService.UpdateLeaveRoute(leaveRoute.Id, 0);
            var registerLeave = await leaveRegisterService.CheckFinalByEmpIdAndRegId(leaveRegister.employeeId, model.loginuserid);
            if (registerLeave.isDelete != 1)
            {
                Status = 3;
            }

            LeaveStatusLog leaveStatusLog = new LeaveStatusLog
            {
                employeeId = leaveRegister.employeeId,
                leaveRegisterId = leaveRegister.Id,
                date = DateTime.Now,
                remarks = "Approved",
                Status = Status,
                comments = model.comments
            };
            await leaveStatusLogService.SaveLeaveStatusLog(leaveStatusLog);


            string host = HttpContext.Request.Host.ToString();
            string scheme = Request.Scheme;
            string baseUrl = $"" + scheme + "://" + host + "/HRPMSLeave/LeaveApproval";

            string htmlApprove = "<div><strong>Leave Recommendation/Approval.</strong></div>"
                    + " <br/>"
                    + "<p>Dear Sir,</p>"
                    + " <br/>"
                    + " This is to inform you that one leave application is waiting for your recommendation/approval."
                    + "<br/>"
                    + "<div><a href='" + baseUrl + "'><button>Login</button></a></div>"
                    + " <br/>"
                    + "<div><p> Thank You & Best Regards</p><p style = 'font-weight:bold' > Human Resource Department.</p></div>"
                    + " <br/>";

            var employee = await personalInfoService.GetEmployeeInfoById((int)leaveRegister.employeeId);
            var leaveEmployee = await personalInfoService.GetEmployeeInfoById((int)leaveRegister.employeeId);

            string html1 = "<div><strong>Leave Application.</strong></div>"
                        + " <br/>"
                        + "<p>Dear Sir,</p>"
                        + " <br/>"
                        + " This is to inform you that your leave application has " + mailtext + " by " + employee.nameEnglish + " just now."
                        + "<br/>"

                        + "<div><p> Thank You & Best Regards</p><p style = 'font-weight:bold' > Human Resource Department.</p></div>"
                        + " <br/>";




            LeaveRoute leaveRouteNew = await leaveRouteService.GetLeaveRouteByRouteOrder((int)leaveRoute.leaveRegisterId, (int)nextOrder);

            if (leaveRouteNew != null)
            {
                //Save Notification
                var notification = new Notification
                {
                    Id = 0,
                    senderId = leaveRegister.employee.ApplicationUserId,
                    receiverId = leaveRouteNew.employee.ApplicationUserId,
                    date = DateTime.Now,
                    title = "Leave application from " + leaveRegister.employee.nameEnglish,
                    description = "Leave application from " + leaveRegister.employee.nameEnglish + " approved by " + EmpInfo?.nameEnglish,
                    type = "Leave",
                    isRead = 0,
                    url = "/HRPMSLeave/LeaveApproval/Index/" + leaveRegister.Id,
                    status = 1,
                    isDelete = leaveRegister.Id //registerId
                };
                await requisitionService.SaveNotification(notification);

                var leaveFutureEmployee = await personalInfoService.GetEmployeeInfoById((int)leaveRouteNew?.employeeId);
                await leaveRouteService.UpdateLeaveRoute(leaveRouteNew.Id, 1);
                await leaveRegisterService.UpdateLeaveApproval(leaveRegister.Id, Status);
                if (leaveEmployee.emailAddress != null)
                {
                    //await emailSenderService.SendEmailWithFrom(leaveEmployee.emailAddress, employee.nameEnglish, "Leave Application", html1);
                }

                if (leaveFutureEmployee.emailAddress != null)
                {
                    //await emailSenderService.SendEmailWithFrom(leaveFutureEmployee.emailAddress, employee.nameEnglish, "Leave Application", htmlApprove);
                }
            }
            else
            {
                await leaveRegisterService.UpdateLeaveApproval(leaveRegister.Id, 3);

                //Save Notification
                var notification = new Notification
                {
                    Id = 0,
                    senderId = leaveRegister.employee.ApplicationUserId,
                    receiverId = leaveRegister.employee.ApplicationUserId,
                    date = DateTime.Now,
                    title = "Leave Approved Finally",
                    description = "Your leave is approved finally by " + EmpInfo?.nameEnglish,
                    type = "Leave",
                    isRead = 0,
                    url = "/HRPMSLeave/LeaveRegister/ApprovedIndLeave/" + leaveRegister.Id,
                    status = 1,
                    isDelete = leaveRegister.Id //registerId
                };
                await requisitionService.SaveNotification(notification);

                string html = "<div><strong>Leave application finally approved.</strong></div>"
                            + " <br/>"
                            + "Your requested leave application (Start date:" + leaveRegister.leaveFrom?.ToString("dd-MMM-yyyy") + ") has been finally approved."
                            + "<br/><br/>"
                            + "<b><u>Application Details</u></b>"
                            + " <br/>"
                            + "Leave Type : " + leaveRegister.leaveType?.nameEn + ""
                            + " <br/>"
                            + "Leave Duration :" + " From " + leaveRegister.leaveFrom?.ToString("dd-MMM-yyyy") + " " + " To " + leaveRegister.leaveTo?.ToString("dd-MMM-yyyy") + " "
                            + " <br/>"
                            + "Approver Remarks : "
                            + "<br/>";
                if (leaveEmployee.emailAddress != null)
                {
                    //await emailSenderService.SendEmailWithFrom(leaveEmployee.emailAddress, employee.nameEnglish, "Leave Application", html);
                }
            }


            return Json("Approved Successfully");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ReturnLeaveAPI([FromBody] LeaveApproveApiViewModel model)
        {
            var userInfo = await personalInfoService.GetUserInfoByUserId(model.loginuserid);
            var leaveRegister = await leaveRegisterService.GetLeaveRegisterById(model.leaveRegisterId);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(leaveRegister.employee?.employeeCode);

            int Status = 4;
            string mailtext = "Returned";


            LeaveRoute leaveRoute = await leaveRouteService.GetLeaveRouteById(model.leaveRouteId);
            int? nextOrder = leaveRoute.routeOrder + 1;
            await leaveRouteService.UpdateLeaveRoute(leaveRoute.Id, 0);
            var registerLeave = await leaveRegisterService.CheckFinalByEmpIdAndRegId(leaveRegister.employeeId, model.loginuserid);
            if (registerLeave.isDelete != 1)
            {
                Status = 1;
            }
            LeaveStatusLog leaveStatusLog = new LeaveStatusLog
            {
                employeeId = leaveRegister.employeeId,
                leaveRegisterId = leaveRegister.Id,
                date = DateTime.Now,
                remarks = "Returned",
                Status = Status,
                comments = model.comments
            };
            await leaveStatusLogService.SaveLeaveStatusLog(leaveStatusLog);

            var notification = new Notification
            {
                Id = 0,
                senderId = userInfo.Id,
                receiverId = leaveRegister.employee.ApplicationUser.Id,
                date = DateTime.Now,
                title = "Leave returned by " + userInfo.UserName,
                description = userInfo.UserName + " returned your leave application",
                type = "Leave",
                isRead = 0,
                url = "/HRPMSLeave/LeaveRegister/ReturnApproval/" + leaveRegister.Id,
                status = 1,
                isDelete = leaveRegister.Id //registerId
            };
            await requisitionService.SaveNotification(notification);

            string host = HttpContext.Request.Host.ToString();
            string scheme = Request.Scheme;
            string baseUrl = $"" + scheme + "://" + host + "/HRPMSLeave/LeaveApproval";

            string htmlApprove = "<div><strong>Leave Recommendation/Approval.</strong></div>"
                    + " <br/>"
                    + "<p>Dear Sir,</p>"
                    + " <br/>"
                    + " This is to inform you that one leave application is waiting for your recommendation/approval."
                    + "<br/>"
                    + "<div><a href='" + baseUrl + "'><button>Login</button></a></div>"
                    + " <br/>"
                    + "<div><p> Thank You & Best Regards</p><p style = 'font-weight:bold' > Human Resource Department.</p></div>"
                    + " <br/>";

            var employee = await personalInfoService.GetEmployeeInfoById((int)leaveRegister.employeeId);
            var leaveEmployee = await personalInfoService.GetEmployeeInfoById((int)leaveRegister.employeeId);

            string html1 = "<div><strong>Leave Application.</strong></div>"
                        + " <br/>"
                        + "<p>Dear Sir,</p>"
                        + " <br/>"
                        + " This is to inform you that your leave application has " + mailtext + " by " + employee.nameEnglish + " just now."
                        + "<br/>"

                        + "<div><p> Thank You & Best Regards</p><p style = 'font-weight:bold' > Human Resource Department.</p></div>"
                        + " <br/>";


            if (Status == 4)
            {
                await leaveRegisterService.UpdateLeaveApproval((int)model.leaveRegisterId, Status);

                if (leaveEmployee.emailAddress != null)
                {
                    //await emailSenderService.SendEmailWithFrom(leaveEmployee.emailAddress, employee.nameEnglish, "Leave Application", html1);
                }

                return Json("Returned Successfully");
            }

            LeaveRoute leaveRouteNew = await leaveRouteService.GetLeaveRouteByRouteOrder((int)leaveRoute.leaveRegisterId, (int)nextOrder);

            if (leaveRouteNew != null)
            {
                var leaveFutureEmployee = await personalInfoService.GetEmployeeInfoById((int)leaveRouteNew?.employeeId);
                await leaveRouteService.UpdateLeaveRoute(leaveRouteNew.Id, 1);
                await leaveRegisterService.UpdateLeaveApproval((int)model.leaveRegisterId, Status);
                if (leaveEmployee.emailAddress != null)
                {
                    //await emailSenderService.SendEmailWithFrom(leaveEmployee.emailAddress, employee.nameEnglish, "Leave Application", html1);
                }

                if (leaveFutureEmployee.emailAddress != null)
                {
                    //await emailSenderService.SendEmailWithFrom(leaveFutureEmployee.emailAddress, employee.nameEnglish, "Leave Application", htmlApprove);
                }

            }
            else
            {
                await leaveRegisterService.UpdateLeaveApproval((int)model.leaveRegisterId, 3);
                string html = "<div><strong>Leave Application.</strong></div>"
                            + " <br/>"
                            + "<p>Dear Sir,</p>"
                            + " <br/>"
                            + " This is to inform you that "
                            + "leave application"
                            + " <br/>"
                            + "Employee Name : '" + leaveEmployee.nameEnglish + "' "
                            + " <br/>"
                            + "Leave From : '" + leaveRegister.leaveFrom?.ToString("dd-MMM-yyyy") + "' " + " To : '" + leaveRegister.leaveTo?.ToString("dd-MMM-yyyy") + "' "
                            + " <br/>"
                            + "has approved by " + employee.nameEnglish + " just now."
                            + "<br/>"
                            + "<br/>"

                            + "<div><p> Thank You & Best Regards</p><p style = 'font-weight:bold' > Human Resource Department.</p></div>"
                            + " <br/>";
                if (leaveEmployee.emailAddress != null)
                {
                    //await emailSenderService.SendEmailWithFrom(leaveEmployee.emailAddress, employee.nameEnglish, "Leave Application", html);
                }
            }


            return Json("Returned Successfully");
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RejectLeaveAPI([FromBody] LeaveApproveApiViewModel model)
        {
            var userInfo = await personalInfoService.GetUserInfoByUserId(model.loginuserid);
            LeaveRoute leaveRoute = await leaveRouteService.GetLeaveRouteById(model.leaveRouteId);
            int? nextOrder = leaveRoute.routeOrder + 1;
            var leaveRegister = await leaveRegisterService.GetLeaveRegisterById(model.leaveRegisterId);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(leaveRegister.employee?.employeeCode);

            int Status = 5;
            string mailtext = "Rejected";


            LeaveStatusLog leaveStatusLog = new LeaveStatusLog
            {
                employeeId = leaveRegister.employeeId,
                leaveRegisterId = model.leaveRegisterId,
                date = DateTime.Now,
                remarks = "Rejected",
                Status = Status,
                comments = model.comments
            };
            await leaveStatusLogService.SaveLeaveStatusLog(leaveStatusLog);

            await leaveStatusLogService.UpdateLeaveRegisterStatus(model.leaveRegisterId, 5);

            var notification = new Notification
            {
                Id = 0,
                senderId = userInfo.Id,
                receiverId = leaveRegister.employee.ApplicationUser.Id,
                date = DateTime.Now,
                title = "Leave returned by " + userInfo.UserName,
                description = userInfo.UserName + " rejected your leave application",
                type = "Leave",
                isRead = 0,
                url = "/HRPMSLeave/LeaveRegister/LeaveReject/" + leaveRegister.Id,
                status = 1,
                isDelete = leaveRegister.Id //registerId
            };
            await requisitionService.SaveNotification(notification);


            string host = HttpContext.Request.Host.ToString();
            string scheme = Request.Scheme;
            string baseUrl = $"" + scheme + "://" + host + "/HRPMSLeave/LeaveApproval";

            string htmlApprove = "<div><strong>Leave Recommendation/Approval.</strong></div>"
                    + " <br/>"
                    + "<p>Dear Sir,</p>"
                    + " <br/>"
                    + " This is to inform you that one leave application is waiting for your recommendation/approval."
                    + "<br/>"
                    + "<div><a href='" + baseUrl + "'><button>Login</button></a></div>"
                    + " <br/>"
                    + "<div><p> Thank You & Best Regards</p><p style = 'font-weight:bold' > Human Resource Department.</p></div>"
                    + " <br/>";

            var employee = await personalInfoService.GetEmployeeInfoById((int)leaveRegister.employeeId);
            var leaveEmployee = await personalInfoService.GetEmployeeInfoById((int)leaveRegister.employeeId);

            string html1 = "<div><strong>Leave Application.</strong></div>"
                        + " <br/>"
                        + "<p>Dear Sir,</p>"
                        + " <br/>"
                        + " This is to inform you that your leave application has " + mailtext + " by " + employee.nameEnglish + " just now."
                        + "<br/>"

                        + "<div><p> Thank You & Best Regards</p><p style = 'font-weight:bold' > Human Resource Department.</p></div>"
                        + " <br/>";

            if (Status == 5)
            {
                await leaveRegisterService.UpdateLeaveApproval((int)model.leaveRegisterId, Status);

                if (leaveEmployee.emailAddress != null)
                {
                    //await emailSenderService.SendEmailWithFrom(leaveEmployee.emailAddress, employee.nameEnglish, "Leave Application", html1);
                }

                return Json("Rejected Successfully");
            }

            LeaveRoute leaveRouteNew = await leaveRouteService.GetLeaveRouteByRouteOrder((int)leaveRoute.leaveRegisterId, (int)nextOrder);

            if (leaveRouteNew != null)
            {
                var leaveFutureEmployee = await personalInfoService.GetEmployeeInfoById((int)leaveRouteNew?.employeeId);
                await leaveRouteService.UpdateLeaveRoute(leaveRouteNew.Id, 1);
                await leaveRegisterService.UpdateLeaveApproval((int)model.leaveRegisterId, Status);
                if (leaveEmployee.emailAddress != null)
                {
                    //await emailSenderService.SendEmailWithFrom(leaveEmployee.emailAddress, employee.nameEnglish, "Leave Application", html1);
                }

                if (leaveFutureEmployee.emailAddress != null)
                {
                    //await emailSenderService.SendEmailWithFrom(leaveFutureEmployee.emailAddress, employee.nameEnglish, "Leave Application", htmlApprove);
                }

            }
            else
            {
                await leaveRegisterService.UpdateLeaveApproval((int)model.leaveRegisterId, 3);
                string html = "<div><strong>Leave Application.</strong></div>"
                            + " <br/>"
                            + "<p>Dear Sir,</p>"
                            + " <br/>"
                            + " This is to inform you that "
                            + "leave application"
                            + " <br/>"
                            + "Employee Name : '" + leaveEmployee.nameEnglish + "' "
                            + " <br/>"
                            + "Leave From : '" + leaveRegister.leaveFrom?.ToString("dd-MMM-yyyy") + "' " + " To : '" + leaveRegister.leaveTo?.ToString("dd-MMM-yyyy") + "' "
                            + " <br/>"
                            + "has approved by " + employee.nameEnglish + " just now."
                            + "<br/>"
                            + "<br/>"

                            + "<div><p> Thank You & Best Regards</p><p style = 'font-weight:bold' > Human Resource Department.</p></div>"
                            + " <br/>";
                if (leaveEmployee.emailAddress != null)
                {
                    //await emailSenderService.SendEmailWithFrom(leaveEmployee.emailAddress, employee.nameEnglish, "Leave Application", html);
                }
            }


            return Json("Rejected Successfully");
        }





        [HttpGet]
        [AllowAnonymous]
        [Route("api/global/PendingApprovalApi/{empCode}")]
        public async Task<IActionResult> PendingApprovalApi(string empCode)
        {
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(empCode);
			var data = await leaveRegisterService.GetAllLeaveRegisterByEmpIdAndStatus(EmpInfo.Id, 1);
			return Json(data);
		}

		[HttpGet]
		[AllowAnonymous]
		[Route("api/global/MyApprovedLeave/{empCode}")]
		public async Task<IActionResult> ApprovedIndLeaveApi(string empCode)
		{
			var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(empCode);
			var data = await leaveRegisterService.GetAllLeaveRegisterByEmpIdAndStatus(EmpInfo.Id, 3);
			return Json(data);

		}

		[HttpGet]
		[AllowAnonymous]
		[Route("api/global/LeaveApprovalMatrixByEmpCode/{empCode}")]
		public async Task<IActionResult> LeaveApprovalMatrixByEmpCode(string empCode)
		{
			var EmpInfo = await personalInfoService.GetEmployeeInfoByCode1(empCode);
			var data = await leavePolicyService.GetApprovalDetailsByUser(Convert.ToInt32(EmpInfo.ApplicationUser?.userId));
			return Json(data);

		}

		[HttpGet]
        [AllowAnonymous]
        [Route("api/ReturnApprovalApi/{id}")]
        public async Task<IActionResult> ReturnApprovalApi(int? id)
        {
            ViewBag.masterId = id;

            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            int empId = 0;
            if (EmpInfo != null)
            {
                empId = EmpInfo.Id;
            }
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                //fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpIdAndStatus(empId, 4)
            };
            if (model.leaveRegisterslist == null)
            {
                return Json(null);
            }
            else
            {
                return Json(model.leaveRegisterslist);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/LeaveRejectApi/{id}")]
        public async Task<IActionResult> LeaveRejectApi(int? id)
        {
            ViewBag.masterId = id;

            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            int empId = 0;
            if (EmpInfo != null)
            {
                empId = EmpInfo.Id;
            }
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                //fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpIdAndStatus(empId, 5)
            };
            if (model.leaveRegisterslist == null)
            {
                return Json(null);
            }
            else
            {
                return Json(model.leaveRegisterslist);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/LeaveStatusApi/{userName}")]
        public async Task<IActionResult> LeaveStatusApi(string userName)
        {
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            int empId = 0;
            if (EmpInfo != null)
            {
                empId = EmpInfo.Id;
            }
            ViewBag.employeeId = empId;

            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                //fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpId(empId),
                leaveTypelist = await leaveTypeService.GetAllLeaveType(),
                approvalDetail = await approvalService.GetSingleApprovalDetailByEmpAndType(empId, "Leave"),
                leaveDays = await leavePolicyService.GetAllLeaveDay(),
                leaveStatusViewModels = await leavePolicyService.GetLeaveStatus(empId),
                approvalDetails = await leavePolicyService.GetApprovalDetailsByUser(userInfos.UserId)
            };
            return Json(model);
        }


        #endregion

        #region  Leave Report
        
        public async Task <IActionResult> AllLeaveReport()
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var emp = await personalInfoService.GetEmployeeInfoByCode(user.EmpCode);
            ViewBag.empcode=emp.employeeCode;
            ViewBag.employeename=emp.nameEnglish;
            var model = new LeaveViewModel
            {
                //leaveRegisters = await leaveRegisterService.GetAllLeaveByUserId(user.userId)
            };
            return View(model);
        }
        #endregion


        #region Proposed-Recreation Leave

        public async Task<IActionResult> ProposedRecreationLeaveApply()
        {

            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
            int empId = 0;
            if (EmpInfo != null)
            {
                empId = EmpInfo.Id;
            }
            ViewBag.employeeId = empId;
            ViewBag.Code = EmpInfo.employeeCode;
            ViewBag.nameEnglish = EmpInfo.nameEnglish;
            ViewBag.designation = EmpInfo.lastDesignation?.designationName;
            ViewBag.postingPlace = EmpInfo.presentPosting;
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),

                leaveRegisterslist = await leaveRegisterService.ProposedRecrationListForAppList(empId),
                leaveTypelist = await leaveTypeService.GetAllLeaveType(),
                approvalDetail = await approvalService.GetSingleApprovalDetailByEmpAndType(empId, "Leave"),
                leaveDays = await leavePolicyService.GetAllLeaveDay(),
                leaveStatusViewModels = await leavePolicyService.GetLeaveStatus(empId),
                approvalDetails = await leavePolicyService.GetApprovalDetailsByUser(userInfos.UserId)
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProposedRecreationApply([FromForm] LeaveRegisterViewModel model)
        {

            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(model.empCode);

           

            int? depId = 0;
            int? brId = 0;
            int? zoneLId = 0;
            if (EmpInfo.departmentId != null && EmpInfo.hrBranchId == 205 && EmpInfo.locationId == null)
            {
                depId = EmpInfo.departmentId;
                brId = 0;
                zoneLId = 0;
            }
            else if (EmpInfo.hrBranchId != null && EmpInfo.departmentId == null && EmpInfo.locationId == null)
            {
                brId = EmpInfo.hrBranchId;
                depId = 0;
                zoneLId = 0;
            }
            else if (EmpInfo.locationId != null && EmpInfo.hrBranchId == null && EmpInfo.departmentId == null)
            {
                zoneLId = EmpInfo.locationId;
                depId = 0;
                brId = 0;
                
            }


            var HeadCheckInfo= await personalInfoService.GetHeadInfoByEmpOrDepartBranchZoneId(EmpInfo.Id, depId, brId, zoneLId);

            if (HeadCheckInfo !=null)
            {
                if (HeadCheckInfo.branchId > 0 && HeadCheckInfo.branchId != 205)  //if Branch Head apply notification send to zone head
                {
                    var UZoneId = await personalInfoService.GetZoneIdByBranchId((int)HeadCheckInfo.branchId);
                    var UserHeadInfo = await personalInfoService.GetHeadByDepartBranchZoneId(0, 0, UZoneId);
                    var userDetails = await personalInfoService.GetEmployeeInfoByCode(userName);
                    await leaveRegisterService.SendNotification(userName, userName, UserHeadInfo.HeadCode, "Proposed Leave From " + userDetails.nameEnglish, "Waiting for Proposed Leave  to Approver", "Proposed", "/HRPMSLeave/LeaveRegister/ProposedLeaveListForHead");

                }
                else if ((HeadCheckInfo.depId == depId && HeadCheckInfo.branchId == 205) || HeadCheckInfo.locationId == zoneLId) //if department/zone  Head apply notification send to HrLeaveAdmin
                {
                    //var UserHeadInfo = await personalInfoService.GetHeadByDepartBranchZoneId(HeadCheckInfo.depId, HeadCheckInfo.branchId, HeadCheckInfo.locationId);
                    //var userDetails = await personalInfoService.GetEmployeeInfoByCode(userName);
                    //await leaveRegisterService.SendNotification(userName, userName, UserHeadInfo.HeadCode, "Proposed Leave From " + userDetails.nameEnglish, "Waiting for Proposed Leave  to Approver", "Proposed", "/HRPMSLeave/LeaveRegister/ProposedLeaveListForHead");

                }
            }
           
            else  //if General Employee   apply notification send to Dep/Br/Zone Head
            {
                var HeadInfo = await personalInfoService.GetHeadByDepartBranchZoneId(depId, brId, zoneLId);

                var userDetails = await personalInfoService.GetEmployeeInfoByCode(userName);
                await leaveRegisterService.SendNotification(userName, userName, HeadInfo.HeadCode, "Proposed Leave From " + userDetails.nameEnglish, "Waiting for Proposed Leave  to Approver", "Proposed", "/HRPMSLeave/LeaveRegister/ProposedLeaveListForHead");

            }





            int empId = 0;
            string email = "";
            string name = "";
            if (EmpInfo != null)
            {
                empId = EmpInfo.Id;
                email = EmpInfo.emailAddress;
                name = EmpInfo.nameEnglish;
            }
            ViewBag.employeeId = empId;

            string fileName = String.Empty;
            string fileNameMain = String.Empty;
            if (model.fileUrl != null)
            {
                string message = FileSave.SaveEmpAttachment(out fileName, model.fileUrl);

                if (message == "success")
                {
                    fileNameMain = fileName;
                }
                else
                {
                    fileNameMain = "";
                }
            }
            else
            {
                fileNameMain = "";
            }

            if (HeadCheckInfo != null) //if department/zone  Head apply application forword to HrLeaveAdmin and status=34 
            {
                if ((HeadCheckInfo.depId > 0 && HeadCheckInfo.branchId == 205)|| HeadCheckInfo.locationId > 0)
                {
                    LeaveRegister data1 = new LeaveRegister
                    {
                        Id = model.id,
                        employeeId = EmpInfo.Id,
                        leaveTypeId = model.leaveTypeId,
                        whenLeave = model.whenLeave,
                        //leaveStatus = 3,//manual leave approve status=3
                        leaveStatus = 34,
                        leaveFrom = model.leaveFrom,
                        leaveTo = model.leaveTo,
                        daysLeave = model.daysLeave,
                        purposeOfLeave = model.purposeOfLeave,
                        emergencyContactNo = model.emergencyContactNo,
                        address = model.address,
                        substitutionUserId = model.substitutionUserId,
                        leaveDayId = model.leaveDayId,
                        fileUrl = fileNameMain,
                        paymentOption = "Proposed Recreation"
                    };

                     await leaveRegisterService.SaveLeaveRegister(data1);
                }

                //if branch Head apply application forword to Zone head and status=1
                else if (HeadCheckInfo.branchId > 0 && HeadCheckInfo.branchId != 205) 
                {
                    LeaveRegister data = new LeaveRegister
                    {
                        Id = model.id,
                        employeeId = EmpInfo.Id,
                        leaveTypeId = model.leaveTypeId,
                        whenLeave = model.whenLeave,
                        //leaveStatus = 3,//manual leave approve status=3
                        leaveStatus = 1,
                        leaveFrom = model.leaveFrom,
                        leaveTo = model.leaveTo,
                        daysLeave = model.daysLeave,
                        purposeOfLeave = model.purposeOfLeave,
                        emergencyContactNo = model.emergencyContactNo,
                        address = model.address,
                        substitutionUserId = model.substitutionUserId,
                        leaveDayId = model.leaveDayId,
                        fileUrl = fileNameMain,
                        paymentOption = "Proposed Recreation"
                    };

                    int id = await leaveRegisterService.SaveLeaveRegister(data);


                }


            }
            //if General Employee apply application forword to Dep/Br/Zone head and status=1

          
            else
            {
                LeaveRegister data = new LeaveRegister
                {
                    Id = model.id,
                    employeeId = EmpInfo.Id,
                    leaveTypeId = model.leaveTypeId,
                    whenLeave = model.whenLeave,
                    //leaveStatus = 3,//manual leave approve status=3
                    leaveStatus = 1,
                    leaveFrom = model.leaveFrom,
                    leaveTo = model.leaveTo,
                    daysLeave = model.daysLeave,
                    purposeOfLeave = model.purposeOfLeave,
                    emergencyContactNo = model.emergencyContactNo,
                    address = model.address,
                    substitutionUserId = model.substitutionUserId,
                    leaveDayId = model.leaveDayId,
                    fileUrl = fileNameMain,
                    paymentOption = "Proposed Recreation"
                };

                int id = await leaveRegisterService.SaveLeaveRegister(data);


            }

            return Json("Saved");
        }

        #region Update Employee 
        [HttpPost]
        public async Task<JsonResult> UpdateReason([FromForm] LeaveRegisterViewModel model)
        {
            try
            {
                var leaveregId = await leaveRegisterService.UpdateReasonStatus(model.leaveregId, model.reason);
                return Json(leaveregId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        public async Task<IActionResult> ProposedApprovedListForEmployee()
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos.EmpCode);

            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
                leaveRegisterslist = await leaveRegisterService.ApprovedProposedRecrationListEmployee(EmpInfo.Id)
            };
            return View(model);
        }

        






        //Department or Branch Head

        public async Task<IActionResult> ProposedLeaveListForHead()
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos.EmpCode);
            int? depId = 0;
            int? brId = 0;
            int? zoneId = 0;
            if (EmpInfo.departmentId != null && EmpInfo.hrBranchId == 205 && EmpInfo.locationId == null)
            {
                 depId =EmpInfo.departmentId;
                 brId = 0;
                 zoneId = 0;
            }
            else if (EmpInfo.hrBranchId != null && EmpInfo.departmentId == null && EmpInfo.locationId == null)
            {
                brId = EmpInfo.hrBranchId;
                depId = 0;
                zoneId = 0;
            }
            else if (EmpInfo.locationId != null && EmpInfo.hrBranchId == null && EmpInfo.departmentId== null)
            {
                depId = 0;
                brId = 0;
                zoneId = EmpInfo.locationId;               
            }
            //var HeadInfo = await personalInfoService.GetHeadByDepartBranchZoneId(depId, brId, zoneId);

            var HeadCheckInfo = await personalInfoService.GetHeadInfoByEmpOrDepartBranchZoneId(EmpInfo.Id, depId, brId, zoneId);

            if (zoneId != 0)
            {
                var empLeave = new List<LeaveRegisterViewModelN>();
                var proposedlists = await leaveRegisterService.GetAllProposedZoneForZoneHead(zoneId);

                if (userName == HeadCheckInfo.HeadCode)
                {
                    foreach (var item in proposedlists)
                    {
                        empLeave.Add(new LeaveRegisterViewModelN
                        {
                            proposedlistsForZone = item,
                        });
                    }
                }

                LeaveRegisterViewModel model = new LeaveRegisterViewModel
                {
                    fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                    visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
                    ProleaveRegisters = empLeave
                };
                return View(model);
            }

            else
            {
                var empLeave = new List<LeaveRegisterViewModelN>();
                var proposedlists = await leaveRegisterService.GetAllProLeaveByDepBrZId(depId, brId, zoneId);

                if (userName == HeadCheckInfo.HeadCode)
                {
                    foreach (var item in proposedlists)
                    {
                        empLeave.Add(new LeaveRegisterViewModelN
                        {
                            proposedlistsForZone = item,
                        });
                    }
                }

                LeaveRegisterViewModel model = new LeaveRegisterViewModel
                {
                    fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                    visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
                    ProleaveRegisters = empLeave
                };
                return View(model);
            }
            
        }




        [HttpPost]
        public async Task<IActionResult> ApprovedProposedRecreation(LeaveRegisterViewModel model)
        { 
            //Proposed Leave Approved By Head=34
            try
            {
               

                for (int i = 0; i < model.registerids.Length; i++)
                {
                    await leaveRegisterService.UpdateProposedLeaveStatusByHead1(Convert.ToInt32(model.registerids[i]), model.reason);

                }
                return RedirectToAction("ProposedLeaveListForHead");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        
        [HttpPost]
        public async Task<IActionResult> RejectProposedLeaveByHead(LeaveRegisterViewModel model)
        {
            //Proposed Leave Approved By Head = 35
            try
            {
               

                for (int i = 0; i < model.registerids.Length; i++)
                {
                    await leaveRegisterService.RejectProposedLeaveStatusByHead(Convert.ToInt32(model.registerids[i]));

                }
                return RedirectToAction("ProposedLeaveListForHead");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        



        public async Task<IActionResult> ApprovedProposedListForHead()
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                var userInfos = await userInfo.GetUserInfoByUser(userName);
                var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos.EmpCode);
                int? depId = 0;
                int? brId = 0;
                int? zoneId = 0;
                if (EmpInfo.departmentId != null && EmpInfo.hrBranchId == 205 && EmpInfo.locationId == null)
                {
                    depId = EmpInfo.departmentId;
                    brId = 0;
                    zoneId = 0;
                }
                else if (EmpInfo.hrBranchId != null && EmpInfo.departmentId == null && EmpInfo.locationId == null)
                {
                    brId = EmpInfo.hrBranchId;
                    depId = 0;
                    zoneId = 0;
                }
                else if (EmpInfo.locationId != null && EmpInfo.hrBranchId == null && EmpInfo.departmentId == null)
                {
                    depId = 0;
                    brId = 0;
                    zoneId = EmpInfo.locationId;
                }
                var HeadInfo = await personalInfoService.GetHeadByDepartBranchZoneId(depId, brId, zoneId);

                var empLeave = new List<LeaveRegisterViewModelN>();
                var proposedlists = await leaveRegisterService.GetAPrrovedProposedLeaveByDepBrZIdZForHead(depId, brId, zoneId);

                if (userName == HeadInfo.HeadCode)
                {
                    foreach (var item in proposedlists)
                    {
                        empLeave.Add(new LeaveRegisterViewModelN
                        {
                            proposedlists = item,
                        });
                    }
                }

                LeaveRegisterViewModel model = new LeaveRegisterViewModel
                {
                    ProleaveRegisters = empLeave
                };
                return View(model);
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }


      public async Task<IActionResult> ApprovedProposedListForHeadYearApi(int? year)
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos.EmpCode);
            int? depId = 0;
            int? brId = 0;
            int? zoneId = 0;
            if (EmpInfo.departmentId != null && EmpInfo.hrBranchId == 205 && EmpInfo.locationId == null)
            {
                depId = EmpInfo.departmentId;
                brId = 0;
                zoneId = 0;
            }
            else if (EmpInfo.hrBranchId != null && EmpInfo.departmentId == null && EmpInfo.locationId == null)
            {
                brId = EmpInfo.hrBranchId;
                depId = 0;
                zoneId = 0;
            }
            else if (EmpInfo.locationId != null && EmpInfo.hrBranchId == null && EmpInfo.departmentId == null)
            {
                depId = 0;
                brId = 0;
                zoneId = EmpInfo.locationId;
            }
            var HeadInfo = await personalInfoService.GetHeadByDepartBranchZoneId(depId, brId, zoneId);

            var empLeave = new List<LeaveRegisterViewModelN>();
            var proposedlists = await leaveRegisterService.GetAPrrovedProposedLeaveByDepBrZIdZForHeadByYear(depId, brId, zoneId, year);

            if (userName == HeadInfo.HeadCode)
            {
                foreach (var item in proposedlists)
                {
                    empLeave.Add(new LeaveRegisterViewModelN
                    {
                        proposedlists = item,
                    });
                }
            }

            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                ProleaveRegisters = empLeave
            };
            return Json(model);
        }


    





        //Hr Admin part
        [HttpPost]
        public async Task<IActionResult> ApprovedProposedByHrAdmin(LeaveRegisterViewModel model)
        {
            //Proposed Leave Approved By Admin=36
            try
            {
               

                for (int i = 0; i < model.registerids.Length; i++)
                {
                    await leaveRegisterService.ApprovedProposedStatusByHRadmin(Convert.ToInt32(model.registerids[i]));

                }
                return RedirectToAction("ProposedLeaveListForHrAdmin");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        
        [HttpPost]
        public async Task<IActionResult> RejectProposedListByHrAdmin(LeaveRegisterViewModel model)
        {
            //Proposed Leave reject By Head = 38
            try
            {
               

                for (int i = 0; i < model.registerids.Length; i++)
                {
                    await leaveRegisterService.RejectProposedStatusByHrAdmin(Convert.ToInt32(model.registerids[i]));

                }
                return RedirectToAction("ProposedLeaveListForHrAdmin");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



        //Anumodito Proposed Leave

        public async Task<IActionResult> FinalAnumodonProposedListForHrAdmin()
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos.EmpCode);

            var empLeave = new List<LeaveRegisterViewModelN>();
            var ProposedLeave = await leaveRegisterService.FinalAnumodonProposedListForHrAdmin();
            if (User.IsInRole("HRLeaveAdmin"))
            {
                foreach (var item in ProposedLeave)
                {
                    empLeave.Add(new LeaveRegisterViewModelN
                    {
                        leaveRegisterslists = item,
                    });
                }
            }



            var model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
                //leaveRegisterslist = await leaveRegisterService.ProposedRecrationListForHrAdmin()
                ProleaveRegisters = empLeave
            };
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> AnumoditoProposedByHrAdmin(LeaveRegisterViewModel model)
        {
            //Proposed Leave Approved By Admin=38
            try
            {


                for (int i = 0; i < model.registerids.Length; i++)
                {
                    await leaveRegisterService.AnumoditoProposedStatusByHRadmin(Convert.ToInt32(model.registerids[i]));

                }
                return RedirectToAction("FinalAnumodonProposedListForHrAdmin");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        public async Task<IActionResult> AnumoditoProposedRejectByHrAdmin(LeaveRegisterViewModel model)
        {
            //Proposed Leave Approved By Head = 39
            try
            {


                for (int i = 0; i < model.registerids.Length; i++)
                {
                    await leaveRegisterService.AnumoditoRejectProposedStatusByHrAdmin(Convert.ToInt32(model.registerids[i]));

                }
                return RedirectToAction("FinalAnumodonProposedListForHrAdmin");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



        public async Task<IActionResult> FinalAnumoditoListForHrAdmin()
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos.EmpCode);

            var empLeave = new List<LeaveRegisterViewModelN>();
            var ProposedLeave = await leaveRegisterService.AnumoditoProposedListForHrAdmin();
            if (User.IsInRole("HRLeaveAdmin"))
            {
                foreach (var item in ProposedLeave)
                {
                    empLeave.Add(new LeaveRegisterViewModelN
                    {
                        leaveRegisterslists = item,
                    });
                }
            }



            var model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
                ProleaveRegisters = empLeave
            };
            return View(model);
        }


        public async Task<IActionResult> FinalRejectProposedForHrAdmin()
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos.EmpCode);

            var empLeave = new List<LeaveRegisterViewModelN>();
            var ProposedLeave = await leaveRegisterService.AnumoditoRejectProposedForHrAdmin();
            if (User.IsInRole("HRLeaveAdmin"))
            {
                foreach (var item in ProposedLeave)
                {
                    empLeave.Add(new LeaveRegisterViewModelN
                    {
                        leaveRegisterslists = item,
                    });
                }
            }



            var model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
                ProleaveRegisters = empLeave
            };
            return View(model);
        }



        public async Task<IActionResult> ProposedLeaveListForHrAdmin()
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos.EmpCode);

            var empLeave = new List<LeaveRegisterViewModelN>();
            var ProposedLeave = await leaveRegisterService.ProposedRecrationListForHrAdmin();
            if (User.IsInRole("HRLeaveAdmin"))
            {
                foreach (var item in ProposedLeave)
                {
                    empLeave.Add(new LeaveRegisterViewModelN
                    {
                        leaveRegisterslists = item,
                    });
                }
            }



            var model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
                //leaveRegisterslist = await leaveRegisterService.ProposedRecrationListForHrAdmin()
                ProleaveRegisters = empLeave
            };
            return View(model); 
        }





        











        public async Task<IActionResult> PApprovedProposedListForHrAdmin()
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos.EmpCode);

            var empLeave = new List<LeaveRegisterViewModelN>();
            var ProposedLeave = await leaveRegisterService.ApprovedProposedListForHrAdmin();
            if (User.IsInRole("HRLeaveAdmin"))
            {
                foreach (var item in ProposedLeave)
                {
                    empLeave.Add(new LeaveRegisterViewModelN
                    {
                        leaveRegisterslists = item,
                    });
                }
            }



            var model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
                ProleaveRegisters = empLeave
            };
            return View(model);
        }


        public async Task<ActionResult> ProposedRecreationReportHrAdmin()
        {
            var model = new AllHrReportViewModel
            {
                hrBranches = await organizationService.GetAllHrBranch(),
                departments = await organizationService.GetAllDepartments(),
                zoneList = await organizationService.GetAllBranch()
            };

            return View(model);
        }

        #endregion

        #region Proposed Leave Report
        [AllowAnonymous]
        public IActionResult ApprovedListForHeadPDF(int? year)
        {
            string userName = HttpContext.User.Identity.Name;
            //var userInfos =  userInfo.GetUserInfoByUser(userName);

            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;

            string url = scheme + "://" + host + "/HRPMSLeave/LeaveRegister/ApprovedListForHeadView?userName=" + userName + "&year=" + year;
            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        [AllowAnonymous]
        public async Task<IActionResult> ApprovedListForHeadView(string userName,int? year)
        {
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userName);
            int? depId = 0;
            int? brId = 0;
            int? zoneId = 0;
            if (EmpInfo.departmentId != null && EmpInfo.hrBranchId == 205 && EmpInfo.locationId == null)
            {
                depId = EmpInfo.departmentId;
                brId = 0;
                zoneId = 0;
            }
            else if (EmpInfo.hrBranchId != null && EmpInfo.departmentId == null && EmpInfo.locationId == null)
            {
                brId = EmpInfo.hrBranchId;
                depId = 0;
                zoneId = 0;
            }
            else if (EmpInfo.locationId != null && EmpInfo.hrBranchId == null && EmpInfo.departmentId == null)
            {
                depId = 0;
                brId = 0;
                zoneId = EmpInfo.locationId;
            }
            var HeadInfo = await personalInfoService.GetHeadByDepartBranchZoneId(depId, brId, zoneId);

            var empLeave = new List<LeaveRegisterViewModelN>();
            var proposedlists = await leaveRegisterService.GetAPrrovedProposedLeaveByYearReport(depId, brId, zoneId,year);

            if (userName == HeadInfo.HeadCode)
            {
                foreach (var item in proposedlists)
                {
                    empLeave.Add(new LeaveRegisterViewModelN
                    {
                        reportlists = item,
                    });
                }
            }

            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                ProleaveRegisters = empLeave
            };
            return View(model);
        }






        [AllowAnonymous]
        public IActionResult ProposedListForHrAdminByYearPDF(int? year)
        {
            string userName = HttpContext.User.Identity.Name;
            //var userInfos =  userInfo.GetUserInfoByUser(userName);

            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;

            string url = scheme + "://" + host + "/HRPMSLeave/LeaveRegister/ProposedListForHrAdminByYearView?year=" + year;
            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }




        [AllowAnonymous]
        public async Task<IActionResult> ProposedListForHrAdminByYearView(int? year)
        {
            var empLeave = new List<LeaveRegisterViewModelN>();
            var ProposedLeave = await leaveRegisterService.ApprovedProposedListForHrAdminReportByYear(year);
            //if (User.IsInRole("HRLeaveAdmin"))
            //{
            //    foreach (var item in ProposedLeave)
            //    {
            //        empLeave.Add(new LeaveRegisterViewModelN
            //        {
            //            leaveRegisterslists = item,
            //        });
            //    }
            //}
            var model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
                leaveRegisterslist = await leaveRegisterService.ApprovedProposedListForHrAdminReportByYear(year)
             };
            return View(model);
        }


        [AllowAnonymous]
        public IActionResult ApprovedListForHrAdminByEmpIdPDF(int? empID)
        {
            string userName = HttpContext.User.Identity.Name;
            //var userInfos =  userInfo.GetUserInfoByUser(userName);

            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;

            string url = scheme + "://" + host + "/HRPMSLeave/LeaveRegister/ApprovedListForHrAdminByEmpIdView?empID=" + empID;
            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }




        [AllowAnonymous]
        public async Task<IActionResult> ApprovedListForHrAdminByEmpIdView(int? empID)
        {
          
            var model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
                leaveRegisterslist = await leaveRegisterService.ApprovedProposedListForHrAdminReportByIndEmp(empID)
            };
            return View(model);
        }


        //Report For Hr branch dep zone year and emp wise.
        [AllowAnonymous]
        public IActionResult ProposedListForHrAdminByBranchDepZonePDF(int? deptId,int? hrBranchId,int? LZoneId,int? empID,int? yearId)
        {
            string userName = HttpContext.User.Identity.Name;
            //var userInfos =  userInfo.GetUserInfoByUser(userName);

            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;

            string url = scheme + "://" + host + "/HRPMSLeave/LeaveRegister/ProposedListForHrAdminByBranchDepZoneView?deptId=" + deptId + "&hrBranchId=" + hrBranchId+ "&LZoneId=" + LZoneId+ "&empID=" + empID+ "&yearId=" + yearId;
            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }




        [AllowAnonymous]
        public async Task<IActionResult> ProposedListForHrAdminByBranchDepZoneView(int? deptId, int? hrBranchId, int? LZoneId, int? empID, int? yearId)
        {
            
            ManualRecreationReportVM model = new ManualRecreationReportVM
            {
                LeaveCommitteList=await leaveRegisterService.GetleaveRegisterListByYear(yearId),
                ManualRecreationReportViewModel = await leaveRegisterService.GetAllProposedReportByDepIdForHrAdmin(deptId, hrBranchId, LZoneId,empID,yearId)
            };
            return View(model);


        }




        [AllowAnonymous]
        public IActionResult AnumoditoProposedListForHrAdminByPDF(int? year)
        {
            string userName = HttpContext.User.Identity.Name;
            //var userInfos =  userInfo.GetUserInfoByUser(userName);

            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;

            string url = scheme + "://" + host + "/HRPMSLeave/LeaveRegister/AnumoditoProposedListForHrAdminByView?year=" + year;
            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }




        [AllowAnonymous]
        public async Task<IActionResult> AnumoditoProposedListForHrAdminByView(int? year)
        {

            ManualRecreationReportVM model = new ManualRecreationReportVM
            {
                LeaveCommitteList = await leaveRegisterService.GetleaveRegisterListByYear(year),
                ManualRecreationReportViewModel = await leaveRegisterService.GetAnumoditoLeaveForHrAdminByYear(year)
            };
            return View(model);
        }



        
        [AllowAnonymous]
        public IActionResult FinalRejectProposedForHrAdminByPDF(int? year)
        {
            string userName = HttpContext.User.Identity.Name;
            //var userInfos =  userInfo.GetUserInfoByUser(userName);

            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;

            string url = scheme + "://" + host + "/HRPMSLeave/LeaveRegister/FinalRejectProposedForHrAdminByView?year=" + year;
            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }




        [AllowAnonymous]
        public async Task<IActionResult> FinalRejectProposedForHrAdminByView(int? year)
        {


            ManualRecreationReportVM model = new ManualRecreationReportVM
            {
                LeaveCommitteList = await leaveRegisterService.GetleaveRegisterListByYear(year),
                ManualRecreationReportViewModel = await leaveRegisterService.GetFinalRejectProposedLeaveForHrAdminByYear(year)
            };
            return View(model);
        }







        [AllowAnonymous]
        public IActionResult ProposedLeaveIndividualPDF(int id)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;

            string url = scheme + "://" + host + "/HRPMSLeave/LeaveRegister/ProposedLeaveIndividualView/" + id;
            string status = myPDF.GeneratePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }


        [AllowAnonymous]
        public async Task<IActionResult> ProposedLeaveIndividualView(int id)
        {
            var leaveRegList = await leaveRegisterService.GetLeaveRegisterById(id);
            DateTime leaveYear = Convert.ToDateTime(leaveRegList.leaveFrom);
            var year = leaveYear.Year;
            ViewBag.leaveYear = year;

         
            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                leaveRegister = await leaveRegisterService.GetLeaveRegisterById(id),
                employeeSignature = await leaveRegisterService.GetSignatureByEmpId(Convert.ToInt32(leaveRegList.employeeId)),
            };

            return View(model);
        }




        #endregion

        #region LeaveCommittee


        public async Task<IActionResult> LeaveCommitteeAssign()
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos.EmpCode);


            LeaveCommitteeViewModel model = new LeaveCommitteeViewModel
            {
                leaveCommitteeList=await leaveRegisterService.GetleaveRegisterList(),
            };
            return View(model);
        }






        [HttpPost]
        public async Task<IActionResult> SaveLeaveCommitteeAssign([FromForm] LeaveCommitteeViewModel model)
        {
            //var approvalMaster = await approvalService.GetApprovalMasterByEmpId(Convert.ToInt32(model.employeeInfoId));
            //var MasterId = 0;
            try
            {
                if (model.employeeId != null)
                {
                    if (model.employeeId.Length > 0)
                    {

                        for (int i = 0; i < model.employeeId.Length; i++)
                        {
                            LeaveCommittee detail = new LeaveCommittee();

                            detail.Id = 0;
                            // detail.employeeId = MasterId;
                            detail.employeeId = Convert.ToInt32(model.employeeId[i]);
                            detail.empNameBn = model.empNameBn[i];
                            detail.empNameEng = model.empNameEng[i];


                            detail.designationEn = model.designationEn[i];
                            detail.designationBn = model.designationBn[i];

                            detail.departmentBn = model.departmentBn[i];
                            detail.departmentEn = model.departmentEn[i];
                            detail.memberRoleId = model.memberRoleId[i];
                            detail.CoYear = model.CoYear[i];

                            await leaveRegisterService.SaveCommetitteDetail(detail);
                            detail = new LeaveCommittee();
                        }

                    }
                }

                return Json(model.CoYear);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }


        public async Task<IActionResult> DeleteMember(int id)
        {
            await leaveRegisterService.DeleteLeaveCommitteeById(id);
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("LeaveCommitteeAssign");
           
        }





        #endregion


        #region Actual Leave



        public async Task<IActionResult> ActualLeaveApplyEmployee()
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos.EmpCode);
            var year = DateTime.Now.Year;

            var HeadCheckInfo = await personalInfoService.GetHeadInfoByEmpOrDepartBranchZoneId(EmpInfo.Id, EmpInfo.departmentId == null ?  0 : EmpInfo.departmentId , EmpInfo.hrBranchId ==null ? 0 : EmpInfo.hrBranchId == 205 ? 0 : EmpInfo.hrBranchId, EmpInfo.locationId == null ? 0 : EmpInfo.locationId);

            int? headId = 0;

            if (HeadCheckInfo != null)
            {
                headId = HeadCheckInfo.HeadId;
            }
            else
            {
                headId = 0;
            }
              
            ViewBag.HeadId = headId;

            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
                leaveRegisterslist = await leaveRegisterService.ActualLeaveApplyFromProposedEmployee(EmpInfo.Id, year)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveActualLeave([FromForm] LeaveRegisterViewModel model)
        {

            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(model.empCode);



            int? depId = 0;
            int? brId = 0;
            int? zoneLId = 0;
            if (EmpInfo.departmentId != null && EmpInfo.hrBranchId == 205 && EmpInfo.locationId == null)
            {
                depId = EmpInfo.departmentId;
                brId = 0;
                zoneLId = 0;
            }
            else if (EmpInfo.hrBranchId != null && EmpInfo.departmentId == null && EmpInfo.locationId == null)
            {
                brId = EmpInfo.hrBranchId;
                depId = 0;
                zoneLId = 0;
            }
            else if (EmpInfo.locationId != null && EmpInfo.hrBranchId == null && EmpInfo.departmentId == null)
            {
                zoneLId = EmpInfo.locationId;
                depId = 0;
                brId = 0;

            }


            var HeadCheckInfo = await personalInfoService.GetHeadInfoByEmpOrDepartBranchZoneId(EmpInfo.Id, depId, brId, zoneLId);
            

            if (HeadCheckInfo != null)
            {
                if (HeadCheckInfo.branchId > 0 && HeadCheckInfo.branchId != 205)  //if Branch Head apply notification send to zone head
                {
                    var UZoneId = await personalInfoService.GetZoneIdByBranchId((int)HeadCheckInfo.branchId);
                    var UserHeadInfo = await personalInfoService.GetHeadByDepartBranchZoneId(0, 0, UZoneId);
                    var userDetails = await personalInfoService.GetEmployeeInfoByCode(userName);
                    await leaveRegisterService.SendNotification(userName, userName, UserHeadInfo.HeadCode, "Actual Leave From " + userDetails.nameEnglish, "Waiting for Actual Leave  to Approver", "Actual", "/HRPMSLeave/LeaveRegister/ActualLeaveListForHead");

                }
                else if ((HeadCheckInfo.depId == depId && HeadCheckInfo.branchId == 205) || HeadCheckInfo.locationId == zoneLId) //if department/zone  Head apply notification send to HrLeaveAdmin
                {
                    //var UserHeadInfo = await personalInfoService.GetHeadByDepartBranchZoneId(HeadCheckInfo.depId, HeadCheckInfo.branchId, HeadCheckInfo.locationId);
                    //var userDetails = await personalInfoService.GetEmployeeInfoByCode(userName);
                    //await leaveRegisterService.SendNotification(userName, userName, UserHeadInfo.HeadCode, "Actual Leave From " + userDetails.nameEnglish, "Waiting for Actual Leave  to Approver", "Actual", "/HRPMSLeave/LeaveRegister/ActualLeaveListForHead");

                }
            }

            else  //if General Employee apply - notification send to Dep/Br/Zone Head
            {
                var HeadInfo = await personalInfoService.GetHeadByDepartBranchZoneId(depId, brId, zoneLId);

                var userDetails = await personalInfoService.GetEmployeeInfoByCode(userName);
                await leaveRegisterService.SendNotification(userName, userName, HeadInfo.HeadCode, "Actual Leave From " + userDetails.nameEnglish, "Waiting for Actual Leave  to Approver", "Actual", "/HRPMSLeave/LeaveRegister/ActualLeaveListForHead");

            }





            int empId = 0;
            string email = "";
            string name = "";
            if (EmpInfo != null)
            {
                empId = EmpInfo.Id;
                email = EmpInfo.emailAddress;
                name = EmpInfo.nameEnglish;
            }
            ViewBag.employeeId = empId;

            string fileName = String.Empty;
            string fileNameMain = String.Empty;
            if (model.fileUrl != null)
            {
                string message = FileSave.SaveEmpAttachment(out fileName, model.fileUrl);

                if (message == "success")
                {
                    fileNameMain = fileName;
                }
                else
                {
                    fileNameMain = "";
                }
            }
            else
            {
                fileNameMain = "";
            }

            if (HeadCheckInfo != null) //if department/zone  Head apply application forword to HrLeaveAdmin and status=41
            {
                if ((HeadCheckInfo.depId >0  && HeadCheckInfo.branchId == 205) || HeadCheckInfo.locationId >0 )
                {
                    LeaveRegister data1 = new LeaveRegister
                    {
                        Id = model.id,
                        employeeId = EmpInfo.Id,
                        leaveTypeId = model.leaveTypeId,
                        whenLeave = model.whenLeave,
                       
                        leaveStatus = 41,
                        leaveFrom = model.leaveFrom,
                        leaveTo = model.leaveTo,

                        ActualLeaveFrom = model.leaveFromN,
                        ActualLeaveTo = model.leaveToN,

                        daysLeave = model.daysLeave,
                        purposeOfLeave = model.purposeOfLeave,
                        emergencyContactNo = model.emergencyContactNo,
                        address = model.address,
                        substitutionUserId = model.substitutionUserId,
                        leaveDayId = model.leaveDayId,
                        fileUrl = fileNameMain,
                        paymentOption = "Actual Recreation"
                    };

                    await leaveRegisterService.SaveLeaveRegister(data1);
                }

                //if branch Head apply application forword to Zone head and status=40
                else
                {
                    LeaveRegister data = new LeaveRegister
                    {
                        Id = model.id,
                        employeeId = EmpInfo.Id,
                        leaveTypeId = model.leaveTypeId,
                        whenLeave = model.whenLeave,
                        leaveStatus = 40,//Actual apply status=40
                        leaveFrom = model.leaveFrom,
                        leaveTo = model.leaveTo,

                        ActualLeaveFrom = model.leaveFromN,
                        ActualLeaveTo = model.leaveToN,

                        daysLeave = model.daysLeave,
                        purposeOfLeave = model.purposeOfLeave,
                        emergencyContactNo = model.emergencyContactNo,
                        address = model.address,
                        substitutionUserId = model.substitutionUserId,
                        leaveDayId = model.leaveDayId,
                        fileUrl = fileNameMain,
                        paymentOption = "Actual Recreation"
                    };

                    int id = await leaveRegisterService.SaveLeaveRegister(data);


                }

            }


            //if General Employee apply application forword to Dep/Br/Zone head and status=40
          
            else
            {
                LeaveRegister data = new LeaveRegister
                {
                    Id = model.id,
                    employeeId = EmpInfo.Id,
                    leaveTypeId = model.leaveTypeId,
                    whenLeave = model.whenLeave,
                    leaveStatus = 40,//Actual apply status=40
                    leaveFrom = model.leaveFrom,
                    leaveTo = model.leaveTo,

                    ActualLeaveFrom = model.leaveFromN,
                    ActualLeaveTo = model.leaveToN,

                    daysLeave = model.daysLeave,
                    purposeOfLeave = model.purposeOfLeave,
                    emergencyContactNo = model.emergencyContactNo,
                    address = model.address,
                    //substitutionUserId = model.substitutionUserId,
                    leaveDayId = model.leaveDayId,
                    fileUrl = fileNameMain,
                    paymentOption = "Actual Recreation"
                };

                int id = await leaveRegisterService.SaveLeaveRegister(data);


            }

           

            return Json("Saved");
        }




        public async Task<IActionResult> ActualApprovedListForEmployee()
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos.EmpCode);

            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
                leaveRegisterslist = await leaveRegisterService.ApprovedActualListEmployee(EmpInfo.Id)
            };
            return View(model);
        }








        //Head part
        public async Task<IActionResult> ActualLeaveListForHead()
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos.EmpCode);
            int? depId = 0;
            int? brId = 0;
            int? zoneId = 0;
            if (EmpInfo.departmentId != null && EmpInfo.hrBranchId == 205 && EmpInfo.locationId == null)
            {
                depId = EmpInfo.departmentId;
                brId = 0;
                zoneId = 0;
            }
            else if (EmpInfo.hrBranchId != null && EmpInfo.departmentId == null && EmpInfo.locationId == null)
            {
                brId = EmpInfo.hrBranchId;
                depId = 0;
                zoneId = 0;
            }
            else if (EmpInfo.locationId != null && EmpInfo.hrBranchId == null && EmpInfo.departmentId == null)
            {
                depId = 0;
                brId = 0;
                zoneId = EmpInfo.locationId;
            }

            var HeadCheckInfo = await personalInfoService.GetHeadInfoByEmpOrDepartBranchZoneId(EmpInfo.Id, depId, brId, zoneId);

            if (zoneId != 0)
            {
                var empLeave = new List<LeaveRegisterViewModelN>();
                var proposedlists = await leaveRegisterService.GetAllActualLForZoneHead(zoneId);

                if (userName == HeadCheckInfo.HeadCode)
                {
                    foreach (var item in proposedlists)
                    {
                        empLeave.Add(new LeaveRegisterViewModelN
                        {
                            actuallists = item,
                        });
                    }
                }

                LeaveRegisterViewModel model = new LeaveRegisterViewModel
                {
                    fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                    visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
                    ProleaveRegisters = empLeave
                };
                return View(model);
            }

            else
            {
                var empLeave = new List<LeaveRegisterViewModelN>();
                var proposedlists = await leaveRegisterService.GetAllActualLeaveByDepBrZId(depId, brId, zoneId);

                if (userName == HeadCheckInfo.HeadCode)
                {
                    foreach (var item in proposedlists)
                    {
                        empLeave.Add(new LeaveRegisterViewModelN
                        {
                            actuallists = item,
                        });
                    }
                }

                LeaveRegisterViewModel model = new LeaveRegisterViewModel
                {
                    fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                    visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
                    ProleaveRegisters = empLeave
                };
                return View(model);
            }

        }


        [HttpPost]
        public async Task<IActionResult> ApprovedActualRecreationByHead(LeaveRegisterViewModel model)
        {
            //Actual Leave Approved By Head=3
           
            try
            {


                for (int i = 0; i < model.registerids.Length; i++)
                {
                    await leaveRegisterService.UpdateActualLeaveStatusByHead(Convert.ToInt32(model.registerids[i]));

                }
                return RedirectToAction("ActualLeaveListForHead");
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public async Task<IActionResult> UpdateSubsEmployee(int? leaveRegId,int? substiEmpId)
        {
            var data = await leaveRegisterService.GetLeaveRegisterById(Convert.ToInt32(leaveRegId));
            data.substitutionUserId = substiEmpId;
         
            var result = await leaveRegisterService.SaveLeaveRegister(data);
            if (result >= 1)
            {
                return Json("saved");
            }
            else
            {
                return Json("Failed");
            }
        }





        [HttpPost]
        public async Task<IActionResult> RejectActualLeaveByHead(LeaveRegisterViewModel model)
        {
            //Actual Leave reject By Head = 42
            try
            {


                for (int i = 0; i < model.registerids.Length; i++)
                {
                    await leaveRegisterService.ActualLeaveRejectStatusByHead(Convert.ToInt32(model.registerids[i]));

                }
                return RedirectToAction("ActualLeaveListForHead");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<IActionResult> ApprovedActualListForHead()
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                var userInfos = await userInfo.GetUserInfoByUser(userName);
                var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos.EmpCode);
                int? depId = 0;
                int? brId = 0;
                int? zoneId = 0;
                if (EmpInfo.departmentId != null && EmpInfo.hrBranchId == 205 && EmpInfo.locationId == null)
                {
                    depId = EmpInfo.departmentId;
                    brId = 0;
                    zoneId = 0;
                }
                else if (EmpInfo.hrBranchId != null && EmpInfo.departmentId == null && EmpInfo.locationId == null)
                {
                    brId = EmpInfo.hrBranchId;
                    depId = 0;
                    zoneId = 0;
                }
                else if (EmpInfo.locationId != null && EmpInfo.hrBranchId == null && EmpInfo.departmentId == null)
                {
                    depId = 0;
                    brId = 0;
                    zoneId = EmpInfo.locationId;
                }
                var HeadInfo = await personalInfoService.GetHeadByDepartBranchZoneId(depId, brId, zoneId);

                var empLeave = new List<LeaveRegisterViewModelN>();
                var proposedlists = await leaveRegisterService.GetActualAprrovedLeaveByDepBrZIdZForHead(depId, brId, zoneId);

                if (userName == HeadInfo.HeadCode)
                {
                    foreach (var item in proposedlists)
                    {
                        empLeave.Add(new LeaveRegisterViewModelN
                        {
                            actuallists = item,
                        });
                    }
                }

                LeaveRegisterViewModel model = new LeaveRegisterViewModel
                {
                    ProleaveRegisters = empLeave
                };
                return View(model);
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public async Task<IActionResult> ApprovedActualLeaveForHeadYearApi(int? year)
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos.EmpCode);
            int? depId = 0;
            int? brId = 0;
            int? zoneId = 0;
            if (EmpInfo.departmentId != null && EmpInfo.hrBranchId == 205 && EmpInfo.locationId == null)
            {
                depId = EmpInfo.departmentId;
                brId = 0;
                zoneId = 0;
            }
            else if (EmpInfo.hrBranchId != null && EmpInfo.departmentId == null && EmpInfo.locationId == null)
            {
                brId = EmpInfo.hrBranchId;
                depId = 0;
                zoneId = 0;
            }
            else if (EmpInfo.locationId != null && EmpInfo.hrBranchId == null && EmpInfo.departmentId == null)
            {
                depId = 0;
                brId = 0;
                zoneId = EmpInfo.locationId;
            }
            var HeadInfo = await personalInfoService.GetHeadByDepartBranchZoneId(depId, brId, zoneId);

            var empLeave = new List<LeaveRegisterViewModelN>();
            var actuallists = await leaveRegisterService.GetActualAPrrovedLeaveByDepBrZIdZForHeadByYear(depId, brId, zoneId, year);

            if (userName == HeadInfo.HeadCode)
            {
                foreach (var item in actuallists)
                {
                    empLeave.Add(new LeaveRegisterViewModelN
                    {
                        actuallists = item,
                    });
                }
            }

            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                ProleaveRegisters = empLeave
            };
            return Json(model);
        }

        [AllowAnonymous]
        public IActionResult ActualApprovedListForHeadPDF(int? year)
        {
            string userName = HttpContext.User.Identity.Name;
            //var userInfos =  userInfo.GetUserInfoByUser(userName);

            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;

            string url = scheme + "://" + host + "/HRPMSLeave/LeaveRegister/ActualApprovedListForHeadView?userName=" + userName + "&year=" + year;
            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        [AllowAnonymous]
        public async Task<IActionResult> ActualApprovedListForHeadView(string userName, int? year)
        {
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userName);
            int? depId = 0;
            int? brId = 0;
            int? zoneId = 0;
            if (EmpInfo.departmentId != null && EmpInfo.hrBranchId == 205 && EmpInfo.locationId == null)
            {
                depId = EmpInfo.departmentId;
                brId = 0;
                zoneId = 0;
            }
            else if (EmpInfo.hrBranchId != null && EmpInfo.departmentId == null && EmpInfo.locationId == null)
            {
                brId = EmpInfo.hrBranchId;
                depId = 0;
                zoneId = 0;
            }
            else if (EmpInfo.locationId != null && EmpInfo.hrBranchId == null && EmpInfo.departmentId == null)
            {
                depId = 0;
                brId = 0;
                zoneId = EmpInfo.locationId;
            }
            var HeadInfo = await personalInfoService.GetHeadByDepartBranchZoneId(depId, brId, zoneId);

            var empLeave = new List<LeaveRegisterViewModelN>();
            var proposedlists = await leaveRegisterService.GetActualAPrrovedLeaveByYearReport(depId, brId, zoneId, year);

            if (userName == HeadInfo.HeadCode)
            {
                foreach (var item in proposedlists)
                {
                    empLeave.Add(new LeaveRegisterViewModelN
                    {
                        reportActuallists = item,
                    });
                }
            }

            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                ProleaveRegisters = empLeave
            };
            return View(model);
        }




        //HrAdmin Part

        public async Task<IActionResult> ActualLeaveListForHrAdmin()
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos.EmpCode);

            var empLeave = new List<LeaveRegisterViewModelN>();
            var ProposedLeave = await leaveRegisterService.ActualRecrationListForHrAdmin();
            if (User.IsInRole("HRLeaveAdmin"))
            {
                foreach (var item in ProposedLeave)
                {
                    empLeave.Add(new LeaveRegisterViewModelN
                    {
                        leaveRegisterslists = item,
                    });
                }
            }



            var model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
                //leaveRegisterslist = await leaveRegisterService.ProposedRecrationListForHrAdmin()
                ProleaveRegisters = empLeave
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ApprovedActualByHrAdmin(LeaveRegisterViewModel model)
        {
            //Actual Leave Approved By Admin=3
            try
            {


                for (int i = 0; i < model.registerids.Length; i++)
                {
                    await leaveRegisterService.ApprovedActualStatusByHRadmin(Convert.ToInt32(model.registerids[i]));

                }
                return RedirectToAction("ActualLeaveListForHrAdmin");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        public async Task<IActionResult> RejectActualListByHrAdmin(LeaveRegisterViewModel model)
        {
            //Actual Leave reject By Head = 44
            try
            {


                for (int i = 0; i < model.registerids.Length; i++)
                {
                    await leaveRegisterService.RejectActualStatusByHrAdmin(Convert.ToInt32(model.registerids[i]));

                }
                return RedirectToAction("ActualLeaveListForHrAdmin");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<IActionResult> ActualApprovedListForHrAdmin()
        {
            string userName = HttpContext.User.Identity.Name;
            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos.EmpCode);

            var empLeave = new List<LeaveRegisterViewModelN>();
            var ProposedLeave = await leaveRegisterService.ApprovedActualListForHrAdmin();
            if (User.IsInRole("HRLeaveAdmin"))
            {
                foreach (var item in ProposedLeave)
                {
                    empLeave.Add(new LeaveRegisterViewModelN
                    {
                        leaveRegisterslists = item,
                    });
                }
            }



            var model = new LeaveRegisterViewModel
            {
                fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
                visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
                ProleaveRegisters = empLeave
            };
            return View(model);
        }


        public async Task<ActionResult> ActualRecreationReportHrAdmin()
        {
            var model = new AllHrReportViewModel
            {
                hrBranches = await organizationService.GetAllHrBranch(),
                departments = await organizationService.GetAllDepartments(),
                zoneList = await organizationService.GetAllBranch()
            };

            return View(model);
        }



        //Report For Hr branch dep zone year and emp wise.
        [AllowAnonymous]
        public IActionResult ActualListForHrAdminByBranchDepZonePDF(int? deptId, int? hrBranchId, int? LZoneId, int? empID, int? yearId)
        {
            string userName = HttpContext.User.Identity.Name;
            //var userInfos =  userInfo.GetUserInfoByUser(userName);

            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;

            string url = scheme + "://" + host + "/HRPMSLeave/LeaveRegister/ActualListForHrAdminByBranchDepZoneView?deptId=" + deptId + "&hrBranchId=" + hrBranchId + "&LZoneId=" + LZoneId + "&empID=" + empID + "&yearId=" + yearId;
            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }




        [AllowAnonymous]
        public async Task<IActionResult> ActualListForHrAdminByBranchDepZoneView(int? deptId, int? hrBranchId, int? LZoneId, int? empID, int? yearId)
        {

            ManualRecreationReportVM model = new ManualRecreationReportVM
            {
                LeaveCommitteList = await leaveRegisterService.GetleaveRegisterListByYear(yearId),
                actualReportHRViewModel = await leaveRegisterService.GetActualReportByDepIdForHrAdmin(deptId, hrBranchId, LZoneId, empID, yearId)
            };
            return View(model);


        }




        //
        [AllowAnonymous]
        public IActionResult ActualLeaveIndividualPDF(int id)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;

            string url = scheme + "://" + host + "/HRPMSLeave/LeaveRegister/ActualLeaveIndividualView/" + id;
            string status = myPDF.GeneratePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }


        [AllowAnonymous]
        public async Task<IActionResult> ActualLeaveIndividualView(int id)
        {
            var leaveRegList = await leaveRegisterService.GetLeaveRegisterById(id);
            DateTime leaveYear = Convert.ToDateTime(leaveRegList.leaveFrom);
            var year = leaveYear.Year;
            ViewBag.leaveYear = year;


            LeaveRegisterViewModel model = new LeaveRegisterViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                leaveRegister = await leaveRegisterService.GetLeaveRegisterById(id),
                employeeSignature = await leaveRegisterService.GetSignatureByEmpId(Convert.ToInt32(leaveRegList.employeeId)),
            };

            return View(model);
        }
        #endregion


        #region Leave Summary Report

        [AllowAnonymous]
        public async Task<IActionResult> GetEmployeeLeaveByAnyKey(string employeeCode, int? leaveTypeId, int? approverId, string fromDate, string toDate)
        {
            var data = await leaveRegisterService.GetEmployeeLeaveByAnyKey(employeeCode, leaveTypeId, approverId, fromDate, toDate);
            var model = new EmployeeLeaveReportVM
            {
                companies = await eRPCompanyService.GetAllCompany(),
                EmployeeLeaveViewModels = data
            };
            return View(model);
        }
        [AllowAnonymous]
        public IActionResult GetLeaveByAnyKeyPDF(string employeeCode, int? leaveTypeId, int? approverId, string fromDate, string toDate)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;
            string url = "";

            url = scheme + "://" + host + "/HRPMSLeave/LeaveRegister/GetEmployeeLeaveByAnyKey?employeeCode=" + employeeCode + "&leaveTypeId=" + leaveTypeId + "&approverId=" + approverId + "&fromDate=" + fromDate + "&toDate=" + toDate;
            //url = scheme + "://" + host + "/HRPMSLeave/LeaveRegister/EarlyLeaveAttendance?userName=" + userName;
            string status = myPDF.GeneratePDF(out fileName, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong. </h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        #endregion

    }

}