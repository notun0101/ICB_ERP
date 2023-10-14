using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.ProvidentFund.Models
{
    public class LoanRepaymentViewModel
    {
        public int? id { get; set; }
        public string adjustmentAmount { get; set; }
        public string adjustmentAccount { get; set; }
        public DateTime? adjustmentDate { get; set; }
        public string note { get; set; }
    }
}
