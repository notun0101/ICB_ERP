using OPUSERP.Areas.Budget.Models;
using OPUSERP.Budget.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Budget.Service.Interface
{
    public interface IBudgetRequsitionMasterService
    {
        Task<IEnumerable<FiscalYear>> GetFiscalYear();
        Task<FiscalYear> GetFiscalYearById(int id);
        Task<FiscalYear> GetFiscalYearByAlias(int id);
        Task<SpecialBranchUnit> GetSpecialBranchUnitById(int id);
        Task<int> SaveFiscalYear(FiscalYear fiscalYear);
        Task<bool> DeleteFiscalYearById(int id);
        Task<IEnumerable<BudgetRequisitionApprovedListViewModel>> GetBudgetRequsitionMasterForApproval(int userId);
        Task<IEnumerable<ColumnHeading>> GetAllColumnBySp();
        Task<IEnumerable<BudgetRequisitionApprovedListViewModel>> GetBudgetRequsitionMasterFinForApproval(int userId);
    }
}
