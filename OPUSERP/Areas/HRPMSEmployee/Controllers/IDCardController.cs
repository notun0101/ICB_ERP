using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSEmployee.Models;
//using OPUSERP.CLUB.Services.Member.Interfaces;
//using OPUSERP.Areas.Auth.Models;
using OPUSERP.Data.Entity;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.IDCard.Interface;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
	[Authorize]
	[Area("HRPMSEmployee")]
	public class IDCardController : Controller
    {
		private readonly IIdCard IdCardService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPersonalInfoService personalInfoService;
        private readonly IPhotographService photographService;
        private readonly ISpouseChildrenService spouseChildrenService;
        private readonly HRPMS.Services.MasterData.Interfaces.IAddressService addressService;
        private readonly IERPCompanyService eRPCompanyService;
        private readonly string rootPath;
        private readonly MyPDF myPDF;

        public IDCardController(IHostingEnvironment hostingEnvironment, IIdCard idCardService, IConverter converter, UserManager<ApplicationUser> userManager , IPhotographService photographService, IPersonalInfoService personalInfoService, ISpouseChildrenService spouseChildrenService, HRPMS.Services.MasterData.Interfaces.IAddressService addressService , IERPCompanyService eRPCompanyService)
		{
			IdCardService = idCardService;
            _userManager = userManager;
            this.photographService = photographService;
            this.personalInfoService = personalInfoService;
            this.spouseChildrenService = spouseChildrenService;
            this.addressService = addressService;

            myPDF = new MyPDF(hostingEnvironment, converter);
            rootPath = hostingEnvironment.ContentRootPath;
        }

		public async Task<IActionResult> Index(int id)
		{
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.employeeID = id.ToString();
            var model = new EmployeeInfoViewModel
            {
                employeeID = id.ToString(),
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                Spouses = await spouseChildrenService.GetSpouseByEmpIdForSingle(id),
                Addresss = await addressService.GetAddressByEmpForSingle(id)
             };
            return View();
        }
        public async Task<IActionResult> IndexNew(int id)
		{
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.employeeID = id.ToString();
            var model = new EmployeeInfoViewModel
            {
                employeeID = id.ToString(),
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                Spouses = await spouseChildrenService.GetSpouseByEmpIdForSingle(id),
                Addresss = await addressService.GetAddressByEmpForSingle(id)
             };
            return View();
        }

        public async Task<IActionResult> IndexApi(int id)
		{
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.employeeID = id.ToString();
            var model = new EmployeeInfoViewModel
            {
                employeeID = id.ToString(),
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                Spouses = await spouseChildrenService.GetSpouseByEmpIdForSingle(id),
                Addresss = await addressService.GetAddressByEmpForSingle(id)
            };
            return Json(model);
        }


        [AllowAnonymous]
        public async Task<IActionResult> GenerateIdCard(int id, string valid)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.employeeID = id.ToString();
            //ViewBag.valid = valid;
            ViewBag.valid = DateTime.Now.ToShortDateString();
            var model = new EmployeeInfoViewModel
            {
                employeeID = id.ToString(),
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                Spouses = await spouseChildrenService.GetSpouseByEmpIdForSingle(id),
                Addresss = await addressService.GetAddressByEmpForSingle(id)
            };
            return View(model);
        }
        
        [AllowAnonymous]
        public async Task<IActionResult> GenerateIdCardNew(int id, string valid)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.employeeID = id.ToString();
            //ViewBag.valid = valid;
            ViewBag.valid = DateTime.Now.ToShortDateString();
            var model = new EmployeeInfoViewModel
            {
                employeeID = id.ToString(),
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                Spouses = await spouseChildrenService.GetSpouseByEmpIdForSingle(id),
                Addresss = await addressService.GetAddressByEmpForSingle(id),
              //  companies = await eRPCompanyService.GetAllCompany(),
            };
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult GenerateIdCardPdf(int id, string valid)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName = "";
            var url = scheme + "://" + host + "/HRPMSEmployee/IDCard/GenerateIdCard?id=" + id + "&valid=" + valid;
            var status = myPDF.GeneratePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [AllowAnonymous]
        public IActionResult GenerateIdCardPdfNew(int id, string valid)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName = "";
            var url = scheme + "://" + host + "/HRPMSEmployee/IDCard/GenerateIdCardNew?id=" + id + "&valid=" + valid;
            var status = myPDF.GeneratePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
    }
}