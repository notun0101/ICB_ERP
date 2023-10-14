using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using OPUSERP.Areas.HRPMSAssignment.Helpers;
using OPUSERP.Areas.HRPMSRetirementAndTermination.Models;
using OPUSERP.Areas.HRPMSRetirementAndTermination.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.RetirementAndTermination;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Services.RetirementAndTermination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.HRPMS.Services.RetirementAndTermination.Interface;
using OPUSERP.Data.Entity;
using Microsoft.AspNetCore.Identity;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using System.IO;
using OPUSERP.Areas.HRPMSEmployee.Models;

namespace OPUSERP.Areas.HRPMSRetirementAndTermination.Controllers
{
    [Authorize]
    [Area("HRPMSRetirementAndTermination")]
    public class ApplyForPRLController : Controller
    {
        private readonly LangGenerate<PRLApplicationLn> _lang;
        private readonly IPRLEntryService pRLEntryService;
        private readonly IResignInformationService resignInformationService;
        private readonly IPersonalInfoService personalInfoService;
		private readonly UserManager<ApplicationUser> _userManager;
        private readonly string rootPath;
        private readonly MyPDF myPDF;

        //public ApplyForPRLController(IHostingEnvironment hostingEnvironment, IResignInformationService resignInformationService, IPRLEntryService pRLEntryService, IPersonalInfoService personalInfoService)
        public ApplyForPRLController(IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, IResignInformationService resignInformationService, IPRLEntryService pRLEntryService, IPersonalInfoService personalInfoService, DinkToPdf.Contracts.IConverter converter)
        {
            this.resignInformationService = resignInformationService;
            _lang = new LangGenerate<PRLApplicationLn>(hostingEnvironment.ContentRootPath);
            this.pRLEntryService = pRLEntryService;
            this.personalInfoService = personalInfoService;
            _userManager = userManager;
            myPDF = new MyPDF(hostingEnvironment, converter);
            rootPath = hostingEnvironment.ContentRootPath;
        }

        public IActionResult Index()
        {
            var model = new PRLApplicationViewModel()
            {
                //flang = _lang.PerseLang("RetirementTermination/PRLApplicationBN.json")
                flang = _lang.PerseLang("RetirementTermination/PRLApplicationEN.json", "RetirementTermination/PRLApplicationBN.json", Request.Cookies["lang"])
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] PRLApplicationViewModel model)
        {
            if (!ModelState.IsValid)
            {

                //model.flang = _lang.PerseLang("RetirementTermination/PRLApplicationBN.json");
                model.flang = _lang.PerseLang("RetirementTermination/PRLApplicationEN.json", "RetirementTermination/PRLApplicationBN.json", Request.Cookies["lang"]);
                return View(model);
            }

            PRLApplication data = new PRLApplication
            {
                applicationDate = model.applicationDate,
                employeeId = Convert.ToInt32(model.employeeId),
                applicationName = model.applicationName,
                status = "Pending"
            };

            await pRLEntryService.SavePrlEntry(data);

            return RedirectToAction(nameof(ApprovePRLApplication));
        }

        public async Task<IActionResult> PRLApplicationList()
        {
            var model = new PRLApplicationViewModel()
            {
                //flang = _lang.PerseLang("RetirementTermination/PRLApplicationBN.json"),
                flang = _lang.PerseLang("RetirementTermination/PRLApplicationEN.json", "RetirementTermination/PRLApplicationBN.json", Request.Cookies["lang"]),
                pRLApplications = await pRLEntryService.GetPRLEntryByStatus("Approved")
            };
            return View(model);
        }

		public async Task<IActionResult> GetMyResignation()
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var employeeInfo = await personalInfoService.GetEmployeeInfoByApplicationId(user.Id);

