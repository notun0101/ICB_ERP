using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Areas.HRPMSLeave.Models;
using OPUSERP.Areas.HRPMSLeave.Models.Lang;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Services.Leave.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.HRPMS.Services.Recruitment.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Data.Entity.Leave;
using OPUSERP.ERPServices.EmailService.interfaces;
using OPUSERP.Areas.Auth.Models;
using OPUSERP.Data.Entity;
using Microsoft.AspNetCore.Identity;
using OPUSERP.SCM.Services.MasterData.Interfaces;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.SCM.Data.Entity.Requisition;
using OPUSERP.SCM.Services.Requisition.Interfaces;
using OPUSERP.ERPServices.AuthService;
using OPUSERP.PushNotification.Models;
using OPUSERP.ERPServices.AuthService.Interfaces;
using OPUSERP.PushNotification.Services;
using DocumentFormat.OpenXml.Spreadsheet;

namespace OPUSERP.Areas.HRPMSLeave.Controllers
{
	[Authorize]
	[Area("HRPMSLeave")]
	public class LeaveApprovalController : Controller
	{
		private readonly LangGenerate<LeaveLn> _lang;
		private readonly ILeaveRegisterService leaveRegisterService;
		private readonly IWagesLeaveRegisterService wagesLeaveRegisterService;
		private readonly ILeaveRouteService leaveRouteService;
		private readonly IPersonalInfoService personalInfoService;
		private readonly IUserInfoes userInfo;
		private readonly ILeaveStatusLogService leaveStatusLogService;
		private readonly IEmailSenderService emailSenderService;
		private readonly ILeavePolicyService leavePolicyService;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IProjectService projectService;
		private readonly IApprovalService approvalService;
		private readonly IRequisitionService requisitionService;
        private readonly INotificationService pushNotificationService;
        private readonly IDbChangeService dbChangeService;

        public LeaveApprovalController(IHostingEnvironment hostingEnvironment, IRequisitionService requisitionService, UserManager<ApplicationUser> _userManager, ILeaveRegisterService leaveRegisterService, ILeaveRouteService leaveRouteService, IUserInfoes userInfo, IPersonalInfoService personalInfoService, ILeaveStatusLogService leaveStatusLogService, IEmailSenderService emailSenderService, IWagesLeaveRegisterService wagesLeaveRegisterService, ILeavePolicyService leavePolicyService, IProjectService projectService, IApprovalService approvalService, INotificationService pushNotificationService, IDbChangeService dbChangeService)
		{
			_lang = new LangGenerate<LeaveLn>(hostingEnvironment.ContentRootPath);
			this.leaveRegisterService = leaveRegisterService;
			this.leaveRouteService = leaveRouteService;
			this.personalInfoService = personalInfoService;
			this.requisitionService = requisitionService;
			this.userInfo = userInfo;
			this.leaveStatusLogService = leaveStatusLogService;
			this.emailSenderService = emailSenderService;
			this.wagesLeaveRegisterService = wagesLeaveRegisterService;
			this.leavePolicyService = leavePolicyService;
			this.projectService = projectService;
			this.approvalService = approvalService;
			this._userManager = _userManager;
			this.dbChangeService = dbChangeService;
			this.pushNotificationService = pushNotificationService;
		}

		#region NewLeaveApproval
		public async Task<IActionResult> NewLeaveApproval(int? id)
		{
			LeaveRegisterNewVM model = new LeaveRegisterNewVM
			{
				approvalTypes = await approvalService.GetAllApprovalType(),
				projects = await projectService.GetActiveProjectList()
			};
			return View(model);
		}

