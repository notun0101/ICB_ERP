using OPUSERP.Data.Entity;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OPUSERP.ProvidentFund.Data.Entity
{
    [Table("PFDistribution")]
    public class PFDistribution : Base
    {
        public string memberCode { get; set; }
        public string memberName { get; set; }
        public string designation { get; set; }
        public decimal? interestShariah { get; set; }
        public decimal? interestNonShariah { get; set; }
        public decimal? interestfromLoan { get; set; }
        public decimal? otherInterest { get; set; }
        public int? salaryPeriodId { get; set; }
        public SalaryPeriod salaryPeriod { get; set; }
        public int? salaryYearId { get; set; }
        public SalaryYear salaryYear { get; set; }

        public int? distributionMasterId { get; set; }
        public PFDistributionMaster distributionMaster { get; set; }
    }
}
