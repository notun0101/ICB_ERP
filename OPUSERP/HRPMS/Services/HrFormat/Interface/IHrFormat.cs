using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.HrFormat.Interface
{
	public interface IHrFormat
	{
		Task<bool> SaveHrFormatMaster(HrFormatMaster hrFormatMaster);
		Task<int> SaveHrFormatDetails(HrFormatDetails hrFormatDetails);
		Task<IEnumerable<HrFormatMaster>> GetAllHrFormatMaster();
		Task<HrFormatMaster> GetHrFormatMasterById(int id);
		Task<HrFormatDetails> GetFormatDetailsById(int id);
		Task<bool> DeleteHrFormatMasterById(int id);

		Task<HrFormatMaster> GetFormatByTypeId(string type);
		Task<EmployeeInfo> GetEmployeeInfoById(int id);
		Task<HrFormatMaster> GetHrFormatMasterByType(string type);
	}
}
