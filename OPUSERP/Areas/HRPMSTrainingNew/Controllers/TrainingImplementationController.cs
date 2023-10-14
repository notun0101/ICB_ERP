using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSTrainingNew.Models;
using OPUSERP.Areas.HRPMSTrainingNew.Models.Lang;
using OPUSERP.Data.Entity;
using OPUSERP.Data.Entity.Training;
using OPUSERP.ERPServices.EmailService.interfaces;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.TrainingNew;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.HRPMS.Services.TrainingNew.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSTrainingNew.Controllers
{
	[Authorize]
	[Area("HRPMSTrainingNew")]
	public class TrainingImplementationController : Controller
	{
		private readonly LangGenerate<TrainingImplementationLn> _lang;
		private readonly ITypeService typeService;
		private readonly ITrainingNewService trainingNewService;
		private readonly IEnrollTraineeService enrollTraineeService;
		private readonly IResourcePersonService resourcePersonService;
		private readonly ITrainingResourcePersonService trainingResourcePersonService;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IEmployeePunchCardInfoService employeePunchCardInfoService;
		private readonly IPersonalInfoService personalInfoService;
		private readonly IYearCourseTitleService yearCourseTitleService;
		private readonly string rootPath;
		private readonly MyPDF myPDF;
		private readonly IPhotographService photographService;
        private readonly IERPCompanyService eRPCompanyService;
        private readonly IEmailSenderService emailSenderService;

        public TrainingImplementationController(IPhotographService photographService, IHostingEnvironment hostingEnvironment,
            IConverter converter, IPersonalInfoService personalInfoService, IEmployeePunchCardInfoService employeePunchCardInfoService,
            ITrainingResourcePersonService trainingResourcePersonService, IResourcePersonService resourcePersonService,
            ITypeService typeService, ITrainingNewService trainingNewService, IEnrollTraineeService enrollTraineeService, 
            UserManager<ApplicationUser> userManager, IERPCompanyService eRPCompanyService, IYearCourseTitleService yearCourseTitleService,
            IEmailSenderService emailSenderService)
		{
			_lang = new LangGenerate<TrainingImplementationLn>(hostingEnvironment.ContentRootPath);
			this.typeService = typeService;
			this.trainingNewService = trainingNewService;
			this.enrollTraineeService = enrollTraineeService;
			this.resourcePersonService = resourcePersonService;
			this.trainingResourcePersonService = trainingResourcePersonService;
			this.employeePunchCardInfoService = employeePunchCardInfoService;
			this.personalInfoService = personalInfoService;
			this.yearCourseTitleService = yearCourseTitleService;
			_userManager = userManager;
			this.photographService = photographService;
            this.eRPCompanyService = eRPCompanyService;
            this.emailSenderService = emailSenderService;
            myPDF = new MyPDF(hostingEnvironment, converter);
			rootPath = hostingEnvironment.ContentRootPath;
		}

		// GET: TrainingImplementation
		public async Task<IActionResult> Index(int id)
		{
			ViewBag.TrainingPlanId = id;
			TrainingImplementationViewModel model = new TrainingImplementationViewModel
			{
				fLang = _lang.PerseLang("TrainingNew/TrainingImplementationEN.json", "TrainingNew/TrainingImplementationBN.json", Request.Cookies["lang"]),
				employeeTypes = await typeService.GetAllEmployeeType(),
				trainingInfoNew = await trainingNewService.GetTrainingInfoNewById(id),
				trainingInfoNews = await trainingNewService.GetTrainingInfoNewByStatus(2)
			};
			return View(model);
		}


        public IActionResult CheckShortOrder(int no,int Tid)
        {
            return Json(trainingNewService.checkShortOrder(no,Tid));
        }

        public async Task<IActionResult> Foreign(int id)
		{
			ViewBag.TrainingPlanId = id;
			TrainingImplementationViewModel model = new TrainingImplementationViewModel
			{
				fLang = _lang.PerseLang("TrainingNew/TrainingImplementationEN.json", "TrainingNew/TrainingImplementationBN.json", Request.Cookies["lang"]),
				employeeTypes = await typeService.GetAllEmployeeType(),
				trainingInfoNew = await trainingNewService.GetTrainingInfoNewById(id),
				trainingInfoNews = await trainingNewService.GetTrainingInfoNewByStatus(2)
			};
			return View(model);
		}

		// POST: TrainingImplementation/Index
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Index([FromForm] TrainingImplementationViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.fLang = _lang.PerseLang("TrainingNew/TrainingImplementationEN.json", "TrainingNew/TrainingImplementationBN.json", Request.Cookies["lang"]);
				model.trainingInfoNew = await trainingNewService.GetTrainingInfoNewById(model.id);
				return View(model);
			}

			TrainingInfoNew data = new TrainingInfoNew
			{
				Id = model.id,
				startDateActual = model.startDateActual,
				endDateActual = model.endDateActual,
				amountActual = model.actualExpence,
				noOfParticipantsActual = model.noOfParticipantsActual,
				status = 2,
			};

			await trainingNewService.UpdateTrainingInfoNewById(data);

			if(model.trainingType == 1)
			{
				return RedirectToAction(nameof(TrainingList));
			}
			else
			{
				return RedirectToAction(nameof(ForeignTrainingList));
			}
		}

		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> CompleteTraining([FromForm] TrainingImplementationViewModel model)
		//{

		//    TrainingInfoNew data = new TrainingInfoNew
		//    {

		//        status = 3,
		//    };

		//    await trainingNewService.UpdateTrainingInfoNewById(data);

		//    if(model.trainingType == 1)
		//    {
		//        return RedirectToAction(nameof(TrainingList));
		//    }
		//    else
		//    {
		//        return RedirectToAction(nameof(ForeignTrainingList));
		//    }
		//}

		// GET: TrainingImplementation
		public async Task<IActionResult> TrainingList()
		{
			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
			string org = user.org;

			TrainingImplementationViewModel model = new TrainingImplementationViewModel
			{
				fLang = _lang.PerseLang("TrainingNew/TrainingImplementationEN.json", "TrainingNew/TrainingImplementationBN.json", Request.Cookies["lang"]),
				employeeTypes = await typeService.GetAllEmployeeType(),
				trainingInfoNews = await trainingNewService.GetTrainingInfoNewByType(1, org)
			};
			return View(model);
		}

		public async Task<IActionResult> ForeignTrainingList()
		{
			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
			string org = user.org;

			TrainingImplementationViewModel model = new TrainingImplementationViewModel
			{
				fLang = _lang.PerseLang("TrainingNew/TrainingImplementationEN.json", "TrainingNew/TrainingImplementationBN.json", Request.Cookies["lang"]),
				employeeTypes = await typeService.GetAllEmployeeType(),
				//trainingInfoNews = await trainingNewService.GetTrainingInfoNewByType(2, org),
				trainingInfoNews = await trainingNewService.GetTrainingInfoNews()
			};
			return View(model);
		}
		public async Task<IActionResult> GetTrainerByTrainingId(int? id)
		{
			return Json(await trainingNewService.GetTrainerInfoNewByTraining(id));
		}
		// GET: TrainingListForEvaluation
		public async Task<IActionResult> TrainingListForEvaluation()
		{
			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
			string org = user.org;

			TrainingImplementationViewModel model = new TrainingImplementationViewModel
			{
				fLang = _lang.PerseLang("TrainingNew/TrainingImplementationEN.json", "TrainingNew/TrainingImplementationBN.json", Request.Cookies["lang"]),
				employeeTypes = await typeService.GetAllEmployeeType(),
				trainingInfoNews = await trainingNewService.GetTrainingInfoNewByStatusandType(2,1,org)
			};
			return View(model);
		}

		public async Task<IActionResult> ForeignTrainingListForEvaluation()
		{
			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
			string org = user.org;

			TrainingImplementationViewModel model = new TrainingImplementationViewModel
			{
				fLang = _lang.PerseLang("TrainingNew/TrainingImplementationEN.json", "TrainingNew/TrainingImplementationBN.json", Request.Cookies["lang"]),
				employeeTypes = await typeService.GetAllEmployeeType(),
				trainingInfoNews = await trainingNewService.GetTrainingInfoNewByStatusandType(2,2,org)
			};
			return View(model);
		}

		// GET: TrainingImplementation
		public async Task<IActionResult> Enroll(int id)
		{
			ViewBag.TrainingPlanId = id;
			TrainingEnrollViewModel model = new TrainingEnrollViewModel
			{
				fLang = _lang.PerseLang("TrainingNew/TrainingImplementationEN.json", "TrainingNew/TrainingImplementationBN.json", Request.Cookies["lang"]),
				employeeTypes = await typeService.GetAllEmployeeType(),
				trainingInfoNew = await trainingNewService.GetTrainingInfoNewById(id),
				//enrolledTrainees = await enrollTraineeService.GetEnrolledTraineeByPlannId(id),
				enrolledTraineees = await enrollTraineeService.GetEnrolledTraineeByTraingingId(id),
				trainingInfoNews = await trainingNewService.GetTrainingInfoNewByStatus(2)
			};
			return View(model);
		}


        [AllowAnonymous]
        public async Task<ActionResult> TrainingLetterformatView(int trainingId, int participantId)
        {


            var trainings = new List<TrainingScheduleAll>();
            var trainingSchedules = await trainingNewService.GetAllTrainingSchedulesByTrainingIdSp(trainingId);
            //var Coordinatorsignature = await photographService.GetEmployeeSignatureByEmpId(trainingId);
            foreach (var item in trainingSchedules.Select(x => x.effectiveDate).Distinct())
            {
                trainings.Add(new TrainingScheduleAll
                {
                    effectiveDate = item,
                    trainingScheduleVms = trainingSchedules.Where(x => x.effectiveDate == item).OrderBy(x => x.effectiveDate).ThenBy(x => x.startTime).ToList(),
                });
            }




            TrainingEnrollViewModel model = new TrainingEnrollViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                trainingInfoNew = await trainingNewService.GetTrainingInfoNewById(trainingId),
                EnrolledTrainee = await enrollTraineeService.GetEnrolledTraineeById(participantId),
                trainingSchedules = trainings
            };
            return View(model);
        }
        [AllowAnonymous]
        public IActionResult TrainingLetterformatPdf(int trainingId, int participantId)
        {
            string schema = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string filename;

            url = schema + "://" + host + "/HRPMSTrainingNew/TrainingImplementation/TrainingLetterformatView?trainingId=" + trainingId + "&participantId=" + participantId;

            string status = myPDF.GeneratePDF(out filename, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }

        [AllowAnonymous]
        public async Task<IActionResult> TrainingLetterformatMail(int trainingId, int participantId)
        {
            string schema = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string filename;
            string filepath;

            url = schema + "://" + host + "/HRPMSTrainingNew/TrainingImplementation/TrainingLetterformatView?trainingId=" + trainingId + "&participantId=" + participantId;


            string status = myPDF.GeneratePDF(out filename, url);
            //var stream = new FileStream(rootPath + "/wwwroot/pdf/" + filename, FileMode.Open);

            var EnrolledTrainee =await enrollTraineeService.GetEnrolledTraineeByTraineeId(participantId);

            string subject = "Participation in the Workshop";
            string body = "<p>Dear Participant,</p><p>"+
                "As per approval of the Managing Director & CEO of the Bank, you have been selected"+
                " for the above workshop to be held at BDBL Training Institute.</p>";
            filepath = rootPath + "/wwwroot/pdf/" + filename;

            if (EnrolledTrainee.email != null)
            {
                await emailSenderService.SendEmailWithAttatchment(EnrolledTrainee.email, subject, body, filepath);
            }
            



            var trainings = new List<TrainingScheduleAll>();
            var trainingSchedules = await trainingNewService.GetAllTrainingSchedulesByTrainingIdSp(trainingId);
            //var Coordinatorsignature = await photographService.GetEmployeeSignatureByEmpId(trainingId);
            foreach (var item in trainingSchedules.Select(x => x.effectiveDate).Distinct())
            {
                trainings.Add(new TrainingScheduleAll
                {
                    effectiveDate = item,
                    trainingScheduleVms = trainingSchedules.Where(x => x.effectiveDate == item).OrderBy(x => x.effectiveDate).ThenBy(x => x.startTime).ToList(),
                });
            }
            TrainingEnrollViewModel model = new TrainingEnrollViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                trainingInfoNew = await trainingNewService.GetTrainingInfoNewById(trainingId),
                EnrolledTrainee = await enrollTraineeService.GetEnrolledTraineeById(participantId),
                trainingSchedules = trainings
            };
            return View(model);

        }










        public async Task<IActionResult> MyTrainigList(int id)
		{
			string userName = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(userName);
			var employee = await personalInfoService.GetEmployeeInfoByApplicationId(user.Id);

			ViewBag.TrainingPlanId = id;
			TrainingEnrollViewModel model = new TrainingEnrollViewModel
			{
				fLang = _lang.PerseLang("TrainingNew/TrainingImplementationEN.json", "TrainingNew/TrainingImplementationBN.json", Request.Cookies["lang"]),
				employeeTypes = await typeService.GetAllEmployeeType(),
				trainingInfoNew = await trainingNewService.GetTrainingInfoNewById(id),
				myTraining = await trainingNewService.GetEnrolledTrainingByEmpId(employee.Id),
				indTrainingReportSPs = await trainingNewService.GetTraingInfoSPByeempId(employee.Id),
				enrolledTrainees = await enrollTraineeService.GetEnrolledTraineeByPlannId(id),
				trainingInfoNews = await trainingNewService.GetTrainingInfoNewByStatus(1)
			};
			return View(model);

			//string userName = HttpContext.User.Identity.Name;
			//var user = await employeePunchCardInfoService.GetUserByUsername(userName);
			//var model = new TrainingEnrollViewModel
			//{
			//    applicationUser = user
			//};


			//return View(model);


		}

		[HttpGet]
		[AllowAnonymous]
		[Route("api/global/MyTrainigListApi/{empCode}")]
		public async Task<IActionResult> MyTrainigListApi(string empCode)
		{
			var user = await _userManager.FindByNameAsync(empCode);
			var employee = await personalInfoService.GetEmployeeInfoByApplicationId(user.Id);
			var model = await trainingNewService.GetTraingInfoSPByeempId(employee.Id);
			return Json(model);
		}

		// POST: TrainingImplementation/Index
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Enroll([FromForm] TrainingEnrollViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.fLang = _lang.PerseLang("TrainingNew/TrainingImplementationEN.json", "TrainingNew/TrainingImplementationBN.json", Request.Cookies["lang"]);
				model.trainingInfoNew = await trainingNewService.GetTrainingInfoNewById(model.id);
				model.enrolledTrainees = await enrollTraineeService.GetEnrolledTraineeByPlannId(model.id);
				return View(model);
			}

			//return Json(model);
			Nullable<int> id = null;
			if (model.employeeId == 0)
			{
				id = null;
			}
			else
			{
				id = model.employeeId;
			}

			EnrolledTrainee data = new EnrolledTrainee
			{
				employeeId = id,
				designation = model.designation,
				name = model.employeeName,
				workingPlace = model.organization,
				address = model.address,
				mobile = model.mobile,
				email = model.email,
				trainingInfoNewId = model.id,
				isPresent = 0,
				marks = 0
			};

			await enrollTraineeService.SaveEnrolledTrainee(data);

			return RedirectToAction("Enroll", "TrainingImplementation", new
			{
				model.id
			});

		}

		// GET: TrainingImplementation
		public async Task<IActionResult> AssignResourcePerson(int id)
		{
			ViewBag.TrainingPlanId = id;
			TrainingEnrollViewModel model = new TrainingEnrollViewModel
			{
				fLang = _lang.PerseLang("TrainingNew/TrainingImplementationEN.json", "TrainingNew/TrainingImplementationBN.json", Request.Cookies["lang"]),
				employeeTypes = await typeService.GetAllEmployeeType(),
				trainingInfoNew = await trainingNewService.GetTrainingInfoNewById(id),
				enrolledTrainees = await enrollTraineeService.GetEnrolledTraineeByPlannId(id),
				trainingInfoNews = await trainingNewService.GetTrainingInfoNewByStatus(2),
				resourcePeople = await resourcePersonService.GetResourcePersonInfo(),
				trainingResourcePersons = await trainingResourcePersonService.GetTrainingResourcePersonByTrainingId(id)
			};
			return View(model);
		}

		// POST: TrainingImplementation/Index
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AssignResourcePerson([FromForm] TrainingEnrollViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.fLang = _lang.PerseLang("TrainingNew/TrainingImplementationEN.json", "TrainingNew/TrainingImplementationBN.json", Request.Cookies["lang"]);
				model.trainingInfoNew = await trainingNewService.GetTrainingInfoNewById(model.id);
				model.enrolledTrainees = await enrollTraineeService.GetEnrolledTraineeByPlannId(model.id);
				model.resourcePeople = await resourcePersonService.GetResourcePersonInfo();
				return View(model);
			}

			//return Json(model);

			TrainingResourcePerson data = new TrainingResourcePerson
			{
				trainingInfoNewId = model.id,
				resourcePersonId = Int32.Parse(model.resourcePersonId)
			};

			await trainingResourcePersonService.SaveTrainingResourcePerson(data);

			return RedirectToAction("AssignResourcePerson", "TrainingImplementation", new
			{
				model.id
			});
		}

		public async Task<IActionResult> TrainingScheduleMaker()
		{
			var data = new TrainingScheduleViewModel
			{
				trainingInfoNews = await trainingNewService.GetAllTrainingInfoNew(),
			};
			return View(data);
		}
		public async Task<IActionResult> CreateTrainingSchedule()
		{
			var data = new TrainingScheduleViewModel
			{
				trainingInfoNews = await trainingNewService.GetAllTrainingInfoNew(),
			};
			return View(data);
		}
		public async Task<IActionResult> GetEnrolledTraineeByPlannId(int id)
		{
			var data = await enrollTraineeService.GetEnrolledTraineeByPlannId(id);
			return Json(data);
		}

		public async Task<IActionResult> SaveSchedule(TrainingScheduleViewModel model)
		{
			try
			{
				var trainers = new List<string>();
				if (model.trainerId[0] != null)
				{
					foreach (var item in model.trainerId)
					{
						var data = await trainingResourcePersonService.GetResourcePersonById(Convert.ToInt32(item.Split('-')[0]));
						trainers.Add(data.Id + "-" + data.name + "-" + data.designation + "-" + data.workPlace + "-" + data.address);
					}
				}
				var schedule = new TrainingSchedule
				{
					Id = Convert.ToInt32(model.trainingScheduleId),
					effectiveDate = model.effectiveDate,
					startTime = model.startTime,
					endTime = model.endTime,
					trainers = trainers.Count() == 0 ? null : string.Join(',', trainers),
					trainingId = model.trainingId,
					isBreak = model.isBreak,
					topic = model.topic,
					sortOrder = model.sortOrder,
					trainerId = model.trainerSId
					
				};
				await trainingNewService.SaveTrainingSchedule(schedule);
				return Json(schedule.trainingId);
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		public async Task<IActionResult> GetAllScheduleByTrainingId(int? id)
		{
			var data = await trainingNewService.GetAllTrainingSchedulesByTrainingId(Convert.ToInt32(id));
			return Json(data);
		}
		public async Task<IActionResult> GetAllTrainingSchedules()
		{
			var data = await trainingNewService.GetAllTrainingSchedules();
			return Json(data);
		}


		public async Task<IActionResult> GetTrainerListByTrainingId(int? id)
		{
			var data = await trainingNewService.GetTrainerListByTrainingId(Convert.ToInt32(id));
			return Json(data);
		}


		public async Task<IActionResult> DeleteSchedule(int? id)
		{
			var data = await trainingNewService.DeleteScheduleById(Convert.ToInt32(id));

			return Json(data);
		}

		[AllowAnonymous]
		public async Task<IActionResult> GetTrainingScheduleByTrainingIdView(int trainingId)
		{
			var trainings = new List<TrainingScheduleAll>();
			var trainingSchedules = await trainingNewService.GetAllTrainingSchedulesByTrainingIdSp(trainingId);
			//var Coordinatorsignature = await photographService.GetEmployeeSignatureByEmpId(trainingId);
			foreach (var item in trainingSchedules.Select(x => x.effectiveDate).Distinct())
			{
				trainings.Add(new TrainingScheduleAll
				{
					effectiveDate = item,
					trainingScheduleVms = trainingSchedules.Where(x => x.effectiveDate == item).OrderBy(x=>x.effectiveDate).ThenBy(x => x.startTime).ToList(),
                });
			}
			var data = new TrainingScheduleViewModel
			{
				trainingSchedules = trainings,
				trainingInfoNews = await trainingNewService.GetTrainerInfoNewByTrainingId(trainingId),
                courseCoordinators = await trainingNewService.GetcourseCoordinatorByTrId(trainingId)

            };
			return View(data);
		}

		[AllowAnonymous]
		public IActionResult GetTrainingScheduleByTrainingIdPdf(int trainingId)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string fileName = "";
			var url = scheme + "://" + host + "/HRPMSTrainingNew/TrainingImplementation/GetTrainingScheduleByTrainingIdView?trainingId=" + trainingId;
			var status = myPDF.GeneratePDF(out fileName, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}
		[AllowAnonymous]
		public async Task<IActionResult> CourseWiseTrainingDateRangeView(int courseId, DateTime? startDate, DateTime? endDate)
		{
			ViewBag.startDate = startDate?.ToString("dd-MM-yyyy");
			ViewBag.endDate = endDate?.ToString("dd-MM-yyyy");
			var trainings = await trainingNewService.GetCourseWiseTrainingDateRange(courseId, startDate, endDate);
			TrainingEnrollViewModel model = new TrainingEnrollViewModel
			{
				courseWiseTrainings = trainings
			};
			return View(model);
		}

		[AllowAnonymous]
		public IActionResult CourseWiseTrainingDateRangePdf(int courseId, DateTime? startDate, DateTime? endDate)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string fileName = "";
			var url = scheme + "://" + host + "/HRPMSTrainingNew/TrainingImplementation/CourseWiseTrainingDateRangeView?courseId=" + courseId + "&startDate=" + startDate + "&endDate=" + endDate;
			var status = myPDF.GenerateLandscapePDFA4(out fileName, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}
		[AllowAnonymous]
		public async Task<IActionResult> InstituteWiseYearlyTrainingReportView(int year)
		{
			var trainings = await trainingNewService.GetTrainingsByYear(year);
			TrainingEnrollViewModel model = new TrainingEnrollViewModel
			{
				instituteTrainings = trainings
			};
			return View(model);
		}

		[AllowAnonymous]
		public IActionResult InstituteWiseYearlyTrainingReportPdf(int year)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string fileName = "";
			var url = scheme + "://" + host + "/HRPMSTrainingNew/TrainingImplementation/InstituteWiseYearlyTrainingReportView?year=" + year;
			var status = myPDF.GenerateLandscapePDFA4(out fileName, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}




		[AllowAnonymous]
		public async Task<IActionResult> InstituteWiseMonthlyTrainingReportView(int year, int month)
		{
			var trainings = await trainingNewService.GetTrainingsByYearAndMonth(year, month);
			TrainingEnrollViewModel model = new TrainingEnrollViewModel
			{
				instituteTrainings = trainings
			};
			return View(model);
		}
		[AllowAnonymous]
		public IActionResult InstituteWiseMonthlyTrainingReportPdf(int year, int month)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string fileName = "";
			var url = scheme + "://" + host + "/HRPMSTrainingNew/TrainingImplementation/InstituteWiseMonthlyTrainingReportView?year=" + year + "&month=" + month;
			var status = myPDF.GenerateLandscapePDFA4(out fileName, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}


		[AllowAnonymous]
		public async Task<IActionResult> yearlyTrainingView(int id)
		{
			var trainings = await trainingNewService.GetYealyTrainingsByTrainingYear(id);
			TrainingEnrollViewModel model = new TrainingEnrollViewModel
			{
				yearlyTrainings = trainings
			};
			return View(model);
		}
		[AllowAnonymous]
		public IActionResult yearlyTrainingPdf(int id)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string fileName = "";
			var url = scheme + "://" + host + "/HRPMSTrainingNew/TrainingImplementation/yearlyTrainingView?id=" + id;
			var status = myPDF.GeneratePDF(out fileName, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}

		[AllowAnonymous]
		public async Task<IActionResult> AttendanceListView(int id)
		{
			//var trainings = await trainingNewService.GetTrainingWithPerticipantsById(id);
			var trainings = await trainingNewService.GetTrainingWithPerticipantsByIdNew(id);
			//var rp = await trainingNewService.GetScheduleResourcePerson(id);
			TrainingEnrollViewModel model = new TrainingEnrollViewModel
			{
				trainingWithPerticipants = trainings,
				//resourcePeople = await trainingNewService.GetTrainingResourcePersonById(id),
				//ScheduleResourcePersons = rp
			};
			return View(model);
		}
		[AllowAnonymous]
		public IActionResult AttendanceListPdf(int id)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string fileName = "";
			var url = scheme + "://" + host + "/HRPMSTrainingNew/TrainingImplementation/AttendanceListView?id=" + id;
			var status = myPDF.GeneratePDF(out fileName, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}


        [AllowAnonymous]
        public async Task<IActionResult> AttendanceListUpdatedView(int id)
        {
            //var trainings = await trainingNewService.GetTrainingWithPerticipantsById(id);
            var trainings = await trainingNewService.GetTrainingWithPerticipantsByIdNew(id);
            var Coordinator = await trainingNewService.GetcourseCoordinatorByTrId(id);
            //var rp = await trainingNewService.GetScheduleResourcePerson(id);
            TrainingEnrollViewModel model = new TrainingEnrollViewModel
            {
                trainingWithPerticipants = trainings,
                //resourcePeople = await trainingNewService.GetTrainingResourcePersonById(id),
                //ScheduleResourcePersons = rp
                CourseCoordinators = Coordinator
            };
            return View(model);
        }
        [AllowAnonymous]
        public IActionResult AttendanceListUpdatedPdf(int id)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName = "";
            var url = scheme + "://" + host + "/HRPMSTrainingNew/TrainingImplementation/AttendanceListUpdatedView?id=" + id;
            var status = myPDF.GeneratePDF(out fileName, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }



        [AllowAnonymous]
		public async Task<IActionResult> RegistrationFormView(int id)
		{
			var trainings = await trainingNewService.GetTrainingWithPerticipantsByIdNew(id);
            
            TrainingEnrollViewModel model = new TrainingEnrollViewModel
            {
                trainingWithPerticipants = trainings,
            };
			return View(model);
		}
		[AllowAnonymous]
		public IActionResult RegistrationFormPdf(int id)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string fileName = "";
			var url = scheme + "://" + host + "/HRPMSTrainingNew/TrainingImplementation/RegistrationFormView?id=" + id;
			var status = myPDF.GenerateLandscapePDF(out fileName, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}


		[AllowAnonymous]
		public async Task<IActionResult> TrainingWithPerticipants(int id)
		{
			var trainings = await trainingNewService.GetTrainingWithPerticipantsByIdNew(id);
			TrainingEnrollViewModel model = new TrainingEnrollViewModel
			{
				//fLang = _lang.PerseLang("TrainingNew/TrainingImplementationEN.json", "TrainingNew/TrainingImplementationBN.json", Request.Cookies["lang"]),
				trainingWithPerticipants = trainings
				//trainingInfoNew = await trainingNewService.GetTrainingInfoNewById(id),
				//enrolledTrainees = await enrollTraineeService.GetEnrolledTraineeByPlannId(id),
				//trainingResourcePersons = await trainingResourcePersonService.GetTrainingResourcePersonByTrainingId(id)
			};
			return View(model);
		}

		[AllowAnonymous]
		public IActionResult TrainingWithPerticipantsPdf(int id)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string fileName = "";
			var url = scheme + "://" + host + "/HRPMSTrainingNew/TrainingImplementation/TrainingWithPerticipants?id=" + id;
			var status = myPDF.GeneratePDF(out fileName, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}

		public async Task<IActionResult> DetailsView(int id)
		{
			TrainingEnrollViewModel model = new TrainingEnrollViewModel
			{
				fLang = _lang.PerseLang("TrainingNew/TrainingImplementationEN.json", "TrainingNew/TrainingImplementationBN.json", Request.Cookies["lang"]),
				trainingInfoNew = await trainingNewService.GetTrainingInfoNewById(id),
				enrolledTrainees = await enrollTraineeService.GetEnrolledTraineeByPlannId(id),
				trainingResourcePersons = await trainingResourcePersonService.GetTrainingResourcePersonByTrainingId(id)
			};
			return View(model);
		}
		[AllowAnonymous]
		public async Task<IActionResult> TrainingDetailsView(int idr)
		{
			TrainingEnrollViewModel model = new TrainingEnrollViewModel
			{
				fLang = _lang.PerseLang("TrainingNew/TrainingImplementationEN.json", "TrainingNew/TrainingImplementationBN.json", Request.Cookies["lang"]),
				trainingInfoNew = await trainingNewService.GetTrainingInfoNewById(idr),
				enrolledTrainees = await enrollTraineeService.GetEnrolledTraineeByPlannId(idr),
				trainingResourcePersons = await trainingResourcePersonService.GetTrainingResourcePersonByTrainingId(idr)
			};
			return View(model);
		}
		[AllowAnonymous]
		public IActionResult GenerateTrainingDetailsPdf(int idr)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string fileName = "";
			var url = scheme + "://" + host + "/HRPMSTrainingNew/TrainingImplementation/TrainingDetailsView?idr=" + idr;
			var status = myPDF.GeneratePDF(out fileName, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}
		[HttpGet]
		public async Task<IActionResult> TrainingTAda()
		{
			TrainingTAdaViewModel model = new TrainingTAdaViewModel
			{
				trainingTAdaList = await trainingResourcePersonService.GetTrainingTaDaList(),
				trainingList = await trainingResourcePersonService.GetTrainingInfoNewList()
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> TrainingTAda([FromForm]TrainingTAdaViewModel model)
		{
			TrainingTaDa data = new TrainingTaDa
			{
				Id = Convert.ToInt32(model.Id),
				trainingId = model.trainingId,
				purpose = model.purpose,
				cost = model.cost,
				date = model.date
			};
			await trainingResourcePersonService.SaveTrainingTaDa(data);
			return RedirectToAction("TrainingTAda");
		}

		public async Task<IActionResult> DeleteTrainigTAda(int id)
		{
			await trainingResourcePersonService.DeleteTrainingTAdaById(id);
			return RedirectToAction("TrainingTAda");
		}


		[HttpGet]
		public async Task<IActionResult> GetTraineeInfoByTraining(int id)
		{
			var data = await trainingNewService.GetTrainingPerticipantsByTrainingId(id);
			return Json(data);
		}




		[HttpGet]
		public async Task<IActionResult> Certificate()
		{
			var data = new CertificateViewModel
			{
				employeeInfos = await personalInfoService.GetActiveAllEmployeeInfo()
			};

			return View(data);
		}

		[AllowAnonymous]
		public async Task<IActionResult> GenerateCertificate(int empId, int trainingId)
		{

			var data = new CertificateViewModel
			{
				employee = await personalInfoService.GetEmployeeInfoById(empId),
				//trainingInfoNews = await trainingNewService.GetAllTrainingInfoNew(),
				trainingInfoNews = await trainingNewService.GetTrainingInfoNewByIdNew(trainingId),
				enrolledTraineeinfo = await trainingNewService.GetEnrollTraineeById(empId, trainingId),

			};
			return View(data);
		}

		[AllowAnonymous]
		public IActionResult GenerateCertificatedPdf(int empId, int trainingId)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string fileName = "";
			var url = scheme + "://" + host + "/HRPMSTrainingNew/TrainingImplementation/GenerateCertificate?empId=" + empId + "&trainingId=" + trainingId;
			var status = myPDF.GeneratePDF(out fileName, url);

			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}


		[AllowAnonymous]
		public async Task<IActionResult> SpeakerEvaluationReportPdfByTrainingId(int trainingId)
		{
			TrainingEnrollViewModel model = new TrainingEnrollViewModel
			{
				trainingResourcePersons =await trainingResourcePersonService.GetTrainingResourcePersonByTrainingId(trainingId),
				trainingInfoNews = await trainingNewService.GetTrainerInfoNewByTrainingId(trainingId)

			};
			return View(model);
		}

		[AllowAnonymous]
		public IActionResult SpeakerEvaluationReportPdf(int trainingId)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string fileName = "";
			var url = scheme + "://" + host + "/HRPMSTrainingNew/TrainingImplementation/SpeakerEvaluationReportPdfByTrainingId?trainingId=" + trainingId;
			var status = myPDF.GenerateLandscapePDFA4(out fileName, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}


		[AllowAnonymous]
		public async Task<IActionResult> MoneyReceptReportPdfView(int Id)
		{

			TrainingEnrollViewModel model = new TrainingEnrollViewModel
			{
				trainingHonorariumViewModel= await trainingNewService.MoneyReceptReportPdf(Id)
				//trainingInfoNew = await trainingNewService.GetTrainingInfoNewById(MoneyReceipt),
				//enrolledTrainees = await enrollTraineeService.GetEnrolledTraineeByPlannId(MoneyReceipt),
				//trainingResourcePersons = await trainingResourcePersonService.GetTrainingResourcePersonByTrainingId(MoneyReceipt)
			};
			return View(model);
		}

		[AllowAnonymous]
		public IActionResult MoneyReceptReportPdf(int Id)
		{



			string scheme = Request.Scheme;
			var host = Request.Host;
			string fileName = "";
			var url = scheme + "://" + host + "/HRPMSTrainingNew/TrainingImplementation/MoneyReceptReportPdfView?id="+Id+"";
			var status = myPDF.GeneratePDF(out fileName, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}


		[AllowAnonymous]
		public async Task<IActionResult> CourseWisePerticipientsInfoView(int courseId, DateTime? startDate, DateTime? endDate)
		{
			ViewBag.startDate = startDate?.ToString("dd-MM-yyyy");
			ViewBag.endDate = endDate?.ToString("dd-MM-yyyy");
			var trainings = await trainingNewService.GetCourseWisePerticipientsDateRange(courseId, startDate, endDate);
			TrainingEnrollViewModel model = new TrainingEnrollViewModel
			{
				courseWiseParticipents = trainings
			};
			return View(model);
		}
		[AllowAnonymous]
		public IActionResult CourseWisePerticipientsInfoPdf(int courseId, DateTime? startDate, DateTime? endDate)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string fileName = "";
			var url = scheme + "://" + host + "/HRPMSTrainingNew/TrainingImplementation/CourseWisePerticipientsInfoView?courseId=" + courseId + "&startDate=" + startDate + "&endDate=" + endDate;
			var status = myPDF.GenerateLandscapePDFA4(out fileName, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}

		[AllowAnonymous]
		public async Task<IActionResult> TrainingInfoForBangladeshBankManPowerView(int year, int month)
		{
			ViewBag.year = year;
			ViewBag.month = month;

			var trainings = await trainingNewService.GetTrainingInfoForBangladeshBankManPower(year,month);
			TrainingEnrollViewModel model = new TrainingEnrollViewModel
			{
				bangladeshBankManpowerReportVMs = trainings
			};
			return View(model);
		}

		[AllowAnonymous]
		public IActionResult TrainingInfoForBangladeshBankManPowerPdf(int year, int month)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string fileName = "";
			var url = scheme + "://" + host + "/HRPMSTrainingNew/TrainingImplementation/TrainingInfoForBangladeshBankManPowerView?year=" + year + "&month=" + month;
			var status = myPDF.GenerateLandscapePDF(out fileName, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}


		[AllowAnonymous]
		public async Task<IActionResult> TrainingReportByInstituteIdView()
		{
			var trainings = await trainingNewService.GetTrainingsByInstituteId();
			TrainingEnrollViewModel model = new TrainingEnrollViewModel
			{
				instituteTrainingReportViewModels = trainings
			};
			return View(model);
		}

		[AllowAnonymous]
		public IActionResult TrainingReportByInstituteIdViewPdf()
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string fileName = "";
			var url = scheme + "://" + host + "/HRPMSTrainingNew/TrainingImplementation/TrainingReportByInstituteIdView";
			var status = myPDF.GenerateLandscapePDFA4(out fileName, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}



		[AllowAnonymous]
		public async Task<IActionResult> GetTrainingByParticipantsIdView(int id, DateTime? endDate)
		{
			ViewBag.Date = endDate;
			TrainingEnrollViewModel model = new TrainingEnrollViewModel
			{
				enrolledTraineeForTraining= await enrollTraineeService.GetEnrolledTraineeById(id),
				myTraining = await enrollTraineeService.GetTrainingInfoByparticipatId(id, endDate),
			};
			return View(model);
		}

		[AllowAnonymous]
		public IActionResult GetTrainingByParticipantsIdPdf(int id, DateTime? endDate)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string fileName = "";
			var url = scheme + "://" + host + "/HRPMSTrainingNew/TrainingImplementation/GetTrainingByParticipantsIdView?id=" + id + "&endDate=" + endDate;
			var status = myPDF.GeneratePDF(out fileName, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}
		#region 
		[AllowAnonymous]
		public async Task<IActionResult> EmployeeSummaryReportView()
		{
			var summary = await trainingNewService.GetManpowerSummaryList();
			ManpowerSummaryViewModel model = new ManpowerSummaryViewModel
			{
				manpowerSummaryList = summary
			};
			return View(model);
		}

		[AllowAnonymous]
		public IActionResult EmployeeSummaryReportViewPdf()
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string fileName = "";
			var url = scheme + "://" + host + "/HRPMSTrainingNew/TrainingImplementation/EmployeeSummaryReportView";
			var status = myPDF.GenerateLandscapePDFA4(out fileName, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}


		[AllowAnonymous]
		public IActionResult TrainingWorkshopBankReportViewPdf(int year,int month )
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string fileName = "";
			var url = scheme + "://" + host + "/HRPMSTrainingNew/TrainingImplementation/TrainingWorkshopBankReportView?year=" + year + "&month=" + month;
			var status = myPDF.GenerateLandscapePDFA4(out fileName, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}
		[AllowAnonymous]
		public IActionResult TrainingWorkshopBDReportViewPdf(int year)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string fileName = "";
			var url = scheme + "://" + host + "/HRPMSTrainingNew/TrainingImplementation/TrainingWorkshopBDReportView?year=" + year;
			var status = myPDF.GenerateLandscapePDFA4(out fileName, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}
		[AllowAnonymous]
		public async Task<IActionResult> TrainingWorkshopBankReportView(string type, int trainingid, int localOforeign, int year,int month)
		{
			ViewBag.year = year;
			ViewBag.month = month;
			var trainings = await trainingNewService.GetTrainingsByYearAndMonthforBank("ALL", 0, 0, year, month);
			TrainingEnrollViewModel model = new TrainingEnrollViewModel
			{

				TrainingofonlyBD = trainings
			};
			return View(model);
		}
		[AllowAnonymous]
		public async Task<IActionResult> TrainingWorkshopBDReportView(string type, int trainingid, int localOforeign, int year, int month)
		{
			ViewBag.year = year;

			var trainings = await trainingNewService.GetTrainingsByYearAndMonthforBank("ALL", 0, 2, year, month);

			TrainingEnrollViewModel model = new TrainingEnrollViewModel
			{
				TrainingofonlyBD = trainings
			};
			return View(model);
		}
        #endregion
        [AllowAnonymous]
        public async Task<IActionResult> BannerReportView()
        {
            var summary = await trainingNewService.GetManpowerSummaryList();
            ManpowerSummaryViewModel model = new ManpowerSummaryViewModel
            {
                manpowerSummaryList = summary
            };
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult BannerReportViewPdf()
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName = "";
            var url = scheme + "://" + host + "/HRPMSTrainingNew/TrainingImplementation/BannerReportView";
            var status = myPDF.GenerateLandscapePDFA4(out fileName, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }


        [AllowAnonymous]
        public async Task<IActionResult> ApplicationTrainningReportView()
        {
            var summary = await trainingNewService.GetManpowerSummaryList();
            ManpowerSummaryViewModel model = new ManpowerSummaryViewModel
            {
                manpowerSummaryList = summary
            };
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult ApplicationTrainningReportViewPdf()
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName = "";
            var url = scheme + "://" + host + "/HRPMSTrainingNew/TrainingImplementation/ApplicationTrainningReportView";
            var status = myPDF.GeneratePDF(out fileName, url);
            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }
            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }




        
    }
}