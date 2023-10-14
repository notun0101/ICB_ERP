using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.ProvidentFund.Models;
using Microsoft.AspNetCore.Authorization;
using OPUSERP.ProvidentFund.Service.Interface;
using OPUSERP.ERPServices.MasterData.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.ProvidentFund.Data.Entity;

namespace OPUSERP.Areas.ProvidentFund.Controllers
{
    [Authorize]
    [Area("ProvidentFund")]
    public class MemberController : Controller
    {
        private readonly IPFMemberInfoService pFMemberInfoService;
        private readonly IEmployeeInfoService employeeInfoService;
        private readonly ERPServices.MasterData.Interfaces.IDesignationDepartmentService designationDepartmentService;
        private readonly ITypeService typeService;
        private readonly IStatusService statusService;
        private readonly IPersonalInfoService personalInfoService;

        public MemberController(ERPServices.MasterData.Interfaces.IDesignationDepartmentService designationDepartmentService, IPFMemberInfoService pFMemberInfoService, IEmployeeInfoService employeeInfoService, ITypeService typeService, IStatusService statusService, IPersonalInfoService personalInfoService)
        {
            this.pFMemberInfoService = pFMemberInfoService;
            this.employeeInfoService = employeeInfoService;
            this.typeService = typeService;
            this.statusService = statusService;
            this.designationDepartmentService = designationDepartmentService;
            this.personalInfoService = personalInfoService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> MemberOverView()
        {
            //PFMemberInfoViewModel model = new PFMemberInfoViewModel
            //{
            //    pFMemberInfos = await pFMemberInfoService.GetApprovePFMemberInfo()
            //};
            var data = new List<PfMemberInfoVm>();
            var pFMemberInfos = await pFMemberInfoService.GetAllPFMemberInfo();
            var totalMembers = pFMemberInfos.Count();
            var pendingMemberInfos = await pFMemberInfoService.GetPendingPFMemberInfo();
            var totalPendingMembers = pendingMemberInfos.Count();
            var NewMemberInfos = await pFMemberInfoService.GetNewPFMemberInfo();
            var totalNewMembers = NewMemberInfos.Count();

            foreach (var item in pFMemberInfos)
            {
                data.Add(new PfMemberInfoVm
                {
                    pFMember = item,
                    totalContribution = await pFMemberInfoService.CalculateTotalContributionByMemberId(item.Id)
                });
            }
            PFMemberInfoViewModel model = new PFMemberInfoViewModel
            {
                pfMemberInfos = data,
                totalMember = totalMembers,
                pendingMember=totalPendingMembers,
                newMember=totalNewMembers
            };
            return View(model);
        }

        public async Task<IActionResult> ApproveMemberList()
        {
            PFMemberInfoViewModel model = new PFMemberInfoViewModel
            {
                pFMemberInfos = await pFMemberInfoService.GetApprovePFMemberInfo()
            };
            return View(model);
        }

        [HttpGet]


        public async Task<IActionResult> MemberApplication()
        {
            var data = await pFMemberInfoService.getLastMemberCode() + 1;
            ViewBag.memberCode = data.ToString("D6");

            PFMemberInfoViewModel model = new PFMemberInfoViewModel
            {
                specialBranchUnits = await pFMemberInfoService.GetAllSbu()                
            };
            return View(model);
        }

        //public async Task<IActionResult> MemberApplication(int id)
        //{

        //    try
        //    {
        //        var data = await pFMemberInfoService.GetPFMemberInfoById(id);

        //        ViewBag.id = id;
        //        //if (id > 0)
        //        //{

        //        //}
        //        PFMemberInfoViewModel model = new PFMemberInfoViewModel
        //        {
        //            memberID = data?.Id,

        //            memberName = data?.memberName,
        //            email = data?.email,
        //            //designation=Convert.ToInt32(data.designation),
        //            designation = data?.designationId,
        //            designations = await designationDepartmentService.GetDesignations(),
        //            dateOfBirth = data?.dateOfBirth,
        //            dateOfJoining = data?.dateOfJoining,
        //            //department=Convert.ToInt32(data.department),
        //            department = data?.departmentId,
        //            departments = await designationDepartmentService.GetDepartment(),
        //            nid = data?.nid,
        //            mobile = data?.mobile,
        //            note = data?.note,
        //            approveStatus = data?.approveStatus,
        //            employeeCode = data?.employeeCode,
        //            memberType = data?.memberTypeId,
        //            memberTypes = await typeService.GetAllEmployeeType()
        //        };

        //        return View(model);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }


        //}


        [HttpPost]
        public async Task<IActionResult> MemberApplication([FromForm] PFMemberInfoViewModel model)
        {
            int status = 0;
            if (model.approveStatus == null)
            {
                status = 0;
            }
            else
            {
                status = (int)model.approveStatus;
            }

            var data1 = await pFMemberInfoService.getLastMemberCode() + 1;
            var maxMembercode = data1.ToString("D6");
            //var maxMembercode = await pFMemberInfoService.getLastMemberCode() + 1;

            PFMemberInfo data = new PFMemberInfo
            {
                Id = (int)model.memberID,
                memberName = model.memberName,
                approveStatus = status,
                isActive = model.isActive,
                isPfContinuing = model.isPfContinuing,
                employeeInfoId = model.employeeInfoId,
                memberCode = maxMembercode,
                applicationDate = model.applicationDate,
                note= model.note,
            };
            await pFMemberInfoService.SavePFMemberInfo(data);
            return RedirectToAction(nameof(MemberApplication));
        }

        public async Task<IActionResult> PendingMemberList()
        {

            PFMemberInfoViewModel model = new PFMemberInfoViewModel
            {
                pFMemberInfos = await pFMemberInfoService.GetPendingPFMemberInfo()
            };
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> ViewMember(int id)
        {

            try
            {
                var data = await pFMemberInfoService.GetPFMemberInfoById(id);

                PFMemberInfoViewModel model = new PFMemberInfoViewModel
                {
                    memberID = data?.Id,
                    memberCode = data?.memberCode,
                    memberName = data?.memberName,
                    email = data.employeeInfo?.emailAddress,
                    note = data?.note,
                    applicationDate = data?.applicationDate,
                    department = data?.employeeInfo?.department?.deptName,
                    designation = data?.employeeInfo?.lastDesignation?.designationName,
                    mobile = data?.employeeInfo?.mobileNumberOffice,
                    dateOfJoining = data.employeeInfo?.joiningDateGovtService,

                };

                return View(model);
            }
            catch (Exception ex) 
            {

                throw ex;
            }
           
        }
        [HttpPost]
        //public async Task<IActionResult> ViewMember([FromForm]PFMemberInfoViewModel model)
        //{
        //    try
        //    {
        //        var data = await pFMemberInfoService.GetPFMemberInfoById((int)model.memberID);
        //        data.approveStatus = 2;

        //        await pFMemberInfoService.SavePFMemberInfo(data);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //    return RedirectToAction(nameof(MemberOverView));

        //    //return View();
        //}
        [HttpGet]
        public async Task<IActionResult> MemberList()
        {
            PFMemberInfoViewModel model = new PFMemberInfoViewModel
            {
                pFMemberInfos = await pFMemberInfoService.GetAllPFMemberInfo()
            };
            return View(model);
        }

        public async Task<IActionResult> MemberPFStatement()
        {
            return View();
        }

        public async Task<JsonResult> UpdateMemberStatus(int id, int status)
        {
            try
            {
                await pFMemberInfoService.UpdateMemberAplicationStatus(id, status, HttpContext.User.Identity.Name);
                return Json(true);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<IActionResult> EditMemberInfo(int id)
        {
            try
            {

                var data = await pFMemberInfoService.GetPFMemberInfoById(id);
                PFMemberInfoViewModel model = new PFMemberInfoViewModel
                {
                    memberID = data.Id,

                    memberName = data.memberName,
                    //email = data.email,
                    ////designation=Convert.ToInt32(data.designation),
                    //designation = data.designationId,
                    //designations = await designationDepartmentService.GetDesignations(),
                    //dateOfBirth = data.dateOfBirth,
                    //dateOfJoining = data.dateOfJoining,
                    ////department=Convert.ToInt32(data.department),
                    //department = data.departmentId,
                    //departments = await designationDepartmentService.GetDepartment(),
                    //nid = data.nid,
                    //mobile = data.mobile,
                    //note = data.note,
                    //approveStatus = data.approveStatus,
                    //employeeCode = data.employeeCode,
                    //memberType = data.memberTypeId,
                    memberTypes = await typeService.GetAllEmployeeType(),
                    memberCode = data.memberCode,
                    applicationDate=data.applicationDate,
                    employeeInfoId=data.employeeInfoId,
                    isPfContinuing=data.isPfContinuing,
                    isActive =data.isActive
                   
                };

                return View(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }



        }
        [HttpPost]
        public async Task<IActionResult> EditMemberInfo([FromForm] PFMemberInfoViewModel model)
        {
            PFMemberInfo data = new PFMemberInfo
            {
                Id = (int)model.memberID,
                memberName = model.memberName,
                //email = model.email,
                //employeeCode = model.employeeCode,
                //nid = model.nid,
                //dateOfJoining = model.dateOfJoining,
                //dateOfBirth = model.dateOfBirth,
                //designationId = model.designation,
                //departmentId = model.department,

                //mobile = model.mobile,
                //note = model.note,
                //memberTypeId = model.memberType,
                approveStatus = model.approveStatus,
                isActive = model.isActive,
                isPfContinuing = model.isPfContinuing,
                //employeeStatusId = model.employeeStatus,
                employeeInfoId = model.employeeInfoId,
                memberCode=model.memberCode,
                applicationDate=model.applicationDate
               
            };


            await pFMemberInfoService.SavePFMemberInfo(data);

            return RedirectToAction(nameof(MemberList));

            //return View();
        }


        [HttpGet]
        public async Task<IActionResult> GetAllEmployeeType()
        {
            return Json(await typeService.GetAllEmployeeType());

        }


        public async Task<IActionResult> DeleteMember(int id)
        {
            //await stockService.DeleteStockMasterById(id);
            if (id > 0)
            {

                try
                {
                    await pFMemberInfoService.DeletePFMemberInfoById(id);
                    return Json(new { Success = true, Message = "Deleted Successfully!" });
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Message = "Unable to delete! please try again.. ", exception = ex.Message });
                }
            }

            return Json("ok");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMembers()
        {
            return Json(await pFMemberInfoService.GetAllPFMemberInfo());

        }
        [Route("global/api/GetApprovePFMemberInfo")]
        [HttpGet]
        public async Task<IActionResult> GetApprovePFMemberInfo()
        {
            return Json(await pFMemberInfoService.GetApprovePFMemberInfo());

        }
        [Route("global/api/GetAllMembersbyId/{Id}")]
        [HttpGet]
        public async Task<IActionResult> GetAllMembersbyId(int id)
        {
            return Json(await pFMemberInfoService.GetPFMemberInfoById(id));
        }
     
        [HttpGet]
        public async Task<IActionResult> CheckMemberById(string code)
        {
            var data = await pFMemberInfoService.GetMemberInfoById(code);
            if (data != null)
            {
                return Json("failed");
            }
            else
            {
                return Json(data);
            }
        }

        
        [HttpGet]
        public async Task<IActionResult> GetNotPfMemberEmployeeList()
        {
            return Json(await personalInfoService.GetNotPfMemberEmployeeList());
        }
    }
}