using OPUSERP.ProvidentFund.Data.Entity;
using System;

namespace OPUSERP.Areas.ProvidentFund.Models
{
    public class LoanCollectionVm
    {
        public int? pfmemberId { get; set; }
        public PFMemberInfo pfmember { get; set; }
        public int? loanManagementId { get; set; }
        public PFLoanManagement loanManagement { get; set; }
        public decimal? collectionAmount { get; set; }
        public decimal? loanInterestAmount { get; set; }
        public DateTime? collectionDate { get; set; }
        public decimal? collectionBehaviour { get; set; }
        public string note { get; set; }
        public int? branchId { get; set; }
        public int? LedgerRelationId { get; set; }
        public int? isJournal { get; set; } // 0 = Data Upload ,1 = AutoVoucher Create
        public int? EmployeeInfoId { get; set; }
    }
}
