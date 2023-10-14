using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.Payroll.Models;
using OPUSERP.Data.Entity;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.Payroll.Data.Entity.Fixation;
using OPUSERP.Payroll.Services.Fixation.Interfaces;
using OPUSERP.Payroll.Services.Salary.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.Payroll.Controllers
{
    [Area("HR")]
    [Authorize]
    public class FixationController : Controller
    {
        private readonly ISalaryService salaryService;
        private readonly IERPCompanyService eRPCompanyService;
        private readonly IFixationService fixationService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly string rootPath;
        private readonly MyPDF myPDF;
        private readonly UserManager<ApplicationUser> _userManager;
        public FixationController(IHostingEnvironment hostingEnvironment, IPersonalInfoService personalInfoService, UserManager<ApplicationUser> _userManager, IConverter converter, ISalaryService salaryService, IERPCompanyService eRPCompanyService, IFixationService fixationService)
        {
            this.salaryService = salaryService;
            this.eRPCompanyService = eRPCompanyService;
            this.fixationService = fixationService;
            this._userManager = _userManager;
            this.personalInfoService = personalInfoService;

            myPDF = new MyPDF(hostingEnvironment, converter);
            rootPath = hostingEnvironment.ContentRootPath;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            FixationIntegrationViewModel model = new FixationIntegrationViewModel
            {
                fixationIntegrationList = await fixationService.GetAllFixationIntegration(),
                salaryGrades = await fixationService.GetSalarygrade(),
                fixaTypes = await fixationService.GetAllFixType()

            };
            if (model.fixationIntegration == null) model.fixationIntegration = new FixationIntegration();
            return View(model);
        }

        public async Task<IActionResult> GetNewIncrementFixationByEmpCode(string code)
        {
            var data = await personalInfoService.GetEmployeeInfoByCode(code);
            //var nextBasic = await fixationService.GetNextBasicByGradeAndAmount(data.currentGradeId, data.currentBasic);
            return Json("ok");
        }
        [HttpGet]
        public async Task<IActionResult> CreateIncrement()
        {
            FixationIntegrationViewModel model = new FixationIntegrationViewModel
            {
                fixationIntegrationList = await fixationService.GetAllFixationIntegration(),
                salaryGrades = await fixationService.GetSalarygrade(),
            };
            if (model.fixationIntegration == null) model.fixationIntegration = new FixationIntegration();
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> CreatePromotion()
        {
            FixationIntegrationViewModel model = new FixationIntegrationViewModel
            {
                fixationIntegrationList = await fixationService.GetAllFixationIntegration(),
                salaryGrades = await fixationService.GetSalarygrade(),
                designations = await personalInfoService.GetAllDesignation()
            };
            if (model.fixationIntegration == null) model.fixationIntegration = new FixationIntegration();
            return View(model);
        }
        public async Task<IActionResult> CreateSelf()
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            FixationIntegrationViewModel model = new FixationIntegrationViewModel
            {
                salaryGrades = await fixationService.GetSalarygrade(),
                employee = await personalInfoService.GetEmployeeInfoByCode(user.EmpCode),
                fixationIntegration = await fixationService.GetLastFixationIntegrationByEmpCode(user.EmpCode)
            };
            if (model.fixationIntegration == null) model.fixationIntegration = new FixationIntegration();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] FixationIntegrationViewModel model)
        {
            var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();

            if (!ModelState.IsValid)
            {

                model.fixationIntegrationList = await fixationService.GetAllFixationIntegration();
                return View(model);
            }

            var signatory = await fixationService.GetSignatoryByDesignationId((int)model.lastDesignationId);


			FixationIntegration data = new FixationIntegration
            {
                Id = model.fixationintegrationId,
                employeeId = model.employeeId,
                empCode = model.empCode,
                brachCode = model.hrBranchId,
                designation = model.designation,
                lastDesignationId = model.lastDesignationId,
                joiningDate = model.joiningDate,
                dateOfBirth = model.dateOfBirth,
                fixationType = model.fixationType,
                fixationGradeId = Int32.Parse(model.fixationGrade),
                currentGrade = model.currentGrade,
                amount = model.amount,
                refNo = model.refNo,
                postingPlace = model.postingPlace,
                letterDate = model.letterDate,
                fileNo = model.fileNo,
                effectiveDate = model.effectiveDate,
                lastIncrementDate = model.lastIncrementDate,
                lastPromotionDate = model.lastPromotionDate,
                lastPromotionDatePrev = model.lastPromotionDate,
                nextIncrementDate = model.nextIncrementDate,
                status = 1,
                categoryId=model.categoryId,
                newSignatory = signatory?.EmployeeInfo?.employeeCode,
                newSignatoryName = signatory?.EmployeeInfo?.nameBangla,
                newSignatoryDesig = signatory?.Designation?.designationNameBN,
                newSignatoryPhone = signatory?.EmployeeInfo?.telephoneOfficeBn

			};

            await fixationService.SaveFixationIntegration(data);

            //var employee = await personalInfoService.GetEmployeeInfoByCode(model.empCode);
            //employee.currentGradeId = Int32.Parse(model.fixationGrade);
            //employee.currentBasic = model.amount;
            //await personalInfoService.SaveEmployeeInfo(employee);

            if (model.fixationType == "Promotion")
            {
                return RedirectToAction("AllPendingPromotion");
            }
            else
            {
                return RedirectToAction("FixationIntegrationList");
            }
        }

        public async Task<IActionResult> AllPendingPromotion()
        {
            var model = new FixationIntegrationViewModel
            {
                fixationIntegrationList = await fixationService.GetAllFixationIntegration(),
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> FinallyPromotion(FixationIntegrationViewModel model)
        {
            for (int i = 0; i < model.chkFixId.Length; i++)
            {
                await fixationService.UpdatePromotionFinally(Convert.ToInt32(model.chkFixId[i]));
                await fixationService.UpdateEmpSalaryStructureByFixationId(Convert.ToInt32(model.chkFixId[i]));
            }
            return RedirectToAction("AllPendingPromotion");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromForm] FixationIntegrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                //go on as normal
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                                       .Where(y => y.Count > 0)
                                       .ToList();
            }
            if (!ModelState.IsValid)
            {
                model.fixationIntegrationList = await fixationService.GetAllFixationIntegration();
                return RedirectToAction("FixationIntegrationList");
            }

            FixationIntegration data = new FixationIntegration
            {
                Id = model.fixationintegrationId,
                employeeId = model.employeeId,
                empCode = model.empCode,
                brachCode = model.hrBranchId,
                designation = model.designation,
                joiningDate = model.joiningDate,
                dateOfBirth = model.dateOfBirth,
                basicPay = model.NbasicPay,
                fixationType = model.fixationType,
                fixationGradeId = Int32.Parse(model.fixationGrade),
                currentGrade = model.currentGrade,
                amount = model.amount,
                refNo = model.refNo,
                //initialBasic = model.initialBasic,
                postingPlace = model.postingPlace,
                //startingBasic = model.startingBasic,
                letterDate = model.letterDate,
                fileNo = model.fileNo,
                //newBasicPoint = model.newBasicPoint,
                effectiveDate = model.effectiveDate,
                lastIncrementDate = model.lastIncrementDate,
                lastPromotionDate = model.lastPromotionDate,
                nextIncrementDate = model.nextIncrementDate,
                status = model.statusId,
                lastDesignationId=model.lastDesignationId
            };

            await fixationService.SaveFixationIntegration(data);
            return RedirectToAction("FixationIntegrationList");
        }


        public async Task<IActionResult> FixationIntegrationList()
        {

            FixationIntegrationViewModel model = new FixationIntegrationViewModel
            {
                fixationIntegrationList = await fixationService.GetAllFixationIntegration(),
                salaryGrades = await fixationService.GetSalarygrade(),
                designations = await personalInfoService.GetAllDesignation(),
                functionInfos = await personalInfoService.GetAllOffices(),

                zoneLocation = await personalInfoService.GetAllZone(),
                hrBranches = await personalInfoService.GetAllBranch(),
                departments = await personalInfoService.GetAllDepartment()
            };
            if (model.fixationIntegration == null) model.fixationIntegration = new FixationIntegration();
            return View(model);
        }

        //promotion 10 years more
        [AllowAnonymous]
        public async Task<IActionResult> PromotionReportViewTenYearsMore(int id)
        {
            FixationViewModel model = new FixationViewModel
            {
                fixationDetails = await personalInfoService.GetFixationByFixId(id),
                empPostingPlace = await fixationService.GetPostingPlaceByFixId(id)
            };

            return View(model);
        }

        //Promotion five year more
        [AllowAnonymous]
        public async Task<IActionResult> PromotionReportViewFiveYearsMore(int id)
        {
            FixationViewModel model = new FixationViewModel
            {
                fixationDetails = await personalInfoService.GetFixationByFixId(id),
                empPostingPlace = await fixationService.GetPostingPlaceByFixId(id)
            };

            return View(model);
        }

        //Promotion 12 years more
        [AllowAnonymous]
        public async Task<IActionResult> PromotionReportViewTwelveYearsMore(int id)
        {
            FixationViewModel model = new FixationViewModel
            {
                fixationDetails = await personalInfoService.GetFixationByFixId(id),
                empPostingPlace = await fixationService.GetPostingPlaceByFixId(id)
            };

            return View(model);
        }
        //Promotion 14 years more
        [AllowAnonymous]
        public async Task<IActionResult> PromotionReportViewFourteenYearsMore(int id)
        {
            FixationViewModel model = new FixationViewModel
            {
                fixationDetails = await personalInfoService.GetFixationByFixId(id),
                empPostingPlace = await fixationService.GetPostingPlaceByFixId(id)
            };

            return View(model);
        }

        //Promotion 17 years more
          [AllowAnonymous]
        public async Task<IActionResult> PromotionReportViewSeventeenYearsMore(int id)
        {
            FixationViewModel model = new FixationViewModel
            {
                fixationDetails = await personalInfoService.GetFixationByFixId(id),
                empPostingPlace = await fixationService.GetPostingPlaceByFixId(id)
            };

            return View(model);
        }

         //Promotion 17 years more
          [AllowAnonymous]
        public async Task<IActionResult> PromotionReportViewTwentyYearsMore(int id)
        {
            FixationViewModel model = new FixationViewModel
            {
                fixationDetails = await personalInfoService.GetFixationByFixId(id),
                empPostingPlace = await fixationService.GetPostingPlaceByFixId(id)
            };

            return View(model);
        }




        [AllowAnonymous]
        public async Task<IActionResult> PromotionReportViewNew(int id)
        {
            FixationViewModel model = new FixationViewModel
            {
                fixationDetails = await personalInfoService.GetFixationByFixId(id),
                empPostingPlace = await fixationService.GetPostingPlaceByFixId(id)
            };

            return View(model);
        }


        [AllowAnonymous]
        public async Task<IActionResult> PromotionReportView(int id)
        {
            var fixation = await fixationService.GetAllFixationIntegrationByIdNew(id);

            FixationViewModel model = new FixationViewModel
            {
                newbasicAmount = await personalInfoService.GetFixAmountByEmpCode(fixation.empCode),
                AllSlab = await fixationService.GetAllSalarySlabNameByGradeId(Convert.ToInt32(fixation.fixationGradeId)),
                companies = await eRPCompanyService.GetAllCompany(),
                fixationIntegration = fixation,
                employeePostingPlaces = await fixationService.GetPostingPlaceByFixationId(id)
            };

            return View(model);
        }
        [AllowAnonymous]
        public async Task<IActionResult> PromotionReportPdf(int id,int? categoryId)
        {

            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = scheme + "://" + host + "/Payroll/Fixation/PromotionReportViewNew?id=" + id;

            var fixationDetails = await personalInfoService.GetFixationByFixId(id);

			if(fixationDetails?.newSignatory != null)
            {

            }
            else
            {

            }



			if (fixationDetails.categoryId == 1 || fixationDetails.categoryId == 2)
            {
                url = scheme + "://" + host + "/Payroll/Fixation/PromotionReportViewFiveYearsMore?id=" + id;
            }
            else if (fixationDetails.categoryId == 3 || fixationDetails.categoryId == 4)
            {
                url = scheme + "://" + host + "/Payroll/Fixation/PromotionReportViewTenYearsMore?id=" + id;
            }
            else if (fixationDetails.categoryId == 5 || fixationDetails.categoryId == 6)
            {
                url = scheme + "://" + host + "/Payroll/Fixation/PromotionReportViewTwelveYearsMore?id=" + id;
            }
            else if (fixationDetails.categoryId == 7 || fixationDetails.categoryId == 8)
            {
                url = scheme + "://" + host + "/Payroll/Fixation/PromotionReportViewFourteenYearsMore?id=" + id;
            }
            else if (fixationDetails.categoryId == 9 || fixationDetails.categoryId == 10)
            {
                url = scheme + "://" + host + "/Payroll/Fixation/PromotionReportViewSeventeenYearsMore?id=" + id;
            }
            else if (fixationDetails.categoryId == 11 || fixationDetails.categoryId == 12)
            {
                url = scheme + "://" + host + "/Payroll/Fixation/PromotionReportViewTwentyYearsMore?id=" + id;
            }
            else
            {
                url = scheme + "://" + host + "/Payroll/Fixation/PromotionReportViewNew?id=" + id;
            };

            string fileName;
            string status = myPDF.GeneratePDFWithOutPrintInfo(out fileName, url);
            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }
      
        #region PreView



        [AllowAnonymous]
        public  IActionResult PromotionReportPreViewByYearsPdf(int empId,string empCode,int designId, DateTime increDate, DateTime effectDate, int fixationGrade, string fixationType, decimal amount, string fileN, string refNo, DateTime letterDate, DateTime nextIncrementDate, int categoryId, DateTime promotionDate)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";

            if (categoryId==1 || categoryId == 2)
            {
                url = scheme + "://" + host + "/Payroll/Fixation/PromotionReportPreViewBy5Years?empId=" + empId + "&empCode=" + empCode + "&designId=" + designId + "&increDate=" + increDate + "&effectDate=" + effectDate + "&fixationGrade=" + fixationGrade + "&fixationType=" + fixationType + "&amount=" + amount + "&fileN=" + fileN + "&refNo=" + refNo + "&letterDate=" + letterDate + "&nextIncrementDate=" + nextIncrementDate + "&categoryId=" + categoryId + "&promotionDate=" + promotionDate;
            }
            else if (categoryId == 3 || categoryId == 4)
            {
                url = scheme + "://" + host + "/Payroll/Fixation/PromotionReportPreViewBy10Years?empId=" + empId + "&empCode=" + empCode + "&designId=" + designId + "&increDate=" + increDate + "&effectDate=" + effectDate + "&fixationGrade=" + fixationGrade + "&fixationType=" + fixationType + "&amount=" + amount + "&fileN=" + fileN + "&refNo=" + refNo + "&letterDate=" + letterDate + "&nextIncrementDate=" + nextIncrementDate + "&categoryId=" + categoryId + "&promotionDate=" + promotionDate;
            }
            else if (categoryId == 5|| categoryId == 6)
            {
                url = scheme + "://" + host + "/Payroll/Fixation/PromotionReportPreViewBy12Years?empId=" + empId + "&empCode=" + empCode + "&designId=" + designId + "&increDate=" + increDate + "&effectDate=" + effectDate + "&fixationGrade=" + fixationGrade + "&fixationType=" + fixationType + "&amount=" + amount + "&fileN=" + fileN + "&refNo=" + refNo + "&letterDate=" + letterDate + "&nextIncrementDate=" + nextIncrementDate + "&categoryId=" + categoryId + "&promotionDate=" + promotionDate;
            }
            else if (categoryId ==7 || categoryId == 8)
            {
                url = scheme + "://" + host + "/Payroll/Fixation/PromotionReportPreViewBy14Years?empId=" + empId + "&empCode=" + empCode + "&designId=" + designId + "&increDate=" + increDate + "&effectDate=" + effectDate + "&fixationGrade=" + fixationGrade + "&fixationType=" + fixationType + "&amount=" + amount + "&fileN=" + fileN + "&refNo=" + refNo + "&letterDate=" + letterDate + "&nextIncrementDate=" + nextIncrementDate + "&categoryId=" + categoryId + "&promotionDate=" + promotionDate;
            }
             else if (categoryId == 9 || categoryId == 10)
            {
                url = scheme + "://" + host + "/Payroll/Fixation/PromotionReportPreViewBy17Years?empId=" + empId + "&empCode=" + empCode + "&designId=" + designId + "&increDate=" + increDate + "&effectDate=" + effectDate + "&fixationGrade=" + fixationGrade + "&fixationType=" + fixationType + "&amount=" + amount + "&fileN=" + fileN + "&refNo=" + refNo + "&letterDate=" + letterDate + "&nextIncrementDate=" + nextIncrementDate + "&categoryId=" + categoryId + "&promotionDate=" + promotionDate;
            }
              else if (categoryId ==11 || categoryId == 12)
            {
                url = scheme + "://" + host + "/Payroll/Fixation/PromotionReportPreViewBy20Years?empId=" + empId + "&empCode=" + empCode + "&designId=" + designId + "&increDate=" + increDate + "&effectDate=" + effectDate + "&fixationGrade=" + fixationGrade + "&fixationType=" + fixationType + "&amount=" + amount + "&fileN=" + fileN + "&refNo=" + refNo + "&letterDate=" + letterDate + "&nextIncrementDate=" + nextIncrementDate + "&categoryId=" + categoryId + "&promotionDate=" + promotionDate;
            }
            else
            {
                url = scheme + "://" + host + "/Payroll/Fixation/PromotionReportPreView?empId=" + empId + "&empCode=" + empCode + "&designId=" + designId + "&increDate=" + increDate + "&effectDate=" + effectDate + "&fixationGrade=" + fixationGrade + "&fixationType=" + fixationType + "&amount=" + amount + "&fileN=" + fileN + "&refNo=" + refNo + "&letterDate=" + letterDate + "&nextIncrementDate=" + nextIncrementDate + "&categoryId=" + categoryId + "&promotionDate=" + promotionDate;
            };

            string fileName;
            string status = myPDF.GeneratePDFWithOutPrintInfo(out fileName, url);
            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        public IActionResult PromotionReportPreViewPdf(int empId, string empCode,int designId, DateTime increDate, DateTime effectDate, int fixationGrade, string fixationType, decimal amount, string fileN, string refNo, DateTime letterDate, DateTime nextIncrementDate,  DateTime promotionDate)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";
            url = scheme + "://" + host + "/Payroll/Fixation/PromotionReportPreView?empId=" + empId  + "&empCode=" + empCode + "&designId=" + designId + "&increDate=" + increDate + "&effectDate=" + effectDate + "&fixationGrade=" + fixationGrade + "&fixationType=" + fixationType + "&amount=" + amount + "&fileN=" + fileN + "&refNo=" + refNo + "&letterDate=" + letterDate + "&nextIncrementDate=" + nextIncrementDate +  "&promotionDate=" + promotionDate;
            string fileName;
            string status = myPDF.GeneratePDFWithOutPrintInfo(out fileName, url);
            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }


        [AllowAnonymous]
        public async Task<IActionResult> PromotionReportPreView(int empId, string empCode,int designId, DateTime increDate, DateTime effectDate, int fixationGrade, string fixationType, decimal amount, string fileN, string refNo, DateTime letterDate, DateTime nextIncrementDate,  DateTime promotionDate)
        {

            var designationName = await personalInfoService.GetDesignationNameBNBydesigId(designId);

            FixationPreviewViewModel model = new FixationPreviewViewModel
            {
                fixationDetails = await personalInfoService.GetFixationByEmpId(empId, fixationGrade),
                empPostingPlace = await fixationService.GetPostingPlaceByEmpId(empId),
                empCode = empCode,
                //lastDesignationId=desigId,
                designationNameBN= designationName,
                //joiningDate=joinDate,
                effectiveDate = effectDate,
                letterDate = letterDate,
                amount = amount,
                fileNo = fileN,
                refNo = refNo
            };
            return View(model);
        }





        [AllowAnonymous]
        public async Task<IActionResult> PromotionReportPreViewBy5Years(int empId, string empCode,int designId, DateTime increDate, DateTime effectDate,int fixationGrade, string fixationType, decimal amount, string fileN, string refNo, DateTime letterDate, DateTime nextIncrementDate, int categoryId, DateTime promotionDate)
        {
            var designationName = await personalInfoService.GetDesignationNameBNBydesigId(designId);
            FixationPreviewViewModel model = new FixationPreviewViewModel
            {
                fixationDetails = await personalInfoService.GetFixationByEmpId(empId, fixationGrade),
                empPostingPlace = await fixationService.GetPostingPlaceByEmpId(empId),
                empCode = empCode,
                //lastDesignationId=desigId,
                designationNameBN = designationName,
                //joiningDate=joinDate,
                effectiveDate = effectDate,
                letterDate = letterDate,
                amount = amount,
                fileNo = fileN,
                refNo = refNo,
                categoryId = categoryId,

            };

            return View(model);
        }

         [AllowAnonymous]
        public async Task<IActionResult> PromotionReportPreViewBy10Years(int empId, string empCode, int designId, DateTime increDate, DateTime effectDate, int fixationGrade, string fixationType, decimal amount, string fileN, string refNo, DateTime letterDate, DateTime nextIncrementDate, int categoryId, DateTime promotionDate)
        {
            var designationName = await personalInfoService.GetDesignationNameBNBydesigId(designId);
            FixationPreviewViewModel model = new FixationPreviewViewModel
            {
                fixationDetails = await personalInfoService.GetFixationByEmpId(empId, fixationGrade),
                empPostingPlace = await fixationService.GetPostingPlaceByEmpId(empId),
                empCode = empCode,
                //lastDesignationId=desigId,
                designationNameBN = designationName,
                //joiningDate=joinDate,
                effectiveDate = effectDate,
                letterDate = letterDate,
                amount = amount,
                fileNo = fileN,
                refNo=refNo,
                categoryId = categoryId,
                lastPromotionDate=promotionDate

            };

            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> PromotionReportPreViewBy12Years(int empId, string empCode,int designId, DateTime increDate, DateTime effectDate,int fixationGrade, string fixationType, decimal amount, string fileN, string refNo, DateTime letterDate, DateTime nextIncrementDate, int categoryId, DateTime promotionDate)
        {
            var designationName = await personalInfoService.GetDesignationNameBNBydesigId(designId);
            FixationPreviewViewModel model = new FixationPreviewViewModel
            {
                fixationDetails = await personalInfoService.GetFixationByEmpId(empId, fixationGrade),
                empPostingPlace = await fixationService.GetPostingPlaceByEmpId(empId),
                empCode = empCode,
                //lastDesignationId=desigId,
                designationNameBN = designationName,
                //joiningDate=joinDate,
                effectiveDate = effectDate,
                letterDate = letterDate,
                amount = amount,
                fileNo = fileN,
                refNo = refNo,
                categoryId = categoryId,

            };

            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> PromotionReportPreViewBy14Years(int empId, string empCode,int designId, DateTime increDate, DateTime effectDate, int fixationGrade, string fixationType, decimal amount, string fileN, string refNo, DateTime letterDate, DateTime nextIncrementDate, int categoryId, DateTime promotionDate)
        {
            var designationName = await personalInfoService.GetDesignationNameBNBydesigId(designId);

            FixationPreviewViewModel model = new FixationPreviewViewModel
            {
                fixationDetails = await personalInfoService.GetFixationByEmpId(empId, fixationGrade),
                empPostingPlace = await fixationService.GetPostingPlaceByEmpId(empId),
                empCode = empCode,
                //lastDesignationId=desigId,
                designationNameBN = designationName,
                //joiningDate=joinDate,
                effectiveDate = effectDate,
                letterDate = letterDate,
                amount = amount,
                fileNo = fileN,
                refNo = refNo,
                categoryId = categoryId,

            };

            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> PromotionReportPreViewBy17Years(int empId, string empCode, int designId, DateTime increDate, DateTime effectDate,int fixationGrade, string fixationType, decimal amount, string fileN, string refNo, DateTime letterDate, DateTime nextIncrementDate, int categoryId, DateTime promotionDate)
        {
            var designationName = await personalInfoService.GetDesignationNameBNBydesigId(designId);

            FixationPreviewViewModel model = new FixationPreviewViewModel
            {
                fixationDetails = await personalInfoService.GetFixationByEmpId(empId, fixationGrade),
                empPostingPlace = await fixationService.GetPostingPlaceByEmpId(empId),
                empCode = empCode,
                //lastDesignationId=desigId,
                designationNameBN = designationName,
                //joiningDate=joinDate,
                effectiveDate = effectDate,
                letterDate = letterDate,
                amount = amount,
                fileNo = fileN,
                refNo = refNo,
                categoryId = categoryId,

            };

            return View(model);
        }
        [AllowAnonymous]
        public async Task<IActionResult> PromotionReportPreViewBy20Years(int empId, string empCode, int designId, DateTime increDate, DateTime effectDate,int fixationGrade, string fixationType, decimal amount, string fileN, string refNo, DateTime letterDate, DateTime nextIncrementDate, int categoryId, DateTime promotionDate)
        {
            var designationName = await personalInfoService.GetDesignationNameBNBydesigId(designId);

            FixationPreviewViewModel model = new FixationPreviewViewModel
            {
                fixationDetails = await personalInfoService.GetFixationByEmpId(empId, fixationGrade),
                empPostingPlace = await fixationService.GetPostingPlaceByEmpId(empId),
                empCode = empCode,
                //lastDesignationId=desigId,
                designationNameBN = designationName,
                //joiningDate=joinDate,
                effectiveDate = effectDate,
                letterDate = letterDate,
                amount = amount,
                fileNo = fileN,
                refNo = refNo,
                categoryId = categoryId,

            };

            return View(model);
        }


       

        #endregion





        public IActionResult FixationReports()
        {

            return View();
        }

        public IActionResult ProcessFixationIncrement()
        {
            var model = new FixationViewModel
            {

            };
            return View(model);
        }

		//New: Added by Asad on 26.07.2023
		public async Task<IActionResult> CreateIncrementProposalApi(int year, int employeeInfoId)
		{
			var data = await fixationService.CreateIncrementProposal(year, employeeInfoId);

			return Json(Convert.ToInt32(data.status));
		}

		//Old: 26.07.2023
		public async Task<IActionResult> ProcessIncrementApi(int year, int employeeInfoId)
        {
            var data = await fixationService.ProcessIncrement(year, employeeInfoId);

            return Json(Convert.ToInt32(data.status));
        }

		[AllowAnonymous]
		public async Task<IActionResult> FixationYearlyreportView(int year, string type)
        {
            var model = new FixationViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                yearlyFixations = await fixationService.GetYearlyFixation(year, type),
                fixationSummaryViewModels = await fixationService.GetFixationSummaryByYearAndType(year, type)
            };

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult FixationYearlyreportPdf(int year, string type)
        {

            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/Payroll/Fixation/FixationYearlyreportView?year=" + year + "&type=" + type;

            //New
            string fileName;
            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            //Old
            //string fileName;
            //string status = myPDF.GenerateLandscapePDFCustom2(out fileName, url, "Increment_" + DateTime.Now.Year + "_");
            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }

        [AllowAnonymous]
        public async Task<IActionResult> FixationYearlyreportStaffView(int year, string type)
        {
            var model = new FixationViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                yearlyFixations = await fixationService.GetYearlyFixation(year, type),
                fixationSummaryViewModels = await fixationService.GetFixationSummaryStaffByYearAndType(year, type)
            };

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult FixationYearlyreportStaffPdf(int year, string type)
        {
            
            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/Payroll/Fixation/FixationYearlyreportStaffView?year=" + year + "&type=" + type;

            string fileName;
            string status = myPDF.GenerateLandscapePDF(out fileName, url);
            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }

		[AllowAnonymous]
		public async Task<ActionResult> IncrementReportViewMail(int id)
		{
			int gradeId = await fixationService.GetSalarygradeByFixId(id);
			int code = await fixationService.GetEmplCodeByFixId(id);
			var empBranch = await personalInfoService.GetEmpBranchByempCode(Convert.ToString(code));
			ViewBag.branchName = empBranch;
			FixationViewModel model = new FixationViewModel
			{
				newbasicAmount = await personalInfoService.GetFixAmountByEmpCode(Convert.ToString(code)),
				AllSlab = await fixationService.GetAllSalarySlabNameByGradeId(gradeId),

				companies = await eRPCompanyService.GetAllCompany(),
				fixationIntegration = await fixationService.GetAllFixationIntegrationByIdNew(id)
			};
			return View(model);
		}

		[AllowAnonymous]
        public async Task<ActionResult> IncrementReportView(int id)
        {
            int gradeId = await fixationService.GetSalarygradeByFixId(id);
            int code = await fixationService.GetEmplCodeByFixId(id);
            var empBranch = await personalInfoService.GetEmpBranchByempCode(Convert.ToString(code));
            ViewBag.branchName = empBranch;
            FixationViewModel model = new FixationViewModel
            {
                newbasicAmount = await personalInfoService.GetFixAmountByEmpCode(Convert.ToString(code)),
                AllSlab = await fixationService.GetAllSalarySlabNameByGradeId(gradeId),

                companies = await eRPCompanyService.GetAllCompany(),
                fixationIntegration = await fixationService.GetAllFixationIntegrationByIdNew(id)
            };
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult IncrementReportPdf(int id)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;

            string url = scheme + "://" + host + "/Payroll/Fixation/IncrementReportView?id=" + id;
            string status = myPDF.GeneratePDFForIncrementReport(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [AllowAnonymous]
        public async Task<ActionResult> IncrementReportViewAll(int year)
        {
            int gradeId, code = 0;
            var empBranch = "";
            var data = new List<FixationAllViewModel>();

            var fixations = await fixationService.GetAllFixationIntegrationByYear(year);

            foreach (var item in fixations)
            {
                gradeId = await fixationService.GetSalarygradeByFixId(item.Id);
                code = await fixationService.GetEmplCodeByFixId(item.Id);
                empBranch = await personalInfoService.GetEmpBranchByempCode(Convert.ToString(code));

                data.Add(new FixationAllViewModel
                {
                    fixation = item,
                    branchName = empBranch,
                    salarySlabs = await fixationService.GetAllSalarySlabNameByGradeId(gradeId)
                });
            };

            var model = new FixationViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                fixationAllViewModels = data
            };
            return View(model);
        }

        public async Task<IActionResult> GetFixationByFilter(int year, string code, int designationId, int zoneId, int branchId, int officeId, int departId, string fixationType)
        {
            FixationIntegrationViewModel model = new FixationIntegrationViewModel
            {
                salaryGrades = await fixationService.GetSalarygrade(),
                fixationLists = await fixationService.FilterFixationIntegration(year, code, designationId, zoneId, branchId, officeId, departId, fixationType)
                //fixationIntegrationList = await fixationService.FilterFixationIntegration(year, code, designationId, officeId),
            };
            return Json(model);
        }

        public async Task<IActionResult> GetFixationById(int id)
        {
            var data = await fixationService.GetAllFixationIntegrationByIdNew(id);
            return Json(data);
        }

        public async Task<IActionResult> GetFixationByEmpCode(string code)
        {
            FixationIntegrationViewModel model = new FixationIntegrationViewModel
            {
                salaryGrades = await fixationService.GetSalarygrade(),

                fixationIntegrationList = await fixationService.GetFixationIntegrationByEmpCode(code),
            };
            return Json(model);
        }
        public async Task<IActionResult> GetPromotionSalaryByEmpCodeAndType(string code, string type)
        {
            var data = new FixationCurrentSalaryViewModel();
            if (type == "Promotion")
            {

            }
            else if (type == "Increment")
            {

            }
            else
            {

            }
            return Json(data);
        }
        public async Task<IActionResult> GetSalaryAndGradeByEmpCode(string code)
        {
            var emp = await personalInfoService.GetEmployeeInfoByCode(code);
            var grade = await fixationService.GetSalarygradeByEmpId(code);
            var jobDuration = (DateTime.Now.Date - Convert.ToDateTime(emp.joiningDateGovtService).Date).TotalDays / 365;
            var data = new SalaryGradeJoiningDateViewModel
            {
                employee = emp,
                gradeId = Convert.ToInt32(grade),
                jobDuration = Convert.ToDecimal(jobDuration)
            };
            return Json(data);
        }
        public async Task<IActionResult> GetDisciplinaryInfobyEmp(string code)
        {
            //int code = await fixationService.GetEmplCodeByFixId(id);

            return Json(await fixationService.GetDisciplinaryIdbyEmpcode(code));
        }

        public async Task<IActionResult> GetNewIncrementSalaryAndGradeByEmpCode1(string code)
        {

            var emp = await personalInfoService.GetEmployeeInfoByCode(code);
            var gradeId = await fixationService.GetSalarygradeIdByEmpCode(code);
            var nGradeBasic = await personalInfoService.GetNewPromotionAmountByEmpCode(code);
            var nGradeNm = await personalInfoService.GetNewPromotionGradeNameByEmpCode(code);
            var jobDuration = (DateTime.Now.Date - Convert.ToDateTime(emp.joiningDateGovtService).Date).TotalDays / 365;
            var data = new SalaryGradeJoiningDateViewModel
            {
                employee = emp,
                gradeBasic = Convert.ToInt32(nGradeBasic),
                gradeId = Convert.ToInt32(gradeId),
                ngradeName = nGradeNm,
                jobDuration = Convert.ToDecimal(jobDuration)
            };
            return Json(data);
        }

        public async Task<IActionResult> GetNewIncrementSalaryAndGradeByEmpCode(string code, string type)
        {
            decimal data = 0;
            if (type == "increment")
            {
                data = await personalInfoService.GetNewIntegrationAmountByEmpCode(code);
            }
            else
            {
                data = await personalInfoService.GetNewPromotionAmountByEmpCode(code);
                //var emp = await personalInfoService.GetEmployeeInfoByCode(code);
                ////var grade = await fixationService.GetSalarygradeByFixId(Convert.ToInt32(code));
                //var nGradeBasic = await personalInfoService.GetNewPromotionAmountByEmpCode(code);
                //var jobDuration = (DateTime.Now.Date - Convert.ToDateTime(emp.joiningDateGovtService).Date).TotalDays / 365;
                //var data1 = new SalaryGradeJoiningDateViewModel
                //{
                //    employee = emp,
                //    gradeBasic = Convert.ToInt32(nGradeBasic),
                //    jobDuration = Convert.ToDecimal(jobDuration)
                //};
            }

            return Json(data);
        }


        public async Task<IActionResult> UpdateSignatory(int? fixId, string newFixCode, string NewFixName, string NewFixPhone, string NewFixDesion )
        {
            var data = await fixationService.GetfixationIntegrationInfoById(Convert.ToInt32(fixId));
            data.newSignatory = newFixCode;
            data.newSignatoryName = NewFixName;
            data.newSignatoryPhone = NewFixPhone;
            data.newSignatoryDesig= NewFixDesion;

            var result = await fixationService.SaveFixationIntegration(data);
            if (result >= 1)
            {
                return Json("saved");
            }
            else
            {
                return Json("Failed");
            }
        }


        public async Task<IActionResult> GetNewGradeByEmpCode(string code)
        {
            var data = await fixationService.GetNewGradeByEmpCode(code);
            return Json(data);
        }
        public async Task<IActionResult> GetNewGradeByEmpCodeNew(string code)
        {
            var emp = await personalInfoService.GetEmployeeInfoByCode(code);
            //var grade = await fixationService.GetSalarygradeByFixId(Convert.ToInt32(code));
            var nGradeBasic = await personalInfoService.GetNewPromotionAmountByEmpCode(code);
            var nGradeNm = await personalInfoService.GetNewPromotionGradeNameByEmpCode(code);
            var nGradeId = await personalInfoService.GetNewPromotionGradeIdByEmpCode(code);
            var jobDuration = (DateTime.Now.Date - Convert.ToDateTime(emp.joiningDateGovtService).Date).TotalDays / 365;
            var data = new SalaryGradeJoiningDateViewModel
            {
                employee = emp,
                gradeBasic = Convert.ToInt32(nGradeBasic),
                ngradeName = nGradeNm,
                ngradeId = nGradeId,
                jobDuration = Convert.ToDecimal(jobDuration)
            };
            return Json(data);
        }


        [HttpGet]
        public async Task<IActionResult> fixationStructure()
        {

            FixationIntegrationViewModel model = new FixationIntegrationViewModel
            {
                fixationIntegrationList = await fixationService.GetAllFixationIntegration(),
                salaryGrades = await fixationService.GetSalarygrade(),
            };
            if (model.fixationIntegration == null) model.fixationIntegration = new FixationIntegration();
            return View(model);
        }

        #region Re-Fixation
        [HttpGet]
        public async Task<IActionResult> Refixation(string code)
        {
            FixationIntegrationViewModel model = new FixationIntegrationViewModel
            {
                //fixationIntegrationList = await fixationService.GetAllFixationIntegration(),
                fixationIntegration = await fixationService.GetFixationIntegrationByEmpCodeForReFixation(code),
                salaryGrades = await fixationService.GetSalarygrade(),
                fixaTypes = await fixationService.GetAllFixType()
            };
            if (model.fixationIntegration == null) model.fixationIntegration = new FixationIntegration();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveReFixation([FromForm] FixationIntegrationViewModel model)
        {
            var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();

            if (!ModelState.IsValid)
            {

                model.fixationIntegrationList = await fixationService.GetAllFixationIntegration();
                return View(model);
            }

            for (int i = 0; i < model.amountMultiple.Length; i++)
            {
                FixationIntegration data = new FixationIntegration
                {
                    Id = model.fixationintegrationId,
                    employeeId = model.employeeId,
                    empCode = model.empCode,
                    brachCode = model.hrBranchId,
                    designation = model.designation,
                    joiningDate = model.joiningDate,
                    dateOfBirth = model.dateOfBirth,
                    //basicPay=model.basicPay,
                    ////fixationType = model.fixationType,
                    //fixationGradeId = Int32.Parse(model.fixationGrade),
                    // fixationGradeId = model.fixationGradeId,
                    currentGrade = model.currentGrade,
                    //amount = model.amount,
                    refNo = model.refNo,
                    reportHeader = model.reportHeader,
                    //particular=model.particular,
                    //initialBasic = model.initialBasic,
                    postingPlace = model.postingPlace,
                    fixaTypeId = 1,
                    letterDate = model.letterDate,
                    fileNo = model.fileNo,
                    //newBasicPoint = model.newBasicPoint,
                    //effectiveDate = model.effectiveDate,
                    lastIncrementDate = model.lastIncrementDate,
                    lastPromotionDate = model.lastPromotionDate,
                    nextIncrementDate = model.nextIncrementDate,
                    status = 1,
                    amount = model.amountMultiple[i],
                    effectiveDate = model.effectiveDateMultiple[i],
                    particular = model.particularMultiple[i],
                    fixationGradeId = Int32.Parse(model.fixationGradeMultiple[i]),

                };

                await fixationService.SaveFixationIntegration(data);
            }


            var employee = await personalInfoService.GetEmployeeInfoByCode(model.empCode);
            employee.currentGradeId = Int32.Parse(model.fixationGradeMultiple[model.fixationGradeMultiple.Length - 1]);
            employee.currentBasic = model.amountMultiple[model.amountMultiple.Length - 1];
            await personalInfoService.SaveEmployeeInfo(employee);
            return RedirectToAction("FixationIntegrationList");
        }





        [AllowAnonymous]
        public async Task<ActionResult> ReFixationReportView(int id)
        {
            int gradeId = await fixationService.GetSalarygradeByFixId(id);
            int code = await fixationService.GetEmplCodeByFixId(id);
            //var reFixationReportVms = await fixationService.GetParticularInfoByfixId(id);
            FixationViewModel model = new FixationViewModel
            {
                newbasicAmount = await personalInfoService.GetFixAmountByEmpCode(Convert.ToString(code)),
                //slabName= await fixationService.GetAllSalarySlabByGradeId(gradeId),
                AllSlab = await fixationService.GetAllSalarySlabNameByGradeId(gradeId),

                companies = await eRPCompanyService.GetAllCompany(),
                fixationIntegrations = await fixationService.GetAllFixationIntegrationById(id),
                fixationIntegration = await fixationService.GetAllFixationIntegrationByIdNew(id)
            };

            return View(model);
        }


        [AllowAnonymous]
        public IActionResult ReFixationReportPdf(int id)
        {
            string scheme = Request.Scheme;
            var host = Request.Host;
            string fileName;

            string url = scheme + "://" + host + "/Payroll/Fixation/ReFixationReportView?id=" + id;
            string status = myPDF.GeneratePDF1(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        #endregion
        #region punishment
        [HttpGet]
        public async Task<IActionResult> PunishMentRefixation()
        {
            FixationIntegrationViewModel model = new FixationIntegrationViewModel
            {
                fixationIntegrationList = await fixationService.GetAllFixationIntegration(),
                salaryGrades = await fixationService.GetSalarygrade(),
                fixaTypes = await fixationService.GetAllFixType()

            };
            if (model.fixationIntegration == null) model.fixationIntegration = new FixationIntegration();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SavePunishMentRefixation([FromForm] FixationIntegrationViewModel model)
        {
            var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();

            if (!ModelState.IsValid)
            {

                model.fixationIntegrationList = await fixationService.GetAllFixationIntegration();
                return View(model);
            }
            for (int i = 0; i < model.amountMultiple.Length; i++)
            {
                FixationIntegration data = new FixationIntegration
                {
                    Id = model.fixationintegrationId,
                    employeeId = model.employeeId,
                    empCode = model.empCode,
                    brachCode = model.hrBranchId,
                    designation = model.designation,
                    joiningDate = model.joiningDate,
                    dateOfBirth = model.dateOfBirth,
                    //basicPay=model.basicPay,
                    //fixationType = model.fixationType,
                    //fixationGradeId = Int32.Parse(model.fixationGrade),
                    currentGrade = model.currentGrade,
                    //amount = model.amount,
                    refNo = model.refNo,
                    remarks = model.remarks,
                    fixaTypeId = 2,
                    //initialBasic = model.initialBasic,
                    postingPlace = model.postingPlace,
                    //startingBasic = model.startingBasic,
                    letterDate = model.letterDate,
                    fileNo = model.fileNo,
                    //newBasicPoint = model.newBasicPoint,
                    //effectiveDate = model.effectiveDate,
                    lastIncrementDate = model.lastIncrementDate,
                    lastPromotionDate = model.lastPromotionDate,
                    nextIncrementDate = model.nextIncrementDate,
                    punishmentDate = model.punishmentDate,
                    status = 1,
                    amount = model.amountMultiple[i],
                    effectiveDate = model.effectiveDateMultiple[i],
                    particular = model.particularMultiple[i],
                    fixationGradeId = Int32.Parse(model.fixationGradeMultiple[i]),


                };

                await fixationService.SaveFixationIntegration(data);
            }


            var employee = await personalInfoService.GetEmployeeInfoByCode(model.empCode);
            //employee.currentGradeId = Int32.Parse(model.fixationGrade);
            employee.currentGradeId = Int32.Parse(model.fixationGradeMultiple[model.fixationGradeMultiple.Length - 1]);
            employee.currentBasic = model.amountMultiple[model.amountMultiple.Length - 1];
            await personalInfoService.SaveEmployeeInfo(employee);
            return RedirectToAction("punishmentReFixationList");
        }

        public async Task<IActionResult> punishmentReFixationList()
        {

            FixationIntegrationViewModel model = new FixationIntegrationViewModel
            {
                fixationIntegrationLists = await fixationService.GetAllPunishmentReFixationIntegration(),
                salaryGrades = await fixationService.GetSalarygrade(),
            };
            if (model.fixationIntegration == null) model.fixationIntegration = new FixationIntegration();
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> PunishmentReFixationReportView(int id)
        {
            int gradeId = await fixationService.GetSalarygradeByFixId(id);
            int code = await fixationService.GetEmplCodeByFixId(id);
            FixationViewModel model = new FixationViewModel
            {
                newbasicAmount = await personalInfoService.GetFixAmountByEmpCode(Convert.ToString(code)),
                AllSlab = await fixationService.GetAllSalarySlabNameByGradeId(gradeId),
                companies = await eRPCompanyService.GetAllCompany(),
                fixationIntegration = await fixationService.GetPunishmentReFixationInfoById(id),
                fixationIntegrations = await fixationService.GetAllFixForPunishmentById(id),
            };

            return View(model);
        }
        [AllowAnonymous]
        public IActionResult PunishmentReFixationReportPdf(int id)
        {

            string scheme = Request.Scheme;
            var host = Request.Host;

            string url = scheme + "://" + host + "/Payroll/Fixation/PunishmentReFixationReportView?id=" + id;

            string fileName;
            string status = myPDF.GeneratePDF(out fileName, url);
            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }

        #endregion

        #region IncrementHeld
        public async Task<IActionResult> IncrementHeld()
        {
            var model = new IncrementViewModel
            {
                IncrementHelds = await fixationService.GetAllIncrementHeld()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> IncrementHeld(IncrementViewModel model)
        {
            var data = new IncrementHeld
            {
                Id = model.Id,
                createdAt = DateTime.Now,
                employeeId = model.employeeId,
                empCode = model.empCode,
                empName = model.empName,
                expireDate = model.expireDate,
                reason = model.reason,
                orderNo = model.reason
            };

            await fixationService.SaveIncrementHeld(data);

            return RedirectToAction("IncrementHeld");
        }

        public async Task<IActionResult> DeleteIncrementHeld(int id)
        {
            var data = await fixationService.DeleteIncrementHeld(id);
            return Json("ok");
        }
        #endregion
    }
}