using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.ProvidentFund.Data.Entity;
using System;
using System.Collections.Generic;

namespace OPUSERP.Areas.ProvidentFund.Models
{
    public class PFLoanDisbursementViewModel
    {
        //New
        public int loanId { get; set; }
        public int? EmployeeInfoId { get; set; }
        //public EmployeeInfo EmployeeInfo { get; set; }
        public int? pfmemberId { get; set; }
        //public PFMemberInfo pfmember { get; set; }

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
        //public string membersCode { get; set; }

        //Extra
        public int? employeeId { get; set; }
        public string employeeCode { get; set; }


        public IEnumerable<PFLoanDisbursement> PFLoanDisbursements { get; set; }
        public IEnumerable<PFMemberInfo> pFMemberInfos { get; set; }
        public IEnumerable<Department> departments { get; set; }
        public IEnumerable<Designation> designations { get; set; }

        //Old
        //public int loanId { get; set; }

        //public int? pfmember { get; set; }

        //public decimal? loanAmount { get; set; }

        //public decimal? installmentAmount { get; set; }

        //public int? interestRate { get; set; }

        //public int? installmentPeriod { get; set; }


        //public string note { get; set; }

        //public DateTime? expectedSettlementDate { get; set; }

        //public DateTime? expectedDate { get; set; }

        //public DateTime? applicationDate { get; set; }


        //public int? approveStatus { get; set; } // 0=Pending, 1= Reject , 2= Approved

        //public int? isActive { get; set; }

        //public IEnumerable<PFLoanManagement> pFLoanManagements { get; set; }
        //public IEnumerable<PFMemberInfo> pFMemberInfos { get; set; }
        //public IEnumerable<Department> departments { get; set; }
        //public IEnumerable<Designation> designations { get; set; }

    }
}
