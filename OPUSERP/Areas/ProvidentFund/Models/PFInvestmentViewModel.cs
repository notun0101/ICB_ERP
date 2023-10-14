using OPUSERP.ProvidentFund.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.Areas.ProvidentFund.Models
{
    public class PFInvestmentViewModel
    {
        public int investId { get; set; }

        public string investmentName { get; set; }


        public int? investmentAccount { get; set; }


        public int? interestRate { get; set; }

        public decimal? investmentAmount { get; set; }

        public int? interestPeriod { get; set; }

        public int? modeOfPayment { get; set; }

        public DateTime? transactionDate { get; set; }

        public DateTime? investmentDate { get; set; }

        public DateTime? maturityDate { get; set; }
        public decimal? totalInvestment { get; set; }
        public decimal? totalNewYearInvestment { get; set; }

        public  IEnumerable<PFInvestment> pFInvestments { get; set; }


    }
}
