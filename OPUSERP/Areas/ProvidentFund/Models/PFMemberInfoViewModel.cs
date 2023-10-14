using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.ProvidentFund.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.ProvidentFund.Models
{
    public class PFMemberInfoViewModel
    {
        public int? memberID { get; set; }

        public int? employeeInfoId { get; set; }

        public string employeeCode { get; set; }

        public string memberName { get; set; }

        public string email { get; set; }

        public DateTime? dateOfBirth { get; set; }

        public DateTime? dateOfJoining { get; set; }

        public int? memberType { get; set; }

        public string department { get; set; }

        public string designation { get; set; }

        public int? employeeStatus { get; set; }

        public string mobile { get; set; }
        public string nid { get; set; }

        public int? approveStatus { get; set; } // 0=Pending, 1= Reject , 2= Approved
        public string note { get; set; }
        public int? isActive { get; set; }
        public int? isPfContinuing { get; set; } //  1= Continuing
        public string memberCode { get; set; }
        public DateTime? applicationDate { get; set; }

        public IEnumerable<PFMemberInfo> pFMemberInfos { get; set; }
        public IEnumerable<EmployeeInfo> employeeInfos { get; set; }
        public IEnumerable<EmployeeType> memberTypes { get; set; }
        public IEnumerable<Department> departments { get; set; }
        public IEnumerable<Designation> designations { get; set; }
        public IEnumerable<ServiceStatus> serviceStatuses { get; set; }

        public IEnumerable<SpecialBranchUnit> specialBranchUnits { get; set; }

        public IEnumerable<PfMemberInfoVm> pfMemberInfos { get; set; }
        public int totalMember { get; set; }
        public int newMember { get; set; }
        public int pendingMember { get; set; }
    }
    public class PfMemberInfoVm
    {
        public PFMemberInfo pFMember { get; set; }
        public decimal? totalContribution { get; set; }
    }
}
