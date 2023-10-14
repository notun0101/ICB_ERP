using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Master;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
    [Table("EmployeeMembership")]
    public class EmployeeMembership : Base
    {
        //Foreign Reliation
        public int employeeId { get; set; }
        public EmployeeInfo employee { get; set; }

        public string nameOrganization { get; set; }
        public string membershipNo { get; set; }

        public int? membershipId { get; set; }
        public MembershipOrganization membership { get; set; }

        public string remarks { get; set; }
    }
}