			var model = new PRLApplicationViewModel
			{
				resignInformation = await resignInformationService.GetResignationListByEmpId(employeeInfo.Id)
			};
			return View(model);
		}
        public async Task<IActionResult> ApprovePRLApplication()
        {
            var model = new PRLApplicationViewModel()
            {
                //flang = _lang.PerseLang("RetirementTermination/PRLApplicationBN.json"),
                flang = _lang.PerseLang("RetirementTermination/PRLApplicationEN.json", "RetirementTermination/PRLApplicationBN.json", Request.Cookies["lang"]),
                pRLApplications = await pRLEntryService.GetPRLEntryByStatus("Pending")
            };
            return View(model);
        }

        public async Task<IActionResult> Approve([FromForm] PRLApplication model)
        {
            await pRLEntryService.UpdatePRLStatus(model.Id, model.fromDate, model.toDate, model.duration, model.status);

            return RedirectToAction(nameof(ApprovePRLApplication));

        }

        public async Task<IActionResult> GetEmpIdByUserId(int id)
        {
            var data = await pRLEntryService.GetEmpIdByUserId(id);
            return Json(data);
        }

        public async Task<IActionResult> ApplicationDetails(int id)
        {
            var model = new PRLApplicationViewModel()
            {
                //flang = _lang.PerseLang("RetirementTermination/PRLApplicationBN.json"),
                flang = _lang.PerseLang("RetirementTermination/PRLApplicationEN.json", "RetirementTermination/PRLApplicationBN.json", Request.Cookies["lang"]),
                pRLApplicationsById = await pRLEntryService.GetPrlEntryById(id)
            };
            return View(model);
        }

        public async Task<IActionResult> ApproveApplicationDetails(int id)
        {
            var model = new PRLApplicationViewModel()
            {
                //flang = _lang.PerseLang("RetirementTermination/PRLApplicationBN.json"),
                flang = _lang.PerseLang("RetirementTermination/PRLApplicationEN.json", "RetirementTermination/PRLApplicationBN.json", Request.Cookies["lang"]),
                pRLApplicationsById = await pRLEntryService.GetPrlEntryById(id)
            };
            return View(model);
        }

        public async Task<IActionResult> ApplyResign()
        {
            var model = new PRLApplicationViewModel()
            {
                flang = _lang.PerseLang("RetirementTermination/PRLApplicationEN.json", "RetirementTermination/PRLApplicationBN.json", Request.Cookies["lang"]),
                resignInformation = await resignInformationService.GetResignInformation(),
            };
            return View(model);
        }
        [HttpPost]
      
        public async Task<IActionResult> ResignClearancePdfGenerate([FromForm] PRLApplicationViewModel model) {
            try
            {
                
                int Id = Int32.Parse(model.resignId);
                var a = await resignInformationService.GetResignInformationById(Id);
              
                a.bodyText = model.bodyText;
                a.letterNo = model.letterNo;
             

                await resignInformationService.SaveResignInformation(a);
                return RedirectToAction(nameof(clearanceResignationByUser));
            }
            catch (Exception ex)
            {

                return Json(ex.Message);
            }
        }

        [HttpPost]
      
