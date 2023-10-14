using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Accounting.Services.AccountingSettings.Interfaces;
using OPUSERP.Areas.Budget.Models;
using OPUSERP.Areas.Budget.Models.Lang;
using OPUSERP.Budget.Data.Entity;
using OPUSERP.Budget.Service.Interface;
using OPUSERP.Helpers;

namespace OPUSERP.Areas.Budget.Controllers
{
    [Authorize]
    [Area("Budget")]
    public class BudgetSubHeadsController : Controller
    {
        private readonly LangGenerate<BudgetSubHeadLn> _lang;
        private readonly IBudgetHeadService budgetHeadService;
        private readonly ILedgerService ledgerService;

        public BudgetSubHeadsController(IHostingEnvironment hostingEnvironment, ILedgerService ledgerService, IBudgetHeadService budgetHeadService)
        {
            _lang = new LangGenerate<BudgetSubHeadLn>(hostingEnvironment.ContentRootPath);
            this.budgetHeadService = budgetHeadService;
            this.ledgerService = ledgerService;
        }
        

        [Route("api/BudgetSubHead/GetBudgetSLHead")]
        [HttpGet]
        public async Task<IActionResult> GetBudgetSLHead()
        {
            return Json(await ledgerService.GetAllSubLedger());
        }
        [Route("api/BudgetSubHead/GetBudgetMLHead/{Id}")]
        [HttpGet]
        public async Task<IActionResult> GetBudgetMLHead(int Id)
        {
            var ledgerrel = await ledgerService.GetLedgerRelationBySubLedgerId(Id);
            List<int?> ledgerlist = ledgerrel.Select(x => x.ledgerId).ToList();
               var data = await ledgerService.GetLedgerWithSubLedger();
            return Json(data.Where(x=>ledgerlist.Contains(x.Id)));
        }
        
    }
}