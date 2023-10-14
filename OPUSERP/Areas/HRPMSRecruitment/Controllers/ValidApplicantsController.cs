using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.Areas.HRPMSRecruitment.Models;
using OPUSERP.HRPMS.Services.Recruitment.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.HRPMS.Data.Entity.Recruitment;

namespace OPUSERP.Areas.HRPMSRecruitment.Controllers
{
    [Authorize]
    [Area("HRPMSRecruitment")]
    public class ValidApplicantsController : Controller
    {
        private readonly IApplicationFormService applicationFormService;

        public ValidApplicantsController(IApplicationFormService applicationFormService)
        {
            this.applicationFormService = applicationFormService;
        }

        // GET: CreateJobCircular
        public async Task<IActionResult> Index()
        {
            ApplicationFormViewModel model = new ApplicationFormViewModel
            {
                applicationForms = await applicationFormService.GetApplicationForm()
            };
            return View(model);
        }

        public async Task<IActionResult> Approve()
        {
            ApplicationFormViewModel model = new ApplicationFormViewModel
            {
                applicationForms = await applicationFormService.GetApplicationForm()
            };
            return View(model);
        }

        public async Task<IActionResult> Rejected()
        {
            ApplicationFormViewModel model = new ApplicationFormViewModel
            {
                applicationForms = await applicationFormService.GetApplicationForm()
            };
            return View(model);
        }

        public async Task<IActionResult> ShortList()
        {
            ApplicationFormViewModel model = new ApplicationFormViewModel
            {
                applicationForms = await applicationFormService.GetApplicationForm()
            };
            return View(model);
        }

        public async Task<IActionResult> SelectedList()
        {
            ApplicationFormViewModel model = new ApplicationFormViewModel
            {
                applicationForms = await applicationFormService.GetApplicationForm()
            };
            return View(model);
        }

        public async Task<IActionResult> FinalSelection()
        {
            ApplicationFormViewModel model = new ApplicationFormViewModel
            {
                applicationForms = await applicationFormService.GetApplicationForm()
            };
            return View(model);
        }

        //[HttpPost]

        //public async Task<IActionResult> ApproveApplication(ApplicationFormViewModel model, int id)
        //{
        //    ApplicationForm data = new ApplicationForm
        //    {
        //        Id = id,
        //        applicationNo = model.applicantsNo,
        //        status = 2,

        //    };

        //    await applicationFormService.SaveApplicationForm(data);
        //    return Json("saved");
        //}

        [HttpPost]
        public async Task<IActionResult> ApproveApplication(int Id,int status)
        {
            try
            {
                var a = await applicationFormService.GetApplicationFormById(Id);
                a.status = status;

                await applicationFormService.SaveApplicationForm(a);
                return Json("updated");
            }
            catch (Exception ex)
            {

                return Json(ex.Message);
            }
           
        }


        [HttpGet]
        public async Task<IActionResult> AcceptedCandidates(int id)
        {
            
            ApplicationFormViewModel model = new ApplicationFormViewModel
            {
                applicationForms = await applicationFormService.GetApplicationForm()

            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AcceptedCandidates([FromForm] ApplicationFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.applicationForms = await applicationFormService.GetApplicationForm();
                return View(model);
            }
            var applicant = await applicationFormService.GetApplicationFormById(model.Id);
            applicant.applicationNo = model.ApplicationNo;
            applicant.nameEN = model.ApplicantName;
            applicant.writtenMarks = model.WrittenMarks;
            applicant.vivaResult = model.VivaMarks;
            applicant.totalMarks = model.TotalMarks;
            applicant.remarks = model.ApplicantRemarks;
            
            await applicationFormService.SaveApplicationForm(applicant);
            return RedirectToAction("AcceptedCandidates");
        }
        [HttpGet]
        public async Task<IActionResult> CandidateStatusChange(string id, int status)
        {
            var ids = id.Split('A');
            try
            {
                for (int i = 0; i < ids.Length - 1; i++)
                {
                    var data = await applicationFormService.GetApplicationFormById(Convert.ToInt32(ids[i]));
                    data.status = status;
                    await applicationFormService.SaveApplicationForm(data);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return Json("ok");
        }
        // POST: CreateJobCircular/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index([FromForm] ValidApplicantsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}