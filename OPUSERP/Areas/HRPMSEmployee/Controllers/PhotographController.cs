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
using OPUSERP.Areas.HRPMSLeave.Models;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.HRPMS.Services.Leave.Interfaces;
using OPUSERP.Areas.HRPMSLeave.Models.Lang;
using OPUSERP.ERPService.AuthService.Interfaces;
using Microsoft.AspNetCore.Identity;
using OPUSERP.Data.Entity;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
	[Authorize]
	[Area("HRPMSEmployee")]
	public class PhotographController : Controller
	{
		private readonly LangGenerate<EmployeeInfoLn> _lang;
		private readonly IPhotographService photographService;
		private readonly IWagesPhotographService wagesPhotographService;
		private readonly IPersonalInfoService personalInfoService;
		private readonly IWagesPersonalInfoService wagesPersonalInfoService;
		private readonly ILeaveRegisterService leaveRegisterService;
		private readonly ILeaveTypeService leaveTypeService;
		private readonly IApprovalService approvalService;
		private readonly ILeavePolicyService leavePolicyService;
		//private readonly LangGenerate<LeaveLn> _langs;
		private readonly IUserInfoes userInfo;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<ApplicationRole> _roleManager;

		public PhotographController(IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IWagesPhotographService wagesPhotographService, IPhotographService photographService, IPersonalInfoService personalInfoService, IWagesPersonalInfoService wagesPersonalInfoService, ILeaveRegisterService leaveRegisterService, ILeaveTypeService leaveTypeService, IApprovalService approvalService, ILeavePolicyService leavePolicyService, IUserInfoes userInfo)
		{
			_lang = new LangGenerate<EmployeeInfoLn>(hostingEnvironment.ContentRootPath);
			//_langs = new LangGenerate<LeaveLn>(hostingEnvironment.ContentRootPath);
			this.photographService = photographService;
			this.personalInfoService = personalInfoService;
			this.wagesPhotographService = wagesPhotographService;
			this.wagesPersonalInfoService = wagesPersonalInfoService;
			this.leaveRegisterService = leaveRegisterService;
			this.leaveTypeService = leaveTypeService;
			this.approvalService = approvalService;
			this.leavePolicyService = leavePolicyService;
			this.userInfo = userInfo;
			_roleManager = roleManager;
			_userManager = userManager;
		}

		// GET: Photograph
		public async Task<IActionResult> Index(int id)
		{
			ViewBag.employeeID = id.ToString();
			PhotographViewModel model = new PhotographViewModel
			{
				employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
				fLang = _lang.PerseLang("Employee/EmployeeInfoEN.json", "Employee/EmployeeInfoBN.json", Request.Cookies["lang"]),
				photograph = await photographService.GetPhotographByEmpIdAndType(id,"profile"),
				employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id)
			};
			if (model.photograph == null) model.photograph = new Photograph();
			return View(model);
		}

		// GET: Photograph
		public async Task<IActionResult> Signature(int id)
		{
			ViewBag.employeeID = id.ToString();
			PhotographViewModel model = new PhotographViewModel
			{
				photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
				employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
				fLang = _lang.PerseLang("Employee/EmployeeInfoEN.json", "Employee/EmployeeInfoBN.json", Request.Cookies["lang"]),
				employeeSignature = await photographService.GetEmployeeSignatureByEmpId(id),
				employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id)
			};
			if (model.employeeSignature == null) model.employeeSignature = new EmployeeSignature();
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Signature([FromForm] PhotographViewModel model)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);

			if (!ModelState.IsValid)
			{
				ViewBag.employeeID = model.employeeID;
				model.employeeSignature = await photographService.GetEmployeeSignatureByEmpId(model.employeeID);
				model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(model.employeeID);
				if (model.employeeSignature == null) model.employeeSignature = new EmployeeSignature();
				return View(model);
			}

            string fileName = "";
            string fileNameB = "";

            string message = "";
            string messageB = "";
            if (model.empPhoto != null)
            {
                message = FileSave.SaveImage(out fileName, model.empPhoto);
            }
            if (model.banglaSign != null)
            {
                messageB = FileSave.SaveImage(out fileNameB, model.banglaSign);
            }

            var data = await personalInfoService.GetSignatureByEmpId(model.employeeID);
            if (data == null)
            {
                data.Id = 0;
            }

            if (message == "success")
			{
				data.employeeId = model.employeeID;
				data.url = fileName;
			}
			if (messageB == "success")
			{
				data.banglaSign = fileNameB;
			}

			if (roles.Contains("HRAdmin") || roles.Contains("admin"))
			{
				data.isDelete = 0;
			}
			else
			{
				data.isDelete = 1;
			}
			await photographService.SaveEmployeeSignature(data);

			//return RedirectToAction(nameof(Signature));
			return RedirectToAction("Signature", "Photograph", new
			{
				id = model.employeeID
			});
		}

		// GET: Photograph
		public async Task<IActionResult> WagesIndex(int id)
		{
			ViewBag.employeeID = id.ToString();
			PhotographViewModel model = new PhotographViewModel
			{
				fLang = _lang.PerseLang("Employee/EmployeeInfoEN.json", "Employee/EmployeeInfoBN.json", Request.Cookies["lang"]),
				wagesPhotograph = await wagesPhotographService.GetPhotographByEmpIdAndType(id, "profile"),
				employeeNameCode = await wagesPersonalInfoService.GetEmployeeNameCodeById(id)
			};
			if (model.wagesPhotograph == null) model.wagesPhotograph = new WagesPhotograph();
			return View(model);
		}

		public async Task<IActionResult> EditGrid(int id)
		{
			ViewBag.employeeID = id.ToString();
			PhotographViewModel model = new PhotographViewModel
			{
				photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
				employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
				employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id),
				visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData(),

				addresse = await wagesPhotographService.GetEditSecByEmpId(id, "addresse"),
				spousee = await wagesPhotographService.GetEditSecByEmpId(id, "spousee"),
				childrene = await wagesPhotographService.GetEditSecByEmpId(id, "childrene"),
				emergencye = await wagesPhotographService.GetEditSecByEmpId(id, "emergencye"),
				nomineee = await wagesPhotographService.GetEditSecByEmpId(id, "nomineee"),
				insurancee = await wagesPhotographService.GetEditSecByEmpId(id, "insurancee"),
				educatione = await wagesPhotographService.GetEditSecByEmpId(id, "educatione"),
				profqualificatione = await wagesPhotographService.GetEditSecByEmpId(id, "profqualificatione"),
				otherqualificatione = await wagesPhotographService.GetEditSecByEmpId(id, "otherqualificatione"),
				traininge = await wagesPhotographService.GetEditSecByEmpId(id, "traininge"),
				servicee = await wagesPhotographService.GetEditSecByEmpId(id, "servicee"),
				promotione = await wagesPhotographService.GetEditSecByEmpId(id, "promotione"),
				pmse = await wagesPhotographService.GetEditSecByEmpId(id, "pmse"),
				diciplinee = await wagesPhotographService.GetEditSecByEmpId(id, "diciplinee"),
				supervisore = await wagesPhotographService.GetEditSecByEmpId(id, "supervisore"),
				drivinge = await wagesPhotographService.GetEditSecByEmpId(id, "drivinge"),
				passporte = await wagesPhotographService.GetEditSecByEmpId(id, "passporte"),
				travele = await wagesPhotographService.GetEditSecByEmpId(id, "travele"),
				membere = await wagesPhotographService.GetEditSecByEmpId(id, "membere"),
				awarde = await wagesPhotographService.GetEditSecByEmpId(id, "awarde"),
				publicatione = await wagesPhotographService.GetEditSecByEmpId(id, "publicatione"),
				languagee = await wagesPhotographService.GetEditSecByEmpId(id, "languagee"),
				otheractivitiese = await wagesPhotographService.GetEditSecByEmpId(id, "otheractivitiese"),
				accountse = await wagesPhotographService.GetEditSecByEmpId(id, "accountse"),
				belongingse = await wagesPhotographService.GetEditSecByEmpId(id, "belongingse"),
				prevempe = await wagesPhotographService.GetEditSecByEmpId(id, "prevempe"),
				freedome = await wagesPhotographService.GetEditSecByEmpId(id, "freedome"),
				refe = await wagesPhotographService.GetEditSecByEmpId(id, "refe"),
				officeassigne = await wagesPhotographService.GetEditSecByEmpId(id, "officeassigne"),
				picturee = await wagesPhotographService.GetEditSecByEmpId(id, "picturee"),
				signe = await wagesPhotographService.GetEditSecByEmpId(id, "signe"),
				contracte = await wagesPhotographService.GetEditSecByEmpId(id, "contracte"),
				projecte = await wagesPhotographService.GetEditSecByEmpId(id, "projecte"),
				finantiale = await wagesPhotographService.GetEditSecByEmpId(id, "finantiale"),
				attatchmente = await wagesPhotographService.GetEditSecByEmpId(id, "attatchmente"),
				proassigne = await wagesPhotographService.GetEditSecByEmpId(id, "proassigne"),
				otherinfoe = await wagesPhotographService.GetEditSecByEmpId(id, "otherinfoe"),
				costcentere = await wagesPhotographService.GetEditSecByEmpId(id, "costcentere"),
				empgradee = await wagesPhotographService.GetEditSecByEmpId(id, "empgradee"),
				shiftassigne = await wagesPhotographService.GetEditSecByEmpId(id, "shiftassigne"),
				leavestatuse = await wagesPhotographService.GetEditSecByEmpId(id, "leavestatuse"),
				sociale = await wagesPhotographService.GetEditSecByEmpId(id, "sociale"),
				ieltse = await wagesPhotographService.GetEditSecByEmpId(id, "ieltse"),
				addinfoe = await wagesPhotographService.GetEditSecByEmpId(id, "addinfoe"),
				emphobbye = await wagesPhotographService.GetEditSecByEmpId(id, "emphobbye"),
				copliteracye = await wagesPhotographService.GetEditSecByEmpId(id, "copliteracye"),
				diplomae = await wagesPhotographService.GetEditSecByEmpId(id, "diplomae"),
				taxe = await wagesPhotographService.GetEditSecByEmpId(id, "taxe"),
				foode = await wagesPhotographService.GetEditSecByEmpId(id, "foode"),
				duale = await wagesPhotographService.GetEditSecByEmpId(id, "duale"),
				Sport = await wagesPhotographService.GetEditSecByEmpId(id, "Sport"),
				Rolee = await wagesPhotographService.GetEditSecByEmpId(id, "Rolee"),
				diseases = await wagesPhotographService.GetEditSecByEmpId(id, "diseases"),
				benifits = await wagesPhotographService.GetEditSecByEmpId(id, "benifits"),


				employeePostings = await wagesPersonalInfoService.GetActivePostingsByEmpIdAndStatus(id, 1),
			  
				suspensions = await wagesPhotographService.GetEditSecByEmpId(id, "suspensions"),
				allegations = await wagesPhotographService.GetEditSecByEmpId(id, "allegations"),
				employeeExtraCurricular = await wagesPhotographService.GetEditSecByEmpId(id, "employeeExtraCurricular")
			};
			if (model.photograph == null) model.photograph = new Photograph();
			return View(model);
		}

		public async Task<IActionResult> WagesEditGrid(int id)
		{
			ViewBag.employeeID = id.ToString();
			PhotographViewModel model = new PhotographViewModel
			{
				wagesPhotograph = await wagesPhotographService.GetPhotographByEmpIdAndType(id, "profile"),
				wagesEmployeeInfo = await wagesPersonalInfoService.GetEmployeeInfoById(id),
				employeeNameCode = await wagesPersonalInfoService.GetEmployeeNameCodeById(id)
			};
			if (model.wagesPhotograph == null) model.wagesPhotograph = new WagesPhotograph();
			return View(model);
		}

		// POST: Photograph/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Index([FromForm] PhotographViewModel model)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var roles = await _userManager.GetRolesAsync(user);

			//if (!ModelState.IsValid)
			//{
			//    ViewBag.employeeID = model.employeeID;
			//    model.photograph = await photographService.GetPhotographByEmpIdAndType(model.employeeID, "profile");
			//    model.employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(model.employeeID);
			//    if (model.photograph == null) model.photograph = new Photograph();
			//    return View(model);
			//}

			string fileName;
			string message = FileSave.SaveImage(out fileName, model.empPhoto);
			
			if(message == "success")    
			{  
				Photograph data = new Photograph
				{
					Id = model.photographID,
					employeeId = model.employeeID,
					url = fileName,
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
				await photographService.SavePhotograph(data);
			}
			var q = model.queryString.Split('/');
			var areaName = q[1];
			var controller = q[2];
			var action = q[3];
			//return RedirectToAction(nameof(Index));
			return RedirectToAction(action, controller, new
			{
				id = model.employeeID,
				area = areaName
			});
		}

		// POST: Photograph/Create
		[HttpPost]
		public async Task<ActionResult> ProfilePictureSave([FromForm] PhotographViewModel model)
		{
			
			string status="";
			string fileName;
			string message = FileSave.SaveImage(out fileName, model.empPhoto);

			if (message == "success")
			{
				Photograph data = new Photograph
				{
					employeeId = model.employeeID,
					url = fileName,
					type = "profile"
				};
				if (model.photographID == 0 || model.photographID <0)
				{
					data.Id = 0;
				}
				else
				{
					data.Id = model.photographID;
				}
				var save = await photographService.SavePhotograph(data);
				if (save >= 0)
				{
					status = "success";
				}
				else
				{
					status = "failed";
				}
			}

			return Json(status);
		}

		// POST: Photograph/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> WagesIndex([FromForm] PhotographViewModel model)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.employeeID = model.employeeID;
				model.wagesPhotograph = await wagesPhotographService.GetPhotographByEmpIdAndType(model.employeeID, "profile");
				model.employeeNameCode = await wagesPersonalInfoService.GetEmployeeNameCodeById(model.employeeID);
				if (model.wagesPhotograph == null) model.wagesPhotograph = new WagesPhotograph();
				return View(model);
			}

			string fileName;
			string message = FileSave.SaveImage(out fileName, model.empPhoto);

			if (message == "success")
			{
				WagesPhotograph data = new WagesPhotograph
				{
					Id = model.photographID,
					employeeId = model.employeeID,
					url = fileName,
					type = "profile"
				};
				await wagesPhotographService.SavePhotograph(data);
			}

			return RedirectToAction(nameof(WagesIndex));
		}


		//public async Task<IActionResult> GetEditedSectionByEmpId(int empId)
		//{
		//    var model = new TrackEditSection
		//    {
		//        address = await wagesPhotographService.GetEditSecByEmpId(empId, "address") > 0 ? "addresse" : "addressn"
		//    };
		//    return Json(model);
		//}
		public async Task<IActionResult> LeaveStatusForSpecificEmployee(int id)
		{
			
			ViewBag.employeeId = id;
		  //  var user = await userInfo.getUserByEmployeeId(id);
			var user = await personalInfoService.GetUserIdByEmpId(id);

			var model = new LeaveRegisterViewModel
			{
				//fLang = _langs.PerseLang("Leave/LeaveApplyEN.json", "Leave/LeaveApplyBN.json", Request.Cookies["lang"]),
				leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpId(id),
				leaveTypelist = await leaveTypeService.GetAllLeaveType(),
				approvalDetail = await approvalService.GetSingleApprovalDetailByEmpAndType(id, "Leave"),
				leaveDays = await leavePolicyService.GetAllLeaveDay(),
				leaveStatusViewModels = await leavePolicyService.GetLeaveStatus(id),
				approvalDetails = await leavePolicyService.GetApprovalDetailsByUser(user.userId)
			};
			return View(model);
		}


		//public async Task<ActionResult> LeaveStatusForSpecificEmployee(int id)
		//{
		//    ViewBag.employeeID = id.ToString();
		//    var model = new DisciplinaryViewModel
		//    {
		//        employeeId = id,
		//        photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
		//        employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
		//        fLang = _lang.PerseLang("Employee/DisciplinaryEN.json", "Employee/DisciplinaryBN.json", Request.Cookies["lang"]),
		//        offenses = await disciplineInvestigation.GetAllOffense(),
		//        punishments = await disciplineInvestigation.GetAllPunishment(),
		//        disciplinaries = await disciplinaryService.GetDisciplinaryLogByEmpId(id),
		//        employeeNameCode = await personalInfoService.GetEmployeeNameCodeById(id)
		//    };
		//    return View(model);
		//}
	}
}