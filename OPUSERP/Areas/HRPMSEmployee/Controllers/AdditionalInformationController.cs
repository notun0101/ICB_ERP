using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.Data.Entity;
using Microsoft.AspNetCore.Identity;
using OPUSERP.Accounting.Services.MasterData.Interfaces;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
    [Authorize]
    [Area("HRPMSEmployee")]
    public class AdditionalInformationController : Controller
    {
        private readonly IPhotographService photographService;

        private readonly IPersonalInfoService personalInfoService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICostCentreService costCentreService;
      
        private readonly RoleManager<ApplicationRole> _roleManager;


        public AdditionalInformationController(IHostingEnvironment hostingEnvironment, IPersonalInfoService personalInfoService, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ICostCentreService costCentreService, IPhotographService photographService)
        {
            this.personalInfoService = personalInfoService;
            this.costCentreService = costCentreService;
            _userManager = userManager;
            _roleManager = roleManager;
            this.photographService = photographService;

        }

        // GET: Spouse
        public async Task<IActionResult> Index(int id)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.employeeID = id.ToString();
            var model = new AdditionalInformationViewModel
            {
                employeeID = id.ToString(),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id),
                visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),


            };
          //  if (model.employeeInfo == null) model.employeeInfo = new EmployeeInfo();
            return View(model);
        }

        // POST: Spouse/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] AdditionalInformationViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user1 = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user1);
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            if (!ModelState.IsValid)
            {
                ViewBag.employeeID = model.employeeID;
                //model.employeeInfo = await personalInfoService.GetEmployeeInfoById(id)
                //model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id),
                //model.visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
             
                return View(model);
            }
            var data = await personalInfoService.GetEmployeeInfoById(Convert.ToInt32(model.employeeID));
            data.favouriteColor = model.favouriteColor;
            //EmployeeInfo data = new EmployeeInfo
            //{
                
            //    favouriteColor = model.favouriteColor,
            //};
            if (roles.Contains("HRAdmin") || roles.Contains("admin"))
            {
                data.isDelete = 0;
            }
            else
            {
                data.isDelete = 1;
                //await employeeInfoService.ChangeEmployeeActivityStatus(model.employeeID, 3);
            }
            await personalInfoService.SaveEmployeeInfo(data);
            return RedirectToAction("Index", "AdditionalInformation", new
            {
                id = model.employeeID
            });

        }

        // Delete: Language
        public async Task<IActionResult> Delete(int id, int empId)
        {
            await personalInfoService.DeleteEmployeeInfoById(id);
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "AdditionalInformation", new
            {
                id = empId
            });
        }

        // GET: ResignationLetter
        public async Task<IActionResult> ResignationLetter(int id)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.employeeID = id.ToString();
            var model = new ResignationLetterViewModel
            {
                employeeID = id.ToString(),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                resignationLetters = await costCentreService.GetResignationLetterByEmpId(id),
               
            };
             return View(model);
            
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResignationLetter([FromForm] ResignationLetterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.employeeID = model.employeeID;
                model.resignationLetters = await costCentreService.GetResignationLetterByEmpId(model.ResignationLetterID);
                return View(model);
            }
            string fileName;

            if (model.imageUrl != null)
            {
                string message = FileSave.SaveImageNew(out fileName, model.imageUrl);
            }
            else
            {
                fileName = await costCentreService.GetResignationLetterImgUrlById(model.ResignationLetterID);
            };

            ResignationLetter data = new ResignationLetter
            {
                Id = model.ResignationLetterID,
                employeeId = Int32.Parse(model.employeeID),
                submissionDate = model.submissionDate,
                lastworkingDate = model.lastworkingDate,
                reason = model.reason,
                totalWorkingDays = model.totalWorkingDays,
                fileUrl = fileName
            };

            await costCentreService.SaveResignationLetter(data);
            await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));
            return RedirectToAction(nameof(ResignationLetter));
        }


        public async Task<IActionResult> DeleteResignationLetter(int id, int empId)
        {
            await costCentreService.DeleteResignationLetterById(id);
            return RedirectToAction("Index", "AdditionalInformation", new
            {
                id = empId
            });
        }

        

    }
}