		public async Task<IActionResult> EditNewLeaveApproval(int? id, int leaveTypeId)
		{
			LeaveRegisterNewVM model = new LeaveRegisterNewVM
			{
				approvalMaster = await approvalService.GetApprovalMasterByApprovalMasterIdWithImage(Convert.ToInt32(id)),
				approvalDetails = await approvalService.GetApprovalDetailByApprovalMasterIdWithImage(Convert.ToInt32(id)),
				approvalTypes = await approvalService.GetAllApprovalType(),
				projects = await projectService.GetActiveProjectList()
			};
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> CreateNewLeavApprovalMatrix([FromForm] LeaveRegisterNewVM model)
		{
			ApprovalMaster master = new ApprovalMaster
			{
				Id = Convert.ToInt32(model.approvalMasterId),
				employeeInfoId = model.nRaiserId,
				approvalTypeId = model.leaveTypeId,

			};
			int MasterId = await approvalService.SaveApprovalMaster(master);

			if (model.approvalMasterId > 0)
			{
				await approvalService.DeleteapprovalDetailsByApprovalMasterId(Convert.ToInt32(MasterId));
			}
			int i = 0;
			if (model.nextApproversId != null)
			{
				if (model.nextApproversId.Length > 0)
				{

					for (i = 0; i < model.nextApproversId.Length; i++)
					{
						ApprovalDetail detail = new ApprovalDetail();

						detail.Id = 0;
						detail.approvalMasterId = MasterId;
						detail.approverId = Convert.ToInt32(model.nextApproversId[i]);
						detail.sortOrder = i + 1;
						detail.status = "Active";
						detail.isDelete = 0;

						await approvalService.SaveApprovalDetail(detail);
						detail = new ApprovalDetail();
					}

				}
			}
			if (model.nFinalId != null)
			{
				ApprovalDetail detail = new ApprovalDetail();
				detail.Id = 0;
				detail.approvalMasterId = MasterId;
				detail.approverId = Convert.ToInt32(model.nFinalId);
				detail.sortOrder = i + 1;
				detail.status = "Active";
				detail.isDelete = 1;
				await approvalService.SaveApprovalDetail(detail);
			}

			return RedirectToAction("LeaveApprovalList");
		}

		public async Task<IActionResult> NewLeaveApprovalList()
		{
			string userName = HttpContext.User.Identity.Name;
			List<ApprovalMastersVM> pro = new List<ApprovalMastersVM>();
			var raisers = await approvalService.GetAllApprovalMaster();

			foreach (var item in raisers)
			{
				var data = new ApprovalMastersVM
				{
					raiserName = item?.employeeInfo?.nameEnglish,
					raiserId = item?.employeeInfoId,
					raiserEmployeeCode = item?.employeeInfo?.employeeCode,
					approverTypeName = item?.approvalType?.approvalTypeName,
					approverTypeId = item?.approvalTypeId,
					raiserPhoto = "",
					approvalDetails = await approvalService.GetApprovalDetailByApprovalMasterId(item.Id)
				};
				pro.Add(data);
			};
			ApprovalListViewModel model = new ApprovalListViewModel
			{
				approvalMasters = pro
			};
			return View(model);
		}

		public async Task<IActionResult> LeaveApprovalList()
		{
			string userName = HttpContext.User.Identity.Name;
			List<ApprovalMastersVM> pro = new List<ApprovalMastersVM>();
			var raisers = await approvalService.GetAllApprovalMaster();

			foreach (var item in raisers)
			{
				var data = new ApprovalMastersVM
				{
					approvalMasterId = item?.Id,
					raiserName = item?.employeeInfo?.nameEnglish,
					raiserId = item?.employeeInfoId,
					raiserEmployeeCode = item?.employeeInfo?.employeeCode,
					approverTypeName = item?.approvalType?.approvalTypeName,
					approverTypeId = item?.approvalTypeId,
					raiserPhoto = await approvalService.GetLeaveApprovalRaiserImage(Convert.ToInt32(item.employeeInfoId)),
					approvalDetails = await approvalService.GetApprovalDetailByApprovalMasterIdWithImage(item.Id)
				};
				pro.Add(data);
			};
			ApprovalListViewModel model = new ApprovalListViewModel
			{
				approvalTypes = await approvalService.GetAllApprovalType(),
				approvalMasters = pro
			};
			return View(model);
		}
		#endregion

		// GET: LeaveApproval
		public async Task<IActionResult> WagesIndex()
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
				wagesLeaveRegisters = await wagesLeaveRegisterService.GetAllLeaveRegisterPending(),
			};
			return View(model);
		}

