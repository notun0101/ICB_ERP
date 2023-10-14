using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.ProvidentFund.Data.Entity
{
    [Table("PFInterestDistributionDetails", Schema = "PF")]
    public class PFInterestDistributionDetails : Base
    {

        public int? PFMemberId { get; set; }
        public PFMemberInfo PFMember { get; set; }
        public int? employeeId { get; set; }
        public EmployeeInfo employee { get; set; }
        public decimal? totalContribution { get; set; }//empContribution+BankContribution
        public decimal? distributedInterest { get; set; }//Interest
        public decimal? balance { get; set; } //after distributed Interest
        public int? distributionMasterId { get; set; }
        public PFInterestDistributionMaster distributionMaster { get; set; }
    }
}
