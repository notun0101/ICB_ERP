using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HospitalManagement.Models;
using OPUSERP.SCM.Services.Hospital.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.Data.Entity;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.HRPMS.Services.Organogram.Interfaces;


namespace OPUSERP.Areas.HospitalManagement.Controllers
{
	[Area("HospitalManagement")]
	public class HospitalCommonController : Controller
    {
		private readonly LangGenerate<EmployeeInfoLn> _lang;
		private readonly IPersonalInfoService personalInfoService;
		private readonly IReligionMunicipalityService religionMunicipalityService;
		private readonly ITypeService typeService;
		private readonly IOrganizationPostService organizationPostService;
		private readonly IEmployeeOrganogramService employeeOrganogramService;
		private readonly IDesignationDepartmentService designationDepartmentService;
		private readonly IAddressService addressService;
		private readonly IDoctor _doctorService;
		private readonly ISpecialBranchUnitService specialBranchUnitService;
		private readonly IFunctionsInfoService functionsInfoService;
		private readonly IPhotographService photographService;
		private readonly ILocationService locationService;
		private readonly IStatusService statusService;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<ApplicationRole> _roleManager;

		public HospitalCommonController(IHostingEnvironment hostingEnvironment, IDoctor _doctorService, IPhotographService photographService, IFunctionsInfoService functionsInfoService, ILocationService locationService, RoleManager<ApplicationRole> roleManager, IStatusService statusService, IPersonalInfoService personalInfoService, IReligionMunicipalityService religionMunicipalityService, ITypeService typeService, UserManager<ApplicationUser> userManager, IOrganizationPostService organizationPostService, IEmployeeOrganogramService employeeOrganogramService, IDesignationDepartmentService designationDepartmentService, IAddressService addressService, ISpecialBranchUnitService specialBranchUnitService)
		{
			_lang = new LangGenerate<EmployeeInfoLn>(hostingEnvironment.ContentRootPath);
			this.personalInfoService = personalInfoService;
			this.religionMunicipalityService = religionMunicipalityService;
			this.organizationPostService = organizationPostService;
			this.employeeOrganogramService = employeeOrganogramService;
			this.designationDepartmentService = designationDepartmentService;
			this.typeService = typeService;
			this.addressService = addressService;
			this._doctorService = _doctorService;
			this.photographService = photographService;


			this.specialBranchUnitService = specialBranchUnitService;
			this.locationService = locationService;
			this.functionsInfoService = functionsInfoService;
			this.statusService = statusService;
			_roleManager = roleManager;
			_userManager = userManager;
		}
		public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Ambulance()
		{
			return View();
		}
		public async Task<IActionResult> AmbulanceList()
		{
			return View();
		}
		public async Task<IActionResult> Nurse()
		{
			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
			var model = new EmployeeInfoViewModel
			{
				fLang = _lang.PerseLang("Employee/EmployeeInfoEN.json", "Employee/EmployeeInfoBN.json", Request.Cookies["lang"]),
				religions = await religionMunicipalityService.GetReligions(),
				employeeTypes = await typeService.GetAllEmployeeType(),
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Nurse([FromForm] EmployeeInfoViewModel model)
        {
            //return Json(model);
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

            int temp = await personalInfoService.IsThisEmpIDPresent(model.employeeCode);
            bool flag = false;
            if (temp != 0 && temp != Int32.Parse((model.employeeID)))
            {
                ModelState.AddModelError(string.Empty, "This Employee Code Already Taken. Please Try Another Employee Code");
                flag = true;
            }
            //var periodCheck = await personalInfoService.GetDuplicateEmpCode(Convert.ToInt32(model.employeeID), model.employeeCode);

            if (!ModelState.IsValid || flag)
            //if (!ModelState.IsValid || periodCheck.Count() > 0)
            {
                ViewBag.employeeID = model.employeeID;
                model.fLang = _lang.PerseLang("Employee/EmployeeInfoEN.json", "Employee/EmployeeInfoBN.json", Request.Cookies["lang"]);
                model.religions = await religionMunicipalityService.GetReligions();
                model.employeeTypes = await typeService.GetAllEmployeeType();
                model.organoOrganizations = await organizationPostService.GetAllOrganizationByIds(this.GetAllIdsByOrg(user.org));
                model.designations = await designationDepartmentService.GetDesignations();
                model.specialBranchUnits = await specialBranchUnitService.GetSpecialBranchUnit();
                model.districts = await addressService.GetAllDistrict();
                model.departments = await designationDepartmentService.GetDepartment();
                model.serviceStatuses = await statusService.GetServiceStatus();
                model.hrPrograms = await statusService.GetHrProgram();
                model.hrUnits = await statusService.GetHrUnit();
                model.locations = await locationService.GetLocation();
                model.functionInfos = await functionsInfoService.GetFunctionInfo();
                model.visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData();

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
                motherNameEnglish = motherNameEnglish,
                motherNameBangla = model.motherNameBangla,
                fatherNameEnglish = fatherNameEnglish,
                fatherNameBangla = model.fatherNameBangla,
                nationality = model.nationality,
                disability = model.disability,
                disablityType = model.disablityType,
                dateOfBirth = model.dateOfBirth,
                gender = model.gender,
                birthPlace = model.birthPlace,
                maritalStatus = model.maritalStatus,
                religionId = model.religion,
                employeeTypeId = model.employeeType,
                tin = model.tin,
                batch = model.batch,
                bloodGroup = model.bloodGroup,
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
                dateofregularity = model.dateofregularity,
                dateOfPermanent = model.dateOfPermanent,
                branchId = model.sbu,
                pNSId = model.pns,

                natureOfRequitment = model.natureOfRequitment,
                activityStatus = model.activityStatus,
                departmentId = model.department,
                specialSkill = model.specialSkill,
                seniorityNumber = model.seniorityNumber,
                joiningDesignation = "Nurse",
                designation = "Nurse",

                skypeId = model.skypeId,
                TwitterId = model.TwitterId,
                FacebookId = model.FacebookId,
                InstagramId = model.InstagramId,
                LinkedInId = model.LinkedInId,

                employeeStatusId = model.employeeStatusId,
                hrProgramId = model.hrProgramId,
                hrUnitId = model.hrUnitId,
                functionInfoId = model.functionInfoId,
                locationId = model.locationId,
                post = model.post,
                homeDistrict = model.homeDistrict,
                orgType = "ddm",
                ApplicationUserId = ApplicationUserId,
                salaryStatus = model.salaryStatus,
                salaryStatusComment = model.salaryStatusComment
            };

            int lstId = await personalInfoService.SaveEmployeeInfo(data);
            await personalInfoService.UpdateEmployeeinfoById(lstId);
            return RedirectToAction(nameof(Nurse));
        }


        public async Task<IActionResult> NurseList()
		{
			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
			string userName = HttpContext.User.Identity.Name;

			var model = new EmployeeInfoViewModel
			{
				allEmployeeJsons = await personalInfoService.GetAllEmployeeInfoJson(),
				allEmployeeJson = await personalInfoService.GetEmployeeInfoJson(userName),
				fLang = _lang.PerseLang("Employee/EmployeeInfoEN.json", "Employee/EmployeeInfoBN.json", Request.Cookies["lang"]),
				allEmployeeInfos = await personalInfoService.GetEmployeeInfoByOrg("ddm"),
				employeeTypes = await typeService.GetAllEmployeeType(),
				visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData()
			};
			return View(model);
		}
		public async Task<IActionResult> BloodBank()
		{
			return View();
		}
		public async Task<IActionResult> BloodBankList()
		{
			return View();
		}
		public async Task<IActionResult> Appointment()
		{
			return View();
		}
		public async Task<IActionResult> AppointmentBar(int doctorId, string date)
		{
			ViewBag.Doctor = doctorId;
			ViewBag.Date = date;
			var data = new AppointmentBarViewModel
			{
				doctorInfo = await _doctorService.GetDoctorInfoById(doctorId),
				date = date
			};
			return View(data);
		}
		public async Task<IActionResult> OperationTheater()
		{
			return View();
		}
		public async Task<IActionResult> Pharmacy()
		{
			return View();
		}
        public async Task<IActionResult> Emergency()
		{
			return View();
		} 
        public async Task<IActionResult> LabTestRegistration()
		{
			return View();
		}
        public async Task<IActionResult> LabTestRegForAdmin()
		{
			return View();
		} 
        
        public async Task<IActionResult> ReportStatus()
		{
			return View();
		}
	}
}