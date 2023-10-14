using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using ACR.Data.ViewModel.Evaluation;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using OPUSERP.Accounting.Services.MasterData.Interfaces;
using OPUSERP.Areas.HRPMSACR.Models;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Data;
using OPUSERP.Data.Entity;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Data.Entity.ACR;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Services.ACR.Interfaces;
using OPUSERP.HRPMS.Services.Employee;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.Workflow.Data.Entity;

namespace OPUSERP.Areas.HRPMSACR.Controllers
{
	[Area("HRPMSACR")]
	public class ExecutiveOfficerController : Controller
	{
		private readonly IPersonalInfoService personalInfoService;
		private readonly IPhotographService photographService;
		private readonly ISignatureService signatureService;
		private readonly IAcrInfoService acrInfoService;
		private readonly IUserInfoes userInfoes;
		private readonly IEmployeePunchCardInfoService employeePunchCardInfoService;
		private readonly IProfessionalQualificationsService professionalQualificationsService;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ERPDbContext _context;

		public ExecutiveOfficerController(
			IPersonalInfoService personalInfoService,
			IPhotographService photographService,
			ISignatureService signatureService,
			IAcrInfoService acrInfoService,
            IProfessionalQualificationsService professionalQualificationsService,
            IUserInfoes userInfoes, ERPDbContext context,

			IEmployeePunchCardInfoService employeePunchCardInfoService,
			UserManager<ApplicationUser> _userManager)
		{
			this.personalInfoService = personalInfoService;
			this.photographService = photographService;
			this.signatureService = signatureService;
			this.acrInfoService = acrInfoService;
			this.userInfoes = userInfoes;
			this.employeePunchCardInfoService = employeePunchCardInfoService;
			this.professionalQualificationsService = professionalQualificationsService;
			this._userManager = _userManager;
			this._context = context;
		}
		[HttpGet]
		public async Task<IActionResult> GetEmployeeInfoByEmpCode(int? id)
		{
			var assessment = await acrInfoService.GetAssessmentInfoById(Convert.ToInt32(id));
            var empCode = await acrInfoService.GetEmployeeCodeByAssmentId((int)id);
			//var lstEmp = new List<EmployeeInfoViewModel>();
			var model = new ACREvaluationViewModel
			{
                empAcrGrade = await acrInfoService.GetEmployeeGradeBySp(empCode),
				empAcr = await acrInfoService.GetEmployeeACRInfoByAssessmentId(Convert.ToInt32(id)),
				childCount = await acrInfoService.GetChildrenNoByAssId(Convert.ToInt32(id)),
				empSignature = await acrInfoService.GetSignatureByEmpCode(assessment.empCode)
			};
			//lstEmp = ObjectMapHelper<EmployeeInfoViewModel>.MapObject(SqlHelper.ExecuteDataset(_context.ConnectionString, "SP_GetEmployeesByCode", new object[] { id }));
			return Json(model);
		}

		[HttpGet]
		public async Task<IActionResult> GetChildrenNoByAssId(int? id)
		{
			var model = new ACREvaluationViewModel
			{
				childCount = await acrInfoService.GetChildrenNoByAssId(Convert.ToInt32(id))
			};
			return Json(model);
		}

