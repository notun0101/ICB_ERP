using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Accounting.Services.MasterData.Interfaces;
using OPUSERP.Data.Entity;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.HRPMS.Services.ACR.Interfaces;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;

namespace OPUSERP.Areas.HRPMSACR.Controllers
{
	[Area("HRPMSACR")]
	public class ExecutiveOfficer2019Controller : Controller
    {
		private readonly IPersonalInfoService personalInfoService;
		private readonly IPhotographService photographService;
		private readonly ISignatureService signatureService;
		private readonly IAcrInfoService acrInfoService;
		private readonly IUserInfoes userInfoes;
		private readonly IEmployeePunchCardInfoService employeePunchCardInfoService;
		private readonly UserManager<ApplicationUser> _userManager;

		public ExecutiveOfficer2019Controller(
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



		public IActionResult Index()
        {
            return View();
        }
    }
}