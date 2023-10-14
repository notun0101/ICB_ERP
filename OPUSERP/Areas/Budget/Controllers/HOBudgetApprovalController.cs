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
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.Helpers;
using OPUSERP.SCM.Helpers;
using OPUSERP.SCM.Services.Matrix.Interfaces;

namespace OPUSERP.Areas.Budget.Controllers
{
    [Authorize]
    [Area("HR")]
    public class HOBudgetApprovalController : Controller
    {
        private readonly LangGenerate<BudgetRequisitionLn> _lang;
        private readonly LangGenerate<BudgetRequisitionExcelLn> _lang1;
        private readonly IBudgetRequsitionMasterService budgetRequsitionMasterService;
        private readonly IHOBudgetRequsitionService hOBudgetRequsitionService;
        private readonly IBudgetHeadService budgetHeadService;
        private readonly IUserInfoes userInfo;
        private readonly IApprovalLogService approvalLogService;
        private readonly RequisitionStatusHistory requisitionStatusHistory;

        public HOBudgetApprovalController(IHostingEnvironment hostingEnvironment, IBudgetRequsitionMasterService budgetRequsitionMasterService, IUserInfoes userInfo, IBudgetHeadService budgetHeadService, RequisitionStatusHistory requisitionStatusHistory, IApprovalLogService approvalLogService, IHOBudgetRequsitionService hOBudgetRequsitionService)
        {
            _lang = new LangGenerate<BudgetRequisitionLn>(hostingEnvironment.ContentRootPath);
            _lang1 = new LangGenerate<BudgetRequisitionExcelLn>(hostingEnvironment.ContentRootPath);
            this.budgetRequsitionMasterService = budgetRequsitionMasterService;
            this.budgetHeadService = budgetHeadService;
            this.userInfo = userInfo;
            this.approvalLogService = approvalLogService;
            this.requisitionStatusHistory = requisitionStatusHistory;
            this.hOBudgetRequsitionService = hOBudgetRequsitionService;
        }
        
    }
}