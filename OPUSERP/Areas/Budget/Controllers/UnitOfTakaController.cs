using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.Budget.Models;
using OPUSERP.Areas.Budget.Models.Lang;
using OPUSERP.Budget.Data.Entity;
using OPUSERP.Budget.Service.Interface;
using OPUSERP.Helpers;

namespace OPUSERP.Areas.Budget.Controllers
{
    [Authorize]
    [Area("Budget")]
    public class UnitOfTakaController : Controller
    {
        private readonly LangGenerate<UnitLn> _lang;
        private readonly IUnitOfTakaService unitOfTakaService;

        public UnitOfTakaController(IHostingEnvironment hostingEnvironment, IUnitOfTakaService unitOfTakaService)
        {
            _lang = new LangGenerate<UnitLn>(hostingEnvironment.ContentRootPath);
            this.unitOfTakaService = unitOfTakaService;
        }
    }
}