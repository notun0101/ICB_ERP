using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.Budget.Models;
using OPUSERP.Budget.Data.Entity;
using OPUSERP.Budget.Service.Interface;
using OPUSERP.ERPService.AuthService.Interfaces;
using Microsoft.AspNetCore.Http;
using OPUSERP.Helpers;
using System.Dynamic;
using OPUSERP.Areas.Budget.Models.Lang;
using Microsoft.AspNetCore.Hosting;
using OPUSERP.Data.Entity.Matrix;
using OPUSERP.SCM.Services.Matrix.Interfaces;
using OPUSERP.Areas.SCMMatrix.Models;
using DinkToPdf.Contracts;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.HRPMS.Data.Entity.Master;

namespace OPUSERP.Areas.Budget.Controllers
{
    [Authorize]
    [Area("HR")]
    public class HOBudgetRequisitionController : Controller
    {
        private readonly LangGenerate<BudgetRequisitionLn> _lang;
        private readonly LangGenerate<BudgetRequisitionExcelLn> _lang1;
        private readonly IERPCompanyService eRPCompanyService;
        private readonly IHOBudgetRequsitionService hOBudgetRequsitionService;
        private readonly IBudgetRequsitionMasterService budgetRequsitionMasterService;
        private readonly IBudgetHeadService budgetHeadService;
        private readonly IUserInfoes userInfo;
        private readonly IApprovalLogService approvalLogService;
        private readonly IApprovalMatrixService approvalMatrixService;

        private readonly string rootPath;
        private readonly MyPDF myPDF;

        public HOBudgetRequisitionController(IHostingEnvironment hostingEnvironment, IConverter converter, IERPCompanyService eRPCompanyService, IHOBudgetRequsitionService hOBudgetRequsitionService, IUserInfoes userInfo, IBudgetHeadService budgetHeadService, IApprovalLogService approvalLogService, IApprovalMatrixService approvalMatrixService, IBudgetRequsitionMasterService budgetRequsitionMasterService)
        {
            _lang = new LangGenerate<BudgetRequisitionLn>(hostingEnvironment.ContentRootPath);
            _lang1 = new LangGenerate<BudgetRequisitionExcelLn>(hostingEnvironment.ContentRootPath);
            this.eRPCompanyService = eRPCompanyService;
            this.hOBudgetRequsitionService = hOBudgetRequsitionService;
            this.budgetHeadService = budgetHeadService;
            this.userInfo = userInfo;
            this.approvalLogService = approvalLogService;
            this.approvalMatrixService = approvalMatrixService;
            this.budgetRequsitionMasterService = budgetRequsitionMasterService;

            myPDF = new MyPDF(hostingEnvironment, converter);
            rootPath = hostingEnvironment.ContentRootPath;
        }
        

        [AllowAnonymous]
        public IActionResult RequisitionPreviewPdf(int id)
        {
            string fileName;
            string host = HttpContext.Request.Host.ToString();
            string scheme = Request.Scheme;
            string url = string.Empty;
            url = $"" + scheme + "://" + host + "/Budget/HOBudgetRequisition/RequisitionPreview?id=" + id;

            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

    }

}