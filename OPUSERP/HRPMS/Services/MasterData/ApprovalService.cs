﻿using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.MasterData.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.MasterData
{
    public class ApprovalService : IApprovalService
    {
        private readonly ERPDbContext _context;

        public ApprovalService(ERPDbContext context)
        {
            _context = context;
        }
        #region ApprovalType
        public async Task<bool> SaveApprovalType(ApprovalType approvalType)
        {
            if (approvalType.Id != 0)
                _context.approvalTypes.Update(approvalType);
            else
                _context.approvalTypes.Add(approvalType);

            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ApprovalType>> GetAllApprovalType()
        {
            return await _context.approvalTypes.AsNoTracking().ToListAsync();
        }       
        public async Task<ApprovalType> GetApprovalTypeById(int id)
        {
            return await _context.approvalTypes.FindAsync(id);
        }

        public async Task<bool> DeleteApprovalTypeById(int id)
        {
            _context.approvalTypes.Remove(_context.approvalTypes.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
        #endregion

        #region ApprovalMaster
        public async Task<int> SaveApprovalMaster(ApprovalMaster approvalMaster)
        {
            try
            {
                if (approvalMaster.Id != 0)
                {
                    approvalMaster.updatedBy = approvalMaster.createdBy;
                    approvalMaster.updatedAt = DateTime.Now;
                    _context.approvalMasters.Update(approvalMaster);
                }
                else
                {
                    approvalMaster.createdAt = DateTime.Now;
                    _context.approvalMasters.Add(approvalMaster);
                }

                await _context.SaveChangesAsync();
                return approvalMaster.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public async Task<IEnumerable<ApprovalMaster>> GetAllApprovalMaster()
        {
            return await _context.approvalMasters.Include(x => x.employeeInfo).Include(x=>x.employeeInfo.ApplicationUser).Include(x => x.approvalType).OrderByDescending(x => x.Id).AsNoTracking().ToListAsync();
        }
		public async Task<IEnumerable<ApprovalMaster>> GetApprovalMaster(int id)
        {
            return await _context.approvalMasters.Include(x => x.employeeInfo).Include(x => x.approvalType).Where(x => x.employeeInfoId == id).OrderByDescending(x => x.Id).AsNoTracking().ToListAsync();
        }
		public async Task<EmployeeInfo> GetEmployeeInfoById(int id)
        {
            return await _context.employeeInfos.Include(x => x.department).Include(x => x.ApplicationUser).Include(x =>x.lastDesignation).Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }
		public async Task<ApprovalMaster> GetApprovalMasterByEmpId(int id)
        {
            return await _context.approvalMasters.Include(x => x.employeeInfo).Where(x => x.employeeInfoId == id).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<ApprovalMaster> GetApprovalMasterByApprovalMasterId(int ApprovalMasterId)
        {
            return await _context.approvalMasters.Include(x => x.employeeInfo).Include(x => x.approvalType).Where(x => x.Id == ApprovalMasterId).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<ApprovalMaster> GetApprovalMasterByApprovalMasterIdWithImage(int ApprovalMasterId)
        {
            var data = await _context.approvalMasters.Include(x => x.employeeInfo).Include(x => x.approvalType).Where(x => x.Id == ApprovalMasterId).AsNoTracking().FirstOrDefaultAsync();
            data.updatedBy = await _context.photographs.Where(x => x.employeeId == data.employeeInfoId).Select(x => x.url).FirstOrDefaultAsync();
            return data;
        }
        public async Task<ApprovalMaster> GetApprovalMasterById(int id)
        {
            return await _context.approvalMasters.FindAsync(id);
        }

        public async Task<bool> DeleteApprovalMasterById(int id)
        {
            _context.approvalMasters.Remove(_context.approvalMasters.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
		public int DeleteApprovalMasterForcelyById(int id)
		{
			_context.Database.ExecuteSqlCommand($"sp_DeleteApprovalMasterForcelyById {id}");
			return 1;
		}
		#endregion

		#region Approval Detail
		public async Task<bool> SaveApprovalDetail(ApprovalDetail approvalDetail)
        {
            if (approvalDetail.Id != 0)
                _context.approvalDetails.Update(approvalDetail);
            else
                _context.approvalDetails.Add(approvalDetail);

            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ApprovalDetail>> GetAllApprovalDetail()
        {
            return await _context.approvalDetails.Include(x => x.approvalMaster).Include(x => x.approver).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<ApprovalDetail>> GetApprovalDetailByApprovalMasterId(int ApprovalMasterId)
        {
            return await _context.approvalDetails.Include(x => x.approvalMaster).Include(x => x.approver.department).Include(x=>x.approver.lastDesignation).Include(x => x.approver).Where(x => x.approvalMasterId == ApprovalMasterId).AsNoTracking().ToListAsync();
        }
        public async Task<string> GetLeaveApprovalRaiserImage(int employeeId)
        {
            return await _context.photographs.Where(x => x.employeeId == employeeId).Select(x=>x.url).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<ApprovalDetail>> GetApprovalDetailByApprovalMasterIdWithImage(int ApprovalMasterId)
        {
            var data=await _context.approvalDetails.Include(x => x.approvalMaster).Include(x => x.approver.department).Include(x => x.approver.lastDesignation).Include(x => x.approver).Where(x => x.approvalMasterId == ApprovalMasterId).AsNoTracking().ToListAsync();
            var list = new List<ApprovalDetail>();
            foreach (var item in data)
            {
                list.Add(new ApprovalDetail
                {
                    isDelete = item.isDelete,
                    approvalMaster = item.approvalMaster,
                    approver = item.approver,
                    sortOrder = item.sortOrder,
                    status = item.status,
                    updatedBy = await _context.photographs.Where(x => x.employeeId == item.approverId).Select(x => x.url).FirstOrDefaultAsync()
                });
            }
            return list;
        }
        public async Task<IEnumerable<ApprovalDetail>> GetApprovalDetailByEmpAndType(int empId, string type)
        {
            return await _context.approvalDetails.Include(x => x.approvalMaster.approvalType).Include(x => x.approver).Where(x => x.approvalMaster.employeeInfoId == empId && x.approvalMaster.approvalType.approvalTypeName == type).OrderBy(x=>x.sortOrder).AsNoTracking().ToListAsync();
        }

        public async Task<ApprovalDetail> GetSingleApprovalDetailByEmpAndType(int empId, string type)
        {
            return await _context.approvalDetails.Include(x => x.approvalMaster.approvalType).Include(x => x.approver).Where(x => x.approvalMaster.employeeInfoId == empId && x.approvalMaster.approvalType.approvalTypeName == type).OrderBy(x => x.sortOrder).AsNoTracking().FirstOrDefaultAsync();
        }


        public async Task<ApprovalDetail> GetapprovalDetailsById(int id)
        {
            return await _context.approvalDetails.FindAsync(id);
        }

        public async Task<bool> DeleteapprovalDetailsById(int id)
        {
            _context.approvalDetails.Remove(_context.approvalDetails.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
        public async Task<bool> DeleteapprovalDetailsByApprovalMasterId(int ApprovalMasterId)
        {
            _context.approvalDetails.RemoveRange(_context.approvalDetails.Where(x => x.approvalMasterId == ApprovalMasterId));
            return 1 == await _context.SaveChangesAsync();
        }
        #endregion

    }
}
