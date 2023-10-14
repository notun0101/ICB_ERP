using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Employee.Interfaces
{
    public interface IMembershipService
    {
        Task<bool> SaveMembershipInfo(EmployeeMembership employeeMembership);
        Task<IEnumerable<EmployeeMembership>> GetMembershipInfo();
        Task<EmployeeMembership> GetMembershipInfoById(int id);
        Task<bool> DeleteMembershipInfoById(int id);
        Task<IEnumerable<EmployeeMembership>> GetMembershipInfoByEmpId(int empId);
        Task<IEnumerable<MembershipOrganization>> GetMembershipOrganization();
        Task<int> SaveMembershipOrganization(MembershipOrganization membershipOrganization);
		Task<bool> DeleteMembershipOrgById(int id);

	}
}
