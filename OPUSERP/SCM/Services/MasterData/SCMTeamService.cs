using Microsoft.EntityFrameworkCore;
using OPUSERP.Areas.SCMMasterData.Models;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.SCM.Data.Entity.MasterData;
using OPUSERP.SCM.Services.MasterData.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.SCM.Services.MasterData
{
	public class SCMTeamService : ISCMTeamService
	{
		private readonly ERPDbContext _context;

		public SCMTeamService(ERPDbContext context)
		{
			_context = context;
		}



		public bool DuplicateData(string memberName)
		{
			return _context.employeeInfos.Any(p => p.nameEnglish == memberName);
		}



		public async Task<int> SaveTeamMaster(TeamMaster teamMaster)
		{
			if (teamMaster.Id > 0)
			{
				_context.TeamMasters.Update(teamMaster);
			}
			else
			{
				_context.TeamMasters.Add(teamMaster);
			}
			await _context.SaveChangesAsync();
			return teamMaster.Id;
		}

		public async Task<IEnumerable<TeamMaster>> GetAllTeamMaster()
		{
			var result = await (from t in _context.TeamMasters
								join u in _context.Users on t.leaderId equals u.userId
								join e in _context.employeeInfos on u.Id equals e.ApplicationUserId
								select new TeamMaster
								{
									Id = t.Id,
									teamCode = t.teamCode,
									teamName = t.teamName,
									leaderId = t.leaderId,
									isActive = t.isActive,
									shortOrder = t.shortOrder,
									teamLeader = e.nameEnglish,
									userId = u.userId
								}).AsNoTracking().ToListAsync();
			return result;
		}



		public async Task<TeamMaster> GetTeamMasterById(int id)
		{
			return await _context.TeamMasters.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
		}

		public async Task<bool> DeleteTeamMasterById(int id)
		{
			_context.TeamMasters.Remove(_context.TeamMasters.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}
        public async Task<bool> DeleteTeamInfoById(int id)
        {
            _context.Teams.Remove(_context.Teams.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        #region TeamMember

        public async Task<int> SaveTeamMember(TeamMember teamMember)
		{
			if (teamMember.Id > 0)
			{
				_context.Entry(teamMember).State = EntityState.Modified;
			}
			else
			{
				_context.TeamMembers.Add(teamMember);
			}
			await _context.SaveChangesAsync();
			return teamMember.Id;
		}

		public async Task<IEnumerable<TeamMember>> GetAllTeamMember()
		{
			return await _context.TeamMembers.AsNoTracking().OrderBy(a => a.Id).ToListAsync();
		}

		public async Task<IEnumerable<System.Object>> GetAllTeamMemberByMasterId(int id)
		{
			var result = (from D in _context.TeamMembers
						  join M in _context.TeamMasters on D.teamMasterId equals M.Id
						  join MM in _context.Users on M.leaderId equals MM.userId
						  join EMM in _context.employeeInfos on MM.Id equals EMM.ApplicationUserId
						  join S in _context.Users on D.memberId equals S.userId
						  join E in _context.employeeInfos on S.Id equals E.ApplicationUserId
						  where D.teamMasterId == id
						  select new
						  {
							  D.Id,
							  D.teamMasterId,
							  teamName = M.teamName + " - " + (M.teamCode),
							  leaderName = E.nameEnglish,
							  D.memberId,
							  memberName = E.nameEnglish
						  }).AsNoTracking().ToListAsync();

			return await result;
		}

		public async Task<IEnumerable<Photograph>> getAllphoto()
		{
			return await _context.photographs.ToListAsync();
		}
		public async Task<IEnumerable<EmployeeInfo>> GetAllTeamMemberByMasterId1(int id)
		{
			var result = (from D in _context.TeamMembers
						  join M in _context.TeamMasters on D.teamMasterId equals M.Id
						  join MM in _context.Users on M.leaderId equals MM.userId
						  join EMM in _context.employeeInfos on MM.Id equals EMM.ApplicationUserId
						  join S in _context.Users on D.memberId equals S.userId
						  join E in _context.employeeInfos on S.Id equals E.ApplicationUserId
						  where D.teamMasterId == id
						  select E).AsNoTracking().ToListAsync();

			return await result;
		}
		public async Task<string> getLeaderNameByLeaderId(int id)
		{
			var result = await (from m in _context.TeamMasters
								join u in _context.Users on m.leaderId equals u.userId
								join e in _context.employeeInfos on u.Id equals e.ApplicationUserId
								where u.userId == id
								select e.nameEnglish).AsNoTracking().FirstOrDefaultAsync();

			return result;
		}
		public async Task<string> getLeaderPhotoByLeaderId(int id)
		{
			var result = await (from m in _context.TeamMasters
								join u in _context.Users on m.leaderId equals u.userId
								join e in _context.employeeInfos on u.Id equals e.ApplicationUserId
								join p in _context.photographs on e.Id equals p.employeeId
								where u.userId == id
								select p.url).AsNoTracking().FirstOrDefaultAsync();

			return result;
		}


		public async Task<IEnumerable<TeamListViewModel>> GetAllTeamList(int id)
		{
			var result = (from D in _context.TeamMembers
						  join M in _context.TeamMasters on D.teamMasterId equals M.Id
						  join MM in _context.Users on M.leaderId equals MM.userId
						  join EMM in _context.employeeInfos on MM.Id equals EMM.ApplicationUserId
						  join S in _context.Users on D.memberId equals S.userId
						  join E in _context.employeeInfos on S.Id equals E.ApplicationUserId
						  select new TeamListViewModel
						  {
							  tmId = D.Id,
							  teamName = M.teamName,
							  leaderName = EMM.nameEnglish,
                              teamMasterId=M.Id,
                              mamberName = E.nameEnglish
							  //D.Id,
							  //D.teamMasterId,
							  //teamName = M.teamName + " - " + (M.teamCode),
							  //leaderName = EMM.nameEnglish,
							  //D.memberId,
							  //memberName = E.nameEnglish
						  }).AsNoTracking().ToListAsync();

			return await result;
		}
		public async Task<IEnumerable<TeamListViewModel>> GetAllTeamInfoList()
		{
			var result = await (from D in _context.Users
								orderby D.userId descending
								join M in _context.TeamMasters on D.userId equals M.leaderId
								join e in _context.employeeInfos on D.Id equals e.ApplicationUserId
								join p in _context.photographs on e.Id equals p.employeeId into pp
								from ph in pp.DefaultIfEmpty()
								select new TeamListViewModel
								{
									teamId = M.Id,
									teamName = M.teamName,
									leaderName = e.nameEnglish,
									teamCode = M.teamCode,
									leaderphoto = ph.url
								}).AsNoTracking().ToListAsync();

			return result;
		}

		public async Task<IEnumerable<TeamListViewModel>> GetAllTeamInfoList1()
		{
			var result = await (from D in _context.Users
								join M in _context.TeamMasters on D.userId equals M.leaderId
								join e in _context.employeeInfos on D.Id equals e.ApplicationUserId
								join p in _context.photographs on e.Id equals p.employeeId into pp
								from ph in pp.DefaultIfEmpty()
								select new TeamListViewModel
								{
									teamId = M.Id,
									teamName = M.teamName,
									leaderName = e.nameEnglish,
									teamCode = M.teamCode,
									leaderphoto = ph.url
								}).AsNoTracking().ToListAsync();

			return result;
		}

		public async Task<IEnumerable<TeamListViewModel>> GetTeamMembersByTeamMasterId(int id)
		{
			//var result = await (from M in _context.TeamMembers
			var result = await (from M in _context.TeamMembers.GroupBy(x => x.memberId).Select(x => x.FirstOrDefault())
								join Tm in _context.TeamMasters on M.teamMasterId equals Tm.Id
								join u in _context.Users on M.memberId equals u.userId
								join e in _context.employeeInfos on u.Id equals e.ApplicationUserId
								join ph in _context.photographs on e.Id equals ph.employeeId into pp
								from p in pp.DefaultIfEmpty()
									//where M.teamMasterId==id
								orderby M.memberId
								select new TeamListViewModel
								{
									tmId = M.Id,
									teamCode = Tm.teamCode,
									leaderphoto = p.url,
									mamberName = e.nameEnglish
								}).AsNoTracking().ToListAsync();

			return result;
		}
		public async Task<IEnumerable<TeamListViewModel>> GetTeamMembersByTeamMasterIdNew(int id)
		{
			//var result = await (from M in _context.TeamMembers
			var result = await (from M in _context.TeamMembers.GroupBy(x => x.memberId).Select(x => x.FirstOrDefault())
								join Tm in _context.TeamMasters on M.teamMasterId equals Tm.Id
								join u in _context.Users on M.memberId equals u.userId
								join e in _context.employeeInfos on u.Id equals e.ApplicationUserId
								orderby M.memberId
								select new TeamListViewModel
								{
									tmId = M.Id,
									teamCode = Tm.teamCode,
									mamberName = e.nameEnglish
								}).AsNoTracking().ToListAsync();
			return result;
		}

		public async Task<TeamMember> GetTeamMemberById(int id)
		{
			return await _context.TeamMembers.Where(x => x.Id == id).FirstOrDefaultAsync();
		}

		public async Task<bool> DeleteTeamMemberById(int id)
		{
			_context.TeamMembers.Remove(await _context.TeamMembers.Where(x => x.Id == id).FirstOrDefaultAsync());
			return 1 == await _context.SaveChangesAsync();
		}


		#endregion

	}
}
