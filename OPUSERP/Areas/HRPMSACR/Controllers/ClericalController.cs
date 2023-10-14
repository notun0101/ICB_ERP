using ACR.Data;
using ACR.Data.ViewModel.Evaluation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OPUSERP.Areas.HRPMSACR.Models;
using OPUSERP.Data;
using OPUSERP.Data.Entity;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.ACR;
using OPUSERP.HRPMS.Services.ACR.Interfaces;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web;

namespace OPUSERP.Areas.HRPMSACR.Controllers
{
    [Authorize]
    [Area("HRPMSACR")]
    public class ClericalController : Controller
    {
        private readonly ERPDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAcrInfoService acrInfoService;
        private readonly IPersonalInfoService personalInfoService;
        private readonly IUserInfoes userInfoes;
        // GET: Clerical
        public ClericalController(ERPDbContext context, IUserInfoes userInfoes, UserManager<ApplicationUser> userManager, IAcrInfoService acrInfoService, IPersonalInfoService personalInfoService)
        {
            this._context = context;
            _userManager = userManager;
            this.acrInfoService = acrInfoService;
            this.personalInfoService = personalInfoService;
            this.userInfoes = userInfoes;
        }

        public async Task<IActionResult>  Index(int? id,string empCode)
        {
            string userName = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var userInfos = await userInfoes.GetUserInfoByUser(userName);
            var emp = await personalInfoService.GetEmployeeUserInfoByCodeBySp(empCode);
            var signature = await personalInfoService.GetSignatureByEmpId(Convert.ToInt32(emp.EmpId));


            var lstAssBasic = await acrInfoService.AssessmentInfoById(empCode, Convert.ToInt32(id));
            ViewBag.UserTypeId = user.userTypeId;
            //ViewBag.UserTypeId = _context.Users.Where(x => x.UserName == userName).FirstOrDefault().userTypeId;
            var assessment = await acrInfoService.GetAssessmentInfoById(Convert.ToInt32(id));
            //ViewBag.fromDate = new DateTime(Convert.ToInt32(assessment.assessmentYear), 1, 1);
            //ViewBag.toDate = new DateTime(Convert.ToInt32(assessment.assessmentYear), 12, 31);
            ViewBag.UserEmpCode = userInfos.EmpCode;
            var headId = await acrInfoService.GetHeadIdByAssIdAndIndicator(Convert.ToInt32(id), 6);
            if (headId == null) headId = 0;

            var userInfo = await acrInfoService.GetAcrUserInfo(userName);
            var assBasic = lstAssBasic.FirstOrDefault();
            ViewBag.fromDate = assBasic.fromDate;
            ViewBag.toDate = assBasic.toDate;

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
                totalPaidByBank = emp.totalPaidByBank
                //employee = emp,
            };
            var userinfo = new EmployeeUserViewModel
            {
                EmpCode = userInfos.EmpCode,
                EmpName = userInfos.EmpName,
            };

            ACREvaluationViewModel model = new ACREvaluationViewModel
            {
                assessmentId=id,
                empCode=empCode,
                AssessmentBasic = assBasic,
                ACRUser = userInfo,
                //AcrEvaluationList = _context.acrEvaluationNames.Where(x => x.acrForId == 3).ToList(),

                headId = headId,
                ACRUsers = userinfovm,
                AcrEvaluationList = await acrInfoService.AcrEvaluationNameByacrForId(3),
                LeaveTypes = await acrInfoService.GetAllLeaveTypes(),
                heads = await acrInfoService.GetAllQuantitativeEvaluationHeads(),
                commentsHead = await acrInfoService.GetAllEvaluationCommentsHeads(),
                aCRIndicators = await acrInfoService.GetAllAcrIndicators(),
                empEducations = await acrInfoService.GetEmpEducations(Convert.ToInt32(userinfovm.EmpId)),
                empPostings = await acrInfoService.GetEmpPostings(Convert.ToInt32(userinfovm.EmpId),(int)id),
                prevJobHistories = await acrInfoService.GetOtherEmployment((int)id)

            };
          
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
            ViewBag.assesmentId = id;
            return View(model);
        }

