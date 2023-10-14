using OPUSERP.Data.Entity;
using OPUSERP.Payroll.Data.Entity.Salary;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPUSERP.ProvidentFund.Data.Entity
{
    [Table("PFInterestDistributionMaster")]
    public class PFInterestDistributionMaster : Base
    {
        public string processBy { get; set; }
        public string processDate { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public int? salaryPeriodId { get; set; }
        public SalaryPeriod salaryPeriod { get; set; }
        public decimal? totalContribution { get; set; }
        public decimal? totaldistributedInterest { get; set; }//Interest
        public int? status { get; set; }

    }
}
