using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
    [Authorize]
    [Area("HRPMSEmployee")]
    public class DualResidenceController : Controller
    {
        private readonly IPhotographService photographService;
        private readonly IPersonalInfoService personalInfoService;
        public DualResidenceController()
        {

        }
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
           
            return View();
        }

    }
}