using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OPUSERP.Accounting.Services.MasterData.Interfaces;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.Areas.HRPMSMasterData.Models;
using OPUSERP.Areas.HRPMSMasterData.Models.Lang;
using OPUSERP.CRM.Services.MasterData.Interfaces;
using OPUSERP.Data;
using OPUSERP.Data.Entity;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.HrComputerLiteracy;
using OPUSERP.HRPMS.Data.Entity.IELTS;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.ACR.Interfaces;
using OPUSERP.HRPMS.Services.DisciplineInvestigation.Interfaces;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.HRPMS.Services.Organogram.Interfaces;
using OPUSERP.HRPMS.Services.Recruitment.Interfaces;
using OPUSERP.HRPMS.Services.SuspensionReport.Interfaces;
using OPUSERP.HRPMS.Services.TrainingNew.Interfaces;
using OPUSERP.Payroll.Services.Salary.Interfaces;
using OPUSERP.SCM.Services.MasterData.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
	[Area("HRPMSEmployee")]
    [Authorize]
    public class FormViewController : Controller
	{
		private readonly LangGenerate<EmployeeInfoLn> _lang;
		private readonly LangGenerate<GridViewLn> _lang1;
		private readonly LangGenerate<ShiftGroupDetailsLn> _lang2;
		private readonly IEmergencyContactService emergencyContactService;
		private readonly ERPDbContext _context;

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
		private readonly IWagesPersonalInfoService wagesPersonalInfoService;
		private readonly IWagesEmployeeInfoService wagesEmployeeInfoService;
        private readonly ITrainingNewService trainingNewService;

        public object dateOfBirth { get; private set; }
		private readonly string rootPath;
		private readonly MyPDF myPDF;


		public FormViewController(IHostingEnvironment hostingEnvironment,
			IConverter converter,
			IResultService resultService,
			ERPDbContext _context,
			IQualificationHeadService qualificationHeadService,
			IProfessionalQualificationsService professionalQualificationsService,
			IEmergencyContactService emergencyContactService,
			ILevelofEducationService levelofEducationService,
            ITrainingNewService trainingNewService,

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
			 IWagesPersonalInfoService wagesPersonalInfoService,
			IEmployeeExtraCurricularService employeeExtraCurricularService
			, IApplicationFormService applicationFormService
			)
		{
			_lang = new LangGenerate<EmployeeInfoLn>(hostingEnvironment.ContentRootPath);
			_lang1 = new LangGenerate<GridViewLn>(hostingEnvironment.ContentRootPath);
			_lang2 = new LangGenerate<ShiftGroupDetailsLn>(hostingEnvironment.ContentRootPath);

			this._context = _context;
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
			this.wagesPersonalInfoService = wagesPersonalInfoService;
            this.trainingNewService = trainingNewService;
            myPDF = new MyPDF(hostingEnvironment, converter);
			rootPath = hostingEnvironment.ContentRootPath;

		}
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> FormView(int id)
		{
			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
			ViewBag.employeeID = id.ToString();
			var data = await personalInfoService.GetEmployeeInfoById(id);
			if (data == null)
			{
				data = await personalInfoService.GetEmployeeInfoByCode(user.EmpCode);
			}
			
            string desigName = data.designation;
			ViewBag.empId = id;

			var model = new FormViewViewModelNew
			{
				employeeID = id.ToString(),
				photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
				fLang = _lang.PerseLang("Employee/EmployeeInfoEN.json", "Employee/EmployeeInfoBN.json", Request.Cookies["lang"]),
				employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
				religions = await religionMunicipalityService.GetReligions(),
				employeeTypes = await typeService.GetAllEmployeeType(),
				employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id),
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
				countries = await addressService.GetAllContry(),
				thanas = await addressService.GetAllThana(),
				salaryGrades = await salaryGradeService.GetAllSalaryGrade(),
				professionalQualifications = await professionalQualificationsService.GetProfessionalQualificationsByEmpId(id),
				qualificationHeads = await qualificationHeadService.GetQualificationHead(),
				results = await resultService.GetAllResult(),
				salarygrades = await statusService.GetCurrentGrades(),
				prevJobHistories = await personalInfoService.GetAllPrevJobHistoryByEmpId(id),
				employeePostingPlaces = await taxService.GetEmpPostingByEmpId(id),
				approvalTypes = await approvalService.GetAllApprovalType(),
				permanent = await employeeInfoService.GetAddressByTypeAndEmpId(id, "permanent"),
				present = await employeeInfoService.GetAddressByTypeAndEmpId(id, "present"),
				spouses = await spouseChildrenService.GetSpouseByEmpId(id),
				spouse = await spouseChildrenService.GetSpouseByEmpId1(id),
				spOccupation = await nomineeService.GetOccupation(),
				levelofEducations = await nomineeService.GetLevelofEducation(),
				children = await spouseChildrenService.GetChildrenByEmpId(id),
				child = await spouseChildrenService.GetchildByEmpId1(id),
				choccupations = await nomineeService.GetOccupation(),
				degrees = await nomineeService.GetDegree(),
				relations = await nomineeService.GetRelation(),
				nomineeFunds = await nomineeFundService.GetNomineeFund(),
				nominees = await nomineeService.GetNomineeByEmpId(id),
				nominee = await nomineeService.GetNomineeByEmpId1(id),
				trainingCategories = await trainingService.GetTrainingCategories(),
				trainingInstitutes = await trainingService.GetTrainingInstitute(),
				traningLogs = await traningHistoryService.GetTraningHistoryByEmpId(id),
				promotionLogs = await promotionLogService.GetPromotionLogByEmpId(id),
				employeeAccounts = await personalInfoService.GetAllEmployeeAccountsByEmpId(id),
				design = await designationDepartmentService.GetDesignationIdByName(desigName),
				offenses = await disciplineInvestigation.GetAllOffense(),
				punishments = await disciplineInvestigation.GetAllPunishment(),
				disciplinaries = await disciplinaryService.GetDisciplinaryLogByEmpId(id),
				passportDetails = await passportInfoService.GetPassportInfoByEmpId(id),
				traveInfos = await travelInfoService.GetTraveInfoByEmpId(id),
				travelPurposes = await travelService.GetTravelPurposes(),
				membershipOrganization = await membershipLanguageService.GetMembershipOrganizationInfo(),
				awardlist = await awardPublicationService.GetAllAward(),
				languages = await membershipLanguageService.GetLanguageInfo(),
				bankInfos = await bankInfoService.GetBankInfoByEmpId(id),
				banks = await bankBranchService.GetAllBank(),
				walletTypes = await salaryService.GetAllWalletType(),
				references = await referenceService.GetReferenceByEmpId(id),
				employeeSignature = await photographService.GetEmployeeSignatureByEmpId(id),
				employeeHobbies = await personalInfoService.GetAllHobbiesByEmp(id),
				computerLiteracies = await subjectService.GetcomputerLiteracyByEmpId(id),
				ComputerSubjectsList = await subjectService.GetAllComputerSubject(),
				extraCurricularType = await extraCurricularTypeService.GetExtraCurricularType(),
				emergencyContacts = await emergencyContactService.GetEmergencyContactByEmpId(id),
				employeeInsurances = await nomineeService.GetEmployeeInsuranceByEmpId(id),
				taxes = await taxService.GetTaxByEmpId(id),
				employeeExtraCurriculars = await employeeExtraCurricularService.GetEmployeeExtraCurricularEmpId(id),
				bankingDiplomas = await bankInfoService.GetBankDiplomaInfoByEmpId(id),
				computerLiteracy = await subjectService.GetAllcomputerLiteracy(),
				ielts = await personalInfoService.GetIeltsInfoByEmpId(id),
				hRPMSAttachments = await hRPMSAttachmentService.GetHRPMSAttachmentByEmpId(id),
				freedomFighters = await freedomFighterService.GetFreedomFighterlistByEmpId(id),
				employeeLanguages = await awardPublicationService.GetLanguageByEmpId(id),
				awards = await awardPublicationService.GetAwardsByEmpId(id),
				employeeMemberships = await membershipService.GetMembershipInfoByEmpId(id),
				licenses = await drivingLicenseService.GetDrivingLicenseByEmpId(id),
				educationalQualifications = await employeeInfoService.GetEducationalQualificationByEmpId(id),
				mobileBenifits = await mobileBenifitService.GetMobileBenifitByEmpId(id),
				jobResponsibilities = await personalInfoService.GetAllResponsibilities(),
				disabilityTypes = await statusService.GeAlltDisabilityType(),
                indTrainingReportSPs = await trainingNewService.GetTraingInfoSPByeempId(id)
            };
			if (model.permanent == null) model.permanent = new HRPMS.Data.Entity.Employee.Address();
			if (model.present == null) model.present = new HRPMS.Data.Entity.Employee.Address();
			model.present.divisionId = model.present.divisionId ?? 0;
			model.present.districtId = model.present.districtId ?? 0;

			model.permanent.divisionId = model.permanent.divisionId ?? 0;
			model.permanent.districtId = model.permanent.districtId ?? 0;

			if (id != 0)
			{
				model.approvalMasters = await approvalService.GetApprovalMaster(Convert.ToInt32(id));
			}
			else
			{
				model.approvalMasters = await approvalService.GetAllApprovalMaster();
			}

			return View(model);

		}

		public async Task<IActionResult> RemoveEducation(int id)
		{
			var data = await personalInfoService.DeleteEducationById(id);
			return Json(data);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateEmpPersonal([FromForm] FormViewViewModelNew model)
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

            //var designation = await designationDepartmentService.GetDesignationById(Convert.ToInt32(empInfo.lastDesignationId > 0 ? empInfo.lastDesignationId : 339));
            empInfo.designation = model.designation;

            try
			{
                _context.Entry(empInfo).State = EntityState.Modified;
                _context.SaveChanges();
                //await personalInfoService.SaveEmployeeInfo(empInfo);
			}
			catch (Exception ex)
			{

			}

			if (model.emailAddress != null)
			{
				var userInfo = await personalInfoService.GetUserIdByEmpId(Convert.ToInt32(model.employeeID));
				userInfo.Email = model.emailAddress;
				userInfo.NormalizedEmail = model.emailAddress.ToUpper();
				await personalInfoService.saveUser(userInfo);
			}
			return Json("updatedEmp");

		}
		[HttpPost]
		public async Task<IActionResult> UpdateEmpF(FormViewViewModelNew model)
		{
			var data = await employeeInfoService.GetEmployeeById(Convert.ToInt32(model.employeeID));
			data.Id = Convert.ToInt32(model.employeeID);
			data.joiningDateGovtService = model.joiningDateGovtService;
			data.natureOfRequitment = model.natureOfRequitment;
			//data.joiningDesignation = model.joiningDesignation;
			data.JoinDesignationId = model.JoinDesignationId;
			data.dateofregularity = model.dateofregularity;
			data.dateOfPermanent = model.dateOfPermanent;
			data.DateOfRetirement = model.DateOfRetirement;
			data.branchId = model.sbu;
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
			data.branchId = model.sbubdbl;
			//data.telephoneOffice = model.telephoneOffice;
			//data.pabx = model.pabx;
			//data.emailAddress = model.emailAddress;
			//data.mobileNumberOffice = model.mobileNumberOffice;
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
			data.SupervisorName = model.SupervisorName;



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

			//if (model.employeeIdJH != null)
			//{
			//    await employeeInfoService.DeleteJobHistoryByEmpId((int)model.employeeIdJH[0]);
			//    for (int i = 0; i < model.employeeIdJH.Length; i++)
			//    {
			//        var jobHistory = new PrevJobHistory
			//        {
			//            Id = 0,
			//            employeeId = model.employeeIdJH[i],
			//            company = model.company[i],
			//            position = model.position[i],
			//            type = model.jobtype[i],
			//            jobstart = Convert.ToDateTime(model.jobstart[i]),
			//            jobend = Convert.ToDateTime(model.jobend[i])
			//        };
			//        await employeeInfoService.SaveJobHistory(jobHistory);
			//    }
			//}


			//await employeeInfoService.DeleteBDBLJobHistoryByEmpId((int)model.employeeId[0]);
			//for (int i = 0; i < model.postingID.Length; i++)
			//{
			//    if (Convert.ToInt32(model.postingID[i]) == 0)
			//    {
			//        var empposting = new EmployeePostingPlace
			//        {
			//            Id = Convert.ToInt32(model.postingID[i]),
			//            employeeId = model.employeeIdBDBL[i],
			//            placeName = model.bdblplaceName[i],
			//            placeNameBn = model.bdblplaceNameBn[i],
			//            remarks = model.bdblremarks[i],
			//            type = Convert.ToInt32(model.bdbltype[i]),
			//            branchId = Convert.ToInt32(model.bdblSBU[i]),
			//            hrBranchId = Convert.ToInt32(model.bdblBranch[i]),
			//            departmentId = Convert.ToInt32(model.bdblDepartment[i]),
			//            hrDivisionId = Convert.ToInt32(model.bdblDivision[i]),
			//            hrUnitId = Convert.ToInt32(model.bdblUnit[i]),
			//            //locationId = Convert.ToInt32(model.bdblZone[i]),
			//            startDate = Convert.ToDateTime(model.bdblStartDatejs[i]),
			//            endDate = Convert.ToDateTime(model.bdblEndDatejs[i]),
			//            officeId = Convert.ToInt32(model.bdblOffice[i]),
			//            status = 1

			//        };
			//        empposting.departmentId = empposting.departmentId == 0 ? null : empposting.departmentId;
			//        empposting.hrBranchId = empposting.hrBranchId == 0 ? null : empposting.hrBranchId;
			//        empposting.hrDivisionId = empposting.hrDivisionId == 0 ? null : empposting.hrDivisionId;
			//        empposting.officeId = empposting.officeId == 0 ? null : empposting.officeId;
			//        empposting.locationId = empposting.locationId == 0 ? null : empposting.locationId;
			//        empposting.hrUnitId = empposting.hrUnitId == 0 ? null : empposting.hrUnitId;

			//        var postid = await employeeInfoService.SaveBDBLJobHistory(empposting);

			//        if (empposting.endDate == null)
			//        {
			//            await personalInfoService.UpdatePreviousPostingDate((int)model.employeeIdBDBL[0], postid);
			//        }
			//    }
			//}


			return Json("empUpdate");
		}

		[HttpPost]
		public async Task<IActionResult> SavePrevJobHistory(FormViewViewModelNew model)
		{
			var data = new PrevJobHistory
			{
				Id = Convert.ToInt32(model.idpv),
				type = model.typepv,
				jobstart = model.startdatepv,
				jobend = model.enddatepv,
				position = model.positionpv,
				company = model.namepv,
				employeeId = model.empidpv
			};
			await employeeInfoService.SaveJobHistory(data);
			return Json("ok");
		}

		public async Task<IActionResult> GetPrevJobHistoryById(int empId)
		{
			var data = await employeeInfoService.getAllPreviousJobHistory(empId);
			return Json(data);
		}


		public async Task<IActionResult> SavePosting(FormViewViewModelNew model)
		{
			var empposting = new EmployeePostingPlace
			{
				Id = (int)model.postplaceId,
				employeeId = Convert.ToInt32(model.employeeID),
				placeName = model.placebdbl,
				placeNameBn = model.placebnbdbl,
				status = model.statusbdbl,
				remarks = model.remarksbdbl,
				branchId = model.sbubdbl,
				departmentId = model.departmentbdbl,
				designationId = model.designationbdbl,
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

			empposting.departmentId = empposting.departmentId == 0 ? null : empposting.departmentId;
			empposting.hrBranchId = empposting.hrBranchId == 0 ? null : empposting.hrBranchId;
			empposting.hrDivisionId = empposting.hrDivisionId == 0 ? null : empposting.hrDivisionId;
			empposting.officeId = empposting.officeId == 0 ? null : empposting.officeId;
			empposting.locationId = empposting.locationId == 0 ? null : empposting.locationId;
			empposting.hrUnitId = empposting.hrUnitId == 0 ? null : empposting.hrUnitId;

			if (empposting.departmentId != null || empposting.officeId != null || empposting.hrUnitId != null || empposting.hrDivisionId != null)
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
		//Approval
		[HttpGet]

		public async Task<IActionResult> GetDesDeptOfApprover(int id)
		{
			var model = new ApprovalViewModel
			{
				employeeInfo = await approvalService.GetEmployeeInfoById(id)
			};
			return Json(model);
		}
		[HttpPost]
		public async Task<IActionResult> SaveApproval([FromForm] FormViewViewModelNew model)
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

			return Json("Lapproval");
		}

		//Address
		public async Task<IActionResult> WagesIndex(int id)
		{
			ViewBag.employeeID = id.ToString();
			var model = new AddressViewModel
			{
				employeeID = id,
				wagesPermanent = await wagesEmployeeInfoService.GetAddressByTypeAndEmpId(id, "permanent"),
				wagesPresent = await wagesEmployeeInfoService.GetAddressByTypeAndEmpId(id, "present"),
				employeeNameCode = await wagesPersonalInfoService.GetEmployeeNameCodeById(id)
			};
			if (model.wagesPermanent == null) model.wagesPermanent = new HRPMS.Data.Entity.Employee.WagesAddress();
			if (model.wagesPresent == null) model.wagesPresent = new HRPMS.Data.Entity.Employee.WagesAddress();
			model.wagesPresent.divisionId = model.wagesPresent.divisionId ?? 0;
			model.wagesPresent.districtId = model.wagesPresent.districtId ?? 0;

			model.wagesPermanent.divisionId = model.wagesPermanent.divisionId ?? 0;
			model.wagesPermanent.districtId = model.wagesPermanent.districtId ?? 0;

			return View(model);
		}

		public async Task<IActionResult> DeletePrevJobHistory(int id)
		{
			var data = await employeeInfoService.DeletePrevJobHistoryById(id);
			return Json("deleted");
		}

		[HttpPost]
		public async Task<IActionResult> SaveAddress([FromForm] FormViewViewModelNew model)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);

			if (!ModelState.IsValid)
			{
				ViewBag.employeeID = model.employeeID;
				model.present = await employeeInfoService.GetAddressByTypeAndEmpId(Int32.Parse(model.employeeID), "present");
				model.permanent = await employeeInfoService.GetAddressByTypeAndEmpId(Int32.Parse(model.employeeID), "permanent");
				if (model.permanent == null) model.permanent = new HRPMS.Data.Entity.Employee.Address();
				if (model.present == null) model.present = new HRPMS.Data.Entity.Employee.Address();

				model.present.divisionId = model.present.divisionId ?? 0;
				model.present.districtId = model.present.districtId ?? 0;
				model.permanent.divisionId = model.permanent.divisionId ?? 0;
				model.permanent.districtId = model.permanent.districtId ?? 0;

				return View(model);
			}

			HRPMS.Data.Entity.Employee.Address presentdata = new HRPMS.Data.Entity.Employee.Address
			{
				Id = model.presentAddressID,
				employeeId = Int32.Parse(model.employeeID),
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
			if (roles.Contains("HRAdmin") || roles.Contains("admin"))
			{
				presentdata.isDelete = 0;
			}
			else
			{
				presentdata.isDelete = 1;
				//await employeeInfoService.ChangeEmployeeActivityStatus(model.employeeID, 3);
			}
			await employeeInfoService.SaveAddress(presentdata);

			HRPMS.Data.Entity.Employee.Address permanentdata = new HRPMS.Data.Entity.Employee.Address
			{
				Id = model.permanentAddressID,
				employeeId = Int32.Parse(model.employeeID),
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
				prCountry = model.prCountry,
				prNo = model.prNo,
				type = "permanent"
			};
			if (roles.Contains("HRAdmin") || roles.Contains("admin"))
			{
				permanentdata.isDelete = 0;
			}
			else
			{
				permanentdata.isDelete = 1;
			}
			await employeeInfoService.SaveAddress(permanentdata);
			return Json("address");
		}
		[HttpPost]
		public async Task<IActionResult> WagesIndex([FromForm] FormViewViewModelNew model)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.employeeID = model.employeeID;
				model.wagesPresent = await wagesEmployeeInfoService.GetAddressByTypeAndEmpId(Int32.Parse(model.employeeID), "present");
				model.wagesPermanent = await wagesEmployeeInfoService.GetAddressByTypeAndEmpId(Int32.Parse(model.employeeID), "permanent");
				if (model.wagesPermanent == null) model.wagesPermanent = new HRPMS.Data.Entity.Employee.WagesAddress();
				if (model.wagesPresent == null) model.wagesPresent = new HRPMS.Data.Entity.Employee.WagesAddress();
				model.wagesPresent.divisionId = model.wagesPresent.divisionId ?? 0;
				model.wagesPresent.districtId = model.wagesPresent.districtId ?? 0;

				model.wagesPermanent.divisionId = model.wagesPermanent.divisionId ?? 0;
				model.wagesPermanent.districtId = model.wagesPermanent.districtId ?? 0;

				return View(model);
			}

			HRPMS.Data.Entity.Employee.WagesAddress presentdata = new HRPMS.Data.Entity.Employee.WagesAddress
			{
				Id = model.presentAddressID,
				employeeId = Int32.Parse(model.employeeID),
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
			await wagesEmployeeInfoService.SaveAddress(presentdata);

			HRPMS.Data.Entity.Employee.WagesAddress permanentdata = new HRPMS.Data.Entity.Employee.WagesAddress
			{
				Id = model.permanentAddressID,
				employeeId = Int32.Parse(model.employeeID),
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
			await wagesEmployeeInfoService.SaveAddress(permanentdata);
			await wagesPersonalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));
			return RedirectToAction(nameof(WagesIndex));
		}
		//Spouse
		[HttpPost]
		public async Task<IActionResult> SaveSpouse([FromForm] FormViewViewModelNew model)
		{

			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);
			var errors = ModelState
   .Where(x => x.Value.Errors.Count > 0)
   .Select(x => new { x.Key, x.Value.Errors })
   .ToArray();
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
			//string fileName;
			//if (model.spousePhoto != null)
			//{
			//    string message = FileSave.SaveImageNew(out fileName, model.spousePhoto);
			//}
			//else
			//{
			//    fileName = await spouseChildrenService.GetSpouseImgUrlById(Int32.Parse(model.spouseID));
			//};

			//string marriagefile;
			//if (model.MarriagePhoto != null)
			//{
			//    string message = FileSave.SaveImageNew(out marriagefile, model.MarriagePhoto);

			//}
			//else
			//{
			//    marriagefile = await spouseChildrenService.GetSpouseMarriageImgUrlById(Int32.Parse(model.spouseID));


			//};
			Spouse data = new Spouse
			{
				Id = Int32.Parse(model.spouseID),
				employeeId = Int32.Parse(model.employeeID),
				spouseName = model.spouseName,
				nationality = model.nationalitysp,
				spouseNameBN = model.spouseNameBN,
				bloodGroup = model.bloodGroupsp,
				contact = model.contact,
				relationship = model.relationship,
				gender = model.gendersp,
				email = model.emailAddresssp,
				dateOfBirth = model.dateOfBirthsp,
				occupationId = model.occupationId,
				nid = model.nid,
				organization = model.organization,
				maritalStatus = model.maritalstatusSpouse,
				designation = model.designationsp,
				imageUrl = "",
				marriageCertificate = "",
				bin = model.bin,
				highestEducation = model.higherEducation,
				homeDistrict = model.homeDistrictsp,
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
			var Id = await spouseChildrenService.SaveSpouse(data);


			//await photographService.SavePhotograph(data);

			await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));
			// return RedirectToAction("Index" , new { id=data.Id});
			// return RedirectToAction(nameof(Index));
			return Json("spouse-" + Id.ToString());
		}
		[HttpPost]
		public async Task<JsonResult> UpdateSpouseFile(int? id)
		{
			var files = Request.Form.Files;
			var Spouse = await spouseChildrenService.GetSpouseById((int)id);
			if (Request.Form.Files.Count > 0)
			{
				for (int i = 0; i < Request.Form.Files.Count; i++)
				{
					int _min = 10000;
					int _max = 99999;
					Random _rdm = new Random();
					int rnd = _rdm.Next(_min, _max);

					string filePath = string.Empty;
					string filePath2 = string.Empty;
					string fileName = string.Empty;
					string fileName2 = string.Empty;
					string fileType = string.Empty;
					string fileType2 = string.Empty;

					IFormFile file = Request.Form.Files[i];
					fileType = file.ContentType;
					fileType2 = file.ContentType;
					fileName = rnd + file.FileName;
					fileName2 = rnd + file.FileName;
					filePath = "wwwroot/EmpImages/" + fileName;
					filePath2 = "wwwroot/EmpImages/" + fileName2;
					//var fileD = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Attachments\\RoutingFile", fileName);
					var fileD = Path.Combine(Directory.GetCurrentDirectory(), filePath);
					var fileM = Path.Combine(Directory.GetCurrentDirectory(), filePath2);
					if (file.Name == "spousePhoto")
					{
						using (var fileSrteam = new FileStream(fileD, FileMode.Create))
						{
							await file.CopyToAsync(fileSrteam);
						}
						//var Spouse = await spouseChildrenService.GetSpouseById((int)id);
						Spouse.imageUrl = fileName;

						await spouseChildrenService.SaveSpouse(Spouse);
					}
					if (file.Name == "MarriagePhoto")
					{
						using (var fileSrteam = new FileStream(fileM, FileMode.Create))
						{
							await file.CopyToAsync(fileSrteam);
						}

						Spouse.marriageCertificate = fileName2;

						await spouseChildrenService.SaveSpouse(Spouse);
					}
				}
			}
			return Json(Spouse.employeeId);
		}
		//Children
		[HttpPost]
		public async Task<IActionResult> SaveChildren([FromForm] FormViewViewModelNew model)
		{
			try
			{
				string userName = HttpContext.User.Identity.Name;
				var user = await _userManager.FindByNameAsync(userName);
				var roles = await _userManager.GetRolesAsync(user);
				if (ModelState.IsValid)
				{
					//go on as normal
				}
				else
				{
					var errors = ModelState.Select(x => x.Value.Errors)
										   .Where(y => y.Count > 0)
										   .ToList();
				}


				//if (!ModelState.IsValid)
				//{
				//    ViewBag.employeeID = model.employeeID;
				//    model.children = await spouseChildrenService.GetChildrenByEmpId(Int32.Parse(model.childrenID));
				//    return View(model);
				//}
				//string fileName;

				//if (model.childrenPhoto != null)
				//{
				//    string message = FileSave.SaveImageNew(out fileName, model.childrenPhoto);
				//}
				//else
				//{
				//    fileName = await spouseChildrenService.GetChildrenImgUrlById(Int32.Parse(model.childrenID));
				//};

				Children data = new Children
				{
					Id = Int32.Parse(model.childrenID),
					employeeId = Int32.Parse(model.employeeID),
					childName = model.childName,
					childNameBN = model.childNameBN,
					dateOfBirth = model.dateOfBirthcd,
					education = model.education,
					occupationId = model.occupationcd,
					gender = model.gendercd,
					designation = model.designationcd,
					organization = model.organizationcd,
					bin = model.bincd,
					nid = model.nidcd,
					bloodGroup = model.bloodGroupcd,
					disability = model.disabilitycd,
					nationality = model.nationalitycd,
					relationship = model.relationshipcd,
					emailAddressPersonal = model.emailAddressPersonalcd,
					presentEducation = model.presentEducation,
					disablityType = model.disablityTypecd,
					phone = model.contactcd,
					childNo = model.childNo,
					maritalstatus = model.maritalstatuscd,

					imageUrl = ""
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

				var chilId = await spouseChildrenService.SaveChildren(data);
				await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));
				if (model.childEduId != null)
				{
					for (int i = 0; i < model.childEduId.Length; i++)
					{
						var childEdu = new ChildrenEducation
						{
							Id = model.childEduId[i],
							childrenId = chilId,
							institution = model.institution[i],
							levelOfEducationId = model.levelOfEducationId[i],
							degreeId = model.degreeId[i],
							majorSubject = model.majorSubject[i],
							passingYear = model.passingYear[i],
							resultType = model.resultType[i],
							result = model.result[i],
						};
						if (roles.Contains("HRAdmin") || roles.Contains("admin"))
						{
							childEdu.isDelete = 0;
						}
						else
						{
							childEdu.isDelete = 1;
							//await employeeInfoService.ChangeEmployeeActivityStatus(model.employeeID, 3);
						}
						await spouseChildrenService.SaveChildrenEducation(childEdu);

					}
				}
				//return Json(chilId);
				return Json("upchild-" + chilId.ToString());
			}
			catch (Exception)
			{

				throw;
			}

			//  return Json("upchild");
		}
		[HttpPost]
		public async Task<JsonResult> UpdateChildrenFile(int? id)
		{
			var files = Request.Form.Files;
			var children = await spouseChildrenService.GetChildrenById((int)id);
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


					children.imageUrl = fileName;

					await spouseChildrenService.SaveChildren(children);
				}
			}
			return Json(children.employeeId);
		}

		[HttpPost]
		public async Task<JsonResult> UpdateDrivingUrl(int? id)
		{
			var files = Request.Form.Files;
			var license = await drivingLicenseService.GetDrivingLicenseById((int)id);
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


					license.url = fileName;

					await drivingLicenseService.SaveDrivingLicenseInfo(license);
				}
			}
			return Json(license.employeeId);
		}
		//EmergencyContact
		[HttpPost]
		public async Task<IActionResult> SaveEmergencyContact([FromForm] FormViewViewModelNew model)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);

			if (!ModelState.IsValid)
			{
				ViewBag.employeeID = model.employeeID;
				model.emergencyContacts = await emergencyContactService.GetEmergencyContactByEmpId(Int32.Parse(model.employeeID));
				model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(Int32.Parse(model.employeeID));
				return View(model);
			}

			EmergencyContact data = new EmergencyContact
			{
				Id = model.refID ?? 0,
				employeeID = Int32.Parse(model.employeeID),
				name = model.refName,
				relation = model.refRelation,
				organization = model.refOrganization,
				designation = model.refDesignation,
				email = model.refEmail,
				contact = model.refContact,
				Occupation = model.EMOccupation,
				OfficeAddress = model.OfficeAddress,
				HomeAddress = model.HomeAddress
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
			await emergencyContactService.SaveEmergencyContact(data);
			return Json("emergency");
		}

		//Nominee
		[HttpPost]
		public async Task<IActionResult> SaveNominee([FromForm] FormViewViewModelNew model)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);

			//if (!ModelState.IsValid)
			//{
			//    ViewBag.employeeID = model.employeeID;
			//    model.nomineeFunds = await nomineeFundService.GetNomineeFund();
			//    model.nominees = await nomineeService.GetNomineeByEmpId(Int32.Parse(model.employeeID));
			//    model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(Int32.Parse(model.employeeID));
			//    return View(model);
			//}


			//string fileName;

			//if (model.nomineePhoto != null)
			//{
			//    string message = FileSave.SaveImageNew(out fileName, model.nomineePhoto);
			//}
			//else
			//{
			//    fileName = await personalInfoService.GetNomineeImgUrlById(model.nomineeID ?? 0);
			//};
			Nominee data = new Nominee
			{
				Id = model.nomineeID ?? 0,
				employeeID = Int32.Parse(model.employeeID),
				name = model.nameNominee,
				relation = model.relationName,
				address = model.addressNomi,
				contact = model.contactnominee,
				guardianName = model.guardianName,
				witnessName = model.witnessName,
				nomineeDate = model.nomineeDate,
				imageUrl = "",
				Email = model.EmailNominee,
				Occupation = model.OccupationNominee,
				Designation = model.DesignationNomi,
				Organization = model.OrganizationNomi,
				NID = model.NID,
				BRN = model.BRN,
				witnessPhone = model.witnessPhone,

			};
			if (roles.Contains("HRAdmin") || roles.Contains("admin"))
			{
				data.isDelete = 0;
			}
			else
			{
				data.isDelete = 1;
			}
			int maxId = await nomineeService.SaveNominee(data);
			//await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));
			//await nomineeDetailService.DeleteNomineeDetailByNomineeId(maxId);
			for (int i = 0; i < model.fundTypeList.Length; i++)
			{
				NomineeDetail nomineeDetail = new NomineeDetail
				{
					Id = 0,
					nomineeId = maxId,
					nomineeFundId = model.fundTypeList[i],
					persentence = model.fundValueList[i]
				};
				if (roles.Contains("HRAdmin") || roles.Contains("admin"))
				{
					nomineeDetail.isDelete = 0;
				}
				else
				{
					nomineeDetail.isDelete = 1;
				}
				await nomineeDetailService.SaveNomineeDetail(nomineeDetail);
			}

			return Json("nominee-" + maxId.ToString());
		}
		[HttpPost]
		public async Task<JsonResult> UpdateNomineeFile(int? id)
		{
			var files = Request.Form.Files;
			var Nominee = await nomineeService.GetNomineeById((int)id);
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


					Nominee.imageUrl = fileName;

					await nomineeService.SaveNominee(Nominee);
				}
			}
			return Json(Nominee.employeeID);
		}
		[HttpPost]
		public async Task<IActionResult> EmployeeInsurance([FromForm] FormViewViewModelNew model)
		{
			//string fileName;
			//if (model.imagePath_Challan != null)
			//{
			//    string message = FileSave.SaveEmpAttachmentNew(out fileName, model.imagePath_Challan);
			//}
			//else
			//{
			//    fileName = await nomineeService.GetNomineeImgUrlById(Convert.ToInt32(model.insuranceID));
			//};
			string userName1 = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName1);
			var roles = await _userManager.GetRolesAsync(user);

			EmployeeInsurance data = new EmployeeInsurance
			{
				Id = model.insuranceID ?? 0,
				employeeInfoId = Int32.Parse(model.employeeID),
				name = model.nameins,
				relation = model.relationins,
				NID = model.NIDins,
				gender = model.genderins,
				dateOfBirth = model.dateOfBirthins,
				insuranceDate = model.insuranceDate,
				imageUrl = ""
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
			int insuranceId = await nomineeService.SaveEmployeeInsurance(data);
			//await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));
			return Json(insuranceId);
		}
		[HttpPost]
		public async Task<JsonResult> UpdateInsuranceFile(int? id)
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

					var Ins = await nomineeService.GetInsuranceById((int)id);
					Ins.imageUrl = fileName;

					await nomineeService.SaveEmployeeInsurance(Ins);
				}
			}
			return Json(true);
		}

		[HttpPost]
		public async Task<IActionResult> SaveEducation([FromForm] FormViewViewModelNew model)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);

			EducationalQualification data = new EducationalQualification
			{
				Id = model.educationId,
				employeeId = Int32.Parse(model.employeeID),
				institution = model.institutionedu,
				resultId = model.resultId,
				grade = model.grade,
				passingYear = model.passingYearedu,
				degreeId = model.degreeIdedu,
				majorGroup = model.majorGroup,
				organizationId = model.organizationId,
				organizationName = model.organizationName,
				reldegreesubjectId = model.reldegreesubjectId
			};
			if (roles.Contains("HRAdmin") || roles.Contains("admin") || roles.Contains("UAT PIMS"))
			{
				data.isDelete = 0;
			}
			else
			{
				data.isDelete = 1;
				//await employeeInfoService.ChangeEmployeeActivityStatus(model.employeeID, 3);
			}
			await employeeInfoService.SaveEducationalQualification(data);
			//await personalInfoService.UpdateEmployeeinfoById(model.employeeId);
			return Json("edu");

		}

		[HttpPost]
		public async Task<IActionResult> SaveCertification([FromForm] FormViewViewModelNew model)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);

			ProfessionalQualifications data = new ProfessionalQualifications
			{
				Id = model.qualificationID ?? 0,
				employeeID = Int32.Parse(model.employeeID),
				qualificationHeadId = model.qualificationHeadId,
				subject = model.qualificaitonsubject,
				resultId = 4,
				instituteName = model.qinstituteName,
				certificationName = model.certificationName,
				passingYear = model.qpassingYear,
				markGrade = model.qmarkGrade,
			};
			if (roles.Contains("HRAdmin") || roles.Contains("admin") || roles.Contains("UAT PIMS"))
			{
				data.isDelete = 0;
			}
			else
			{
				data.isDelete = 1;
				//await employeeInfoService.ChangeEmployeeActivityStatus(model.employeeID, 3);
			}
			await professionalQualificationsService.SaveProfessionalQualifications(data);
			//await personalInfoService.UpdateEmployeeinfoById(model.employeeId);
			return Json("cer");

		}

		public async Task<IActionResult> DeleteCertification(int id)
		{
			var data = await professionalQualificationsService.DeleteProfessionalQualificationsById(id);
			return Json("cer");
		}

		[HttpPost]
		public async Task<IActionResult> SaveTrainingHistory([FromForm] FormViewViewModelNew model)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			TraningLog data = new TraningLog
			{
				Id = model.trainingLogID,
				employeeId = Int32.Parse(model.employeeID),
				fromDate = model.fromDate,
				toDate = model.toDate,
				trainingCategoryId = model.category,
				trainingInstituteId = model.institute == 0 ? null : model.institute,
				trainingInstituteName = model.instituteName,
				trainingTitle = model.trainingTitle,
				remarks = model.remarks,
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
			await traningHistoryService.SaveTraningHistory(data);
			//await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));
			return Json("training");
		}

		[HttpPost]
		public async Task<IActionResult> SavePromotion([FromForm] FormViewViewModelNew model)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);

			if (!ModelState.IsValid)
			{
				model.designations = await designationDepartmentService.GetDesignations();
				model.promotionLogs = await promotionLogService.GetPromotionLogByEmpId(Int32.Parse(model.employeeID));
				model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(Int32.Parse(model.employeeID));
				model.salaryGrades = await salaryGradeService.GetAllSalaryGrade();
				return View(model);
			}

			PromotionLog data = new PromotionLog
			{
				Id = Int32.Parse(model.promotionId),
				employeeId = Int32.Parse(model.employeeID),
				date = model.date,
				designationNewId = model.designationNewId,
				designationOldId = model.designationOldId,
				remark = model.remark,
				goNumber = model.goNumber,
				goDate = model.goDate,
				payScaleId = Int32.Parse(model.payScale)
				//nature = model.nature,
				//basic = model.basic,
				//rank = model.rank,

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
			await promotionLogService.SavePromotionLog(data);
			return Json("promotion");

		}

		[HttpPost]
		public async Task<ActionResult> SaveDisciplinary([FromForm] FormViewViewModelNew model)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);
			var errors = ModelState
						 .Where(x => x.Value.Errors.Count > 0)
						 .Select(x => new { x.Key, x.Value.Errors })
						 .ToArray();

			if (!ModelState.IsValid)
			{
				ViewBag.employeeID = model.employeeId;
				model.offenses = await disciplineInvestigation.GetAllOffense();
				model.punishments = await disciplineInvestigation.GetAllPunishment();
				model.disciplinaries = await disciplinaryService.GetDisciplinaryLogByEmpId(Int32.Parse(model.employeeID));
				model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(Int32.Parse(model.employeeID));
				return View(model);
			}

			string fileName = "";
			if (model.goAttachments != null)
			{
				FileSave.SaveFile(out fileName, model.goAttachments, "DecActionFiles");
			}
			DisciplinaryLog data = new DisciplinaryLog
			{
				Id = Convert.ToInt32(model.disciplinaryId),
				employeeId = Int32.Parse(model.employeeID),
				//OffenseId = model.offenseId,
				//naturalPunishmentId = Convert.ToInt32(model.punishmentId),
				offenceName = model.offenceName,
				naturalPunishmentName = model.naturalPunishmentName,
				punishmentDate = model.punishmentDate,
				startingDate = model.startFrom,
				endDate = model.endTo,
				goNumberWithDate = model.goNo,
				remarks = model.remarksdis,
				goFileURL = fileName
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
			var Id = await disciplinaryService.SaveDisciplinaryLog(data);

			return Json("displinary");
			//return Json("displinary-" + Id.ToString());

		}

		[HttpPost]
		public async Task<IActionResult> SaveLicense([FromForm] FormViewViewModelNew model)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);
			if (!ModelState.IsValid)
			{
				ViewBag.employeeID = model.employeeID;
				model.licenses = await drivingLicenseService.GetDrivingLicenseByEmpId(Int32.Parse(model.employeeID));
				return View(model);
			}
			string fileName = "";
			if (model.licenseUrl != null)
			{
				FileSave.SaveFile(out fileName, model.licenseUrl, "DecActionFilesss");
			}

			DrivingLicense data = new DrivingLicense
			{
				Id = Int32.Parse(model.licenseId),
				employeeId = Int32.Parse(model.employeeID),
				licenseNumber = model.licenseNumber,
				category = model.licenseCategory,
				placeOfIssue = model.place,
				dateOfIssue = model.dateOfIssue,
				dateOfExpair = model.dateOfExpair,
				url = fileName
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
			var licenseId = await drivingLicenseService.SaveDrivingLicense(data);
			//await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));
			//           return RedirectToAction("Index", "License", new
			//{
			//    id = model.employeeID
			//});
			return Json("license-" + licenseId.ToString());
		}


		[HttpPost]
		public async Task<IActionResult> SavePassport([FromForm] FormViewViewModelNew model)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);

			//if (!ModelState.IsValid)
			//{
			//    ViewBag.employeeID = model.employeeID;
			//    model.passportDetails = await passportInfoService.GetPassportInfoByEmpId(Int32.Parse(model.employeeID));
			//    return View(model);
			//}
			//string fileName;
			//if (model.passportPhoto != null)
			//{
			//    string message = FileSave.SaveEmpAttachmentNew(out fileName, model.passportPhoto);
			//}
			//else
			//{
			//    fileName = await personalInfoService.GetPassportImgUrlById(Int32.Parse(model.passportId));
			//};
			PassportDetails data = new PassportDetails
			{
				Id = Int32.Parse(model.passportId),
				employeeId = Int32.Parse(model.employeeID),
				nameInPassport = model.nameInPassport,
				passportNumber = model.passPortNumber,
				placeOfIssue = model.placepp,
				dateOfIssue = model.dateOfIssuepp,
				dateOfExpair = model.dateOfExpairpp,
				attachmentUrl = ""
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
			var Id = await passportInfoService.SavePassportInfo1(data);

			return Json("passport-" + Id.ToString());
		}

		[HttpPost]
		public async Task<JsonResult> UpdatePassportFile(int? id)
		{
			var files = Request.Form.Files;
			var passport = await nomineeService.GetPassportById((int)id);
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


					passport.attachmentUrl = fileName;

					await passportInfoService.SavePassportInfo1(passport);
				}
			}
			return Json(passport.employeeId);
		}

		[HttpPost]
		public async Task<IActionResult> SaveTravel([FromForm] FormViewViewModelNew model)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);

			//if (!ModelState.IsValid)
			//{
			//    ViewBag.employeeID = model.employeeID;
			//    model.traveInfos = await travelInfoService.GetTraveInfo();
			//    model.travelPurposes = await travelService.GetTravelPurposes();
			//    model.countries = await addressService.GetAllContry();
			//    model.hrPrograms = await statusService.GetHrProgram();
			//    //model.projects = await projectService.GetProjectList();
			//    return View(model);
			//}


			//string fileName = "";
			//if (model.goAttachmenttr != null)
			//{
			//    FileSave.SaveImageNew(out fileName, model.goAttachmenttr);
			//    // FileSave.SaveFile(out fileName, model.goAttachment, "DecActionFiles");
			//}
			//else
			//{
			//    fileName = model.goAttachmentExisttr;
			//}
			TraveInfo data = new TraveInfo
			{
				Id = model.travelId,
				employeeId = Int32.Parse(model.employeeID),
				travelPurposeId = Int32.Parse(model.type),
				titleName = model.titleName,
				location = model.location,
				countryId = Int32.Parse(model.country),
				sponsoringAgency = model.sponsoringAgencytr,
				startDate = model.startDate,
				endDate = model.endDate,
				goNumber = model.goNumbertr,
				goDate = model.goDatetr,
				file = "",
				titleOfFile = model.titleOfFile,
				remarks = model.remarkstr,
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
			var Id = await travelInfoService.SaveTraveInfo1(data);

			return Json("travel-" + Id.ToString());
		}
		[HttpPost]
		public async Task<JsonResult> UpdateTravelFile(int? id)
		{
			var files = Request.Form.Files;
			var travel = await nomineeService.GetTravelById((int)id);
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


					travel.file = fileName;

					await travelInfoService.SaveTraveInfo1(travel);
				}
			}
			return Json(travel.employeeId);
		}
		[HttpPost]
		public async Task<IActionResult> SaveMembership([FromForm] FormViewViewModelNew model)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);

			//if (!ModelState.IsValid)
			//{
			//    ViewBag.employeeID = model.employeeID;
			//    model.memberships = await membershipLanguageService.GetMembershipInfo();
			//    model.employeeMemberships = await membershipService.GetMembershipInfoByEmpId(Int32.Parse(model.employeeID));
			//    model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(Int32.Parse(model.employeeID));
			//    model.membershipOrganization = await membershipLanguageService.GetMembershipOrganizationInfo();

			//    // model.fLang = _lang.PerseLang("Employee/MembershipEN.json");
			//    return View(model);
			//}

			EmployeeMembership data = new EmployeeMembership
			{
				Id = model.employeeMembershipID,
				employeeId = Int32.Parse(model.employeeID),
				membershipId = Int32.Parse(model.nameOrganization),
				membershipNo = model.membershipNo,
				//membershipId = Int32.Parse(model.membershipType),
				remarks = model.remarksmm

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
			await membershipService.SaveMembershipInfo(data);
			//await personalInfoService.UpdateEmployeeinfoById(model.employeeID);
			return Json("membership");
		}

		[HttpPost]
		public async Task<IActionResult> SaveAward([FromForm] FormViewViewModelNew model)
		{
			try
			{
				string userName = HttpContext.User.Identity.Name;
				var user = await _userManager.FindByNameAsync(userName);
				var roles = await _userManager.GetRolesAsync(user);

                //if (!ModelState.IsValid)
                //{
                //    ViewBag.employeeID = model.employeeID;
                //    model.awards = await awardPublicationService.GetAwardsByEmpId(Int32.Parse(model.employeeID));
                //    model.awardlist = await awardPublicationService.GetAllAward();
                //    return View(model);
                //}


                //string fileName;
                //if (model.awardPhoto != null)
                //{
                //    string message = FileSave.SaveEmpAttachmentNew(out fileName, model.awardPhoto);
                //}
                //else
                //{
                //    fileName = await awardPublicationService.GetAwardImgUrlById(Int32.Parse(model.awardId));
                //};
                var urlPhoto = "";
                if (Convert.ToInt32(model.awardId) > 0)
                {
                    var award = await nomineeService.GetAwardById(Convert.ToInt32(model.awardId));
                    urlPhoto = award.url == null ? "" : award.url;
                }

				EmployeeAward data = new EmployeeAward
				{
					Id = Int32.Parse(model.awardId),
					employeeId = Int32.Parse(model.employeeID),
					awardId = Convert.ToInt32(model.awardd),
					purpose = model.perpose,
					awardDate = model.txtAwardDate,
                    url = urlPhoto

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

				var Id = await awardPublicationService.SaveAward1(data);
				//await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));
				return Json("award-" + Id.ToString());
			}
			catch (Exception ex)
			{

				throw;
			}

		}
		[HttpPost]
		public async Task<JsonResult> UpdateAwardFile(int? id)
		{
			var files = Request.Form.Files;
			var award = await nomineeService.GetAwardById((int)id);
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
					fileName = rnd + "_Reward_" + award.employee?.nameEnglish + "_" + award.employee.employeeCode;
					filePath = "wwwroot/EmpImages/" + fileName;
					//var fileD = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Attachments\\RoutingFile", fileName);
					var fileD = Path.Combine(Directory.GetCurrentDirectory(), filePath);
					using (var fileSrteam = new FileStream(fileD, FileMode.Create))
					{
						await file.CopyToAsync(fileSrteam);
					}

					award.url = fileName;

					await awardPublicationService.SaveAward(award);
				}
			}
			return Json(award.employeeId);
		}

		[HttpPost]
		public async Task<IActionResult> SaveLanguage([FromForm] FormViewViewModelNew model)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);

			if (!ModelState.IsValid)
			{
				ViewBag.employeeID = model.employeeID;
				model.employeeLanguages = await awardPublicationService.GetLanguageByEmpId(Int32.Parse(model.employeeID));
				model.languages = await membershipLanguageService.GetLanguageInfo();
				return View(model);
			}

			EmployeeLanguage data = new EmployeeLanguage
			{
				Id = Int32.Parse(model.languageId),
				employeeId = Int32.Parse(model.employeeID),
				reading = model.reading,
				writing = model.writing,
				speaking = model.speaking,
				listening = model.listening,
				languageId = Int32.Parse(model.language),
				proficiency = model.proficiency

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
			await awardPublicationService.SaveLanguage(data);
			//await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));
			return Json("language");
		}

		[HttpPost]
		public async Task<IActionResult> SaveAcc([FromForm] FormViewViewModelNew model)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);

			var pfemp = await personalInfoService.GetPfMemberByEmpId(Convert.ToInt32(model.employeeID));
			
			//BankInfo data = new BankInfo
			//{
			//    Id = model.bankInfoId,
			//    employeeId = Int32.Parse(model.employeeID),
			//    walletTypeId = model.walletTypeId,
			//    bankId = model.bankId,
			//    branchName = model.branchName,
			//    accountNumber = model.accountNumber,
			//    walletNumber = model.walletNumber,
			//    ibus = model.ibus
			//};


			var data = new EmployeeAccounts
			{
				Id = model.bankInfoId,
				accountType = model.accountType,
				bankId = model.bankId,
				accountNumber = model.accountNumber,
				employeeInfoId = Convert.ToInt32(model.employeeID),
				status = 1
			};

			if (pfemp != null)
			{
				data.pfMemberInfoId = pfemp.Id;
			}

			try
			{
				await bankInfoService.SaveEmployeeAccounts(data);
			}
			catch (Exception ex)
			{
				
			}
			return Json("Acc");
		}

		[HttpPost]
		public async Task<IActionResult> SaveFreedomFighter([FromForm] FormViewViewModelNew model)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);

			//if (!ModelState.IsValid)
			//{
			//    ViewBag.employeeID = model.employeeID;
			//    //model.freedomFighter = await freedomFighterService.GetFreedomFighterByEmpId(Int32.Parse(model.employeeID));
			//    model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(Int32.Parse(model.employeeID));
			//    return View(model);
			//}

			FreedomFighter data = new FreedomFighter
			{
				Id = model.FFID ?? 0,
				employeeID = Int32.Parse(model.employeeID),
				number = model.ffNo,
				owner = model.owner,
				relationship = model.relationShip,
				sectorNo = model.sectorNo,
				remarks = model.remarkff
			};
			if (roles.Contains("HRAdmin") || roles.Contains("admin"))
			{
				data.isDelete = 0;
			}
			else
			{
				data.isDelete = 1;
			}
			await freedomFighterService.SaveFreedomFighter(data);

			return Json("Freedom");
		}
		[HttpPost]
		public async Task<IActionResult> SaveReference([FromForm] FormViewViewModelNew model)
		{
			//if (!ModelState.IsValid)
			//{
			//    ViewBag.employeeID = model.employeeID;
			//    model.references = await referenceService.GetReferenceByEmpId(Int32.Parse(model.employeeID));
			//    model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(Int32.Parse(model.employeeID));
			//    return View(model);
			//}

			HRPMS.Data.Entity.Employee.Reference data = new HRPMS.Data.Entity.Employee.Reference
			{
				Id = model.refIDR ?? 0,
				employeeID = Int32.Parse(model.employeeID),
				name = model.refNameR,
				organization = model.refOrganizationR,
				designation = model.refDesignationR,
				email = model.refEmailR,
				contact = model.refContactR
			};

			await referenceService.SaveReference(data);
			//await personalInfoService.UpdateEmployeeinfoById((int)model.employeeID);
			return Json("Ref");
		}

		[HttpPost]
		public async Task<ActionResult> SaveProfilePhoto([FromForm] FormViewViewModelNew model)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);

			//string fileName;
			//string message = FileSave.SaveImage(out fileName, model.empPhoto);
			//var employeeProfile = await photographService.GetPhotographByEmpId(Int32.Parse(model.employeeID));
			//if (employeeProfile != null)
			//{
			//    model.photographID = employeeProfile.Id;
			//    return Json(model.photographID);
			//}
			//if (message == "success")
			//else
			//{
			Photograph data = new Photograph
			{
				Id = model.photographID,
				employeeId = Int32.Parse(model.employeeID),
				//url = fileName,
				url = "",
				type = "profile",
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


			var Id = await photographService.SavePhotograph(data);
			return Json(Id);
			// }

			//var q = model.queryString.Split('/');
			//var areaName = q[1];
			//var controller = q[2];
			//var action = q[3];

			//return RedirectToAction(nameof(FormView), new { id = Int32.Parse(model.employeeID) });

			//return Json(Int32.Parse(model.employeeID));
		}

		[HttpPost]
		public async Task<JsonResult> UpdateProfileFile(int? id)
		{
			var files = Request.Form.Files;
			if (Request.Form.Files.Count >= 0)
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

					//fileName = rnd + file.FileName;

					//filePath =  "wwwroot/EmpImages/" + fileName;
					var extention = Path.GetExtension(file.FileName);
					/////var fileD = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Attachments\\RoutingFile", fileName);
					//var fileD = Path.Combine(Directory.GetCurrentDirectory(), filePath);
					fileName = Path.Combine("EmpImages", DateTime.Now.Ticks + extention);
					var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);

					if (file.Name == "empPhoto")
					{
						using (var fileSrteam = new FileStream(path, FileMode.Create))
						{
							await file.CopyToAsync(fileSrteam);
						}
						var photo = await photographService.GetPhotographByEmpIdNew((int)id);
						photo.url = fileName;

						await photographService.SavePhotograph(photo);
					}

				}
			}
			return Json(true);
		}







		[HttpPost]
		public async Task<ActionResult> Signature([FromForm] FormViewViewModelNew model)
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
			var employeeSignature = await photographService.GetEmployeeSignatureByEmpId(Int32.Parse(model.employeeID));
			if (employeeSignature != null)
			{
				model.signatureid = employeeSignature.Id.ToString();
				return Json(Convert.ToInt32(model.signatureid));
			}
			else
			{
				model.signatureid = "0";
				EmployeeSignature data = new EmployeeSignature
				{
					Id = Int32.Parse(model.signatureid),
					employeeId = Int32.Parse(model.employeeID),
					url = "",
					banglaSign = ""
				};

				if (roles.Contains("HRAdmin") || roles.Contains("admin"))
				{
					data.isDelete = 0;
				}
				else
				{
					data.isDelete = 1;
				}
				var Id = await photographService.SaveEmployeeSignature(data);

				return Json(Id);
			}
		}
		[HttpPost]
		public async Task<JsonResult> UpdateSignatureFile(int? id)
		{
			var files = Request.Form.Files;
			if (Request.Form.Files.Count >= 0)
			{
				for (int i = 0; i < Request.Form.Files.Count; i++)
				{
					int _min = 10000;
					int _max = 99999;
					Random _rdm = new Random();
					int rnd = _rdm.Next(_min, _max);

					IFormFile file = Request.Form.Files[i];

					if (file.Name == "EnglishPhoto")
					{
						string filePath = string.Empty;
						string fileName = string.Empty;
						string fileType = string.Empty;

						fileType = file.ContentType;
						fileName = rnd + file.FileName;
						filePath = "wwwroot/EmpImages/" + fileName;
						//var fileD = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Attachments\\RoutingFile", fileName);
						var fileD = Path.Combine(Directory.GetCurrentDirectory(), filePath);

						using (var fileSrteam = new FileStream(fileD, FileMode.Create))
						{
							await file.CopyToAsync(fileSrteam);
						}
						var engsig = await photographService.GetEmpSignatureById((int)id);
						engsig.url = fileName;

						await photographService.SaveEmployeeSignature(engsig);
					}
					if (file.Name == "BanglaPhoto")
					{
						string filePath2 = string.Empty;
						string fileName2 = string.Empty;
						string fileType2 = string.Empty;

						fileType2 = file.ContentType;
						fileName2 = rnd + file.FileName;
						filePath2 = "wwwroot/EmpImages/" + fileName2;
						var fileM = Path.Combine(Directory.GetCurrentDirectory(), filePath2);


						using (var fileSrteam = new FileStream(fileM, FileMode.Create))
						{
							await file.CopyToAsync(fileSrteam);
						}
						var bnsig = await photographService.GetEmpSignatureById((int)id);
						bnsig.banglaSign = fileName2;

						await photographService.SaveEmployeeSignature(bnsig);
					}
				}
			}
			return Json(true);
		}


		public async Task<IActionResult> GetSignaturesByEmpId(int empId)
		{
			var data = await personalInfoService.GetSignatureByEmpId(empId);
			return Json(data);
		}
		[HttpPost]
		public async Task<IActionResult> SaveAttachment([FromForm] FormViewViewModelNew model)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);

			if (!ModelState.IsValid)
			{
				model.atttachmentCategory = await attachmentCategoryService.GetAllAttachmentCategory();
				model.hRPMSAttachments = await hRPMSAttachmentService.GetHRPMSAttachmentByEmpId(Int32.Parse(model.employeeID));
				model.atttachmentGroups = await attachmentCategoryService.GetAllAtttachmentGroup();
				return View(model);
			}

			//  string fileName = String.Empty;
			//   string fileNameMain = String.Empty;
			// string message = FileSave.SaveEmpAttachment(out fileName, model.fileUrl);
			//   string message = FileSave.SaveImageNew(out fileName, model.fileUrl);

			//string fileName;

			//if (model.fileUrl != null)
			//{
			//    string message = FileSave.SaveImageNew(out fileName, model.fileUrl);
			//}
			//else
			//{
			//    fileName = await hRPMSAttachmentService.GetAttachmentUrlById(model.attachmentId);
			//};

			//if (message == "success")
			//{
			//    fileNameMain = fileName;
			//}

			HRPMSAttachment data = new HRPMSAttachment
			{
				Id = model.attachmentId,
				employeeId = Int32.Parse(model.employeeID),
				atttachmentCategoryId = model.atttachmentCategoryId,
				fileTitle = model.fileTitle,
				fileUrl = "",
				remarks = model.remarksat,
				attachmentDate = model.attachmentDate

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
			var Id = await hRPMSAttachmentService.SaveHRPMSAttachment(data);

			//return Json(Id);
			return Json("attatch-" + Id.ToString());
		}

		[HttpPost]
		public async Task<JsonResult> UpdateAttachmentFile(int? id)
		{
			var files = Request.Form.Files;
			var attachment = await nomineeService.GetAttachmentById((int)id);
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


					attachment.fileUrl = fileName;

					await hRPMSAttachmentService.SaveHRPMSAttachment(attachment);
				}
			}
			return Json(attachment.employeeId);
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
		[HttpPost]
		public async Task<IActionResult> SocialMedia(FormViewViewModelNew model)
		{
			var data = await employeeInfoService.GetEmployeeById(Convert.ToInt32(model.employeeID));
			data.skypeId = model.skypeId;
			data.FacebookId = model.FacebookId;
			data.LinkedInId = model.LinkedInId;
			data.TwitterId = model.TwitterId;
			data.InstagramId = model.InstagramId;
			data.WhatsAppId = model.WhatsAppId;
			await personalInfoService.SaveEmployeeInfo(data);
			return Json("SocialMedia");
		}

		[HttpPost]
		public async Task<IActionResult> SaveIeltsInfo([FromForm] FormViewViewModelNew model)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);

			//if (!ModelState.IsValid)
			//{
			//    ViewBag.employeeID = model.employeeID;
			//    model.ielts = await personalInfoService.GetIeltsInfoByEmpId(Int32.Parse(model.employeeID));
			//    model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(Int32.Parse(model.employeeID));
			//    return View(model);
			//}
			//string fileName;
			//if (model.ieltsPhoto != null)
			//{
			//    string message = FileSave.SaveEmpAttachmentNew(out fileName, model.ieltsPhoto);
			//}
			//else
			//{
			//    fileName = await personalInfoService.GetIeltsImgUrlById(Int32.Parse(model.ieltsId));
			//};
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
				attachment = "",
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
			var Id = await personalInfoService.SaveIeltsInfo(data);
			return Json("IeltsInfos-" + Id.ToString());
		}

		[HttpPost]
		public async Task<JsonResult> UpdateIeltsFile(int? id)
		{
			var files = Request.Form.Files;

			var ielts = await nomineeService.GetIeltsById((int)id);
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


					ielts.attachment = fileName;

					await personalInfoService.SaveIeltsInfo(ielts);
				}
			}
			return Json(ielts.employeeId);
		}

		[HttpPost]
		public async Task<IActionResult> SaveHobby(FormViewViewModelNew model)
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
					employeeInfoId = Int32.Parse(model.employeeID),
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

			return Json("Hobby");
		}

		[HttpPost]
		public async Task<IActionResult> SaveComputerLiteracy([FromForm] FormViewViewModelNew model)
		{

			HrComputerLiteracy data = new HrComputerLiteracy
			{
				Id = Int32.Parse(model.LiteracyId),
				employeeId = Int32.Parse(model.employeeID),
				subject = model.subjectlit,
				competencyLevel = model.competencyLevel,
				training = model.traininglt,
				diploma = model.diplomalt,
				status = 1,
				isActive = 1,
				remarks = model.remarkslt

			};

			await subjectService.SaveHrComputerLiteracy(data);
			await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));

			return Json("comlit");
		}

		public async Task<IActionResult> GetAllComputerLiteracyByEmployeeId(int empId)
		{
			var data = await subjectService.GetcomputerLiteracyByEmpId(empId);
			return Json(data);
		}

		[HttpPost]
		public async Task<IActionResult> SaveBankingDiploma([FromForm] FormViewViewModelNew model)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);

			//if (!ModelState.IsValid)
			//{
			//    ViewBag.employeeID = model.employeeID;
			//    model.bankingDiplomas = await bankInfoService.GetBankDiplomaInfoByEmpId(Int32.Parse(model.employeeID));
			//    model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(Int32.Parse(model.employeeID));
			//    return View(model);
			//}

			BankingDiploma data = new BankingDiploma
			{
				Id = model.bankDiplomaId,
				employeeId = Int32.Parse(model.employeeID),
				diplomaPart = model.diplomaPart,
				//diplomaName = model.diplomaName,
				passingYear = model.passingYeardp,
				//resultType=model.resultType,
				result = model.resultdp,
				session = model.session

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
			await bankInfoService.SaveBankingDiplomaInfo(data);
			//////await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));
			return Json("BankingDp");
		}

		[HttpPost]
		public async Task<IActionResult> SaveTax([FromForm] FormViewViewModelNew model)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);

			Tax data = new Tax
			{
				Id = Int32.Parse(model.taxID),
				employeeId = Int32.Parse(model.employeeID),
				taxZone = model.taxZone,
				taxCircle = model.taxCircle,
				eTin = model.etinTax
			};
			await taxService.SaveTax(data);
			if (roles.Contains("HRAdmin") || roles.Contains("admin"))
			{
				data.isDelete = 0;
			}
			else
			{
				data.isDelete = 1;
			}

			return Json("tax");

		}

		public async Task<IActionResult> GetAllTaxByEmployeeId(int empId)
		{
			var data = await taxService.GetTaxByEmpId(empId);
			return Json(data);
		}
		[HttpPost]
		public async Task<IActionResult> SaveMobile([FromForm] FormViewViewModelNew model)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);

			//if (!ModelState.IsValid)
			//{
			//    ViewBag.employeeID = model.employeeId;
			//    //model.photograph = await photographService.GetPhotographByEmpIdAndType(model.employeeId, "profile");
			//    //  model.employeeInfo = await personalInfoService.GetEmployeeInfoById(Int32.Parse(model.employeeId));
			//    model.mobileBenifits = await mobileBenifitService.GetMobileBenifitByEmpId(Int32.Parse(model.employeeID));
			//    return View(model);
			//}
			MobileBenifit data = new MobileBenifit
			{
				Id = (Int32.Parse(model.MobileBenifitID)),
				employeeId = (Int32.Parse(model.employeeID)),
				type = model.typemb,
				amount = model.amount,
				date = model.datemb,
				status = 1,


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
			await mobileBenifitService.SaveMobileBenifit(data);
			await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));
			return Json("Mobile");

		}
		[HttpPost]
		public async Task<IActionResult> SaveExtraCurricular([FromForm] FormViewViewModelNew model)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);
			//if (!ModelState.IsValid)
			//{
			//    ViewBag.employeeID = model.employeeID;
			//    model.extraCurricularType = await extraCurricularTypeService.GetExtraCurricularType();
			//    //model.photograph = await photographService.GetPhotographByEmpIdAndType(model.employeeId, "profile");
			//    model.employeeInfo = await personalInfoService.GetEmployeeInfoById(Int32.Parse(model.employeeID));
			//    model.employeeExtraCurriculars = await employeeExtraCurricularService.GetEmployeeExtraCurricularEmpId(Int32.Parse(model.employeeID));
			//    return View(model);
			//}
			EmployeeExtraCurricular data = new EmployeeExtraCurricular
			{
				Id = (Int32.Parse(model.ExtraCurricularId)),
				employeeId = (Int32.Parse(model.employeeID)),
				extraCurricularTypeId = model.extraCurricularTypeId,

				skillLevel = model.skillLevel,
				skillType = model.skillType,
				description = model.description


			};
			if (roles.Contains("HRAdmin") || roles.Contains("admin"))
			{
				data.isDelete = 0;
			}
			else
			{
				data.isDelete = 1;
			}
			await employeeExtraCurricularService.SaveEmployeeExtraCurricular(data);
			//await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeId));
			return Json("extra");

		}

		///Delete
		public async Task<IActionResult> DeleteSpouse(int id, int empId)
		{
			await spouseChildrenService.DeleteSpouseById(id);
			return Json("spouse");


			//return RedirectToAction("formview", "formview", new
			//{
			//    id = empId
			//});
		}

    

        public async Task<IActionResult> DeleteEmergencyContact(int id, int empId)
		{
			await emergencyContactService.DeleteEmergencyContactById(id);
			return Json("emergency");
		}
		public async Task<IActionResult> DeleteEmployeeInsuranceById(int Id, int empId)
		{
			await nomineeService.DeleteEmployeeInsuranceById(Id);
			return RedirectToAction("formview", "formview", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeleteTAX(int id, int empId)
		{
			await taxService.DeleteTaxById(id);

			return Json("tax");
		}
		public async Task<IActionResult> DeletePromotion(int id, int empId)
		{
			await promotionLogService.DeletePromotionLogById(id);
			//return RedirectToAction(nameof(Index));
			return Json("promotion");
		}
		public async Task<IActionResult> DeleteExtraCurricular(int id, int empId)
		{
			await employeeExtraCurricularService.DeleteEmployeeExtraCurricularId(id);
			return Json("extracurricular");
		}
		public async Task<IActionResult> DeleteDiploma(int id, int empId)
		{
			await bankInfoService.DeleteBankDiplomaById(id);
			return Json("BankingDp");
			//return RedirectToAction("formview", "formview", new
			//{
			//    id = empId
			//});
		}

		public async Task<IActionResult> DeleteDiplomaJson(int id, int empId)
		{
			await bankInfoService.DeleteBankDiplomaById(id);
			//return RedirectToAction(nameof(Index));
			return Json("deleted");
		}
		public async Task<IActionResult> DeleteComputer(int id, int empId)
		{
			await subjectService.DeleteComputerLiteracyById(id);
			//return RedirectToAction(nameof(Index));
			return RedirectToAction("FormView", "formview", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeleteComputerJson(int id, int empId)
		{
			await subjectService.DeleteComputerLiteracyById(id);
			//return RedirectToAction(nameof(Index));
			return Json("computerLit");
		}
		public async Task<IActionResult> DeleteIelts(int id, int empId)
		{
			await personalInfoService.DeleteIeltsById(id);
			return Json("IeltsInfos");
		}
		public async Task<IActionResult> DeleteAttachment(int id, int empId)
		{
			await hRPMSAttachmentService.DeleteHRPMSAttachmentById(id);
			return Json("attatch");
		}
		public async Task<IActionResult> DeleteReference(int id, int empId)
		{
			await referenceService.DeleteReferenceById(id);
			return Json("ref");
		}
		public async Task<IActionResult> DeleteFreedom(int id, int empId)
		{
			await freedomFighterService.DeleteFreedomFighterById(id);
			return Json("freedom");
		}
		public async Task<IActionResult> DeleteAccounts(int id, int empId)
		{
			//await bankInfoService.DeleteBankInfoById(id);
			await bankInfoService.DeleteEmpAccountInfoById(id);
			//return RedirectToAction(nameof(Index));
			return Json("Acc");
		}
		public async Task<IActionResult> DeleteLanguage(int id, int empId)
		{
			await awardPublicationService.DeleteLanguageById(id);
			//return RedirectToAction(nameof(Index));
			return Json("language");
		}
		public async Task<IActionResult> DeleteAward(int id, int empId)
		{
			await awardPublicationService.DeleteAwardById(id);
			//return RedirectToAction(nameof(Index));
			return Json("award");
		}
		public async Task<IActionResult> DeleteMembership(int id, int empId)
		{
			await membershipService.DeleteMembershipInfoById(id);
			//return RedirectToAction(nameof(Index));
			return Json("membership");
		}

		public async Task<IActionResult> GetAllMembership(int empId)
		{
			var data = await membershipService.GetMembershipInfoByEmpId(empId);
			return Json(data);
		}
		public async Task<IActionResult> GetAllExtraCurricular(int empId)
		{
			var data = await employeeExtraCurricularService.GetEmployeeExtraCurricularEmpId(empId);
			return Json(data);
		}
		public async Task<IActionResult> GetAllFreedomFighter(int empId)
		{
			var data = await freedomFighterService.GetFreedomFighterlistByEmpId(empId);
			return Json(data);
		}
		public async Task<IActionResult> GetAllReference(int empId)
		{
			var data = await referenceService.GetReferenceByEmpId(empId);
			return Json(data);
		}
		public async Task<IActionResult> GetAllTravel(int empId)
		{
			var data = await travelInfoService.GetTraveInfoByEmpId(empId);
			return Json(data);
		}
		public async Task<IActionResult> GetAllPassport(int empId)
		{
			var data = await passportInfoService.GetPassportInfoByEmpId(empId);
			return Json(data);
		}

		public async Task<IActionResult> GetAllLicense(int empId)
		{
			var data = await drivingLicenseService.GetDrivingLicenseByEmpId(empId);
			return Json(data);
		}
		public async Task<IActionResult> GetAllIeltsInfo(int empId)
		{
			var data = await personalInfoService.GetIeltsInfoByEmpId(empId);
			return Json(data);
		}
		public async Task<IActionResult> GetAllLanguage(int empId)
		{
			var data = await awardPublicationService.GetLanguageByEmpId(empId);
			return Json(data);
		}
		public async Task<IActionResult> GetAllSpouseInfo(int empId)
		{
			var data = await spouseChildrenService.GetSpouseByEmpId(empId);
			return Json(data);
		}
		//   public async Task<IActionResult> GetAllHobbyInfo(int empId)
		//{
		//    var data = await spouseChildrenService.Gete(empId);
		//    return Json(data);
		//}



		public async Task<IActionResult> GetAllEmergencyContact(int empId)
		{
			var data = await emergencyContactService.GetEmergencyContactByEmpId(empId);
			return Json(data);
		}
		public async Task<IActionResult> GetAllAccountsInfo(int empId)
		{
			//var data = await bankInfoService.GetBankInfoByEmpId(empId);
			var data = await personalInfoService.GetAllEmployeeAccountsByEmpId(empId);
			return Json(data);
		}
		public async Task<IActionResult> GetAllDisplinaryInfo(int empId)
		{
			var data = await disciplinaryService.GetDisciplinaryLogByEmpId(empId);
			return Json(data);
		}
		public async Task<IActionResult> GetAllAwardInfo(int empId)
		{
			var data = await awardPublicationService.GetAwardsByEmpId(empId);
			return Json(data);
		}
		public async Task<IActionResult> GetAllPromotionInfo(int empId)
		{
			var data = await promotionLogService.GetPromotionLogByEmpId(empId);
			return Json(data);
		}
		public async Task<IActionResult> GetAllTrainingInfo(int empId)
		{
			var data = await traningHistoryService.GetTraningHistoryByEmpId(empId);
			return Json(data);
		}
		public async Task<IActionResult> GetAllBankingDiplomaInfo(int empId)
		{
			var data = await bankInfoService.GetBankDiplomaInfoByEmpId(empId);
			return Json(data);
		}
		public async Task<IActionResult> GetAllCertificationInfo(int empId)
		{
			var data = await professionalQualificationsService.GetProfessionalQualificationsByEmpId(empId);
			return Json(data);
		}
		public async Task<IActionResult> GetAllEducationalQualification(int empId)
		{
			var data = await employeeInfoService.GetEducationalQualificationByEmpId(empId);
			return Json(data);
		}
        public async Task<IActionResult> GetAllApprovalLeave(int empId)
		{
			var data = await approvalService.GetApprovalMaster(empId);
			return Json(data);
		}



		public async Task<IActionResult> GetAllNomineeInfo(int empId)
		{
			var data = await nomineeService.GetNomineeByEmpId(empId);
			return Json(data);
		}
		public async Task<IActionResult> GetAllChildrenInfo(int empId)
		{
			var data = await spouseChildrenService.GetChildrenByEmpId(empId);
			return Json(data);
		}



		public async Task<IActionResult> GetAllAttatchments(int empId)
		{
			var data = await hRPMSAttachmentService.GetHRPMSAttachmentByEmpId(empId);
			return Json(data);
		}
		public async Task<IActionResult> GetSocialMedia(int empId)
		{
			var data = await referenceService.GetSocialMediaByEmpId(empId);
			return Json(data);
		}
		public async Task<IActionResult> DeleteTravel(int id, int empId)
		{
			await travelInfoService.DeleteTraveInfoById(id);
			//return RedirectToAction(nameof(Index));
			return Json("travel");
		}
		public async Task<IActionResult> DeletePassport(int id, int empId)
		{
			await passportInfoService.DeletePassportInfoById(id);
			//return RedirectToAction(nameof(Index));
			return Json("passport");
		}
		public async Task<IActionResult> DeleteLicense(int id, int empId)
		{
			await drivingLicenseService.DeleteDrivingLicenseById(id);
			//return RedirectToAction(nameof(Index));
			return Json("license");
		}
		public async Task<IActionResult> DeleteDisciplinary(int id, int empId)
		{
			await disciplinaryService.DeleteDisciplinaryLogById(id);
			//return RedirectToAction(nameof(Index));
			return Json("displinary");
		}
		public async Task<IActionResult> DeleteTraining(int id, int empId)
		{
			await traningHistoryService.DeleteTraningHistoryById(id);
			//return RedirectToAction(nameof(Index));
			return Json("training");
		}
		public async Task<IActionResult> DeleteEducationQualification(int id, int empId)
		{
			await employeeInfoService.DeleteEducationalQualificationById(id);
			return Json("edu");
			//return RedirectToAction("FormView", "FormView", new
			//{
			//    id = empId
			//});
		}
		public async Task<IActionResult> DeleteChildren(int id, int empId)
		{
			await spouseChildrenService.DeleteChildrenById(id);
			return Json("upchild");
			//return RedirectToAction("FormView", "FormView", new
			//{
			//    id = empId
			//});
		}

        public  IActionResult DeleteLeaveApproval(int id, int empId)
        {
            approvalService.DeleteApprovalMasterForcelyById(id);
            return Json("Lapproval");
        }


        public async Task<IActionResult> DeleteMobile(int id, int empId)
		{
			await mobileBenifitService.DeleteMobileBenifitById(id);
			return RedirectToAction("FormView", "FormView", new
			{
				id = empId
			});
		}
		public async Task<IActionResult> DeleteNominee(int id, int empId)
		{
			await nomineeDetailService.DeleteNomineeDetailByNomineeId(id);
			await nomineeService.DeleteNomineeById(id);

			return Json("nominee");
			//return RedirectToAction("FormView", "FormView", new
			//{
			//    id = empId
			//});
		}

		//public async Task<IActionResult> DeleteById(int id, string table)
		//{
		//    if (table == "ielts")
		//    {
		//        await personalInfoService.DeleteIeltsById(id);
		//    }
		//    else if (table == "nominee")
		//    {

		//    }
		//    else if (table == "nominee")
		//    {

		//    }
		//    else if (table == "nominee")
		//    {

		//    }
		//    else if (table == "nominee")
		//    {

		//    }
		//    else if (table == "nominee")
		//    {

		//    }
		//    else if (table == "nominee")
		//    {

		//    }
		//    else if (table == "nominee")
		//    {

		//    }
		//    else if (table == "nominee")
		//    {

		//    }
		//    else if (table == "nominee")
		//    {

		//    }
		//    else if (table == "nominee")
		//    {

		//    }
		//    else if (table == "nominee")
		//    {

		//    }
		//    else if (table == "nominee")
		//    {

		//    }
		//    else if (table == "nominee")
		//    {

		//    }
		//    else if (table == "nominee")
		//    {

		//    }
		//    else if (table == "nominee")
		//    {

		//    }
		//    else if (table == "nominee")
		//    {

		//    }
		//    else if (table == "nominee")
		//    {

		//    }
		//    else if (table == "nominee")
		//    {

		//    }
		//    else if (table == "nominee")
		//    {

		//    }
		//    else if (table == "nominee")
		//    {

		//    }
		//    else if (table == "nominee")
		//    {

		//    }
		//    else if (table == "nominee")
		//    {

		//    }
		//    return Json("deleted");
		//}

	}
}