using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSTrainingNew.Models;
//using OPUSERP.CLUB.Services.Member.Interfaces;
using OPUSERP.Data.Entity;
using OPUSERP.Data.Entity.Training;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.HRPMS.Services.TrainingNew.Interfaces;

namespace OPUSERP.Areas.HRPMSTrainingNew.Controllers
{

    [Authorize]
    [Area("HRPMSTrainingNew")]
    public class TrainingFeedbackController : Controller
    {
        private readonly ITrainingNewService trainingNewService;
        private readonly ITypeService typeService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly UserManager<ApplicationUser> _userManager;
     


        public TrainingFeedbackController(IHostingEnvironment hostingEnvironment, ITypeService typeService, ITrainingNewService trainingNewService,UserManager<ApplicationUser> userManager, IPersonalInfoService personalInfoService)
        {
            this.typeService = typeService;
            this.trainingNewService = trainingNewService;
            this.personalInfoService = personalInfoService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(int Id)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            string org = user.org;
           // string EmpCode = user.EmpCode;
           // var v  = await personalInfoService.GetUserInfoByEmpCode(EmpCode);
           // string UserId = v.aspnetId;
            TrainingFeedbackViewModel model = new TrainingFeedbackViewModel
            {
                trainingFeedbackList = await trainingNewService.GetTrainingFeedback(),
                trainingInfoNewList = await trainingNewService.GetAllTrainingInfoNew(),
               // trainee = await personalInfoService.GetEmployeeInfoByOrg("Bank"),
                //employeeIdUser = UserId,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] TrainingFeedbackViewModel model)
        {
            //return Json(model);
            if (!ModelState.IsValid)
            {

               
                model.trainingFeedbackList = await trainingNewService.GetTrainingFeedback();
                return View(model);
            }

            //return Json(model);

            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            string org = user.org;

            TrainingFeedback data = new TrainingFeedback
            {
                Id = Convert.ToInt32(model.trainingfeedbackId),
                feedback = model.feedback,
                type = model.type,
                trainingId=model.trainingId

            };

            await trainingNewService.SaveTrainingFeedBack(data);

            return RedirectToAction(nameof(Index));
            
           

        }

        public async Task<IActionResult> PariticipientFeedback()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            string org = user.org;

            TrainingFeedbackViewModel model = new TrainingFeedbackViewModel
            {
                trainingFeedbackList = await trainingNewService.GetTrainingFeedback()

            };
            return View(model);
        }

        public async Task<IActionResult> TrainerFeedback()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            string org = user.org;

            TrainingFeedbackViewModel model = new TrainingFeedbackViewModel
            {
                trainingFeedbackList = await trainingNewService.GetTrainingFeedback()

            };
            return View(model);
        }
        public async Task<IActionResult> Delete(int id)
        {
            await trainingNewService.DeleteFeedback(id);
            return RedirectToAction("Index");
        }

		//public	async Task<IActionResult> CheckTrainerOrTrainer()
		//{
		//	string userName = HttpContext.User.Identity.Name;
		//	var user = await _userManager.FindByNameAsync(userName);

		//	var data = await trainingNewService.CheckTrainingOrTrainer(user.EmpCode);
		//	return Json(data);
		//}

	}
    
}
