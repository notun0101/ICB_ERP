using OPUSERP.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.ProvidentFund.Data.Entity
{
    [Table("PfInterest")]
    public class PfInterest:Base
    {
        public string year { get; set; }
        public decimal? investmentAmount { get; set; }
        public decimal? interestRate { get; set; }
        public decimal? total { get; set; } //calculated
    }
}
