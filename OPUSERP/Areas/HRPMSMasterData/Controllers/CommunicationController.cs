using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Areas.HRPMSMasterData.Models;
using OPUSERP.Areas.HRPMSMasterData.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.ERPServices.EmailService.interfaces;
using OPUSERP.ERPService.AuthService.Interfaces;

namespace OPUSERP.Areas.HRPMSMasterData.Controllers
{
	[Authorize]
	[Area("HRPMSMasterData")]
	public class CommunicationController : Controller
	{
		private readonly IStatusService statusService;
		private readonly IPersonalInfoService personalInfoService;
		private readonly IEmailSenderService emailSenderService;
		private readonly IUserInfoes userInfo;

		public CommunicationController(IHostingEnvironment hostingEnvironment, IUserInfoes userInfo, IStatusService statusService, IPersonalInfoService personalInfoService, IEmailSenderService emailSenderService)
		{
			this.statusService = statusService;
			this.userInfo = userInfo;
			this.personalInfoService = personalInfoService;
			this.emailSenderService = emailSenderService;
		}

		public async Task<IActionResult> Index()
		{

			HrCommunicationViewModel model = new HrCommunicationViewModel
			{
				hrCommunications = await statusService.GetHrCommunication()
			};

			return View(model);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Index([FromForm] HrCommunicationViewModel model)
		{

			string userName = HttpContext.User.Identity.Name;
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

			if (!ModelState.IsValid)
			{

				model.hrCommunications = await statusService.GetHrCommunication();
				return View(model);
			}

			ViewBag.employeeId = empId;
			HrCommunication data = new HrCommunication
			{

				Id = model.CommunicationId,
				employeeId = model.employeeId,
				refNo = model.refNo,
				email = model.email,
				date = model.date,
				status = 1,
				remarks = "condolance"

			};

			await statusService.SaveHrCommunication(data);



			return RedirectToAction(nameof(Index));
		}

		public async Task<JsonResult> DeleteCondolence(int Id)
		{
			await statusService.DeleteHrCommunicationById(Id);
			return Json(true);
		}
        public async Task<JsonResult> DeleteRetirement(int Id)
		{
			await statusService.DeleteRetirementById(Id);
			return Json(true);
		}

		public async Task<IActionResult> Retirement()
		{

			HrCommunicationViewModel model = new HrCommunicationViewModel
			{
				hrCommunications = await statusService.GetHrCommunication()
			};

			return View(model);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Retirement([FromForm] HrCommunicationViewModel model)
		{



			if (!ModelState.IsValid)
			{

				model.hrCommunications = await statusService.GetHrCommunication();
				return View(model);
			}

			HrCommunication data = new HrCommunication
			{

				Id = model.CommunicationId,
				employeeId = model.employeeId,
				refNo = model.refNo,
				email = model.email,
				date = model.date,
				status = 1,
				remarks = "retirement"

			};

			await statusService.SaveHrCommunicationForRetirement(data);

			return RedirectToAction(nameof(Retirement));
		}



		public async Task<IActionResult> GetEmployeeInfoById(int id)
		{
			var data = await personalInfoService.GetEmployeeInfoById(id);
			return Json(data);
		}
		public async Task<IActionResult> HRCommunications()
		{

			HrCommunicationViewModel model = new HrCommunicationViewModel
			{
				hrCommunications = await statusService.GetHrCommunication()
			};

			return View(model);
		}


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> SendCondolenceMailByComId(int id)
        {
            var data = await statusService.GetHrCommunicationById(id)
;
            var body = "<div class='letter' style='font-family: corbel;'>" +
                        "<p class='name' style='padding-top:40px;'>Dear," + data.employee?.nameEnglish + ",</p>" +
                        "<p>No words can express the deep shock and grief We felt, when we heard about the sad and untimely demise<br>of your dear father.He was ,undoubtedly, a man of great courage,rare virtues and an endearing personality. His absence would <br>always be felt.</p>" +
                        "<br>" +
                        "<p>We offer my sincere condolence to you and your family.We pray to God to give you courage and  fortitude to bear this <br>unbearable loss.</p>" +
                        "<p>Your sincerely,</p>" +
                        "<p>Mr.X</p>" +
                        "<p>BDBL</p>" +
                        "</div>";
            await emailSenderService.SendEmailWithFrom(data.email, "BDBL HR", "Condollence", body);

            data.status = 2;
            data.mailBody = body;
            data.mailSendingDate = DateTime.Now;
            await statusService.SaveHrCommunication(data);
            return Json("ok");
        }

        [AllowAnonymous]
		[HttpGet]
		public async Task<IActionResult> SendRetirementMailByComId(int id)
		{
			var data = await statusService.GetHrCommunicationById(id);
			var body = "<div class='letter' style='font-family: corbel;'>" +
						"<p class='name' style='padding-top:40px;'>Dear,"+data.employee?.nameEnglish+",</p>" +
                        "<p>On behalf of our company, we must inform you with a heavy heart that you have been given freedom from all your services.<br>" +
                        " You have been an incredible employee all these years. You have served the company for more than twenty-five years,<br>" +
                        " without any complaint or discrepancy. You received the best employee award six times in the last ten years.<br></p>" +

                        "<br>" +
                        "<p>Your intelligence and experience will be missed. There have been many cases when the company has <br>" +
                        "relied on your instincts to make crucial decisions and emerged successfully.<br>" +
                        " We cannot be more obliged to you. We have settled your accounts and wished you all the luck in the world.</p>" +
						"<p>Your sincerely,</p>" +
						"<p>Mr.X</p>" +
						"<p>BDBL</p>" +
						"</div>";
			await emailSenderService.SendEmailWithFrom(data.email, "BDBL HR", "Retirement", body);

			data.status = 2;
			data.mailBody = body;
			data.mailSendingDate = DateTime.Now;
			await statusService.SaveHrCommunicationForRetirement(data);
			return Json("ok");
		}

	}
}