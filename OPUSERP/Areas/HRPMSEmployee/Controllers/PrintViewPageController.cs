using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OPUSERP.Data.Entity;
using OPUSERP.Areas.HRPMSRetirementAndTermination.Models;
using OPUSERP.HRPMS.Services.RetirementAndTermination.Interface;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
    [Authorize]
    [Area("HRPMSEmployee")]
    public class PrintViewPageController : Controller
    {
        private readonly IPromotionService promotionService;
        private readonly LangGenerate<EmployeeInfoLn> _langEmp;
        private readonly LangGenerate<Promotion> _lang;
        private readonly IDesignationDepartmentService designationDepartmentService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly ISalaryGradeService salaryGradeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IResignInformationService resignInformationService;


        public PrintViewPageController(IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IPersonalInfoService personalInfoService, IDesignationDepartmentService designationDepartmentService,ISalaryGradeService salaryGradeService,IPromotionService promotionService, IResignInformationService resignInformationService)
        {
            _langEmp = new LangGenerate<EmployeeInfoLn>(hostingEnvironment.ContentRootPath);
            _lang = new LangGenerate<Promotion>(hostingEnvironment.ContentRootPath);
            this.personalInfoService = personalInfoService;
            this.designationDepartmentService = designationDepartmentService;
            this.salaryGradeService = salaryGradeService;
            this.promotionService = promotionService;
            this.resignInformationService = resignInformationService;
        }

      
        public async Task<IActionResult> DeclarationFidelity()
        {
            
            return View();
        }
        public async Task<IActionResult> DeclarationFidelity2()
        {
            
            return View();
        }
        public async Task<IActionResult> DeclarationFidelity3()
        {
            
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> SuretyBond()
        {
            
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> SuretyBond2()
        {
            
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> SuretyBond3()
        {
            
            return View();
        }


       public async Task<IActionResult> DeclarationSecrecy()
        {
            
            return View();
        }

         public async Task<IActionResult> DeclarationSecrecy2()
        {
            
            return View();
        }

         public async Task<IActionResult> DeclarationSecrecy3()
        {
            
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> AppriciationLetter(int id)
        {
            var model = new AppreciationLetterViewModel()
            {

                appreciationLetter = await personalInfoService.GetAppreciationInformationById(id)


            };
            return View(model);

        }
      
       public async Task<IActionResult> PRLForm()
        {
            
            return View();
        }
 
        //public async Task<IActionResult> ResigntionLetter()
        //{
            
        //    return View();
        //}
 
        public async Task<IActionResult> ChargeReport()
        {
            
            return View();
        }
 
         public async Task<IActionResult> HRManagementDepartment()
        {
            
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> JoiningAcceptanceLetter()
        {
            
            return View();
        }
        public async Task<IActionResult> RealeseLetter()
        {

            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> ResigntionLetter(int id)
        {
            var model = new ResignationLetterViewModel()
            {

                resignInformations =  await resignInformationService.GetResignInformationById(id)


            };
            return View(model);

        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Index([FromForm] PromotionViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        model.fLang = _lang.PerseLang("Employee/PromotionEmpListEN.json", "Employee/PromotionEmpListBN.json", Request.Cookies["lang"]);
        //        model.designations = await designationDepartmentService.GetDesignations();
        //        model.employeeInfos = await personalInfoService.GetEmployeeInfo();
        //        model.salaryGrades = await salaryGradeService.GetAllSalaryGrade();
        //        return View(model);
        //    }

        //    HRPMS.Data.Entity.Employee.Promotion data = new HRPMS.Data.Entity.Employee.Promotion
        //    {
        //        Id = Convert.ToInt32(model.ID),
        //        employeeId = Convert.ToInt32(model.employeeId),
        //        designationId = model.designationId,
        //        salaryGradeId = model.salaryGradeId,
        //        promotionDate = model.promotionDate,
        //        promotionType = model.promotionType,
        //        Basic = model.Basic,
        //        Remarks = model.Remarks
        //    };

        //    int lstId = await promotionService.SavePromotion(data);

        //    return RedirectToAction(nameof(Index), new { @id = model.employeeId });
        //}

    }
}