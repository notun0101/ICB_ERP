using OPUSERP.HRPMS.Data.Entity.Master;
using OPUSERP.ProvidentFund.Data.Entity;
using OPUSERP.ProvidentFund.VMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.ProvidentFund.Models
{
    public class LoanApplicationViewModel
    {
        public int loanId { get; set; }

        public int? pfmemberId { get; set; }

        public decimal? loanAmount { get; set; }

        public decimal? installmentAmount { get; set; }

        public int? interestRate { get; set; }

        public int? installmentPeriod { get; set; }
        public int? LedgerRelationId { get; set; }


        public string note { get; set; }

        public DateTime? expectedSettlementDate { get; set; }

        public DateTime? expectedDate { get; set; }

        public DateTime? applicationDate { get; set; }


        public int? approveStatus { get; set; } // 0=Pending, 1= Reject , 2= Approved

        public int? isActive { get; set; }
        public int? employeeInfoId { get; set; }
        public string memberName { get; set; }
        public string memberCode { get; set; }

        public IEnumerable<PFLoanManagement> pFLoanManagements { get; set; }
        public IEnumerable<PFMemberInfo> pFMemberInfos { get; set; }
        public IEnumerable<Department> departments { get; set; }
        public IEnumerable<Designation> designations { get; set; }
        public IEnumerable<PFReportVm> pFReportVms { get; set; }



    }
}
