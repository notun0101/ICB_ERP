using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.SCMRequisition.Models;
using OPUSERP.Data.Entity.Matrix;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using OPUSERP.SCM.Data.Entity.Requisition;
using OPUSERP.SCM.Services.Requisition.Interfaces;

namespace OPUSERP.API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class RequisitionInfoController : ControllerBase
    {
        private readonly IRequisitionService requisitionService;
        private readonly IUserInfoes userInfoes;
        private readonly ILeaveRegisterService leaveRegisterService;
        private readonly IPersonalInfoService personalInfoService;

        public RequisitionInfoController(IRequisitionService requisitionService, IPersonalInfoService personalInfoService, IUserInfoes userInfoes, ILeaveRegisterService leaveRegisterService)
        {
            this.requisitionService = requisitionService;
            this.userInfoes = userInfoes;
            this.leaveRegisterService = leaveRegisterService;
            this.personalInfoService = personalInfoService;
        }

        [HttpGet("{userName}")]
        [AllowAnonymous]
        public async Task<IEnumerable<GetRequisitionListForApprovedViewModel>> ReqApprovelistApi(string userName)
        {
            var userInfo = await userInfoes.GetUserInfoByUser(userName);

            var result = await requisitionService.GetRequisitionApproveList(userInfo.UserId);
            return result;
        }

        [HttpGet("{userName}")]
        [AllowAnonymous]
        public async Task<IEnumerable<RequisitionMaster>> ReturnRequisitionApi(string userName)
        {
            var userInfo = await userInfoes.GetUserInfoByUser(userName);

            var result = await requisitionService.GetRequisitionMasterListByPRStatus(userInfo.UserId, 4);

            return result;
        }

        [HttpGet("{userName}")]
        [AllowAnonymous]
        public async Task<IEnumerable<RequisitionMaster>> RejectRequisitionApi(string userName)
        {
            var userInfo = await userInfoes.GetUserInfoByUser(userName);

            var result = await requisitionService.GetRequisitionMasterListByPRStatus(userInfo.UserId, 4);

            return result;
        }


        //api/RequisitionInfo/LeaveRejectAPI
        [AllowAnonymous]
        [HttpGet("{id}")]
        //[HttpGet("{userName}")]
        public async Task<IActionResult> LeaveRejectAPI(string id)
        {
            //ViewBag.masterId = id;

            //string userName = HttpContext.User.Identity.Name;
            //var userInfos = await userInfoes.GetUserInfoByUser(userName);
            var EmpInfo = await personalInfoService.GetEmployeeInfoByCode(id);
            int empId = 0;
            if (EmpInfo != null)
            {
                empId = EmpInfo.Id;
            }

            var leaveRegisterslist = await leaveRegisterService.GetAllLeaveRegisterByEmpIdAndStatusAPI(empId, 5);
            
            return Ok(leaveRegisterslist);
        }
    }
}