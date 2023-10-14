using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.HRPMSEmployee.Models
{
    public class PreviousEmploymentViewModel
    {
        public int employeeID { get; set; }
        public int? previousEmploymentID { get; set; }
      //  public string companyName { get; set; }
        public int? organizationType { get; set; }
      //  public string position { get; set; }
     //   public string department { get; set; }
     //   public string companyBusiness { get; set; }
     //   public DateTime? startDate { get; set; }
     //   public DateTime? endDate { get; set; }
      //  public string companyLocation { get; set; }
     //   public string responsibilities { get; set; }
        public string employeeNameCode { get; set; }

       

        public string organizationName { get; set; }
        public int? organizationTypeId { get; set; }
        public string employer { get; set; }
        public int? joiningDesignationId { get; set; }
        public int? lastDesignationId { get; set; }
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


       
        public Photograph photograph { get; set; }
        public EmployeeInfo employeeInfo { get; set; }

        public IEnumerable<HRPMSOrganizationType> hRPMSOrganizationTypes { get; set; }
        public IEnumerable<PriviousEmployment> priviousEmployments { get; set; }
        public IEnumerable<WagesPriviousEmployment> wagesPriviousEmployments { get; set; }
        public IEnumerable<Designation>  designations { get; set; }
      
    }
}
