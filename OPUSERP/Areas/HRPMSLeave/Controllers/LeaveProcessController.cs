using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSLeave.Models;
using OPUSERP.Areas.HRPMSLeave.Models.Lang;
using OPUSERP.Data.Entity;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.ERPServices.EmailService.interfaces;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.Leave;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.Leave.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.Payroll.Services.Salary.Interfaces;
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
    public class LeaveProcessController : Controller
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

        private readonly string rootPath;
        private readonly MyPDF myPDF;

        public LeaveProcessController(IHostingEnvironment hostingEnvironment, IRequisitionService requisitionService, UserManager<ApplicationUser> _userManager, IERPCompanyService eRPCompanyService, IConverter converter, ILeaveRegisterService leaveRegisterService, ILeaveTypeService leaveTypeService, ILeaveRouteService leaveRouteService, IUserInfoes userInfo, IPersonalInfoService personalInfoService, ISupervisorService supervisorService, ILeaveStatusLogService leaveStatusLogService, IEmailSenderService emailSenderService, ILeavePolicyService leavePolicyService, IApprovalService approvalService, IYearCourseTitleService yearCourseTitleService, IWagesLeaveRegisterService wagesLeaveRegisterService, ISalaryService salaryService)
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
            //For PDF
            myPDF = new MyPDF(hostingEnvironment, converter);
            rootPath = hostingEnvironment.ContentRootPath;
        }

        #region Leave Process
		public async Task<IActionResult> YearlyLeaveProcess()
		{

            YearlyLeaveProcessViewModel model = new YearlyLeaveProcessViewModel
            {
                AllYears = await leaveRegisterService.GetAllYear()
            };

            return View(model);
		
		}
        [HttpGet]
		public async Task<IActionResult> YearlyLeaveProcessApi(int yearid)
		{
			
			var opBalances = await leaveRegisterService.GetOpeningBalanceByYearId(yearid);
            return Json("ok");
		
		}
		
		
		public async Task<IActionResult> LeaveProcessList()
        {
            YearlyLeaveProcessViewModel model = new YearlyLeaveProcessViewModel
            {
                yearlyLeaveProcesses = await leaveRegisterService.GetAllLeaveProcess()
            };
            return View(model);
		}
		#endregion		
	}
}