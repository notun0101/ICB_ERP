using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Employee.Interfaces
{
    public interface IServiceHistoryService
    {
        Task<TransferLog> GetTransferLogByEmpId1(int id);
        Task<int> SaveTransferLog(TransferLog transferLog);
      //  Task<bool> SaveServiceHistory(TransferLog traveInfo);
        Task<int> SaveServiceHistory(TransferLog traveInfo);
        Task<IEnumerable<TransferLog>> GetServiceHistory();
        Task<TransferLog> GetServiceHistoryById(int id);
        Task<bool> DeleteServiceHistoryById(int id);
        Task<IEnumerable<TransferLog>> GetServiceHistoryByEmpId(int empId);
        Task<TransferLog> GetTransferLogByEmpId(int id);
        Task<int> SaveEmpPostingPlace(EmployeePostingPlace postingPlace);
        Task<EmployeePostingPlace> GetEmpPostingPlaceByEmpId(int id);
        Task<EmployeePostingPlace> GetEmpPostingPlaceByEmpIdAndFromDate(int empId, DateTime? fromdate);
        Task<IEnumerable<EmployeePostingPlace>> GetPostingPlaceByEmpId(int empId);
        Task<TransferLog> GetTransferLogById(int id);
        Task<EmployeeInfo> GetJoiningSignatoryByTransferId(int id, string code);
    }
}
