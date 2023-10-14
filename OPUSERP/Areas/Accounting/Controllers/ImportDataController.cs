using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OPUSERP.Accounting.Data.Entity.Voucher;
using OPUSERP.Accounting.Services.AccountingSettings.Interfaces;
using OPUSERP.Accounting.Services.MasterData.Interfaces;
using OPUSERP.Accounting.Services.Voucher.Interfaces;
using OPUSERP.Areas.Accounting.Models;
using OPUSERP.Areas.Budget.Models;
using OPUSERP.Budget.Service.Interface;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.Payroll.Services.Salary.Interfaces;
using OPUSERP.SCM.Services.MasterData.Interfaces;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Accounting.Controllers
{
    [Authorize]
    [Area("Accounting")]
    public class ImportDataController : Controller
    {
        private readonly IVoucherTypeService voucherTypeService;
        private readonly ILedgerService ledgerService;
        private readonly IAccountingReportService reportingService;
        private readonly ICostCentreService costCentreService;
        private readonly IERPCompanyService eRPCompanyService;
        private readonly ISpecialBranchUnitService specialBranchUnitService;
        private readonly IProjectService projectService;
        private readonly IUserInfoes userInfo;
        private readonly IBudgetRequsitionMasterService budgetRequsitionMasterService;
        private readonly ISalaryService salaryService;
        private readonly IVoucherService voucherService;

        private readonly string rootPath;
        private readonly MyPDF myPDF;
        public string FileName;

        public ImportDataController(IVoucherTypeService voucherTypeService,
            IUserInfoes userInfo, IBudgetRequsitionMasterService budgetRequsitionMasterService,
            ISpecialBranchUnitService specialBranchUnitService,
            IProjectService projectService, ILedgerService ledgerService,
            IAccountingReportService reportingService, ICostCentreService costCentreService,
            IHostingEnvironment hostingEnvironment, IConverter converter,
            IERPCompanyService eRPCompanyService, ISalaryService salaryService, IVoucherService voucherService)
        {
            this.voucherTypeService = voucherTypeService;
            this.ledgerService = ledgerService;
            this.reportingService = reportingService;
            this.costCentreService = costCentreService;
            this.eRPCompanyService = eRPCompanyService;
            this.specialBranchUnitService = specialBranchUnitService;
            this.projectService = projectService;
            this.userInfo = userInfo;
            this.budgetRequsitionMasterService = budgetRequsitionMasterService;
            this.salaryService = salaryService;
            this.voucherService = voucherService;

            //For PDF
            myPDF = new MyPDF(hostingEnvironment, converter);
            rootPath = hostingEnvironment.ContentRootPath;
        }
        // GET: AccountingReport
        public ActionResult Index()
        {
            BulkUploadModel model = new BulkUploadModel();
            return View(model);
        }


        public async Task<IActionResult> UploadedVoucerList()
        {
            BulkUploadModel model = new BulkUploadModel();
            model.voucherLists = await voucherService.AllUploadedVoucherData();
            ViewBag.fileNo = "";
            if (model.voucherLists.Count() > 0)
            {
                model.fileList = model.voucherLists.OrderByDescending(x=> x.createdAt).Select(x => x.refNo).Distinct().ToList();
                ViewBag.fileNo = model.fileList.First();
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] IFormFile formFile)
        {


            if (formFile == null || formFile.Length <= 0)
            {
                return Json("formfile is empty");
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return Json("Not Support file extension");
            }
            int rownumber = 0;
            try
            {
                //string userName = HttpContext.User.Identity.Name;
                // var user = await _userManager.FindByNameAsync(userName);
                string userName = User.Identity.Name;
                var userInfos = await userInfo.GetUserInfoByUser(userName);
                ViewBag.sbuId = userInfos.projectId;
                var salaryYear = await salaryService.GetAllSalaryYear();
                var taxYear = await salaryService.GetAllTaxYear();
                var company = await eRPCompanyService.GetAllCompany();
                var ledgerRelations = await ledgerService.GetSubledgerR();
                var projects = await projectService.GetProjectList();
                var sbus = await specialBranchUnitService.GetSpecialBranchUnit();




                using (var stream = new MemoryStream())
                {
                    await formFile.CopyToAsync(stream);

                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                        var rowCount = worksheet.Dimension.Rows;
                        int _min = 1000000;
                        int _max = 9999999;
                        Random _rdm = new Random();
                        //string fileNo = _rdm.Next(_min, _max).ToString();
                        string fileNo = "DM-" + DateTime.Now.ToString("ddMMyyyyhhmm");
                        for (int row = 2; row <= rowCount; row++)
                        {
                            rownumber = row;
                            DateTime? _voucherDate = null;
                            DateTime? _chequeDate = null;
                            decimal? _amount = 0;
                            int? projectid = null;
                            int? sbuId = null;
                            int? ledgerRelationId = null;

                            string voucherDate = worksheet.Cells[row, 1].Value == null ? "" : worksheet.Cells[row, 1].Value.ToString().Trim();

                            if (voucherDate == "")
                            {
                                continue;
                            }
                            string voucherNo = worksheet.Cells[row, 2].Value == null ? "" : worksheet.Cells[row, 2].Value.ToString().Trim();

                            string voucherType = worksheet.Cells[row, 3].Value == null ? "" : worksheet.Cells[row, 3].Value.ToString().Trim();

                            string description = worksheet.Cells[row, 4].Value == null ? "" : worksheet.Cells[row, 4].Value.ToString().Trim();

                            string sBUCode = worksheet.Cells[row, 5].Value == null ? "" : worksheet.Cells[row, 5].Value.ToString().Trim();

                            sbuId = sbus.Where(x => x.branchCode?.ToLower() == sBUCode.ToLower()).Select(x => x.Id).FirstOrDefault();

                            var specialBranch = sbus.Where(x => x.Id == sbuId).FirstOrDefault();

                            string projectCode = worksheet.Cells[row, 6].Value == null ? "" : worksheet.Cells[row, 6].Value.ToString().Trim();
                            if (specialBranch != null)
                            {
                                projectid = projects.Where(x => x.specialBranchUnitId == specialBranch?.Id &&  x.projectShortName?.ToLower() == projectCode.ToLower()).Select(x => x.Id).FirstOrDefault();
                            }
                            

                            string chequeNo = worksheet.Cells[row, 7].Value == null ? "" : worksheet.Cells[row, 7].Value.ToString().Trim();

                            string chequeDate = worksheet.Cells[row, 8].Value == null ? "" : worksheet.Cells[row, 8].Value.ToString().Trim();

                            string accountsHeadName = worksheet.Cells[row, 9].Value == null ? "" : worksheet.Cells[row, 9].Value.ToString().Trim();

                            string accountsHeadCode = worksheet.Cells[row, 10].Value == null ? "" : worksheet.Cells[row, 10].Value.ToString().Trim();

                            ledgerRelationId = ledgerRelations.Where(x => x.ledger?.accountCode?.ToLower() == accountsHeadCode.ToLower()).Select(x => x.Id).FirstOrDefault();

                            string debit = worksheet.Cells[row, 11].Value == null ? "" : worksheet.Cells[row, 11].Value.ToString().Trim();

                            string credit = worksheet.Cells[row, 12].Value == null ? "" : worksheet.Cells[row, 12].Value.ToString().Trim();


                            int _transectionModeId = 1;


                            if (debit != "0")
                            {
                                _amount = Convert.ToDecimal(debit);
                                _transectionModeId = 1;
                            }

                            if (credit != "0")
                            {
                                _amount = Convert.ToDecimal(credit);   
                                _transectionModeId = 2;
                            }

                            if (chequeDate != "")
                            {
                                _chequeDate = Convert.ToDateTime(voucherDate);
                            }
                            
                            if (voucherDate != "")
                            {
                                _voucherDate = Convert.ToDateTime(voucherDate);
                            }

                            int? _voucherType = null;
                            if (voucherType == "Received")
                            {
                                _voucherType = 8;
                            }
                            else if (voucherType == "Payment")
                            {
                                _voucherType = 6;
                            }
                            else if (voucherType == "Journal")
                            {
                                _voucherType = 7;
                            }
                            else if (voucherType == "Contra")
                            {
                                _voucherType = 1;
                            }

                            int? hasWrongData = (sbuId == 0) ? 1 : (projectid == 0) ? 1 : (ledgerRelationId == 0) ? 1 : 0;

                            UploadedVoucherData voucher = new UploadedVoucherData
                            {
                                Id = 0,
                                refNo = fileNo,
                                voucherNo = voucherNo,
                                voucherDate = _voucherDate,
                                voucherTypeId = _voucherType,
                                description = description,
                                sbuName = sBUCode,
                                chequeNo = chequeNo,
                                chequeDate = _chequeDate,
                                isProcessed =0,


                                accountName = accountsHeadName,
                                accountCode = accountsHeadCode,
                                projectName = (projectCode == null) ? "" : projectCode,

                                amount = _amount,
                                transectionModeId = _transectionModeId,

                                specialBranchUnitId = (sbuId == 0) ? null : sbuId,
                                projectId = (projectid == 0) ? null : projectid,
                                ledgerRelationId = (ledgerRelationId == 0) ? null : ledgerRelationId,

                                companyId = company.OrderByDescending(a => a.Id).FirstOrDefault().Id,
                                fiscalYearId = salaryYear.OrderByDescending(a => a.Id).FirstOrDefault().Id,
                                taxYearId = taxYear.OrderByDescending(a => a.Id).FirstOrDefault().Id,
                                fundSourceId = 1,
                                hasWrongData = hasWrongData
                            };
                            int vId = await voucherService.SaveUplodedvoucherdata(voucher);

                        }
                    }
                }
                return RedirectToAction("UploadedVoucerList", "ImportData", new { Area = "Accounting" });
            }
            catch (Exception ex)
            {
                //Console.WriteLine(rownumber);
                return Json(false);
            }
            // return RedirectToAction(nameof(UploadedVoucerList));
            // return RedirectToAction("UploadedVoucerList");
        }

        public async Task<IActionResult> DeleteUploadedFile(string fileNo)
        {

            try
            {
                var voucherMasters = await voucherService.GetvoucherMasterByFileNo(fileNo);
                if (voucherMasters.Count() > 0)
                {
                    foreach (var voucherMaster in voucherMasters)
                    {
                        await voucherService.DeletevoucherDetailByVMId(voucherMaster.Id);
                        await voucherService.DeletevoucherMasterById(voucherMaster.Id);
                    }
                }
                await voucherService.DeleteUploadedVoucherDataByFileNo(fileNo);

                return Json(true);
            }
            catch (Exception ex)
            {

                return Json(false);
            }
           
           

        }


        public async Task<IActionResult> GetFileNoWiseData(string fileNo)
        {
            return Json(await voucherService.GetFileNoWiseData(fileNo));

        }

        public async Task<IActionResult> ProcessFileNoWiseData(string fileNo)
        {
            bool hasMessage = false;
            try
            {
             
                string WrongVouchers = "";
                string ProcesseVouchers = "";
                string DebitCreditMismatchedVouchers = "";
                var uploadedDatas = await voucherService.GetUploadedDataFileNoWise(fileNo);

                foreach (var uploadedVouchers in uploadedDatas.GroupBy(x => x.voucherNo))
                {
                   


                    decimal? hasWrongdata = uploadedVouchers.Sum(x => (x.hasWrongData==null)?0: x.hasWrongData);

                    if (hasWrongdata > 0) {
                        WrongVouchers +=  uploadedVouchers.Key + ",";
                        hasMessage = true;
                        continue;
                    }
                    

                    decimal? isProcessed = uploadedVouchers.Sum(x => (x.isProcessed == null) ? 0 : x.isProcessed);

                    if (isProcessed > 0) {
                        ProcesseVouchers += uploadedVouchers.Key + ",";
                        hasMessage = true;
                        continue;
                    }

                    decimal debitAmount = uploadedVouchers.Where(x=>x.transectionModeId ==1).Sum(x => (x.amount == null) ? 0 : Convert.ToDecimal(x.amount));
                    decimal ceditAmount = uploadedVouchers.Where(x=>x.transectionModeId ==2).Sum(x => (x.amount == null) ? 0 : Convert.ToDecimal(x.amount));
                    if(Math.Round(debitAmount) != Math.Round(ceditAmount))
                    {
                        DebitCreditMismatchedVouchers += uploadedVouchers.Key + ",";
                        hasMessage = true;
                        continue;
                    }

                    string userName = User.Identity.Name;
                    var userInfos = await userInfo.GetUserInfoByUser(userName);
                    ViewBag.sbuId = userInfos.projectId;
                    var salaryYear = await salaryService.GetAllSalaryYear();
                    var taxYear = await salaryService.GetAllTaxYear();
                    var company = await eRPCompanyService.GetAllCompany();
                    var data = uploadedVouchers.FirstOrDefault();
                    VoucherMaster voucherMaster = new VoucherMaster
                    {
                        Id = 0,
                        voucherNo = data.voucherNo,
                        refNo = data.refNo,
                        voucherDate = data.voucherDate,
                        voucherTypeId = data.voucherTypeId,
                        remarks = data.description,
                        voucherAmount = data.amount,
                        isPosted = 2,
                        companyId = company.OrderByDescending(a => a.Id).FirstOrDefault().Id,
                        fiscalYearId = salaryYear.OrderByDescending(a => a.Id).FirstOrDefault().Id,
                        taxYearId = taxYear.OrderByDescending(a => a.Id).FirstOrDefault().Id,
                        fundSourceId = 1,
                        projectId = data.projectId,
                        chequeNumber = data.chequeNo,
                        chequeDate = data.chequeDate,
                        fileNo= data.refNo

                    };
                    int VMID = await voucherService.SavevoucherMaster(voucherMaster);

                    var ledgers = await ledgerService.GetLedgerForPayment((int)data.specialBranchUnitId);
                    List<int> ledgerRelationIds = ledgers.Select(x => (int)x.Id).ToList();
                    bool isPrint = true;
                    foreach (var Upvoucher in uploadedVouchers.Where(x=>x.isProcessed ==0))
                    {
                        isPrint = ledgerRelationIds.Contains((int)Upvoucher.ledgerRelationId) ? true : false;
                        VoucherDetail voucherDetail = new VoucherDetail();
                        voucherDetail.Id = 0;
                        voucherDetail.voucherId = VMID;
                        voucherDetail.ledgerRelationId = Convert.ToInt32(Upvoucher.ledgerRelationId);
                        voucherDetail.transectionModeId = Upvoucher.transectionModeId;
                        voucherDetail.amount = Upvoucher.amount;
                        voucherDetail.accountName = Upvoucher.ledgerRelation?.ledger?.accountName;
                        voucherDetail.isPrincAcc = (isPrint) ? 1 : 0;
                        voucherDetail.specialBranchUnitId = userInfos?.specialBranchUnitId;

                        int vid = await voucherService.SavevoucherDetail(voucherDetail);

                        Upvoucher.isProcessed = 1;
                        int UplodedvoucherId= await voucherService.SaveUplodedvoucherdata(Upvoucher);

                        isPrint = false;
                    }


                }


                return Json(new { Success= true, hasMessage, WrongVouchers, ProcesseVouchers, DebitCreditMismatchedVouchers, Message = "Processed Successfully!" });
                }
            catch (Exception ex)
            {

                return Json(new { Success = false, Message= "Error occurred When Uploading..!" });
            }
            

        }
    }
}