 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSAssignment.Helpers;
using OPUSERP.Areas.HRPMSMasterData.Models;
using OPUSERP.Areas.HRPMSMasterData.Models.Lang;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.Suspensions;
using OPUSERP.HRPMS.Services.BoardofDirector.Interfaces;
using OPUSERP.HRPMS.Services.Dashboard.interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;

namespace OPUSERP.Areas.HRPMSMasterData.Controllers
{
	[Authorize]
	[Area("HRPMSMasterData")]
	public class BoardOfDirectorController : Controller
	{
		//private readonly LangGenerate<ActivityStatusLn> _lang;
		private readonly IStatusService statusService;
		private readonly IHrmDashboardService hrmDashboardService;
		private readonly IBoardofDirector boardOfDirectorService;

		public BoardOfDirectorController(IHostingEnvironment hostingEnvironment, IHrmDashboardService hrmDashboardService, IStatusService statusService, IBoardofDirector boardOfDirectorService)
		{
			//_lang = new LangGenerate<ActivityStatusLn>(hostingEnvironment.ContentRootPath);
			this.statusService = statusService;
			this.hrmDashboardService = hrmDashboardService;
			this.boardOfDirectorService = boardOfDirectorService;
		}
		[HttpGet]
		public async Task<IActionResult> createBoardDirector()
		{

			var model = new BoardOfDirectorViewModel
			{
				companies = await boardOfDirectorService.GetAllCompanyList()

			};
			return View(model);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> createBoardDirector([FromForm] BoardOfDirectorViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.companies = await boardOfDirectorService.GetAllCompanyList();
				return View(model);
			}
            //string fileName;

            //if (model.chairmanPhotoUrl != null)
            //{
            //    string message = FileSave.SaveEmpAttachmentNew(out fileName, model.chairmanPhotoUrl);

            //}
            //else
            //{
            //    fileName = await boardOfDirectorService.GetPhotoUrlById(Int32.Parse(model.boardId));
            //};


            var data = new BoardOfDirector
			{
				Id = Convert.ToInt32(model.boardId),
				name = model.name,
				designation = model.designation,
				mobile = model.mobile,
				email = model.email,
				address = model.address,
				bio = model.bio,
				//photoUrl = fileName,
				companyId = model.companyId,
				company = model.company,
				year = model.year,
				status = 1, //chairmen
				isActive = 1,
				date = model.date
			};
			await boardOfDirectorService.SaveBoardOfDirector(data);

            //string directorFileName;

            //if (model.dirPhotos != null)
            //{
            //    string message = FileSave.SaveEmpAttachmentNew (out directorFileName, model.dirPhotos);

            //}
            //else
            //{
            //    directorFileName = await boardOfDirectorService.GetDirectorPhotoUrlById(Int32.Parse(model.boardId));
            //};

            var directorCount = model.dirNames.Length;


			for (int i = 0; i < directorCount; i++)
			{
				var dataDir = new BoardOfDirector
				{
					Id = 0,
					name = model.dirNames[i],
					designation = model.dirDesis[i],
					mobile = model.dirMobiles[i],
					email = model.dirEmails[i],
					address = model.dirAddresses[i],
					bio = model.dirBios[i],
					//photoUrl = directorFileName,
					companyId = model.companyId,
					company = model.company,
					year = model.year,
					status = 2, // director
					isActive = 1
				};
				await boardOfDirectorService.SaveBoardOfDirector(dataDir);
			}
            //string CeoFileName;

            //if (model.ceoPhotos != null)
            //{
            //    string message = FileSave.SaveEmpAttachmentNew(out CeoFileName, model.ceoPhotos);

            //}
            //else
            //{
            //    CeoFileName = await boardOfDirectorService.GetPhotoUrlById(Int32.Parse(model.boardId));
            //};
            var dataCeo = new BoardOfDirector
			{
				Id = 0,
				name = model.ceoname,
				designation = model.ceodesignation,
				mobile = model.ceomobile,
				email = model.ceoemail,
				address = model.ceoaddress,
				bio = model.ceobio,
				//photoUrl =CeoFileName,
				companyId = model.companyId,
				company = model.company,
				year = model.year,
				status = 3, //ceo
				isActive = 1
			};
			await boardOfDirectorService.SaveBoardOfDirector(dataCeo);
			return RedirectToAction("Index", new { companyId = model.companyId, year = model.year });

		}
        [HttpGet]
        public async Task<IActionResult> EditBoardDirector(int companyId,int year)
        {
			var companies = await boardOfDirectorService.GetAllCompanyListWithBOD(companyId, year);
			var data = new List<BODWithCompany>();
			foreach (var item in companies)
			{
				data.Add(new BODWithCompany
				{
					company = item,
					chairmen = await boardOfDirectorService.GetChairmenByCompanyIdAndYear(item.companyId, item.year),
					directors = await boardOfDirectorService.GetDirectorsByCompanyIdAndYear(item.companyId, item.year),
					ceo = await boardOfDirectorService.GetCeoByCompanyIdAndYear(item.companyId, item.year)
				});
			}
			var model1 = new BoardOfDirectorsVm
			{
				bODWithCompanies = data
			};
			return View(model1);
            //var model1 = new BoardOfDirectorsVm
            //{
            //    bODWithCompanies = data
            //};

            //return View(model1);

        }
		[HttpPost]
		public async Task<IActionResult> EditBoardDirector(BoardOfDirectorsVm model)
		{
			await boardOfDirectorService.DeleteBoardOfDirectorByCompanyIdAndYear(model.companyId, Convert.ToInt32(model.year));

			var data = new BoardOfDirector
			{
				Id = 0,
				name = model.name,
				designation = model.designation,
				mobile = model.mobile,
				email = model.email,
				address = model.address,
				bio = model.bio,
				//photoUrl = fileName,
				companyId = model.companyId,
				year = model.year,
				status = 1, //chairmen
				isActive = 1,
				date = model.date
			};
			await boardOfDirectorService.SaveBoardOfDirector(data);
			var directorCount = model.dirNames.Length;


			for (int i = 0; i < directorCount; i++)
			{
				var dataDir = new BoardOfDirector
				{
					Id = 0,
					name = model.dirNames[i],
					designation = model.dirDesis[i],
					mobile = model.dirMobiles[i],
					email = model.dirEmails[i],
					address = model.dirAddresses[i],
					bio = model.dirBios[i],
					//photoUrl = directorFileName,
					companyId = model.companyId,
					company = model.company,
					year = model.year,
					status = 2, // director
					isActive = 1
				};
				await boardOfDirectorService.SaveBoardOfDirector(dataDir);
				var dataCeo = new BoardOfDirector
				{
					Id = 0,
					name = model.ceoname,
					designation = model.ceodesignation,
					mobile = model.ceomobile,
					email = model.ceoemail,
					address = model.ceoaddress,
					bio = model.ceobio,
					//photoUrl =CeoFileName,
					companyId = model.companyId,
					company = model.company,
					year = model.year,
					status = 3, //ceo
					isActive = 1
				};
				await boardOfDirectorService.SaveBoardOfDirector(dataCeo);
			}
			return RedirectToAction("Index", new { companyId = model.companyId, year = model.year });
		}
        public async Task<IActionResult> DeleteBoardDirector(int companyId, int year)
        {
            await boardOfDirectorService.DeleteBoardOfDirectorByCompanyIdAndYear(companyId, year);
            return RedirectToAction("FilterBOD");
        }

