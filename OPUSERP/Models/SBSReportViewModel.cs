using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Models
{
	public class SBSReportViewModel
	{
		public string nameEnglish { get; set; }
		public DateTime? Dated { get; set; }
		public string FIID { get; set; }
		public string FIBranchId { get; set; }
		public int? SlNo { get; set; }
		public string ACID { get; set; }
		public DateTime? dateOfBirth { get; set; }
		public string GenderCode { get; set; }
		public string UniqueId { get; set; }
		public string eTin { get; set; }
		public string eBin { get; set; }
		public int? NatureOfLoan { get; set; }
		public string EcoSecId { get; set; }
		public string EcoPurId { get; set; }
		public string CollateralId { get; set; }
		public int? ColValue { get; set; }
		public int? LoanClass { get; set; }
		public int? IndustryScaleId { get; set; }
		public string ProductType { get; set; }
		public decimal? InttRate { get; set; }
		public decimal? SancLimit { get; set; }
		public DateTime? DisburseDate { get; set; }
		public DateTime? ExpiryDate { get; set; }
		public decimal? OpeningBalance { get; set; }
		public decimal? DisburseAmount { get; set; }
		public decimal? ChIntt { get; set; }
		public decimal? AccIntt { get; set; }
		public decimal? Others { get; set; }
		public decimal? ReceiveAmount { get; set; }
		public decimal? WaiverAmount { get; set; }
		public decimal? WriteOffAmount { get; set; }
		public decimal? OutStanding { get; set; }
		public decimal? OverdueAmount { get; set; }
	}
    //Added by Asad on 13.06.2023
    public class IndividualLoanSummaryVm
    {
        public int LoanId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string LoanType { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestAmount { get; set; }
        public decimal InterestCharge { get; set; }
                
    }

    public class VoucherVm
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string LoanType { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public string Particular { get; set; }
        public DateTime EffectiveDate { get; set; }
        public decimal PrincipalAmount { get; set; }

    }

    public class LoanInterest
    {
        public decimal? totalInterest { get; set; }
    }

	public class ReconciallationVm
	{
        public string empCode { get; set; }
        public string empName { get; set; }
        public string loanType { get; set; }
        public decimal? credit { get; set; }
        public decimal? debit { get; set; }
        public string particular { get; set; }
        public DateTime? effectiveDate { get; set; }
        public decimal? principal { get; set; }
    }

	public class QuarterlyVm
	{
        public int? Sl { get; set; }
        public string branchName { get; set; }
        public decimal? OpeningBalance { get; set; }
        public decimal? DisburseAmount { get; set; }
        public decimal? OutStanding { get; set; }
    }
}
