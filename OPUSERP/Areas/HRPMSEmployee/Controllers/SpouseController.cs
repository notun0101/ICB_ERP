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

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
    [Authorize]
    [Area("HRPMSEmployee")]
    public class SpouseController : Controller
    {
        private readonly LangGenerate<SpouseLn> _lang;
        private readonly ISpouseChildrenService spouseChildrenService;
        private readonly IPhotographService photographService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly IAddressService addressService;
        private readonly INomineeService nomineeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public SpouseController(IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, INomineeService nomineeService,IPhotographService photographService, IPersonalInfoService personalInfoService, ISpouseChildrenService spouseChildrenService, IAddressService addressService)
        {
            _lang = new LangGenerate<SpouseLn>(hostingEnvironment.ContentRootPath);
            this.spouseChildrenService = spouseChildrenService;
            this.personalInfoService = personalInfoService;
            this.addressService = addressService;
            this.photographService = photographService;
            this.nomineeService = nomineeService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: Spouse
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            var model = new SpouseViewModel
            {
                employeeID = id.ToString(),
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                fLang = _lang.PerseLang("Employee/SpouseEN.json", "Employee/SpouseBN.json", Request.Cookies["lang"]),
                spouses = await spouseChildrenService.GetSpouseByEmpId(id),
                spouse = await spouseChildrenService.GetSpouseByEmpId1(id),
                employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id),
                districts = await addressService.GetAllDistrict(),
                occupations = await nomineeService.GetOccupation()
            };
            if (model.spouse == null) model.spouse = new Spouse();
            return View(model);
        }

        // POST: Spouse/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] SpouseViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            if (!ModelState.IsValid)
            {
                ViewBag.employeeID = model.employeeID;
                model.fLang = _lang.PerseLang("Employee/SpouseEN.json", "Employee/SpouseBN.json", Request.Cookies["lang"]);
                model.spouses = await spouseChildrenService.GetSpouseByEmpId(Int32.Parse(model.employeeID));
                model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(Int32.Parse(model.employeeID));
                model.districts = await addressService.GetAllDistrict();
                if (model.spouse == null) model.spouse = new Spouse();
                return View(model);
            }
            string fileName;
            if (model.spousePhoto != null)
            {
                string message = FileSave.SaveImageNew(out fileName, model.spousePhoto);
            }
            else
            {
                fileName = await spouseChildrenService.GetSpouseImgUrlById(Int32.Parse(model.spouseID));
            };

            string marriagefile;
            if (model.MarriagePhoto != null)
            {
                string message = FileSave.SaveImageNew(out marriagefile, model.MarriagePhoto);

            }
            else
            {
                marriagefile = await spouseChildrenService.GetSpouseMarriageImgUrlById(Int32.Parse(model.spouseID));


            };
            Spouse data = new Spouse
            {
                Id = Int32.Parse(model.spouseID),
                employeeId = Int32.Parse(model.employeeID),
                spouseName = model.spouseName,
                nationality = model.nationality,
                spouseNameBN = model.spouseNameBN,
                bloodGroup = model.bloodGroup,
                contact = model.contact,
                relationship = model.relationship,
                gender = model.gender,
                email = model.emailAddress,
                dateOfBirth = model.dateOfBirth,
                occupationId = model.occupationId,
                nid = model.nid,
                organization = model.organization,
                maritalStatus = model.maritalstatus,
                designation = model.designation,
                imageUrl = fileName,
                marriageCertificate= marriagefile,
                bin = model.bin,
                highestEducation = model.higherEducation,
                homeDistrict = model.homeDistrict,
               // dateOfBirth = model.dateOfBirth,
                dateOfMarriage = model.dateOfMarriage,
            };
            if (roles.Contains("HRAdmin") || roles.Contains("admin"))
            {
                await personalInfoService.UpdateEmployeeinfoById1(Int32.Parse(model.employeeID));
                data.isDelete = 0;
            }
            else
            {
                await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));
                data.isDelete = 1;
            }
            await spouseChildrenService.SaveSpouse(data);
            

            //await photographService.SavePhotograph(data);
           
            await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));
            // return RedirectToAction("Index" , new { id=data.Id});
            // return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "Spouse", new
            {
                id = model.employeeID
            });

        }

        // Delete: Language
        public async Task<IActionResult> Delete(int id, int empId)
        {
            await spouseChildrenService.DeleteSpouseById(id);
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "Spouse", new
            {
                id = empId
            });
        }

    }
}