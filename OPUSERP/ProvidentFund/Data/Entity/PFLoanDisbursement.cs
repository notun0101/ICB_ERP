using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.ProvidentFund.Data.Entity
{
    [Table("PFLoanDisbursement", Schema = "PF")]
    public class PFLoanDisbursement : Base
    {
        public int? EmployeeInfoId { get; set; }
        public EmployeeInfo EmployeeInfo { get; set; }
        public int? pfmemberId { get; set; }
        public PFMemberInfo pfmember { get; set; }

        public decimal? loanAmount { get; set; }

        public decimal? installmentAmount { get; set; }
        //New
        public int? tenure { get; set; }
        //Old
        //public int? installmentPeriod { get; set; }
        public int? interestRate { get; set; }

        public string note { get; set; }

        public DateTime? expectedSettlementDate { get; set; }

        public DateTime? expectedDate { get; set; }
        public DateTime? applicationDate { get; set; }

        public int? approveStatus { get; set; } // 0=Pending, 1= Reject , 2= Approved

        public int? isActive { get; set; }
    }
}
