﻿using OPUSERP.Areas.Auth.Models;
using OPUSERP.Data.Entity.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OPUSERP.ERPServices.AuthService.Interfaces
{
    public interface IDbChangeService
    {
        Task<int> SaveUserLogHistory(UserLogHistory userLogHistory);
        Task<UserLogHistory> GetUserLastLogHistory(string empCode);

        List<UserLogsViewModel> GetUserLogs(string empCode);


		Task<IEnumerable<UserLogHistory>> GetAllUserLogHistory();

        Task<IEnumerable<UserLogHistory>> GetUserLogHistoryByUser(string userName);
    }
}
