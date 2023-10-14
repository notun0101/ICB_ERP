using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.Areas.HRPMSMasterData.Models;
using OPUSERP.Areas.HRPMSMasterData.Models.Lang;

using OPUSERP.Data.Entity;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.IELTS;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.ACR.Interfaces;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.HRPMS.Services.Organogram.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.HRPMS.Services.DisciplineInvestigation.Interfaces;
using OPUSERP.SCM.Services.MasterData.Interfaces;
using OPUSERP.CRM.Services.MasterData.Interfaces;
using OPUSERP.Payroll.Services.Salary.Interfaces;
using OPUSERP.Accounting.Services.MasterData.Interfaces;
using OPUSERP.HRPMS.Data.Entity.HrComputerLiteracy;
using Microsoft.AspNetCore.Http;
using System.IO;
using OPUSERP.HRPMS.Data.Entity.Attendance;
using OPUSERP.HRPMS.Services.SuspensionReport.Interfaces;
using OPUSERP.HRPMS.Data.Entity.Suspensions;
using DinkToPdf.Contracts;
using OPUSERP.HRPMS.Services.Recruitment.Interfaces;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.Areas.HRPMSReport.Models;
using OPUSERP.ERPServices.MasterData.Interfaces;
using IAddressService = OPUSERP.HRPMS.Services.MasterData.Interfaces.IAddressService;
using IDesignationDepartmentService = OPUSERP.HRPMS.Services.MasterData.Interfaces.IDesignationDepartmentService;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
	[Area("HRPMSEmployee")]
	[Authorize]
	public class InfoController : Controller
	{
		private readonly LangGenerate<EmployeeInfoLn> _lang;
		private readonly LangGenerate<GridViewLn> _lang1;
		private readonly LangGenerate<ShiftGroupDetailsLn> _lang2;
		private readonly IEmergencyContactService emergencyContactService;


		private readonly IERPCompanyService eRPCompanyService;
		private readonly IPersonalInfoService personalInfoService;
		private readonly IReligionMunicipalityService religionMunicipalityService;
		private readonly ITypeService typeService;
		private readonly IOrganizationPostService organizationPostService;
		private readonly IEmployeeOrganogramService employeeOrganogramService;
		private readonly IDesignationDepartmentService designationDepartmentService;
		private readonly IAddressService addressService;
		private readonly ISpecialBranchUnitService specialBranchUnitService;
		private readonly IFunctionsInfoService functionsInfoService;
		private readonly IPhotographService photographService;
		private readonly ILocationService locationService;
		private readonly IStatusService statusService;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<ApplicationRole> _roleManager;
		private readonly IShiftGroupDetailService shiftGroupDetailService;
		private readonly IShiftGroupMasterService shiftGroupMasterService;
		private readonly IEmployeeInfoService employeeInfoService;
		private readonly ISpouseChildrenService spouseChildrenService;
		private readonly INomineeService nomineeService;
		private readonly ILevelofEducationService levelofEducationService;
		private readonly IDegreeService degreeService;
		private readonly IRelDegreeSubjectService degreeSubjectService;
		private readonly ISubjectService subjectService;
		private readonly INomineeFundService nomineeFundService;
		private readonly INomineeDetailService nomineeDetailService;
		private readonly IResultService resultService;
		private readonly IQualificationHeadService qualificationHeadService;
		private readonly IProfessionalQualificationsService professionalQualificationsService;
		private readonly IOtherQualificationService otherQualificationService;
		private readonly IOtherQualificationHeadService otherQualificationHeadService;
		private readonly ITrainingService trainingService;
		private readonly ITraningHistoryService traningHistoryService;
		private readonly ISalaryGradeService salaryGradeService;
		private readonly IServiceHistoryService serviceHistoryService;
		private readonly IPromotionLogService promotionLogService;
		private readonly IAcrInfoService acrInfoService;
		private readonly IDisciplineInvestigation disciplineInvestigation;
		private readonly IDisciplinaryService disciplinaryService;
		private readonly IApprovalService approvalService;
		private readonly IDrivingLicenseService drivingLicenseService;
		private readonly IPassportInfoService passportInfoService;
		private readonly IProjectService projectService;
		private readonly ITravelService travelService;
		private readonly ITravelInfoService travelInfoService;
		private readonly IMembershipService membershipService;
		private readonly IMembershipLanguageService membershipLanguageService;
		private readonly IAwardPublicationLanguageService awardPublicationService;
		private readonly IOtherActivityService otherActivityService;
		private readonly IHRPMSActivityTypeService iHRPMSActivityTypeService;
		private readonly IBankInfoService bankInfoService;
		private readonly IBankBranchService bankBranchService;
		private readonly ISalaryService salaryService;
		private readonly IBelongingsService belongingsService;
		private readonly IBelongingsItemService belongingsItemService;
		private readonly IPriviousEmploymentService priviousEmploymentService;
		private readonly IHRPMSOrganizationTypeService iHRPMSOrganizationTypeService;
		private readonly IFreedomFighterService freedomFighterService;
		private readonly IReferenceService referenceService;
		private readonly IOfficeAssignService officeAssignService;
		private readonly IEmployeeContratInfoService employeeContratInfoService;
		private readonly IEmployeeProjectActivityService employeeProjectActivityService;
		private readonly IHRDonerSevice hRDonerSevice;
		private readonly IHRProjectService hRProjectService;
		private readonly IHRActivityService hRActivityService;
		private readonly IFinanceCodeService financeCodeService;
		private readonly IHRPMSAttachmentService hRPMSAttachmentService;
		private readonly IAttachmentCategoryService attachmentCategoryService;
		private readonly ICostCentreService costCentreService;
		private readonly IEmployeePunchCardInfoService employeePunchCardInfoService;
		private readonly ICustomRoleService customRoleService;
		private readonly IBookAwardService bookAwardService;
		private readonly ITaxService taxService;
		private readonly IEmployeeSportsService EmployeeSportsService;
		private readonly IExtraCurricularTypeService extraCurricularTypeService;
		private readonly ISuspension suspensionReportService;
		private readonly IMobileBenifitService mobileBenifitService;
		private readonly IEmployeeDiseaseService employeeDiseaseService;
		private readonly IEmployeeExtraCurricularService employeeExtraCurricularService;
		private readonly IApplicationFormService applicationFormService;
		private readonly IUserInfoes userInfoes;
		public object dateOfBirth { get; private set; }
		private readonly string rootPath;
		private readonly MyPDF myPDF;


		public InfoController(IHostingEnvironment hostingEnvironment,
			IConverter converter,
			IResultService resultService,
			IERPCompanyService eRPCompanyService,
			IQualificationHeadService qualificationHeadService,
			IProfessionalQualificationsService professionalQualificationsService,
			IEmergencyContactService emergencyContactService,
			ILevelofEducationService levelofEducationService,
			IDegreeService degreeService,
			IUserInfoes userInfoes,
			ISubjectService subjectService,
			INomineeService nomineeService,
			IPhotographService photographService,
			IFunctionsInfoService functionsInfoService,
			ILocationService locationService,
			RoleManager<ApplicationRole> roleManager,
			IStatusService statusService,
			IPersonalInfoService personalInfoService,
			IReligionMunicipalityService religionMunicipalityService,
			ITypeService typeService,
			UserManager<ApplicationUser> userManager,
			IOrganizationPostService organizationPostService,
			IEmployeeOrganogramService employeeOrganogramService,
			IDesignationDepartmentService designationDepartmentService,
			IAddressService addressService,
			ISpecialBranchUnitService specialBranchUnitService,
			IShiftGroupDetailService shiftGroupDetailService,
			IShiftGroupMasterService shiftGroupMasterService,
			IEmployeeInfoService employeeInfoService,
			ISpouseChildrenService spouseChildrenService,
			INomineeFundService nomineeFundService,
			INomineeDetailService nomineeDetailService,
			IOtherQualificationHeadService otherQualificationHeadService,
			IOtherQualificationService otherQualificationService,
			ITrainingService trainingService,
			ITraningHistoryService traningHistoryService,
			ISalaryGradeService salaryGradeService,
			IServiceHistoryService serviceHistoryService,
			IPromotionLogService promotionLogService,
			IAcrInfoService acrInfoService,
			IDisciplineInvestigation disciplineInvestigation,
			IDisciplinaryService disciplinaryService,
			IApprovalService approvalService,
			IDrivingLicenseService drivingLicenseService,
			IPassportInfoService passportInfoService,
			IProjectService projectService,
			ITravelService travelService,
			ITravelInfoService travelInfoService,
			IMembershipService membershipService,
			IMembershipLanguageService membershipLanguageService,
			IAwardPublicationLanguageService awardPublicationService,
			IOtherActivityService otherActivityService,
			IHRPMSActivityTypeService iHRPMSActivityTypeService,
			IBankInfoService bankInfoService,
			IBankBranchService bankBranchService,
			ISalaryService salaryService,
			IBelongingsService belongingsService,
			IBelongingsItemService belongingsItemService,
			IPriviousEmploymentService priviousEmploymentService,
			IHRPMSOrganizationTypeService iHRPMSOrganizationTypeService,
			IFreedomFighterService freedomFighterService,
			IReferenceService referenceService,
			IOfficeAssignService officeAssignService,
			IEmployeeContratInfoService employeeContratInfoService,
			IEmployeeProjectActivityService employeeProjectActivityService,
			IHRDonerSevice hRDonerSevice,
			IHRProjectService hRProjectService,
			IHRActivityService hRActivityService,
			IFinanceCodeService financeCodeService,
			IHRPMSAttachmentService hRPMSAttachmentService,
			IAttachmentCategoryService attachmentCategoryService,
			ICostCentreService costCentreService,
			IEmployeePunchCardInfoService employeePunchCardInfoService,
			ICustomRoleService customRoleService,
			IBookAwardService bookAwardService,
			ITaxService taxService,
			IEmployeeSportsService EmployeeSportsService,
			IExtraCurricularTypeService extraCurricularTypeService,
			ISuspension suspensionReportService,
			IMobileBenifitService mobileBenifitService,
			IEmployeeDiseaseService employeeDiseaseService,
			IEmployeeExtraCurricularService employeeExtraCurricularService
			, IApplicationFormService applicationFormService
			// ILocationService locationService


			)
		{
			_lang = new LangGenerate<EmployeeInfoLn>(hostingEnvironment.ContentRootPath);
			_lang1 = new LangGenerate<GridViewLn>(hostingEnvironment.ContentRootPath);
			_lang2 = new LangGenerate<ShiftGroupDetailsLn>(hostingEnvironment.ContentRootPath);

			this.eRPCompanyService = eRPCompanyService;
			this.personalInfoService = personalInfoService;
			this.emergencyContactService = emergencyContactService;
			this.religionMunicipalityService = religionMunicipalityService;
			this.organizationPostService = organizationPostService;
			this.employeeOrganogramService = employeeOrganogramService;
			this.designationDepartmentService = designationDepartmentService;
			this.typeService = typeService;
			this.addressService = addressService;
			this.photographService = photographService;
			this.specialBranchUnitService = specialBranchUnitService;
			this.locationService = locationService;
			this.functionsInfoService = functionsInfoService;
			this.statusService = statusService;
			_roleManager = roleManager;
			_userManager = userManager;
			this.shiftGroupDetailService = shiftGroupDetailService;
			this.shiftGroupMasterService = shiftGroupMasterService;
			this.employeeInfoService = employeeInfoService;
			this.spouseChildrenService = spouseChildrenService;
			this.nomineeService = nomineeService;
			this.levelofEducationService = levelofEducationService;
			this.degreeService = degreeService;
			this.subjectService = subjectService;
			this.nomineeFundService = nomineeFundService;
			this.nomineeDetailService = nomineeDetailService;
			this.resultService = resultService;
			this.qualificationHeadService = qualificationHeadService;
			this.professionalQualificationsService = professionalQualificationsService;
			this.otherQualificationService = otherQualificationService;
			this.otherQualificationHeadService = otherQualificationHeadService;
			this.trainingService = trainingService;
			this.traningHistoryService = traningHistoryService;
			this.salaryGradeService = salaryGradeService;
			this.serviceHistoryService = serviceHistoryService;
			this.promotionLogService = promotionLogService;
			this.acrInfoService = acrInfoService;
			this.disciplineInvestigation = disciplineInvestigation;
			this.disciplinaryService = disciplinaryService;
			this.approvalService = approvalService;
			this.drivingLicenseService = drivingLicenseService;
			this.passportInfoService = passportInfoService;
			this.travelInfoService = travelInfoService;
			this.travelService = travelService;
			this.projectService = projectService;
			this.membershipService = membershipService;
			this.membershipLanguageService = membershipLanguageService;
			this.awardPublicationService = awardPublicationService;
			this.otherActivityService = otherActivityService;
			this.iHRPMSActivityTypeService = iHRPMSActivityTypeService;
			this.bankInfoService = bankInfoService;
			this.bankBranchService = bankBranchService;
			this.salaryService = salaryService;
			this.belongingsService = belongingsService;
			this.belongingsItemService = belongingsItemService;
			this.priviousEmploymentService = priviousEmploymentService;
			this.iHRPMSOrganizationTypeService = iHRPMSOrganizationTypeService;
			this.freedomFighterService = freedomFighterService;
			this.referenceService = referenceService;
			this.officeAssignService = officeAssignService;
			this.employeeContratInfoService = employeeContratInfoService;
			this.employeeProjectActivityService = employeeProjectActivityService;
			this.hRDonerSevice = hRDonerSevice;
			this.hRProjectService = hRProjectService;
			this.hRActivityService = hRActivityService;
			this.financeCodeService = financeCodeService;
			this.hRPMSAttachmentService = hRPMSAttachmentService;
			this.attachmentCategoryService = attachmentCategoryService;
			this.costCentreService = costCentreService;
			this.employeePunchCardInfoService = employeePunchCardInfoService;
			this.customRoleService = customRoleService;
			this.bookAwardService = bookAwardService;
			this.taxService = taxService;
			this.EmployeeSportsService = EmployeeSportsService;
			this.extraCurricularTypeService = extraCurricularTypeService;
			this.suspensionReportService = suspensionReportService;
			this.mobileBenifitService = mobileBenifitService;
			this.employeeDiseaseService = employeeDiseaseService;
			this.userInfoes = userInfoes;
			this.employeeExtraCurricularService = employeeExtraCurricularService;
			this.applicationFormService = applicationFormService;
			myPDF = new MyPDF(hostingEnvironment, converter);
			rootPath = hostingEnvironment.ContentRootPath;

		}

		// GET: Info/AllEmployeeList
		public async Task<IActionResult> AllEmployeeList()
		{
			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
			string userName = HttpContext.User.Identity.Name;

			var model = new EmployeeInfoViewModel
			{
				//allEmployeeJsons = await personalInfoService.GetAllActiveEmployeeInfo(),
				allEmployeeJsonNew = await personalInfoService.GetAllActiveDraftEmployeeInfo(),
				employeeInfoByUsername = await personalInfoService.EmployeeInfoByUsernameSp(userName),
				//allEmployeeJson = await personalInfoService.GetEmployeeInfoJson(userName),
				fLang = _lang.PerseLang("Employee/EmployeeInfoEN.json", "Employee/EmployeeInfoBN.json", Request.Cookies["lang"]),
				allEmployeeInfos = await personalInfoService.GetEmployeeInfoByOrg("ddm"),
				employeeTypes = await typeService.GetAllEmployeeType(),
				visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData()
			};
			return View(model);
		}
		public async Task<IActionResult> AllEmployeeListApi()
		{
			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
			string userName = HttpContext.User.Identity.Name;

			var model = new HrReportApiViewModel
			{
				allEmployee = await personalInfoService.GetAllActiveDraftEmployeeInfo(),
			};
			return Json(model);
		}

		// GET: Info/AllEmployeeList
		public async Task<IActionResult> EmployeeProfileView()
		{

			//ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);

			var model = new EmployeeInfoViewModel
			{
				allEmployeeJson = await personalInfoService.GetEmployeeInfoJson(userName),
				fLang = _lang.PerseLang("Employee/EmployeeInfoEN.json", "Employee/EmployeeInfoBN.json", Request.Cookies["lang"]),
				allEmployeeInfos = await personalInfoService.GetEmployeeInfoByOrg("ddm"),
				employeeTypes = await typeService.GetAllEmployeeType(),
				visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData()
			};
			return View(model);
		}
        public async Task<IActionResult> AuditTrail()
        {
            var data = new AuditViewModel
            {
				auditTables = await personalInfoService.GetAuditTables(),
				auditTrails = await personalInfoService.GetAuditTrailsForLastSevenDays()
            };
            return View(data);
        }


        public async Task<IActionResult> AuditTrailApi(string tableName, string operationType, DateTime? startDate, DateTime? endDate)
        {
            var data = new AuditViewModel
            {
                auditTrails = await personalInfoService.GetAuditTrailsByParams(tableName, operationType, startDate, endDate)
            };
            return Json(data);
        }
        public async Task<IActionResult> AllInactiveEmployeeList()
		{
			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

			var model = new EmployeeInfoViewModel
			{
				allEmployeeJsons = await personalInfoService.GetAllInactiveEmployeeInfoJson(),
				fLang = _lang.PerseLang("Employee/EmployeeInfoEN.json", "Employee/EmployeeInfoBN.json", Request.Cookies["lang"]),
				allEmployeeInfos = await personalInfoService.GetEmployeeInfoByOrg("ddm"),
				employeeTypes = await typeService.GetAllEmployeeType()
			};
			return View(model);
		}


		public async Task<IActionResult> ApproveInfo(int Id)
		{
			//ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
			await personalInfoService.ApproveEmployeeinfoById(Id);
			//var model = new EmployeeInfoViewModel
			//{
			//    fLang = _lang.PerseLang("Employee/EmployeeInfoEN.json", "Employee/EmployeeInfoBN.json", Request.Cookies["lang"]),
			//    allEmployeeInfos = await personalInfoService.GetEmployeeInfoByOrg("ddm"),
			//    employeeTypes = await typeService.GetAllEmployeeType()
			//};
			return RedirectToAction(nameof(AllEmployeeListForApprove));
		}
		public async Task<IActionResult> AllEmployeeListForApprove()
		{
			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

			var model = new EmployeeInfoViewModel
			{
				fLang = _lang.PerseLang("Employee/EmployeeInfoEN.json", "Employee/EmployeeInfoBN.json", Request.Cookies["lang"]),
				allEmployeeInfos = await personalInfoService.GetEmployeeInfoByOrg("ddm"),
				employeeTypes = await typeService.GetAllEmployeeType()
			};
			return View(model);
		}

		// GET: Info
		public async Task<IActionResult> Create()
		{
			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
			var model = new EmployeeInfoViewModel
			{
				fLang = _lang.PerseLang("Employee/EmployeeInfoEN.json", "Employee/EmployeeInfoBN.json", Request.Cookies["lang"]),
				religions = await religionMunicipalityService.GetReligions(),
				empExpertises = await religionMunicipalityService.GetExpertise(),
				employeeTypes = await typeService.GetAllEmployeeType(),
				//   organoOrganizations = await organizationPostService.GetAllOrganizationByIds(this.GetAllIdsByOrg(user.org)),
				organoOrganizations = await organizationPostService.GetAllOrganization(),
				designations = await designationDepartmentService.GetDesignations(),
				specialBranchUnits = await specialBranchUnitService.GetSpecialBranchUnit(),
				districts = await addressService.GetAllDistrict(),
				departments = await designationDepartmentService.GetDepartment(),
				serviceStatuses = await statusService.GetServiceStatus(),
				salaryGrades = await statusService.GetJoiningGrades(),
				salarygrades = await statusService.GetCurrentGrades(),
				salarySlabs = await statusService.GetJoiningBasic(),
				salaryslabs = await statusService.GetCurrentBasic(),
				hrPrograms = await statusService.GetHrProgram(),
				hrUnits = await statusService.GetHrUnit(),
				locations = await locationService.GetLocation(),
				functionInfos = await functionsInfoService.GetFunctionInfo(),
				visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
				divisions = await addressService.GetAllDivision(),
				hrBranches = await statusService.GetHrBranch(),
				hrDivisions = await statusService.GetHrDivision(),
				parentsOccupation = await nomineeService.GetOccupation(),
				//fatherOccupation = await nomineeService.GetFatherOccupation(),
				//motherOccupation = await nomineeService.GetMotherOccupation(),
				countries = await addressService.GetAllContry(),
				CustomRoles = await customRoleService.GetCustomRole(),
				disabilityTypes = await statusService.GeAlltDisabilityType()


			};
			return View(model);
		}

		// POST: Info/Create
		[HttpPost]
		//[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([FromForm] EmployeeInfoViewModel model)
		{
			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

			bool flag = false;

			try
			{

				int temp = await personalInfoService.IsThisEmpIDPresent(model.employeeCode);
				if (temp != 0 && temp != Int32.Parse((model.employeeID)))
				{
					ModelState.AddModelError(string.Empty, "This Employee Code Already Taken. Please Try Another Employee Code");
					flag = true;
				}
			}
			catch (Exception ex)
			{

				throw;
			}
			//var periodCheck = await personalInfoService.GetDuplicateEmpCode(Convert.ToInt32(model.employeeID), model.employeeCode);


			if (!ModelState.IsValid || flag)
			//if (!ModelState.IsValid || periodCheck.Count() > 0)
			{
				ViewBag.employeeID = model.employeeID;
				model.fLang = _lang.PerseLang("Employee/EmployeeInfoEN.json", "Employee/EmployeeInfoBN.json", Request.Cookies["lang"]);
				model.religions = await religionMunicipalityService.GetReligions();
				model.empExpertises = await religionMunicipalityService.GetExpertise();
				model.employeeTypes = await typeService.GetAllEmployeeType();
				model.organoOrganizations = await organizationPostService.GetAllOrganization();
				model.designations = await designationDepartmentService.GetDesignations();
				model.specialBranchUnits = await specialBranchUnitService.GetSpecialBranchUnit();
				model.districts = await addressService.GetAllDistrict();
				model.departments = await designationDepartmentService.GetDepartment();
				model.serviceStatuses = await statusService.GetServiceStatus();
				model.salaryGrades = await statusService.GetJoiningGrades();
				model.salarygrades = await statusService.GetCurrentGrades();
				model.hrPrograms = await statusService.GetHrProgram();
				model.hrUnits = await statusService.GetHrUnit();
				model.locations = await locationService.GetLocation();
				model.functionInfos = await functionsInfoService.GetFunctionInfo();
				model.visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData();
				model.divisions = await addressService.GetAllDivision();
				model.hrBranches = await statusService.GetHrBranch();
				model.hrDivisions = await statusService.GetHrDivision();
				model.parentsOccupation = await nomineeService.GetOccupation();
				model.countries = await addressService.GetAllContry();
				model.CustomRoles = await customRoleService.GetCustomRole();
				model.disabilityTypes = await statusService.GeAlltDisabilityType();

				//if (periodCheck.Count() > 0)
				//{
				//    ModelState.AddModelError(string.Empty, "This Employee Code Already Taken. Please try another Code !!!");
				//}

				return View(model);
			}


			DateTime dateBirth = Convert.ToDateTime(model.dateOfBirth);
			DateTime? dateLPR = dateBirth.AddYears(59);
			DateTime? LPRDate;
			DateTime? PRLStartDate;
			DateTime? PRLEndDate;
			if (model.freedomFighter == "Yes")
			{
				dateLPR = Convert.ToDateTime(dateLPR).AddYears(1);
			}
			if (model.employeeType != 1)
			{
				LPRDate = null;
				PRLStartDate = null;
				PRLEndDate = null;
			}
			else
			{
				LPRDate = Convert.ToDateTime(dateLPR).AddDays(-1);
				PRLStartDate = Convert.ToDateTime(dateLPR);
				PRLEndDate = Convert.ToDateTime(dateLPR).AddYears(1);
			}


			string motherNameEnglish;
			string fatherNameEnglish;
			if (string.IsNullOrEmpty(model.motherNameEnglish))
			{
				motherNameEnglish = "";
			}
			else
			{
				motherNameEnglish = model.motherNameEnglish.ToUpper();
			}
			if (string.IsNullOrEmpty(model.fatherNameEnglish))
			{
				fatherNameEnglish = "";
			}
			else
			{
				fatherNameEnglish = model.fatherNameEnglish.ToUpper();
			}

			string ApplicationUserId = await personalInfoService.GetAuthCodeByUserId(Int32.Parse(model.employeeID));




			EmployeeInfo data = new EmployeeInfo
			{
				Id = Int32.Parse(model.employeeID),
				employeeCode = model.employeeCode,

				nationalID = model.nationalID,
				birthIdentificationNo = model.birthIdentificationNo,
				govtID = model.govtID,
				gpfNomineeName = model.gpfNomineeName,
				gpfAcNo = model.gpfAcNo,
				nameEnglish = model.nameEnglish.ToUpper(),
				nameBangla = model.nameBangla,
				expertiseId = model.expertiseId,
				PassportNo = model.PassportNo,
				PassportIssueDate = model.PassportIssueDate,
				PassportExDate = model.PassportExDate,
				identificationSign = model.identificationSign,

				motherNameEnglish = model.motherNameEnglish,
				motherNameBangla = model.motherNameBangla,
				motherOccupationId = model.motherOccupationId,
				motherNationality = model.motherNationality,
				motherNID = model.motherNID,
				motherMobile = model.motherMobile,
				motherEmail = model.motherEmail,
				motherAddress = model.motherAddress,
				motherPassportNo = model.motherPassportNo,
				motherPassportIssueDate = model.motherPassportIssueDate,
				motherPassportExDate = model.motherPassportExDate,

				fatherNameEnglish = model.fatherNameEnglish,
				fatherNameBangla = model.fatherNameBangla,
				fatherOccupationId = model.fatherOccupationId,
				fatherNationality = model.fatherNationality,
				fatherNID = model.fatherNID,
				fatherMobile = model.fatherMobile,
				fatherEmail = model.fatherEmail,
				fatherAddress = model.fatherAddress,
				fatherPassportNo = model.fatherPassportNo,
				fatherPassportIssueDate = model.fatherPassportIssueDate,
				fatherPassportExDate = model.fatherPassportExDate,

				nationality = model.nationality,
				disability = model.disability,
				disablityType = model.disablityType,
				dateOfBirth = model.dateOfBirth,
				gender = model.gender,
				birthPlace = model.birthPlace,
				maritalStatus = model.maritalStatus,
				religionId = model.religion,
				//employeeTypeId = model.employeeType,
				employeeTypeId = model.empTypeId,
				tin = model.tin,
				batch = model.batch,
				bloodGroup = model.bloodGroup,
				bloodDonateStatus = model.bloodDonateStatus,
				freedomFighter = model.freedomFighter == "Yes" ? true : false,
				freedomFighterNo = model.freedomFighterNo,
				telephoneOffice = model.telephoneOffice,
				telephoneResidence = model.telephoneResidence,
				pabx = model.pabx,
				emailAddress = model.emailAddress,
				mobileNumberOffice = model.mobileNumberOffice,
				mobileNumberPersonal = model.mobileNumberPersonal,
				emailAddressPersonal = model.emailAddressPersonal,

				LPRDate = LPRDate, // Convert.ToDateTime(dateLPR).AddDays(-1),
				PRLStartDate = PRLStartDate, // Convert.ToDateTime(dateLPR),
				PRLEndDate = PRLEndDate, //Convert.ToDateTime(dateLPR).AddYears(1),
				joiningDatePresentWorkstation = model.joiningDatePresentWorkstation,
				joiningDateGovtService = model.joiningDateGovtService,
				//dateofregularity = model.dateofregularity,
				dateOfPermanent = model.dateOfPermanent,
				branchId = model.branchIdsbu,
				pNSId = model.pns,
				height = model.height,
				weight = model.weight,
				DateOfRetirement = model.DateOfRetirement,
				BranchName = model.BranchName,
				sbAccount = model.sbAccount,

				natureOfRequitment = model.natureOfRequitment,
				activityStatus = model.activityStatus, //
				departmentId = model.department,
				specialSkill = model.specialSkill,
				seniorityNumber = model.seniorityNumber,
                //joiningDesignation = model.joiningDesignation,
                JoinDesignationId = model.JoinDesignationId,
				designation = model.designation,
				disabilityTypeId = model.disabilityTypeId,
				skypeId = model.skypeId,
				TwitterId = model.TwitterId,
				FacebookId = model.FacebookId,
				InstagramId = model.InstagramId,
				LinkedInId = model.LinkedInId,
				WhatsAppId = model.WhatsAppId,

				DivisionId = model.DivisionId,
				hrBranchId = model.hrBranchId,
				hrDivisionId = model.hrDivisionId,

				//permanentResidence=model.permanentResidence,
				//prCountryId=model.prCountryId,
				//duelCountryId=model.duelCountryId,
				//duelPassport=model.duelPassport,
				//foodLiking=model.foodLiking,

				//joiningGradeId = model.joiningGradeId,
				joiningGradeName = model.joiningGradeName,
				joiningBasic = model.joiningBasic,
				currentGradeId = model.currentGradeId,
				currentBasic = model.currentBasic,

				employeeStatusId = model.employeeStatusId,
				hrProgramId = model.hrProgramId,
				hrUnitId = model.hrUnitId,
				functionInfoId = model.functionInfoId,
				locationId = model.locationId,
				post = model.post,
				homeDistrict = model.homeDistrict,
				orgType = "Bank",
				salaryStatus = model.salaryStatus,
				salaryStatusComment = model.salaryStatusComment,

				//isDelete = 1, //inactive
				//approveStatus = 1 //waiting for emp edit

				lastPromotionDate = model.lastPromotionDate,
				prlDate = model.prlDate,
				refLetterNo = model.refLetterNo,
				lastDesignationId = model.lastDesignationId,
				lastSalary = model.lastSalary,
				prevOrganisationResp = model.prevOrganisationResp,
				significantAchivement = model.significantAchivement,
				areaOfProficiency = model.areaOfProficiency,
				joiningLocation = model.joiningLocation,
				placeOfPosting = model.placeOfPosting,
				probationStart = model.probationStart,
				probationEnd = model.probationEnd,
				bondDateStart = model.bondDateStart,
				bondDateEnd = model.bondDateEnd,
				lastWorkingDate = model.lastWorkingDate,
				fileNo = model.fileNo,
				presentPosting = model.presentPosting,
				lastTransferDate = model.lastTransferDate,
				qualification = model.qualification,
				problem = model.problem,
				contructualType = model.contructualType,
				shiftGroupId = model.shiftGroupId,
				isFestival = model.eligible,
				customRoleId = model.role,
				isDelete = 0,
				isApproved = 1,
				isManager = model.isManager,
				seniorityLevel = model.seniorityNumber,
				SupervisorCode = model.SupervisorCode,
				SupervisorName = model.SupervisorName



			};

			//create user
			//var userinfo = await userInfoes.GetSbuIdByEmployeeEmail(model.emailAddress);
			int maxUserId = await userInfoes.GetMaxUserId() + 1;
			var user1 = new ApplicationUser { UserName = model.employeeCode, isActive = 1, Email = model.emailAddress, userId = maxUserId, EmpCode = model.employeeCode, userTypeId = 9 };
			var result = await _userManager.CreateAsync(user1, "123456");

			if (result.Succeeded)
			{
				await _userManager.AddToRoleAsync(user1, "UAT PIMS");
				IdentityUser temp = await _userManager.FindByNameAsync(model.employeeCode);
				//var emp = await personalInfoService.GetEmployeeInfoByCode(model.employeeCode);
				data.ApplicationUserId = temp.Id;
				//await personalInfoService.SaveEmployeeInfo(emp);
			}

			int lstId = await personalInfoService.SaveEmployeeInfo(data);
			await personalInfoService.UpdateEmployeeinfoById(lstId);

			var punchCard = new EmployeePunchCardInfo
			{
				Id = 0,
				employeeCode = model.employeeCode,
				punchCardNo = model.employeeCode,
				shiftGroupMasterId = 1
			};
			await employeePunchCardInfoService.SaveEmployeePunchCardInfo(punchCard);

			int ApplicantId = await personalInfoService.IsThisApplicantIdPresent(model.ApplicantId);
			if (ApplicantId == (model.ApplicantId))
			{
				var a = await applicationFormService.GetApplicationFormById(ApplicantId);
				a.newBranchId = model.hrBranchId;
				a.newDesignationId = model.lastDesignationId;
				a.email = model.emailAddress;

				await applicationFormService.SaveApplicationForm(a);
			}

			//await employeeInfoService.DeleteJobHistoryByEmpId((int)model.employeeId[0]);
			if (model.employeeIdJH != null)
			{
				if (model.employeeIdJH.Length > 0)
				{
					for (int i = 0; i < model.employeeIdJH.Length; i++)
					{
						var jobHistory = new PrevJobHistory
						{
							Id = 0,
							employeeId = model.employeeIdJH[i],
							company = model.company[i],
							position = model.position[i],
							jobstart = Convert.ToDateTime(model.jobstart[i]),
							jobend = Convert.ToDateTime(model.jobend[i]),
						};
						await employeeInfoService.SaveJobHistory(jobHistory);
					}
				}

			}



			//await employeeInfoService.DeleteBDBLJobHistoryByEmpId((int)model.employeeId[0]);

			if (model.postingID != null)
			{
				for (int i = 0; i < model.postingID.Length; i++)
				{
					if (Convert.ToInt32(model.postingID[i]) == 0)
					{
						var empposting = new EmployeePostingPlace
						{
							Id = Convert.ToInt32(model.postingID[i]),
							employeeId = model.employeeIdBDBL[i],
							placeName = model.bdblplaceName[i],
							placeNameBn = model.bdblplaceNameBn[i],
							remarks = model.bdblremarks[i],
							type = Convert.ToInt32(model.bdbltype[i]),
							branchId = Convert.ToInt32(model.bdblSBU[i]),
							hrBranchId = Convert.ToInt32(model.bdblBranch[i]),
							departmentId = Convert.ToInt32(model.bdblDepartment[i]),
							hrDivisionId = Convert.ToInt32(model.bdblDivision[i]),
							hrUnitId = Convert.ToInt32(model.bdblUnit[i]),
							locationId = Convert.ToInt32(model.bdblZone[i]),
							startDate = Convert.ToDateTime(model.bdblStartDatejs[i]),
							endDate = Convert.ToDateTime(model.bdblEndDatejs[i]),
							officeId = Convert.ToInt32(model.bdblOffice[i]),
							status = 1

						};
						empposting.departmentId = empposting.departmentId == 0 ? null : empposting.departmentId;
						empposting.hrBranchId = empposting.hrBranchId == 0 ? null : empposting.hrBranchId;
						empposting.hrDivisionId = empposting.hrDivisionId == 0 ? null : empposting.hrDivisionId;
						empposting.officeId = empposting.officeId == 0 ? null : empposting.officeId;
						empposting.locationId = empposting.locationId == 0 ? null : empposting.locationId;
						empposting.hrUnitId = empposting.hrUnitId == 0 ? null : empposting.hrUnitId;

						//var postid = await employeeInfoService.SaveBDBLJobHistory(empposting);

						if (empposting.departmentId != null || empposting.officeId != null)
						{
							empposting.hrBranchId = 205;
						}

						if (model.branchbdbl != null)
						{
							empposting.placeNameBn = await employeePunchCardInfoService.GetPlaceNameById("branch", Convert.ToInt32(model.branchbdbl));
						}
						else if (model.departmentbdbl != null)
						{
							empposting.placeNameBn = await employeePunchCardInfoService.GetPlaceNameById("department", Convert.ToInt32(model.departmentbdbl));
						}
						else if (model.divisionbdbl != null)
						{
							empposting.placeNameBn = await employeePunchCardInfoService.GetPlaceNameById("division", Convert.ToInt32(model.divisionbdbl));
						}
						else if (model.officebdbl != null)
						{
							empposting.placeNameBn = await employeePunchCardInfoService.GetPlaceNameById("office", Convert.ToInt32(model.officebdbl));
						}
						else if (model.locationbdbl != null)
						{
							empposting.placeNameBn = await employeePunchCardInfoService.GetPlaceNameById("location", Convert.ToInt32(model.locationbdbl));
						}
						else if (model.unitbdbl != null)
						{
							empposting.placeNameBn = await employeePunchCardInfoService.GetPlaceNameById("unit", Convert.ToInt32(model.unitbdbl));
						}
						else
						{

						}

						var postid = await employeeInfoService.SaveBDBLJobHistory(empposting);

						personalInfoService.UpdateEmployeePosting(Convert.ToInt32(model.employeeID), Convert.ToInt32(model.statusbdbl), empposting.departmentId, empposting.locationId, empposting.officeId, empposting.hrDivisionId, empposting.hrUnitId, empposting.hrBranchId, postid);

					}
				}
			}



			return RedirectToAction("EditGrid", "Photograph", new { @id = lstId });
		}

		[HttpPost]
		public async Task<IActionResult> JoiningDesignation(EmployeeInfoViewModel model)
		{
			var plan = await designationDepartmentService.GetDesignations();
			string desiCode = (plan.Count() + 1).ToString();
			ViewBag.desiCode = desiCode;
			var data = new Designation
			{
				Id = 0,
				designationName = model.designationName,
				designationCode = desiCode


			};
			await designationDepartmentService.SaveDesignation(data);
			return Json("saved");
		}

		public async Task<IActionResult> GetAllJoiningDesignation()
		{

			return Json(await designationDepartmentService.GetDesignations());
		}

		[HttpPost]

		public async Task<IActionResult> Location(EmployeeInfoViewModel model)
		{
			Location data = new Location
			{
				Id = 0,
				branchUnitName = model.LocationName,
				branchCode = model.branchCode,
				companyId = model.companyId,

			};

			await locationService.SaveLocation(data);
			return Json("saved");
		}

		public async Task<IActionResult> GetAllLocation()
		{

			return Json(await locationService.GetLocation());
		}
		[HttpPost]

		public async Task<IActionResult> Function(EmployeeInfoViewModel model)
		{
			FunctionInfo data = new FunctionInfo
			{
				Id = 0,
				branchUnitName = model.LocationName,
				branchCode = model.branchCode,
				companyId = model.companyId,

			};

			await functionsInfoService.SaveFunctionInfo(data);


			return Json("saved");
		}

		public async Task<IActionResult> GetAllFunction()
		{

			return Json(await functionsInfoService.GetFunctionInfo());
		}
		public async Task<IActionResult> Department(EmployeeInfoViewModel model)
		{
			Department data = new Department
			{
				Id = 0,
				deptName = model.LocationName,
				deptCode = model.branchCode,

			};

			await designationDepartmentService.SaveDepartment(data);

			return Json("saved");
		}

		public async Task<IActionResult> GetAllDepartment()
		{
			return Json(await designationDepartmentService.GetDepartment());

		}

		public async Task<IActionResult> Unit(EmployeeInfoViewModel model)
		{

			HrUnit data = new HrUnit
			{
				Id = 0,
				unitName = model.LocationName,


			};

			await statusService.SaveHrUnit(data);


			return Json("saved");
		}

		public async Task<IActionResult> GetAllUnit()
		{
			return Json(await statusService.GetHrUnit());

		}

		public async Task<IActionResult> Branch(EmployeeInfoViewModel model)
		{

			HrBranch data = new HrBranch
			{
				Id = 0,
				branchName = model.LocationName,
				branchTypeId = Convert.ToInt32(model.branchCode),

			};

			await statusService.SaveHrBranch(data);
			return Json("saved");
		}

		public async Task<IActionResult> GetAllProfileUpdateRequest()
		{
			var model = new EmployeeInfoViewModel
			{
				allEmployeeInfos = await employeeInfoService.GetAllProfileUpdateReqEmp()
			};
			return View(model);
		}

		public async Task<IActionResult> GetAllBranch()
		{
			return Json(await statusService.GetHrBranch());

		}
		public async Task<IActionResult> Program(EmployeeInfoViewModel model)
		{

			HrProgram data = new HrProgram
			{
				Id = 0,
				programName = model.LocationName,

			};

			await statusService.SaveHrProgram(data);
			return Json("saved");
		}

		public async Task<IActionResult> GetAllProgram()
		{
			return Json(await statusService.GetHrProgram());

		}
		public async Task<IActionResult> GetAllBranchType()
		{
			return Json(await statusService.GetHrBranchType());

		}
		public async Task<IActionResult> GetAllCompany()
		{
			return Json(await functionsInfoService.GetAllCompany());

		}
		public async Task<IActionResult> GetPresentPosting(int empid)
		{
			return Json(await functionsInfoService.GetpresentPosting(empid));
		}



		// GET: Info/FormView
		public async Task<IActionResult> FormViewInfo(int id)
		{
			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
			ViewBag.employeeID = id.ToString();
			var data = await personalInfoService.GetEmployeeInfoById(id);
			string desigName = data.designation;
			var model = new FormViewViewModel
			{
				employeeID = id.ToString(),
				permanent = await employeeInfoService.GetAddressByTypeAndEmpId(id, "permanent"),
				present = await employeeInfoService.GetAddressByTypeAndEmpId(id, "present"),
				photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
				fLang = _lang.PerseLang("Employee/EmployeeInfoEN.json", "Employee/EmployeeInfoBN.json", Request.Cookies["lang"]),
				employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
				religions = await religionMunicipalityService.GetReligions(),
				employeeTypes = await typeService.GetAllEmployeeType(),
				employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id),
				// organoOrganizations = await organizationPostService.GetAllOrganizationByIds(this.GetAllIdsByOrg(user.org)),
				organoOrganizations = await organizationPostService.GetAllOrganization(),
				designations = await designationDepartmentService.GetDesignations(),
				specialBranchUnits = await specialBranchUnitService.GetSpecialBranchUnit(),
				districts = await addressService.GetAllDistrict(),
				departments = await designationDepartmentService.GetDepartment(),
				serviceStatuses = await statusService.GetServiceStatus(),
				hrPrograms = await statusService.GetHrProgram(),
				hrUnits = await statusService.GetHrUnit(),
				locations = await locationService.GetLocation(),
				functionInfos = await functionsInfoService.GetFunctionInfo(),
				visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
				divisions = await addressService.GetAllDivision(),

				hrDivisions = await statusService.GetHrDivision(),
				hrBranches = await statusService.GetHrBranch(),
				spouses = await spouseChildrenService.GetSpouseByEmpId(id),
				spouse = await spouseChildrenService.GetSpouseByEmpId1(id),
				Sdistricts = await addressService.GetAllDistrict(),
				Soccupations = await nomineeService.GetOccupation(),
				children = await spouseChildrenService.GetChildrenByEmpId(id),
				child = await spouseChildrenService.GetchildByEmpId1(id),
				occupations = await nomineeService.GetOccupation(),
				educationalQualifications = await employeeInfoService.GetEducationalQualificationByEmpId(id),
				levelofeducationNamelist = await levelofEducationService.GetAllLevelofEducation(),
				degreeslist = await degreeService.GetAllDegree(),
				subjectslist = await subjectService.GetAllSubject(),
				emergencyContacts = await emergencyContactService.GetEmergencyContactByEmpId(id),
				relations = await nomineeService.GetRelation(),
				//nominee
				nomineeFunds = await nomineeFundService.GetNomineeFund(),
				nominees = await nomineeService.GetNomineeByEmpId(id),
				nominee = await nomineeService.GetNomineeByEmpId1(id),
				employeeInsurances = await nomineeService.GetEmployeeInsuranceByEmpId(id),

				qualificationHeads = await qualificationHeadService.GetQualificationHead(),
				results = await resultService.GetAllResult(),
				professionalQualifications = await professionalQualificationsService.GetProfessionalQualificationsByEmpId(id),
				otherQualificationHeads = await otherQualificationHeadService.GetOtherQualificationHead(),
				otherQualifications = await otherQualificationService.GetOtherQualificationByEmpId(id),
				countries = await addressService.GetAllContry(),
				trainingCategories = await trainingService.GetTrainingCategories(),
				trainingInstitutes = await trainingService.GetTrainingInstitute(),
				traningLogs = await traningHistoryService.GetTraningHistoryByEmpId(id),
				salaryGrade = await salaryGradeService.GetAllSalaryGrade(),
				transferLogs = await serviceHistoryService.GetServiceHistoryByEmpId(id),
				promotionLogs = await promotionLogService.GetPromotionLogByEmpId(id),
				salaryGrades = await salaryGradeService.GetAllSalaryGrade(),
				designationName = await designationDepartmentService.GetDesignationIdByName(desigName),
				acrInfos = await acrInfoService.GetAcrInfoByEmpId(id),

				offenses = await disciplineInvestigation.GetAllOffense(),
				punishments = await disciplineInvestigation.GetAllPunishment(),
				disciplinaries = await disciplinaryService.GetDisciplinaryLogByEmpId(id),
				approvalTypes = await approvalService.GetAllApprovalType(),
				licenses = await drivingLicenseService.GetDrivingLicenseByEmpId(id),
				passportDetails = await passportInfoService.GetPassportInfoByEmpId(id),
				traveInfos = await travelInfoService.GetTraveInfoByEmpId(id),
				travelPurposes = await travelService.GetTravelPurposes(),
				projects = await projectService.GetProjectList(),
				memberships = await membershipLanguageService.GetMembershipInfo(),
				employeeMemberships = await membershipService.GetMembershipInfoByEmpId(id),
				awards = await awardPublicationService.GetAwardsByEmpId(id),
				publications = await awardPublicationService.GetPublicationsByEmpId(id),
				employeeLanguages = await awardPublicationService.GetLanguageByEmpId(id),
				languages = await membershipLanguageService.GetLanguageInfo(),
				hRPMSActivityTypes = await iHRPMSActivityTypeService.GetHRPMSActivityType(),
				otherActivities = await otherActivityService.GetOtherActivityByEmpId(id),
				bankInfos = await bankInfoService.GetBankInfoByEmpId(id),
				banks = await bankBranchService.GetAllBank(),
				walletTypes = await salaryService.GetAllWalletType(),
				belongings = await belongingsService.GetBelongingsByEmpId(id),
				belongingItems = await belongingsItemService.GetBelongingItem(),
				priviousEmployments = await priviousEmploymentService.GetPriviousEmploymentByEmpId(id),
				hRPMSOrganizationTypes = await iHRPMSOrganizationTypeService.GetHRPMSOrganizationType(),
				// freedomFighter = await freedomFighterService.GetFreedomFighterByEmpId(id),
				references = await referenceService.GetReferenceByEmpId(id),
				officeAssigns = await officeAssignService.GetOfficeAssignByEmpId(id),
				employeeContractInfos = await employeeContratInfoService.GetContractInfoByEmpId(id),
				hRDoners = await hRDonerSevice.GetHRDoner(),
				hRActivities = await hRActivityService.GetHRActivity(),
				hRProjects = await hRProjectService.GetHRProject(),
				employeeProjectActivities = await employeeProjectActivityService.GetEmployeeProjectActivityByEmpId(id),
				financeCodes = await financeCodeService.GetFinanceCodeByEmpId(id),
				atttachmentCategory = await attachmentCategoryService.GetAllAttachmentCategory(),
				hRPMSAttachments = await hRPMSAttachmentService.GetHRPMSAttachmentByEmpId(id),
				atttachmentGroups = await attachmentCategoryService.GetAllAtttachmentGroup(),
				employeeProjectAssigns = await employeeProjectActivityService.GetEmployeeProjectAssignByEmpId(id),
				hRFacilities = await hRActivityService.GetHRFacility(),
				employeeOtherInfos = await employeeProjectActivityService.GetEmployeeOtherInfoByEmpId(id),
				costCentres = await costCentreService.GetCostCentres(),
				employeeCostCentres = await employeeProjectActivityService.GetEmployeeCostCentreByEmpId(id),
				employeeGrades = await employeeProjectActivityService.GetEmployeeGradeByEmpId(id),
				ielts = await personalInfoService.GetIeltsInfoByEmpId(id),
				computerLiteracy = await subjectService.GetAllcomputerLiteracy(),
				computerLiteracies = await subjectService.GetcomputerLiteracyByEmpId(id),
				ComputerSubjectsList = await subjectService.GetAllComputerSubject(),
				//  employeeSignature = await photographService.GetEmployeeSignatureByEmpId(id),
				// locations = await specialBranchUnitService.GetLocation(),
				freedomFighters = await freedomFighterService.GetFreedomFighterlistByEmpId(id),
				awardMaster = await bookAwardService.GetAwardInfo(),
				bankingDiplomas = await bankInfoService.GetBankDiplomaInfoByEmpId(id),
				taxes = await taxService.GetTaxByEmpId(id),
				duelResidences = await taxService.GetDuelResidenceByEmpId(id),
				duelCountry = await addressService.GetAllContry(),
				foodLiking = await personalInfoService.GetFoodLikingById(id),
				employeeSports = await EmployeeSportsService.GetEmployeeSportsByEmpId(id),
				customRoles = await addressService.GetCustomRoleName(),
				employeeRoles = await addressService.GetEmployeeRoleByEmpId(id),
				employeeRole = await addressService.GetEmployeeRoleByEmpId1(id),
				medicalDiseases = await religionMunicipalityService.GetMedicalDiseases(),
				employeeDiseases = await employeeDiseaseService.GetEmployeeDiseaseByEmpId(id),
				mobileBenifits = await mobileBenifitService.GetMobileBenifitByEmpId(id),
				supensionDetails = await suspensionReportService.GetSuspensionsByEmployeeId(id),
				supensionDetail = await suspensionReportService.GetSuspensionById(id),
				extraCurricularType = await extraCurricularTypeService.GetExtraCurricularType(),
				employeeExtraCurriculars = await employeeExtraCurricularService.GetEmployeeExtraCurricularEmpId(id),
				employeeSignature = await photographService.GetEmployeeSignatureByEmpId(id),

			};
			if (id != null)
			{
				model.approvalMasters = await approvalService.GetApprovalMaster(Convert.ToInt32(id));
			}
			else
			{
				model.approvalMasters = await approvalService.GetAllApprovalMaster();
			}

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Signature([FromForm] FormViewViewModel model)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);

			if (!ModelState.IsValid)
			{
				ViewBag.employeeID = model.employeeID;
				model.employeeSignature = await photographService.GetEmployeeSignatureByEmpId(Int32.Parse(model.employeeID));
				model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(Int32.Parse(model.employeeID));
				if (model.employeeSignature == null) model.employeeSignature = new EmployeeSignature();
				return View(model);
			}

			string fileName;
			string message = FileSave.SaveImage(out fileName, model.EngSign);
			string fileNameB;
			string messageB = FileSave.SaveImage(out fileNameB, model.banglaSign);

			EmployeeSignature data = new EmployeeSignature();
			if (message == "success")
			{
				data.Id = model.photographID;
				data.employeeId = Int32.Parse(model.employeeID);
				data.url = fileName;
			}
			if (messageB == "success")
			{
				data.banglaSign = fileNameB;
			}

			await photographService.SaveEmployeeSignature(data);

			//return RedirectToAction(nameof(Signature));
			return RedirectToAction("FormView", "Info", new
			{
				id = model.employeeID
			});
		}

		[HttpPost]
		public async Task<IActionResult> UpdateCurricular([FromForm] FormViewViewModel model)
		{
			EmployeeExtraCurricular data = new EmployeeExtraCurricular
			{
				Id = (Int32.Parse(model.ExtraCurricularId)),
				employeeId = model.employeeId,
				extraCurricularTypeId = model.extraCurricularTypeId,

				skillLevel = model.skillLevel,
				skillType = model.skillType,
				description = model.description


			};
			await employeeExtraCurricularService.SaveEmployeeExtraCurricular(data);
			return Json("updated");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateAllegation([FromForm] FormViewViewModel model)
		{
			Allegation data = new Allegation
			{
				Id = Convert.ToInt32(model.allegationID),
				allegationDetail = model.allegationDetail,
				//  allegationUrl = fileName,
				clarification = model.clarification,
				//  clarificationUrl = fileNameC,
				management = model.management,
				//   managementUrl = fileNameM,
				isActive = 1,
				status = 1,
				employeeId = model.employeeId
			};
			await disciplineInvestigation.SaveAllegation(data);
			return Json("updated");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateMobileBenifit([FromForm] FormViewViewModel model)
		{
			MobileBenifit data = new MobileBenifit
			{
				Id = (Int32.Parse(model.MobileBenifitID)),
				employeeId = model.employeeId,
				type = model.type,
				amount = model.amount,
				date = model.date,
				status = 1,


			};
			await mobileBenifitService.SaveMobileBenifit(data);
			return Json("updated");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateSuspension([FromForm] FormViewViewModel model)
		{
			Suspension data = new Suspension
			{
				Id = Convert.ToInt32(model.suspensionID),
				susDesc = model.susDesc,
				hearingReport = model.hearingReport,
				chargeSheetDesc = model.chargeSheetDesc,
				punishmentType = model.punishmentType,
				effectiveForm = model.effectiveForm,
				status = 1,
				isActive = 1,
				employeeId = model.employeeId
			};
			await suspensionReportService.SaveSuspension(data);
			return Json("updated");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateEmployeeRoll([FromForm] FormViewViewModel model)
		{
			EmployeeRole data = new EmployeeRole
			{
				employeeId = Int32.Parse(model.employeeID),
				Id = model.emproleid,
				startDate = model.startDate,
				endDate = model.endDate,
				roleId = model.roleId,



			};
			await addressService.SaveEmployeeRole(data);
			return Json("updated");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateEmployeeDisease([FromForm] FormViewViewModel model)
		{
			EmployeeDisease data = new EmployeeDisease
			{
				Id = model.EmployeeDiseaseId,
				employeeInfoId = Int32.Parse(model.employeeID),
				medicalDiseaseId = model.medicalDiseaseId,
				status = model.status1,

			};
			await employeeDiseaseService.SaveEmployeeDisease(data);
			return Json("updated");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateEmployeeSports([FromForm] FormViewViewModel model)
		{
			EmployeeSports data = new EmployeeSports
			{
				Id = model.EmployeeSportsId,
				employeeId = Int32.Parse(model.employeeID),
				skillType = model.skillType,
				skillLevel = model.skillLevel,

			};
			await EmployeeSportsService.SaveEmployeeSports(data);
			return Json("updated");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateFoodLiking([FromForm] FormViewViewModel model)
		{
			var data = new FoodLiking
			{
				Id = model.foodLikingId,
				employeeId = Int32.Parse(model.employeeID),
				vegiterian = model.vegiterian,
				nonVegiterian = model.nonVegiterian
			};

			await personalInfoService.SaveFoodLiking(data);
			return Json("updated");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateDuelResidence([FromForm] FormViewViewModel model)
		{
			DuelResidence data = new DuelResidence
			{
				Id = Int32.Parse(model.DuelResidenceID),
				employeeId = Int32.Parse(model.employeeID),
				duelCountryId = model.duelCountryId,
				duelPassport = model.duelPassport,

			};

			await taxService.SaveDuelResidence(data);
			return Json("updated");
		}
		[HttpPost]
		public async Task<IActionResult> UpdateTax([FromForm] FormViewViewModel model)
		{
			Tax data = new Tax
			{
				Id = Int32.Parse(model.taxID),
				employeeId = Int32.Parse(model.employeeID),
				taxZone = model.taxZone,
				taxCircle = model.taxCircle,

			};

			await taxService.SaveTax(data);
			return Json("updated");
		}
		[HttpPost]
		public async Task<IActionResult> UpdateDiploma([FromForm] FormViewViewModel model)
		{
			BankingDiploma data = new BankingDiploma
			{
				Id = model.bankDiplomaId,
				employeeId = Int32.Parse(model.employeeID),
				diplomaPart = model.diplomaPart,
				//diplomaName = model.diplomaName,
				passingYear = model.passingYear2,
				//resultType=model.resultType,
				result = model.result2

			};

			await bankInfoService.SaveBankingDiplomaInfo(data);
			return Json("updated");
		}

		// new

		[HttpPost]
		public async Task<IActionResult> UpdateEmpPersonalInfo([FromForm] FormViewViewModel model)
		{
			var emp = await personalInfoService.GetEmployeeInfoById(Convert.ToInt32(model.employID));
			emp.employeeCode = model.employeeCode;
			emp.maritalStatus = model.maritalStatus;
			emp.nameEnglish = model.nameEnglish;
			emp.bloodGroup = model.bloodGroup;
			emp.nameBangla = model.nameBangla;
			emp.mobileNumberPersonal = model.mobileNumberPersonal;
			//	emp.fatherNameEnglish = model.fatherNameEnglish;
			emp.telephoneResidence = model.telephoneResidence;
			emp.fatherNameBangla = model.fatherNameBangla;
			emp.emailAddressPersonal = model.emailAddressPersonal;
			emp.motherNameEnglish = model.motherNameEnglish;
			emp.birthPlace = model.birthPlace;
			emp.motherNameBangla = model.motherNameBangla;
			emp.homeDistrict = model.homeDistrict;
			emp.dateOfBirth = model.dateOfBirth;
			emp.tin = model.tin;
			emp.nationalID = model.nationalID;
			emp.height = model.height;
			emp.nationality = model.nationality;
			emp.weight = model.weight;
			emp.gender = model.gender;
			emp.disability = model.disability;
			emp.religionId = model.religion;
			emp.disablityType = model.disablityType;
			emp.salaryStatusComment = model.salaryStatusComment;
			emp.skypeId = model.skypeId;
			emp.FacebookId = model.FacebookId;
			emp.LinkedInId = model.LinkedInId;
			emp.TwitterId = model.TwitterId;
			emp.InstagramId = model.InstagramId;
			emp.WhatsAppId = model.WhatsAppId;
			emp.joiningDateGovtService = model.joiningDateGovtService;
			emp.natureOfRequitment = model.natureOfRequitment;
			emp.joiningDesignation = model.joiningDesignation;
			emp.dateofregularity = model.dateofregularity;
			emp.dateOfPermanent = model.dateOfPermanent;
			emp.DateOfRetirement = model.DateOfRetirement;
			emp.activityStatus = model.activityStatus;
			emp.salaryStatus = model.salaryStatus;
			emp.employeeTypeId = model.employeeType;
			emp.employeeStatusId = model.employeeStatusId;
			emp.telephoneOffice = model.telephoneOffice;
			emp.pabx = model.pabx;
			emp.emailAddress = model.emailAddress;
			emp.mobileNumberOffice = model.mobileNumberOffice;
			emp.locationId = model.locationId;
			emp.branchId = model.sbu;
			emp.hrDivisionId = model.hrDivisionId;
			emp.functionInfoId = model.functionInfoId;
			emp.departmentId = model.departmentId;
			emp.hrUnitId = model.hrUnitId;
			emp.hrBranchId = model.hrBranchId;
			emp.hrProgramId = model.hrProgramId;
			emp.locationId = model.locationId;
			//emp.maritalStatus = model.maritalStatus;
			emp.DivisionId = model.DivisionId;
			emp.lastPromotionDate = model.lastPromotionDate;
			emp.prlDate = model.prlDate;
			emp.refLetterNo = model.refLetterNo;
			emp.lastDesignationId = model.lastDesignationId;
			emp.lastSalary = model.lastSalary;
			emp.prevOrganisationResp = model.prevOrganisationResp;
			emp.significantAchivement = model.significantAchivement;
			emp.areaOfProficiency = model.areaOfProficiency;
			emp.joiningLocation = model.joiningLocation;
			emp.placeOfPosting = model.placeOfPosting;
			emp.probationStart = model.probationStart;
			emp.probationEnd = model.probationEnd;
			emp.bondDateStart = model.bondDateStart;
			emp.bondDateEnd = model.bondDateEnd;
			emp.lastWorkingDate = model.lastWorkingDate;
			emp.fileNo = model.fileNo;
			emp.presentPosting = model.presentPosting;
			emp.lastTransferDate = model.lastTransferDate;
			emp.qualification = model.qualification;
			emp.problem = model.problem;
			emp.contructualType = model.contructualType;
			emp.shiftGroupId = model.shiftGroupId;
			emp.isDelete = model.isDelete;


			emp.fatherNameBangla = model.fatherNameBangla;
			emp.fatherNationality = model.fatherNationality;
			emp.motherNationality = model.motherNationality;
			emp.fatherNID = model.fatherNID;
			emp.motherNID = model.motherNID;
			emp.fatherOccupationId = model.fatherOccupationId;
			emp.motherOccupationId = model.motherOccupationId;
			emp.fatherMobile = model.fatherMobile;
			emp.motherMobile = model.motherMobile;
			emp.fatherEmail = model.fatherEmail;
			emp.motherEmail = model.motherEmail;
			emp.fatherPassportNo = model.fatherPassportNo;
			emp.motherPassportNo = model.motherPassportNo;
			emp.fatherPassportIssueDate = model.fatherPassportIssueDate;
			emp.motherPassportIssueDate = model.motherPassportIssueDate;
			emp.fatherPassportExDate = model.fatherPassportExDate;
			emp.motherPassportExDate = model.motherPassportExDate;
			emp.fatherAddress = model.fatherAddress;
			emp.motherAddress = model.motherAddress;
			emp.favouriteColor = model.favouriteColor;
			emp.birthIdentificationNo = model.birthIdentificationNo;

			await personalInfoService.SaveEmployeeInfo(emp);
			return Json("updated");
		}
		[HttpPost]
		public async Task<IActionResult> UpdateAddress([FromForm] FormViewViewModel model)
		{
			try
			{
				HRPMS.Data.Entity.Employee.Address presentdata = new HRPMS.Data.Entity.Employee.Address
				{
					Id = model.presentAddressID,
					employeeId = model.addressEmployeeID,
					countryId = 1,
					divisionId = Int32.Parse(model.presentDivision),
					districtId = Int32.Parse(model.presentDistrict),
					thanaId = Int32.Parse(model.presentUpazila),
					union = model.presentUnion,
					postOffice = model.presentPostOffice,
					postCode = model.presentPostCode,
					blockSector = model.presentBlockSector,
					houseVillage = model.presentHouseVillage,
					roadNumber = model.presentRoadNo,
					type = "present"
				};
				await employeeInfoService.SaveAddress(presentdata);


				HRPMS.Data.Entity.Employee.Address permanentdata = new HRPMS.Data.Entity.Employee.Address
				{
					Id = model.permanentAddressID,
					employeeId = model.addressEmployeeID,
					countryId = 1,
					divisionId = Int32.Parse(model.permanentDivision),
					districtId = Int32.Parse(model.permanentDistrict),
					thanaId = Int32.Parse(model.permanentUpazila),
					union = model.permanentUnion,
					postOffice = model.permanentPostOffice,
					postCode = model.permanentPostCode,
					blockSector = model.permanentBlockSector,
					houseVillage = model.permanentHouseVillage,
					roadNumber = model.permanentRoadNo,
					type = "permanent"
				};

				await employeeInfoService.SaveAddress(permanentdata);
			}
			catch (Exception ex)
			{

			}
			return Json("updated");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateSpouce([FromForm] FormViewViewModel model)
		{
			//var emp = await personalInfoService.GetEmployeeInfoById(Convert.ToInt32(model.employID));
			var spouce = new Spouse
			{
				Id = model.spouseID,
				employeeId = model.SpouseEmployeeID,
				spouseName = model.spouseName,
				nationality = model.Snationality,
				spouseNameBN = model.spouseNameBN,
				bloodGroup = model.SbloodGroup,
				contact = model.Scontact,
				relationship = model.Srelationship,
				gender = model.Sgender,
				email = model.SemailAddress,
				dateOfBirth = model.SdateOfBirth,
				occupationId = model.SoccupationId,
				nid = model.Snid,
				organization = model.Sorganization,
				imageUrl = ""
			};


			//return Json("updated");
			var Id = await spouseChildrenService.SaveSpouse(spouce);
			return Json(Id);
		}

		[HttpPost]
		public async Task<JsonResult> UpdateSpouseFile(int? id)
		{
			var files = Request.Form.Files;
			if (Request.Form.Files.Count > 0)
			{
				for (int i = 0; i < Request.Form.Files.Count; i++)
				{
					int _min = 10000;
					int _max = 99999;
					Random _rdm = new Random();
					int rnd = _rdm.Next(_min, _max);

					string filePath = string.Empty;
					string fileName = string.Empty;
					string fileType = string.Empty;

					IFormFile file = Request.Form.Files[i];
					fileType = file.ContentType;
					fileName = rnd + file.FileName;
					filePath = "wwwroot/EmpImages/" + fileName;
					//var fileD = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Attachments\\RoutingFile", fileName);
					var fileD = Path.Combine(Directory.GetCurrentDirectory(), filePath);
					using (var fileSrteam = new FileStream(fileD, FileMode.Create))
					{
						await file.CopyToAsync(fileSrteam);
					}

					var Spouse = await spouseChildrenService.GetSpouseById((int)id);
					Spouse.imageUrl = fileName;

					await spouseChildrenService.SaveSpouse(Spouse);
				}
			}
			return Json(true);
		}



		[HttpPost]
		public async Task<IActionResult> UpdateChildren([FromForm] FormViewViewModel model)
		{
			var children = new Children
			{
				Id = Int32.Parse(model.childrenID),
				employeeId = model.childrenEmployeeID,
				childName = model.childName,
				childNameBN = model.childNameBN,
				dateOfBirth = model.CdateOfBirth,
				occupationId = model.CoccupationId,
				gender = model.Cgender,
				designation = model.Cdesignation,
				organization = model.organization,
				bin = model.bin,
				nid = model.nid,
				bloodGroup = model.CbloodGroup,
				disability = model.Cdisability,
				nationality = model.Cnationality,
				relationship = model.Crelationship,
				emailAddressPersonal = model.CemailAddressPersonal,
				disablityType = model.CdisablityType,
				//  contact = model.contact,
				imageUrl = ""
			};
			var Id = await spouseChildrenService.SaveChildren(children);
			return Json(Id);
		}
		[HttpPost]
		public async Task<JsonResult> UpdateChildrenFile(int? id)
		{
			var files = Request.Form.Files;
			if (Request.Form.Files.Count > 0)
			{
				for (int i = 0; i < Request.Form.Files.Count; i++)
				{
					int _min = 10000;
					int _max = 99999;
					Random _rdm = new Random();
					int rnd = _rdm.Next(_min, _max);

					string filePath = string.Empty;
					string fileName = string.Empty;
					string fileType = string.Empty;

					IFormFile file = Request.Form.Files[i];
					fileType = file.ContentType;
					fileName = rnd + file.FileName;
					filePath = "wwwroot/EmpImages/" + fileName;
					//var fileD = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Attachments\\RoutingFile", fileName);
					var fileD = Path.Combine(Directory.GetCurrentDirectory(), filePath);
					using (var fileSrteam = new FileStream(fileD, FileMode.Create))
					{
						await file.CopyToAsync(fileSrteam);
					}

					var children = await spouseChildrenService.GetChildrenById((int)id);
					children.imageUrl = fileName;

					await spouseChildrenService.SaveChildren(children);
				}
			}
			return Json(true);
		}


		[HttpPost]
		public async Task<IActionResult> UpdatePhoto([FromForm] FormViewViewModel model)
		{
			var Photograph = new Photograph
			{
				Id = model.photographID,
				employeeId = model.photographemployeeID,
				url = "",
				type = "profile"
			};

			var Id = await photographService.SavePhotograph(Photograph);
			return Json(Id);
		}
		[HttpPost]
		public async Task<JsonResult> UpdatePhotoFile(int? id)
		{
			var files = Request.Form.Files;
			if (Request.Form.Files.Count > 0)
			{
				for (int i = 0; i < Request.Form.Files.Count; i++)
				{
					int _min = 10000;
					int _max = 99999;
					Random _rdm = new Random();
					int rnd = _rdm.Next(_min, _max);

					string filePath = string.Empty;
					string fileName = string.Empty;
					string fileType = string.Empty;

					IFormFile file = Request.Form.Files[i];
					fileType = file.ContentType;
					fileName = rnd + file.FileName;
					filePath = "wwwroot/EmpImages/" + fileName;
					//var fileD = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Attachments\\RoutingFile", fileName);
					var fileD = Path.Combine(Directory.GetCurrentDirectory(), filePath);
					using (var fileSrteam = new FileStream(fileD, FileMode.Create))
					{
						await file.CopyToAsync(fileSrteam);
					}

					var photo = await photographService.GetPhotographById((int)id);
					photo.url = fileName;

					await photographService.SavePhotograph(photo);
				}
			}
			return Json(true);
		}


		[HttpPost]
		public async Task<IActionResult> UpdateNominee([FromForm] FormViewViewModel model)
		{
			try
			{
				var IdN = 0;
				Nominee data = new Nominee
				{
					Id = model.nomineeID ?? 0,
					employeeID = model.nomineeEmployeeID,
					name = model.name,
					relation = model.relation,
					address = model.address,
					contact = model.contact,
					guardianName = model.guardianName,
					witnessName = model.witnessName,
					nomineeDate = model.nomineeDate,
					Email = model.Email,
					Occupation = model.Occupation,
					Designation = model.Designation,
					Organization = model.Organization,
					NID = model.NID,
					BRN = model.BRN,
					witnessPhone = model.witnessPhone,
					imageUrl = ""

				};
				int maxId = await nomineeService.SaveNominee(data);
				await personalInfoService.UpdateEmployeeinfoById(model.nomineeEmployeeID);
				await nomineeDetailService.DeleteNomineeDetailByNomineeId(maxId);
				for (int i = 0; i < model.fundTypeList.Length; i++)
				{
					NomineeDetail nomineeDetail = new NomineeDetail
					{
						Id = 0,
						nomineeId = maxId,
						nomineeFundId = model.fundTypeList[i],
						persentence = model.fundValueList[i]
					};
					IdN = await nomineeDetailService.SaveNomineeDetail(nomineeDetail);

				}

				return Json(IdN);

			}
			catch (Exception ex)
			{


			}
			return Json("updated");
		}
		[HttpPost]
		public async Task<JsonResult> UpdateNomineeFile(int? id)
		{
			var files = Request.Form.Files;
			if (Request.Form.Files.Count > 0)
			{
				for (int i = 0; i < Request.Form.Files.Count; i++)
				{
					int _min = 10000;
					int _max = 99999;
					Random _rdm = new Random();
					int rnd = _rdm.Next(_min, _max);

					string filePath = string.Empty;
					string fileName = string.Empty;
					string fileType = string.Empty;

					IFormFile file = Request.Form.Files[i];
					fileType = file.ContentType;
					fileName = rnd + file.FileName;
					filePath = "wwwroot/EmpImages/" + fileName;
					//var fileD = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Attachments\\RoutingFile", fileName);
					var fileD = Path.Combine(Directory.GetCurrentDirectory(), filePath);
					using (var fileSrteam = new FileStream(fileD, FileMode.Create))
					{
						await file.CopyToAsync(fileSrteam);
					}

					var Nominee = await nomineeService.GetNomineeById((int)id);
					Nominee.imageUrl = fileName;

					await nomineeService.SaveNominee(Nominee);
				}
			}
			return Json(true);
		}



		//      [HttpPost]
		//public async Task<JsonResult> UpdateChildrenFile(int? id, IFormFile arrayFileAtach)
		//{
		//	//string userName = HttpContext.User.Identity.Name;
		//	var files = Request.Form.Files;
		//	if (Request.Form.Files.Count > 0)
		//	{
		//		for (int i = 0; i < Request.Form.Files.Count; i++)
		//		{
		//			int _min = 10000;
		//			int _max = 99999;
		//			Random _rdm = new Random();
		//			int rnd = _rdm.Next(_min, _max);

		//			string filePath = string.Empty;
		//			string fileName = string.Empty;
		//			string fileType = string.Empty;

		//			IFormFile file = Request.Form.Files[i];
		//			fileType = file.ContentType;
		//			fileName = rnd + file.FileName;
		//			filePath = "wwwroot/Upload/CS/" + fileName;
		//			//var fileD = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Attachments\\RoutingFile", fileName);
		//			var fileD = Path.Combine(Directory.GetCurrentDirectory(), filePath);
		//			using (var fileSrteam = new FileStream(fileD, FileMode.Create))
		//			{
		//				await file.CopyToAsync(fileSrteam);
		//			}

		//			Children children = new Children
		//			{
		//				Id = (int)id,
		//				imageUrl = filePath
		//			};
		//			await spouseChildrenService.SaveChildren(children);
		//		}
		//	}
		//	return Json(true);
		//}

		[HttpPost]
		public async Task<IActionResult> UpdateEmergencyContact([FromForm] FormViewViewModel model)
		{
			EmergencyContact data = new EmergencyContact
			{
				Id = model.refID,
				employeeID = model.contactEmployeeID,
				name = model.refName,
				relation = model.refRelation,
				organization = model.refOrganization,
				designation = model.refDesignation,
				email = model.refEmail,
				contact = model.refContact,
				Occupation = model.Occupation,
				OfficeAddress = model.OfficeAddress,
				HomeAddress = model.HomeAddress
			};

			await emergencyContactService.SaveEmergencyContact(data);
			return Json("updated");
		}
		//      [HttpPost]
		//public async Task<IActionResult> UpdateNominee([FromForm] FormViewViewModel model)
		//{
		//          Nominee data = new Nominee
		//          {
		//              Id = model.nomineeID ?? 0,
		//              employeeID = model.nomineeEmployeeID,
		//              name = model.name,
		//              relation = model.relation,
		//              address = model.address,
		//              contact = model.contact,
		//              guardianName = model.guardianName,
		//              witnessName = model.witnessName,
		//              nomineeDate = model.nomineeDate,
		//              Email = model.Email,
		//              Occupation = model.Occupation,
		//              Designation = model.Designation,
		//              Organization = model.Organization,
		//              NID = model.NID,
		//              BRN = model.BRN,
		//              witnessPhone = model.witnessPhone,

		//          };
		//          int maxId = await nomineeService.SaveNominee(data);
		//          await personalInfoService.UpdateEmployeeinfoById(model.nomineeEmployeeID);
		//          await nomineeDetailService.DeleteNomineeDetailByNomineeId(maxId);
		//          for (int i = 0; i < model.fundTypeList.Length; i++)
		//          {
		//              NomineeDetail nomineeDetail = new NomineeDetail
		//              {
		//                  Id = 0,
		//                  nomineeId = maxId,
		//                  nomineeFundId = model.fundTypeList[i],
		//                  persentence = model.fundValueList[i]
		//              };
		//              await nomineeDetailService.SaveNomineeDetail(nomineeDetail);
		//          }


		//	return Json("updated");
		//}



		[HttpPost]
		public async Task<IActionResult> UpdateInsurance([FromForm] FormViewViewModel model)
		{
			EmployeeInsurance data = new EmployeeInsurance
			{
				Id = model.insuranceID ?? 0,
				employeeInfoId = model.InsuranceemployeeID,
				name = model.name,
				relation = model.relation,
				NID = model.NID,
				gender = model.gender,
				dateOfBirth = model.dateOfBirth,
				insuranceDate = model.insuranceDate
				//imageUrl = ""
			};

			await nomineeService.SaveEmployeeInsurance(data);

			return Json("updated");
		}
		[HttpPost]
		public async Task<IActionResult> UpdateEducation([FromForm] FormViewViewModel model)
		{
			EducationalQualification data = new EducationalQualification
			{
				Id = model.educationId,
				employeeId = model.EducationEmployeeID,
				institution = model.institution,
				resultId = model.resultId,
				grade = model.grade,
				passingYear = model.passingYear,
				degreeId = model.degreeId,
				organizationId = model.organizationId,
				reldegreesubjectId = model.reldegreesubjectId
			};

			await employeeInfoService.SaveEducationalQualification(data);

			return Json("updated");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateQualifications([FromForm] FormViewViewModel model)
		{
			ProfessionalQualifications data = new ProfessionalQualifications
			{
				Id = model.qualificationID ?? 0,
				employeeID = model.ProfessionalQemployeeID,
				qualificationHeadId = model.qualificationHeadId,
				subject = model.subject,
				resultId = model.result,
				instituteName = model.instituteName,
				// passingYear = Convert.ToInt32(model.passingYear),
				passingYear = model.PropassingYear,
				markGrade = model.markGrade,
			};

			await professionalQualificationsService.SaveProfessionalQualifications(data);

			return Json("updated");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateOtherQualifications([FromForm] FormViewViewModel model)
		{
			OtherQualification data = new OtherQualification
			{
				Id = model.qualificationID ?? 0,
				employeeID = (int)model.OtherQualificationemployeeID,
				otherQualificationHeadId = model.qualificationHeadId,
				subject = model.subject,
				resultId = model.result,
				instituteName = model.instituteName,
				passingYear = model.PropassingYear,
				markGrade = model.markGrade,
			};

			await otherQualificationService.SaveOtherQualification(data);

			return Json("updated");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateTraining([FromForm] FormViewViewModel model)
		{
			TraningLog data = new TraningLog
			{
				Id = model.trainingLogID,
				employeeId = model.TrainingemployeeID,
				fromDate = model.fromDate,
				toDate = model.toDate,
				trainingCategoryId = Int32.Parse(model.category),
				trainingInstituteId = Int32.Parse(model.institute),
				trainingTitle = model.trainingTitle,
				remarks = model.remarks,

			};

			await traningHistoryService.SaveTraningHistory(data);

			return Json("updated");
		}


		[HttpPost]
		public async Task<IActionResult> UpdateService([FromForm] FormViewViewModel model)
		{
			TransferLog data = new TransferLog
			{
				Id = model.transfarID,
				// employeeId = Int32.Parse(model.employeeID),
				employeeId = model.TLogemployeeID,
				workStation = model.workStation,
				departmentId = model.departmentId,
				designatioId = model.designationId,
				from = model.fromDate,
				to = model.toDate,
				salaryGradeId = model.TLgrade,
				designation = model.designation
			};

			await serviceHistoryService.SaveServiceHistory(data);
			return Json("updated");
		}


		[HttpPost]
		public async Task<IActionResult> UpdatePromotion([FromForm] FormViewViewModel model)
		{
			PromotionLog data = new PromotionLog
			{
				Id = model.promotionId,
				employeeId = model.PLogEmployeeID,
				date = model.date,
				designationNewId = model.designationNewId,
				designationOldId = model.designationOldId,
				remark = model.remark,
				goNumber = model.goNumber,
				goDate = model.goDate,
				payScaleId = Int32.Parse(model.payScale)


			};
			await promotionLogService.SavePromotionLog(data);
			return Json("updated");
		}


		[HttpPost]
		public async Task<IActionResult> UpdatePMSHistory([FromForm] FormViewViewModel model)
		{
			AcrInfo data = new AcrInfo
			{
				Id = Convert.ToInt32(model.ACRLogID),
				employeeId = Convert.ToInt32(model.ACRLogEmployeeId),
				signatoryName = model.signatoryName,
				signatoryDesg = model.signatoryDesignation,
				supervisorName = model.supervisorName,
				supervisorDesg = model.supervisorDesignation,
				startDate = model.startDate,
				endDate = model.endDate,
				remarks = model.remarks,
				year = model.year,
				advanceComment = model.advanceComment,
				score = Convert.ToInt32(model.score),
				supervisorCode = model.supervisorCode,
				signatoryCode = model.signatoryCode,
				finalStatus = model.finalStatus,
				effectiveDate = model.effectiveDate
			};

			await acrInfoService.SaveACRInfo(data);
			return Json("updated");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateDisciplinary([FromForm] FormViewViewModel model)
		{
			string fileName = "";
			if (model.goAttachment != null)
			{
				FileSave.SaveFile(out fileName, model.goAttachment, "DecActionFiles");
			}
			DisciplinaryLog data = new DisciplinaryLog
			{
				Id = Convert.ToInt32(model.disciplinaryId),
				employeeId = model.disciplinaryemployeeId,
				OffenseId = model.offenseId,
				naturalPunishmentId = Convert.ToInt32(model.punishmentId),
				punishmentDate = model.punishmentDate,
				startingDate = model.startFrom,
				endDate = model.endTo,
				goNumberWithDate = model.goNo,
				remarks = model.remarks,
				goFileURL = fileName
			};

			await disciplinaryService.SaveDisciplinaryLog(data);
			return Json("updated");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateApproval([FromForm] FormViewViewModel model)
		{
			ApprovalMaster master = new ApprovalMaster
			{
				Id = Convert.ToInt32(model.approvalMasterId),
				employeeInfoId = model.employeeInfoId,
				approvalTypeId = model.approvalTypeId,

			};
			int MasterId = await approvalService.SaveApprovalMaster(master);

			if (model.approvalMasterId > 0)
			{
				await approvalService.DeleteapprovalDetailsByApprovalMasterId(Convert.ToInt32(MasterId));
			}
			if (model.approverId != null)
			{
				if (model.approverId.Length > 0)
				{

					for (int i = 0; i < model.approverId.Length; i++)
					{
						ApprovalDetail detail = new ApprovalDetail();

						detail.Id = 0;
						detail.approvalMasterId = MasterId;
						detail.approverId = Convert.ToInt32(model.approverId[i]);
						detail.sortOrder = model.sortOrder[i];
						detail.status = model.status[i];
						detail.isDelete = model.canFinalApprover[i];

						await approvalService.SaveApprovalDetail(detail);
						detail = new ApprovalDetail();
					}

				}
			}

			return Json("updated");
		}


		[HttpPost]
		public async Task<IActionResult> UpdateLicense([FromForm] FormViewViewModel model)
		{
			DrivingLicense data = new DrivingLicense
			{
				Id = model.licenseId,
				employeeId = model.LicenseemployeeID,
				licenseNumber = model.licenseNumber,
				category = model.licenseCategory,
				placeOfIssue = model.place,
				dateOfIssue = model.dateOfIssue,
				dateOfExpair = model.dateOfExpair
			};

			await drivingLicenseService.SaveDrivingLicenseInfo(data);

			return Json("updated");
		}

		[HttpPost]
		public async Task<IActionResult> UpdatePassport([FromForm] FormViewViewModel model)
		{
			PassportDetails data = new PassportDetails
			{
				Id = model.passportId,
				employeeId = model.PassportemployeeID,
				nameInPassport = model.nameInPassport,
				passportNumber = model.passPortNumber,
				placeOfIssue = model.place,
				dateOfIssue = model.dateOfIssue,
				dateOfExpair = model.dateOfExpair,
				// attachmentUrl = fileName
			};

			await passportInfoService.SavePassportInfo(data);

			return Json("updated");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateTravel([FromForm] FormViewViewModel model)
		{
			TraveInfo data = new TraveInfo
			{
				Id = model.travelId,
				employeeId = model.TraveEmployeeID,
				travelPurposeId = Int32.Parse(model.type),
				titleName = model.titleName,
				location = model.location,
				countryId = Int32.Parse(model.country),
				sponsoringAgency = model.sponsoringAgency,
				startDate = model.startDate,
				endDate = model.endDate,
				goNumber = model.goNumber,
				goDate = model.goDate,
				file = model.file,
				titleOfFile = model.titleOfFile,
				remarks = model.remarks,
				accountCode = model.accountCode,
				hrProgramId = model.hrProgramId,
				projectId = model.projectId,
				purpose = model.purpose,
				leaveStartDate = model.leaveStartDate,
				leaveEndDate = model.leaveEndDate


			};

			await travelInfoService.SaveTraveInfo(data);

			return Json("updated");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateMembership([FromForm] FormViewViewModel model)
		{
			EmployeeMembership data = new EmployeeMembership
			{
				Id = model.membershipId,
				employeeId = model.MemberemployeeID,
				nameOrganization = model.nameOrganization,
				membershipNo = model.membershipNo,
				membershipId = Int32.Parse(model.membershipType),
				remarks = model.remarks

			};

			await membershipService.SaveMembershipInfo(data);

			return Json("updated");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateAward([FromForm] FormViewViewModel model)
		{
			EmployeeAward data = new EmployeeAward
			{
				Id = model.awardId,
				employeeId = model.AwardemployeeID,
				awardId = (int)model.awardIdName,
				purpose = model.perpose,
				awardDate = model.txtAwardDate,
				//  url = fileName

			};

			await awardPublicationService.SaveAward(data);

			return Json("updated");
		}


		[HttpPost]
		public async Task<IActionResult> UpdatePublication([FromForm] FormViewViewModel model)
		{
			HRPMS.Data.Entity.Employee.Publication data = new HRPMS.Data.Entity.Employee.Publication
			{
				Id = model.publicationId,
				employeeId = model.PublicationemployeeID,
				publicationName = model.publicationName,
				publicationType = model.publicationType,
				publicationPlace = model.publicationPlace,
				publicationDate = model.publicationDate

			};

			await awardPublicationService.SavePublication(data);
			return Json("updated");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateLanguage([FromForm] FormViewViewModel model)
		{
			EmployeeLanguage data = new EmployeeLanguage
			{
				Id = model.languageId,
				employeeId = model.LanguageemployeeID,
				reading = model.reading,
				writing = model.writing,
				speaking = model.speaking,
				languageId = Int32.Parse(model.language),
				proficiency = model.proficiency

			};

			await awardPublicationService.SaveLanguage(data);
			return Json("updated");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateOtherActivity([FromForm] FormViewViewModel model)
		{
			OtherActivity data = new OtherActivity
			{
				Id = model.activityID ?? 0,
				employeeID = (int)model.OtherActemployeeID,
				activityNameId = model.activityName,
				description = model.description
			};

			await otherActivityService.SaveOtherActivity(data);
			return Json("updated");
		}
		[HttpPost]
		public async Task<IActionResult> UpdateBankInfo([FromForm] FormViewViewModel model)
		{
			BankInfo data = new BankInfo
			{
				Id = model.bankInfoId,
				employeeId = model.BankemployeeID,
				walletTypeId = model.walletTypeId,
				bankId = model.bankId,
				branchName = model.branchName,
				accountNumber = model.accountNumber,
				walletNumber = model.walletNumber,
				ibus = model.ibus
			};

			await bankInfoService.SaveBankInfo(data);
			return Json("updated");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateBelongings([FromForm] FormViewViewModel model)
		{
			Belongings data = new Belongings
			{
				Id = model.belongingsItemID ?? 0,
				employeeId = (int)model.BelongemployeeID,
				belongingItemId = model.belongingId,
				assetNo = model.assetNo,
				itemSpecificationId = model.specificationID,
				description = model.remarks,
				issueDate = model.issueDate,
				returnDate = model.returnDate
			};

			await belongingsService.SaveBelongings(data);
			return Json("updated");
		}

		[HttpPost]
		public async Task<IActionResult> UpdatePreviousEmployment([FromForm] FormViewViewModel model)
		{
			PriviousEmployment data = new PriviousEmployment
			{
				Id = model.previousEmploymentID ?? 0,
				employeeID = (int)model.EmploymentemployeeID,
				organizationTypeId = model.PEorganizationType,
				companyName = model.companyName,
				position = model.position,
				department = model.PEdepartment,
				companyBusiness = model.companyBusiness,
				startDate = model.startDate,
				endDate = model.endDate,
				companyLocation = model.companyLocation,
				responsibilities = model.responsibilities
			};

			await priviousEmploymentService.SavePriviousEmployment(data);
			return Json("updated");
		}


		[HttpPost]
		public async Task<IActionResult> UpdateFreedomFighter([FromForm] FormViewViewModel model)
		{
			FreedomFighter data = new FreedomFighter
			{
				Id = model.FFID ?? 0,
				employeeID = (int)model.FreedomemployeeID,
				number = model.ffNo,
				owner = model.owner,
				relationship = model.relationShip,
				sectorNo = model.sectorNo,
				remarks = model.remark
			};

			await freedomFighterService.SaveFreedomFighter(data);
			return Json("updated");
		}
		[HttpPost]
		public async Task<IActionResult> UpdateReference([FromForm] FormViewViewModel model)
		{
			HRPMS.Data.Entity.Employee.Reference data = new HRPMS.Data.Entity.Employee.Reference
			{
				Id = model.refID,
				employeeID = (int)model.ReferenceemployeeID,
				name = model.refName,
				organization = model.refOrganization,
				designation = model.refDesignation,
				email = model.refEmail,
				contact = model.refContact
			};

			await referenceService.SaveReference(data);
			return Json("updated");
		}


		[HttpPost]
		public async Task<IActionResult> UpdateOfficeAssign([FromForm] FormViewViewModel model)
		{
			OfficeAssign data = new OfficeAssign
			{
				Id = model.officeAssignID ?? 0,
				employeeInfoId = (int)model.OfficeAssignemployeeID,
				roomNo = model.roomNo,
				floorNo = model.floorNo,
				deskNo = model.deskNo,

			};

			await officeAssignService.SaveofficeAssign(data);

			return Json("updated");
		}
		[HttpPost]
		public async Task<IActionResult> UpdateEmployeeContract([FromForm] FormViewViewModel model)
		{
			EmployeeContractInfo data = new EmployeeContractInfo
			{
				Id = Convert.ToInt32(model.contractemployeeID),
				employeeId = model.EmpcontractId,
				contractStartDate = model.ContractStartDate,
				contractEndDate = model.ContractEndDate,
				isDelete = model.isDelete,
				remarks = model.remarks,
			};

			await employeeContratInfoService.SaveContractInfo(data);
			return Json("updated");
		}


		[HttpPost]
		public async Task<IActionResult> UpdateProjectActivity([FromForm] FormViewViewModel model)
		{
			EmployeeProjectActivity data = new EmployeeProjectActivity
			{
				Id = (int)model.ProjectActivityid,
				employeeId = model.ProjectActivityemployeeId,
				hRDonerId = model.hRDonerId,
				hRProjectId = model.hRProjectId,
				hRActivityId = model.hRActivityId,
				isActive = model.isActive
			};
			await employeeProjectActivityService.SaveEmployeeProjectActivity(data);
			return Json("updated");
		}
		[HttpPost]
		public async Task<IActionResult> UpdateFinanceCode([FromForm] FormViewViewModel model)
		{
			FinanceCode data = new FinanceCode
			{
				Id = (int)model.fnCodeId,
				employeeId = model.BankemployeeID,
				fnCode = model.fnCode,
			};

			await financeCodeService.SaveFinanceCode(data);
			return Json("updated");
		}
		[HttpPost]
		public async Task<IActionResult> UpdateAttachment([FromForm] FormViewViewModel model)
		{
			HRPMSAttachment data = new HRPMSAttachment
			{
				employeeId = model.AttachmentemployeeId,
				atttachmentCategoryId = model.atttachmentCategoryId,
				fileTitle = model.fileTitle,
				// fileUrl = fileNameMain,
				remarks = model.remarks,
				attachmentDate = model.attachmentDate

			};

			await hRPMSAttachmentService.SaveHRPMSAttachment(data);
			return Json("updated");
		}


		[HttpPost]
		public async Task<IActionResult> UpdateProjectAssign([FromForm] FormViewViewModel model)
		{
			EmployeeProjectAssign data = new EmployeeProjectAssign
			{
				Id = (int)model.ProjectActivityid,
				employeeId = model.ProjectActivityemployeeId,
				projectId = model.projectId,
				isActive = model.isActive
			};
			await employeeProjectActivityService.SaveEmployeeProjectAssign(data);
			return Json("updated");
		}
		[HttpPost]
		public async Task<IActionResult> UpdateEmployeeOtherInfo([FromForm] FormViewViewModel model)
		{
			EmployeeOtherInfo data = new EmployeeOtherInfo
			{
				Id = model.EmployeeOtherid,
				employeeInfoId = model.EmployeeOtheremployeeId,
				hRFacilityId = model.hRFacilityId,
				simsId = model.simsId,
				busArea = model.busArea,
				type = model.type
			};
			await employeeProjectActivityService.SaveEmployeeOtherInfo(data);
			return Json("updated");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateCostCentre([FromForm] FormViewViewModel model)
		{
			EmployeeCostCentre data = new EmployeeCostCentre
			{
				Id = model.EmployeeOtherid,
				employeeInfoId = model.EmployeeOtheremployeeId,
				costCentreId = model.costCentreId,
				costCentreDate = model.costCentreDate
			};
			await employeeProjectActivityService.SaveEmployeeCostCentre(data);
			return Json("updated");
		}
		[HttpPost]
		public async Task<IActionResult> UpdateEmployeeGrade([FromForm] FormViewViewModel model)
		{
			EmployeeGrade data = new EmployeeGrade
			{
				Id = model.EmployeeOtherid,
				employeeInfoId = model.EmployeeOtheremployeeId,
				salaryGradeId = model.salaryGradeId,
				effectiveDate = model.effectiveDate
			};
			await employeeProjectActivityService.SaveEmployeeGrade(data);
			return Json("updated");
		}
		[HttpPost]
		public async Task<IActionResult> UpdateIeltsInfo([FromForm] FormViewViewModel model)
		{
			IeltsInfo data = new IeltsInfo
			{
				Id = Int32.Parse(model.ieltsId),
				employeeId = Int32.Parse(model.employeeID),
				examType = model.examType,
				centerNo = model.centerNo,
				date = model.date,
				candidateNo = model.candidateNo,
				listeningScore = model.listeningScore,
				writingScore = model.writingScore,
				readingScore = model.readingScore,
				speakingScore = model.speakingScore,
				cefrScore = model.cefrScore,
				overallScore = model.overallScore,
				// attachment = fileName,
				status = 1
			};

			await personalInfoService.SaveIeltsInfo(data);
			return Json("updated");
		}


		[HttpPost]
		public async Task<IActionResult> UpdateComputerLiteracy([FromForm] FormViewViewModel model)
		{
			HrComputerLiteracy data = new HrComputerLiteracy
			{
				Id = Int32.Parse(model.LiteracyId),
				employeeId = Int32.Parse(model.ComputeremployeeID),
				subject = model.subject,
				competencyLevel = model.competencyLevel,
				training = model.training,
				diploma = model.diploma,
				status = 1,
				isActive = 1,
				remarks = model.remarks

			};

			await subjectService.SaveHrComputerLiteracy(data);
			return Json("updated");
		}

		[HttpPost]
		public async Task<IActionResult> SaveHobby(FormViewViewModel model)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);

			var count = model.hobbyName;

			foreach (var item in model.hobbyName)
			{
				var data = new EmployeeHobby
				{
					Id = Int32.Parse(model.EmployeeHobbyID),
					employeeInfoId = model.employeeInfoIdHobby,
					hobbyName = item,
					isActive = 1,
					status = 1,
					date = DateTime.Now
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
				//service for save
				await spouseChildrenService.SaveEmployeeHobby(data);
			}

			return Json("Saved");
		}


		public async Task<IActionResult> DeleteFighter(int id, int empId)
		{
			await freedomFighterService.DeleteFreedomFighterById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeleteSpouse(int id, int empId)
		{
			await spouseChildrenService.DeleteSpouseById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeleteChildren(int id, int empId)
		{
			await spouseChildrenService.DeleteChildrenById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> EmergencyContactDelete(int id, int empId)
		{
			await emergencyContactService.DeleteEmergencyContactById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}

		public async Task<IActionResult> DeleteNominee(int id, int empId)
		{
			await nomineeDetailService.DeleteNomineeDetailByNomineeId(id);
			await nomineeService.DeleteNomineeById(id);

			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}

		public async Task<IActionResult> DeleteInsurance(int id, int empId)
		{
			await nomineeService.DeleteEmployeeInsuranceById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeleteEducation(int id, int empId)
		{
			await employeeInfoService.DeleteEducationalQualificationById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeleteProfessionalQualifications(int id, int empId)
		{
			await professionalQualificationsService.DeleteProfessionalQualificationsById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}

		public async Task<IActionResult> DeleteOtherQualification(int id, int empId)
		{
			await otherQualificationService.DeleteOtherQualificationById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}

		public async Task<IActionResult> DeleteTraningHistory(int id, int empId)
		{
			await traningHistoryService.DeleteTraningHistoryById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeleteServiceHistory(int id, int empId)
		{
			await serviceHistoryService.DeleteServiceHistoryById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeletePromotionLog(int id, int empId)
		{
			await promotionLogService.DeletePromotionLogById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeletePMSHistory(int id, int empId)
		{
			await acrInfoService.DeleteAcrInfoById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}


		public async Task<IActionResult> DeleteDisciplinary(int id, int empId)
		{
			await disciplinaryService.DeleteDisciplinaryLogById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> Deleteapproval(int id, int empId)
		{
			await approvalService.DeleteapprovalDetailsById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}

		public async Task<IActionResult> DeleteDrivingLicense(int id, int empId)
		{
			await drivingLicenseService.DeleteDrivingLicenseById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeletePassportInfo(int id, int empId)
		{
			await passportInfoService.DeletePassportInfoById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeleteTraveInfo(int id, int empId)
		{
			await travelInfoService.DeleteTraveInfoById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}

		public async Task<IActionResult> DeleteMembershipInfo(int id, int empId)
		{
			await membershipService.DeleteMembershipInfoById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeleteAward(int id, int empId)
		{
			await awardPublicationService.DeleteAwardById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeletePublication(int id, int empId)
		{
			await awardPublicationService.DeletePublicationById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}

		public async Task<IActionResult> DeleteLanguage(int id, int empId)
		{
			await awardPublicationService.DeleteLanguageById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeleteOtherActivity(int id, int empId)
		{
			await otherActivityService.DeleteOtherActivityById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeleteBankInfo(int id, int empId)
		{
			await bankInfoService.DeleteBankInfoById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}

		public async Task<IActionResult> DeleteBelongings(int id, int empId)
		{
			await belongingsService.DeleteBelongingsById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeletePriviousEmployment(int id, int empId)
		{
			await priviousEmploymentService.DeletePriviousEmploymentById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeleteFreedomFighter(int id, int empId)
		{
			await freedomFighterService.DeleteFreedomFighterById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}

		public async Task<IActionResult> DeleteReference(int id, int empId)
		{
			await referenceService.DeleteReferenceById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeleteOfficeAssign(int id, int empId)
		{
			await officeAssignService.DeleteOfficeAssignById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeleteContractInfo(int id, int empId)
		{
			await employeeContratInfoService.DeletContractInfoById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}

		public async Task<IActionResult> DeleteProjectActivity(int id, int empId)
		{
			await employeeProjectActivityService.DeleteEmployeeProjectActivityById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeleteFinanceCode(int id, int empId)
		{
			await financeCodeService.DeleteFinanceCodeById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeleteAttachment(int id, int empId)
		{
			await hRPMSAttachmentService.DeleteHRPMSAttachmentById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}

		public async Task<IActionResult> DeleteProjectAssign(int id, int empId)
		{
			await employeeProjectActivityService.DeleteEmployeeProjectAssignById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeleteOtherInfo(int id, int empId)
		{
			await employeeProjectActivityService.DeleteEmployeeOtherInfoById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeleteCostCentre(int id, int empId)
		{
			await employeeProjectActivityService.DeleteEmployeeCostCentreById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}

		public async Task<IActionResult> DeleteEmployeeGrade(int id, int empId)
		{
			await employeeProjectActivityService.DeleteEmployeeGradeById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeleteIeltsFormView(int id, int empId)
		{
			await personalInfoService.DeleteIeltsById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeleteComputerLiteracy(int id, int empId)
		{
			await subjectService.DeleteComputerLiteracyById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}

		//new

		public async Task<IActionResult> DeleteDiploma(int id, int empId)
		{
			await bankInfoService.DeleteBankDiplomaById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}

		public async Task<IActionResult> DeleteTax(int id, int empId)
		{
			await taxService.DeleteTaxById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeleteDuelResidence(int id, int empId)
		{
			await taxService.DeleteDuelResidenceById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeleteEmployeeSports(int id, int empId)
		{
			await EmployeeSportsService.DeleteEmployeeSportsById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}

		public async Task<IActionResult> DeleteEmployeeRoll(int id, int empId)
		{
			await addressService.DeleteEmployeeRoleById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}


		public async Task<IActionResult> DeleteDisease(int id, int empId)
		{
			await employeeDiseaseService.DeleteEmployeeDiseaseById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeletemobileBenifit(int id, int empId)
		{
			await mobileBenifitService.DeleteMobileBenifitById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}

		public async Task<IActionResult> Deletesuspension(int id, int empId)
		{
			await suspensionReportService.DeleteSuspensionById(id);
			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeleteAllegation(int id, int empId)
		{
			await disciplineInvestigation.DeleteAllegationById(id);

			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}

		public async Task<IActionResult> DeleteCurricular(int id, int empId)
		{
			await employeeExtraCurricularService.DeleteEmployeeExtraCurricularId(id);

			return RedirectToAction("FormView", "Info", new
			{
				id = empId
			});
		}











		public async Task<IActionResult> GetDesDeptOfApprover(int id)
		{
			var model = new ApprovalViewModel
			{
				employeeInfo = await approvalService.GetEmployeeInfoById(id)
			};
			return Json(model);
		}

		// GET: Info
		public async Task<IActionResult> Index(int id)
		{
			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
			ViewBag.employeeID = id.ToString();
			var model = new EmployeeInfoViewModel
			{
				employeeID = id.ToString(),
				photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
				fLang = _lang.PerseLang("Employee/EmployeeInfoEN.json", "Employee/EmployeeInfoBN.json", Request.Cookies["lang"]),
				employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
				religions = await religionMunicipalityService.GetReligions(),
				empExpertises = await religionMunicipalityService.GetExpertise(),
				employeeTypes = await typeService.GetAllEmployeeType(),
				employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id),
				organoOrganizations = await organizationPostService.GetAllOrganizationByIds(this.GetAllIdsByOrg(user.org)),
				designations = await designationDepartmentService.GetDesignations(),
				specialBranchUnits = await specialBranchUnitService.GetSpecialBranchUnit(),
				districts = await addressService.GetAllDistrict(),
				departments = await designationDepartmentService.GetDepartment(),
				serviceStatuses = await statusService.GetServiceStatus(),
				hrPrograms = await statusService.GetHrProgram(),
				hrUnits = await statusService.GetHrUnit(),
				locations = await locationService.GetLocation(),
				functionInfos = await functionsInfoService.GetFunctionInfo(),
				visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
				divisions = await addressService.GetAllDivision(),
				parentsOccupation = await nomineeService.GetOccupation(),
				countries = await addressService.GetAllContry(),
				disabilityTypes = await statusService.GeAlltDisabilityType()

			};
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdateBasic([FromForm] EmployeeInfoViewModel model)
		{
			var empInfo = await personalInfoService.GetEmployeeInfoById(Convert.ToInt32(model.employeeID));
			empInfo.employeeCode = model.employeeCode;
			empInfo.maritalStatus = model.maritalStatus;
			empInfo.nameEnglish = model.nameEnglish;
			empInfo.bloodGroup = model.bloodGroup;
			empInfo.nameBangla = model.nameBangla;
			empInfo.bloodDonateStatus = model.bloodDonateStatus;
			empInfo.mobileNumberPersonal = model.mobileNumberPersonal;
			empInfo.telephoneResidence = model.telephoneResidence;
			empInfo.birthIdentificationNo = model.birthIdentificationNo;
			empInfo.emailAddressPersonal = model.emailAddressPersonal;
			empInfo.homeDistrict = model.homeDistrict;
			empInfo.birthPlace = model.birthPlace;
			empInfo.dateOfBirth = model.dateOfBirth;
			empInfo.tin = model.tin;
			empInfo.nationalID = model.nationalID;
			empInfo.height = model.height;
			empInfo.nationality = model.nationality;
			empInfo.weight = model.weight;
			empInfo.gender = model.gender;
			empInfo.disability = model.Cdisability;
			empInfo.disablityType = model.CdisablityType;
			empInfo.religionId = model.religion;
			empInfo.expertiseId = model.expertiseId;
			empInfo.disabilityTypeId = model.disabilityTypeId;
			empInfo.salaryStatusComment = model.salaryStatusComment;

			empInfo.motherNameEnglish = model.motherNameEnglish;
			empInfo.motherNameBangla = model.motherNameBangla;
			empInfo.motherOccupationId = model.motherOccupationId;
			empInfo.motherNationality = model.motherNationality;
			empInfo.motherNID = model.motherNID;
			empInfo.motherMobile = model.motherMobile;
			empInfo.motherEmail = model.motherEmail;
			empInfo.motherAddress = model.motherAddress;
			empInfo.motherPassportNo = model.motherPassportNo;
			empInfo.motherPassportIssueDate = model.motherPassportIssueDate;
			empInfo.motherPassportExDate = model.motherPassportExDate;

			empInfo.fatherNameEnglish = model.fatherNameEnglish;
			empInfo.fatherNameBangla = model.fatherNameBangla;
			empInfo.fatherOccupationId = model.fatherOccupationId;
			empInfo.fatherNationality = model.fatherNationality;
			empInfo.fatherNID = model.fatherNID;
			empInfo.fatherMobile = model.fatherMobile;
			empInfo.fatherEmail = model.fatherEmail;
			empInfo.fatherAddress = model.fatherAddress;
			empInfo.fatherPassportNo = model.fatherPassportNo;
			empInfo.fatherPassportIssueDate = model.fatherPassportIssueDate;
			empInfo.fatherPassportExDate = model.fatherPassportExDate;
			empInfo.PassportNo = model.PassportNo;
			empInfo.PassportIssueDate = model.PassportIssueDate;
			empInfo.PassportExDate = model.PassportExDate;
			empInfo.telephoneOffice = model.telephoneOffice;
			empInfo.pabx = model.pabx;
			empInfo.emailAddress = model.emailAddress;
			empInfo.mobileNumberOffice = model.mobileNumberOffice;
			empInfo.identificationSign = model.identificationSign;
			empInfo.disabilityTypeId = model.disabilityTypeId;


			await personalInfoService.SaveEmployeeInfo(empInfo);
			var aspnetuser = await userInfoes.getUserByEmployeeId(Convert.ToInt32(model.employeeID));
			if (aspnetuser != null)
			{
				aspnetuser.Email = model.emailAddress.ToLower();
				aspnetuser.NormalizedEmail = model.emailAddress.ToUpper();
				if (model.activityStatus != 1)
				{
					aspnetuser.isActive = 0;
				}
				else
				{
					aspnetuser.isActive = 1;
				}
				await userInfoes.UpdateAspNetUser(aspnetuser);
			}
			return RedirectToAction("Index", new { id = model.employeeID });
		}

		public async Task<IActionResult> EmploymentInformation(int id)
		{
			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
			ViewBag.employeeID = id.ToString();
			var model = new EmployeeInfoViewModel
			{
				employeeID = id.ToString(),
				photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
				fLang = _lang.PerseLang("Employee/EmployeeInfoEN.json", "Employee/EmployeeInfoBN.json", Request.Cookies["lang"]),
				employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
				religions = await religionMunicipalityService.GetReligions(),
				employeeTypes = await typeService.GetAllEmployeeType(),
				employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id),
				organoOrganizations = await organizationPostService.GetAllOrganizationByIds(this.GetAllIdsByOrg(user.org)),
				designations = await designationDepartmentService.GetDesignations(),
				specialBranchUnits = await specialBranchUnitService.GetSpecialBranchUnit(),
				districts = await addressService.GetAllDistrict(),
				departments = await designationDepartmentService.GetDepartment(),
				serviceStatuses = await statusService.GetServiceStatus(),
				salaryGrades = await statusService.GetJoiningGrades(),
				salarygrades = await statusService.GetCurrentGrades(),
				salarySlabs = await statusService.GetCurrentBasic(),
				currentBasic = await salaryService.GetBasicFromSalaryStructure(id),

				hrPrograms = await statusService.GetHrProgram(),
				hrUnits = await statusService.GetHrUnit(),
				locations = await locationService.GetLocation(),
				functionInfos = await functionsInfoService.GetFunctionInfo(),
				visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
				hrBranches = await statusService.GetHrBranch(),
				hrDivisions = await statusService.GetHrDivision(),
				divisions = await addressService.GetAllDivision(),
				CustomRoles = await customRoleService.GetCustomRole(),
				prevJobHistories = await personalInfoService.GetAllPrevJobHistoryByEmpId(id),
				employeePostingPlaces = await taxService.GetEmpPostingByEmpId(id),

			};
			return View(model);
		}

		public async Task<IActionResult> EmploymentInfo(int id)
		{
			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
			ViewBag.employeeID = id.ToString();
			var model = new EmployeeInfoViewModel
			{
				employeeID = id.ToString(),
				photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
				fLang = _lang.PerseLang("Employee/EmployeeInfoEN.json", "Employee/EmployeeInfoBN.json", Request.Cookies["lang"]),
				employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
				religions = await religionMunicipalityService.GetReligions(),
				employeeTypes = await typeService.GetAllEmployeeType(),
				employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id),
				organoOrganizations = await organizationPostService.GetAllOrganizationByIds(this.GetAllIdsByOrg(user.org)),
				designations = await designationDepartmentService.GetDesignations(),
				specialBranchUnits = await specialBranchUnitService.GetSpecialBranchUnit(),
				districts = await addressService.GetAllDistrict(),
				departments = await designationDepartmentService.GetDepartment(),
				serviceStatuses = await statusService.GetServiceStatus(),
				salaryGrades = await statusService.GetJoiningGrades(),
				salarygrades = await statusService.GetCurrentGrades(),
				salarySlabs = await statusService.GetCurrentBasic(),
				currentBasic = await salaryService.GetBasicFromSalaryStructure(id),

				hrPrograms = await statusService.GetHrProgram(),
				hrUnits = await statusService.GetHrUnit(),
				locations = await locationService.GetLocation(),
				functionInfos = await functionsInfoService.GetFunctionInfo(),
				visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
				hrBranches = await statusService.GetHrBranch(),
				hrDivisions = await statusService.GetHrDivision(),
				divisions = await addressService.GetAllDivision(),
				CustomRoles = await customRoleService.GetCustomRole(),
				prevJobHistories = await personalInfoService.GetAllPrevJobHistoryByEmpId(id),
				employeePostingPlaces = await taxService.GetEmpPostingByEmpId(id),
				jobResponsibilities = await personalInfoService.GetAllResponsibilities()
			};
			return View(model);
		}


		[HttpPost]
		public async Task<IActionResult> CreateEmp(EmployeeInfoViewModel model)
		{
			var data = await employeeInfoService.GetEmployeeById(Convert.ToInt32(model.employeeID));
			data.Id = Convert.ToInt32(model.employeeID);
			data.joiningDateGovtService = model.joiningDateGovtService;
			data.natureOfRequitment = model.natureOfRequitment;
			data.joiningDesignation = model.joiningDesignation;
			data.dateofregularity = model.dateofregularity;
			data.dateOfPermanent = model.dateOfPermanent;
			data.DateOfRetirement = model.DateOfRetirement;
			data.branchId = model.sbu;
			data.locationId = model.locationId;
			data.hrDivisionId = model.hrDivisionId;
			data.functionInfoId = model.functionInfoId;
			data.departmentId = model.department;
			data.hrUnitId = model.hrUnitId;
			data.hrBranchId = model.hrBranchId;
			data.hrProgramId = model.hrProgramId;
			data.activityStatus = model.activityStatus;
			data.salaryStatus = model.salaryStatus;
			//data.employeeTypeId = model.employeeType;
			data.employeeTypeId = model.empTypeId;
			data.contructualType = model.contructualType;
			data.employeeStatusId = model.employeeStatusId;
			data.telephoneOffice = model.telephoneOffice;
			data.pabx = model.pabx;
			data.emailAddress = model.emailAddress;
			data.mobileNumberOffice = model.mobileNumberOffice;
			data.telephoneOffice = model.telephoneOffice;
			data.lastPromotionDate = model.lastPromotionDate;
			data.prlDate = model.prlDate;
			data.refLetterNo = model.refLetterNo;
			data.lastDesignationId = model.lastDesignationId;
			data.lastSalary = model.lastSalary;
			data.prevOrganisationResp = model.prevOrganisationResp;
			data.significantAchivement = model.significantAchivement;
			data.areaOfProficiency = model.areaOfProficiency;
			data.joiningLocation = model.joiningLocation;
			data.placeOfPosting = model.placeOfPosting;
			data.probationStart = model.probationStart;
			data.probationEnd = model.probationEnd;
			data.bondDateStart = model.bondDateStart;
			data.bondDateEnd = model.bondDateEnd;
			data.lastWorkingDate = model.lastWorkingDate;
			data.fileNo = model.fileNo;
			data.presentPosting = model.presentPosting;
			data.lastTransferDate = model.lastTransferDate;
			data.qualification = model.qualification;
			data.problem = model.problem;
			data.DivisionId = model.DivisionId;
			data.customRoleId = model.role;
			data.isFestival = model.eligible;
			data.SupervisorCode = model.SupervisorCode;
			data.SupervisorName = model.SupervisorName;


			var id = await personalInfoService.SaveEmployeeInfo(data);

			if (id > 0)
			{
				var userInfo = await personalInfoService.GetUserIdByEmpId(id);
				userInfo.Email = model.emailAddress;
				userInfo.NormalizedEmail = model.emailAddress.ToUpper();
				await personalInfoService.saveUser(userInfo);
			}

			return RedirectToAction("EmploymentInformation", new { id = model.employeeID });
		}

		public async Task<IActionResult> SavePosting(EmployeeInfoViewModel model)
		{
			var empposting = new EmployeePostingPlace
			{
				Id = (int)model.postplaceId,
				employeeId = Convert.ToInt32(model.employeeID),
				placeName = model.placebdbl,
				placeNameBn = model.placebnbdbl,
				status = model.statusbdbl,
				designationId = model.designationbdbl,
				remarks = model.remarksbdbl,
				branchId = model.sbubdbl,
				departmentId = model.departmentbdbl,
				hrBranchId = model.branchbdbl,
				hrUnitId = model.unitbdbl,
				officeId = model.officebdbl,
				hrDivisionId = model.divisionbdbl,
				locationId = model.locationbdbl,
				startDate = model.startbdbl,
				endDate = model.endbdbl,
				type = model.typebdbl,
				responsibilityId = model.responsibilitybdbl
			};

			//empposting.departmentId = empposting.departmentId == 0 ? null : empposting.departmentId;
			//empposting.hrBranchId = empposting.hrBranchId == 0 ? null : empposting.hrBranchId;
			//empposting.hrDivisionId = empposting.hrDivisionId == 0 ? null : empposting.hrDivisionId;
			//empposting.officeId = empposting.officeId == 0 ? null : empposting.officeId;
			//empposting.locationId = empposting.locationId == 0 ? null : empposting.locationId;
			//empposting.hrUnitId = empposting.hrUnitId == 0 ? null : empposting.hrUnitId;

			empposting.departmentId = empposting.departmentId == 0 ? null : empposting.departmentId;
			empposting.hrBranchId = empposting.hrBranchId == 0 ? null : empposting.hrBranchId;
			empposting.hrDivisionId = empposting.hrDivisionId == 0 ? null : empposting.hrDivisionId;
			empposting.officeId = empposting.officeId == 0 ? null : empposting.officeId;
			empposting.locationId = empposting.locationId == 0 ? null : empposting.locationId;
			empposting.hrUnitId = empposting.hrUnitId == 0 ? null : empposting.hrUnitId;

			if (empposting.departmentId != null || empposting.officeId != null)
			{
				empposting.hrBranchId = 205;
			}

			if (model.branchbdbl != null)
			{
				empposting.placeNameBn = await employeePunchCardInfoService.GetPlaceNameById("branch", Convert.ToInt32(model.branchbdbl));
			}
			else if (model.departmentbdbl != null)
			{
				empposting.placeNameBn = await employeePunchCardInfoService.GetPlaceNameById("department", Convert.ToInt32(model.departmentbdbl));
			}
			else if (model.divisionbdbl != null)
			{
				empposting.placeNameBn = await employeePunchCardInfoService.GetPlaceNameById("division", Convert.ToInt32(model.divisionbdbl));
			}
			else if (model.officebdbl != null)
			{
				empposting.placeNameBn = await employeePunchCardInfoService.GetPlaceNameById("office", Convert.ToInt32(model.officebdbl));
			}
			else if (model.locationbdbl != null)
			{
				empposting.placeNameBn = await employeePunchCardInfoService.GetPlaceNameById("location", Convert.ToInt32(model.locationbdbl));
			}
			else if (model.unitbdbl != null)
			{
				empposting.placeNameBn = await employeePunchCardInfoService.GetPlaceNameById("unit", Convert.ToInt32(model.unitbdbl));
			}
			else
			{

			}

			var postid = await employeeInfoService.SaveBDBLJobHistory(empposting);

			personalInfoService.UpdateEmployeePosting(Convert.ToInt32(model.employeeID), Convert.ToInt32(model.statusbdbl), empposting.departmentId, empposting.locationId, empposting.officeId, empposting.hrDivisionId, empposting.hrUnitId, empposting.hrBranchId, postid);

			return Json("ok");
		}

		public async Task<IActionResult> DeleteBdblPosting(int id)
		{
			var data = await employeeInfoService.DeleteBDBLPosting(id);
			if (data == 1)
			{
				return Json("deleted");
			}
			else
			{
				return Json("failed");
			}
		}

		public async Task<IActionResult> GetPostingsByEmpId(int empId)
		{
			var data = await employeeInfoService.GetBdblJobInfoByEmpId(empId);
			return Json(data);
		}
		[HttpPost]
		public async Task<IActionResult> UpdateEmp(EmployeeInfoViewModel model)
		{
			var data = await employeeInfoService.GetEmployeeById(Convert.ToInt32(model.employeeID));
			data.Id = Convert.ToInt32(model.employeeID);
			data.joiningDateGovtService = model.joiningDateGovtService;
			data.natureOfRequitment = model.natureOfRequitment;
			data.joiningDesignation = model.joiningDesignation;
			data.dateofregularity = model.dateofregularity;
			data.dateOfPermanent = model.dateOfPermanent;
			data.DateOfRetirement = model.DateOfRetirement;
			data.branchId = model.sbubdbl;
			//data.locationId = model.locationId;
			//data.hrDivisionId = model.hrDivisionId;
			//data.functionInfoId = model.functionInfoId;
			// data.departmentId = model.department;
			//data.hrUnitId = model.hrUnitId;
			//data.hrBranchId = model.hrBranchId;
			data.hrProgramId = model.hrProgramId;
			data.activityStatus = model.activityStatus;
			data.salaryStatus = model.salaryStatus;
			//data.employeeTypeId = model.employeeType;
			data.employeeTypeId = model.empTypeId;
			data.contructualType = model.contructualType;
			data.employeeStatusId = model.employeeStatusId;
			//data.telephoneOffice = model.telephoneOffice;
			//data.pabx = model.pabx;
			//data.emailAddress = model.emailAddress;
			//data.mobileNumberOffice = model.mobileNumberOffice;
			data.telephoneOffice = model.telephoneOffice;
			data.lastPromotionDate = model.lastPromotionDate;
			data.prlDate = model.prlDate;
			data.refLetterNo = model.refLetterNo;
			data.lastDesignationId = model.lastDesignationId;
			data.JoinDesignationId = model.JoinDesignationId;
			data.lastSalary = model.lastSalary;
			data.prevOrganisationResp = model.prevOrganisationResp;
			data.significantAchivement = model.significantAchivement;
			data.areaOfProficiency = model.areaOfProficiency;
			data.joiningLocation = model.joiningLocation;
			data.placeOfPosting = model.placeOfPosting;
			data.probationStart = model.probationStart;
			data.probationEnd = model.probationEnd;
			data.bondDateStart = model.bondDateStart;
			data.bondDateEnd = model.bondDateEnd;
			data.lastWorkingDate = model.lastWorkingDate;
			data.fileNo = model.fileNo;
			data.presentPosting = model.presentPosting;
			data.lastTransferDate = model.lastTransferDate;
			data.qualification = model.qualification;
			data.problem = model.problem;
			data.DivisionId = model.DivisionId;
			data.customRoleId = model.role;
			data.isFestival = model.eligible;
			data.postingDate = model.postingDate;
			//data.joiningGradeId = model.joiningGradeId;
			data.joiningGradeName = model.joiningGradeName;
			data.joiningBasic = model.joiningBasic;
			data.currentGradeId = model.currentGradeId;
			data.currentBasic = model.currentBasic;
			data.isManager = model.isManager;
			data.sbAccount = model.sbAccount;
			data.seniorityLevel = model.seniorityNumber;
			data.SupervisorCode = model.SupervisorCode;
			data.currentBasic = model.currentBasic;
			data.currentGradeId = model.currentGradeId;


			var id = await personalInfoService.SaveEmployeeInfo(data);

			//var aspnetuser = await userInfoes.getUserByEmployeeId(Convert.ToInt32(model.employeeID));
			//if (aspnetuser != null)
			//{
			//	aspnetuser.Email = model.emailAddress.ToLower();
			//	aspnetuser.NormalizedEmail = model.emailAddress.ToUpper();
			//             if (model.activityStatus != 1)
			//             {
			//                 aspnetuser.isActive = 0;
			//             }
			//             else
			//             {
			//                 aspnetuser.isActive = 1;
			//             }
			//	await userInfoes.UpdateAspNetUser(aspnetuser);
			//}

			//if (model.currentBasic != null)
			//{
			//await salaryService.UpdateEmployeeStructureByBasic(Convert.ToInt32(model.employeeID), Convert.ToDecimal(model.currentBasic));
			//}

			if (model.employeeIdJH != null)
			{
				await employeeInfoService.DeleteJobHistoryByEmpId((int)model.employeeIdJH[0]);
				for (int i = 0; i < model.employeeIdJH.Length; i++)
				{
					var jobHistory = new PrevJobHistory
					{
						Id = 0,
						employeeId = model.employeeIdJH[i],
						company = model.company[i],
						position = model.position[i],
						jobstart = Convert.ToDateTime(model.jobstart[i]),
						jobend = DateTime.Now
					};
					await employeeInfoService.SaveJobHistory(jobHistory);
				}
			}


			//await employeeInfoService.DeleteBDBLJobHistoryByEmpId((int)model.employeeId[0]);
			//for (int i = 0; i < model.postingID.Length; i++)
			//{
			//	if (Convert.ToInt32(model.postingID[i]) == 0)
			//	{
			//		var empposting = new EmployeePostingPlace
			//		{
			//			Id = Convert.ToInt32(model.postingID[i]),
			//			employeeId = model.employeeIdBDBL[i],
			//			placeName = model.bdblplaceName[i],
			//			placeNameBn = model.bdblplaceNameBn[i],
			//			remarks = model.bdblremarks[i],
			//			type = Convert.ToInt32(model.bdbltype[i]),
			//			branchId = Convert.ToInt32(model.bdblSBU[i]),
			//			hrBranchId = Convert.ToInt32(model.bdblBranch[i]),
			//			departmentId = Convert.ToInt32(model.bdblDepartment[i]),
			//			hrDivisionId = Convert.ToInt32(model.bdblDivision[i]),
			//			hrUnitId = Convert.ToInt32(model.bdblUnit[i]),
			//			//locationId = Convert.ToInt32(model.bdblZone[i]),
			//			startDate = Convert.ToDateTime(model.bdblStartDatejs[i]),
			//			endDate = Convert.ToDateTime(model.bdblEndDatejs[i]),
			//			officeId = Convert.ToInt32(model.bdblOffice[i]),
			//			status = 1

			//		};
			//		empposting.departmentId = empposting.departmentId == 0 ? null : empposting.departmentId;
			//		empposting.hrBranchId = empposting.hrBranchId == 0 ? null : empposting.hrBranchId;
			//		empposting.hrDivisionId = empposting.hrDivisionId == 0 ? null : empposting.hrDivisionId;
			//		empposting.officeId = empposting.officeId == 0 ? null : empposting.officeId;
			//		empposting.locationId = empposting.locationId == 0 ? null : empposting.locationId;
			//		empposting.hrUnitId = empposting.hrUnitId == 0 ? null : empposting.hrUnitId;

			//		var postid = await employeeInfoService.SaveBDBLJobHistory(empposting);

			//		if (empposting.endDate == null)
			//		{
			//			await personalInfoService.UpdatePreviousPostingDate((int)model.employeeIdBDBL[0], postid);
			//		}
			//	}
			//}


			return RedirectToAction("EmploymentInfo", new { id = model.employeeID });
		}
		[Authorize(Roles = "admin,user")]
		public async Task<IActionResult> GridView(int id)
		{
			ViewBag.employeeID = id.ToString();
			GridViewViewModel model = new GridViewViewModel
			{
				fLang = _lang1.PerseLang("Home/HomeEN.json", "Home/HomeBN.json", Request.Cookies["lang"]),
				employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id)
			};
			return View(model);
		}

		public IActionResult PeerSearch()
		{
			return View();
		}

		//[HttpGet]
		//public async Task<IActionResult> GetAllEmpList(string empStatus)
		//{
		//    ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
		//    var roles = await _userManager.GetRolesAsync(user);

		//    if (roles.Contains("HR CRM CRO Admin") || roles.Contains("HRAdmin") || roles.Contains("SCMAdmin") || roles.Contains("admin"))
		//    {
		//        return Json(await personalInfoService.GetAllEmployeeInfo(empStatus, "ddm"));

		//    }
		//    else
		//    {
		//        return Json(await personalInfoService.GetEmployeeInfoAsQueryAbleSingle(empStatus, "ddm", user.EmpCode));
		//    }
		//}

		#region API Section
		[AllowAnonymous]
		[HttpGet]
		public async Task<IActionResult> GetEmployeeByempCode(string code)
		{
			var data = await personalInfoService.GetEmployeeIdByCode2(code);
			return Json(data);
		}



		[Route("global/api/GetApplicationByStatus")]
		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> GetApplicationByStatus()
		{
			return Json(await applicationFormService.GetApplicationFormByStatus(8));
		}

		[Route("global/api/GetEmployeeInfoBySearch/{searchKey}")]
		[HttpGet]
		public async Task<IActionResult> GetEmployeeInfoBySearch(string searchKey)
		{
			return Json(await personalInfoService.GetEmployeeInfoBySearch(searchKey));
		}
		[AllowAnonymous]
		[Route("global/api/employeeByCode/{code}")]
		[HttpGet]
		public async Task<IActionResult> employeeById(string code)
		{
			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
			return Json(await personalInfoService.GetEmployeeInfoByCodeAndOrg(code, "ddm"));
		}



		[AllowAnonymous]
		[Route("global/api/getEmployeeInfoByOrg/")]
		[HttpGet]
		public async Task<IActionResult> GetEmployeeInfoByOrg()
		{
			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
			return Json(await personalInfoService.GetEmployeeInfoByOrg("Bank"));
		}

		[AllowAnonymous]
		[Route("global/api/freeEmployeeByCode/{code}")]
		[HttpGet]
		public async Task<IActionResult> FreeEmployeeByCode(string code)
		{
			return Json(await personalInfoService.GetFreeEmployeeByCode(code));
		}

		[AllowAnonymous]
		[Route("global/api/EmployeeByCodeNew/{code}")]
		[HttpGet]
		public async Task<IActionResult> EmployeeByCodeNew(string code)
		{
			return Json(await personalInfoService.GetEmployeeIdByCode2(code));
		}

		[AllowAnonymous]
		[Route("global/api/GetEmployeeInformationByCode/{code}")]
		[HttpGet]
		public async Task<IActionResult> GetEmployeeInformationByCode(string code)
		{
			return Json(await personalInfoService.GetEmployeeInformationByCode(code));
		}





		// Newly added
		[AllowAnonymous]
		[Route("global/api/allEmployeeList/{queryString}")]
		[HttpGet]
		public async Task<IActionResult> AllEmployeeList(int queryString)
		{
			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
			var roles = await _userManager.GetRolesAsync(user);

			if (roles.Contains("HR CRM CRO Admin") || roles.Contains("HRAdmin") || roles.Contains("SCMAdmin") || roles.Contains("admin"))
			{
				return Json(await personalInfoService.GetAllEmployeeInfo(queryString, "ddm"));

			}
			else
			{
				return Json(await personalInfoService.GetEmployeeInfoSingle(queryString, "ddm", user.EmpCode));
			}

		}

		[AllowAnonymous]
		[Route("global/api/allEmpList/{queryString}")]
		[HttpGet]
		public async Task<IActionResult> AllEmpList(string queryString)
		{
			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
			var roles = await _userManager.GetRolesAsync(user);
			//var data = await personalInfoService.GetEmployeeInfoAsQueryAble(queryString, "ddm");
			//if (roles[0].ToString() == "HR CRM CRO Admin" || roles[0].ToString() == "HRAdmin" || roles[0].ToString() == "SCMAdmin")
			if (roles.Contains("HR CRM CRO Admin") || roles.Contains("HRAdmin") || roles.Contains("SCMAdmin") || roles.Contains("admin"))
			{
				return Json(await personalInfoService.GetEmployeeInfoAsQueryAble(queryString, "ddm"));

			}
			else
			{
				return Json(await personalInfoService.GetEmployeeInfoAsQueryAbleSingle(queryString, "ddm", user.EmpCode));
			}
		}

		[AllowAnonymous]
		[Route("global/api/allActiveEmpList/{queryString}")]
		[HttpGet]
		public async Task<IActionResult> AllActiveEmpList(string queryString)
		{
			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
			var roles = await _userManager.GetRolesAsync(user);
			//var data = await personalInfoService.GetEmployeeInfoAsQueryAble(queryString, "ddm");
			//if (roles[0].ToString() == "HR CRM CRO Admin" || roles[0].ToString() == "HRAdmin" || roles[0].ToString() == "SCMAdmin")
			if (roles.Contains("HR CRM CRO Admin") || roles.Contains("HRAdmin") || roles.Contains("SCMAdmin") || roles.Contains("admin"))
			{
				//return Json(await personalInfoService.GetEmployeeInfoAsQueryAble(queryString, "ddm"));
				return Json(await personalInfoService.GetActiveEmployeeInfoAsQueryAble(queryString, "ddm"));

			}
			else
			{
				return Json(await personalInfoService.GetEmployeeInfoAsQueryAbleSingle(queryString, "ddm", user.EmpCode));
			}
		}

		[AllowAnonymous]
		[Route("global/api/allInactiveEmpList/{queryString}")]
		[HttpGet]
		public async Task<IActionResult> AllInactiveEmpList(string queryString)
		{
			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
			var roles = await _userManager.GetRolesAsync(user);
			//var data = await personalInfoService.GetEmployeeInfoAsQueryAble(queryString, "ddm");
			//if (roles[0].ToString() == "HR CRM CRO Admin" || roles[0].ToString() == "HRAdmin" || roles[0].ToString() == "SCMAdmin")
			if (roles.Contains("HR CRM CRO Admin") || roles.Contains("HRAdmin") || roles.Contains("SCMAdmin") || roles.Contains("admin"))
			{
				//return Json(await personalInfoService.GetEmployeeInfoAsQueryAble(queryString, "ddm"));
				return Json(await personalInfoService.GetInactiveEmployeeInfoAsQueryAble(queryString, "ddm"));

			}
			else
			{
				return Json(await personalInfoService.GetEmployeeInfoAsQueryAbleSingle(queryString, "ddm", user.EmpCode));
			}
		}

		[AllowAnonymous]
		[Route("global/api/allEmpListForApp/{queryString}")]
		[HttpGet]
		public async Task<IActionResult> allEmpListForApp(string queryString)
		{
			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
			var roles = await _userManager.GetRolesAsync(user);
			////var data = await personalInfoService.GetEmployeeInfoAsQueryAble(queryString, "ddm");
			//if (roles[0].ToString() == "General User")
			//{
			//    return Json(await personalInfoService.GetEmployeeInfoAsQueryAbleSingle(queryString, "ddm", user.EmpCode));
			//}
			//else
			//{
			//    return Json(await personalInfoService.GetEmployeeInfoAsQueryAble(queryString, "ddm"));
			//}

			return Json(await personalInfoService.GetEmployeeInfoAsQueryAbleApprove(queryString, "ddm", user.EmpCode));

		}
		[HttpGet]
		public async Task<IActionResult> GetSalarySlabBysalaryGradeId(int salaryGradeId)
		{
			return Json(await salaryService.GetSalarySlabBysalaryGradeId(salaryGradeId));
		}

		[AllowAnonymous]
		[Route("global/api/allAvailablePosts/{orgId}")]
		[HttpGet]
		public async Task<IActionResult> allAvailablePosts(int orgId)
		{
			return Json(await employeeOrganogramService.employeeAvailablePosts(orgId));
		}
		[AllowAnonymous]
		[Route("global/api/GetAllEmployeeInfo")]
		[HttpGet]
		public async Task<IActionResult> GetAllEmployeeInfo()
		{
			return Json(await personalInfoService.GetEmployeeInfo());
		}
		[AllowAnonymous]
		[Route("global/api/GetAllEmployeeInfos")]
		[HttpGet]
		public async Task<IActionResult> GetAllEmployeeInfos()
		{
			return Json(await personalInfoService.GetAllEmployeeInfoWithPosting());
		}

        [AllowAnonymous]
        [Route("global/api/GetAllEmployeeInfosForVoucherEntry")]
        [HttpGet]
        public async Task<IActionResult> GetAllEmployeeInfosForVoucherEntry()
        {
            return Json(await personalInfoService.GetAllEmployeeInfoWithPosting());
        }

        [AllowAnonymous]
		[Route("global/api/GetAllEmployeeInfo/{id}")]
		[HttpGet]
		public async Task<IActionResult> GetEmployeeInfoById(int id)
		{
			return Json(await personalInfoService.GetEmployeeInfoById(id));
		}


		[AllowAnonymous]
		[Route("global/api/GetAllEmployeeInfoById/{id}")]
		[HttpGet]
		public async Task<IActionResult> GetAllEmployeeInfoById(int id)
		{
			return Json(await personalInfoService.GetEmployeeInfoById(id));
		}
		[AllowAnonymous]
		[Route("global/api/GetAllUserEmployeeInfo")]
		[HttpGet]
		public async Task<IActionResult> GetAllUserEmployeeInfo()
		{
			return Json(await personalInfoService.GetUserEmployeeInfo());
		}
		[HttpGet]
		public async Task<IActionResult> DeleteEmployee(int id)
		{
			await personalInfoService.DeleteEmployeeById(id);
			return Json("ok");
		}

		#endregion

		#region Recursion
		private List<int> GetAllIdsByOrg(string org)
		{
			List<int> data = new List<int>();

			if (org == "ddm")
			{
				data.Add(1);
				data.AddRange(this.GetChildIds(1));

			}
			else if (org == "ministry")
			{
				data.Add(2);
				data.AddRange(this.GetChildIds(2));
			}
			else
			{
				data.Add(0);
				data.AddRange(this.GetChildIds(0));
			}
			return data;
		}

		private List<int> GetChildIds(int id)
		{
			List<int> childs = organizationPostService.GetllChildIdsByparrentId(id);
			List<int> Temp = new List<int>();
			foreach (int myId in childs)
			{
				Temp.AddRange(GetChildIds(myId));
			}
			Temp.AddRange(childs);
			return Temp;
		}
		#endregion

		//shitAssign
		public async Task<ActionResult> ShiftAssign(int id)
		{
			var shiftMasters = await shiftGroupMasterService.GetAllShiftGroupMaster();
			ViewBag.empCode = id;
			var masterDetails = new List<ShiftGroupWithDetails>();
			foreach (var item in shiftMasters)
			{
				masterDetails.Add(new ShiftGroupWithDetails
				{
					shiftGroupMaster = item,
					isAssigned = await shiftGroupDetailService.CheckEmployeeInShiftGroup(item.Id, id),
					shiftGroupDetail = await shiftGroupDetailService.GetAllShiftGroupDetailByMasterId(item.Id)
				});
			}
			ShiftGroupDetailViewModel model = new ShiftGroupDetailViewModel
			{
				fLang = _lang2.PerseLang("MasterData/ShiftGroupDetailsEN.json", "MasterData/ShiftGroupDetailsBN.json", Request.Cookies["lang"]),
				shiftGroupDetailslist = await shiftGroupDetailService.GetAllShiftGroupDetail(),
				shiftGroupMasterslist = await shiftGroupMasterService.GetAllShiftGroupMaster(),
				shiftGroupWithDetails = masterDetails
			};
			return View(model);
		}

		public async Task<IActionResult> AssignShiftToEmployee(int empCode, int masterId)
		{
			return Json(await shiftGroupMasterService.AssignShiftToEmployeeSer(empCode, masterId));
		}

		// POST: ShiftGroupDetail/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ShiftAssign([FromForm] ShiftGroupDetailViewModel model)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);
			//return Json(model);

			if (!ModelState.IsValid)
			{
				model.fLang = _lang2.PerseLang("MasterData/ShiftGroupDetailsEN.json", "MasterData/ShiftGroupDetailsBN.json", Request.Cookies["lang"]);
				model.shiftGroupDetailslist = await shiftGroupDetailService.GetAllShiftGroupDetail();
				model.shiftGroupMasterslist = await shiftGroupMasterService.GetAllShiftGroupMaster();
				return View(model);
			}

			ShiftGroupDetail data = new ShiftGroupDetail
			{
				Id = model.shiftMasterId,
				shiftGroupMasterId = model.shiftGroupMasterId,
				weekDay = model.weekDay,
				startTime = model.startTime,
				endTime = model.endTime,
				holiday = bool.Parse(model.holiday),
				holidayType = model.holiday
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
			await shiftGroupDetailService.SaveShiftGroupDetail(data);

			return RedirectToAction(nameof(ShiftAssign));
		}


		public async Task<IActionResult> SocialMedia(int id)
		{
			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
			ViewBag.employeeID = id.ToString();
			var model = new EmployeeInfoViewModel
			{
				employeeID = id.ToString(),
				photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
				fLang = _lang.PerseLang("Employee/EmployeeInfoEN.json", "Employee/EmployeeInfoBN.json", Request.Cookies["lang"]),
				employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
				religions = await religionMunicipalityService.GetReligions(),
				employeeTypes = await typeService.GetAllEmployeeType(),
				employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id),
				organoOrganizations = await organizationPostService.GetAllOrganizationByIds(this.GetAllIdsByOrg(user.org)),
				designations = await designationDepartmentService.GetDesignations(),
				specialBranchUnits = await specialBranchUnitService.GetSpecialBranchUnit(),
				districts = await addressService.GetAllDistrict(),
				departments = await designationDepartmentService.GetDepartment(),
				serviceStatuses = await statusService.GetServiceStatus(),
				hrPrograms = await statusService.GetHrProgram(),
				hrUnits = await statusService.GetHrUnit(),
				locations = await locationService.GetLocation(),
				functionInfos = await functionsInfoService.GetFunctionInfo(),
				visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData()
			};
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> SocialMedia(EmployeeInfoViewModel model)
		{
			var data = await employeeInfoService.GetEmployeeById(Convert.ToInt32(model.employeeID));
			data.skypeId = model.skypeId;
			data.FacebookId = model.FacebookId;
			data.LinkedInId = model.LinkedInId;
			data.TwitterId = model.TwitterId;
			data.InstagramId = model.InstagramId;
			data.WhatsAppId = model.WhatsAppId;
			await personalInfoService.SaveEmployeeInfo(data);
			return RedirectToAction("SocialMedia", new { id = model.employeeID });
		}

		//public async Task<IActionResult> IeltsInfo(int id)
		//      {
		//          ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
		//          ViewBag.employeeID = id.ToString();
		//          var model = new EmployeeInfoViewModel
		//          {
		//              employeeID = id.ToString(),
		//              photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
		//              fLang = _lang.PerseLang("Employee/EmployeeInfoEN.json", "Employee/EmployeeInfoBN.json", Request.Cookies["lang"]),
		//              employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
		//              employeeTypes = await typeService.GetAllEmployeeType(),
		//              employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id),
		//              visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
		//              ieltsInfos = await personalInfoService.GetIeltsInfos()
		//          };
		//          return View(model);
		//      }

		public async Task<IActionResult> IeltsInfo(int id)
		{
			ViewBag.employeeID = id.ToString();
			var model = new IELTSViewModel
			{
				employeeID = id.ToString(),
				photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
				employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
				//employeeTypes = await typeService.GetAllEmployeeType(),
				ielts = await personalInfoService.GetIeltsInfoByEmpId(id),
				employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id),
				visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),
				//ieltsInfos = await personalInfoService.GetIeltsInfos()
			};
			return View(model);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> IeltsInfo([FromForm] IELTSViewModel model)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);

			if (!ModelState.IsValid)
			{
				ViewBag.employeeID = model.employeeID;
				model.ielts = await personalInfoService.GetIeltsInfoByEmpId(Int32.Parse(model.employeeID));
				model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(Int32.Parse(model.employeeID));
				return View(model);
			}
			string fileName;
			if (model.ieltsPhoto != null)
			{
				string message = FileSave.SaveEmpAttachmentNew(out fileName, model.ieltsPhoto);
			}
			else
			{
				fileName = await personalInfoService.GetIeltsImgUrlById(Int32.Parse(model.ieltsId));
			};
			IeltsInfo data = new IeltsInfo
			{
				Id = Int32.Parse(model.ieltsId),
				employeeId = Int32.Parse(model.employeeID),
				examType = model.examType,
				centerNo = model.centerNo,
				date = model.date,
				candidateNo = model.candidateNo,
				listeningScore = model.listeningScore,
				writingScore = model.writingScore,
				readingScore = model.readingScore,
				speakingScore = model.speakingScore,
				cefrScore = model.cefrScore,
				overallScore = model.overallScore,
				attachment = fileName,
				status = 1
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
			await personalInfoService.SaveIeltsInfo(data);
			// return RedirectToAction(nameof(IeltsInfo));
			return RedirectToAction("IeltsInfo", "Info", new
			{
				id = Int32.Parse(model.employeeID)
			});
		}
		public async Task<IActionResult> DeleteIelts(int id, int empId)
		{
			// await personalInfoService.DeleteIeltsById(id);
			//return Json(true);
			await personalInfoService.DeleteIeltsById(id);
			//return RedirectToAction(nameof(IeltsInfo));
			return RedirectToAction("IeltsInfo", "Info", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> PRL()
		{
			var model = new PensionViewModel
			{
				PRLInfos = await personalInfoService.GetPRLEmployeeList()
			};
			return View(model);
		}
		[HttpPost]
		public async Task<IActionResult> PRL([FromForm] PensionViewModel model)
		{
			var fullDate = model.banglaDate + " " + model.banglaMonth + " " + model.banglaYear;
			var data = await personalInfoService.GetEmployeeInfoById(model.EmployeeId);
			data.prlDate = model.prlDate;
			data.activityStatus = 7;
			data.PRLreportDateEn = model.PRLreportDateEn;
			data.PRLreportDateBn = fullDate;
			await personalInfoService.SaveEmployeeInfo(data);
			return RedirectToAction("PRL");
		}

		[AllowAnonymous]
		public IActionResult PRLPdf(int Id)
		{

			string scheme = Request.Scheme;
			var host = Request.Host;

			string url = scheme + "://" + host + "/HRPMSEmployee/Info/PRLView?id=" + Id;

			string fileName;
			string status = myPDF.GeneratePDF(out fileName, url);
			if (status != "done")
			{
				return Content("<h1>" + status + "</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> PRLView(int id)
		{
			var model = new EmployeeInfoViewModel
			{
				employeeInfo = await personalInfoService.GetEmployeeInfoById(id)
			};

			return View(model);
		}
		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> Pension()
		{
			var model = new PensionViewModel
			{
				PensionInfos = await personalInfoService.GetPensionEmployeeList()

			};
			//if (model.activityStatus == 7) model.activityStatus = new PRLInfos();
			return View(model);
		}
		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> appreciationletter()
		{
			var model = new AppreciationLetterViewModel
			{
				appreciationLetters = await personalInfoService.GetAppreciationLetters()

			};
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> appreciationletter([FromForm]AppreciationLetterViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			string fileName;
			if (model.imageUrl != null)
			{
				string message = FileSave.SaveEmpAttachmentNew(out fileName, model.imageUrl);
			}
			else
			{
				fileName = await personalInfoService.GetAppreciationImgUrlById(model.appreciationId);
			};

			AppreciationLetter data = new AppreciationLetter
			{
				Id = model.appreciationId,
				employeeId = model.employeeId,
				date = model.date,
				fileUrl = fileName

			};

			await personalInfoService.SaveAppreciationLetter(data);
			return RedirectToAction("appreciationletter");
		}


		[AllowAnonymous]
		public IActionResult APPreciationPDF()
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string fileName;

			string url = scheme + "://" + host + "/HRPMSEmployee/PrintViewPage/AppriciationLetter/";
			string status = myPDF.GeneratePDF1(out fileName, url);

			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}

		public async Task<IActionResult> DeleteAppretiactionLetter(int id)
		{

			await personalInfoService.DeleteAppreciationLetterById(id);
			return RedirectToAction("appreciationletter");
		}

		public async Task<IActionResult> ExperienceLetter()
		{
			var model = new ExperienceLetterViewModel
			{
				ExprienceLetterEmployeeInfos = await personalInfoService.GetExprienceLetterEmployeeList()
			};
			return View(model);
		}
		[HttpPost]
		public async Task<IActionResult> ExperienceLetter(ExperienceLetterViewModel model)
		{
			string fileName;
			if (model.ExprienceLetterPhoto != null)
			{
				string message = FileSave.SaveEmpAttachmentNew(out fileName, model.ExprienceLetterPhoto);
			}
			else
			{
				fileName = await personalInfoService.GetExperienceLetterById(model.ExpLetterId);
			};
			var data = new ExperianceLetter
			{
				Id = model.ExpLetterId,
				employeeId = model.EmployeeId,
				//letterNo = model.letterNo,
				date = model.Date,
				//fileUrl = model.ExprienceLetterPhoto,
				fileUrl = fileName
			};
			await personalInfoService.SaveExperienceLetter(data);
			return RedirectToAction("ExperienceLetter");
		}
		public async Task<IActionResult> ExperienceLetterView()
		{
			var model = new ExperienceLetterViewModel
			{
				ExprienceLetterEmployeeInfos = await personalInfoService.GetExprienceLetterEmployeeList()
			};
			return View(model);
		}
		public IActionResult ExperienceLetterPdf()
		{

			string scheme = Request.Scheme;
			var host = Request.Host;

			string url = scheme + "://" + host + "/HRPMSEmployee/Info/ExperienceLetterView";

			string fileName;
			string status = myPDF.GeneratePDF(out fileName, url);
			if (status != "done")
			{
				return Content("<h1>" + status + "</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}
		public async Task<IActionResult> DeleteExperienceLetter(int id)
		{

			await personalInfoService.DeleteExperienceLetterById(id);
			return RedirectToAction("ExperienceLetter");
		}

		//BondLetter
		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> bondLetter()
		{
			var model = new BondLetterViewModel
			{
				bondLetters = await personalInfoService.GetBondLetters()

			};
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> bondLetter([FromForm]BondLetterViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			string fileName;
			if (model.imageUrl != null)
			{
				string message = FileSave.SaveEmpAttachmentNew(out fileName, model.imageUrl);
			}
			else
			{
				fileName = await personalInfoService.GetBondFileUrlById(model.bondId);
			};

			BondLetter data = new BondLetter
			{
				Id = model.bondId,
				employeeId = model.employeeId,
				type = model.type,
				date = model.date,
				fileUrl = fileName

			};

			await personalInfoService.SaveBondLetter(data);
			return RedirectToAction("bondLetter");
		}
		[AllowAnonymous]
		public async Task<IActionResult> BondLetterView()
		{
			var model = new BondLetterViewModel
			{
				bondLetters = await personalInfoService.GetBondLetters()
			};
			return View(model);
		}
		[AllowAnonymous]
		public IActionResult bondLetterPDF()
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string fileName;

			string url = scheme + "://" + host + "/HRPMSEmployee/Info/BondLetterView";
			string status = myPDF.GeneratePDF1(out fileName, url);

			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}



		[AllowAnonymous]
		public async Task<IActionResult> DeclarationFidelityBondView()
		{
			var model = new BondLetterViewModel
			{
				bondLetters = await personalInfoService.GetBondLetters()
			};
			return View(model);
		}
		[AllowAnonymous]
		public IActionResult DeclarationFidelityBondPDF()
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string fileName;

			string url = scheme + "://" + host + "/HRPMSEmployee/Info/DeclarationFidelityBondView";
			string status = myPDF.GeneratePDF1(out fileName, url);

			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}




		[AllowAnonymous]
		public async Task<IActionResult> DeclarationSecrecyBondView()
		{
			var model = new BondLetterViewModel
			{
				bondLetters = await personalInfoService.GetBondLetters()
			};
			return View(model);
		}
		[AllowAnonymous]
		public IActionResult DeclarationSecrecyBondPDF()
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string fileName;

			string url = scheme + "://" + host + "/HRPMSEmployee/Info/DeclarationSecrecyBondView";
			string status = myPDF.GeneratePDF1(out fileName, url);

			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}

		public async Task<IActionResult> DeletebondLetter(int id)
		{

			await personalInfoService.DeleteBondLetterById(id);
			return RedirectToAction("bondLetter");
		}

		public async Task<IActionResult> SeniorityList() {
			var data = new SeniorViewModel
			{
				designations = await personalInfoService.GetAllDesignation()
			};
			return View(data);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateSeniorityList(SeniorViewModel model)
		{
			await personalInfoService.UpdateSeniorityNullByDesignation(Convert.ToInt32(model.designationIdSeniority[1]));

			for (int i = 0; i < model.empCodeSeniority.Length; i++)
			{
				var employee = new EmployeeInfo
				{
					Id = Convert.ToInt32(model.empIdSeniority[i]),
					seniorityLevel = model.seniorityNum[i] != null ? model.seniorityNum[i].ToString() : null
				};
				await personalInfoService.UpdateEmployeeNew(employee);
			}

			return RedirectToAction("SeniorityList");
		}


		public async Task<IActionResult> GetSeniorityListAPI(int designationId) {
			var data = await personalInfoService.GetSeniorityList(designationId);
			return Json(data);
		}

		[AllowAnonymous]
		public async Task<IActionResult> EmployeeMarkingReport(int employeeId)
		{
			var model = new EmployeeMarkingViewModel
			{
				companies = await eRPCompanyService.GetAllCompany(),
				markingInfoVm = await personalInfoService.GetMarkingInfosByEmpId(employeeId),
				markingEducation = await personalInfoService.GetMarkingEducationByEmpId(employeeId),
				awards = await personalInfoService.GetAllAwardsByEmpId(employeeId)
			};
			return View(model);
		}

		[AllowAnonymous]
		public IActionResult EmployeeMarkingReportPdf(int employeeId)
		{

			string scheme = Request.Scheme;
			var host = Request.Host;

			string url = scheme + "://" + host + "/HRPMSEmployee/Info/EmployeeMarkingReport?employeeId=" + employeeId;

			string fileName;
			string status = myPDF.GenerateLegalPDFCustom(out fileName, url, "MarkingReport");
			if (status != "done")
			{
				return Content("<h1>" + status + "</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}
	}
}
