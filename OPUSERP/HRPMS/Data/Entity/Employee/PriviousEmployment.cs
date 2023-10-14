using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.HRPMS.Data.Entity.Employee
{
    [Table("PriviousEmployment")]
    public class PriviousEmployment:Base
    {
        public int employeeID { get; set; }
        public EmployeeInfo employee { get; set; }

		public string organizationName { get; set; }

		public int? organizationTypeId { get; set; }
        public HRPMSOrganizationType organizationType { get; set; }

		public string employer { get; set; }

		public int? joiningDesignationId { get; set; }
		public Designation joiningDesignation { get; set; }

		public int? lastDesignationId { get; set; }
		public Designation lastDesignation { get; set; }

		public string jobTitle { get; set; }
		public decimal? lastSalary { get; set; }
		public string lengthOfService { get; set; }

		public string companyName { get; set; }

        public string position { get; set; }

        public string department { get; set; }

        public string companyBusiness { get; set; }

        public DateTime? startDate { get; set; } //joiningDate

        public DateTime? endDate { get; set; } //last working date

        public string companyLocation { get; set; } //org address

        public string proficiency { get; set; }
        public string achivments { get; set; }
        public string responsibilities { get; set; }
    }
}
