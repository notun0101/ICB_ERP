using Microsoft.EntityFrameworkCore;
using OPUSERP.Accounting.Data.Entity.AccountingSettings;
using OPUSERP.Accounting.Data.Entity.Voucher;
using OPUSERP.Accounting.Services.AccountingSettings.Interfaces;
using OPUSERP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Accounting.Services.AccountingSettings
{
    public class OpeningBalanceService : IOpeningBalanceService
    {
        private readonly ERPDbContext _context;

        public OpeningBalanceService(ERPDbContext context)
        {
            _context = context;
        }
        #region Opening Balance
        public async Task<int> SaveopeningBalance(OpeningBalance openingBalance)
        {
            try
            {
                if (openingBalance.Id != 0)
                {
                    _context.OpeningBalances.Update(openingBalance);
                }
                else
                {
                    _context.OpeningBalances.Add(openingBalance);
                }

                await _context.SaveChangesAsync();
                return openingBalance.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IEnumerable<OpeningBalance>> GetOpeningBalance()
        {
            return await _context.OpeningBalances.Include(x=>x.ledgerRelation.ledger).Include(x => x.ledgerRelation.subLedger).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<OpeningBalance>> GetOpeningBalanceBranchUnitWise(int branchUnitId)
        {
            return await _context.OpeningBalances.Include(x => x.ledgerRelation.ledger).Include(x => x.ledgerRelation.subLedger).Where(x=>x.branchUnitId== branchUnitId).AsNoTracking().ToListAsync();
        }
           public async Task<IEnumerable<VoucherDetail>> GetOpeningBalancebyVoucherNo(int branchUnitId)
        {
            return await _context.VoucherDetails.Include(x => x.voucher).Include(x => x.transectionMode).Include(e=>e.ledgerRelation.ledger).Include(e=>e.ledgerRelation.subLedger).Where(x=>x.specialBranchUnitId== branchUnitId).Where(e=>e.voucher.voucherNo == "Opening Balance").AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<OpeningBalance>> GetOpeningBalancebyLedgerRelId(int id)
        {
            return await _context.OpeningBalances.Where(x=>x.ledgerRelationId==id).AsNoTracking().ToListAsync();
        }



        public async Task<OpeningBalance> GetopeningBalanceById(int id)
        {
            try
            {
                var record = await _context.OpeningBalances.FindAsync(id);
                return record;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public async Task<bool> DeleteopeningBalanceById(int id)
        {
            var data = _context.OpeningBalances.Find(id);
            _context.OpeningBalances.Remove(_context.OpeningBalances.Where(x => x.Id ==data.isDelete).FirstOrDefault());
            _context.OpeningBalances.Remove(_context.OpeningBalances.Find(id));

            return 1 == await _context.SaveChangesAsync();
        }

       public async Task<bool> DeleteopeningBalanceByVoucherMasterId(int id)
        {
            _context.VoucherDetails.RemoveRange(_context.VoucherDetails.Where(x => x.voucherId ==id).ToList());
            _context.VoucherMasters.Remove(_context.VoucherMasters.Find(id));

            return 1 == await _context.SaveChangesAsync();
        }

          public async Task<bool> DeleteopeningOpeningBalanceDetailsByVoucherMasterId(int id)
        {
            _context.VoucherDetails.RemoveRange(_context.VoucherDetails.Where(x => x.voucherId ==id).ToList());

            return 1 == await _context.SaveChangesAsync();
        }

       

        #endregion
    }
}
