using Microsoft.EntityFrameworkCore;
using OPUSERP.Accounting.Data.Entity.BankReconciliation;
using OPUSERP.Accounting.Data.Entity.Voucher;
using OPUSERP.Accounting.Services.Voucher.Interfaces;
using OPUSERP.Areas.Accounting.Models;
using OPUSERP.Data;
using OPUSERP.Models.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Accounting.Services.Voucher
{
    public class VoucherService : IVoucherService
    {
        private readonly ERPDbContext _context;

        public VoucherService(ERPDbContext context)
        {
            _context = context;
        }
        #region Bank Reconcilation
        public async Task<int> SaveBankReconcilation(BankReconcileMaster bankReconcileMaster)
        {
            try
            {
                if (bankReconcileMaster.Id != 0)
                {
                    _context.BankReconcileMasters.Update(bankReconcileMaster);
                }
                else
                {
                    _context.BankReconcileMasters.Add(bankReconcileMaster);
                }

                await _context.SaveChangesAsync();
                return bankReconcileMaster.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<IEnumerable<BankReconcileMaster>> GetAllBankReconcileMaster()
        {
            return await _context.BankReconcileMasters.Include(x => x.ledger).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<BankReconcileMaster>> GetAllBankReconcileMasters(int? sbuId)
        {
            return await _context.BankReconcileMasters.Include(x => x.ledger).Where(x => x.sbuId == sbuId).AsNoTracking().ToListAsync();
        }


        public async Task<BankReconcileMaster> GetBankReconcileMasterById(int id)
        {
            try
            {
                return await _context.BankReconcileMasters.Where(x => x.Id == id).Include(x => x.ledger).AsNoTracking().FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }


        public async Task<int> SaveBankReconcilationDetail(BankReconcileDetail bankReconcileDetail)
        {
            try
            {
                if (bankReconcileDetail.Id != 0)
                {
                    _context.BankReconcileDetails.Update(bankReconcileDetail);
                }
                else
                {
                    _context.BankReconcileDetails.Add(bankReconcileDetail);
                }

                await _context.SaveChangesAsync();
                return bankReconcileDetail.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IEnumerable<BankReconcileDetail>> GetBankReconcileDetailByMasterId(int MasterId)
        {


            return await _context.BankReconcileDetails
                .Where(x => x.bankReconcileMasterId == MasterId).Include(x => x.voucherDetail).Include(x => x.voucherMaster)
                .OrderBy(X => X.Id)
                .AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<BankReconcileDetail>> GetBankReconcileDetail()
        {
            return await _context.BankReconcileDetails.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<BankReconcileDetail>> GetUnCheckBankReconcileDetail()
        {
            return await _context.BankReconcileDetails.Where(x => x.isChecked == 0).AsNoTracking().ToListAsync();
        }

        public async Task<bool> DeleteBankReconcileDetailsByMasterId(int id)
        {
            _context.BankReconcileDetails.RemoveRange(_context.BankReconcileDetails.Where(x => x.bankReconcileMasterId == id));
            return 1 == await _context.SaveChangesAsync();
        }

        #endregion

        #region Voucher Master 
        public async Task<int> SavevoucherMaster(VoucherMaster voucherMaster)
        {
            try
            {
                if (voucherMaster.Id != 0)
                {
                    _context.VoucherMasters.Update(voucherMaster);
                }
                else
                {
                    _context.VoucherMasters.Add(voucherMaster);
                }

                await _context.SaveChangesAsync();
                return voucherMaster.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IEnumerable<VoucherMaster>> GetvoucherMaster()
        {
            return await _context.VoucherMasters.Include(x => x.fiscalYear).Include(X => X.fundSource).Include(X => X.voucherType).Include(x => x.taxYear).AsNoTracking().ToListAsync();
        }

        public IQueryable<VoucherMaster> GetvoucherMasterByTypeId(int TypeId)
        {
            return _context.VoucherMasters.Where(x => x.voucherTypeId == TypeId).Include(x => x.fiscalYear).Include(X => X.fundSource).Include(X => X.voucherType).Include(x => x.taxYear);
        }

        public async Task<IEnumerable<VoucherMaster>> GetvoucherMasterByFileNo(string fileNo)
        {
            return  await _context.VoucherMasters.Where(x => x.fileNo == fileNo).ToListAsync();
        }


        public async Task<IEnumerable<VoucherMaster>> GetvoucherMasterDetailsByTypeIdPIUwise(int TypeId)
        {
            try
            {
                List<VoucherMaster> record = new List<VoucherMaster>();
                if (TypeId == 0)
                {
                    record = await (from vm in _context.VoucherMasters
                                    join vd in _context.VoucherDetails.Include(x => x.specialBranchUnit).Where(x => x.specialBranchUnitId != 1) on vm.Id equals vd.voucherId
                                    join f in _context.salaryYears on vm.fiscalYearId equals f.Id
                                    join fs in _context.FundSources on vm.fundSourceId equals fs.Id
                                    join vt in _context.VoucherTypes on vm.voucherTypeId equals vt.Id
                                    join ty in _context.taxYears on vm.taxYearId equals ty.Id
                                    join vp in _context.VoucherApproveLogs.Where(x => x.statusId == 2) on vm.Id equals vp.voucherMasterId
                                    where vd.isPrincAcc == 1
                                    select new VoucherMaster
                                    {
                                        Id = vm.Id,
                                        voucherNo = vm.voucherNo,
                                        branchName = vd.specialBranchUnit.branchUnitName,
                                        voucherDate = vm.voucherDate,
                                        voucherAmount = vm.voucherAmount,
                                        voucherTypeId = vm.voucherTypeId,
                                        remarks = vm.remarks,
                                        isPosted = vm.isPosted,
                                        fiscalYearId = vm.fiscalYearId,
                                        taxYearId = vm.taxYearId,
                                        companyId = vm.companyId,
                                        fundSourceId = vm.fundSourceId,
                                        ledgerRelationId = vd.ledgerRelationId,
                                        accountName = vd.accountName,
                                        amount = vd.amount,
                                        refNo = vm.refNo,
                                        voucherType = vt,
                                        fiscalYear = f,
                                        taxYear = ty,
                                        fundSource = fs
                                    }).OrderByDescending(a => a.Id).ToListAsync();
                }
                else
                {
                    record = await (from vm in _context.VoucherMasters
                                    join vd in _context.VoucherDetails.Include(x => x.specialBranchUnit).Where(x => x.specialBranchUnitId != 1) on vm.Id equals vd.voucherId
                                    join f in _context.salaryYears on vm.fiscalYearId equals f.Id
                                    join fs in _context.FundSources on vm.fundSourceId equals fs.Id
                                    join vt in _context.VoucherTypes on vm.voucherTypeId equals vt.Id
                                    join ty in _context.taxYears on vm.taxYearId equals ty.Id
                                    join vp in _context.VoucherApproveLogs.Where(x => x.statusId == 2) on vm.Id equals vp.voucherMasterId
                                    where vm.voucherTypeId == TypeId && vd.isPrincAcc == 1
                                    select new VoucherMaster
                                    {
                                        Id = vm.Id,
                                        voucherNo = vm.voucherNo,
                                        branchName = vd.specialBranchUnit.branchUnitName,
                                        voucherDate = vm.voucherDate,
                                        voucherAmount = vm.voucherAmount,
                                        voucherTypeId = vm.voucherTypeId,
                                        remarks = vm.remarks,
                                        isPosted = vm.isPosted,
                                        fiscalYearId = vm.fiscalYearId,
                                        taxYearId = vm.taxYearId,
                                        companyId = vm.companyId,
                                        fundSourceId = vm.fundSourceId,
                                        ledgerRelationId = vd.ledgerRelationId,
                                        accountName = vd.accountName,
                                        amount = vd.amount,
                                        refNo = vm.refNo,
                                        voucherType = vt,
                                        fiscalYear = f,
                                        taxYear = ty,
                                        fundSource = fs
                                    }).OrderByDescending(a => a.Id).ToListAsync();
                }



                return record;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public async Task<IEnumerable<VoucherMaster>> GetvoucherMasterDetailsByTypeId(int TypeId)
        {
            try
            {
                List<VoucherMaster> record = new List<VoucherMaster>();
                if (TypeId == 0)
                {
                    record = await (from vm in _context.VoucherMasters
                                    join vd in _context.VoucherDetails on vm.Id equals vd.voucherId
                                    join f in _context.salaryYears on vm.fiscalYearId equals f.Id
                                    join fs in _context.FundSources on vm.fundSourceId equals fs.Id
                                    join vt in _context.VoucherTypes on vm.voucherTypeId equals vt.Id
                                    join ty in _context.taxYears on vm.taxYearId equals ty.Id
                                    where vd.isPrincAcc == 1
                                    select new VoucherMaster
                                    {
                                        Id = vm.Id,
                                        voucherNo = vm.voucherNo,
                                        voucherDate = vm.voucherDate,
                                        voucherAmount = vm.voucherAmount,
                                        voucherTypeId = vm.voucherTypeId,
                                        remarks = vm.remarks,
                                        isPosted = vm.isPosted,
                                        fiscalYearId = vm.fiscalYearId,
                                        taxYearId = vm.taxYearId,
                                        companyId = vm.companyId,
                                        fundSourceId = vm.fundSourceId,
                                        ledgerRelationId = vd.ledgerRelationId,
                                        accountName = vd.accountName,
                                        amount = vd.amount,
                                        refNo = vm.refNo,
                                        voucherType = vt,
                                        fiscalYear = f,
                                        taxYear = ty,
                                        fundSource = fs
                                    }).OrderByDescending(a => a.Id).ToListAsync();
                }
                else
                {
                    record = await (from vm in _context.VoucherMasters
                                    join vd in _context.VoucherDetails on vm.Id equals vd.voucherId
                                    join f in _context.salaryYears on vm.fiscalYearId equals f.Id
                                    join fs in _context.FundSources on vm.fundSourceId equals fs.Id
                                    join vt in _context.VoucherTypes on vm.voucherTypeId equals vt.Id
                                    join ty in _context.taxYears on vm.taxYearId equals ty.Id
                                    where vm.voucherTypeId == TypeId && vd.isPrincAcc == 1
                                    select new VoucherMaster
                                    {
                                        Id = vm.Id,
                                        voucherNo = vm.voucherNo,
                                        voucherDate = vm.voucherDate,
                                        voucherAmount = vm.voucherAmount,
                                        voucherTypeId = vm.voucherTypeId,
                                        remarks = vm.remarks,
                                        isPosted = vm.isPosted,
                                        fiscalYearId = vm.fiscalYearId,
                                        taxYearId = vm.taxYearId,
                                        companyId = vm.companyId,
                                        fundSourceId = vm.fundSourceId,
                                        ledgerRelationId = vd.ledgerRelationId,
                                        accountName = vd.accountName,
                                        amount = vd.amount,
                                        refNo = vm.refNo,
                                        voucherType = vt,
                                        fiscalYear = f,
                                        taxYear = ty,
                                        fundSource = fs
                                    }).OrderByDescending(a => a.Id).ToListAsync();
                }



                return record;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }


        public async Task<IEnumerable<VoucherMaster>> GetvoucherMasterDetailsBybranchUnitWise(int TypeId, int branchUnitId)
        {
            try
            {
                List<VoucherMaster> record = new List<VoucherMaster>();
                if (TypeId == 0)
                {
                    record = await (from vm in _context.VoucherMasters
                                    join vd in _context.VoucherDetails.Where(x => x.specialBranchUnitId == branchUnitId) on vm.Id equals vd.voucherId
                                    join f in _context.salaryYears on vm.fiscalYearId equals f.Id
                                    join fs in _context.FundSources on vm.fundSourceId equals fs.Id
                                    join vt in _context.VoucherTypes on vm.voucherTypeId equals vt.Id
                                    join ty in _context.taxYears on vm.taxYearId equals ty.Id
                                    where vd.isPrincAcc == 1
                                    select new VoucherMaster
                                    {
                                        Id = vm.Id,
                                        voucherNo = vm.voucherNo,
                                        voucherDate = vm.voucherDate,
                                        voucherAmount = vm.voucherAmount,
                                        voucherTypeId = vm.voucherTypeId,
                                        remarks = vm.remarks,
                                        isPosted = vm.isPosted,
                                        fiscalYearId = vm.fiscalYearId,
                                        taxYearId = vm.taxYearId,
                                        companyId = vm.companyId,
                                        fundSourceId = vm.fundSourceId,
                                        ledgerRelationId = vd.ledgerRelationId,
                                        accountName = vd.accountName,
                                        amount = vd.amount,
                                        refNo = vm.refNo,
                                        voucherType = vt,
                                        fiscalYear = f,
                                        taxYear = ty,
                                        fundSource = fs
                                    }).OrderByDescending(a => a.Id).ToListAsync();
                }
                else
                {
                    record = await (from vm in _context.VoucherMasters
                                    join vd in _context.VoucherDetails.Where(x => x.specialBranchUnitId == branchUnitId) on vm.Id equals vd.voucherId
                                    join f in _context.salaryYears on vm.fiscalYearId equals f.Id
                                    join fs in _context.FundSources on vm.fundSourceId equals fs.Id
                                    join vt in _context.VoucherTypes on vm.voucherTypeId equals vt.Id
                                    join ty in _context.taxYears on vm.taxYearId equals ty.Id
                                    where vm.voucherTypeId == TypeId && vd.isPrincAcc == 1
                                    select new VoucherMaster
                                    {
                                        Id = vm.Id,
                                        voucherNo = vm.voucherNo,
                                        voucherDate = vm.voucherDate,
                                        voucherAmount = vm.voucherAmount,
                                        voucherTypeId = vm.voucherTypeId,
                                        remarks = vm.remarks,
                                        isPosted = vm.isPosted,
                                        fiscalYearId = vm.fiscalYearId,
                                        taxYearId = vm.taxYearId,
                                        companyId = vm.companyId,
                                        fundSourceId = vm.fundSourceId,
                                        ledgerRelationId = vd.ledgerRelationId,
                                        accountName = vd.accountName,
                                        amount = vd.amount,
                                        refNo = vm.refNo,
                                        voucherType = vt,
                                        fiscalYear = f,
                                        taxYear = ty,
                                        fundSource = fs
                                    }).OrderByDescending(a => a.Id).ToListAsync();
                }



                return record;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public async Task<IEnumerable<VoucherMaster>> GetVoucherApprovedDetailsBybranchUnitWise(int TypeId, int branchUnitId)
        {
            try
            {
                List<VoucherMaster> record = new List<VoucherMaster>();
                if (TypeId == 0)
                {
                    record = await (from vm in _context.VoucherMasters
                                    join vd in _context.VoucherDetails.Where(x => x.specialBranchUnitId == branchUnitId) on vm.Id equals vd.voucherId
                                    join f in _context.salaryYears on vm.fiscalYearId equals f.Id
                                    join fs in _context.FundSources on vm.fundSourceId equals fs.Id
                                    join vt in _context.VoucherTypes on vm.voucherTypeId equals vt.Id
                                    join ty in _context.taxYears on vm.taxYearId equals ty.Id
                                    join vp in _context.VoucherApproveLogs.Where(x => x.statusId == 2) on vm.Id equals vp.voucherMasterId
                                    where vd.isPrincAcc == 1
                                    select new VoucherMaster
                                    {
                                        Id = vm.Id,
                                        voucherNo = vm.voucherNo,
                                        voucherDate = vm.voucherDate,
                                        voucherAmount = vm.voucherAmount,
                                        voucherTypeId = vm.voucherTypeId,
                                        remarks = vm.remarks,
                                        isPosted = vm.isPosted,
                                        fiscalYearId = vm.fiscalYearId,
                                        taxYearId = vm.taxYearId,
                                        companyId = vm.companyId,
                                        fundSourceId = vm.fundSourceId,
                                        ledgerRelationId = vd.ledgerRelationId,
                                        accountName = vd.accountName,
                                        amount = vd.amount,
                                        refNo = vm.refNo,
                                        voucherType = vt,
                                        fiscalYear = f,
                                        taxYear = ty,
                                        fundSource = fs
                                    }).OrderByDescending(a => a.Id).ToListAsync();
                }
                else
                {
                    record = await (from vm in _context.VoucherMasters
                                    join vd in _context.VoucherDetails.Where(x => x.specialBranchUnitId == branchUnitId) on vm.Id equals vd.voucherId
                                    join f in _context.salaryYears on vm.fiscalYearId equals f.Id
                                    join fs in _context.FundSources on vm.fundSourceId equals fs.Id
                                    join vt in _context.VoucherTypes on vm.voucherTypeId equals vt.Id
                                    join ty in _context.taxYears on vm.taxYearId equals ty.Id
                                    join vp in _context.VoucherApproveLogs.Where(x => x.statusId == 2) on vm.Id equals vp.voucherMasterId
                                    where vm.voucherTypeId == TypeId && vd.isPrincAcc == 1
                                    select new VoucherMaster
                                    {
                                        Id = vm.Id,
                                        voucherNo = vm.voucherNo,
                                        voucherDate = vm.voucherDate,
                                        voucherAmount = vm.voucherAmount,
                                        voucherTypeId = vm.voucherTypeId,
                                        remarks = vm.remarks,
                                        isPosted = vm.isPosted,
                                        fiscalYearId = vm.fiscalYearId,
                                        taxYearId = vm.taxYearId,
                                        companyId = vm.companyId,
                                        fundSourceId = vm.fundSourceId,
                                        ledgerRelationId = vd.ledgerRelationId,
                                        accountName = vd.accountName,
                                        amount = vd.amount,
                                        refNo = vm.refNo,
                                        voucherType = vt,
                                        fiscalYear = f,
                                        taxYear = ty,
                                        fundSource = fs
                                    }).OrderByDescending(a => a.Id).ToListAsync();
                }



                return record;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public async Task<VoucherMaster> GetGetvoucherMasterByJournalId(int id)
        {
            try
            {
                var record = await (from vm in _context.VoucherMasters
                                    join vd in _context.VoucherDetails on vm.Id equals vd.voucherId
                                    join f in _context.salaryYears on vm.fiscalYearId equals f.Id
                                    join fs in _context.FundSources on vm.fundSourceId equals fs.Id
                                    join vt in _context.VoucherTypes on vm.voucherTypeId equals vt.Id
                                    join ty in _context.taxYears on vm.taxYearId equals ty.Id
                                    where vm.Id == id
                                    select new VoucherMaster
                                    {
                                        Id = vm.Id,
                                        voucherNo = vm.voucherNo,
                                        voucherDate = vm.voucherDate,
                                        voucherAmount = vm.voucherAmount,
                                        voucherTypeId = vm.voucherTypeId,
                                        remarks = vm.remarks,
                                        isPosted = vm.isPosted,
                                        fiscalYearId = vm.fiscalYearId,
                                        taxYearId = vm.taxYearId,
                                        companyId = vm.companyId,
                                        fundSourceId = vm.fundSourceId,
                                        ledgerRelationId = vd.ledgerRelationId,
                                        accountName = vd.accountName,
                                        amount = vd.amount,
                                        refNo = vm.refNo,
                                        voucherType = vt,
                                        fiscalYear = f,
                                        taxYear = ty,
                                        fundSource = fs,
                                        projectId = vm.projectId
                                    }).FirstOrDefaultAsync();


                return record;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public async Task<VoucherMaster> GetGetvoucherMasterById(int id)
        {
            try
            {
                var record = await (from vm in _context.VoucherMasters
                                    join vd in _context.VoucherDetails on vm.Id equals vd.voucherId
                                    join f in _context.salaryYears on vm.fiscalYearId equals f.Id
                                    join fs in _context.FundSources on vm.fundSourceId equals fs.Id
                                    join vt in _context.VoucherTypes on vm.voucherTypeId equals vt.Id
                                    join ty in _context.taxYears on vm.taxYearId equals ty.Id
                                    where vm.Id == id && vd.isPrincAcc.GetValueOrDefault(1) == 1
                                    select new VoucherMaster
                                    {
                                        Id = vm.Id,
                                        voucherNo = vm.voucherNo,
                                        voucherDate = vm.voucherDate,
                                        voucherAmount = vm.voucherAmount,
                                        voucherTypeId = vm.voucherTypeId,
                                        remarks = vm.remarks,
                                        refNo = vm.refNo,
                                        isPosted = vm.isPosted,
                                        fiscalYearId = vm.fiscalYearId,
                                        taxYearId = vm.taxYearId,
                                        companyId = vm.companyId,
                                        fundSourceId = vm.fundSourceId,
                                        ledgerRelationId = vd.ledgerRelationId,
                                        accountName = vd.accountName,
                                        amount = vd.amount,
                                        chequeDate = vm.chequeDate,
                                        chequeNumber = vm.chequeNumber,
                                        voucherType = vt,
                                        fiscalYear = f,
                                        taxYear = ty,
                                        fundSource = fs,
                                        projectId = vm.projectId
                                    }).FirstOrDefaultAsync();


                return record;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }


        public async Task<bool> DeletevoucherMasterById(int id)
        {
            _context.VoucherMasters.Remove(_context.VoucherMasters.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateVoucherMasterStatus(int? id, int? isPosted, string updateBy)
        {
            var VoucherMasters = _context.VoucherMasters.Find(id);
            VoucherMasters.isPosted = isPosted;
            VoucherMasters.updatedBy = updateBy;
            VoucherMasters.updatedAt = DateTime.Now;
            _context.Entry(VoucherMasters).State = EntityState.Modified;
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateVoucherNo(int? id, string newVoucherNo)
        {
            var VoucherMasters = _context.VoucherMasters.Find(id);
            VoucherMasters.voucherNo = newVoucherNo;
            _context.Entry(VoucherMasters).State = EntityState.Modified;
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<VoucherMaster> GetVoucherMasterByRefNo(string refNo)
        {
            return await _context.VoucherMasters.Where(x => x.refNo == refNo).AsNoTracking().FirstOrDefaultAsync();
        }


        public async Task<int> SaveUplodedvoucherdata(UploadedVoucherData uploadedVoucherData)
        {
            try
            {
                if (uploadedVoucherData.Id != 0)
                {
                    _context.UploadedVoucherDatas.Update(uploadedVoucherData);
                }
                else
                {
                    _context.UploadedVoucherDatas.Add(uploadedVoucherData);
                }

                await _context.SaveChangesAsync();
                return uploadedVoucherData.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public async Task<IEnumerable<UploadedVoucherData>> AllUploadedVoucherData()
        {
            return await _context.UploadedVoucherDatas.Include(x => x.fiscalYear).Include(X => X.fundSource).Include(X => X.voucherType).Include(X => X.specialBranchUnit).Include(X => X.project).Include(X => X.ledgerRelation.ledger).Include(x => x.taxYear).AsNoTracking().ToListAsync();
        }

        public async Task<bool> DeleteUploadedVoucherDataByFileNo(string fileNo)
        {
            _context.UploadedVoucherDatas.RemoveRange(_context.UploadedVoucherDatas.Where(x => x.refNo == fileNo));
            return 1 == await _context.SaveChangesAsync();
        }

        public async Task<List<UploadedVoucherDataModel>> GetFileNoWiseData(string fileNo)
        {
            List<UploadedVoucherDataModel> datas = new List<UploadedVoucherDataModel>();
            var result = await _context.UploadedVoucherDatas.Include(x => x.fiscalYear).Include(X => X.fundSource).Include(X => X.voucherType).Include(X => X.specialBranchUnit).Include(X => X.project).Include(X => X.ledgerRelation.ledger).Include(x => x.taxYear).Include(x => x.transectionMode).Where(x => x.refNo == fileNo).AsNoTracking().ToListAsync();

            int i = 1;
            foreach (var data in result)
            {
                var vouchers = result.Where(x => x.voucherNo == data.voucherNo).ToList();
                decimal debitAmount = vouchers.Where(x => x.transectionModeId == 1).Sum(x => (x.amount == null) ? 0 : Convert.ToDecimal(x.amount));

                decimal ceditAmount = vouchers.Where(x => x.transectionModeId == 2).Sum(x => (x.amount == null) ? 0 : Convert.ToDecimal(x.amount));
                if (Math.Round(debitAmount) != Math.Round(ceditAmount))
                {
                    
                }

                UploadedVoucherDataModel model = new UploadedVoucherDataModel
                {
                    sl = i++,
                    refNo = data.refNo,
                    voucherNo = (data.voucherNo == null) ? "" : data.voucherNo,
                    voucherDate = data.voucherDate?.ToString("dd-MMM-yyyy"),
                    voucherType = (data.voucherType == null) ? "" : data.voucherType?.voucherTypeName,
                    description = (data.description == null) ? "" : data.description,
                    sbuName = (data.specialBranchUnit == null) ? "" : data.specialBranchUnit?.branchUnitName,
                    projectName = (data.project == null) ? "" : data.project?.projectName,
                    chequeNo = (data.chequeNo == null) ? "" : data.chequeNo,
                    chequeDate = (data.chequeDate==null)?"":data.chequeDate?.ToString("dd-MMM-yyyy"),
                    accountName = (data.ledgerRelation == null) ? "" : data.ledgerRelation?.ledger?.accountName + " (" + data.ledgerRelation?.ledger?.accountCode + ")",
                    transectionMode = (data.transectionMode == null) ? "" : data.transectionMode?.modeName,
                    amount = (data.amount == null) ? "" : data.amount?.ToString("0,0.00"),
                    isProcessedMessage = (data.isProcessed==0)?"No":"Yes",
                    
                    sbuMessage = (data.specialBranchUnit == null) ? "wrongData" : "",
                    proMessage = (data.project == null) ? "wrongData" : "",
                    accMessage = (data.ledgerRelation == null) ? "wrongData" : "",
                    hasAmountMismatched= (Math.Round(debitAmount) != Math.Round(ceditAmount))? "wrongData":""
                };
                datas.Add(model);
            }

            return datas;

        }


        public async Task<List<UploadedVoucherData>> GetUploadedDataFileNoWise(string fileNo)
        {
            return await _context.UploadedVoucherDatas.Include(x => x.fiscalYear).Include(X => X.fundSource).Include(X => X.voucherType).Include(X => X.specialBranchUnit).Include(X => X.project).Include(X => X.ledgerRelation.ledger).Include(x => x.taxYear).Include(x => x.transectionMode).Where(x => x.refNo == fileNo).AsNoTracking().ToListAsync();
        }

        #endregion

        #region iOU Voucher Master
        public async Task<int> SaveIOUvoucherMaster(IOUVoucherMaster iOUVoucherMaster)
        {
            try
            {
                if (iOUVoucherMaster.Id != 0)
                {
                    _context.IOUVoucherMasters.Update(iOUVoucherMaster);
                }
                else
                {
                    _context.IOUVoucherMasters.Add(iOUVoucherMaster);
                }

                await _context.SaveChangesAsync();
                return iOUVoucherMaster.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IEnumerable<IOUVoucherMaster>> GetIOUVoucherMaster()
        {
            return await _context.IOUVoucherMasters.Include(x => x.voucherMaster).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<IOUVoucherMaster>> GetIOUVoucherMastersByStatus(int? status)
        {
            return await _context.IOUVoucherMasters.Include(x => x.voucherMaster).Where(x => x.status == status).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<IOUVoucherMaster>> GetIOUVoucherMastersBySubLedger(int? subLedgerId, int? status)
        {
            return await _context.IOUVoucherMasters.Include(x => x.voucherMaster).Where(x => x.status == status && x.subLedgerRelationId == subLedgerId).AsNoTracking().ToListAsync();
        }

        public async Task<List<IOUVoucherMaster>> GetIOUVoucherMastersByIds(string ids)
        {
            int[] iouIds = ids.Split(",").Select(int.Parse).ToArray();
            return await _context.IOUVoucherMasters.Include(x => x.voucherMaster).Include(x => x.ledgerRelation).Include(x => x.ledgerRelation.ledger).Where(x => iouIds.Contains(x.Id)).AsNoTracking().ToListAsync();
        }

        public async Task<IOUVoucherMaster> GetIOUVoucherMasterId(int id)
        {
            try
            {

                var record = await _context.IOUVoucherMasters.Include(x => x.voucherMaster).Where(x => x.Id == id).FirstOrDefaultAsync();
                return record;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }


        public async Task<int> SaveIOUVoucherLog(IOUVoucherLog iOUVoucherLog)
        {
            try
            {
                if (iOUVoucherLog.Id != 0)
                {
                    _context.IOUVoucherLogs.Update(iOUVoucherLog);
                }
                else
                {
                    _context.IOUVoucherLogs.Add(iOUVoucherLog);
                }

                await _context.SaveChangesAsync();
                return iOUVoucherLog.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IEnumerable<IOUVoucherLog>> GetIOUVoucherLogByIOUVoucherMasterId(int MasterId)
        {
            return await _context.IOUVoucherLogs
                .Where(x => x.IOUVoucherMasterId == MasterId).OrderBy(X => X.Id)
                .AsNoTracking().ToListAsync();
        }


        //public async Task<IEnumerable<IOUDetailItemsViewModel>> GetIOUDetailItemsByIOUMasterIds(List<int> MasterIds)
        //{
        //    try
        //    {
        //        List<IOUDetailItemsViewModel> record = new List<IOUDetailItemsViewModel>();

        //        record = await (from ioulog in _context.IOUVoucherLogs
        //                        join iom in _context.IOUMasters on ioulog.IOUId equals iom.Id
        //                        join ioud in _context.IOUDetails on iom.Id equals ioud.IOUId
        //                        join req in _context.RequisitionDetails on ioud.requisitionDetailId equals req.Id
        //                        join isp in _context.ItemSpecifications on req.itemSpecificationId equals isp.Id
        //                        join l in _context.Ledgers on isp.ledgerId equals l.Id
        //                        join lR in _context.LedgerRelations on l.Id equals lR.ledgerId
        //                        where MasterIds.Contains(iom.Id)
        //                        select new IOUDetailItemsViewModel
        //                        {
        //                            iouMasterd = iom.Id,
        //                            iouDetailId = ioud.Id,
        //                            specName = isp.specificationName,
        //                            specId = isp.Id,
        //                            ledgerAccountName = l.accountName,
        //                            ledgerAccountCode = l.accountCode,
        //                            ledgerRelationId = lR.Id,
        //                            ledgerId = isp.Id,
        //                            rate = ioud.rate,
        //                            qty = ioud.qty,
        //                            vatAmount = ioud.vatAmount,
        //                            vatPercent = ioud.vatPercent
        //                        }).OrderByDescending(a => a.iouMasterd).ToListAsync();




        //        return record;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;

        //    }
        //}
        public async Task<IEnumerable<IOUVoucherLog>> GetIOUVoucherLog()
        {
            return await _context.IOUVoucherLogs.Include(x => x.IOU).Include(X => X.IOUVoucherMaster).AsNoTracking().ToListAsync();
        }


        //public async Task<bool> DeletevoucherDetailByVMId(int vmid)
        //{
        //    _context.VoucherDetails.RemoveRange(_context.VoucherDetails.Where(x => x.voucherId == vmid));
        //    return 1 == await _context.SaveChangesAsync();
        //}

        #endregion


        #region Voucher Details 
        public async Task<int> SavevoucherDetail(VoucherDetail voucherDetail)
        {
            try
            {
                if (voucherDetail.Id != 0)
                {
                    _context.VoucherDetails.Update(voucherDetail);
                }
                else
                {
                    _context.VoucherDetails.Add(voucherDetail);
                }

                await _context.SaveChangesAsync();
                return voucherDetail.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IEnumerable<VoucherDetail>> GetvoucherDetail()
        {
            return await _context.VoucherDetails.Include(x => x.ledgerRelation).Include(X => X.transectionMode).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<VoucherDetail>> GetvoucherDetailByMasterIdEdit(int MasterId)
        {


            return await _context.VoucherDetails
                .Where(x => x.voucherId == MasterId && x.isPrincAcc.GetValueOrDefault(0) == 0)
                .Include(x => x.ledgerRelation.ledger)
                .Include(x => x.ledgerRelation.subLedger)
                .Include(X => X.transectionMode)
                .OrderBy(X => X.Id)
                .AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<VoucherDetail>> GetvoucherDetailByMasterId(int MasterId)
        {


            return await _context.VoucherDetails
                .Where(x => x.voucherId == MasterId)
                .Include(x => x.ledgerRelation.ledger.group)
                .Include(x => x.ledgerRelation.subLedger)
                .Include(X => X.transectionMode)
                .OrderBy(X => X.Id)
                .AsNoTracking().ToListAsync();
        }



        public async Task<VoucherDetail> GetGetvoucherDetailById(int id)
        {
            try
            {
                var record = await _context.VoucherDetails.Where(x => x.Id == id).Include(x => x.ledgerRelation).Include(X => X.transectionMode).FirstOrDefaultAsync();
                return record;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public async Task<IEnumerable<VoucherDetail>> GetGetvoucherDetailByLedgerRelId(int id)
        {
            try
            {
                var record = await _context.VoucherDetails.Where(x => x.ledgerRelationId == id).Include(x => x.ledgerRelation).Include(X => X.voucher).Include(X => X.transectionMode).ToListAsync();
                return record;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }


        public async Task<bool> DeletevoucherDetailByVMId(int vmid)
        {
            _context.VoucherDetails.RemoveRange(_context.VoucherDetails.Where(x => x.voucherId == vmid));
            return 1 == await _context.SaveChangesAsync();
        }

        #endregion

        #region Cost CEntre Allocation
        public async Task<int> SavecostCentreAllocation(CostCentreAllocation costCentreAllocation)
        {
            try
            {
                if (costCentreAllocation.Id != 0)
                {
                    _context.CostCentreAllocations.Update(costCentreAllocation);
                }
                else
                {
                    _context.CostCentreAllocations.Add(costCentreAllocation);
                }

                await _context.SaveChangesAsync();
                return costCentreAllocation.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<IEnumerable<CostCentreAllocation>> GetCostCentreAllocations()
        {
            return await _context.CostCentreAllocations.Include(x => x.voucherDetail).Include(X => X.costCentre).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<CostCentreAllocation>> GetCostCentreAllocationsByDetailId(int id)
        {
            var record = new List<CostCentreAllocation>();
            var recordV = new List<CostCentreAllocation>();
            try
            {
                var data = await _context.VoucherDetails.Where(x => x.voucherId == id).ToListAsync();
                foreach (var vdata in data)
                {
                    recordV = await _context.CostCentreAllocations.Where(x => x.voucherDetailId == vdata.Id).Include(x => x.voucherDetail).Include(X => X.costCentre).ToListAsync();
                    foreach (CostCentreAllocation costCentreAllocation in recordV)
                    {
                        record.Add(costCentreAllocation);
                    }

                }
                return record;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public async Task<bool> DeleteCostCentreAllocationByVMId(int vmid)
        {
            var data = await _context.VoucherDetails.Where(x => x.voucherId == vmid).AsNoTracking().ToListAsync();
            for (int i = 0; i < data.Count(); i++)
            {
                _context.CostCentreAllocations.RemoveRange(_context.CostCentreAllocations.Where(x => x.voucherDetailId == data[i].Id));
            }
            return 1 == await _context.SaveChangesAsync();
        }

        #endregion

        #region Dashboard

        public async Task<IEnumerable<VoucherViewModel>> GetVoucherListDashboard(string userName)
        {
            IQueryable<VoucherViewModel> result = from vm in _context.VoucherMasters
                                                  join t in _context.VoucherTypes on vm.voucherTypeId equals t.Id
                                                  join vd in _context.VoucherDetails on vm.Id equals vd.voucherId
                                                  where vd.transectionModeId == 2
                                                  group new { vm, t, vd } by new { vm.voucherTypeId, t.voucherTypeName } into g
                                                  select new VoucherViewModel
                                                  {
                                                      voucherTypeId = g.Key.voucherTypeId,
                                                      voucherTypeName = g.Key.voucherTypeName,
                                                      totalCount = g.Count(),
                                                      amount = g.Sum(x => x.vd.amount)
                                                  };

            return await result.ToListAsync();

        }

        public async Task<IEnumerable<VoucherViewModel>> CountVoucherByType(string userName)
        {
            IQueryable<VoucherViewModel> result = from vm in _context.VoucherMasters
                                                  join t in _context.VoucherTypes on vm.voucherTypeId equals t.Id
                                                  group new { vm, t } by new { vm.voucherTypeId, t.voucherTypeName } into g
                                                  select new VoucherViewModel
                                                  {
                                                      voucherTypeId = g.Key.voucherTypeId,
                                                      voucherTypeName = g.Key.voucherTypeName,
                                                      totalCount = g.Count(),
                                                      amount = g.Sum(x => x.vm.amount)
                                                  };

            return await result.ToListAsync();

        }

        #endregion

        #region Voucher Setting 
        public async Task<int> SaveVoucherSetting(VoucherSetting voucherSetting)
        {
            try
            {
                if (voucherSetting.Id != 0)
                {
                    _context.VoucherSettings.Update(voucherSetting);
                }
                else
                {
                    _context.VoucherSettings.Add(voucherSetting);
                }

                await _context.SaveChangesAsync();
                return voucherSetting.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IEnumerable<VoucherSetting>> GetVoucherSetting()
        {
            return await _context.VoucherSettings.Include(x => x.paymentMode).Include(x => x.bankAccountLedger.ledger).Include(x => x.cashAccountLedger.ledger).AsNoTracking().ToListAsync();
        }

        public async Task<bool> DeleteVoucherSettingById(int id)
        {
            _context.VoucherSettings.Remove(_context.VoucherSettings.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        #endregion

        #region Voucher Approve Log

        public async Task<int> SaveVoucherApproveLog(VoucherApproveLog voucherApproveLog)
        {
            try
            {
                if (voucherApproveLog.Id != 0)
                {
                    _context.VoucherApproveLogs.Update(voucherApproveLog);
                }
                else
                {
                    _context.VoucherApproveLogs.Add(voucherApproveLog);
                }

                await _context.SaveChangesAsync();
                return voucherApproveLog.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<VoucherApproveLog>> GetVoucherApproveLog()
        {
            return await _context.VoucherApproveLogs.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<VoucherApproveLog>> GetVoucherApproveLogByMasterId(int? masterId)
        {
            return await _context.VoucherApproveLogs
                .Where(x => x.voucherMasterId == masterId)
                .OrderBy(X => X.Id)
                .AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<VoucherMaster>> GetNotApproveVoucherList()
        {
            //List<int?> masterIds = _context.VoucherApproveLogs.Select(x => x.voucherMasterId).ToList();

            return await _context.VoucherMasters.Include(x => x.fiscalYear).Include(X => X.fundSource).Include(X => X.voucherType).Include(x => x.taxYear).ToListAsync();
        }

        #endregion

        #region Delete voucher
        public async Task<IEnumerable<GetVoucherListForDeleteViewModel>> GetVoucherListForDelete(string FDate, string TDate, string voucherNo)
        {
            return await _context.getVoucherListForDeleteViewModels.FromSql($"SP_GetVoucherListForDelete {Convert.ToDateTime(FDate).ToString("yyyyMMdd")},{Convert.ToDateTime(TDate).ToString("yyyyMMdd")},{voucherNo}").AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<GetVoucherListForDeleteViewModel>> GetVoucherListByVoucherNo(string voucherNo)
        {
            return await _context.getVoucherListForDeleteViewModels.FromSql($"SP_GetVoucherListByVoucherNo {voucherNo}").AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<DeleteVoucherViewModel>> DeleteVoucher(int? voucherMasterId, string remarks, string user)
        {
            return await _context.deleteVoucherViewModels.FromSql($"SP_DeleteVoucher {voucherMasterId},{remarks},{user}").AsNoTracking().ToListAsync();
        }

        #endregion

        #region autoVoucher
        public async Task<IEnumerable<VoucherMaster>> GetAllAutotVoucherMasterDetails()
        {
            try
            {
                List<VoucherMaster> record = new List<VoucherMaster>();
                record = await (from vm in _context.VoucherMasters.Where(x => x.autoVoucherNameId != null)
                                join vd in _context.VoucherDetails on vm.Id equals vd.voucherId
                                join f in _context.salaryYears on vm.fiscalYearId equals f.Id
                                join fs in _context.FundSources on vm.fundSourceId equals fs.Id
                                join vt in _context.VoucherTypes on vm.voucherTypeId equals vt.Id
                                join ty in _context.taxYears on vm.taxYearId equals ty.Id
                                //  where vm.voucherTypeId == TypeId && vd.isPrincAcc == 1
                                select new VoucherMaster
                                {
                                    Id = vm.Id,
                                    voucherNo = vm.voucherNo,
                                    voucherDate = vm.voucherDate,
                                    voucherAmount = vm.voucherAmount,
                                    voucherTypeId = vm.voucherTypeId,
                                    remarks = vm.remarks,
                                    isPosted = vm.isPosted,
                                    fiscalYearId = vm.fiscalYearId,
                                    taxYearId = vm.taxYearId,
                                    companyId = vm.companyId,
                                    fundSourceId = vm.fundSourceId,
                                    ledgerRelationId = vd.ledgerRelationId,
                                    accountName = vd.accountName,
                                    amount = vd.amount,
                                    refNo = vm.refNo,
                                    voucherType = vt,
                                    fiscalYear = f,
                                    taxYear = ty,
                                    fundSource = fs
                                }).OrderByDescending(a => a.Id).ToListAsync();

                return record;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public async Task<IEnumerable<VoucherMaster>> GetAutoVoucherMasterDetailsBybranchUnitWise(int TypeId, int branchUnitId)
        {
            try
            {
                List<VoucherMaster> record = new List<VoucherMaster>();
                if (TypeId == 0)
                {
                    record = await (from vm in _context.VoucherMasters.Where(x => x.autoVoucherNameId != null)
                                    join vd in _context.VoucherDetails.Where(x => x.specialBranchUnitId == branchUnitId) on vm.Id equals vd.voucherId
                                    join f in _context.salaryYears on vm.fiscalYearId equals f.Id
                                    join fs in _context.FundSources on vm.fundSourceId equals fs.Id
                                    join vt in _context.VoucherTypes on vm.voucherTypeId equals vt.Id
                                    join ty in _context.taxYears on vm.taxYearId equals ty.Id
                                    where vd.isPrincAcc == 1
                                    select new VoucherMaster
                                    {
                                        Id = vm.Id,
                                        voucherNo = vm.voucherNo,
                                        voucherDate = vm.voucherDate,
                                        voucherAmount = vm.voucherAmount,
                                        voucherTypeId = vm.voucherTypeId,
                                        remarks = vm.remarks,
                                        isPosted = vm.isPosted,
                                        fiscalYearId = vm.fiscalYearId,
                                        taxYearId = vm.taxYearId,
                                        companyId = vm.companyId,
                                        fundSourceId = vm.fundSourceId,
                                        ledgerRelationId = vd.ledgerRelationId,
                                        accountName = vd.accountName,
                                        amount = vd.amount,
                                        refNo = vm.refNo,
                                        voucherType = vt,
                                        fiscalYear = f,
                                        taxYear = ty,
                                        fundSource = fs
                                    }).OrderByDescending(a => a.Id).ToListAsync();
                }
                else
                {
                    record = await (from vm in _context.VoucherMasters.Where(x => x.autoVoucherNameId != null)
                                    join vd in _context.VoucherDetails.Where(x => x.specialBranchUnitId == branchUnitId) on vm.Id equals vd.voucherId
                                    join f in _context.salaryYears on vm.fiscalYearId equals f.Id
                                    join fs in _context.FundSources on vm.fundSourceId equals fs.Id
                                    join vt in _context.VoucherTypes on vm.voucherTypeId equals vt.Id
                                    join ty in _context.taxYears on vm.taxYearId equals ty.Id
                                    where vm.voucherTypeId == TypeId && vd.isPrincAcc == 1
                                    //where vm.voucherTypeId == TypeId 
                                    select new VoucherMaster
                                    {
                                        Id = vm.Id,
                                        voucherNo = vm.voucherNo,
                                        voucherDate = vm.voucherDate,
                                        voucherAmount = vm.voucherAmount,
                                        voucherTypeId = vm.voucherTypeId,
                                        remarks = vm.remarks,
                                        isPosted = vm.isPosted,
                                        fiscalYearId = vm.fiscalYearId,
                                        taxYearId = vm.taxYearId,
                                        companyId = vm.companyId,
                                        fundSourceId = vm.fundSourceId,
                                        ledgerRelationId = vd.ledgerRelationId,
                                        accountName = vd.accountName,
                                        amount = vd.amount,
                                        refNo = vm.refNo,
                                        voucherType = vt,
                                        fiscalYear = f,
                                        taxYear = ty,
                                        fundSource = fs
                                    }).OrderByDescending(a => a.Id).ToListAsync();
                }



                return record;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        #endregion

        #region ReBack voucher
        public async Task<IEnumerable<GetVoucherListForDeleteViewModel>> GetVoucherListForReBack(string FDate, string TDate, string voucherNo)
        {
            return await _context.getVoucherListForDeleteViewModels.FromSql($"SP_GetVoucherListForReBack {Convert.ToDateTime(FDate).ToString("yyyyMMdd")},{Convert.ToDateTime(TDate).ToString("yyyyMMdd")},{voucherNo}").AsNoTracking().ToListAsync();

        }

        #endregion

    }
}
