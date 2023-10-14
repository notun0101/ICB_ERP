using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.Budget.Models;
using OPUSERP.Budget.Data.Entity;
using OPUSERP.Budget.Service.Interface;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;

namespace OPUSERP.Areas.Budget.Controllers
{
    [Authorize]
    [Area("HR")]
    public class DisbursementController : Controller
    {
        private readonly IBudgetRequsitionMasterService budgetRequsitionMasterService;
        private readonly IUserInfoes userInfo;
        private readonly ISpecialBranchUnitService specialBranchUnitService;
        private readonly IBudgetDisbursmentMasterService budgetDisbursmentMasterService;

        public DisbursementController(IHostingEnvironment hostingEnvironment, IBudgetRequsitionMasterService budgetRequsitionMasterService, IUserInfoes userInfo, ISpecialBranchUnitService specialBranchUnitService, IBudgetDisbursmentMasterService budgetDisbursmentMasterService)
        {
            this.budgetRequsitionMasterService = budgetRequsitionMasterService;
            this.userInfo = userInfo;
            this.specialBranchUnitService = specialBranchUnitService;
            this.budgetDisbursmentMasterService = budgetDisbursmentMasterService;
        }

        
        
    }
}