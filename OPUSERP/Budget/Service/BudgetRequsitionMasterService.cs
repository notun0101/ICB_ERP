using Microsoft.EntityFrameworkCore;
using OPUSERP.Areas.Budget.Models;
using OPUSERP.Budget.Data.Entity;
using OPUSERP.Budget.Service.Interface;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Budget.Service
{
    public class BudgetRequsitionMasterService : IBudgetRequsitionMasterService
    {
        private readonly ERPDbContext _context;

        public BudgetRequsitionMasterService(ERPDbContext context)
        {
            _context = context;
        }
        //Fiscal Year
        public async Task<IEnumerable<FiscalYear>> GetFiscalYear()
        {
            return await _context.fiscalYears.AsNoTracking().ToListAsync();
        }


        public async Task<FiscalYear> GetFiscalYearById(int id)
        {
            return await _context.fiscalYears.FindAsync(id);
        }

        public async Task<FiscalYear> GetFiscalYearByAlias(int id)
        {
            return await _context.fiscalYears.Where(x => x.aliasName == id.ToString()).FirstOrDefaultAsync();
        }
        public async Task<SpecialBranchUnit> GetSpecialBranchUnitById(int id)
        {
            return await _context.SpecialBranchUnits.FindAsync(id);
        }

        public async Task<int> SaveFiscalYear(FiscalYear fiscalYear)
        {
            if (fiscalYear.Id != 0)
                _context.fiscalYears.Update(fiscalYear);
            else
                _context.fiscalYears.Add(fiscalYear);
            await _context.SaveChangesAsync();
            return fiscalYear.Id;
        }

        public async Task<bool> DeleteFiscalYearById(int id)
        {
            _context.fiscalYears.Remove(_context.fiscalYears.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }
        

        public async Task<IEnumerable<BudgetRequisitionApprovedListViewModel>> GetBudgetRequsitionMasterForApproval(int userId)
        {
            return await _context.budgetRequisitionApprovedListViewModels.FromSql(@"Sp_GetBudgetRequisitionListForApproved @p0", userId).AsNoTracking().ToListAsync();
        }
        

        public async Task<IEnumerable<ColumnHeading>> GetAllColumnBySp()
        {
            var data = _context.columnHeadings.FromSql("sp_GetColumnName");
            return await data.ToListAsync();
        }


        
        public async Task<IEnumerable<BudgetRequisitionApprovedListViewModel>> GetBudgetRequsitionMasterFinForApproval(int userId)
        {
            return await _context.budgetRequisitionApprovedListViewModels.FromSql(@"Sp_GetBudgetRequisitionListFinForApproved @p0", userId).AsNoTracking().ToListAsync();
        }

    }
}