        public async Task<ActionResult> GetAssessmentBasicInfo(int? assId)
        {
            string userName = User.Identity.Name;
            var lstEmp = await acrInfoService.AssessmentInfoById(userName, Convert.ToInt32(assId));

            //var lstEmp = new List<AssessmentViewModel>();
            //lstEmp = ObjectMapHelper<AssessmentViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetAssessmentInfoById", new object[] { assId, userName }));
            return Json(lstEmp);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateClarck1stPart(string assessmentId, string assessmentDate, string expBanking, string expBankingwithout, string drawerCode)
        {
            try
            {
                var data = _context.assessments.Find(Convert.ToInt32(assessmentId));

                data.assessmentDate = Convert.ToDateTime(assessmentDate);
                if (drawerCode != "")
                {
                    data.assignToRecomDate = DateTime.Now;
                    data.recommendator = drawerCode;
                    data.statusInfoId = 4;
                }
                else
                {
                    data.statusInfoId = 1;
                }
                data.updatedBy = User.Identity.Name;
                data.updatedAt = DateTime.Now;
                _context.Entry(data).State = EntityState.Modified;

                _context.SaveChanges();

                int asstId = Convert.ToInt32(assessmentId);
                var dataS = _context.aCROtherTables.Where(x => x.assessmentId == asstId);
                if (dataS.Count() > 0)
                {
                    _context.aCROtherTables.RemoveRange(_context.aCROtherTables.Where(x => x.assessmentId == asstId));
                    _context.SaveChanges();
                }
                if (expBanking != "" || expBankingwithout != "")
                {
                    ACROtherTables acrS = new ACROtherTables
                    {
                        assessmentId = asstId,
                        bankingExperience = expBanking,
                        nobankingExperience = expBankingwithout,
                        isDelete = 0,
                        createdAt = DateTime.Now,
                        createdBy = HttpContext.User.Identity.Name
                    };
                    _context.aCROtherTables.Add(acrS);
                    _context.SaveChanges();
                }

                #region User Update
                string userName = User.Identity.Name;
                var userInfo = await _userManager.FindByNameAsync(userName);
                var userType = userInfo.userTypeId;
                if (userType == 4)
                {
                    userInfo.isActive = 0;
                }

                await _userManager.UpdateAsync(userInfo);
                _context.SaveChanges();

                #endregion


                return Json(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<IActionResult> SaveClerkAcrInfo([FromBody]ACREvaluationViewModel model)
        {
            if (model.EmployeesAcr.Count() >= 1)
            {
                string userName = HttpContext.User.Identity.Name;
                var user = await userInfoes.GetUserInfoByUserNameNew(userName);
                int? userTypeId = user.UserTypeId;
                int? acrTypeId = Convert.ToInt32(model.actionType);

                //EmployeeUserViewModel userInfo = GetUserInfo(userName);
                var assBasic = await acrInfoService.GetAssessmentInfoById(Convert.ToInt32(model.assessmentId));

                List<EmployeesAcr> lstACR = new List<EmployeesAcr>();
                foreach (var data in model.EmployeesAcr)
                {
                    EmployeesAcr acr = new EmployeesAcr();

                    acr.Id = data.Id;
                    acr.assessmentId = model.assessmentId;
                    acr.acrEvaluationNameId = data.acrEvaluationNameId;
                    acr.empCode = model.empCode;
                    acr.isDelete = 0;

                    if (data.Id > 0)
                    {

                        var acrMark = await acrInfoService.GetEmployeesAcrById(data.Id);
                        if (user.EmpCode == assBasic.signatory)
                        {

                            if (data.firstValue == 2)
                            {
                                acrMark.firstValue = acrMark.firstValue == 1 ? 1 : 2;
                                acrMark.sencondValue = acrMark.sencondValue == 2 ? 0 : acrMark.sencondValue;
                                acrMark.thirdValue = acrMark.thirdValue == 2 ? 0 : acrMark.thirdValue;
                                acrMark.forthValue = acrMark.forthValue == 2 ? 0 : acrMark.forthValue;
                                acrMark.fifthValue = acrMark.fifthValue == 2 ? 0 : acrMark.fifthValue;
                            }
                            else if (data.sencondValue == 2)
                            {
                                acrMark.sencondValue = acrMark.sencondValue == 1 ? 1 : 2;
                                acrMark.firstValue = acrMark.firstValue == 2 ? 0 : acrMark.firstValue;
                                acrMark.thirdValue = acrMark.thirdValue == 2 ? 0 : acrMark.thirdValue;
                                acrMark.forthValue = acrMark.forthValue == 2 ? 0 : acrMark.forthValue;
                                acrMark.fifthValue = acrMark.fifthValue == 2 ? 0 : acrMark.fifthValue;
                            }
                            else if (data.thirdValue == 2)
                            {
                                acrMark.thirdValue = acrMark.thirdValue == 1 ? 1 : 2;
                                acrMark.firstValue = acrMark.firstValue == 2 ? 0 : acrMark.firstValue;
                                acrMark.sencondValue = acrMark.sencondValue == 2 ? 0 : acrMark.sencondValue;
                                acrMark.forthValue = acrMark.forthValue == 2 ? 0 : acrMark.forthValue;
                                acrMark.fifthValue = acrMark.fifthValue == 2 ? 0 : acrMark.fifthValue;
                            }
                            else if (data.forthValue == 2)
                            {
                                acrMark.forthValue = acrMark.forthValue == 1 ? 1 : 2;
                                acrMark.firstValue = acrMark.firstValue == 2 ? 0 : acrMark.firstValue;
                                acrMark.sencondValue = acrMark.sencondValue == 2 ? 0 : acrMark.sencondValue;
                                acrMark.thirdValue = acrMark.thirdValue == 2 ? 0 : acrMark.thirdValue;
                                acrMark.fifthValue = acrMark.fifthValue == 2 ? 0 : acrMark.fifthValue;
                            }
                            else if (data.fifthValue == 2)
                            {
                                acrMark.fifthValue = acrMark.fifthValue == 1 ? 1 : 2;
                                acrMark.firstValue = acrMark.firstValue == 2 ? 0 : acrMark.firstValue;
                                acrMark.sencondValue = acrMark.sencondValue == 2 ? 0 : acrMark.sencondValue;
                                acrMark.thirdValue = acrMark.thirdValue == 2 ? 0 : acrMark.thirdValue;
                                acrMark.forthValue = acrMark.forthValue == 2 ? 0 : acrMark.forthValue;
                            }

                        }
                        else
                        {
                            acrMark.firstValue = data.firstValue;
                            acrMark.sencondValue = data.sencondValue;
                            acrMark.thirdValue = data.thirdValue;
                            acrMark.forthValue = data.forthValue;
                            acrMark.fifthValue = data.fifthValue;
                        }
                        acrMark.updatedAt = DateTime.Now;
                        acrMark.updatedBy = userName;

                        await acrInfoService.SaveEmployeesAcr(acrMark);
                    }
                    else
                    {
                        acr.Id = 0;
                        acr.firstValue = data.firstValue;
                        acr.sencondValue = data.sencondValue;
                        acr.thirdValue = data.thirdValue;
                        acr.forthValue = data.forthValue;
                        acr.fifthValue = data.fifthValue;
                        acr.createdAt = DateTime.Now;
                        acr.createdBy = userName;

                        await acrInfoService.SaveEmployeesAcr(acr);

                    }

                }
            }

            return Json(true);
        }
        public async Task<IActionResult> SaveClerk2ndPartComment([FromBody]ClerkPart2CommentViewModel model)
        {
            string userName = User.Identity.Name;
            var userInfo = await acrInfoService.GetAcrUserInfo(userName); 
            int? userTypeId = _context.Users.Where(x => x.UserName == userName).FirstOrDefault().userTypeId;
            var data = _context.aCRComments.Where(x => x.assessmentId == model.assessmentId);
            if (data.Count() > 0)
            {
                _context.aCRComments.RemoveRange(_context.aCRComments.Where(x => x.assessmentId == model.assessmentId));
                _context.SaveChanges();
            }
            ACRComments acr = new ACRComments
            {
                assessmentId = model.assessmentId,
                remarks = model.remarks,
                signatoryRemarks = model.signatoryRemarks,
                specificJob = model.specificJob,                
                recommendation = model.recommendation,
                signRecommendation = model.signRecommendation,
                languageUsage = model.languageUsage,
                signlanguageUsage = model.signlanguageUsage,

                isDelete = 0,
                createdAt = DateTime.Now,
                createdBy = HttpContext.User.Identity.Name
            };
            await acrInfoService.SaveACRComments(acr);
            //_context.aCRComments.Add(acr);
            //_context.SaveChanges();

            var dataS = _context.aCRSignatories.Where(x => x.assessmentId == model.assessmentId);
            if (dataS.Count() > 0)
            {
                _context.aCRSignatories.RemoveRange(_context.aCRSignatories.Where(x => x.assessmentId == model.assessmentId));
                _context.SaveChanges();
            }
            ACRSignatories acrS = new ACRSignatories
            {
                assessmentId = model.assessmentId,
                
                additionalComment = model.additionalComment,
                isDelete = 0,
                createdAt = DateTime.Now,
                createdBy = HttpContext.User.Identity.Name
            };
            _context.aCRSignatories.Add(acrS);
            _context.SaveChanges();

            var assessment = _context.assessments.Find(model.assessmentId);
            if (userInfo.EmpCode == assessment.recommendator)
            {
                assessment.recommendationDate = DateTime.Now;
                assessment.statusInfoId = 6;
                assessment.updatedBy = User.Identity.Name;
                assessment.updatedAt = DateTime.Now;
                assessment.signatory = model.signatoryCode;
            }
            else if (userInfo.EmpCode == assessment.signatory)
            {
                assessment.signatoryDate = DateTime.Now;
                assessment.statusInfoId = 5;
                assessment.updatedBy = User.Identity.Name;
                assessment.updatedAt = DateTime.Now;
            }
            else if (userInfo.EmpCode == assessment.empType)
            {
                assessment.signatoryDate = DateTime.Now;
                assessment.statusInfoId = 7;
                assessment.updatedBy = User.Identity.Name;
                assessment.updatedAt = DateTime.Now;
            }
            _context.Entry(assessment).State = EntityState.Modified;
            _context.SaveChanges();


            return Json(true);
        }
        [HttpGet]
        public JsonResult GetACRSignatories(int? id)
        {
            var result = (from C in _context.aCRSignatories
                          where C.assessmentId == id
                          select new ClerkPart2CommentViewModel
                          {
                              additionalComment = C.additionalComment
                          }).ToList();
            //return Json(result.FirstOrDefault(), JsonRequestBehavior.AllowGet);
            return Json(result.FirstOrDefault());
        }
        [HttpGet]
        public JsonResult GetACRComments(int? id)
        {
            var result = (from C in _context.aCRComments
                          where C.assessmentId == id
                          select new ClerkPart2CommentViewModel
                          {
                              remarks = C.remarks,
                              languageUsage = C.languageUsage,
                              specificJob = C.specificJob,
                              recommendation=C.recommendation,
                              signRecommendation=C.signRecommendation

                          }).ToList();
            return Json(result.FirstOrDefault());
            //return Json(result.FirstOrDefault(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetACROtherTables(int? id)
        {
            var result = (from C in _context.aCROtherTables
                          where C.assessmentId == id
                          select new ACROtherTableViewModel
                          {
                              isPhysicallyCapable = C.isPhysicallyCapable,
                              bankingExperience = C.bankingExperience,
                              nobankingExperience = C.nobankingExperience
                          }).ToList();
            return Json(result.FirstOrDefault());
            //return Json(result.FirstOrDefault(), JsonRequestBehavior.AllowGet);
        }

        public async Task<IActionResult> GetUserInfo(string userName)
        {
            var userInfo = await personalInfoService.GetEmployeeInfoByCode(userName);
            //var lstEmp = new List<EmployeeUserViewModel>();
            //lstEmp = ObjectMapHelper<EmployeeUserViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GETUserInformation", new object[] { userName }));
            return Json(userInfo);
        }

        [HttpGet]
        public JsonResult GetClericalEmployeeACRDetailsInfo(int? id, int? acrTypeId)
        {
            var marks = _context.employeesAcrs.Where(x => x.assessmentId == id).Sum(x => (x.firstValue == 2 ? 0 : x.firstValue * 2) + (x.sencondValue == 2 ? 0 : x.sencondValue * 2) + (x.thirdValue == 2 ? 0 : x.thirdValue * 4) + (x.forthValue == 2 ? 0 : x.forthValue * 5));

            //if (marks == null)
            //{
            //    marks = 0;
            //}
            
            var amountInWord = AmountInWord.ConvertWholeNumber(marks.ToString());
            var result = (from e in _context.employeesAcrs
                          join a in _context.acrEvaluationNames on e.acrEvaluationNameId equals a.Id
                          where e.assessmentId == id && a.acrTypeId == acrTypeId
                          select new ACRDetailsInfoViewModel
                          {
                              acrId = e.Id,
                              empCode = e.empCode,
                              acrEvaluationNameId = e.acrEvaluationNameId,
                              firstValue = e.firstValue,
                              sencondValue = e.sencondValue,
                              thirdValue = e.thirdValue,
                              forthValue = e.forthValue,
                              fifthValue = e.fifthValue,
                              marks = marks.ToString(),
                              marksInWord = amountInWord
                          }).ToList();
            return Json(result);
            //return Json(result, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public JsonResult GetEmployeeLeaveInfoByEmpCode(int? assesmentId)
        //{
        //    var lstEmp = new List<EmployeeLeaveInfoViewModel>();
        //    lstEmp = ObjectMapHelper<EmployeeLeaveInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesLeave", new object[] { assesmentId }));
        //    return Json(lstEmp.FirstOrDefault(), JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public async Task<IActionResult> GetUsersProfile(int id)
        {
            //var lstEmp = new List<UserProfileViewModel>();
            //lstEmp = ObjectMapHelper<UserProfileViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GET_UserProfilebyempcode", new object[] { id }));
            //var jsonResult = Json(lstEmp, JsonRequestBehavior.AllowGet);
            //jsonResult.MaxJsonLength = int.MaxValue;
            //return jsonResult;

            var lstUser = await acrInfoService.GetUserProfile(id);
            return Json(lstUser.FirstOrDefault());
        }
        //[HttpGet]
        //public JsonResult GetEmployeeTraining(int? id)
        //{
        //    var lstEmp = new List<EmployeeTrainingViewModel>();
        //    lstEmp = ObjectMapHelper<EmployeeTrainingViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeeTraining", new object[] { id }));
        //    return Json(lstEmp, JsonRequestBehavior.AllowGet);
        //}
        [HttpGet]
        public async Task<IActionResult> GetClericalEmployeeInfo(int id)
        {
            var lstClericalEmp = await acrInfoService.GetEmployeeInfo(id);
            return Json(lstClericalEmp.FirstOrDefault());
        }
    }
}