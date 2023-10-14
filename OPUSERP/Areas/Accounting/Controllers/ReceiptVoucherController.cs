﻿using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Accounting.Data.Entity.Voucher;
using OPUSERP.Accounting.Services.AccountingSettings.Interfaces;
using OPUSERP.Accounting.Services.MasterData.Interfaces;
using OPUSERP.Accounting.Services.Voucher.Interfaces;
using OPUSERP.Areas.Accounting.Models;
using OPUSERP.Areas.MasterData.Models;
using OPUSERP.Data.Entity.AttachmentComment;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.ERPServices.AttachmentComment.Interfaces;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.Helpers;
using OPUSERP.Payroll.Services.Salary.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Accounting.Controllers
{
    [Authorize]
    [Area("Accounting")]
    public class ReceiptVoucherController : Controller
    {
        private readonly ILedgerService ledgerService;
        private readonly IVoucherService voucherService;
        private readonly IAttachmentCommentService attachmentCommentService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ICostCentreService costCentreService;
        private readonly ISalaryService salaryService;
        private readonly IERPCompanyService eRPCompanyService;
        private readonly string rootPath;
        private readonly MyPDF myPDF;
        private readonly IUserInfoes userInfo;
        private readonly ISignatureService signatureService;

        public string FileName;
        public ReceiptVoucherController(IHostingEnvironment hostingEnvironment, IUserInfoes userInfo, ICostCentreService costCentreService, ILedgerService ledgerService, IAttachmentCommentService attachmentCommentService, IVoucherService voucherService, ISalaryService salaryService, IERPCompanyService eRPCompanyService, IConverter converter, ISignatureService signatureService)
        {
            this.ledgerService = ledgerService;
            this._hostingEnvironment = hostingEnvironment;
            this.attachmentCommentService = attachmentCommentService;
            this.voucherService = voucherService;
            this.ledgerService = ledgerService;
            this.costCentreService = costCentreService;
            this.salaryService = salaryService;
            this.eRPCompanyService = eRPCompanyService;
            this.userInfo = userInfo;
            this.signatureService = signatureService;
            myPDF = new MyPDF(hostingEnvironment, converter);
            rootPath = hostingEnvironment.ContentRootPath;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int id, string actionType)
        {
            string username = HttpContext.User.Identity.Name;
            var userinfo = await userInfo.GetUserInfoByUser(username);
            ViewBag.branchid = userinfo?.specialBranchUnitId;

            ViewBag.masterId = 0;
            if (id > 0)
            {
                ViewBag.masterId = id;
            }
            var data = await voucherService.GetvoucherDetailByMasterId(id);

            VoucherMasterViewModel model = new VoucherMasterViewModel
            {
                VoucherMaster = await voucherService.GetGetvoucherMasterById(id),
                costCentres = await costCentreService.GetCostCentres()

            };
            if (model.VoucherMaster == null) model.VoucherMaster = new VoucherMaster();


            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> Index([FromForm] VoucherMasterViewModel model)
        {
            string userName = User.Identity.Name;

            var userInfos = await userInfo.GetUserInfoByUser(userName);
            var salaryYear = await salaryService.GetAllSalaryYear();
            var taxYear = await salaryService.GetAllTaxYear();
            var company = await eRPCompanyService.GetAllCompany();
            VoucherMaster voucherMaster = new VoucherMaster
            {
                Id = Convert.ToInt32(model.voucherId),
                voucherNo = model.voucherNo,
                refNo = model.refNo,
                voucherDate = model.voucherDate,
                voucherTypeId = 8,
                remarks = model.remarks,
                voucherAmount = model.voucherAmount,
                isPosted = 1,
                companyId = company.OrderByDescending(a => a.Id).FirstOrDefault().Id,
                fiscalYearId = salaryYear.OrderByDescending(a => a.Id).FirstOrDefault().Id,
                taxYearId = taxYear.OrderByDescending(a => a.Id).FirstOrDefault().Id,
                fundSourceId = 1,
                projectId = model.projectId
            };
            int VMID = await voucherService.SavevoucherMaster(voucherMaster);

            if (model.voucherId > 0)
            {
                await voucherService.DeleteCostCentreAllocationByVMId(Convert.ToInt32(model.voucherId));
                await voucherService.DeletevoucherDetailByVMId(Convert.ToInt32(model.voucherId));

            }
            if (model.ledgerRelationId.Length > 0)
            {
                var doAccountCode = 0;
                for (int i = 0; i < model.ledgerRelationId.Length; i++)
                {
                    //if (model.accountCode[i].StartsWith("4"))
                    if (model.natureId[i] == 4)
                    {
                        int vid = 0;
                        decimal Amount = 0;

                        for (int z = 0; z < model.costCentreallId.Length; z++)
                        {
                            if (model.transectionModeId[i] == 1)
                            {
                                if (model.costCentreallId[z] != 0 && model.ledgerRelationId[z] == model.ledgerRelationId[i] && model.transectionModeId[z] == model.transectionModeId[i])
                                {
                                    Amount = Amount + (decimal)model.drAmount[z];
                                }
                            }
                            else
                            {
                                if (model.costCentreallId[z] != 0 && model.ledgerRelationId[z] == model.ledgerRelationId[i] && model.transectionModeId[z] == model.transectionModeId[i])
                                {
                                    Amount = Amount + (decimal)model.crAmount[z];
                                }
                            }
                        }
                        VoucherDetail voucherDetail = new VoucherDetail();

                        voucherDetail.Id = 0;
                        voucherDetail.voucherId = VMID;
                        voucherDetail.ledgerRelationId = Convert.ToInt32(model.ledgerRelationId[i]);
                        voucherDetail.transectionModeId = model.transectionModeId[i];

                        voucherDetail.amount = Amount;
                        voucherDetail.accountName = model.subLedgerName[i];
                        voucherDetail.isPrincAcc = model.isPrincAcc[i];
                        voucherDetail.specialBranchUnitId = userInfos?.specialBranchUnitId; // new 11 march

                        if (doAccountCode != (int)model.ledgerRelationId[i])
                        {
                            vid = await voucherService.SavevoucherDetail(voucherDetail);


                            for (int j = 0; j < model.costCentreallId.Length; j++)
                            {
                                if (model.ledgerRelationId[j] == model.ledgerRelationId[i])
                                {
                                    CostCentreAllocation costCentreAllocation = new CostCentreAllocation();
                                    costCentreAllocation.Id = 0;
                                    costCentreAllocation.costCentreId = model.costCentreallId[j];
                                    costCentreAllocation.voucherDetailId = vid;
                                    if (model.transectionModeId[i] == 1)
                                    {
                                        costCentreAllocation.amount = model.drAmount[j];
                                    }
                                    else
                                    {
                                        costCentreAllocation.amount = model.crAmount[j];
                                    }
                                    if (costCentreAllocation.costCentreId > 0 && costCentreAllocation.amount > 0)
                                    {
                                        await voucherService.SavecostCentreAllocation(costCentreAllocation);
                                    }
                                }
                            }
                            doAccountCode = (int)model.ledgerRelationId[i];
                        }
                    }
                    else
                    {
                        VoucherDetail voucherDetail = new VoucherDetail();

                        voucherDetail.Id = 0;
                        voucherDetail.voucherId = VMID;
                        voucherDetail.ledgerRelationId = Convert.ToInt32(model.ledgerRelationId[i]);
                        voucherDetail.transectionModeId = model.transectionModeId[i];
                        if (model.transectionModeId[i] == 1)
                        {
                            voucherDetail.amount = model.drAmount[i];
                        }
                        else
                        {
                            voucherDetail.amount = model.crAmount[i];
                        }
                        voucherDetail.accountName = model.subLedgerName[i];
                        voucherDetail.isPrincAcc = model.isPrincAcc[i];
                        voucherDetail.specialBranchUnitId = userInfos?.specialBranchUnitId; // new 11 march

                        int vid = await voucherService.SavevoucherDetail(voucherDetail);

                    }
                }

            }
            return Json(VMID);
        }

        [HttpGet]
        public async Task<IActionResult> ReceiptVoucherList()
        {

            var model = new VoucherMasterViewModel
            {
                GetVoucherMasters = await voucherService.GetvoucherMasterDetailsByTypeId(8),

            };

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult ReceiptVoucherPdfAction(int MasterId)
        {

            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/Accounting/ReceiptVoucher/ReceiptVoucherPDF?MasterId=" + MasterId;

            string fileName;
            string status = myPDF.GeneratePDFVoucher(out fileName, url);


            FileName = fileName;
            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ReceiptVoucherPDF(int MasterId)
        {
            var data = await voucherService.GetvoucherDetailByMasterId(MasterId);
            var ledgerRelationId = data.FirstOrDefault().ledgerRelationId;
            var dataLedger = await ledgerService.GetledgerRelationByLedgerRelationId(ledgerRelationId);
            var companyId = dataLedger.FirstOrDefault().ledger.companyId;
            var costInf = await voucherService.GetCostCentreAllocationsByDetailId(MasterId);
            if (costInf == null)
            {
                costInf = new List<CostCentreAllocation>();
            }

            var model = new VoucherMasterViewModel
            {
                VoucherMaster = await voucherService.GetGetvoucherMasterById(MasterId),
                GetVoucherDetailsByMasterId = await voucherService.GetvoucherDetailByMasterId(MasterId),
                costCentreAllocations = costInf,
                companies = await eRPCompanyService.GetAllCompany(),
                getSignatureViewModel = await signatureService.GetSignatureByType(1)

            };
            string Val = model.GetVoucherDetailsByMasterId.Where(x => x.transectionModeId == 1).Sum(x => x.amount).ToString();
            ViewBag.InWord = AmountInWord.ConvertToWords(Val);

            return PartialView(model);
        }

        public async Task<IActionResult> ReceiptDetails(int Id)
        {
            try
            {
                ViewBag.masterId = Id;
                var CommentInfo = await attachmentCommentService.GetCommentByActionTypeId(Id, "Voucher", 4);
                if (CommentInfo == null)
                {
                    CommentInfo = new List<Comment>();
                }
                var photoInfo = await attachmentCommentService.GetDocumentAttachmentByActionId(Id, "Voucher", "photo", 4);
                if (photoInfo == null)
                {
                    photoInfo = new List<DocumentPhotoAttachment>();
                }
                var docInfo = await attachmentCommentService.GetDocumentAttachmentByActionId(Id, "Voucher", "Document", 4);
                if (docInfo == null)
                {
                    docInfo = new List<DocumentPhotoAttachment>();
                }
                var costInf = await voucherService.GetCostCentreAllocationsByDetailId(Id);
                if (costInf == null)
                {
                    costInf = new List<CostCentreAllocation>();
                }
                var model = new VoucherMasterViewModel
                {
                    GetGetvoucherMasterById = await voucherService.GetGetvoucherMasterById(Id),
                    VoucherDetails = await voucherService.GetvoucherDetailByMasterId(Id),
                    GetVoucherDetailsByMasterId = await voucherService.GetvoucherDetailByMasterId(Id),
                    costCentreAllocations = costInf,
                    comments = CommentInfo,
                    photoes = photoInfo,
                    documents = docInfo,
                };
                string Val = model.GetVoucherDetailsByMasterId.Where(x => x.transectionModeId == 1).Sum(x => x.amount).ToString();
                ViewBag.InWord = AmountInWord.ConvertToWords(Val);
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        [HttpPost]
        public async Task<ActionResult> SaveComment([FromForm] CommentsViewModel model)
        {
            try
            {
                Comment entityComment = new Comment
                {
                    Id = Convert.ToInt32(model.commentsId),
                    actionType = "Voucher",
                    actionTypeId = model.actionTypeId,
                    title = model.title,
                    comment = model.comment,
                    moduleId = 4,
                };
                int commentsId = await attachmentCommentService.SaveComment(entityComment);
                return RedirectToAction("ReceiptDetails", "ReceiptVoucher", new { id = model.actionTypeId, Area = "Accounting" });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteComments(int Id, int actionId)
        {
            await attachmentCommentService.DeleteCommentById(Id);
            return RedirectToAction("ReceiptDetails", "ReceiptVoucher", new { id = actionId, Area = "Accounting" });
        }
        [HttpPost]
        public async Task<IActionResult> SaveDoc([FromForm] DocumentAttachmentViewModel model, IFormFile formFile)
        {
            try
            {
                if (formFile != null)
                {
                    string userName = User.Identity.Name;
                    string documentType = "Document";
                    var filesPath = $"{_hostingEnvironment.WebRootPath}/Upload/Photo";
                    var fileName = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName;
                    string fileType = Path.GetExtension(fileName);
                    fileName = fileName.Contains("\\")
                        ? fileName.Trim('"').Substring(fileName.LastIndexOf("\\", StringComparison.Ordinal) + 1)
                        : fileName.Trim('"');

                    string NewFileName = model.actionTypeId + "_" + documentType + "_" + fileName;
                    string fileLocation = "/Upload/Photo/" + NewFileName;
                    var fullFilePath = Path.Combine(filesPath, NewFileName);

                    using (var stream = new FileStream(fullFilePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    DocumentPhotoAttachment data = new DocumentPhotoAttachment
                    {
                        Id = Convert.ToInt32(model.documentId),
                        actionType = model.actionType,
                        actionTypeId = model.actionTypeId,
                        subject = model.subject,
                        fileName = NewFileName,
                        filePath = fileLocation,
                        fileType = fileType,
                        description = model.description,
                        documentType = documentType,
                        moduleId = 4
                    };
                    await attachmentCommentService.SaveDocumentAttachment(data);
                }

                return RedirectToAction("ReceiptDetails", "ReceiptVoucher", new { id = model.actionTypeId, Area = "Accounting" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> SavePhoto([FromForm] DocumentAttachmentViewModel model, IFormFile formFile)
        {
            try
            {
                if (formFile != null)
                {
                    string userName = User.Identity.Name;
                    string documentType = "photo";
                    var filesPath = $"{_hostingEnvironment.WebRootPath}/Upload/Photo";
                    var fileName = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName;
                    string fileType = Path.GetExtension(fileName);
                    fileName = fileName.Contains("\\")
                        ? fileName.Trim('"').Substring(fileName.LastIndexOf("\\", StringComparison.Ordinal) + 1)
                        : fileName.Trim('"');

                    string NewFileName = model.actionTypeId + "_" + documentType + "_" + fileName;
                    string fileLocation = "/Upload/Photo/" + NewFileName;
                    var fullFilePath = Path.Combine(filesPath, NewFileName);

                    using (var stream = new FileStream(fullFilePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    DocumentPhotoAttachment data = new DocumentPhotoAttachment
                    {
                        Id = Convert.ToInt32(model.documentId),
                        actionType = model.actionType,
                        actionTypeId = model.actionTypeId,
                        subject = model.subject,
                        fileName = NewFileName,
                        filePath = fileLocation,
                        fileType = fileType,
                        description = model.description,
                        documentType = documentType,
                        moduleId = 4
                    };
                    await attachmentCommentService.SaveDocumentAttachment(data);
                }

                return RedirectToAction("ReceiptDetails", "ReceiptVoucher", new { id = model.actionTypeId, Area = "Accounting" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeletePhoto(int actionId, int photoId)
        {
            var data = await attachmentCommentService.GetDocumentAttachmentById(photoId);
            var filesPath = $"{_hostingEnvironment.WebRootPath}/Upload/Photo";

            var fullFilePath = Path.Combine(filesPath, data.fileName);
            if (System.IO.File.Exists(fullFilePath))
                System.IO.File.Delete(fullFilePath);
            await attachmentCommentService.DeleteDocumentAttachmentById(photoId);
            return RedirectToAction("ReceiptDetails", "ReceiptVoucher", new { id = actionId, Area = "Accounting" });
        }
    }
}