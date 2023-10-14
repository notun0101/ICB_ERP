using Microsoft.EntityFrameworkCore;
using OPUSERP.Areas.CRMLead.Models;
using OPUSERP.Areas.SCMMasterData.Models;
using OPUSERP.CRM.Data.Entity.Team;
using OPUSERP.Data;
using OPUSERP.Data.Entity.MasterData;
using OPUSERP.ERPService.MasterData.Interfaces;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.ERPService.MasterData
{
    public class TeamService : ITeamService
    {
        private readonly ERPDbContext _context;

        public TeamService(ERPDbContext context)
        {
            _context = context;
        }

        // Sector
        public async Task<bool> SaveTeam(Team team)
        {
            if (team.Id != 0)
                _context.Teams.Update(team);
            else
                _context.Teams.Add(team);
            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Team>> GetAllTeam()
        {
            return await _context.Teams.Include(x => x.module).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        }
        public async Task<Team> GetTeambyCode(string teamcode)
        {
            return await _context.Teams.Include(x => x.module).Where(x => x.teamCode == teamcode).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Team>> GetAllTeamByModule(int moduleId)
        {
            return await _context.Teams.Where(x => x.moduleId == moduleId).Include(x => x.module).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<Team>> GetTeamByParrentId(int? parrentId)
        {
            return await _context.Teams.Where(x => x.teamId == parrentId).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        }
        public async Task<Team> GetTeamByaspnetuserId(string Id)
        {
            return await _context.Teams.Where(x => x.aspnetuserId == Id).OrderByDescending(a => a.Id).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Team> GetTeamById(int id)
        {
            return await _context.Teams.FindAsync(id);
        }

        public async Task<bool> DeleteTeamById(int id)
        {
            _context.Teams.Remove(_context.Teams.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }


        public async Task<int> GetRootId(int currentID)
        {
            Team team;
            do
            {
                team = await _context.Teams.FindAsync(currentID);
                currentID = team.teamId ?? 0;
            }
            while (currentID != 0);
            //  int a = 10;
            return team.Id;
        }

        public async Task<IEnumerable<CRMTeamViewModel>> GetAllCRMTeam()
        {
            var result = await _context.Teams.Include(x=>x.area).Include(x=>x.team).Include(x=>x.module).Include(x=>x.aspnetuser).ToListAsync();
            List<CRMTeamViewModel> team = new List<CRMTeamViewModel>();
            foreach (var item in result)
            {
                team.Add(new CRMTeamViewModel
                {
                    team = item,
                    empName = await _context.employeeInfos.Where(x => x.ApplicationUserId == item.aspnetuserId).Select(x => x.nameEnglish).FirstOrDefaultAsync()
                });
            }
            return team;
        }

        public async Task<CRMTeamMaster> GetTeamMasterById(int id)
        {
            return await _context.CRMTeamMasters.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveTeamMember(CRMTeamMember teamMember)
        {
            if (teamMember.Id > 0)
            {
                _context.Entry(teamMember).State = EntityState.Modified;
            }
            else
            {
                _context.CRMTeamMembers.Add(teamMember);
            }
            await _context.SaveChangesAsync();
            return teamMember.Id;
        }

        public async Task<int> SaveTeamMaster(CRMTeamMaster teamMaster)
        {
            if (teamMaster.Id > 0)
            {
                _context.CRMTeamMasters.Update(teamMaster);
            }
            else
            {
                _context.CRMTeamMasters.Add(teamMaster);
            }
            await _context.SaveChangesAsync();
            return teamMaster.Id;
        }

        public async Task<IEnumerable<TeamListViewModel>> GetTeamMembersByTeamMasterId(int id)
        {
            //var result = await (from M in _context.TeamMembers
            var result = await (from M in _context.CRMTeamMembers.GroupBy(x => x.memberId).Select(x => x.FirstOrDefault())
                                join Tm in _context.CRMTeamMasters on M.cRMTeamMasterId equals Tm.Id
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

        public async Task<IEnumerable<EmployeeInfo>> GetAllTeamMemberByMasterId1(int id)
        {
            var result = await (from D in _context.CRMTeamMembers
                          join M in _context.CRMTeamMasters on D.cRMTeamMasterId equals M.Id
                          join MM in _context.Users on M.leaderId equals MM.userId
                          join EMM in _context.employeeInfos on MM.Id equals EMM.ApplicationUserId
                          join S in _context.Users on D.memberId equals S.userId
                          join E in _context.employeeInfos on S.Id equals E.ApplicationUserId
                          where D.cRMTeamMasterId == id
                          select E).Include(x=>x.ApplicationUser).AsNoTracking().ToListAsync();

            return result;
        }

        public async Task<IEnumerable<CRMTeamViewModel>> GetTeamInfoByTeamId(int? teamId)
        {
            List<CRMTeamViewModel> result = (from T in _context.Teams
                                             join U in _context.Users on T.aspnetuserId equals U.Id
                                             join E in _context.employeeInfos on U.EmpCode equals E.employeeCode
                                             //where T.teamId == teamId
                                             select new CRMTeamViewModel
                                             {
                                                 tId = T.Id,
                                                 teamId = T.teamId,
                                                 areaId = T.area.Id,
                                                 memberName = T.memberName,
                                                 teamCode = T.teamCode,
                                                 aspnetuserId = T.aspnetuserId,
                                                 areaName = T.area.areaName,
                                                 empName=E.nameEnglish,
                                                 moduleId=T.moduleId

                                             }).OrderByDescending(a => a.tId).ToList();
            return result;
        }

        public async Task<IEnumerable<TeamListViewModel>> GetAllTeamInfoList()
        {
            var result = await (from D in _context.Users
                                orderby D.userId descending
                                join M in _context.CRMTeamMasters on D.userId equals M.leaderId
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

        public async Task<string> getLeaderNameByLeaderId(int id)
        {
            var result = await (from m in _context.CRMTeamMasters
                                join u in _context.Users on m.leaderId equals u.userId
                                join e in _context.employeeInfos on u.Id equals e.ApplicationUserId
                                where u.userId == id
                                select e.nameEnglish).AsNoTracking().FirstOrDefaultAsync();

            return result;
        }
        public async Task<string> getLeaderPhotoByLeaderId(int id)
        {
            var result = await (from m in _context.CRMTeamMasters
                                join u in _context.Users on m.leaderId equals u.userId
                                join e in _context.employeeInfos on u.Id equals e.ApplicationUserId
                                join p in _context.photographs on e.Id equals p.employeeId
                                where u.userId == id
                                select p.url).AsNoTracking().FirstOrDefaultAsync();

            return result;
        }

        public async Task<bool> DeleteTeamMemberById(int id)
        {
            _context.CRMTeamMembers.RemoveRange(await _context.CRMTeamMembers.Where(x => x.cRMTeamMasterId == id).ToListAsync());
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Team>> GetTeamByTeamIdAndUserId(int? teamId, string aspnetuserId)
        {
            return await _context.Teams.Include(x => x.area).Where(x => x.teamId == teamId && x.aspnetuserId == aspnetuserId).AsNoTracking().ToListAsync();
        }

        public async Task<int> SaveTeamNew(Team team)
        {
            if (team.Id != 0)
                _context.Teams.Update(team);
            else
                _context.Teams.Add(team);

            await _context.SaveChangesAsync();
            return team.Id;
        }
    }
}
