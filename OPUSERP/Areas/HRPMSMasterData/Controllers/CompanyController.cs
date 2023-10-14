using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.HRPMSMasterData.Models;
using OPUSERP.Areas.HRPMSMasterData.Models.Lang;
using OPUSERP.Data.Entity.Master;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using System;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSMasterData.Controllers
{
    [Authorize]
    [Area("HRPMSMasterData")]
    public class CompanyController : Controller
    {
        private readonly LangGenerate<CountryLn> _lang;
        private readonly IAddressService addressService;

        public CompanyController(IHostingEnvironment hostingEnvironment, IAddressService addressService)
        {
            _lang = new LangGenerate<CountryLn>(hostingEnvironment.ContentRootPath);
            this.addressService = addressService;
        }
        // GET: Company
        public async Task<IActionResult> CompanyEdit()
        {
            var model = new CompanyViewModel
              {

                allCompany = await addressService.GetCompany()

            };
            return View(model);
            
        }
		[HttpPost]
		public async Task<IActionResult> CompanyEdit([FromForm] CompanyViewModel model)
		{
            string fileName;
            if (model.companyFilePath != null)
            {
                string message = FileSave.SaveImageNew(out fileName, model.companyFilePath);
            }
            else
            {
                fileName = await addressService.GetCompanyFilePathById(model.companyId);
            };
            var data = new Company
			{
				Id = model.companyId,
				companyName = model.companyName,
                ownerName=model.ownerName,
                managerName=model.managerName,
                companyEmail=model.companyEmail,
                officeTelephone=model.officeTelephone,
                addressLine=model.addressLine,
                dateOfEstablishment=model.dateOfEstablishment,
                height = model.height,
                width = model.width,
                filePath=fileName,
                certificateOfInspir=model.certificateOfInspir,
                permission=model.permission,
                generation=model.generation,
                vision=model.vision,
                mission=model.mission,
                tinNo=model.tinNo,
                binNo=model.binNo,
                swiftCode=model.swiftCode,
                dateOfAgm=model.dateOfAgm,
                labelColor = model.labelColor


            };
			await addressService.SaveCompany(data);
			return RedirectToAction("CompanyEdit");
        }
    }
}