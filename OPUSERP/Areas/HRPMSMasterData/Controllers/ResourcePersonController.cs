using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Areas.HRPMSMasterData.Models;
using OPUSERP.Areas.HRPMSMasterData.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.HRPMS.Services.TrainingNew.Interfaces;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Data.Entity.TrainingNew;

namespace OPUSERP.Areas.HRPMSMasterData.Controllers
{
    [Authorize]
    [Area("HRPMSMasterData")]
    public class ResourcePersonController : Controller
    {
        private readonly LangGenerate<TrainigInstituteLn> _lang;
        private readonly ITrainingService trainingService;
		private readonly ITrainingNewService trainingNewService;

		private readonly IResourcePersonService resourcePersonService;
        private readonly IPersonalInfoService personalInfoService;
		private readonly IPhotographService photographService;

		public ResourcePersonController(IHostingEnvironment hostingEnvironment, ITrainingService trainingService, IPersonalInfoService personalInfoService, IResourcePersonService resourcePersonService, IPhotographService photographService, ITrainingNewService trainingNewService)
        {
            _lang = new LangGenerate<TrainigInstituteLn>(hostingEnvironment.ContentRootPath);
            this.trainingService = trainingService;
            this.personalInfoService = personalInfoService;
            this.resourcePersonService = resourcePersonService;
			this.photographService = photographService;
			this.trainingNewService =  trainingNewService;
		}
        // GET: TrainingInstitute
        public async Task<IActionResult> Index()
        {
			

			ResourcePersonViewModel model = new ResourcePersonViewModel
            {
                resourcePersons = await resourcePersonService.GetResourcePersonInfo()
            };
            return View(model);
        }
		public async Task<IActionResult> GetEmployeePostingPlace(int? EmpId) 
		{
			var postingPlace = await trainingNewService.GetEmployeePostingPlace(EmpId); 
			return Json(postingPlace);
		}
		

		// POST: TrainingInstitute/Edit
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] ResourcePersonViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.resourcePersons = await resourcePersonService.GetResourcePersonInfo();
                return View(model);
            }
            string fileName;
            if (model.personPhoto != null)
            {
                string message = FileSave.SaveImageNew(out fileName, model.personPhoto);
            }
            else 
            {
                fileName = await resourcePersonService.GetresourcePersonImgUrlById(model.ResourcePersonId);
			}
			if (model.photoUrl != null && model.personPhoto == null && fileName== null)
			{
				fileName = model.photoUrl;
			}
            ResourcePerson data = new ResourcePerson
            {
                Id = model.ResourcePersonId,
                name = model.name,
                designation = model.designation,
                workPlace = model.workPlace,
                specialization = model.specialization,
                type = model.type,
                contactNumber = model.contactNumber,
                performance = model.performance,
                email = model.email,
                address = model.address,
                remarks = model.remarks,
                orgType = model.orgType,
                url = fileName,
				employeeInfoId = model.employeeInfoId
			};

            await resourcePersonService.SaveResourcePersonInfo(data);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteResourcePerson(int id)
        {
            await resourcePersonService.DeleteResourcePersonInfoById(id);
            return RedirectToAction("Index");
        }
		public async Task<IActionResult> GetEmployeeImage(int id)
		{
		    var image= 	await photographService.GetPhotographByEmpIdAndType(id, "profile");
			return Json(image);
		}

	}
}