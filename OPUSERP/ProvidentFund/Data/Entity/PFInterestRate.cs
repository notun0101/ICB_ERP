using OPUSERP.Data.Entity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.ProvidentFund.Data.Entity
{
    [Table("PFInterestRate")]
    public class PFInterestRate : Base
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? InterestRate { get; set; }
        public int? status { get; set; } = 1;
    }
}
