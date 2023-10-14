using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.SCM.Services.Hospital.Interfaces
{
	public interface IDoctor
	{
		Task<bool> SaveDoctorInfo(EmployeeContractInfo DoctorInfo);
		Task<EmployeeContractInfo> GetDoctorInfoById(int id);
		Task<bool> DeletDoctorInfoById(int id);
		Task<IEnumerable<EmployeeContractInfo>> GetDoctorInfoByDoctId(int empId);
	}
}
