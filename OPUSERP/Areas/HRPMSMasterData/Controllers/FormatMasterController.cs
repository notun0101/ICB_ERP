using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSMasterData.Models;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Formats;
using OPUSERP.HRPMS.Services.HrFormat.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace OPUSERP.Areas.HRPMSMasterData.Controllers
{
	[Authorize]
	[Area("HRPMSMasterData")]
	public class FormatMasterController : Controller
    {
		private readonly IHrFormat formatService;

		//private readonly IHostingEnvironment _hostingEnvironment;
		private readonly string rootPath;
		private readonly MyPDF myPDF;
		
		public FormatMasterController(IHrFormat formatService, IHostingEnvironment hostingEnvironment, IConverter converter)
		{
			this.formatService = formatService;

			myPDF = new MyPDF(hostingEnvironment, converter);
			rootPath = hostingEnvironment.ContentRootPath;
		}

		public async Task<IActionResult> Index()
        {
            var HrFormatMasters = new HrFormatMasterViewModel()
            {
                hrFormatMasters = await formatService.GetAllHrFormatMaster()
            };
           

            return View(HrFormatMasters);
        }

		[HttpPost]
		public async Task<IActionResult> Index(HrFormatMasterViewModel model)
		{

           var hrFormateMasterdata= await formatService.GetAllHrFormatMaster();
           var hrFrMasters= hrFormateMasterdata.Where(x => x.type == model.type).FirstOrDefault();
         
                var data = new HrFormatMaster
                    {
                    //    Id= model.formatMasterId, //Id
                        Id= (hrFrMasters==null)?0: hrFrMasters.Id, //Id
                       name = model.name,
                        type = model.type,
                        body = model.body
                    };
                  bool isSuccess=  await formatService.SaveHrFormatMaster(data);
            if(isSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error has found!!");
                return View(model);
            };

        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        public async Task<IActionResult> FormatDetails()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> FormatDetails(HrFormatDetailsViewModel model)
		{
			var data = new HrFormatDetails
			{
				Id = model.formatDetailsId,
				employeeId = model.employeeId,
				formatMasterId = model.formatMasterId,
				editedFormat = model.editedFormat,
				isActive = 1
			};
			var detailsId = await formatService.SaveHrFormatDetails(data);
			return RedirectToAction("FormatDetailsPdf", new { id = detailsId });
		}

		[AllowAnonymous]
		public async Task<IActionResult> FormatDetailsView(int id)
		{
			var data = new HrFormatDetailsViewModel
			{
				hrFormatDetail = await formatService.GetFormatDetailsById(id)
			};
			return View(data);
		}

		[AllowAnonymous]
		public async Task<IActionResult> FormatDetailsPdf(int id)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string fileName = "";
			string url = "";
			string status = "";

			try
			{
				url = scheme + "://" + host + "/HRPMSMasterData/FormatMaster/FormatDetailsView/" + id;
				status = myPDF.GeneratePDF(out fileName, url);
			}
			catch (Exception ex)
			{
				
			}

			if (status != "done")
			{
				return Content("<h1>Something Went Wrong</h1>");
			}

			var details = await formatService.GetFormatDetailsById(id);
			details.fileName = fileName;

			await formatService.SaveHrFormatDetails(details);

			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			//return new FileStreamResult(stream, "application/pdf");
			return RedirectToAction("FormatDetails");
		}


		#region Api
		public async Task<IActionResult> FormatMasterByType(string type)
		{
			var data = await formatService.GetFormatByTypeId(type);
			return Json(data);
		}

		public async Task<IActionResult> GetAllEmployeeInfoById(int id)
		{
			var data = await formatService.GetEmployeeInfoById(id);
			return Json(data);
		}

		public async Task<IActionResult> GetFormatByType(string type)
		{
			var data = await formatService.GetHrFormatMasterByType(type);
			return Json(data);
		}
        [HttpPost]
        public async Task<IActionResult> DeleteFormatById(int id)
		{
			var data = await formatService.DeleteHrFormatMasterById(id);
			return Json(data);
		}
		#endregion
	}
}