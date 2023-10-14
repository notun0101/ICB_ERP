using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.ProvidentFund.Data.Entity;
using OPUSERP.ProvidentFund.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.ProvidentFund.Service
{
    public class PFLoanDisbursementService : IPFLoanDisbursementService
    {
        private readonly ERPDbContext _context;
        public PFLoanDisbursementService(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<PFLoanDisbursement> GetLoanDisbursementByEmployeeId(int employeeId)
        {
            return await _context.PFLoanDisbursements.Where(x => x.EmployeeInfoId == employeeId).AsNoTracking().SingleOrDefaultAsync();
        }

        public async Task<bool> SaveLoanDisbursement(PFLoanDisbursement disbursement)
        {
            if (disbursement.Id != 0)
                _context.PFLoanDisbursements.Update(disbursement);
            else
                _context.PFLoanDisbursements.Add(disbursement);

            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteLoanDisbursement(int id)
        {
            _context.PFLoanDisbursements.Remove(_context.PFLoanDisbursements.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PFLoanDisbursement>> GetAllApproveLoanDisbursement()
        {
            return await _context.PFLoanDisbursements.Where(x => x.approveStatus == 2).AsNoTracking().ToListAsync();

        }

        public async Task<IEnumerable<PFLoanDisbursement>> GetAllPendingLoanDisbursement()
        {
            return await _context.PFLoanDisbursements.Include(x => x.pfmember).Where(x => x.approveStatus == 0).AsNoTracking().ToListAsync();

        }

        public async Task<IEnumerable<PFLoanDisbursement>> GetAllLoanDisbursement()
        {
            return await _context.PFLoanDisbursements.Include(x => x.pfmember).ToListAsync();
        }

        public async Task<PFLoanDisbursement> GetLoanDisbursementId(int id)
        {
            return await _context.PFLoanDisbursements.FindAsync(id);
        }

        public async Task<bool> UpdateLoanDisbursementStatus(int? id, int? status, string updateBy)
        {
            var disbursement = _context.PFLoanDisbursements.Find(id);
            disbursement.approveStatus = status;
            disbursement.isActive = 1;
            disbursement.updatedBy = updateBy;
            disbursement.updatedAt = DateTime.Now;

            _context.Entry(disbursement).State = EntityState.Modified;

            return 1 == await _context.SaveChangesAsync();
        }
    }

}
