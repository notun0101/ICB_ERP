using Microsoft.EntityFrameworkCore;
using OPUSERP.Areas.Budget.Models;
using OPUSERP.Budget.Data.Entity;
using OPUSERP.Budget.Service.Interface;
using OPUSERP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Budget.Service
{
    public class HOBudgetRequsitionService: IHOBudgetRequsitionService
    {
        private readonly ERPDbContext _context;

        public HOBudgetRequsitionService(ERPDbContext context)
        {
            _context = context;
        }
        

        public async Task<IEnumerable<BudgetRequisitionApprovedListViewModel>> GetBudgetRequsitionMasterForApproval(int userId)
        {
            return await _context.budgetRequisitionApprovedListViewModels.FromSql(@"Sp_GetBudgetRequisitionListForApprovedHO @p0", userId).AsNoTracking().ToListAsync();
        }
        
        

        public async Task<IEnumerable<ColumnHeading>> GetAllColumnBySp()
        {
            var data = _context.columnHeadings.FromSql("sp_GetColumnName");
            return await data.ToListAsync();
        }
    }
}