		public async Task<IActionResult> Index(int? id)
		{
			ViewBag.masterId = id;
			string userName = HttpContext.User.Identity.Name;
			var userInfos = await userInfo.GetUserInfoByUser(userName);
			//var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(userInfos?.EmpCode);
			var EmpInfo = await personalInfoService.GetEmployeeInfoByAspnetId(userInfos?.aspnetId);

			int empId = 0;
			if (EmpInfo != null)
			{
				empId = EmpInfo.Id;
			}
			LeaveRegisterViewModel model = new LeaveRegisterViewModel
			{
				fLang = _lang.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
				leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpId(empId),
				leaveRoutes = await leaveRouteService.GetLeaveRouteNewByEmpId(empId),
				leaveDays = await leavePolicyService.GetAllLeaveDay(),
				visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
				leaveStatusViewModels = await leavePolicyService.GetLeaveStatus(empId),
				approvalDetails = await leavePolicyService.GetApprovalDetailsByUser(userInfos.UserId)
			};
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> WagesAction([FromForm] LeaveApprovalViewModel model)
		{
			await wagesLeaveRegisterService.UpdateLeaveApproval((int)model.leaveId, Convert.ToInt32(model.leaveApprove));
			return RedirectToAction(nameof(WagesIndex));
		}

		public async Task<IActionResult> GetBalanceStats(int id)
		{
			var model = new LeaveRegisterViewModel
			{
				leaveStatusViewModels = await leavePolicyService.GetLeaveStatus(id)
			};
			return Json(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Action([FromForm] LeaveApprovalViewModel model)
		{
			//  return Json(model);
			LeaveRoute leaveRoute = await leaveRouteService.GetLeaveRouteById((int)model.id);
			int? nextOrder = leaveRoute.routeOrder + 1;
			int Status = 1;
			string mailtext = "";
			if (model.leaveApprove == "Approve")
			{
				Status = 2;
				mailtext = "Approved";
			}
			else if (model.leaveApprove == "Reject")
			{
				Status = 5;
				mailtext = "rejected";
			}
			else
			{
				Status = 4;
				mailtext = "returned";
			}

			await leaveRouteService.UpdateLeaveRoute(leaveRoute.Id, 0);

			LeaveStatusLog leaveStatusLog = new LeaveStatusLog
			{
				employeeId = model.employeeId,
				leaveRegisterId = model.leaveId,
				date = DateTime.Now,
				remarks = model.leaveApprove,
				comments = model.comments,
				Status = Status
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

			var employee = await personalInfoService.GetEmployeeInfoById((int)model.employeeId);
			var leavRegister = await leaveRegisterService.GetLeaveRegisterById((int)model.leaveId);
			var leaveEmployee = await personalInfoService.GetEmployeeInfoById((int)leavRegister.employeeId);

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
				await leaveRegisterService.UpdateLeaveApproval((int)model.leaveId, Status);

				if (leaveEmployee.emailAddress != null)
				{
					await emailSenderService.SendEmailWithFrom(leaveEmployee.emailAddress, employee.nameEnglish, "Leave Application", html1);
				}

				return RedirectToAction(nameof(Index));
			}
			else if (Status == 4)
			{
				await leaveRegisterService.UpdateLeaveApproval((int)model.leaveId, Status);
				if (leaveEmployee.emailAddress != null)
				{
					await emailSenderService.SendEmailWithFrom(leaveEmployee.emailAddress, employee.nameEnglish, "Leave Application", html1);
				}
				return RedirectToAction(nameof(Index));
			}

			LeaveRoute leaveRouteNew = await leaveRouteService.GetLeaveRouteByRouteOrder((int)leaveRoute.leaveRegisterId, (int)nextOrder);

			if (leaveRouteNew != null)
			{
				var leaveFutureEmployee = await personalInfoService.GetEmployeeInfoById((int)leaveRouteNew?.employeeId);
				await leaveRouteService.UpdateLeaveRoute(leaveRouteNew.Id, 1);
				await leaveRegisterService.UpdateLeaveApproval((int)model.leaveId, Status);
				if (leaveEmployee.emailAddress != null)
				{
					await emailSenderService.SendEmailWithFrom(leaveEmployee.emailAddress, employee.nameEnglish, "Leave Application", html1);
				}

				if (leaveFutureEmployee.emailAddress != null)
				{
					await emailSenderService.SendEmailWithFrom(leaveFutureEmployee.emailAddress, employee.nameEnglish, "Leave Application", htmlApprove);
				}

			}
			else
			{
				await leaveRegisterService.UpdateLeaveApproval((int)model.leaveId, 3);
				//string html = "<div><strong>Leave Application.</strong></div>"
				//            + " <br/>"
				//            + "<p>Dear Sir,</p>"
				//            + " <br/>"
				//            + " This is to inform you that "
				//            + "leave application"
				//            + " <br/>"
				//            + "Employee Name : '" + leaveEmployee.nameEnglish + "' "
				//            + " <br/>"
				//            + "Leave From : '" + leavRegister.leaveFrom?.ToString("dd-MMM-yyyy") + "' " + " To : '" + leavRegister.leaveTo?.ToString("dd-MMM-yyyy") + "' "
				//            + " <br/>"
				//            + "has approved by " + employee.nameEnglish + " just now."
				//            + "<br/>"
				//            + "<br/>"

				//            + "<div><p> Thank You & Best Regards</p><p style = 'font-weight:bold' > Human Resource Department.</p></div>"
				//            + " <br/>";
				string html = "<div><strong>Application for leave.</strong></div>"
								  + " <br/>"
								  + "Your requested leave (Start date:" + leavRegister.leaveFrom?.ToString("dd-MMM-yyyy") + ") has been finally approved."
								  + "<br/><br/>"
								  + "<b><u>Leave Details</u></b>"
								  + " <br/>"
								  + "Leave Type : " + leavRegister.leaveType?.nameEn + ""
								  + " <br/>"
								  + "Leave Duration :" + " From " + leavRegister.leaveFrom?.ToString("dd-MMM-yyyy") + " " + " To " + leavRegister.leaveTo?.ToString("dd-MMM-yyyy") + " "
								  + " <br/>"
								  + "Approver Remarks : "
								  + " <br/>"
								  + "Emergency Contact : " + leavRegister.emergencyContactNo
								  + "<br/>";
				if (leaveEmployee.emailAddress != null)
				{
					await emailSenderService.SendEmailWithFrom(leaveEmployee.emailAddress, employee.nameEnglish, "Leave Application", html);
				}
			}

			return RedirectToAction(nameof(Index));

		}

		[AllowAnonymous]
		[Route("api/global/GetLeavesAsSupervisor/{empCode}")]
		public async Task<IActionResult> PendingApprovalApi(string empCode)
		{
			var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(empCode);

			var model = await leaveRouteService.GetLeaveRouteNewByEmpId(EmpInfo.Id);
			return Json(model);
		}
		public async Task<IActionResult> GetApprovalMatrixByEmpId(int id)
		{
			var user = await leavePolicyService.UserByEmployeeId(id);
			var model = new LeaveRegisterViewModel
			{
				approvalDetails = await leavePolicyService.GetApprovalDetailsByUser(user.userId)
			};
			return Json(model);
		}
		[HttpPost]
		//[ValidateAntiForgeryToken]
		public async Task<IActionResult> ActionAll([FromForm] LeaveApprovalViewModel model)
		{
            //  return Json(model);
            try
            {
                string userName = HttpContext.User.Identity.Name;
                var user = await _userManager.FindByNameAsync(userName);
                var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(user?.EmpCode);

                var regLeave = await leaveRegisterService.GetLeaveRegisterById(Convert.ToInt32(model.registerids[0]));
                regLeave.leaveFrom = model.leaveFrom;
                regLeave.leaveTo = model.leaveTo;
                regLeave.daysLeave = model.daysLeave;
                await leaveRegisterService.SaveLeaveRegister(regLeave);


                int Status = 2;
                string mailtext = "recommended";

                if (model.registerids != null)
                {
                    for (int i = 0; i < model.registerids.Count(); i++)
                    {
                        LeaveRoute leaveRoute = await leaveRouteService.GetLeaveRouteById((int)model.ids[i]);
                        LeaveRegister leaveRegister = await leaveRegisterService.GetLeaveRegisterById((int)model.registerids[0]);
                        int? nextOrder = leaveRoute.routeOrder + 1;
                        await leaveRouteService.UpdateLeaveRoute(leaveRoute.Id, 0);
                        var registerLeave = await leaveRegisterService.CheckFinalByEmpIdAndRegId(leaveRegister.employeeId, user.userId);
                        if (registerLeave.isDelete != 1)
                        {
                            Status = 1;
                        }
                        LeaveStatusLog leaveStatusLog = new LeaveStatusLog
                        {
                            employeeId = leaveRegister.employeeId,
                            leaveRegisterId = model.registerids[i],
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
                        // var leavRegister = await leaveRegisterService.GetLeaveRegisterById((int)model.leaveId);
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
                            var device = await dbChangeService.GetUserLastLogHistory(leaveRegister.employee?.employeeCode);
                            if (device != null)
                            {
                                var pushNotify = new NotificationModel
                                {
                                    DeviceId = device?.deviceId,
                                    IsAndroiodDevice = true,
                                    Title = "Leave application from " + leaveRegister.employee?.nameEnglish,
                                    Body = "Leave application from " + leaveRegister.employee?.nameEnglish + " approved by " + EmpInfo?.nameEnglish,
                                };
                                await pushNotificationService.SendNotification(pushNotify);
                            }
                            var deviceApprover = await dbChangeService.GetUserLastLogHistory(leaveRouteNew?.employee?.employeeCode);
                            if (deviceApprover != null)
                            {
                                var pushNotify = new NotificationModel
                                {
                                    DeviceId = device?.deviceId,
									IsAndroiodDevice = true,
                                    Title = "Leave application from " + leaveRegister.employee?.nameEnglish +" is forwarded to you.",
                                    Body = leaveRegister.employee?.nameEnglish + " apply leave for " + leaveRegister.purposeOfLeave +". Leave application from " + leaveRegister.employee?.nameEnglish + " is forwarded to you.",
                                };
                                await pushNotificationService.SendNotification(pushNotify);
                            }
                            var leaveFutureEmployee = await personalInfoService.GetEmployeeInfoById((int)leaveRouteNew?.employeeId);
                            await leaveRouteService.UpdateLeaveRoute(leaveRouteNew.Id, 1);
                            await leaveRegisterService.UpdateLeaveApproval((int)model.registerids[i], Status);
                            if (leaveEmployee.emailAddress != null)
                            {
                                await emailSenderService.SendEmailWithFrom(leaveEmployee.emailAddress, employee.nameEnglish, "Leave Application", html1);
                            }

                            if (leaveFutureEmployee.emailAddress != null)
                            {
                                await emailSenderService.SendEmailWithFrom(leaveFutureEmployee.emailAddress, employee.nameEnglish, "Leave Application", htmlApprove);
                            }


                        }
                        else
                        {
                            await leaveRegisterService.UpdateLeaveApproval((int)model.registerids[0], 3);

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
                            var device = await dbChangeService.GetUserLastLogHistory(leaveRegister.employee?.employeeCode);
                            if (device != null)
                            {
                                var pushNotify = new NotificationModel
                                {
                                    DeviceId = device?.deviceId,
                                    IsAndroiodDevice = true,
                                    Title = "Leave Approved Finally",
                                    Body = "Your leave is approved finally by " + EmpInfo?.nameEnglish,
                                };
                                await pushNotificationService.SendNotification(pushNotify);
                            }
                            //string html = "<div><strong>Leave Application</strong></div>"
                            //            + " <br/>"
                            //            + "<p>Dear Sir,</p>"
                            //            + " <br/>"
                            //            + " This is to inform you that "
                            //            + "leave application"
                            //            + " <br/>"
                            //            + "Employee Name : '" + leaveEmployee.nameEnglish + "' "
                            //            + " <br/>"
                            //            + "Leave From : '" + leaveRegister.leaveFrom?.ToString("dd-MMM-yyyy") + "' " + " To : '" + leaveRegister.leaveTo?.ToString("dd-MMM-yyyy") + "' "
                            //            + " <br/>"
                            //            + "has approved by " + employee.nameEnglish + " just now."
                            //            + "<br/>"
                            //            + "<br/>"

                            //            + "<div><p> Thank You & Best Regards</p><p style = 'font-weight:bold' > Human Resource Department.</p></div>"
                            //            + " <br/>";
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
                                await emailSenderService.SendEmailWithFrom(leaveEmployee.emailAddress, employee.nameEnglish, "Leave Application", html);
                            }
                        }
                    }
                }

                //return RedirectToAction(nameof(Index));
                return Json("Saved");
            }
            catch (Exception ex)
            {

                throw ex;
            }
			

		}

		public async Task<IActionResult> UpdateLeaveRegister(int regId, DateTime? fromD, DateTime? toD, int daysLeave)
		{
			var leaveRegister = await leaveRegisterService.GetLeaveRegisterById(regId);
			leaveRegister.leaveFrom = fromD;
			leaveRegister.leaveTo = toD;
			leaveRegister.daysLeave = daysLeave;
			await leaveRegisterService.SaveLeaveRegister(leaveRegister);
			return Json("ok");
		}
		[HttpPost]
		//[ValidateAntiForgeryToken]
		public async Task<IActionResult> ActionAllReject([FromForm] LeaveApprovalViewModel model)
		{
            try
            {
                //  return Json(model);
                string userName = HttpContext.User.Identity.Name;
                var userInfos = await _userManager.FindByNameAsync(userName);


                int Status = 5;
                string mailtext = "Rejected";

                if (model.registerids != null)
                {
                    for (int i = 0; i < model.registerids.Count(); i++)
                    {
                        LeaveRoute leaveRoute = await leaveRouteService.GetLeaveRouteById((int)model.ids[i]);
                        LeaveRegister leaveRegister = await leaveRegisterService.GetLeaveRegisterById((int)model.registerids[0]);
                        int? nextOrder = leaveRoute.routeOrder + 1;
                        await leaveRouteService.UpdateLeaveRoute(leaveRoute.Id, 0);

                        LeaveStatusLog leaveStatusLog = new LeaveStatusLog
                        {
                            employeeId = leaveRegister.employeeId,
                            leaveRegisterId = model.registerids[i],
                            date = DateTime.Now,
                            remarks = "Rejected",
                            Status = Status,
                            comments = model.comments
                        };
                        await leaveStatusLogService.SaveLeaveStatusLog(leaveStatusLog);

                        var notification = new Notification
                        {
                            Id = 0,
                            senderId = userInfos.Id,
                            receiverId = leaveRegister.employee.ApplicationUser.Id,
                            date = DateTime.Now,
                            title = "Leave rejected by " + userInfos.UserName,
                            description = userInfos.UserName + " rejected your leave application",
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
                        // var leavRegister = await leaveRegisterService.GetLeaveRegisterById((int)model.leaveId);
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
                            await leaveRegisterService.UpdateLeaveApproval((int)model.registerids[i], Status);

                            if (leaveEmployee.emailAddress != null)
                            {
                                await emailSenderService.SendEmailWithFrom(leaveEmployee.emailAddress, employee.nameEnglish, "Leave Application", html1);
                            }

                            return RedirectToAction(nameof(Index));
                        }

                        LeaveRoute leaveRouteNew = await leaveRouteService.GetLeaveRouteByRouteOrder((int)leaveRoute.leaveRegisterId, (int)nextOrder);

                        if (leaveRouteNew != null)
                        {
                            var leaveFutureEmployee = await personalInfoService.GetEmployeeInfoById((int)leaveRouteNew?.employeeId);
                            await leaveRouteService.UpdateLeaveRoute(leaveRouteNew.Id, 1);
                            await leaveRegisterService.UpdateLeaveApproval((int)model.registerids[i], Status);
                            if (leaveEmployee.emailAddress != null)
                            {
                                await emailSenderService.SendEmailWithFrom(leaveEmployee.emailAddress, employee.nameEnglish, "Leave Application", html1);
                            }

                            if (leaveFutureEmployee.emailAddress != null)
                            {
                                await emailSenderService.SendEmailWithFrom(leaveFutureEmployee.emailAddress, employee.nameEnglish, "Leave Application", htmlApprove);
                            }

                        }
                        else
                        {
                            await leaveRegisterService.UpdateLeaveApproval((int)model.registerids[0], 3);
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
                                await emailSenderService.SendEmailWithFrom(leaveEmployee.emailAddress, employee.nameEnglish, "Leave Application", html);
                            }
                        }

                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                throw;
            }
			

		}

		[HttpPost]
		//[ValidateAntiForgeryToken]
		public async Task<IActionResult> ActionAllReturn([FromForm] LeaveApprovalViewModel model)
		{
			//  return Json(model);
			string userName = HttpContext.User.Identity.Name;
			var userInfos = await _userManager.FindByNameAsync(userName);


			int Status = 4;
			string mailtext = "Returned";

			if (model.registerids != null)
			{
				for (int i = 0; i < model.registerids.Count(); i++)
				{
					LeaveRoute leaveRoute = await leaveRouteService.GetLeaveRouteById((int)model.ids[i]);
					LeaveRegister leaveRegister = await leaveRegisterService.GetLeaveRegisterById((int)model.registerids[0]);
					int? nextOrder = leaveRoute.routeOrder + 1;
					await leaveRouteService.UpdateLeaveRoute(leaveRoute.Id, 0);

					LeaveStatusLog leaveStatusLog = new LeaveStatusLog
					{
						employeeId = leaveRegister.employeeId,
						leaveRegisterId = model.registerids[i],
						date = DateTime.Now,
						remarks = "Returned",
						Status = Status,
						comments = model.comments
					};
					await leaveStatusLogService.SaveLeaveStatusLog(leaveStatusLog);

					var notification = new Notification
					{
						Id = 0,
						senderId = userInfos.Id,
						receiverId = leaveRegister.employee.ApplicationUser.Id,
						date = DateTime.Now,
						title = "Leave returned by " + userInfos.UserName,
						description = userInfos.UserName + " returned your leave application",
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
					// var leavRegister = await leaveRegisterService.GetLeaveRegisterById((int)model.leaveId);
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
						await leaveRegisterService.UpdateLeaveApproval((int)model.registerids[i], Status);

						if (leaveEmployee.emailAddress != null)
						{
							await emailSenderService.SendEmailWithFrom(leaveEmployee.emailAddress, employee.nameEnglish, "Leave Application", html1);
						}

						return RedirectToAction(nameof(Index));
					}

					LeaveRoute leaveRouteNew = await leaveRouteService.GetLeaveRouteByRouteOrder((int)leaveRoute.leaveRegisterId, (int)nextOrder);

					if (leaveRouteNew != null)
					{
						var leaveFutureEmployee = await personalInfoService.GetEmployeeInfoById((int)leaveRouteNew?.employeeId);
						await leaveRouteService.UpdateLeaveRoute(leaveRouteNew.Id, 1);
						await leaveRegisterService.UpdateLeaveApproval((int)model.registerids[i], Status);
						if (leaveEmployee.emailAddress != null)
						{
							await emailSenderService.SendEmailWithFrom(leaveEmployee.emailAddress, employee.nameEnglish, "Leave Application", html1);
						}

						if (leaveFutureEmployee.emailAddress != null)
						{
							await emailSenderService.SendEmailWithFrom(leaveFutureEmployee.emailAddress, employee.nameEnglish, "Leave Application", htmlApprove);
						}

					}
					else
					{
						await leaveRegisterService.UpdateLeaveApproval((int)model.registerids[0], 3);
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
							await emailSenderService.SendEmailWithFrom(leaveEmployee.emailAddress, employee.nameEnglish, "Leave Application", html);
						}
					}

				}
			}

			return RedirectToAction(nameof(Index));

		}


		#region MobileAPI Section

		[Route("global/api/GetLeaveRouteByEmpId/{empId}")]
		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> GetLeaveRouteByEmpId(int empId)
		{


			LeaveRegisterViewModel model = new LeaveRegisterViewModel
			{

				leaveRoutes = await leaveRouteService.GetLeaveRouteByEmpId(empId),
			};
			return Json(model);
		}


		#endregion


		[HttpPost]
		[AllowAnonymous]
		[Route("api/LeaveApprovalAction")]
		public async Task<IActionResult> LeaveApprovalAction([FromBody] LeaveApprovalViewModel model)
		{
			//return Json(model);
			LeaveRoute leaveRoute = await leaveRouteService.GetLeaveRouteById((int)model.id);
			int? nextOrder = leaveRoute.routeOrder + 1;
			int Status = 1;
			string mailtext = "";
			if (model.leaveApprove == "Approve")
			{
				Status = 2;
				mailtext = "Approved";
			}
			else if (model.leaveApprove == "Reject")
			{
				Status = 5;
				mailtext = "rejected";
			}
			else if (model.leaveApprove == "Return")
			{
				Status = 4;
				mailtext = "rejected";
			}
			else
			{
				Status = 4;
				mailtext = "returned";
			}

			await leaveRouteService.UpdateLeaveRoute(leaveRoute.Id, 0);

			LeaveStatusLog leaveStatusLog = new LeaveStatusLog
			{
				employeeId = model.employeeId,
				leaveRegisterId = model.leaveId,
				date = DateTime.Now,
				remarks = model.leaveApprove,
				Status = Status
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

			var employee = await personalInfoService.GetEmployeeInfoById((int)model.employeeId);
			var leavRegister = await leaveRegisterService.GetLeaveRegisterById((int)model.leaveId);
			var leaveEmployee = await personalInfoService.GetEmployeeInfoById((int)leavRegister.employeeId);

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
				await leaveRegisterService.UpdateLeaveApproval((int)model.leaveId, Status);
				//await emailSenderService.SendEmailWithFrom(leaveEmployee.emailAddress, employee.nameEnglish, "Leave Application", html1);

			}
			else if (Status == 4)
			{
				await leaveRegisterService.UpdateLeaveApproval((int)model.leaveId, Status);
				//await emailSenderService.SendEmailWithFrom(leaveEmployee.emailAddress, employee.nameEnglish, "Leave Application", html1);

			}

			LeaveRoute leaveRouteNew = await leaveRouteService.GetLeaveRouteByRouteOrder((int)leaveRoute.leaveRegisterId, (int)nextOrder);

			if (leaveRouteNew != null)
			{
				var leaveFutureEmployee = await personalInfoService.GetEmployeeInfoById((int)leaveRouteNew?.employeeId);
				await leaveRouteService.UpdateLeaveRoute(leaveRouteNew.Id, 1);
				await leaveRegisterService.UpdateLeaveApproval((int)model.leaveId, Status);
				//await emailSenderService.SendEmailWithFrom(leaveEmployee.emailAddress, employee.nameEnglish, "Leave Application", html1);
				//await emailSenderService.SendEmailWithFrom(leaveFutureEmployee.emailAddress, employee.nameEnglish, "Leave Application", htmlApprove);
			}
			else
			{
				await leaveRegisterService.UpdateLeaveApproval((int)model.leaveId, Status);
				string html = "<div><strong>Leave Application.</strong></div>"
							+ " <br/>"
							+ "<p>Dear Sir,</p>"
							+ " <br/>"
							+ " This is to inform you that "
							+ "leave application"
							+ " <br/>"
							+ "Employee Name : '" + leaveEmployee.nameEnglish + "' "
							+ " <br/>"
							+ "Leave From : '" + leavRegister.leaveFrom?.ToString("dd-MMM-yyyy") + "' " + " To : '" + leavRegister.leaveTo?.ToString("dd-MMM-yyyy") + "' "
							+ " <br/>"
							+ "has approved by " + employee.nameEnglish + " just now."
							+ "<br/>"
							+ "<br/>"

							+ "<div><p> Thank You & Best Regards</p><p style = 'font-weight:bold' > Human Resource Department.</p></div>"
							+ " <br/>";
				//  await emailSenderService.SendEmailWithFrom(leaveEmployee.emailAddress, employee.nameEnglish, "Leave Application", html);
			}

			return Json(leavRegister);

		}

		[HttpGet]

		public async Task<IActionResult> GetAllApproveLeave()
		{
			LeaveRegisterViewModel model = new LeaveRegisterViewModel
			{
				leaveRegisterslist = await leaveRegisterService.GetAllLeaveApprovByEmpIdAndStatus(3)
			};
			return View(model);
		}

		[HttpGet]

		public async Task<IActionResult> GetAllPendingLeave()
		{
			LeaveRegisterViewModel model = new LeaveRegisterViewModel
			{
				leaveRegisterslist = await leaveRegisterService.GetAllLeaveApprovByEmpIdAndStatus(1)
			};
			return View(model);
		}

		public async Task<IActionResult> GetAllLeave(int userId)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			LeaveRegisterViewModel model = new LeaveRegisterViewModel
			{
				leaveRegisterslist = await leaveRegisterService.GetAllApproveLeaveByUserId(user.userId)
				//leaveRegisterslist = await leaveRegisterService.GetAllLeaveByEmpId(id)
			};
			return View(model);
		}

        
        [AllowAnonymous]
        [Route("api/LeaveAppr/GetLeaveReqByApproverId/{empCode}")]
        public async Task<IActionResult> GetLeaveRequesterByApproverId(string empCode)
        {
            //var employee = await personalInfoService.GetEmployeeInfoByCode(empCode);
            var user = await _userManager.FindByNameAsync(empCode);
            var allLeaves = await leaveRegisterService.GetLeaveRequesterByApproverId(user.userId);
            return Json(allLeaves);
        }

        [AllowAnonymous]
		[Route("api/global/GetAllLeaveApi/{empCode}")]
		public async Task<IActionResult> GetAllLeaveApi(string empCode)
		{
			var user = await _userManager.FindByNameAsync(empCode);
			var allLeaves = await leaveRegisterService.GetAllApproveLeaveByUserId(user.userId);
			return Json(allLeaves);
		}


		public async Task<IActionResult> CancelApprovedLeave(int id)
		{
			await leaveRegisterService.CancelApprovedLeave(id);
			return RedirectToAction("GetAllApproveLeave");
		}

		#region Leave Approval module
		[HttpGet]

		public async Task<IActionResult> AllLeaveApproved(int userId, string status)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);

