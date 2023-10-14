using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OPUSERP.Areas.SCMMasterData.Models;
using OPUSERP.ERPService.AuthService.Interfaces;
using OPUSERP.SCM.Data.Entity.MasterData;
using OPUSERP.SCM.Services.MasterData.Interfaces;
using OPUSERP.SCM.Services.Requisition.Interfaces;

namespace OPUSERP.Areas.SCMMasterData.Controllers
{
	[Area("SCMMasterData")]
	public class SCMTeamController : Controller
	{
		private readonly ISCMTeamService sCMTeamService;
		private readonly IUserInfoes userInfoes;
		private readonly IRequisitionService requisitionService;

		public SCMTeamController(IRequisitionService requisitionService, ISCMTeamService sCMTeamService, IUserInfoes userInfoes)
		{
			this.sCMTeamService = sCMTeamService;
			this.userInfoes = userInfoes;
			this.requisitionService = requisitionService;
		}

		public async Task<IActionResult> Index()
		{
			TeamMasterViewModel model = new TeamMasterViewModel
			{
				teamMasters = await sCMTeamService.GetAllTeamMaster(),
				aspNetUsersViews = await userInfoes.GetUserInfo()
			};
			return View(model);
		}


		[Route("api/SCMTeam/GetTeamMember/TeamListApi/")]
		public async Task<JsonResult> TeamListApi()
		{
			TeamMasterViewModel model = new TeamMasterViewModel
			{
				teamMasters = await sCMTeamService.GetAllTeamMaster(),
				aspNetUsersViews = await userInfoes.GetUserInfo()
			};
			return Json(model);
		}