		[HttpPost]
		public async Task<ActionResult> UpdateExecutiveOfficer2thPart(int assementId, string durationInRecommendetor, string goalAchievement, string assessmentDate, string drawerCode)
		{
			try
			{
				string userName = User.Identity.Name;
				
				var data = await acrInfoService.GetAssessmentInfoById(assementId);

				data.durationInRecommendetor = durationInRecommendetor;
				data.goalAchievement = goalAchievement;
				data.assessmentDate = Convert.ToDateTime(assessmentDate);
				data.updatedBy = User.Identity.Name;
				data.updatedAt = DateTime.Now;
                data.newSignatory = "1";
                if (drawerCode != "" && drawerCode != null)
				{
					var userDetails = await personalInfoService.GetEmployeeInfoByCode(userName);

					data.assignToRecomDate = DateTime.Now;
					data.recommendator = drawerCode;
					data.statusInfoId = 4;
					await acrInfoService.SendNotification(userName, userName, drawerCode, "ACR for " + userDetails.nameEnglish, "", "ACR", "/HRPMSACR/AssessmentInfo/Authority");
				}
				else
				{
					var userDetails = await personalInfoService.GetEmployeeInfoByCode(userName);

					data.statusInfoId = 1;
					data.routeOrder = 1;
					await acrInfoService.SendNotification(userName, userName, "2503", "ACR for " + userDetails.nameEnglish, "", "ACR", "/HRPMSACR/AssessmentInfo/ACRPendingListForHRAdmin");
				}

				await acrInfoService.SaveAssessment(data);

				var userInfo = await userInfoes.GetUserInfoByUserName(userName);
				//if (userInfo.userTypeId == 4)
				//{
				//	userInfo.IsActive = 0;
				//}

				//await UserManager.UpdateAsync(userInfo);
				//_context.SaveChanges();

				return Json(true);
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}


		public async Task<IActionResult> GetEmpByEmpCode(string empcode)
		{
			var data = await acrInfoService.GetEmployeeByEmpCode(empcode);
			return Json(data);
		}
		public async Task<IActionResult> Index(int? id, string empCode)
		{
            try
            {
                string userName = HttpContext.User.Identity.Name;
                var user = await _userManager.FindByNameAsync(userName);
                var userInfos = await userInfoes.GetUserInfoByUser(userName);
                ViewBag.UserName = userName;
                var emp = await personalInfoService.GetEmployeeInfoByCode(empCode);
                var checkRole = await personalInfoService.CheckUserIsHRAdmin(userName);
                var lstEmp = await acrInfoService.AssessmentInfoById(empCode, Convert.ToInt32(id));
                var assBasic = lstEmp.FirstOrDefault();
                ViewBag.UserTypeId = user.userTypeId;
                ViewBag.UserEmpCode = userInfos.EmpCode;

                var userInfo = user;
                var headId = await acrInfoService.GetHeadIdByAssId(Convert.ToInt32(id));

                var userinfovm = new EmployeeUserViewModel
                {
                    EmpId = emp.Id,
                    EmpCode = empCode,
                    EmpName = emp.nameEnglish,
                    BranchCode = emp.branch?.branchCode,
                    BranchName = emp.branch?.branchUnitName,
                    DesignationName = emp.lastDesignation?.designationName,
                    DesiCode = emp.DesiCode,
                    //DesiCat = emp.desiCat,
                    UserTypeId = user.userTypeId,
                    ActionDate = DateTime.Now.ToString(),
                    signature = await personalInfoService.GetSignatureByEmpId(emp.Id),
                    JoiningDate = emp.joiningDateGovtService.ToString()
                };


                ACREvaluationViewModel model = new ACREvaluationViewModel
                {
                    assessmentId = id,
                    empCode = empCode,
                    isHRAdmin = checkRole.isHRAdmin,
                    AssessmentBasic = assBasic,
                    ACRUsers = userinfovm,
                    headId = headId,
                    AcrEvaluationList = await acrInfoService.AcrEvaluationNameByacrForId(1),
                    //aCRIndicators=_context.ACRIndicators.ToList(),
                    heads = await acrInfoService.GetAllQuantitativeEvaluationHeads(),
                    commentsHead = await acrInfoService.GetAllEvaluationCommentsHeads(),
                    acrTotal = await acrInfoService.GetAcrTotalByAssessmentId(id),
                    empPostings = await acrInfoService.GetEmpPostings(Convert.ToInt32(userinfovm.EmpId), (int)id),
                };
                ViewBag.assessmentId = id;


                //recommendator signatory Signatory2
                //Asad Added On 019.09.2023
                if (assBasic != null)
                {
                    if (userName == assBasic.recommendator)
                    {
                        ViewBag.Evaluator = "Recomendator";
                    }
                    else if (userName == assBasic.signatory)
                    {
                        ViewBag.Evaluator = "Signatory";
                    }
                    else if (userName == assBasic.empType)
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
            catch (Exception ex)
            {

                throw ex;
            }
			
		}

        public async Task<IActionResult> AcrPreviewExecutiveOfficer(int? id, string empCode)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                var user = await _userManager.FindByNameAsync(userName);
                var userInfos = await userInfoes.GetUserInfoByUser(userName);
                ViewBag.UserName = userName;
                var emp = await personalInfoService.GetEmployeeInfoByCode(empCode);
                var checkRole = await personalInfoService.CheckUserIsHRAdmin(userName);
                var lstEmp = await acrInfoService.AssessmentInfoById(empCode, Convert.ToInt32(id));
                var assBasic = lstEmp.FirstOrDefault();
                ViewBag.UserTypeId = user.userTypeId;
                ViewBag.UserEmpCode = userInfos.EmpCode;

                var userInfo = user;
                var headId = await acrInfoService.GetHeadIdByAssId(Convert.ToInt32(id));

                var userinfovm = new EmployeeUserViewModel
                {
                    EmpId = emp.Id,
                    EmpCode = empCode,
                    EmpName = emp.nameEnglish,
                    BranchCode = emp.branch?.branchCode,
                    BranchName = emp.branch?.branchUnitName,
                    DesignationName = emp.lastDesignation?.designationName,
                    DesiCode = emp.DesiCode,
                    //DesiCat = emp.desiCat,
                    UserTypeId = user.userTypeId,
                    ActionDate = DateTime.Now.ToString(),
                    signature = await personalInfoService.GetSignatureByEmpId(emp.Id),
                    JoiningDate = emp.joiningDateGovtService.ToString()
				};
				var doc=await GetTargetAchiveDocCertification(id);
				if (doc == null)
				{
					doc = new List<DocumentAttachments>();
				}
                ACREvaluationViewModel model = new ACREvaluationViewModel
				{
					assessmentId = id,
					empCode = empCode,
					isHRAdmin = checkRole.isHRAdmin,
					AssessmentBasic = assBasic,
					ACRUsers = userinfovm,
					headId = headId,
					AcrEvaluationList = await acrInfoService.AcrEvaluationNameByacrForId(1),
					//aCRIndicators=_context.ACRIndicators.ToList(),
					heads = await acrInfoService.GetAllQuantitativeEvaluationHeads(),
					commentsHead = await acrInfoService.GetAllEvaluationCommentsHeads(),
					acrTotal = await acrInfoService.GetAcrTotalByAssessmentId(id),
					empPostings = await acrInfoService.GetEmpPostings(Convert.ToInt32(userinfovm.EmpId), (int)id),
					empEducations = await acrInfoService.GetEmpEducations(Convert.ToInt32(userinfovm.EmpId)),
					professionalQualifications = await professionalQualificationsService.GetProfessionalQualificationsByEmpId(Convert.ToInt32(userinfovm.EmpId)),
                    documentAttachments = doc
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
            catch (Exception ex)
            {

                throw ex;
            }

        }

        //
        [HttpGet]
        public async Task<IActionResult> GetEmployeePostingInfo(string empCode,int? id)
        {
            int? empId = await personalInfoService.GetEmployeeIdByCode(empCode);
            var model = new ACREvaluationViewModel
            {
                empPostings = await acrInfoService.GetEmpPostings((int)empId, (int)id)
            };
            return Json(model);
        }

        public async Task<IActionResult> SaveExecutiveOfficerInfo([FromBody]ACREvaluationViewModel model)
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
							if (acrMark != null)
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

                        }
                        else
                        {
							if (acrMark != null)
							{
								acrMark.firstValue = data.firstValue;
								acrMark.sencondValue = data.sencondValue;
								acrMark.thirdValue = data.thirdValue;
								acrMark.forthValue = data.forthValue;
								acrMark.fifthValue = data.fifthValue == null ? 0 : data.fifthValue;
							}
                        }

						if (acrMark != null)
						{
							acrMark.updatedAt = DateTime.Now;
							acrMark.updatedBy = userName;

							//encryption
							acrMark.firstValue = acrInfoService.EncryptACRNumber((int)model.assessmentId, model.empCode, (int)acrMark.firstValue);
							acrMark.sencondValue = acrInfoService.EncryptACRNumber((int)model.assessmentId, model.empCode, (int)acrMark.sencondValue);
							acrMark.thirdValue = acrInfoService.EncryptACRNumber((int)model.assessmentId, model.empCode, (int)acrMark.thirdValue);
							acrMark.forthValue = acrInfoService.EncryptACRNumber((int)model.assessmentId, model.empCode, (int)acrMark.forthValue);
							acrMark.fifthValue = acrInfoService.EncryptACRNumber((int)model.assessmentId, model.empCode, (int)acrMark.fifthValue);

							await acrInfoService.SaveEmployeesAcr(acrMark);
						}
                    }
                    else
                    {
                       

                        acr.Id = 0;
                        acr.firstValue = data.firstValue;
                        acr.sencondValue = data.sencondValue;
                        acr.thirdValue = data.thirdValue;
                        acr.forthValue = data.forthValue;
                        acr.fifthValue = data.fifthValue ==  null? 0: data.fifthValue;
                        acr.createdAt = DateTime.Now;
                        acr.createdBy = userName;

                        //encryption
                        acr.firstValue = acrInfoService.EncryptACRNumber((int)model.assessmentId, model.empCode, (int)acr.firstValue);
                        acr.sencondValue = acrInfoService.EncryptACRNumber((int)model.assessmentId, model.empCode, (int)acr.sencondValue);
                        acr.thirdValue = acrInfoService.EncryptACRNumber((int)model.assessmentId, model.empCode, (int)acr.thirdValue);
                        acr.forthValue = acrInfoService.EncryptACRNumber((int)model.assessmentId, model.empCode, (int)acr.forthValue);
                        acr.fifthValue = acrInfoService.EncryptACRNumber((int)model.assessmentId, model.empCode, (int)acr.fifthValue);

                        await acrInfoService.SaveEmployeesAcr(acr);
                    }
                }

             
                //    if (userName == assBasic.recommendator)
                //{
                //    var userDetails = await personalInfoService.GetEmployeeInfoByCode(assBasic.empCode);
                //    await acrInfoService.SendNotification(userName, userName, assBasic.signatory, "ACR From " + userDetails.nameEnglish, "Waiting for ACR to Signatory", "ACR", "/HRPMSACR/AssessmentInfo/Authority");
                //    await acrInfoService.SendNotification(userName, userName, "2503", userDetails.nameEnglish + "'s ACR has been waiting for signatory.", "", "ACR", "/HRPMSACR/AssessmentInfo/ACRPendingListForHRAdmin");