			LeaveRegisterViewModel model = new LeaveRegisterViewModel
			{
				leaveRegisterslist = await leaveRegisterService.GetAllApproveLeaveByUserIdStatus(user.userId, 2)

			};
			return View(model);
		}

		[HttpGet]
		[AllowAnonymous]
		[Route("api/global/GetApprovedLeavesAsSupervisor/{empCode}")]
		public async Task<IActionResult> GetApprovedLeavesAsSupervisor(string empCode)
		{
			var user = await _userManager.FindByNameAsync(empCode);
			var model = await leaveRegisterService.GetAllApproveLeaveByUserIdStatus(user.userId, 2);
			return Json(model);
		}
		
		[HttpGet]
		[AllowAnonymous]
		[Route("api/global/GetAllPendingLeaveBySupervisorCode/{empCode}")]
		public async Task<IActionResult> GetAllPendingLeaveBySupervisorCode(string empCode)
		{
			var user = await _userManager.FindByNameAsync(empCode);
			var model = await leaveRegisterService.GetAllPendingLeaveBySupervisorUserId(user.userId);
			return Json(model);
		}

		[HttpGet]

		public async Task<IActionResult> AllReject(int userId, string status)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);

			LeaveRegisterViewModel model = new LeaveRegisterViewModel
			{
				leaveRegisterslist = await leaveRegisterService.GetAllApproveLeaveByUserIdStatus(user.userId, 5)

			};
			return View(model);
		}
		[HttpGet]

		public async Task<IActionResult> AllReturn(int userId, string status)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);

			LeaveRegisterViewModel model = new LeaveRegisterViewModel
			{
				leaveRegisterslist = await leaveRegisterService.GetAllApproveLeaveByUserIdStatus(user.userId, 4)

			};
			return View(model);
		}
		[HttpGet]
		public async Task<IActionResult> AllLeave(int userId)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);

			LeaveRegisterViewModel model = new LeaveRegisterViewModel
			{
				leaveRegisterslist = await leaveRegisterService.GetAllApproveLeaveByUserId(user.userId)

			};
			return View(model);
		}

		#endregion




	}

}