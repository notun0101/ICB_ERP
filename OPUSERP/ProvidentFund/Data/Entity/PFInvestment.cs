using OPUSERP.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.ProvidentFund.Data.Entity
{
    [Table("PFInvestment")]

    public class PFInvestment : Base
    {
        [MaxLength(150)]
        public string investmentName { get; set; }


        public int? investmentAccount { get; set; }

        
        public int? interestRate { get; set; }

        public decimal? investmentAmount { get; set; }

        public int? interestPeriod { get; set; }

        public int? modeOfPayment { get; set; }

        public DateTime? transactionDate { get; set; }

        public DateTime? investmentDate { get; set; }

        public DateTime? maturityDate { get; set; }
    }
}
