using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.CRMLead.Models;
using OPUSERP.Areas.MasterData.Models;
using OPUSERP.Areas.SCMMasterData.Models;
using OPUSERP.CRM.Data.Entity.Team;
using OPUSERP.CRM.Services.MasterData.Interfaces;
using OPUSERP.Data.Entity.MasterData;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.ERPService.MasterData.Interfaces;
using OPUSERP.ERPServices.Interfaces;
using OPUSERP.SCM.Services.Requisition.Interfaces;

namespace OPUSERP.Areas.CRMLead.Controllers
{
    [Area("CRMLead")]
    public class CRMTeamController : Controller
    {
        private readonly ITeamService teamService;
        private readonly IUserInfoes userInfoes;
        private readonly ICommunicationService communicationService;
        private readonly IModuleAssignService moduleAssignService;
        private readonly IRequisitionService requisitionService;

        public CRMTeamController(ITeamService teamService, IRequisitionService requisitionService, IUserInfoes userInfoes, IModuleAssignService moduleAssignService, ICommunicationService communicationService)
        {
            this.teamService = teamService;
            this.userInfoes = userInfoes;
            this.communicationService = communicationService;
            this.moduleAssignService = moduleAssignService;
            this.requisitionService = requisitionService;
        }
        #region CRM Team
        public async Task<IActionResult> CreateTeam(int? id)
        {
            var result = new TeamInfoViewModel
            {

                CRMTeamMaster = await teamService.GetTeamMasterById(Convert.ToInt32(id)),
                TeamMembers = await teamService.GetTeamMembersByTeamMasterId(Convert.ToInt32(id))
            };
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> EditTeamMember(int id)
        {
            var item = await teamService.GetTeamMasterById(id);

            if (id > 0)
            {
                CRMTeamMaster data = await teamService.GetTeamMasterById(id);
            }

            TeamListViewModel1 model = new TeamListViewModel1
            {
                teamId = id,
                teamMembers = item.teamName,
                leaderId = item.leaderId,
                LeaderName = await teamService.getLeaderNameByLeaderId(Convert.ToInt32(item.leaderId)),
                leaderphoto = await teamService.getLeaderPhotoByLeaderId(Convert.ToInt32(item.leaderId)),
                teamCode = item.teamCode,
                teamName = item.teamName,
                employeeInfos = await teamService.GetAllTeamMemberByMasterId1(id)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditTeamMember(TeamListViewModel1 model)
        {
            var masterId = await teamService.GetTeamMasterById(model.teamId);
            masterId.Id = model.teamId;
                masterId.teamCode = model.teamCode;
                masterId.leaderId = model.leaderId;
                masterId.teamName = model.teamName;
                masterId.teamLeader = model.leaderName;

            int tmid = await teamService.SaveTeamMaster(masterId);

            if (model.teamMemberById != null)
            {
                var deleteTeamMemeber = await teamService.DeleteTeamMemberById(tmid);
                foreach (var memberId in model.teamMemberById)
                {
                    if (memberId != null)
                    {
                        var dataA = new CRMTeamMember
                        {
                            Id = 0,
                            cRMTeamMasterId = tmid,
                            memberId = memberId,
                        };
                        await teamService.SaveTeamMember(dataA);
                    }
                    //var reqDetails = await requisitionService.GetRequisitionDetailByMemberId(model.memberId);
                    //if (reqDetails == null)
                    //{
                    //    await teamService.DeleteTeamMemberById(tmid);
                    //}
                    //else
                    //{
                    //    var dataA = new CRMTeamMember
                    //    {
                    //        cRMTeamMasterId = tmid,
                    //        memberId = memberId,
                    //    };
                    //    await teamService.SaveTeamMember(dataA);
                    //}
                }
            }

            return RedirectToAction(nameof(CRMTeamList));
        }

        [HttpPost]
        public async Task<IActionResult> AddTeamMember(TeamRequisitionViewModel model)
        {
            var dataF = new CRMTeamMaster
            {
                Id = 0,
                teamCode = model.teamCode,
                leaderId = model.leaderId,
                teamName = model.teamName,
                teamLeader = model.leaderName,
            };

            int id = await teamService.SaveTeamMaster(dataF);

            if (model.teamMemberById != null)
            {
                foreach (var data in model.teamMemberById)
                {
                    var dataA = new CRMTeamMember
                    {
                        Id = 0,
                        cRMTeamMasterId = id,
                        memberId = data,
                    };
                    await teamService.SaveTeamMember(dataA);
                }
            }

            return RedirectToAction(nameof(CRMTeamList));
        }

        public async Task<IActionResult> CreateLeadTeam()
        {
            var team = await teamService.GetTeamInfoByTeamId(null);
            int Cteam = 0;
            Cteam = team.Count();
            string idate = Convert.ToDateTime(DateTime.Now).ToString("yyyy");
            string autoTeamCode = idate + '-' + (Cteam + 1);

            CRMTeamViewModel model = new CRMTeamViewModel()
            {
                teams = await teamService.GetTeamInfoByTeamId(null),
                aspNetUsers = await userInfoes.GetUserInfoByModule(2),
                areas = await communicationService.GetAllArea(),
                eRPModules = await moduleAssignService.GetERPModules(),
                GetTeams = await teamService.GetAllTeam()
            };
            ViewBag.autoTeamCode = autoTeamCode;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTeam([FromForm] CRMTeamViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.teams = await teamService.GetTeamInfoByTeamId(null);
                model.aspNetUsers = await userInfoes.GetUserInfoByModule(2);
                return View(model);
            }

            Team data = new Team
            {
                Id = Convert.ToInt32(model.tId),
                aspnetuserId = model.teamHead,
                areaId = model.areaId,
                memberName = model.teamName,
                teamCode = model.teamCode,
                moduleId=model.moduleId,
                teamId=model.teamId,
                isActive = 1

            };

            await teamService.SaveTeam(data);
            int i = 0;
            if (model.teamMemberById != null)
            {
                foreach (var dataTM in model.teamMemberById)
                {
                    var dataA = new Team
                    {
                        Id = 0,
                        areaId = model.areaId,
                        memberName = model.teamMemberName[i],
                        teamCode = model.teamCode,
                        isActive = 1,
                        moduleId = model.moduleId,
                        teamId = data.Id,
                        aspnetuserId = model.teamMemberUserId[i]
                    };
                    i++;
                    var teamMember= await teamService.SaveTeamNew(dataA);
                }
            }
            return RedirectToAction(nameof(CreateTeam));
        }

        public async Task<IActionResult> CRMTeamList()
        {
            List<TeamListViewModel1> data = new List<TeamListViewModel1>();
            var member = await teamService.GetAllTeamInfoList();
            foreach (var item in member)
            {
                TeamListViewModel1 model = new TeamListViewModel1
                {
                    teamId = item.teamId,
                    teamMembers = item.teamName,
                    mamberName = item.mamberName,
                    LeaderName = item.leaderName,
                    leaderphoto = item.leaderphoto,
                    teamCode = item.teamCode,
                    teamName = item.teamName,
                    employeeInfos = await teamService.GetAllTeamMemberByMasterId1(item.teamId)
                };
                data.Add(model);
            }
            return View(data);
        }

        #endregion
        #region Assign Team

        public async Task<IActionResult> AssignTeam()
        {
            CRMTeamViewModel model = new CRMTeamViewModel()
            {
                teams = await teamService.GetTeamInfoByTeamId(null),
                aspNetUsers = await userInfoes.GetUserInfoByModule(2)                
            };
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> SaveAssignTeam([FromForm] CRMTeamViewModel model)
        {
            try
            {
                int teamId = 0;

                var teamCheck = await teamService.GetTeamByTeamIdAndUserId(model.teamId, model.aspnetuserId);
                if (teamCheck.Count() > 0)
                {
                    teamId = 0;
                }
                else
                {
                    Team data = new Team
                    {
                        Id = 0,                        
                        areaId = model.areaId,
                        memberName = model.memberName,
                        teamCode = model.teamCode,
                        isActive = 1,
                        moduleId=2,
                        teamId =model.teamId,
                        aspnetuserId = model.aspnetuserId
                    };

                    teamId = await teamService.SaveTeamNew(data);
                }

                return Json(teamId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTeamInfoByTeamId(int Id)
        {
            return Json(await teamService.GetTeamInfoByTeamId(Id));
        }

        [HttpPost]
        public async Task<JsonResult> DeleteTeamById(int Id)
        {
            await teamService.DeleteTeamById(Id);
            return Json(true);
        }

        #endregion
    }
}