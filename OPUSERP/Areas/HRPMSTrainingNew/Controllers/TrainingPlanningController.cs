using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSTrainingNew.Models;
using OPUSERP.Areas.HRPMSTrainingNew.Models.Lang;
using OPUSERP.Data.Entity;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.TrainingNew;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.HRPMS.Services.TrainingNew.Interfaces;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using OPUSERP.Data.Entity.Training;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using System.IO;
using DinkToPdf.Contracts;
using OPUSERP.Areas.HRPMSReport.Models;

namespace OPUSERP.Areas.HRPMSTrainingNew.Controllers
{
    [Authorize]
    [Area("HRPMSTrainingNew")]
    public class TrainingPlanningController : Controller
    {
        private readonly LangGenerate<TrainingPlanningLn> _lang;
        private readonly ITypeService typeService;
        private readonly ITrainingNewService trainingNewService;
        private readonly IYearCourseTitleService yearCourseTitleService;
        private readonly IAddressService addressService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITrainingService trainingService;
        private readonly IResourcePersonService resourcePersonService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly string rootPath;
        private readonly MyPDF myPDF;

        public TrainingPlanningController(IHostingEnvironment hostingEnvironment, IConverter converter, IPersonalInfoService personalInfoService, IAddressService addressService, IYearCourseTitleService yearCourseTitleService, ITypeService typeService, ITrainingNewService trainingNewService, UserManager<ApplicationUser> userManager, ITrainingService trainingService, IResourcePersonService resourcePersonService)
        {
            _lang = new LangGenerate<TrainingPlanningLn>(hostingEnvironment.ContentRootPath);
            this.typeService = typeService;
            this.trainingNewService = trainingNewService;
            this.yearCourseTitleService = yearCourseTitleService;
            this.addressService = addressService;
            this.trainingService = trainingService;
            this.personalInfoService = personalInfoService;
            this.resourcePersonService = resourcePersonService;
            _userManager = userManager;
            myPDF = new MyPDF(hostingEnvironment, converter);
            rootPath = hostingEnvironment.ContentRootPath;
        }



        public async Task<ActionResult> TrainingReportAll()
        {
            var model = new TrainingWithAttendanceDetails
            {
                //specialBranchUnits = await specialBranchUnitService.GetSpecialBranchUnit(),
                //subjects = await subjectService.GetAllSubject(),
                //organizations = await organizationService.GetAllOrganization(),
                //religions = await organizationService.GetAllReligions(),
                //divisions = await organizationService.GetAllDivisions(),
                courseTitles = await yearCourseTitleService.GetCourseTitle(),
                AlltrainingInfoNews = await trainingNewService.GetAllTrainingInfoNewReport(),
                AllEnrollTrainee = await resourcePersonService.GetAllEnrollTrainee(),
				employeeInfoList = await trainingNewService.GetemployeeInfos()

			};
            return View(model);
        }


        // GET: TrainingPlanning
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            string org = user.org;

            //return Json(org);

            TrainingPlanningViewModel model = new TrainingPlanningViewModel
            {
                fLang = _lang.PerseLang("TrainingNew/TrainingPlaneEN.json", "TrainingNew/TrainingPlaneBN.json", Request.Cookies["lang"]),
                employeeTypes = await typeService.GetAllEmployeeType(),
                trainingInfoNews = await trainingNewService.GetTrainingInfoNewByType(1, org),
                courseTitles = await yearCourseTitleService.GetCourseTitle(),
                years = await yearCourseTitleService.GetYear()
            };
            return View(model);
        }

        public async Task<IActionResult> Foreign(int id = 0)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            string org = user.org;

