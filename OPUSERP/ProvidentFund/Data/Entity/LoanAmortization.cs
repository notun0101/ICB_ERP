using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.ProvidentFund.Data.Entity
{
    [Table("LoanAmortization")]
    public class LoanAmortization:Base
    {
        public int? pfmemberId { get; set; }
        public PFMemberInfo pfmember { get; set; }
        public int? pFLoanManagementId { get; set; }
        public PFLoanManagement  pFLoanManagement { get; set; }
        public int? PI { get; set; }  //Installment
        public decimal? PPMT { get; set; }  //Principle Portion
        public decimal? IPMT { get; set; }  //Interest Portion
        public decimal? PMT { get; set; }  //Total Monthly Payment
        public DateTime PaymentDate { get; set; }  //Total Monthly Payment

    }
}
