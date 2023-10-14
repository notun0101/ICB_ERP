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
using OPUSERP.Data.Entity;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.Employee;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
    [Area("HRPMSEmployee")]
    public class EmployeeAttachmentController : Controller
    {
        private readonly IHRPMSAttachmentService hRPMSAttachmentService;
        private readonly IAttachmentCategoryService attachmentCategoryService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly IPhotographService photographService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly MyPDF myPDF;
        private readonly string rootPath;
           public EmployeeAttachmentController(IHostingEnvironment hostingEnvironment,IConverter converter,IHRPMSAttachmentService hRPMSAttachmentService, IPhotographService photographService, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IAttachmentCategoryService attachmentCategoryService, IPersonalInfoService personalInfoService)
        {
            this.hRPMSAttachmentService = hRPMSAttachmentService;
            this.attachmentCategoryService = attachmentCategoryService;
            this.personalInfoService = personalInfoService;
            this.photographService = photographService;
            _roleManager = roleManager;
            _userManager = userManager;
            myPDF = new MyPDF(hostingEnvironment, converter);
            rootPath = hostingEnvironment.ContentRootPath;
        }


        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            HRPMSAttachmentViewModel model = new HRPMSAttachmentViewModel
            {
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                atttachmentCategory = await attachmentCategoryService.GetAllAttachmentCategory(),
                hRPMSAttachments = await hRPMSAttachmentService.GetHRPMSAttachmentByEmpId(id),
                employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id),
                atttachmentGroups= await attachmentCategoryService.GetAllAtttachmentGroup()
            };
            return View(model);
        }

        // POST: Language/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] HRPMSAttachmentViewModel  model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            if (!ModelState.IsValid)
            {
                model.atttachmentCategory = await attachmentCategoryService.GetAllAttachmentCategory();
                model.hRPMSAttachments = await hRPMSAttachmentService.GetHRPMSAttachmentByEmpId(model.employeeId);
                model.atttachmentGroups = await attachmentCategoryService.GetAllAtttachmentGroup();
                return View(model);
            }

          //  string fileName = String.Empty;
         //   string fileNameMain = String.Empty;
           // string message = FileSave.SaveEmpAttachment(out fileName, model.fileUrl);
         //   string message = FileSave.SaveImageNew(out fileName, model.fileUrl);

            string fileName;

            if (model.fileUrl != null)
            {
                string message = FileSave.SaveImageNew(out fileName, model.fileUrl);
            }
            else
            {
                fileName = await hRPMSAttachmentService.GetAttachmentUrlById(model.attachmentId);
            };

            //if (message == "success")
            //{
            //    fileNameMain = fileName;
            //}

            HRPMSAttachment data = new HRPMSAttachment
            { 
                Id = model.attachmentId,
                employeeId = model.employeeId,
                atttachmentCategoryId = model.atttachmentCategoryId,
                fileTitle = model.fileTitle,
                fileUrl = fileName,
                remarks = model.remarks,
                attachmentDate=model.attachmentDate

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
            await hRPMSAttachmentService.SaveHRPMSAttachment(data);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id, int empId)
        {
            await hRPMSAttachmentService.DeleteHRPMSAttachmentById(id);
            return RedirectToAction("Index", "EmployeeAttachment", new
            {
                id = empId
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAttachmentCategoryByGroupId(int masterId)
        {
            return Json(await attachmentCategoryService.GetAllAttachmentCategoryByGroupId(masterId));
        }

        [HttpPost]
        public async Task<IActionResult> Group(HRPMSAttachmentViewModel model)
        {
            var data = new AtttachmentGroup
            {
                Id = 0,
                groupName = model.groupName


            };
            await attachmentCategoryService.SaveAtttachmentGroup(data);
            return Json("saved");
        }

        public async Task<IActionResult> GetAllGroup()
        {

            return Json(await attachmentCategoryService.GetAllAtttachmentGroup());
        }

        [HttpPost]
        public async Task<IActionResult> Category(HRPMSAttachmentViewModel model)
        {
            var data = new AtttachmentCategory
            {
                Id = 0,
                categoryName = model.categoryName,
                atttachmentGroupId = model.atttachmentGroupId


            };
            await attachmentCategoryService.SaveAttachmentCategory(data);
            return Json("saved");
        }

        public async Task<IActionResult> GetAllCategory()
        {

            return Json(await attachmentCategoryService.GetAllAttachmentCategory());
        }
        [AllowAnonymous]
        public IActionResult PfPdf(int id)
		{
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;

            string url = scheme + "://" + host + "/HRPMSEmployee/EmployeeAttachment/PoliceVarificationView/" + id;
            string status = myPDF.GeneratePDF1(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
            //return Json("ok");
		}
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> PoliceVarificationView(int id)
        {
            var model = new HRPMSAttachmentViewModel()
            {

                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),


            };
            return View(model);
          
        }


    }
}