            TrainingPlanningViewModel model = new TrainingPlanningViewModel
            {
                fLang = _lang.PerseLang("TrainingNew/TrainingPlaneEN.json", "TrainingNew/TrainingPlaneBN.json", Request.Cookies["lang"]),
                employeeTypes = await typeService.GetAllEmployeeType(),
                trainingInfoNews = await trainingNewService.GetTrainingInfoNewByType(2, org),
                courseTitles = await yearCourseTitleService.GetCourseTitle(),
                years = await yearCourseTitleService.GetYear(),
                countries = await addressService.GetAllContry(),
                trainingInstitutes = await trainingService.GetTrainingInstitute(),
                trainingSubjects = await trainingService.GetTrainingSubject(),
                //resourcePeople = await resourcePersonService.GetResourcePersonInfoByOrg(org)
                resourcePeople = await resourcePersonService.GetResourcePersonInfoByOrgNew(),
				sourceOfFundList = await trainingNewService.GetSourceOfFund(),
				courseTypeList = await trainingNewService.GetCourseType(),
				employeeInfoList = await trainingNewService.GetemployeeInfos(),
				trainingInfoNewsSingle = await trainingNewService.GetTrainingInfoNewById(id), 
			};
			ViewBag.TrainingId = id;
            return View(model);
        }
		public async Task<IActionResult> UpdateTraining(int id) 
		{
			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
			string org = user.org;

			TrainingPlanningViewModel model = new TrainingPlanningViewModel
			{
				fLang = _lang.PerseLang("TrainingNew/TrainingPlaneEN.json", "TrainingNew/TrainingPlaneBN.json", Request.Cookies["lang"]),
				employeeTypes = await typeService.GetAllEmployeeType(),
				trainingInfoNews = await trainingNewService.GetTrainingInfoNewByType(2, org),
				courseTitles = await yearCourseTitleService.GetCourseTitle(),
				years = await yearCourseTitleService.GetYear(),
				countries = await addressService.GetAllContry(),
				trainingInstitutes = await trainingService.GetTrainingInstitute(),
				trainingSubjects = await trainingService.GetTrainingSubject(),
				//resourcePeople = await resourcePersonService.GetResourcePersonInfoByOrg(org)
				resourcePeople = await resourcePersonService.GetResourcePersonInfoByOrgNew(),
				sourceOfFundList = await trainingNewService.GetSourceOfFund(),
				courseTypeList = await trainingNewService.GetCourseType(),
				employeeInfoList = await trainingNewService.GetemployeeInfos(),
				trainingInfoNewsSingle = await trainingNewService.GetTrainingInfoNewById(id),
			};
			ViewBag.TrainingId = id;
			return View(model);
		}

		public async Task<IActionResult> AllTrainingList()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            string org = user.org;

            TrainingPlanningViewModel model = new TrainingPlanningViewModel
            {
                fLang = _lang.PerseLang("TrainingNew/TrainingPlaneEN.json", "TrainingNew/TrainingPlaneBN.json", Request.Cookies["lang"]),
                employeeTypes = await typeService.GetAllEmployeeType(),
                //trainingInfoNews = await trainingNewService.GetTrainingInfoNewByType(2, org),
                trainingInfoNews = await trainingNewService.GetTrainingInfoNews(),
                courseTitles = await yearCourseTitleService.GetCourseTitle(),
                years = await yearCourseTitleService.GetYear(),
                countries = await addressService.GetAllContry(),
                trainingInstitutes = await trainingService.GetTrainingInstitute(),
                trainingSubjects = await trainingService.GetTrainingSubject(),
                resourcePeople = await resourcePersonService.GetResourcePersonInfoByOrg(org),
				sourceOfFundList = await trainingNewService.GetSourceOfFund(),
				courseTypeList = await trainingNewService.GetCourseType(),
				employeeInfoList = await trainingNewService.GetemployeeInfos()
			};
            return View(model);
        }
		public async Task<IActionResult> GetTrainingResourcePersonById(int id) 
		{
		    var data = await trainingNewService.GetTrainingResourcePersonById(id); 
		     return Json(data);
		}
        public async Task<IActionResult> GetCourseCoordinatorById(int id) 
		{
		    var data = await trainingNewService.GetcourseCoordinatorByTrId(id); 
		     return Json(data);
		}

		public async Task<IActionResult> TrainingCalender()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            string org = user.org;

            //return Json(org);

            TrainingPlanningViewModel model = new TrainingPlanningViewModel
            {
                fLang = _lang.PerseLang("TrainingNew/TrainingPlaneEN.json", "TrainingNew/TrainingPlaneBN.json", Request.Cookies["lang"]),
                employeeTypes = await typeService.GetAllEmployeeType(),
                trainingInfoNews = await trainingNewService.GetTrainingInfoNewByType(1, org),
                courseTitles = await yearCourseTitleService.GetCourseTitle(),
                years = await yearCourseTitleService.GetYear()
            };
            return View(model);
        }

        public async Task<IActionResult> MyTrainingCalender(int id)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            string org = user.org;
            ViewBag.employeeID = id.ToString();
            //return Json(org);

            TrainingPlanningViewModel model = new TrainingPlanningViewModel
            {
                employeeID = id.ToString(),
                fLang = _lang.PerseLang("TrainingNew/TrainingPlaneEN.json", "TrainingNew/TrainingPlaneBN.json", Request.Cookies["lang"]),
                employeeTypes = await typeService.GetAllEmployeeType(),
                trainingInfoNews = await trainingNewService.GetTrainingInfoNewByType(1, org),
                courseTitles = await yearCourseTitleService.GetCourseTitle(),
                years = await yearCourseTitleService.GetYear(),
                employeeInfo = await personalInfoService.GetEmployeeInfoById(id),
                trainingPerticipants = await trainingNewService.GetTrainingPerticipantsByEmpId(id)
            };
            return View(model);
        }
		[HttpPost]
		public async Task<IActionResult> SaveIsCompleteTraining(int? id)
		{
			TrainingInfoNew data = new TrainingInfoNew
			{
				Id = Convert.ToInt32(id),
				status = 255,
			};
			var result = await trainingNewService.SaveIsCompleteTraining(data);
			return Json(result);
		}

		// POST: TrainingPlanning/Edit
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] TrainingPlanningViewModel model)
        {
			//return Json(model);
			//if (!ModelState.IsValid)
			//{
			//	model.fLang = _lang.PerseLang("TrainingNew/TrainingPlaneEN.json", "TrainingNew/TrainingPlaneBN.json", Request.Cookies["lang"]);
			//	model.trainingInfoNews = await trainingNewService.GetTrainingInfoNew();
			//	return View(model);
			//}

			//return Json(model);

			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            string org = user.org;

            //var arr = await typeService.GetTypNamesByIds(model.employeeTypeMultiple);

            TrainingInfoNew data = new TrainingInfoNew
            {
                Id = Convert.ToInt32(model.planningId),
                course = model.course,
                courseObjective = model.courseObjective,
                amount = model.amount,
                countryId = model.country,
                startDate = model.planeStartDate,
                endDate = model.planeEndDate,
                year = model.year,
                noOfParticipants = model.participant,
                trainerId = model.trainerId,
                subjectId = model.subjectId,
                budget = model.budget,
                //employeeTypeNames = String.Join(", ", arr),
                //employeeTypes = String.Join(",", model.employeeTypeMultiple),
                location = model.location,
                trainingInstituteId = model.trainingInstituteId,
                remarks = model.remark,
                startTime = model.startTime,
                endTime = model.endTime,
                durationMinutes = model.durationMinutes,
                //employeeTypeId = model.employeeType,
                trainingType = model.trainingType,
				//orgType = org,
				//status = 1,

				//instituteId = model.trainingInstitutes,
				//
				sourceOfFundId= model.sourceOfFundId,
				courseTypeId= model.courseTypeId,
				employeeInfoId = model.employeeInfoId

			};

            var trainingInfoNewId = await trainingNewService.SaveTrainingInfoNew(data);
			List<TrainingResourcePerson> trainingResourcePersonList = new List<TrainingResourcePerson>();
			if (model.resourcePersonIdList!=null  )
			{
				string[] idList = model.resourcePersonIdList.Split(",");
				foreach (string id in idList)
				{
					TrainingResourcePerson trainingResourcePerson = new TrainingResourcePerson()
					{
						trainingInfoNewId = trainingInfoNewId,
						resourcePersonId = (int.Parse(id))
					};
					trainingResourcePersonList.Add(trainingResourcePerson);
				}
				if (trainingResourcePersonList.Count > 0)
				{
					// Save TrainingResourcePerson 
					if (model.planningId == null || model.planningId == 0)
					{
						var firstid = await trainingNewService.SaveTrainingResourcePerson(trainingResourcePersonList);
					}
					else if(model.planningId > 0)
					{
					// Update TrainingResourcePerson 
						var firstid = await trainingNewService.UpdateTrainingResourcePerson(trainingResourcePersonList); 
					}
					
				}
			}


            List<CourseCoordinator> courseCoordinatorList = new List<CourseCoordinator>();
            if (model.resourcePersonIdList != null)
            {
                string[] idList1 = model.coordinatorIdList.Split(",");
                foreach (string id in idList1)
                {
                    CourseCoordinator courseCoordinator = new CourseCoordinator()
                    {
                        trainingInfoNewId = trainingInfoNewId,
                        employeeInfoId = (int.Parse(id))
                    };
                    courseCoordinatorList.Add(courseCoordinator);
                }
                if (courseCoordinatorList.Count > 0)
                {
                    // Save TrainingResourcePerson 
                    if (model.planningId == null || model.planningId == 0)
                    {
                        var firstid1 = await trainingNewService.SaveCoordinator(courseCoordinatorList);
                    }
                    else if (model.planningId > 0)
                    {
                        // Update TrainingResourcePerson 
                        var firstid1 = await trainingNewService.UpdateCoordinator(courseCoordinatorList);
                    }

                }
            }


            if (model.trainingType == 1)
            {
                return RedirectToAction(nameof(AllTrainingList));
            }
            else
            {
                return RedirectToAction(nameof(AllTrainingList));
            }
        }


		public async Task<IActionResult> GetFinishedTrainings()
		{
			var data = await yearCourseTitleService.GetAllFinishedTrainingList();
			return Json(data);
		}
        public async Task<IActionResult> GetTrainingInfo(int id)
        {
            TrainingPlanningViewModel model = new TrainingPlanningViewModel
            {
                trainingInfoNewsSingle = await yearCourseTitleService.GetTrainingInfoById(id),
                resourcePeople= await trainingNewService.GetTrainingResourcePersonById(id),
            };

			return Json(model);
		}
        public async Task<IActionResult> GetPerticipantsByTrainingId(int id)
		{
			var data = await yearCourseTitleService.GetPerticipantsByTrainingId(id);
			return Json(data);
		}
		public async Task<IActionResult> GetAllTraining()
		{
			var data = await yearCourseTitleService.GetAllTrainingList();
			return Json(data);
		}
        public async Task<IActionResult> GetAllPerticipants()
		{
			var data = await yearCourseTitleService.GetAllTrainingPerticipants();
			return Json(data);
		}

		[HttpGet]
		public async Task<IActionResult> TrainingPerticipants()
		{
            var data = new TrainingPerticipantViewModel
            {
                employeeInfos = await personalInfoService.GetActiveAllEmployeeInfo()
            };

            return View(data);
		}
		[HttpPost]
		public async Task<IActionResult> TrainingPerticipants(TrainingPerticipantViewModel model)
		{
			//perticipant mean employee
			foreach (var item in model.perticipants)
			{
				var data = new TrainingPerticipants
				{
					Id = 0,
					trainingId = model.trainingId,
					traineeId = item //employeeid
				};
				await yearCourseTitleService.SaveTrainingPerticipant(data);
			}
			return Json("ok");
		}

		public async Task<IActionResult> GetTrainingWithPerticipants(int id) //trainingId
		{
			var allPerticipants = await yearCourseTitleService.GetPerticipantsByTrainingId(id);
			return Json(allPerticipants);
		}

        #region Training_Perticipants

        #endregion

        #region Training_Attendance 

        [HttpGet]
        public async Task<IActionResult> TrainingAttendance()
        {
            TrainingPlanningViewModel model = new TrainingPlanningViewModel
            {
                trainingPerticipants = await yearCourseTitleService.GetAllTrainingPerticipants(),
                trainingInfoNews = await trainingNewService.GetAllTrainingInfoNew(),


            };
            return View(model);
            
            
        }
		public async Task<IActionResult> TrainingAttendanceNew(int id)
        {
			var data = new TrainingWithTrainees
			{
				training = await trainingNewService.GetTrainingInfoNewById(id),
				trainees = await trainingNewService.GetTrainingPerticipantsByTrainingId(id)
			};
			var model = new TrainingWithAttendanceDetails
			{
				trainingWithTrainees = data
			};
			return View(model);
        }

		[HttpPost]
		public async Task<IActionResult> TrainingAttendanceNew(TrainingWithAttendanceDetails model)
		{
			for (int i = 0; i < model.traineeId.Length; i++)
			{
				var trainee = await trainingNewService.GetTrainingPerticipantsById(model.traineeId[i]);
				trainee.isPresent = model.isPresentStatus[i];
				trainee.marks = model.marks[i];

				await trainingNewService.SaveEnrollTrainee(trainee);
			}
			return RedirectToAction("TrainingAttendanceNew", new { id = model.trainingId });
		}
        public async Task<IActionResult> TrainingRegisterReport()
        {
            var data = new TrainingWithAttendanceDetails
            {
				trainingInfoNews = await trainingNewService.GetAllTrainingInfoNewcomplited(),
				trainers = await trainingNewService.GetAllTrainer()
            };
            return View(data);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllTrainingRegisterView(DateTime? start, DateTime? end, int trainerId)
        {
            var model = new List<TrainingWithPerticipantVm>();

            if ((start != null && end != null) && trainerId == 0)
            {
                var data = await trainingNewService.GetTrainingInfoNewByDate(start, end);
                foreach (var item in data)
                {
                    model.Add(new TrainingWithPerticipantVm
                    {
                        training = item,
                        trainees = await trainingNewService.GetTrainingPerticipantsByTrainingId(item.Id)
                    });
                }
            }
            else if ((start != null && end != null) && trainerId > 0)
            {
                var data = await trainingNewService.GetTrainingInfoNewByDateAndTrainerId(start, end, trainerId);
                foreach (var item in data)
                {
                    model.Add(new TrainingWithPerticipantVm
                    {
                        training = item,
                        trainees = await trainingNewService.GetTrainingPerticipantsByTrainingId(item.Id)
                    });
                }
            }
            else if ((start == null && end == null) && trainerId > 0)
            {
                var data = await trainingNewService.GetTrainingInfoNewByTrainerId(trainerId);
                foreach (var item in data)
                {
                    model.Add(new TrainingWithPerticipantVm
                    {
                        training = item,
                        trainees = await trainingNewService.GetTrainingPerticipantsByTrainingId(item.Id)
                    });
                }
            }
            else
            {
                var data = await trainingNewService.GetAllTrainingInfoNew();
                foreach (var item in data)
                {
                    model.Add(new TrainingWithPerticipantVm
                    {
                        training = item,
                        trainees = await trainingNewService.GetTrainingPerticipantsByTrainingId(item.Id)
                    });
                }
            }
            return View(model);
        }

        public IActionResult GetTrainingRegisterPdf(DateTime? start, DateTime? end, int trainerId)
        {

            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/HRPMSTrainingNew/TrainingPlanning/GetAllTrainingRegisterView?start=" + start + "&end=" + end + "&trainerId=" + trainerId;

            string fileName;
            string status = myPDF.GeneratePDF(out fileName, url);
            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }

        public async Task<IActionResult> TrainingEvaluationReport()
        {
            var data = new TrainingWithAttendanceDetails
            {
                trainers = await trainingNewService.GetAllTrainer(),
                trainingInfoNews = await trainingNewService.GetAllTrainingInfoNew(),
            };
            return View(data);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllTrainingEvaluationView(DateTime? start, DateTime? end, int? trainerId, int? TrainingId)
        {
            var model = new List<TrainingWithPerticipantVm>();

            if ((start != null && end != null) && trainerId == 0 && TrainingId == 0)
            {
                var data = await trainingNewService.GetTrainingInfoNewByDate(start, end);
                foreach (var item in data)
                {
                    model.Add(new TrainingWithPerticipantVm
                    {
                        training = item,
                        trainees = await trainingNewService.GetTrainingPerticipantsByTrainingId(item.Id)
                    });
                }
            }
            else if ((start != null && end != null) && trainerId > 0 && TrainingId == 0)
            {
                var data = await trainingNewService.GetTrainingInfoNewByDateAndTrainerId(start, end, trainerId);
                foreach (var item in data)
                {
                    model.Add(new TrainingWithPerticipantVm
                    {
                        training = item,
                        trainees = await trainingNewService.GetTrainingPerticipantsByTrainingId(item.Id)
                    });
                }
            }
            else if ((start == null && end == null) && trainerId > 0 && TrainingId == 0)
            {
                var data = await trainingNewService.GetTrainingInfoNewByTrainerId(trainerId);
                foreach (var item in data)
                {
                    model.Add(new TrainingWithPerticipantVm
                    {
                        training = item,
                        trainees = await trainingNewService.GetTrainingPerticipantsByTrainingId(item.Id)
                    });
                }
            }
            else if ((start != null && end != null) && TrainingId > 0 && trainerId > 0)
            {
                var data = await trainingNewService.GetTrainingInfoByDateTrainingIdandTrainerId(start, end, TrainingId, trainerId);
                foreach (var item in data)
                {
                    model.Add(new TrainingWithPerticipantVm
                    {
                        training = item,
                        trainees = await trainingNewService.GetTrainingPerticipantsByTrainingId(item.Id)
                    });
                }
            }
            else if ((start == null && end == null) && TrainingId > 0 && trainerId > 0)
            {
                var data = await trainingNewService.GetTrainingInfoNewByTrainerIdandTrainingId(trainerId, TrainingId);
                foreach (var item in data)
                {
                    model.Add(new TrainingWithPerticipantVm
                    {
                        training = item,
                        trainees = await trainingNewService.GetTrainingPerticipantsByTrainingId(item.Id)
                    });
                }
            }
            else if ((start != null && end != null) && TrainingId > 0 && trainerId == 0)
            {
                var data = await trainingNewService.GetTrainingInfoNewByDateAndTrainingId(start, end, TrainingId);
                foreach (var item in data)
                {
                    model.Add(new TrainingWithPerticipantVm
                    {
                        training = item,
                        trainees = await trainingNewService.GetTrainingPerticipantsByTrainingId(item.Id)
                    });
                }
            }
            else
            {
                var data = await trainingNewService.GetAllTrainingInfoNew();
                foreach (var item in data)
                {
                    model.Add(new TrainingWithPerticipantVm
                    {
                        training = item,
                        trainees = await trainingNewService.GetTrainingPerticipantsByTrainingId(item.Id)
                    });
                }
            }
            return View(model);
        }

        public IActionResult GetTrainingREvaluationPdf(DateTime? start, DateTime? end, int? trainerId , int? TrainingId)
        {

            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/HRPMSTrainingNew/TrainingPlanning/GetAllTrainingEvaluationView?start=" + start + "&end=" + end + "&trainerId=" + trainerId+ "&TrainingId=" + TrainingId;

            string fileName;
            string status = myPDF.GeneratePDF(out fileName, url);
            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }


        public async Task<IActionResult> TrainingAttendanceReport()
        {
            var data = new TrainingWithAttendanceDetails
            {
                trainers = await trainingNewService.GetAllTrainer(),
                trainingInfoNews = await trainingNewService.GetAllTrainingInfoNew(),

            };
            return View(data);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllTrainingAttendanceView(DateTime? start, DateTime? end, int? trainerId , int? TrainingId)
        {
            var model = new List<TrainingWithPerticipantVm>();

            if ((start != null && end != null) && trainerId == 0 && TrainingId == 0)
            {
                var data = await trainingNewService.GetTrainingInfoNewByDate(start, end);
                foreach (var item in data)
                {
                    model.Add(new TrainingWithPerticipantVm
                    {
                        training = item,
                        trainees = await trainingNewService.GetTrainingPerticipantsByTrainingId(item.Id)
                    });
                }
            }
            else if ((start != null && end != null) && trainerId > 0 && TrainingId == 0)
            {
                var data = await trainingNewService.GetTrainingInfoNewByDateAndTrainerId(start, end, trainerId);
                foreach (var item in data)
                {
                    model.Add(new TrainingWithPerticipantVm
                    {
                        training = item,
                        trainees = await trainingNewService.GetTrainingPerticipantsByTrainingId(item.Id)
                    });
                }
            }
            else if ((start == null && end == null) && trainerId > 0 && TrainingId == 0)
            {
                var data = await trainingNewService.GetTrainingInfoNewByTrainerId(trainerId);
                foreach (var item in data)
                {
                    model.Add(new TrainingWithPerticipantVm
                    {
                        training = item,
                        trainees = await trainingNewService.GetTrainingPerticipantsByTrainingId(item.Id)
                    });
                }
            }
            else if ((start != null && end != null) && TrainingId > 0 && trainerId > 0)
            {
                var data = await trainingNewService.GetTrainingInfoByDateTrainingIdandTrainerId(start, end, TrainingId, trainerId);
                foreach (var item in data)
                {
                    model.Add(new TrainingWithPerticipantVm
                    {
                        training = item,
                        trainees = await trainingNewService.GetTrainingPerticipantsByTrainingId(item.Id)
                    });
                }
            }
            else if ((start == null && end == null) && TrainingId > 0 && trainerId > 0)
            {
                var data = await trainingNewService.GetTrainingInfoNewByTrainerIdandTrainingId(trainerId, TrainingId);
                foreach (var item in data)
                {
                    model.Add(new TrainingWithPerticipantVm
                    {
                        training = item,
                        trainees = await trainingNewService.GetTrainingPerticipantsByTrainingId(item.Id)
                    });
                }
            }
            else if ((start != null && end != null) && TrainingId > 0 && trainerId == 0)
            {
                var data = await trainingNewService.GetTrainingInfoNewByDateAndTrainingId(start, end, TrainingId);
                foreach (var item in data)
                {
                    model.Add(new TrainingWithPerticipantVm
                    {
                        training = item,
                        trainees = await trainingNewService.GetTrainingPerticipantsByTrainingId(item.Id)
                    });
                }
            }
            else
            {
                var data = await trainingNewService.GetAllTrainingInfoNew();
                foreach (var item in data)
                {
                    model.Add(new TrainingWithPerticipantVm
                    {
                        training = item,
                        trainees = await trainingNewService.GetTrainingPerticipantsByTrainingId(item.Id)
                    });
                }
            }
            return View(model);
        }
        public IActionResult GetTrainingAttendancePdf(DateTime? start, DateTime? end, int? trainerId, int? TrainingId)
        {

            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/HRPMSTrainingNew/TrainingPlanning/GetAllTrainingAttendanceView?start=" + start + "&end=" + end + "&trainerId=" + trainerId + "&TrainingId=" + TrainingId;

            string fileName;
            string status = myPDF.GeneratePDF(out fileName, url);
            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }


        //public async Task<IActionResult> TrainingWithAttendanceAndMarks(int id)
        //{
        //    var data = new TrainingWithAttendanceDetails
        //    {
        //        //trainingDetails = 
        //    };
        //    return Json("");
        //}


        //[HttpGet]
        //public async Task<IActionResult> TrainingAttendanceNew()
        //{
        //    TrainingPlanningViewModel model = new TrainingPlanningViewModel
        //    {

        //    };
        //    return View(model);
        //}

        public async Task<IActionResult> TrainingSubjectReport()
        {
            var data = new TrainingWithAttendanceDetails
            {
                trainers = await trainingNewService.GetAllTrainer(),
                trainingInfoNews = await trainingNewService.GetAllTrainingInfoNew(),
                trainingSubjects = await trainingService.GetTrainingSubject(),
            };
            return View(data);
        }
        public IActionResult GetTrainingSubjectReportPdf(DateTime? start, DateTime? end, int? trainerId, int? TrainingId, int? SubjectId)
        {

            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/HRPMSTrainingNew/TrainingPlanning/GetTrainingSubjectView?start=" + start + "&end=" + end + "&trainerId=" + trainerId + "&TrainingId=" + TrainingId+ "&SubjectId=" + SubjectId;

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
        public async Task<IActionResult> GetTrainingSubjectView(DateTime? start, DateTime? end, int? trainerId, int? TrainingId, int? SubjectId)
        {
            var model = new List<TrainingWithPerticipantVm>();

            if ((start != null && end != null) && trainerId == 0 && TrainingId == 0 && SubjectId == 0)
            {
                var data = await trainingNewService.GetTrainingInfoNewByDate(start, end);
                foreach (var item in data)
                {
                    model.Add(new TrainingWithPerticipantVm
                    {
                        training = item,
                        trainees = await trainingNewService.GetTrainingPerticipantsByTrainingId(item.Id)
                    });
                }
            }
            else if ((start != null && end != null) && TrainingId > 0 && trainerId > 0 && SubjectId > 0)
            {
                var data = await trainingNewService.GetTrainingInfoByDateTrainingIdandTrainerIdSubjectId(start, end, TrainingId, trainerId,SubjectId);
                foreach (var item in data)
                {
                    model.Add(new TrainingWithPerticipantVm
                    {
                        training = item,
                        trainees = await trainingNewService.GetTrainingPerticipantsByTrainingId(item.Id)
                    });
                }
            }
            else if ((start == null && end == null) && SubjectId > 0 && trainerId > 0)
            {
                var data = await trainingNewService.GetTrainingInfoNewByTrainerIdandSubjectId(trainerId, SubjectId);
                foreach (var item in data)
                {
                    model.Add(new TrainingWithPerticipantVm
                    {
                        training = item,
                        trainees = await trainingNewService.GetTrainingPerticipantsBySubjectId(item.Id)
                    });
                }
            }
            else if ((start != null && end != null) && SubjectId > 0 && trainerId == 0)
            {
                var data = await trainingNewService.GetTrainingInfoNewByDateAndSubjectId(start, end, SubjectId);
                foreach (var item in data)
                {
                    model.Add(new TrainingWithPerticipantVm
                    {
                        training = item,
                        trainees = await trainingNewService.GetTrainingPerticipantsBySubjectId(item.Id)
                    });
                }
            }
            else if ((start != null && end != null) && trainerId > 0 && SubjectId == 0)
            {
                var data = await trainingNewService.GetTrainingInfoNewByDateAndTrainerId(start, end, SubjectId);
                foreach (var item in data)
                {
                    model.Add(new TrainingWithPerticipantVm
                    {
                        training = item,
                        trainees = await trainingNewService.GetTrainingPerticipantsBySubjectId(item.Id)
                    });
                }
            }
            else if ((start == null && end == null) && trainerId > 0 && SubjectId == 0)
            {
                var data = await trainingNewService.GetTrainingInfoNewByTrainerId(trainerId);
                foreach (var item in data)
                {
                    model.Add(new TrainingWithPerticipantVm
                    {
                        training = item,
                        trainees = await trainingNewService.GetTrainingPerticipantsBySubjectId(item.Id)
                    });
                }
            }



            else
            {
                var data = await trainingNewService.GetAllTrainingInfoWithSubject(SubjectId);
                foreach (var item in data)
                {
                    model.Add(new TrainingWithPerticipantVm
                    {
                        training = item,
                        trainees = await trainingNewService.GetTrainingPerticipantsBySubjectId(item.Id)
                    });
                }
            }
            return View(model);
        }
        public async Task<IActionResult> TrainingSummaryReport()
		{
			var data = new TrainingWithAttendanceDetails
			{
				trainers = await trainingNewService.GetAllTrainer(),
                trainingInfoNews = await trainingNewService.GetAllTrainingInfoNew(),
            };
			return View(data);
		}
		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> GetAllTrainingSummaryView(DateTime? start, DateTime? end, int? trainerId, int? TrainingId)
		{
			var model = new List<TrainingWithPerticipantVm>();

			if ((start != null && end != null) && trainerId == 0 && TrainingId == 0)
			{
				var data = await trainingNewService.GetTrainingInfoNewByDate(start, end);
				foreach (var item in data)
				{
					model.Add(new TrainingWithPerticipantVm
					{
						training = item,
						trainees = await trainingNewService.GetTrainingPerticipantsByTrainingId(item.Id)
					});
				}
			}
            else if ((start != null && end != null) && TrainingId > 0 && trainerId > 0)
            {
                var data = await trainingNewService.GetTrainingInfoByDateTrainingIdandTrainerId(start, end, TrainingId, trainerId);
                foreach (var item in data)
                {
                    model.Add(new TrainingWithPerticipantVm
                    {
                        training = item,
                        trainees = await trainingNewService.GetTrainingPerticipantsByTrainingId(item.Id)
                    });
                }
            }
            else if ((start == null && end == null) && TrainingId > 0 && trainerId > 0)
            {
                var data = await trainingNewService.GetTrainingInfoNewByTrainerIdandTrainingId(trainerId, TrainingId);
                foreach (var item in data)
                {
                    model.Add(new TrainingWithPerticipantVm
                    {
                        training = item,
                        trainees = await trainingNewService.GetTrainingPerticipantsByTrainingId(item.Id)
                    });
                }
            }
            else if ((start != null && end != null) && TrainingId > 0 && trainerId == 0)
            {
                var data = await trainingNewService.GetTrainingInfoNewByDateAndTrainingId(start, end, TrainingId);
                foreach (var item in data)
                {
                    model.Add(new TrainingWithPerticipantVm
                    {
                        training = item,
                        trainees = await trainingNewService.GetTrainingPerticipantsByTrainingId(item.Id)
                    });
                }
            }
            else if ((start != null && end != null) && trainerId > 0 && TrainingId == 0)
			{
				var data = await trainingNewService.GetTrainingInfoNewByDateAndTrainerId(start, end, trainerId);
				foreach (var item in data)
				{
					model.Add(new TrainingWithPerticipantVm
					{
						training = item,
						trainees = await trainingNewService.GetTrainingPerticipantsByTrainingId(item.Id)
					});
				}
			}
			else if ((start == null && end == null) && trainerId > 0 && TrainingId == 0)
			{
				var data = await trainingNewService.GetTrainingInfoNewByTrainerId(trainerId);
				foreach (var item in data)
				{
					model.Add(new TrainingWithPerticipantVm
					{
						training = item,
						trainees = await trainingNewService.GetTrainingPerticipantsByTrainingId(item.Id)
					});
				}
			}

            
           
            else
			{
				var data = await trainingNewService.GetAllTrainingInfoNew();
				foreach (var item in data)
				{
					model.Add(new TrainingWithPerticipantVm
					{
						training = item,
						trainees = await trainingNewService.GetTrainingPerticipantsByTrainingId(item.Id)
					});
				}
			}
			return View(model);
		}

		public IActionResult GetTrainingSummaryPdf(DateTime? start, DateTime? end, int? trainerId, int? TrainingId)
		{

			string scheme = Request.Scheme;
			var host = Request.Host;

			string url = scheme + "://" + host + "/HRPMSTrainingNew/TrainingPlanning/GetAllTrainingSummaryView?start=" + start + "&end=" + end + "&trainerId=" + trainerId+ "&TrainingId=" + TrainingId;

			string fileName;
			string status = myPDF.GeneratePDF(out fileName, url);
			if (status != "done")
			{
				return Content("<h1>" + status + "</h1>");
			}

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");

		}

		#endregion


		//xxxxxxxxxxxxxx 

		#region API


		[Route("global/api/TrainingInfoNewsDetails")]
        [HttpGet]
        public async Task<IActionResult> TrainingInfoNewsDetails()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            string org = user.org;
            //var trainingInfoNews = await trainingNewService.GetTrainingInfoNewByType(1, org);
            var trainingInfoNews = await trainingNewService.GetAllTrainingInfoNew();
            List<TrainingPlanningViewModel> ListOfTrainingPlanningViewModel = new List<TrainingPlanningViewModel>();
            foreach (var startDate in trainingInfoNews.Select(x=>x.startDate).Distinct())
            {
                TrainingPlanningViewModel VmData = new TrainingPlanningViewModel
                {
                    CountTotalTrainingInfo = trainingInfoNews.Where(x => x.startDate == startDate).ToList().Count,
                    planeStartDate = startDate,
                   
                };
                ListOfTrainingPlanningViewModel.Add(VmData);
            }
            return Json(ListOfTrainingPlanningViewModel);
        }

		[Route("global/api/MyTrainingInfoNewsDetails")]
        [HttpGet]
        public async Task<IActionResult> MyTrainingInfoNewsDetails()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            string org = user.org;
            //var trainingInfoNews = await trainingNewService.GetTrainingInfoNewByType(1, org);
            var trainingInfoNews = await trainingNewService.GetMyTrainingInfoNew(user.EmpCode);
            List<TrainingPlanningViewModel> ListOfTrainingPlanningViewModel = new List<TrainingPlanningViewModel>();
            foreach (var startDate in trainingInfoNews.Select(x=>x.startDate).Distinct())
            {
                TrainingPlanningViewModel VmData = new TrainingPlanningViewModel
                {
                    CountTotalTrainingInfo = trainingInfoNews.Where(x => x.startDate == startDate).ToList().Count,
                    planeStartDate = startDate,
                   
                };
                ListOfTrainingPlanningViewModel.Add(VmData);
            }
            return Json(ListOfTrainingPlanningViewModel);
        }







        [Route("global/api/TrainingInfoNewsByStartDate/{startdate}")]
        [HttpGet]
        public async Task<IActionResult> TrainingInfoNewsByStartDate(DateTime startdate)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            string org = user.org;
            //var trainingInfoNews = await trainingNewService.GetTrainingInfoNewByType(1, org);
            var trainingInfoNews = await trainingNewService.GetAllTrainingInfoNew();

            TrainingPlanningViewModel VmData = new TrainingPlanningViewModel()
            {
                trainingInfoNews = trainingInfoNews.Where(x => x.startDate == startdate).ToList()
            };
                
            
            
            return Json(VmData);
        }

        
        [Route("global/api/TrainingInfoNewsFilter/{startdate}/{endDate}/{instituteId}/{trainerId}/{subjects}")]
        [HttpGet]
        public async Task<IActionResult> TrainingInfoNewsFilter(DateTime? startdate, DateTime? endDate, int? instituteId,int? trainerId, string subjects)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            string org = user.org;
            var trainingInfoNews = await trainingNewService.GetTrainingInfoNewByType(1, org);

            TrainingPlanningViewModel VmData = new TrainingPlanningViewModel()
            {
                trainingInfoNews = trainingInfoNews.Where(x => x.startDate == startdate && Convert.ToDateTime(x.startDate) >= DateTime.Now).Where(x => x.endDate == endDate && Convert.ToDateTime(x.endDate) <= DateTime.Now).Where(x => x.instituteId == instituteId).Where(x => x.trainerId == trainerId).Where(x => x.subjects == subjects).ToList()
            };
                
            
            
            return Json(VmData);
        }



        #endregion
        public async Task<IActionResult> GetCourseTitle()
        {
            //  return Json(await employeeInfoService.GetLevelOfEducationList());
            return Json(await yearCourseTitleService.GetCourseTitle());
        }

        [HttpPost]
        public async Task<IActionResult> CourseTitle(TrainingPlanningViewModel model)
        {
            var data = new CourseTitle
            {
                Id = 0,
                nameEN = model.nameEN,



            };
            await yearCourseTitleService.SaveCourseTitle(data);
            return Json("saved");
        }
        public async Task<IActionResult> Delete(int id)
        {
           await trainingNewService.DeleteTrainingInfoNewById(id);
           
            return RedirectToAction("Index");
        }
         public async Task<IActionResult> Delete1(int id)
        {
           
            await trainingNewService.DeleteTrainingInfoNewById(id);
            return RedirectToAction("AllTrainingList");
        }

        public async Task<IActionResult> Delete(int id, int empId)
        {
            await trainingNewService.DeleteTrainingInfoNewById(id);
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> TrainingInfoFilter()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            string org = user.org;

            TrainingInfoFilterViewModel model = new TrainingInfoFilterViewModel
            {
              
                trainingInstitutes = await trainingService.GetTrainingInstitute(),
                resourcePeople = await resourcePersonService.GetResourcePersonInfoByOrg(org),
                trainingSubjects = await trainingService.GetTrainingSubject(),
             
            };
            return View(model);
        }
		#region Source Of Fund Date:11/12/2021
		[HttpPost]
		public async Task<IActionResult> SaveUpdateSourceOfFund(SourceOfFund model) 
		{
			var result= await trainingNewService.SaveUpdateSourceOfFund(model);
			return Json(result);   
		}
		
		[HttpPost]
		public async Task<IActionResult> DeletesourceOfFundById(int Id)
		{
			var result = await trainingNewService.DeletesourceOfFundById(Id);
			return Json(result);
		}
		#endregion
		#region CourseType Date 11/12/2021
		public async Task<IActionResult> SaveUpdateCourseType(CourseType model)
		{
			var result = await trainingNewService.SaveUpdateCourseType(model); 
			return Json(result);
		}
		[HttpPost]
		public async Task<IActionResult> DeleteCourseTypeById(int Id)
		{
			var result = await trainingNewService.DeletecourseTypeById(Id);
			return Json(result);
		}
		#endregion

	}
}