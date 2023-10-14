using OPUSERP.Areas.Budget.Models;
using OPUSERP.Budget.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Budget.Service.Interface
{
   public interface IHOBudgetRequsitionService
    {
        Task<IEnumerable<BudgetRequisitionApprovedListViewModel>> GetBudgetRequsitionMasterForApproval(int userId);
        Task<IEnumerable<ColumnHeading>> GetAllColumnBySp();
    }
}