		[HttpPost]
		public async Task<IActionResult> Index([FromForm] TeamMasterViewModel model)
		{
			try
			{
				TeamMaster master = new TeamMaster
				{
					Id = Convert.ToInt32(model.teamMasterId),
					teamCode = model.teamCode,
					teamName = model.teamName,
					leaderId = model.leaderId,
					isActive = 1
				};
				await sCMTeamService.SaveTeamMaster(master);
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		[HttpGet]
		public async Task<IActionResult> TeamMember()
		{
			TeamMemberViewModel model = new TeamMemberViewModel
			{
				aspNetUsersViews = await userInfoes.GetUserInfo(),
				teamMasters = await sCMTeamService.GetAllTeamMaster(),
				AllTeams = await sCMTeamService.GetAllTeamList(1)
			};
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> TeamMember([FromForm] TeamMasterViewModel model)
		{
			try
			{
				TeamMember master = new TeamMember
				{
					Id = Convert.ToInt32(model.teamMemberId),
					teamMasterId = Convert.ToInt32(model.teamMasterId),
					memberId = model.memberId,
					isActive = 1
				};

				await sCMTeamService.SaveTeamMember(master);
				return RedirectToAction(nameof(TeamMember));
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		//[Route("api/SCMTeam/GetTeamMember/{masterId}")]
		//public async Task<JsonResult> GetTeamMember(int masterId)
		//{
		//    var result = await sCMTeamService.GetAllTeamMemberByMasterId(masterId);
		//    return Json(result);
		//}



		[Route("api/SCMTeam/GetTeamMember/GetTeamMemberList")]
		public async Task<JsonResult> GetTeamMemberList()
		{
			var result = await sCMTeamService.GetAllTeamMember();
			return Json(result);
		}



		//[HttpGet]
		//public async Task<IActionResult> TeamMemberInfo(int masterId)
		//{
		//    //var data = await sCMTeamService.GetAllTeamList(masterId);
		//    //return View(data);

		//    TeamListViewModel model = new TeamListViewModel
		//    {
		//        GetTeams = await sCMTeamService.GetAllTeamList(masterId),
		//        teamMasters = await sCMTeamService.GetAllTeamMaster(),
		//        teamMembers = await sCMTeamService.GetAllTeamMember()
		//    };
		//    return View(model);

		//    //var result = await sCMTeamService.GetAllTeamList(masterId);
		//    //return View(result);
		//}
		[HttpGet]
		public async Task<IActionResult> TeamMemberInfo(int masterId)
		{
			List<TeamListViewModel1> data = new List<TeamListViewModel1>();
			var member = await sCMTeamService.GetAllTeamInfoList();
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
					employeeInfos = await sCMTeamService.GetAllTeamMemberByMasterId1(item.teamId)
				};
				data.Add(model);
			}
			return View(data);
		}

		[HttpGet]
		public async Task<IActionResult> AddTeamMember(int id)
		{
			var result = new TeamInfoViewModel
			{

				TeamMaster = await sCMTeamService.GetTeamMasterById(id),
				TeamMembers = await sCMTeamService.GetTeamMembersByTeamMasterId(id),
			};
			return View(result);
		}



		[HttpGet]
		public async Task<IActionResult> EditTeamMember(int id)
		{
			var item = await sCMTeamService.GetTeamMasterById(id);

			if (id > 0)
			{
				TeamMaster data = await sCMTeamService.GetTeamMasterById(id);
			}

			TeamListViewModel1 model = new TeamListViewModel1
			{
				teamId = id,
				teamMembers = item.teamName,
				leaderId = item.leaderId,
				LeaderName = await sCMTeamService.getLeaderNameByLeaderId(Convert.ToInt32(item.leaderId)),
				leaderphoto = await sCMTeamService.getLeaderPhotoByLeaderId(Convert.ToInt32(item.leaderId)),
				teamCode = item.teamCode,
				teamName = item.teamName,
				employeeInfos = await sCMTeamService.GetAllTeamMemberByMasterId1(id)
			};

			return View(model);
		}


		[HttpPost]
		public async Task<IActionResult> EditTeamMember(TeamListViewModel1 model)
		{

			//if (sCMTeamService.DuplicateData(model.memberName))
			//{
			//    ModelState.AddModelError("txtName", "Member name already exists.");
			//}

			//if (!ModelState.IsValid)
			//{
			//    model.teamMembersList = await sCMTeamService.GetAllTeamMember();
			//    return View(model);
			//}


			var masterId = await sCMTeamService.GetTeamMasterById(model.teamId);

			var dataF = new TeamMaster
			{
				Id = model.teamId,
				teamCode = model.teamCode,
				leaderId = model.leaderId,
				teamName = model.teamName,
				teamLeader = model.leaderName,
			};

			int tmid = await sCMTeamService.SaveTeamMaster(dataF);

			if (model.teamMemberById != null)
			{

				foreach (var memberId in model.teamMemberById)
				{
					var reqDetails = await requisitionService.GetRequisitionDetailByMemberId(model.memberId);

					if (reqDetails == null)
					{
						await sCMTeamService.DeleteTeamMemberById(tmid);
					}
					else
					{
						var dataA = new TeamMember
						{
							teamMasterId = tmid,
							memberId = memberId,
						};

						await sCMTeamService.SaveTeamMember(dataA);

					}
				}
			}

			return RedirectToAction(nameof(TeamMemberInfo));
		}



		[HttpPost]
		public async Task<IActionResult> AddTeamMember(TeamRequisitionViewModel model)
		{



			//if (sCMTeamService.DuplicateData(model.memberName))
			//{
			//    ModelState.AddModelError("txtName", "Member name already exists.");
			//}

			//if (!ModelState.IsValid)
			//{
			//    model.teamMembers = await sCMTeamService.GetAllTeamMember();
			//    return View(model);
			//}



			var dataF = new TeamMaster
			{
				Id = 0,
				teamCode = model.teamCode,
				leaderId = model.leaderId,
				teamName = model.teamName,
				teamLeader = model.leaderName,
			};

			int id = await sCMTeamService.SaveTeamMaster(dataF);

			if (model.teamMemberById != null)
			{
				foreach (var data in model.teamMemberById)
				{
					var dataA = new TeamMember
					{
						Id = 0,
						teamMasterId = id,
						memberId = data,
					};
					await sCMTeamService.SaveTeamMember(dataA);
				}
			}

			return RedirectToAction(nameof(TeamMemberInfo));
		}

		public IActionResult Test()
		{
			return View();
		}
        public async Task<JsonResult> DeleteTeamById(int Id)
        {
            await sCMTeamService.DeleteTeamInfoById(Id);
            return Json(true);
        }
    }
}