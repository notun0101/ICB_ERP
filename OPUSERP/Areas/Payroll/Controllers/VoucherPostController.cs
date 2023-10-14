using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.Payroll.Services.Salary.Interfaces;

namespace OPUSERP.Areas.Payroll.Controllers
{
	[Area("Payroll")]
	public class VoucherPostController : Controller
    {
		private readonly ISalaryService salaryService;
		private readonly IPhotographService photographService;
		private readonly IPersonalInfoService personalInfoService;
		private readonly IEmployeeProjectActivityService employeeProjectActivityService;
		public VoucherPostController(ISalaryService salaryService, IPhotographService photographService, IPersonalInfoService personalInfoService, IEmployeeProjectActivityService employeeProjectActivityService)
		{
			this.salaryService = salaryService;
			this.photographService = photographService;
			this.personalInfoService = personalInfoService;
			this.employeeProjectActivityService = employeeProjectActivityService;
		}
		public IActionResult Index()
        {
            return View();
        }
    }
}