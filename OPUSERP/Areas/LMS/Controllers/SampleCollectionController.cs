using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OPUSERP.Areas.LMS.Controllers
{
    [Authorize]
    [Area ("LMS")]
    public class SampleCollectionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> SaveSampleInfo()
        {
            return View();
        }
        
    }
}