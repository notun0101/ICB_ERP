using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Employee
{
    public class ServiceHistoryService : IServiceHistoryService
    {
        private readonly ERPDbContext _context;

        public ServiceHistoryService(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteServiceHistoryById(int id)
        {
            _context.transferLogs.Remove(_context.transferLogs.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TransferLog>> GetServiceHistory()
        {
            return await _context.transferLogs.Include(x => x.department).Include(x => x.designatio).Include(x => x.toBranch).Include(x => x.employee).Include(x => x.employee.department).Include(x => x.employee.lastDesignation).Include(x => x.salaryGrade).Include(x => x.employee.hrBranch).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TransferLog>> GetServiceHistoryByEmpId(int empId)
        {
            return await _context.transferLogs.Where(x => x.employeeId == empId).Include(x => x.department).Include(x => x.designatio).Include(x => x.employee).Include(x => x.salaryGrade).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<EmployeePostingPlace>> GetPostingPlaceByEmpId(int empId)
        {
            return await _context.employeePostings.Where(x => x.employeeId == empId).Include(x => x.department).Include(x => x.hrDivision).Include(x => x.hrBranch).Include(x => x.location).Include(x => x.office).Include(x => x.employee).AsNoTracking().ToListAsync();
        }

        public async Task<TransferLog> GetServiceHistoryById(int id)
        {
            return await _context.transferLogs.FindAsync(id);
        }
        public async Task<EmployeePostingPlace> GetEmpPostingPlaceByEmpIdAndFromDate(int empId, DateTime? fromdate)
        {
            return await _context.employeePostings.Where(x => x.employeeId == empId && Convert.ToDateTime(x.startDate).Date == Convert.ToDateTime(fromdate).Date).FirstOrDefaultAsync();
        }
        // public async Task<TransferLog> GetTransferLogByEmpId(int id)
        //{
        //    return await _context.transferLogs.FirstOrDefaultAsync();
        //}

        public async Task<TransferLog> GetTransferLogByEmpId(int id)
        {
            return await _context.transferLogs.Where(x => x.employeeId == id).FirstAsync();
        }
        public async Task<EmployeeInfo> GetJoiningSignatoryByTransferId(int id, string code)
        {
            return await _context.employeeInfos.Include(x => x.lastDesignation).Where(x => x.employeeCode == code).FirstOrDefaultAsync();
        }

        public async Task<TransferLog> GetTransferLogByEmpId1(int id)
        {
            return await _context.transferLogs.Where(x => x.employeeId == id).FirstOrDefaultAsync();
        }


        public async Task<TransferLog> GetTransferLogById(int id)
        {
            return await _context.transferLogs.Where(x => x.Id == id).FirstAsync();
        }




        public async Task<int> SaveServiceHistory(TransferLog transferLog)
        {
            try
            {
                if (transferLog.Id != 0)
                    _context.transferLogs.Update(transferLog);
                else
                    _context.transferLogs.Add(transferLog);
                await _context.SaveChangesAsync();
                return transferLog.Id;
            }
            catch (System.Exception)
            {

                throw;
            }
        }



        public async Task<int> SaveTransferLog(TransferLog transferLog)
        {
            try
            {
                if (transferLog.Id != 0)
                    _context.transferLogs.Update(transferLog);
                else
                    _context.transferLogs.Add(transferLog);
                await _context.SaveChangesAsync();
                return transferLog.Id;
            }
            catch (System.Exception)
            {

                throw;
            }
        }











        public async Task<int> SaveEmpPostingPlace(EmployeePostingPlace postingPlace)
        {
            if (postingPlace.Id != 0)
                _context.employeePostings.Update(postingPlace);
            else
                _context.employeePostings.Add(postingPlace);

            await _context.SaveChangesAsync();
			return postingPlace.Id;
        }

        public async Task<EmployeePostingPlace> GetEmpPostingPlaceByEmpId(int id)
        {
            return await _context.employeePostings.Where(x => x.employeeId == id && x.endDate == null).LastOrDefaultAsync();
        }
    }
}
