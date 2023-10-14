using Microsoft.EntityFrameworkCore;
using OPUSERP.Areas.HRPMSACR.Models;
using OPUSERP.Areas.HRPMSLeave.Models;
using OPUSERP.Areas.HRPMSLeave.Models.NotMapped;
using OPUSERP.Areas.HRPMSRecruitment.Controllers;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Leave;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Leave
{
    public class LeaveRegisterService : ILeaveRegisterService
    {
        private readonly ERPDbContext _context;

        public LeaveRegisterService(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveLeaveRegister(LeaveRegister leaveRegister)
        {
            try
            {
                if (leaveRegister.Id != 0)
                    _context.leaveRegisters.Update(leaveRegister);
                else
                    _context.leaveRegisters.Add(leaveRegister);
                await _context.SaveChangesAsync();
                return leaveRegister.Id;
            }
            catch (Exception ex)
            {

                throw ex;
            }
          
        }

        #region
        public async Task<bool> UpdateReasonStatus(int? id, string reason)
        {
            var LeaveRegisters = _context.leaveRegisters.Find(id);
            LeaveRegisters.reason = reason;
            _context.Entry(LeaveRegisters).State = EntityState.Modified;
            return 1 == await _context.SaveChangesAsync();
        }
        #endregion


        public async Task<bool> DeleteRecordById(int id)
        {            
            _context.leaveRegisters.Remove(_context.leaveRegisters.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LeaveRegister>> GetAllLeaveRegister()
        {
            return await _context.leaveRegisters.Include(x => x.employee).Include(x => x.leaveType).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<LeaveRegister>> GetAllReCreationLeave()
        {
            return await _context.leaveRegisters.Include(x => x.employee).Include(x => x.leaveType).Where(x => x.leaveTypeId == 16).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<LeaveRegister>> GetAllReCreationLeaveByDateRange(DateTime? from, DateTime? to)
        {
            var data = await _context.leaveRegisters.Include(x => x.employee).Include(x => x.leaveType).Where(x => Convert.ToDateTime(x.leaveFrom).Date >= Convert.ToDateTime(from).Date && Convert.ToDateTime(x.leaveTo).Date <= Convert.ToDateTime(to).Date && x.leaveTypeId == 16).ToListAsync();
            return data;
        }

        public async Task<IEnumerable<NotificationVmAcr>> SendNotification(string username, string sender, string receiver, string title, string description, string type, string url)
        {
            var data = await _context.notificationVmAcrs.FromSql($"sp_SendNotification {username}, {sender}, {receiver}, {title}, {description}, {type}, {url}").AsNoTracking().ToListAsync();
            return data;
        }


        public async Task<IEnumerable<LeaveRegister>> GetAllReCreationLeaveByDateAndTypeRange(DateTime? from, DateTime? to, string type, int? typeId)
        {
            var data = await _context.leaveRegisters.Include(x => x.employee).Include(x => x.leaveType).Where(x => Convert.ToDateTime(x.leaveFrom).Date >= Convert.ToDateTime(from).Date && Convert.ToDateTime(x.leaveTo).Date <= Convert.ToDateTime(to).Date && x.leaveTypeId == 16).ToListAsync();

            if (type == "Department")
            {
                return data.Where(x => x.employee.departmentId == typeId).ToList();
            }
            else if (type == "Branch")
            {
                return data.Where(x => x.employee.hrBranchId == typeId).ToList();
            }
            else if (type == "Zone")
            {
                return data.Where(x => x.employee.locationId == typeId).ToList();
            }
            else
            {
                return data;
            }
        }

        public async Task<IEnumerable<LeaveRegister>> GetLeavesByType(int typeId, int empId)
        {
            var data = await _context.leaveRegisters.AsNoTracking().Where(x => x.leaveTypeId == typeId && x.employeeId == empId && x.leaveStatus == 3).ToListAsync();
            return data;
        }
        public async Task<int> DeleteLeaveStatusByRegId(int id)
        {
            var data = await _context.leaveStatusLogs.Where(x => x.leaveRegisterId == id).ToListAsync();
            _context.leaveStatusLogs.RemoveRange(data);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> DeleteLeaveRouteByRegId(int id)
        {
            var data = await _context.leaveRoutes.Where(x => x.leaveRegisterId == id).ToListAsync();
            _context.leaveRoutes.RemoveRange(data);
            return await _context.SaveChangesAsync();
        }
        //new
        //public async Task<IEnumerable<LeaveRegister>> GetAllApproveLeaveByEmpid()
        //{
        //    return await _context.leaveRegisters.Include(x => x.employee).Include(x => x.leaveType).Include(x=>x.daysLeave).Include(x=>x.leaveFrom).Include(x=>x.leaveTo).Include(x=>x.purposeOfLeave).Include(x=>x.whenLeave).Where(x=>x.leaveStatus==3).AsNoTracking().ToListAsync();
        //}
        //

        public async Task<IEnumerable<LeaveRegister>> GetAllLeaveByUserIdAndStatus(int userid, int status)
        {
            var emp = _context.employeeInfos.Where(x => x.ApplicationUser.userId == userid).FirstOrDefault();
            return await _context.leaveRegisters.Include(x => x.employee).Include(x => x.leaveType).Where(x => x.employeeId == emp.Id && x.leaveStatus == status).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<LeaveRegister>> GetTodayAprovedLeave(int userid, int status)
        {
            var emp = _context.employeeInfos.Where(x => x.ApplicationUser.userId == userid).FirstOrDefault();
            return await _context.leaveRegisters.Include(x => x.employee).Include(x => x.leaveType).Where(x => x.leaveStatus == status && Convert.ToDateTime(x.leaveFrom).Date <= DateTime.Now.Date && Convert.ToDateTime(x.leaveTo).Date >= DateTime.Now.Date).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<LeaveRegister>> GetAvailedLeaveByUser(int userid)
        {
            var emp = _context.employeeInfos.Where(x => x.ApplicationUser.userId == userid).FirstOrDefault();
            return await _context.leaveRegisters.Include(x => x.employee).Include(x => x.leaveType).Where(x => x.employeeId == emp.Id && Convert.ToDateTime(x.leaveTo).Date < DateTime.Now.Date).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<LeaveRegister>> GetAllLeaveByUserId(int userid)
        {
            var emp = _context.employeeInfos.Where(x => x.ApplicationUser.userId == userid).FirstOrDefault();
            return await _context.leaveRegisters.Include(x => x.employee).Include(x => x.leaveType).Where(x => x.employeeId == emp.Id).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<LeaveRegister>> GetAllLeaveRegisterByUser(int userid)
        {
            var emp = _context.employeeInfos.Where(x => x.ApplicationUser.userId == userid).FirstOrDefault();
            return await _context.leaveRegisters.Include(x => x.employee).Include(x => x.leaveType).Where(x => x.employeeId == emp.Id).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<LeaveSummaryReport>> GetAllLeaveSummaryReport(string year)
        {
            var employee = await _context.employeeInfos.AsNoTracking().ToListAsync();
            List<LeaveSummaryReport> leaveSummaryReports = new List<LeaveSummaryReport>();
            foreach (var emp in employee)
            {
                List<LeaveSummary> leaveSummaries = new List<LeaveSummary>();
                var leaveTypes = await _context.leaveTypes.AsNoTracking().ToListAsync();
                foreach (var data in leaveTypes)
                {
                    decimal? open = await _context.leaveOpeningBalances.Where(x => x.leaveTypeId == data.Id && x.employeeId == emp.Id && x.year.year == year).Select(x => x.leaveDays).FirstOrDefaultAsync();
                    if (open == null)
                    {
                        open = 0;
                    }
                    int bal = await GetLeaveBalanceByempIdYear(emp.Id, data.Id, year);
                    leaveSummaries.Add(new LeaveSummary
                    {
                        type = data.nameEn,
                        Opening = (int)open,
                        Balance = bal,
                        Leave = (int)open - bal,
                    });
                }
                leaveSummaryReports.Add(new LeaveSummaryReport
                {
                    Name = emp.nameEnglish,
                    Code = emp.employeeCode,
                    leaveSummaries = leaveSummaries,
                });
            }
            return leaveSummaryReports;
        }

        public async Task<IEnumerable<LeaveRegister>> GetAllLeaveRegisterByEmpIdAndStatus(int empId, int status)
        {
            return await _context.leaveRegisters.Where(x => x.employeeId == empId && x.leaveStatus == status).Include(x => x.employee).Include(x => x.leaveType).AsNoTracking().ToListAsync();
        }
        #region
        public async Task<IEnumerable<LeaveRegister>> GetAllLeaveRegisterByEmpIdAndStatusAPI(int empId, int status)
        {
            return await _context.leaveRegisters
                .Where(x => x.employeeId == empId && x.leaveStatus == status)
                .Include(x => x.employee)
                .Include(x => x.leaveType)
                .AsNoTracking().ToListAsync();
        }

        #endregion

        public async Task<EmployeeInfo> GetGeneralLeaveSummarybyId(string id)
        {
            var data = await _context.employeeInfos.Include(x => x.lastDesignation).Where(x => x.employeeCode == id).FirstOrDefaultAsync();
            return data;
        }

        public async Task<decimal> GetLeaveCountByTypeShortNameAndEmpId(string leaveType, int empId)
        {
            var data = await _context.leaveRegisters.Where(x => x.leaveType.shortName == leaveType && x.employeeId == empId && x.leaveStatus == 3).AsNoTracking().SumAsync(x => x.daysLeave);
            return Convert.ToDecimal(data);
        }

        public async Task<int> GetReCreationLeaveCountByEmpId(string leaveType, int empId)
        {
            var data = await _context.leaveRegisters.Where(x => x.leaveType.nameEn == leaveType && x.employeeId == empId).AsNoTracking().CountAsync();
            //return Convert.ToDecimal(data);
            return data;
        }

        //new
        public async Task<IEnumerable<LeaveRegister>> GetAllLeaveApprovByEmpIdAndStatus(int status)
        {
            return await _context.leaveRegisters.Where(x => x.leaveStatus == status).Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.leaveType).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<LeaveRegister>> GetAllLeaveByEmpId(int id)
        {
            return await _context.leaveRegisters.Where(x => x.employeeId == id).Include(x => x.employee).Include(x => x.leaveType).AsNoTracking().ToListAsync();
        }


        public async Task<int> CancelApprovedLeave(int id)
        {
            var reg = _context.leaveRegisters.Find(id);
            reg.leaveStatus = 4;
            _context.leaveRegisters.Update(reg);
            _context.SaveChanges();

            var opening = _context.leaveOpeningBalances.Where(x => x.employeeId == reg.employeeId).FirstOrDefault();
            opening.leaveDays += Convert.ToInt32(reg.daysLeave);
            _context.leaveOpeningBalances.Update(opening);
            return await _context.SaveChangesAsync();
        }
		//
		public async Task<IEnumerable<LeaveRegister>> GetAllLeaveRegisterByEmpIdLeaveApply(int empId)
		{
			return await _context.leaveRegisters.Where(x => x.employeeId == empId).Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.leaveType).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<LeaveRegister>> GetAllLeaveRegisterByEmpId(int empId)
        {
            return await _context.leaveRegisters.Where(x => x.leaveStatus == 3 && x.paymentOption == "Manual Recreation" && x.leaveTypeId == 16).Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.leaveType).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<LeaveRegister>>ManualRecrationListForAppList(int empId)
        {
            return await _context.leaveRegisters.Where(x => x.leaveStatus == 33 && x.paymentOption == "Manual Recreation" && x.leaveTypeId == 16).Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.leaveType).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<LeaveRegister>>ProposedRecrationListForAppList(int empId)
        {
            return await _context.leaveRegisters.Where(x =>( x.leaveStatus == 1 || x.leaveStatus == 34 || x.leaveStatus == 36) && x.paymentOption == "Proposed Recreation" && x.leaveTypeId == 16 && x.employeeId==empId).Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.leaveType).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<LeaveRegister>>ApprovedProposedRecrationListEmployee(int empId)
        {
            return await _context.leaveRegisters.Where(x => (x.leaveStatus == 34 || x.leaveStatus == 36|| x.leaveStatus == 1|| x.leaveStatus == 38) && x.paymentOption == "Proposed Recreation" && x.leaveTypeId == 16 && x.employeeId==empId).Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.leaveType).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        }


         public async Task<IEnumerable<LeaveRegister>>ActualLeaveApplyFromProposedEmployee(int empId,int year)
        {
            return await _context.leaveRegisters.Where(x =>  x.leaveStatus == 38 && x.paymentOption == "Proposed Recreation" && x.leaveTypeId == 16 && x.employeeId==empId && Convert.ToDateTime(x.leaveFrom).Year == year).Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.leaveType).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        }






         public async Task<IEnumerable<ProposedListForHeadViewModel>> GetAllProposedLeaveByDepBrZId(int? depId,int? branchId,int? zoneId)
        {
            try
            {
                var data = await _context.proposedListForHeadVM.FromSql($"sp_GetProposedLeaveForHead  @p0,@p1,@p2", depId, branchId, zoneId).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
      
        }
         public async Task<IEnumerable<ProposedListForZoneHeadViewModel>> GetAllProLeaveByDepBrZId(int? depId,int? branchId,int? zoneId)
        {
            try
            {
                var data = await _context.proposedListForZoneHeadVM.FromSql($"sp_GetProposedLeaveForHeadNew  @p0,@p1,@p2", depId, branchId, zoneId).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
      
        }





        public async Task<IEnumerable<ProposedListForHeadViewModel>> GetAllProposedLForZoneHead(int? zoneId)
        {
            try
            {
                var data = await _context.proposedListForHeadVM.FromSql($"sp_GetProposedLeaveForZoneHead @p0", zoneId).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        #region--sp_GetProposedLeaveForZoneHeadNew
        public async Task<IEnumerable<ProposedListForZoneHeadViewModel>> GetAllProposedZoneForZoneHead(int? zoneId)
        {
            try
            {
                var data = await _context.proposedListForZoneHeadVM.FromSql($"sp_GetProposedLeaveForZoneHeadNew @p0", zoneId).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        #endregion


        public async Task<IEnumerable<ProposedListForHeadViewModel>> GetAPrrovedProposedLeaveByDepBrZIdZForHead(int? depId,int? branchId,int? zoneId)
        {
            try
            {
                var data = await _context.proposedListForHeadVM.FromSql($"sp_GetApproveProposedLListForHead  @p0,@p1,@p2", depId, branchId, zoneId).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
      
        }

        public async Task<IEnumerable<ProposedListForHeadViewModel>> GetAPrrovedProposedLeaveByDepBrZIdZForHeadByYear(int? depId,int? branchId,int? zoneId,int? year)
        {
            try
            {
                var data = await _context.proposedListForHeadVM.FromSql($"sp_GetApproveProposedLListForHeadByYear  @p0,@p1,@p2,@p3", depId, branchId, zoneId, year).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
      
        }
        public async Task<IEnumerable<ReportProposedListViewModel>> GetAPrrovedProposedLeaveByYearReport(int? depId,int? branchId,int? zoneId,int? year)
        {
            try
            {
                var data = await _context.reportProposedListVM.FromSql($"sp_ReportProposedListForHeadByYear  @p0,@p1,@p2,@p3", depId, branchId, zoneId, year).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
      
        }


        public async Task<IEnumerable<LeaveRegister>> ProposedRecrationListForHrAdmin()
        {
            return await _context.leaveRegisters.Where(x => x.leaveStatus == 34 && x.paymentOption == "Proposed Recreation" && x.leaveTypeId == 16).Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.leaveType).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<LeaveRegister>> ApprovedProposedListForHrAdmin()
        {
            return await _context.leaveRegisters.Where(x => (x.leaveStatus == 36 || x.leaveStatus == 38|| x.leaveStatus == 40|| x.leaveStatus == 41|| x.leaveStatus == 3) && (x.paymentOption == "Proposed Recreation" || x.paymentOption == "Actual Recreation") && x.leaveTypeId == 16).Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.leaveType).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<LeaveRegister>> FinalAnumodonProposedListForHrAdmin()
        {
            return await _context.leaveRegisters.Where(x => x.leaveStatus == 36  && x.paymentOption == "Proposed Recreation"  && x.leaveTypeId == 16).Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.leaveType).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        }



         public async Task<IEnumerable<LeaveRegister>> ApprovedProposedListForHrAdminReportByYear( int? year)
        {
            return await _context.leaveRegisters.Where(x => x.leaveStatus == 36 && x.paymentOption == "Proposed Recreation" && x.leaveTypeId == 16 && Convert.ToDateTime(x.leaveFrom).Year == year).Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.employee.department).Include(x => x.employee.location).Include(x => x.leaveType).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<LeaveRegister>> ApprovedProposedListForHrAdminReportByIndEmp( int? empId)
        {
            var data= await _context.leaveRegisters.Where(x => x.leaveStatus == 36 && x.paymentOption == "Proposed Recreation" && x.leaveTypeId == 16 && x.employeeId == empId).Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.employee.department).Include(x => x.employee.location).Include(x => x.leaveType).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
            return data;
        }

        public async Task<IEnumerable<LeaveRegister>> AnumoditoProposedListForHrAdmin()
        {
            return await _context.leaveRegisters.Where(x => (x.leaveStatus == 38 || x.leaveStatus == 40 || x.leaveStatus == 41 || x.leaveStatus == 3) && (x.paymentOption == "Proposed Recreation" || x.paymentOption == "Actual Recreation") && x.leaveTypeId == 16).Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.employee.department).Include(x => x.employee.location).Include(x => x.leaveType).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        }
         public async Task<IEnumerable<LeaveRegister>> AnumoditoRejectProposedForHrAdmin()
        {
            return await _context.leaveRegisters.Where(x => x.leaveStatus == 39 && x.paymentOption == "Proposed Recreation" && x.leaveTypeId == 16).Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.employee.department).Include(x => x.employee.location).Include(x => x.leaveType).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        }


        public async Task<bool> SaveCommetitteDetail(LeaveCommittee leaveCommittee)
        {
            if (leaveCommittee.Id != 0)
                _context.leaveCommittees.Update(leaveCommittee);
            else
                _context.leaveCommittees.Add(leaveCommittee);

            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteLeaveCommitteeById(int id)
        {
            _context.leaveCommittees.Remove(_context.leaveCommittees.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LeaveCommittee>> GetleaveRegisterList()
        {
            var data = await _context.leaveCommittees.Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.employee.department).Include(x => x.employee.location).AsNoTracking().ToListAsync();
            return data;
        }

         public async Task<IEnumerable<LeaveCommittee>> GetleaveRegisterListByYear(int? Year)
        {
            var data = await _context.leaveCommittees.Include(x => x.employee).Where(x=>x.CoYear==Year).Include(x => x.employee.lastDesignation).Include(x => x.employee.department).Include(x => x.employee.location).AsNoTracking().ToListAsync();
            return data;
        }






        public async Task<int> UpdateLeaveStatus(int id, int status)
        {
            var data = await _context.leaveRegisters.FindAsync(id);
            data.leaveStatus = status;
            _context.leaveRegisters.Update(data);
            await _context.SaveChangesAsync();
            return data.Id;
        }

        public async Task<int> UpdateManualLeaveStatus(int id)
        {
            try
            {
                var data = await _context.leaveRegisters.FindAsync(id);
                data.leaveStatus = 3;
                _context.leaveRegisters.Update(data);
                await _context.SaveChangesAsync();
                return data.Id;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        public async Task<int> UpdateProposedLeaveStatusByHead(int id)
        {
            try
            {
                var data = await _context.leaveRegisters.FindAsync(id);
                data.leaveStatus = 34;//Proposed Leave Approved By Head=34
                _context.leaveRegisters.Update(data);
                await _context.SaveChangesAsync();
                return data.Id;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
        public async Task<int> UpdateProposedLeaveStatusByHead1(int id, string reason)
        {
            try
            {
                var data = await _context.leaveRegisters.FindAsync(id);
                data.reason = reason;
                data.leaveStatus = 34;//Proposed Leave Approved By Head=34
                _context.leaveRegisters.Update(data);
                await _context.SaveChangesAsync();
                return data.Id;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<int> RejectProposedLeaveStatusByHead(int id)
        {
            try
            {
                var data = await _context.leaveRegisters.FindAsync(id);
                data.leaveStatus =35;//Proposed Leave Reject By Head=35
                _context.leaveRegisters.Update(data);
                await _context.SaveChangesAsync();
                return data.Id;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
        public async Task<int> ApprovedProposedStatusByHRadmin(int id)
        {
            try
            {
                var data = await _context.leaveRegisters.FindAsync(id);
                data.leaveStatus = 36;//Proposed Leave Approved By HrAdmin=36
                _context.leaveRegisters.Update(data);
                await _context.SaveChangesAsync();
                return data.Id;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        public async Task<int> RejectProposedStatusByHrAdmin(int id)
        {
            try
            {
                var data = await _context.leaveRegisters.FindAsync(id);
                data.leaveStatus =37;//Proposed Leave Reject By Head=37
                _context.leaveRegisters.Update(data);
                await _context.SaveChangesAsync();
                return data.Id;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        #region  অনুমোদিতLeave
        public async Task<int> AnumoditoProposedStatusByHRadmin(int id)
        {
            try
            {
                var data = await _context.leaveRegisters.FindAsync(id);
                data.leaveStatus = 38;//Proposed Leave Approved By HrAdmin=38
                _context.leaveRegisters.Update(data);
                await _context.SaveChangesAsync();
                return data.Id;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<int> AnumoditoRejectProposedStatusByHrAdmin(int id)
        {
            try
            {
                var data = await _context.leaveRegisters.FindAsync(id);
                data.leaveStatus = 39;//Proposed Leave Reject By Head=39
                _context.leaveRegisters.Update(data);
                await _context.SaveChangesAsync();
                return data.Id;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        #endregion

        #region Actual Leave

        public async Task<IEnumerable<LeaveRegister>> ApprovedActualListEmployee(int empId)
        {
            return await _context.leaveRegisters.Where(x => (x.leaveStatus == 40  || x.leaveStatus == 41 || x.leaveStatus == 42 || x.leaveStatus == 3 || x.leaveStatus == 44) && x.paymentOption == "Actual Recreation" && x.leaveTypeId == 16 && x.employeeId == empId).Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.leaveType).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        }


        public async Task<int> UpdateActualLeaveStatusByHead(int id)
        {
            try
            {
                var data = await _context.leaveRegisters.FindAsync(id);
                data.leaveStatus = 3;//Actual Leave Approved By Head=3
                _context.leaveRegisters.Update(data);
                await _context.SaveChangesAsync();
                return data.Id;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<int> ActualLeaveRejectStatusByHead(int id)
        {
            try
            {
                var data = await _context.leaveRegisters.FindAsync(id);
                data.leaveStatus = 42;//Proposed Leave Reject By Head=42
                _context.leaveRegisters.Update(data);
                await _context.SaveChangesAsync();
                return data.Id;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



        public async Task<IEnumerable<ActualListForHeadViewModel>> GetAllActualLeaveByDepBrZId(int? depId, int? branchId, int? zoneId)
        {
            try
            {
                var data = await _context.actualListForHeadVM.FromSql($"sp_GetActualLeaveForHead  @p0,@p1,@p2", depId, branchId, zoneId).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public async Task<IEnumerable<ActualListForHeadViewModel>> GetAllActualLForZoneHead(int? zoneId)
        {
            try
            {
                var data = await _context.actualListForHeadVM.FromSql($"sp_GetActualLeaveForZoneHead @p0", zoneId).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<IEnumerable<ActualListForHeadViewModel>> GetActualAprrovedLeaveByDepBrZIdZForHead(int? depId, int? branchId, int? zoneId)
        {
            try
            {
                var data = await _context.actualListForHeadVM.FromSql($"sp_GetActualApproveLeaveLListForHead  @p0,@p1,@p2", depId, branchId, zoneId).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<IEnumerable<ActualListForHeadViewModel>> GetActualAPrrovedLeaveByDepBrZIdZForHeadByYear(int? depId, int? branchId, int? zoneId, int? year)
        {
            try
            {
                var data = await _context.actualListForHeadVM.FromSql($"sp_GetActualApproveListForHeadByYear  @p0,@p1,@p2,@p3", depId, branchId, zoneId, year).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<IEnumerable<ReportActualListViewModel>> GetActualAPrrovedLeaveByYearReport(int? depId, int? branchId, int? zoneId, int? year)
        {
            try
            {
                var data = await _context.repotactualListForHeadVM.FromSql($"sp_ReportActualListForHeadByYear  @p0,@p1,@p2,@p3", depId, branchId, zoneId, year).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        //HRLeaveAdmin

        public async Task<IEnumerable<LeaveRegister>> ActualRecrationListForHrAdmin()
        {
            return await _context.leaveRegisters.Where(x => x.leaveStatus == 41 && x.paymentOption == "Actual Recreation" && x.leaveTypeId == 16).Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.substitutionUser).Include(x => x.substitutionUser.lastDesignation).Include(x => x.leaveType).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        }


        public async Task<int> ApprovedActualStatusByHRadmin(int id)
        {
            try
            {
                var data = await _context.leaveRegisters.FindAsync(id);
                data.leaveStatus = 3;//Actual Leave Approved By HrAdmin=3
                _context.leaveRegisters.Update(data);
                await _context.SaveChangesAsync();
                return data.Id;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<int> RejectActualStatusByHrAdmin(int id)
        {
            try
            {
                var data = await _context.leaveRegisters.FindAsync(id);
                data.leaveStatus = 44;//Actual Leave Reject By Head=44
                _context.leaveRegisters.Update(data);
                await _context.SaveChangesAsync();
                return data.Id;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<IEnumerable<LeaveRegister>> ApprovedActualListForHrAdmin()
        {
            return await _context.leaveRegisters.Where(x => x.leaveStatus == 3 && x.paymentOption == "Actual Recreation" && x.leaveTypeId == 16).Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.substitutionUser).Include(x => x.substitutionUser.lastDesignation).Include(x => x.leaveType).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        }


        public async Task<IEnumerable<ActualRecreationReportHRViewModel>> GetActualReportByDepIdForHrAdmin(int? deptId, int? hrBranchId, int? LZoneId, int? empID, int? yearId)
        {
            try
            {
                var data = await _context.actualRecreationReportHRVM.FromSql(@"sp_GetActualLeaveForHRAdmminDepBrZone @p0,@p1,@p2,@p3,@p4", deptId, hrBranchId, LZoneId, empID, yearId).AsNoTracking().ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        #endregion

        public async Task<IEnumerable<LeaveRegister>> GetAllLeaveRegisterByEmpIdForRecreation(int empId)
        {
            return await _context.leaveRegisters.Where(x => x.employeeId == empId).Where(x => x.leaveTypeId == 16).Include(x => x.employee).Include(x => x.leaveType).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        }


        public async Task<IEnumerable<LeaveRegister>> GetAllPendingLeaveByEmpId(int empId)
        {
            return await _context.leaveRegisters.Where(x => x.employeeId == empId).Where(x => x.leaveStatus == 1).Include(x => x.employee).Include(x => x.leaveType).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<LeaveRegister>> GetAllLeaveByEmpCode(string empId)
        {
            var employeeId = _context.employeeInfos.Where(x => x.employeeCode == Convert.ToString(empId)).Select(x => x.Id).FirstOrDefault();
            var data = await _context.leaveRegisters.Where(x => x.employeeId == employeeId && x.leaveStatus == 3).Include(x => x.employee).Include(x => x.leaveType).OrderByDescending(a => a.Id).AsNoTracking().ToListAsync();
            return data;
        }

        public async Task<IEnumerable<LeaveRegister>> GetAllLeaveRegisterByEmpIdAndDateRange(int empId, DateTime? from, DateTime? to)
        {
            return await _context.leaveRegisters.Where(x => x.employeeId == empId && x.leaveStatus == 3 && x.leaveFrom >= from && x.leaveTo <= to).Include(x => x.employee).Include(x => x.leaveType).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<LeaveRegisterNotMapped>> GetAllLeaveRegisterByEmpIdAndDate(int empId, DateTime? from, DateTime? to)
        {
            return await _context.leaveRegisterNotMappeds.FromSql(@"LeaveApplyValidation @p0,@p1,@p2", from?.ToString("yyyyMMdd"), to?.ToString("yyyyMMdd"), empId).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<ManualRecreationReportViewModel>> GetAllManualRecreationBranchReportByDate(DateTime? from, DateTime? to)
        {
            var fromDate = from?.ToString("yyyy-MM-dd");
            var toDate = to?.ToString("yyyy-MM-dd");
            var data = await _context.manualRecreationReportViewModels.FromSql(@"sp_getManualRecreationLeave @p0,@p1", fromDate, toDate).AsNoTracking().ToListAsync();
           return data;
        }
        public async Task<IEnumerable<ManualRecreationReportViewModel>> GetAllManualRecreationHeadOfficeReportByDate(DateTime? from, DateTime? to)
        {
            var fromDate = from?.ToString("yyyy-MM-dd");
            var toDate = to?.ToString("yyyy-MM-dd");
            var data = await _context.manualRecreationReportViewModels.FromSql(@"sp_getManualRecreationLeaveForHeadOffice @p0,@p1", fromDate, toDate).AsNoTracking().ToListAsync();
           return data;
        }


        public async Task<IEnumerable<ManualRecreationReportViewModel>> GetAllProposedReportByDepIdForHrAdmin(int? deptId, int? hrBranchId, int? LZoneId, int? empID, int? yearId)
        {
            try
            {
                var data = await _context.manualRecreationReportViewModels.FromSql(@"sp_getProposedLeaveForHRAdminDepBrZone @p0,@p1,@p2,@p3,@p4", deptId, hrBranchId, LZoneId, empID, yearId).AsNoTracking().ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        public async Task<IEnumerable<ManualRecreationReportViewModel>> GetAnumoditoLeaveForHrAdminByYear(int? year)
        {
            try
            {
                var data = await _context.manualRecreationReportViewModels.FromSql(@"sp_GetAnumoditoLeaveForHrByYear @p0",  year).AsNoTracking().ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }
         public async Task<IEnumerable<ManualRecreationReportViewModel>> GetFinalRejectProposedLeaveForHrAdminByYear(int? year)
        {
            try
            {
                var data = await _context.manualRecreationReportViewModels.FromSql(@"sp_GetFinalRejectProposedLeaveForHrByYear @p0",  year).AsNoTracking().ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        public async Task<bool> CheckLeave(string empCode, DateTime? fromDate, DateTime? toDate)
        {
            var employee = await _context.employeeInfos.Where(x => x.employeeCode == empCode).FirstOrDefaultAsync();

            var data = await _context.leaveRegisters.AsNoTracking()
                .Where(x => ((Convert.ToDateTime(x.leaveFrom).Date >= Convert.ToDateTime(fromDate).Date &&
                Convert.ToDateTime(x.leaveTo).Date <= Convert.ToDateTime(fromDate).Date) ||
                (Convert.ToDateTime(x.leaveFrom).Date >= Convert.ToDateTime(toDate).Date &&
                Convert.ToDateTime(x.leaveTo).Date <= Convert.ToDateTime(toDate).Date)) && x.employeeId == employee.Id).ToListAsync();

            //var data1 = await _context.leaveRegisters.AsNoTracking()
            //    .Where(x =>(Convert.ToDateTime(x.leaveFrom).Date >= Convert.ToDateTime(fromDate).Date &&
            //    Convert.ToDateTime(x.leaveTo).Date <= Convert.ToDateTime(fromDate).Date) ||
            //    (Convert.ToDateTime(x.leaveFrom).Date >= Convert.ToDateTime(toDate).Date && 
            //    Convert.ToDateTime(x.leaveTo).Date <= Convert.ToDateTime(toDate).Date) ).ToListAsync();


            //var data = data1.Where(x => x.employeeId == employee.Id).ToList();

            if (data.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<DayLeaveNotMapped> GetAllLeaveRegisterByEmpIdAndDateWithType(int empId, DateTime? from, DateTime? to, int typeId)
        {
            var data = await _context.dayLeaveNotMappeds.FromSql(@"LeaveApplyValidationWithType @p0,@p1,@p2,@p3", from?.ToString("yyyyMMdd"), to?.ToString("yyyyMMdd"), empId, typeId).AsNoTracking().FirstOrDefaultAsync();
            return data;
        }

        public async Task<LeaveRegister> GetLeaveRegisterById(int id)
        {
            return await _context.leaveRegisters.Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.substitutionUser).Include(x => x.substitutionUser.lastDesignation).Include(x => x.employee.department).Include(x => x.employee.hrUnit).Include(x => x.employee.hrBranch).Include(x => x.employee.functionInfo).Include(x => x.employee.hrDivision).Include(x => x.leaveType).Include(x => x.employee.ApplicationUser).AsNoTracking().Where(x => x.Id == id).FirstAsync();
        }
        public async Task<IEnumerable<LeaveStatusLog>> GetLeaveStatusLogByRegId(int id)
        {
            return await _context.leaveStatusLogs.Include(x => x.employee).Include(x => x.leaveRegister).Include(x => x.employee.ApplicationUser).Include(x => x.leaveRegister.leaveType).AsNoTracking().Where(x => x.leaveRegisterId == id).ToListAsync();
        }
        public async Task<Photograph> GetPhotoByEmpId(int id)
        {
            return await _context.photographs.Include(x => x.employee).Where(x => x.employeeId == id).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<EmployeeSignature> GetSignatureByEmpId(int id)
        {
            return await _context.employeeSignatures.Include(x => x.employee).Where(x => x.employeeId == id).AsNoTracking().FirstOrDefaultAsync();
        }


        public async Task<LeaveStatusLog> GetStatusLogByAppUserName(string appusername, int id, int regid)
        {
            var employeeId = _context.approvalMasters.Where(x => x.Id == id).AsNoTracking().Select(x => x.employeeInfoId).FirstOrDefault();
            var registerId = regid;
            var data = await _context.leaveStatusLogs.Where(x => x.createdBy == appusername && x.leaveRegisterId == registerId).AsNoTracking().FirstOrDefaultAsync();
            return data;
            //return await _context.leaveRegisters.Include(x => x.employee).Include(x => x.substitutionUser).Include(x => x.employee.department).Include(x => x.leaveType).AsNoTracking().Where(x => x.Id == id).FirstAsync();
        }
        public async Task<ApprovalDetail> CheckFinalByEmpIdAndRegId(int? empId, int? userid)
        {
            var appId = _context.Users.Where(x => x.userId == userid).Select(x => x.Id).FirstOrDefault();
            var emp = _context.employeeInfos.Where(x => x.ApplicationUserId == appId).Select(x => x.Id).FirstOrDefault();
            var masterId = _context.approvalMasters.Where(x => x.employeeInfoId == empId).Select(x => x.Id).FirstOrDefault();
            return await _context.approvalDetails.Where(x => x.approvalMasterId == masterId).Where(x => x.approverId == emp).FirstOrDefaultAsync();
        }
        public async Task<decimal> GetLeaveBalanceByempId(int empId, int leaveType)
        {
            DateTime dateTime = DateTime.Now;
            string year = dateTime.Year.ToString();
            decimal? half = 0;
            if (leaveType == 2)
            {
                half = await _context.leaveRegisters.Where(x => x.employeeId == empId && x.leaveStatus == 3 && x.leaveType.nameEn == "Half Day Leave").SumAsync(x => x.daysLeave);
                half = half / 2;
            }

            var openingBalance = await _context.leaveOpeningBalances.Where(x => x.employeeId == empId && x.leaveTypeId == leaveType && x.year.year == year).FirstOrDefaultAsync();
            if (openingBalance == null)
            {
                return 0;
            }
			decimal? leaveApprove = 0;
			if (leaveType == 1 || leaveType == 2)
			{
				leaveApprove = await _context.leaveRegisters.Where(x => x.employeeId == empId && x.leaveTo >= new DateTime(DateTime.Now.Year, 1, 1) && x.leaveStatus == 3 && x.leaveTypeId == leaveType).SumAsync(x => x.daysLeave);
			}
			else
			{
				leaveApprove = await _context.leaveRegisters.Where(x => x.employeeId == empId && x.leaveStatus == 3 && x.leaveTypeId == leaveType).SumAsync(x => x.daysLeave);
			}
			decimal leaveBalance = (decimal)(openingBalance.leaveDays - leaveApprove - half);
            return leaveBalance;
        }

        public async Task<int> GetLeaveBalanceByempIdYear(int empId, int leaveType, string Year)
        {
            DateTime dateTime = DateTime.Now;
            string year = Year;
            decimal? half = 0;
            if (leaveType == 2)
            {
                half = await _context.leaveRegisters.Where(x => x.employeeId == empId && x.leaveStatus == 3 && x.leaveType.nameEn == "Half Day Leave").SumAsync(x => x.daysLeave);
                half = half / 2;
            }

            var openingBalance = await _context.leaveOpeningBalances.Where(x => x.employeeId == empId && x.leaveTypeId == leaveType && x.year.year == year).FirstOrDefaultAsync();
            if (openingBalance == null)
            {
                return 0;
            }
            var leaveApprove = await _context.leaveRegisters.Where(x => x.employeeId == empId && x.leaveStatus == 3 && x.leaveTypeId == leaveType).SumAsync(x => x.daysLeave);
            int leaveBalance = (int)(openingBalance.leaveDays - leaveApprove - half);
            return leaveBalance;
        }

        public async Task<IEnumerable<LeaveReportModel>> GetLeaveReport(int year, int empId)
        {
            var types = await _context.leaveTypes.ToListAsync();
            List<LeaveReportModel> leaveReports = new List<LeaveReportModel>();
            int[] month = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            DateTime dateTime = new DateTime(year, 1, 1);

            var Sum = 0;

            foreach (var type in types)
            {
                var name = type.nameEn;
                var typeId = 0;
                if (type.nameEn == "Half Day Leave")
                {
                    typeId = 2;
                }
                else
                {
                    typeId = type.Id;
                }
                var openingBalance = await _context.leaveOpeningBalances.Where(x => x.employeeId == empId && x.leaveTypeId == typeId && x.year.year == year.ToString()).FirstOrDefaultAsync();

                int balance = 0;
                if (openingBalance != null)
                {
                    balance = (int)openingBalance.leaveDays;
                }
                if (balance > 0)
                {
                    for (int i = 0; i < month.Length; i++)
                    {
                        var leaveDay = await GetAllLeaveRegisterByEmpIdAndDateWithType(empId, dateTime, dateTime.AddMonths(1).AddDays(-1), type.Id);
                        month[i] = (int)leaveDay.daysLeave;
                        Sum = Sum + (int)leaveDay.daysLeave;
                        dateTime = dateTime.AddMonths(1);
                    }
                }

                var dueDay = balance - Sum;
                leaveReports.Add(new LeaveReportModel
                {
                    type = name,
                    allMonth = balance,
                    jan = month[0],
                    feb = month[1],
                    mar = month[2],
                    apr = month[3],
                    may = month[4],
                    jun = month[5],
                    jul = month[6],
                    aug = month[7],
                    sep = month[8],
                    oct = month[9],
                    nov = month[10],
                    dec = month[11],
                    total = Sum,
                    balance = dueDay
                });
                Sum = 0;
                Array.Clear(month, 0, month.Length);
                dateTime = new DateTime(year, 1, 1);
            }
            return leaveReports;
        }

        public async Task<bool> DeleteLeaveRegisterById(int id)
        {
            _context.leaveRegisters.Remove(_context.leaveRegisters.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<string> GetEmpNameByUserName(string username)
        {
            return await _context.employeeInfos.Where(x => x.ApplicationUser.UserName == username).Select(x => x.nameEnglish).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateLeaveApproval(int Id, int Type)
        {
            LeaveRegister data = await _context.leaveRegisters.FindAsync(Id);
            if (data != null)
            {
                data.leaveStatus = Type;
                _context.leaveRegisters.Update(data);
                return 1 == await _context.SaveChangesAsync();
            }
            return false;
        }

        public async Task<IEnumerable<LeaveSupervisorRecomViewModel>> GetSupervisorRecomForLeaveByEmpId(int leaveId, int empId)
        {
            var result = await (from l in _context.leaveRegisters
                                join s in _context.supervisors on l.employeeId equals s.employeeID
                                join e in _context.employeeInfos on s.supervisorId equals e.Id
                                where l.employeeId == empId && l.Id == leaveId
                                select new LeaveSupervisorRecomViewModel
                                {
                                    supervisorName = e.nameEnglish,
                                    supervisorDesignation = e.designation,
                                    leaveStatus = l.leaveStatus,
                                    date = s.supervisorDate
                                }).AsNoTracking().ToListAsync();
            return result;
        }

        public async Task<DayLeaveNotMapped> GetTotalLeaveDaysBetweenDate(DateTime? frmDate, DateTime? toDate, int leaveType, int ShiftGrpID)
        {
            try
            {
                var data = await _context.dayLeaveNotMappeds.FromSql(@"spGetTotalLeaveDaysBetweenDateByEmp @p0,@p1,@p2, @p3", frmDate?.ToString("yyyyMMdd"), toDate?.ToString("yyyyMMdd"), leaveType, ShiftGrpID).AsNoTracking().FirstOrDefaultAsync();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<decimal> GetTotalAvailedLeaveByYear(int id, int year)
        {
            var employee = await _context.leaveRegisters.Where(x => x.Id == id).Select(x => x.employee).FirstOrDefaultAsync();
            var data = await _context.leaveRegisters.Where(x => x.employeeId == employee.Id && x.leaveStatus == 3 && (Convert.ToDateTime(x.leaveFrom).Year == year) && (Convert.ToDateTime(x.leaveTo).Year == year)).SumAsync(x => x.daysLeave);
            return Convert.ToDecimal(data);
        }
        public async Task<decimal> GetTotalAvailedLeaveByDate(int id, DateTime? date)
        {
            var employee = await _context.leaveRegisters.Where(x => x.Id == id).Select(x => x.employee).FirstOrDefaultAsync();
            var data = await _context.leaveRegisters.Where(x => x.employeeId == employee.Id && x.leaveStatus == 3 && (Convert.ToDateTime(x.leaveFrom).Year == Convert.ToDateTime(date).Year) && Convert.ToDateTime(x.leaveFrom).Date < Convert.ToDateTime(date).Date).SumAsync(x => x.daysLeave);
            return Convert.ToDecimal(data);
        }
        public async Task<decimal> GetTotalAvailedLeaveByLeaveDate(int? id , DateTime? ToLeave)
        {
            try
            {
                var employee = await _context.leaveRegisters.Where(x => x.Id == id).Select(x => x.employee).FirstOrDefaultAsync();
                var data = await _context.leaveRegisters.Where(x => x.employeeId == employee.Id && x.leaveStatus == 3 && (Convert.ToDateTime(x.leaveTo).Date <= Convert.ToDateTime(ToLeave).Date)).SumAsync(x => x.daysLeave);
                return Convert.ToDecimal(data);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        public async Task<decimal> GetTotalYearAvailedLeaveByLeaveDate(int? id, DateTime? ToLeave)
        {
            var employee = await _context.leaveRegisters.Where(x => x.Id == id).Select(x => x.employee).FirstOrDefaultAsync();
            var data = await _context.leaveRegisters.Where(x => x.employeeId == employee.Id && x.leaveStatus == 3 && (Convert.ToDateTime(x.leaveFrom).Year == Convert.ToDateTime(ToLeave).Year) && (Convert.ToDateTime(x.leaveFrom).Date < Convert.ToDateTime(ToLeave).Date)).SumAsync(x => x.daysLeave);
            return Convert.ToDecimal(data);
        }

        public async Task<decimal> GetOpeningBalanceByRegId(int id, int year)
        {
            var yearId = await _context.years.Where(x => x.year == year.ToString()).AsNoTracking().Select(x => x.Id).FirstOrDefaultAsync();
            var reg = await _context.leaveRegisters.Include(x => x.leaveType).Include(x => x.employee).Where(x => x.Id == id).FirstOrDefaultAsync();
            var openingBalance = await _context.leaveOpeningBalances.Where(x => x.employeeId == reg.employeeId && x.yearId == yearId && x.leaveTypeId == reg.leaveTypeId).Select(x => x.leaveDays).FirstOrDefaultAsync();
            return Convert.ToDecimal(openingBalance);
        }

        public async Task<IEnumerable<LeaveBalanceViewModel>> GetLeaveBalanceSummaryByEmpId(int empId)
        {
            return await _context.leaveBalanceViewModels.FromSql(@"spLoadIndividualLeaveSummary @p0", empId).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<LeaveIndividualViewModel>> GetIndividualLeaveReport(int empId, string fdate, string tdate)
        {
            return await _context.leaveIndividualViewModels.FromSql($"spGetLeaveDetailsNew {empId},{Convert.ToDateTime(fdate).ToString("yyyyMMdd")},{Convert.ToDateTime(tdate).ToString("yyyyMMdd")}").AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<LeaveRegister>> GetAllApproveLeaveByUserIdStatus(int UserId, int status)
        {
            var data = await (
                                     from u in _context.Users
                                     join ei in _context.employeeInfos
                                     on u.Id equals ei.ApplicationUserId
                                     join ls in _context.leaveStatusLogs
                                     on ei.employeeCode equals ls.createdBy
                                     join lr in _context.leaveRegisters
                                     on ls.leaveRegisterId equals lr.Id
                                     where u.userId == UserId && ls.Status == status
                                     select lr).Include(x => x.employee).Include(x => x.leaveType).AsNoTracking().ToListAsync();


            return data;
        }
        
        public async Task<IEnumerable<LeaveRegister>> GetAllPendingLeaveBySupervisorUserId(int UserId)
        {
            var data = await (
                            from u in _context.Users
                            join ei in _context.employeeInfos
                            on u.Id equals ei.ApplicationUserId
                            join ls in _context.leaveRoutes
                            on ei.Id equals ls.employeeId
                            join lr in _context.leaveRegisters
                            on ls.leaveRegisterId equals lr.Id
                            where u.userId == UserId && lr.leaveStatus == 1
                            select lr).Include(x => x.employee).Include(x => x.leaveType).AsNoTracking().ToListAsync();


            return data;
        }

        public async Task<IEnumerable<LeaveRegister>> GetLeaveRequesterByApproverId(int UserId)
        {

            var data = await (
                                     from u in _context.Users
                                     join ei in _context.employeeInfos
                                     on u.Id equals ei.ApplicationUserId
                                     join ls in _context.leaveStatusLogs
                                     on ei.employeeCode equals ls.createdBy
                                     join lr in _context.leaveRegisters
                                     on ls.leaveRegisterId equals lr.Id
                                     where u.userId == UserId //&& ls.Status == status
                                     select lr).Include(x => x.employee).Include(x=> x.employee.lastDesignation).Include(x => x.leaveType).AsNoTracking().ToListAsync();

            //var data = await (from lrt in _context.leaveRoutes                       
            //                  join lr in _context.leaveRegisters
            //                  on lrt.leaveRegisterId equals lr.Id
            //                  where lrt.employeeId == UserId
            //                  select lr).Include(x => x.employee).Include(x => x.employee.lastDesignation).Include(x => x.leaveType).AsNoTracking().ToListAsync();

            return data;
        }

        public async Task<IEnumerable<LeaveRegister>> GetAllApproveLeaveByUserId(int UserId)
        {
            var data = await _context.leaveRegisters.Include(x => x.employee).Include(x =>  x.employee.lastDesignation).Include(x => x.leaveType).AsNoTracking().ToListAsync();
              return data;
        }
        public async Task<IEnumerable<YearlyLeaveProcess>> GetAllYearlyLeaveProcessByEmpIdYearType(int id, int yearId, int typeId)
        {
            return await _context.yearlyLeaveProcesses.Where(x => x.employeeId == id).Where(x => x.yearId == yearId).Where(x => x.leaveTypeId == typeId).Include(x => x.employee).Include(x => x.leaveType).AsNoTracking().ToListAsync();
        }

        public async Task<string> GetYearById(int id)
        {
            var year = await _context.years.Where(x => x.Id == id).AsNoTracking().Select(x => x.year).FirstOrDefaultAsync();
            return year;
        }

        //public async Task<IEnumerable<LeaveOpeningBalance>> GetOpeningBalanceByYearId1(int id)
        //{
        //    var opBalances = await _context.leaveOpeningBalances.Where(x => x.yearId == id).AsNoTracking().ToListAsync();

        //    return opBalances;
        //}
        public async Task<int> GetOpeningBalanceByYearId(int id)
        {

            //return await _context.leaveOpeningBalances.FromSql($" {id}").AsNoTracking().ToListAsync();



            _context.Database.ExecuteSqlCommand($"sp_processYearlyLeave {id}");
            return 1;



        }
        //public async Task<IEnumerable<YearlyLeaveProcessViewModel>> GetOpeningBalanceByYearId(int id)
        //{

        //        return  await _context.yearlyLeaveProcessViewModels.FromSql($"sp_processYearlyLeave {id}").AsNoTracking().ToListAsync(); 
        //}


        public async Task<IEnumerable<Year>> GetAllYear()
        {
            return await _context.years.AsNoTracking().ToListAsync();


        }

        public async Task<int> DeleteYearlyLeaveProcessByYearId(int yearid)
        {
            var opBalances = await _context.yearlyLeaveProcesses.Where(x => x.yearId == yearid).AsNoTracking().ToListAsync();
            _context.yearlyLeaveProcesses.RemoveRange(opBalances);
            await _context.SaveChangesAsync();
            return 1;
        }

        public async Task<decimal> GetLeaveAvailedByTypeEmpIdAndYear(int type, int empId, int year)
        {
            var data = await _context.leaveRegisters.AsNoTracking().Where(x => x.leaveTypeId == type && x.employeeId == empId && Convert.ToDateTime(x.updatedAt).Year == year && x.leaveStatus == 3).GroupBy(x => x.daysLeave).Select(x => x.Sum(y => y.daysLeave)).FirstOrDefaultAsync();
            return Convert.ToDecimal(data);
        }


        public async Task<int> SaveYearlyLeaveProcess(YearlyLeaveProcess model)
        {
            if (model.Id != 0)
            {
                _context.yearlyLeaveProcesses.Update(model);
            }
            else
            {
                _context.yearlyLeaveProcesses.Add(model);
            }
            await _context.SaveChangesAsync();
            return model.Id;
        }



        public async Task<IEnumerable<YearlyLeaveProcess>> GetAllLeaveProcess()
        {
            return await _context.yearlyLeaveProcesses.Include(x => x.employee).Include(x => x.leaveType).Include(x => x.employee.lastDesignation).Include(x => x.year).AsNoTracking().ToListAsync();
        }

		public async Task<IEnumerable<EmployeeLeaveViewModel>> GetEmployeeLeaveByAnyKey(string employeeCode, int? leaveTypeId, int? approverId,  string fromDate, string toDate)
		{
            var data = new List<EmployeeLeaveViewModel>();

            try
            {
                data = await _context.EmployeeLeaveViewModels.FromSql($"SP_GetLeaveByAnyKey {employeeCode},{leaveTypeId},{approverId}, {fromDate}, {toDate}").AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {

            }

            return data;


            //Old
			//return await _context.EmployeeLeaveViewModels.FromSql($"SP_GetLeaveByAnyKey {empId},{Convert.ToDateTime(fdate).ToString("yyyyMMdd")},{Convert.ToDateTime(tdate).ToString("yyyyMMdd")}").AsNoTracking().ToListAsync();
		}

		#region Late Attendance Report
		//public async Task<IEnumerable<MonthlyAttendanceVm>> GetMonthlyAttendance(int year, int month, int deptId, int branchId, int zoneId)
		//{
		//    var data = await _context.monthlyAttendanceVms.FromSql($"sp_GetMonthlyAttendanceSp {year}, {month}, {deptId}, {branchId}, {zoneId}").ToListAsync();
		//    return data;
		//}

		public async Task<IEnumerable<LateAttandenceDataVM>> GetLateAttendanceReports(string userName)
        {
            try
            {
                var data = await _context.lateAttandenceDataVMs.FromSql($"sp_GetAttendanceForRepotLateEarly {userName}").ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        public async Task<IEnumerable<LateAttandenceDataVM>> MyEarlyLeaveAttendance(string userName)
        {
            try
            {
                var data = await _context.lateAttandenceDataVMs.FromSql($"sp_GetMyEarlyLeaveAttendance {userName}").ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
    }
}
