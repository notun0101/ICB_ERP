﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.Data.Entity;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.SCM.Services.MasterData.Interfaces;
using System;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
    [Authorize]
    [Area("HRPMSEmployee")]
    public class TravelController : Controller
    {
        private readonly IProjectService projectService;
        private readonly ITravelService travelService;
        private readonly ITravelInfoService travelInfoService;
        private readonly IPhotographService photographService;
       
        private readonly IAddressService addressService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly IStatusService statusService;
      
        private readonly LangGenerate<Travel> _lang;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public TravelController(IHostingEnvironment hostingEnvironment, IPhotographService photographService, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IPersonalInfoService personalInfoService, IAddressService addressService, ITravelInfoService travelInfoService, ITravelService travelService, IStatusService statusService, IProjectService projectService)
        {
            _lang = new LangGenerate<Travel>(hostingEnvironment.ContentRootPath);
            this.travelInfoService = travelInfoService;
            this.photographService = photographService;
            this.travelService = travelService;
            this.addressService = addressService;
            this.personalInfoService = personalInfoService;
            this.statusService = statusService;
            this.projectService = projectService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: Travel
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            var model = new TraveInfoViewModel
            {
                employeeID = id.ToString(),
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                traveInfos = await travelInfoService.GetTraveInfoByEmpId(id),
                travelPurposes = await travelService.GetTravelPurposes(),
                countries = await addressService.GetAllContry(),
                hrPrograms = await statusService.GetHrProgram(),
                //projects = await projectService.GetProjectList(),
                fLang = _lang.PerseLang("Employee/TraveInfoEN.json", "Employee/TraveInfoBN.json", Request.Cookies["lang"]),
                employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id)
            };
            return View(model);
        }

        // POST: Travel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] TraveInfoViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            if (!ModelState.IsValid)
            {
                ViewBag.employeeID = model.employeeID;
                model.traveInfos = await travelInfoService.GetTraveInfo();
                model.travelPurposes = await travelService.GetTravelPurposes();
                model.countries = await addressService.GetAllContry();
                model.hrPrograms = await statusService.GetHrProgram();
                //model.projects = await projectService.GetProjectList();
                model.fLang = _lang.PerseLang("Employee/TraveInfoEN.json", "Employee/TraveInfoBN.json", Request.Cookies["lang"]);
                return View(model);
            }
            string fileName = "";
            if (model.goAttachment != null)
            {
                FileSave.SaveImageNew(out fileName, model.goAttachment);
                // FileSave.SaveFile(out fileName, model.goAttachment, "DecActionFiles");
            }
            else
            {
                fileName = model.goAttachmentExist;
            }
            TraveInfo data = new TraveInfo
            {
                Id = model.travelId,
                employeeId = Int32.Parse(model.employeeID),
                travelPurposeId = Int32.Parse(model.type),
                titleName = model.titleName,
                location = model.location,
                countryId = Int32.Parse(model.country),
                sponsoringAgency = model.sponsoringAgency,
                startDate = model.startDate,
                endDate = model.endDate,
                goNumber = model.goNumber,
                goDate = model.goDate,
                file = fileName,
                titleOfFile = model.titleOfFile,
                remarks = model.remarks,
                accountCode = model.accountCode,
                hrProgramId = model.hrProgramId,
                //projectId = model.projectId,
                purpose = model.purpose,
                leaveStartDate = model.leaveStartDate,
                leaveEndDate = model.leaveEndDate


            };
            if (roles.Contains("HRAdmin") || roles.Contains("admin"))
            {
                data.isDelete = 0;
            }
            else
            {
               data.isDelete = 1;
                //await employeeInfoService.ChangeEmployeeActivityStatus(model.employeeID, 3);
            }
            await travelInfoService.SaveTraveInfo(data);

            return RedirectToAction(nameof(Index));
        }

        // Delete: Travel
        public async Task<IActionResult> Delete(int id, int empId)
        {
            await travelInfoService.DeleteTraveInfoById(id);
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "Travel", new
            {
                id = empId
            });
        }

    }
}