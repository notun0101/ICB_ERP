using OPUSERP.HRPMS.Data.Entity.HrBudget;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.HrBudget.Interface
{
	public interface IHrBudget
	{
		Task<bool> SaveHrBudget(HrBudgetDpt hrBudget);
		Task<IEnumerable<HrBudgetDpt>> GetAllHrBudget();
		Task<HrBudgetDpt> GetHrBudgetById(int id);
		Task<bool> DeleteHrBudgetById(int id);

		Task<IEnumerable<HrBudgetDpt>> GetAllDepartments();
		Task<IEnumerable<HrBudgetDpt>> GetHrBudgetsByDeptId(int deptId);
		Task<IEnumerable<Department>> GetAllHrDepartments();
		Task<IEnumerable<Designation>> GetAllHrDesignations();
		Task<IEnumerable<HrBranch>> GetAllHrBranch();
	}
}
