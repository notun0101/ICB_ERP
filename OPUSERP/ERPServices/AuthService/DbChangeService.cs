using OPUSERP.Areas.Auth.Models;
using OPUSERP.Data;
using OPUSERP.Data.Entity.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.ERPServices.AuthService.Interfaces;
using OPUSERP.Helpers;

namespace OPUSERP.ERPServices.AuthService
{
    public class DbChangeService: IDbChangeService
    {
        private readonly ERPDbContext _context;

        public DbChangeService(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveUserLogHistory(UserLogHistory userLogHistory)
        {
            try
            {
                if (userLogHistory.Id != 0)
                {
                    _context.UserLogHistories.Update(userLogHistory);
                }
                else
                {
                    _context.UserLogHistories.Add(userLogHistory);
                }

                await _context.SaveChangesAsync();
                return userLogHistory.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public List<UserLogsViewModel> GetUserLogs(string empCode)
		{

			var logs = new List<string>();
			if (empCode != "0")
			{
				logs = _context.UserLogHistories.Where(x => x.userId == empCode).Select(x => x.userId).AsNoTracking().Distinct().ToList();
			}
			else
			{
				logs = _context.UserLogHistories.Select(x => x.userId).AsNoTracking().Distinct().ToList();
			}
			var emps = _context.employeeInfos.Where(x => logs.Contains(x.employeeCode)).Include(x => x.lastDesignation).Include(x => x.religion).Include(x => x.fatherOccupation).Include(x => x.motherOccupation).AsNoTracking().ToList();

			var data = new List<UserLogsViewModel>();

			foreach (var item in logs)
			{
				data.Add(new UserLogsViewModel
				{
					empCode = item,
					empName = emps.Where(x => x.employeeCode == item).Select(x => x.nameEnglish).FirstOrDefault(),
					designation = emps.Where(x => x.employeeCode == item).Select(x => x.lastDesignation?.shortName).FirstOrDefault(),
					dateOfBirth = emps.Where(x => x.employeeCode == item).Select(x => x.dateOfBirth).FirstOrDefault(),
					joiningDate = emps.Where(x => x.employeeCode == item).Select(x => x.joiningDateGovtService).FirstOrDefault(),
					emailOffice = emps.Where(x => x.employeeCode == item).Select(x => x.emailAddress).FirstOrDefault(),
					emailPersonal = emps.Where(x => x.employeeCode == item).Select(x => x.emailAddressPersonal).FirstOrDefault(),
					phoneNumberOffice = emps.Where(x => x.employeeCode == item).Select(x => x.mobileNumberOffice).FirstOrDefault(),
					phoneNumberPersonal = emps.Where(x => x.employeeCode == item).Select(x => x.mobileNumberPersonal).FirstOrDefault(),
					gender = emps.Where(x => x.employeeCode == item).Select(x => x.gender).FirstOrDefault(),
					bloodGroup = emps.Where(x => x.employeeCode == item).Select(x => x.bloodGroup).FirstOrDefault(),
					nid = emps.Where(x => x.employeeCode == item).Select(x => x.nationalID).FirstOrDefault(),
					homeDistrict = emps.Where(x => x.employeeCode == item).Select(x => x.homeDistrict).FirstOrDefault(),
					disability = emps.Where(x => x.employeeCode == item).Select(x => x.disability).FirstOrDefault(),
					maritalStatus = emps.Where(x => x.employeeCode == item).Select(x => x.maritalStatus).FirstOrDefault(),
					religion = emps.Where(x => x.employeeCode == item).Select(x => x.religion?.name).FirstOrDefault(),
					salaryAccount = emps.Where(x => x.employeeCode == item).Select(x => x.sbAccount).FirstOrDefault(),
					passportNo = emps.Where(x => x.employeeCode == item).Select(x => x.PassportNo).FirstOrDefault(),
					passportIssue = emps.Where(x => x.employeeCode == item).Select(x => x.PassportIssueDate).FirstOrDefault(),
					passportExpire = emps.Where(x => x.employeeCode == item).Select(x => x.PassportExDate).FirstOrDefault(),
					fatherName = emps.Where(x => x.employeeCode == item).Select(x => x.fatherNameEnglish).FirstOrDefault(),
					fatherOccupation = emps.Where(x => x.employeeCode == item).Select(x => x.fatherOccupation?.occupationName).FirstOrDefault(),
					fatherMobile = emps.Where(x => x.employeeCode == item).Select(x => x.fatherMobile).FirstOrDefault(),
					motherName = emps.Where(x => x.employeeCode == item).Select(x => x.motherNameEnglish).FirstOrDefault(),
					motherOccupation = emps.Where(x => x.employeeCode == item).Select(x => x.motherOccupation?.occupationName).FirstOrDefault(),
					motherMobile = emps.Where(x => x.employeeCode == item).Select(x => x.motherMobile).FirstOrDefault(),
					employeeSpouses = _context.spouses.Where(x => x.employee.employeeCode == item).ToList(),
					employeeChildren = _context.childrens.Where(x => x.employee.employeeCode == item).ToList(),
					posting = emps.Where(x => x.employeeCode == item).Select(x => x.presentPosting).FirstOrDefault(),
					ipAddress1 = _context.UserLogHistories.Where(x => x.userId == item).Select(x => x.ipAddress).LastOrDefault(),
					ipAddress2 = _context.UserLogHistories.Where(x => x.userId == item).Select(x => x.ipAddress2).LastOrDefault(),
					MAC = _context.UserLogHistories.Where(x => x.userId == item).Select(x => x.mac).LastOrDefault(),
					PCName = _context.UserLogHistories.Where(x => x.userId == item).Select(x => x.pcName).LastOrDefault(),
					Latitude = _context.UserLogHistories.Where(x => x.userId == item).Select(x => x.Latitude).LastOrDefault(),
					Longitude = _context.UserLogHistories.Where(x => x.userId == item).Select(x => x.Longitude).LastOrDefault(),
					Address = _context.UserLogHistories.Where(x => x.userId == item).Select(x => x.Address).LastOrDefault(),
					LastLogin = _context.UserLogHistories.Where(x => x.userId == item).Select(x => x.logTime).LastOrDefault(),
				});
			}
			return data;
		}
		public async Task<UserLogHistory> GetUserLastLogHistory(string empCode)
		{ 
            return await _context.UserLogHistories.Where(x=>x.userId==empCode).OrderByDescending(x=>x.Id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<UserLogHistory>> GetAllUserLogHistory()
        {
            return await _context.UserLogHistories.Select(x=>new UserLogHistory {userId=x.userId,logTime=x.logTime,ipAddress=x.ipAddress}).ToListAsync();
        }

        public async Task<IEnumerable<UserLogHistory>> GetUserLogHistoryByUser(string userName)
        {
            return await _context.UserLogHistories.Where(x => x.createdBy == userName).ToListAsync();
        }






    }
}
