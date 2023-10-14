//New
using OPUSERP.Data.Entity;
using OPUSERP.HRPMS.Data.Entity.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.ProvidentFund.Data.Entity
{
    [Table("PFLoanManagement")]

    public class PFLoanManagement : Base
    {
        public int? EmployeeInfoId { get; set; }
        public EmployeeInfo EmployeeInfo { get; set; }

        public int? pfmemberId { get; set; }
        public PFMemberInfo pfmember { get; set; }

        public decimal? loanAmount { get; set; }

        public decimal? installmentAmount { get; set; }

        public int? interestRate { get; set; }

        public int? installmentPeriod { get; set; }


        public string note { get; set; }

        public DateTime? expectedSettlementDate { get; set; }

        public DateTime? disbursementDate { get; set; }
        public DateTime? applicationDate { get; set; }

        public int? approveStatus { get; set; } // 0=Pending, 1= Reject , 2= Approved 3,Disbursement

        public int? isActive { get; set; }

    }
}


//Old
//using OPUSERP.Data.Entity;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Threading.Tasks;

//namespace OPUSERP.ProvidentFund.Data.Entity
//{
//    [Table("PFLoanManagement")]

//    public class PFLoanManagement: Base
//    {
//        public int? pfmemberId { get; set; }
//        public PFMemberInfo pfmember { get; set; }

//        public decimal? loanAmount { get; set; }

//        public decimal? installmentAmount { get; set; }

//        public int? interestRate { get; set; }

//        public int? installmentPeriod { get; set; }


//        public string note { get; set; }

//        public DateTime? expectedSettlementDate { get; set; }

//        public DateTime? expectedDate { get; set; }
//        public DateTime? applicationDate { get; set; }

//        public int? approveStatus { get; set; } // 0=Pending, 1= Reject , 2= Approved

//        public int? isActive { get; set; }

//    }
//}
