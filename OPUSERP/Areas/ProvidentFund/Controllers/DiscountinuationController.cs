using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace OPUSERP.Areas.ProvidentFund.Controllers
{
    [Authorize]
    [Area("ProvidentFund")]

    public class DiscountinuationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DiscountinuationOverview()
        {
            return View();
        }

        public async Task<IActionResult> ApplyDiscountinuation()
        {
            return View();
        }
        public async Task<IActionResult> DiscountinuationPending()
        {
            return View();
        }
        public async Task<IActionResult> EditAppliedDiscont()
        {
            return View();
        }
        public async Task<IActionResult> FinalSettlement()
        {
            return View();
        }
        public async Task<IActionResult> EditFinalSettlement()
        {
            return View();
        }

    }
}