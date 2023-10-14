using OPUSERP.Areas.HRPMSMasterData.Models;
using OPUSERP.Areas.HRPMSMasterData.Models.Lang;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Controllers
{
    [Authorize]
    [Area("HRPMSMasterData")]
    public class ComputerSubjectController : Controller
    {
        private readonly LangGenerate<SubjectInfoLn> _lang;
        private readonly ISubjectService subjectService;

        public ComputerSubjectController(IHostingEnvironment hostingEnvironment, ISubjectService subjectService)
        {
            _lang = new LangGenerate<SubjectInfoLn>(hostingEnvironment.ContentRootPath);
            this.subjectService = subjectService;
        }

        // GET: Subject
        public async Task<IActionResult> Index()
        {
            ComputerSubjectViewModel model = new ComputerSubjectViewModel
            {
               // fLang = _lang.PerseLang("MasterData/SubjectEN.json", "MasterData/SubjectBN.json", Request.Cookies["lang"]),
                ComputerSubjects = await subjectService.GetAllComputerSubject()
            };

            return View(model);
        }


        // POST: Subject/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] ComputerSubjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
               
                model.ComputerSubjects = await subjectService.GetAllComputerSubject();
                return View(model);
            }

            ComputerSubject data = new ComputerSubject
            {
                Id = model.CsId,
                name = model.name,
                status = 1,
                isActive = 1,
                remarks=model.remarks
            };

            await subjectService.SaveComputerSubject(data);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<JsonResult> DeleteComputerSubjectById(int id)
        {
            await subjectService.DeleteComputerSubjectById(id);
            return Json(true);
        }
    }
}