using OPUSERP.Data.Entity;
using OPUSERP.Data.Entity.Master;
using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.ProvidentFund.Data.Entity
{
    [Table("PFMemberInfo", Schema = "PF")]
    public class PFMemberInfo : Base
    {
      

        public string memberName { get; set; }
        public string memberCode { get; set; }
        public int? employeeInfoId { get; set; }
        public EmployeeInfo employeeInfo { get; set; }
        public int? approveStatus { get; set; } // 0=Pending, 1= Reject , 2= Approved
        public DateTime? applicationDate { get; set; }
        public DateTime? approveDate { get; set; }
        public int? isPfContinuing { get; set; } //  1= Continuing
        public int? isActive { get; set; }
        public string note { get; set; }
    }
}