                //}

               
                // if (userName == assBasic.empType)
                //{
                //    var userDetails = await personalInfoService.GetEmployeeInfoByCode(assBasic.empCode);
                //    await acrInfoService.SendNotification(userName, userName, "2503", userDetails.nameEnglish + "'s ACR has been Completed.", "", "ACR", "/HRPMSACR/AssessmentInfo/AcrCompletedList");
                //    await acrInfoService.SendNotification(userName, userName, "2064", userDetails.nameEnglish + "'s ACR has been Completed.", "", "ACR", "/HRPMSACR/AssessmentInfo/AcrCompletedList");

                //}

            }
            return Json(true);
        }

		[HttpGet]
		public JsonResult GetEmployeeACRDetailsInfo(int? id, int? acrTypeId)
		{
			var assinfo = _context.assessments.Where(x => x.Id == id).FirstOrDefault();
           
			var marks = _context.employeesAcrs.Where(x => x.assessmentId == id).Sum(x => ((x.firstValue == 2 ? 0 : x.firstValue) + (x.sencondValue == 2 ? 0 : x.sencondValue * 2) + (x.thirdValue == 2 ? 0 : x.thirdValue * 3) + (x.forthValue == 2 ? 0 : x.forthValue * 4) + (x.fifthValue == 2 ? 0 : (x.fifthValue == null ? 0 : x.fifthValue) * 5)));
           
                //var markss = (int)assinfo.assessmentValue;
                //var amountInWord = AmountInWord.ConvertWholeNumber(markss.ToString());
            
           
           
            var amountInWord = AmountInWord.ConvertWholeNumber(marks.ToString());
			var result = (from e in _context.employeesAcrs
						  join a in _context.acrEvaluationNames on e.acrEvaluationNameId equals a.Id
						  where e.assessmentId == id && a.acrTypeId == acrTypeId
						  select new ACRDetailsInfoViewModel
						  {
							  acrId = e.Id,
							  empCode = e.empCode,
							  acrEvaluationNameId = e.acrEvaluationNameId,
							  firstValue = acrInfoService.DecryptACRNumber((int)id, assinfo.empCode, (int)e.firstValue),
							  sencondValue = acrInfoService.DecryptACRNumber((int)id, assinfo.empCode, (int)e.sencondValue),
							  thirdValue = acrInfoService.DecryptACRNumber((int)id, assinfo.empCode, (int)e.thirdValue),
							  forthValue = acrInfoService.DecryptACRNumber((int)id, assinfo.empCode, (int)e.forthValue),
							  fifthValue = acrInfoService.DecryptACRNumber((int)id, assinfo.empCode, (int)e.fifthValue),
							  marks = marks.ToString(),
							  marksInWord = amountInWord,
                              //markss = markss

                          }).ToList();
			return Json(result);
			//return Json(result, JsonRequestBehavior.AllowGet);
		}


        [HttpGet]
		public JsonResult GetACRIndicatorByHeadId(int headId, int assessmentId)
		{

			var isExist = _context.quantitativeEvaluations.Where(x => x.assessmentId == assessmentId && x.aCRIndicator.indicatorName != "6").Include(x => x.aCRIndicator).FirstOrDefault()?.aCRIndicator?.headId;
			List<ACRQuantitativeEvaluationViewModel> records = new List<ACRQuantitativeEvaluationViewModel>();
			if (isExist > 0)
			{
				records = (from i in _context.aCRIndicators
						   join h in _context.quantitativeEvaluationHeads on i.headId equals h.Id
						   join q in _context.quantitativeEvaluations on i.Id equals q.aCRIndicatorId into que
						   from qe in que.DefaultIfEmpty()
						   join a in _context.assessments on qe.assessmentId equals a.Id into aa
						   from ass in aa.DefaultIfEmpty()
						   where h.Id == headId && ass.Id == assessmentId
						   select new ACRQuantitativeEvaluationViewModel
						   {
							   indicatorId = i.Id,
							   indicatorNameBn = i.indicatorNameBn,
							   headId = h.Id,
							   evaluationId = qe.Id,
							   target = qe.target == null ? 0 : qe.target,
							   achievement = qe.achievement == null ? 0 : qe.achievement,
							   achievementPercent = qe.achievementPercent == null ? 0 : qe.achievementPercent,
							   achievementSign = qe.achievementSign == null ? 0 : qe.achievementSign,
							   achievementPercentSign = qe.achievementPercentSign == null ? 0 : qe.achievementPercentSign,
							   acrCounter = qe.acrCounter,
							   assessmentId = ass.Id
						   }).ToList();
			}
			else
			{
				records = (from i in _context.aCRIndicators
						   join h in _context.quantitativeEvaluationHeads on i.headId equals h.Id
						   where h.Id == headId
						   select new ACRQuantitativeEvaluationViewModel
						   {
							   indicatorId = i.Id,
							   indicatorNameBn = i.indicatorNameBn,
							   headId = h.Id,
							   evaluationId = 0,
							   target = 5,
							   achievement = 0,
							   achievementPercent = 0,
							   achievementSign = 0,
							   achievementPercentSign = 0,
							   acrCounter = 100000,
							   assessmentId = 0
						   }).ToList();
			}

			return Json(records);
			//return Json(records, JsonRequestBehavior.AllowGet);
		}

		#region 5th Part , 6th Part
		[HttpPost]
		public async Task<JsonResult> SaveExecutiveOfficer5thPart(ExecutiveOfficerPart5ViewModel model)
		{
            string userName = User.Identity.Name;
            var userInfo = await userInfoes.GetUserInfoByUserNameNew(userName);
            var data = _context.aCRComments.Where(x => x.assessmentId == model.assessmentId5thPart);
            var assessment = _context.assessments.Find(model.assessmentId5thPart);

            if (data.Count() > 0)
			{
				_context.aCRComments.RemoveRange(_context.aCRComments.Where(x => x.assessmentId == model.assessmentId5thPart));
				_context.SaveChanges();
			}
			ACRComments acr = new ACRComments
			{
				assessmentId = model.assessmentId5thPart,
				remarks = model.specialRemarks,
				isDelete = 0,
				createdAt = DateTime.Now,
				createdBy = HttpContext.User.Identity.Name
			};
			_context.aCRComments.Add(acr);
			_context.SaveChanges();

            if (userInfo.EmpCode == assessment.recommendator)
            {
                var userDetails = await personalInfoService.GetEmployeeInfoByCode(userName);
                await acrInfoService.SendNotification(userName, userName, assessment.signatory, "ACR From " + userDetails.nameEnglish, "Waiting for ACR to Signatory", "ACR", "/HRPMSACR/AssessmentInfo/Authority");
                await acrInfoService.SendNotification(userName, userName, "2503", userDetails.nameEnglish + "'s ACR has been waiting for signatory.", "", "ACR", "/HRPMSACR/AssessmentInfo/ACRPendingListForHRAdmin");
                await acrInfoService.SendNotification(userName, userName, "2064", userDetails.nameEnglish + "'s ACR has been waiting for signatory.", "", "ACR", "/HRPMSACR/AssessmentInfo/ACRPendingListForHRAdmin");
            }


            //var assessment = _context.Assessments.Find(model.assessmentId5thPart);
            //assessment.statusInfoId = 2;
            //assessment.updatedBy = User.Identity.Name;
            //assessment.updatedAt = DateTime.Now;
            //_context.Entry(assessment).State = EntityState.Modified;
            //_context.SaveChanges();


            return Json(true);

		}

		[HttpPost]
		public JsonResult SaveExecutiveOfficer6thPart(ExecutiveOfficerPart6ViewModel model)
		{

			var data = _context.aCRRecommendations.Where(x => x.assessmentId == model.assessmentId6thPart);
			if (data.Count() > 0)
			{
				_context.aCRRecommendations.RemoveRange(_context.aCRRecommendations.Where(x => x.assessmentId == model.assessmentId6thPart));
				_context.SaveChanges();
			}
			ACRRecommendations acr = new ACRRecommendations
			{
				assessmentId = model.assessmentId6thPart,
				specialComment = model.specialComment6,
				moralComment = model.moralComment6,
				intellectualComment = model.intellectualComment6,
				subjectiveComment = model.subjectiveComment6,
				trainingComment = model.trainingComment6,
				promotionalQualification = model.promotionalQualification,
				//Asad Added On 08.10.2023
				//promotionalQualificationSignatory = model.promotionalQualificationSignatory,

				otherQualification = model.othersRecommends6,
				isDelete = 0,
				createdAt = DateTime.Now,
				createdBy = HttpContext.User.Identity.Name,


			};
			_context.aCRRecommendations.Add(acr);
			_context.SaveChanges();

			//var assessment = _context.Assessments.Find(model.assessmentId6thPart);
			//assessment.recommendationDate = model.reportDate;
			//assessment.statusInfoId = 2;
			//assessment.updatedBy = User.Identity.Name;
			//assessment.updatedAt = DateTime.Now;
			//_context.Entry(assessment).State = EntityState.Modified;
			//_context.SaveChanges();


			return Json(true);

		}

		[HttpPost]
		public async Task<JsonResult> SaveQuantitativeEvaluation([FromForm] QuantitativeEvaluationViewModel model)
		{
			try
			{
				string userName = HttpContext.User.Identity.Name;
				var user = await userInfoes.GetUserInfoByUserNameNew(userName);
				string userEmpCode = user.EmpCode;
				//var assBasic = await acrInfoService.GetAssessmentInfoById(Convert.ToInt32(model.qaassessmentId));
				var assBasic = await acrInfoService.GetAssessmentBasicInfo(Convert.ToInt32(model.qaassessmentId));
				var recommendator = assBasic.recommendator;
				var signatory = assBasic.signatory;
				//var assBasic = GetAssessmentBasicInfo(model.qaassessmentId);

				for (int i = 0; i < model.aCRIndicatorId.Length; i++)
				{
					var data =await _context.quantitativeEvaluations.SingleOrDefaultAsync(e => e.assessmentId == model.qaassessmentId && e.aCRIndicatorId == model.aCRIndicatorId[i]);
					if (data !=null)
					{
						if (userEmpCode == recommendator)
						{
							data.target = model.target[i];
							data.achievement = model.achievement[i];
							data.achievementPercent = model.achievementPercent[i];
						}
						else if (userEmpCode == signatory)
						{
							if (data.target == 0 || data.target == null)
								data.target = model.target[i];
							data.achievementSign = model.achievementSign[i];
							data.achievementPercentSign = model.achievementPercentSign[i];
						}

						data.acrCounter = model.acrCounter;

						if (model.PerformanceCategory == 8)
						{
							data.posting = "branch";
						}
						else if (model.PerformanceCategory == 9)
						{
							data.posting = "zone";
						}
						else if (model.PerformanceCategory == 10)
						{
							data.posting = "head office";
						}
						else
						{

						}

						//encrypt
						data.target = acrInfoService.EncryptACRNumber((int)assBasic.assessmentId, assBasic.empCode, Convert.ToInt32(data.target));
						data.achievement = acrInfoService.EncryptACRNumber((int)assBasic.assessmentId, assBasic.empCode, Convert.ToInt32(data.achievement));
						data.achievementPercent = acrInfoService.EncryptACRNumber((int)assBasic.assessmentId, assBasic.empCode, Convert.ToInt32(data.achievementPercent));

						data.updatedBy = userName;
						data.updatedAt = DateTime.Now;
						_context.Entry(data).State = EntityState.Modified;
						_context.SaveChanges();
					}
					else
					{
                        QuantitativeEvaluation qa = new QuantitativeEvaluation();
                        qa.Id = 0;
						qa.assessmentId = model.qaassessmentId;
						qa.aCRIndicatorId = model.aCRIndicatorId[i];
						qa.target = model.target[i];

						if (model.PerformanceCategory == 8)
						{
							qa.posting = "branch";
						}
						else if (model.PerformanceCategory == 9)
						{
							qa.posting = "zone";
						}
						else if (model.PerformanceCategory == 10)
						{
							qa.posting = "head office";
						}
						else
						{

						}


						qa.acrCounter = model.acrCounter;


						if (userEmpCode == recommendator)
						{
							qa.achievement = model.achievement[i];
							qa.achievementPercent = model.achievementPercent[i];
						}
						else if (userEmpCode == signatory)
						{
							qa.achievementSign = model.achievementSign[i];
							qa.achievementPercentSign = model.achievementPercentSign[i];
						}
						qa.createdBy = userName;
						qa.createdAt = DateTime.Now;

						//encrypt
						qa.target = acrInfoService.EncryptACRNumber((int)assBasic.assessmentId, assBasic.empCode, Convert.ToInt32(qa.target));
						qa.achievement = acrInfoService.EncryptACRNumber((int)assBasic.assessmentId, assBasic.empCode, Convert.ToInt32(qa.achievement));
						qa.achievementPercent = acrInfoService.EncryptACRNumber((int)assBasic.assessmentId, assBasic.empCode, Convert.ToInt32(qa.achievementPercent));

						_context.quantitativeEvaluations.Add(qa);
						_context.SaveChanges();
					}

				}

				if (model.headId == 7)
				{
					var assesmentIdExists = _context.quantitativeEvaluationComments.Where(x => x.assessmentId == model.qaassessmentId);
					if (assesmentIdExists.Count() > 0)
					{
						_context.quantitativeEvaluationComments.RemoveRange(_context.quantitativeEvaluationComments.Where(x => x.assessmentId == model.qaassessmentId));
						_context.SaveChanges();
					}

					QuantitativeEvaluationComments commnets = new QuantitativeEvaluationComments
					{
						assessmentId = model.qaassessmentId,
						//evaluationComments = model.evaluationComments,
						evaluationCommentsHeadId = model.evaluationCommentsHeadId,
						isDelete = 0,
						createdAt = DateTime.Now,
						createdBy = HttpContext.User.Identity.Name
					};
					_context.quantitativeEvaluationComments.Add(commnets);
					_context.SaveChanges();
				}


				return Json(true);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		//[HttpGet]
		//public async Task<IActionResult> GetQuantativeEvaluation(int? id, int? headId)
		//{
		//    var model = new ACREvaluationViewModel
		//    {
		//        aCRQuantitativeEvaluationViewModels = await acrInfoService.GetQuantativeEvaluation(id, headId)
		//    };
		//    return Json(model);
		//}

		//[HttpGet]
		//public async Task<IEnumerable<ACRQuantitativeEvaluationViewModel>> GetACRIndicatorByAssmentId(int id, int? headId)
		//{
		//    var records = (from i in _context.aCRIndicators
		//                   join h in _context.quantitativeEvaluationHeads on i.headId equals h.Id
		//                   join q in _context.quantitativeEvaluations on i.Id equals q.aCRIndicatorId into que
		//                   from qe in que.DefaultIfEmpty()
		//                   join a in _context.assessments on qe.assessmentId equals a.Id into aa
		//                   from ass in aa.DefaultIfEmpty()
		//                   where ass.Id == id && h.Id == headId && h.Id != 6
		//                   select new ACRQuantitativeEvaluationViewModel
		//                   {
		//                       indicatorId = i.Id,
		//                       indicatorNameBn = i.indicatorNameBn,
		//                       headId = h.Id,
		//                       evaluationId = qe.Id,
		//                       target = qe.target == null ? 0 : qe.target,
		//                       achievement = qe.achievement == null ? 0 : qe.achievement,
		//                       achievementPercent = qe.achievementPercent == null ? 0 : qe.achievementPercent,
		//                       achievementSign = qe.achievementSign == null ? 0 : qe.achievementSign,
		//                       achievementPercentSign = qe.achievementPercentSign == null ? 0 : qe.achievementPercentSign,
		//                       acrCounter = qe.acrCounter,
		//                       assessmentId = ass.Id
		//                   }).ToList();

		//    //var result = _context.ACRIndicators.Where(x=>x.headId==headId).ToList();
		//    //return Json(records, JsonRequestBehavior.AllowGet);
		//    return records;
		//}

		[HttpGet]
		public async Task<IActionResult> GetUserInfo(string userName)
		{
			var lstUser = await acrInfoService.GetUserInfo(userName);
			return Json(lstUser);
		}
		[HttpGet]
		internal async Task<IActionResult> GetAssessmentBasicInfo(int? assId)
		{
			var lstAssessmentInfo = await acrInfoService.GetAssessmentBasicInfo(assId);
			return Json(lstAssessmentInfo);
		}

		[HttpGet]
		public async Task<ActionResult> UpdateRecommendator(int assesId, string recomId)
		{
			try
			{
                string userName = HttpContext.User.Identity.Name;
                var user =await userInfoes.GetUserInfoByUserNameNew(userName);
                var data = _context.assessments.Find(assesId);

                //var assBasicInfo = acrInfoService.GetAssessmentInfoById(data.Id);
                if (data.acrType == "Yearly")
                {
                    data.statusInfoId = 5;
                }
                else
                {
                    data.statusInfoId = 6;
                }
                data.routeOrder = 2;
				data.recommendationDate = DateTime.Now;
				data.updatedBy = User.Identity.Name;
				data.updatedAt = DateTime.Now;
				_context.Entry(data).State = EntityState.Modified;

				_context.SaveChanges();

              
                if (userName == data.signatory)
                {
                     var userDetails =await personalInfoService.GetEmployeeInfoByCode(data.empCode);
                    await acrInfoService.SendNotification(userName, userName, data.empType, "ACR From " + userDetails.nameEnglish, "Waiting for ACR to Approved", "ACR", "/HRPMSACR/AssessmentInfo/Authority");
                    await acrInfoService.SendNotification(userName, userName, "2503", userDetails.nameEnglish + "'s ACR is on MD hand.", "", "ACR", "/HRPMSACR/AssessmentInfo/ACRPendingListForHRAdmin");

                }
                return Json(assesId);
				//return Json(assesId, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[HttpPost]
		public JsonResult SaveExecutiveOfficer7thPartForMD(ExecutiveOfficerPart7ViewModel model)
		{
			var data = _context.aCRSignatories.Where(x => x.assessmentId == model.assessmentId7thPart);
			if (data.Count() > 0)
			{
				_context.aCRSignatories.RemoveRange(_context.aCRSignatories.Where(x => x.assessmentId == model.assessmentId7thPart));
				_context.SaveChanges();
			}
			ACRSignatories acr = new ACRSignatories
			{
				assessmentId = model.assessmentId7thPart,
				evaluationType = model.evaluationType7,
				disAgreementComment = model.disAgreementComment7,
				isDelete = 0,
				createdAt = DateTime.Now,
				createdBy = HttpContext.User.Identity.Name
			};
			_context.aCRSignatories.Add(acr);
			_context.SaveChanges();


			return Json(true);

		}

		[HttpPost]
		public async Task<JsonResult> SaveExecutiveOfficer7thPart(ExecutiveOfficerPart7ViewModel model)
		{
            string userName = HttpContext.User.Identity.Name;
            var user = await userInfoes.GetUserInfoByUserNameNew(userName);

            var data = _context.aCRSignatories.Where(x => x.assessmentId == model.assessmentId7thPart);
			if (data.Count() > 0)
			{
				_context.aCRSignatories.RemoveRange(_context.aCRSignatories.Where(x => x.assessmentId == model.assessmentId7thPart));
				_context.SaveChanges();
			}
			ACRSignatories acr = new ACRSignatories
			{
				assessmentId = model.assessmentId7thPart,
				evaluationType = model.evaluationType7,
				disAgreementComment = model.disAgreementComment7,
				isDelete = 0,
				createdAt = DateTime.Now,
				createdBy = HttpContext.User.Identity.Name
			};
			_context.aCRSignatories.Add(acr);
			_context.SaveChanges();

			var assessment = _context.assessments.Find(model.assessmentId7thPart);

			assessment.signatoryDate = model.signDate;
            if (assessment.acrType == "Yearly")
            {
                assessment.statusInfoId = 7;
            }
            else
            {
                if (assessment.empType == HttpContext.User.Identity.Name && assessment.statusInfoId == 5)
                {
                    assessment.statusInfoId = 7;
                }
                else if(assessment.statusInfoId == 6)
                {
                    assessment.statusInfoId = 5;
                }
            }
			assessment.assessmentValue = model.totalMarks;
			assessment.updatedBy = User.Identity.Name;
			assessment.updatedAt = DateTime.Now;
			_context.Entry(assessment).State = EntityState.Modified;

			_context.SaveChanges();

            if (userName == assessment.signatory)
            {
                var userDetails = await personalInfoService.GetEmployeeInfoByCode(assessment.empCode);
                await acrInfoService.SendNotification(userName, userName, assessment.empType, "ACR From " + userDetails.nameEnglish, "Waiting for ACR to Approved", "ACR", "/HRPMSACR/AssessmentInfo/Authority");
                await acrInfoService.SendNotification(userName, userName, "2503", userDetails.nameEnglish + "'s ACR is on MD hand.", "", "ACR", "/HRPMSACR/AssessmentInfo/ACRPendingListForHRAdmin");

            }

            return Json(true);

		}
		#endregion

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
		public JsonResult GetACRComments(int? id)
		{
			ExecutiveOfficerPart5ViewModel result = (from C in _context.aCRComments
													 where C.assessmentId == id
													 select new ExecutiveOfficerPart5ViewModel
													 {
														 specialRemarks = C.remarks
													 }).FirstOrDefault();

			return Json(result);
		}
		[HttpGet]
		public JsonResult GetACRRecommendations(int? id)
		{
			ExecutiveOfficerPart6ViewModel result = (from C in _context.aCRRecommendations
													 where C.assessmentId == id
													 select new ExecutiveOfficerPart6ViewModel
													 {
														 specialComment6 = C.specialComment,
														 moralComment6 = C.moralComment,
														 intellectualComment6 = C.intellectualComment,
														 subjectiveComment6 = C.subjectiveComment,
														 trainingComment6 = C.trainingComment,
														 promotionalQualification = C.promotionalQualification,
														 othersRecommends6 = C.otherQualification,
														 moralComment6Signatory = C.moralCommentSignatory,
														 intellectualComment6Signatory = C.intellectualCommentSignatory,
														 subjectiveComment6Signatory = C.subjectiveCommentSignatory,
														 promotionalQualificationSignatory = C.promotionalQualificationSignatory
													 }).FirstOrDefault();
			return Json(result);
		}
		[HttpGet]
		public JsonResult GetACRSignatories(int? id)
		{
			ExecutiveOfficerPart7ViewModel result = (from C in _context.aCRSignatories
													 where C.assessmentId == id
													 select new ExecutiveOfficerPart7ViewModel
													 {
														 evaluationType7 = C.evaluationType,
														 disAgreementComment7 = C.disAgreementComment

													 }).FirstOrDefault();
			return Json(result);
		}
		[HttpGet]
		public JsonResult GetAmountInWord(string number)
		{

			var result = AmountInWord.ConvertWholeNumber(number);
			return Json(result);
		}
		//[HttpPost]
		//public ActionResult UploadTargetAchiveDocumentCertification()
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
		//                fname = "CERT_" + assId + "_" + testfiles[testfiles.Length - 1];
		//            }
		//            else
		//            {
		//                fname = "CERT_" + assId + "_" + file.FileName;
		//            }

		//            string filname = Path.Combine(Server.MapPath("~/Assets/Upload/Document"), fname);
		//            var existDoc = _context.DocumentAttachments.Where(x => x.assesmentId == assId && x.attachFor == "Certification").FirstOrDefault();
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
		//                    attachFor = "Certification",
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

		[HttpPost]
		public async Task<JsonResult> UploadTargetAchiveDocument(string empCode, int assesmentId)
		{
			var files = Request.Form.Files;
			//string ID = files.FirstOrDefault().;
			//int? assId = Convert.ToInt32(Request.Form[1]);
			if (Request.Form.Files.Count > 0)
			{
				for (int i = 0; i < Request.Form.Files.Count; i++)
				{
					int _min = 10000;
					int _max = 99999;
					Random _rdm = new Random();
					int rnd = _rdm.Next(_min, _max);

					string filePath = string.Empty;
					string fileName = string.Empty;
					string fileType = string.Empty;

					IFormFile file = Request.Form.Files[i];
					fileType = file.ContentType;
					fileName = rnd + file.FileName;
                    filePath = "wwwroot/Upload/ACR/" + fileName;
                    var fileD = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    using (var fileSrteam = new FileStream(fileD, FileMode.Create))
					{
						await file.CopyToAsync(fileSrteam);
					}
					var existDoc = _context.docAttachment.Where(x => x.assesmentId == assesmentId && x.attachFor == "Certification").FirstOrDefault();
					if (existDoc != null)
					{
						string existFilname = Path.Combine(Directory.GetCurrentDirectory(), existDoc.attachmentpath);
						if (System.IO.File.Exists(existFilname))
						{
							System.IO.File.Delete(existFilname);
						}
						existDoc.attachmentpath = fileName;
						existDoc.updatedBy = User.Identity.Name;
						existDoc.updatedAt = DateTime.Now;
						_context.Entry(existDoc).State = EntityState.Modified;

						_context.SaveChanges();
					}
					else
					{
                        DocumentAttachments model = new DocumentAttachments
                        {
                            assesmentId = Convert.ToInt32(assesmentId),
                            empCode = empCode,
                            attachFor = "Certification",
                            attachmentpath = fileName,
                            createdAt = DateTime.Now,
                            createdBy = User.Identity.Name
                        };
						_context.docAttachment.Add(model);
						_context.SaveChanges();
					}
				}
			}
			return Json(true);
		}

        [HttpGet]
        public async Task<IActionResult> GetTargetAchiveDocsCertification(int? assesId)
        {
            var data = await _context.docAttachment.Where(x => x.assesmentId == assesId).FirstOrDefaultAsync();
            return Json(data);
        }
        [HttpGet]
        public async Task<IEnumerable<DocumentAttachments>> GetTargetAchiveDocCertification(int? assesId)
        {
            var data = await _context.docAttachment.Where(x => x.assesmentId == assesId && x.attachFor == "Certification").ToListAsync();
            return data;
        }
    }
}