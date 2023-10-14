using OPUSERP.Data.Entity.Master;
using System;
using System.Collections.Generic;

namespace OPUSERP.ProvidentFund.VMS
{
    public class EarlySettlementClosingBalanceVM
    {
        public decimal? PrincipalBalance { get; set; }
        public decimal? InterestProvisionBalance { get; set; }
    }

    public class PFReportVm
    {
        public string month { get; set; }
        public decimal? Basic { get; set; }
        public decimal? own { get; set; }
        public decimal? distributedInterest { get; set; }
        public decimal? company { get; set; }
        public decimal? loan { get; set; }
        public decimal? loanRepay { get; set; }
        public decimal? loanBalance { get; set; }
        public decimal? interestCharge { get; set; }

        public decimal? ownBalance { get; set; }
        public decimal? companyBalance { get; set; }
        public decimal? totalNetBalance { get; set; }
        public string nameEnglish { get; set; }
        public string designationName { get; set; }
        public string deptName { get; set; }
        public string memberCode { get; set; }
        public string year { get; set; }
        public DateTime? transectionDate { get; set; }
    }

    public class PFReportViewModel
    {
        public IEnumerable<PFReportVm> pfreport { get; set; }
        public IEnumerable<PFStatementVM> pFStatementVMs { get; set; }
        public IEnumerable<LoanStatementVM> loanStatementVMs { get; set; }
        public BalanceConfirmationVM BalanceConfirmationVM { get; set; }
        public IEnumerable<Company> companies { get; set; }
    }

    public class PFStatementVM
    {
        public string nameEnglish { get; set; }
        public string employeeCode { get; set; }
        public string designationName { get; set; }
        public string deptName { get; set; }
        public string memberCode { get; set; }
        public DateTime? transectionDate { get; set; }
        public string particulars { get; set; }
        public decimal? own { get; set; }
        public decimal? company { get; set; }
        public decimal? withDrawn { get; set; }
        public decimal? credit { get; set; }
        public decimal? debit { get; set; }
        public decimal? distributedInterest { get; set; }



    }

    public class LoanStatementVM
    {
        public string nameEnglish { get; set; }
        public string employeeCode { get; set; }
        public string designationName { get; set; }
        public string deptName { get; set; }
        public DateTime? transectionDate { get; set; }
        public string particulars { get; set; }
        public decimal? credit { get; set; }
        public decimal? debit { get; set; }

    }
    public class BalanceConfirmationVM
    {
        public string nameEnglish { get; set; }
        public string employeeCode { get; set; }
        public string designationName { get; set; }
        public string deptName { get; set; }
        public string memberCode { get; set; }
        public decimal? OutstandingBalance { get; set; }
        public decimal? InterestCharge { get; set; }
        public decimal? BankContribution { get; set; }
        public decimal? ownContribution { get; set; }
        public decimal? distributedInterest { get; set; }

    }



}
