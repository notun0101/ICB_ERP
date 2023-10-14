using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OPUSERP.Areas.HRPMSEmployee.Models.Lang;
using System.Linq;
using System.Threading.Tasks;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class MembershipViewModel
    {
        public int employeeMembershipID { get; set; }

        public int employeeId { get; set; }
        public EmployeeInfo employee { get; set; }


        public string membershipNo { get; set; }


        public string remarks { get; set; }


        public int? membershipId { get; set; }
        public MembershipOrganization membership { get; set; }

        public string nameOrganization { get; set; }

        public IEnumerable<EmployeeMembership> employeeMemberships { get; set; }
        //public IEnumerable<MembershipOrganization> Orgmembership { get; set; }
        public IEnumerable<HRPMS.Data.Entity.Master.Membership> memberships { get; set; }
        public IEnumerable<MembershipOrganization> membershipOrganization { get; set; }
        public string membershipType { get; set; }

        

        //public Membership fLang { get; set; }

        public string employeeNameCode { get; set; }

        public Photograph photograph { get; set; }
        

        public string organizationId { get; set; }
        public string organizationName { get; set; }
		public string organizationNameBn { get; set; }
		public string organizationType { get; set; }
	}
}
