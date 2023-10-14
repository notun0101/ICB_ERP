using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.Accounting.Models;
using OPUSERP.Areas.Budget.Models;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.Helpers;
using System.IO;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Budget.Controllers
{
    [Authorize]
    [Area("HR")]
    public class BudgetBranchController : Controller
    {
        private readonly IConverter converter;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IUserInfoes userInfo;
        private readonly IERPCompanyService eRPCompanyService;
        private readonly MyPDF myPDF;
        private readonly string rootPath;
        public string FileName;

        //private readonly IAddressService addressService;

        public BudgetBranchController(IConverter converter, IHostingEnvironment hostingEnvironment, IUserInfoes userInfo, IERPCompanyService eRPCompanyService)
        {
            this.converter = converter;
            this.hostingEnvironment = hostingEnvironment;
            this.userInfo = userInfo;
            this.eRPCompanyService = eRPCompanyService;
            myPDF = new MyPDF(hostingEnvironment, converter);
            rootPath = hostingEnvironment.ContentRootPath;
        }

        // GET: 
        public IActionResult Index()
        {
            BudgetBranchViewModel model = new BudgetBranchViewModel
            {
                //countries = await addressService.GetAllContry()
            };
            return View(model);
        }


    

        // POST: Country/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([FromForm] BudgetBranchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //model.countries = await addressService.GetAllContry();
                return View(model);
            }

            //BudgetHead country = new BudgetHead
            //{
            //    Id = Int32.Parse(model.countryId),
            //    countryCode = model.countryCode,
            //    shortName = model.shortName,
            //    countryName = model.countryName,
            //    countryNameBn = model.countryNameBn
            //};

            //await addressService.SaveCountry(country);

            return RedirectToAction(nameof(Index));

        }
    }
}