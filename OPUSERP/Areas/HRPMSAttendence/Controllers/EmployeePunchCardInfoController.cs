using OPUSERP.Areas.HRPMSAttendence.Models;
using OPUSERP.Areas.HRPMSAttendence.Models.Lang;
using OPUSERP.Areas.HRPMSMasterData.Models;
using OPUSERP.HRPMS.Data.Entity.Attendance;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.Helpers;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using OPUSERP.HRPMS.Data.Entity.Wages;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using System.Collections.Generic;
using System;
using OPUSERP.Data.Entity;
using Microsoft.AspNetCore.Identity;
using OfficeOpenXml;
using System.IO;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.ERPServices.MasterData.Interfaces;
using System.Linq;
using OPUSERP.SCM.Data.Entity.Requisition;
using OPUSERP.SCM.Services.Requisition.Interfaces;
using OPUSERP.Areas.HRPMSEmployee.Models;
using OPUSERP.Areas.SCMRequisition.Models;
using System.Reflection.Emit;
using OPUSERP.Areas.HRPMSLeave.Models;
using OPUSERP.HRPMS.Services.Leave;
//using DocumentFormat.OpenXml.Drawing;

namespace OPUSERP.Areas.HRPMSAttendence.Controllers
{
    [Area("HRPMSAttendence")]
    public class EmployeePunchCardInfoController : Controller
    {
        private readonly LangGenerate<AttendanceLn> _lang;
        private readonly IEmployeePunchCardInfoService employeePunchCardInfoService;
        private readonly IShiftGroupMasterService shiftGroupMasterService;
        private readonly IWagesPunchCardInfoService wagesPunchCardInfoService;
        private readonly IPersonalInfoService personalInfoService;
        private UserManager<ApplicationUser> _userManager;
        private readonly IERPCompanyService eRPCompanyService;
        private readonly string rootPath;
        private readonly MyPDF myPDF;
        public string FileName;
        private readonly IRequisitionService requisitionService;
        public EmployeePunchCardInfoController(IHostingEnvironment hostingEnvironment, IRequisitionService requisitionService, IERPCompanyService eRPCompanyService, IConverter converter, IEmployeePunchCardInfoService employeePunchCardInfoService, IShiftGroupMasterService shiftGroupMasterService, IWagesPunchCardInfoService wagesPunchCardInfoService, IPersonalInfoService personalInfoService)
        {
            _lang = new LangGenerate<AttendanceLn>(hostingEnvironment.ContentRootPath);
            this.employeePunchCardInfoService = employeePunchCardInfoService;
            this.shiftGroupMasterService = shiftGroupMasterService;
            this.wagesPunchCardInfoService = wagesPunchCardInfoService;
            this.personalInfoService = personalInfoService;
            this.requisitionService = requisitionService;
            myPDF = new MyPDF(hostingEnvironment, converter);
            this.eRPCompanyService = eRPCompanyService;
            rootPath = hostingEnvironment.ContentRootPath;
        }

        // GET: EmployeePunchCardInfo
        public async Task<ActionResult> Index()
        {
            EmployeePunchCardInfoViewModel model = new EmployeePunchCardInfoViewModel
            {
                fLang = _lang.PerseLang("Attendance/PunchCardInfoEN.json", "Attendance/PunchCardInfoBN.json", Request.Cookies["lang"]),
                employeePunchCardInfoslist = await employeePunchCardInfoService.GetAllEmployeePunchCardInfo(),
                employeePunchCardInfoslistNew = await employeePunchCardInfoService.GetAllEmployeeWithPunchCardInfo(),
                shiftGroupMasterslist = await shiftGroupMasterService.GetAllShiftGroupMaster(),
                visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData()
            };
            return View(model);
        }

        // GET: EmployeePunchCardInfo

        public async Task<ActionResult> ManualAttendanceList()
        {
            var manual = await employeePunchCardInfoService.GetAllManualAttendence();
            var empAtt = new List<EmpWithManualAttendence>();

            foreach (var item in manual)
            {
                empAtt.Add(new EmpWithManualAttendence
                {
                    empAttendance = item,
                    employeeInfo = await employeePunchCardInfoService.GetEmpInfoByPunchCard(item.punchCardNo)
                });
            };
            var model = new EmpAttendanceViewModel
            {
                empAttendances = empAtt
            };

            return View(model);
        }
        public async Task<ActionResult> ManualAttendanceListApi(string from, string to)
        {
            var manual = await employeePunchCardInfoService.GetAllManualAttendenceByDateRange(Convert.ToDateTime(from), Convert.ToDateTime(to));
            var empAtt = new List<EmpWithManualAttendence>();

            foreach (var item in manual)
            {
                empAtt.Add(new EmpWithManualAttendence
                {
                    empAttendance = item,
                    employeeInfo = await employeePunchCardInfoService.GetEmpInfoByPunchCard(item.punchCardNo)
                });
            };
            var model = new EmpAttendanceViewModel
            {
                empAttendances = empAtt
            };

            return Json(model);
        }
        public async Task<IActionResult> GetShiftGroupByEmpCode(string code)
        {
            var data = await employeePunchCardInfoService.GetShiftByEmpCode(code);
            return Json(data);
        }

        public async Task<ActionResult> ManualAttendance()
        {
            var manual = await employeePunchCardInfoService.GetAllManualAttendence();
            var empAtt = new List<EmpWithManualAttendence>();


            foreach (var item in manual)
            {
                empAtt.Add(new EmpWithManualAttendence
                {
                    empAttendance = item,
                    employeeInfo = await employeePunchCardInfoService.GetEmpInfoByPunchCard(item.punchCardNo)
                });
            };
            var model = new EmpAttendanceViewModel
            {
                empAttendances = empAtt,
                shiftGroupMasterslist = await shiftGroupMasterService.GetAllShiftGroupMaster()
            };

            return View(model);
        }

        public async Task<IActionResult> ManualAttendanceNew()
        {
            var username = HttpContext.User.Identity.Name;
            EmpWithManualAttendence model = new EmpWithManualAttendence
            {
                employeeInfo = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(username)
                //empManualAttBySp = await employeePunchCardInfoService.GetAllManualAttendanceBySp(),
                //shiftGroupMasterslist = await shiftGroupMasterService.GetAllShiftGroupMaster()
            };
            return View(model);
        }


        public async Task<IActionResult> ManualAttendanceForApproval()
        {
            var username = HttpContext.User.Identity.Name;
            var data = new EmpWithManualAttendence
            {
                manualAttendanceApprovals = await employeePunchCardInfoService.GetManualAttendanceForApproval(username)
            };
            return View(data);
        }

        public async Task<IActionResult> ApproveManualAttendance(int Id)
        {

            string userName = HttpContext.User.Identity.Name;

            var ManualAttdnc = await employeePunchCardInfoService.GetEmpManualAttendanceById(Id);
            var receiver = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(ManualAttdnc.punchCardNo);
            var sender = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(userName);
            await employeePunchCardInfoService.ApproveManualAttendance(Id, sender.employeeCode);
            var notification = new Notification
            {
                Id = 0,
                senderId = sender.ApplicationUserId,
                receiverId = receiver.ApplicationUserId,
                date = DateTime.Now,
                title = "Manual Attendance Approved By " + sender.nameEnglish,
                description = "Manual Attendance Approved By " + sender.nameEnglish,
                type = "Manual Attendance",
                isRead = 0,
                url = "/HRPMSAttendence/EmployeePunchCardInfo/IndivisualEmployeeAttendence",
                status = 1,
                //isDelete = leaveRegister.Id //registerId
            };
            await requisitionService.SaveNotification(notification);
            return Json(Id);
        }


        public async Task<IActionResult> RejectManualAttendance(int Id)
        {

            string userName = HttpContext.User.Identity.Name;

            var ManualAttdnc = await employeePunchCardInfoService.GetEmpManualAttendanceById(Id);
            var receiver = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(ManualAttdnc.punchCardNo);
            var sender = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(userName);
            var result = await employeePunchCardInfoService.RejectManualAttendance(receiver.employeeCode, Id, userName, "Rejected");

            var notification = new Notification
            {
                Id = 0,
                senderId = sender.ApplicationUserId,
                receiverId = receiver.ApplicationUserId,
                date = DateTime.Now,
                title = "Manual Attendance Rejected By " + sender.nameEnglish,
                description = "Manual Attendance Rejected By " + sender.nameEnglish,
                type = "Manual Attendance",
                isRead = 0,
                url = "/HRPMSAttendence/EmployeePunchCardInfo/IndivisualEmployeeAttendence",
                status = 1,
                //isDelete = leaveRegister.Id //registerId
            };
            await requisitionService.SaveNotification(notification);



            return Json(Id);
        }