        public async Task<IActionResult> ResignClearance([FromForm] PRLApplicationViewModel model) {
            try
            {
                string fileName;
                if (model.clearanceFile != null)
                {
                    string message = FileSave.SaveEmpAttachmentNew(out fileName, model.clearanceFile);
                }
                else
                {
                    fileName = await resignInformationService.GetResignclearanceFileById(Int32.Parse(model.resignId));
                };
                int Id = Int32.Parse(model.resignId);
                var a = await resignInformationService.GetResignInformationById(Id);
                a.status = 3;
                a.clearanceDate = model.clearanceDate;
                a.clearanceFile = fileName;
             

                await resignInformationService.SaveResignInformation(a);
                return RedirectToAction(nameof(ApproveResignationByUser));
            }
            catch (Exception ex)
            {

                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> GetEmployeeByUserId(int userId)
        {
            
            return Json(await personalInfoService.GetEmployeeInfoByApplicationId1(userId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyResign([FromForm] PRLApplicationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.flang = _lang.PerseLang("RetirementTermination/PRLApplicationEN.json", "RetirementTermination/PRLApplicationBN.json", Request.Cookies["lang"]);
                return View(model);
            }
            string fileName;
            if (model.resignPhoto != null)
            {
                string message = FileSave.SaveEmpAttachmentNew(out fileName, model.resignPhoto);
            }
            else
            {
                fileName = await resignInformationService.GetResignImgUrlById(Int32.Parse(model.resignId));
            };

            try
            {
                int v = model.nextApprover;
               // var user = await personalInfoService.GetUserIdByEmpId(v);
                var user = await personalInfoService.GetUserIdByEmpId(v);

                ResignInformation data = new ResignInformation
                {
                    Id = Int32.Parse(model.resignId),
                    employeeId = Convert.ToInt32(model.employeeId),
                    resignDate = model.resignDate,
                    lastWorkingDate = model.lastWorkingDate,
                    resignReason = model.resignReason,
                    url = fileName,
                    nextApprover = user.userId,
                    status = 1,
                    type = ""
                };

                await resignInformationService.SaveResignInformation(data);
            }
            catch (Exception ex)
            {

                throw;
            }
           

            return RedirectToAction(nameof(ApplyResign));
        }



        public async Task<IActionResult> ApproveMyApplication()
        {
            var model = new PRLApplicationViewModel()
            {
                flang = _lang.PerseLang("RetirementTermination/PRLApplicationEN.json", "RetirementTermination/PRLApplicationBN.json", Request.Cookies["lang"]),
                pRLApplications = await pRLEntryService.GetPRLEntryByStatus("2")
            };
            return View(model);
        }
         public async Task<IActionResult> ReturnMyApplication()
        {
            var model = new PRLApplicationViewModel()
            {
                flang = _lang.PerseLang("RetirementTermination/PRLApplicationEN.json", "RetirementTermination/PRLApplicationBN.json", Request.Cookies["lang"]),
                pRLApplications = await pRLEntryService.GetPRLEntryByStatus("3")
            };
            return View(model);
        }
         public async Task<IActionResult> RejetMyApplication()
        {
            var model = new PRLApplicationViewModel()
            {
                flang = _lang.PerseLang("RetirementTermination/PRLApplicationEN.json", "RetirementTermination/PRLApplicationBN.json", Request.Cookies["lang"]),
                pRLApplications = await pRLEntryService.GetPRLEntryByStatus("4")
            };
            return View(model);
        }
         public async Task<IActionResult> MyResignationList()
        {
            var model = new PRLApplicationViewModel()
            {
                flang = _lang.PerseLang("RetirementTermination/PRLApplicationEN.json", "RetirementTermination/PRLApplicationBN.json", Request.Cookies["lang"]),
                pRLApplications = await pRLEntryService.GetPRLEntryByStatus("1")
            };
            return View(model);
        }

        public async Task<IActionResult> OngoingResignation()
        {
            var model = new PRLApplicationViewModel()
            {
                //flang = _lang.PerseLang("RetirementTermination/PRLApplicationBN.json"),
                flang = _lang.PerseLang("RetirementTermination/PRLApplicationEN.json", "RetirementTermination/PRLApplicationBN.json", Request.Cookies["lang"]),
                // pRLApplications = await pRLEntryService.GetPRLEntryByStatus("Pending")
            };
            return View(model);
        }
          public async Task<IActionResult> ApproveResignation()
        {
            var model = new PRLApplicationViewModel()
            {
                //flang = _lang.PerseLang("RetirementTermination/PRLApplicationBN.json"),
                flang = _lang.PerseLang("RetirementTermination/PRLApplicationEN.json", "RetirementTermination/PRLApplicationBN.json", Request.Cookies["lang"]),
                // pRLApplications = await pRLEntryService.GetPRLEntryByStatus("Pending")
            };
            return View(model);
        }

        public async Task<IActionResult> ReturnResignation()
        {
            var model = new PRLApplicationViewModel()
            {
                //flang = _lang.PerseLang("RetirementTermination/PRLApplicationBN.json"),
                flang = _lang.PerseLang("RetirementTermination/PRLApplicationEN.json", "RetirementTermination/PRLApplicationBN.json", Request.Cookies["lang"]),
                // pRLApplications = await pRLEntryService.GetPRLEntryByStatus("Pending")
            };
            return View(model);
        }
        public async Task<IActionResult> RejetResignation()
        {
            var model = new PRLApplicationViewModel()
            {
                //flang = _lang.PerseLang("RetirementTermination/PRLApplicationBN.json"),
                flang = _lang.PerseLang("RetirementTermination/PRLApplicationEN.json", "RetirementTermination/PRLApplicationBN.json", Request.Cookies["lang"]),
                // pRLApplications = await pRLEntryService.GetPRLEntryByStatus("Pending")
            };
            return View(model);
        }
        public async Task<IActionResult> ResignationList()
        {
            var model = new PRLApplicationViewModel()
            {
                //flang = _lang.PerseLang("RetirementTermination/PRLApplicationBN.json"),
                  flang = _lang.PerseLang("RetirementTermination/PRLApplicationEN.json", "RetirementTermination/PRLApplicationBN.json", Request.Cookies["lang"]),
                // pRLApplications = await pRLEntryService.GetPRLEntryByStatus("Pending")
            };
            return View(model);
        }
        public async Task<IActionResult> pendingResignations()
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);


            var model = new PRLApplicationViewModel()
            {
                //pRLApplications = await pRLEntryService.GetPRLEntryByStatus("1"),
                resignInformation = await pRLEntryService.GetResignationByUserIdAndStatus(user.userId, 1)


            };
            return View(model);
        }
		public async Task<IActionResult> resignationApprovedByUser()
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var model = new PRLApplicationViewModel()
            {
                //pRLApplications = await pRLEntryService.GetPRLEntryByStatus("1"),
                resignInformation = await pRLEntryService.GetResignationByUserIdAndStatus(user.userId, 3)
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveResignations(int Id, int status)
        {
            try
            {
                var a = await resignInformationService.GetResignInformationById(Id);
                a.status = status;

                await resignInformationService.SaveResignInformation(a);

                var employee = await personalInfoService.GetEmployeeInfoById(Convert.ToInt32(a.employeeId));
                employee.prlDate = a.resignDate;
                await personalInfoService.SaveEmployeeInfo(employee);
                return Json("updated");
            }
            catch (Exception ex)
            {

                return Json(ex.Message);
            }

        }

        public async Task<IActionResult> ApproveResignationByUser()
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);


            var model = new PRLApplicationViewModel()
            {

                resignInformation = await pRLEntryService.GetResignationByUserIdAndStatus(user.userId, 2)


            };
            return View(model);
        }

        public async Task<IActionResult> clearanceResignationByUser()
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);


            var model = new PRLApplicationViewModel()
            {
               
                resignInformation = await pRLEntryService.GetResignationByUserIdAndStatus(user.userId, 3),
                employeeInfo = await pRLEntryService.GetEmpIdByUserId(user.userId)

        };
            return View(model);
        }
        [AllowAnonymous]
        public async Task<IActionResult> ResigntionLetter(int id)
        {
            var model = new PRLApplicationViewModel()
            {

                resignInformations = await resignInformationService.GetResignInformationById(id)


            };
            return View(model);

        }
        [AllowAnonymous]
        public IActionResult ReleaseLetter(int id)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;

            string url = scheme + "://" + host + "/HRPMSRetirementAndTermination/ApplyForPRL/ResigntionLetter/" + id;
            string status = myPDF.GeneratePDF1(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        public async Task<IActionResult> ShowAllResignation()
        {
            var model = new ShowAllResignationViewModel()
            {
                resignInformationList = await pRLEntryService.GetShowAllResignation()
            };
            return View(model);
        }

    }
}