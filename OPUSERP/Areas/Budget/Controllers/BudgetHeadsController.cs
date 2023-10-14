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
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.Helpers;

namespace OPUSERP.Areas.Budget.Controllers
{
    [Authorize]
    [Area("Budget")]
    public class BudgetHeadsController : Controller
    {
        private readonly LangGenerate<BudgetMainHeadLn> _lang;
        private readonly LangGenerate<BudgetHeadLn> _lang1;
        private readonly IBudgetHeadService budgetHeadService;
        private readonly ILedgerService ledgerService;
        private readonly IUserInfoes userInfo;

        public BudgetHeadsController(IHostingEnvironment hostingEnvironment, 
            IBudgetHeadService budgetHeadService, ILedgerService ledgerService, IUserInfoes userInfo)
        {
            _lang = new LangGenerate<BudgetMainHeadLn>(hostingEnvironment.ContentRootPath);
            _lang1 = new LangGenerate<BudgetHeadLn>(hostingEnvironment.ContentRootPath);
            this.budgetHeadService = budgetHeadService;
            this.ledgerService = ledgerService;
            this.userInfo = userInfo;
        }

        [Route("global/api/GetLedgerWithoutSubLedger/")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetLedgerWithoutSubLedger()
        {
            var data = await ledgerService.GetLedgerWithoutSubLedger();
            return Json(data);
            //return Json(data.Where(x=>x.group.natureId==4));
        }   

        [Route("global/api/GetSubLedgerBySbuId/{id}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetSubLedgerBySbuId(int Id)
        {
            return Json(await ledgerService.GetSubLedgerBySbuId(Id));
        }

    }
}