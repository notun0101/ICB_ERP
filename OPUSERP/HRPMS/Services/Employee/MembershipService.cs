using Microsoft.EntityFrameworkCore;
using OPUSERP.Data;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.HRPMS.Services.Employee.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.HRPMS.Services.Employee
{
    public class MembershipService : IMembershipService
    {
        private readonly ERPDbContext _context;

        public MembershipService(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteMembershipInfoById(int id)
        {
            _context.employeeMemberships.Remove(_context.employeeMemberships.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

		public async Task<bool> DeleteMembershipOrgById(int id)
		{
			_context.membershipOrganizations.Remove(_context.membershipOrganizations.Find(id));
			return 1 == await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<EmployeeMembership>> GetMembershipInfo()
        {
            return await _context.employeeMemberships.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<EmployeeMembership>> GetMembershipInfoByEmpId(int empId)
        {
            return await _context.employeeMemberships.Where(x => x.employeeId == empId).Include(x => x.membership).AsNoTracking().ToListAsync();
        }

        public async Task<EmployeeMembership> GetMembershipInfoById(int id)
        {
            return await _context.employeeMemberships.FindAsync(id);
        }

        public async Task<bool> SaveMembershipInfo(EmployeeMembership employeeMembership)
        {
            if (employeeMembership.Id != 0)
                _context.employeeMemberships.Update(employeeMembership);
            else
                _context.employeeMemberships.Add(employeeMembership);

            return 1 == await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<MembershipOrganization>> GetMembershipOrganization()
        {
            return await _context.membershipOrganizations.AsNoTracking().OrderBy(x => x.organizationName).ToListAsync();
        }

        public async Task<int> SaveMembershipOrganization(MembershipOrganization membershipOrganization)
        {
            if (membershipOrganization.Id != 0)
                _context.membershipOrganizations.Update(membershipOrganization);
            else
                _context.membershipOrganizations.Add(membershipOrganization);

            await _context.SaveChangesAsync();
            return membershipOrganization.Id;
        }
    }
}
