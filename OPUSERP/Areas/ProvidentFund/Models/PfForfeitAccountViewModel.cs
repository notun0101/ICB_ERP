using OPUSERP.HRPMS.Data.Entity.Employee;
using OPUSERP.Payroll.Data.Entity.Salary;
using OPUSERP.ProvidentFund.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.ProvidentFund.Models
{
    public class PfForfeitAccountViewModel
    {
        public int Id { get; set; }
        public int? employeeId { get; set; }
        public int? pfId { get; set; } //autofill
        public DateTime? forfeitDate { get; set; }
        public string reason { get; set; }
        public decimal? pfOwn { get; set; }
        public decimal? pfCompany { get; set; }
        public decimal? loan { get; set; }
        public decimal? outstandingLoan { get; set; }
        public decimal? balance { get; set; } //calculated (pfOwn+pfCompany+loan-outstandingLoan=balance)
        public int? status { get; set; }
        public string remarks { get; set; }

        public IEnumerable<PFMemberInfo> pFMemberInfos { get; set; }
        public IEnumerable<ForfeitAccount> forfeitAccounts { get; set; }
        public IEnumerable<PfFinalSettlement> finalSettlements { get; set; }
        
        public PFMemberInfo pFMemberInfo { get; set; }

        public int? pfMemberId { get; set; }
        public PFMemberInfo pfMember { get; set; }

        public string pfMemberCode { get; set; }
        public decimal? pfTotalOwn { get; set; }
        public decimal? pfTotalBank { get; set; }
        public decimal? pfTotalInterest { get; set; }
        public decimal? pfTotalLoan { get; set; }
        public decimal? pfTotal { get; set; }

    }
    
}
