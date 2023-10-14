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
using OPUSERP.HRPMS.Data.Entity.HrComputerLiteracy;
using OPUSERP.HRPMS.Services.Employee.Interfaces;

namespace OPUSERP.Areas.HRPMSMasterData.Controllers
{
    [Authorize]
    [Area("HRPMSMasterData")]
    public class ComputerLiteracyController : Controller
    {
        private readonly LangGenerate<DegSubRelationInfoLn> _lang;
        private readonly IRelDegreeSubjectService degreeSubjectService;
        private readonly IDegreeService degreeService;
        private readonly ISubjectService subjectService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly IPhotographService photographService;


        public ComputerLiteracyController(IHostingEnvironment hostingEnvironment, IRelDegreeSubjectService degreeSubjectService, IDegreeService degreeService, ISubjectService subjectService, IPersonalInfoService personalInfoService, IPhotographService photographService)
        {
            _lang = new LangGenerate<DegSubRelationInfoLn>(hostingEnvironment.ContentRootPath);
            this.degreeSubjectService = degreeSubjectService;
            this.degreeService = degreeService;
            this.subjectService = subjectService;
            this.personalInfoService = personalInfoService;
            this.photographService = photographService;
        }
        // GET: RelDegreeSubject
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            var model = new HrComputerLiteracyViewModel
            {
                employeeID = id.ToString(),
                photograph = await photographService.GetPhotographByEmpIdAndType(id, "profile"),
                computerLiteracy = await subjectService.GetAllcomputerLiteracy(),
                computerLiteracies = await subjectService.GetcomputerLiteracyByEmpId(id),
                employee = await personalInfoService.GetEmployeeInfoById(id),
                ComputerSubjectsList = await subjectService.GetAllComputerSubject()

            };
            return View(model);
        }

        // POST: RelDegreeSubject/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] HrComputerLiteracyViewModel model)
        {
            if (!ModelState.IsValid)
            {

                ViewBag.employeeID = model.employeeID;
                model.computerLiteracies = await subjectService.GetcomputerLiteracyByEmpId(Int32.Parse(model.employeeID));
                model.computerLiteracy = await subjectService.GetAllcomputerLiteracy();
                model.ComputerSubjectsList = await subjectService.GetAllComputerSubject();
              
            }
           
            HrComputerLiteracy data = new HrComputerLiteracy
            {
                Id= Int32.Parse(model.LiteracyId),
                employeeId = Int32.Parse(model.employeeID),
                subject = model.subject,
                competencyLevel=model.competencyLevel,
                training=model.training,
                diploma=model.diploma,
                status=1,
                isActive=1,
                remarks=model.remarks

            };

            //await subjectService.SaveHrComputerLiteracy(data);

            //return RedirectToAction(nameof(Index));
            await subjectService.SaveHrComputerLiteracy(data);
            await personalInfoService.UpdateEmployeeinfoById(Int32.Parse(model.employeeID));
          
            return RedirectToAction("Index", "ComputerLiteracy", new
            {
                id = model.employeeID
            });
        }

        //[HttpPost]
        //public async Task<JsonResult> DeleteComputerLiteracy(int Id)
        //{
        //    await subjectService.DeleteComputerLiteracyById(Id);
        //    return Json(true);
        //}

        public async Task<IActionResult> Delete(int id, int empId)
        {
            await subjectService.DeleteComputerLiteracyById(id);
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "ComputerLiteracy", new
            {
                id = empId
            });
        }
    }
}