        [HttpGet]
		public async Task<IActionResult> Index(int companyId, int year)
		{
			//var companies = await boardOfDirectorService.GetAllCompanyListWithBOD();
			var companies = await boardOfDirectorService.GetAllCompanyListWithBOD(companyId, year);
			var data = new List<BODWithCompany>();
			foreach (var item in companies)
			{
				data.Add(new BODWithCompany
				{
					company = item,
					chairmen = await boardOfDirectorService.GetChairmenByCompanyIdAndYear(item.companyId, item.year),
					directors = await boardOfDirectorService.GetDirectorsByCompanyIdAndYear(item.companyId, item.year),
					ceo = await boardOfDirectorService.GetCeoByCompanyIdAndYear(item.companyId, item.year)
				});
			}
			var model1 = new BoardOfDirectorsVm
			{
				bODWithCompanies = data
			};
			return View(model1);
		}

		public async Task<IActionResult> FilterBOD()
		{
			var companies = await boardOfDirectorService.GetAllCompanyList();
			var years = new List<int>();
			for (int i = 1950; i <= DateTime.Now.Year; i++)
			{
				years.Add(i);
			}

			ViewBag.companies = companies;
			ViewBag.years = years;

			var model = new FilterBODVm
			{
				companies = companies,
				years = years
			};
			return View(model);
		}
		public async Task<IActionResult> FilterBODApi(int companyId, int year)
		{
			var companies = await boardOfDirectorService.GetAllCompanyListWithBOD(companyId, year);
			var data = new List<BODWithCompany>();
			foreach (var item in companies)
			{
				data.Add(new BODWithCompany
				{
					company = item,
					chairmen = await boardOfDirectorService.GetChairmenByCompanyIdAndYear(item.companyId, item.year),
					directors = await boardOfDirectorService.GetDirectorsByCompanyIdAndYear(item.companyId, item.year),
					ceo = await boardOfDirectorService.GetCeoByCompanyIdAndYear(item.companyId, item.year)
				});
			}
			var model1 = new BoardOfDirectorsVm
			{
				bODWithCompanies = data
			};
			return Json(model1);
		}
        public async Task<IActionResult> DeleteBODbyId(int companyId, int year)
        {
            await boardOfDirectorService.DeleteBoardOfDirectorById(companyId,year);
            return RedirectToAction(nameof(Index));
         
        }
    }
}