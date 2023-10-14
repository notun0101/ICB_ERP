using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.ProvidentFund.Models
{
    public class FinalSettlementAcViewModel
    {
        public int memberId { get; set; }
        public int employeeId { get; set; }
        public decimal? pfTotalOwn { get; set; }
        public decimal? pfTotalBank { get; set; }
        public decimal? pfTotalInterest { get; set; }
        public decimal? pfTotalLoan { get; set; }
        public decimal? pfTotal { get; set; }
    }
}
