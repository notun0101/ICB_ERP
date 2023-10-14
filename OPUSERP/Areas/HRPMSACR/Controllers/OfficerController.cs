using ACR.Data.ViewModel.Evaluation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OPUSERP.Accounting.Services.MasterData.Interfaces;
using OPUSERP.Areas.HRPMSACR.Models;
using OPUSERP.Data;
using OPUSERP.Data.Entity;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.HRPMS.Data.Entity.ACR;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Services.ACR.Interfaces;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSACR.Controllers
{
    [Area("HRPMSACR")]
    [Authorize]
    public class OfficerController : Controller
    {
        private readonly IPersonalInfoService personalInfoService;
        private readonly IPhotographService photographService;
        private readonly ISignatureService signatureService;
        private readonly IAcrInfoService acrInfoService;
        private readonly IUserInfoes userInfoes;
        private readonly IEmployeePunchCardInfoService employeePunchCardInfoService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ERPDbContext _context;

        public OfficerController(
            IPersonalInfoService personalInfoService,
            IPhotographService photographService,
            ISignatureService signatureService,
            IAcrInfoService acrInfoService,
            IUserInfoes userInfoes,
            ERPDbContext context,
            IEmployeePunchCardInfoService employeePunchCardInfoService,
            UserManager<ApplicationUser> _userManager,
            RoleManager<ApplicationRole> _roleManager)
        {
            this.personalInfoService = personalInfoService;
            this.photographService = photographService;
            this.signatureService = signatureService;
            this.acrInfoService = acrInfoService;
            this.userInfoes = userInfoes;
            this.employeePunchCardInfoService = employeePunchCardInfoService;
            this._userManager = _userManager;
            this._roleManager = _roleManager;
            this._context = context;
        }

        //public ApplicationUserManager UserManager
        //{
        //    get
        //    {
        //        return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    }
        //    private set
        //    {
        //        _userManager = value;
        //    }
        //}
        // GET: ACREvaluation/Officer
        public async Task<ActionResult> Index(int? id, string empCode)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var checkRole = await personalInfoService.CheckUserIsHRAdmin(userName);
            var userInfos = await userInfoes.GetUserInfoByUser(userName);

            //Asad Shift from bellow and Added On 26.09.2023
            //var assessment = await acrInfoService.GetAssessmentInfoById(Convert.ToInt32(id));
                        
            //Asad Commented On 26.09.2023
            var emp = await personalInfoService.GetEmployeeUserInfoByCodeBySp(empCode);

            var signature = new EmployeeSignature();
            if (emp != null)
            {
                signature = await personalInfoService.GetSignatureByEmpId(Convert.ToInt32(emp.EmpId));
            }

			var totalqEvaluation = await acrInfoService.GetTotalQuantitativeEvaluationByAssId(Convert.ToInt32(id));
			ViewBag.empacrMarks = await acrInfoService.GetTotalEmployeesAcrByAssId(Convert.ToInt32(id));
			ViewBag.qevaluation = totalqEvaluation;
			ViewBag.totalPercentage = ((totalqEvaluation / 40.0)*100);
            var assBasic = await acrInfoService.AssessmentInfoById(empCode, Convert.ToInt32(id));
            ViewBag.UserTypeId = user.userTypeId;
            
            var assessment = await acrInfoService.GetAssessmentInfoById(Convert.ToInt32(id));
            ViewBag.fromDate = new DateTime(Convert.ToInt32(assessment.assessmentYear), 1, 1);
            ViewBag.toDate = new DateTime(Convert.ToInt32(assessment.assessmentYear), 12, 31);
            ViewBag.UserEmpCode = userInfos.EmpCode;

            //EmployeeUserViewModel userInfo = GetUserInfo(userName);
            var headId = await acrInfoService.GetHeadIdByAssIdAndIndicator(Convert.ToInt32(id), 6);
            if (headId == null) headId = 0;

            var userinfovm = new EmployeeUserViewModel
            {
                EmpId = emp.EmpId,
                EmpCode = empCode,
                EmpName = emp.EmpName,
                EmpNameBn = emp.EmpNameBn,
                WorkStation = emp.WorkStation,
                BranchCode = emp.BranchCode,
                BranchName = emp.BranchName,
                FatherName = emp.FatherName,
                MotherName = emp.MotherName,
                SpouseName = emp.SpouseName,
				MaritalStatus = emp.MaritalStatus,
                JoiningDate = emp.JoiningDate.ToString(),
                BirthDate = emp.BirthDate.ToString(),
                PrlDate = emp.PrlDate.ToString(),
                JoiningDesignation = emp.JoiningDesignation,
                DesignationName = emp.DesignationName,
                PresentAddress = emp.PresentAddress,
                PermanentAddress = emp.PermanentAddress,
                DesiCode = emp.DesiCode,
                UserTypeId = user.userTypeId,
                ActionDate = DateTime.Now.ToString(),
                BD1 = emp.BD1,
                BD2 = emp.BD2,
                NID = emp.NID,
                childrenCount = emp.childrenCount,
				SignUrl = emp.SignUrl,
				ImageUrl = emp.ImageUrl,
				dateOfConfirmation = emp.dateOfConfirmation.ToString(),
				lastPromotionDate = emp.lastPromotionDate.ToString(),
				currentGrade = emp.currentGrade,
				currentBasic = emp.currentBasic,
				totalPaidByBank = emp.totalPaidByBank,
                DistrictName=emp.homeDistrict
                //employee = emp,
            };
            var userinfo = new EmployeeUserViewModel
            {
                EmpCode = userInfos.EmpCode,
                EmpName = userInfos.EmpName,
            };

			ACREvaluationViewModel model = new ACREvaluationViewModel
			{
				assessmentId = id,
				AssessmentBasic = assBasic.FirstOrDefault(),
				headId = headId,
				isHRAdmin = checkRole.isHRAdmin,
				acrType = await acrInfoService.GetAcrTypeByAssId(Convert.ToInt32(id)),
				ACRUsers = userinfovm,
				AcrEvaluationList = await acrInfoService.AcrEvaluationNameByacrForId(2),
				LeaveTypes = await acrInfoService.GetAllLeaveTypes(),
				heads = await acrInfoService.GetAllQuantitativeEvaluationHeads(),
				commentsHead = await acrInfoService.GetAllEvaluationCommentsHeads(),
				aCRIndicators = await acrInfoService.GetAllAcrIndicators(),
				empEducations = await acrInfoService.GetEmpEducations(Convert.ToInt32(userinfovm.EmpId)),
				empPostings = await acrInfoService.GetEmpPostings(Convert.ToInt32(userinfovm.EmpId), (int)id),
				employeeTrainingViewModels = await acrInfoService.GetEmployeeTrainingInfo(id),
				ACRRecommendations = await acrInfoService.GetACRRecommendationByAssId((int)id)
                //quantitativeEvaluations = await acrInfoService.GetQuantitativeEvaluationsByAssId(Convert.ToInt32(id))
            };

            
            ViewBag.assessmentId = id;

            //Asad Added On 019.09.2023
            if(assessment != null)
            {
                if(userName == assessment.recommendator)
                {
                    ViewBag.Evaluator = "Recomendator";
                }
                else if (userName == assessment.signatory)
                {
                    ViewBag.Evaluator = "Signatory";
                }
                else if (userName == assessment.empType)
                {
                    ViewBag.Evaluator = "FinalSignatory";
                }
                else
                {
                    ViewBag.Evaluator = "None";
                }
            }
            else
            {
                ViewBag.Evaluator = "None";
            }
            


            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ipaddress = "";
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipaddress = ip.ToString();
                }
            }


            ViewBag.BaseUrl = ipaddress;
            return View(model);
        }


        public async Task<ActionResult> AcrPreview(int? id, string empCode)
        {
            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var checkRole = await personalInfoService.CheckUserIsHRAdmin(userName);
            var userInfos = await userInfoes.GetUserInfoByUser(userName);
            //var emp = await personalInfoService.GetEmployeeUserInfoByCode(empCode);
            var emp = await personalInfoService.GetEmployeeUserInfoByCodeBySp(empCode);
            var employeeSignature = await personalInfoService.GetSignatureByEmpId(Convert.ToInt32(emp.EmpId));

            var totalqEvaluation = await acrInfoService.GetTotalQuantitativeEvaluationByAssId(Convert.ToInt32(id));
            ViewBag.empacrMarks = await acrInfoService.GetTotalEmployeesAcrByAssId(Convert.ToInt32(id));
            ViewBag.qevaluation = totalqEvaluation;
            ViewBag.totalPercentage = ((totalqEvaluation / 40.0) * 100);

            var assBasic = await acrInfoService.AssessmentInfoById(empCode, Convert.ToInt32(id));
            ViewBag.UserTypeId = user.userTypeId;
            var assessment = await acrInfoService.GetAssessmentInfoById(Convert.ToInt32(id));
            ViewBag.fromDate = new DateTime(Convert.ToInt32(assessment.assessmentYear), 1, 1);
            ViewBag.toDate = new DateTime(Convert.ToInt32(assessment.assessmentYear), 12, 31);
            ViewBag.UserEmpCode = userInfos.EmpCode;

            //EmployeeUserViewModel userInfo = GetUserInfo(userName);
            var headId = await acrInfoService.GetHeadIdByAssIdAndIndicator(Convert.ToInt32(id), 6);
            if (headId == null) headId = 0;

            var userinfovm = new EmployeeUserViewModel
            {
                EmpId = emp.EmpId,
                EmpCode = empCode,
                EmpName = emp.EmpName,
                EmpNameBn = emp.EmpNameBn,
                WorkStation = emp.WorkStation,
                BranchCode = emp.BranchCode,
                BranchName = emp.BranchName,
                FatherName = emp.FatherName,
                MotherName = emp.MotherName,
                SpouseName = emp.SpouseName,
                MaritalStatus = emp.MaritalStatus,
                JoiningDate = emp.JoiningDate.ToString(),
                BirthDate = emp.BirthDate.ToString(),
                PrlDate = emp.PrlDate.ToString(),
                JoiningDesignation = emp.JoiningDesignation,
                DesignationName = emp.DesignationName,
                PresentAddress = emp.PresentAddress,
                PermanentAddress = emp.PermanentAddress,
                DesiCode = emp.DesiCode,
                UserTypeId = user.userTypeId,
                ActionDate = DateTime.Now.ToString(),
                BD1 = emp.BD1,
                BD2 = emp.BD2,
                NID = emp.NID,
                childrenCount = emp.childrenCount,
                SignUrl = emp.SignUrl,
                ImageUrl = emp.ImageUrl,
                dateOfConfirmation = emp.dateOfConfirmation.ToString(),
                lastPromotionDate = emp.lastPromotionDate.ToString(),
                currentGrade = emp.currentGrade,
                currentBasic = emp.currentBasic,
                totalPaidByBank = emp.totalPaidByBank,
                DistrictName = emp.homeDistrict
                //employee = emp,
            };
            var userinfo = new EmployeeUserViewModel
            {
                EmpCode = userInfos.EmpCode,
                EmpName = userInfos.EmpName,
            };

            ACREvaluationViewModel model = new ACREvaluationViewModel
            {
                assessmentId = id,
                AssessmentBasic = assBasic.FirstOrDefault(),
                headId = headId,
                isHRAdmin = checkRole.isHRAdmin,
                acrType = await acrInfoService.GetAcrTypeByAssId(Convert.ToInt32(id)),
                ACRUsers = userinfovm,
                AcrEvaluationList = await acrInfoService.AcrEvaluationNameByacrForId(2),
                LeaveTypes = await acrInfoService.GetAllLeaveTypes(),
                heads = await acrInfoService.GetAllQuantitativeEvaluationHeads(),
                commentsHead = await acrInfoService.GetAllEvaluationCommentsHeads(),
                aCRIndicators = await acrInfoService.GetAllAcrIndicators(),
                empEducations = await acrInfoService.GetEmpEducations(Convert.ToInt32(userinfovm.EmpId)),
                empPostings = await acrInfoService.GetEmpPostings(Convert.ToInt32(userinfovm.EmpId), (int)id),
                employeeTrainingViewModels = await acrInfoService.GetEmployeeTrainingInfo(id),
                ACRRecommendations = await acrInfoService.GetACRRecommendationByAssId((int)id)
                //quantitativeEvaluations = await acrInfoService.GetQuantitativeEvaluationsByAssId(Convert.ToInt32(id))
            };

            ViewBag.assessmentId = id;

            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ipaddress = "";
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipaddress = ip.ToString();
                }
            }


            ViewBag.BaseUrl = ipaddress;
            return View(model);
        }







        [HttpGet]
        public async Task<IActionResult> GetDocumentAttachmentByAssId(int? assessmentId)
        {
            var docList = await acrInfoService.GetDocumentAttachmentByAssId(assessmentId);
            return Json(docList);
        }
        [HttpGet]
        public async Task<IActionResult> GetQuantitativeEvaluationsByAssId(int id)
        {
			var username = Request.HttpContext.User.Identity.Name;
            //var data = await acrInfoService.GetQuantitativeEvaluationsByAssId(Convert.ToInt32(id));
            var data = await acrInfoService.GetQuantitativeEvaByAssId(username, Convert.ToInt32(id));
            return Json(data);
        }
        [HttpGet]
        public async Task<IActionResult> GetEmployeeLeaveInfoByEmpCode(int? assesmentId)
        {
            var lstEmpLeave = await acrInfoService.GetEmployeeLeaveDetailsForAcr(assesmentId);
            return Json(lstEmpLeave);
        }
        [HttpGet]
        public async Task<IActionResult> GetRecommendationComments(int? id)
        {
            var model = new ACREvaluationViewModel
            {
                officerPart5ViewModels = await acrInfoService.GetRecommendationComments(id)
            };
            return Json(model);
            //var data = await acrInfoService.GetRecommendationComments(assesmentId);
            //return Json(data);
        }
        //[HttpGet]
        //public JsonResult GetEducationalQualifications(int? id)
        //{
        //    var lstEmp = new List<EmployeeEducationInfoViewModel>();
        //    lstEmp = ObjectMapHelper<EmployeeEducationInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeeEducation", new object[] { id }));
        //    return Json(lstEmp, JsonRequestBehavior.AllowGet);
        //}
        [HttpGet]
        public async Task<IActionResult> GetEducationalQualifications(int? id)
        {
            var model = new ACREvaluationViewModel
            {
                employeeEducationInfoViewModels = await acrInfoService.GetEmployeeEducation(id)
            };
            return Json(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeeTraining(int? id)
        {
            var model = new ACREvaluationViewModel
            {
                employeeTrainingViewModels = await acrInfoService.GetEmployeeTraining(id)
            };
            return Json(model);
        }
        [HttpGet]
        public async Task<IActionResult> GetOtherEmployment(int id)
        {
            var model = new ACREvaluationViewModel
            {
                prevJobHistories = await acrInfoService.GetOtherEmployment(id)
            };
            return Json(model);
        }
        [HttpGet]
        public async Task<IActionResult> GetEmployeePromotion(int? id)
        {
            var model = new ACREvaluationViewModel
            {
                employeePromotionViewModels = await acrInfoService.GetEmployeePromotion(id)
            };
            return Json(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeeAssignment(int? id)
        {
            var model = new ACREvaluationViewModel
            {
                employeeAssignmentViewModels = await acrInfoService.GetEmployeeAssignment(id)
            };
            return Json(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetQuantativeEvaluation(int? id, int? headId)
        {
            var model = new ACREvaluationViewModel
            {
                aCRQuantitativeEvaluationViewModels = await acrInfoService.GetQuantativeEvaluation(id, headId)
            };
            return Json(model);
        }
        [HttpGet]
        public async Task<IActionResult> GetEvaluationTotalMarks(int? id)
        {
            var model = new ACREvaluationViewModel
            {
                evaluationOfficerPartTwoViewModels = await acrInfoService.GetEmployeeAcrInfoForOfficers(id)
            };
            return Json(model);
        }
        //[HttpGet]
        //public async Task<IActionResult> GetRecommendationComments(int? id)
        //{
        //    var model = new ACREvaluationViewModel
        //    {
        //        officerPart5ViewModels = await acrInfoService.GetRecommendationComments(id)
        //    };
        //    return Json(model);
        //}

        [HttpPost]
        public async Task<IActionResult> SaveLeave([FromForm]LeaveDetailsViewModel model)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                var user = await _userManager.FindByNameAsync(userName);

                var assBasic = await acrInfoService.AssessmentInfoById(userName, Convert.ToInt32(model.leaveAssessId));

                for (int i = 0; i < model.leaveTypeId.Length; i++)
                {
                    LeaveDetail leave = new LeaveDetail();
                    if (model.leaveId[i] > 0)
                    {
                        var data = await acrInfoService.GetLeaveDetailsById(Convert.ToInt32(model.leaveId[i]));

                        data.totalLeave = model.totalLeave[i];
                        data.consumptionLeave = model.consumptionLeave[i];
                        data.leaveBalance = model.leaveBalance[i];

                        await acrInfoService.SaveLeaveDetails(data);
                    }
                    else
                    {
                        leave.Id = 0;
                        leave.assessmentId = model.leaveAssessId;
                        leave.leaveTypeId = model.leaveTypeId[i];
                        leave.totalLeave = model.totalLeave[i];
                        leave.consumptionLeave = model.consumptionLeave[i];
                        leave.leaveBalance = model.leaveBalance[i];

                        await acrInfoService.SaveLeaveDetails(leave);
                    }
                }
                var assess = await acrInfoService.GetAssessmentInfoById(Convert.ToInt32(model.leaveAssessId));

                if (assess != null) // && userInfo.UserTypeId==4
                {
                    if (assess.statusInfoId == 1)
                    {
                        var quan = await acrInfoService.DeleteAcrDataByChangeDrawer(Convert.ToInt32(model.leaveAssessId));
                        //quan = ObjectMapHelper<ACROtherTableViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_DeleteAcrData_ByChangeDrawer", new object[] { model.leaveAssessId }));
                    }

                    assess.assessmentDate = DateTime.Now;
                    assess.updatedBy = User.Identity.Name;
                    assess.updatedAt = DateTime.Now;
                    assess.durationInRecommendetor = model.durationInRecommendetor;
                    assess.duration1 = model.duration1;
                    assess.duration2 = model.duration2;
                    assess.newSignatory = "1";



                    //if (model.signatoryF != "" && model.signatoryF != null)
                    //{
                    //    var userDetails = await personalInfoService.GetEmployeeInfoByCode(assess.empCode);
                    //    //assess.signatoryDate = DateTime.Now;
                    //    //assess.signatory = model.signatoryF;
                    //    //assess.statusInfoId = 5;
                    //    await acrInfoService.SendNotification(userName, userName, model.signatoryF, "ACR From " + userDetails.nameEnglish, "Waiting for ACR to Signatory", "ACR", "/HRPMSACR/AssessmentInfo/Authority");

                    //}
                    //else
                    //{
                    //    var userDetails = await personalInfoService.GetEmployeeInfoByCode(userName);
                    //    assess.routeOrder = 1;
                    //    assess.statusInfoId = 1;
                    //    await acrInfoService.SendNotification(userName, userName, "2503", "ACR From " + userDetails.nameEnglish, "Waiting to choose the recommendator", "ACR", "/HRPMSACR/AssessmentInfo/ACRPendingListForHRAdmin");
                    //}

                    if (user.UserName == assess.empCode)
                    {
                        var userDetails = await personalInfoService.GetEmployeeInfoByCode(userName);
                        assess.routeOrder = 1;
                        assess.statusInfoId = 1;
                        await acrInfoService.SendNotification(userName, userName, "2503", "ACR From " + userDetails.nameEnglish, "Waiting to choose the recommendator", "ACR", "/HRPMSACR/AssessmentInfo/ACRPendingListForHRAdmin");
                        await acrInfoService.SendNotification(userName, userName, "2064", "ACR From " + userDetails.nameEnglish, "Waiting to choose the recommendator", "ACR", "/HRPMSACR/AssessmentInfo/ACRPendingListForHRAdmin");
                    }

                    else if (user.UserName == assess.recommendator)
                    {

                        var userDetails = await personalInfoService.GetEmployeeInfoByCode(userName);
                        await acrInfoService.SendNotification(userName, userName, assess.signatory, "ACR From " + userDetails.nameEnglish, "Waiting for ACR to Signatory", "ACR", "/HRPMSACR/AssessmentInfo/Authority");
                        await acrInfoService.SendNotification(userName, userName, "2503", userDetails.nameEnglish + "'s ACR has been waiting for signatory.", "", "ACR", "/HRPMSACR/AssessmentInfo/ACRPendingListForHRAdmin");
                        await acrInfoService.SendNotification(userName, userName, "2064", userDetails.nameEnglish + "'s ACR has been waiting for signatory.", "", "ACR", "/HRPMSACR/AssessmentInfo/ACRPendingListForHRAdmin");

                    }
                    



                    await acrInfoService.SaveAssessment(assess);

                    if (user.userTypeId == 4)
                    {
                        user.isActive = 0;
                        await acrInfoService.SaveUserInfo(user);
                    }

                    var dataS = await acrInfoService.GetAcrOtherByAsssId(Convert.ToInt32(model.leaveAssessId));
                    if (dataS.Count() > 0)
                    {
                        await acrInfoService.DeleteAcrOtherByAssId(Convert.ToInt32(model.leaveAssessId));
                    }
                    if (model.expBanking != null || model.expBanking != "null" || model.expBanking != "")
                    {
                        ACROtherTables acrS = new ACROtherTables
                        {
                            assessmentId = model.leaveAssessId,
                            bankingExperience = model.expBanking,
                            isDelete = 0,
                            createdAt = DateTime.Now,
                            createdBy = HttpContext.User.Identity.Name
                        };
                        await acrInfoService.SaveACROther(acrS);
                    }
                }


                return Json(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //[HttpGet]
        //public JsonResult GetACRLeaveInfo(int? id)
        //{
        //    var result = _context.LeaveDetails.Where(x => x.assessmentId == id).ToList();
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public async Task<IActionResult> SaveOfficer5thPart(OfficerPart5ViewModel model)
        {
            string userName = User.Identity.Name;
            var userInfo = await userInfoes.GetUserInfoByUserNameNew(userName);
            //var userInfo = GetUserInfo(userName);
            var checkRole = await personalInfoService.CheckUserIsHRAdmin(userName);

            var assessment = _context.assessments.Find(model.assessmentId5thPart);
            var signatory2 = await personalInfoService.GetEmployeeInfoByCodeSp(assessment.empType);
            var signatory = await personalInfoService.GetEmployeeInfoByCodeSp(assessment.signatory);

            if (userInfo.EmpCode == assessment.recommendator)
            {
                var userDetails = await personalInfoService.GetEmployeeInfoByCode(assessment.empCode);
                assessment.recommendationDate = DateTime.Now;
                assessment.statusInfoId = 6;
                assessment.updatedBy = User.Identity.Name;
                assessment.updatedAt = DateTime.Now;
				assessment.routeOrder = 2;
                //await acrInfoService.SendNotification(userName, userName, "2503", "ACR From " + userDetails.nameEnglish, "Waiting to choose the Signatory", "ACR", "/HRPMSACR/AssessmentInfo/ACRPendingListForHRAdmin");
            }
            else if (checkRole.isHRAdmin > 0 && assessment.recommendator != null)
            {
                var userDetails = await personalInfoService.GetEmployeeInfoByCode(assessment.empCode);

                assessment.updatedBy = User.Identity.Name;
                assessment.updatedAt = DateTime.Now;
                assessment.signatory = model.signEmpId;
                assessment.signatoryDate = DateTime.Now;
                await acrInfoService.SendNotification(userName, userName, model.finalApprover, "ACR From " + userDetails.nameEnglish, "Waiting for ACR to Signatory", "ACR", "/HRPMSACR/AssessmentInfo/Authority");
            }
            else if (userInfo.EmpCode == assessment.signatory)
            {
                var userDetails = await personalInfoService.GetEmployeeInfoByCode(assessment.empCode);

                assessment.signatoryDate = DateTime.Now;
                assessment.empType = assessment.empType;
                assessment.statusInfoId = 5;
                assessment.updatedBy = User.Identity.Name;
                assessment.updatedAt = DateTime.Now;

                await acrInfoService.SendNotification(userName, userName, "2503", userDetails.nameEnglish + "'s ACR is on MD hand.", "", "ACR", "/HRPMSACR/AssessmentInfo/ACRPendingListForHRAdmin");
                await acrInfoService.SendNotification(userName, userName, model.finalApprover, "ACR From " + userDetails.nameEnglish, "Waiting for ACR to Complete", "ACR", "/HRPMSACR/AssessmentInfo/Authority");
            }
            else if (userInfo.EmpCode == assessment.empType)
			{
                var userDetails = await personalInfoService.GetEmployeeInfoByCode(assessment.empCode);

                //assessment.signatoryDate = DateTime.Now;
				assessment.statusInfoId = 7;
				assessment.updatedBy = User.Identity.Name;
				assessment.updatedAt = DateTime.Now;

                await acrInfoService.SendNotification(userName, userName, "2503", userDetails.nameEnglish + "'s ACR has been Completed.", "", "ACR", "/HRPMSACR/AssessmentInfo/AcrCompletedList");
            }


            //if (model.finalApprover != "" && model.finalApprover != null)
            //{
            //    var userDetails = await personalInfoService.GetEmployeeInfoByCode(assessment.empCode);
            //    //assess.signatoryDate = DateTime.Now;
            //    //assess.signatory = model.signatoryF;
            //    //assess.statusInfoId = 5;
            //    await acrInfoService.SendNotification(userName, userName, model.finalApprover, "ACR From " + userDetails.nameEnglish, "Waiting for ACR to Complete", "ACR", "/HRPMSACR/AssessmentInfo/Authority");

            //}

         
            await acrInfoService.SaveAssessment(assessment);
            //_context.Entry(assessment).State = EntityState.Modified;

            //_context.SaveChanges();

            var dataS = _context.aCRSignatories.Where(x => x.assessmentId == model.assessmentId5thPart);
            if (dataS.Count() > 0)
            {
                _context.aCRSignatories.RemoveRange(_context.aCRSignatories.Where(x => x.assessmentId == model.assessmentId5thPart));
                _context.SaveChanges();
            }
            ACRSignatories acrS = new ACRSignatories
            {
                assessmentId = model.assessmentId5thPart,
                evaluationType = model.evaluationType,
                disAgreementComment = model.disAgreementComment,
                additionalComment = model.additionalComment,
                isDelete = 0,
                createdAt = DateTime.Now,
                createdBy = HttpContext.User.Identity.Name
            };
            _context.aCRSignatories.Add(acrS);
            _context.SaveChanges();


            int idRecom = 0;
			var recom = await _context.aCRRecommendations.AsNoTracking().Where(x => x.assessmentId == model.assessmentId5thPart).FirstOrDefaultAsync();
			if (recom != null)
			{
				idRecom = recom.Id;
			}

            var acr = new ACRRecommendations
            {
                Id = idRecom,
                assessmentId = model.assessmentId5thPart,
                //specialComment = model.specialComment6,
                //moralComment = model.moralComment6,
                //intellectualComment = model.intellectualComment6,
                //subjectiveComment = model.subjectiveComment6,
                //trainingComment = model.trainingComment6,

                PromotionValue = model.promotionalQualification,
                IncrementValue = model.IncrementValue,
                PermanentValue = model.PermanentValue,
                logical = model.logically,
                type = 2,
                //evaluationValueFirstRecom = model.evaluationType,
                //evaluationValueSecondRecom = model.evaluationValueSecondRecom,

                //evaluationTotalFirstRecom = model.evaluationTotalFirstRecom,
                //evaluationTotalSecondRecom = model.evaluationTotalSecondRecom,
                //evaluationTextFirstRecom = model.evaluationTextFirstRecom,
                //evaluationSecondFirstRecom = model.evaluationTextSecondRecom,

                Recom6Name = signatory2.nameEnglish,
                Recom6Desig = signatory2.designationName,
                Recom6Code = signatory2.employeeCode,
                evaluationDateSecondRecom = model.evaluationDateSecondRecom,

                Recom5Name = signatory.nameEnglish,
                Recom5Desig = signatory.designationName,
                Recom5Code = signatory.employeeCode,
                evaluationDateFirstRecom = model.evaluationDateFirstRecom,

                specialComment = model.evaluationTotalRecommendator.ToString(),
                //otherQualification = model.othersRecommends6,
                isDelete = 0,
                createdAt = DateTime.Now,
                createdBy = HttpContext.User.Identity.Name,
            };
            if (userInfo.EmpCode == assessment.signatory)
            {
                acr.evaluationTotalFirstRecom = model.evaluationTotalFirstRecom;
                acr.evaluationValueFirstRecom = model.evaluationType;
                acr.evaluationTextFirstRecom = model.evaluationTextFirstRecom;
                acr.logical = recom.logical;
                acr.evaluationDateFirstRecom = recom.evaluationDateFirstRecom;
            }
            if (userInfo.EmpCode == assessment.empType)
            {
                acr.evaluationTotalFirstRecom = recom.evaluationTotalFirstRecom;
                acr.evaluationValueFirstRecom = recom.evaluationValueFirstRecom;
                acr.evaluationTextFirstRecom = recom.evaluationTextFirstRecom;
                acr.evaluationDateFirstRecom = recom.evaluationDateFirstRecom;
                acr.logical = recom.logical;

                acr.evaluationValueSecondRecom = model.evaluationValueSecondRecom;
                acr.evaluationTotalSecondRecom = model.evaluationTotalSecondRecom;
                acr.evaluationSecondFirstRecom = model.evaluationTextSecondRecom;
                //acr.evaluationDateSecondRecom = model.evaluationDateSecondRecom;
                acr.evaluationDateSecondRecom = model.evaluationDateSecondRecomNew;
            }

                //if (userName != "2666" && checkRole.isHRAdmin > 0)
                //{
                //	acr.Recom5Name = model.signEmpName;
                //	acr.Recom5Code = model.signEmpId;
                //	acr.Recom5Desig = model.signDesig;
                //	acr.evaluationDateFirstRecom = DateTime.Now;
                //}
                //if (userName == "2666")
                //{
                //	acr.Recom6Name = "কাজী আলমগীর";
                //	acr.Recom6Desig = "ম্যানেজিং ডিরেক্টর";
                //	acr.Recom6Code = "2666";

                // acr.Recom5Name = model.signEmpName;
                // acr.Recom5Code = model.signEmpId;
                // acr.Recom5Desig = model.signDesig;
                // acr.evaluationDateFirstRecom = model.evaluationDateFirstRecom;
                // acr.evaluationDateSecondRecom = DateTime.Now;
                //}

                //if (model.evaluationType != null)
                //{
                //	acr.evaluationDateFirstRecom = DateTime.Now;
                //}

                //if (model.evaluationType != null && model.evaluationValueSecondRecom != null)
                //{
                //	acr.evaluationDateSecondRecom = DateTime.Now;
                //}

                try
			{
				if (acr.Id != 0)
				{
					_context.aCRRecommendations.Update(acr);
				}
				else
				{
					_context.aCRRecommendations.Add(acr);
				}
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{

				throw;
			}


			return Json(true);

        }

        [HttpGet]
        public ActionResult UpdateRecommendatorOfficer(int assesId, string recomId)
        {
            try
            {
                var data = _context.assessments.Find(assesId);

                data.signatory = recomId;
                data.statusInfoId = 6;
                data.recommendationDate = DateTime.Now;
                data.updatedBy = User.Identity.Name;
                data.updatedAt = DateTime.Now;
                _context.Entry(data).State = EntityState.Modified;

                _context.SaveChanges();

                return Json(assesId);
                //return Json(assesId, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //[HttpPost]
        //public JsonResult SaveOfficer5thPartForDrawer(OfficerPart5ViewModel model)
        //{
        //    var data = _context.ACRComments.Where(x => x.assessmentId == model.assessmentId5thPart);
        //    if (data.Count() > 0)
        //    {
        //        _context.ACRComments.RemoveRange(_context.ACRComments.Where(x => x.assessmentId == model.assessmentId5thPart));
        //        _context.SaveChanges();
        //    }
        //    ACRComment acr = new ACRComment
        //    {
        //        assessmentId = model.assessmentId5thPart,
        //        livingStandard = model.livingStandard,
        //        isDelete = 0,
        //        createdAt = DateTime.Now,
        //        createdBy = HttpContext.User.Identity.Name
        //    };
        //    _context.ACRComments.Add(acr);
        //    _context.SaveChanges();
        //    var dataR = _context.ACRRecommendations.Where(x => x.assessmentId == model.assessmentId5thPart);
        //    if (dataR.Count() > 0)
        //    {
        //        _context.ACRRecommendations.RemoveRange(_context.ACRRecommendations.Where(x => x.assessmentId == model.assessmentId5thPart));
        //        _context.SaveChanges();
        //    }
        //    ACRRecommendation acrR = new ACRRecommendation
        //    {
        //        assessmentId = model.assessmentId5thPart,
        //        promotionalQualification = model.promotionalQualification,
        //        isDelete = 0,
        //        createdAt = DateTime.Now,
        //        createdBy = HttpContext.User.Identity.Name
        //    };
        //    _context.ACRRecommendations.Add(acrR);
        //    _context.SaveChanges();

        //    return Json(true);

        //}

        [HttpPost]
        public async Task<IActionResult> SaveMdChange(OfficerPart5ViewModel model)
        {
            var assestId = model.assessmentId5thPart;

            var promotionalQualification = model.promotionalQualification;
            //var moralComment = "";
            //var intellectualComment = "";
            //var subjectiveComment = "";

            //var specialComment = "";
            //var trainingComment = "";
            //var otherQualification = "";


            var userName = HttpContext.User.Identity.Name;

            //var ff = ObjectMapHelper<ApplicationUser>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_Update_ACRRecommendations", new object[] { assestId, promotionalQualification, moralComment, intellectualComment, subjectiveComment, specialComment, trainingComment, otherQualification, userName }));
            //var ff = await acrInfoService.UpdateAcrRecommendations(assestId, model.logically,promotionalQualification, moralComment, intellectualComment, subjectiveComment, specialComment, trainingComment, otherQualification, userName);



            var dataS = _context.aCRSignatories.Where(x => x.assessmentId == model.assessmentId5thPart);
            if (dataS.Count() > 0)
            {
                _context.aCRSignatories.RemoveRange(_context.aCRSignatories.Where(x => x.assessmentId == model.assessmentId5thPart));
                _context.SaveChanges();
            }
            ACRSignatories acrS = new ACRSignatories
            {
                assessmentId = model.assessmentId5thPart,
                evaluationType = model.evaluationType,
                disAgreementComment = model.disAgreementComment,
                additionalComment = model.additionalComment,
                isDelete = 0,
                createdAt = DateTime.Now,
                createdBy = HttpContext.User.Identity.Name
            };
            _context.aCRSignatories.Add(acrS);
            _context.SaveChanges();

            return Json(true);

        }

        //[HttpGet]
        //public ActionResult UpdateRecommendatorForDrawer(int assesId, string recomId)
        //{
        //    try
        //    {
        //        var data = _context.Assessments.Find(assesId);

        //        data.signatory = recomId;
        //        data.recommendationDate = DateTime.Now;
        //        data.updatedBy = User.Identity.Name;
        //        data.updatedAt = DateTime.Now;
        //        _context.Entry(data).State = EntityState.Modified;

        //        _context.SaveChanges();

        //        return Json(assesId, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        [HttpGet]
        public JsonResult GetACRRecommendations(int? id)
        {
            var result = (from C in _context.aCRRecommendations
                          where C.assessmentId == id
                          select new OfficerPart5ViewModel
                          {
                              promotionalQualification = C.promotionalQualification,
                              promotionalQualificationSignatory = C.promotionalQualificationSignatory

                          }).ToList();
            return Json(result.FirstOrDefault());
        }
		public async Task<IActionResult> GetACRRecommendationsJson(int id)
		{
			var data = await acrInfoService.GetACRRecommendationByAssId(id);
			return Json(data);
		}
		[HttpGet]
        public JsonResult GetACRSignatories(int? id)
        {
            var result = (from C in _context.aCRSignatories
                          where C.assessmentId == id
                          select new OfficerPart5ViewModel
                          {
                              evaluationType = C.evaluationType,
                              disAgreementComment = C.disAgreementComment,
                              additionalComment = C.additionalComment
                          }).ToList();
            return Json(result.FirstOrDefault());
        }
        [HttpGet]
        public JsonResult GetACRComments(int? id)
        {
            var result = (from C in _context.aCRComments
                          where C.assessmentId == id
                          select new OfficerPart5ViewModel
                          {
                              livingStandard = C.livingStandard,

                          }).ToList();
            return Json(result.FirstOrDefault());
        }

        [HttpGet]
        public JsonResult GetEvaluationMarksByCommentsId(int? id)
        {
            var result = (from C in _context.evaluationCommentsHeads
                          where C.Id == id
                          select new EvaluationCommentsHeadsViewModel
                          {
                              commentsMark = C.commentsMark,

                          }).ToList();
            return Json(result.FirstOrDefault());
            //return Json(result.FirstOrDefault(), JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public JsonResult GetTargetAchivementTotalMarks(int? id)
        //{
        //    var quan = new List<QuantitativeEvaluationsViewModel>();
        //    quan = ObjectMapHelper<QuantitativeEvaluationsViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_SUB_TargetEvaluation", new object[] { id }));
        //    return Json(quan.FirstOrDefault(), JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public JsonResult GetTargetAchivement92DaysTotalMarks(int? id)
        //{
        //    var quan = new List<QuantitativeEvaluationsViewModel>();
        //    quan = ObjectMapHelper<QuantitativeEvaluationsViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_RPT_SUB_TargetEvaluation_Officer_4thPart", new object[] { id }));
        //    return Json(quan.FirstOrDefault(), JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public JsonResult GetEvaluationTotalMarks(int? id)
        //{
        //    var quan = new List<EmployeesAcrsViewModel>();
        //    quan = ObjectMapHelper<EmployeesAcrsViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_Getparttwobyassignmentid", new object[] { id }));
        //    return Json(quan.FirstOrDefault(), JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public JsonResult GetEmployeeAssignment(int? id)
        //{
        //    var lstEmp = new List<EmployeeAssignmentViewModel>();
        //    lstEmp = ObjectMapHelper<EmployeeAssignmentViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesAssignment", new object[] { id }));
        //    return Json(lstEmp, JsonRequestBehavior.AllowGet);
        //}

        //internal AssessmentViewModel GetAssessmentBasicInfo(int? assId)
        //{
        //    string userName = User.Identity.Name;
        //    var lstEmp = new List<AssessmentViewModel>();
        //    lstEmp = ObjectMapHelper<AssessmentViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetAssessmentInfoById", new object[] { assId, userName }));
        //    return lstEmp.FirstOrDefault();
        //}

        //public EmployeeUserViewModel GetUserInfo(string userName)
        //{
        //    var lstEmp = new List<EmployeeUserViewModel>();
        //    lstEmp = ObjectMapHelper<EmployeeUserViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GETUserInformation", new object[] { userName }));
        //    return lstEmp.FirstOrDefault();
        //}



        //[HttpPost]
        //public ActionResult UploadTargetAchiveDocumentCertification_92()
        //{
        //    try
        //    {

        //        string ID = Request.Form[0];
        //        int? assId = Convert.ToInt32(Request.Form[1]);


        //        HttpFileCollectionBase files = Request.Files;
        //        for (int i = 0; i < files.Count; i++)
        //        {
        //            string filename = Path.GetFileName(Request.Files[i].FileName);
        //            Guid newGuid = Guid.NewGuid();
        //            HttpPostedFileBase file = files[i];
        //            string fname;
        //            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
        //            {
        //                string[] testfiles = file.FileName.Split(new char[] { '\\' });
        //                fname = "CERT92_" + assId + "_" + testfiles[testfiles.Length - 1];
        //            }
        //            else
        //            {
        //                fname = "CERT92_" + assId + "_" + file.FileName;
        //            }

        //            string filname = Path.Combine(Server.MapPath("~/Assets/Upload/Document"), fname);
        //            var existDoc = _context.DocumentAttachments.Where(x => x.assesmentId == assId && x.attachFor == "Certification_92").FirstOrDefault();
        //            if (existDoc != null)
        //            {
        //                string existFilname = Path.Combine(Server.MapPath("~/"), existDoc.attachmentpath);
        //                if (System.IO.File.Exists(existFilname))
        //                {
        //                    System.IO.File.Delete(existFilname);
        //                }
        //                existDoc.attachmentpath = "Assets/Upload/Document/" + fname;
        //                existDoc.updatedBy = User.Identity.Name;
        //                existDoc.updatedAt = DateTime.Now;
        //                _context.Entry(existDoc).State = EntityState.Modified;

        //                _context.SaveChanges();
        //            }
        //            else
        //            {
        //                DocumentAttachment model = new DocumentAttachment
        //                {
        //                    assesmentId = Convert.ToInt32(assId),
        //                    empCode = ID,
        //                    attachFor = "Certification_92",
        //                    attachmentpath = "Assets/Upload/Document/" + fname,
        //                    createdAt = DateTime.Now,
        //                    createdBy = User.Identity.Name
        //                };
        //                _context.DocumentAttachments.Add(model);
        //                _context.SaveChanges();
        //            }

        //            file.SaveAs(filname);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json("Error occurred. Error details: " + ex.Message);
        //    }
        //    return Json("File Uploaded Successfully!");
        //}

        //[HttpGet]
        //public JsonResult GetTargetAchiveDocsCertification_92(int assesId)
        //{

        //    var result = _context.DocumentAttachments.Where(x => x.assesmentId == assesId && x.attachFor == "Certification_92").FirstOrDefault();
        //    if (result == null)
        //    {
        //        result = new DocumentAttachment();
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public async Task<IActionResult> GetEmployeesDrawerWithoutOwnCode(string empCode, string ownCode, int assessmentId)
        {
            var data = new ReCommendatorViewModel
            {
                employee = await acrInfoService.GetEmployeeByEmpCode(empCode),
                assessment = await acrInfoService.GetAssessmentInfoById(assessmentId),
                signatureUrl = await acrInfoService.GetSignatureByEmpCode(empCode)
            };
            
            return Json(data);
        }
    }
}