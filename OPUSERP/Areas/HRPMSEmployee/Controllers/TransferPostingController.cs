using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using OPUSERP.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using OPUSERP.Data.Entity;
using OPUSERP.ERPServices.MasterData.Interfaces;

namespace OPUSERP.Areas.HRPMSEmployee.Controllers
{
    [Authorize]
    [Area("HRPMSEmployee")]
    public class TransferPostingController : Controller
    {
        private readonly LangGenerate<TransferPosting> _lang;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IAddressService addressService;


        public TransferPostingController(IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IAddressService addressService)
        {
            _lang = new LangGenerate<TransferPosting>(hostingEnvironment.ContentRootPath);
            this.addressService = addressService;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        // GET: TransferPosting

        public ActionResult Index(int id)
        {
            ViewBag.employeeID = id.ToString();
            var model = new TransferPostingViewModel
            {
                fLang = _lang.PerseLang("Employee/TransferPostingEN.json")
            };
            return View(model);
        }

        // POST: TransferPosting/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([FromForm] TransferPostingViewModel model)
        {
            //string userName = HttpContext.User.Identity.Name;
            //var user = await _userManager.FindByNameAsync(userName);
            //var roles = await _userManager.GetRolesAsync(user);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //if (roles.Contains("HRAdmin") || roles.Contains("admin"))
            //{
            //    presentdata.isDelete = 0;
            //}
            //else
            //{
            //    presentdata.isDelete = 1;
              
            //}
            return Json(model);
        }

    }
}