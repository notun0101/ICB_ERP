using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OPUSERP.Areas.LMS.Controllers
{
    [Authorize]
    [Area("LMS")]
    public class TestController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> TestList()
        {
            return View();
        }
    }
}