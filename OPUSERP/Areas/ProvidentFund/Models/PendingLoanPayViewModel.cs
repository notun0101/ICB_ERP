using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.ProvidentFund.Models
{
    public class PendingLoanPayViewModel
    {
        public int paymentId { get; set; }
        public string paymentAmount { get; set; }
        public string check { get; set; }
        public string modeOfPayment { get; set; }
        public DateTime? paymentDate { get; set; }
        public string note { get; set; }

    }
}
