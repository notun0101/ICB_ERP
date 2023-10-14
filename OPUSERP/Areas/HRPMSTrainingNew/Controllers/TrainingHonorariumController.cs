using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSTrainingNew.Models;
using OPUSERP.Areas.HRPMSTrainingNew.Models.Lang;
using OPUSERP.Data.Entity;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.TrainingNew;
using OPUSERP.HRPMS.Services.TrainingNew;
using OPUSERP.HRPMS.Services.TrainingNew.Interfaces;
using System;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSTrainingNew.Controllers
{
    [Authorize]
    [Area("HRPMSTrainingNew")]
    public class TrainingHonorariumController : Controller
    {
        private readonly LangGenerate<TrainerInfoLn> _lang;
        private readonly ITrainingNewService trainingNewService;
		private readonly IResourcePersonService resourcePersonService;
		private readonly UserManager<ApplicationUser> _userManager;

        public TrainingHonorariumController(IHostingEnvironment hostingEnvironment, ITrainingNewService trainingNewService,IResourcePersonService resourcePersonService, UserManager<ApplicationUser> userManager)
        {
            _lang = new LangGenerate<TrainerInfoLn>(hostingEnvironment.ContentRootPath);
            this.trainingNewService = trainingNewService;
			this.resourcePersonService = resourcePersonService;

			_userManager = userManager;
        }

        // GET: TrainerInfo
        public async Task<IActionResult> Index()
        {
			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
			string org = user.org;
			TrainingPlanningViewModel model = new TrainingPlanningViewModel
			{
				trainingInfoNews = await trainingNewService.GetTrainingInfoNews(),
				//resourcePeople = await resourcePersonService.GetResourcePersonInfoByOrgNew(),
				trainingHonorariumsList = await trainingNewService.GetTrainingHonorarium()   
			};
			return View(model);
		}
		public async Task<IActionResult> GetTrainingResourcePersonById(int id)
		{
			var data = await trainingNewService.GetTrainingResourcePersonById(id);
			return Json(data);
		}
        public async Task<IActionResult> GetTrainingCoOrdinatorById(int id)
		{
			var data = await resourcePersonService.GetTrainingCoOrdinatorById(id);
			return Json(data);
		}
      


		// POST: TrainingHonorarium/Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Index([FromForm] TrainingPlanningViewModel model)  
		{
			//if (!ModelState.IsValid)
			//{
			//	//model.trainingInfoNews = await trainingNewService.GetTrainingInfoNews();
			//	//model.resourcePeople = await resourcePersonService.GetResourcePersonInfoByOrgNew();
			//	//model.trainingHonorariums = await trainingNewService.GetTrainingHonorarium();
			//	return View(model); 
			//}
			ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
			string org = user.org;
			if (model.CoOrdinatorId != null)
			{
				model.trainerId = null; 
			}
			TrainingHonorarium data = new TrainingHonorarium
			{
				//Id	isDelete	createdAt	updatedAt	createdBy	updatedBy	trainingId	trainerId	
				//receivedMoney	taxPercentage	taxAmount	session	effectiveDate	status	isPaid
				Id = Convert.ToInt32(model.Id),  
				trainingId = model.trainingId,
				trainerId = model.trainerId,
				receivedMoney = model.receivedMoney,
				taxPercentage = model.taxPercentage,
				taxAmount = model.taxAmount,
				session = model.session,
				effectiveDate = model.effectiveDate, 
				status=1,
				isPaid=1,
                coOrdinatorPayment=model.coOrdinatorPayment,
                CoOrdinatorId=model.CoOrdinatorId
				
			};
			var Id = await trainingNewService.SaveUpdateTrainingHonorarium(data);
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Delete(int id)
		{
			await trainingNewService.DeleteTrainingHonorariumById(id);
			return RedirectToAction("Index");
		}




		//// POST: TrainerInfo/Create
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> Index([FromForm] TrainerInfoViewModel model)
		//{
		//    if (!ModelState.IsValid)
		//    {
		//        model.fLang = _lang.PerseLang("TrainingNew/TrainerInfoEN.json", "TrainingNew/TrainerInfoBN.json", Request.Cookies["lang"]);
		//        return View(model);
		//    }

		//    //return Json(model);
		//    ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
		//    string org = user.org;

		//    ResourcePerson data = new ResourcePerson
		//    {
		//        Id = model.trainerId,
		//        name = model.name,
		//        designation = model.designation,
		//        workPlace = model.workPlace,
		//        specialization = model.Specialization,
		//        contactNumber = model.contactNumber,
		//        performance = model.Performance,
		//        remarks = model.Remarks,
		//        orgType = org,
		//        email = model.email
		//    };

		//    await resourcePersonService.SaveResourcePersonInfo(data);

		//    return RedirectToAction(nameof(Index));
		//}
		//public async Task<IActionResult> Delete(int id, int empId)
		//{
		//    await resourcePersonService.DeleteTrainerById(id);
		//    //return RedirectToAction(nameof(Index));
		//    return RedirectToAction("Index");
		//}


	}
}