        public async Task<ActionResult> AttendanceUpload()
        {
            var addedAtt = new List<AttendenceApi>();
            var model = new EmpAttendanceViewModel
            {
                addedAttendence = addedAtt
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AttendanceUpload([FromForm] IFormFile formFile)
        {
            string userName = HttpContext.User.Identity.Name;
            //var user = await _userManager.FindByNameAsync(userName);
            if (formFile == null || formFile.Length <= 0)
            {
                return Json("formfile is empty");
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".log", StringComparison.OrdinalIgnoreCase))
            {
                return Json("Not Support file extension");
            }
            int rownumber = 0;
            string line = "";
            try
            {
                var addedAtt = new List<AttendenceApi>();
                using (StreamReader file = new StreamReader(formFile.OpenReadStream()))
                {
                    string id = "";
                    DateTime date = new DateTime();
                    string time = "";
                    int hour = 0;
                    int minute = 0;
                    //Read the first line of text
                    line = file.ReadLine();
                    //Continue to read until you reach end of file
                    while (line != null)
                    {
                        string[] column = line.Split(',');
                        if (column.Length > 2)
                        {
                            id = column[0];
                            date = Convert.ToDateTime(column[1]);
                            time = column[2];
                            string[] timeS = time.Split(':');
                            hour = Convert.ToInt32(timeS[0]);
                            minute = Convert.ToInt32(timeS[1]);
                            var data = new AttendenceApi()
                            {
                                empCode = id,
                                entryDate = date,
                                LoginTime = new DateTime(date.Year, date.Month, date.Day, hour, minute, 0)

                            };
                            var savedata = await employeePunchCardInfoService.SaveUploadEmployeeAttendence(data);
                            var info = await personalInfoService.GetEmployeeInfoByCode(id);
                            var punchCard = await employeePunchCardInfoService.GetEmployeePunchCardInfoByByEmpCode(id);
                            if (savedata == true)
                            {
                                var punchCardNumber = "Not Found";
                                if (punchCard != null)
                                {
                                    punchCardNumber = punchCard.punchCardNo;
                                }
                                if (info != null)
                                {
                                    addedAtt.Add(new AttendenceApi
                                    {
                                        EmpName = info.nameEnglish,
                                        empCode = info.employeeCode,
                                        Longitudeout = info.department?.deptName,
                                        Reason = punchCardNumber,
                                        entryDate = data.entryDate,
                                        LoginTime = data.LoginTime
                                    });
                                }
                                else
                                {
                                    addedAtt.Add(new AttendenceApi
                                    {
                                        EmpName = "Not Found",
                                        empCode = id,
                                        Longitudeout = "Not Found",
                                        Reason = "Not Found",
                                        entryDate = data.entryDate,
                                        LoginTime = data.LoginTime
                                    });
                                }
                            }
                        }
                        //Read the next line
                        line = file.ReadLine();
                    }
                    var model = new EmpAttendanceViewModel
                    {
                        addedAttendence = addedAtt
                    };
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<ActionResult> ManualAttendance(EmpAttendanceViewModel model)
        {

            #region Head Check
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(model.EmpCode);
            
            if (EmpInfo == null)
            {
                return Json("Invalid Employee Code.");
            }

            var HeadInfo = new DepartBranchZoneHeadViewModel();

            if (EmpInfo.lastDesignation.Id != 285)
            {
                HeadInfo = await personalInfoService.GetApproverByEmployeeId(EmpInfo.Id);
            }
            else
            {
                HeadInfo = await personalInfoService.GetSecondLevelApproverByEmployeeId(EmpInfo.Id);
            }

            if (HeadInfo.HeadId == null)
            {
                return Json("Please Set Approver.");
            }

            var HeadInfoForHead = new DepartBranchZoneHeadViewModel();
            int counter = 0;
            if (EmpInfo.Id == HeadInfo.HeadId)
            {
                counter = counter + 1;
                HeadInfoForHead = await personalInfoService.GetSecondLevelApproverByEmployeeId(EmpInfo.Id);
            }

            if (counter > 0 && HeadInfoForHead.HeadId == null)
            {
                return Json("Please Set Approver.");
            }

            int? approverId = 0;
            if (HeadInfoForHead.HeadId != null)
            {
                approverId = HeadInfoForHead.HeadId;
            }
            else if (HeadInfo.HeadId != null)
            {
                approverId = HeadInfo.HeadId;
            }

            #endregion
                        
            var attendance = await personalInfoService.GetManualAttendanceByType(model.EmpCode, Convert.ToDateTime(model.workDate), 2);
                                    
            var punchCard = await employeePunchCardInfoService.GetEmployeePunchCardInfoByByEmpCode(model.EmpCode);
                       

            try
            {
                if (model.startTime == null && model.endTime == null)
                {
                    ModelState.AddModelError("", "Please add Start or End Time for the employee.");
                }
                else if (model.startTime == null && model.endTime != null)
                {
                   if ( attendance != null && attendance.Id != 0)
                    {
                        attendance.workDate = model.workDate;
                        attendance.latetime = model.latetime;
                        attendance.endTime = model.endTime.Split(" ")[1];
                        attendance.summaryId = 8;
                        attendance.approveStatus = 0;
                        attendance.notes = model.notes;
                        attendance.AttendanceTypeId = 2; //manual
                        attendance.ApproverId = approverId;
                        if (punchCard == null || attendance.workDate == null)
                        {
                            ModelState.AddModelError("", "Please add Punch Card for the employee.");
                        }
                        else
                        {
                            attendance.punchCardNo = punchCard.punchCardNo;
                            await employeePunchCardInfoService.SaveEmployeeManualAttendence(attendance);
                        }
                    }
                    else
                    {
                        var data = new EmpManualAttendance
                        {
                            Id = 0,
                            workDate = model.workDate,
                            latetime = model.latetime,
                            notes = model.notes,
                            endTime = model.endTime.Split(" ")[1],
                            summaryId = 8,
                            remarks = "Manual Attendance",
                            approveStatus = 0, //penidng
                            AttendanceTypeId = 2, //manual
                            ApproverId = approverId
                    };
                        if (punchCard == null || data.workDate == null)
                        {
                            ModelState.AddModelError("", "Please add Punch Card for the employee.");
                        }
                        else
                        {
                            data.punchCardNo = punchCard.punchCardNo;
                            await employeePunchCardInfoService.SaveEmployeeManualAttendence(data);
                        }

                    }
                }
                else if (model.startTime != null && model.endTime == null)
                {                    
                    if (attendance != null && attendance.Id != 0)
                    {                 
                        attendance.workDate = model.workDate;
                        attendance.latetime = model.latetime;
                        attendance.startTime = model.startTime.Split(" ")[1];
                        attendance.summaryId = 8;
                        attendance.approveStatus = 0;
                        attendance.notes = model.notes;
                        attendance.AttendanceTypeId = 2; //manual
                        attendance.ApproverId = approverId;
                        if (punchCard == null || attendance.workDate == null)
                        {
                            ModelState.AddModelError("", "Please add Punch Card for the employee.");
                        }
                        else
                        {
                            attendance.punchCardNo = punchCard.punchCardNo;
                            await employeePunchCardInfoService.SaveEmployeeManualAttendence(attendance);
                        }
                    }
                    else
                    {
                        var data = new EmpManualAttendance
                        {
                            Id = 0,
                            workDate = model.workDate,
                            latetime = model.latetime,
                            startTime = model.startTime.Split(" ")[1],
                            notes = model.notes,
                            summaryId = 8,
                            remarks = "Manual Attendance",
                            approveStatus = 0, //penidng
                            AttendanceTypeId = 2, //manual
                            ApproverId = approverId
                        };
                        if (punchCard == null || data.workDate == null)
                        {
                            ModelState.AddModelError("", "Please add Punch Card for the employee.");
                        }
                        else
                        {
                            data.punchCardNo = punchCard.punchCardNo;
                            await employeePunchCardInfoService.SaveEmployeeManualAttendence(data);
                        }
                    }
                }
                else
                {
                   if (attendance != null && attendance.Id != 0)
                    {
                    
                        attendance.workDate = model.workDate;
                        attendance.latetime = model.latetime;
                        attendance.startTime = model.startTime.Split(" ")[1];
                        attendance.endTime = model.endTime.Split(" ")[1];
                        attendance.summaryId = 8;
                        attendance.approveStatus = 0;
                        attendance.notes = model.notes;
                        attendance.AttendanceTypeId = 2; //manual
                        attendance.ApproverId = approverId;
                        if (punchCard == null || attendance.workDate == null)
                        {
                            ModelState.AddModelError("", "Please add Punch Card for the employee.");
                        }
                        else
                        {
                            attendance.punchCardNo = punchCard.punchCardNo;
                            await employeePunchCardInfoService.SaveEmployeeManualAttendence(attendance);
                        }
                    }
                    else
                    {
                        var data = new EmpManualAttendance
                        {
                            Id = 0,
                            workDate = model.workDate,
                            latetime = model.latetime,
                            startTime = model.startTime.Split(" ")[1],
                            endTime = model.endTime.Split(" ")[1],
                            summaryId = 8,
                            approveStatus = 0,
                            notes = model.notes,
                            AttendanceTypeId = 2, //manual
                            ApproverId = approverId
                    };
                        if (punchCard == null || data.workDate == null)
                        {
                            ModelState.AddModelError("", "Please add Punch Card for the employee.");
                        }
                        else
                        {
                            data.punchCardNo = punchCard.punchCardNo;
                            await employeePunchCardInfoService.SaveEmployeeManualAttendence(data);
                        }
                    }
                }

                var Aprover = "";
                var ApproverPhone = "";

                if (EmpInfo.Id == HeadInfo.HeadId)
                {
                    var senderId = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(HeadInfoForHead.HeadCode);
                    Aprover = senderId.ApplicationUserId;
                    if (senderId.mobileNumberOffice != null)
                    {
                        ApproverPhone = senderId.mobileNumberOffice.Length == 13 ? senderId.mobileNumberOffice : "88" + senderId.mobileNumberOffice;
                    }
                }
                else
                {
                    var senderId = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(HeadInfo.HeadCode);
                    Aprover = senderId.ApplicationUserId;

                    if (senderId.mobileNumberOffice != null)
                    {
                        ApproverPhone = senderId.mobileNumberOffice.Length == 13 ? senderId.mobileNumberOffice : "88" + senderId.mobileNumberOffice;
                    }
                }

                var Emp = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(model.EmpCode);


                #region SMSNotification
                try
                {
                    var message = "An attendance is waiting for your approval.\nApplicant: " + Emp.nameEnglish + "(" + Emp.employeeCode + ")" + "\nAtt. Date: " + model.workDate + "\nAtt. Time: " + model.startTime?.Split(" ")[1] + "\nReason: " + model.notes;
                    var smsResponse = Messaging.SendSMSMessage(ApproverPhone, message);

                    await employeePunchCardInfoService.SaveSMSResponseLog(smsResponse);
                }
                catch (Exception ex)
                {

                }
                #endregion

                var notification = new Notification
                {
                    Id = 0,
                    senderId = Emp.ApplicationUserId,
                    receiverId = Aprover,
                    date = DateTime.Now,
                    title = "Manual Attendance application from " + Emp.nameEnglish,
                    description = "Manual Attendance application from " + Emp.nameEnglish,
                    type = "Manual Attendance",
                    isRead = 0,
                    url = "/HRPMSAttendence/EmployeePunchCardInfo/ManualAttendanceForApproval",
                    status = 1,
                    
                };
                await requisitionService.SaveNotification(notification);

            }
            catch (Exception ex)
            {

            }
            return Json("updated");

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ManualAttendanceAPI([FromBody] ManualAttendanceForAPIViewModel model)
        {

            #region Head Check
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(model.empCode);
            
            if(EmpInfo == null)
            {
				return Json("Invalid Employee Code.");
			}

            var HeadInfo = new DepartBranchZoneHeadViewModel();

            if (EmpInfo.lastDesignation.Id != 285)
			{
                HeadInfo = await personalInfoService.GetApproverByEmployeeId(EmpInfo.Id);
            }
            else
            {
				HeadInfo = await personalInfoService.GetSecondLevelApproverByEmployeeId(EmpInfo.Id);
			}

            if (HeadInfo.HeadId == null)
            {
                return Json("Please Set Approver.");
            }
                        
            var HeadInfoForHead = new DepartBranchZoneHeadViewModel();
            int counter = 0;
			if (EmpInfo.Id == HeadInfo.HeadId)
            {
                counter = counter + 1;
				HeadInfoForHead = await personalInfoService.GetSecondLevelApproverByEmployeeId(EmpInfo.Id);
			}

            if(counter>0 && HeadInfoForHead.HeadId == null)
			{
				return Json("Please Set Approver.");
			}

            int? approverId = 0;
            if(HeadInfoForHead.HeadId != null)
            {
                approverId = HeadInfoForHead.HeadId;
            }
            else if(HeadInfo.HeadId !=null)
            {
                approverId = HeadInfo.HeadId;
            }

            #endregion

            
            var data = await personalInfoService.GetManualAttendanceByType(model.empCode, Convert.ToDateTime(model.punchDate), 2);
                        
            var result = 0;

            if (data == null)
            {
                var att = new EmpManualAttendance
                {
                    Id = 0,
                    createdAt = DateTime.Now,
                    punchCardNo = model.empCode,
                    workDate = Convert.ToDateTime(model.punchDate).ToString("dd-MMM-yyyy"),
                    startTime = model.punchTime,
                    summaryId = 8,
                    approveStatus = 0,
                    remarks = "Manual Attendance",
                    notes = model.notes,
                    location = model.location,
                    lat = model.lat,
                    lon = model.lon,
                    AttendanceTypeId = 2, //manual
                    ApproverId = approverId
                };
                result = await personalInfoService.SaveManualAttendance(att);
            }
            else
            {
                data.workDate = Convert.ToDateTime(model.punchDate).ToString("dd-MMM-yyyy");
                data.endTime = model.punchTime;
                data.notes = model.notes;
                data.location = model.location;
                data.lat = model.lat;
                data.lon = model.lon;
                data.ApproverId = approverId;
                result = await personalInfoService.SaveManualAttendance(data);
            };

            #region notification
                        
            #region Head Check
            #endregion


            var Aprover = "";
            var ApproverPhone = "";

            if (EmpInfo.Id == HeadInfo.HeadId)
            {                                
                var senderId = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(HeadInfoForHead.HeadCode);
                Aprover = senderId.ApplicationUserId;
                if (senderId.mobileNumberOffice != null)
                {
                    ApproverPhone = senderId.mobileNumberOffice.Length == 13 ? senderId.mobileNumberOffice : "88" + senderId.mobileNumberOffice;
                }
            }
            else
            {
                var senderId = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(HeadInfo.HeadCode);
                Aprover = senderId.ApplicationUserId;

                if (senderId.mobileNumberOffice != null)
                {
                    ApproverPhone = senderId.mobileNumberOffice.Length == 13 ? senderId.mobileNumberOffice : "88" + senderId.mobileNumberOffice;
                }
            }

            var Emp = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(model.empCode);


            #region SMSNotification
            try
            {
                var message = "An attendance is waiting for your approval.\nApplicant: " + Emp.nameEnglish + "(" + Emp.employeeCode + ")" + "\nAtt. Date: " + model.punchDate?.ToString("dd-MMM-yyyy") + "\nAtt. Time: " + model.punchTime + "\nReason: " + model.notes;
                var smsResponse = Messaging.SendSMSMessage(ApproverPhone, message);

                await employeePunchCardInfoService.SaveSMSResponseLog(smsResponse);
            }
            catch (Exception ex)
            {

            }
            #endregion


            var notification = new Notification
            {
                Id = 0,
                senderId = Emp.ApplicationUserId,
                receiverId = Aprover,
                date = DateTime.Now,
                title = "Manual Attendance application from " + Emp.nameEnglish,
                description = "Manual Attendance application from " + Emp.nameEnglish,
                type = "Manual Attendance",
                isRead = 0,
                url = "/HRPMSAttendence/EmployeePunchCardInfo/ManualAttendanceForApproval",
                status = 1,
            
            };
            await requisitionService.SaveNotification(notification);
            #endregion

            if (result > 0)
            {
                return Json("Saved");
            }
            else
            {
                return Json("Failed");
            }
        }


        #region Late Reminder
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ManualAttendanceForlateReminderAPI([FromBody] ManualAttendanceForAPIViewModel model)
        {


            #region Head Check
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(model.empCode);

            if (EmpInfo == null)
            {
                return Json("Invalid Employee Code.");
            }

            var HeadInfo = new DepartBranchZoneHeadViewModel();

            if (EmpInfo.lastDesignation.Id != 285)
            {
                HeadInfo = await personalInfoService.GetApproverByEmployeeId(EmpInfo.Id);
            }
            else
            {
                HeadInfo = await personalInfoService.GetSecondLevelApproverByEmployeeId(EmpInfo.Id);
            }

            if (HeadInfo.HeadId == null)
            {
                return Json("Please Set Approver.");
            }

            var HeadInfoForHead = new DepartBranchZoneHeadViewModel();
            int counter = 0;
            if (EmpInfo.Id == HeadInfo.HeadId)
            {
                counter = counter + 1;
                HeadInfoForHead = await personalInfoService.GetSecondLevelApproverByEmployeeId(EmpInfo.Id);
            }

            if (counter > 0 && HeadInfoForHead.HeadId == null)
            {
                return Json("Please Set Approver.");
            }

            int? approverId = 0;
            if (HeadInfoForHead.HeadId != null)
            {
                approverId = HeadInfoForHead.HeadId;
            }
            else if (HeadInfo.HeadId != null)
            {
                approverId = HeadInfo.HeadId;
            }

            #endregion

            var data = await personalInfoService.GetManualAttendanceByType(model.empCode, Convert.ToDateTime(model.punchDate), 3);

            var result = 0;

            if (data == null)
            {
                var att = new EmpManualAttendance
                {
                    Id = 0,
                    createdAt = DateTime.Now,
                    punchCardNo = model.empCode,
                    workDate = Convert.ToDateTime(model.punchDate).ToString("dd-MMM-yyyy"),
                    startTime = model.punchTime,
                    //endTime = null,
                    summaryId = 8,
                    approveStatus = 0,
                    remarks = "Late Reminder",
                    notes = model.notes,
                    location = model.location,
                    lat = model.lat,
                    lon = model.lon,
                    AttendanceTypeId = 3, //Late Reminder
                    ApproverId = approverId

                };
                result = await employeePunchCardInfoService.SaveEmployeeManualAttendence(att);
            }
            else
            {
                data.workDate = Convert.ToDateTime(model.punchDate).ToString("dd-MMM-yyyy");
                data.endTime = model.punchTime;
                data.notes = model.notes;
                data.location = model.location;
                data.lat = model.lat;
                data.lon = model.lon;
                data.ApproverId = approverId;
                result = await personalInfoService.SaveManualAttendance(data);
            };

            #region notification
            
            
            
            var Aprover = "";
            var ApproverPhone = "";
            if (EmpInfo.Id == HeadInfo.HeadId)
            {
                var senderId = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(HeadInfoForHead.HeadCode);
                Aprover = senderId.ApplicationUserId;
                if (senderId.mobileNumberOffice != null)
                {
                    ApproverPhone = senderId.mobileNumberOffice.Length == 13 ? senderId.mobileNumberOffice : "88" + senderId.mobileNumberOffice;
                }
            }
            else
            {
                var senderId = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(HeadInfo.HeadCode);
                Aprover = senderId.ApplicationUserId;

                if (senderId.mobileNumberOffice != null)
                {
                    ApproverPhone = senderId.mobileNumberOffice.Length == 13 ? senderId.mobileNumberOffice : "88" + senderId.mobileNumberOffice;
                }
            }

            var Emp = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(model.empCode);





            #region SMSNotification
            try
            {
                var message = "A late attendance request is waiting for your approval.\nApplicant: " + Emp.nameEnglish + "(" + Emp.employeeCode + ")" + "\nAtt. Date: " + model.punchDate?.ToString("dd-MMM-yyyy") + "\nAtt. Time: " + model.punchTime + "\nReason: " + model.notes;
                var smsResponse = Messaging.SendSMSMessage(ApproverPhone, message);

                await employeePunchCardInfoService.SaveSMSResponseLog(smsResponse);
            }
            catch (Exception ex)
            {

            }
            #endregion


            var notification = new Notification
            {
                Id = 0,
                senderId = Emp.ApplicationUserId,
                receiverId = Aprover,
                date = DateTime.Now,
                title = "Late Reminder application from " + Emp.nameEnglish,
                description = "Late Reminder application from " + Emp.nameEnglish,
                type = "Late Reminder",
                isRead = 0,
                url = "/HRPMSAttendence/EmployeePunchCardInfo/ManualAttendanceForApproval",
                status = 1,
                //isDelete = leaveRegister.Id //registerId
            };
            await requisitionService.SaveNotification(notification);
            #endregion

            if (result > 0)
            {
                return Json("Saved");
            }
            else
            {
                return Json("Failed");
            }
           
        }
        #endregion

        #region Early Leave Attendance
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ManualAttendanceForEarlyLeaveAPI([FromBody] ManualAttendanceForAPIViewModel model)
        {
                     
            #region Head Check
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(model.empCode);

            if (EmpInfo == null)
            {
                return Json("Invalid Employee Code.");
            }

            var HeadInfo = new DepartBranchZoneHeadViewModel();

            if (EmpInfo.lastDesignation.Id != 285)
            {
                HeadInfo = await personalInfoService.GetApproverByEmployeeId(EmpInfo.Id);
            }
            else
            {
                HeadInfo = await personalInfoService.GetSecondLevelApproverByEmployeeId(EmpInfo.Id);
            }

            if (HeadInfo.HeadId == null)
            {
                return Json("Please Set Approver.");
            }

            var HeadInfoForHead = new DepartBranchZoneHeadViewModel();
            int counter = 0;
            if (EmpInfo.Id == HeadInfo.HeadId)
            {
                counter = counter + 1;
                HeadInfoForHead = await personalInfoService.GetSecondLevelApproverByEmployeeId(EmpInfo.Id);
            }

            if (counter > 0 && HeadInfoForHead.HeadId == null)
            {
                return Json("Please Set Approver.");
            }

            int? approverId = 0;
            if (HeadInfoForHead.HeadId != null)
            {
                approverId = HeadInfoForHead.HeadId;
            }
            else if (HeadInfo.HeadId != null)
            {
                approverId = HeadInfo.HeadId;
            }

            #endregion

            var data = await personalInfoService.GetManualAttendanceByType(model.empCode, Convert.ToDateTime(model.punchDate), 4);

            var result = 0;
            
            if (data == null)
            {
                var att = new EmpManualAttendance
                {
                    Id = 0,
                    createdAt = DateTime.Now,
                    punchCardNo = model.empCode,
                    workDate = Convert.ToDateTime(model.punchDate).ToString("dd-MMM-yyyy"),
                    startTime = model.punchTime,
                    //endTime = null,
                    summaryId = 8,
                    approveStatus = 0,
                    remarks = "Early Leave",
                    notes = model.notes,
                    location = model.location,
                    lat = model.lat,
                    lon = model.lon,
                    AttendanceTypeId = 4, //Early Leave
                    ApproverId = approverId
                };
                result = await personalInfoService.SaveManualAttendance(att);
            }
            else
            {
                data.workDate = Convert.ToDateTime(model.punchDate).ToString("dd-MMM-yyyy");
                data.endTime = model.punchTime;
                data.notes = model.notes;
                data.location = model.location;
                data.lat = model.lat;
                data.lon = model.lon;
                data.ApproverId = approverId;
                result = await personalInfoService.SaveManualAttendance(data);
            };

            #region notification
                        
            
            var Aprover = "";
            var ApproverPhone = "";
            if (EmpInfo.Id == HeadInfo.HeadId)
            {
                var senderId = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(HeadInfoForHead.HeadCode);
                Aprover = senderId.ApplicationUserId;
                if (senderId.mobileNumberOffice != null)
                {
                    ApproverPhone = senderId.mobileNumberOffice.Length == 13 ? senderId.mobileNumberOffice : "88" + senderId.mobileNumberOffice;
                }
            }
            else
            {
                var senderId = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(HeadInfo.HeadCode);
                Aprover = senderId.ApplicationUserId;

                if (senderId.mobileNumberOffice != null)
                {
                    ApproverPhone = senderId.mobileNumberOffice.Length == 13 ? senderId.mobileNumberOffice : "88" + senderId.mobileNumberOffice;
                }
            }

            var Emp = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(model.empCode);


            #region SMSNotification
            try
            {
                var message = "An attendance is waiting for your approval.\nApplicant: " + Emp.nameEnglish + "(" + Emp.employeeCode + ")" + "\nAtt. Date: " + model.punchDate + "\nAtt. Time: " + model.punchTime + "\nReason: " + model.notes;
                var smsResponse = Messaging.SendSMSMessage(ApproverPhone, message);

                await employeePunchCardInfoService.SaveSMSResponseLog(smsResponse);
            }
            catch (Exception ex)
            {

            }
            #endregion

            var notification = new Notification
            {
                Id = 0,
                senderId = Emp.ApplicationUserId,
                receiverId = Aprover,
                date = DateTime.Now,
                title = "Early Leave Attendance application from " + Emp.nameEnglish,
                description = "Early Leave Attendance application from " + Emp.nameEnglish,
                type = "Early Leave Attendance",
                isRead = 0,
                url = "/HRPMSAttendence/EmployeePunchCardInfo/ManualAttendanceForApproval",
                status = 1,
                //isDelete = leaveRegister.Id //registerId
            };
            await requisitionService.SaveNotification(notification);
            #endregion

            if (result > 0)
            {
                return Json("Saved");
            }
            else
            {
                return Json("Failed");
            }
        }
        #endregion

        #region notification 
        [HttpGet("api/GetAllNotificationsByUser/{empCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllNotificationsByUser(string empCode)
        {

            var user = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(empCode);
            var data = await requisitionService.GetAllNotificationsByReceiverId(user.ApplicationUserId);
            return Json(data);

        }

        [HttpGet("api/GetUnreadNotificationCount/{empCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUnreadNotificationCount(string empCode)
        {
            var user = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(empCode);
            var data = await requisitionService.GetAllNotificationsCountByReceiverId(user.ApplicationUserId);
            return Json(data);

        }


        #endregion

        //[AllowAnonymous]
        //public async Task<IActionResult> GetManualAttendanceByAnyKey(string employeeCode, int? approverId, string fromDate, string toDate)
        //{
        //    var data = await employeePunchCardInfoService.GetManualAttendanceByAnyKey(employeeCode, approverId, fromDate, toDate);
        //    return Json(data);
        //}



        [HttpGet("api/attendance/GetMyPendingManualAttendance/{empCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMyPendingManualAttendance(string empCode)
        {
            var data = await employeePunchCardInfoService.GetMyPendingManualAttendanceAPI(empCode);
            return Json(data);
        }

        [HttpGet("api/attendance/GetMyApprovedManualAttendance/{empCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMyApprovedManualAttendance(string empCode)
        {
            var data = await employeePunchCardInfoService.GetMyApprovedManualAttendanceAPI(empCode);
            return Json(data);
        }

        [HttpGet("api/attendance/GetMyRejectedManualAttendance/{empCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMyRejectedManualAttendance(string empCode)
        {
            var data = await employeePunchCardInfoService.GetMyRejectedManualAttendanceAPI(empCode);
            return Json(data);
        }

        [HttpGet("api/attendance/ApproveManualAttendance/{empCode}/{id}/{approverEmpCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> ApproveManualAttendance(string empCode, int id, string approverEmpCode)
        {
            var result = await employeePunchCardInfoService.ApproveManualAttendance(id, approverEmpCode);


            var receiver = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(empCode);
            var sender = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(approverEmpCode);
            var notification = new Notification
            {
                Id = 0,
                senderId = sender.ApplicationUserId,
                receiverId = receiver.ApplicationUserId,
                date = DateTime.Now,
                title = "Manual Attendance Approved By " + sender.nameEnglish,
                description = "Manual Attendance Approved By " + sender.nameEnglish,
                type = "Manual Attendance",
                isRead = 0,
                url = "/HRPMSAttendence/EmployeePunchCardInfo/IndivisualEmployeeAttendence",
                status = 1,
            };
            await requisitionService.SaveNotification(notification);
            return Json(result);
        }
        [HttpPost("api/attendance/RejectManualAttendanceforReason")]
        [AllowAnonymous]
        public async Task<IActionResult> RejectManualAttendanceforReason([FromBody] ManualAttendanceRejectApi model)
        {
            var result = await employeePunchCardInfoService.RejectManualAttendance(model.empCode, model.id, model.approverEmpCode, model.reasonforreject);
            var receiver = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(model.empCode);
            var sender = await employeePunchCardInfoService.GetEmployeeInfoByByEmpCode(model.approverEmpCode);
            var notification = new Notification
            {
                Id = 0,
                senderId = sender.ApplicationUserId,
                receiverId = receiver.ApplicationUserId,
                date = DateTime.Now,
                title = "Manual Attendance Rejected By " + sender.nameEnglish,
                description = "Manual Attendance Rejected By " + sender.nameEnglish,
                type = "Manual Attendance",
                isRead = 0,
                url = "/HRPMSAttendence/EmployeePunchCardInfo/IndivisualEmployeeAttendence",
                status = 1,
            };
            await requisitionService.SaveNotification(notification);

            return Json(result);
        }
        [HttpGet("api/attendance/ApprovedManualAttendanceforApprover/{empCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> ApprovedManualAttendanceforApprover(string empCode)
        {
            var result = await employeePunchCardInfoService.GetApprovedAttendanceForApproval(empCode);
            return Json(result);
        }
        [HttpGet("api/attendance/RejectedManualAttendanceforApprover/{empCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> RejectedManualAttendanceforApprover(string empCode)
        {
            var result = await employeePunchCardInfoService.GetRejectedAttendanceForApproval(empCode);
            return Json(result);
        }	
        [HttpGet("api/attendance/ManualAttendanceForApprovalAPI/{empCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> ManualAttendanceForApprovalAPI(string empCode)
        {
            var manualAttendanceApprovals = await employeePunchCardInfoService.GetManualAttendanceForApproval(empCode);
            return Json(manualAttendanceApprovals);
        }

        public async Task<ActionResult> WagesIndex()
        {
            EmployeePunchCardInfoViewModel model = new EmployeePunchCardInfoViewModel
            {
                fLang = _lang.PerseLang("Attendance/PunchCardInfoEN.json", "Attendance/PunchCardInfoBN.json", Request.Cookies["lang"]),
                wagesPunchCardInfos = await wagesPunchCardInfoService.GetAllEmployeePunchCardInfo(),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] EmployeePunchCardInfoViewModel model)
        {
            try
            {
                EmployeePunchCardInfo data = new EmployeePunchCardInfo
                {
                    Id = model.editId,
                    shiftGroupMasterId = (int)model.shiftGroupMasterId,
                    punchCardNo = model.punchCardNo,
                    employeeCode = model.employeeCode
                };

                await employeePunchCardInfoService.SaveEmployeePunchCardInfo(data);

                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }








        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WagesIndex([FromForm] EmployeePunchCardInfoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.fLang = _lang.PerseLang("Attendance/PunchCardInfoEN.json", "Attendance/PunchCardInfoBN.json", Request.Cookies["lang"]);
                model.wagesPunchCardInfos = await wagesPunchCardInfoService.GetAllEmployeePunchCardInfo();
                return View(model);
            }

            WagesPunchCardInfo data = new WagesPunchCardInfo
            {
                Id = model.editId,
                punchCardNo = model.punchCardNo,
                employeeId = (int)model.employeeId
            };

            await wagesPunchCardInfoService.SaveEmployeePunchCardInfo(data);

            return RedirectToAction(nameof(WagesIndex));
        }
        public async Task<ActionResult> AdminDailyTimeSheet(string id)
        {
            DailyAttendenceViewModel model = new DailyAttendenceViewModel
            {
                departments = await employeePunchCardInfoService.GetAllDepartment(),
                hrBranches = await employeePunchCardInfoService.GetAllHrBranches(),
                hrDivisions = await employeePunchCardInfoService.GetAllHrDivisions(),
                shifts = await employeePunchCardInfoService.GetAllShift(),
                employeeInfosd = await employeePunchCardInfoService.GetAllEmployee(),
                employeeInfoDepts = await employeePunchCardInfoService.GetAllEmployeeDept(),
                employeeInfo = await employeePunchCardInfoService.GetEmpInfoByPunchCard(id),
                designations = await employeePunchCardInfoService.GetAllDesignation(),
                zone = await employeePunchCardInfoService.GetAllZone(),
                Office = await employeePunchCardInfoService.GetAllOffice(),


            };
            return View(model);

        }

        [AllowAnonymous]
        public async Task<ActionResult> MonthlyAttendanceView(int year, int month, int deptId, int branchId, int zoneId)
        {
            ViewBag.DaysInMonth = DateTime.DaysInMonth(year, month);
            ViewBag.Month = new DateTime(year, month, 1).ToString("MMMM") + " " + year.ToString();

            var data = await employeePunchCardInfoService.GetMonthlyAttendance(year, month, deptId, branchId, zoneId);
            var model = new MonlyAttendanceReportVm
            {
                companies = await eRPCompanyService.GetAllCompany(),
                attendance = data
            };
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult MonthlyAttendancePdf(int year, int month, int deptId, int branchId, int zoneId)
        {

            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string fileName;

            url = scheme + "://" + host + "/HRPMSAttendence/EmployeePunchCardInfo/MonthlyAttendanceView?year=" + year + "&month=" + month + "&deptId=" + deptId + "&branchId=" + branchId + "&zoneId=" + zoneId;

            string status = myPDF.GenerateLandscapePDFCustom2(out fileName, url, "attendatceReport");

            FileName = fileName;
            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }



        [AllowAnonymous]
        public async Task<ActionResult> MonthlyAttendanceInTimeView(int year, int month, int deptId, int branchId, int zoneId, int officeId, int hrUnitId, int hrDivitionId)
        {
            ViewBag.DaysInMonth = DateTime.DaysInMonth(year, month);
            ViewBag.Month = new DateTime(year, month, 1).ToString("MMMM") + " " + year.ToString();

            var data = await employeePunchCardInfoService.GetMonthlyInTimeAttendance(year, month, deptId, branchId, zoneId, officeId, hrUnitId, hrDivitionId);
            var model = new MonlyAttendanceReportVm
            {
                companies = await eRPCompanyService.GetAllCompany(),
                attendance = data
            };
            return View(model);
        }
        [AllowAnonymous]
        public IActionResult MonthlyAttendanceInTimePdf(int year, int month, int deptId, int branchId, int zoneId,int officeId,int hrUnitId,int hrDivitionId)
        {

            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string fileName;

            url = scheme + "://" + host + "/HRPMSAttendence/EmployeePunchCardInfo/MonthlyAttendanceInTimeView?year=" + year + "&month=" + month + "&deptId=" + deptId + "&branchId=" + branchId + "&zoneId=" + zoneId + "&officeId=" + officeId + "&hrUnitId=" + hrUnitId + "&hrDivitionId=" + hrDivitionId;

            string status = myPDF.GenerateLandscapePDFCustom2(out fileName, url, "attendatceReport");

            FileName = fileName;
            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }


        public async Task<ActionResult> AbsentEmpSheet(string id)
        {
            try
            {
                await employeePunchCardInfoService.ProcessAttendance(DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("dd/MM/yyyy"));
            }
            catch (Exception)
            {

            }

            DailyAttendenceViewModel model = new DailyAttendenceViewModel
            {
                departments = await employeePunchCardInfoService.GetAllDepartment(),
                hrBranches = await employeePunchCardInfoService.GetAllHrBranches(),
                hrDivisions = await employeePunchCardInfoService.GetAllHrDivisions(),
                shifts = await employeePunchCardInfoService.GetAllShift(),
                employeeInfosd = await employeePunchCardInfoService.GetAllEmployee(),
                employeeInfoDepts = await employeePunchCardInfoService.GetAllEmployeeDept(),
                employeeInfo = await employeePunchCardInfoService.GetEmpInfoByPunchCard(id),
                designations = await employeePunchCardInfoService.GetAllDesignation(),
                zone = await employeePunchCardInfoService.GetAllZone(),
                Office = await employeePunchCardInfoService.GetAllOffice()

            };
            return View(model);

        }

        public async Task<ActionResult> GetAbsenteByMonthDDSapi(string date, int department, string designation, int shift, int branch, int division)
        {
            IEnumerable<EmployeeInfo> empList = Enumerable.Empty<EmployeeInfo>();
            var designations = await personalInfoService.GetAllDesignation();

            if ((department == 0 && branch == 0 && division == 0) && date != null)
            {
                empList = await employeePunchCardInfoService.GetAllEmployeeWithPunchCardNo(department, designation, shift);
            }
            if (department != 0)
            {
                empList = await employeePunchCardInfoService.GetAllEmployeeWithPunchCardNo(department, designation, shift);
            }
            if (branch != 0)
            {
                empList = await employeePunchCardInfoService.GetAllEmployeeWithPunchCardNoBranch(branch, designation, shift);
            }
            if (division != 0)
            {
                empList = await employeePunchCardInfoService.GetAllEmployeeWithPunchCardNoDivision(division, designation, shift);
            }

            var emp = new List<AllEmployeeWithAttendence>();
            var tLate = await employeePunchCardInfoService.GetDailyTotalLateByMonth(Convert.ToDateTime(date).Date);
            var tLeaveEmp = await employeePunchCardInfoService.GetTotalLeaveByDate(Convert.ToDateTime(date).Date);
            var count = 0;
            var present = 0;
            foreach (var item in empList)
            {
                count++;
            }
            var tLeave = 0;
            foreach (var item in tLeaveEmp)
            {
                tLeave += Convert.ToInt32(item.isDelete);
            }

            foreach (var item in empList)
            {
                emp.Add(new AllEmployeeWithAttendence
                {
                    employeeInfo = item,
                    designation = designations.Where(x => x.Id == item.lastDesignationId).Select(x => x.designationName).FirstOrDefault(),
                    isLeaved = await employeePunchCardInfoService.GetLeaveStatusByEmpCodeAndDate(item.employeeCode, date),
                    empAttendance = await employeePunchCardInfoService.GetDailyAbsentByEmpCodeDate(item.employeeCode, date)
                });
            }
            foreach (var item in emp)
            {
                if (item.empAttendance != null)
                {
                    present++;
                }
            }
            var model = new DailyAttendenceViewModel
            {
                totalPresent = present,
                totalAbsent = count - present,
                totalLeave = tLeave,
                DailyAttendence = emp,
                totalLate = tLate
            };

            return Json(model);


        }
        [AllowAnonymous]
        public async Task<ActionResult> getDailyAbsentByDateView(string type, string date, int dbd, string designation, int shift)
        {
            IEnumerable<EmployeeInfo> empList = Enumerable.Empty<EmployeeInfo>();
            var designations = await personalInfoService.GetAllDesignation();

            ViewBag.date = date;
            ViewBag.designation = designation;
            if (type == "department")
            {
                empList = await employeePunchCardInfoService.GetAllEmployeeWithPunchCardNo(dbd, designation, shift);
            }
            if (type == "branch")
            {
                empList = await employeePunchCardInfoService.GetAllEmployeeWithPunchCardNoBranch(dbd, designation, shift);
            }
            if (type == "division")
            {
                empList = await employeePunchCardInfoService.GetAllEmployeeWithPunchCardNoDivision(dbd, designation, shift);
            }

            if (type == "zone")
            {
                empList = await employeePunchCardInfoService.GetAllEmployeeWithPunchCardNoZoneNew(dbd, designation, shift);
            }
            if (type == "office")
            {
                empList = await employeePunchCardInfoService.GetAllEmployeeWithPunchCardNoOffice(dbd, designation, shift);
            }

            var emp = new List<AllEmployeeWithAttendence>();
            var tLate = await employeePunchCardInfoService.GetDailyTotalLateByMonth(Convert.ToDateTime(date).Date);
            var tLeaveEmp = await employeePunchCardInfoService.GetTotalLeaveByDate(Convert.ToDateTime(date).Date);
            var count = 0;
            var present = 0;
            foreach (var item in empList)
            {
                count++;
            }
            var tLeave = 0;
            foreach (var item in tLeaveEmp)
            {
                tLeave += Convert.ToInt32(item.isDelete);
            }

            foreach (var item in empList)
            {
                emp.Add(new AllEmployeeWithAttendence
                {
                    employeeInfo = item,
                    designation = designations.Where(x => x.Id == item.lastDesignationId).Select(x => x.designationName).FirstOrDefault(),
                    isLeaved = await employeePunchCardInfoService.GetLeaveStatusByEmpCodeAndDate(item.employeeCode, date),
                    empAttendance = await employeePunchCardInfoService.GetDailyAttendenceByEmpCodeDate(item.employeeCode, date)
                });
            }
            foreach (var item in emp)
            {
                if (item.empAttendance != null)
                {
                    present++;
                }
            }
            var model = new DailyAttendenceViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                departments = await employeePunchCardInfoService.GetAllDepartment(),
                shifts = await employeePunchCardInfoService.GetAllShift(),
                zone = await employeePunchCardInfoService.GetAllZone(),
                Office = await employeePunchCardInfoService.GetAllOffice(),
                employeeInfoDepts = await employeePunchCardInfoService.GetAllEmployeeDept(),
                totalPresent = present,
                totalAbsent = count - present,
                totalLeave = tLeave,
                DailyAttendence = emp,
                totalLate = tLate,
                employeeInfo = await employeePunchCardInfoService.GetEmployeeInfoByDeptDesigShift(Convert.ToInt32(emp[0].employeeInfo.departmentId), emp[0].employeeInfo.designation, Convert.ToInt32(emp[0].employeeInfo.shiftGroupId))
            };

            if (type == "department")
            {
                model.departmentName = empList.Select(x => x.department?.deptName).FirstOrDefault();
            }
            if (type == "branch")
            {
                model.departmentName = empList.Select(x => x.hrBranch?.branchName).FirstOrDefault();
            }
            if (type == "division")
            {
                model.departmentName = empList.Select(x => x.hrDivision?.divName).FirstOrDefault();
            }
            if (type == "zone")
            {
                model.zoneName = empList.Select(x => x.location?.branchUnitName).FirstOrDefault();
            }
            if (type == "office")
            {
                model.officeName = empList.Select(x => x.functionInfo?.branchUnitName).FirstOrDefault();
            }

            if (model.employeeInfo?.lastDesignation?.designationName != "0")
            {
                model.designationName = emp[0].employeeInfo?.lastDesignation?.designationName;
            }
            else
            {
                model.designationName = null;
            }

            if (shift != 0)
            {
                model.shiftName = await employeePunchCardInfoService.GetShiftNameById(shift);
            }
            else
            {
                model.shiftName = null;
            }
            return View(model);


        }


        [AllowAnonymous]
        public IActionResult getAbsentAttendenceByDatePdf(string type, string date, int dbdId, string designation, int shift)
        {
            if (designation == null)
            {
                designation = "0";
            }
            string fileName;
            string host = HttpContext.Request.Host.ToString();
            string scheme = Request.Scheme;
            string url = string.Empty;
            if (type == "department")
            {
                url = $"" + scheme + "://" + host + "/HRPMSAttendence/EmployeePunchCardInfo/getDailyAbsentByDateView?type=department&date=" + date + "&dbd=" + dbdId + "&designation=" + designation + "&shift=" + shift;
            }
            if (type == "branch")
            {
                url = $"" + scheme + "://" + host + "/HRPMSAttendence/EmployeePunchCardInfo/getDailyAbsentByDateView?type=branch&date=" + date + "&dbd=" + dbdId + "&designation=" + designation + "&shift=" + shift;
            }
            if (type == "division")
            {
                url = $"" + scheme + "://" + host + "/HRPMSAttendence/EmployeePunchCardInfo/getDailyAbsentByDateView?type=division&date=" + date + "&dbd=" + dbdId + "&designation=" + designation + "&shift=" + shift;
            }
            if (type == "zone")
            {
                url = $"" + scheme + "://" + host + "/HRPMSAttendence/EmployeePunchCardInfo/getDailyAbsentByDateView?type=zone&date=" + date + "&dbd=" + dbdId + "&designation=" + designation + "&shift=" + shift;
            }
            if (type == "office")
            {
                url = $"" + scheme + "://" + host + "/HRPMSAttendence/EmployeePunchCardInfo/getDailyAbsentByDateView?type=office&date=" + date + "&dbd=" + dbdId + "&designation=" + designation + "&shift=" + shift;
            }

            string status = myPDF.GeneratePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }


        public async Task<ActionResult> GetAllAttendenceByDateApi(string date)
        {
            var empList = await employeePunchCardInfoService.GetAllEmployeeWithPunchCard();
            var emp = new List<AllEmployeeWithAttendence>();
            var allPresent = await employeePunchCardInfoService.GetAllEmployeeAttendenceCountByDateRange1(Convert.ToDateTime(date).Date, Convert.ToDateTime(date).Date);
            var tLate = await employeePunchCardInfoService.GetDailyTotalLateByMonth(Convert.ToDateTime(date).Date);
            var tLeaveEmp = await employeePunchCardInfoService.GetTotalLeaveByDate(Convert.ToDateTime(date).Date);
            var count = 0;
            foreach (var item in empList)
            {
                count++;
            }
            var tLeave = 0;
            foreach (var item in tLeaveEmp)
            {
                tLeave += Convert.ToInt32(item.isDelete);
            }

            foreach (var item in empList)
            {
                emp.Add(new AllEmployeeWithAttendence
                {
                    employeeInfo = item,
                    empAttendance = await employeePunchCardInfoService.GetDailyAttendenceByEmpCodeDate(item.employeeCode, date)
                });
            }
            var model = new DailyAttendenceViewModel
            {
                totalPresent = allPresent,
                totalAbsent = count - allPresent,
                totalLeave = tLeave,
                DailyAttendence = emp,
                totalLate = tLate
            };

            return Json(model);


        }

        public async Task<IActionResult> AllLateAttendences()
        {
            var data = new AllEmployeeLateViewModel
            {
                department = await employeePunchCardInfoService.GetAllDepartment(),
                shift = await employeePunchCardInfoService.GetAllShift(),
                employeeInfoDept = await employeePunchCardInfoService.GetAllEmployeeDept(),
                designations = await employeePunchCardInfoService.GetAllDesignation()
            };
            return View(data);
        }
        public async Task<IActionResult> AllLateAttendencesApi(int year, int month, int department, string designation, int shift)
        {
            var empAtt = new List<EmployeeWithAtt>();
            var employees = await employeePunchCardInfoService.GetAllFilteredEmployee(department, designation, shift);
            var activecount = await employeePunchCardInfoService.FilteredEmployeeCount(department, designation, shift);
            var latecount = await employeePunchCardInfoService.FilteredEmployeeLateCount(year, month, department, designation, shift);
            foreach (var item in employees)
            {
                empAtt.Add(new EmployeeWithAtt
                {
                    employeeInfo = item,
                    totalWorks = await employeePunchCardInfoService.GetTotalWorkingDaysByEmpId(item.Id, year, month),
                    totalLate = await employeePunchCardInfoService.GetTotalLateByEmpId(item.Id, year, month)
                });
            }
            var data = new AllEmployeeLateViewModel
            {
                totalActive = activecount,
                totalLate = latecount,
                department = await employeePunchCardInfoService.GetAllDepartment(),
                shift = await employeePunchCardInfoService.GetAllShift(),
                employeeInfoDept = await employeePunchCardInfoService.GetAllEmployeeDept(),
                empAttendances = empAtt
            };
            return Json(data);
        }

        public async Task<IActionResult> AllAbset()
        {
            var data = new AllEmployeeLate
            {
                department = await employeePunchCardInfoService.GetAllDepartment(),
                shift = await employeePunchCardInfoService.GetAllShift(),
                employeeInfoDept = await employeePunchCardInfoService.GetAllEmployeeDept(),
                designations = await employeePunchCardInfoService.GetAllDesignation(),
            };
            return View(data);
        }
        public async Task<IActionResult> AllAbsentApi(int year, int month, int department, string designation, int shift)
        {
            if (designation == "null")
            {
                designation = null;
            }
            var allemp = await employeePunchCardInfoService.GetAllFilteredEmployee(department, designation, shift);
            var lates = new List<EmployeeLateAttendance>();
            foreach (var item in allemp)
            {
                lates.Add(new EmployeeLateAttendance
                {
                    employee = item,

                    totalPresent = await employeePunchCardInfoService.GetTotalAttendanceByEmpCodeMonthAndYear(item.employeeCode, month, year),
                    totalAbsent = DateTime.DaysInMonth(year, month) - Convert.ToInt32(await employeePunchCardInfoService.GetTotalAttendanceByEmpCodeMonthAndYear(item.employeeCode, month, year))
                });
            }
            var active = await employeePunchCardInfoService.GetAllActiveCountByMonthAndYear(month, year);
            var model = new AllEmployeeLate
            {
                allActive = active,
                allAbsent = lates.Count,
                employeeLates = lates
            };
            return Json(model);
        }

        public async Task<IActionResult> MyAbsent()
        {
            string userName = HttpContext.User.Identity.Name;
            var emp = await employeePunchCardInfoService.GetEmployeeByUsername(userName);
            ViewBag.Name = emp.nameEnglish;
            ViewBag.Code = emp.employeeCode;

            var data = new AllEmployeeLate
            {
                department = await employeePunchCardInfoService.GetAllDepartment(),
                shift = await employeePunchCardInfoService.GetAllShift(),
                employeeInfoDept = await employeePunchCardInfoService.GetAllEmployeeDept(),
                employeeInfo = await employeePunchCardInfoService.GetEmployeename(),

            };
            return View(data);
        }

        public async Task<IActionResult> IndividualAbsentApi(string empCode, int month, int year)
        {
            var emp = await employeePunchCardInfoService.GetEmployeeByEmpCode(empCode);
            var totalhoiday = await employeePunchCardInfoService.GetAllHolidaysByMonthYearNew(month, year);
            var totalLeaves = await employeePunchCardInfoService.GetTotalLeaveByMonthAndEmpCodeInd(empCode, month, year);

            var data = new IndAbsent
            {
                totalWorking = await employeePunchCardInfoService.GetAllAbsentCountByMonthAndYearAndEmpCode(month, year, empCode),
                totalAbsent = DateTime.DaysInMonth(year, month) - await employeePunchCardInfoService.GetAllAbsentCountByMonthAndYearAndEmpCode(month, year, empCode) - totalhoiday - totalLeaves,
                employeeInfo = emp,
                empAbsentDates = await employeePunchCardInfoService.GetAllAbsentByMonthAndYearAndEmpCode(month, year, empCode),
                tLeaves = totalLeaves,
            };
            var model = new AllEmployeeLate
            {
                indAbsent = data
            };
            return Json(model);
        }

        public async Task<ActionResult> GetAdminDailyAttendenceByMonthDDSapi(string date, int department, string designation, int shift, int branch, int division, int zone, int office)
        {
            IEnumerable<EmployeeInfo> empList = Enumerable.Empty<EmployeeInfo>();
            var designations = await personalInfoService.GetAllDesignation();

            if ((department == 0 && branch == 0 && division == 0 && zone == 0 && office == 0) && date != null)
            {
                empList = await employeePunchCardInfoService.GetAllEmployeeWithPunchCardNoForDDSZO(department, designation, shift, zone, office);
            }
            if (department != 0)
            {
                empList = await employeePunchCardInfoService.GetAllEmployeeWithPunchCardNo(department, designation, shift);
            }
            if (branch != 0)
            {
                empList = await employeePunchCardInfoService.GetAllEmployeeWithPunchCardNoBranch(branch, designation, shift);
            }
            if (division != 0)
            {
                empList = await employeePunchCardInfoService.GetAllEmployeeWithPunchCardNoDivision(division, designation, shift);
            }
            if (zone != 0)
            {
                empList = await employeePunchCardInfoService.GetAllEmployeeWithPunchCardNoZoneNew(zone, designation, shift);
            }
            if (office != 0)
            {
                empList = await employeePunchCardInfoService.GetAllEmployeeWithPunchCardNoOffice(office, designation, shift);
            }


            var emp = new List<AllEmployeeWithAttendence>();
            var tLate = await employeePunchCardInfoService.GetDailyTotalLateByMonth(Convert.ToDateTime(date).Date);
            var tLeaveEmp = await employeePunchCardInfoService.GetTotalLeaveByDate(Convert.ToDateTime(date).Date);
            var count = 0;
            var present = 0;
            foreach (var item in empList)
            {
                count++;
            }
            var tLeave = 0;
            foreach (var item in tLeaveEmp)
            {
                tLeave += Convert.ToInt32(item.isDelete);
            }

            foreach (var item in empList)
            {
                emp.Add(new AllEmployeeWithAttendence
                {
                    employeeInfo = item,
                    designation = designations.Where(x => x.Id == item.lastDesignationId).Select(x => x.designationName).FirstOrDefault(),
                    isLeaved = await employeePunchCardInfoService.GetLeaveStatusByEmpCodeAndDate(item.employeeCode, date),
                    empAttendance = await employeePunchCardInfoService.GetDailyAttendenceByEmpCodeDate(item.employeeCode, date)
                });
            }
            foreach (var item in emp)
            {
                if (item.empAttendance != null)
                {
                    present++;
                }
            }
            var model = new DailyAttendenceViewModel
            {
                totalPresent = present,
                totalAbsent = count - present,
                totalLeave = tLeave,
                DailyAttendence = emp,
                totalLate = tLate
            };

            return Json(model);


        }

        public async Task<ActionResult> DailyTimeSheet()
        {
            var empList = await employeePunchCardInfoService.GetAllEmployeeWithPunchCard();
            var emp = new List<AllEmployeeWithAttendence>();
            var allPresent = await employeePunchCardInfoService.GetAllEmployeeAttendenceCountByDateRange1(DateTime.Now.Date, DateTime.Now.Date);
            var tLate = await employeePunchCardInfoService.GetDailyTotalLateByMonth(DateTime.Now.Date);
            var tLeaveEmp = await employeePunchCardInfoService.GetTotalLeaveByDate(DateTime.Now.Date);
            //var tLeaveEmpWP = await employeePunchCardInfoService.GetTotalLeaveByDateWP(DateTime.Now.Date);
            var count = 0;
            foreach (var item in empList)
            {
                count++;
            }
            var tLeave = 0;
            foreach (var item in tLeaveEmp)
            {
                tLeave += Convert.ToInt32(item.isDelete);
            }

            foreach (var item in empList)
            {
                emp.Add(new AllEmployeeWithAttendence
                {
                    employeeInfo = item,
                    empAttendance = await employeePunchCardInfoService.GetDailyAttendenceByEmpCode(item.employeeCode)
                });
            }
            var model = new DailyAttendenceViewModel
            {
                totalPresent = allPresent,
                totalAbsent = count - allPresent,
                totalLeave = tLeave,
                DailyAttendence = emp,
                totalLate = tLate
            };

            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> DailyAttendenceSheetPdf()
        {
            var empList = await employeePunchCardInfoService.GetAllEmployeeWithPunchCard();
            var emp = new List<AllEmployeeWithAttendence>();
            var allPresent = await employeePunchCardInfoService.GetAllEmployeeAttendenceCountByDateRange1(DateTime.Now.Date, DateTime.Now.Date);
            var tLate = await employeePunchCardInfoService.GetDailyTotalLateByMonth(DateTime.Now.Date);
            var tLeaveEmp = await employeePunchCardInfoService.GetTotalLeaveByDate(DateTime.Now.Date);
            var count = 0;
            foreach (var item in empList)
            {
                count++;
            }
            var tLeave = 0;
            foreach (var item in tLeaveEmp)
            {
                tLeave += Convert.ToInt32(item.isDelete);
            }

            foreach (var item in empList)
            {
                emp.Add(new AllEmployeeWithAttendence
                {
                    employeeInfo = item,
                    empAttendance = await employeePunchCardInfoService.GetDailyAttendenceByEmpCode(item.employeeCode)
                });
            }
            var model = new DailyAttendenceViewModel
            {
                totalPresent = allPresent,
                totalAbsent = count - allPresent,
                totalLeave = tLeave,
                DailyAttendence = emp,
                totalLate = tLate
            };

            return View(model);
        }
        public async Task<ActionResult> IndivisualEmployeeAttendence(string id, string code)
        {
            try
            {
                await employeePunchCardInfoService.ProcessAttendance(DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("dd/MM/yyyy"));
            }
            catch (Exception)
            {

            }


            string userName = HttpContext.User.Identity.Name;
            var emp = await employeePunchCardInfoService.GetEmployeeByEmpCode(userName);
            ViewBag.Name = emp.nameEnglish;
            ViewBag.Code = emp.employeeCode;

            AllEmployeeWithAttendence model = new AllEmployeeWithAttendence
            {
                employeeInfo = await employeePunchCardInfoService.GetEmpInfoByPunchCard(id),
                empAttendance = await employeePunchCardInfoService.GetDailyAttendenceByEmpCode(code),

            };

            return View(model);


        }

        public async Task<IActionResult> GetIndividualAttendenceByMonth(int month)
        {
            string userName = HttpContext.User.Identity.Name;
            var punchCardInfo = await employeePunchCardInfoService.GetPunchCardByUserName(userName);

            var attendenceCount = await employeePunchCardInfoService.MonthlyAttendenceCountByUserAndPunchCard(userName, month);
            var tLate = await employeePunchCardInfoService.GetIndivisualTotalLateByMonth(month);
            var tLeaveEmp = await employeePunchCardInfoService.GetIndivisualTotalLeaveByMonth(month);
            var tLeaveEmpWP = await employeePunchCardInfoService.GetIndivisualTotalLeaveByMonthWP(month);
            var tLeave = 0;
            foreach (var item in tLeaveEmp)
            {
                tLeave += Convert.ToInt32(item.isDelete);
            }
            var tLeaveWP = 0;
            foreach (var item in tLeaveEmpWP)
            {
                tLeaveWP += Convert.ToInt32(item.isDelete);
            }
            var dates = new List<DateTime>();

            for (var date = new DateTime(DateTime.Now.Year, month, 1); date.Month == month; date = date.AddDays(1))
            {
                dates.Add(date);
            }

            var data = new List<IndAttendenceByMonth>();

            foreach (var item in dates)
            {
                data.Add(new IndAttendenceByMonth
                {
                    Date = Convert.ToDateTime(item),
                    empAttendance = await employeePunchCardInfoService.GetAttendenceByDateAndPunchCard(item, punchCardInfo.punchCardNo)
                });
            }

            var model = new DailyAttendenceViewModel
            {
                employeeInfo = await employeePunchCardInfoService.GetEmployeeByUsername(userName),
                totalPresent = attendenceCount,
                totalAbsent = DateTime.DaysInMonth(DateTime.Now.Year, month) - attendenceCount,
                indAttendenceByMonths = data,
                totalLate = tLate,
                totalLeave = tLeave,
                totalLeaveWP = tLeaveWP

            };


            return Json(model);
        }

        public async Task<IActionResult> GetIndAttendenceByMonthYear(int month, int year)
        {
            var holidays = await personalInfoService.GetAllHolidaysByMonthYear(month, year);
            string userName = HttpContext.User.Identity.Name;
            var punchCardInfo = await employeePunchCardInfoService.GetPunchCardByUserName(userName);

            var attendenceCount = await employeePunchCardInfoService.MonthlyPresentAttendanceByPunchCard(punchCardInfo.punchCardNo, month, year);
            var tLeaveEmp = await employeePunchCardInfoService.GetTotalLeaveByMonth(month, year);
            var tLeave = 0;
            foreach (var item in tLeaveEmp)
            {
                tLeave += Convert.ToInt32(item.isDelete);
            }
            var dates = new List<DateTime>();

            for (var date = new DateTime(year, month, 1); date.Month == month; date = date.AddDays(1))
            {
                dates.Add(date);
            }

            var data = new List<IndAttendenceByMonth>();

            foreach (var item in dates)
            {
                data.Add(new IndAttendenceByMonth
                {
                    Date = Convert.ToDateTime(item),
                    isHoliday = holidays.Contains(Convert.ToDateTime(item)) ? 1 : 0,
                    isLeave = await employeePunchCardInfoService.CheckLeaveByDate(item, punchCardInfo.punchCardNo),
                    empAttendance = await employeePunchCardInfoService.GetAttendenceByDateAndPunchCard(item, punchCardInfo.punchCardNo)
                });
            }
            var totalhoiday = await employeePunchCardInfoService.GetAllHolidaysByMonthYearNew(month, year);
            var totalLeaves = await employeePunchCardInfoService.GetTotalLeaveByMonthAndEmpCodeInd(punchCardInfo.punchCardNo, month, year);

            var model = new DailyAttendenceViewModel
            {
                employeeInfo = await employeePunchCardInfoService.GetEmployeeByUsername(userName),
                totalPresent = attendenceCount,
                totalAbsent = DateTime.DaysInMonth(DateTime.Now.Year, month) - attendenceCount - totalhoiday - totalLeaves,
                indAttendenceByMonths = data,
                totalLeave = tLeave

            };


            return Json(model);
        }
        public async Task<IActionResult> GetIndividualAttendenceByMonthAndEmpCode(int month, string empCode)
        {
            string userName = await employeePunchCardInfoService.GetUsernameByEMpCode(empCode);
            var punchCardInfo = await employeePunchCardInfoService.GetPunchCardByCode(empCode);
            var attendenceCount = await employeePunchCardInfoService.MonthlyPresentAttendanceByPunchCard(punchCardInfo.punchCardNo, month);
            var tLeaveEmp = await employeePunchCardInfoService.GetIndivisualTotalLeaveByMonth(month);
            var tLeave = 0;
            foreach (var item in tLeaveEmp)
            {
                tLeave += Convert.ToInt32(item.isDelete);
            }
            var dates = new List<DateTime>();

            for (var date = new DateTime(DateTime.Now.Year, month, 1); date.Month == month; date = date.AddDays(1))
            {
                dates.Add(date);
            }

            var data = new List<IndAttendenceByMonth>();

            foreach (var item in dates)
            {
                data.Add(new IndAttendenceByMonth
                {
                    Date = Convert.ToDateTime(item),
                    empAttendance = await employeePunchCardInfoService.GetAttendenceByDateAndPunchCard(item, punchCardInfo.punchCardNo)
                });
            }
            var totalhoiday = await employeePunchCardInfoService.GetHolidaysByMonth(month);
            var totalLeaves = await employeePunchCardInfoService.GetTotalLeaveByMonthAndEmpCode(punchCardInfo.punchCardNo, month);

            var model = new DailyAttendenceViewModel
            {
                employeeInfo = await employeePunchCardInfoService.GetEmployeeByUsername(userName),
                totalPresent = attendenceCount,
                totalAbsent = DateTime.DaysInMonth(DateTime.Now.Year, month) - attendenceCount - totalhoiday - totalLeaves,
                indAttendenceByMonths = data,
                totalLeave = tLeave

            };


            return Json(model);
        }
        public async Task<IActionResult> GetIndividualAttendenceByMonthAndEmpCode2(int month, string empCode, int year)
        {
            var holidays = await personalInfoService.GetAllHolidaysByMonthYear(month, year);
            string userName = await employeePunchCardInfoService.GetUsernameByEMpCode(empCode);
            var punchCardInfo = await employeePunchCardInfoService.GetPunchCardByCode(empCode);
            var attendenceCount = await employeePunchCardInfoService.MonthlyPresentAttendanceByPunchCard(punchCardInfo.punchCardNo, month, year);
            var tLeaveEmp = await employeePunchCardInfoService.GetTotalLeaveByMonth(month, year);
            var tLeave = 0;
            foreach (var item in tLeaveEmp)
            {
                tLeave += Convert.ToInt32(item.isDelete);
            }
            var dates = new List<DateTime>();

            for (var date = new DateTime(year, month, 1); date.Month == month; date = date.AddDays(1))
            {
                dates.Add(date);
            }

            var data = new List<IndAttendenceByMonth>();

            foreach (var item in dates)
            {
                data.Add(new IndAttendenceByMonth
                {
                    Date = Convert.ToDateTime(item),
                    isHoliday = holidays.Contains(Convert.ToDateTime(item)) ? 1 : 0,
                    isLeave = await employeePunchCardInfoService.CheckLeaveByDate(item, punchCardInfo.punchCardNo),

                    empAttendance = await employeePunchCardInfoService.GetAttendenceByDateAndPunchCard(item, punchCardInfo.punchCardNo)
                });
            }
            var totalhoiday = await employeePunchCardInfoService.GetAllHolidaysByMonthYearNew(month, year);
            var totalLeaves = await employeePunchCardInfoService.GetTotalLeaveByMonthAndEmpCodeInd(punchCardInfo.punchCardNo, month, year);

            var model = new DailyAttendenceViewModel
            {
                employeeInfo = await employeePunchCardInfoService.GetEmployeeByUsername(userName),
                totalPresent = attendenceCount,
                totalAbsent = DateTime.DaysInMonth(DateTime.Now.Year, month) - attendenceCount - totalhoiday - totalLeaves,
                indAttendenceByMonths = data,
                totalLeave = tLeave

            };


            return Json(model);
        }
        [AllowAnonymous]
        public async Task<IActionResult> GetIndividualAttendenceByDateRangeAndEmpCode(DateTime fromDate, DateTime toDate, string empCode)
        {
            try
            {
                await employeePunchCardInfoService.ProcessAttendance(DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("dd/MM/yyyy"));
            }
            catch (Exception)
            {

            }
            string userName = await employeePunchCardInfoService.GetUsernameByEMpCode(empCode);
            var punchCardInfo = await employeePunchCardInfoService.GetPunchCardByCode(empCode);
            var holidays = await personalInfoService.GetAllHolidaysByDateRange(fromDate, toDate);
            //var attendenceCount = await employeePunchCardInfoService.MonthlyAttendenceCountByUserAndPunchCard(userName, month);
            var attendenceCount = await employeePunchCardInfoService.PresentAttendanceByPunchCardAndDateRange(punchCardInfo.punchCardNo, fromDate, toDate);
            var tLate = await employeePunchCardInfoService.GetIndivisualTotalLateByDateRange(fromDate, toDate, punchCardInfo.punchCardNo);
            var tLeaveEmp = await employeePunchCardInfoService.GetIndivisualTotalLeaveByDateRange(fromDate, toDate);
            var tLeaveEmpWP = await employeePunchCardInfoService.GetIndivisualTotalLeaveByDateRangeWP(fromDate, toDate);
            var tLeave = 0;
            foreach (var item in tLeaveEmp)
            {
                tLeave += Convert.ToInt32(item.isDelete);
            }
            var tLeaveWP = 0;
            foreach (var item in tLeaveEmpWP)
            {
                tLeaveWP += Convert.ToInt32(item.isDelete);
            }
            var dates = new List<DateTime>();

            for (var date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                dates.Add(date);
            }

            var data = new List<IndAttendenceByMonth>();

            foreach (var item in dates)
            {
                data.Add(new IndAttendenceByMonth
                {
                    Date = Convert.ToDateTime(item),
                    empAttendance = await employeePunchCardInfoService.GetAttendenceByDateAndPunchCard(item, punchCardInfo.punchCardNo),
                    isHoliday = holidays.Contains(Convert.ToDateTime(item)) ? 1 : 0,
                    isLeave = await employeePunchCardInfoService.CheckLeaveByDate(item, punchCardInfo.punchCardNo)
                });
            }
            var totalhoiday = await employeePunchCardInfoService.GetHolidaysByDateRange(fromDate, toDate);
            var model = new DailyAttendenceViewModel
            {
                employeeInfo = await employeePunchCardInfoService.GetEmployeeByUsername(userName),
                totalPresent = attendenceCount,
                totalAbsent = (toDate - fromDate).Days - attendenceCount - totalhoiday,
                indAttendenceByMonths = data,
                totalLate = tLate,
                totalLeave = tLeave,
                totalLeaveWP = tLeaveWP

            };


            return Json(model);
        }
        public async Task<IActionResult> CheckLeave(DateTime? mydateh, string empCode)
        {
            var data = await employeePunchCardInfoService.CheckLeaveByDate(mydateh, empCode);
            return Json(data);
        }

        public async Task<ActionResult> MonthlyTimeSet(string id, string code)
        {
            AllEmployeeWithAttendence model = new AllEmployeeWithAttendence
            {
                department = await employeePunchCardInfoService.GetAllDepartment(),
                shift = await employeePunchCardInfoService.GetAllShift(),
                employeeInfos = await employeePunchCardInfoService.GetAllEmployee(),
                employeeInfo = await employeePunchCardInfoService.GetEmpInfoByPunchCard(id),
                employeeInfoDept = await employeePunchCardInfoService.GetAllEmployeeDept(),
                empAttendance = await employeePunchCardInfoService.GetDailyAttendenceByEmpCode(code),
                designations = await employeePunchCardInfoService.GetAllDesignation()
            };
            return View(model);
        }
        public async Task<IActionResult> GetMonthlyAttendenceByMonth(int month, int year)
        {
            var totalDays = DateTime.DaysInMonth(year, month);
            var empList = await employeePunchCardInfoService.GetAllEmployeeWithPunchCard();
            var tPresent = await employeePunchCardInfoService.GetTotalPresentByMonthandYear(month, year);
            var tLate = await employeePunchCardInfoService.GetTotalLateByMonth(month, year);
            var tLeaveEmp = await employeePunchCardInfoService.GetTotalLeaveByMonth(month, year);
            var tLeaveEmpWP = await employeePunchCardInfoService.GetTotalLeaveByMonthWP(month, year);
            var tLeave = 0;
            foreach (var item in tLeaveEmp)
            {
                tLeave += Convert.ToInt32(item.isDelete);
            }
            var tLeaveWP = 0;
            foreach (var item in tLeaveEmpWP)
            {
                tLeaveWP += Convert.ToInt32(item.isDelete);
            }
            var data = new List<MonthlyEmployeeAttendence>();
            var empListCount = 0;
            foreach (var item in empList)
            {
                empListCount++;
            }
            var tAbsent = (empListCount * totalDays) - tPresent;


            foreach (var item in empList)
            {
                empListCount++;
                data.Add(new MonthlyEmployeeAttendence
                {
                    totalPresent = tPresent,
                    totalAbsent = tAbsent,
                    totalLate = tLate,
                    totalLeave = tLeave,
                    totalLeaveWP = tLeaveWP,
                    employeeInfo = item,
                    empAttendances = await employeePunchCardInfoService.GetAttendenceByMonthAndEmpCode(month, item.employeeCode, year)
                });

            }
            return Json(data);
        }
        public async Task<IActionResult> GetMonthlyAttendenceByMonthDepartDesigShift(int month, int year, int department, string designation, int shift)
        {
            var totalDays = DateTime.DaysInMonth(year, month);
            var empList = await employeePunchCardInfoService.GetAllEmployeeWithPunchCardNo(department, designation, shift);
            var tPresent = await employeePunchCardInfoService.GetTotalPresentByMonthandYear(month, year, department, designation, shift);
            var tLate = await employeePunchCardInfoService.GetTotalLateByMonth(month, year);
            var tLeaveEmp = await employeePunchCardInfoService.GetTotalLeaveByMonth(month, year);
            var tLeaveEmpWP = await employeePunchCardInfoService.GetTotalLeaveByMonthWP(month, year);
            var tLeave = 0;
            foreach (var item in tLeaveEmp)
            {
                tLeave += Convert.ToInt32(item.isDelete);
            }
            var tLeaveWP = 0;
            foreach (var item in tLeaveEmpWP)
            {
                tLeaveWP += Convert.ToInt32(item.isDelete);
            }
            var data = new List<MonthlyEmployeeAttendence>();
            var empListCount = 0;
            foreach (var item in empList)
            {
                empListCount++;
            }
            var tAbsent = (empListCount * totalDays) - tPresent;


            try
            {
                foreach (var item in empList)
                {
                    empListCount++;
                    data.Add(new MonthlyEmployeeAttendence
                    {
                        totalPresent = tPresent,
                        totalAbsent = tAbsent,
                        totalLate = tLate,
                        totalLeave = tLeave,
                        totalLeaveWP = tLeaveWP,
                        employeeInfo = item,
                        empAttendances = await employeePunchCardInfoService.GetAttendenceByMonthAndEmpCodeWithDepert1(month, item.employeeCode, year, department, designation, shift)
                        //empAttendances = await employeePunchCardInfoService.GetAttendenceByMonthAndEmpCode(month, item.employeeCode, year)
                    });

                }
            }
            catch (Exception ex)
            {

            }
            return Json(data);
        }



        public async Task<IActionResult> GetMonthlyAttendenceByMonthDepartDesigZoneBranchShift(int month, int year, int department, string designation, int shift)
        {
            var totalDays = DateTime.DaysInMonth(year, month);
            var empList = await employeePunchCardInfoService.GetAllEmployeeWithPunchCardNo(department, designation, shift);
            var tPresent = await employeePunchCardInfoService.GetTotalPresentByMonthandYear(month, year, department, designation, shift);
            var tLate = await employeePunchCardInfoService.GetTotalLateByMonth(month, year);
            var tLeaveEmp = await employeePunchCardInfoService.GetTotalLeaveByMonth(month, year);
            var tLeaveEmpWP = await employeePunchCardInfoService.GetTotalLeaveByMonthWP(month, year);
            var tLeave = 0;
            foreach (var item in tLeaveEmp)
            {
                tLeave += Convert.ToInt32(item.isDelete);
            }
            var tLeaveWP = 0;
            foreach (var item in tLeaveEmpWP)
            {
                tLeaveWP += Convert.ToInt32(item.isDelete);
            }
            var data = new List<MonthlyEmployeeAttendence>();
            var empListCount = 0;
            foreach (var item in empList)
            {
                empListCount++;
            }
            var tAbsent = (empListCount * totalDays) - tPresent;


            try
            {
                foreach (var item in empList)
                {
                    empListCount++;
                    data.Add(new MonthlyEmployeeAttendence
                    {
                        totalPresent = tPresent,
                        totalAbsent = tAbsent,
                        totalLate = tLate,
                        totalLeave = tLeave,
                        totalLeaveWP = tLeaveWP,
                        employeeInfo = item,
                        empAttendances = await employeePunchCardInfoService.GetAttendenceByMonthAndEmpCodeWithDepert1(month, item.employeeCode, year, department, designation, shift)
                        //empAttendances = await employeePunchCardInfoService.GetAttendenceByMonthAndEmpCode(month, item.employeeCode, year)
                    });

                }
            }
            catch (Exception ex)
            {

            }
            return Json(data);
        }



        [AllowAnonymous]
        public async Task<IActionResult> GetMonthlyAttendenceByMonthPrintPage(int month, int year, int department, string designation, int shift)
        {
            ViewBag.year = year;
            ViewBag.department = department;
            ViewBag.designation = designation;
            ViewBag.shift = shift;

            var monthName = "";
            if (month == 1)
            {
                monthName = "January";
            }
            else if (month == 2)
            {
                monthName = "February";
            }
            else if (month == 3)
            {
                monthName = "March";
            }
            else if (month == 4)
            {
                monthName = "April";
            }
            else if (month == 5)
            {
                monthName = "May";
            }
            else if (month == 6)
            {
                monthName = "June";
            }
            else if (month == 7)
            {
                monthName = "July";
            }
            else if (month == 8)
            {
                monthName = "August";
            }
            else if (month == 9)
            {
                monthName = "September";
            }
            else if (month == 10)
            {
                monthName = "October";
            }
            else if (month == 11)
            {
                monthName = "November";
            }
            else
            {
                monthName = "December";
            }
            ViewBag.month = monthName;

            var totalDays = DateTime.DaysInMonth(year, month);
            var empList = await employeePunchCardInfoService.GetAllEmployeeWithPunchCardNo(department, designation, shift);
            var tPresent = await employeePunchCardInfoService.GetTotalPresentByMonthandYear(month, year, department, designation, shift);
            var tLate = await employeePunchCardInfoService.GetTotalLateByMonth(month, year);
            //var tLeaveEmp = await employeePunchCardInfoService.GetTotalLeaveByMonth(month, year);
            //var tLeaveEmpWP = await employeePunchCardInfoService.GetTotalLeaveByMonthWP(month, year);
            //var tLeave = 0;
            //foreach (var item in tLeaveEmp)
            //{
            //    tLeave += Convert.ToInt32(item.isDelete);
            //}
            //var tLeaveWP = 0;
            //foreach (var item in tLeaveEmpWP)
            //{
            //    tLeaveWP += Convert.ToInt32(item.isDelete);
            //}
            var data = new List<MonthlyEmployeeAttendence>();
            //var empListCount = 0;
            //foreach (var item in empList)
            //{
            //    empListCount++;
            //}
            var tAbsent = totalDays - tPresent;

            try
            {
                foreach (var item in empList)
                {
                    data.Add(new MonthlyEmployeeAttendence
                    {
                        //totalPresent = tPresent,
                        //totalAbsent = tAbsent,
                        //totalLate = tLate,
                        //totalLeave = tLeave,
                        //totalLeaveWP = tLeaveWP,
                        employeeInfo = item,
                        empAttendances = await employeePunchCardInfoService.GetAttendenceByMonthAndEmpCodeWithDepert(month, item.employeeCode, year, department, designation, shift)
                        //empAttendances = await employeePunchCardInfoService.GetAttendenceByMonthAndEmpCode(month, item.employeeCode, year)
                    });

                }
                ViewBag.totalPresent = tPresent;
                ViewBag.totalAbsent = tAbsent;
                ViewBag.totalLate = tLate;
            }
            catch (Exception ex)
            {

            }

            return View(data);
        }

        [AllowAnonymous]
        public IActionResult GetMonthlyAttendenceByMonthPdf(int month, int year, int department, string designation, int shift)
        {
            if (designation == null)
            {
                designation = "0";
            }
            string fileName;
            string host = HttpContext.Request.Host.ToString();
            string scheme = Request.Scheme;
            string url = string.Empty;
            url = $"" + scheme + "://" + host + "/HRPMSAttendence/EmployeePunchCardInfo/GetMonthlyAttendenceByMonthPrintPage?month=" + month + "&year=" + year + "&department=" + department + "&designation=" + designation + "&shift=" + shift;

            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [AllowAnonymous]
        public IActionResult GetAttendenceByMonthPdf(int year, int month)
        {

            string fileName;
            string host = HttpContext.Request.Host.ToString();
            string scheme = Request.Scheme;
            string url = string.Empty;
            url = $"" + scheme + "://" + host + "/HRPMSAttendence/EmployeePunchCardInfo/GetMonthPrintPage?year=" + year + "&month=" + month;

            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }






        [AllowAnonymous]
        public async Task<IActionResult> MyAbsentPrintPage(string empCode, int month, int year)
        {
            ViewBag.year = year;
            var monthName = "";
            if (month == 1)
            {
                monthName = "January";
            }
            else if (month == 2)
            {
                monthName = "February";
            }
            else if (month == 3)
            {
                monthName = "March";
            }
            else if (month == 4)
            {
                monthName = "April";
            }
            else if (month == 5)
            {
                monthName = "May";
            }
            else if (month == 6)
            {
                monthName = "June";
            }
            else if (month == 7)
            {
                monthName = "July";
            }
            else if (month == 8)
            {
                monthName = "August";
            }
            else if (month == 9)
            {
                monthName = "September";
            }
            else if (month == 10)
            {
                monthName = "October";
            }
            else if (month == 11)
            {
                monthName = "November";
            }
            else
            {
                monthName = "December";
            }
            ViewBag.month = monthName;

            string userName = HttpContext.User.Identity.Name;
            var empo = await employeePunchCardInfoService.GetEmployeeByUsername(userName);
            var empl = await personalInfoService.GetEmployeeInfoByCode(empCode);
            ViewBag.Name = empl.nameEnglish;
            ViewBag.Code = empl.employeeCode;
            //var totalhoiday = await employeePunchCardInfoService.GetHolidaysByMonth(month);
            var totalhoiday = await employeePunchCardInfoService.GetAllHolidaysByMonthYearNew(month, year);
            var totalLeaves = await employeePunchCardInfoService.GetTotalLeaveByMonthAndEmpCodeInd(empCode, month, year);

            var emp = await employeePunchCardInfoService.GetEmployeeByEmpCode(empCode);
            var data = new IndAbsent
            {
                totalWorking = await employeePunchCardInfoService.GetAllAbsentCountByMonthAndYearAndEmpCode(month, year, empCode),
                totalAbsent = DateTime.DaysInMonth(year, month) - await employeePunchCardInfoService.GetAllAbsentCountByMonthAndYearAndEmpCode(month, year, empCode) - totalhoiday - totalLeaves,
                employeeInfo = emp,
                empAbsentDates = await employeePunchCardInfoService.GetAllAbsentByMonthAndYearAndEmpCode(month, year, empCode)
            };
            var model = new AllEmployeeLate
            {
                indAbsent = data
            };

            return View(model);
        }


        [AllowAnonymous]
        public IActionResult MyAbsentPdf(string empCode, int month, int year)
        {
            string fileName;
            string host = HttpContext.Request.Host.ToString();
            string scheme = Request.Scheme;
            string url = string.Empty;
            url = $"" + scheme + "://" + host + "/HRPMSAttendence/EmployeePunchCardInfo/MyAbsentPrintPage?empCode=" + empCode + "&month=" + month + "&year=" + year;

            string status = myPDF.GeneratePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }


        [AllowAnonymous]
        public async Task<IActionResult> AllAbsentPrintPage(int year, int month, int department, string designation, int shift)
        {
            ViewBag.year = year;
            var monthName = "";
            if (month == 1)
            {
                monthName = "January";
            }
            else if (month == 2)
            {
                monthName = "February";
            }
            else if (month == 3)
            {
                monthName = "March";
            }
            else if (month == 4)
            {
                monthName = "April";
            }
            else if (month == 5)
            {
                monthName = "May";
            }
            else if (month == 6)
            {
                monthName = "June";
            }
            else if (month == 7)
            {
                monthName = "July";
            }
            else if (month == 8)
            {
                monthName = "August";
            }
            else if (month == 9)
            {
                monthName = "September";
            }
            else if (month == 10)
            {
                monthName = "October";
            }
            else if (month == 11)
            {
                monthName = "November";
            }
            else
            {
                monthName = "December";
            }
            ViewBag.month = monthName;

            if (designation == "null")
            {
                designation = null;
            }
            var allemp = await employeePunchCardInfoService.GetAllFilteredEmployee(department, designation, shift);
            var lates = new List<EmployeeLateAttendance>();
            foreach (var item in allemp)
            {
                lates.Add(new EmployeeLateAttendance
                {
                    employee = item,

                    totalPresent = await employeePunchCardInfoService.GetTotalAttendanceByEmpCodeMonthAndYear(item.employeeCode, month, year),
                    totalAbsent = DateTime.DaysInMonth(year, month) - Convert.ToInt32(await employeePunchCardInfoService.GetTotalAttendanceByEmpCodeMonthAndYear(item.employeeCode, month, year))
                });
            }
            var active = await employeePunchCardInfoService.GetAllActiveCountByMonthAndYear(month, year);
            var model = new AllEmployeeLate
            {
                allActive = active,
                allAbsent = DateTime.DaysInMonth(year, month) - active,
                employeeLates = lates
            };
            if (department != 0)
            {
                model.departmentName = lates[0]?.employee?.department?.deptName;
            }
            if (lates[0]?.employee?.lastDesignation?.designationName != null)
            {
                model.designationName = lates[0]?.employee?.lastDesignation?.designationName;
            }
            if (shift != 0)
            {
                model.shiftName = lates[0]?.employee?.shiftGroup?.shiftName;
            }

            return View(model);
        }


        [AllowAnonymous]
        public IActionResult AllAbsentPdf(int year, int month, int department, string designation, int shift)
        {
            string fileName;
            string host = HttpContext.Request.Host.ToString();
            string scheme = Request.Scheme;
            string url = string.Empty;
            url = $"" + scheme + "://" + host + "/HRPMSAttendence/EmployeePunchCardInfo/AllAbsentPrintPage?year=" + year + "&month=" + month + "&department=" + department + "&designation=" + designation + "&shift=" + shift;

            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }


        public async Task<ActionResult> IndEmployeeAttendenceNew(string id, string code)
        {
            try
            {
                await employeePunchCardInfoService.ProcessAttendance(DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("dd/MM/yyyy"));
            }
            catch (Exception)
            {

            }
            AllEmployeeWithAttendence model = new AllEmployeeWithAttendence
            {
                employeeInfo = await employeePunchCardInfoService.GetEmpInfoByPunchCard(id),
                empAttendance = await employeePunchCardInfoService.GetDailyAttendenceByEmpCode(code),
            };

            return View(model);


        }
        public async Task<IActionResult> GetIndividualLateAttendenceByMonth(int month)
        {
            try
            {
                await employeePunchCardInfoService.ProcessAttendance(DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("dd/MM/yyyy"));
            }
            catch (Exception)
            {

            }
            string userName = HttpContext.User.Identity.Name;
            var punchCardInfo = await employeePunchCardInfoService.GetPunchCardByUserName(userName);

            var attendenceCount = await employeePunchCardInfoService.MonthlyAttendenceCountByUserAndPunchCard(userName, month);
            var tLate = await employeePunchCardInfoService.GetIndivisualTotalLateByMonthAndCard(month, punchCardInfo.punchCardNo);
            var tLeaveEmp = await employeePunchCardInfoService.GetIndivisualTotalLeaveByMonth(month);
            var tLeaveEmpWP = await employeePunchCardInfoService.GetIndivisualTotalLeaveByMonthWP(month);
            var tLeave = 0;
            foreach (var item in tLeaveEmp)
            {
                tLeave += Convert.ToInt32(item.isDelete);
            }
            var tLeaveWP = 0;
            foreach (var item in tLeaveEmpWP)
            {
                tLeaveWP += Convert.ToInt32(item.isDelete);
            }
            var dates = new List<DateTime>();

            for (var date = new DateTime(DateTime.Now.Year, month, 1); date.Month == month; date = date.AddDays(1))
            {
                dates.Add(date);
            }

            var data = new List<IndAttendenceByMonth>();

            foreach (var item in dates)
            {
                data.Add(new IndAttendenceByMonth
                {
                    Date = Convert.ToDateTime(item),
                    empAttendance = await employeePunchCardInfoService.GetAttendenceByDateAndPunchCard(item, punchCardInfo.punchCardNo)
                });
            }

            var model = new DailyAttendenceViewModel
            {
                employeeInfo = await employeePunchCardInfoService.GetEmployeeByUsername(userName),
                totalPresent = attendenceCount,
                totalAbsent = DateTime.DaysInMonth(DateTime.Now.Year, month) - attendenceCount,
                indAttendenceByMonths = data,
                totalLate = tLate,
                totalLeave = tLeave,
                totalLeaveWP = tLeaveWP

            };


            return Json(model);
        }
        public async Task<IActionResult> MyLateAttendance()
        {
            string userName = HttpContext.User.Identity.Name;
            var emp = await employeePunchCardInfoService.GetEmployeeByUsername(userName);
            ViewBag.Name = emp.nameEnglish;
            ViewBag.Code = emp.employeeCode;

            //string userName = HttpContext.User.Identity.Name;
            var user = await employeePunchCardInfoService.GetUserByUsername(userName);
            var model = new MyLateAttendanceViewModel
            {
                applicationUser = user
            };
            //MyLateAttendanceViewModel

            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> getIndividualAttendenceByMonthPrintPage(int month, string userName, int year)
        {
            var holidays = await personalInfoService.GetAllHolidaysByMonthYear(month, year);
            ViewBag.year = year;
            ViewBag.month = month;
            var monthName = "";
            if (month == 1)
            {
                monthName = "January";
            }
            else if (month == 2)
            {
                monthName = "February";
            }
            else if (month == 3)
            {
                monthName = "March";
            }
            else if (month == 4)
            {
                monthName = "April";
            }
            else if (month == 5)
            {
                monthName = "May";
            }
            else if (month == 6)
            {
                monthName = "June";
            }
            else if (month == 7)
            {
                monthName = "July";
            }
            else if (month == 8)
            {
                monthName = "August";
            }
            else if (month == 9)
            {
                monthName = "September";
            }
            else if (month == 10)
            {
                monthName = "October";
            }
            else if (month == 11)
            {
                monthName = "November";
            }

            else
            {
                monthName = "December";
            }
            ViewBag.month = monthName;

            var punchCardInfo = await employeePunchCardInfoService.GetPunchCardByUserName(userName);
            var emp = await employeePunchCardInfoService.GetEmployeeByUsername(userName);
            ViewBag.Name = emp.nameEnglish;
            ViewBag.Code = emp.employeeCode;

            var attendenceCount = await employeePunchCardInfoService.MonthlyAttendenceCountByUserAndPunchCard(userName, month);
            // var tPresent = await employeePunchCardInfoService.GetTotalPresentByMonthandYear(month, year);
            //var tLate = await employeePunchCardInfoService.GetTotalLateByMonth(month, punchCardInfo.punchCardNo, year);
            var tLeaveEmp = await employeePunchCardInfoService.GetTotalLeaveByMonth(month, year);
            //var tLeaveEmpWP = await employeePunchCardInfoService.GetTotalLeaveByMonthWP(month, year);
            var tLeave = 0;
            foreach (var item in tLeaveEmp)
            {
                tLeave += Convert.ToInt32(item.isDelete);
            }
            //var tLeaveWP = 0;
            //foreach (var item in tLeaveEmpWP)
            //{
            //	tLeaveWP += Convert.ToInt32(item.isDelete);
            //}
            var dates = new List<DateTime>();

            for (var date = new DateTime(year, month, 1); date.Month == month; date = date.AddDays(1))
            {
                dates.Add(date);
            }

            var data = new List<IndAttendenceByMonth>();

            foreach (var item in dates)
            {
                data.Add(new IndAttendenceByMonth
                {
                    Date = Convert.ToDateTime(item),
                    isHoliday = holidays.Contains(Convert.ToDateTime(item)) ? 1 : 0,
                    isLeave = await employeePunchCardInfoService.CheckLeaveByDate(item, punchCardInfo.punchCardNo),
                    empAttendance = await employeePunchCardInfoService.GetAttendenceByDateAndPunchCard(item, punchCardInfo.punchCardNo)
                });
            }
            var totalhoiday = await employeePunchCardInfoService.GetAllHolidaysByMonthYearNew(month, year);
            var totalLeaves = await employeePunchCardInfoService.GetTotalLeaveByMonthAndEmpCodeInd(punchCardInfo.punchCardNo, month, year);

            var model = new DailyAttendenceViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeInfo = await employeePunchCardInfoService.GetEmployeeByUsername(userName),
                totalPresent = attendenceCount,
                totalAbsent = DateTime.DaysInMonth(DateTime.Now.Year, month) - attendenceCount - totalhoiday - totalLeaves,
                indAttendenceByMonths = data,
                //totalLate = tLate,
                totalLeave = tLeave,
                //totalLeaveWP = tLeaveWP

            };


            return View(model);
        }


        [AllowAnonymous]
        public IActionResult getIndivisuaslAttendenceByMonthPdf(int month, int year)
        {
            string userName = HttpContext.User.Identity.Name;
            string fileName;
            string host = HttpContext.Request.Host.ToString();
            string scheme = Request.Scheme;
            string url = string.Empty;
            url = $"" + scheme + "://" + host + "/HRPMSAttendence/EmployeePunchCardInfo/getIndividualAttendenceByMonthPrintPage?month=" + month + "&userName=" + userName + "&year=" + year;

            string status = myPDF.GeneratePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        public IActionResult CheckHolidayByDate(DateTime? mydateh)
        {
            var data = personalInfoService.CheckHolidayByDate(mydateh);
            return Json(data);
        }

        [AllowAnonymous]
        public async Task<IActionResult> getIndAttendenceByMonthForAdminPrintPage(int month, string empCode)
        {
            ViewBag.month = month;
            var holidays = await personalInfoService.GetAllHolidaysByMonthYear(month, DateTime.Now.Year);
            var monthName = "";
            if (month == 1)
            {
                monthName = "January";
            }
            else if (month == 2)
            {
                monthName = "February";
            }
            else if (month == 3)
            {
                monthName = "March";
            }
            else if (month == 4)
            {
                monthName = "April";
            }
            else if (month == 5)
            {
                monthName = "May";
            }
            else if (month == 6)
            {
                monthName = "June";
            }
            else if (month == 7)
            {
                monthName = "July";
            }
            else if (month == 8)
            {
                monthName = "August";
            }
            else if (month == 9)
            {
                monthName = "September";
            }
            else if (month == 10)
            {
                monthName = "October";
            }
            else if (month == 11)
            {
                monthName = "November";
            }

            else
            {
                monthName = "December";
            }
            ViewBag.month = monthName;


            string userName = await employeePunchCardInfoService.GetUsernameByEMpCode(empCode);
            var punchCardInfo = await employeePunchCardInfoService.GetPunchCardByCode(empCode);
            //var emp = await employeePunchCardInfoService.GetEmployeeByUsername(userName);
            var empl = await personalInfoService.GetEmployeeInfoByCode(empCode);
            ViewBag.Name = empl.nameEnglish;
            ViewBag.Code = empl.employeeCode;

            var attendenceCount = await employeePunchCardInfoService.MonthlyPresentAttendanceByPunchCard(punchCardInfo.punchCardNo, month);
            //var tLate = await employeePunchCardInfoService.GetIndivisualTotalLateByMonth(month, punchCardInfo.punchCardNo);
            var tLeaveEmp = await employeePunchCardInfoService.GetIndivisualTotalLeaveByMonth(month);
            //var tLeaveEmpWP = await employeePunchCardInfoService.GetIndivisualTotalLeaveByMonthWP(month);
            var tLeave = 0;
            foreach (var item in tLeaveEmp)
            {
                tLeave += Convert.ToInt32(item.isDelete);
            }
            //var tLeaveWP = 0;
            //foreach (var item in tLeaveEmpWP)
            //{
            //	tLeaveWP += Convert.ToInt32(item.isDelete);
            //}
            var dates = new List<DateTime>();

            for (var date = new DateTime(DateTime.Now.Year, month, 1); date.Month == month; date = date.AddDays(1))
            {
                dates.Add(date);
            }

            var data = new List<IndAttendenceByMonth>();

            foreach (var item in dates)
            {
                data.Add(new IndAttendenceByMonth
                {
                    Date = Convert.ToDateTime(item),
                    isHoliday = holidays.Contains(Convert.ToDateTime(item)) ? 1 : 0,
                    isLeave = await employeePunchCardInfoService.CheckLeaveByDate(item, punchCardInfo.punchCardNo),
                    empAttendance = await employeePunchCardInfoService.GetAttendenceByDateAndPunchCard(item, punchCardInfo.punchCardNo)
                });
            }
            var totalhoiday = await employeePunchCardInfoService.GetHolidaysByMonth(month);
            var totalLeaves = await employeePunchCardInfoService.GetTotalLeaveByMonthAndEmpCode(punchCardInfo.punchCardNo, month);

            //new
            var model = new DailyAttendenceViewModel();

            model.companies = await eRPCompanyService.GetAllCompany();
            model.employeeInfo = await employeePunchCardInfoService.GetEmployeeByUsername(userName);
            model.totalPresent = attendenceCount;
            model.totalAbsent = DateTime.DaysInMonth(DateTime.Now.Year, month) - attendenceCount - totalhoiday - totalLeaves;
            model.indAttendenceByMonths = data;
            model.totalLeave = tLeave;
            


            //old
            //var model = new DailyAttendenceViewModel
            //{
            //    companies = await eRPCompanyService.GetAllCompany(),
            //    employeeInfo = await employeePunchCardInfoService.GetEmployeeByUsername(userName),
            //    totalPresent = attendenceCount,
            //    totalAbsent = DateTime.DaysInMonth(DateTime.Now.Year, month) - attendenceCount - totalhoiday - totalLeaves,
            //    indAttendenceByMonths = data,
            //    totalLeave = tLeave,
            //};


            return View(model);
        }

        [AllowAnonymous]
        public IActionResult getIndlAttendenceByMonthForAdminPdf(int month, string empCode)
        {
            string userName = HttpContext.User.Identity.Name;
            string fileName;
            string host = HttpContext.Request.Host.ToString();
            string scheme = Request.Scheme;
            string url = string.Empty;
            url = $"" + scheme + "://" + host + "/HRPMSAttendence/EmployeePunchCardInfo/getIndAttendenceByMonthForAdminPrintPage?month=" + month + "&empCode=" + empCode;

            string status = myPDF.GeneratePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [AllowAnonymous]
        public async Task<IActionResult> getLateAttendancePrintPage(int month, string empCode)
        {
            ViewBag.month = month;
            var monthName = "";
            if (month == 1)
            {
                monthName = "January";
            }
            else if (month == 2)
            {
                monthName = "February";
            }
            else if (month == 3)
            {
                monthName = "March";
            }
            else if (month == 4)
            {
                monthName = "April";
            }
            else if (month == 5)
            {
                monthName = "May";
            }
            else if (month == 6)
            {
                monthName = "June";
            }
            else if (month == 7)
            {
                monthName = "July";
            }
            else if (month == 8)
            {
                monthName = "August";
            }
            else if (month == 9)
            {
                monthName = "September";
            }
            else if (month == 10)
            {
                monthName = "October";
            }
            else if (month == 11)
            {
                monthName = "November";
            }

            else
            {
                monthName = "December";
            }
            ViewBag.month = monthName;
            string userName = await employeePunchCardInfoService.GetUsernameByEMpCode(empCode);
            var punchCardInfo = await employeePunchCardInfoService.GetPunchCardByUserName(userName);
            var emp = await employeePunchCardInfoService.GetEmployeeByUsername(userName);
            ViewBag.Name = emp.nameEnglish;
            ViewBag.Code = emp.employeeCode;

            var attendenceCount = await employeePunchCardInfoService.MonthlyAttendenceCountByUserAndPunchCard(userName, month);
            var tLate = await employeePunchCardInfoService.GetIndivisualTotalLateByMonth(month);
            var tLeaveEmp = await employeePunchCardInfoService.GetIndivisualTotalLeaveByMonth(month);
            var tLeaveEmpWP = await employeePunchCardInfoService.GetIndivisualTotalLeaveByMonthWP(month);
            var tLeave = 0;
            foreach (var item in tLeaveEmp)
            {
                tLeave += Convert.ToInt32(item.isDelete);
            }
            var tLeaveWP = 0;
            foreach (var item in tLeaveEmpWP)
            {
                tLeaveWP += Convert.ToInt32(item.isDelete);
            }
            var dates = new List<DateTime>();

            for (var date = new DateTime(DateTime.Now.Year, month, 1); date.Month == month; date = date.AddDays(1))
            {
                dates.Add(date);
            }

            var data = new List<IndAttendenceByMonth>();

            foreach (var item in dates)
            {
                data.Add(new IndAttendenceByMonth
                {
                    Date = Convert.ToDateTime(item),
                    empAttendance = await employeePunchCardInfoService.GetAttendenceByDateAndPunchCard(item, punchCardInfo.punchCardNo)
                });
            }

            var model = new DailyAttendenceViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                employeeInfo = await employeePunchCardInfoService.GetEmployeeByUsername(userName),
                totalPresent = attendenceCount,
                totalAbsent = DateTime.DaysInMonth(DateTime.Now.Year, month) - attendenceCount,
                indAttendenceByMonths = data,
                totalLate = tLate,
                totalLeave = tLeave,
                totalLeaveWP = tLeaveWP

            };


            return View(model);
        }
        [AllowAnonymous]
        public IActionResult getLateAttendancePdf(int month, string empCode)
        {
            string userName = HttpContext.User.Identity.Name;
            string fileName;
            string host = HttpContext.Request.Host.ToString();
            string scheme = Request.Scheme;
            string url = string.Empty;
            url = $"" + scheme + "://" + host + "/HRPMSAttendence/EmployeePunchCardInfo/getLateAttendancePrintPage?month=" + month + "&empCode=" + empCode;

            string status = myPDF.GeneratePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [AllowAnonymous]
        public IActionResult getDailyAttendenceByMonthPdf()
        {
            string fileName;
            string host = HttpContext.Request.Host.ToString();
            string scheme = Request.Scheme;
            string url = string.Empty;
            url = $"" + scheme + "://" + host + "/HRPMSAttendence/EmployeePunchCardInfo/DailyAttendenceSheetPdf";

            string status = myPDF.GeneratePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [AllowAnonymous]
        public async Task<ActionResult> getAdminDailyAttendenceByDatePrintpage(string type, string date, int dbd, string designation, int shift)
        {
            IEnumerable<EmployeeInfo> empList = Enumerable.Empty<EmployeeInfo>();
            var designations = await personalInfoService.GetAllDesignation();

            ViewBag.date = date;
            ViewBag.designation = designation;
            if (type == "department")
            {
                empList = await employeePunchCardInfoService.GetAllEmployeeWithPunchCardNo(dbd, designation, shift);
            }
            if (type == "branch")
            {
                empList = await employeePunchCardInfoService.GetAllEmployeeWithPunchCardNoBranch(dbd, designation, shift);
            }
            if (type == "division")
            {
                empList = await employeePunchCardInfoService.GetAllEmployeeWithPunchCardNoDivision(dbd, designation, shift);
            }
            if (type == "zone")
            {
                empList = await employeePunchCardInfoService.GetAllEmployeeWithPunchCardNoZoneNew(dbd, designation, shift);
            }
            if (type == "office")
            {
                empList = await employeePunchCardInfoService.GetAllEmployeeWithPunchCardNoOffice(dbd, designation, shift);
            }

            var emp = new List<AllEmployeeWithAttendence>();
            //var allPresent = await employeePunchCardInfoService.GetAllEmployeeAttendenceCountByDateRange(Convert.ToDateTime(date).Date, department, designation, shift);
            var tLate = await employeePunchCardInfoService.GetDailyTotalLateByMonth(Convert.ToDateTime(date).Date);
            var tLeaveEmp = await employeePunchCardInfoService.GetTotalLeaveByDate(Convert.ToDateTime(date).Date);


            //var tLeaveEmpWP = await employeePunchCardInfoService.GetTotalLeaveByDateWP(DateTime.Now.Date);
            var count = 0;
            var present = 0;
            foreach (var item in empList)
            {
                count++;
            }
            var tLeave = 0;
            foreach (var item in tLeaveEmp)
            {
                tLeave += Convert.ToInt32(item.isDelete);
            }

            foreach (var item in empList)
            {
                emp.Add(new AllEmployeeWithAttendence
                {
                    employeeInfo = item,
                    designation = designations.Where(x => x.Id == item.lastDesignationId).Select(x => x.designationName).FirstOrDefault(),
                    isLeaved = await employeePunchCardInfoService.GetLeaveStatusByEmpCodeAndDate(item.employeeCode, date),
                    empAttendance = await employeePunchCardInfoService.GetDailyAttendenceByEmpCodeDate(item.employeeCode, date)
                });
            }
            foreach (var item in emp)
            {
                if (item.empAttendance != null)
                {
                    present++;
                }
            }
            var model = new DailyAttendenceViewModel
            {
                companies = await eRPCompanyService.GetAllCompany(),
                departments = await employeePunchCardInfoService.GetAllDepartment(),
                shifts = await employeePunchCardInfoService.GetAllShift(),
                zone = await employeePunchCardInfoService.GetAllZone(),
                Office = await employeePunchCardInfoService.GetAllOffice(),
                employeeInfoDepts = await employeePunchCardInfoService.GetAllEmployeeDept(),
                totalPresent = present,
                totalAbsent = count - present,
                totalLeave = tLeave,
                DailyAttendence = emp,
                totalLate = tLate,
                employeeInfo = await employeePunchCardInfoService.GetEmployeeInfoByDeptDesigShift(Convert.ToInt32(emp[0].employeeInfo.departmentId), emp[0].employeeInfo.designation, Convert.ToInt32(emp[0].employeeInfo.shiftGroupId))
            };

            if (type == "department")
            {
                model.departmentName = empList.Select(x => x.department?.deptName).FirstOrDefault();
            }
            if (type == "branch")
            {
                model.departmentName = empList.Select(x => x.hrBranch?.branchName).FirstOrDefault();
            }
            if (type == "division")
            {
                model.departmentName = empList.Select(x => x.hrDivision?.divName).FirstOrDefault();
            }
            if (type == "zone")
            {
                model.zoneName = empList.Select(x => x.location?.branchUnitName).FirstOrDefault();
            }
            if (type == "office")
            {
                model.officeName = empList.Select(x => x.functionInfo?.branchUnitName).FirstOrDefault();
            }


            if (model.employeeInfo?.lastDesignation?.designationName != "0")
            {
                model.designationName = emp[0].employeeInfo?.lastDesignation?.designationName;
            }
            else
            {
                model.designationName = null;
            }

            if (shift != 0)
            {
                model.shiftName = await employeePunchCardInfoService.GetShiftNameById(shift);
            }
            else
            {
                model.shiftName = null;
            }
            return View(model);


        }

        [AllowAnonymous]
        public IActionResult getAdminDailyAttendenceByDatePdf(string type, string date, int dbdId, string designation, int shift)
        {
            if (designation == null)
            {
                designation = "0";
            }
            string fileName;
            string host = HttpContext.Request.Host.ToString();
            string scheme = Request.Scheme;
            string url = string.Empty;
            if (type == "department")
            {
                url = $"" + scheme + "://" + host + "/HRPMSAttendence/EmployeePunchCardInfo/getAdminDailyAttendenceByDatePrintpage?type=department&date=" + date + "&dbd=" + dbdId + "&designation=" + designation + "&shift=" + shift;
            }
            if (type == "branch")
            {
                url = $"" + scheme + "://" + host + "/HRPMSAttendence/EmployeePunchCardInfo/getAdminDailyAttendenceByDatePrintpage?type=branch&date=" + date + "&dbd=" + dbdId + "&designation=" + designation + "&shift=" + shift;
            }
            if (type == "division")
            {
                url = $"" + scheme + "://" + host + "/HRPMSAttendence/EmployeePunchCardInfo/getAdminDailyAttendenceByDatePrintpage?type=division&date=" + date + "&dbd=" + dbdId + "&designation=" + designation + "&shift=" + shift;
            }
            if (type == "zone")
            {
                url = $"" + scheme + "://" + host + "/HRPMSAttendence/EmployeePunchCardInfo/getAdminDailyAttendenceByDatePrintpage?type=zone&date=" + date + "&dbd=" + dbdId + "&designation=" + designation + "&shift=" + shift;
            }
            if (type == "office")
            {
                url = $"" + scheme + "://" + host + "/HRPMSAttendence/EmployeePunchCardInfo/getAdminDailyAttendenceByDatePrintpage?type=office&date=" + date + "&dbd=" + dbdId + "&designation=" + designation + "&shift=" + shift;
            }


            string status = myPDF.GeneratePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [AllowAnonymous]
        public async Task<IActionResult> LateAttendenceByMonthPrintPage(int year, int month, int department, string designation, int shift)
        {
            var monthName = "";
            if (month == 1)
            {
                monthName = "January";
            }
            else if (month == 2)
            {
                monthName = "February";
            }
            else if (month == 3)
            {
                monthName = "March";
            }
            else if (month == 4)
            {
                monthName = "April";
            }
            else if (month == 5)
            {
                monthName = "May";
            }
            else if (month == 6)
            {
                monthName = "June";
            }
            else if (month == 7)
            {
                monthName = "July";
            }
            else if (month == 8)
            {
                monthName = "August";
            }
            else if (month == 9)
            {
                monthName = "September";
            }
            else if (month == 10)
            {
                monthName = "October";
            }
            else if (month == 11)
            {
                monthName = "November";
            }

            else
            {
                monthName = "December";
            }
            ViewBag.month = monthName;
            ViewBag.year = year;
            var empAtt = new List<EmployeeWithAtt>();
            var employees = await employeePunchCardInfoService.GetAllFilteredEmployee(department, designation, shift);
            var activecount = await employeePunchCardInfoService.FilteredEmployeeCount(department, designation, shift);
            var latecount = await employeePunchCardInfoService.FilteredEmployeeLateCount(year, month, department, designation, shift);
            foreach (var item in employees)
            {
                empAtt.Add(new EmployeeWithAtt
                {
                    employeeInfo = item,
                    totalWorks = await employeePunchCardInfoService.GetTotalWorkingDaysByEmpId(item.Id, year, month),
                    totalLate = await employeePunchCardInfoService.GetTotalLateByEmpId(item.Id, year, month)
                });
            }
            var data = new AllEmployeeLateViewModel
            {
                totalActive = activecount,
                totalLate = latecount,
                department = await employeePunchCardInfoService.GetAllDepartment(),
                shift = await employeePunchCardInfoService.GetAllShift(),
                employeeInfoDept = await employeePunchCardInfoService.GetAllEmployeeDept(),
                empAttendances = empAtt
            };
            if (department != 0)
            {
                data.departmentName = await employeePunchCardInfoService.GetDepartmentNameById(department);
            }
            else
            {
                data.departmentName = null;
            }

            if (designation != "0")
            {
                data.designationName = empAtt[0].employeeInfo.designation;
            }
            else
            {
                data.designationName = null;
            }
            ViewBag.designationName = data.designationName;
            if (shift != 0)
            {
                data.shiftName = await employeePunchCardInfoService.GetShiftNameById(shift);
            }
            else
            {
                data.shiftName = null;
            }
            return View(data);
        }

        [AllowAnonymous]
        public async Task<IActionResult> AbsentAttendenceByMonthPdfPrintPage(int year, int month, int department, string designation, int shift)
        {
            var monthName = "";
            if (month == 1)
            {
                monthName = "January";
            }
            else if (month == 2)
            {
                monthName = "February";
            }
            else if (month == 3)
            {
                monthName = "March";
            }
            else if (month == 4)
            {
                monthName = "April";
            }
            else if (month == 5)
            {
                monthName = "May";
            }
            else if (month == 6)
            {
                monthName = "June";
            }
            else if (month == 7)
            {
                monthName = "July";
            }
            else if (month == 8)
            {
                monthName = "August";
            }
            else if (month == 9)
            {
                monthName = "September";
            }
            else if (month == 10)
            {
                monthName = "October";
            }
            else if (month == 11)
            {
                monthName = "November";
            }

            else
            {
                monthName = "December";
            }
            ViewBag.month = monthName;
            ViewBag.year = year;
            var allemp = await employeePunchCardInfoService.GetAllFilteredEmployee(department, designation, shift);
            var lates = new List<EmployeeLateAttendance>();
            foreach (var item in allemp)
            {
                lates.Add(new EmployeeLateAttendance
                {
                    employee = item,

                    totalPresent = await employeePunchCardInfoService.GetTotalAttendanceByEmpCodeMonthAndYear(item.employeeCode, month, year),
                    totalAbsent = DateTime.DaysInMonth(year, month) - await employeePunchCardInfoService.GetTotalAttendanceByEmpCodeMonthAndYear(item.employeeCode, month, year)
                });
            }
            var active = await employeePunchCardInfoService.GetAllActiveCountByMonthAndYear(month, year);

            var model = new AllEmployeeLate
            {
                allActive = active,
                allAbsent = DateTime.DaysInMonth(month, year) - active,
                employeeLates = lates,
                department = await employeePunchCardInfoService.GetAllDepartment(),
                shift = await employeePunchCardInfoService.GetAllShift(),
                employeeInfoDept = await employeePunchCardInfoService.GetAllEmployeeDept()
            };
            if (department != 0)
            {
                model.departmentName = await employeePunchCardInfoService.GetDepartmentNameById(department);
            }
            else
            {
                model.departmentName = null;
            }

            if (designation != "0")
            {
                model.designationName = lates[0].employee.designation;
            }
            else
            {
                model.designationName = null;
            }

            if (shift != 0)
            {
                model.shiftName = await employeePunchCardInfoService.GetShiftNameById(shift);
            }
            else
            {
                model.shiftName = null;
            }
            return View(model);
        }


        [AllowAnonymous]
        public IActionResult LateAttendenceByMonthPdf(int month, int year, int department, string designation, int shift)
        {
            if (designation == null)
            {
                designation = "0";
            }
            string fileName;
            string host = HttpContext.Request.Host.ToString();
            string scheme = Request.Scheme;
            string url = string.Empty;
            url = $"" + scheme + "://" + host + "/HRPMSAttendence/EmployeePunchCardInfo/LateAttendenceByMonthPrintPage?month=" + month + "&year=" + year + "&department=" + department + "&designation=" + designation + "&shift=" + shift;

            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        [AllowAnonymous]
        public IActionResult AbsentAttendenceByMonthPdf(int month, int year, int department, string designation, int shift)
        {
            if (designation == null)
            {
                designation = "0";
            }
            string fileName;
            string host = HttpContext.Request.Host.ToString();
            string scheme = Request.Scheme;
            string url = string.Empty;
            url = $"" + scheme + "://" + host + "/HRPMSAttendence/EmployeePunchCardInfo/AbsentAttendenceByMonthPdfPrintPage?month=" + month + "&year=" + year + "&department=" + department + "&designation=" + designation + "&shift=" + shift;

            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        [AllowAnonymous]
        public IActionResult MonthWiseEmployeeAttendancePdf(int year, int month)
        {
            string fileName;
            string host = HttpContext.Request.Host.ToString();
            string scheme = Request.Scheme;
            string url = string.Empty;
            url = $"" + scheme + "://" + host + "/HRPMSAttendence/EmployeePunchCardInfo/MonthWiseEmployeeAttendanceView?year=" + year + "&month=" + month;

            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }
        [AllowAnonymous]
        public IActionResult MonthWiseEmployeeAttendancePdf1(DateTime? fromDate, DateTime? toDate)
        {
            string fileName;
            string host = HttpContext.Request.Host.ToString();
            string scheme = Request.Scheme;
            string url = string.Empty;
            url = $"" + scheme + "://" + host + "/HRPMSAttendence/EmployeePunchCardInfo/MonthWiseEmployeeAttendanceView1?fromDate=" + fromDate + "&toDate=" + toDate;

            string status = myPDF.GenerateLandscapePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        public async Task<ActionResult> MachineAttendance()
        {
            //var date = EnglishToBanglaNumber.EnglishToBanglaDate(DateTime.Now);
            EmployeePunchCardInfoViewModel model = new EmployeePunchCardInfoViewModel
            {
                //employeePunchCardInfoslist = await employeePunchCardInfoService.GetAllEmployeePunchCardInfo(),
                //employeePunchCardInfoslistNew = await employeePunchCardInfoService.GetAllEmployeeWithPunchCardInfo(),
                //shiftGroupMasterslist = await shiftGroupMasterService.GetAllShiftGroupMaster(),
                //visualEmpCodeName = await personalInfoService.GetEmpCodeNameVisualData()
            };
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> MachineAttendanceApi(string Fdate, string Tdate)
        {
            var from = Convert.ToDateTime(Fdate).Date;
            var to = Convert.ToDateTime(Tdate).Date;
            var data = await employeePunchCardInfoService.ProcessAttendance(from.ToString("dd/MM/yyyy"), to.ToString("dd/MM/yyyy"));
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> MonthWiseEmployeeAttendanceBySPApi(string fromDate, string toDate)
        {

            var model = await employeePunchCardInfoService.GetEmployeeAttendanceBySp(fromDate, toDate);


            return Json(model);
        }

        public async Task<IActionResult> DateWiseEmployeeAttendance(DateTime? startDate, DateTime? endDate)
        {
            var emp = await personalInfoService.GetAllEmployeeInfoJson();
            var model = new List<MonthWiseViewModel>();

            foreach (var item in emp)
            {
                model.Add(new MonthWiseViewModel
                {
                    employee = item,
                    totalPresent = await employeePunchCardInfoService.GetTotalPresentByDateAndEmpCode(startDate, endDate, item.employeeCode),
                    totalAbsent = await employeePunchCardInfoService.GetTotalAbsentByDateAndEmpCode(startDate, endDate, item.employeeCode),
                    totalOnLeave = await employeePunchCardInfoService.GetTotalLeaveByDateAndEmpCode(startDate, endDate, item.employeeCode),
                    totalHolidays = await employeePunchCardInfoService.GetTotalHolidayByDateAndEmpCode(startDate, endDate),
                    actuallyDays = (Convert.ToDateTime(endDate) - Convert.ToDateTime(startDate)).Days + 1,
                    totalWorkingDays = await employeePunchCardInfoService.GetTotalWorkByDateAndEmpCode(startDate, endDate, item.employeeCode),
                    average = await employeePunchCardInfoService.GetAverageByDateAndEmpCode(startDate, endDate, item.employeeCode),
                });
            }
            return Json(model);
        }


        public async Task<IActionResult> MonthWiseEmployeeAttendanceView(int year, int month)
        {
            ViewBag.year = year;
            ViewBag.month = month == 1 ? "January" : (month == 2 ? "February" : (month == 3 ? "March" : (month == 4 ? "April" : (month == 5 ? "May" : (month == 6 ? "June" : (month == 7 ? "July" : (month == 8 ? "August" : (month == 9 ? "September" : (month == 10 ? "October" : (month == 11 ? "November" : "December"))))))))));
            var emp = await personalInfoService.GetAllEmployeeInfoJson();
            var model = new List<MonthWiseViewModel>();

            foreach (var item in emp)
            {
                model.Add(new MonthWiseViewModel
                {
                    employee = item,
                    totalPresent = await employeePunchCardInfoService.GetTotalPresentByMonthYearAndEmpCode(month, year, item.employeeCode),
                    totalAbsent = await employeePunchCardInfoService.GetTotalAbsentByMonthYearAndEmpCode(month, year, item.employeeCode),
                    totalOnLeave = await employeePunchCardInfoService.GetTotalLeaveByMonthYearAndEmpCode(month, year, item.employeeCode),
                    totalHolidays = await employeePunchCardInfoService.GetTotalHolidayByMonthYearAndEmpCode(month, year),
                    actuallyDays = DateTime.DaysInMonth(year, month),
                    totalWorkingDays = await employeePunchCardInfoService.GetTotalWorkByMonthYearAndEmpCode(month, year, item.employeeCode),
                    average = await employeePunchCardInfoService.GetAverageByMonthYearAndEmpCode(month, year, item.employeeCode),
                });
            }
            var model1 = new MonthWiseAttendance
            {
                monthWiseViewAttendance = model
            };
            return View(model1);
        }
        public async Task<IActionResult> MonthWiseEmployeeAttendanceViewold(DateTime? startDate, DateTime? endDate)
        {
            ViewBag.startDate = Convert.ToDateTime(startDate);
            ViewBag.endDate = Convert.ToDateTime(endDate);
            var emp = await personalInfoService.GetAllEmployeeInfoJson();
            var model = new List<MonthWiseViewModel>();

            foreach (var item in emp)
            {
                model.Add(new MonthWiseViewModel
                {
                    employee = item,
                    totalPresent = await employeePunchCardInfoService.GetTotalPresentByDateAndEmpCode(startDate, endDate, item.employeeCode),
                    totalAbsent = await employeePunchCardInfoService.GetTotalAbsentByDateAndEmpCode(startDate, endDate, item.employeeCode),
                    totalOnLeave = await employeePunchCardInfoService.GetTotalLeaveByDateAndEmpCode(startDate, endDate, item.employeeCode),
                    totalHolidays = await employeePunchCardInfoService.GetTotalHolidayByDateAndEmpCode(startDate, endDate),
                    actuallyDays = (Convert.ToDateTime(endDate) - Convert.ToDateTime(startDate).Date).Days,
                    totalWorkingDays = await employeePunchCardInfoService.GetTotalWorkByDateAndEmpCode(startDate, endDate, item.employeeCode),
                    average = await employeePunchCardInfoService.GetAverageByDateAndEmpCode(startDate, endDate, item.employeeCode),
                });
            }
            var model1 = new MonthWiseAttendance
            {
                monthWiseViewAttendance = model
            };
            return View(model1);
        }
        [HttpGet]
        public async Task<IActionResult> MonthWiseEmployeeAttendanceView1(string fromDate, string toDate)
        {
            ViewBag.startDate = Convert.ToDateTime(fromDate);
            ViewBag.endDate = Convert.ToDateTime(toDate);

            var model1 = new MonthWiseAttendance
            {
                allEmployeeAttendanceVM = await employeePunchCardInfoService.GetEmployeeAttendanceBySp(fromDate, toDate)
            };

            return View(model1);
        }

        public async Task<ActionResult> MonthWiseAttendance()
        {
            try
            {
                await employeePunchCardInfoService.ProcessAttendance(DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("dd/MM/yyyy"));
            }
            catch (Exception)
            {

            }
            MonthWiseViewModel model = new MonthWiseViewModel
            {

            };
            return View(model);
        }

        public async Task<ActionResult> MonthWiseAttendanceBySp()
        {
            try
            {
                await employeePunchCardInfoService.ProcessAttendance(DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("dd/MM/yyyy"));
            }
            catch (Exception)
            {

            }
            MonthWiseViewModel model = new MonthWiseViewModel
            {

            };
            return View(model);
        }




        #region API Section
        [Route("global/api/shiftgroupmasters")]
        [HttpGet]
        public async Task<IActionResult> ProcessAttendance()
        {
            return Json(await shiftGroupMasterService.GetAllShiftGroupMaster());
        }


        [Route("global/api/GetEmployeePunchCardInfoByEmpCode/{empCode}")]
        [HttpGet]
        public async Task<IActionResult> GetEmployeePunchCardInfoByEmpCode(string empCode)
        {
            return Json(await employeePunchCardInfoService.GetEmployeePunchCardInfoByByEmpCode(empCode));
        }


        [Route("global/api/GetEmployeePunchCardList")]
        [HttpGet]
        public async Task<IActionResult> GetEmployeePunchCardList()
        {
            return Json(await employeePunchCardInfoService.GetAllEmployeePunchCardInfo());
        }

        [Route("global/api/ManualAttendanceByDate/{workingDate}")]
        public async Task<ActionResult> ManualAttendanceByDate(string workingDate)
        {
            var AttenceList = await employeePunchCardInfoService.GetAllManualAttendenceByDateFilter(workingDate);
            var empAtt = new List<EmpWithManualAttendence>();

            foreach (var item in AttenceList)
            {
                empAtt.Add(new EmpWithManualAttendence
                {
                    empAttendance = item,
                    employeeInfo = await employeePunchCardInfoService.GetEmpInfoByPunchCard(item.punchCardNo)
                });
            };
            var model = new EmpAttendanceViewModel
            {
                empAttendances = empAtt
            };

            return Json(model);
        }



        [HttpPost]

        public async Task<IActionResult> SavePunchCardInfoData([FromBody] EmployeePunchCardInfo model)
        {
            try
            {

                await employeePunchCardInfoService.SaveEmployeePunchCardInfo(model);
                return Json(true);
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }


        public async Task<IActionResult> LunchSubsidyReportView(int month, int year, string type, int typeId)
        {

            //Asad Added On 05.08.2023
            try
            {
                await employeePunchCardInfoService.ProcessAttendance(DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("dd/MM/yyyy"));
            }
            catch (Exception ex)
            {
            }

            ViewBag.branchName = await employeePunchCardInfoService.GetBranchNameBanglaByType(type, typeId);
            ViewBag.type = type;
            var model = new List<SubsidyViewModel>();

            //var emp = await employeePunchCardInfoService.GetEmployeesByTypeNew(type, typeId, month, year);

            ViewBag.finalEmp = await employeePunchCardInfoService.GetEmployeesByTypeAndTypeId(type, typeId);
            ViewBag.month = month == 1 ? "জানুয়ারি" : (month == 2 ? "ফেব্রুয়ারী" : (month == 3 ? "মার্চ" : (month == 4 ? "এপ্রিল" : (month == 5 ? "মে " : (month == 6 ? "জুন" : (month == 7 ? "জুলাই" : (month == 8 ? "আগষ্ট" : (month == 9 ? "সেপ্টেম্বর" : (month == 10 ? "অক্টোবর" : (month == 11 ? "নভেম্বর" : "ডিসেম্বর"))))))))));
            ViewBag.year = year;


            var emp = await employeePunchCardInfoService.GetLunchSubsidy(type, typeId, year, month);


            foreach (var item in emp)
            {
                var data = new SubsidyViewModel
                {
                    employee = await employeePunchCardInfoService.GetEmployeeByEmpCode(item.EmployeeCode),
                    attendDays = item.AttendDays,
                    amount = item.Amount
                };
                if (data.attendDays > 0)
                {
                    model.Add(data);
                }
            }

            //foreach (var item in emp)
            //{
            //    var attend = employeePunchCardInfoService.GetTotalSubsideryDaysByEmpCodeBranchMonthAndYear(item.employeeCode, typeId, month, year);
            //    var data = new SubsidyViewModel
            //    {
            //        employee = item,
            //        attendDays = attend,
            //        amount = attend * Convert.ToDecimal(200.00)
            //    };
            //    if (data.attendDays > 0)
            //    {
            //        model.Add(data);
            //    }
            //}


            //Asad Commrnted On 05.08.2023
            //try
            //{
            //    await employeePunchCardInfoService.ProcessAttendance(DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("dd/MM/yyyy"));
            //}
            //catch (Exception ex)
            //{
            //}

            //ViewBag.branchName = await employeePunchCardInfoService.GetBranchNameBanglaByType(type, typeId);
            //ViewBag.type = type;        
            //var model = new List<SubsidyViewModel>();
            //var emp = await employeePunchCardInfoService.GetEmployeesByTypeNew(type, typeId, month, year);
            //ViewBag.finalEmp = await employeePunchCardInfoService.GetEmployeesByTypeAndTypeId(type, typeId);
            //ViewBag.month = month == 1 ? "জানুয়ারি" : (month == 2 ? "ফেব্রুয়ারী" : (month == 3 ? "মার্চ" : (month == 4 ? "এপ্রিল" : (month == 5 ? "মে " : (month == 6 ? "জুন" : (month == 7 ? "জুলাই" : (month == 8 ? "আগষ্ট" : (month == 9 ? "সেপ্টেম্বর" : (month == 10 ? "অক্টোবর" : (month == 11 ? "নভেম্বর" : "ডিসেম্বর"))))))))));
            //ViewBag.year = year;
            //foreach (var item in emp)
            //{
            //    var attend = employeePunchCardInfoService.GetTotalSubsideryDaysByEmpCodeBranchMonthAndYear(item.employeeCode, typeId, month, year);
            //    var data = new SubsidyViewModel
            //    {
            //        employee = item,
            //        attendDays = attend,
            //        amount = attend * Convert.ToDecimal(200.00)
            //    };
            //    if (data.attendDays > 0)
            //    {
            //        model.Add(data);
            //    }
            //}



            return View(model);
        }

        [AllowAnonymous]
        public IActionResult LunchSubsidyReportPDF(int month, int year, string type, int typeId)
        {
            string fileName;
            string host = HttpContext.Request.Host.ToString();
            string scheme = Request.Scheme;
            string url = string.Empty;
            url = $"" + scheme + "://" + host + "/HRPMSAttendence/EmployeePunchCardInfo/LunchSubsidyReportView?month=" + month + "&year=" + year + "&type=" + type + "&typeID=" + typeId;

            string status = myPDF.GeneratePDF(out fileName, url);

            if (status != "done")
            {
                return Content("<h1>Something Went Wrong</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        #endregion
        public async Task<IActionResult> LunchSubsidy()
        {
            var username = HttpContext.User.Identity.Name;
            var userInfo = await personalInfoService.GetUserAllInfoByEmpCode(username);
            var model = new LunchPageViewModel
            {
                userInfo = userInfo
            };

            return View(model);
        }
        public async Task<IActionResult> GetAllDepartments()
        {
            var data = await personalInfoService.GetAllDepartment();
            return Json(data);
        }
        public async Task<IActionResult> GetAllBranch()
        {
            var data = await personalInfoService.GetAllBranch();
            return Json(data);
        }
        public async Task<IActionResult> GetAllZone()
        {
            var data = await personalInfoService.GetAllZone();
            return Json(data);
        }
        public async Task<IActionResult> GetAlloffice()
        {
            var data = await personalInfoService.GetAllOffices();
            return Json(data);
        }

        #region MonthAttendance_BDBL
        public async Task<ActionResult> MonthlyAttendanceBDBL()
        {
            try
            {
                await employeePunchCardInfoService.ProcessAttendance(DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("dd/MM/yyyy"));
            }
            catch (Exception)
            {

            }

            AllEmployeeWithAttendence model = new AllEmployeeWithAttendence
            {
                department = await employeePunchCardInfoService.GetAllDepartment(),
                employeeInfos = await employeePunchCardInfoService.GetAllEmployee(),
                hrBranches = await employeePunchCardInfoService.GetAllHrBranches(),
                zone = await employeePunchCardInfoService.GetAllZone(),
                Office = await employeePunchCardInfoService.GetAllOffice(),
            };
            return View(model);
        }


        public async Task<ActionResult> MonthlyAttendanceInTime()
        {
            try
            {
                await employeePunchCardInfoService.ProcessAttendance(DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("dd/MM/yyyy"));
            }
            catch (Exception)
            {

            }

            AllEmployeeWithAttendence model = new AllEmployeeWithAttendence
            {
                department = await employeePunchCardInfoService.GetAllDepartment(),
                employeeInfos = await employeePunchCardInfoService.GetAllEmployee(),
                hrBranches = await employeePunchCardInfoService.GetAllHrBranches(),
                zone = await employeePunchCardInfoService.GetAllZone(),
                Office = await employeePunchCardInfoService.GetAllOffice(),
                hrUnits = await employeePunchCardInfoService.GetAllUnits(),
                hrDivitions = await employeePunchCardInfoService.GetAllHrDivisions(),
            };
            return View(model);
        }

        #endregion


        public async Task<ActionResult> OverTimeReport(string id)
        {
            DailyAttendenceViewModel model = new DailyAttendenceViewModel
            {
                departments = await employeePunchCardInfoService.GetAllDepartment(),
                hrBranches = await employeePunchCardInfoService.GetAllHrBranches(),
                hrDivisions = await employeePunchCardInfoService.GetAllHrDivisions(),
                employeeInfosd = await employeePunchCardInfoService.GetAllEmployee(),
                employeeInfoDepts = await employeePunchCardInfoService.GetAllEmployeeDept(),
                employeeInfo = await employeePunchCardInfoService.GetEmpInfoByPunchCard(id),
                zone = await employeePunchCardInfoService.GetAllZone(),
                Office = await employeePunchCardInfoService.GetAllOffice(),


            };
            return View(model);

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult OverTimeReportPdf(DateTime fdate, DateTime todate, int depId, int branchId, int divisionId, int zoneId, int officeId, int employeeInfoId)
        {

            string scheme = Request.Scheme;
            var host = Request.Host;
            string url = "";
            string fileName;
            url = scheme + "://" + host + "/HRPMSAttendence/EmployeePunchCardInfo/OverTimeReportview?fdate=" + fdate + "&todate=" + todate + "&depId=" + depId + "&branchId=" + branchId + "&divisionId=" + divisionId + "&zoneId=" + zoneId + "&officeId=" + officeId + "&employeeInfoId=" + employeeInfoId;
            string status = myPDF.GenerateLandscapePDF(out fileName, url);
            FileName = fileName;
            if (status != "done")
            {
                return Content("<h1>" + status + "</h1>");
            }

            var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        [AllowAnonymous]
        public async Task<IActionResult> OverTimeReportview(DateTime fdate, DateTime todate, int depId, int branchId, int divisionId, int zoneId, int officeId, int employeeInfoId)
        {
            ViewBag.fdate = fdate;
            ViewBag.todate = todate;
            try
            {
                var model = new DailyAttendenceViewModel
                {
                    overtimereportviewmodels = await employeePunchCardInfoService.GetOverTimeReport(fdate, todate, depId, branchId, divisionId, zoneId, officeId, employeeInfoId),


                    departmentName = await employeePunchCardInfoService.GetDepartmentNameById(depId),
                    branchName = await employeePunchCardInfoService.GetBrancheNameById(branchId),
                    divisionName = await employeePunchCardInfoService.GetDivisionNameById(divisionId),
                    locationName = await employeePunchCardInfoService.GetLocatioNameById(zoneId),
                    officeName = await employeePunchCardInfoService.GetOfficeNameById(officeId),
                    companies = await eRPCompanyService.GetAllCompany(),

                };
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //[AllowAnonymous]
        //public async Task<IActionResult> GetManualAttendanceByAnyKey(string employeeCode, int? approverId, string fromDate, string toDate)
        //{
        //    var data = await employeePunchCardInfoService.GetManualAttendanceByAnyKey(employeeCode, approverId, fromDate, toDate);
        //    return Json(data);
        //}

        #region Attendance Summary Report

        [AllowAnonymous]
		public async Task<IActionResult> GetManualAttendanceByAnyKey(string employeeCode, int? approverId, string fromDate, string toDate)
		{
			var data = await employeePunchCardInfoService.GetManualAttendanceByAnyKey(employeeCode, approverId, fromDate, toDate);

			var model = new ManualAttendanceReportVM
			{
				companies = await eRPCompanyService.GetAllCompany(),
				ManualAttendanceViewModels = data
			};
			return View(model);
		}
		[AllowAnonymous]
		public IActionResult GetManualAttendanceByAnyKeyPDF(string employeeCode, int? approverId, string fromDate, string toDate)
		{
			string scheme = Request.Scheme;
			var host = Request.Host;
			string fileName;
			string url = "";

			url = scheme + "://" + host + "/HRPMSAttendence/EmployeePunchCardInfo/GetManualAttendanceByAnyKey?employeeCode=" + employeeCode + "&approverId=" + approverId + "&fromDate=" + fromDate + "&toDate=" + toDate;
			//url = scheme + "://" + host + "/HRPMSLeave/LeaveRegister/EarlyLeaveAttendance?userName=" + userName;
			string status = myPDF.GeneratePDF(out fileName, url);
			if (status != "done")
			{
				return Content("<h1>Something Went Wrong. </h1>");
			}
			var stream = new FileStream(rootPath + "/wwwroot/pdf/" + fileName, FileMode.Open);
			return new FileStreamResult(stream, "application/pdf");
		}

		#endregion



	}
}